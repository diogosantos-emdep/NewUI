using DevExpress.Data.Filtering;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Serialization;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.SRM;
using Emdep.Geos.Modules.SRM.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Microsoft.Win32;
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

namespace Emdep.Geos.Modules.SRM.ViewModels
{
    public class ContactsViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable
    {
        #region Services

        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        CrmRestServiceController CrmRestStartUp = new CrmRestServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ISRMService SRMService = new SRMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //ISRMService SRMService = new SRMServiceController("localhost:6699");
        //ICrmService CrmStartUp = new CrmServiceController("localhost:6699");

        #endregion // Services

        #region Declaration

        bool isBusy;

        bool isContactColumnChooserVisible;
        private char? _SelectedLetter;
        string filterStringForName;
        public string ContactGridSettingFilePath = GeosApplication.Instance.UserSettingFolderName + "ContactGridSetting.Xml";

        ObservableCollection<Contacts> peopleContacts;
        private ObservableCollection<char> _Letters;

        private Contacts importedContact;
        private Object selectedObject;
        private string myFilterString;

        public string PreviouslySelectedSalesOwners { get; set; }
        public string PreviouslySelectedPlantOwners { get; set; }
        private ObservableCollection<Emdep.Geos.UI.Helper.Column> columns;
        private ObservableCollection<LookupValue> listDepartment;

        bool saleOwnerVisibility;
        List<Country> countries;

        ObservableCollection<ArticleSupplier> supplierList;//[sudhir.jangra][GEOS2-4676]


        #endregion // Declaration

        #region  public Properties

        public ObservableCollection<LookupValue> ListDepartment
        {
            get { return listDepartment; }
            set
            {
                listDepartment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListDepartment"));
            }
        }

        public List<LogEntriesByActivity> ChangedLogsEntries { get; set; }

        public Contacts ImportedContact
        {
            get { return importedContact; }
            set
            {
                importedContact = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ImportedContact"));
            }
        }

        public Object SelectedObject
        {
            get { return selectedObject; }
            set
            {
                selectedObject = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedObject"));
            }
        }

        public ObservableCollection<Contacts> PeopleContacts
        {
            get { return peopleContacts; }
            set
            {
                peopleContacts = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PeopleContacts"));
            }
        }

        public ObservableCollection<char> Letters
        {
            get
            {
                if (this._Letters == null)
                {
                    this._Letters = new ObservableCollection<char>();
                    char c = 'A';
                    while (c <= 'Z')
                    {
                        this._Letters.Add(c);
                        c++;
                    }
                }
                return this._Letters;
            }
        }

        public string FilterStringForName
        {
            get { return filterStringForName; }
            set
            {
                filterStringForName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FilterStringForName"));
            }
        }

        public char? SelectedLetter
        {
            get { return this._SelectedLetter; }
            set
            {
                if (this._SelectedLetter != value)
                {
                    this._SelectedLetter = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("SelectedLetter"));

                    if (this._SelectedLetter == null)
                    {
                        FilterStringForName = string.Empty;
                    }
                    else
                    {
                        FilterStringForName = "StartsWith([Firstname], '" + this.SelectedLetter + "')";
                    }
                }
            }
        }

