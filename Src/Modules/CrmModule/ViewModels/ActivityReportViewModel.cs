using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Spreadsheet;
using DevExpress.Xpf.CodeView;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Spreadsheet;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.Utility;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Emdep.Geos.Modules.Crm.Views;
using System.IO;
using Emdep.Geos.UI.Validations;
using DevExpress.Xpf.Editors;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Emdep.Geos.Modules.Crm.ViewModels
{
    public class ActivityReportViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDataErrorInfo, IDisposable
    {
        #region Services

        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion // Services

        #region Declaration

        private DateTime fromDate;
        private DateTime toDate;
        private DateTime minDate;
        private DateTime maxDate;
        private List<UserManagerDtl> userManagerDtlList;
        private List<object> selectedUserManagerDtl;
        private bool isBusy;
        private int selectedIndexBusinessUnit;
        private int selectedIndexAccount;
        private int selectedIndexContact;

        private List<Activity> activityList;
        private Visibility salesOwnerVisibility;
        private ObservableCollection<Company> accountList; // { get; set; }
        private ObservableCollection<People> contactList;
        private string selectedsalesOwnersIds;
        protected ISaveFileDialogService SaveFileDialogService { get { return this.GetService<ISaveFileDialogService>(); } }
        private string visible;
        #endregion  // Declaration

        #region  public Properties
        public IList<LookupValue> TypeList { get; set; }
        public IList<LookupValue> BusinessUnitList { get; set; }
        public virtual string ResultFileName { get; protected set; }
        public virtual bool DialogResult { get; protected set; }


        public ObservableCollection<People> ContactList
        {
            get
            {
                return contactList;
            }

            set
            {
                contactList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ContactList"));
            }
        }

        public DateTime FromDate
        {
            get { return fromDate; }
            set
            {
                fromDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FromDate"));
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

        public DateTime ToDate
        {
            get { return toDate; }
            set
            {
                toDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ToDate"));
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

        public ObservableCollection<Company> AccountList
        {
            get { return accountList; }
            set
            {
                accountList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AccountList"));
            }
        }

        public List<UserManagerDtl> UserManagerDtlList
        {
            get
            {
                return userManagerDtlList;
            }

            set
            {
                userManagerDtlList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UserManagerDtlList"));
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

        public int SelectedIndexBusinessUnit
        {
            get
            {
                return selectedIndexBusinessUnit;
            }

            set
            {
                selectedIndexBusinessUnit = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexBusinessUnit"));
            }
        }

        public int SelectedIndexContact
        {
            get
            {
                return selectedIndexContact;
            }

            set
            {
                selectedIndexContact = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexContact"));
            }
        }
        public int SelectedIndexAccount
        {
            get
            {
                return selectedIndexAccount;
            }

            set
            {
                selectedIndexAccount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexAccount"));
                if (selectedIndexAccount > 0)
                    FillContactList();
                //if ( ContactList !=null && ContactList.Count > 0)
                //    ContactList = new ObservableCollection<People>(ContactList);


            }
        }

        public Visibility SalesOwnerVisibility
        {
            get
            {
                return salesOwnerVisibility;
            }

            set
            {
                salesOwnerVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SalesOwnerVisibility"));
            }
        }

        public List<Activity> ActivityList
        {
            get
            {
                return activityList;
            }

            set
            {
                activityList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ActivityList"));
            }
        }

        public List<object> SelectedUserManagerDtl
        {
            get
            {
                return selectedUserManagerDtl;
            }

            set
            {
                selectedUserManagerDtl = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedUserManagerDtl"));
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

        #endregion  //public Properties

        #region ICommands

        public ICommand ActivityReportAcceptButtonCommand { get; set; }
        public ICommand ActivityReportCancelButtonCommand { get; set; }
        public ICommand SalesOwnerPopupClosedCommand { get; set; }
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
                    me[BindableBase.GetPropertyName(() => SelectedUserManagerDtl)] +
                    me[BindableBase.GetPropertyName(() => FromDate)] +
                    me[BindableBase.GetPropertyName(() => ToDate)];

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
                string SelectedUserManagerDtlProp = BindableBase.GetPropertyName(() => SelectedUserManagerDtl);
                string FromDateProp = BindableBase.GetPropertyName(() => FromDate);
                string ToDateProp = BindableBase.GetPropertyName(() => ToDate);

                if (columnName == SelectedUserManagerDtlProp)
                    return ActivityReportValidation.GetErrorMessage(SelectedUserManagerDtlProp, SelectedUserManagerDtl);

                if (columnName == FromDateProp)
                {
                    //return ActivityReportValidation.GetErrorMessage(FromDateProp, FromDate);
                    int result = DateTime.Compare(ToDate.Date, FromDate.Date);

                    if (result < 0)
                        return "The date you entered occurs before the to date.";
                }

                if (columnName == ToDateProp)
                {
                    //return ActivityReportValidation.GetErrorMessage(ToDateProp, ToDate);
                    int result = DateTime.Compare(ToDate.Date, FromDate.Date);
                    if (result < 0)
                        return "The date you entered occurs before the from date.";
                }

                return null;
            }
        }

        #endregion

        #region  Constructor

        public ActivityReportViewModel()
        {
            //var fr = GeosApplication.Instance.SalesOwnerUsersList;


            SalesOwnerPopupClosedCommand = new DevExpress.Mvvm.DelegateCommand<object>(SalesOwnerPopupClosedCommandAction);
            ActivityReportAcceptButtonCommand = new RelayCommand(new Action<object>(ActivityReportAcceptAction));
            ActivityReportCancelButtonCommand = new RelayCommand(new Action<object>(ActivityReportCancelAction));
            CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);

            int year = GeosApplication.Instance.SelectedyearStarDate.Year;
            MinDate = new DateTime(year, 1, 1);
            MaxDate = new DateTime(year, 12, 31);

            FromDate = new DateTime(year, 1, 1);
            ToDate = new DateTime(year, 12, 31);

            if (GeosApplication.Instance.IdUserPermission == 21)
            {
                UserManagerDtlList = GeosApplication.Instance.SalesOwnerUsersList;
            }
            else if (GeosApplication.Instance.IdUserPermission == 22)
            {
                if (GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
                {
                    List<Company> plantOwners = GeosApplication.Instance.SelectedPlantOwnerUsersList.Cast<Company>().ToList();
                    string idPlants = string.Join(",", plantOwners.Select(i => i.ConnectPlantId));
                    UserManagerDtlList = new List<UserManagerDtl>();
                    UserManagerDtlList = CrmStartUp.GetSalesUserByPlant(idPlants);
                }
            }

            SalesOwnerVisibility = Visibility.Visible;
            FillBusinessUnitList();
            FillAccountList();
            TypeList = CrmStartUp.GetLookupValues(9);

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

        #endregion  // Constructor

        #region  Methods

        /// <summary>
        /// Method for fill account list.
        /// </summary>
        private void FillAccountList()
        {
            GeosApplication.Instance.Logger.Log("Method FillLinkedItems ...", category: Category.Info, priority: Priority.Low);

            try
            {
                if (GeosApplication.Instance.IdUserPermission == 21)
                {
                    if (GeosApplication.Instance.SelectedSalesOwnerUsersList != null)
                    {
                        List<UserManagerDtl> salesOwners = GeosApplication.Instance.SelectedSalesOwnerUsersList.Cast<UserManagerDtl>().ToList();
                        var salesOwnersIds = string.Join(",", salesOwners.Select(i => i.IdUser));

                        //Account
                        AccountList = new ObservableCollection<Company>();    // (CrmStartUp.GetSelectedSalesOwnerSites(salesOwnersIds).ToList());
                    }
                }
                else if (GeosApplication.Instance.IdUserPermission == 22)
                {
                    //Account
                    List<UserManagerDtl> salesOwners = GeosApplication.Instance.SelectedSalesOwnerUsersList.Cast<UserManagerDtl>().ToList();
                    var salesOwnersIds = string.Join(",", salesOwners.Select(i => i.IdUser));
                    AccountList = new ObservableCollection<Company>();        //CrmStartUp.GetSelectedSalesOwnerSites(salesOwnersIds).OrderBy(x => x.GroupPlantName).ToList());
                }
                else
                {
                    //Account
                    //CompaniesList = CrmStartUp.GetCustomersBySalesOwnerId(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission);
                    AccountList = new ObservableCollection<Company>(CrmStartUp.GetSelectedSalesOwnerSites(GeosApplication.Instance.ActiveUser.IdUser.ToString()).OrderBy(x => x.GroupPlantName).ToList());
                    AccountList.Insert(0, new Company() { GroupPlantName = "---", IdCompany = 0 });
                    SalesOwnerVisibility = Visibility.Collapsed;
                }

                SelectedIndexAccount = 0;

                GeosApplication.Instance.Logger.Log("Method FillLinkedItems() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillLinkedItems() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillLinkedItems() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillLinkedItems() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for fill Contact list.
        /// </summary>
        private void FillContactList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillContactList ...", category: Category.Info, priority: Priority.Low);

                short IdSite = (Int16)AccountList[SelectedIndexAccount].IdCompany;
                ObservableCollection<People> TempContactList = new ObservableCollection<People>(CrmStartUp.GetContactsOfSiteByOfferId(IdSite).AsEnumerable().ToList());

                TempContactList.Insert(0, new People() { Name = "---", FullName = "---", IdPerson = 0 });
                ContactList = new ObservableCollection<People>(TempContactList);

                SelectedIndexContact = 0;


                GeosApplication.Instance.Logger.Log("Method FillContactList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillContactList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillContactList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillContactList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for fill BusinessUnit list.
        /// </summary>
        private void FillBusinessUnitList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillBusinessUnitList ...", category: Category.Info, priority: Priority.Low);

                IList<LookupValue> tempBusinessUnitList = CrmStartUp.GetLookupvaluesWithoutRestrictedBU(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.IdUserPermission);
                //CrmStartUp.GetLookupValues(2);
                BusinessUnitList = new List<LookupValue>();
                BusinessUnitList.Insert(0, new LookupValue() { Value = "---" ,InUse=true});
                BusinessUnitList.AddRange(tempBusinessUnitList);
                GeosApplication.Instance.Logger.Log("Method FillBusinessUnitList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillBusinessUnitList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillBusinessUnitList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillBusinessUnitList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void SalesOwnerPopupClosedCommandAction(object obj)
        {
            ClosePopupEventArgs closePopupEventArgs = (ClosePopupEventArgs)obj;

            if (closePopupEventArgs.CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
            {
                return;
            }

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

            try
            {
                if (closePopupEventArgs.EditValue != null)
                {
                    List<UserManagerDtl> salesOwners = ((List<object>)closePopupEventArgs.EditValue).Cast<UserManagerDtl>().ToList();

                    //((List<UserManagerDtl>)closePopupEventArgs.EditValue).ToList();
                    //GeosApplication.Instance.SelectedSalesOwnerUsersList.Cast<UserManagerDtl>().ToList();
                    var salesOwnersIds = string.Join(",", salesOwners.Select(i => i.IdUser));
                    selectedsalesOwnersIds = salesOwnersIds;
                    ObservableCollection<Company> TempAccountList = new ObservableCollection<Company>();
                    TempAccountList = new ObservableCollection<Company>(CrmStartUp.GetSelectedSalesOwnerSites(salesOwnersIds).OrderBy(x => x.GroupPlantName).ToList());
                    TempAccountList.Insert(0, new Company() { GroupPlantName = "---", IdCompany = 0 });
                    AccountList = new ObservableCollection<Company>(TempAccountList);
                    SelectedIndexAccount = 0;
                }
                else
                {
                    AccountList = new ObservableCollection<Company>();
                }

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
            }
            catch (FaultException<ServiceException> ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillLinkedItems() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillLinkedItems() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillLinkedItems() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void ActivityReportAcceptAction(object obj)
        {
            try
            {
                string error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedUserManagerDtl"));
                //PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexBusinessUnit"));
                //PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexAccount"));
                PropertyChanged(this, new PropertyChangedEventArgs("FromDate"));
                PropertyChanged(this, new PropertyChangedEventArgs("ToDate"));

                if (error != null)
                {
                    IsBusy = false;
                    return;
                }

                GeosApplication.Instance.Logger.Log("Method ExportReleaseDetailsButtonCommandAction ...", category: Category.Info, priority: Priority.Low);
                SaveFileDialogService.DefaultExt = "xlsx";
                SaveFileDialogService.DefaultFileName = "ActivityReport";
                SaveFileDialogService.Filter = "Excel Files (.xlsx)|*.xlsx|All Files (*.*)|*.*";
                SaveFileDialogService.FilterIndex = 1;
                DialogResult = SaveFileDialogService.ShowDialog();

                if (!DialogResult)
                {
                    ResultFileName = string.Empty;
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

                    ResultFileName = (SaveFileDialogService.File).DirectoryName + "\\" + (SaveFileDialogService.File).Name;

                    List<User> listUserId = new List<User>(); ;
                    Workbook workbook = new Workbook();
                    List<Offer> offers;

                    if (GeosApplication.Instance.IdUserPermission == 20)
                    {
                        listUserId.Add(GeosApplication.Instance.ActiveUser);
                    }

                    if (GeosApplication.Instance.IdUserPermission == 21 || GeosApplication.Instance.IdUserPermission == 22)
                    {
                        List<UserManagerDtl> finalUserManagerDtlLista = SelectedUserManagerDtl.Cast<UserManagerDtl>().ToList();

                        foreach (UserManagerDtl item in finalUserManagerDtlLista)
                        {
                            listUserId.Add(item.User);
                        }
                    }

                    if (listUserId.Count > 0)
                    {
                        int index = 0;

                        foreach (User item in listUserId)
                        {
                            workbook.Worksheets.Insert(index, item.FullName);
                            index++;
                        }

                        string strIdcontacts = "0";
                        if (ContactList != null && selectedIndexContact > -1)
                            strIdcontacts = ContactList[selectedIndexContact].IdPerson.ToString();

                        foreach (User item in listUserId)
                        {
                            // old function without contact. - ActivityList = CrmStartUp.GetActivityReportDetails(item.IdUser.ToString(), FromDate, ToDate, AccountList[SelectedIndexAccount].IdCompany.ToString());
                            //ActivityList = CrmStartUp.GetActivityReportDetails(item.IdUser.ToString(), FromDate, ToDate, AccountList[SelectedIndexAccount].IdCompany.ToString());
                            ActivityList = CrmStartUp.GetAllActivityReportDetails(item.IdUser.ToString(), FromDate, ToDate, AccountList[SelectedIndexAccount].IdCompany.ToString(), strIdcontacts);
                            //GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate
                            offers = new List<Offer>();
                            foreach (var itemCompaniesDetails in GeosApplication.Instance.CompanyList)
                            {
                                try
                                {
                                    GeosApplication.Instance.SplashScreenMessage = "Connecting to " + itemCompaniesDetails.Alias;

                                    List<Offer> offlst = new List<Offer>();
                                    offlst = CrmStartUp.GetAllOfferReportDetails_V2035(item.IdUser.ToString(), FromDate, ToDate, BusinessUnitList[SelectedIndexBusinessUnit].IdLookupValue.ToString(), AccountList[SelectedIndexAccount].IdCompany.ToString(), GeosApplication.Instance.IdCurrencyByRegion, itemCompaniesDetails, strIdcontacts);
                                    //offlst = CrmStartUp.GetOfferReportDetails(item.IdUser.ToString(), FromDate, ToDate, BusinessUnitList[SelectedIndexBusinessUnit].IdLookupValue.ToString(), AccountList[SelectedIndexAccount].IdCompany.ToString(), GeosApplication.Instance.IdCurrencyByRegion, itemCompaniesDetails);

                                    //old function without contact. - offlst = CrmStartUp.GetOfferReportDetails(item.IdUser.ToString(), FromDate, ToDate, BusinessUnitList[SelectedIndexBusinessUnit].IdLookupValue.ToString(), AccountList[SelectedIndexAccount].IdCompany.ToString(), GeosApplication.Instance.IdCurrencyByRegion, itemCompaniesDetails);
                                    //offlst = CrmStartUp.GetOfferReportDetails(item.IdUser.ToString(), FromDate, ToDate, "", "", GeosApplication.Instance.IdCurrencyByRegion, itemCompaniesDetails);
                                    //GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate
                                    offers.AddRange(offlst);
                                }
                                catch (Exception ex)
                                {
                                    GeosApplication.Instance.SplashScreenMessage = string.Empty;
                                    GeosApplication.Instance.Logger.Log("Get an error in ExportReleaseDetailsButtonCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                                }
                            }

                            GeosApplication.Instance.SplashScreenMessage = string.Empty;

                            //SendMailActivity(ActivityList, offers,"");
                            //AutomaticReportMailSendToSalesPerson();
                            CreateActivityReport(item, ActivityList, offers, workbook);
                        }
                    }

                    using (FileStream stream = new FileStream(ResultFileName, FileMode.Create, FileAccess.ReadWrite))
                    {
                        workbook.SaveDocument(stream, DocumentFormat.OpenXml);
                    }
                    //   control.SaveDocument(ResultFileName);

                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    CustomMessageBox.Show(string.Format(string.Format(System.Windows.Application.Current.FindResource("ActivityReportExportSuccessfull").ToString())), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    System.Diagnostics.Process.Start(ResultFileName);

                    RequestClose(null, null);
                }

                GeosApplication.Instance.Logger.Log("Method ExportReleaseDetailsButtonCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in ExportReleaseDetailsButtonCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// method for fill worksheet for user as per activity and offers. 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="ActivityList"></param>
        /// <param name="userOffersList"></param>
        /// <param name="workbook"></param>
        private void CreateActivityReport(User user, List<Activity> ActivityList, List<Offer> userOffersList, Workbook workbook)
        {
            List<Offer> offersCreated = new List<Offer>();
            List<Offer> offersWon = new List<Offer>();
            List<Offer> offersLost = new List<Offer>();

            Worksheet ws = workbook.Worksheets[user.FullName];

            if (userOffersList.Count > 0)
            {
                offersCreated = userOffersList.Where(of => of.ReportGroup == "OfferCreated").Select(i => i).ToList();
                offersWon = userOffersList.Where(of => of.ReportGroup == "OfferWon").Select(i => i).ToList();
                offersLost = userOffersList.Where(of => of.ReportGroup == "OfferLost").Select(i => i).ToList();
            }

            //ws.Name = salesName;
            ws.Cells[0, 2].Value = "Activity Report";
            ws.Cells[0, 0].ColumnWidth = 400;

            ws.Cells[2, 0].Value = "Sales";
            ws.Cells[2, 0].Font.Bold = true;

            ws.Cells[2, 1].Value = user.FullName;
            ws.Cells[2, 1].Font.Bold = true;
            ws.Cells[2, 1].ColumnWidth = 400;

            ws.Cells[3, 0].Value = "From";
            ws.Cells[3, 0].Font.Bold = true;

            ws.Cells[3, 1].Value = FromDate;    // GeosApplication.Instance.SelectedyearStarDate;
            ws.Cells[3, 1].ColumnWidth = 400;

            ws.Cells[4, 0].Value = "To";
            ws.Cells[4, 0].Font.Bold = true;

            ws.Cells[4, 1].Value = ToDate;      // GeosApplication.Instance.SelectedyearEndDate;
            ws.Cells[4, 1].ColumnWidth = 400;

            ws.Cells[5, 0].Value = "Business Unit";
            ws.Cells[5, 0].Font.Bold = true;
            ws.Cells[5, 1].Value = BusinessUnitList[SelectedIndexBusinessUnit].Value;
            ws.Cells[5, 1].ColumnWidth = 1000;

            ws.Range["A1:G1"].Font.Bold = true;
            ws.Range["A1:G1"].Fill.BackgroundColor = System.Drawing.Color.LightGray;

            int counter = 8;
            foreach (var item1 in TypeList)
            {
                List<Activity> TempActivityList = ActivityList.Where(x => x.IdActivityType == item1.IdLookupValue).ToList();

                // for fill activity.
                if (TempActivityList.Count > 0)
                {
                    ws.Cells[counter, 0].Value = item1.Value;
                    ws.Cells[counter, 0].Font.Bold = true;
                    ws.Cells[counter, 2].Value = TempActivityList.Count;
                    ws.Cells[counter, 2].Font.Bold = true;
                    int rangnumber = counter;

                    counter++;
                    ws.Cells[counter, 0].Value = "Title";
                    ws.Cells[counter, 0].Font.Bold = true;
                    // for set border on cell.
                    ws.Cells[counter, 0].Borders.SetAllBorders(System.Drawing.Color.Black, DevExpress.Spreadsheet.BorderLineStyle.Thin);

                    ws.Cells[counter, 1].Value = "Description";
                    ws.Cells[counter, 1].Font.Bold = true;
                    ws.Cells[counter, 1].Borders.SetAllBorders(System.Drawing.Color.Black, DevExpress.Spreadsheet.BorderLineStyle.Thin);

                    ws.Cells[counter, 2].Value = "Date";
                    ws.Cells[counter, 2].ColumnWidth = 400;
                    ws.Cells[counter, 2].Font.Bold = true;
                    ws.Cells[counter, 2].Borders.SetAllBorders(System.Drawing.Color.Black, DevExpress.Spreadsheet.BorderLineStyle.Thin);
                    ws.Cells[counter, 3].ColumnWidth = 300;

                    if (item1.IdLookupValue == 40)
                    {
                        ws.Cells[counter, 3].Value = "Status";
                        ws.Cells[counter, 3].Font.Bold = true;
                        ws.Cells[counter, 3].Borders.SetAllBorders(System.Drawing.Color.Black, DevExpress.Spreadsheet.BorderLineStyle.Thin);
                        ws.Cells[rangnumber, 3].Value = TempActivityList.Count;
                        ws.Cells[rangnumber, 2].Value = string.Empty;

                        //for header backgroung.
                        ws.Range["A" + (rangnumber + 1).ToString() + ":D" + (rangnumber + 1).ToString()].Fill.BackgroundColor = System.Drawing.Color.LightGray;
                    }
                    else
                    {
                        //for header backgroung.
                        ws.Range["A" + (rangnumber + 1).ToString() + ":C" + (rangnumber + 1).ToString()].Fill.BackgroundColor = System.Drawing.Color.LightGray;
                    }

                    foreach (Activity activity in TempActivityList)
                    {
                        counter++;

                        ws.Cells[counter, 0].Value = activity.Subject;
                        ws.Cells[counter, 0].Borders.SetAllBorders(System.Drawing.Color.Black, DevExpress.Spreadsheet.BorderLineStyle.Thin);

                        ws.Cells[counter, 1].Value = activity.Description;
                        ws.Cells[counter, 1].Borders.SetAllBorders(System.Drawing.Color.Black, DevExpress.Spreadsheet.BorderLineStyle.Thin);

                        ws.Cells[counter, 2].Value = activity.ToDate;
                        ws.Cells[counter, 2].Borders.SetAllBorders(System.Drawing.Color.Black, DevExpress.Spreadsheet.BorderLineStyle.Thin);

                        if (item1.IdLookupValue == 40)
                        {
                            ws.Cells[counter, 3].Value = activity.ActivityStatus.Value;
                            ws.Cells[counter, 3].Borders.SetAllBorders(System.Drawing.Color.Black, DevExpress.Spreadsheet.BorderLineStyle.Thin);
                        }
                    }

                    counter = counter + 2;
                }
            }

            //For Created offers.
            if (offersCreated.Count > 0)
            {
                int offerCreateRowNumver = counter + 1;
                ws.Range["A" + offerCreateRowNumver.ToString() + ":G" + offerCreateRowNumver.ToString()].Fill.BackgroundColor = System.Drawing.Color.LightGray;

                ws.Cells[counter, 0].Value = "Offer Created";
                ws.Cells[counter, 0].Font.Bold = true;

                ws.Cells[counter, 4].ColumnWidth = 430;

                ws.Cells[counter, 5].Value = offersCreated.Count;
                ws.Cells[counter, 5].Font.Bold = true;
                ws.Cells[counter, 5].ColumnWidth = 400;

                ws.Cells[counter, 6].Value = offersCreated.Sum(x => x.Value);
                ws.Cells[counter, 6].NumberFormat = GeosApplication.Instance.CurrentCurrencySymbol + "#,##0.00";
                ws.Cells[counter, 6].Font.Bold = true;
                ws.Cells[counter, 6].ColumnWidth = 400;

                counter++;
                ws.Cells[counter, 0].Value = "Code";
                ws.Cells[counter, 0].Font.Bold = true;
                ws.Cells[counter, 0].Borders.SetAllBorders(System.Drawing.Color.Black, DevExpress.Spreadsheet.BorderLineStyle.Thin);

                ws.Cells[counter, 1].Value = "Description";
                ws.Cells[counter, 1].Font.Bold = true;
                ws.Cells[counter, 1].Borders.SetAllBorders(System.Drawing.Color.Black, DevExpress.Spreadsheet.BorderLineStyle.Thin);

                ws.Cells[counter, 2].Value = "Group";
                ws.Cells[counter, 2].Font.Bold = true;
                ws.Cells[counter, 2].Borders.SetAllBorders(System.Drawing.Color.Black, DevExpress.Spreadsheet.BorderLineStyle.Thin);

                ws.Cells[counter, 3].Value = "Plant";
                ws.Cells[counter, 3].Font.Bold = true;
                ws.Cells[counter, 3].Borders.SetAllBorders(System.Drawing.Color.Black, DevExpress.Spreadsheet.BorderLineStyle.Thin);

                ws.Cells[counter, 4].Value = "Confidence Level(%)";
                ws.Cells[counter, 4].Font.Bold = true;
                ws.Cells[counter, 4].Borders.SetAllBorders(System.Drawing.Color.Black, DevExpress.Spreadsheet.BorderLineStyle.Thin);

                ws.Cells[counter, 5].Value = "Date";
                ws.Cells[counter, 5].Font.Bold = true;
                ws.Cells[counter, 5].Borders.SetAllBorders(System.Drawing.Color.Black, DevExpress.Spreadsheet.BorderLineStyle.Thin);

                ws.Cells[counter, 6].Value = "Amount";
                ws.Cells[counter, 6].Font.Bold = true;
                ws.Cells[counter, 6].Borders.SetAllBorders(System.Drawing.Color.Black, DevExpress.Spreadsheet.BorderLineStyle.Thin);

                foreach (var item in offersCreated)
                {
                    counter++;
                    ws.Cells[counter, 0].Value = item.Code;
                    ws.Cells[counter, 0].Borders.SetAllBorders(System.Drawing.Color.Black, DevExpress.Spreadsheet.BorderLineStyle.Thin);

                    ws.Cells[counter, 1].Value = item.Title;
                    ws.Cells[counter, 1].Borders.SetAllBorders(System.Drawing.Color.Black, DevExpress.Spreadsheet.BorderLineStyle.Thin);

                    ws.Cells[counter, 2].Value = item.Site.Customers[0].CustomerName;
                    ws.Cells[counter, 2].Borders.SetAllBorders(System.Drawing.Color.Black, DevExpress.Spreadsheet.BorderLineStyle.Thin);

                    ws.Cells[counter, 3].Value = item.Site.Name;
                    ws.Cells[counter, 3].Borders.SetAllBorders(System.Drawing.Color.Black, DevExpress.Spreadsheet.BorderLineStyle.Thin);

                    ws.Cells[counter, 4].Value = item.ProbabilityOfSuccess;
                    ws.Cells[counter, 4].Borders.SetAllBorders(System.Drawing.Color.Black, DevExpress.Spreadsheet.BorderLineStyle.Thin);

                    ws.Cells[counter, 5].Value = item.DeliveryDate;
                    ws.Cells[counter, 5].Borders.SetAllBorders(System.Drawing.Color.Black, DevExpress.Spreadsheet.BorderLineStyle.Thin);

                    ws.Cells[counter, 6].Value = item.Value;
                    ws.Cells[counter, 6].Borders.SetAllBorders(System.Drawing.Color.Black, DevExpress.Spreadsheet.BorderLineStyle.Thin);
                    ws.Cells[counter, 6].NumberFormat = GeosApplication.Instance.CurrentCurrencySymbol + "#,##0.00";
                }

                counter = counter + 2;
            }

            //For Won offers.
            if (offersWon.Count > 0)
            {
                int offersWonRowNumver = counter + 1;
                ws.Range["A" + offersWonRowNumver.ToString() + ":G" + offersWonRowNumver.ToString()].Fill.BackgroundColor = System.Drawing.Color.LightGray;

                ws.Cells[counter, 0].Value = "Offer Won";
                ws.Cells[counter, 0].Font.Bold = true;

                ws.Cells[counter, 5].Value = offersWon.Count;
                ws.Cells[counter, 5].Font.Bold = true;
                ws.Cells[counter, 5].ColumnWidth = 400;

                ws.Cells[counter, 6].Value = offersWon.Sum(x => x.Value);
                ws.Cells[counter, 6].NumberFormat = GeosApplication.Instance.CurrentCurrencySymbol + "#,##0.00";
                ws.Cells[counter, 6].Font.Bold = true;
                ws.Cells[counter, 6].ColumnWidth = 400;

                counter++;
                ws.Cells[counter, 0].Value = "Code";
                ws.Cells[counter, 0].Font.Bold = true;
                ws.Cells[counter, 0].Borders.SetAllBorders(System.Drawing.Color.Black, DevExpress.Spreadsheet.BorderLineStyle.Thin);

                ws.Cells[counter, 1].Value = "Description";
                ws.Cells[counter, 1].Font.Bold = true;
                ws.Cells[counter, 1].Borders.SetAllBorders(System.Drawing.Color.Black, DevExpress.Spreadsheet.BorderLineStyle.Thin);

                ws.Cells[counter, 2].Value = "Group";
                ws.Cells[counter, 2].Font.Bold = true;
                ws.Cells[counter, 2].Borders.SetAllBorders(System.Drawing.Color.Black, DevExpress.Spreadsheet.BorderLineStyle.Thin);

                ws.Cells[counter, 3].Value = "Plant";
                ws.Cells[counter, 3].Font.Bold = true;
                ws.Cells[counter, 3].Borders.SetAllBorders(System.Drawing.Color.Black, DevExpress.Spreadsheet.BorderLineStyle.Thin);

                ws.Cells[counter, 4].Value = "Confidence Level(%)";
                ws.Cells[counter, 4].Font.Bold = true;
                ws.Cells[counter, 4].Borders.SetAllBorders(System.Drawing.Color.Black, DevExpress.Spreadsheet.BorderLineStyle.Thin);

                ws.Cells[counter, 5].Value = "Date";
                ws.Cells[counter, 5].Font.Bold = true;
                ws.Cells[counter, 5].Borders.SetAllBorders(System.Drawing.Color.Black, DevExpress.Spreadsheet.BorderLineStyle.Thin);

                ws.Cells[counter, 6].Value = "Amount";
                ws.Cells[counter, 6].Font.Bold = true;
                ws.Cells[counter, 6].Borders.SetAllBorders(System.Drawing.Color.Black, DevExpress.Spreadsheet.BorderLineStyle.Thin);

                foreach (var item in offersWon)
                {
                    counter++;
                    ws.Cells[counter, 0].Value = item.Code;
                    ws.Cells[counter, 0].Borders.SetAllBorders(System.Drawing.Color.Black, DevExpress.Spreadsheet.BorderLineStyle.Thin);

                    ws.Cells[counter, 1].Value = item.Title;
                    ws.Cells[counter, 1].Borders.SetAllBorders(System.Drawing.Color.Black, DevExpress.Spreadsheet.BorderLineStyle.Thin);

                    ws.Cells[counter, 2].Value = item.Site.Customers[0].CustomerName;
                    ws.Cells[counter, 2].Borders.SetAllBorders(System.Drawing.Color.Black, DevExpress.Spreadsheet.BorderLineStyle.Thin);

                    ws.Cells[counter, 3].Value = item.Site.Name;
                    ws.Cells[counter, 3].Borders.SetAllBorders(System.Drawing.Color.Black, DevExpress.Spreadsheet.BorderLineStyle.Thin);

                    ws.Cells[counter, 4].Value = item.ProbabilityOfSuccess;
                    ws.Cells[counter, 4].Borders.SetAllBorders(System.Drawing.Color.Black, DevExpress.Spreadsheet.BorderLineStyle.Thin);

                    ws.Cells[counter, 5].Value = item.DeliveryDate;
                    ws.Cells[counter, 5].Borders.SetAllBorders(System.Drawing.Color.Black, DevExpress.Spreadsheet.BorderLineStyle.Thin);

                    ws.Cells[counter, 6].Value = item.Value;
                    ws.Cells[counter, 6].Borders.SetAllBorders(System.Drawing.Color.Black, DevExpress.Spreadsheet.BorderLineStyle.Thin);
                    ws.Cells[counter, 6].NumberFormat = GeosApplication.Instance.CurrentCurrencySymbol + "#,##0.00";
                }

                counter = counter + 2;
            }

            //For Lost offers.
            if (offersLost.Count > 0)
            {
                int offersLostRowNumver = counter + 1;
                ws.Range["A" + offersLostRowNumver.ToString() + ":G" + offersLostRowNumver.ToString()].Fill.BackgroundColor = System.Drawing.Color.LightGray;

                ws.Cells[counter, 0].Value = "Offer Lost";
                ws.Cells[counter, 0].Font.Bold = true;

                ws.Cells[counter, 5].Value = offersLost.Count;
                ws.Cells[counter, 5].Font.Bold = true;
                ws.Cells[counter, 5].ColumnWidth = 400;

                ws.Cells[counter, 6].Value = offersLost.Sum(x => x.Value);
                ws.Cells[counter, 6].NumberFormat = GeosApplication.Instance.CurrentCurrencySymbol + "#,##0.00";
                ws.Cells[counter, 6].Font.Bold = true;
                ws.Cells[counter, 6].ColumnWidth = 400;

                counter++;
                ws.Cells[counter, 0].Value = "Code";
                ws.Cells[counter, 0].Font.Bold = true;
                ws.Cells[counter, 0].Borders.SetAllBorders(System.Drawing.Color.Black, DevExpress.Spreadsheet.BorderLineStyle.Thin);

                ws.Cells[counter, 1].Value = "Description";
                ws.Cells[counter, 1].Font.Bold = true;
                ws.Cells[counter, 1].Borders.SetAllBorders(System.Drawing.Color.Black, DevExpress.Spreadsheet.BorderLineStyle.Thin);

                ws.Cells[counter, 2].Value = "Group";
                ws.Cells[counter, 2].Font.Bold = true;
                ws.Cells[counter, 2].Borders.SetAllBorders(System.Drawing.Color.Black, DevExpress.Spreadsheet.BorderLineStyle.Thin);

                ws.Cells[counter, 3].Value = "Plant";
                ws.Cells[counter, 3].Font.Bold = true;
                ws.Cells[counter, 3].Borders.SetAllBorders(System.Drawing.Color.Black, DevExpress.Spreadsheet.BorderLineStyle.Thin);

                ws.Cells[counter, 4].Value = "Confidence Level(%)";
                ws.Cells[counter, 4].Font.Bold = true;
                ws.Cells[counter, 4].Borders.SetAllBorders(System.Drawing.Color.Black, DevExpress.Spreadsheet.BorderLineStyle.Thin);

                ws.Cells[counter, 5].Value = "Date";
                ws.Cells[counter, 5].Font.Bold = true;
                ws.Cells[counter, 5].Borders.SetAllBorders(System.Drawing.Color.Black, DevExpress.Spreadsheet.BorderLineStyle.Thin);

                ws.Cells[counter, 6].Value = "Amount";
                ws.Cells[counter, 6].Font.Bold = true;
                ws.Cells[counter, 6].Borders.SetAllBorders(System.Drawing.Color.Black, DevExpress.Spreadsheet.BorderLineStyle.Thin);

                foreach (var item in offersLost)
                {
                    counter++;
                    ws.Cells[counter, 0].Value = item.Code;
                    ws.Cells[counter, 0].Borders.SetAllBorders(System.Drawing.Color.Black, DevExpress.Spreadsheet.BorderLineStyle.Thin);

                    ws.Cells[counter, 1].Value = item.Title;
                    ws.Cells[counter, 1].Borders.SetAllBorders(System.Drawing.Color.Black, DevExpress.Spreadsheet.BorderLineStyle.Thin);

                    ws.Cells[counter, 2].Value = item.Site.Customers[0].CustomerName;
                    ws.Cells[counter, 2].Borders.SetAllBorders(System.Drawing.Color.Black, DevExpress.Spreadsheet.BorderLineStyle.Thin);

                    ws.Cells[counter, 3].Value = item.Site.Name;
                    ws.Cells[counter, 3].Borders.SetAllBorders(System.Drawing.Color.Black, DevExpress.Spreadsheet.BorderLineStyle.Thin);

                    ws.Cells[counter, 4].Value = item.ProbabilityOfSuccess;
                    ws.Cells[counter, 4].Borders.SetAllBorders(System.Drawing.Color.Black, DevExpress.Spreadsheet.BorderLineStyle.Thin);

                    ws.Cells[counter, 5].Value = item.DeliveryDate;
                    ws.Cells[counter, 5].Borders.SetAllBorders(System.Drawing.Color.Black, DevExpress.Spreadsheet.BorderLineStyle.Thin);

                    ws.Cells[counter, 6].Value = item.Value;
                    ws.Cells[counter, 6].Borders.SetAllBorders(System.Drawing.Color.Black, DevExpress.Spreadsheet.BorderLineStyle.Thin);
                    ws.Cells[counter, 6].NumberFormat = GeosApplication.Instance.CurrentCurrencySymbol + "#,##0.00";
                }

                counter = counter + 2;
            }
        }

        public void ActivityReportCancelAction(object obj)
        {
            RequestClose(null, null);
        }

        public void Dispose()
        {
        }

        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        ///// <summary>
        ///// Method for send mail Automaticaly on perticular interval. 
        ///// </summary>
        //private void AutomaticReportMailSendToSalesPerson()
        //{

        //    try
        //    {


        //        List<AutomaticReport> automaticReportLst = WorkbenchStartUp.GetAutomaticReports();

        //        foreach (AutomaticReport automaticReport in automaticReportLst)
        //        {
        //            if (automaticReport.IsEnabled == 1 && automaticReport.StartDate.Date.Equals(DateTime.Now.Date))
        //            {

        //                List<SalesUser> salesUserLst = CrmStartUp.GetAllSalesUsersForReport();

        //                if (salesUserLst != null && salesUserLst.Count > 0)
        //                {

        //                    List<Activity> activityList = CrmStartUp.GetActivitiesGoingToDueInInverval(automaticReport.Interval);

        //                    List<Offer> offers;

        //                    foreach (SalesUser item in salesUserLst)
        //                    {
        //                        if (item.IdSalesUser == 28)
        //                        {

        //                        }

        //                        List<Activity> usersActivity = activityList.Where(ac => ac.IdOwner == item.IdSalesUser).Select(act => act).ToList();
        //                        offers = new List<Offer>();

        //                        List<Company> CompanyList = CrmStartUp.GetAllCompaniesDetails(item.IdSalesUser);

        //                        foreach (var itemCompaniesDetails in CompanyList)
        //                        {
        //                            try
        //                            {
        //                                List<Offer> offlst = new List<Offer>();

        //                                offlst = CrmStartUp.GetReportOffersPerSalesUser(1, item.IdSalesUser, 1, 2017, itemCompaniesDetails, automaticReport.Interval);
        //                                offers.AddRange(offlst);
        //                            }
        //                            catch (Exception ex)
        //                            {
        //                                GeosApplication.Instance.Logger.Log("Get an error in SendMailActivity() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
        //                            }
        //                        }

        //                        if (usersActivity.Count > 0 || offers.Count > 0)
        //                            SendMailActivity(usersActivity, offers, automaticReport);
        //                    }
        //                }

        //                WorkbenchStartUp.UpdateAutomaticReport(automaticReport);
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}


        ///// <summary>
        ///// Method for send mail Automaticaly. 
        ///// </summary>
        //private void SendMailActivity(List<Activity> ActivityList, List<Offer> userOffersList, AutomaticReport automaticReport)
        //{
        //    try
        //    {
        //        // DateTime currentDateTime = WorkbenchStartUp.GetServerDateTime();
        //        string reportSubject = "";
        //        DateTime currentDateTime = WorkbenchStartUp.GetServerDateTime();
        //        CultureInfo ciCurr = CultureInfo.CurrentCulture;

        //        //[Start] Set Mail Sublect.
        //        if (automaticReport.Interval.Equals("daily"))
        //        {
        //            reportSubject = "[CRM] " + automaticReport.Name + "_" + automaticReport.StartDate.ToString("dd / MM / yyyy");
        //        }
        //        else if (automaticReport.Interval.Equals("weekly"))
        //        {
        //            int weekNum = ciCurr.Calendar.GetWeekOfYear(automaticReport.StartDate, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        //            reportSubject = "[CRM] " + automaticReport.Name + "_" + automaticReport.StartDate.Year + "_W" + weekNum;
        //        }
        //        else if (automaticReport.Interval.Equals("monthly"))
        //        {
        //            reportSubject = "[CRM] " + automaticReport.Name + "_" + automaticReport.StartDate.Year + "_" + automaticReport.StartDate.ToString("MMMM");
        //        }

        //        else if (automaticReport.Interval.Equals("quarterly"))
        //        {
        //            reportSubject = "[CRM] " + automaticReport.Name + "_" + automaticReport.StartDate.ToString("yyyy") + "_Q" + Math.Ceiling(automaticReport.StartDate.Month / 3.0) + "";
        //        }
        //        else if (automaticReport.Interval.Equals("yearly"))
        //        {
        //            reportSubject = "[CRM] " + automaticReport.Name + "_" + automaticReport.StartDate.Year;
        //        }

        //        //[End] Set Mail Sublect.


        //        List<Offer> offersWaitingforQuoteLst = new List<Offer>();
        //        List<Offer> offersCloseDateOverdueLst = new List<Offer>();
        //        List<Offer> offersUnactivityListFinal = new List<Offer>();

        //        if (userOffersList.Count > 0)
        //        {
        //            offersWaitingforQuoteLst = userOffersList.Where(of => of.GeosStatus.IdOfferStatusType == 2).Select(i => i).ToList();
        //            offersCloseDateOverdueLst = userOffersList.Where(of => of.DeliveryDate < currentDateTime).Select(i => i).ToList();
        //            List<Offer> offersUnactivityLst = new List<Offer>();
        //            offersUnactivityLst = userOffersList.Where(of => of.LastActivityDate.HasValue && of.LastActivityDate < currentDateTime).Select(i => i).ToList();

        //            foreach (Offer item in offersUnactivityLst)
        //            {
        //                if (Convert.ToInt32((currentDateTime - item.LastActivityDate.Value.Date).TotalDays) > 30)
        //                {
        //                    //unactivity add
        //                    offersUnactivityListFinal.Add(item);
        //                }
        //            }
        //        }

        //        StringBuilder EmailBodyWithTable = new StringBuilder();

        //        foreach (var item1 in TypeList)
        //        {
        //            List<Activity> TempActivityList = ActivityList.Where(x => x.LookupValue.IdLookupValue == item1.IdLookupValue).ToList();

        //            // for create activity table template.
        //            if (TempActivityList.Count > 0)
        //            {
        //                EmailBodyWithTable.Append("<table style=\"border-collapse:collapse; \" width=100%; border=1>");
        //                EmailBodyWithTable.Append("<tr style=\" background-color:#D3D3D3;\">");

        //                if (item1.IdLookupValue == 40)
        //                {
        //                    EmailBodyWithTable.Append("<td colspan=\"3\"> <b>" + item1.Value + "</td>");
        //                    EmailBodyWithTable.Append("<td style=\" text-align:right; \"> <b> " + TempActivityList.Count + " </td>");
        //                }
        //                else
        //                {
        //                    EmailBodyWithTable.Append("<td colspan=\"2\"> <b>" + item1.Value + "</td>");
        //                    EmailBodyWithTable.Append("<td style=\" text-align:right; \"> <b> " + TempActivityList.Count + " </td>");
        //                }

        //                EmailBodyWithTable.Append("</ tr >");

        //                EmailBodyWithTable.Append("<tr>");
        //                EmailBodyWithTable.Append("<td style=\" border-color:#5c87b2; border-style:solid; border-width:thin; padding: 2px;\" width=20%>\n");
        //                EmailBodyWithTable.Append("<b> Title </td>\n");
        //                EmailBodyWithTable.Append("<td style=\" border-color:#5c87b2;  border-style:solid; border-width:thin; padding: 2px;\" width=65%>\n");
        //                EmailBodyWithTable.Append("<b> Description </td>\n");
        //                EmailBodyWithTable.Append("<td style=\" border-color:#5c87b2;  border-style:solid; border-width:thin; padding: 2px;\" width=15%>\n");
        //                EmailBodyWithTable.Append("<b> Date &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </td>\n");

        //                if (item1.IdLookupValue == 40)
        //                {
        //                    EmailBodyWithTable.Append("<td style=\" border-color:#5c87b2;text-align:left; border-style:solid; border-width:thin; padding: 2px;\" width=15%>\n");
        //                    EmailBodyWithTable.Append("<b> Status &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </td>\n");
        //                }

        //                EmailBodyWithTable.Append("</tr>\n");

        //                foreach (Activity activity in TempActivityList)
        //                {

        //                    EmailBodyWithTable.Append("<tr>\n");
        //                    EmailBodyWithTable.Append("<td style=\"text-align:left; \">" + activity.Subject + "</td>\n");
        //                    EmailBodyWithTable.Append("<td style=\"text-align:left; \">" + activity.Description + "</td>\n");
        //                    EmailBodyWithTable.Append("<td style=\"text-align:right; \">" + activity.ToDate.Value.ToShortDateString() + "</td>\n");

        //                    if (item1.IdLookupValue == 40)
        //                    {
        //                        EmailBodyWithTable.Append("<td style=\"text-align:left; \">" + activity.ActivityStatus.Value + "</td>\n");
        //                    }

        //                    EmailBodyWithTable.Append("</tr>\n");
        //                }

        //                EmailBodyWithTable.Append("</table>");
        //                EmailBodyWithTable.Append("<br>\n");
        //            }

        //        }

        //        //Start Created offers Tables.
        //        if (offersWaitingforQuoteLst.Count > 0)
        //        {

        //            string offerTableStr = CreateOfferTebles(offersWaitingforQuoteLst, "Offer Waiting for Qoute").ToString();
        //            EmailBodyWithTable.Append(offerTableStr);
        //        }

        //        if (offersCloseDateOverdueLst.Count > 0)
        //        {

        //            string offerTableStr = CreateOfferTebles(offersCloseDateOverdueLst, "Offer Close Date Overdue").ToString();
        //            EmailBodyWithTable.Append(offerTableStr);
        //        }

        //        if (offersUnactivityListFinal.Count > 0)
        //        {

        //            string offerTableStr = CreateOfferTebles(offersUnactivityListFinal, "Offer Unactivity").ToString();
        //            EmailBodyWithTable.Append(offerTableStr);
        //        }
        //        //End Created offers Tables.

        //        if (ActivityList.Count > 0 || offersWaitingforQuoteLst.Count > 0 ||
        //            offersCloseDateOverdueLst.Count > 0 || offersUnactivityListFinal.Count > 0)

        //            MailControl.SendHtmlMail(reportSubject, EmailBodyWithTable.ToString(), "apawar@emdep.com", "CRM-noreply@emdep.com", "mail.emdep.com", "25", new Dictionary<string, string>());

        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        ///// <summary>
        ///// Method for create create offers table template.
        ///// </summary>
        ///// <param name="offerlst"></param>
        ///// <param name="tableHeader"></param>
        ///// <returns></returns>
        //private StringBuilder CreateOfferTebles(List<Offer> offerlst, string tableHeader)
        //{
        //    StringBuilder emailbodyStr = new StringBuilder();

        //    try
        //    {

        //        double amount = offerlst.Sum(of => of.Value);

        //        CultureInfo clt = new CultureInfo("es-Es");
        //        string amountstr = amount.ToString("C", clt);

        //        //For Created offers.
        //        if (offerlst.Count > 0)
        //        {
        //            emailbodyStr.Append("<br>\n");

        //            emailbodyStr.Append("<table style=\"border-collapse:collapse; \" width=100%; border=1>");
        //            emailbodyStr.Append("<tr style=\" background-color:#D3D3D3;\">");

        //            emailbodyStr.Append("<td colspan=\"5\"> <b>" + tableHeader + "</td>");
        //            emailbodyStr.Append("<td style=\" text-align:right; \"> <b> " + offerlst.Count + " </td>");

        //            emailbodyStr.Append("<td style=\" text-align:right; \"> <b>" + amountstr + " </td>");

        //            emailbodyStr.Append("</ tr >");

        //            emailbodyStr.Append("<tr>");
        //            emailbodyStr.Append("<td style=\" border-color:#5c87b2; border-style:solid; border-width:thin; padding: 2px;\" width=20%>\n");
        //            emailbodyStr.Append("<b> Code </td>\n");
        //            emailbodyStr.Append("<td style=\" border-color:#5c87b2;  border-style:solid; border-width:thin; padding: 2px;\" width=65%>\n");
        //            emailbodyStr.Append("<b> Description </td>\n");
        //            emailbodyStr.Append("<td style=\" border-color:#5c87b2;  border-style:solid; border-width:thin; padding: 2px;\" width=15%>\n");
        //            emailbodyStr.Append("<b> Group </td>\n");

        //            emailbodyStr.Append("<td style=\" border-color:#5c87b2;  border-style:solid; border-width:thin; padding: 2px;\" width=15%>\n");
        //            emailbodyStr.Append("<b> Plant </td>\n");

        //            emailbodyStr.Append("<th style=\" border-color:#5c87b2; text-align:left; border-style:solid; border-width:thin; padding: 2px;\" >\n");
        //            emailbodyStr.Append("<b> Confidence Level(%) </th>\n");

        //            emailbodyStr.Append("<td style=\" border-color:#5c87b2; border-style:solid; border-width:thin; padding: 2px;\" width=30px>\n");
        //            emailbodyStr.Append("<b> Date &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>\n");

        //            emailbodyStr.Append("<td style=\" border-color:#5c87b2;  border-style:solid; border-width:thin; padding: 2px;\" width=15%>\n");
        //            emailbodyStr.Append("<b> Amount &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>\n");

        //            emailbodyStr.Append("</tr>\n");

        //            foreach (var item in offerlst)
        //            {
        //                emailbodyStr.Append("<tr>\n");
        //                emailbodyStr.Append("<td style=\"text-align:left; \">" + item.Code + "</td>\n");
        //                emailbodyStr.Append("<td style=\"text-align:left; \">" + item.Description + "</td>\n");
        //                emailbodyStr.Append("<td style=\"text-align:left; \">" + item.Site.Customer.CustomerName + "</td>\n");
        //                emailbodyStr.Append("<td style=\"text-align:left; \">" + item.Site.Name + "</td>\n");

        //                emailbodyStr.Append("<td style=\"text-align:left; \">" + item.ProbabilityOfSuccess + "</td>\n");

        //                if (item.DeliveryDate != null)
        //                    emailbodyStr.Append("<td style=\"text-align:right; \" >" + item.DeliveryDate.Value.ToShortDateString() + "</td>\n");
        //                else
        //                    emailbodyStr.Append("<td style=\"text-align:right; \">" + string.Empty + "</td>\n");

        //                emailbodyStr.Append("<td style=\"text-align:right; \">" + item.Value.ToString("C", clt) + "</td>\n");


        //                emailbodyStr.Append("</tr>\n");

        //            }

        //            emailbodyStr.Append("</table>");
        //            emailbodyStr.Append("<br>\n");

        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //    return emailbodyStr;
        //}

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

        #endregion  // Methods
    }
}
