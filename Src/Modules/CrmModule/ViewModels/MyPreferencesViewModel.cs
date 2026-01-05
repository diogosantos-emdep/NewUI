using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Validations;
using Emdep.Geos.Utility;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;

namespace Emdep.Geos.Modules.Crm.ViewModels
{
    public class MyPreferencesViewModel : NavigationViewModelBase, IDataErrorInfo, INotifyPropertyChanged, IDisposable
    {
        #region Services
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion // Services

        #region Declaration

        private int selectedIndexTopOffers;
        private int selectedIndexTopCustomer;
        private int selectedIndexRegion;
        private int? autoRefreshOffOption;
        private long selectedPeriod;
        private bool isAutoRefresh;

        private DateTime offerPeriodDate;
        private DateTime maxDate;
        private DateTime minDate;

        List<Currency> currencies;
        private int selectedIndexCurrency = 0;
        private int selectedIndexAutoRefresh = 0;
        private int selectedIndexWorkbenchUserPermission = 0;
        public List<long> FinancialYearLst { get; set; }

        public List<UserPermission> WorkbenchUserPermissions { get; set; }

        private Visibility loadDataVisisbility;
        private Visibility isSectionVisible;
        private List<object> selectedCRMSections;

        private bool isYearChecked = true;
        private bool isCustomChecked;
        private DateTime? fromDate = null;
        private DateTime? toDate = null;
        private int? customPeriodOption;

        TimeSpan timeSpanStart = new TimeSpan(0, 0, 0);
        TimeSpan timeSpanEnd = new TimeSpan(23, 59, 59);

        private ObservableCollection<Company> entireCompanyPlantList;
        private ObservableCollection<Customer> companyGroupList;


        private string opportunity;
        private string contact;
        private string account;
        private string appointment;
        private string call;
        private string task;
        private string email;
        private string action;
        private string searchOpportunityOrOrder;
        private string matrixList;


        #endregion // Declaration

        #region public Properties
        public static bool IsPreferenceChanged { get; set; }
        
