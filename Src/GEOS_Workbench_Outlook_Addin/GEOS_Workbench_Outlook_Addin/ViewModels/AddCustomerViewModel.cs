using DevExpress.Mvvm;
using DevExpress.Xpf.CodeView;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Map;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
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
        #endregion // Declaration

        #region Properties


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

        public string CustomerAddress
        {
            get { return customerAddress; }
            set
            {

                customerAddress = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomerAddress"));

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
                    me[BindableBase.GetPropertyName(()=>InformationError)]+
                    me[BindableBase.GetPropertyName(() => CustomerCity)];

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
                    return CustomerAddRequiredFieldValidation.GetErrorMessage(CustomerAddressProp, CustomerAddress);

                if (columnName == CustomerCityProp)
                    return CustomerAddRequiredFieldValidation.GetErrorMessage(CustomerCityProp, CustomerCity);

                if(columnName== informationError)
                    return CustomerAddRequiredFieldValidation.GetErrorMessage(informationError, InformationError);
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
            PlantNameLst = AllPlantsLst.Where(cust => cust.IdCustomer == SelectedIdCustomer).ToList();
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
                BusinessField.Insert(0, new LookupValue() { Value = "---", InUse = true });
                BusinessField = new List<LookupValue>(tempBusinessUnitList.Where(inUseOption => inUseOption.InUse == true));
                //BusinessField.AddRange(tempBusinessUnitList);

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
        private void FillCountry()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCountry ...", category: Category.Info, priority: Priority.Low);

                IList<Country> tempCountryList = CrmStartUp.GetCountries();
                CountryList = new List<Country>();
                CountryList.Insert(0, new Country() { Name = "---" });
                CountryList.AddRange(tempCountryList);
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

                CompanyGroupList.AddRange(CrmStartUp.GetCompanyGroup(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, 22, true));

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
        /// </summary>
        /// <param name="obj"></param>
        private void SaveCustomerDetails(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SaveCustomerDetails ...", category: Category.Info, priority: Priority.Low);
                IsBusy = true;
                InformationError = null;
                string error = EnableValidationAndGetError();
                if (string.IsNullOrEmpty(error))
                    InformationError = null;
                else
                    InformationError = "";
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexCompanyGroup"));
                PropertyChanged(this, new PropertyChangedEventArgs("CustomerPlantName"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedBusinessProductItems"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexBusinessField"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexBusinessCenter"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexCountry"));
                PropertyChanged(this, new PropertyChangedEventArgs("CustomerAddress"));
                PropertyChanged(this, new PropertyChangedEventArgs("CustomerCity"));
                PropertyChanged(this, new PropertyChangedEventArgs("InformationError"));

                if (error != null)
                {
                    IsBusy = false;
                    return;
                }
                else
                {
                    if (CompanyGroupList[SelectedIndexCompanyGroup].IdCustomer == 0)
                    {
                        Customer addCustomer = CompanyGroupList[SelectedIndexCompanyGroup];
                        addCustomer = CrmStartUp.AddCustomer(addCustomer);
                        CompanyGroupList[SelectedIndexCompanyGroup].IdCustomer = addCustomer.IdCustomer;
                    }

                    CustomerData = new Company();
                    if (!string.IsNullOrEmpty(CustomerAccountName))
                        CustomerData.RegisteredName = CustomerAccountName.Trim();
                    CustomerData.Size = Size;
                    CustomerData.NumberOfEmployees = CustomerNumberofEmplyee;

                    CustomerData.IdCustomer = Convert.ToInt32(CompanyGroupList[SelectedIndexCompanyGroup].IdCustomer);
                    if (!string.IsNullOrEmpty(CustomerPlantName))
                        CustomerData.Name = CustomerPlantName.Trim();
                    CustomerData.IdBusinessCenter = Convert.ToByte(BusinessCenter[SelectedIndexBusinessCenter].IdLookupValue);
                    CustomerData.IdBusinessField = Convert.ToByte(BusinessField[SelectedIndexBusinessField].IdLookupValue);
                    CustomerData.Country = CountryList[SelectedIndexCountry];
                    CustomerData.IdCountry = Convert.ToByte(CountryList[SelectedIndexCountry].IdCountry);

                    if (!string.IsNullOrEmpty(CustomerState))
                        CustomerData.Region = CustomerState.Trim();
                    if (!string.IsNullOrEmpty(CustomerWebsite))
                        CustomerData.Website = CustomerWebsite.Trim();

                    CustomerData.Line = CustomerLines;
                    CustomerData.CuttingMachines = CuttingMachines;

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

                    CustomerData.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                    CustomerData.Latitude = Latitude;
                    CustomerData.Longitude = Longitude;
                    CustomerData.IdSalesResponsible = GeosApplication.Instance.ActiveUser.IdUser;
                    CustomerData.People = new People();
                    CustomerData.People.Name = GeosApplication.Instance.ActiveUser.FirstName;
                    CustomerData.People.Surname = GeosApplication.Instance.ActiveUser.LastName;
                    CustomerData.People.FullName = GeosApplication.Instance.PeopleList.FirstOrDefault(pl => pl.IdPerson == GeosApplication.Instance.ActiveUser.IdUser).FullName;
                    CustomerData.People.Email = GeosApplication.Instance.PeopleList.FirstOrDefault(pl => pl.IdPerson == GeosApplication.Instance.ActiveUser.IdUser).Email;
                    CustomerData.People.IdPerson = GeosApplication.Instance.ActiveUser.IdUser;
                    
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
                    }
                    //else
                    //{
                    //    CustomerData.IdSalesResponsible = null;
                    //    CustomerData.IdSalesResponsibleAssemblyBU = null;
                    //}

                    CustomerData.ShortName = "";

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

                        companyReturnValue = CrmStartUp.AddCompany(CustomerData, EmdepSite);

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
                            CustomerData.PeopleCreatedBy.Surname  = GeosApplication.Instance.ActiveUser.LastName ;
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
                ListSalesUsers = new ObservableCollection<People>(CrmStartUp.GetAllActivePeoples());
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
                    People people = obj as People;  // (People)obj;
                                                    //SalesOwnerList = new ObservableCollection<People>();
                    if (SalesOwnerList.Count < 2)
                    {
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
                        //TempListChangeLog.Add(new LogEntryBySite() { IdSite = SelectedCompanyList[0].IdCompany, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("CustomerEditViewUserChangeLogEntryAssignedSalesAdded").ToString(), people.FullName) });
                    }
                    else
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("CustomerEditViewWarningExceedAddSalesResponsible").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        //if (cellValueEventArgs.Column.FieldType == typeof(bool))
                        //{
                        //    cellValueEventArgs.Source.CloseEditor();
                        //    people.IsSelected = false;
                        //}
                    }

                    //}
                    //else
                    //{
                    //    SalesOwnerList.Remove(people);
                    //    SalesResponsible = SalesOwnerList.FirstOrDefault(x => x.IsSalesResponsible == true);
                    //    TempListChangeLog.Add(new LogEntryBySite() { IdSite = SelectedCompanyList[0].IdCompany, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("CustomerEditViewUserChangeLogEntryAssignedSalesDeleted").ToString(), people.FullName) });
                    //}

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
                    if (SalesOwnerList.Count == 1)
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
        #endregion // Methods
    }
}