        public bool IsContactColumnChooserVisible
        {
            get { return isContactColumnChooserVisible; }
            set
            {
                isContactColumnChooserVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsContactColumnChooserVisible"));
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

        public string MyFilterString
        {
            get { return myFilterString; }
            set
            {
                myFilterString = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MyFilterString"));
            }
        }

        public ObservableCollection<Emdep.Geos.UI.Helper.Column> Columns
        {
            get { return columns; }
            set
            {
                columns = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Columns"));
            }
        }

        public bool SaleOwnerVisibility
        {
            get { return saleOwnerVisibility; }
            set
            {
                saleOwnerVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SaleOwnerVisibility"));
            }
        }

        // Export Excel .xlsx
        public virtual string ResultFileName { get; set; }

        public virtual bool DialogResult { get; set; }

        public List<Country> Countries
        {
            get { return countries; }
            set
            {
                countries = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Countries"));
            }
        }

        public ObservableCollection<ArticleSupplier> SupplierList
        {
            get { return supplierList; }
            set
            {
                supplierList = value;
                OnPropertyChanged(new PropertyChangedEventArgs(""));
            }
        }

        #endregion // Properties

        #region Event

        public event EventHandler RequestClose;

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region  public ICommand

        public ICommand CustomCellAppearanceCommand { get; private set; }
        public ICommand PrintButtonCommand { get; set; }
        public ICommand HyperlinkForEmail { get; set; }
        public ICommand ForSellectAllCommand { get; set; }
        public ICommand AddContactCommand { get; set; }


        public ICommand EditContactGridDoubleClickCommand { get; set; }
        public ICommand SalesOwnerPopupClosedCommand { get; private set; }
        public ICommand PlantOwnerPopupClosedCommand { get; private set; }
        public ICommand RefreshContactViewCommand { get; set; }
        public ICommand ExportButtonCommand { get; set; }
        public ICommand DisableContactCommand { get; set; }
        public ICommand CustomShowFilterPopupCommand { get; set; }
        public ICommand CustomUnboundColumnDataCommand { get; set; }

        public ICommand DeleteCommand { get; set; }
        #endregion

        #region Constructor

        public ContactsViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor ContactsViewModel ...", category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive)
                {
                    //  DXSplashScreen.Show<SplashScreenView>(); 
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


                CustomCellAppearanceCommand = new DevExpress.Mvvm.DelegateCommand<RoutedEventArgs>(CustomCellAppearanceGridControl);
                PrintButtonCommand = new Prism.Commands.DelegateCommand<object>(PrintAction);
                ForSellectAllCommand = new Prism.Commands.DelegateCommand<object>(SelectAllAction);
                RefreshContactViewCommand = new Prism.Commands.DelegateCommand<object>(RefreshContactDetails);
                HyperlinkForEmail = new DelegateCommand<object>(new Action<object>((obj) => { SendMailtoPerson(obj); }));
                AddContactCommand = new RelayCommand(new Action<object>(AddContactViewWindowShow));

                EditContactGridDoubleClickCommand = new DelegateCommand<object>(EditContactAction);
                SalesOwnerPopupClosedCommand = new DevExpress.Mvvm.DelegateCommand<object>(SalesOwnerPopupClosedCommandAction);
                PlantOwnerPopupClosedCommand = new DevExpress.Mvvm.DelegateCommand<object>(PlantOwnerPopupClosedCommandAction);
                ExportButtonCommand = new RelayCommand(new Action<object>(ExportContactsGridButtonCommandAction));
                DisableContactCommand = new DelegateCommand<object>(DisableContactRowCommandAction);
                CustomShowFilterPopupCommand = new DelegateCommand<FilterPopupEventArgs>(CustomShowFilterPopupAction);
                CustomUnboundColumnDataCommand = new DelegateCommand<object>(CustomUnboundColumnDataAction);
                DeleteCommand = new DelegateCommand<object>(DeleteCommandAction);


                fillCountries();
                FillCmbSalesOwner();
                MyFilterString = string.Empty;
                IsContactColumnChooserVisible = true;

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Constructor ContactsViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ContactsViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }



        #endregion // Constructor

        #region Methods

        /// <summary>
        /// Method for fill data as per user permission.
        /// </summary>
        private void FillCmbSalesOwner()
        {
            SaleOwnerVisibility = false;
            FillContactList();
            FillSupplierList();//[Sudhir.jangra][GEOS2-4676]
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        private void DisableContactRowCommandAction(object parameter)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DisableContactRowCommandAction() ...", category: Category.Info, priority: Priority.Low);

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                IsBusy = true;
                bool result = false;
                Contacts ObjPeople = (Contacts)parameter;

                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["ContactDisableMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    if (PeopleContacts != null && PeopleContacts.Count > 0)
                    {
                        ObjPeople.IsStillActive = 0;
                        result = SRMService.DeleteContactDetails_V2340(ObjPeople);
                        PeopleContacts.Remove((Contacts)ObjPeople);
                    }

                    if (result)
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ContactDisableSuccess").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        IsBusy = false;
                        GeosApplication.Instance.Logger.Log("Method DisableContactRowCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
                    }
                    else
                    {
                        IsBusy = false;
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ContactDisableFailed").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                }
                else
                {
                    IsBusy = false;
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DisableContactRowCommandAction() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DisableContactRowCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in DisableContactRowCommandAction() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        /// This method is used to get data by salesowner group
        /// </summary>
        /// <param name="obj"></param>
        private void SalesOwnerPopupClosedCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method SalesOwnerPopupClosedCommandAction ...", category: Category.Info, priority: Priority.Low);

            if (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
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

            if (GeosApplication.Instance.SelectedSalesOwnerUsersList != null)
            {
                // FillContactListByUser();
            }
            else
            {
                PeopleContacts = new ObservableCollection<Contacts>();
            }
            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

            GeosApplication.Instance.Logger.Log("Method SalesOwnerPopupClosedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// This method is used to get data by salesowner group
        /// </summary>
        /// <param name="obj"></param>
        private void PlantOwnerPopupClosedCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method PlantOwnerPopupClosedCommandAction ...", category: Category.Info, priority: Priority.Low);

            if (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
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

            if (SRMCommon.Instance.SelectedAuthorizedWarehouseList != null)
            {
                FillContactList();
                FillSupplierList();//[Sudhir.jangra][GEOS2-4676]
            }
            else
            {
                SupplierList = new ObservableCollection<ArticleSupplier>();
                PeopleContacts = new ObservableCollection<Contacts>();
            }

            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

            GeosApplication.Instance.Logger.Log("Method PlantOwnerPopupClosedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method for refresh contact details.
        /// </summary>
        /// <param name="obj"></param>
        private void RefreshContactDetails(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method RefreshContactDetails ...", category: Category.Info, priority: Priority.Low);

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

            MyFilterString = string.Empty;


            FillCmbSalesOwner();

            // code for hide column chooser if empty
            TableView detailView = (TableView)obj;
            //detailView.ShowGroupPanel = false;
            GridControl gridControl = (detailView).Grid;

            int visibleFalseCoulumn = 0;
            foreach (GridColumn column in gridControl.Columns)
            {
                if (column.Visible == false)
                {
                    visibleFalseCoulumn++;
                }
            }

            if (visibleFalseCoulumn > 0)
            {
                IsContactColumnChooserVisible = true;
            }
            else
            {
                IsContactColumnChooserVisible = false;
            }
            detailView.SearchString = null;

            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
            GeosApplication.Instance.Logger.Log("Method RefreshContactDetails() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method for Fill all contact list depend on Sales Owner. 
        /// </summary>
        //private void FillContactListByUser()
        //{
        //    try
        //    {
        //        if (GeosApplication.Instance.SelectedSalesOwnerUsersList != null)
        //        {
        //            GeosApplication.Instance.Logger.Log("Method FillContactListByUser ...", category: Category.Info, priority: Priority.Low);

        //            List<UserManagerDtl> salesOwners = GeosApplication.Instance.SelectedSalesOwnerUsersList.Cast<UserManagerDtl>().ToList();
        //            var salesOwnersIds = string.Join(",", salesOwners.Select(i => i.IdUser));
        //            PreviouslySelectedSalesOwners = salesOwnersIds;
        //            #region
        //            //PeopleContacts = new ObservableCollection<People>(CrmStartUp.GetSelectedUserContactsBySalesOwnerId(salesOwnersIds));
        //            //[001] Changed service method GetContactsByIdPermission_V2033 to GetContactsByIdPermission_V2300 [GEOS2-2312][12.08.2022][rdixit]
        //            //PeopleContacts = new ObservableCollection<People>(CrmStartUp.GetContactsByIdPermission_V2300(GeosApplication.Instance.ActiveUser.IdUser, salesOwnersIds, null, GeosApplication.Instance.IdUserPermission));
        //            //Service V2300 updated with V2380 by [rdixit][26.04.2023][GEOS2-4324]
        //            //Service V2380 updated with V2390 by [rdixit][04.05.2023][GEOS2-4280]
        //            #endregion
        //           // PeopleContacts = new ObservableCollection<People>(SRMService.GetContactsByIdPermission_V2390(GeosApplication.Instance.ActiveUser.IdUser, salesOwnersIds, null, GeosApplication.Instance.IdUserPermission));
        //            //[Sudhir.Jangra][GEOS2-4676]
        //            PeopleContacts = new ObservableCollection<People>(SRMService.GetContactsByIdPermission_V2430(GeosApplication.Instance.ActiveUser.IdUser, salesOwnersIds, null, GeosApplication.Instance.IdUserPermission));


        //        }
        //        else
        //        {
        //            PeopleContacts = new ObservableCollection<People>();
        //        }
        //        GeosApplication.Instance.Logger.Log("Method FillContactListByUser() executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (FaultException<ServiceException> ex)
        //    {
        //        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Get an error in FillContactListByUser() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
        //        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
        //    }
        //    catch (ServiceUnexceptedException ex)
        //    {
        //        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Get an error in FillContactListByUser() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
        //    }
        //}

        /// <summary>
        /// Method for Fill all contact list depend on Sales Owner. 
        /// </summary>
        //private void FillContactListByPlant()
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method FillContactListByPlant ...", category: Category.Info, priority: Priority.Low);
        //        if (GeosApplication.Instance.SelectedAuthorizedWarehouseList != null)
        //        {
        //            List<Company> plantOwners = GeosApplication.Instance.SelectedAuthorizedWarehouseList.Cast<Company>().ToList();
        //            var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.ConnectPlantId));
        //            #region
        //            //PeopleContacts = new ObservableCollection<People>(CrmStartUp.GetSelectedUserContactsByPlant(plantOwnersIds));
        //            //[001] Changed service method GetContactsByIdPermission_V2033 to GetContactsByIdPermission_V2300 [GEOS2-2312][12.08.2022][rdixit]
        //            //PeopleContacts = new ObservableCollection<People>(CrmStartUp.GetContactsByIdPermission_V2300(GeosApplication.Instance.ActiveUser.IdUser, null, plantOwnersIds, GeosApplication.Instance.IdUserPermission));
        //            //Service V2300 updated with V2380 by [rdixit][26.04.2023][GEOS2-4324]
        //            //Service V2380 updated with V2390 by [rdixit][04.05.2023][GEOS2-4280]
        //            #endregion
        //            // PeopleContacts = new ObservableCollection<People>(SRMService.GetContactsByIdPermission_V2390(GeosApplication.Instance.ActiveUser.IdUser, null, plantOwnersIds, GeosApplication.Instance.IdUserPermission));
        //            //[Sudhir.Jangra][GEOS2-4676]
        //            PeopleContacts = new ObservableCollection<People>(SRMService.GetContactsByIdPermission_V2430(GeosApplication.Instance.ActiveUser.IdUser, null, plantOwnersIds, GeosApplication.Instance.IdUserPermission));

        //            // PeopleContacts = new ObservableCollection<People>(CrmStartUp.GetContactsByIdPermission_V2300(GeosApplication.Instance.ActiveUser.IdUser, null, plantOwnersIds, GeosApplication.Instance.IdUserPermission));
        //        }
        //        else
        //        {
        //            PeopleContacts = new ObservableCollection<People>();
        //        }
        //        GeosApplication.Instance.Logger.Log("Method FillContactListByPlant() executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (FaultException<ServiceException> ex)
        //    {
        //        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Get an error in FillContactListByPlant() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
        //        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
        //    }
        //    catch (ServiceUnexceptedException ex)
        //    {
        //        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Get an error in FillContactListByPlant() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
        //    }
        //}

        /// <summary>
        /// Method for Fill all contact list. 
        /// </summary>
        /// 

        #region old FillContactList committed for GEOS2-5070

        //private void FillContactList()
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method FillContactList ...", category: Category.Info, priority: Priority.Low);

        //        //[Sudhir.Jangra][GEOS2-4676]

        //        if (SRMCommon.Instance.SelectedAuthorizedWarehouseList != null)
        //        {
        //            PeopleContacts = new ObservableCollection<Contacts>();
        //            List<Warehouses> plantOwners = SRMCommon.Instance.SelectedAuthorizedWarehouseList.Cast<Warehouses>().ToList();
        //            foreach (var item in plantOwners)
        //            {
        //                var temp = new List<Contacts>(SRMService.GetContactsAndSupplier_V2430(item));
        //                if (temp != null)
        //                {
        //                    foreach (var tempList in temp.GroupBy(x => x.CountryList.Iso))
        //                    {
        //                        ImageSource countryFlagImage = ByteArrayToBitmapImage(tempList.ToList().FirstOrDefault().CountryList.CountryIconBytes);
        //                        tempList.ToList().Where(a => a.CountryList.Iso == tempList.Key).ToList().ForEach(x => x.CountryList.CountryIconImage = countryFlagImage);
        //                    }
        //                }
        //                PeopleContacts.AddRange(temp);
        //            }
        //        }

        //        GeosApplication.Instance.Logger.Log("Method FillContactList() executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (FaultException<ServiceException> ex)
        //    {
        //        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Get an error in FillContactList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
        //        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
        //    }
        //    catch (ServiceUnexceptedException ex)
        //    {
        //        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Get an error in FillContactList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
        //    }
        //}


        #endregion

        /// <summary>
        /// New Method for Fill all contact list depend on Sales Owner. 
        /// [GEOS2-5070][22-12-2023]
        /// </summary>
        /// 

        //[pramod.misal][GEOS2-5070][22-12-2023]
        private void FillContactList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillContactList ...", category: Category.Info, priority: Priority.Low);

                if (SRMCommon.Instance.SelectedAuthorizedWarehouseList != null)
                {
                    PeopleContacts = new ObservableCollection<Contacts>();
                    HashSet<int> uniqueContactIds = new HashSet<int>();

                    List<Warehouses> plantOwners = SRMCommon.Instance.SelectedAuthorizedWarehouseList.Cast<Warehouses>().ToList();

                    foreach (var item in plantOwners)
                    {
                        var temp = new List<Contacts>(SRMService.GetContactsAndSupplier_V2430(item));

                        if (temp != null)
                        {
                            foreach (var tempList in temp.GroupBy(x => x.CountryList.Iso))
                            {
                                ImageSource countryFlagImage = ByteArrayToBitmapImage(tempList.ToList().FirstOrDefault().CountryList.CountryIconBytes);
                                tempList.ToList().Where(a => a.CountryList.Iso == tempList.Key).ToList().ForEach(x => x.CountryList.CountryIconImage = countryFlagImage);
                            }
                        }

                        foreach (var contact in temp)
                        {
                            if (uniqueContactIds.Add(contact.IdContact))
                            {
                                PeopleContacts.Add(contact);
                            }
                        }
                    }

                }

                GeosApplication.Instance.Logger.Log("Method FillContactList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillContactList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillContactList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
        }

        /// <summary>
        /// Method for open MailTo in Outlook for send Email. 
        /// </summary>
        /// <param name="obj"></param>
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
                //Outlook.Application objApp = new Outlook.Application();
                //Outlook.MailItem mailItem = null;
                //mailItem = (Outlook.MailItem)objApp.CreateItem(OlItemType.olMailItem);
                //mailItem.To = emailAddess;
                IsBusy = false;
                //mailItem.Display(true);

                GeosApplication.Instance.Logger.Log("Method SendMailtoPerson() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in SendMailtoPerson() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for BestFit the grid and save and load Grid as per user.
        /// </summary>
        /// <param name="obj"></param>
        private void CustomCellAppearanceGridControl(RoutedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CustomCellAppearanceGridControl ...", category: Category.Info, priority: Priority.Low);

                int visibleFalseCoulumn = 0;

                if (File.Exists(ContactGridSettingFilePath))
                {
                    ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.RestoreLayoutFromXml(ContactGridSettingFilePath);
                }

                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.AddHandler(DXSerializer.AllowPropertyEvent, new AllowPropertyEventHandler(OnAllowProperty));

                //This code for save grid layout...
                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.SaveLayoutToXml(ContactGridSettingFilePath);

                GridControl gridControl = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;
                foreach (GridColumn column in gridControl.Columns)
                {
                    DependencyPropertyDescriptor descriptor = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleProperty, typeof(GridColumn));
                    if (descriptor != null)
                    {
                        descriptor.AddValueChanged(column, VisibleChanged);
                    }

                    DependencyPropertyDescriptor descriptorColumnPosition = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleIndexProperty, typeof(GridColumn));
                    if (descriptorColumnPosition != null)
                    {
                        descriptorColumnPosition.AddValueChanged(column, VisibleIndexChanged);
                    }

                    if (column.Visible == false)
                    {
                        visibleFalseCoulumn++;
                    }
                }

                if (visibleFalseCoulumn > 0)
                {
                    IsContactColumnChooserVisible = true;
                }
                else
                {
                    IsContactColumnChooserVisible = false;
                }

                TableView detailView = ((TableView)((System.Windows.RoutedEventArgs)obj).OriginalSource);
                detailView.BestFitColumns();

                GeosApplication.Instance.Logger.Log("Method CustomCellAppearanceGridControl() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CustomCellAppearanceGridControl() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for remove filter save on grid layout.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAllowProperty(object sender, AllowPropertyEventArgs e)
        {
            if (e.DependencyProperty == GridControl.FilterStringProperty)
                e.Allow = false;

            if (e.Property.Name == "GroupCount")
                e.Allow = false;

            if (e.DependencyProperty == TableView.SearchStringProperty)
                e.Allow = false;
        }

        /// <summary>
        /// Method for save Grid as per user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void VisibleChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method VisibleChanged ...", category: Category.Info, priority: Priority.Low);

                GridColumn column = sender as GridColumn;
                if (column.ShowInColumnChooser)
                {
                    ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(ContactGridSettingFilePath);
                }
                if (column.Visible == false)
                {
                    IsContactColumnChooserVisible = true;
                }

                GeosApplication.Instance.Logger.Log("Method VisibleChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in VisibleChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for save Grid as per user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void VisibleIndexChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method VisibleIndexChanged ...", category: Category.Info, priority: Priority.Low);

                GridColumn column = sender as GridColumn;
                ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(ContactGridSettingFilePath);

                GeosApplication.Instance.Logger.Log("Method VisibleIndexChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in VisibleIndexChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for print Contact grid.
        /// </summary>
        /// <param name="obj"></param>
        public void PrintAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintAction ...", category: Category.Info, priority: Priority.Low);
                IsBusy = true;
                PrintableControlLink pcl = new PrintableControlLink((TableView)obj);
                pcl.Margins.Bottom = 5;
                pcl.Margins.Top = 5;
                pcl.Margins.Left = 5;
                pcl.Margins.Right = 5;
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["ContactViewCustomPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["ContactViewCustomPrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);
                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                IsBusy = false;
                window.Show();
                GeosApplication.Instance.Logger.Log("Method PrintAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in PrintAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method convert BitmapImage to Image Source
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        ImageSource GetImage(string path)
        {
            return new BitmapImage(new Uri(path, UriKind.Relative));
        }

        /// <summary>
        /// For Filter grid Sellect All button Command.
        /// </summary>
        /// <param name="obj"></param>
        private void SelectAllAction(object obj)
        {
            SelectedLetter = null;
        }

        /// <summary>
        /// Method for open form for add new contact.
        /// </summary>
        /// <param name="obj"></param>
        private void AddContactViewWindowShow(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddContactViewWindowShow...", category: Category.Info, priority: Priority.Low);

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

                TableView tableView = (TableView)obj;
                AddContactViewModel addContactViewModel = new AddContactViewModel();
                AddContactView addContactView = new AddContactView();
                EventHandler handle = delegate { addContactView.Close(); };
                addContactViewModel.RequestClose += handle;
                addContactViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddContactViewHeader").ToString();
                addContactViewModel.IsSupplierComboBoxVisible = Visibility.Visible;//[Sudhir.Jangra][GEOS2-4676]
                addContactViewModel.IsSupplierTextEditVisible = Visibility.Collapsed;//[Sudhir.Jangra][GEOS2-4676]
                addContactViewModel.SupplierList = SupplierList;
                addContactViewModel.IsNew = true;
                addContactViewModel.AddTab();
                addContactView.DataContext = addContactViewModel;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                //IsContactColumnChooserVisible = false;
                var ownerInfo = (tableView as FrameworkElement);
                addContactView.Owner = Window.GetWindow(ownerInfo);
                addContactView.ShowDialogWindow();

                if (addContactViewModel.IsSave)
                {
                    FillCmbSalesOwner();
                }

                GeosApplication.Instance.Logger.Log("Method AddContactViewWindowShow() executed successfully...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddContactViewWindowShow() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void EditContactAction(object obj)
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
                        return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }

                TableView detailView = (TableView)obj;
                GridControl gridControl = (detailView).Grid;
                Contacts selectedRow = (Contacts)detailView.FocusedRow;

                AddContactViewModel editContactViewModel = new AddContactViewModel();
                AddContactView editContactView = new AddContactView();
                editContactViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditContactViewHeader").ToString();
                editContactViewModel.IsSupplierComboBoxVisible = Visibility.Visible;//[Sudhir.Jangra][GEOS2-4676]
                editContactViewModel.IsSupplierTextEditVisible = Visibility.Collapsed;//[Sudhir.Jangra][GEOS2-4676]
                editContactViewModel.IsNew = false;
                editContactViewModel.SupplierList = SupplierList;
                editContactViewModel.EditInit(selectedRow);
                EventHandler handle = delegate { editContactView.Close(); };
                editContactViewModel.RequestClose += handle;
                editContactView.DataContext = editContactViewModel;

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                var ownerInfo = (detailView as FrameworkElement);
                editContactView.Owner = Window.GetWindow(ownerInfo);
                editContactView.ShowDialogWindow();

                if (editContactViewModel.IsSave)
                {
                    FillCmbSalesOwner();
                }

                GeosApplication.Instance.Logger.Log("Method EditContactAction() executed successfully...", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EditContactAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ExportContactsGridButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportContactsGridButtonCommandAction ...", category: Category.Info, priority: Priority.Low);

                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Contacts";
                saveFile.Filter = "Excel Files (.xlsx)|*.xlsx|All Files (*.*)|*.*";
                saveFile.FilterIndex = 1;
                saveFile.Title = "Save Excel Report";
                DialogResult = (bool)saveFile.ShowDialog();

                if (!DialogResult)
                {
                    ResultFileName = string.Empty;
                }
                else
                {
                    IsBusy = true;

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

                    ResultFileName = (saveFile.FileName);
                    TableView contactTableView = ((TableView)obj);
                    contactTableView.ShowTotalSummary = false;
                    contactTableView.ShowFixedTotalSummary = false;
                    contactTableView.ExportToXlsx(ResultFileName);
                    IsBusy = false;

                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);
                    contactTableView.ShowTotalSummary = false;

                    contactTableView.ShowFixedTotalSummary = true;
                }

                GeosApplication.Instance.Logger.Log("Method ExportContactsGridButtonCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in ExportContactsGridButtonCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// [001][2018-08-22][skhade][CRM-M045-03] Add Sales owner column in contacts - Customized filter.
        /// </summary>
        /// <param name="e"></param>
        private void CustomShowFilterPopupAction(FilterPopupEventArgs e)
        {
            if (e.Column.FieldName != "SalesOwner")
            {
                return;
            }

            try
            {
                List<object> filterItems = new List<object>();

                if (e.Column.FieldName == "SalesOwner")
                {
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("IsNullOrEmpty([SalesOwner])")   // SalesOwner is equal to ' '
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("SalesOwner <> ' '")  // SalesOwner does not equal to ' '
                    });

                    foreach (Contacts people in PeopleContacts)
                    {
                        #region PrevCode
                        /*
                        if (people.Company.IdSalesResponsible == null && people.Company.IdSalesResponsibleAssemblyBU == null)
                        {
                            continue;
                        }
                        else if (people.Company.IdSalesResponsible != null && people.Company.IdSalesResponsible > 0 && people.Company.IdSalesResponsibleAssemblyBU == null)
                        {
                            if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == GeosApplication.Instance.PeopleList.Where(y => y.IdPerson == people.Company.IdSalesResponsible).Select(slt => slt.FullName).FirstOrDefault().Trim()))
                            {
                                CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                customComboBoxItem.DisplayValue = GeosApplication.Instance.PeopleList.Where(y => y.IdPerson == people.Company.IdSalesResponsible).Select(slt => slt.FullName).FirstOrDefault().Trim();
                                customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("SalesOwner Like '%{0}%'", GeosApplication.Instance.PeopleList.Where(y => y.IdPerson == people.Company.IdSalesResponsible).Select(slt => slt.FullName).FirstOrDefault().Trim()));
                                filterItems.Add(customComboBoxItem);
                            }
                        }
                        else if (people.Company.IdSalesResponsible == null && people.Company.IdSalesResponsibleAssemblyBU != null && people.Company.IdSalesResponsibleAssemblyBU > 0)
                        {
                            if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == GeosApplication.Instance.PeopleList.Where(y => y.IdPerson == people.Company.IdSalesResponsibleAssemblyBU).Select(slt => slt.FullName).FirstOrDefault().Trim()))
                            {
                                CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                customComboBoxItem.DisplayValue = GeosApplication.Instance.PeopleList.Where(y => y.IdPerson == people.Company.IdSalesResponsibleAssemblyBU).Select(slt => slt.FullName).FirstOrDefault().Trim();
                                customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("SalesOwner Like '%{0}%'", GeosApplication.Instance.PeopleList.Where(y => y.IdPerson == people.Company.IdSalesResponsibleAssemblyBU).Select(slt => slt.FullName).FirstOrDefault().Trim()));
                                filterItems.Add(customComboBoxItem);
                            }
                        }
                        else if (people.Company.IdSalesResponsible != null && people.Company.IdSalesResponsibleAssemblyBU != null)
                        {
                            if (people.Company.IdSalesResponsible > 0 &&
                                !filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == GeosApplication.Instance.PeopleList.Where(y => y.IdPerson == people.Company.IdSalesResponsible).Select(slt => slt.FullName).FirstOrDefault().Trim()))
                            {
                                CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                customComboBoxItem.DisplayValue = GeosApplication.Instance.PeopleList.Where(x => x.IdPerson == people.Company.IdSalesResponsible).Select(slt => slt.FullName).FirstOrDefault().Trim();
                                customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("SalesOwner Like '%{0}%'", GeosApplication.Instance.PeopleList.Where(y => y.IdPerson == people.Company.IdSalesResponsible).Select(slt => slt.FullName).FirstOrDefault().Trim()));
                                filterItems.Add(customComboBoxItem);
                            }
                            if (people.Company.IdSalesResponsibleAssemblyBU > 0 &&
                                !filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == GeosApplication.Instance.PeopleList.Where(y => y.IdPerson == people.Company.IdSalesResponsibleAssemblyBU).Select(slt => slt.FullName).FirstOrDefault().Trim()))
                            {
                                CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                customComboBoxItem.DisplayValue = GeosApplication.Instance.PeopleList.Where(y => y.IdPerson == people.Company.IdSalesResponsibleAssemblyBU).Select(slt => slt.FullName).FirstOrDefault().Trim();
                                customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("SalesOwner Like '%{0}%'", GeosApplication.Instance.PeopleList.Where(y => y.IdPerson == people.Company.IdSalesResponsibleAssemblyBU).Select(slt => slt.FullName).FirstOrDefault().Trim()));
                                filterItems.Add(customComboBoxItem);
                            }
                        }
                        */
                        #endregion
                        ////[rdixit][GEOS2-4280][05.05.2023]
                        //if (people.Company.SalesOwnerList != null)
                        //{
                        //    foreach (var item in people.Company.SalesOwnerList)
                        //    {
                        //        if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == item.Name.Trim()))
                        //        {
                        //            CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                        //            customComboBoxItem.DisplayValue = item.Name.Trim();
                        //            customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("SalesOwner Like '%{0}%'", item.Name.Trim()));
                        //            filterItems.Add(customComboBoxItem);
                        //        }
                        //    }
                        //}
                    }
                }

                e.ComboBoxEdit.ItemsSource = filterItems.OrderBy(sort => Convert.ToString(((CustomComboBoxItem)sort).DisplayValue)).ToList();
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CustomShowFilterPopupAction() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// [001][2018-08-22][skhade][CRM-M045-03] Add Sales owner column in contacts - Customized filter.
        /// </summary>
        /// <param name="e">The GridColumnDataEventArgs</param>
        private void CustomUnboundColumnDataAction(object e)
        {
            if (e == null) return;

            GridColumnDataEventArgs obj = e as GridColumnDataEventArgs;

            if (obj.Column.Name == "SalesOwner")
            {
                obj.Value = string.Format("{0}{1}{2}", obj.GetListSourceFieldValue("Company.People.FullName"), Environment.NewLine, obj.GetListSourceFieldValue("Company.PeopleSalesResponsibleAssemblyBU.FullName"));
            }
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        //[pmisal][GESO2-4325][17.04.2023]
        public void fillCountries()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method fillCountries ...", category: Category.Info, priority: Priority.Low);
                Countries = SRMService.GetAllCountriesDetails();
                GeosApplication.Instance.Logger.Log("Method fillCountries() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in fillCountries() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in fillCountries() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in fillCountries() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void DeleteCommandAction(object obj)
        {
            TableView detailView = (TableView)obj;
            Contacts selectedContact = (Contacts)detailView.FocusedRow;
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteCommandAction()...", category: Category.Info, priority: Priority.Low);
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(String.Format(Application.Current.Resources["SRMContact_delete"].ToString()), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    selectedContact.CountryList = new Country();
                    PeopleContacts.Remove(selectedContact);
                    bool IsDeleted = SRMService.DeleteContactDetails_V2340(selectedContact);

                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("SRMContact_DeletedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                }
                GeosApplication.Instance.Logger.Log("Method DeleteCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in DeleteCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in DeleteCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method DeleteCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public ImageSource ByteArrayToBitmapImage(byte[] byteArrayIn)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method convert byteArrayToImage ...", category: Category.Info, priority: Priority.Low);

                if (byteArrayIn == null || byteArrayIn.Length == 0) return null;
                var image = new System.Windows.Media.Imaging.BitmapImage();
                using (var mem = new System.IO.MemoryStream(byteArrayIn))
                {
                    mem.Position = 0;
                    image.BeginInit();
                    image.CreateOptions = System.Windows.Media.Imaging.BitmapCreateOptions.PreservePixelFormat;
                    image.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;
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

        //[sudhir.jangra][GEOS2-4676]
        public void FillSupplierList()
        {
            try
            {
                if (SRMCommon.Instance.SelectedAuthorizedWarehouseList != null)
                {
                    SupplierList = new ObservableCollection<ArticleSupplier>();
                    List<Warehouses> plantOwners = SRMCommon.Instance.SelectedAuthorizedWarehouseList.Cast<Warehouses>().ToList();
                    foreach (var item in plantOwners)
                    {
                        var temp = new ObservableCollection<ArticleSupplier>(SRMService.GetArticleSuppliersForSRMContact_V2430(item));
                        SupplierList.AddRange(temp);
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillSupplierList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion // Methods

    }
}
