using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.SRM;
using Emdep.Geos.Modules.SRM.Views;
using Emdep.Geos.Modules.Warehouse.ViewModels;
using Emdep.Geos.Modules.Warehouse.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.UI.Validations;
using DevExpress.Data.Filtering;

namespace Emdep.Geos.Modules.SRM.ViewModels
{
    public class AddArticleSupplierViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable, IDataErrorInfo
    {
        public void Dispose()
        {

        }

        #region Service

        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        ISRMService SRMService = new SRMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService crmControl = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        // ISRMService SRMService = new SRMServiceController("localhost:6699");
        // ICrmService crmControl = new CrmServiceController("localhost:6699");
        #endregion


        #region Public Events

        public event EventHandler RequestClose;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion // end public events region

        #region Declaration
        private string informationError;
        LookupValue selectedCarriageMethod;
        ObservableCollection<LookupValue> carriageMethodList;
        int estimatedDeliveryQuantity;
        LookupValue orderreception;
        private bool isSaveChanges;
        private double dialogHeight;
        private double dialogWidth;
        bool isMoreNeeded = true;
        private ArticleSupplier articleSupplier;
        private int selectedArticleSupplierIndex;
        private List<Country> articleSupplierList;
        private Country selectedArticleSupplier;
        private ObservableCollection<Contacts> contactsList;
        public string CreatedDays { get; set; }
        public string CreatedDaysStr { get; set; }
        public Int32 articleSupplierPlantsCount;
        List<LogEntriesByArticleSuppliers> logEntriesByArticleSuppliers;
        bool isPermissionEnabled = false;

        private int selectedArticleSupplierGroupIndex;
        private List<Group> articleSupplierGroupList;
        private Group selectedArticleSupplierGroup;
        private Group selectedArticleSupplierGroupVisibility;
        private int selectedArticleSupplierCategoryIndex;
        private List<Categorys> articleSupplierCategoryList;
        private Categorys selectedArticleSupplierCategory;


        private int selectedArticleSupplierPaymentIndex;
        private List<PaymentTerm> articleSupplierPaymentList;
        private PaymentTerm selectedArticleSupplierPayment;
        private List<LookupValue> orderreceptionlist;
        private LookupValue selectedOrderReception;
        private int selectedIndexOrderReception;
        sbyte isStillActive;
        bool isActive;
        string isAcountActiveMsg;
        bool isAddSupplierViewOpened;

        string emdepCode;
        string name;
        string registrationNumber;
        string observations;
        string phone1;
        string phone2;
        string email;
        string fax;
        string website;
        string address;
        string city;
        string state;
        string zipCode;
        string coordinates;
        DateTime createdIn;
        Int64 idArticleSupplier;
        ArticleSupplier newArticleSupplier;

        bool isAdded;

        ObservableCollection<Contacts> toContactList;//[Sudhir.Jangra][GEOS2-5491]
        ObservableCollection<Contacts> ccContactList;//[Sudhir.Jangra][GEOS2-5491]
        #endregion

