using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
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
    public class AddUserViewModel : NavigationViewModelBase, IDataErrorInfo, INotifyPropertyChanged, IDisposable
    {
        #region Services

        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion // Services

        #region Commands
        public ICommand AddUserCancelButtonCommand { get; set; }
        public ICommand SalesQuotaAmountChangedCommand { get; set; }
        public ICommand AddUserAcceptButtonCommand { get; set; }
        public ICommand SearchUserButtonCommand { get; set; }
        public ICommand HyperlinkForEmail { get; set; }
        public ICommand SalesQuotaAmountLostFocusCommand { get; set; }
        public ICommand SalesQuotaCurrencyChangedCommand { get; set; }
        public ICommand CurrencyRateDateEditValueChangingCommand { get; set; }
        public ICommand CommandTextInput { get; set; }
        #endregion

        #region Declaration

        private SalesUser salesUser;
        private People contactData;
        private bool isSave;
        byte[] UserProfileImageByte = null;

        private string firstName;
        private string lastName;
        private ObservableCollection<LookupValue> userGenderList;
        private List<Customer> listSalesUnit;
        private List<Company> listPlant;
        private List<int> listpermission;

        private int selectedIndexGender = -1;
        //private int selectedIndexCountry = -1;
        private int selectedIndexSalesUnit = -1;
        private int selectedIndexCompanyPlant = -1;
        private int selectedIndexPermission = -1;

        private string salesOwnersIds = "";
        private ObservableCollection<People> selectedContact = new ObservableCollection<People>();
        private string email;
        bool isBusy;
        private int selectedIndexCurrency = 0;
        private List<Currency> currencies;
        private SalesUser saleUserData;
        private SalesUser salesQuota;
        private ObservableCollection<SalesUserQuota> salesUserQuotas;
        private ObservableCollection<UserQuotas> userQuotasList;
        private List<User> userList;
        private string office;
        private User user;
        private Object selectedPermissionList;
        private List<object> selectedAuthorizedPlant;
        private List<object> selectedPermission;
        private Company selectedPlant;
        private List<LookupValue> salesUnitList;
        private List<Permission> listPermissions;
        private List<EmdepSite> listAuthorizedPlants;
        private User selectedUser;
        private List<EmdepSite> listPlants;
        private List<int> authorizedPlant;
        private int idUser;
      //  private string firstName;
        private bool isSaveUser;
        private int selectedIndexAuthorizedPlant;
        private int selectedSalesUnit;
        public People UserDetail { get; set; }
        private DateTime? currencyRateDate;
        private DateTime? currencyRateMinDate;
        private DateTime? currencyRateMaxDate;
        private double currencyConversionRate;
        private string toCurrency;
        private int tabIndex;
        private double salesQuotaAmount;
        private byte IdCurrencyFrom;
        private string visible;
        #endregion // Declaration
        private int selectedIndexCurrencynew;

        #region Properties

        public int SelectedIndexCurrencynew
        {
            get
            {
                return selectedIndexCurrencynew;
            }

            set
            {
                selectedIndexCurrencynew = value; OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexCurrencynew"));

            }
        }

        public ObservableCollection<UserQuotas> UserQuotasList
        {
            get
            {
                return userQuotasList;
            }

            set
            {
                userQuotasList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UserQuotasList"));
            }
        }

        public double CurrencyConversionRate
        {
            get
            {
                return currencyConversionRate;
            }

            set
            {
                currencyConversionRate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CurrencyConversionRate"));
            }
        }
        public string ToCurrency
        {
            get
            {
                return toCurrency;
            }

            set
            {
                toCurrency = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ToCurrency"));
            }
        }
        public DateTime? CurrencyRateMinDate
        {
            get
            {
                return currencyRateMinDate;
            }
            set
            {
                currencyRateMinDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CurrencyRateMinDate"));
            }
        }
        public DateTime? CurrencyRateMaxDate
        {
            get
            {
                return currencyRateMaxDate;
            }
            set
            {
                currencyRateMaxDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CurrencyRateMaxDate"));
            }
        }

        public int TabIndex
        {
            get
            {
                return tabIndex;
            }

            set
            {
                tabIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TabIndex"));
            }
        }
        public List<Currency> Currencies
        {
            get { return currencies; }
            set { currencies = value; }
        }

        public SalesUser SaleUserData
        {
            get { return saleUserData; }
            set { saleUserData = value; }
        }

        public SalesUser SalesQuota
        {
            get { return salesQuota; }
            set { salesQuota = value; OnPropertyChanged(new PropertyChangedEventArgs("SalesQuota")); }
        }

        public ObservableCollection<SalesUserQuota> SalesUserQuotas
        {
            get
            {
                return salesUserQuotas;
            }
            set
            {
                salesUserQuotas = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SalesUserQuotas"));
            }
        }

        public string Office
        {
            get { return office; }
            set
            {
                office = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Office"));
            }
        }

        public List<User> UserList
        {
            get { return userList; }
            set { userList = value; OnPropertyChanged(new PropertyChangedEventArgs("UserList")); }
        }

        public User Users
        {
            get { return user; }
            set { user = value; OnPropertyChanged(new PropertyChangedEventArgs("Users")); }
        }

        public Object SelectedPermissionList
        {
            get { return selectedPermissionList; }
            set
            {
                selectedPermissionList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPermissionList"));
            }
        }

        public List<object> SelectedAuthorizedPlant
        {
            get { return selectedAuthorizedPlant; }
            set { selectedAuthorizedPlant = value; OnPropertyChanged(new PropertyChangedEventArgs("SelectedAuthorizedPlant")); }
        }

        public List<object> SelectedPermission
        {
            get
            {
                return selectedPermission;
            }

            set
            {
                selectedPermission = value;

                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPermission"));
            }
        }


        public Company SelectedPlant
        {
            get { return selectedPlant; }
            set { selectedPlant = value; OnPropertyChanged(new PropertyChangedEventArgs("SelectedPlant")); }
        }

        public List<LookupValue> SalesUnitList
        {
            get { return salesUnitList; }
            set { salesUnitList = value; OnPropertyChanged(new PropertyChangedEventArgs("ListPermissions")); }
        }

        public List<Permission> ListPermissions
        {
            get { return listPermissions; }
            set { listPermissions = value; OnPropertyChanged(new PropertyChangedEventArgs("ListPermissions")); }
        }

        public List<EmdepSite> ListAuthorizedPlants
        {
            get { return listAuthorizedPlants; }
            set { listAuthorizedPlants = value; OnPropertyChanged(new PropertyChangedEventArgs("ListAuthorizedPlants")); }
        }

        public User SelectedUser
        {
            get { return selectedUser; }
            set
            {
                selectedUser = value;
                FirstName = selectedUser.FirstName;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedUser"));
            }
        }

        public List<EmdepSite> ListPlants
        {
            get { return listPlants; }
            set { listPlants = value; OnPropertyChanged(new PropertyChangedEventArgs("ListPlants")); }
        }

        public List<int> AuthorizedPlants
        {
            get { return authorizedPlant; }
            set { authorizedPlant = value; OnPropertyChanged(new PropertyChangedEventArgs("AuthorizedPlants")); }
        }

        public int IdUser
        {
            get { return idUser; }
            set { idUser = value; OnPropertyChanged(new PropertyChangedEventArgs("IdUser")); }
        }

        public string FirstName
        {
            get { return firstName; }
            set
            {
                firstName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FirstName"));
            }
        }

        public string LastName
        {
            get { return lastName; }
            set
            {
                lastName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LastName"));
            }
        }

        public SalesUser SalesUser
        {
            get { return salesUser; }
            set
            {
                // contact = value;
                SetProperty(ref salesUser, value, () => SalesUser);
            }
        }

        public People ContactData
        {
            get { return contactData; }
            set { contactData = value; }
        }

        public bool IsSave
        {
            get { return isSave; }
            set { isSave = value; }
        }
        public bool IsSaveUser
        {
            get { return isSaveUser; }
            set { isSaveUser = value; OnPropertyChanged(new PropertyChangedEventArgs("IsSaveUser")); }
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

        public int SelectedIndexAuthorizedPlant
        {
            get { return selectedIndexAuthorizedPlant; }
            set { selectedIndexAuthorizedPlant = value; OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexAuthorizedPlant")); }
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

        public int SelectedIndexPermission
        {
            get { return selectedIndexPermission; }
            set
            {
                selectedIndexPermission = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexPermission"));
            }
        }

        public int SelectedSalesUnit
        {
            get { return selectedSalesUnit; }
            set { selectedSalesUnit = value; OnPropertyChanged(new PropertyChangedEventArgs("SelectedSalesUnit")); }
        }

        public int SelectedIndexSalesUnit
        {
            get { return selectedIndexSalesUnit; }
            set
            {
                selectedIndexSalesUnit = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexSalesUnit"));
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
        public List<Company> ListPlant
        {
            get { return listPlant; }
            set
            {
                listPlant = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListPlant"));
            }
        }

        public List<int> ListPermission
        {
            get { return listpermission; }
            set
            {
                listpermission = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListPermission"));
            }
        }

        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Email"));
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

        #region Constructor
        public AddUserViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor AddUserViewModel...", category: Category.Info, priority: Priority.Low);

                SalesUser = new SalesUser();
                //Contact.Company = new Company();
                SearchUserButtonCommand = new DelegateCommand<object>(SearchUserViewWindowShow);
                AddUserAcceptButtonCommand = new RelayCommand(new Action<object>(AddUserAcceptAction));
                AddUserCancelButtonCommand = new DelegateCommand<object>(CloseWindow);
                CurrencyRateDateEditValueChangingCommand = new DelegateCommand<EditValueChangingEventArgs>(CurrencyRateDateEditValueChanging);
                SalesQuotaAmountLostFocusCommand = new DelegateCommand<object>(SalesQuotaAmountLostFocusAction);
                SalesQuotaCurrencyChangedCommand = new DelegateCommand<EditValueChangingEventArgs>(SalesQuotaCurrencyChangeAction);
                SalesQuotaAmountChangedCommand = new DelegateCommand<EditValueChangedEventArgs>(SalesQuotaAmountChangeAction);
                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);

                GeosApplication.Instance.IdCurrencyByRegion = GeosApplication.Instance.Currencies.Where(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency"].ToString()).Select(cur => cur.IdCurrency).SingleOrDefault();
                GeosApplication.Instance.CurrentCurrencySymbol = GeosApplication.Instance.Currencies.Where(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency"].ToString()).Select(cur => cur.Symbol).SingleOrDefault();

                HyperlinkForEmail = new DelegateCommand<object>(new Action<object>((obj) =>
                {
                    SendMailtoPerson(obj);
                }));

                if (GeosApplication.Instance.IdUserPermission == 21)
                {
                    List<UserManagerDtl> salesOwners = GeosApplication.Instance.SelectedSalesOwnerUsersList.Cast<UserManagerDtl>().ToList();
                    salesOwnersIds = string.Join(",", salesOwners.Select(i => i.IdUser));
                }

                if (GeosApplication.Instance.ObjectPool.ContainsKey("CRM_USERGENDER"))
                {
                    UserGenderList = (ObservableCollection<Data.Common.Epc.LookupValue>)GeosApplication.Instance.ObjectPool["CRM_USERGENDER"];
                    SelectedIndexGender = -1;
                }
                else
                {
                    UserGenderList = new ObservableCollection<Data.Common.Epc.LookupValue>(CrmStartUp.GetLookupValues(1).AsEnumerable());
                    GeosApplication.Instance.ObjectPool.Add("CRM_USERGENDER", UserGenderList);
                    SelectedIndexGender = -1;
                }
                FillCurrencyDetails();
                FillPlants();
                FillPermissions();
                FillSalesUnit();
                FillAuthorizedPlants();
                FillSalesQuota();

                Currency curr = new Currency();
                curr = Currencies.FirstOrDefault(x => x.IdCurrency == GeosApplication.Instance.IdCurrencyByRegion);
                ToCurrency = curr.Name;
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
                GeosApplication.Instance.Logger.Log("Constructor AddUserViewModel() executed successfully Constructor...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddUserViewModel() Constructor...", category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

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

        #endregion

        #region Methods
        /// <summary>
        ///Get Currency Value based on Currency Rate Date
        ///Task  [CRM-M039-02] Specify the date of the exchange rates in targets
        ///Amit
        /// </summary>
        public void CurrencyRateDateEditValueChanging(EditValueChangingEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CurrencyRateDateEditValueChanging ...", category: Category.Info, priority: Priority.Low);
                // UserQuotasList
                UserQuotasList[TabIndex].ExchangeRateDate = Convert.ToDateTime(e.NewValue);
                CalculateCurrencyExchangeRate(UserQuotasList[TabIndex].SalesQuotaAmount);

                GeosApplication.Instance.Logger.Log("Method CurrencyRateDateEditValueChanging() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CurrencyRateDateEditValueChanging() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method do for Calculate Currency Exchange Rate
        /// </summary>
        public void CalculateCurrencyExchangeRate(double amount)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CalculateCurrencyExchangeRate ...", category: Category.Info, priority: Priority.Low);

                DailyCurrencyConversion dailyCurrencyConversion = new DailyCurrencyConversion();
                dailyCurrencyConversion.CurrencyConversionDate = (DateTime)UserQuotasList[TabIndex].ExchangeRateDate;
                dailyCurrencyConversion.IdCurrencyConversionFrom = UserQuotasList[TabIndex].IdSalesQuotaCurrency;
                dailyCurrencyConversion.IdCurrencyConversionTo = GeosApplication.Instance.IdCurrencyByRegion;
                dailyCurrencyConversion = CrmStartUp.GetCurrencyRateByDateAndId(dailyCurrencyConversion);
                CurrencyConversionRate = dailyCurrencyConversion.CurrencyConversationRate;
                CurrencyConversionRate = Math.Round(CurrencyConversionRate, 4) * Math.Round(amount, 4);
                CurrencyConversionRate = Math.Round(CurrencyConversionRate, 4);

                Currency cur = new Currency();
                cur = Currencies.FirstOrDefault(x => x.IdCurrency == GeosApplication.Instance.IdCurrencyByRegion);
                ToCurrency = cur.Name;
                UserQuotasList[tabIndex].SalesQuotaAmountWithExchangeRate = CurrencyConversionRate;

                GeosApplication.Instance.Logger.Log("Method CalculateCurrencyExchangeRate() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CalculateCurrencyExchangeRate() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method do for Change Sales Quota Amount Changed
        /// </summary>
        public void SalesQuotaAmountChangeAction(EditValueChangedEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SalesQuotaAmountChangeAction ...", category: Category.Info, priority: Priority.Low);

                UserQuotasList[TabIndex].SalesQuotaAmount = Convert.ToDouble(e.NewValue);

                GeosApplication.Instance.Logger.Log("Method SalesQuotaAmountChangeAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SalesQuotaAmountChangeAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method do for Change Sales Quota Currency Change Action
        /// </summary>
        public void SalesQuotaCurrencyChangeAction(EditValueChangingEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SalesQuotaAmountChangeAction ...", category: Category.Info, priority: Priority.Low);

                Currency curr = (Currency)e.NewValue;
                UserQuotasList[TabIndex].IdSalesQuotaCurrency = curr.IdCurrency;
                CalculateCurrencyExchangeRate(UserQuotasList[TabIndex].SalesQuotaAmount);

                GeosApplication.Instance.Logger.Log("Method SalesQuotaAmountChangeAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SalesQuotaAmountChangeAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method do for Change Sales Quota Amount Lost Focus
        /// </summary>
        public void SalesQuotaAmountLostFocusAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SalesQuotaAmountLostFocusAction ...", category: Category.Info, priority: Priority.Low);

                CalculateCurrencyExchangeRate(UserQuotasList[TabIndex].SalesQuotaAmount);

                GeosApplication.Instance.Logger.Log("Method SalesQuotaAmountLostFocusAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SalesQuotaAmountLostFocusAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        /// Methdo for fill currency details.
        /// </summary>
        public void FillCurrencyDetails()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCurrencyDetails()...", category: Category.Info, priority: Priority.Low);
                
                Currencies = GeosApplication.Instance.Currencies.ToList();
                SelectedIndexCurrencynew = Currencies.FindIndex(i => i.IdCurrency == GeosApplication.Instance.IdCurrencyByRegion);
                
                GeosApplication.Instance.Logger.Log("Method FillCurrencyDetails() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillCurrencyDetails() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Methdo for fill authorized plants.
        /// </summary>
        private void FillAuthorizedPlants()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillAuthorizedPlants()...", category: Category.Info, priority: Priority.Low);

                ListAuthorizedPlants = CrmStartUp.GetAllEmdepSites(GeosApplication.Instance.ActiveUser.IdUser);
                ListAuthorizedPlants = ListAuthorizedPlants.GroupBy(x => x.DatabaseIP).Select(y => y.First()).ToList();

                GeosApplication.Instance.Logger.Log("Method FillAuthorizedPlants executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillAuthorizedPlants() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillAuthorizedPlants() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillAuthorizedPlants()." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Methdo for fill plants list.
        /// </summary>
        private void FillPlants()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillPlants()...", category: Category.Info, priority: Priority.Low);

                ListPlants = CrmStartUp.GetAllEmdepSites(GeosApplication.Instance.ActiveUser.IdUser);
                ListPlants = ListPlants.GroupBy(x => x.DatabaseIP).Select(y => y.First()).ToList();
                SelectedIndexCompanyPlant = -1;

                GeosApplication.Instance.Logger.Log("Method FillPlants executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillPlants() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillPlants() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillPlants()." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for fill sales quota.
        /// </summary>
        private void FillSalesQuota()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillSalesQuota()...", category: Category.Info, priority: Priority.Low);

                UserQuotasList = new ObservableCollection<UserQuotas>();

                UserQuotas sale = new UserQuotas();
                sale.Year = Convert.ToInt32(GeosApplication.Instance.CrmOfferYear);
                sale.ExchangeRateDate = new DateTime(sale.Year, 1, 1);
                sale.IdSalesQuotaCurrency = GeosApplication.Instance.IdCurrencyByRegion;
                UserQuotasList.Add(sale);
                
                int Year = UserQuotasList[0].Year;

                if (DateTime.Now.Year == Year)
                {
                    CurrencyRateMinDate = new DateTime(Year, 1, 1);
                    CurrencyRateMaxDate = DateTime.Now;
                }
                else
                {
                    CurrencyRateMinDate = new DateTime(Year, 1, 1);
                    CurrencyRateMaxDate = new DateTime(Year, 12, 31);
                }              

                GeosApplication.Instance.Logger.Log("Method FillSalesQuota() executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillSalesQuota() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillSalesQuota() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillPlants()." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for fill sales unit.
        /// </summary>
        private void FillSalesUnit()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillSalesUnit()...", category: Category.Info, priority: Priority.Low);

                List<LookupValue> tempSalesUnitList = CrmStartUp.GetLookupValues(8).ToList();
                SalesUnitList = new List<LookupValue>();
                tempSalesUnitList = new List<LookupValue>(tempSalesUnitList.Where(inUseOption => inUseOption.InUse == true));
                SalesUnitList.Insert(0, new LookupValue() { Value = "---", InUse = true });
                SalesUnitList.AddRange(tempSalesUnitList);

                //SalesUnitList = new List<LookupValue>(CrmStartUp.GetLookupValues(8).AsEnumerable());

                GeosApplication.Instance.Logger.Log("Method FillSalesUnit() exceuted successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillSalesUnit() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillSalesUnit() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillSalesUnit()." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for permissions list.
        /// </summary>
        private void FillPermissions()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillPermissions()...", category: Category.Info, priority: Priority.Low);

                //ListPermissions = WorkbenchStartUp.GetAllPermissions(GeosApplication.Instance.ActiveUser.IdUser);
                //5 - CRM Module.
                ListPermissions = WorkbenchStartUp.GetUserPermissionsByGeosModule(GeosApplication.Instance.ActiveUser.IdUser, 5);

                GeosApplication.Instance.Logger.Log("Method FillPermissions() executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillPermissions() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillPermissions() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillPermissions()." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for search user.
        /// </summary>
        /// <param name="obj"></param>
        private void SearchUserViewWindowShow(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SearchUserViewWindowShow...", category: Category.Info, priority: Priority.Low);
                DXSplashScreen.Show<SplashScreenView>();
                SearchUserViewModel searchUserViewModel = new SearchUserViewModel();
                SearchUserView searchUserView = new SearchUserView();
                EventHandler handle = delegate { searchUserView.Close(); };
                searchUserViewModel.RequestClose += handle;
                searchUserView.DataContext = searchUserViewModel;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                searchUserView.ShowDialogWindow();

                if (searchUserViewModel.IsSave)
                {
                    IdUser = searchUserViewModel.AllUsers;
                    UserDetail = new People();
                    UserDetail = CrmStartUp.GetPeopleDetailByIdPerson(IdUser);
                    User tempSelectedUser = searchUserViewModel.ListUser.Where(u => u.IdUser == IdUser).FirstOrDefault();

                    // fill detail of user from people table.
                    if (UserDetail != null)
                    {
                        tempSelectedUser.FirstName = UserDetail.Name;
                        tempSelectedUser.LastName = UserDetail.Surname;
                        tempSelectedUser.IdUserGender = UserDetail.IdPersonGender;

                        if (UserDetail.Company != null)
                        {
                            tempSelectedUser.Company = new Company();
                            tempSelectedUser.Company.ShortName = UserDetail.Company.ShortName;
                        }

                        tempSelectedUser.CompanyEmail = UserDetail.Email;
                    }

                    SelectedUser = tempSelectedUser;

                    try
                    {
                        if (SelectedUser != null)
                        {
                            UserProfileImageByte = GeosRepositoryServiceController.GetUserProfileImageWithoutException(SelectedUser.Login);
                            if (UserProfileImageByte != null)
                                SelectedUser.OwnerImage = ByteArrayToBitmapImage(UserProfileImageByte);

                            if (UserProfileImageByte == null)
                            {
                                if (ThemeManager.ApplicationThemeName == "BlackAndBlue")
                                {
                                    if (SelectedUser.IdUserGender != null && SelectedUser.IdUserGender == 1)
                                        SelectedUser.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_White.png");
                                    else
                                        if (SelectedUser.IdUserGender != null && SelectedUser.IdUserGender == 2)
                                        SelectedUser.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_White.png");
                                }
                                else
                                {
                                    if (SelectedUser.IdUserGender != null && SelectedUser.IdUserGender == 1)
                                        SelectedUser.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_Blue.png");
                                    else
                                        if (SelectedUser.IdUserGender != null && SelectedUser.IdUserGender == 2)
                                        SelectedUser.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_Blue.png");
                                }
                            }
                        }
                    }
                    catch (FaultException<ServiceException> ex)
                    {
                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Get an error in SearchUserViewWindowShow() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                    catch (ServiceUnexceptedException ex)
                    {
                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Get an error in SearchUserViewWindowShow() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
                    }
                    catch (Exception ex)
                    {
                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Get an error in SearchUserViewWindowShow()." + ex.Message, category: Category.Exception, priority: Priority.Low);
                    }

                    // to show gender
                    if (SelectedUser.IdUserGender == 1)
                    {
                        SelectedIndexGender = 0;
                    }
                    else if (SelectedUser.IdUserGender == 2)
                    {
                        SelectedIndexGender = 1;
                    }
                    else
                    {
                        SelectedIndexGender = -1;
                    }

                    // to show selected permission
                    //ListPermissions = WorkbenchStartUp.GetAllPermissions(IdUser);
                    // 5 - CRM module
                    ListPermissions = WorkbenchStartUp.GetUserPermissionsByGeosModule(GeosApplication.Instance.ActiveUser.IdUser, 5);

                    List<object> tempSelectedPermission = new List<object>();
                    foreach (Permission item in ListPermissions)
                    {
                        if (item.IsUserPermission)
                            tempSelectedPermission.Add(item);
                    }
                    SelectedPermission = new List<object>();

                    ListAuthorizedPlants = CrmStartUp.GetAllEmdepSites(IdUser);
                    ListAuthorizedPlants = ListAuthorizedPlants.GroupBy(x => x.DatabaseIP).Select(y => y.First()).ToList();

                    SelectedAuthorizedPlant = new List<object>();

                    // to show selected plants
                    ListPlants = CrmStartUp.GetAllEmdepSites(IdUser);
                    ListPlants = ListPlants.GroupBy(x => x.DatabaseIP).Select(y => y.First()).ToList();

                    // to show selected sales unit
                    SalesQuota = CrmStartUp.GetUserAllSalesQuotaByDate(IdUser, GeosApplication.Instance.IdCurrencyByRegion);
                    SelectedIndexSalesUnit = SalesUnitList.FindIndex(y => y.IdLookupValue == SalesQuota.IdSalesTeam);

                    // to show selected sales Quota
                    if (SalesQuota.SalesUserQuotas.Count > 0)
                    {
                        SalesQuota = CrmStartUp.GetUserAllSalesQuotaByDate(IdUser, GeosApplication.Instance.IdCurrencyByRegion);
                        SalesUserQuotas = new ObservableCollection<SalesUserQuota>();
                        for (int i = 0; i < SalesQuota.SalesUserQuotas.Count; i++)
                        {
                            SalesUserQuotas.Add(SalesQuota.SalesUserQuotas[i]);
                        }
                    }

                    else
                    {

                        SalesUserQuotas = new ObservableCollection<SalesUserQuota>();
                        SalesUserQuota sale = new SalesUserQuota();
                        sale.Year = Convert.ToInt32(GeosApplication.Instance.CrmOfferYear);
                        SalesUserQuotas.Add(sale);

                    }

                    // to show currnecy in sales Quota
                    //FillCurrencyDetails();
                    SiteUserPermission siteUserPermission = new SiteUserPermission();
                }

                GeosApplication.Instance.Logger.Log("Method SearchUserViewWindowShow() executed successfully...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SearchUserViewWindowShow() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for add new user.
        /// </summary>
        /// <param name="obj"></param>
        public void AddUserAcceptAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method AddUserAcceptAction ...", category: Category.Info, priority: Priority.Low);

            IsBusy = true;
            string error = EnableValidationAndGetError();

            PropertyChanged(this, new PropertyChangedEventArgs("FirstName"));
            PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexSalesUnit"));
            PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexCompanyPlant"));   // Plant
            PropertyChanged(this, new PropertyChangedEventArgs("SelectedPermission"));
            PropertyChanged(this, new PropertyChangedEventArgs("SelectedAuthorizedPlant"));
            PropertyChanged(this, new PropertyChangedEventArgs("SalesQuotaAmount"));

            if (error != null)
            {
                IsBusy = false;
                return;
            }
            SalesUserQuotas = new ObservableCollection<SalesUserQuota>();

            for (int i = 0; i < UserQuotasList.Count; i++)
            {
                SalesUserQuota objSalesUserQuotas = new SalesUserQuota();
                objSalesUserQuotas.IdSalesUser = SelectedUser.IdUser;
                objSalesUserQuotas.IdSalesQuotaCurrency = UserQuotasList[i].IdSalesQuotaCurrency;
                objSalesUserQuotas.ExchangeRateDate = UserQuotasList[i].ExchangeRateDate;
                objSalesUserQuotas.SalesQuotaAmount = UserQuotasList[i].SalesQuotaAmount;
                objSalesUserQuotas.SalesQuotaAmountWithExchangeRate = UserQuotasList[i].SalesQuotaAmountWithExchangeRate;
                objSalesUserQuotas.Year = UserQuotasList[i].Year;
                SalesUserQuotas.Add(objSalesUserQuotas);
            }
            int tIndex = 0;
            foreach (var item in UserQuotasList)
            {
                IsBusy = true;
                if (item.SalesQuotaAmount > 0)
                {
                    string error1 = item.CheckValidation();
                    if (error1 != null)
                    {
                        IsBusy = false;
                        TabIndex = tIndex;
                        return;
                    }
                    tIndex++;
                }
            }

            // user permission

            List<Permission> _SelectedPermissionsList = new List<Permission>();
            _SelectedPermissionsList = SelectedPermission.Cast<Permission>().ToList();

            List<UserPermission> FinalListPermissions = new List<UserPermission>();

            //List<int> permissionIds = new List<int>();
            //permissionIds = ListPermissions.Where(spm => spm.IsUserPermission == true).Select(lp => lp.IdPermission).ToList();

            List<int> _SelectedPermissionsListId = new List<int>();
            _SelectedPermissionsListId = _SelectedPermissionsList.Select(lp => lp.IdPermission).ToList();

            foreach (Permission item in _SelectedPermissionsList)
            {
                //if (ListPermissions.Any(lp => lp.IdPermission == item.IdPermission && lp.IsUserPermission == true))
                //{
                //}
                //else
                //{
                UserPermission userPermission = new UserPermission();
                userPermission.IdUser = IdUser;
                userPermission.IdPermission = item.IdPermission;
                userPermission.IsDeleted = false;

                FinalListPermissions.Add(userPermission);
                //}
            }

            //foreach (var item in permissionIds.Where(sp => !_SelectedPermissionsListId.Contains(sp)))
            //{
            //    UserPermission userPermission = new UserPermission();

            //    userPermission.IdUser = IdUser;
            //    userPermission.IdPermission = item;
            //    userPermission.IsDeleted = true;

            //    FinalListPermissions.Add(userPermission);
            //}

            // user authorized Plant permission...

            List<EmdepSite> _SelectedListAuthorizedPlants = new List<EmdepSite>();
            _SelectedListAuthorizedPlants = SelectedAuthorizedPlant.Cast<EmdepSite>().ToList();

            List<SiteUserPermission> FinalListAuthorizedPlants = new List<SiteUserPermission>();

            foreach (EmdepSite item in _SelectedListAuthorizedPlants)
            {
                if (ListAuthorizedPlants.Any(lp => lp.IdSite == item.IdSite))
                {
                    SiteUserPermission siteUserPermission = new SiteUserPermission();
                    siteUserPermission.IdCompany = Convert.ToInt32(item.IdSite);
                    siteUserPermission.IdUser = IdUser;
                    siteUserPermission.IsDeleted = false;

                    FinalListAuthorizedPlants.Add(siteUserPermission);
                }
                else
                {
                    //SiteUserPermission siteUserPermission = new SiteUserPermission();
                    //siteUserPermission.IdCompany = Convert.ToInt32(item.IdCompany);
                    //siteUserPermission.IdUser = IdUser;
                    //siteUserPermission.IsDeleted = false;

                    ////item.UserPermissions = new List<UserPermission>()
                    //FinalListAuthorizedPlants.Add(siteUserPermission);
                }
            }


            try
            {
                GeosApplication.Instance.Logger.Log("Method AddUserAcceptAction()...", category: Category.Info, priority: Priority.Low);

                if (SalesUser != null)
                {
                    SalesUser.IdSalesPlant = Convert.ToInt32(ListPlants[SelectedIndexCompanyPlant].IdSite);
                    SalesUser.IdSalesTeam = SalesUnitList[SelectedIndexSalesUnit].IdLookupValue;
                    SalesUser.IdSalesUser = SelectedUser.IdUser;
                    IsSaveUser = CrmStartUp.AddSaleUser(SalesUser);
                }

                bool isPermission = false;
                bool isPlantPermission = false;
                bool isSalesQuota = false;

                if (FinalListPermissions != null && FinalListPermissions.Count > 0)
                {
                    isPermission = WorkbenchStartUp.AddUserPermissions(FinalListPermissions);
                }

                if (_SelectedListAuthorizedPlants != null && _SelectedListAuthorizedPlants.Count > 0)
                {
                    isPlantPermission = WorkbenchStartUp.AddUserSitePermissions(FinalListAuthorizedPlants);
                }

                if (SalesUserQuotas != null && SalesUserQuotas.Count > 0)
                {
                    SalesUserQuotas[0].IdSalesUser = SelectedUser.IdUser;
                    isSalesQuota = CrmStartUp.AddSaleUserQuotaWithDate(SalesUserQuotas.ToList());
                }

                if (isPermission == false || isPlantPermission == false || isSalesQuota == false || IsSaveUser == false)
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("UserSaveFailMessage").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                else
                {

                    user = WorkbenchStartUp.GetUserById(SelectedUser.IdUser);
                    SaleUserData = new SalesUser();

                    SaleUserData.IdSalesUser = SelectedUser.IdUser;

                    SaleUserData.People = new People();//UserDetail
                    SaleUserData.People = UserDetail;

                    if (SelectedUser.IdUserGender != null)
                        SaleUserData.People.UserGender = UserGenderList.FirstOrDefault(usr => usr.IdLookupValue == SelectedUser.IdUserGender).Value;


                    if (SelectedIndexCompanyPlant > -1)
                    {
                        SaleUserData.Company = new Company();
                        SaleUserData.Company.ShortName = ListPlants[SelectedIndexCompanyPlant].ShortName;
                    }

                    SaleUserData.SalesQuotaAmount = SalesUserQuotas[0].SalesQuotaAmount;

                    if (SelectedIndexSalesUnit > -1)
                        SaleUserData.LookupValue = SalesUnitList[SelectedIndexSalesUnit];

                    SaleUserData.SalesUserQuotas = SalesUserQuotas.ToList();

                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("UserSaveSuccessMessage").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                }

                GeosApplication.Instance.Logger.Log("Method AddUserAcceptAction() executed Successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddUserAcceptAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddUserAcceptAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddUserAcceptAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            IsBusy = true;
            RequestClose(null, null);
        }

        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
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
        /// This method is for to convert from Bytearray to ImageSource
        /// </summary>
        /// <param name="byteArrayIn"></param>
        /// <returns></returns>
        public ImageSource ByteArrayToBitmapImage(byte[] byteArrayIn)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method convert byteArrayToImage ...", category: Category.Info, priority: Priority.Low);

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
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in byteArrayToImage() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            return null;
        }

        /// <summary>
        /// Method for send mail.
        /// </summary>
        /// <param name="obj"></param>
        public void SendMailtoPerson(object obj)
        {
            try
            {
                if (string.IsNullOrEmpty(Convert.ToString(obj)))
                {
                    return;
                }

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
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SendMailtoPerson() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            IsBusy = false;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
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
        #endregion

        #region validation

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
                     me[BindableBase.GetPropertyName(() => FirstName)] + // User name
                    me[BindableBase.GetPropertyName(() => SelectedIndexSalesUnit)] +     // Sales Unit
                    me[BindableBase.GetPropertyName(() => SelectedIndexCompanyPlant)] +      // Plant
                    me[BindableBase.GetPropertyName(() => SelectedPermission)] +         // Permission
                    me[BindableBase.GetPropertyName(() => SelectedAuthorizedPlant)] +      // Authorized Plant
                    me[BindableBase.GetPropertyName(() => CurrencyConversionRate)];      // Currency Conversion Exchange Rate

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
                string selectedUserFirstNameProp = BindableBase.GetPropertyName(() => FirstName); // selected User
                string selectedIndexSalesUnitProp = BindableBase.GetPropertyName(() => SelectedIndexSalesUnit);   // selectedIndexSalesUnit
                string selectedIndexCompanyPlantProp = BindableBase.GetPropertyName(() => SelectedIndexCompanyPlant);
                string SelectedIndexPermissionProp = BindableBase.GetPropertyName(() => SelectedPermission);
                string SelectedAuthorizedPlantProp = BindableBase.GetPropertyName(() => SelectedAuthorizedPlant);
                string currencyConversionRateProp = BindableBase.GetPropertyName(() => CurrencyConversionRate);

                if (columnName == selectedUserFirstNameProp)
                    return UserValidation.GetErrorMessage(selectedUserFirstNameProp, FirstName);

                if (columnName == selectedIndexSalesUnitProp)
                    return UserValidation.GetErrorMessage(selectedIndexSalesUnitProp, SelectedIndexSalesUnit);
                else if (columnName == selectedIndexCompanyPlantProp)
                    return UserValidation.GetErrorMessage(selectedIndexCompanyPlantProp, SelectedIndexCompanyPlant);
                else if (columnName == SelectedIndexPermissionProp)
                    return UserValidation.GetErrorMessage(SelectedIndexPermissionProp, SelectedPermission);
                else if (columnName == SelectedAuthorizedPlantProp)
                    return UserValidation.GetErrorMessage(SelectedAuthorizedPlantProp, SelectedAuthorizedPlant);
                else if (columnName == currencyConversionRateProp)
                    return UserValidation.GetErrorMessage(currencyConversionRateProp, CurrencyConversionRate);
                return null;
            }
        }

        #endregion

        public class UserQuotas : INotifyPropertyChanged, IDataErrorInfo
        {

            public event PropertyChangedEventHandler PropertyChanged;
            public void OnPropertyChanged(PropertyChangedEventArgs e)
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, e);
                }
            }

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
            string IDataErrorInfo.this[string columnName]
            {
                get
                {
                    if (!allowValidation) return null;


                    string salesQuotaAmountWithExchangeRateProp = BindableBase.GetPropertyName(() => SalesQuotaAmountWithExchangeRate);

                    if (columnName == salesQuotaAmountWithExchangeRateProp)
                        return UserValidation.GetErrorMessage(salesQuotaAmountWithExchangeRateProp, SalesQuotaAmountWithExchangeRate);


                    return null;
                }
            }

            string IDataErrorInfo.Error
            {
                get
                {
                    //if (!allowValidation) return null;
                    IDataErrorInfo me = (IDataErrorInfo)this;
                    string error =
                    me[BindableBase.GetPropertyName(() => SalesQuotaAmountWithExchangeRate)];

                    if (!string.IsNullOrEmpty(error))
                        return "Please check inputted data.";

                    return null;
                }
            }


            #endregion

            #region Fields

            Int32 year;
            double salesQuotaAmount;
            byte idSalesQuotaCurrency;
            object tag;
            DateTime? exchangeRateDate;
            double salesQuotaAmountWithExchangeRate;
            #endregion

            #region Constructor
            public UserQuotas()
            {

            }
            #endregion

            #region Properties
            
            public Int32 Year
            {
                get
                {
                    return year;
                }

                set
                {
                    year = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Year"));

                }
            }


            public DateTime? ExchangeRateDate
            {
                get
                {
                    return exchangeRateDate;
                }

                set
                {
                    exchangeRateDate = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("ExchangeRateDate"));
                }
            }


            public double SalesQuotaAmount
            {
                get
                {
                    return salesQuotaAmount;
                }

                set
                {
                    salesQuotaAmount = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("SalesQuotaAmount"));

                }
            }


            public byte IdSalesQuotaCurrency
            {
                get
                {
                    return idSalesQuotaCurrency;
                }

                set
                {
                    idSalesQuotaCurrency = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IdSalesQuotaCurrency"));

                }
            }



            public object Tag
            {
                get
                {
                    return tag;
                }

                set
                {
                    tag = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Tag"));

                }
            }
            public double SalesQuotaAmountWithExchangeRate
            {
                get
                {
                    return salesQuotaAmountWithExchangeRate;
                }

                set
                {
                    salesQuotaAmountWithExchangeRate = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("SalesQuotaAmountWithExchangeRate"));

                }
            }
            #endregion

            #region Methods
            public string CheckValidation()
            {
                string error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("SalesQuotaAmountWithExchangeRate"));

                return error;
            }
            #endregion
        }
    }
}
