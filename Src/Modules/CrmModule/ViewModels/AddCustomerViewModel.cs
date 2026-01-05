using DevExpress.Mvvm;
using DevExpress.Mvvm.Native;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.CodeView;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Map;
using DevExpress.XtraSpreadsheet.Model;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Data.Common.OTM;
using Emdep.Geos.Data.Common.PCM;
using Emdep.Geos.Modules.Crm.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
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
    public class AddCustomerViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDataErrorInfo, IDisposable
    {
        #region TaskLog

        //[CRM-M052-17] No message displaying the user about missing mandatory fields [adadibathina]
        //GEOS2-194 (#70171) Not possible to create customer [adadibathina]
        #endregion

        #region Services

        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //ICrmService CrmStartUp = new CrmServiceController("localhost:6699");
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion // Services

        #region Declaration
        private int selectedIndexCompanyGroup;
        private int selectedIndexBusinessField = 0;
        private int selectedIndexBusinessCenter = 0;
        private int selectedIndexCountry = 0;

        private int? customerNumberofEmplyee;
        private int? customerLines;
        private int? cuttingMachines;
        private double? size;

        private string informationError;

        private ObservableCollection<Customer> companyGroupList;
        private List<Company> companyPlantList;
        public List<Country> CountryList { get; set; }
        public IList<LookupValue> BusinessCenter { get; set; }
        public IList<LookupValue> BusinessField { get; set; }

        public List<Company> AllPlantsLst = new List<Company>();
        private List<Company> plantNameLst;
        private List<string> plantNameStrLst;
        private Visibility alertVisibility;



        public bool IsCreateCustomer { get; set; }

        private string customerAccountName;
        private string customerPlantName;
        private string customerWebsite;
        private string customerAddress;
        private string registeredName;
        private string customerCity;
        private string customerState;
        private string customerZip;
        private string customerTelephone;
        private string customerEmail;
        private string customerFax;

        private GeoPoint mapLatitudeAndLongitude;
        private double? latitude;    // map  latitude
        private double? longitude;   // map  longitude

        private ImageSource customerImageSource;
        private Company customerData;
        public IList<LookupValue> BusinessProduct { get; set; }
        private List<Object> selectedBusinessProductItems;
        private ObservableCollection<People> salesOwnerList;
        private People salesResponsible;
        private ObservableCollection<People> listSalesUsers;
        private bool isBusy;
        byte[] UserProfileImageByte = null;
        private bool isCoordinatesNull;
        private List<LogEntryBySite> tempListChangeLog = new List<LogEntryBySite>();
        private List<User> ownerUser;
        private int selectedIndexOwner;
        private List<City> cityList;
        private List<string> countryWiseCities;
        private List<string> countryWiseRegions;
        private List<CountryRegion> countryRegionList;
        private string visible;
        private bool isShowErrorInAddress;
        private string customerAddressError;
        private Visibility showAddValidationVisibility;
        private string registerationNumber;
        private string assignedSalesOwnerError;
        private ObservableCollection<LookupValue> sourceList;//[Sudhir.Jangra][GEOS2-4663][28/08/2023]
        private LookupValue selectedSource;//[Sudhir.jangra][GEOS2-4663][28/08/2023]
        private bool isSourceButtonEnabled;//[Sudhir.Jangra][GEOS2-5170]
        //[RGadhave][12.11.2024][GEOS2-6461]
        private ObservableCollection<CustomerDetail> incotermsList;
        private ObservableCollection<CustomerDetail> paymentTermsList;
        private int selectedIndexIncoterms;
        private int selectedIndexPaymentTerms;
        private string alias;
        private ObservableCollection<Emdep.Geos.Data.Common.Crm.ShippingAddress> shippingAddressList;
        private string contentcountryname;
        string contentcountryimgurl = null;
        private Emdep.Geos.Data.Common.Crm.ShippingAddress selectedLinkedAddress; // [nsatpute][22-11-2024][GEOS2-6462]
        #region//chitra.girigosavi GEOS2-7242 31/03/2025
        private List<LookupValue> statusList;
        private int selectedStatusIndex;
        private LookupValue selectedStatus;
        private string status;
        #endregion
        #endregion // Declaration

        #region Properties
        public string AssignedSalesOwnerError
        {
            get { return assignedSalesOwnerError; }
            set { assignedSalesOwnerError = value; OnPropertyChanged(new PropertyChangedEventArgs("AssignedSalesOwnerError")); }
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
        public string InformationError
        {
            get { return informationError; }
            set { informationError = value; OnPropertyChanged(new PropertyChangedEventArgs("InformationError")); }
        }

        private string selectedCustomerName;
        public string SelectedCustomerName
        {
            get { return selectedCustomerName; }
            set { selectedCustomerName = value; OnPropertyChanged(new PropertyChangedEventArgs("SelectedCustomerName")); }
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

        public Company CustomerData
        {
            get { return customerData; }
            set
            {
                customerData = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomerData"));
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


        public int SelectedIndexCompanyGroup
        {
            get { return selectedIndexCompanyGroup; }
            set
            {
                selectedIndexCompanyGroup = value;
                if (CompanyGroupList != null)
                {
                    SelectedCustomerName = CompanyGroupList[SelectedIndexCompanyGroup].CustomerName;
                }
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexCompanyGroup"));

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

        public ObservableCollection<Customer> CompanyGroupList
        {
            get { return companyGroupList; }
            set
            {
                companyGroupList = value; OnPropertyChanged(new PropertyChangedEventArgs("CompanyGroupList"));
            }
        }

        public List<Company> CompanyPlantList
        {
            get { return companyPlantList; }
            set
            {
                companyPlantList = value; OnPropertyChanged(new PropertyChangedEventArgs("CompanyPlantList"));
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


        public string CustomerAccountName
        {
            get { return customerAccountName; }
            set
            {
                customerAccountName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomerAccountName"));
            }
        }

        public string CustomerPlantName
        {
            get { return customerPlantName; }
            set
            {
                customerPlantName = value.TrimStart();
                OnPropertyChanged(new PropertyChangedEventArgs("CustomerPlantName"));
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
        public string CustomerAddressError
        {
            get { return customerAddressError; }
            set
            {
                if (value != null)
                {
                    customerAddressError = value.Trim();
                    OnPropertyChanged(new PropertyChangedEventArgs("CustomerAddressError"));
                }
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

        public string RegisteredName
        {
            get { return registeredName; }
            set
            {

                registeredName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RegisteredName"));
            }
        }

        public string CustomerCity
        {
            get { return customerCity; }
            set
            {

                if (value != null)
                {
                    customerCity = value.TrimStart();
                    OnPropertyChanged(new PropertyChangedEventArgs("CustomerCity"));

                    if (!string.IsNullOrEmpty(customerCity) && !string.IsNullOrEmpty(CustomerPlantName))
                        GeosApplication.Instance.IsCityNameExist = CustomerPlantName.ToUpper().Contains(CustomerCity.ToUpper());
                }
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
                //if (customerAddress != null)
                //{
                //    if (customerAddress.ToUpper().Contains(RegisteredName.ToUpper()))
                //    {
                //        IsShowErrorInAddress = true;
                //    }
                //    if (customerAddress.ToUpper().Contains(CustomerCity.ToUpper()))
                //    {
                //        IsShowErrorInAddress = true;
                //    }
                //    if (customerAddress.ToUpper().Contains(CustomerZip))
                //    {
                //        IsShowErrorInAddress = true;
                //    }
                //    if (customerAddress.ToUpper().Contains(CustomerState.ToUpper()))
                //    {
                //        IsShowErrorInAddress = true;
                //    }
                //    if (SelectedIndexCountry > 0 && CountryList != null)
                //    {
                //        if (customerAddress.ToUpper().Contains(CountryList[SelectedIndexCountry].Name.ToUpper()))
                //        {
                //            IsShowErrorInAddress = true;
                //        }
                //    }

                //}
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexCountry"));
            }
        }


        public ImageSource CustomerImageSource
        {
            get { return customerImageSource; }
            set
            {
                customerImageSource = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomerImageSource"));
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
        public ObservableCollection<People> SalesOwnerList
        {
            get { return salesOwnerList; }
            set
            {
                salesOwnerList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SalesOwnerList"));
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
        public ObservableCollection<People> ListSalesUsers
        {
            get { return listSalesUsers; }
            set { listSalesUsers = value; OnPropertyChanged(new PropertyChangedEventArgs("ListSalesUsers")); }
        }
        public List<LogEntryBySite> TempListChangeLog
        {
            get { return tempListChangeLog; }
            set
            {
                tempListChangeLog = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TempListChangeLog"));
            }
        }
        public List<User> OwnerUser
        {
            get { return ownerUser; }
            set { ownerUser = value; OnPropertyChanged(new PropertyChangedEventArgs("OwnerUser")); }
        }
        public int SelectedIndexOwner
        {
            get { return selectedIndexOwner; }
            set
            {
                selectedIndexOwner = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexOwner"));
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
        public string Visible
        {
            get
            {
                return visible;
            }

            set
            {
                visible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Visible"));
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
        //[Sudhir.Jangra][GEOS2-4663][28/08/2023]
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
        #region Rahul.Gadhave
        //[RGadhave][12.11.2024][GEOS2-6461]


        public string Alias
        {
            get { return alias; }
            set
            {
                alias = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Alias"));
            }
        }







        private string selectedIncotermsName;
        public string SelectedIncotermsName
        {
            get { return selectedIncotermsName; }
            set { selectedIncotermsName = value; OnPropertyChanged(new PropertyChangedEventArgs("SelectedIncotermsName")); }
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

        public int SelectedIndexIncoterms
        {
            get { return selectedIndexIncoterms; }
            set
            {
                selectedIndexIncoterms = value;
                if (IncotermsList != null && selectedIndexIncoterms >= 0 && selectedIndexIncoterms < IncotermsList.Count)
                {
                    // SelectedIncotermsName = IncotermsList[selectedIndexIncoterms].IncotermName;
                    SelectedIncotermsName = IncotermsList[selectedIndexIncoterms].IncotermName;
                    SelectedIncotermId = IncotermsList[selectedIndexIncoterms].IdIncoterm;

                }
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexIncoterms"));
            }
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


        public ObservableCollection<Emdep.Geos.Data.Common.Crm.ShippingAddress> ShippingAddressList
        {
            get { return shippingAddressList; }
            set
            {
                shippingAddressList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ShippingAddressList"));
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
        private bool isPrimaryAddress;
        public bool IsPrimaryAddress
        {
            get
            {
                return isPrimaryAddress;
            }

            set
            {
                isPrimaryAddress = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPrimaryAddress"));
            }
        }
        // [nsatpute][22-11-2024][GEOS2-6462]
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
        #endregion

        #region//chitra.girigosavi GEOS2-7242 10/04/2025
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
        #endregion
        #endregion // Properties

        #region public ICommand

        public ICommand AddCustomerViewCancelButtonCommand { get; set; }
        public ICommand AddCustomerViewAcceptButtonCommand { get; set; }
        public ICommand CustomerImageRemoveButtonCommand { get; set; }
        public ICommand SearchButtonClickCommand { get; set; }
        public ICommand AddCustomerButtonCommand { get; set; }
        public ICommand SetSalesResponsibleCommand { get; set; }
        public ICommand AssignedSalesCancelCommand { get; set; }
        public ICommand GetSalesUserContactCommand { get; set; }
        public ICommand CommandTextInput { get; set; }
        public ICommand LinkedAddressDoubleClickCommand { get; set; }
        public ICommand AddSourceButtonCommand { get; set; }//[Sudhir.Jangra][GEOS2-5170]

        public ICommand AddShippingAddressCommand { get; set; }// [pramod.misal][GEOS2-6462][13.11.2024]

        public ICommand SetDefaultShippingAddressCommand { get; set; } // [nsatpute][22-11-2024][GEOS2-6462]

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
                    me[BindableBase.GetPropertyName(() => IsShowErrorInAddress)] +
                    me[BindableBase.GetPropertyName(() => InformationError)] +
                    me[BindableBase.GetPropertyName(() => CustomerCity)] +
                    me[BindableBase.GetPropertyName(() => AssignedSalesOwnerError)] +
                      //[RGadhave][12.11.2024][GEOS2-6462]
                      me[BindableBase.GetPropertyName(() => SelectedIndexIncoterms)] +
                      me[BindableBase.GetPropertyName(() => SelectedIndexPaymentTerms)] +
                      me[BindableBase.GetPropertyName(() => SelectedStatusIndex)];//chitra.girigosavi[GEOS2-7242][14/04/2025]
                // me[BindableBase.GetPropertyName(() => CustomerAddress)];

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
                //[RGadhave][12.11.2024][GEOS2-6462]
                string IncotermsError = BindableBase.GetPropertyName(() => SelectedIndexIncoterms);
                string PaymentTermsError = BindableBase.GetPropertyName(() => SelectedIndexPaymentTerms);
                string SelectedStatusIndexProp = BindableBase.GetPropertyName(() => SelectedStatusIndex);//chitra.girigosavi[GEOS2-7242][14/04/2025]
                // string isShowErrorInAddress = BindableBase.GetPropertyName(() => CustomerAddress);
                if (columnName == SelectedIndexCompanyGroupProp)
                    return CustomerAddRequiredFieldValidation.GetErrorMessage(SelectedIndexCompanyGroupProp, SelectedIndexCompanyGroup);

                if (columnName == CustomerPlantNameProp)
                    return CustomerAddRequiredFieldValidation.GetErrorMessage(CustomerPlantNameProp, CustomerPlantName);

                if (columnName == SelectedBusinessProductItemsProp)
                    return CustomerAddRequiredFieldValidation.GetErrorMessage(SelectedBusinessProductItemsProp, SelectedBusinessProductItems);

                if (columnName == SelectedIndexBusinessFieldProp)
                    return CustomerAddRequiredFieldValidation.GetErrorMessage(SelectedIndexBusinessFieldProp, SelectedIndexBusinessField);

                if (columnName == SelectedIndexBusinessCenterProp)
                    return CustomerAddRequiredFieldValidation.GetErrorMessage(SelectedIndexBusinessCenterProp, SelectedIndexBusinessCenter);

                if (columnName == SelectedIndexCountryProp)
                    return CustomerAddRequiredFieldValidation.GetErrorMessage(SelectedIndexCountryProp, SelectedIndexCountry);

                if (columnName == CustomerAddressProp)
                    return CustomerAddRequiredFieldValidation.GetErrorMessage(CustomerAddressProp, IsShowErrorInAddress);

                if (columnName == CustomerCityProp)
                    return CustomerAddRequiredFieldValidation.GetErrorMessage(CustomerCityProp, CustomerCity);

                if (columnName == informationError)
                    return CustomerAddRequiredFieldValidation.GetErrorMessage(informationError, InformationError);

                if (columnName == assignedSalesOwnerError)
                    return CustomerAddRequiredFieldValidation.GetErrorMessage(assignedSalesOwnerError, AssignedSalesOwnerError);
                // //[RGadhave][12.11.2024][GEOS2-6462]
                if (columnName == IncotermsError)
                    return CustomerAddRequiredFieldValidation.GetErrorMessage(IncotermsError, SelectedIndexIncoterms);

                if (columnName == PaymentTermsError)
                    return CustomerAddRequiredFieldValidation.GetErrorMessage(PaymentTermsError, SelectedIndexPaymentTerms);
                //chitra.girigosavi[GEOS2-7242][14/04/2025]
                if (columnName == SelectedStatusIndexProp)
                    return CustomerAddRequiredFieldValidation.GetErrorMessage(SelectedStatusIndexProp, SelectedStatusIndex);
                return null;
            }
        }

        #endregion

        #region Constructor

        public AddCustomerViewModel()
        {

            AddCustomerViewCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
            AddCustomerViewAcceptButtonCommand = new RelayCommand(new Action<object>(SaveCustomerDetails));
            CustomerImageRemoveButtonCommand = new RelayCommand(new Action<object>(RemoveCustomerImage));
            SearchButtonClickCommand = new DelegateCommand<object>(new Action<object>((obj) => { SearchButtonClickCommandAction(obj); }));
            AddCustomerButtonCommand = new RelayCommand(new Action<object>(AddCustomerName));
            SetSalesResponsibleCommand = new DelegateCommand<object>(SetSalesResponsibleCommandAction);
            GetSalesUserContactCommand = new DelegateCommand<object>(SalesUserContactCheckedAction);
            AssignedSalesCancelCommand = new DelegateCommand<object>(AssignedSalesCancelCommandAction);
            CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);

            if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 101))
            {
                IsSourceButtonEnabled = true;
            }
            else
            {
                IsSourceButtonEnabled = false;
            }

            AddSourceButtonCommand = new RelayCommand(new Action<object>(AddSourceButtonCommandAction));//[Sudhir.Jangra][GEOS2-5170]
            AddShippingAddressCommand = new DelegateCommand<object>(AddShippingAddressCommandAction);// [pramod.misal][GEOS2-4450][27.06.2023]
            LinkedAddressDoubleClickCommand = new DelegateCommand<object>(LinkedAddressDoubleClickCommandAction);
            SetDefaultShippingAddressCommand = new DelegateCommand<object>(SetDefaultShippingAddressCommandAction); // [nsatpute][22-11-2024][GEOS2-6462] 
            AllPlantsLst = CrmStartUp.GetAllCustomerCompanies();

            AlertVisibility = Visibility.Hidden;
            SalesOwnerList = new ObservableCollection<People>();
            FillGroupList();
            FillBusinessCenter();
            FillBusinessField();
            FillCityList();
            FillCountryRegionList();
            FillCountry();
            FillBusinessProduct();
            FillUsers();
            FillSalesOwnerList();
            FillSourceList();//[Sudhir.Jangra][GEOS2-4663]28/08/2023]
                             //[RGadhave][12.11.2024][GEOS2-6462]
            FillIncotermsList();
            FillPaymentTermsList();
            FillStatusList();   //chitra.girigosavi[GEOS2-7242][10/04/2025]

            ShowAddValidationVisibility = Visibility.Hidden;

            //set hide/show shortcuts on permissions
            Visible = Visibility.Visible.ToString();
            if (GeosApplication.Instance.IsPermissionReadOnly)
            {
                Visible = Visibility.Hidden.ToString();
            }
            else
            {
                Visible = Visibility.Visible.ToString();
            }

        }

        #endregion // Constructor

        #region Methods
        //[GEOS2-6462][rdixit][23.11.2024]
        public void LinkedAddressDoubleClickCommandAction(object obj)
        {
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
        /// <param name="CustomerPlantName"></param>
        private void ShowPopupAsPerPlantName(string CustomerPlantName)
        {
            GeosApplication.Instance.Logger.Log("Method ShowPopupAsPerPlantName ...", category: Category.Info, priority: Priority.Low);

            GeosApplication.Instance.IsCityNameExist = true;
            // PlantNameLst = AllPlantsLst.ToList();
            int SelectedIdCustomer = CompanyGroupList[SelectedIndexCompanyGroup].IdCustomer;
            PlantNameLst = AllPlantsLst.Where(cust => cust.IdCustomer == SelectedIdCustomer).ToList().GroupBy(cpl => cpl.IdCompany).Select(group => group.First()).ToList();
            if (PlantNameLst != null && !string.IsNullOrEmpty(CustomerPlantName))
            {
                if (CustomerPlantName.Length > 1)
                {
                    PlantNameLst = PlantNameLst.Where(h => h.Name.ToUpper().Contains(CustomerPlantName.ToUpper()) || h.Name.ToUpper().StartsWith(CustomerPlantName.Substring(0, 2).ToUpper())
                                                            || h.Name.ToUpper().EndsWith(CustomerPlantName.Substring(CustomerPlantName.Length - 2).ToUpper())).OrderByDescending(apn => StringSimilarityScore(apn.Name, CustomerPlantName)).ToList();
                    PlantNameStrLst = PlantNameLst.Select(pn => pn.Name).ToList();
                }
                else
                {
                    PlantNameLst = PlantNameLst.Where(h => h.Name.ToUpper().Contains(CustomerPlantName.ToUpper()) || h.Name.ToUpper().StartsWith(CustomerPlantName.Substring(0, 1).ToUpper())
                                                            || h.Name.ToUpper().EndsWith(CustomerPlantName.Substring(CustomerPlantName.Length - 1).ToUpper())).OrderByDescending(apn => StringSimilarityScore(apn.Name, CustomerPlantName)).ToList();
                    PlantNameStrLst = PlantNameLst.Select(pn => pn.Name).ToList();
                }

                if (!string.IsNullOrEmpty(CustomerCity))
                    GeosApplication.Instance.IsCityNameExist = CustomerPlantName.ToUpper().Contains(CustomerCity.ToUpper());

                //   GeosApplication.Instance.IsGroupNameExist = CompanyGroupList.Any(cl => CustomerPlantName.ToUpper().Contains(cl.CustomerName.ToUpper()));
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

        /// <summary>
        /// Method for add customer name.
        /// </summary>
        /// <param name="obj"></param>
        private void AddCustomerName(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method AddCustomerName ...", category: Category.Info, priority: Priority.Low);

            AddCustomerNameViewModel addCustomerNameViewModel = new AddCustomerNameViewModel();
            AddCustomerNameView addCustomerNameView = new AddCustomerNameView();

            EventHandler handle = delegate { addCustomerNameView.Close(); };
            addCustomerNameViewModel.RequestClose += handle;
            addCustomerNameView.DataContext = addCustomerNameViewModel;
            addCustomerNameView.ShowDialogWindow();

            if (addCustomerNameViewModel.IsSave)
            {
                IsCreateCustomer = addCustomerNameViewModel.IsSave;//true
                ObservableCollection<Customer> TempCompanyGroupList = new ObservableCollection<Customer>(CompanyGroupList);

                TempCompanyGroupList.Add(new Customer() { CustomerName = addCustomerNameViewModel.CustomerName.ToUpper(), IsStillActive = 1, Logo = "", IdCustomerType = 1 });

                CompanyGroupList = new ObservableCollection<Customer>(TempCompanyGroupList);

                SelectedIndexCompanyGroup = CompanyGroupList.IndexOf(CompanyGroupList.FirstOrDefault(cmpg => cmpg.CustomerName == addCustomerNameViewModel.CustomerName.ToUpper()));
            }

            GeosApplication.Instance.Logger.Log("Method AddCustomerName() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method for fill business center.
        /// </summary>
        private void FillBusinessCenter()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillBusinessCenter ...", category: Category.Info, priority: Priority.Low);

                IList<LookupValue> tempBusinessUnitList = CrmStartUp.GetLookupValues(6);
                BusinessCenter = new List<LookupValue>();
                BusinessCenter = new List<LookupValue>(tempBusinessUnitList.Where(inUseOption => inUseOption.InUse == true));
                BusinessCenter.Insert(0, new LookupValue() { Value = "---", InUse = true });
                //BusinessCenter.AddRange(tempBusinessUnitList);

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
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillBusinessCenter() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for fill business field.
        /// </summary>
        private void FillBusinessField()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillBusinessField ...", category: Category.Info, priority: Priority.Low);

                IList<LookupValue> tempBusinessUnitList = CrmStartUp.GetLookupValues(5);
                BusinessField = new List<LookupValue>();
                BusinessField.Add(new LookupValue() { Value = "---", InUse = true });
                BusinessField.AddRange(tempBusinessUnitList.Where(inUseOption => inUseOption.InUse == true));
                selectedIndexBusinessField = 0;

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
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillBusinessField() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for search location.
        /// </summary>
        /// <param name="obj"></param>
        public void SearchButtonClickCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method SearchButtonClickCommandAction ...", category: Category.Info, priority: Priority.Low);

            //BingSearchDataProvider bingSearchDataProvider = (BingSearchDataProvider)obj;
            //bingSearchDataProvider.Search(CustomerCity);
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

            if (customerSetLocationMapViewModel.MapLatitudeAndLongitude != null
                && !string.IsNullOrEmpty(customerSetLocationMapViewModel.LocationAddress))
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
        /// Method for fill Fill Country.
        /// </summary>
        #region //chitra.girigosavi GEOS2-6498 When add/Edit customer taking to much time to load form
        private void FillCountry()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCountry ...", category: Category.Info, priority: Priority.Low);

                IList<Country> tempCountryList = CrmStartUp.GetCountries_V2570();
                CountryList = new List<Country>(tempCountryList);
                // Insert "---" at the 0 index after populating the list
                CountryList.Insert(0, new Country() { Name = "---" });

                SelectedIndexCountry = 0;

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
        #endregion
        /// <summary>
        /// Method for fill emdep Group list.
        /// </summary>
        private void FillGroupList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillGroupList ...", category: Category.Info, priority: Priority.Low);

                //Added user permission type 22 for show all group type.
                if (CompanyGroupList == null)
                    CompanyGroupList = new ObservableCollection<Customer>();

                //CompanyGroupList.AddRange(CrmStartUp.GetCompanyGroup(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, 22, true));
                //[pramod.misal][GEOS2-4682][08-08-2023]
                //[19.10.2023][GEOS2-4903][rdixit] Service GetCompanyGroup_V2420 updated with GetCompanyGroup_V2450
                CompanyGroupList.AddRange(CrmStartUp.GetCompanyGroup_V2450(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, 22, true));
                SelectedIndexCompanyGroup = 0;
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
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillGroupList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }



        /// <summary>
        /// Method For Save Cutomer Details
        /// [001][cpatil][GEOS2-2196] PCM customer/region update when creating a new account
        /// </summary>
        /// <param name="obj"></param>
        private void SaveCustomerDetails(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SaveCustomerDetails ...", category: Category.Info, priority: Priority.Low);
                IsBusy = true;
                InformationError = null;
                AssignedSalesOwnerError = null;
                IsShowErrorInAddress = false;
                ShowAddValidationVisibility = Visibility.Hidden;
                if (CustomerAddress != null && CustomerAddress != "")
                {
                    if (RegisteredName != null && RegisteredName != "")
                    {
                        if (CustomerAddress.ToUpper().Contains(RegisteredName.ToUpper()))
                        {
                            IsShowErrorInAddress = true;
                        }
                    }
                    if (RegisterationNumber != null && RegisterationNumber != "")
                    {
                        if (CustomerAddress.Contains(RegisterationNumber))
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

                    if (Alias != null && Alias != "")
                    {
                        if (CustomerAddress.ToUpper().Contains(Alias.ToUpper()))
                        {
                            IsShowErrorInAddress = true;
                        }
                    }

                }
                string error = EnableValidationAndGetError();
                if (string.IsNullOrEmpty(error))
                    InformationError = null;
                else
                    InformationError = "";
                if (SalesOwnerList.Count != 0 || (ListSalesUsers.Any(i => i.IdPerson == GeosApplication.Instance.ActiveUser.IdUser)))
                {
                    AssignedSalesOwnerError = null;
                }
                else
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
                //[RGadhave][12.11.2024][GEOS2-6462]
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexIncoterms"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexPaymentTerms"));
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
                    if (CompanyGroupList[SelectedIndexCompanyGroup].IdCustomer == 0)
                    {
                        Customer addCustomer = CompanyGroupList[SelectedIndexCompanyGroup];
                        addCustomer = CrmStartUp.AddCustomer(addCustomer);
                        CompanyGroupList[SelectedIndexCompanyGroup].IdCustomer = addCustomer.IdCustomer;
                    }

                    CustomerData = new Company();
                    if (!string.IsNullOrEmpty(CustomerAccountName))
                        CustomerData.RegisteredName = CustomerAccountName.Trim();

                    CustomerData.IdSource = SelectedSource.IdLookupValue;//[Sudhir.Jangra][GEOS2-4663][28/08/2023] //rajashri[GEOS2-4964]
                    if (SelectedSource.Value == "---")
                    {
                        CustomerData.SourceName = "";//[Sudhir.Jangra][GEOS2-4664]
                    }
                    else
                    {
                        CustomerData.SourceName = SelectedSource.Value;
                    }
                    CustomerData.IdCustomer = Convert.ToInt32(CompanyGroupList[SelectedIndexCompanyGroup].IdCustomer);
                    if (!string.IsNullOrEmpty(CustomerPlantName))
                        CustomerData.Name = CustomerPlantName.Trim();
                    CustomerData.IdBusinessCenter = Convert.ToByte(BusinessCenter[SelectedIndexBusinessCenter].IdLookupValue);
                    CustomerData.IdBusinessField = Convert.ToByte(BusinessField[SelectedIndexBusinessField].IdLookupValue);
                    CustomerData.Country = CountryList[SelectedIndexCountry];
                    CustomerData.IdCountry = Convert.ToByte(CountryList[SelectedIndexCountry].IdCountry);

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

                    if (!string.IsNullOrEmpty(CustomerState))
                        CustomerData.Region = CustomerState.Trim();
                    if (!string.IsNullOrEmpty(CustomerWebsite))
                        CustomerData.Website = CustomerWebsite.Trim();

                    CustomerData.Line = CustomerLines;
                    CustomerData.CuttingMachines = CuttingMachines;

                    if (!string.IsNullOrEmpty(CustomerAddress))
                        CustomerData.Address = CustomerAddress.Trim();
                    if (!string.IsNullOrEmpty(RegisteredName))
                        CustomerData.RegisteredName = RegisteredName.Trim();
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
                    //[RGadhave][12.11.2024][GEOS2-6462]
                    if (!string.IsNullOrEmpty(Alias))
                        CustomerData.Alias = Alias.Trim();

                    CustomerData.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                    CustomerData.Latitude = Latitude;
                    CustomerData.Longitude = Longitude;
                    //CustomerData.IdSalesResponsible = GeosApplication.Instance.ActiveUser.IdUser;
                    CustomerData.People = new People();
                    CustomerData.People.Name = GeosApplication.Instance.ActiveUser.FirstName;
                    CustomerData.People.Surname = GeosApplication.Instance.ActiveUser.LastName;
                    CustomerData.People.FullName = GeosApplication.Instance.PeopleList.FirstOrDefault(pl => pl.IdPerson == GeosApplication.Instance.ActiveUser.IdUser).FullName;
                    CustomerData.People.Email = GeosApplication.Instance.PeopleList.FirstOrDefault(pl => pl.IdPerson == GeosApplication.Instance.ActiveUser.IdUser).Email;
                    CustomerData.People.IdPerson = GeosApplication.Instance.ActiveUser.IdUser;
                    //[GEOS2-4279][rdixit][05.05.2023]
                    CustomerData.SalesOwnerList = new List<People>(SalesOwnerList);
                    if (CustomerData.SalesOwnerList.Count == 0)
                    {
                        if (ListSalesUsers.Any(i => i.IdPerson == GeosApplication.Instance.ActiveUser.IdUser))
                        {
                            People saleowner = new People();
                            saleowner.IdPerson = GeosApplication.Instance.ActiveUser.IdUser;
                            saleowner.Name = GeosApplication.Instance.PeopleList.FirstOrDefault(pl => pl.IdPerson == GeosApplication.Instance.ActiveUser.IdUser).Name;
                            saleowner.Surname = GeosApplication.Instance.PeopleList.FirstOrDefault(pl => pl.IdPerson == GeosApplication.Instance.ActiveUser.IdUser).Surname;
                            saleowner.FullName = GeosApplication.Instance.PeopleList.FirstOrDefault(pl => pl.IdPerson == GeosApplication.Instance.ActiveUser.IdUser).FullName;
                            saleowner.Email = GeosApplication.Instance.PeopleList.FirstOrDefault(pl => pl.IdPerson == GeosApplication.Instance.ActiveUser.IdUser).Email;
                            saleowner.IsSalesResponsible = true;
                            CustomerData.SalesOwnerList.Add(saleowner);
                        }
                        //else
                        //{
                        //    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("SalesOwnerErrorMessage").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        //    IsBusy = false;
                        //    CustomerData = null;
                        //    return;
                        //}
                    }

                    #region PrevCommented [GEOS2-4279][rdixit][05.05.2023]
                    /*
                    // Sales responsible
                    if (SalesOwnerList.Count == 0)
                    {
                        CustomerData.IdSalesResponsibleAssemblyBU = GeosApplication.Instance.ActiveUser.IdUser;
                        CustomerData.PeopleSalesResponsibleAssemblyBU = new People();
                        CustomerData.PeopleSalesResponsibleAssemblyBU.IdPerson = GeosApplication.Instance.PeopleList.FirstOrDefault(pl => pl.IdPerson == GeosApplication.Instance.ActiveUser.IdUser).IdPerson;
                        CustomerData.PeopleSalesResponsibleAssemblyBU.Name = GeosApplication.Instance.PeopleList.FirstOrDefault(pl => pl.IdPerson == GeosApplication.Instance.ActiveUser.IdUser).Name;
                        CustomerData.PeopleSalesResponsibleAssemblyBU.Surname = GeosApplication.Instance.PeopleList.FirstOrDefault(pl => pl.IdPerson == GeosApplication.Instance.ActiveUser.IdUser).Surname;
                        CustomerData.PeopleSalesResponsibleAssemblyBU.FullName = GeosApplication.Instance.PeopleList.FirstOrDefault(pl => pl.IdPerson == GeosApplication.Instance.ActiveUser.IdUser).FullName;
                        CustomerData.PeopleSalesResponsibleAssemblyBU.Email = GeosApplication.Instance.PeopleList.FirstOrDefault(pl => pl.IdPerson == GeosApplication.Instance.ActiveUser.IdUser).Email;
                    }
                   
                    if (SalesOwnerList.Count != 0)
                    {
                        if (SalesOwnerList.Count == 1)
                        {
                            if (SalesOwnerList[0].IsSelected == true && SalesOwnerList[0].IsSalesResponsible == true)
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
                                    if (item.IsSalesResponsible == true)
                                        CustomerData.IdSalesResponsible = item.IdPerson;
                                    else
                                        CustomerData.IdSalesResponsibleAssemblyBU = item.IdPerson;
                                }
                            }
                            else
                            {
                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("CustomerEditViewWarningSalesResponsible").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                IsBusy = false;
                                return;

                            }
                        }
                    }*/
                    //else
                    //{
                    //    CustomerData.IdSalesResponsible = null;
                    //    CustomerData.IdSalesResponsibleAssemblyBU = null;
                    //}
                    #endregion
                    //CustomerData.ShortName = "";
                    CustomerData.ShortName = CustomerData.Alias;
                    List<string> bpstring = new List<string>();
                    CustomerData.BusinessProductList = new List<SitesByBusinessProduct>();
                    foreach (var item in SelectedBusinessProductItems)
                    {
                        CustomerData.BusinessProductList.Add(new SitesByBusinessProduct() { IdBusinessProduct = ((LookupValue)item).IdLookupValue, IdSite = 0, IsAdded = true });
                        bpstring.Add(((LookupValue)item).Value.ToString());
                    }

                    // Create notification and email to send to admin when account created
                    Notification notification = new Notification();
                    notification.FromUser = GeosApplication.Instance.ActiveUser.IdUser;
                    notification.Title = "New Account";
                    notification.Message = string.Format("A new account named \"{0} - {1} ({2})\" has been created by {3}.", CompanyGroupList[SelectedIndexCompanyGroup].CustomerName, CustomerPlantName, CountryList[SelectedIndexCountry].Name, GeosApplication.Instance.ActiveUser.FullName);
                    notification.IdModule = 5;
                    notification.Status = "Unread";
                    notification.IsNew = 1;

                    MailTemplateFormat mailTemplateFormat = new MailTemplateFormat();
                    // mailTemplateFormat.SendToUserName = "" because it is added from service. (fullname)
                    mailTemplateFormat.EmailForSection = System.Windows.Application.Current.FindResource("ActivityAccount").ToString();
                    mailTemplateFormat.AccountName = string.Format("{0} - {1}", CompanyGroupList[SelectedIndexCompanyGroup].CustomerName, CustomerPlantName);
                    mailTemplateFormat.CreatedByUserName = GeosApplication.Instance.ActiveUser.FullName;
                    notification.MailTemplateFormat = mailTemplateFormat;

                    Company companyReturnValue = null;
                    try
                    {
                        Company EmdepSite = null;

                        if (GeosApplication.Instance.EmdepSiteList != null)
                        {
                            Company temp = GeosApplication.Instance.EmdepSiteList.FirstOrDefault(x => x.Alias == GeosApplication.Instance.SiteName);

                            if (temp != null)
                            {
                                EmdepSite = new Company();
                                EmdepSite.IdCompany = temp.IdCompany;
                            }
                        }
                        CustomerData.Country.CountryIconImage = null;
                        CustomerData.SalesOwnerList.ForEach(i => i.OwnerImage = null);
                        CustomerData.IdPaymentType = PaymentTermsList[SelectedIndexPaymentTerms].IdPaymentType;
                        CustomerData.Latitude = MapLatitudeAndLongitude?.Latitude;
                        CustomerData.Longitude = MapLatitudeAndLongitude?.Longitude;
                        #region Service Comments
                        //[001]
                        //companyReturnValue = CrmStartUp.AddCompany_V2043(CustomerData, EmdepSite);
                        //[GEOS2-3994][rdixit][17.11.2022]
                        //Service Method AddCompany_V2340 Updated with AddCompany_V2390 by [rdixit][GEOS2-4279][05.05.2023]

                        // companyReturnValue = CrmStartUp.AddCompany_V2390(CustomerData, EmdepSite);
                        //[Sudhir.Jangra][GEOS2-4663][28/08/2023]
                        //[RGadhave][12.11.2024][GEOS2-6462]


                        //  CrmStartUp = new CrmServiceController("localhost:6699");
                        //companyReturnValue = CrmStartUp.AddCompany_V2430(CustomerData, EmdepSite);
                        #endregion
                        // CrmStartUp = new CrmServiceController("localhost:6699");
                        //companyReturnValue = CrmStartUp.AddCompany_V2580(CustomerData, SelectedIncotermId, SelectedPaymentTermsName, ShippingAddressList, EmdepSite);

                        //chitra.girigosavi[GEOS2-7242][10/04/2025]
                        companyReturnValue = CrmStartUp.AddCompany_V2630(CustomerData, SelectedIncotermId, SelectedPaymentTermsName, ShippingAddressList, EmdepSite);

                        if (companyReturnValue != null && companyReturnValue.IdCompany > 0)
                        {
                            notification = CrmStartUp.AddCommonNotification(notification);
                            if (notification.Id > 0)
                            {
                                GeosApplication.Instance.Logger.Log("Add customer Notification added successfully...", category: Category.Info, priority: Priority.Low);
                            }

                            CustomerData.IdCompany = companyReturnValue.IdCompany;
                            CustomerData.Customers = new List<Customer>();
                            CustomerData.Customers.Add(CompanyGroupList[SelectedIndexCompanyGroup]);

                            CustomerData.BusinessField = BusinessField[SelectedIndexBusinessField];
                            CustomerData.BusinessCenter = BusinessCenter[SelectedIndexBusinessCenter];
                            CustomerData.Country = CountryList[SelectedIndexCountry];
                            CustomerData.BusinessProductString = string.Join(" ,", bpstring.ToList());
                            CustomerData.BusinessProductString = CustomerData.BusinessProductString.Replace(",", "\n");
                            CustomerData.CreatedIn = GeosApplication.Instance.ServerDateTime.Date;
                            CustomerData.PeopleCreatedBy = new People();
                            CustomerData.PeopleCreatedBy.Name = GeosApplication.Instance.ActiveUser.FirstName;
                            CustomerData.PeopleCreatedBy.Surname = GeosApplication.Instance.ActiveUser.LastName;



                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("CustomerAddViewCreatedSuccessfully").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                            RequestClose(null, null);
                        }
                        else if (companyReturnValue.IdCompany == -1)
                        {
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("CustomerAddViewCompanyExist").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        }
                        else
                        {
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("CustomerAddViewNotCreated").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        }
                        IsBusy = false;
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
                        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
                    }
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                CustomerData = null;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SaveCustomerDetails() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for fill emdep BusinessCenter.
        /// </summary>
        /// 
        private void FillBusinessProduct()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillBusinessProduct ...", category: Category.Info, priority: Priority.Low);

                IList<LookupValue> tempBusinessProductList = CrmStartUp.GetLookupValues(7);
                BusinessProduct = new List<LookupValue>();
                BusinessProduct = new List<LookupValue>(tempBusinessProductList.Where(inUseOption => inUseOption.InUse == true));
                //BusinessProduct.AddRange(tempBusinessProductList);
                SelectedBusinessProductItems = new List<Object>();

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
        /// Method for Fill all contact list depend on Sales Owner. 
        /// </summary>
        private void FillUsers()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillUsers ...", category: Category.Info, priority: Priority.Low);
                //Updated Service Method GetAllActivePeoples with GetAllActivePeoples_V2390 TO get only IsStillActive and IsEnabled Users
                ListSalesUsers = new ObservableCollection<People>(CrmStartUp.GetAllActivePeoples_V2390());
                GeosApplication.Instance.Logger.Log("Method FillUsers() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillUsers() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillUsers() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
        }

        ///// <summary>
        ///// Method for fill Sales Owner list.
        ///// </summary>
        private void FillSalesOwnerList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillSalesOwnerList ...", category: Category.Info, priority: Priority.Low);
                People people = new People();
                if (SalesOwnerList.Count == 0)
                {
                    people = ListSalesUsers.FirstOrDefault(f => f.IdPerson == GeosApplication.Instance.ActiveUser.IdUser);

                    User user = WorkbenchStartUp.GetUserById(GeosApplication.Instance.ActiveUser.IdUser);
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
                }

                // if there is only one saleOwner then assign SalesResponsible also.
                if (SalesOwnerList != null && SalesOwnerList.Count == 1)
                {
                    SalesOwnerList[0].IsSalesResponsible = true;
                    SalesResponsible = SalesOwnerList[0];
                    SalesOwnerList[0].IsSelected = true;
                }

                SalesResponsible = SalesOwnerList.FirstOrDefault(x => x.IsSalesResponsible == true);
                ListSalesUsers.Remove(people);
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
                GeosApplication.Instance.Logger.Log("Get an error in FillSalesOwnerList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillSalesOwnerList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// This Method for Add or Remove Sales User in Customer through check uncheck
        /// </summary>
        /// <param name="e"></param>
        public void SalesUserContactCheckedAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method SalesUserContactCheckedAction ...", category: Category.Info, priority: Priority.Low);

            try
            {
                if (obj is People)
                {
                    People people = obj as People;
                    #region Prev Code [GEOS2-4279][rdixit][05.05.2023]
                    // (People)obj;
                    //SalesOwnerList = new ObservableCollection<People>();
                    //if (SalesOwnerList.Count < 2)
                    //{
                    #endregion
                    User user = WorkbenchStartUp.GetUserById(people.IdPerson);
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
                    #region Prev Code [GEOS2-4279][rdixit][05.05.2023]
                    //TempListChangeLog.Add(new LogEntryBySite() { IdSite = SelectedCompanyList[0].IdCompany, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("CustomerEditViewUserChangeLogEntryAssignedSalesAdded").ToString(), people.FullName) });
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
                GeosApplication.Instance.Logger.Log("Get an error in SalesUserContactCheckedAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// This Method is for set a sales responsible
        /// </summary>
        /// <param name="obj"></param>
        public void SetSalesResponsibleCommandAction(object obj)
        {
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
                                //TempListChangeLog.Add(new LogEntryBySite() { IdSite = SelectedCompanyList[0].IdCompany, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("CustomerEditViewUserChangeLogEntryAssignedSalesResponsibleAdded").ToString(), item.FullName) });
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
                                    //TempListChangeLog.Add(new LogEntryBySite() { IdSite = SelectedCompanyList[0].IdCompany, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("CustomerEditViewUserChangeLogEntryAssignedSalesResponsibleChanged").ToString(), OldSalesResponsible, SalesResponsible.FullName) });
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
                                    //TempListChangeLog.Add(new LogEntryBySite() { IdSite = SelectedCompanyList[0].IdCompany, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("CustomerEditViewUserChangeLogEntryAssignedSalesResponsibleRemoved").ToString(), item.FullName) });
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
        /// Method to delete linkeditem from linked items.
        /// </summary>
        /// <param name="obj"></param>
        private void AssignedSalesCancelCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method AssignedSalesCancelCommandAction ...", category: Category.Info, priority: Priority.Low);

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

                    // if there is only one saleOwner then assign SalesResponsible also.
                    if (SalesOwnerList.Count == 1 && (!SalesOwnerList.Any(j => j.IsSalesResponsible == true)))
                    {
                        SalesOwnerList[0].IsSalesResponsible = true;
                        SalesResponsible = SalesOwnerList[0];
                        SalesOwnerList[0].IsSelected = true;
                    }

                    ListSalesUsers.Add(people);
                }

                GeosApplication.Instance.Logger.Log("Method AssignedSalesCancelCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AssignedSalesCancelCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Remove user profile picture 
        /// </summary>
        /// <param name="obj"></param>
        public void RemoveCustomerImage(object obj)
        {
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
        /// Method For Close Window
        /// </summary>
        /// <param name="obj"></param>
        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }
        public void Dispose()
        {

            throw new NotImplementedException();
        }
        /// <summary>
        /// Method to fill City list
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
                if (Visible == Visibility.Hidden.ToString())
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
        //[Sudhir.Jangra][GEOS2-4663][28/08/2023]
        private void FillSourceList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillBusinessCenter ...", category: Category.Info, priority: Priority.Low);

                //  IList<LookupValue> tempSourceListList = CrmStartUp.GetLookupValues(126);
                SourceList = new ObservableCollection<LookupValue>(CrmStartUp.GetLookupValues(126));
                SourceList.Insert(0, new LookupValue() { Value = "---", InUse = true });
                //  SourceList.AddRange(tempSourceListList);
                SelectedSource = SourceList.FirstOrDefault();


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
                    //LookupValue lookupValue = new LookupValue();
                    //lookupValue.IdLookupValue = addSourceViewModel.NewSourceList.IdLookupValue;
                    //lookupValue.Value = addSourceViewModel.NewSourceList.Value;
                    //lookupValue.HtmlColor = addSourceViewModel.NewSourceList.HtmlColor;
                    //lookupValue.IdLookupKey = addSourceViewModel.NewSourceList.IdLookupKey;
                    //lookupValue.Position = addSourceViewModel.NewSourceList.Position;
                    //lookupValue.InUse = addSourceViewModel.NewSourceList.InUse;

                    //SourceList.Add(lookupValue);
                    SelectedSource = SourceList.FirstOrDefault(x => x.Value == addSourceViewModel.NewSourceList.Value);

                }

                GeosApplication.Instance.Logger.Log("Method AddSourceButtonCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddSourceButtonCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }



        /// <summary>
        /// Method for fill emdep ncoterms List.
        /// </summary>
        private void FillIncotermsList()
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

                SelectedIndexIncoterms = IncotermsList.IndexOf(IncotermsList.FirstOrDefault());  // Get the index of the first item



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


        private void FillPaymentTermsList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillPaymentTermsList ...", category: Category.Info, priority: Priority.Low);


                if (PaymentTermsList == null)
                    PaymentTermsList = new ObservableCollection<CustomerDetail>();
                //    CrmStartUp = new CrmServiceController("localhost:6699");

                PaymentTermsList.AddRange(CrmStartUp.GetPaymentTermsList_V2580());
                PaymentTermsList.Insert(0, new CustomerDetail() { PaymentTypeName = "---" });

                SelectedIndexPaymentTerms = PaymentTermsList.IndexOf(PaymentTermsList.FirstOrDefault());
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
        #endregion // Methods
        // [nsatpute][22-11-2024][GEOS2-6462]
        private void AddShippingAddressCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddShippingAddressCommandAction()...", category: Category.Info, priority: Priority.Low);
                AddCustomerViewModel addCustomerViewModel = this;
                AddEditShippingAddressView addEditShippingAddressView = new AddEditShippingAddressView();
                AddEditShippingAddressViewModel addEditShippingAddressViewModel = new AddEditShippingAddressViewModel();
                EventHandler handle = delegate { addEditShippingAddressView.Close(); };
                addEditShippingAddressViewModel.RequestClose += handle;
                addEditShippingAddressViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddShippingAddress").ToString();
                addEditShippingAddressViewModel.IsNew = true;
                addEditShippingAddressView.DataContext = addEditShippingAddressViewModel;
                addEditShippingAddressView.ShowDialog();

                if (addEditShippingAddressViewModel.IsSave)
                {
                    #region [GEOS2-6462][rdixit][23.11.2024]
                    if (ShippingAddressList == null)
                        ShippingAddressList = new ObservableCollection<Emdep.Geos.Data.Common.Crm.ShippingAddress>();

                    if (addEditShippingAddressViewModel.IsPrimaryAddress)
                    {
                        var oldprimary = ShippingAddressList.FirstOrDefault(x => x.IsDefault == true);
                        if (oldprimary != null)
                            oldprimary.IsDefault = false;
                    }

                    ShippingAddressList.Add(addEditShippingAddressViewModel.NewAddress);

                    if (addEditShippingAddressViewModel.IsPrimaryAddress)
                        ReorderShippingAddresses();

                    ShippingAddressList = new ObservableCollection<Data.Common.Crm.ShippingAddress>(ShippingAddressList);
                    #endregion
                }
                GeosApplication.Instance.Logger.Log("Method AddShippingAddressCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddShippingAddressCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

        }
        // [nsatpute][22-11-2024][GEOS2-6462]
        public void SetDefaultShippingAddressCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SetDefaultShippingAddressCommandAction...", category: Category.Info, priority: Priority.Low);
                if (obj != null)
                {
                    SelectedLinkedAddress = obj as Emdep.Geos.Data.Common.Crm.ShippingAddress;
                    ShippingAddressList.ToList().ForEach(x => x.IsDefault = false);
                    SelectedLinkedAddress.IsDefault = true;
                    ReorderShippingAddresses();
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

        //chitra.girigosavi GEOS2-7242 10/04/2025
        private void FillStatusList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillStatusList()...", category: Category.Info, priority: Priority.Low);

                IList<LookupValue> tempStatusList = CrmStartUp.GetLookupValues(177);
                StatusList = new List<LookupValue>();
                StatusList = new List<LookupValue>(tempStatusList);
                SelectedStatusIndex = -1;
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
    }
}