        #region Properties
        public string InformationError
        {
            get { return informationError; }
            set { informationError = value; OnPropertyChanged(new PropertyChangedEventArgs("InformationError")); }
        }
        public int SelectedIndexOrderReception
        {
            get
            {
                return selectedIndexOrderReception;
            }

            set
            {
                selectedIndexOrderReception = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexOrderReception"));
                if (InformationError != null && SelectedIndexOrderReception != 0 && SelectedArticleSupplierCategoryIndex != 0 && SelectedArticleSupplierPaymentIndex != 0)
                {
                    InformationError = null;
                }
            }
        }
        public int SelectedArticleSupplierCategoryIndex
        {
            get { return selectedArticleSupplierCategoryIndex; }
            set
            {
                selectedArticleSupplierCategoryIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedArticleSupplierCategoryIndex"));
                if (InformationError != null && SelectedIndexOrderReception != 0 && SelectedArticleSupplierCategoryIndex != 0 && SelectedArticleSupplierPaymentIndex != 0)
                {
                    InformationError = null;
                }
            }
        }
        public int SelectedArticleSupplierPaymentIndex
        {
            get { return selectedArticleSupplierPaymentIndex; }
            set
            {
                selectedArticleSupplierPaymentIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedArticleSupplierPaymentIndex"));
                if (InformationError != null && SelectedIndexOrderReception != 0 && SelectedArticleSupplierCategoryIndex != 0 && SelectedArticleSupplierPaymentIndex != 0)
                {
                    InformationError = null;
                }
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
        public ArticleSupplier ArticleSupplier
        {
            get { return articleSupplier; }
            set { articleSupplier = value; OnPropertyChanged(new PropertyChangedEventArgs("ArticleSupplier")); }
        }
        public List<Country> ArticleSupplierList
        {
            get { return articleSupplierList; }
            set
            {
                articleSupplierList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArticleSupplierList"));
            }
        }

        public Country SelectedArticleSupplier
        {
            get { return selectedArticleSupplier; }
            set
            {
                selectedArticleSupplier = value;
                OnPropertyChanged(new PropertyChangedEventArgs("selectedArticleSupplier"));
            }
        }


        public ObservableCollection<Contacts> ContactsList
        {
            get { return contactsList; }
            set
            {
                contactsList = value;
                //[GEOS2-4642][rdixit][13.07.2023]
                if (contactsList != null)
                {
                    if (contactsList.Count == 1)
                    {
                        contactsList.FirstOrDefault().IsMainContact = true;
                    }
                }
                OnPropertyChanged(new PropertyChangedEventArgs("ContactsList"));
            }
        }


        Warehouses selectedWarehouse { get; set; }
        ObservableCollection<Contacts> AllContactsList { get; set; }
        public List<LogEntriesByArticleSuppliers> ArticleSuppliersChangeLogList { get; set; }

        public List<Group> ArticleSupplierGroupList
        {
            get { return articleSupplierGroupList; }
            set
            {
                articleSupplierGroupList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArticleSupplierGroupList"));
            }
        }

        public Group SelectedArticleSupplierGroup
        {
            get { return selectedArticleSupplierGroup; }
            set
            {
                selectedArticleSupplierGroup = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedArticleSupplierGroup"));
            }
        }


        public List<Categorys> ArticleSupplierCategoryList
        {
            get { return articleSupplierCategoryList; }
            set
            {
                articleSupplierCategoryList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArticleSupplierCategoryList"));
            }
        }

        public Categorys SelectedArticleSupplierCategory
        {
            get { return selectedArticleSupplierCategory; }
            set
            {
                selectedArticleSupplierCategory = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedArticleSupplierCategory"));
            }
        }

        public List<PaymentTerm> ArticleSupplierPaymentList
        {
            get { return articleSupplierPaymentList; }
            set
            {
                articleSupplierPaymentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArticleSupplierPaymentList"));
            }
        }

        public PaymentTerm SelectedArticleSupplierPayment
        {
            get { return selectedArticleSupplierPayment; }
            set
            {
                selectedArticleSupplierPayment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedArticleSupplierPayment"));
            }
        }

        public List<LookupValue> OrderReceptionList
        {
            get { return orderreceptionlist; }
            set
            {
                orderreceptionlist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OrderReceptionList"));
            }
        }
        public LookupValue SelectedOrderReception
        {
            get { return selectedOrderReception; }
            set
            {
                selectedOrderReception = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedOrderReception"));
            }
        }
        public LookupValue SelectedCarriageMethod
        {
            get { return selectedCarriageMethod; }
            set
            {
                selectedCarriageMethod = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCarriageMethod"));
            }
        }
        public ObservableCollection<LookupValue> CarriageMethodList
        {
            get { return carriageMethodList; }
            set
            {
                carriageMethodList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CarriageMethodList"));
            }
        }
        public int EstimatedDeliveryQuantity
        {
            get { return estimatedDeliveryQuantity; }
            set
            {
                estimatedDeliveryQuantity = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EstimatedDeliveryQuantity"));
            }
        }
        public sbyte IsStillActive
        {
            get { return isStillActive; }
            set
            {
                isStillActive = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsStillActive"));
            }
        }

        public string EmdepCode
        {
            get { return emdepCode; }
            set
            {
                emdepCode = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmdepCode"));
            }
        }
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name"));
            }
        }
        public string RegistrationNumber
        {
            get { return registrationNumber; }
            set
            {
                registrationNumber = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RegistrationNumber"));
            }
        }
        public string Observations
        {
            get { return observations; }
            set
            {
                observations = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Observations"));
            }
        }
        public string Phone1
        {
            get { return phone1; }
            set
            {
                phone1 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Phone1"));
            }
        }
        public string Phone2
        {
            get { return phone2; }
            set
            {
                phone2 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Phone2"));
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
        public string Fax
        {
            get { return fax; }
            set
            {
                fax = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Fax"));
            }
        }

        public string Website
        {
            get { return website; }
            set
            {
                website = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Website"));
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
        public string City
        {
            get { return city; }
            set
            {
                city = value;
                OnPropertyChanged(new PropertyChangedEventArgs("City"));
            }
        }
        public string State
        {
            get { return state; }
            set
            {
                state = value;
                OnPropertyChanged(new PropertyChangedEventArgs("State"));
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

        public string Coordinates
        {
            get { return coordinates; }
            set
            {
                coordinates = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Coordinates"));
            }
        }
        public DateTime CreatedIn
        {
            get { return createdIn; }
            set
            {
                createdIn = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CreatedIn"));
            }
        }

        public bool IsAddSupplierViewOpened
        {
            get { return isAddSupplierViewOpened; }
            set
            {
                isAddSupplierViewOpened = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAddSupplierViewOpened"));
            }
        }

        public bool IsActive
        {
            get { return isActive; }
            set
            {
                isActive = value;
                if (isActive == true)
                {
                    IsStillActive = 1;
                    IsAcountActiveMsg = string.Format(System.Windows.Application.Current.FindResource("SRMEditSupplierAccountOpen").ToString());
                }
                else
                {
                    IsStillActive = 0;
                    IsAcountActiveMsg = string.Format(System.Windows.Application.Current.FindResource("SRMEditSupplierAccountClosed").ToString());
                }
                OnPropertyChanged(new PropertyChangedEventArgs("IsActive"));
            }
        }

        public string IsAcountActiveMsg
        {
            get { return isAcountActiveMsg; }
            set
            {
                isAcountActiveMsg = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAcountActiveMsg"));
            }
        }

        public Int64 IdArticleSupplier
        {
            get { return idArticleSupplier; }
            set
            {
                idArticleSupplier = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdArticleSupplier"));
            }
        }

        public ArticleSupplier NewArticleSupplier
        {
            get { return newArticleSupplier; }
            set
            {
                newArticleSupplier = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NewArticleSupplier"));
            }
        }

        public bool IsAdded
        {
            get { return isAdded; }
            set
            {
                isAdded = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAdded"));
            }
        }

        public ObservableCollection<Contacts> ToContactList
        {
            get { return toContactList; }
            set
            {
                toContactList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ToContactList"));
            }
        }

        //[Sudhir.Jangra][GEOS2-5491]
        public ObservableCollection<Contacts> CCContactList
        {
            get { return ccContactList; }
            set
            {
                ccContactList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CCContactList"));
            }
        }
        #endregion

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
                    me[BindableBase.GetPropertyName(() => SelectedArticleSupplierCategoryIndex)] +
                    me[BindableBase.GetPropertyName(() => SelectedArticleSupplierPaymentIndex)] +
                    me[BindableBase.GetPropertyName(() => SelectedIndexOrderReception)] +
                    me[BindableBase.GetPropertyName(() => InformationError)] +
                    me[BindableBase.GetPropertyName(() => SelectedArticleSupplier)] +
                    me[BindableBase.GetPropertyName(() => Name)];
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
                string selectedArticleSupplierCategoryName = BindableBase.GetPropertyName(() => SelectedArticleSupplierCategoryIndex);
                string selectedArticleSupplierPaymentType = BindableBase.GetPropertyName(() => SelectedArticleSupplierPaymentIndex);
                string selectedOrderReceptionValue = BindableBase.GetPropertyName(() => SelectedIndexOrderReception);
                string informationError = BindableBase.GetPropertyName(() => InformationError);
                string selectedArticleSupplier = BindableBase.GetPropertyName(() => SelectedArticleSupplier);
                string nameValue = BindableBase.GetPropertyName(() => Name);
                if (columnName == nameValue)//[rdixit][GEOS2-4738][18.10.2023]
                {
                    return SRMArticleSupplierValidation.GetErrorMessage(nameValue, Name);
                }
                if (columnName == selectedArticleSupplierCategoryName)
                {
                    return SRMArticleSupplierValidation.GetErrorMessage(selectedArticleSupplierCategoryName, SelectedArticleSupplierCategoryIndex);
                }
                if (columnName == selectedArticleSupplierPaymentType)
                {
                    return SRMArticleSupplierValidation.GetErrorMessage(selectedArticleSupplierPaymentType, SelectedArticleSupplierPaymentIndex);
                }
                if (columnName == selectedOrderReceptionValue)
                {
                    return SRMArticleSupplierValidation.GetErrorMessage(selectedOrderReceptionValue, SelectedIndexOrderReception);
                }
                if (columnName == informationError)//[rdixit][14.07.2023][GEOS2-4643] 
                {
                    return SRMArticleSupplierValidation.GetErrorMessage(informationError, InformationError);
                }
                if (columnName == selectedArticleSupplier)//[Sudhir.Jangra][GEOS2-4738]
                {
                    return SRMArticleSupplierValidation.GetErrorMessage(selectedArticleSupplier, SelectedArticleSupplier.Name);
                }

                return null;
            }
        }
        #endregion

        #region ICommands

        public ICommand CancelButtonCommand { get; set; }
        public ICommand AddNewContactCommand { get; set; }
        public ICommand LinkedContactDoubleClickCommand { get; set; }
        public ICommand HyperlinkForEmail { get; set; }

        public ICommand SetMainContactCommand { get; set; }
        public ICommand DeleteButtonCommand { get; set; }
        public ICommand ArticleSupplierAcceptButtonCommand { get; set; }

        public ICommand AddToButtonCommand { get; set; }//[Sudhir.Jangra][GEOS2-5491]

        public ICommand AddCCButtonCommand { get; set; }//[Sudhir.Jangra][GEOS2-5491]

        public ICommand DeleteOrdersToButtonCommand { get; set; }//[Sudhir.Jangra][GEOS2-5491]

        public ICommand DeleteOrdersCCButtonCommand { get; set; }//[Sudhir.Jangra][GEOS2-5491]
        #endregion

        #region Constructor

        public AddArticleSupplierViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor EditArticleSupplierViewModel()...", category: Category.Info, priority: Priority.Low);

                DialogHeight = System.Windows.SystemParameters.PrimaryScreenHeight - 130;
                DialogWidth = System.Windows.SystemParameters.PrimaryScreenWidth - 50;
                CancelButtonCommand = new DelegateCommand<object>(CancelButtonAction);
                AddNewContactCommand = new DelegateCommand<object>(AddContactViewWindowShow);
                LinkedContactDoubleClickCommand = new DelegateCommand<object>(LinkedContactDoubleClickCommandAction);
                HyperlinkForEmail = new DelegateCommand<object>(new Action<object>((obj) => { SendMailtoPerson(obj); }));
                SetMainContactCommand = new DelegateCommand<object>(SetMainContactCommandAction);

                DeleteButtonCommand = new DelegateCommand<object>(DeleteButtonCommandAction);
                ArticleSupplierAcceptButtonCommand = new DelegateCommand<object>(ArticleSupplierAcceptButtonCommandAction);

                AddToButtonCommand = new RelayCommand(new Action<object>(AddToButtonCommandAction));//[Sudhir.Jangra][GEOS2-5491]
                AddCCButtonCommand = new RelayCommand(new Action<object>(AddCCButtonCommandAction));//[Sudhir.Jangra][GEOS2-5491]
                DeleteOrdersToButtonCommand = new RelayCommand(new Action<object>(DeleteOrdersToButtonCommandAction));//[Sudhir.Jangra][GEOS2-5491]
                DeleteOrdersCCButtonCommand = new RelayCommand(new Action<object>(DeleteOrdersCCButtonCommandAction));//[Sudhir.Jangra][GEOS2-5491]

                CreatedIn = DateTime.Now;
                NewArticleSupplier = new ArticleSupplier();
                ArticleSuppliersChangeLogList = new List<LogEntriesByArticleSuppliers>();
                IsAcountActiveMsg = string.Format(System.Windows.Application.Current.FindResource("SRMEditSupplierAccountClosed").ToString());
                GeosApplication.Instance.Logger.Log("Constructor EditArticleSupplierViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EditArticleSupplierViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Methods

        public void Init()
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
                ContactsList = new ObservableCollection<Contacts>();
                FillCode();
                FillCountry();
                FillGroups();
                FillCategory();
                FillPaymentTerm();
                FillOrderReception();
                FillCarriageMethods();
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void CancelButtonAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PoCancelButton()...", category: Category.Info, priority: Priority.Low);

                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log("Method PoCancelButton()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method PoCancelButton()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void AddContactViewWindowShow(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method AddContactViewWindowShow...", category: Category.Info, priority: Priority.Low);
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
                    return new Views.SplashScreenView() { DataContext = new SplashScreenViewModel() };
                }, null, null);
            }

            AddContactView addContactView = new AddContactView();
            AddContactViewModel addContactViewModel = new AddContactViewModel();
            addContactViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddContactViewHeader").ToString();
            addContactViewModel.IsSupplierComboBoxVisible = Visibility.Collapsed;//[Sudhir.Jangra][GEOS2-4676]
            addContactViewModel.IsSupplierTextEditVisible = Visibility.Visible;//[Sudhir.Jangra][GEOS2-4676]
            if (ArticleSupplier == null && IsAddSupplierViewOpened)
            {
                ArticleSupplier = new ArticleSupplier();
                ArticleSupplier.IdArticleSupplier = IdArticleSupplier;
                ArticleSupplier.Code = EmdepCode;
                ArticleSupplier.EnterpriseGroup = SelectedArticleSupplierGroup;
                ArticleSupplier.Name = Name;
                ArticleSupplier.Cif = RegistrationNumber;
                // ArticleSupplier.ArticleSupplierType = SelectedArticleSupplierCategory;
                ArticleSupplier.Observations = Observations;
                ArticleSupplier.PaymentTerm = SelectedArticleSupplierPayment;
                ArticleSupplier.OrderReceptionLookup = SelectedOrderReception;
                ArticleSupplier.DeliveryDays = EstimatedDeliveryQuantity;
                ArticleSupplier.CarriageMethod = SelectedCarriageMethod;
                // ArticleSupplier.IsActive = isActive;
                ArticleSupplier.Phone1 = Phone1;
                ArticleSupplier.Phone2 = Phone2;
                ArticleSupplier.Email = Email;
                ArticleSupplier.Fax = Fax;
                ArticleSupplier.Web = Website;
                ArticleSupplier.Address = Address;
                ArticleSupplier.City = City;
                ArticleSupplier.Region = State;
                ArticleSupplier.PostCode = ZipCode;
                ArticleSupplier.Country = SelectedArticleSupplier;
                ArticleSupplier.Coordinates = Coordinates;

                ArticleSupplier.Warehouse = new Warehouses();
                addContactViewModel.IsAddSupplierView = true;
            }
            ArticleSupplier.Warehouse = SRMCommon.Instance.Selectedwarehouse;
            addContactViewModel.Init(ArticleSupplier, null);
            EventHandler handle = delegate { addContactView.Close(); };
            addContactViewModel.RequestClose += handle;
            addContactView.DataContext = addContactViewModel;
            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
            addContactView.ShowDialogWindow();
            if (addContactViewModel.IsSave)
            {
               

                if (addContactViewModel.ContactForIsSave.OwnerImage == null)
                {
                    if (!string.IsNullOrEmpty(addContactViewModel.ContactForIsSave.ImageText))
                    {
                        byte[] imageBytes = Convert.FromBase64String(addContactViewModel.ContactForIsSave.ImageText);
                        MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                        ms.Write(imageBytes, 0, imageBytes.Length);
                        System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
                        addContactViewModel.ContactForIsSave.OwnerImage = byteArrayToImage(imageBytes);
                       // ContactImage = addContactViewModel.ContactForIsSave.OwnerImage;
                        Emdep.Geos.UI.Helper.ImageEditHelper.Base64String = addContactViewModel.ContactForIsSave.ImageText;
                    }
                    else
                    {
                        if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                        {
                            if (addContactViewModel.ContactForIsSave.IdGender == 1)
                            {
                                addContactViewModel.ContactForIsSave.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_White.png");
                           //     ContactImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_White.png");
                                Emdep.Geos.UI.Helper.ImageEditHelper.Base64String = null;
                            }
                            else if (addContactViewModel.ContactForIsSave.IdGender == 2)
                            {
                                addContactViewModel.ContactForIsSave.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_White.png");
                           //     ContactImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_White.png");
                                Emdep.Geos.UI.Helper.ImageEditHelper.Base64String = null;
                            }
                        }
                        else
                        {
                            if (addContactViewModel.ContactForIsSave.IdGender == 1)
                            {
                                addContactViewModel.ContactForIsSave.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_Blue.png");
                             //   ContactImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_Blue.png");
                                Emdep.Geos.UI.Helper.ImageEditHelper.Base64String = null;
                            }
                            else if (addContactViewModel.ContactForIsSave.IdGender == 2)
                            {
                                addContactViewModel.ContactForIsSave.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_Blue.png");
                            //    ContactImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_Blue.png");
                                Emdep.Geos.UI.Helper.ImageEditHelper.Base64String = null;
                            }
                        }
                    }
                }

                ContactsList.Add(addContactViewModel.ContactForIsSave);
            }
               

            if (ContactsList.Any(x => x.IsMainContact))
            {
                if (ToContactList == null)
                {
                    ToContactList = new ObservableCollection<Contacts>(ContactsList.Where(x => x.IsMainContact));
                }
                else
                {
                    if (!ToContactList.Contains(ContactsList.FirstOrDefault(x => x.IsMainContact)))
                    {
                        ToContactList.Add(ContactsList.FirstOrDefault(x => x.IsMainContact));
                    }
                }
            }

            if (ArticleSupplier.LogEntriesByArticleSuppliers == null)
            {
                ArticleSupplier.LogEntriesByArticleSuppliers = new List<Data.Common.SRM.LogEntriesByArticleSuppliers>();
            }
            //  ArticleSupplier.LogEntriesByArticleSuppliers = SRMService.GetLogEntriesByArticleSuppliers(selectedWarehouse, Convert.ToUInt64(ArticleSupplier.IdArticleSupplier));
            GeosApplication.Instance.Logger.Log("Method AddContactViewWindowShow() executed successfully...", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method to open contact on double click
        /// </summary>
        /// <param name="obj"></param>
        public void LinkedContactDoubleClickCommandAction(object obj)
        {
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
                        return new Views.SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }
                Contacts con = obj as Contacts;



                AddContactViewModel addContactViewModel = new AddContactViewModel();
                AddContactView addContactView = new AddContactView();
                addContactViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditContactViewHeader").ToString();
                addContactViewModel.IsSupplierComboBoxVisible = Visibility.Collapsed;//[Sudhir.Jangra][GEOS2-4676]
                addContactViewModel.IsSupplierTextEditVisible = Visibility.Visible;//[Sudhir.Jangra][GEOS2-4676]
                if (ArticleSupplier == null && IsAddSupplierViewOpened)
                {
                    ArticleSupplier = new ArticleSupplier();
                    ArticleSupplier.IdArticleSupplier = IdArticleSupplier;
                    ArticleSupplier.Code = EmdepCode;
                    ArticleSupplier.EnterpriseGroup = SelectedArticleSupplierGroup;
                    ArticleSupplier.Name = Name;
                    ArticleSupplier.Cif = RegistrationNumber;
                    // ArticleSupplier.ArticleSupplierType = SelectedArticleSupplierCategory;
                    ArticleSupplier.Observations = Observations;
                    ArticleSupplier.PaymentTerm = SelectedArticleSupplierPayment;
                    ArticleSupplier.OrderReceptionLookup = SelectedOrderReception;
                    ArticleSupplier.DeliveryDays = EstimatedDeliveryQuantity;
                    ArticleSupplier.CarriageMethod = SelectedCarriageMethod;
                    // ArticleSupplier.IsActive = isActive;
                    ArticleSupplier.Phone1 = Phone1;
                    ArticleSupplier.Phone2 = Phone2;
                    ArticleSupplier.Email = Email;
                    ArticleSupplier.Fax = Fax;
                    ArticleSupplier.Web = Website;
                    ArticleSupplier.Address = Address;
                    ArticleSupplier.City = City;
                    ArticleSupplier.Region = State;
                    ArticleSupplier.PostCode = ZipCode;
                    ArticleSupplier.Country = SelectedArticleSupplier;
                    ArticleSupplier.Coordinates = Coordinates;

                    ArticleSupplier.Warehouse = new Warehouses();

                }
                ArticleSupplier.Warehouse = SRMCommon.Instance.Selectedwarehouse;
                addContactViewModel.IsAddSupplierView = true;
                addContactViewModel.IsNew = false;
                addContactViewModel.Init(ArticleSupplier, con);
                EventHandler handle = delegate { addContactView.Close(); };
                addContactViewModel.RequestClose += handle;
                addContactView.DataContext = addContactViewModel;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                addContactView.ShowDialogWindow();

                if (addContactViewModel.IsSave && addContactViewModel.SelectedContacts != null)
                {
                    int indexToUpdate = ContactsList.ToList().FindIndex(x => x.IdContact == addContactViewModel.SelectedContacts.IdContact);
                    if (indexToUpdate >= 0)
                    {
                        Contacts existingItem = ContactsList[indexToUpdate];
                        existingItem.OwnerImage = addContactViewModel.SelectedContacts.OwnerImage;
                        existingItem.Firstname = addContactViewModel.SelectedContacts.Firstname;
                        existingItem.Lastname = addContactViewModel.SelectedContacts.Lastname;
                        existingItem.JobTitle = addContactViewModel.SelectedContacts.JobTitle;
                        existingItem.Phone = addContactViewModel.SelectedContacts.Phone;
                        existingItem.Phone2 = addContactViewModel.SelectedContacts.Phone2;
                        existingItem.Email = addContactViewModel.SelectedContacts.Email;
                        existingItem.Remarks = addContactViewModel.SelectedContacts.Remarks;
                        existingItem.IdGender = addContactViewModel.SelectedContacts.IdGender;
                    }
                }

                if (ArticleSupplier.LogEntriesByArticleSuppliers == null)
                {
                    ArticleSupplier.LogEntriesByArticleSuppliers = new List<Data.Common.SRM.LogEntriesByArticleSuppliers>();
                }
                // ArticleSupplier.LogEntriesByArticleSuppliers = SRMService.GetLogEntriesByArticleSuppliers(selectedWarehouse, Convert.ToUInt64(ArticleSupplier.IdArticleSupplier));
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
        /// This Method is for set a sales responsible
        /// </summary>
        /// <param name="obj"></param>
        public void SetMainContactCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method SetMainContactCommandAction ...", category: Category.Info, priority: Priority.Low);
            ArticleSuppliersChangeLogList = null;
            if (ArticleSuppliersChangeLogList == null)
            {
                ArticleSuppliersChangeLogList = new List<LogEntriesByArticleSuppliers>();
            }
            try
            {
                List<Contacts> tempContactsList = new List<Contacts>(ContactsList);

                Contacts data = (Contacts)(obj);
                if (ContactsList != null && ContactsList.Any(x => x.IdContact == data.IdContact))
                {


                    foreach (Contacts item in tempContactsList)
                    {
                        if (item.IdContact == data.IdContact)
                        {
                            item.IsMainContact = true;
                        }
                        else
                        {
                            if (item != null)
                            {
                                if (item.IsMainContact)
                                    item.IsMainContact = false;
                                //shubham[skadam] for GEOS2-3432  [16-Mar-2022]
                                //var OldIsMainContact = AllContactsList.Where(w => w.IsMainContact = true).FirstOrDefault();
                                //ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
                                //{
                                //    IdArticleSupplier = ArticleSupplier.IdArticleSupplier,
                                //    Datetime = GeosApplication.Instance.ServerDateTime,
                                //    IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                //    Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleSuppliersChangeLogIsMainContactRemove").ToString(), OldIsMainContact.FullName, item.FullName)
                                //});
                            }

                            if (item.IsMainContact)
                            {
                                //var OldIsMainContact = AllContactsList.Where(w =>w.IsMainContact=true).FirstOrDefault();
                                if (item.IsMainContact == true)
                                {
                                    item.IsMainContact = false;
                                    //shubham[skadam] for GEOS2-3432  [16-Mar-2022]
                                    //ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
                                    //{
                                    //    IdArticleSupplier = ArticleSupplier.IdArticleSupplier,
                                    //    Datetime = GeosApplication.Instance.ServerDateTime,
                                    //    IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                    //    Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleSuppliersChangeLogIsMainContactRemove").ToString(), OldIsMainContact.FullName, item.FullName)
                                    //});                                 
                                }
                            }
                        }


                    }
                }

                ContactsList = new ObservableCollection<Contacts>(tempContactsList);
                //shubham[skadam] for GEOS2-3432  [17-Mar-2022]
                if (ContactsList != null)
                    ContactsList = new ObservableCollection<Contacts>(ContactsList.OrderByDescending(x => x.IsMainContact).ToList());

                if (ToContactList == null)
                {
                    Contacts to =ContactsList.FirstOrDefault(x => x.IsMainContact);
                    ToContactList = new ObservableCollection<Contacts>();
                    ToContactList.Add(to);
                }
                else
                {
                    if (!ToContactList.Any(x => x.IsMainContact = true))
                    {
                        if (ContactsList.Any(x => x.IsMainContact))
                        {
                            Contacts to = ContactsList.FirstOrDefault(x => x.IsMainContact = true);
                            ToContactList.Add(to);
                        }
                    }
                    else if (ToContactList.Any(x => x.IsMainContact = true) && ContactsList.Any(x => x.IsMainContact))
                    {
                        var idTo = ToContactList.Where(x => x.IsMainContact = true).Select(a => a.IdContact);
                        var idContact = ContactsList.FirstOrDefault(x => x.IsMainContact = true).IdContact;

                        if (!idTo.Any(x=>x==idContact))
                        {
                            Contacts Temp = ToContactList.FirstOrDefault(a => a.IsMainContact);
                            Temp.IsMainContact = false;
                            ToContactList.Remove(Temp);
                            ToContactList.Add(ContactsList.FirstOrDefault(x => x.IsMainContact));
                        }

                    }
                }

                if (ContactsList != null)
                    ContactsList = new ObservableCollection<Contacts>(ContactsList.OrderByDescending(x => x.IsMainContact).ToList());
                GeosApplication.Instance.Logger.Log("Method SetMainContactCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SetMainContactCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        public void SendMailtoPerson(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SendMailtoPerson ...", category: Category.Info, priority: Priority.Low);
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
                        return new Views.SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }
                string emailAddess = Convert.ToString(obj);
                string command = "mailto:" + emailAddess;
                System.Diagnostics.Process myProcess = new System.Diagnostics.Process();
                myProcess.StartInfo.FileName = command;
                myProcess.StartInfo.UseShellExecute = true;
                myProcess.StartInfo.RedirectStandardOutput = false;
                myProcess.Start();
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method SendMailtoPerson() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SendMailtoPerson() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        private void FillContacts()
        {
            GeosApplication.Instance.Logger.Log("Method FillContacts...", category: Category.Info, priority: Priority.Low);

            //[pramod.misal][GEOS2-4673][22-08-2023]
            ContactsList = new ObservableCollection<Contacts>(SRMService.GetContactsByIdArticleSupplier(Convert.ToInt32(ArticleSupplier.IdArticleSupplier)));
            if (ContactsList != null)
            {
                AllContactsList = (ObservableCollection<Contacts>)ContactsList;
            }

            if (ContactsList != null)
                ContactsList = new ObservableCollection<Contacts>(ContactsList.OrderByDescending(x => x.IsMainContact).ToList());
            int count = 0;
            foreach (Contacts contacts in ContactsList)
            {
                //if (count == 0)
                //{
                //    contacts.IsMainContact = true;
                //    count = count + 1;
                //}
                //else
                //    contacts.IsMainContact = false;

                if (contacts.OwnerImage == null)
                {
                    if (!string.IsNullOrEmpty(contacts.ImageText))
                    {
                        byte[] imageBytes = Convert.FromBase64String(contacts.ImageText);
                        MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                        ms.Write(imageBytes, 0, imageBytes.Length);
                        System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
                        contacts.OwnerImage = byteArrayToImage(imageBytes);
                    }
                    else    // If User is Null then Show temporary image by gender.
                    {
                        if (ThemeManager.ApplicationThemeName == "BlackAndBlue")
                        {
                            if (contacts.IdGender == 1)
                                contacts.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_White.png");
                            else if (contacts.IdGender == 2)
                                contacts.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_White.png");
                        }
                        else
                        {
                            if (contacts.IdGender == 1)
                                contacts.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_Blue.png");
                            else if (contacts.IdGender == 2)
                                contacts.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_Blue.png");
                        }
                    }
                }
            }

            GeosApplication.Instance.Logger.Log("Method FillSiteContacts executed Successfully.", category: Category.Info, priority: Priority.Low);
        }
        ImageSource GetImage(string path)
        {
            return new BitmapImage(new Uri(path, UriKind.Relative));
        }

        public ImageSource byteArrayToImage(byte[] byteArrayIn)
        {
            ImageSource imgSrc = null;
            try
            {
                GeosApplication.Instance.Logger.Log("Method convert byteArrayToImage ...", category: Category.Info, priority: Priority.Low);

                BitmapImage biImg = new BitmapImage();
                MemoryStream ms = new MemoryStream(byteArrayIn);
                biImg.BeginInit();
                biImg.StreamSource = ms;
                biImg.EndInit();
                biImg.DecodePixelHeight = 10;
                biImg.DecodePixelWidth = 10;

                imgSrc = biImg as ImageSource;

                GeosApplication.Instance.Logger.Log("Method byteArrayToImage() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in byteArrayToImage() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            return imgSrc;
        }

        private void FillCountry()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCountry()...", category: Category.Info, priority: Priority.Low);

                IList<Country> tempArticleSupplierList = SRMService.GetCountries_V2301();
                ArticleSupplierList = new List<Country>();
                ArticleSupplierList = new List<Country>(tempArticleSupplierList);
                ArticleSupplierList.Insert(0, new Country() { Name = "---" });
                SelectedArticleSupplier = ArticleSupplierList.FirstOrDefault();
                //  SelectedArticleSupplierIndex = ArticleSupplierList.FindIndex(x => x.IdCountry == ArticleSupplier.IdCountry);
                //  SelectedArticleSupplier = ArticleSupplierList.Where(i => i.IdCountry == ArticleSupplier.IdCountry).FirstOrDefault();
                GeosApplication.Instance.Logger.Log("Method FillCountry()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCountry() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCountry() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillCountry() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

        }
        private void FillGroups()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillGroups()...", category: Category.Info, priority: Priority.Low);

                IList<Group> tempArticleSupplierGroupList = SRMService.GetGroups_V2301();
                ArticleSupplierGroupList = new List<Group>();
                ArticleSupplierGroupList = new List<Group>(tempArticleSupplierGroupList);
                // selectedArticleSupplierGroupIndex = ArticleSupplierGroupList.FindIndex(x => x.IdEnterpriseGroup == ArticleSupplier.IdEnterpriseGroup);
                // SelectedArticleSupplierGroup = ArticleSupplierGroupList.Where(i => i.IdEnterpriseGroup == ArticleSupplier.IdEnterpriseGroup).FirstOrDefault();
                GeosApplication.Instance.Logger.Log("Method FillGroups()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillGroups() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillGroups() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillGroups() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

        }

        private void FillCategory()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCategory()...", category: Category.Info, priority: Priority.Low);

                IList<Categorys> tempArticleSupplierCategoryList = SRMService.GetCategorys_V2301();
                ArticleSupplierCategoryList = new List<Categorys>();
                ArticleSupplierCategoryList = new List<Categorys>(tempArticleSupplierCategoryList);
                ArticleSupplierCategoryList.Insert(0, new Categorys() { CategoryName = "---" });

                //if (ArticleSupplier.IdArticleSupplierType != null && ArticleSupplier.IdArticleSupplierType > 0)
                //{
                //    SelectedArticleSupplierCategoryIndex = ArticleSupplierCategoryList.FindIndex(x => x.IdArticleSupplierType == ArticleSupplier.IdArticleSupplierType);
                //    SelectedArticleSupplierCategory = ArticleSupplierCategoryList.Where(i => i.IdArticleSupplierType == ArticleSupplier.IdArticleSupplierType).FirstOrDefault();
                //}
                //else
                //{
                SelectedArticleSupplierCategoryIndex = 0;
                SelectedArticleSupplierCategory = ArticleSupplierCategoryList.FirstOrDefault();
                // }

                GeosApplication.Instance.Logger.Log("Method FillCategory()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCategory() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCategory() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillCategory() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

        }

        private void FillPaymentTerm()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillPaymentTerm()...", category: Category.Info, priority: Priority.Low);

                IList<PaymentTerm> tempArticleSupplierPaymentList = SRMService.GetPayments_V2301();
                ArticleSupplierPaymentList = new List<PaymentTerm>();
                ArticleSupplierPaymentList = new List<PaymentTerm>(tempArticleSupplierPaymentList);
                ArticleSupplierPaymentList.Insert(0, new PaymentTerm() { PaymentType = "---" });
                //if (ArticleSupplier.IdPaymentType != null && ArticleSupplier.IdPaymentType > 0)
                //{
                //    SelectedArticleSupplierPaymentIndex = ArticleSupplierPaymentList.FindIndex(x => x.IdPaymentType == ArticleSupplier.IdPaymentType);
                //    SelectedArticleSupplierPayment = ArticleSupplierPaymentList.Where(i => i.IdPaymentType == ArticleSupplier.IdPaymentType).FirstOrDefault();
                //}
                //else
                //{
                SelectedArticleSupplierPaymentIndex = 0;
                SelectedArticleSupplierPayment = ArticleSupplierPaymentList.FirstOrDefault();
                //}
                GeosApplication.Instance.Logger.Log("Method FillPaymentTerm()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillPaymentTerm() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillPaymentTerm() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillPaymentTerm() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

        }

        public void FillOrderReception()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("FillOrderReception Method ..", category: Category.Info, priority: Priority.Low);

                IList<LookupValue> tempList = crmControl.GetLookupValues(99);
                OrderReceptionList = new List<LookupValue>();
                OrderReceptionList = new List<LookupValue>(tempList);
                OrderReceptionList.Insert(0, new LookupValue() { Value = "---", InUse = true });

                //if (ArticleSupplier.IdOrderReceptionLookup != null && ArticleSupplier.IdOrderReceptionLookup > 0)
                //{
                //    SelectedIndexOrderReception = OrderReceptionList.FindIndex(x => x.IdLookupValue == ArticleSupplier.IdOrderReceptionLookup);
                //    SelectedOrderReception = OrderReceptionList.Where(i => i.IdLookupValue == ArticleSupplier.IdOrderReceptionLookup).FirstOrDefault();
                //}
                //else
                //{
                SelectedIndexOrderReception = 0;
                SelectedOrderReception = OrderReceptionList.FirstOrDefault();
                //}
                GeosApplication.Instance.Logger.Log("FillOrderReception Method ...executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddEditDiscountsListGridViewModel Method FillLeavesTypeList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillCarriageMethods()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCarriageMethods()...", category: Category.Info, priority: Priority.Low);
                CarriageMethodList = new ObservableCollection<LookupValue>(crmControl.GetLookupValues(74).AsEnumerable());
                CarriageMethodList.ToList().ForEach(i => i.Value = i.Abbreviation + " - " + i.Value);
                CarriageMethodList.Insert(0, new LookupValue() { Value = "---", IdLookupValue = 0, IdLookupKey = 0 });
                //if (CarriageMethodList.FirstOrDefault(i => i.IdLookupValue == ArticleSupplier.IdCarriageMethod) != null)
                //    SelectedCarriageMethod = CarriageMethodList.FirstOrDefault(i => i.IdLookupValue == ArticleSupplier.IdCarriageMethod);
                //else
                SelectedCarriageMethod = CarriageMethodList.FirstOrDefault();
                GeosApplication.Instance.Logger.Log("Method FillCarriageMethods()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCarriageMethods() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCarriageMethods() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillCarriageMethods() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

        }
        private void DeleteButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
                Contacts con = obj as Contacts;
                //bool isDelete = SRMService.DeleteContacts(con.IdContact);

                ContactsList.Remove(con);
                //[GEOS2-4642][rdixit][13.07.2023]
                if (ContactsList != null)
                {
                    if (ContactsList.Count == 1)
                    {
                        ContactsList.FirstOrDefault().IsMainContact = true;
                    }
                }
                GeosApplication.Instance.Logger.Log("Method DeleteButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ArticleSupplierAcceptButtonCommandAction(object obj)
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Method ArticleSupplierAcceptButtonCommandAction()...", category: Category.Info, priority: Priority.Low);


                InformationError = null;
                string error = EnableValidationAndGetError();
                if (string.IsNullOrEmpty(error))
                    InformationError = null;
                else
                    InformationError = "";


                PropertyChanged(this, new PropertyChangedEventArgs("SelectedArticleSupplierCategoryIndex"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedArticleSupplierPaymentIndex"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexOrderReception"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedArticleSupplier"));//[GEOS2-4738]
                PropertyChanged(this, new PropertyChangedEventArgs("Name"));
                if (error != null)
                {
                    return;
                }
                var temp = SRMService.GetSRMEmdepCodeForAddSupplier();
                var newIdArticleSupplier = temp.IdArticleSupplier;

                if (newIdArticleSupplier == IdArticleSupplier)
                {
                    NewArticleSupplier.IdArticleSupplier = IdArticleSupplier;
                }
                else
                {
                    NewArticleSupplier.IdArticleSupplier = newIdArticleSupplier;
                }

                if (SelectedArticleSupplierGroup != null)
                {
                    NewArticleSupplier.IdEnterpriseGroup = SelectedArticleSupplierGroup.IdEnterpriseGroup;
                }


                NewArticleSupplier.Name = Name;
                NewArticleSupplier.Cif = RegistrationNumber;
                NewArticleSupplier.IdArticleSupplierType = SelectedArticleSupplierCategory.IdArticleSupplierType;
                NewArticleSupplier.Observations = Observations;
                NewArticleSupplier.IdPaymentType = SelectedArticleSupplierPayment.IdPaymentType;
                NewArticleSupplier.IdOrderReceptionLookup = SelectedOrderReception.IdLookupValue;
                NewArticleSupplier.DeliveryDays = EstimatedDeliveryQuantity;
                NewArticleSupplier.IdCarriageMethod = SelectedCarriageMethod.IdLookupValue;
                NewArticleSupplier.IsStillActive = IsStillActive;
                NewArticleSupplier.CreatedIn = CreatedIn;
                NewArticleSupplier.Phone1 = Phone1;
                NewArticleSupplier.Phone2 = Phone2;
                NewArticleSupplier.Email = Email;
                NewArticleSupplier.Fax = Fax;
                NewArticleSupplier.Web = Website;
                NewArticleSupplier.Address = Address;
                NewArticleSupplier.City = City;
                NewArticleSupplier.Region = State;
                NewArticleSupplier.PostCode = ZipCode;
                NewArticleSupplier.IdCountry = SelectedArticleSupplier.IdCountry;
                NewArticleSupplier.Coordinates = Coordinates;
                List<Warehouses> warehouses = new List<Warehouses>();
                if (SRM.SRMCommon.Instance.SelectedAuthorizedWarehouseList != null)
                {
                    List<Warehouses> plantOwners = SRM.SRMCommon.Instance.SelectedAuthorizedWarehouseList.Cast<Warehouses>().ToList();
                    foreach (var item in plantOwners)
                    {
                        warehouses.Add(item);
                    }
                }
                ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
                {
                    IdArticleSupplier = NewArticleSupplier.IdArticleSupplier,
                    Datetime = GeosApplication.Instance.ServerDateTime,
                    IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                    Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleSuppliersChangeLogIsMainContactAdded").ToString(), NewArticleSupplier.Name)
                });
                //[rdixit][19.10.2023]
                if (ContactsList != null)
                {
                    foreach (var item in ContactsList)
                    {
                        ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
                        {
                            IdArticleSupplier = NewArticleSupplier.IdArticleSupplier,
                            Datetime = GeosApplication.Instance.ServerDateTime,
                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleSuppliersChangeLogContactAdded").ToString(), item.FullName)
                        });
                    }
                }
                ContactsList.ToList().ForEach(x => x.OwnerImage =null);
                NewArticleSupplier.LogEntriesByArticleSuppliers = ArticleSuppliersChangeLogList;
                //Service Updated from AddArticleSupplier_V2440 to AddArticleSupplier_V2450 by [rdixit][19.10.2023][GEOS2-4961]
                // IsAdded = SRMService.AddArticleSupplier_V2450(NewArticleSupplier, ContactsList.ToList(), warehouses);
                //[Sudhir.Jangra]
                IsAdded = SRMService.AddArticleSupplier_V2530(NewArticleSupplier, ContactsList.ToList(), warehouses);



                if (IsAdded)
                {
                   // SRMService.AddCommentsOrLogEntriesByArticleSuppliers_V2300(ArticleSuppliersChangeLogList);

                    //[Sudhir.Jangra][GEOS2-5491]
                    if (ToContactList != null)
                    {
                        List<Contacts> data = new List<Contacts>();
                        foreach (var item in ToContactList)
                        {
                            item.TransactionOperation = ModelBase.TransactionOperations.Add;
                            data.Add(item);
                        }

                        data.ForEach(x => { x.IdType = 1918; x.IdArticleSupplier = (Int32)NewArticleSupplier.IdArticleSupplier; x.OwnerImage = null; x.ImageText = null; });
                      //  ISRMService SRMService = new SRMServiceController("localhost:6699");

                        foreach (var item in warehouses)
                        {
                            //SRMService.AddDeleteArticleSupplierOrder_V2510(data, item.IdWarehouse);
                            //[pramod.misal][31.05.2024]
                            SRMService.AddDeleteArticleSupplierOrder_V2520(data, item.IdWarehouse);
                        }
                        
                    }

                    if (CCContactList != null)
                    {
                        List<Contacts> data = new List<Contacts>();

                        foreach (var item in CCContactList)
                        {
                            item.TransactionOperation = ModelBase.TransactionOperations.Add;
                            data.Add(item);
                        }

                        data.ForEach(x => { x.IdType = 1919; x.IdArticleSupplier = (Int32)NewArticleSupplier.IdArticleSupplier; x.OwnerImage = null; x.ImageText = null; });
                     //   ISRMService SRMService = new SRMServiceController("localhost:6699");

                        foreach (var item in warehouses)
                        {
                            //SRMService.AddDeleteArticleSupplierOrder_V2510(data, item.IdWarehouse);
                            //[pramod.misal][31.05.2024]
                            SRMService.AddDeleteArticleSupplierOrder_V2520(data, item.IdWarehouse);
                        }
                    }
                }
                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ArticleCompaniesSuccessNew").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("Method ArticleSupplierAcceptButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ArticleSupplierAcceptButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #region ChangeLog
        public void AddChangedLogDetails()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddChangedLogDetails()...", category: Category.Info, priority: Priority.Low);


                GeosApplication.Instance.Logger.Log("Method AddChangedLogDetails()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an Error in Method AddChangedLogDetails()........" + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion


        private void FillCode()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCode()...", category: Category.Info, priority: Priority.Low);
                var temp = SRMService.GetSRMEmdepCodeForAddSupplier();
                EmdepCode = temp.Code;
                IdArticleSupplier = temp.IdArticleSupplier;

                GeosApplication.Instance.Logger.Log("Method FillCode()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCode() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCode() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillCode() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        //[Sudhir.Jangra][GEOS2-5491]
        private void AddToButtonCommandAction(object obj)
        {
            try
            {
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
                        return new Views.SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }

                AddReceiversView addReceiversView = new AddReceiversView();
                AddReceiversViewModel addReceiversViewModel = new AddReceiversViewModel();
                EventHandler handle = delegate { addReceiversView.Close(); };
                addReceiversViewModel.RequestClose += handle;
                if (ToContactList == null)
                {
                    ToContactList = new ObservableCollection<Contacts>();
                    if (ContactsList.Any(x => x.IsMainContact))
                    {
                        ToContactList.Add(ContactsList.FirstOrDefault(x => x.IsMainContact = true));
                    }


                }
                else
                {
                    if (!ToContactList.Any(x => x.IsMainContact = true))
                    {
                        if (ContactsList.Any(x => x.IsMainContact))
                        {
                            Contacts to = ContactsList.FirstOrDefault(x => x.IsMainContact = true);
                            ToContactList.Add(to);
                        }

                    }
                    else if (ToContactList.Any(x => x.IsMainContact = true) && ContactsList.Any(x => x.IsMainContact))
                    {
                        var idTo = ToContactList.Where(x => x.IsMainContact = true).Select(a => a.IdContact);
                        var idContact = ContactsList.FirstOrDefault(x => x.IsMainContact = true).IdContact;

                        if (!idTo.Any(x => x == idContact))
                        {
                            Contacts Temp = ToContactList.FirstOrDefault(a => a.IsMainContact);
                            Temp.IsMainContact = false;
                            ToContactList.Remove(Temp);
                            ToContactList.Add(ContactsList.FirstOrDefault(x => x.IsMainContact));
                        }

                    }

                }
                if (ContactsList.Count == 0 && ToContactList.Count > 0)
                {
                    ToContactList = new ObservableCollection<Contacts>();
                }
                if (CCContactList == null)
                {
                    CCContactList = new ObservableCollection<Contacts>();
                }
                if (ContactsList.Count == 0 && CCContactList.Count > 0)
                {
                    CCContactList = new ObservableCollection<Contacts>();
                }
                if (ContactsList!=null)
                {
                    ContactsList = new ObservableCollection<Contacts>(ContactsList.OrderByDescending(x=>x.IsMainContact).ToList());
                }
                addReceiversViewModel.Init(ContactsList.Where(contact => !ToContactList.Any(toContact => toContact.IdContact == contact.IdContact) && !CCContactList.Any(toContact => toContact.IdContact == contact.IdContact)).ToList(), ToContactList);
                addReceiversView.DataContext = addReceiversViewModel;
                addReceiversView.ShowDialog();

                if (addReceiversViewModel.IsSave)
                {
                    ToContactList = new ObservableCollection<Contacts>(addReceiversViewModel.IncludedContactList);
                }

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method AddToButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //[Sudhir.Jangra][GEOS2-5491]
        private void AddCCButtonCommandAction(object obj)
        {
            try
            {
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
                        return new Views.SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }

                AddReceiversView addReceiversView = new AddReceiversView();
                AddReceiversViewModel addReceiversViewModel = new AddReceiversViewModel();
                EventHandler handle = delegate { addReceiversView.Close(); };
                addReceiversViewModel.RequestClose += handle;
                List<Contacts> contactList = new List<Contacts>(ContactsList.Where(x => x.IsMainContact == false));
                if (ToContactList == null)
                {
                    ToContactList = new ObservableCollection<Contacts>();
                    if (ContactsList.Any(x => x.IsMainContact))
                    {
                        ToContactList.Add(ContactsList.FirstOrDefault(x => x.IsMainContact = true));
                    }
                }
                else
                {
                    if (!ToContactList.Any(x => x.IsMainContact = true))
                    {
                        if (ContactsList.Any(x => x.IsMainContact))
                        {
                            Contacts to = ContactsList.FirstOrDefault(x => x.IsMainContact = true);
                            ToContactList.Add(to);
                        }

                    }
                    else if (ToContactList.Any(x => x.IsMainContact = true) && ContactsList.Any(x => x.IsMainContact))
                    {
                        var idTo = ToContactList.Where(x => x.IsMainContact = true).Select(a => a.IdContact);
                        var idContact = ContactsList.FirstOrDefault(x => x.IsMainContact = true).IdContact;

                        if (!idTo.Any(x => x == idContact))
                        {
                            Contacts Temp = ToContactList.FirstOrDefault(a => a.IsMainContact);
                            Temp.IsMainContact = false;
                            ToContactList.Remove(Temp);
                            ToContactList.Add(ContactsList.FirstOrDefault(x => x.IsMainContact));
                        }

                    }
                }
                if (ContactsList.Count == 0 && ToContactList.Count > 0)
                {
                    ToContactList = new ObservableCollection<Contacts>();
                }
                if (CCContactList == null)
                {
                    CCContactList = new ObservableCollection<Contacts>();
                }
                if (ContactsList.Count == 0 && CCContactList.Count > 0)
                {
                    CCContactList = new ObservableCollection<Contacts>();
                }
                if (ContactsList != null)
                {
                    ContactsList = new ObservableCollection<Contacts>(ContactsList.OrderByDescending(x => x.IsMainContact).ToList());
                }
                addReceiversViewModel.Init(contactList.Where(contact => !ToContactList.Any(toContact => toContact.IdContact == contact.IdContact) && !CCContactList.Any(toContact => toContact.IdContact == contact.IdContact)).ToList(), CCContactList);
                addReceiversView.DataContext = addReceiversViewModel;
                addReceiversView.ShowDialog();

                if (addReceiversViewModel.IsSave)
                {
                    CCContactList = new ObservableCollection<Contacts>(addReceiversViewModel.IncludedContactList);
                }


            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method AddToButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

      

        //[Sudhir.Jangra][GEOS2-5491]
        private void DeleteOrdersToButtonCommandAction(object obj)
        {
            try
            {
                Contacts con = obj as Contacts;
                ToContactList.Remove(con);
                // bool isDeleted = SRMService.DeleteArticleSupplierOrder_V2510(con.IdArticleSupplierPOReceiver);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteOrdersToButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //[Sudhir.Jangra][GEOS2-5491]
        private void DeleteOrdersCCButtonCommandAction(object obj)
        {
            try
            {
                Contacts con = obj as Contacts;
                CCContactList.Remove(con);
                // bool isDeleted = SRMService.DeleteArticleSupplierOrder_V2510(con.IdArticleSupplierPOReceiver);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteOrdersCCButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion


    }
}
