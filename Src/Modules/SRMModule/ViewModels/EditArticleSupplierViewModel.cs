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
    class EditArticleSupplierViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable, IDataErrorInfo
    {
        public void Dispose()
        {

        }

        #region Service

        private INavigationService Service { get { return this.GetService<INavigationService>(); } }

        ISRMService SRMService = new SRMServiceController((SRMCommon.Instance.Selectedwarehouse != null && SRMCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? SRMCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService crmControl = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        // ISRMService SRMService = new SRMServiceController("localhost:6699");
        //  ICrmService crmControl = new CrmServiceController("localhost:6699");
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
        List<Contacts> addedContactList;
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
        // string myFilterString;
        //  public List<ArticleBySupplier> articleBySupplierList;

        ObservableCollection<Contacts> toContactList;//[Sudhir.Jangra][GEOS2-5491]
        ObservableCollection<Contacts> ccContactList;//[Sudhir.Jangra][GEOS2-5491]
        private ulong idEditArticleSupplier;//[Sudhir.Jangra][GEOS2-5491]
        List<Contacts> clonedCCList;//[Sudhir.Jangra][GEOS2-5491]
        List<Contacts> clonedToList;//[Sudhir.Jangra][GEOS2-5491]

        string regNoBorderColor;//[Sudhir.Jangra][GEOS2-5491]
   

        #endregion

        #region Properties

        public List<Contacts> AddedContactList
        {
            get { return addedContactList; }
            set
            {
                addedContactList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AddedContactList"));
            }
        }
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
        public int SelectedArticleSupplierIndex
        {
            get { return selectedArticleSupplierIndex; }
            set
            {
                selectedArticleSupplierIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedArticleSupplierIndex"));
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

        public Int32 ArticleSupplierPlantsCount
        {
            get { return articleSupplierPlantsCount; }
            set
            {
                articleSupplierPlantsCount = value;

                OnPropertyChanged(new PropertyChangedEventArgs("ArticleSupplierPlantsCount"));

            }
        }
        public List<LogEntriesByArticleSuppliers> LogEntriesByArticleSuppliers
        {
            get
            {
                return logEntriesByArticleSuppliers;
            }
            set
            {
                logEntriesByArticleSuppliers = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LogEntriesByArticleSuppliers"));
            }
        }
        public bool IsPermissionEnabled
        {
            get { return isPermissionEnabled; }
            set
            {
                isPermissionEnabled = value;

                OnPropertyChanged(new PropertyChangedEventArgs("IsPermissionEnabled"));

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
        public int SelectedArticleSupplierGroupIndex
        {
            get { return selectedArticleSupplierGroupIndex; }
            set
            {
                selectedArticleSupplierGroupIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedArticleSupplierGroupIndex"));
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
        //[GEOS2-4027][rdixit][18.11.2022]
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

        public sbyte IsStillActive
        {
            get { return isStillActive; }
            set
            {
                isStillActive = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsStillActive"));
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

        // [sshegaonkar]
        //  public string MyFilterString
        //{
        //    get { return myFilterString; }
        //    set
        //    {
        //        myFilterString = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("MyFilterString"));
        //    }
        //}

        //public List<ArticleBySupplier> ArticleBySupplierList
        //{
        //    get { return articleBySupplierList; }
        //    set
        //    {
        //        articleBySupplierList = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("ArticleBySupplierList"));
        //    }
        //}

        //[Sudhir.Jangra][GEOS2-5491]
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

        public ulong IdEditArticleSupplier
        {
            get { return idEditArticleSupplier; }
            set
            {
                idEditArticleSupplier = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdEditArticleSupplier"));
            }
        }

        //[Sudhir.Jangra][GEOS2-5491]
        public List<Contacts> ClonedToList
        {
            get { return clonedToList; }
            set
            {
                clonedToList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ClonedToList"));
            }
        }

        //[Sudhir.Jangra][GEOS2-5491]
        public List<Contacts> ClonedCCList
        {
            get { return clonedCCList; }
            set
            {
                clonedCCList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ClonedCCList"));
            }
        }
        //[Sudhir.Jangra][GEOS2-5491]
        public string RegNoBorderColor
        {
            get { return regNoBorderColor; }
            set
            {
                regNoBorderColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RegNoBorderColor"));
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
                    me[BindableBase.GetPropertyName(() => InformationError)];
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
                return null;
            }
        }
        #endregion

        #region ICommands

        public ICommand CancelButtonCommand { get; set; }
        public ICommand ArticleHyperlinkClickCommand { get; set; }
        public ICommand AddNewContactCommand { get; set; }
        public ICommand OrderHyperlinkClickCommand { get; set; }
        public ICommand LinkedContactDoubleClickCommand { get; set; }
        public ICommand HyperlinkForEmail { get; set; }
        public ICommand AcceptButtonCommand { get; set; }
        public ICommand SetMainContactCommand { get; set; }
        public ICommand DeleteButtonCommand { get; set; }
        public ICommand ArticleSupplierAcceptButtonCommand { get; set; }
        // public ICommand CustomShowFilterPopupCommand { get; set; }

        public ICommand AddToButtonCommand { get; set; }//[Sudhir.Jangra][GEOS2-5491]

        public ICommand AddCCButtonCommand { get; set; }//[Sudhir.Jangra][GEOS2-5491]

        public ICommand DeleteOrdersToButtonCommand { get; set; }//[Sudhir.Jangra][GEOS2-5491]

        public ICommand DeleteOrdersCCButtonCommand { get; set; }//[Sudhir.Jangra][GEOS2-5491]
        #endregion

        #region Constructor

        public EditArticleSupplierViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor EditArticleSupplierViewModel()...", category: Category.Info, priority: Priority.Low);

                DialogHeight = System.Windows.SystemParameters.PrimaryScreenHeight - 130;
                DialogWidth = System.Windows.SystemParameters.PrimaryScreenWidth - 50;
                AcceptButtonCommand = new RelayCommand(new Action<object>(SaveSupplierDetails));
                CancelButtonCommand = new DelegateCommand<object>(CancelButtonAction);
                ArticleHyperlinkClickCommand = new DelegateCommand<object>(ArticleHyperlinkClickAction);
                AddNewContactCommand = new DelegateCommand<object>(AddContactViewWindowShow);
                OrderHyperlinkClickCommand = new DelegateCommand<object>(OrderHyperlinkClickAction);
                LinkedContactDoubleClickCommand = new DelegateCommand<object>(LinkedContactDoubleClickCommandAction);
                HyperlinkForEmail = new DelegateCommand<object>(new Action<object>((obj) => { SendMailtoPerson(obj); }));
                SetMainContactCommand = new DelegateCommand<object>(SetMainContactCommandAction);
                //shubham[skadam] for GEOS2-3432
                DeleteButtonCommand = new DelegateCommand<object>(DeleteButtonCommandAction);
                ArticleSupplierAcceptButtonCommand = new DelegateCommand<object>(ArticleSupplierAcceptButtonCommandAction);
                AddToButtonCommand = new RelayCommand(new Action<object>(AddToButtonCommandAction));//[Sudhir.Jangra][GEOS2-5491]
                AddCCButtonCommand = new RelayCommand(new Action<object>(AddCCButtonCommandAction));//[Sudhir.Jangra][GEOS2-5491]
                DeleteOrdersToButtonCommand = new RelayCommand(new Action<object>(DeleteOrdersToButtonCommandAction));//[Sudhir.Jangra][GEOS2-5491]
                DeleteOrdersCCButtonCommand = new RelayCommand(new Action<object>(DeleteOrdersCCButtonCommandAction));//[Sudhir.Jangra][GEOS2-5491]
                //  CustomShowFilterPopupCommand = new DelegateCommand<DevExpress.Xpf.Grid.FilterPopupEventArgs>(CustomShowFilterPopupAction);
                // MyFilterString = string.Empty;


                if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                {
                    RegNoBorderColor = "AntiqueWhite";
                   
                }
                else
                {
                    RegNoBorderColor = "Black";
                  
                }

                GeosApplication.Instance.Logger.Log("Constructor EditArticleSupplierViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EditArticleSupplierViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Methods
        private void SaveSupplierDetails(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SaveSupplierDetails ...", category: Category.Info, priority: Priority.Low);


                GeosApplication.Instance.Logger.Log("Method SaveSupplierDetails() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {


                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SaveSupplierDetails() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {


                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SaveSupplierDetails() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
        }
        public void ArticleHyperlinkClickAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ArticleHyperlinkClickAction....", category: Category.Info, priority: Priority.Low);
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

                TableView detailView = (TableView)obj;
                ArticleBySupplier otItem = (ArticleBySupplier)detailView.DataControl.CurrentItem;
                ArticleDetailsViewModel articleDetailsViewModel = new ArticleDetailsViewModel();
                ArticleDetailsView articleDetailsView = new ArticleDetailsView();
                EventHandler handle = delegate { articleDetailsView.Close(); };
                articleDetailsViewModel.RequestClose += handle;
                Warehouses warehouse = SRMCommon.Instance.Selectedwarehouse;
                long warehouseId = warehouse.IdWarehouse;
                int ArticleSleepDays = SRMCommon.Instance.ArticleSleepDays;
                articleDetailsViewModel.Init_SRM(otItem.Reference, warehouseId, warehouse, ArticleSleepDays);
                articleDetailsView.DataContext = articleDetailsViewModel;

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                var ownerInfo = (obj as FrameworkElement);
                articleDetailsView.Owner = Window.GetWindow(ownerInfo);
                articleDetailsView.ShowDialog();
                //[001] added
                if (articleDetailsViewModel.IsResult)
                {
                    //if (articleDetailsViewModel.UpdateArticle.MyWarehouse != null)
                    //otItem.ArticleMinimumStock = articleDetailsViewModel.UpdateArticle.MyWarehouse.MinimumStock;
                }
                //end
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method ArticleHyperlinkClickAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ArticleHyperlinkClickAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void OrderHyperlinkClickAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OrderHyperlinkClickAction....", category: Category.Info, priority: Priority.Low);
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

                TableView detailView = (TableView)obj;
                WarehousePurchaseOrder ordersCode = (WarehousePurchaseOrder)detailView.DataControl.CurrentItem;

                long IdWarehousePurchaseOrder = ArticleSupplier.Orders.Where(w => w.Code.Equals(ordersCode.Code, StringComparison.OrdinalIgnoreCase)).Select(s => s.IdWarehousePurchaseOrder).FirstOrDefault();
                ViewPurchaseOrderViewModel viewPurchaseOrderViewModel = new ViewPurchaseOrderViewModel();
                ViewPurchaseOrderView viewPurchaseOrderView = new ViewPurchaseOrderView();
                EventHandler handle = delegate { viewPurchaseOrderView.Close(); };

                viewPurchaseOrderViewModel.RequestClose += handle;
                Warehouses warehouse = SRMCommon.Instance.Selectedwarehouse;
                long warehouseId = warehouse.IdWarehouse;
                int ArticleSleepDays = SRMCommon.Instance.ArticleSleepDays;
                viewPurchaseOrderViewModel.Init(ordersCode, warehouse);
                viewPurchaseOrderView.DataContext = viewPurchaseOrderViewModel;

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                var ownerInfo = (obj as FrameworkElement);
                viewPurchaseOrderView.Owner = Window.GetWindow(ownerInfo);
                viewPurchaseOrderView.ShowDialog();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method OrderHyperlinkClickAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method OrderHyperlinkClickAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void Init(ulong IdArticleSupplier, Warehouses objWarehouse)
        {
            try
            {
                IdEditArticleSupplier = IdArticleSupplier;
                //shubham[skadam] for GEOS2-3432  [15-Mar-2022]

                if (objWarehouse != null)
                {
                    selectedWarehouse = (Warehouses)objWarehouse.Clone();
                }
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);

                //[skadam] [10-Mar-2022] [GEOS2-3438]
                //[skadam] [11-Mar-2022] [GEOS2-3435]                      
                // Service Method GetArticleSupplierByIdArticleSupplier_V2301 replaced with GetArticleSupplierByIdArticleSupplier_V2340 by [rdixit][GEOS2-3436][19.11.2022] 
                // Service Method GetArticleSupplierByIdArticleSupplier_V2340 replaced with GetArticleSupplierByIdArticleSupplier_V2360 by [sshegaonkar][GEOS2-3441][24.01.2023] 
                // Service Method GetArticleSupplierByIdArticleSupplier_V2360 replaced with GetArticleSupplierByIdArticleSupplier_V2370 by [sshegaonkar][GEOS2-3442][21.03.2023] 
                SRMCommon.Instance.Selectedwarehouse = objWarehouse;
                SRMService = new SRMServiceController((objWarehouse != null && objWarehouse.Company.ServiceProviderUrl != null) ? objWarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());



                // ArticleSupplier = SRMService.GetArticleSupplierByIdArticleSupplier_V2370(objWarehouse, IdArticleSupplier);
                //[Sudhir.Jangra][GEOS2-4738]
                ArticleSupplier = SRMService.GetArticleSupplierByIdArticleSupplier_V2440(objWarehouse, IdArticleSupplier);

                var tempPlantsCount = ArticleSupplier.Plants.Where(p => p.Allow == true).DefaultIfEmpty(null);
                if (tempPlantsCount != null)
                {
                    articleSupplierPlantsCount = tempPlantsCount.Count();
                }
                //[GEOS2-4027][rdixit][18.11.2022]
                if (ArticleSupplier.IsStillActive == 1)
                {
                    IsActive = true;
                }
                else
                {
                    IsActive = false;
                }
                // FillArticleSupplier();
                FillCountry();
                FillContacts();
                FillToAndCCContactList();//[Sudhir.jangra][GEOS2-5491]
                FillOrders();//[Sudhir.Jangra][GEOS2-5491]
                FillGroups();
                FillCategory();
                FillPaymentTerm();
                FillOrderReception();
                FillCarriageMethods();
                EstimatedDeliveryQuantity = Convert.ToInt32(ArticleSupplier.DeliveryDays);
                //ArticleSupplier.Age = Math.Round((double)((GeosApplication.Instance.ServerDateTime.Month - ArticleSupplier.CreatedIn.Month) + 12 * (GeosApplication.Instance.ServerDateTime.Year - ArticleSupplier.CreatedIn.Year)) / 12, 1);
                DateCalculateInYearAndMonthHelper dateCalculateInYearAndMonth = new DateCalculateInYearAndMonthHelper(GeosApplication.Instance.ServerDateTime.Date, ArticleSupplier.CreatedIn.Date);

                if (dateCalculateInYearAndMonth.Years > 0)
                {
                    CreatedDays = dateCalculateInYearAndMonth.Years.ToString() + "+";
                    CreatedDaysStr = string.Format(System.Windows.Application.Current.FindResource("SRMEditSupplierYears").ToString());
                }
                else
                {
                    if ((GeosApplication.Instance.ServerDateTime.Date - ArticleSupplier.CreatedIn.Date).Days > 99)
                        CreatedDays = "99+";
                    else
                        CreatedDays = (GeosApplication.Instance.ServerDateTime.Date - ArticleSupplier.CreatedIn.Date).Days.ToString();

                    CreatedDaysStr = string.Format(System.Windows.Application.Current.FindResource("SRMEditSupplierDays").ToString());
                }
                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
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
            ArticleSupplier.Warehouse = SRMCommon.Instance.Selectedwarehouse;
            addContactViewModel.Init(ArticleSupplier, null);
            EventHandler handle = delegate { addContactView.Close(); };
            addContactViewModel.RequestClose += handle;
            addContactView.DataContext = addContactViewModel;
            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
            addContactView.ShowDialogWindow();
            //[rdixit][19.10.2023]
            if (addContactViewModel.IsSave)
            {
                FillContacts();

                if (AddedContactList == null)
                    AddedContactList = new List<Contacts>();
                AddedContactList.Add(addContactViewModel.ContactForIsSave);

            }
            //shubham[skadam] for GEOS2-3432  [15-Mar-2022]
            if (ArticleSupplier.LogEntriesByArticleSuppliers == null)
            {
                ArticleSupplier.LogEntriesByArticleSuppliers = new List<Data.Common.SRM.LogEntriesByArticleSuppliers>();
            }
            ArticleSupplier.LogEntriesByArticleSuppliers = SRMService.GetLogEntriesByArticleSuppliers(selectedWarehouse, Convert.ToUInt64(ArticleSupplier.IdArticleSupplier));
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
                //Contacts contactsData = SRMService.GetContacts_V2250(con.IdContact);

                //[pramod.misal][GEOS2-4673][22-08-2023]
                Contacts contactsData = SRMService.GetContacts_V2430(con.IdContact);


                AddContactViewModel addContactViewModel = new AddContactViewModel();
                AddContactView addContactView = new AddContactView();
                addContactViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditContactViewHeader").ToString();
                addContactViewModel.IsSupplierComboBoxVisible = Visibility.Collapsed;//[Sudhir.Jangra][GEOS2-4676]
                addContactViewModel.IsSupplierTextEditVisible = Visibility.Visible;//[Sudhir.Jangra][GEOS2-4676]
                ArticleSupplier.Warehouse = SRMCommon.Instance.Selectedwarehouse;
                addContactViewModel.Init(ArticleSupplier, contactsData);
                EventHandler handle = delegate { addContactView.Close(); };
                addContactViewModel.RequestClose += handle;
                addContactView.DataContext = addContactViewModel;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                addContactView.ShowDialogWindow();

                if (addContactViewModel.IsSave && addContactViewModel.SelectedContacts != null)
                {
                    FillContacts();
                    FillOrders();//[Sudhir.Jangra][GEOS2-5491]
                }
                   

                //shubham[skadam] for GEOS2-3432  [15-Mar-2022]
                if (ArticleSupplier.LogEntriesByArticleSuppliers == null)
                {
                    ArticleSupplier.LogEntriesByArticleSuppliers = new List<Data.Common.SRM.LogEntriesByArticleSuppliers>();
                }
                ArticleSupplier.LogEntriesByArticleSuppliers = SRMService.GetLogEntriesByArticleSuppliers(selectedWarehouse, Convert.ToUInt64(ArticleSupplier.IdArticleSupplier));
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
                    Contacts to = ContactsList.FirstOrDefault(x => x.IsMainContact);
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

                        if (!idTo.Any(x => x == idContact))
                        {
                            Contacts Temp = ToContactList.FirstOrDefault(a => a.IsMainContact);
                            Temp.IsMainContact = false;
                            if (CCContactList.Any(x=>x.IdContact== idContact))
                            {
                                Contacts data1 = CCContactList.FirstOrDefault(a => a.IdContact == idContact);
                                CCContactList.Remove(data1);
                            }
                           // ToContactList.Remove(Temp);
                            ToContactList.Add(ContactsList.FirstOrDefault(x => x.IsMainContact));
                        }
                    }
                }

                if (ContactsList != null)
                    ContactsList.First().IsMainContact = true;
                foreach (var contact in ContactsList.Skip(1))
                {
                    contact.IsMainContact = false;
                }
                ContactsList = new ObservableCollection<Contacts>(ContactsList.OrderByDescending(x => x.IsMainContact).ToList());
                if (ToContactList.Count(x=>x.IsMainContact)>1)
                {
                    var mainContact = ContactsList.First(x => x.IsMainContact);
                    ToContactList.FirstOrDefault(x=>x.IdContact==mainContact.IdContact).IsMainContact = true;
                    foreach (var item in ToContactList)
                    {
                        if (item.IdContact!=mainContact.IdContact)
                        {
                            item.IsMainContact = false;
                        }
                    }
                    ToContactList = new ObservableCollection<Contacts>(ToContactList.OrderByDescending(x => x.IsMainContact?1:0).ToList());
                }
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
                SelectedArticleSupplierIndex = ArticleSupplierList.FindIndex(x => x.IdCountry == ArticleSupplier.IdCountry);
                SelectedArticleSupplier = ArticleSupplierList.Where(i => i.IdCountry == ArticleSupplier.IdCountry).FirstOrDefault();
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
                selectedArticleSupplierGroupIndex = ArticleSupplierGroupList.FindIndex(x => x.IdEnterpriseGroup == ArticleSupplier.IdEnterpriseGroup);
                SelectedArticleSupplierGroup = ArticleSupplierGroupList.Where(i => i.IdEnterpriseGroup == ArticleSupplier.IdEnterpriseGroup).FirstOrDefault();
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

                if (ArticleSupplier.IdArticleSupplierType != null && ArticleSupplier.IdArticleSupplierType > 0)
                {
                    SelectedArticleSupplierCategoryIndex = ArticleSupplierCategoryList.FindIndex(x => x.IdArticleSupplierType == ArticleSupplier.IdArticleSupplierType);
                    SelectedArticleSupplierCategory = ArticleSupplierCategoryList.Where(i => i.IdArticleSupplierType == ArticleSupplier.IdArticleSupplierType).FirstOrDefault();
                }
                else
                {
                    SelectedArticleSupplierCategoryIndex = 0;
                    SelectedArticleSupplierCategory = ArticleSupplierCategoryList.FirstOrDefault();
                }

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
                if (ArticleSupplier.IdPaymentType != null && ArticleSupplier.IdPaymentType > 0)
                {
                    SelectedArticleSupplierPaymentIndex = ArticleSupplierPaymentList.FindIndex(x => x.IdPaymentType == ArticleSupplier.IdPaymentType);
                    SelectedArticleSupplierPayment = ArticleSupplierPaymentList.Where(i => i.IdPaymentType == ArticleSupplier.IdPaymentType).FirstOrDefault();
                }
                else
                {
                    SelectedArticleSupplierPaymentIndex = 0;
                    SelectedArticleSupplierPayment = ArticleSupplierPaymentList.FirstOrDefault();
                }
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

                if (ArticleSupplier.IdOrderReceptionLookup != null && ArticleSupplier.IdOrderReceptionLookup > 0)
                {
                    SelectedIndexOrderReception = OrderReceptionList.FindIndex(x => x.IdLookupValue == ArticleSupplier.IdOrderReceptionLookup);
                    SelectedOrderReception = OrderReceptionList.Where(i => i.IdLookupValue == ArticleSupplier.IdOrderReceptionLookup).FirstOrDefault();
                }
                else
                {
                    SelectedIndexOrderReception = 0;
                    SelectedOrderReception = OrderReceptionList.FirstOrDefault();
                }
                GeosApplication.Instance.Logger.Log("FillOrderReception Method ...executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddEditDiscountsListGridViewModel Method FillLeavesTypeList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[rdixit][GEOS2-3442][21.03.2023]
        private void FillCarriageMethods()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCarriageMethods()...", category: Category.Info, priority: Priority.Low);
                CarriageMethodList = new ObservableCollection<LookupValue>(crmControl.GetLookupValues(74).AsEnumerable());
                CarriageMethodList.ToList().ForEach(i => i.Value = i.Abbreviation + " - " + i.Value);
                CarriageMethodList.Insert(0, new LookupValue() { Value = "---", IdLookupValue = 0, IdLookupKey = 0 });
                if (CarriageMethodList.FirstOrDefault(i => i.IdLookupValue == ArticleSupplier.IdCarriageMethod) != null)
                    SelectedCarriageMethod = CarriageMethodList.FirstOrDefault(i => i.IdLookupValue == ArticleSupplier.IdCarriageMethod);
                else
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

        //shubham[skadam] for GEOS2-3432  [15-Mar-2022]
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
        //shubham[skadam] for GEOS2-3432  [16-Mar-2022]
        private void ArticleSupplierAcceptButtonCommandAction(object obj)
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Method ArticleSupplierAcceptButtonCommandAction()...", category: Category.Info, priority: Priority.Low);

                #region[rdixit][GEOS2-4643][14.07.2023]
                InformationError = null;
                string error = EnableValidationAndGetError();
                if (string.IsNullOrEmpty(error))
                    InformationError = null;
                else
                    InformationError = "";
                #endregion

                PropertyChanged(this, new PropertyChangedEventArgs("SelectedArticleSupplierCategoryIndex"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedArticleSupplierPaymentIndex"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexOrderReception"));

                if (error != null)
                {
                    return;
                }

                if (ArticleSuppliersChangeLogList == null)
                {
                    ArticleSuppliersChangeLogList = new List<LogEntriesByArticleSuppliers>();
                }
                #region [rdixit][GEOS2-3442][21.03.2023]
                if (ArticleSupplier.DeliveryDays != EstimatedDeliveryQuantity)
                {
                    if (ArticleSupplier.DeliveryDays == null || ArticleSupplier.DeliveryDays == 0)
                    {
                        if (EstimatedDeliveryQuantity != null && EstimatedDeliveryQuantity != 0)
                        {
                            ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
                            {
                                IdArticleSupplier = ArticleSupplier.IdArticleSupplier,
                                Datetime = GeosApplication.Instance.ServerDateTime,
                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleSuppliersDeliverydayChangeLogAdded").ToString(), EstimatedDeliveryQuantity)
                            });
                        }
                    }
                    else
                    {
                        ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
                        {
                            IdArticleSupplier = ArticleSupplier.IdArticleSupplier,
                            Datetime = GeosApplication.Instance.ServerDateTime,
                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleSuppliersDeliverydayChangeLogUpdated").ToString(), ArticleSupplier.DeliveryDays, EstimatedDeliveryQuantity)
                        });
                    }
                }
                if (ArticleSupplier.IdCarriageMethod != SelectedCarriageMethod.IdLookupValue)
                {
                    string OldCarriageMethod = string.Empty;
                    if (ArticleSupplier.CarriageMethod == null)
                        ArticleSupplier.CarriageMethod = new LookupValue();
                    if (CarriageMethodList.FirstOrDefault(i => i.IdLookupValue == ArticleSupplier.CarriageMethod.IdLookupValue) != null)
                        OldCarriageMethod = CarriageMethodList.FirstOrDefault(i => i.IdLookupValue == ArticleSupplier.CarriageMethod.IdLookupValue).Value;
                    if (ArticleSupplier.IdCarriageMethod == null || ArticleSupplier.IdCarriageMethod == 0 || OldCarriageMethod == string.Empty || OldCarriageMethod == "---")
                    {
                        if (SelectedCarriageMethod.IdLookupValue != null && SelectedCarriageMethod.IdLookupValue != 0)
                        {
                            ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
                            {
                                IdArticleSupplier = ArticleSupplier.IdArticleSupplier,
                                Datetime = GeosApplication.Instance.ServerDateTime,
                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleSuppliersCarriageMethodChangeLogAdded").ToString(), SelectedCarriageMethod.Value)
                            });
                        }
                    }
                    else
                    {
                        if (SelectedCarriageMethod.IdLookupValue != null && SelectedCarriageMethod.IdLookupValue != 0)
                        {
                            ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
                            {
                                IdArticleSupplier = ArticleSupplier.IdArticleSupplier,
                                Datetime = GeosApplication.Instance.ServerDateTime,
                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleSuppliersCarriageMethodChangeLogUpdated").ToString(), OldCarriageMethod, SelectedCarriageMethod.Value)
                            });
                        }
                        else
                        {
                            ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
                            {
                                IdArticleSupplier = ArticleSupplier.IdArticleSupplier,
                                Datetime = GeosApplication.Instance.ServerDateTime,
                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleSuppliersCarriageMethodChangeLogUpdated").ToString(), OldCarriageMethod, "None")
                            });
                        }
                    }
                }
                #endregion
                var tempList = SRMService.GetContactsByIdArticleSupplier(Convert.ToInt32(ArticleSupplier.IdArticleSupplier));
                var OldIsMainContact = tempList.Where(t => t.IsMainContact == true).FirstOrDefault();

                if (ArticleSupplier.IdArticleSupplierType != SelectedArticleSupplierCategory.IdArticleSupplierType)
                {
                    if (ArticleSupplier.IdArticleSupplierType == null || ArticleSupplier.IdArticleSupplierType == 0)
                    {
                        if (SelectedArticleSupplierCategory.IdArticleSupplierType != null || ArticleSupplier.IdArticleSupplierType != 0)
                        {
                            ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
                            {
                                IdArticleSupplier = ArticleSupplier.IdArticleSupplier,
                                Datetime = GeosApplication.Instance.ServerDateTime,
                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleSuppliersCategoryChangeLogAdded").ToString(), SelectedArticleSupplierCategory.CategoryName)
                            });
                        }
                    }
                    else
                    {
                        if (SelectedArticleSupplierCategory.IdArticleSupplierType != null || ArticleSupplier.IdArticleSupplierType != 0)
                        {
                            ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
                            {
                                IdArticleSupplier = ArticleSupplier.IdArticleSupplier,
                                Datetime = GeosApplication.Instance.ServerDateTime,
                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleSuppliersCategoryChangeLogUpdated").ToString(), ArticleSupplier.ArticleSupplierType.Name, SelectedArticleSupplierCategory.CategoryName)
                            });
                        }
                    }
                }

                if (ArticleSupplier.IdCountry != SelectedArticleSupplier.IdCountry)
                {
                    if (ArticleSupplier.IdCountry == null || ArticleSupplier.IdCountry == 0)
                    {
                        if (SelectedArticleSupplier.IdCountry != null && SelectedArticleSupplier.IdCountry != 0)
                        {
                            ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
                            {
                                IdArticleSupplier = ArticleSupplier.IdArticleSupplier,
                                Datetime = GeosApplication.Instance.ServerDateTime,
                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleSuppliersCountryChangeLogAdded").ToString(), SelectedArticleSupplier.Name)
                            });
                        }
                    }
                    else
                    {
                        if (SelectedArticleSupplier.IdCountry != null && SelectedArticleSupplier.IdCountry != 0)
                        {
                            ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
                            {
                                IdArticleSupplier = ArticleSupplier.IdArticleSupplier,
                                Datetime = GeosApplication.Instance.ServerDateTime,
                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleSuppliersCountryChangeLogUpdated").ToString(), ArticleSupplier.Country.Name, SelectedArticleSupplier.Name)
                            });
                        }
                    }
                }

                if (ArticleSupplier.IdPaymentType != SelectedArticleSupplierPayment.IdPaymentType)
                {
                    if (ArticleSupplier.IdPaymentType == null || ArticleSupplier.IdPaymentType == 0)
                    {
                        if (SelectedArticleSupplierPayment.IdPaymentType != null && SelectedArticleSupplierPayment.IdPaymentType != 0)
                        {
                            ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
                            {
                                IdArticleSupplier = ArticleSupplier.IdArticleSupplier,
                                Datetime = GeosApplication.Instance.ServerDateTime,
                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleSuppliersPaymentTermsChangeLogAdded").ToString(), SelectedArticleSupplierPayment.PaymentType)
                            });
                        }
                    }
                    else
                    {
                        if (SelectedArticleSupplierPayment.IdPaymentType != null && SelectedArticleSupplierPayment.IdPaymentType != 0)
                        {
                            ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
                            {
                                IdArticleSupplier = ArticleSupplier.IdArticleSupplier,
                                Datetime = GeosApplication.Instance.ServerDateTime,
                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleSuppliersPaymentTermsChangeLogUpdated").ToString(), ArticleSupplier.PaymentTerm.PaymentType, SelectedArticleSupplierPayment.PaymentType)
                            });
                        }
                    }
                }

                if (SelectedArticleSupplierGroup != null && ArticleSupplier.IdEnterpriseGroup != SelectedArticleSupplierGroup.IdEnterpriseGroup)
                {
                    if (ArticleSupplier.IdEnterpriseGroup == null || ArticleSupplier.IdEnterpriseGroup == 0)
                    {
                        if (SelectedArticleSupplierGroup.IdEnterpriseGroup != null && SelectedArticleSupplierGroup.IdEnterpriseGroup != 0)
                        {
                            ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
                            {
                                IdArticleSupplier = ArticleSupplier.IdArticleSupplier,
                                Datetime = GeosApplication.Instance.ServerDateTime,
                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleSuppliersGroupChangeLogAdded").ToString(), SelectedArticleSupplierGroup.Name)
                            });
                        }
                    }
                    else
                    {
                        if (SelectedArticleSupplierGroup != null && SelectedArticleSupplierGroup.IdEnterpriseGroup != null && SelectedArticleSupplierGroup.IdEnterpriseGroup != 0)
                        {
                            ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
                            {
                                IdArticleSupplier = ArticleSupplier.IdArticleSupplier,
                                Datetime = GeosApplication.Instance.ServerDateTime,
                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleSuppliersGroupChangeLogUpdated").ToString(), ArticleSupplier.EnterpriseGroup.Name, SelectedArticleSupplierGroup.Name)
                            });
                        }
                    }
                }

                if (ArticleSupplier.IdOrderReceptionLookup != SelectedOrderReception.IdLookupValue)
                {
                    if (ArticleSupplier.IdOrderReceptionLookup == null || ArticleSupplier.IdOrderReceptionLookup == 0)
                    {
                        if (SelectedOrderReception.IdLookupValue != null && SelectedOrderReception.IdLookupValue != 0)
                        {
                            ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
                            {
                                IdArticleSupplier = ArticleSupplier.IdArticleSupplier,
                                Datetime = GeosApplication.Instance.ServerDateTime,
                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleSuppliersOrderReceptionChangeLogAdded").ToString(), SelectedOrderReception.Value)
                            });
                        }
                    }
                    else
                    {
                        if (SelectedOrderReception.IdLookupValue != null && SelectedOrderReception.IdLookupValue != 0)
                        {
                            ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
                            {
                                IdArticleSupplier = ArticleSupplier.IdArticleSupplier,
                                Datetime = GeosApplication.Instance.ServerDateTime,
                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleSuppliersOrderReceptionChangeLogAdded").ToString(), ArticleSupplier.OrderReceptionLookup.Value, SelectedOrderReception.Value)
                            });
                        }
                    }
                }

                //[rdixit][GEOS2-3436][19.11.2022]
                if (ArticleSupplier.IsStillActive != IsStillActive)
                {

                    string prevIsActive;
                    string UpdatedIsActive;
                    if (ArticleSupplier.IsStillActive == 0)
                        prevIsActive = "No";
                    else
                        prevIsActive = "Yes";

                    if (IsStillActive == 0)
                        UpdatedIsActive = "No";
                    else
                        UpdatedIsActive = "Yes";

                    ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
                    {
                        IdArticleSupplier = ArticleSupplier.IdArticleSupplier,
                        Datetime = GeosApplication.Instance.ServerDateTime,
                        IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                        Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleSuppliersIsStillActiveChangeLog").ToString(), prevIsActive, UpdatedIsActive)
                    });

                }
                //var OldIsMainContact = AllContactsList.Where(w => w.IsMainContact == true).FirstOrDefault();
                var NewIsMainContact = ContactsList.Where(c => c.IsMainContact == true).FirstOrDefault();

                if (NewIsMainContact != null && OldIsMainContact != null)
                {

                    if (OldIsMainContact.IdContact != NewIsMainContact.IdContact)
                    {
                        //SRMService = new SRMServiceController("localhost:6699");
                        //[001]SetIsMainContact changed to SetIsMainContact_V2300
                        //bool isIsMainContact = SRMService.SetIsMainContact_V2300(ContactsList.Where(c => c.IsMainContact == true).Select(s => s.IdContact).FirstOrDefault(), ArticleSupplier.IdArticleSupplier);
                        //[pramod.misal][GEOS2-5136][23.01.2024]
                        bool isIsMainContact = SRMService.SetIsMainContact_V2480(ContactsList.Where(c => c.IsMainContact == true).Select(s => s.IdContact).FirstOrDefault(), ArticleSupplier.IdArticleSupplier);
                        ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
                        {
                            IdArticleSupplier = ArticleSupplier.IdArticleSupplier,
                            Datetime = GeosApplication.Instance.ServerDateTime,
                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleSuppliersChangeLogIsMainContactRemove").ToString(), OldIsMainContact.FullName, NewIsMainContact.FullName)
                        });
                    }

                }
                else
                {
                    if (NewIsMainContact != null)
                    {
                        //[001]SetIsMainContact changed to SetIsMainContact_V2300
                        //bool isIsMainContact = SRMService.SetIsMainContact_V2300(ContactsList.Where(c => c.IsMainContact == true).Select(s => s.IdContact).FirstOrDefault(), ArticleSupplier.IdArticleSupplier);
                        //[pramod.misal][GEOS2-5136][23.01.2024]
                        bool isIsMainContact = SRMService.SetIsMainContact_V2480(ContactsList.Where(c => c.IsMainContact == true).Select(s => s.IdContact).FirstOrDefault(), ArticleSupplier.IdArticleSupplier);
                        ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
                        {
                            IdArticleSupplier = ArticleSupplier.IdArticleSupplier,
                            Datetime = GeosApplication.Instance.ServerDateTime,
                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleSuppliersChangeLogIsMainContactAdd").ToString(), NewIsMainContact.FullName)
                        });
                    }
                }



                var RemoveList = AllContactsList.Where(w => !ContactsList.Any(c => c.IdContact == w.IdContact)).ToList();
                foreach (Contacts isRemove in RemoveList)
                {
                    
                    // [Rahul.Gadhave][17-06-2024] [GEOS2-2530]
                    bool IsContactAvailablrInOrders = ToContactList.Any(t => t.IdContact == isRemove.IdContact) ||
                                CCContactList.Any(c => c.IdContact == isRemove.IdContact);

                    if (IsContactAvailablrInOrders)
                    {
                        MessageBoxResult messageBoxResult = CustomMessageBox.Show(
                        Application.Current.Resources["ArticleSuppliersContactDeleted"].ToString(),
                        Application.Current.Resources["PopUpDeleteColor"].ToString(),
                        CustomMessageBox.MessageImagePath.Question,
                        MessageBoxButton.YesNo
                    );

                        // If the user chooses 'No', skip deletion for this contact
                        if (messageBoxResult == MessageBoxResult.No)
                        {
                            continue; // Skip deletion and continue with the next contact
                        }
                    }
                    //[001] DeleteContacts Changed to DeleteContacts_V2300
                    //bool isDelete = SRMService.DeleteContacts_V2300(isRemove.IdContact);
                    bool isDelete = SRMService.DeleteContacts_V2530(isRemove.IdContact);
                    ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
                    {
                        IdArticleSupplier = ArticleSupplier.IdArticleSupplier,
                        Datetime = GeosApplication.Instance.ServerDateTime,
                        IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                        Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleSuppliersChangeLogIsMainContactDeleted").ToString(), isRemove.FullName)
                    });
                }
                //[rdixit][19.10.2023]
                if (AddedContactList != null)
                {
                    foreach (var item in AddedContactList)
                    {
                        if (ContactsList.Any(c => c.IdContact == item.IdContact))
                        {
                            ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
                            {
                                IdArticleSupplier = ArticleSupplier.IdArticleSupplier,
                                Datetime = GeosApplication.Instance.ServerDateTime,
                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleSuppliersChangeLogContactAdded").ToString(), item.FullName)
                            });
                        }
                    }
                }

                //Added By[Rahul.Gadhave][GEOS2-5733][Date:21/05/2024]
                #region 
                if (ToContactList != null)
                {
                    List<Contacts> data = new List<Contacts>();

                    // Delete
                    foreach (Contacts item in ClonedToList)
                    {
                        if (!ToContactList.Any(x => x.IdArticleSupplierPOReceiver == item.IdArticleSupplierPOReceiver))
                        {
                            Contacts con = (Contacts)item.Clone();
                            // Log deletion
                            ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
                            {
                                IdArticleSupplier = ArticleSupplier.IdArticleSupplier,
                                Datetime = GeosApplication.Instance.ServerDateTime,
                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleSuppliersToContactChangeLogUpdated").ToString(), con.FullName)
                            });
                        }
                    }

                    // Add
                    foreach (var item in ToContactList)
                    {
                        if (!ClonedToList.Any(x => x.IdArticleSupplierPOReceiver == item.IdArticleSupplierPOReceiver))
                        {
                            Contacts con = (Contacts)item.Clone();
                            // Log addition
                            ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
                            {
                                IdArticleSupplier = ArticleSupplier.IdArticleSupplier,
                                Datetime = GeosApplication.Instance.ServerDateTime,
                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleSuppliersToContactChangeLogAdded").ToString(), con.FullName)
                            });
                        }
                    }
                }

                if (CCContactList != null)
                {
                    List<Contacts> data = new List<Contacts>();

                    // Delete
                    foreach (Contacts item in ClonedCCList)
                    {
                        if (!CCContactList.Any(x => x.IdArticleSupplierPOReceiver == item.IdArticleSupplierPOReceiver))
                        {
                            Contacts con = (Contacts)item.Clone();
                            // Log deletion
                            ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
                            {
                                IdArticleSupplier = ArticleSupplier.IdArticleSupplier,
                                Datetime = GeosApplication.Instance.ServerDateTime,
                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleSuppliersCCContactChangeLogUpdated").ToString(), con.FullName)
                            });
                        }
                    }

                    // Add
                    foreach (var item in CCContactList)
                    {
                        if (!ClonedCCList.Any(x => x.IdArticleSupplierPOReceiver == item.IdArticleSupplierPOReceiver))
                        {
                            Contacts con = (Contacts)item.Clone();
                            // Log addition
                            ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
                            {
                                IdArticleSupplier = ArticleSupplier.IdArticleSupplier,
                                Datetime = GeosApplication.Instance.ServerDateTime,
                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleSuppliersCCContactChangeLogAdded").ToString(), con.FullName)
                            });
                        }
                    }
                }
                #endregion

                if (ArticleSuppliersChangeLogList != null)
                {
                    if ((SelectedArticleSupplierGroup != null && ArticleSupplier.IdEnterpriseGroup != SelectedArticleSupplierGroup.IdEnterpriseGroup) || ArticleSupplier.IdCountry != SelectedArticleSupplier.IdCountry || ArticleSupplier.IdArticleSupplierType != SelectedArticleSupplierCategory.IdArticleSupplierType || ArticleSupplier.IdPaymentType != SelectedArticleSupplierPayment.IdPaymentType || ArticleSupplier.IdOrderReceptionLookup != SelectedOrderReception.IdLookupValue || ArticleSupplier.IsStillActive != IsStillActive ||
                        ArticleSupplier.DeliveryDays != EstimatedDeliveryQuantity || ArticleSupplier.CarriageMethod != SelectedCarriageMethod)

                    {
                        ArticleSupplier updatedArticle = new ArticleSupplier();

                        updatedArticle.IdArticleSupplier = ArticleSupplier.IdArticleSupplier;
                        updatedArticle.IdArticleSupplierType = SelectedArticleSupplierCategory.IdArticleSupplierType;
                        updatedArticle.IdCountry = SelectedArticleSupplier.IdCountry;
                        updatedArticle.IsStillActive = IsStillActive;
                        if (SelectedArticleSupplierGroup != null)
                        {
                            updatedArticle.IdEnterpriseGroup = SelectedArticleSupplierGroup.IdEnterpriseGroup;
                        }
                        else
                        {
                            updatedArticle.IdEnterpriseGroup = 0;
                        }
                        updatedArticle.IdPaymentType = SelectedArticleSupplierPayment.IdPaymentType;
                        updatedArticle.IdOrderReceptionLookup = SelectedOrderReception.IdLookupValue;
                        updatedArticle.CarriageMethod = SelectedCarriageMethod;
                        updatedArticle.IdCarriageMethod = SelectedCarriageMethod.IdLookupValue;
                        updatedArticle.DeliveryDays = EstimatedDeliveryQuantity;
                        updatedArticle.ModifiedBy = GeosApplication.Instance.ActiveUser.IdUser;
                        // Service Method UpdateArticleSupplier replaced with UpdateArticleSupplier_V2340 by [rdixit][GEOS2-3436][19.11.2022]
                        // Service Method UpdateArticleSupplier_V2340 replaced with UpdateArticleSupplier_V2370 by [rdixit][GEOS2-3442][21.03.2023]
                        IsSaveChanges = SRMService.UpdateArticleSupplier_V2370(updatedArticle);
                    }
                    //[001] AddCommentsOrLogEntriesByArticleSuppliers Changed to AddCommentsOrLogEntriesByArticleSuppliers_V2300
                    SRMService.AddCommentsOrLogEntriesByArticleSuppliers_V2300(ArticleSuppliersChangeLogList);
                }


                //[Sudhir.Jangra][GEOS2-5491]
                if (ToContactList != null)
                {
                    List<Contacts> data = new List<Contacts>();
                    //Delete
                    foreach (Contacts item in ClonedToList)
                    {
                        if (!ToContactList.Any(x => x.IdArticleSupplierPOReceiver == item.IdArticleSupplierPOReceiver))
                        {
                            Contacts con = (Contacts)item.Clone();
                            con.TransactionOperation = ModelBase.TransactionOperations.Delete;
                            data.Add(con);
                        }
                    }

                    //Add
                    foreach (var item in ToContactList)
                    {
                        if (!ClonedToList.Any(x => x.IdArticleSupplierPOReceiver == item.IdArticleSupplierPOReceiver))
                        {
                            Contacts con = (Contacts)item.Clone();
                            con.TransactionOperation = ModelBase.TransactionOperations.Add;
                            data.Add(con);
                        }
                    }



                    data.ForEach(x => { x.IdType = 1918; x.IdArticleSupplier = (Int32)IdEditArticleSupplier; x.OwnerImage = null; x.ImageText = null; });
                  //  ISRMService SRMService = new SRMServiceController("localhost:6699");

                    //SRMService.AddDeleteArticleSupplierOrder_V2510(data, selectedWarehouse.IdWarehouse);
                    //[pramod.misal][31.05.2024]
                    SRMService.AddDeleteArticleSupplierOrder_V2520(data, selectedWarehouse.IdWarehouse);
                }

                if (CCContactList != null)
                {
                    List<Contacts> data = new List<Contacts>();

                    //Delete
                    foreach (Contacts item in ClonedCCList)
                    {
                        if (!CCContactList.Any(x => x.IdArticleSupplierPOReceiver == item.IdArticleSupplierPOReceiver))
                        {
                            Contacts con = (Contacts)item.Clone();
                            con.TransactionOperation = ModelBase.TransactionOperations.Delete;
                            data.Add(con);
                        }
                    }

                    //Add
                    foreach (var item in CCContactList)
                    {
                        if (!ClonedCCList.Any(x => x.IdArticleSupplierPOReceiver == item.IdArticleSupplierPOReceiver))
                        {
                            Contacts con = (Contacts)item.Clone();
                            con.TransactionOperation = ModelBase.TransactionOperations.Add;
                            data.Add(con);
                        }
                    }



                    data.ForEach(x => { x.IdType = 1919; x.IdArticleSupplier = (Int32)IdEditArticleSupplier; x.OwnerImage = null; x.ImageText = null; });
                  // ISRMService SRMService = new SRMServiceController("localhost:6699");

                    //SRMService.AddDeleteArticleSupplierOrder_V2510(data, selectedWarehouse.IdWarehouse);
                    //[pramod.misal][31.05.2024][]
                    SRMService.AddDeleteArticleSupplierOrder_V2520(data, selectedWarehouse.IdWarehouse);


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
                if (ContactsList != null)
                {
                    ContactsList = new ObservableCollection<Contacts>(ContactsList.OrderByDescending(x => x.IsMainContact).ToList());
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

        private void FillToAndCCContactList()
        {
            // ContactsList
            ToContactList = new ObservableCollection<Contacts>(ContactsList.Where(x => x.IsMainContact == true));
            CCContactList = new ObservableCollection<Contacts>();
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

        //[Sudhir.Jangra][GEOS2-5491]
        private void FillOrders()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillOrders...", category: Category.Info, priority: Priority.Low);
               // ISRMService SRMService = new SRMServiceController("localhost:6699");


                //ObservableCollection<Contacts> temp = new ObservableCollection<Contacts>(SRMService.GetArticleSuppliersOrders_V2510(Convert.ToInt32(ArticleSupplier.IdArticleSupplier), selectedWarehouse.IdWarehouse));
                //[pramod.misal][31.05.2024]
                ObservableCollection<Contacts> temp = new ObservableCollection<Contacts>(SRMService.GetArticleSuppliersOrders_V2520(Convert.ToInt32(ArticleSupplier.IdArticleSupplier), selectedWarehouse.IdWarehouse));
                if (temp.Count > 0)
                {
                    ToContactList = new ObservableCollection<Contacts>(temp.Where(x => x.IdType == 1918));
                    CCContactList = new ObservableCollection<Contacts>(temp.Where(x => x.IdType == 1919));
                }
                ClonedToList = new List<Contacts>(temp.Where(x => x.IdType == 1918));
                ClonedCCList = new List<Contacts>(temp.Where(x => x.IdType == 1919));

                if (ToContactList != null)
                {
                    ToContactList = new ObservableCollection<Contacts>(ToContactList.OrderByDescending(x => x.IsMainContact).ToList());
                    ToContactList.FirstOrDefault().IsMainContact = true;
                }
                    
                int count = 0;
                foreach (Contacts contacts in ToContactList)
                {
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


                foreach (Contacts contacts in CCContactList)
                {
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



                GeosApplication.Instance.Logger.Log("Method FillOrders executed Successfully.", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillOrders()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion




    }
}


