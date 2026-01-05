using DevExpress.Mvvm;
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
    public class EditUserViewModel : NavigationViewModelBase, IDataErrorInfo, INotifyPropertyChanged, IDisposable
    {
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

        #region Services

        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion // Services

        #region Commands
        public ICommand AddUserCancelButtonCommand { get; set; }
        public ICommand AddUserAcceptButtonCommand { get; set; }
        public ICommand SalesQuotaAmountChangedCommand { get; set; }
        public ICommand SalesQuotaAmountLostFocusCommand { get; set; }
        public ICommand SearchUserButtonCommand { get; set; }
        public ICommand HyperlinkForEmail { get; set; }
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
        private List<EmdepSite> listPlant;
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
        private IList<Currency> currencies;
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
        private bool isUpdateUser;
        private int selectedIndexAuthorizedPlant;
        private int selectedSalesUnit;
        private DateTime? currencyRateDate;
        private DateTime? currencyRateMinDate;
        private DateTime? currencyRateMaxDate;
        private double currencyConversionRate;
        private double salesQuotaAmount;
        private string toCurrency;
        private int tabIndex;
        private byte IdCurrencyFrom;
        private int selectedIndexCurrencynew;
        private Currency selectedCurrency;
        private int salesMaxDiscount;
        #endregion // Declaration

        #region Properties
        public int SalesMaxDiscount
        {
            get { return salesMaxDiscount; }
            set
            {
                salesMaxDiscount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SalesMaxDiscount"));
            }
        }
        public SalesUser Sale_user { get; set; }

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
        public Currency SelectedCurrency
        {
            get
            {
                return selectedCurrency;
            }

            set
            {
                selectedCurrency = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UserQuotasList"));
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
                int Year = UserQuotasList[TabIndex].Year;
                SelectedCurrency = Currencies.FirstOrDefault(x => x.IdCurrency == UserQuotasList[TabIndex].IdSalesQuotaCurrency);
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
                OnPropertyChanged(new PropertyChangedEventArgs("TabIndex"));
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

        public IList<Currency> Currencies
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
            set { selectedUser = value; OnPropertyChanged(new PropertyChangedEventArgs("SelectedUser")); }
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
        public bool IsUpdateUser
        {
            get { return isUpdateUser; }
            set { isUpdateUser = value; OnPropertyChanged(new PropertyChangedEventArgs("IsUpdateUser")); }
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
        public List<EmdepSite> ListPlant
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

        #endregion // Properties

        #region Constructor
        public EditUserViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor EditUserViewModel...", category: Category.Info, priority: Priority.Low);

                SalesUser = new SalesUser();
                CurrencyRateDateEditValueChangingCommand = new DelegateCommand<EditValueChangingEventArgs>(CurrencyRateDateEditValueChanging);
                AddUserAcceptButtonCommand = new RelayCommand(new Action<object>(EditUserAcceptAction));
                AddUserCancelButtonCommand = new DelegateCommand<object>(CloseWindow);
                SalesQuotaAmountLostFocusCommand = new DelegateCommand<object>(SalesQuotaAmountLostFocusAction);
                SalesQuotaAmountChangedCommand = new DelegateCommand<EditValueChangedEventArgs>(SalesQuotaAmountChangedAction);
                SalesQuotaCurrencyChangedCommand = new DelegateCommand<EditValueChangingEventArgs>(SalesQuotaCurrencyChangeAction);
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
                FillPlants();
                FillPermissions();
                FillSalesUnit();
                FillAuthorizedPlants();
                FillSalesQuota();
                SalesMaxDiscount = 0;

                GeosApplication.Instance.Logger.Log("Constructor EditUserViewModel() executed successfully Constructor...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EditUserViewModel() Constructor...", category: Category.Exception, priority: Priority.Low);
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
        public void SalesQuotaAmountChangedAction(EditValueChangedEventArgs e)
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
        /// Method do for Change Sales Quota Currency Change Action
        /// </summary>
        public void SalesQuotaCurrencyChangeAction(EditValueChangingEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SalesQuotaCurrencyChangeAction ...", category: Category.Info, priority: Priority.Low);

                Currency curr = (Currency)e.NewValue;
                UserQuotasList[TabIndex].IdSalesQuotaCurrency = curr.IdCurrency;
                CalculateCurrencyExchangeRate(UserQuotasList[TabIndex].SalesQuotaAmount);

                GeosApplication.Instance.Logger.Log("Method SalesQuotaCurrencyChangeAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SalesQuotaCurrencyChangeAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method do for fill currency details
        /// </summary>
        public void FillCurrencyDetails()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCurrencyDetails ...", category: Category.Info, priority: Priority.Low);

                Currencies = GeosApplication.Instance.Currencies;

                GeosApplication.Instance.Logger.Log("Method FillCurrencyDetails() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCurrencyDetails() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for fill Authorize plants.
        /// </summary>
        private void FillAuthorizedPlants()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("FillAuthorizedPlants...", category: Category.Info, priority: Priority.Low);

                ListAuthorizedPlants = CrmStartUp.GetAllEmdepSites(GeosApplication.Instance.ActiveUser.IdUser);
                ListAuthorizedPlants = ListAuthorizedPlants.GroupBy(x => x.DatabaseIP).Select(y => y.First()).ToList();

                GeosApplication.Instance.Logger.Log("FillAuthorizedPlants()...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("FillAuthorizedPlants()...", category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for fill plant list
        /// </summary>
        private void FillPlants()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("FillPlants...", category: Category.Info, priority: Priority.Low);

                ListPlants = CrmStartUp.GetAllEmdepSites(GeosApplication.Instance.ActiveUser.IdUser);

                ListPlants = ListPlants.GroupBy(x => x.DatabaseIP).Select(y => y.First()).ToList();

                SelectedIndexCompanyPlant = -1;

                GeosApplication.Instance.Logger.Log("FillPlants()...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("FillPlants()...", category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for fill sales quota.
        /// </summary>
        private void FillSalesQuota()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("FillSalesQuota...", category: Category.Info, priority: Priority.Low);

                SalesQuota = CrmStartUp.GetUserAllSalesQuotaByDate(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.IdCurrencyByRegion);
                SalesQuota.SalesUserQuotas = SalesQuota.SalesUserQuotas.OrderBy(sl => sl.Year).ToList();
                SalesUserQuotas = new ObservableCollection<SalesUserQuota>();
                if (SalesQuota.SalesUserQuotas.Count > 0 || SalesQuota.SalesUserQuotas == null)
                {
                    SalesQuota.SalesUserQuotas = new List<SalesUserQuota>();
                    SalesUserQuota sale = new SalesUserQuota();
                    sale.Year = Convert.ToInt32(GeosApplication.Instance.CrmOfferYear);
                    SalesUserQuotas.Add(sale);
                }
                GeosApplication.Instance.Logger.Log("FillSalesQuota()...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("FillSalesQuota()...", category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Methdo for fill sales unit.
        /// </summary>
        private void FillSalesUnit()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("FillSalesUnit...", category: Category.Info, priority: Priority.Low);

                List<LookupValue> tempSalesUnitList = CrmStartUp.GetLookupValues(8).ToList();
                SalesUnitList = new List<LookupValue>();
                SalesUnitList.Insert(0, new LookupValue() { Value = "---", InUse = true });
                SalesUnitList.AddRange(tempSalesUnitList);

                //SalesUnitList = new List<LookupValue>(CrmStartUp.GetLookupValues(8).AsEnumerable());


                GeosApplication.Instance.Logger.Log("FillSalesUnit()...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("FillSalesUnit()...", category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for fill permissions list.
        /// </summary>
        private void FillPermissions()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("FillPermissions...", category: Category.Info, priority: Priority.Low);

                //ListPermissions = WorkbenchStartUp.GetAllPermissions(GeosApplication.Instance.ActiveUser.IdUser);
                // 5 - CRM module
                ListPermissions = WorkbenchStartUp.GetUserPermissionsByGeosModule(GeosApplication.Instance.ActiveUser.IdUser, 5);

                GeosApplication.Instance.Logger.Log("FillPermissions()...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("FillPermissions()...", category: Category.Exception, priority: Priority.Low);
            }
        }

        public void InIt(SalesUser obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method InIt...", category: Category.Info, priority: Priority.Low);
                FillCurrencyDetails();

                Currency cur = new Currency();
                cur = Currencies.FirstOrDefault(x => x.IdCurrency == GeosApplication.Instance.IdCurrencyByRegion);
                ToCurrency = cur.Name;

                Sale_user = new SalesUser();
                Sale_user = obj;
                User user = new User();
                user = WorkbenchStartUp.GetUserById(Sale_user.People.IdPerson);

                IdUser = Sale_user.People.IdPerson;

                FirstName = Sale_user.People.Name;
                LastName = Sale_user.People.Surname;
                Email = Sale_user.People.Email;
                Office = Sale_user.People.Company.ShortName;
                salesMaxDiscount = Sale_user.MaxDiscountAllowed;

                try
                {
                    if (Sale_user != null)
                    {
                        UserProfileImageByte = GeosRepositoryServiceController.GetUserProfileImageWithoutException(user.Login);
                        if (UserProfileImageByte != null)
                            Sale_user.People.OwnerImage = ByteArrayToBitmapImage(UserProfileImageByte);

                        if (UserProfileImageByte == null)
                        {
                            if (user != null)
                            {
                                if (ThemeManager.ApplicationThemeName == "BlackAndBlue")
                                {
                                    if (user.IdUserGender == 1)
                                        Sale_user.People.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_White.png");
                                    else
                                        if (user.IdUserGender == 2)
                                        Sale_user.People.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_White.png");
                                }
                                else
                                {
                                    if (user.IdUserGender == 1)
                                        Sale_user.People.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_Blue.png");
                                    else
                                        if (user.IdUserGender == 2)
                                        Sale_user.People.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_Blue.png");
                                }
                            }
                            else
                            {
                                if (ThemeManager.ApplicationThemeName == "BlackAndBlue")
                                {
                                    if (Sale_user.People.IdPersonGender == 1)
                                        Sale_user.People.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_White.png");
                                    else
                                        if (Sale_user.People.IdPersonGender == 2)
                                        Sale_user.People.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_White.png");
                                }
                                else
                                {
                                    if (Sale_user.People.IdPersonGender == 1)
                                        Sale_user.People.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_Blue.png");
                                    else
                                        if (Sale_user.People.IdPersonGender == 2)
                                        Sale_user.People.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_Blue.png");
                                }
                            }
                        }
                    }
                }
                catch (FaultException<ServiceException> ex)
                {
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error in InIt() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                    CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                catch (ServiceUnexceptedException ex)
                {
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error in InIt() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                    GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
                }
                catch (Exception ex)
                {
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error in InIt()." + ex.Message, category: Category.Exception, priority: Priority.Low);
                }

                // to show gender
                if (Sale_user.People.IdPersonGender == 1)
                {
                    SelectedIndexGender = 0;
                }
                else if (Sale_user.People.IdPersonGender == 2)
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
                ListPermissions = WorkbenchStartUp.GetUserPermissionsByGeosModule(IdUser, 5);

                List<object> tempSelectedPermission = new List<object>();
                foreach (Permission item in ListPermissions)
                {
                    if (item.IsUserPermission)
                        tempSelectedPermission.Add(item);
                }
                SelectedPermission = new List<object>();
                SelectedPermission = tempSelectedPermission;

                // to show selected authorized plants
                ListAuthorizedPlants = CrmStartUp.GetAllEmdepSites(IdUser);
                ListAuthorizedPlants = ListAuthorizedPlants.GroupBy(x => x.DatabaseIP).Select(y => y.First()).ToList();

                List<object> tempSelectedAuthorizedPlant = new List<object>();
                foreach (EmdepSite item in ListAuthorizedPlants)
                {
                    if (item.IsSitePermission)
                        tempSelectedAuthorizedPlant.Add(item);
                }
                SelectedAuthorizedPlant = new List<object>();
                SelectedAuthorizedPlant = tempSelectedAuthorizedPlant;

                // to show selected plants
                ListPlants = CrmStartUp.GetAllEmdepSites(IdUser);
                ListPlants = ListPlants.GroupBy(x => x.DatabaseIP).Select(y => y.First()).ToList();
                SelectedIndexCompanyPlant = ListPlants.FindIndex(f => f.ShortName == Sale_user.Company.ShortName);

                // to show selected sales unit
                SalesQuota = CrmStartUp.GetUserAllSalesQuotaByDate(IdUser, GeosApplication.Instance.IdCurrencyByRegion);
                SelectedIndexSalesUnit = SalesUnitList.FindIndex(y => y.IdLookupValue == SalesQuota.IdSalesTeam);
                //SelectedIndexSalesUnit = SalesQuota.IdSalesTeam;

                // to show selected sales Quota
                if (SalesQuota.SalesUserQuotas.Count > 0)
                {
                    SalesQuota = CrmStartUp.GetUserAllSalesQuotaByDate(IdUser, GeosApplication.Instance.IdCurrencyByRegion);

                    SalesQuota.SalesUserQuotas = SalesQuota.SalesUserQuotas.OrderByDescending(sl => sl.Year).ToList();

                    SalesUserQuotas = new ObservableCollection<SalesUserQuota>();
                    int startYear = SalesQuota.SalesUserQuotas[SalesQuota.SalesUserQuotas.Count - 1].Year;
                    int endYear = GeosApplication.Instance.SelectedyearEndDate.Year;

                    for (int j = endYear; j >= startYear; j--)
                    {
                        SalesUserQuota tempSaleQuota = new SalesUserQuota();
                        tempSaleQuota = SalesQuota.SalesUserQuotas.FirstOrDefault(x => x.Year == j);
                        if (tempSaleQuota == null)
                        {
                            SalesUserQuota newSaleQuota = new SalesUserQuota();
                            newSaleQuota.Year = j;
                            newSaleQuota.ExchangeRateDate = new DateTime(newSaleQuota.Year, 1, 1);
                            newSaleQuota.IdSalesQuotaCurrency = GeosApplication.Instance.IdCurrencyByRegion;
                            SalesUserQuotas.Add(newSaleQuota);
                        }
                    }


                    for (int i = 0; i < SalesQuota.SalesUserQuotas.Count; i++)
                    {

                        SalesUserQuotas.Add(SalesQuota.SalesUserQuotas[i]);
                        if (SalesQuota.SalesUserQuotas[i].ExchangeRateDate == null)
                        {
                            SalesQuota.SalesUserQuotas[i].ExchangeRateDate = new DateTime(SalesQuota.SalesUserQuotas[i].Year, 1, 1);
                        }
                    }

                    foreach (var item in SalesQuota.SalesUserQuotas)
                    {
                        SalesUserQuota saleyear = new SalesUserQuota();
                        saleyear.Year = Convert.ToInt32(GeosApplication.Instance.ServerDateTime.Year);
                        if (item.Year == saleyear.Year)
                        {
                            item.Tag = true;

                        }
                        else
                        {
                            item.Tag = false;
                        }
                    }
                }
                else
                {
                    SalesQuota = CrmStartUp.GetUserAllSalesQuotaByDate(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.IdCurrencyByRegion);
                    SalesUserQuotas = new ObservableCollection<SalesUserQuota>();
                    if (SalesQuota.SalesUserQuotas.Count > 0 || SalesQuota.SalesUserQuotas == null)
                    {
                        SalesQuota.SalesUserQuotas = new List<SalesUserQuota>();
                        SalesUserQuota sale = new SalesUserQuota();
                        sale.Year = Convert.ToInt32(GeosApplication.Instance.CrmOfferYear);
                        SalesUserQuotas.Add(sale);
                    }

                }

                UserQuotasList = new ObservableCollection<UserQuotas>();

                for (int i = 0; i < SalesUserQuotas.Count; i++)
                {
                    UserQuotas objUserQuotas = new UserQuotas();
                    objUserQuotas.IdSalesQuotaCurrency = SalesUserQuotas[i].IdSalesQuotaCurrency;
                    objUserQuotas.ExchangeRateDate = SalesUserQuotas[i].ExchangeRateDate;
                    objUserQuotas.SalesQuotaAmount = SalesUserQuotas[i].SalesQuotaAmount;
                    objUserQuotas.SalesQuotaAmountWithExchangeRate = SalesUserQuotas[i].SalesQuotaAmountWithExchangeRate;
                    objUserQuotas.Year = SalesUserQuotas[i].Year;
                    UserQuotasList.Add(objUserQuotas);
                }

                SiteUserPermission siteUserPermission = new SiteUserPermission();

                GeosApplication.Instance.Logger.Log("Method InIt() executed successfully...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in InIt() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void EditUserAcceptAction(object obj)
        {
            IsBusy = true;
            string error = EnableValidationAndGetError();

            PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexSalesUnit"));
            PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexCompanyPlant"));
            PropertyChanged(this, new PropertyChangedEventArgs("SelectedPermission"));
            PropertyChanged(this, new PropertyChangedEventArgs("SelectedAuthorizedPlant"));

            if (error != null)
            {
                IsBusy = false;
                return;
            }

            SalesUserQuotas = new ObservableCollection<SalesUserQuota>();

            for (int i = 0; i < UserQuotasList.Count; i++)
            {
                SalesUserQuota objSalesUserQuotas = new SalesUserQuota();

                objSalesUserQuotas.IdSalesUser = IdUser;
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

            List<int> permissionIds = new List<int>();
            permissionIds = ListPermissions.Where(spm => spm.IsUserPermission == true).Select(lp => lp.IdPermission).ToList();

            List<int> _SelectedPermissionsListId = new List<int>();
            _SelectedPermissionsListId = _SelectedPermissionsList.Select(lp => lp.IdPermission).ToList();

            foreach (Permission item in _SelectedPermissionsList)
            {
                if (ListPermissions.Any(lp => lp.IdPermission == item.IdPermission && lp.IsUserPermission == true))
                {

                }
                else
                {
                    UserPermission userPermission = new UserPermission();
                    userPermission.IdUser = IdUser;
                    userPermission.IdPermission = item.IdPermission;
                    userPermission.IsDeleted = false;

                    FinalListPermissions.Add(userPermission);
                }
            }

            foreach (var item in permissionIds.Where(sp => !_SelectedPermissionsListId.Contains(sp)))
            {
                UserPermission userPermission = new UserPermission();

                userPermission.IdUser = IdUser;
                userPermission.IdPermission = item;
                userPermission.IsDeleted = true;

                FinalListPermissions.Add(userPermission);
            }

            // user authorized Plant permission...
            List<EmdepSite> _SelectedListAuthorizedPlants = new List<EmdepSite>();        // new selected user permission
            _SelectedListAuthorizedPlants = SelectedAuthorizedPlant.Cast<EmdepSite>().ToList();

            List<SiteUserPermission> FinalListAuthorizedPlants = new List<SiteUserPermission>();
            List<long> AuthorizedPlantsIds = new List<long>();                                  // old selected user permission
            AuthorizedPlantsIds = ListAuthorizedPlants.Where(spm => spm.IsSitePermission == true).Select(lp => lp.IdSite).ToList();

            List<long> _SelectedAuthorizedPlantId = new List<long>();
            _SelectedAuthorizedPlantId = _SelectedListAuthorizedPlants.Select(lp => lp.IdSite).ToList();

            foreach (EmdepSite item in _SelectedListAuthorizedPlants)
            {
                if (ListAuthorizedPlants.Any(lp => lp.IdSite == item.IdSite && lp.IsSitePermission == true))
                {
                }
                else
                {
                    SiteUserPermission siteUserPermission = new SiteUserPermission();
                    siteUserPermission.IdCompany = Convert.ToInt32(item.IdSite);
                    siteUserPermission.IdUser = IdUser;
                    siteUserPermission.IsDeleted = false;

                    //item.UserPermissions = new List<UserPermission>()
                    FinalListAuthorizedPlants.Add(siteUserPermission);
                }
            }

            foreach (var item in AuthorizedPlantsIds.Where(sp => !_SelectedAuthorizedPlantId.Contains(sp)))
            {
                SiteUserPermission siteUserPermission = new SiteUserPermission();
                siteUserPermission.IdCompany = Convert.ToInt32(item);
                siteUserPermission.IdUser = IdUser;
                siteUserPermission.IsDeleted = true;

                //item.UserPermissions = new List<UserPermission>()
                FinalListAuthorizedPlants.Add(siteUserPermission);
            }

            try
            {
                GeosApplication.Instance.Logger.Log("Method EditUserAcceptAction()...", category: Category.Info, priority: Priority.Low);

                if (SalesUser != null)              // to update sales user
                {
                    SalesUser.IdSalesPlant = Convert.ToInt32(ListPlants[SelectedIndexCompanyPlant].IdSite);

                    SalesUser.IdSalesTeam = SalesUnitList[SelectedIndexSalesUnit].IdLookupValue;
                    SalesUser.IdSalesUser = IdUser;
                    SalesUser.MaxDiscountAllowed = SalesMaxDiscount;
                    IsUpdateUser = CrmStartUp.UpdateSaleUser_V2110(SalesUser);
                }

                bool isPermission = false;
                bool isPlantPermission = false;
                bool isSalesQuota = false;

                if (IsUpdateUser == true)
                {
                    if (FinalListPermissions != null && FinalListPermissions.Count > 0)
                    {
                        isPermission = WorkbenchStartUp.AddUserPermissions(FinalListPermissions);       //to update user permission
                    }
                    if (FinalListAuthorizedPlants != null && FinalListAuthorizedPlants.Count > 0)
                    {
                        isPlantPermission = WorkbenchStartUp.AddUserSitePermissions(FinalListAuthorizedPlants);    // //to update authortized plants
                    }
                    if (SalesUserQuotas != null && SalesUserQuotas.Count > 0)
                    {
                        SalesUserQuotas[0].IdSalesUser = IdUser;
                        isSalesQuota = CrmStartUp.AddSaleUserQuotaWithDate(SalesUserQuotas.ToList());
                    }
                }

                if (IsUpdateUser == false)
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("UserSaveFailMessage").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                else
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("UserUpdateSuccessMessage").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                }

                GeosApplication.Instance.Logger.Log("Method EditUserAcceptAction() executed Successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EditUserAcceptAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EditUserAcceptAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EditUserAcceptAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
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
                GeosApplication.Instance.Logger.Log("Method ByteArrayToBitmapImage ...", category: Category.Info, priority: Priority.Low);

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
                GeosApplication.Instance.Logger.Log("Get an error in ByteArrayToBitmapImage() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
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
                    me[BindableBase.GetPropertyName(() => SelectedIndexSalesUnit)] +     // Sales Unit
                    me[BindableBase.GetPropertyName(() => SelectedIndexCompanyPlant)] +      // Plant
                    me[BindableBase.GetPropertyName(() => SelectedPermission)] +             // Permission
                    me[BindableBase.GetPropertyName(() => SelectedAuthorizedPlant)];      // Authorized Plant                   

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
                string selectedIndexSalesUnitProp = BindableBase.GetPropertyName(() => SelectedIndexSalesUnit);   // selectedIndexSalesUnit
                string selectedIndexCompanyPlantProp = BindableBase.GetPropertyName(() => SelectedIndexCompanyPlant);
                string SelectedIndexPermissionProp = BindableBase.GetPropertyName(() => SelectedPermission);
                string SelectedAuthorizedPlantProp = BindableBase.GetPropertyName(() => SelectedAuthorizedPlant);

                if (columnName == selectedIndexSalesUnitProp)
                    return UserValidation.GetErrorMessage(selectedIndexSalesUnitProp, SelectedIndexSalesUnit);
                else if (columnName == selectedIndexCompanyPlantProp)
                    return UserValidation.GetErrorMessage(selectedIndexCompanyPlantProp, SelectedIndexCompanyPlant);
                else if (columnName == SelectedIndexPermissionProp)
                    return UserValidation.GetErrorMessage(SelectedIndexPermissionProp, SelectedPermission);
                else if (columnName == SelectedAuthorizedPlantProp)
                    return UserValidation.GetErrorMessage(SelectedAuthorizedPlantProp, SelectedAuthorizedPlant);

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