        public ObservableCollection<Company> EntireCompanyPlantList
        {
            get { return entireCompanyPlantList; }
            set
            {
                entireCompanyPlantList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EntireCompanyPlantList"));
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
        public bool IsYearChecked
        {
            get { return isYearChecked; }
            set
            {
                isYearChecked = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsYearChecked"));
            }
        }
        public bool IsCustomChecked
        {
            get { return isCustomChecked; }
            set
            {
                isCustomChecked = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCustomChecked"));
            }
        }
        public DateTime? FromDate
        {
            get { return fromDate; }
            set
            {
                fromDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FromDate"));
                //if (FromDate != null && ToDate != null && FromDate > ToDate)
                //{
                //    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("MyPreferencesCustomIntervalFailFromDate").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                //    FromDate = null;

                //}

            }
        }

        public DateTime? ToDate
        {
            get { return toDate; }
            set
            {
                toDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ToDate"));
                //if (FromDate != null && ToDate != null && FromDate > ToDate)
                //{
                //    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("MyPreferencesCustomIntervalFailToDate").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                //    ToDate = null;
                //}
            }
        }


        public int? CustomPeriodOption
        {
            get
            {
                return customPeriodOption;
            }

            set
            {
                customPeriodOption = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomPeriodOption"));
            }
        }

        public List<object> SelectedCRMSections
        {
            get
            {
                return selectedCRMSections;
            }

            set
            {
                selectedCRMSections = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCRMSections"));
            }
        }
        public Visibility IsSectionVisible
        {
            get { return isSectionVisible; }
            set { isSectionVisible = value; OnPropertyChanged(new PropertyChangedEventArgs("IsSectionVisible")); }
        }

        public List<string> lstTopRange { get; set; }
        public List<string> AutoRefreshLst { get; set; }

        public int? AutoRefreshOffOption
        {
            get
            {
                return autoRefreshOffOption;
            }

            set
            {
                autoRefreshOffOption = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AutoRefreshOffOption"));
                if (value == 1)
                {
                    IsSectionVisible = Visibility.Visible;
                }
                else
                {
                    IsSectionVisible = Visibility.Collapsed;
                }
            }
        }

        public Visibility LoadDataVisisbility
        {
            get
            {
                return loadDataVisisbility;
            }

            set
            {
                loadDataVisisbility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LoadDataVisisbility"));
            }
        }


        public int SelectedIndexRegion
        {
            get { return selectedIndexRegion; }
            set
            {
                selectedIndexRegion = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexRegion"));
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

        public int SelectedIndexTopOffers
        {
            get { return selectedIndexTopOffers; }
            set
            {
                selectedIndexTopOffers = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexTopOffers"));
            }
        }

        public int SelectedIndexTopCustomer
        {
            get { return selectedIndexTopCustomer; }
            set
            {
                selectedIndexTopCustomer = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexTopCutomer"));
            }
        }

        public DateTime MaxDate
        {
            get { return maxDate; }
            set
            {
                maxDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MaxDate"));
            }
        }

        public DateTime MinDate
        {
            get { return minDate; }
            set
            {
                minDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MinDate"));
            }
        }

        public DateTime OfferPeriodDate
        {
            get { return offerPeriodDate; }
            set
            {
                offerPeriodDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OfferPeriodDate"));
            }
        }

        public int SelectedIndexCurrency
        {
            get
            {
                return selectedIndexCurrency;
            }
            set
            {
                selectedIndexCurrency = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexCurrency"));
            }
        }

        public int SelectedIndexAutoRefresh
        {
            get
            {
                return selectedIndexAutoRefresh;
            }

            set
            {
                selectedIndexAutoRefresh = value; OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexAutoRefresh"));
            }
        }

        public bool IsAutoRefresh
        {
            get
            {
                return isAutoRefresh;
            }

            set
            {
                isAutoRefresh = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAutoRefresh"));

                if (!isAutoRefresh)
                {
                    LoadDataVisisbility = Visibility.Visible;
                    AutoRefreshOffOption = 0;
                }
                else
                {
                    LoadDataVisisbility = Visibility.Collapsed;
                    AutoRefreshOffOption = null;
                }
            }
        }

        public List<Currency> Currencies
        {
            get
            {
                return currencies;
            }

            set
            {
                currencies = value;
            }
        }

        public int SelectedIndexWorkbenchUserPermission
        {
            get
            {
                return selectedIndexWorkbenchUserPermission;
            }

            set
            {
                selectedIndexWorkbenchUserPermission = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexWorkbenchUserPermission"));
                FillSectionsDeatils(selectedIndexWorkbenchUserPermission);
            }
        }

        public string Opportunity
        {
            get
            {
                return opportunity;
            }

            set
            {
                opportunity = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Opportunity"));
            }
        }

        public string Contact
        {
            get
            {
                return contact;
            }

            set
            {
                contact = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Contact"));
            }
        }

        public string Account
        {
            get
            {
                return account;
            }

            set
            {
                account = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Account"));
            }
        }

        public string Appointment
        {
            get
            {
                return appointment;
            }

            set
            {
                appointment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Appointment"));
            }
        }

        public string Call
        {
            get
            {
                return call;
            }

            set
            {
                call = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Call"));
            }
        }

        public string Task
        {
            get
            {
                return task;
            }

            set
            {
                task = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Task"));
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
                OnPropertyChanged(new PropertyChangedEventArgs("Email"));
            }
        }

        public string Action
        {
            get
            {
                return action;
            }

            set
            {
                action = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Action"));
            }
        }

        public string SearchOpportunityOrOrder
        {
            get
            {
                return searchOpportunityOrOrder;
            }

            set
            {
                searchOpportunityOrOrder = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SearchOpportunityOrOrder"));
            }
        }

        public string MatrixList
        {
            get
            {
                return matrixList;
            }

            set
            {
                matrixList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MatrixList"));
            }
        }

        #endregion // Properties

        #region public ICommand

        public ICommand MyPreferencesViewAcceptButtonCommand { get; set; }
        public ICommand MyPreferencesViewCancelButtonCommand { get; set; }
        public ICommand Opportunity_KeyDownCommand { get; set; }
        public ICommand Contact_KeyDownCommand { get; set; }
        public ICommand Account_KeyDownCommand { get; set; }
        public ICommand Appointment_KeyDownCommand { get; set; }
        public ICommand Task_KeyDownCommand { get; set; }
        public ICommand Email_KeyDownCommand { get; set; }
        public ICommand Call_KeyDownCommand { get; set; }
        public ICommand Action_KeyDownCommand { get; set; }
        public ICommand SearchOpportunityOrOrder_KeyDownCommand { get; set; }

        public ICommand Opportunity_PreviewKeyDownCopyCommand { get; set; }
        public ICommand Opportunity_PreviewKeyDownPasteCommand { get; set; }
        public ICommand Opportunity_PreviewKeyDownAllCommand { get; set; }

        public ICommand Contact_PreviewKeyDownCopyCommand { get; set; }
        public ICommand Contact_PreviewKeyDownPasteCommand { get; set; }
        public ICommand Contact_PreviewKeyDownAllCommand { get; set; }

        public ICommand Account_PreviewKeyDownCopyCommand { get; set; }
        public ICommand Account_PreviewKeyDownPasteCommand { get; set; }
        public ICommand Account_PreviewKeyDownAllCommand { get; set; }

        public ICommand Appointment_PreviewKeyDownCopyCommand { get; set; }
        public ICommand Appointment_PreviewKeyDownPasteCommand { get; set; }
        public ICommand Appointment_PreviewKeyDownAllCommand { get; set; }

        public ICommand Task_PreviewKeyDownCopyCommand { get; set; }
        public ICommand Task_PreviewKeyDownPasteCommand { get; set; }
        public ICommand Task_PreviewKeyDownAllCommand { get; set; }

        public ICommand Email_PreviewKeyDownCopyCommand { get; set; }
        public ICommand Email_PreviewKeyDownPasteCommand { get; set; }
        public ICommand Email_PreviewKeyDownAllCommand { get; set; }

        public ICommand Call_PreviewKeyDownCopyCommand { get; set; }
        public ICommand Call_PreviewKeyDownPasteCommand { get; set; }
        public ICommand Call_PreviewKeyDownAllCommand { get; set; }

        public ICommand Action_PreviewKeyDownCopyCommand { get; set; }
        public ICommand Action_PreviewKeyDownPasteCommand { get; set; }
        public ICommand Action_PreviewKeyDownAllCommand { get; set; }

        public ICommand SearchOpportunityOrOrder_PreviewKeyDownCopyCommand { get; set; }
        public ICommand SearchOpportunityOrOrder_PreviewKeyDownPasteCommand { get; set; }
        public ICommand SearchOpportunityOrOrder_PreviewKeyDownAllCommand { get; set; }

        public ICommand MatrixList_KeyDownCommand { get; set; }
        public ICommand MatrixList_PreviewKeyDownCopyCommand { get; set; }
        public ICommand MatrixList_PreviewKeyDownPasteCommand { get; set; }
        public ICommand MatrixList_PreviewKeyDownAllCommand { get; set; }
        public ICommand CommandTextInput { get; set; }
        #endregion // ICommand

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
                     //me[BindableBase.GetPropertyName(() => AutoRefreshOffOption)] +    // AutoRefresh Off Option
                     me[BindableBase.GetPropertyName(() => FromDate)] +
                      me[BindableBase.GetPropertyName(() => ToDate)]+
                    me[BindableBase.GetPropertyName(() => Opportunity)]+
                    me[BindableBase.GetPropertyName(() => Contact)]+
                    me[BindableBase.GetPropertyName(() => Account)]+
                    me[BindableBase.GetPropertyName(() => Appointment)]+
                    me[BindableBase.GetPropertyName(() => Task)]+
                    me[BindableBase.GetPropertyName(() => Email)] +
                    me[BindableBase.GetPropertyName(() => Call)]+
                    me[BindableBase.GetPropertyName(() => Action)] +
                    me[BindableBase.GetPropertyName(() => SearchOpportunityOrOrder)] +
                    me[BindableBase.GetPropertyName(() => MatrixList)];


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
                //string AutoRefreshOffOptionProp = BindableBase.GetPropertyName(() => AutoRefreshOffOption);   // selectedIndexSalesUnit
                string FromDateProp = BindableBase.GetPropertyName(() => FromDate);
                string ToDateProp = BindableBase.GetPropertyName(() => ToDate);
                string OpportunityProp = BindableBase.GetPropertyName(() => Opportunity);
                string ContactProp = BindableBase.GetPropertyName(() => Contact);
                string AccountProp = BindableBase.GetPropertyName(() => Account);
                string AppointmentProp = BindableBase.GetPropertyName(() => Appointment);
                string TaskProp = BindableBase.GetPropertyName(() => Task);
                string EmailProp = BindableBase.GetPropertyName(() => Email);
                string CallProp = BindableBase.GetPropertyName(() => Call);
                string ActionProp = BindableBase.GetPropertyName(() => Action);
                string SearchOpportunityOrOrderProp = BindableBase.GetPropertyName(() => SearchOpportunityOrOrder);
                string MatrixListProp = BindableBase.GetPropertyName(() => MatrixList);

                //if (columnName == AutoRefreshOffOptionProp)
                //    return MyPreferencesValidations.GetErrorMessage(AutoRefreshOffOptionProp, AutoRefreshOffOption);
                //else 


                if (columnName == FromDateProp && IsCustomChecked == true)               //From Date
                {
                    if (FromDate != null && ToDate != null)
                    {
                        int result = DateTime.Compare(FromDate.Value, ToDate.Value);
                        if (result > 0)
                            return "The date you entered occurs after the end date.";
                        else if (result == 0)
                            return "From Date is the same as To Date";
                    }
                    return MyPreferencesValidations.GetErrorMessage(FromDateProp, FromDate);
                }
                else if (columnName == ToDateProp && IsCustomChecked == true)               //To Date
                {
                    if (FromDate != null && ToDate != null)
                    {
                        int result = DateTime.Compare(ToDate.Value, FromDate.Value);
                        if (result < 0)
                            return "The date you entered occurs before the start date.";
                        else if (result == 0)
                            return "To Date is the same as From Date";
                    }
                    return MyPreferencesValidations.GetErrorMessage(ToDateProp, ToDate);
                }
                else if (columnName == OpportunityProp)
                {
                    return MyPreferencesValidations.GetErrorMessage(OpportunityProp, Opportunity);
                }
                else if (columnName == ContactProp)
                {
                    return MyPreferencesValidations.GetErrorMessage(ContactProp, Contact);
                }
                else if (columnName == AccountProp)
                {
                    return MyPreferencesValidations.GetErrorMessage(AccountProp, Account);
                }
                
                else if (columnName == AppointmentProp)
                {
                    return MyPreferencesValidations.GetErrorMessage(AppointmentProp, Appointment);
                }
                else if (columnName == TaskProp)
                {
                    return MyPreferencesValidations.GetErrorMessage(TaskProp, Task);
                }
                else if (columnName == EmailProp)
                {
                    return MyPreferencesValidations.GetErrorMessage(EmailProp, Email);
                }
                else if (columnName == CallProp)
                {
                    return MyPreferencesValidations.GetErrorMessage(CallProp, Call);
                }
                else if (columnName == ActionProp)
                {
                    return MyPreferencesValidations.GetErrorMessage(ActionProp, Action);
                }
                else if (columnName == SearchOpportunityOrOrderProp)
                {
                    return MyPreferencesValidations.GetErrorMessage(SearchOpportunityOrOrderProp, SearchOpportunityOrOrder);
                }
                else if (columnName == MatrixListProp)
                {
                    return MyPreferencesValidations.GetErrorMessage(MatrixListProp, MatrixList);
                }
                return null;
            }
        }

        #endregion

        #region Constructor

        public MyPreferencesViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor MyPreferencesViewModel ...", category: Category.Info, priority: Priority.Low);

                MyPreferencesViewAcceptButtonCommand = new RelayCommand(new Action<object>(SaveMyPreference));
                MyPreferencesViewCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                Opportunity_KeyDownCommand = new DelegateCommand<KeyEventArgs>(Opportunity_KeyDown);
                Contact_KeyDownCommand = new DelegateCommand<KeyEventArgs>(Contact_KeyDown);
                Account_KeyDownCommand = new DelegateCommand<KeyEventArgs>(Account_KeyDown);
                Appointment_KeyDownCommand = new DelegateCommand<KeyEventArgs>(Appointment_KeyDown);
                Task_KeyDownCommand = new DelegateCommand<KeyEventArgs>(Task_KeyDown);
                Email_KeyDownCommand = new DelegateCommand<KeyEventArgs>(Email_KeyDown);
                Call_KeyDownCommand = new DelegateCommand<KeyEventArgs>(Call_KeyDown);
                Action_KeyDownCommand = new DelegateCommand<KeyEventArgs>(Action_KeyDown);
                SearchOpportunityOrOrder_KeyDownCommand = new DelegateCommand<KeyEventArgs>(SearchOpportunityOrOrder_KeyDown);
                MatrixList_KeyDownCommand = new DelegateCommand<KeyEventArgs>(MatrixList_KeyDown);

                Opportunity_PreviewKeyDownCopyCommand = new DelegateCommand<KeyEventArgs>(Opportunity_PreviewKeyDown_Copy);
                Opportunity_PreviewKeyDownPasteCommand = new DelegateCommand<KeyEventArgs>(Opportunity_PreviewKeyDown_Paste);
                Opportunity_PreviewKeyDownAllCommand = new DelegateCommand<KeyEventArgs>(Opportunity_PreviewKeyDown_All);

                Contact_PreviewKeyDownCopyCommand = new DelegateCommand<KeyEventArgs>(Contact_PreviewKeyDown_Copy);
                Contact_PreviewKeyDownPasteCommand = new DelegateCommand<KeyEventArgs>(Contact_PreviewKeyDown_Paste);
                Contact_PreviewKeyDownAllCommand = new DelegateCommand<KeyEventArgs>(Contact_PreviewKeyDown_All);

                Account_PreviewKeyDownCopyCommand = new DelegateCommand<KeyEventArgs>(Account_PreviewKeyDown_Copy);
                Account_PreviewKeyDownPasteCommand = new DelegateCommand<KeyEventArgs>(Account_PreviewKeyDown_Paste);
                Account_PreviewKeyDownAllCommand = new DelegateCommand<KeyEventArgs>(Account_PreviewKeyDown_All);

                Appointment_PreviewKeyDownCopyCommand = new DelegateCommand<KeyEventArgs>(Appointment_PreviewKeyDown_Copy);
                Appointment_PreviewKeyDownPasteCommand = new DelegateCommand<KeyEventArgs>(Appointment_PreviewKeyDown_Paste);
                Appointment_PreviewKeyDownAllCommand = new DelegateCommand<KeyEventArgs>(Appointment_PreviewKeyDown_All);

                Task_PreviewKeyDownCopyCommand = new DelegateCommand<KeyEventArgs>(Task_PreviewKeyDown_Copy);
                Task_PreviewKeyDownPasteCommand = new DelegateCommand<KeyEventArgs>(Task_PreviewKeyDown_Paste);
                Task_PreviewKeyDownAllCommand = new DelegateCommand<KeyEventArgs>(Task_PreviewKeyDown_All);

                Email_PreviewKeyDownCopyCommand = new DelegateCommand<KeyEventArgs>(Email_PreviewKeyDown_Copy);
                Email_PreviewKeyDownPasteCommand = new DelegateCommand<KeyEventArgs>(Email_PreviewKeyDown_Paste);
                Email_PreviewKeyDownAllCommand = new DelegateCommand<KeyEventArgs>(Email_PreviewKeyDown_All);

                Call_PreviewKeyDownCopyCommand = new DelegateCommand<KeyEventArgs>(Call_PreviewKeyDown_Copy);
                Call_PreviewKeyDownPasteCommand = new DelegateCommand<KeyEventArgs>(Call_PreviewKeyDown_Paste);
                Call_PreviewKeyDownAllCommand = new DelegateCommand<KeyEventArgs>(Call_PreviewKeyDown_All);

                Action_PreviewKeyDownCopyCommand = new DelegateCommand<KeyEventArgs>(Action_PreviewKeyDown_Copy);
                Action_PreviewKeyDownPasteCommand = new DelegateCommand<KeyEventArgs>(Action_PreviewKeyDown_Paste);
                Action_PreviewKeyDownAllCommand = new DelegateCommand<KeyEventArgs>(Action_PreviewKeyDown_All);

                SearchOpportunityOrOrder_PreviewKeyDownCopyCommand = new DelegateCommand<KeyEventArgs>(SearchOpportunityOrOrder_PreviewKeyDown_Copy);
                SearchOpportunityOrOrder_PreviewKeyDownPasteCommand = new DelegateCommand<KeyEventArgs>(SearchOpportunityOrOrder_PreviewKeyDown_Paste);
                SearchOpportunityOrOrder_PreviewKeyDownAllCommand = new DelegateCommand<KeyEventArgs>(SearchOpportunityOrOrder_PreviewKeyDown_All);

                MatrixList_PreviewKeyDownCopyCommand = new DelegateCommand<KeyEventArgs>(MatrixList_PreviewKeyDown_Copy);
                MatrixList_PreviewKeyDownPasteCommand = new DelegateCommand<KeyEventArgs>(MatrixList_PreviewKeyDown_Paste);
                MatrixList_PreviewKeyDownAllCommand = new DelegateCommand<KeyEventArgs>(MatrixList_PreviewKeyDown_All);

                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);
                FillTopRangeList();
                FillCurrencyDetails();
                FillUserPermissionsList();
                MaxDate = GeosApplication.Instance.ServerDateTime.Date;
                FillFinancialYear();
                IsSectionVisible = Visibility.Collapsed;

                if (GeosApplication.Instance.UserSettings != null)
                {
                    if (GeosApplication.Instance.UserSettings.ContainsKey("CustomPeriodOption"))
                    {
                        if (GeosApplication.Instance.UserSettings["CustomPeriodOption"].Equals("0"))
                        {
                            IsYearChecked = true;
                            IsCustomChecked = false;
                            if (GeosApplication.Instance.UserSettings.ContainsKey("CrmOfferPeriod"))
                            {
                                SelectedPeriod = GeosApplication.Instance.CrmOfferYear;
                                GeosApplication.Instance.SelectedyearStarDate = new DateTime(Convert.ToInt32(GeosApplication.Instance.CrmOfferYear), 1, 1).Add(timeSpanStart);
                                GeosApplication.Instance.SelectedyearEndDate = new DateTime(Convert.ToInt32(GeosApplication.Instance.CrmOfferYear), 12, 31).Add(timeSpanEnd);
                            }
                            else
                            {
                                SelectedPeriod = GeosApplication.Instance.ServerDateTime.Date.Year;
                                GeosApplication.Instance.SelectedyearStarDate = new DateTime(Convert.ToInt32(GeosApplication.Instance.ServerDateTime.Date.Year), 1, 1).Add(timeSpanStart);
                                GeosApplication.Instance.SelectedyearEndDate = new DateTime(Convert.ToInt32(GeosApplication.Instance.ServerDateTime.Date.Year), 12, 31).Add(timeSpanEnd);
                            }
                        }
                        else if (GeosApplication.Instance.UserSettings["CustomPeriodOption"].Equals("1"))
                        {
                            IsYearChecked = false;
                            IsCustomChecked = true;
                            FromDate = GeosApplication.Instance.SelectedyearStarDate;
                            ToDate = GeosApplication.Instance.SelectedyearEndDate;
                            SelectedPeriod = GeosApplication.Instance.CrmOfferYear;

                        }
                        else
                        {
                            IsYearChecked = true;
                            IsCustomChecked = false;
                            SelectedPeriod = GeosApplication.Instance.CrmOfferYear;
                            FromDate = new DateTime(Convert.ToInt32(GeosApplication.Instance.CrmOfferYear), 1, 1);
                            ToDate = new DateTime(Convert.ToInt32(GeosApplication.Instance.CrmOfferYear), 12, 31);
                        }
                    }


                    if (GeosApplication.Instance.UserSettings.ContainsKey("CrmTopOffers"))
                    {
                        SelectedIndexTopOffers = lstTopRange.FindIndex(i => i.Contains(GeosApplication.Instance.UserSettings["CrmTopOffers"].ToString()));
                    }

                    if (GeosApplication.Instance.UserSettings.ContainsKey("CrmTopCustomers"))
                    {
                        SelectedIndexTopCustomer = lstTopRange.FindIndex(i => i.Contains(GeosApplication.Instance.UserSettings["CrmTopCustomers"].ToString()));
                    }

                    if (GeosApplication.Instance.UserSettings.ContainsKey("AutoRefresh"))
                    {
                        if (GeosApplication.Instance.UserSettings["AutoRefresh"].ToString() == "Yes")
                            IsAutoRefresh = true;
                        else
                        {
                            IsAutoRefresh = false;

                            if (GeosApplication.Instance.UserSettings.ContainsKey("LoadDataOn"))
                            {
                                if (!string.IsNullOrEmpty(GeosApplication.Instance.UserSettings["LoadDataOn"]))
                                {
                                    AutoRefreshOffOption = Convert.ToInt32(GeosApplication.Instance.UserSettings["LoadDataOn"]);
                                }
                            }

                        }
                    }

                    if (GeosApplication.Instance.UserSettings.ContainsKey("CurrentProfile"))
                    {
                        if (GeosApplication.Instance.IdUserPermission > 0)
                        {
                            GeosApplication.Instance.UserSettings["CurrentProfile"] = GeosApplication.Instance.IdUserPermission.ToString();
                            SelectedIndexWorkbenchUserPermission = WorkbenchUserPermissions.FindIndex(i => i.IdPermission == Convert.ToInt32(GeosApplication.Instance.UserSettings["CurrentProfile"].ToString()));
                        }
                        else
                        {
                            int value;
                            if (int.TryParse(GeosApplication.Instance.UserSettings["CurrentProfile"].ToString(), out value))
                                SelectedIndexWorkbenchUserPermission = WorkbenchUserPermissions.FindIndex(i => i.IdPermission == Convert.ToInt32(GeosApplication.Instance.UserSettings["CurrentProfile"].ToString()));
                            else
                                SelectedIndexWorkbenchUserPermission = -1;
                        }
                    }

                    // shortcuts
                    // Get shortcut for Opportunity
                    if (GeosApplication.Instance.UserSettings.ContainsKey("Opportunity"))
                    {
                        Opportunity = GeosApplication.Instance.UserSettings["Opportunity"].ToString();
                    }

                    // Get shortcut for  Contact
                    if (GeosApplication.Instance.UserSettings.ContainsKey("Contact"))
                    {
                        Contact = GeosApplication.Instance.UserSettings["Contact"].ToString();
                    }

                    // Get shortcut for  Account
                    if (GeosApplication.Instance.UserSettings.ContainsKey("Account"))
                    {
                        Account = GeosApplication.Instance.UserSettings["Account"].ToString();
                    }

                    // Get shortcut for Appointment
                    if (GeosApplication.Instance.UserSettings.ContainsKey("Appointment"))
                    {
                        Appointment = GeosApplication.Instance.UserSettings["Appointment"].ToString();
                    }

                    // Get shortcut for Call
                    if (GeosApplication.Instance.UserSettings.ContainsKey("Call"))
                    {
                        Call = GeosApplication.Instance.UserSettings["Call"].ToString();
                    }

                    // Get shortcut for Task
                    if (GeosApplication.Instance.UserSettings.ContainsKey("Task"))
                    {
                        Task = GeosApplication.Instance.UserSettings["Task"].ToString();
                    }

                    // Get shortcut for Email
                    if (GeosApplication.Instance.UserSettings.ContainsKey("Email"))
                    {
                        Email = GeosApplication.Instance.UserSettings["Email"].ToString();
                    }

                    // Get shortcut for Action
                    if (GeosApplication.Instance.UserSettings.ContainsKey("Action"))
                    {
                        Action = GeosApplication.Instance.UserSettings["Action"].ToString();
                    }

                    // Get shortcut Search Opportunity Or Order 
                    if (GeosApplication.Instance.UserSettings.ContainsKey("SearchOpportunityOrOrder"))
                    {
                        SearchOpportunityOrOrder = GeosApplication.Instance.UserSettings["SearchOpportunityOrOrder"].ToString();
                    }
                    //MatrixList
                    // Get shortcut Search Opportunity Or Order 
                    if (GeosApplication.Instance.UserSettings.ContainsKey("MatrixList"))
                    {
                        MatrixList = GeosApplication.Instance.UserSettings["MatrixList"].ToString();
                    }


                }

                GeosApplication.Instance.Logger.Log("Constructor MyPreferencesViewModel() executed successfully", category: Category.Info, priority: Priority.Low);

                if (GeosApplication.Instance.SelectedCrmsectionsList.Count == 0 || GeosApplication.Instance.SelectedCrmsectionsList[0] == null)
                {
                    SelectedCRMSections = new List<object>(GeosApplication.Instance.CrmsectionsList);
                }
                else
                {
                    SelectedCRMSections = new List<object>(GeosApplication.Instance.SelectedCrmsectionsList);
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in MyPreferencesViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void Opportunity_PreviewKeyDown_Copy(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Opportunity_PreviewKeyDown_Copy ...", category: Category.Info, priority: Priority.Low);
            try
            {
                Opportunity = "Ctrl + C";
                GeosApplication.Instance.Logger.Log("Method Opportunity_PreviewKeyDown_Copy....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Opportunity_PreviewKeyDown_Copy...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void Opportunity_PreviewKeyDown_Paste(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Opportunity_PreviewKeyDown_Paste ...", category: Category.Info, priority: Priority.Low);
            try
            {
                Opportunity = "Ctrl + V";
                GeosApplication.Instance.Logger.Log("Method Opportunity_PreviewKeyDown_Paste....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Opportunity_PreviewKeyDown_Paste...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void Opportunity_PreviewKeyDown_All(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Opportunity_PreviewKeyDown_Paste ...", category: Category.Info, priority: Priority.Low);
            try
            {
                Opportunity = "Ctrl + A";
                GeosApplication.Instance.Logger.Log("Method Opportunity_PreviewKeyDown_Paste....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Opportunity_PreviewKeyDown_Paste...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void Contact_PreviewKeyDown_Copy(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Contact_PreviewKeyDown_Copy ...", category: Category.Info, priority: Priority.Low);
            try
            {
                Contact = "Ctrl + C";
                GeosApplication.Instance.Logger.Log("Method Contact_PreviewKeyDown_Copy....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Contact_PreviewKeyDown_Copy...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void Contact_PreviewKeyDown_Paste(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Contact_PreviewKeyDown_Paste ...", category: Category.Info, priority: Priority.Low);
            try
            {
                Contact = "Ctrl + V";
                GeosApplication.Instance.Logger.Log("Method Contact_PreviewKeyDown_Paste....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Contact_PreviewKeyDown_Paste...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void Contact_PreviewKeyDown_All(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Contact_PreviewKeyDown_Paste ...", category: Category.Info, priority: Priority.Low);
            try
            {
                Contact = "Ctrl + A";
                GeosApplication.Instance.Logger.Log("Method Contact_PreviewKeyDown_Paste....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Contact_PreviewKeyDown_Paste...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void Account_PreviewKeyDown_Copy(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Account_PreviewKeyDown_Copy ...", category: Category.Info, priority: Priority.Low);
            try
            {
                Account = "Ctrl + C";
                GeosApplication.Instance.Logger.Log("Method Account_PreviewKeyDown_Copy....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Account_PreviewKeyDown_Copy...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void Account_PreviewKeyDown_Paste(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Account_PreviewKeyDown_Paste ...", category: Category.Info, priority: Priority.Low);
            try
            {
                Account = "Ctrl + V";
                GeosApplication.Instance.Logger.Log("Method Account_PreviewKeyDown_Paste....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Account_PreviewKeyDown_Paste...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void Account_PreviewKeyDown_All(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Account_PreviewKeyDown_Paste ...", category: Category.Info, priority: Priority.Low);
            try
            {
                Account = "Ctrl + A";
                GeosApplication.Instance.Logger.Log("Method Account_PreviewKeyDown_Paste....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Account_PreviewKeyDown_Paste...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void Appointment_PreviewKeyDown_Copy(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Appointment_PreviewKeyDown_Copy ...", category: Category.Info, priority: Priority.Low);
            try
            {
                Appointment = "Ctrl + C";
                GeosApplication.Instance.Logger.Log("Method Appointment_PreviewKeyDown_Copy....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Appointment_PreviewKeyDown_Copy...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void Appointment_PreviewKeyDown_Paste(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Appointment_PreviewKeyDown_Paste ...", category: Category.Info, priority: Priority.Low);
            try
            {
                Appointment = "Ctrl + V";
                GeosApplication.Instance.Logger.Log("Method Appointment_PreviewKeyDown_Paste....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Appointment_PreviewKeyDown_Paste...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void Appointment_PreviewKeyDown_All(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Appointment_PreviewKeyDown_Paste ...", category: Category.Info, priority: Priority.Low);
            try
            {
                Appointment = "Ctrl + A";
                GeosApplication.Instance.Logger.Log("Method Appointment_PreviewKeyDown_Paste....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Appointment_PreviewKeyDown_Paste...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void Task_PreviewKeyDown_Copy(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Task_PreviewKeyDown_Copy ...", category: Category.Info, priority: Priority.Low);
            try
            {
                Task = "Ctrl + C";
                GeosApplication.Instance.Logger.Log("Method Task_PreviewKeyDown_Copy....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Task_PreviewKeyDown_Copy...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void Task_PreviewKeyDown_Paste(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Task_PreviewKeyDown_Paste ...", category: Category.Info, priority: Priority.Low);
            try
            {
                Task = "Ctrl + V";
                GeosApplication.Instance.Logger.Log("Method Task_PreviewKeyDown_Paste....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Task_PreviewKeyDown_Paste...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void Task_PreviewKeyDown_All(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Task_PreviewKeyDown_Paste ...", category: Category.Info, priority: Priority.Low);
            try
            {
                Task = "Ctrl + A";
                GeosApplication.Instance.Logger.Log("Method Task_PreviewKeyDown_Paste....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Task_PreviewKeyDown_Paste...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void Email_PreviewKeyDown_Copy(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Email_PreviewKeyDown_Copy ...", category: Category.Info, priority: Priority.Low);
            try
            {
                Email = "Ctrl + C";
                GeosApplication.Instance.Logger.Log("Method Email_PreviewKeyDown_Copy....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Email_PreviewKeyDown_Copy...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void Email_PreviewKeyDown_Paste(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Email_PreviewKeyDown_Paste ...", category: Category.Info, priority: Priority.Low);
            try
            {
                Email = "Ctrl + V";
                GeosApplication.Instance.Logger.Log("Method Email_PreviewKeyDown_Paste....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Email_PreviewKeyDown_Paste...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void Email_PreviewKeyDown_All(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Email_PreviewKeyDown_Paste ...", category: Category.Info, priority: Priority.Low);
            try
            {
                Email = "Ctrl + A";
                GeosApplication.Instance.Logger.Log("Method Email_PreviewKeyDown_Paste....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Email_PreviewKeyDown_Paste...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void Call_PreviewKeyDown_Copy(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Call_PreviewKeyDown_Copy ...", category: Category.Info, priority: Priority.Low);
            try
            {
                Call = "Ctrl + C";
                GeosApplication.Instance.Logger.Log("Method Call_PreviewKeyDown_Copy....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Call_PreviewKeyDown_Copy...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void Call_PreviewKeyDown_Paste(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Call_PreviewKeyDown_Paste ...", category: Category.Info, priority: Priority.Low);
            try
            {
                Call = "Ctrl + V";
                GeosApplication.Instance.Logger.Log("Method Call_PreviewKeyDown_Paste....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Call_PreviewKeyDown_Paste...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void Call_PreviewKeyDown_All(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Call_PreviewKeyDown_Paste ...", category: Category.Info, priority: Priority.Low);
            try
            {
                Call = "Ctrl + A";
                GeosApplication.Instance.Logger.Log("Method Call_PreviewKeyDown_Paste....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Call_PreviewKeyDown_Paste...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void Action_PreviewKeyDown_Copy(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Action_PreviewKeyDown_Copy ...", category: Category.Info, priority: Priority.Low);
            try
            {
                Action = "Ctrl + C";
                GeosApplication.Instance.Logger.Log("Method Action_PreviewKeyDown_Copy....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Action_PreviewKeyDown_Copy...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void Action_PreviewKeyDown_Paste(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Action_PreviewKeyDown_Paste ...", category: Category.Info, priority: Priority.Low);
            try
            {
                Action = "Ctrl + V";
                GeosApplication.Instance.Logger.Log("Method Action_PreviewKeyDown_Paste....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Action_PreviewKeyDown_Paste...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void Action_PreviewKeyDown_All(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Action_PreviewKeyDown_Paste ...", category: Category.Info, priority: Priority.Low);
            try
            {
                Action = "Ctrl + A";
                GeosApplication.Instance.Logger.Log("Method Action_PreviewKeyDown_Paste....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Action_PreviewKeyDown_Paste...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void SearchOpportunityOrOrder_PreviewKeyDown_Copy(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method SearchOpportunityOrOrder_PreviewKeyDown_Copy ...", category: Category.Info, priority: Priority.Low);
            try
            {
                SearchOpportunityOrOrder = "Ctrl + C";
                GeosApplication.Instance.Logger.Log("Method SearchOpportunityOrOrder_PreviewKeyDown_Copy....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SearchOpportunityOrOrder_PreviewKeyDown_Copy...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void SearchOpportunityOrOrder_PreviewKeyDown_Paste(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method SearchOpportunityOrOrder_PreviewKeyDown_Paste ...", category: Category.Info, priority: Priority.Low);
            try
            {
                SearchOpportunityOrOrder = "Ctrl + V";
                GeosApplication.Instance.Logger.Log("Method SearchOpportunityOrOrder_PreviewKeyDown_Paste....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SearchOpportunityOrOrder_PreviewKeyDown_Paste...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void SearchOpportunityOrOrder_PreviewKeyDown_All(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method SearchOpportunityOrOrder_PreviewKeyDown_Paste ...", category: Category.Info, priority: Priority.Low);
            try
            {
                SearchOpportunityOrOrder = "Ctrl + A";
                GeosApplication.Instance.Logger.Log("Method SearchOpportunityOrOrder_PreviewKeyDown_Paste....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SearchOpportunityOrOrder_PreviewKeyDown_Paste...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void MatrixList_PreviewKeyDown_Copy(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method MatrixList_PreviewKeyDown_Copy ...", category: Category.Info, priority: Priority.Low);
            try
            {
                MatrixList = "Ctrl + C";
                GeosApplication.Instance.Logger.Log("Method MatrixList_PreviewKeyDown_Copy....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method MatrixList_PreviewKeyDown_Copy...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void MatrixList_PreviewKeyDown_Paste(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method MatrixList_PreviewKeyDown_Paste ...", category: Category.Info, priority: Priority.Low);
            try
            {
                MatrixList = "Ctrl + V";
                GeosApplication.Instance.Logger.Log("Method MatrixList_PreviewKeyDown_Paste....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method MatrixList_PreviewKeyDown_Paste...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void MatrixList_PreviewKeyDown_All(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method MatrixList_PreviewKeyDown_Paste ...", category: Category.Info, priority: Priority.Low);
            try
            {
                MatrixList = "Ctrl + A";
                GeosApplication.Instance.Logger.Log("Method MatrixList_PreviewKeyDown_Paste....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method MatrixList_PreviewKeyDown_Paste...." + ex.Message, category: Category.Exception, priority: Priority.Low);
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

        #endregion // Constructor

        #region Methods

        private void FillTopRangeList()
        {
            GeosApplication.Instance.Logger.Log("Constructor FillTopRangeList ...", category: Category.Info, priority: Priority.Low);
            lstTopRange = new List<string>();
            lstTopRange.Add("10");
            lstTopRange.Add("20");
            lstTopRange.Add("30");
            lstTopRange.Add("40");
            lstTopRange.Add("50");
            lstTopRange.Add("60");
            lstTopRange.Add("70");
            lstTopRange.Add("80");
            lstTopRange.Add("90");
            lstTopRange.Add("100");
            GeosApplication.Instance.Logger.Log("Constructor FillTopRangeList() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// method for fill financial year.
        /// </summary>
        private void FillFinancialYear()
        {
            FinancialYearLst = new List<long>();
            for (long i = maxDate.Year + 1; i >= 2000; i--)
            {
                FinancialYearLst.Add(i);
            }
        }

        /// <summary>
        /// Method for fill user permission list.
        /// </summary>
        private void FillUserPermissionsList()
        {
            GeosApplication.Instance.Logger.Log("Method FillUserPermissionsList ...", category: Category.Info, priority: Priority.Low);

            List<int> userPermissionlst = new List<int>() { 20, 21, 22 };
            // this code remove duplicate User Permissions in list.
            WorkbenchUserPermissions = new List<UserPermission>(GeosApplication.Instance.ActiveUser.UserPermissions.Where(up => userPermissionlst.Contains(up.IdPermission)).GroupBy(x => x.IdPermission).Select(c => c.First()).ToList());
            WorkbenchUserPermissions = WorkbenchUserPermissions.OrderBy(c => c.IdPermission).ToList();

            GeosApplication.Instance.Logger.Log("Method FillUserPermissionsList() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        private void FillAutoRefreshLst()
        {
            GeosApplication.Instance.Logger.Log("Method FillAutoRefreshLst ...", category: Category.Info, priority: Priority.Low);

            AutoRefreshLst = new List<string>();
            AutoRefreshLst.Add("Yes");
            AutoRefreshLst.Add("No");

            GeosApplication.Instance.Logger.Log("Method FillAutoRefreshLst() executed successfully", category: Category.Info, priority: Priority.Low);
        }


        private void SaveMyPreference(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SaveMyPreference ...", category: Category.Info, priority: Priority.Low);
                string error = EnableValidationAndGetError();
                if (!IsAutoRefresh)
                {
                    //string error = EnableValidationAndGetError();
                    //PropertyChanged(this, new PropertyChangedEventArgs("AutoRefreshOffOption"));
                    if (IsCustomChecked)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("FromDate"));
                        PropertyChanged(this, new PropertyChangedEventArgs("ToDate"));
                    }
                    //if (error != null)
                    //{
                    //    return;
                    //}
                }
                else
                {
                    if (IsCustomChecked)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("FromDate"));
                        PropertyChanged(this, new PropertyChangedEventArgs("ToDate"));
                    }
                }
                PropertyChanged(this, new PropertyChangedEventArgs("Opportunity"));
                PropertyChanged(this, new PropertyChangedEventArgs("Contact"));
                PropertyChanged(this, new PropertyChangedEventArgs("Account"));
                PropertyChanged(this, new PropertyChangedEventArgs("Appointment"));
                PropertyChanged(this, new PropertyChangedEventArgs("Task"));
                PropertyChanged(this, new PropertyChangedEventArgs("Email"));
                PropertyChanged(this, new PropertyChangedEventArgs("Call"));
                PropertyChanged(this, new PropertyChangedEventArgs("Action"));
                PropertyChanged(this, new PropertyChangedEventArgs("SearchOpportunityOrOrder"));
                PropertyChanged(this, new PropertyChangedEventArgs("MatrixList"));

                if (error != null)
                {
                    return;
                }

                string selectedCRMSectionStr = string.Empty;

                List<string> Records = new List<string>();
                Records.Add(Opportunity);
                Records.Add(Contact);
                Records.Add(Account);
                Records.Add(Appointment);
                Records.Add(Task);
                Records.Add(Email);
                Records.Add(Call);
                Records.Add(Action);
                Records.Add(SearchOpportunityOrOrder);
                Records.Add(MatrixList);

                var query = Records.GroupBy(x => x).Where(g => g.Count() > 1).Select(y => y.Key).ToList();

                if(query!=null && query.Count>0)
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("DuplicateShortcutKey").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }
                if (SelectedCRMSections != null && SelectedCRMSections.Count > 0)
                {
                    foreach (CRMSections CRMSection in SelectedCRMSections)
                    {
                        if (CRMSection != null)
                        {
                            if (string.IsNullOrEmpty(selectedCRMSectionStr))
                                selectedCRMSectionStr = CRMSection.IdSection.ToString();
                            else
                                selectedCRMSectionStr += "," + CRMSection.IdSection.ToString();
                        }
                    }

                    GeosApplication.Instance.SelectedCrmsectionsList = SelectedCRMSections.Cast<CRMSections>().ToList();
                }

                else
                {
                    GeosApplication.Instance.SelectedCrmsectionsList = null;
                }

                List<Tuple<string, string>> lstUserConfiguration = new List<Tuple<string, string>>();

                if (IsYearChecked == true)
                {
                    CustomPeriodOption = 0;
                    GeosApplication.Instance.UserSettings["CustomPeriodOption"] = Convert.ToString(CustomPeriodOption);

                    if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("CustomPeriodOption"))
                    {

                        if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("CrmOfferPeriod"))
                        {
                            GeosApplication.Instance.UserSettings["CrmOfferPeriod"] = SelectedPeriod.ToString();
                        }
                        else
                        {
                            GeosApplication.Instance.UserSettings["CrmOfferPeriod"] = SelectedPeriod.ToString();
                        }
                        GeosApplication.Instance.CrmOfferYear = Convert.ToInt64(GeosApplication.Instance.UserSettings["CrmOfferPeriod"].ToString());
                        GeosApplication.Instance.SelectedyearStarDate = new DateTime(Convert.ToInt32(GeosApplication.Instance.CrmOfferYear), 1, 1).Add(timeSpanStart);
                        GeosApplication.Instance.SelectedyearEndDate = new DateTime(Convert.ToInt32(GeosApplication.Instance.CrmOfferYear), 12, 31).Add(timeSpanEnd);

                    }
                }
                else if (IsCustomChecked == true)
                {
                    CustomPeriodOption = 1;
                    GeosApplication.Instance.UserSettings["CustomPeriodOption"] = Convert.ToString(CustomPeriodOption);
                    if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("CustomPeriodOption"))
                    {
                        if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("CrmOfferPeriod"))
                        {
                            GeosApplication.Instance.UserSettings["CrmOfferFromInterval"] = FromDate.Value.ToString("yyyy/MM/dd");
                        }
                        else
                        {
                            GeosApplication.Instance.UserSettings["CrmOfferFromInterval"] = DateTime.Now.ToString("yyyy/MM/dd");
                        }

                        if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("CrmOfferPeriod"))
                        {
                            GeosApplication.Instance.UserSettings["CrmOfferToInterval"] = ToDate.Value.ToString("yyyy/MM/dd");
                        }
                        else
                        {
                            GeosApplication.Instance.UserSettings["CrmOfferToInterval"] = DateTime.Now.ToString("yyyy/MM/dd");
                        }

                        GeosApplication.Instance.SelectedyearStarDate = FromDate != null ? (DateTime)FromDate : GeosApplication.Instance.ServerDateTime.Date;

                        GeosApplication.Instance.SelectedyearEndDate = ToDate != null ? (DateTime)ToDate : GeosApplication.Instance.ServerDateTime.Date;


                        GeosApplication.Instance.SelectedyearStarDate.Add(timeSpanStart);
                        GeosApplication.Instance.SelectedyearEndDate.Add(timeSpanEnd);
                    }

                }

                if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("CrmTopOffers"))
                {
                    GeosApplication.Instance.UserSettings["CrmTopOffers"] = lstTopRange[SelectedIndexTopOffers].ToString();
                }
                else
                {
                    GeosApplication.Instance.UserSettings["CrmTopOffers"] = lstTopRange[SelectedIndexTopOffers].ToString();
                }

                if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("CrmTopCustomers"))
                {
                    GeosApplication.Instance.UserSettings["CrmTopCustomers"] = lstTopRange[SelectedIndexTopCustomer].ToString();
                }
                else
                {
                    GeosApplication.Instance.UserSettings["CrmTopCustomers"] = lstTopRange[SelectedIndexTopCustomer].ToString();
                }

                if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("SelectedCurrency"))
                {
                    GeosApplication.Instance.UserSettings["SelectedCurrency"] = Currencies[selectedIndexCurrency].Name;
                }
                else
                {
                    GeosApplication.Instance.UserSettings["SelectedCurrency"] = Currencies[selectedIndexCurrency].Name;
                }

                if (GeosApplication.Instance.UserSettings != null
                    && GeosApplication.Instance.UserSettings.ContainsKey("AutoRefresh")
                    && GeosApplication.Instance.UserSettings.ContainsKey("LoadDataOn"))
                {
                    if (IsAutoRefresh)
                    {
                        GeosApplication.Instance.UserSettings["AutoRefresh"] = "Yes";
                        GeosApplication.Instance.UserSettings["LoadDataOn"] = Convert.ToString(AutoRefreshOffOption);
                    }
                    else
                    {
                        GeosApplication.Instance.UserSettings["AutoRefresh"] = "No";
                        GeosApplication.Instance.UserSettings["LoadDataOn"] = Convert.ToString(AutoRefreshOffOption);
                    }
                }
                else
                {
                    if (IsAutoRefresh)
                    {
                        GeosApplication.Instance.UserSettings["AutoRefresh"] = "Yes";
                        GeosApplication.Instance.UserSettings["LoadDataOn"] = Convert.ToString(AutoRefreshOffOption);
                    }
                    else
                    {
                        GeosApplication.Instance.UserSettings["AutoRefresh"] = "No";
                        GeosApplication.Instance.UserSettings["LoadDataOn"] = Convert.ToString(AutoRefreshOffOption);
                    }
                }

                if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("SelectedCRMSectionLoadData"))
                {
                    GeosApplication.Instance.UserSettings["SelectedCRMSectionLoadData"] = selectedCRMSectionStr;
                }
                else
                {
                    GeosApplication.Instance.UserSettings["SelectedCRMSectionLoadData"] = selectedCRMSectionStr;
                }

                if (SelectedIndexWorkbenchUserPermission > -1)
                {
                    if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("CurrentProfile"))
                    {

                        GeosApplication.Instance.UserSettings["CurrentProfile"] = WorkbenchUserPermissions[SelectedIndexWorkbenchUserPermission].Permission.IdPermission.ToString();

                        int value1;
                        if (int.TryParse(GeosApplication.Instance.UserSettings["CurrentProfile"].ToString(), out value1))
                            GeosApplication.Instance.IdUserPermission = Convert.ToInt32(GeosApplication.Instance.UserSettings["CurrentProfile"].ToString());
                    }
                    else
                    {
                        GeosApplication.Instance.UserSettings["CurrentProfile"] = WorkbenchUserPermissions[SelectedIndexWorkbenchUserPermission].Permission.IdPermission.ToString(); ;
                    }
                }

                // shortcuts
                if (GeosApplication.Instance.UserSettings != null)
                {
                    //Set shortcut for Opportunity
                    if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("Opportunity"))
                    {
                        GeosApplication.Instance.UserSettings["Opportunity"] = Opportunity.TrimStart().TrimEnd();
                    }

                    //Set shortcut for Contact
                    if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("Contact"))
                    {
                        GeosApplication.Instance.UserSettings["Contact"] = Contact.TrimStart().TrimEnd();
                    }

                    //Set shortcut for Account
                    if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("Account"))
                    {
                        GeosApplication.Instance.UserSettings["Account"] = Account.TrimStart().TrimEnd();
                    }

                    //Set shortcut for Appointment
                    if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("Appointment"))
                    {
                        GeosApplication.Instance.UserSettings["Appointment"] = Appointment.TrimStart().TrimEnd();
                    }

                    //Set shortcut for Call
                    if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("Call"))
                    {
                        GeosApplication.Instance.UserSettings["Call"] = Call.TrimStart().TrimEnd();
                    }

                    //Set shortcut for Task
                    if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("Task"))
                    {
                        GeosApplication.Instance.UserSettings["Task"] = Task.TrimStart().TrimEnd();
                    }

                    //Set shortcut for Email
                    if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("Email"))
                    {
                        GeosApplication.Instance.UserSettings["Email"] = Email.TrimStart().TrimEnd();
                    }

                    //Set shortcut for Action
                    if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("Action"))
                    {
                        GeosApplication.Instance.UserSettings["Action"] = Action.TrimStart().TrimEnd();
                    }

                    //Set shortcut for Search Opprtunity/Order
                    if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("SearchOpportunityOrOrder"))
                    {
                        GeosApplication.Instance.UserSettings["SearchOpportunityOrOrder"] = SearchOpportunityOrOrder.TrimStart().TrimEnd();
                    }

                    //Set shortcut for MatrixList
                    if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("MatrixList"))
                    {
                        GeosApplication.Instance.UserSettings["MatrixList"] = MatrixList.TrimStart().TrimEnd();
                    }

                    CRMCommon.Instance.GetShortcuts();
                }

                

                foreach (KeyValuePair<string, string> setting in GeosApplication.Instance.UserSettings)
                {
                    lstUserConfiguration.Add(new Tuple<string, string>(setting.Key.ToString(), setting.Value.ToString()));
                }

                ApplicationOperation.CreateNewSetting(lstUserConfiguration, GeosApplication.Instance.UserSettingFilePath, "UserSettings");

                //GeosApplication.Instance.CrmOfferYear = Convert.ToInt64(GeosApplication.Instance.UserSettings["CrmOfferPeriod"].ToString());
                GeosApplication.Instance.CrmTopCustomers = Convert.ToUInt16(GeosApplication.Instance.UserSettings["CrmTopCustomers"].ToString());
                GeosApplication.Instance.CrmTopOffers = Convert.ToUInt16(GeosApplication.Instance.UserSettings["CrmTopOffers"].ToString());
                GeosApplication.Instance.IdCurrencyByRegion = GeosApplication.Instance.Currencies.Where(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency"].ToString()).Select(cur => cur.IdCurrency).SingleOrDefault();
                GeosApplication.Instance.CurrentCurrencySymbol = GeosApplication.Instance.Currencies.Where(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency"].ToString()).Select(cur => cur.Symbol).SingleOrDefault();

                int value;
                if (int.TryParse(GeosApplication.Instance.UserSettings["CurrentProfile"].ToString(), out value))
                    GeosApplication.Instance.IdUserPermission = Convert.ToInt32(GeosApplication.Instance.UserSettings["CurrentProfile"].ToString());

                // FillGroupList();
                // FillCompanyPlantList();

                FillPlantOrSalesOwnerList();

                GeosApplication.SetCurrencySymbol(GeosApplication.Instance.CurrentCurrencySymbol);

                //GeosApplication.Instance.SelectedyearStarDate = new DateTime(Convert.ToInt32(GeosApplication.Instance.CrmOfferYear), 1, 1);
                //GeosApplication.Instance.SelectedyearEndDate = new DateTime(Convert.ToInt32(GeosApplication.Instance.CrmOfferYear), 12, 31);

                //if (GeosApplication.Instance.SelectedyearEndDate.Year < GeosApplication.Instance.ServerDateTime.Year)
                //{
                //    GeosApplication.Instance.RemainingDays = "0";
                //}
                //else
                //{
                //    GeosApplication.Instance.RemainingDays = (Math.Round((GeosApplication.Instance.SelectedyearEndDate.Date - GeosApplication.Instance.ServerDateTime.Date).TotalDays, 0)).ToString();
                //}

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
                            //EntireCompanyPlantList = new ObservableCollection<Company>(CrmStartUp.GetSelectedUserCompanyPlantByIdUser(salesOwnersIds, true));
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
                        //EntireCompanyPlantList = new ObservableCollection<Company>(CrmStartUp.GetCompanyPlantByUserId(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission, true));
                        //[pramod.misal][GEOS2-4682][08-08-2023]
                        EntireCompanyPlantList = new ObservableCollection<Company>(CrmStartUp.GetCompanyPlantByUserId_V2420(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission, true));
                        GeosApplication.Instance.ObjectPool.Remove("CRM_COMPANYPLANT");
                        GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYPLANT", EntireCompanyPlantList);
                    }
                }
                catch (Exception ex)
                {
                }

                //GeosApplication.Instance.RemainingDays = (Math.Round((GeosApplication.Instance.SelectedyearEndDate.Date.AddDays(+1) - GeosApplication.Instance.SelectedyearStarDate.Date).TotalDays, 0)).ToString();

                int RemainDays = (int)(GeosApplication.Instance.SelectedyearEndDate.Date - DateTime.Now.Date).TotalDays;
                if (RemainDays <= 0)
                {
                    RemainDays = 0;
                }
                GeosApplication.Instance.RemainingDays = RemainDays.ToString();
                IsPreferenceChanged = true;
                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log("Method SaveMyPreference() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SaveMyPreference() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        static IEnumerable<CultureInfo> GetCultureInfosByCurrencySymbol(string currencySymbol)
        {
            if (currencySymbol == null)
            {
            }

            return CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                .Where(x => new RegionInfo(x.LCID).ISOCurrencySymbol == currencySymbol);
        }


        /// <summary>
        /// Method for get fill currency list and get IdCurrency By current System Region Culture.
        /// </summary>
        public void FillCurrencyDetails()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCurrencyDetails ...", category: Category.Info, priority: Priority.Low);

                Currencies = GeosApplication.Instance.Currencies.ToList();
                SelectedIndexCurrency = Currencies.FindIndex(i => i.IdCurrency == GeosApplication.Instance.IdCurrencyByRegion);

                GeosApplication.Instance.Logger.Log("Method FillCurrencyDetails() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCurrencyDetails() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// [001][GEOS2-2074][cpatil][18-02-2020]CRM - OPPORTUNITIES - Timeline
        /// [001][GEOS2-1977][cpatil][18-02-2020]The code added in the offer code must be taken from the application selected site
        /// [003][cpatil][GEOS2-5299][26-02-2024]

        private void FillPlantOrSalesOwnerList()
        {
            GeosApplication.Instance.Logger.Log("Method FillPlantOrSalesOwnerList ...", category: Category.Info, priority: Priority.Low);
            GeosApplication.Instance.CmbPlantOwnerUsers = Visibility.Hidden;
            GeosApplication.Instance.CmbSalesOwnerUsers = Visibility.Hidden;

            // Start (SalesOwner) - Selected Sales Owners User list for CRM. 
            if (GeosApplication.Instance.IdUserPermission == 21)
            {
                if (GeosApplication.Instance.SalesOwnerUsersList == null)
                {
                    GeosApplication.Instance.SalesOwnerUsersList = WorkbenchStartUp.GetManagerUsers(GeosApplication.Instance.ActiveUser.IdUser);
                  
                }

                //GeosApplication.Instance.SelectedSalesOwnerUsersList = new List<object>();
                UserManagerDtl usrDefault = GeosApplication.Instance.SalesOwnerUsersList.FirstOrDefault(x => x.IdUser == GeosApplication.Instance.ActiveUser.IdUser);

                GeosApplication.Instance.CmbPlantOwnerUsers = Visibility.Hidden;
                GeosApplication.Instance.CmbSalesOwnerUsers = Visibility.Visible;

                if (GeosApplication.Instance.SelectedSalesOwnerUsersList == null)
                {
                    GeosApplication.Instance.SelectedSalesOwnerUsersList = new List<object>();

                    if (usrDefault != null)
                    {
                        GeosApplication.Instance.SelectedSalesOwnerUsersList.Add(usrDefault);
                    }
                    else
                    {
                        GeosApplication.Instance.SelectedSalesOwnerUsersList.AddRange(GeosApplication.Instance.SalesOwnerUsersList);
                    }

                }
                else if (GeosApplication.Instance.SelectedSalesOwnerUsersList.Count == 0)
                {
                    if (usrDefault != null)
                    {
                        GeosApplication.Instance.SelectedSalesOwnerUsersList.Add(usrDefault);
                    }
                    else
                    {
                        GeosApplication.Instance.SelectedSalesOwnerUsersList.AddRange(GeosApplication.Instance.SalesOwnerUsersList);
                    }
                }

                GeosApplication.Instance.SelectedSalesOwnerUsersList = new List<object>(GeosApplication.Instance.SelectedSalesOwnerUsersList);

            }
            // End (SalesOwner)

            // Start (PlantOwner) - Selected Plant Owners User list for CRM. 
            if (GeosApplication.Instance.IdUserPermission == 22)
            {
                //[002] Changed service method GetAllCompaniesDetails to GetAllCompaniesDetails_V2040
                //[003] Changed service method GetAllCompaniesDetails_V2040 to GetAllCompaniesDetails_V2490
                //Service GetAllCompaniesDetails_V2490 updated with GetAllCompaniesDetails_V2500 by [GEOS2-5556][27.03.2024][rdixit]
                if (GeosApplication.Instance.PlantOwnerUsersList == null)
                {
                    GeosApplication.Instance.PlantOwnerUsersList = CrmStartUp.GetAllCompaniesDetails_V2500(GeosApplication.Instance.ActiveUser.IdUser);
                }

                GeosApplication.Instance.PlantOwnerUsersList = GeosApplication.Instance.PlantOwnerUsersList.OrderBy(o => o.Country.IdCountry).ToList();
                // GeosApplication.Instance.SelectedPlantOwnerUsersList = new List<object>();

                GeosApplication.Instance.CmbPlantOwnerUsers = Visibility.Visible;
                GeosApplication.Instance.CmbSalesOwnerUsers = Visibility.Hidden;

                // EmdepSite emdepSite = CrmStartUp.GetEmdepSiteById(Convert.ToInt32(GeosApplication.Instance.ActiveUser...Site.ConnectPlantId));
                string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.IsSelected == true).Select(xy => xy.Name).FirstOrDefault();
                Company usrDefault = GeosApplication.Instance.PlantOwnerUsersList.FirstOrDefault(x => x.ShortName == serviceurl);

                if (GeosApplication.Instance.SelectedPlantOwnerUsersList == null)
                {
                    GeosApplication.Instance.SelectedPlantOwnerUsersList = new List<object>();

                    if (usrDefault != null)
                    {
                        GeosApplication.Instance.SelectedPlantOwnerUsersList.Add(usrDefault);
                    }
                    else
                    {
                        GeosApplication.Instance.SelectedPlantOwnerUsersList.AddRange(GeosApplication.Instance.PlantOwnerUsersList);
                    }
                }
                else if (GeosApplication.Instance.SelectedPlantOwnerUsersList.Count == 0)
                {
                    if (usrDefault != null)
                    {
                        GeosApplication.Instance.SelectedPlantOwnerUsersList.Add(usrDefault);
                    }
                    else
                    {
                        GeosApplication.Instance.SelectedPlantOwnerUsersList.AddRange(GeosApplication.Instance.PlantOwnerUsersList);
                    }
                }

                GeosApplication.Instance.SelectedPlantOwnerUsersList = new List<object>(GeosApplication.Instance.SelectedPlantOwnerUsersList);
            }
            GeosApplication.Instance.Logger.Log("Method FillPlantOrSalesOwnerList() executed successfully", category: Category.Info, priority: Priority.Low);
            // End (PlantOwner)
        }


        /// <summary>
        /// Method for fill list of all sections of CRM module.
        /// </summary>
        private void FillSectionsDeatils(int indexSelectedProfile)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillSectionsDeatils ...", category: Category.Info, priority: Priority.Low);

                int idCurrentSelectedProfile = WorkbenchUserPermissions[indexSelectedProfile].IdPermission;

                List<CRMSections> crmList = new List<CRMSections>();

                crmList.Add(new CRMSections() { IdSection = 1, SectionName = System.Windows.Application.Current.FindResource("CrmMainViewModelDashboard1").ToString() });
                crmList.Add(new CRMSections() { IdSection = 2, SectionName = System.Windows.Application.Current.FindResource("CrmMainViewModelDashboard2").ToString() });
                crmList.Add(new CRMSections() { IdSection = 3, SectionName = System.Windows.Application.Current.FindResource("CrmMainViewModelDashboard3").ToString() });

                if (idCurrentSelectedProfile == 22)
                {
                    crmList.Add(new CRMSections() { IdSection = 4, SectionName = System.Windows.Application.Current.FindResource("CrmMainViewModelDashboardOperations").ToString() });
                    crmList.Add(new CRMSections() { IdSection = 5, SectionName = System.Windows.Application.Current.FindResource("CrmMainViewModelDashboardEngineeringAnalysis").ToString() });
                }

                crmList.Add(new CRMSections() { IdSection = 6, SectionName = System.Windows.Application.Current.FindResource("CrmMainViewModelActivities").ToString() });
                crmList.Add(new CRMSections() { IdSection = 7, SectionName = System.Windows.Application.Current.FindResource("CrmMainViewModelAccounts").ToString() });
                crmList.Add(new CRMSections() { IdSection = 8, SectionName = System.Windows.Application.Current.FindResource("CrmMainViewModelContacts").ToString() });
                crmList.Add(new CRMSections() { IdSection = 9, SectionName = System.Windows.Application.Current.FindResource("CrmMainViewModelTimeline").ToString() });

                crmList.Add(new CRMSections() { IdSection = 10, SectionName = System.Windows.Application.Current.FindResource("CrmMainViewModelPipeline").ToString() });
                crmList.Add(new CRMSections() { IdSection = 11, SectionName = System.Windows.Application.Current.FindResource("CrmMainViewModelOrders").ToString() });
                crmList.Add(new CRMSections() { IdSection = 12, SectionName = System.Windows.Application.Current.FindResource("ItemsForecastHeader").ToString() });
                //crmList.Add(new CRMSections() { IdSection = 13, SectionName = System.Windows.Application.Current.FindResource("ReportDashboardHeader").ToString() });

                crmList.Add(new CRMSections() { IdSection = 15, SectionName = System.Windows.Application.Current.FindResource("CrmMainViewModelTargetForecast").ToString() });
                crmList.Add(new CRMSections() { IdSection = 16, SectionName = System.Windows.Application.Current.FindResource("CrmMainViewModelPlantQuota").ToString() });
                crmList.Add(new CRMSections() { IdSection = 17, SectionName = System.Windows.Application.Current.FindResource("CrmMainViewModelUsers").ToString() });

                GeosApplication.Instance.CrmsectionsList = crmList;

                string[] CrmSelectedStr = GeosApplication.Instance.UserSettings["SelectedCRMSectionLoadData"].Split(',');
                GeosApplication.Instance.SelectedCrmsectionsList = new List<CRMSections>();

                if (CrmSelectedStr != null && CrmSelectedStr[0] != "")
                {
                    foreach (var item in CrmSelectedStr)
                    {
                        GeosApplication.Instance.SelectedCrmsectionsList.Add(GeosApplication.Instance.CrmsectionsList.FirstOrDefault(crm => crm.IdSection == Convert.ToInt16(item.ToString())));
                    }
                }

                if (GeosApplication.Instance.SelectedCrmsectionsList.Count == 0 || GeosApplication.Instance.SelectedCrmsectionsList[0] == null)
                {
                    SelectedCRMSections = new List<object>(GeosApplication.Instance.CrmsectionsList);
                }
                else
                {
                    SelectedCRMSections = new List<object>(GeosApplication.Instance.SelectedCrmsectionsList);
                }

                GeosApplication.Instance.Logger.Log("Method FillAllObjectsOneTime() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillSectionsDeatils() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for close Window.
        /// </summary>
        /// <param name="obj"></param>
        private void CloseWindow(object obj)
        {
            IsPreferenceChanged = false;
            RequestClose(null, null);
            
        }

       
        private void Opportunity_KeyDown(KeyEventArgs obj)
        { 
            GeosApplication.Instance.Logger.Log("Method Opportunity_KeyDown ...", category: Category.Info, priority: Priority.Low);
            try
            {
                string ShortcutKey = GetShortcutKey(obj);
                bool validate = ShortKeyValidate(ShortcutKey);
                if (validate == false)
                {
                    obj.Handled = false;
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ValidateShortcutKey").ToString(), ShortcutKey), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }
                obj.Handled = true;
                Opportunity = GetFirstCharCapital(ShortcutKey);
                GeosApplication.Instance.Logger.Log("Method Opportunity_KeyDown....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Opportunity_KeyDown...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void Contact_KeyDown(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Contact_KeyDown ...", category: Category.Info, priority: Priority.Low);
            try
            {
                string ShortcutKey = GetShortcutKey(obj);
                bool validate = ShortKeyValidate(ShortcutKey);
                if (validate == false)
                {
                    obj.Handled = false;
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ValidateShortcutKey").ToString(), ShortcutKey), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }
                obj.Handled = true;
                Contact = GetFirstCharCapital(ShortcutKey);
                GeosApplication.Instance.Logger.Log("Method Contact_KeyDown....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Contact_KeyDown...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void Account_KeyDown(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Account_KeyDown ...", category: Category.Info, priority: Priority.Low);
            try
            {
                string ShortcutKey = GetShortcutKey(obj);
                bool validate = ShortKeyValidate(ShortcutKey);
                if (validate == false)
                {
                    obj.Handled = false;
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ValidateShortcutKey").ToString(), ShortcutKey), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }
                obj.Handled = true;
                Account = GetFirstCharCapital(ShortcutKey);
                GeosApplication.Instance.Logger.Log("Method Account_KeyDown....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Account_KeyDown...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void Appointment_KeyDown(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Appointment_KeyDown ...", category: Category.Info, priority: Priority.Low);
            try
            {
                string ShortcutKey = GetShortcutKey(obj);
                bool validate = ShortKeyValidate(ShortcutKey);
                if (validate == false)
                {
                    obj.Handled = false;
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ValidateShortcutKey").ToString(), ShortcutKey), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }
                obj.Handled = true;
                Appointment = GetFirstCharCapital(ShortcutKey);
                GeosApplication.Instance.Logger.Log("Method Appointment_KeyDown....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Appointment_KeyDown...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void Task_KeyDown(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Task_KeyDown ...", category: Category.Info, priority: Priority.Low);
            try
            {
                string ShortcutKey = GetShortcutKey(obj);
                bool validate = ShortKeyValidate(ShortcutKey);
                if (validate == false)
                {
                    obj.Handled = false;
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ValidateShortcutKey").ToString(), ShortcutKey), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }
                obj.Handled = true;
                Task = GetFirstCharCapital(ShortcutKey);
                GeosApplication.Instance.Logger.Log("Method Task_KeyDown....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Task_KeyDown...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void Email_KeyDown(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Email_KeyDown ...", category: Category.Info, priority: Priority.Low);
            try
            {
                string ShortcutKey = GetShortcutKey(obj);
                bool validate = ShortKeyValidate(ShortcutKey);
                if (validate == false)
                {
                    obj.Handled = false;
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ValidateShortcutKey").ToString(), ShortcutKey), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }
                obj.Handled = true;
                Email = GetFirstCharCapital(ShortcutKey);
                GeosApplication.Instance.Logger.Log("Method Email_KeyDown....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Email_KeyDown...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void Call_KeyDown(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Call_KeyDown ...", category: Category.Info, priority: Priority.Low);
            try
            {
                string ShortcutKey = GetShortcutKey(obj);
                bool validate = ShortKeyValidate(ShortcutKey);
                if (validate == false)
                {
                    obj.Handled = false;
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ValidateShortcutKey").ToString(), ShortcutKey), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }
                obj.Handled = true;
                Call = GetFirstCharCapital(ShortcutKey);
                GeosApplication.Instance.Logger.Log("Method Call_KeyDown....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Call_KeyDown...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void Action_KeyDown(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Action_KeyDown ...", category: Category.Info, priority: Priority.Low);
            try
            {
                string ShortcutKey = GetShortcutKey(obj);
                bool validate = ShortKeyValidate(ShortcutKey);
                if (validate == false)
                {
                    obj.Handled = false;
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ValidateShortcutKey").ToString(), ShortcutKey), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }
                obj.Handled = true;
                Action= GetFirstCharCapital(ShortcutKey);
                GeosApplication.Instance.Logger.Log("Method Action_KeyDown....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Action_KeyDown...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void SearchOpportunityOrOrder_KeyDown(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method SearchOpportunityOrOrder_KeyDown ...", category: Category.Info, priority: Priority.Low);
            try
            {
                string ShortcutKey = GetShortcutKey(obj);
                bool validate = ShortKeyValidate(ShortcutKey);
                if(validate == false)
                {
                    obj.Handled = false;
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ValidateShortcutKey").ToString(),ShortcutKey), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }
                obj.Handled = true;
                SearchOpportunityOrOrder = GetFirstCharCapital(ShortcutKey);
                GeosApplication.Instance.Logger.Log("Method SearchOpportunityOrOrder_KeyDown....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SearchOpportunityOrOrder_KeyDown...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        
        private void MatrixList_KeyDown(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method MatrixList_KeyDown ...", category: Category.Info, priority: Priority.Low);
            try
            {
                string ShortcutKey = GetShortcutKey(obj);
                bool validate = ShortKeyValidate(ShortcutKey);
                if (validate == false)
                {
                    obj.Handled = false;
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ValidateShortcutKey").ToString(), ShortcutKey), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }
                obj.Handled = true;
                MatrixList = GetFirstCharCapital(ShortcutKey);
                GeosApplication.Instance.Logger.Log("Method MatrixList_KeyDown....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method MatrixList_KeyDown...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private string GetShortcutKey(KeyEventArgs obj)
        {
            string ShortcutKey = "";
            if (obj.KeyboardDevice.Modifiers == ModifierKeys.None)
            {
                ShortcutKey = obj.Key.ToString();
            }
            else
            {
                if ((obj.KeyboardDevice.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                {
                    ShortcutKey = "ctrl";
                }
                if ((obj.KeyboardDevice.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
                {
                    if (ShortcutKey != "")
                    {
                        ShortcutKey = ShortcutKey + " + shift";
                    }
                    else
                    {
                        ShortcutKey = "shift";
                    }
                }
                if ((obj.KeyboardDevice.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt)
                {
                    if (ShortcutKey != "")
                    {
                        ShortcutKey = ShortcutKey + " + alt";
                    }
                    else
                    {
                        ShortcutKey = "alt";
                    }
                }

                if ((obj.KeyboardDevice.Modifiers & ModifierKeys.Windows) == ModifierKeys.Windows)
                {
                    if (ShortcutKey != "")
                    {
                        ShortcutKey = ShortcutKey + " + windows";
                    }
                    else
                    {
                        ShortcutKey = "windows";
                    }
                }
                if (obj.Key == Key.System)
                {
                    if (obj.SystemKey.ToString().Contains("Left") || obj.SystemKey.ToString().Contains("Right"))
                    {
                        //checking
                    }
                    else
                    {
                        ShortcutKey = ShortcutKey + " + " + obj.SystemKey.ToString();
                    }
                }
                else
                {
                    if (obj.Key.ToString().Contains("Left") || obj.Key.ToString().Contains("Right"))
                    {
                        //checking
                    }
                    else
                    {
                        ShortcutKey = ShortcutKey + " + " + obj.Key.ToString();
                    }
                }
            }
            return ShortcutKey;
        }

        private string GetFirstCharCapital(string str)
        {
            if (str.Length == 1)
                return char.ToUpper(str[0]).ToString();
            else
                return char.ToUpper(str[0]) + str.Substring(1);
        }


        public bool ShortKeyValidate(string shortcutKey)
        {
            bool status = true;

            if(shortcutKey.ToUpper().Contains(Key.Escape.ToString().ToUpper()) ||
                shortcutKey.ToUpper().Contains(Key.LWin.ToString().ToUpper()) ||
                shortcutKey.ToUpper().Contains("VOLUME") ||
                shortcutKey.ToUpper().Contains("MEDIA") ||
                shortcutKey.ToUpper().Contains("SYSTEM") ||
                shortcutKey.ToUpper().Contains("OEM") ||
                shortcutKey.ToUpper().Contains("NUM") ||
                shortcutKey.ToUpper().Contains(Key.Subtract.ToString().ToUpper()) ||
                shortcutKey.ToUpper().Contains(Key.Multiply.ToString().ToUpper()) ||
                shortcutKey.ToUpper().Contains(Key.Divide.ToString().ToUpper()) ||
                shortcutKey.ToUpper().Contains(Key.Tab.ToString().ToUpper()) ||
                shortcutKey.ToUpper().Contains(Key.Add.ToString().ToUpper()) ||
                shortcutKey.ToUpper().Contains(Key.Return.ToString().ToUpper()) ||
                shortcutKey.ToUpper().Contains(Key.Decimal.ToString().ToUpper()) ||
                shortcutKey.ToUpper().Contains(Key.D1.ToString().ToUpper()) ||
                shortcutKey.ToUpper().Contains(Key.D2.ToString().ToUpper()) ||
                shortcutKey.ToUpper().Contains(Key.D3.ToString().ToUpper()) ||
                shortcutKey.ToUpper().Contains(Key.D4.ToString().ToUpper()) ||
                shortcutKey.ToUpper().Contains(Key.D5.ToString().ToUpper()) ||
                shortcutKey.ToUpper().Contains(Key.D6.ToString().ToUpper()) ||
                shortcutKey.ToUpper().Contains(Key.D7.ToString().ToUpper()) ||
                shortcutKey.ToUpper().Contains(Key.D8.ToString().ToUpper()) ||
                shortcutKey.ToUpper().Contains(Key.D9.ToString().ToUpper()) ||
                shortcutKey.ToUpper().Contains(Key.D0.ToString().ToUpper()))
            {
                status = false;
            }
            return status;
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion // Methods
    }

}
