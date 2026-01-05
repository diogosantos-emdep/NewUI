using DevExpress.Mvvm;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.Crm.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
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
using DevExpress.Xpf.Core;
using System.Data;
using DevExpress.Xpf.Core.Serialization;
using DevExpress.Spreadsheet;
using Microsoft.Win32;
using DevExpress.Mvvm.UI;
using Emdep.Geos.Data.Common.Epc;
using DevExpress.Data.Filtering;
using System.Threading.Tasks;
using DevExpress.Xpf.Map;
using Emdep.Geos.UI.Helper;
using DevExpress.Data.Filtering.Helpers;
using System.Text.RegularExpressions;
using Emdep.Geos.Utility;
using DevExpress.DataProcessing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;
namespace Emdep.Geos.Modules.Crm.ViewModels
{
    public class AccountViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable
    {
        #region Services

        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //ICrmService CrmStartUp = new CrmServiceController("localhost:6699");
        #endregion // Services

        #region Declaration
        List<Country> countries;
        ObservableCollection<Company> companies;
        public string AccountGridSettingFilePath = GeosApplication.Instance.UserSettingFolderName + "AccountGridSetting.Xml";
        bool isAccountColumnChooserVisible;
        bool isBusy;

        private string myFilterString;
        private Company selectedObject;
        private ObservableCollection<Emdep.Geos.UI.Helper.Column> columns;
        private List<LookupValue> BusinessProductList { get; set; }
        private ObservableCollection<Company> entireCompanyPlantList;
        private ObservableCollection<Customer> companyGroupList;
        bool saleOwnerAccount;//[001][kshinde][GEOS2-1902][04/07/2022]
        List<LookupValue> statusList;//chitra.girigosavi GEOS2-7242 31/03/2025
        private Company previousSelectedObject; //[pallavi.kale][GEOS2-8949][15.09.2025]
        private string moduleName;//[pallavi.kale][GEOS2-8949][15.09.2025]
        private ObservableCollection<TileBarFilters> listofitem;//[pallavi.kale][GEOS2-8957][29.09.2025]
        private string customFilterStringName;//[pallavi.kale][GEOS2-8957][29.09.2025]
        private List<GridColumn> gridColumnList;//[pallavi.kale][GEOS2-8957][29.09.2025]
        private bool isEdit;//[pallavi.kale][GEOS2-8957][29.09.2025]
        private string customFilterHTMLColor;//[pallavi.kale][GEOS2-8957][29.09.2025]
        private TileBarFilters selectedTileBarItem;//[pallavi.kale][GEOS2-8957][29.09.2025]
        private string userSettingsKeyForAccount = "CRM_Account_Filter_";//[pallavi.kale][GEOS2-8957][29.09.2025]
        private int visibleRowCount;//[pallavi.kale][GEOS2-8957][29.09.2025]
        private TileBarFilters previousSelectedTopTileBarItem;//[pallavi.kale][GEOS2-8957][29.09.2025]
        #endregion // Declaration

        #region  public Properties
        public List<Country> Countries
        {
            get { return countries; }
            set
            {
                countries = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Countries"));
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
        public ObservableCollection<Customer> CompanyGroupList
        {
            get { return companyGroupList; }
            set
            {
                companyGroupList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CompanyGroupList"));
            }
        }
        public List<LogEntryBySite> ChangedLogsEntries { get; set; }
        public string PreviouslySelectedSalesOwners { get; set; }
        public string PreviouslySelectedPlantOwners { get; set; }
        public IList<LookupValue> BusinessProduct { get; set; }
        public Company SelectedObject
        {
            get { return selectedObject; }
            set
            {
                //[pallavi.kale][GEOS2-8949][15.09.2025]
                if (PreviousSelectedObject!= null)
                    PreviousSelectedObject.IsSelectedRow = false;
                selectedObject = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedObject"));
                if (selectedObject != null && selectedObject.IsCoordinatesNull)
                {
                    selectedObject.IsSelectedRow = true;
                    PreviousSelectedObject = selectedObject;
                }
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
        public ObservableCollection<Company> Companies
        {
            get { return companies; }
            set
            {
                companies = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Companies"));
            }
        }

        public bool IsAccountColumnChooserVisible
        {
            get { return isAccountColumnChooserVisible; }
            set
            {
                isAccountColumnChooserVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAccountColumnChooserVisible"));
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

        public ObservableCollection<Emdep.Geos.UI.Helper.Column> Columns
        {
            get { return columns; }
            set
            {
                columns = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Columns"));
            }
        }

        // Export Excel .xlsx
        public virtual string ResultFileName { get; set; }
        public virtual bool DialogResult { get; set; }
        //public ISaveFileDialogService SaveFileDialogService { get { return this.GetService<ISaveFileDialogService>(); } }

        public bool SaleOwnerAccount
        {
            get { return saleOwnerAccount; }
            set
            {
                saleOwnerAccount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SaleOwnerAccount"));
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
        //[pallavi.kale][GEOS2-8949][15.09.2025]
        public Company PreviousSelectedObject
        {
            get { return previousSelectedObject; }
            set
            {
                previousSelectedObject = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PreviousSelectedObject"));
            }
        }

        //[pallavi.kale][GEOS2-8949][15.09.2025]
        public string ModuleName
        {
            get { return moduleName; }
            set
            {
                moduleName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ModuleName"));
            }
        }
        //[pallavi.kale][GEOS2-8957][29.09.2025]
        public ObservableCollection<TileBarFilters> Listofitem
        {
            get
            {
                return listofitem;
            }
            set
            {
                listofitem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Listofitem"));
            }
        }
        //[pallavi.kale][GEOS2-8957][29.09.2025]
        public string CustomFilterStringName
        {
            get { return customFilterStringName; }
            set
            {
                customFilterStringName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomFilterStringName"));
            }
        }
        //[pallavi.kale][GEOS2-8957][29.09.2025]
        public List<GridColumn> GridColumnList
        {
            get { return gridColumnList; }
            set
            {
                gridColumnList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GridColumnList"));
            }
        }
        //[pallavi.kale][GEOS2-8957][29.09.2025]
        public bool IsEdit
        {
            get { return isEdit; }
            set
            {
                isEdit = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEdit"));
            }
        }
        //[pallavi.kale][GEOS2-8957][29.09.2025]
        public string CustomFilterHTMLColor
        {
            get { return customFilterHTMLColor; }
            set
            {
                customFilterHTMLColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomFilterHTMLColor"));
            }
        }
        //[pallavi.kale][GEOS2-8957][29.09.2025]
        public TileBarFilters SelectedTileBarItem
        {
            get { return selectedTileBarItem; }
            set
            {
                selectedTileBarItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedTileBarItem"));
            }
        }
        //[pallavi.kale][GEOS2-8957][29.09.2025]
        public int VisibleRowCount
        {
            get { return visibleRowCount; }
            set
            {
                visibleRowCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("VisibleRowCount"));
            }
        }
        //[pallavi.kale][GEOS2-8957][29.09.2025]
        public TileBarFilters PreviousSelectedTopTileBarItem
        {
            get { return previousSelectedTopTileBarItem; }
            set
            {
                previousSelectedTopTileBarItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PreviousSelectedTopTileBarItem"));
            }
        }
        #endregion // Properties.

        #region public Events

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion // Events

        #region Public Commands

        public ICommand CustomCellAppearanceCommand { get; private set; }
        public ICommand PrintButtonCommand { get; set; }
        public ICommand HyperlinkForWebsite { get; set; }
        public ICommand HyperlinkForEmail { get; set; }
        public ICommand CommandNewAccountClick { get; set; }
        public ICommand CommandGridDoubleClick { get; set; }
        public ICommand SalesOwnerUnboundDataCommand { get; set; }
        public ICommand SalesOwnerCustomShowFilterPopup { get; set; }
        public ICommand SalesOwnerPopupClosedCommand { get; private set; }
        public ICommand PlantOwnerPopupClosedCommand { get; private set; }
        public ICommand RefreshAccountViewCommand { get; set; }
        public ICommand ExportButtonCommand { get; set; }
        public ICommand DisableAccountCommand { get; set; }
        public ICommand ImportAccountsCommand { get; set; }
        public ICommand AccountGridControlUnloadedCommand { get; set; } //[pallavi.kale][GEOS2-8949][15.09.2025]
        public ICommand LocationIconClickCommand { get; set; } //[pallavi.kale][GEOS2-8949][15.09.2025]
        public ICommand CommandShowFilterPopupClick { get; set; }//[pallavi.kale][GEOS2-8957][29.09.2025]
        public ICommand CommandTileBarDoubleClick { get; set; }//[pallavi.kale][GEOS2-8957][29.09.2025]
        public ICommand FilterEditorCreatedCommand { get; set; }//[pallavi.kale][GEOS2-8957][29.09.2025]
        #endregion // Commands

        #region Constructor

        public AccountViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor AccountViewModel ...", category: Category.Info, priority: Priority.Low);

                CustomCellAppearanceCommand = new DevExpress.Mvvm.DelegateCommand<RoutedEventArgs>(CustomCellAppearanceGridControl);

                CommandGridDoubleClick = new DelegateCommand<object>(EditAccountViewWindowShow);
                PrintButtonCommand = new Prism.Commands.DelegateCommand<object>(PrintAction);
                CommandNewAccountClick = new RelayCommand(new Action<object>(AddCustomerViewWindowShow));
                RefreshAccountViewCommand = new RelayCommand(new Action<object>(RefreshAccountDetails));
                HyperlinkForWebsite = new DelegateCommand<object>(new Action<object>((obj) => { OpenWebsite(obj); }));
                HyperlinkForEmail = new DelegateCommand<object>(new Action<object>((obj) => { SendMailtoPerson(obj); }));
                PlantOwnerPopupClosedCommand = new DevExpress.Mvvm.DelegateCommand<object>(PlantOwnerPopupClosedCommandAction);
                SalesOwnerCustomShowFilterPopup = new DelegateCommand<FilterPopupEventArgs>(SalesOwnerCustomShowFilterPopupAction);
                SalesOwnerPopupClosedCommand = new DevExpress.Mvvm.DelegateCommand<object>(SalesOwnerPopupClosedCommandAction);
                ExportButtonCommand = new RelayCommand(new Action<object>(ExportAccountsGridButtonCommandAction));
                DisableAccountCommand = new DelegateCommand<object>(DisableAccountRowCommandAction);
                ImportAccountsCommand = new RelayCommand(new Action<object>(ImportAccountViewWindowShow));
                AccountGridControlUnloadedCommand = new RelayCommand(new Action<object>(AccountGridControlUnloadedCommandAction)); //[pallavi.kale][GEOS2-8949][15.09.2025]
                LocationIconClickCommand = new DelegateCommand<object>(new Action<object>((obj) => { LocationIconClickCommandAction(obj); })); //[pallavi.kale][GEOS2-8949][15.09.2025]
                CommandShowFilterPopupClick = new DelegateCommand<object>(AccountShowFilterValue); //[pallavi.kale][GEOS2-8957][29.09.2025]
                CommandTileBarDoubleClick = new DelegateCommand<object>(CommandTileBarDoubleClickAction); //[pallavi.kale][GEOS2-8957][29.09.2025]
                FilterEditorCreatedCommand = new DelegateCommand<FilterEditorEventArgs>(FilterEditorCreatedCommandAction); //[pallavi.kale][GEOS2-8957][29.09.2025]
                FillCmbSalesOwner();
                FillTileBarList(); //[pallavi.kale][GEOS2-8957][29.09.2025]
                AddCustomSetting(); //[pallavi.kale][GEOS2-8957][29.09.2025]
                MyFilterString = string.Empty;
               
                GeosApplication.Instance.Logger.Log("Constructor AccountViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AccountViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion // Constructor.

        #region Methods

        /// <summary>
        /// Method for show data as per user permission.
        /// </summary>
        private void FillCmbSalesOwner()
        {
            if (GeosApplication.Instance.IdUserPermission == 21)
            {
                FillAccountListByUser();
            }
            else if (GeosApplication.Instance.IdUserPermission == 22)
            {
                FillAccountListByPlant();
            }
            else
            {
                FillAccountList();
            }
            fillCountries();
          
            BusinessProductList = CrmStartUp.GetLookupValues(7).ToList();
        }

        /// <summary>
        /// This method is used to get data by salesowner group
        /// </summary>
        /// <param name="obj"></param>
        /// 
        private void SalesOwnerPopupClosedCommandAction(object obj)
        {
            if (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
            {
                return;
            }
            Listofitem = new ObservableCollection<TileBarFilters>(); //[pallavi.kale][GEOS2-8957][29.09.2025]
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
                FillAccountListByUser();
                FillTileBarList(); //[pallavi.kale][GEOS2-8957][29.09.2025]
                AddCustomSetting(); //[pallavi.kale][GEOS2-8957][29.09.2025]
            }
            else
            {
                Companies = new ObservableCollection<Company>();
                FillTileBarList(); //[pallavi.kale][GEOS2-8957][29.09.2025]
                AddCustomSetting(); //[pallavi.kale][GEOS2-8957][29.09.2025]
            }

            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
        }

        /// <summary>
        /// Method for action after close plant owner popup.
        /// </summary>
        /// <param name="obj"></param>
        private void PlantOwnerPopupClosedCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method PlantOwnerPopupClosedCommandAction ...", category: Category.Info, priority: Priority.Low);

            if (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
            {
                return;
            }
            Listofitem = new ObservableCollection<TileBarFilters>(); //[pallavi.kale][GEOS2-8957][29.09.2025]
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

            if (GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
            {
                FillAccountListByPlant();
                FillTileBarList(); //[pallavi.kale][GEOS2-8957][29.09.2025]
                AddCustomSetting(); //[pallavi.kale][GEOS2-8957][29.09.2025]
            }
            else
            {
                Companies = new ObservableCollection<Company>();
                FillTileBarList(); //[pallavi.kale][GEOS2-8957][29.09.2025]
                AddCustomSetting(); //[pallavi.kale][GEOS2-8957][29.09.2025]
            }

            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
            GeosApplication.Instance.Logger.Log("Method PlantOwnerPopupClosedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Methdo for fill account details from database.
        /// </summary>
        private void FillAccountListByUser()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillAccountListByUser ...", category: Category.Info, priority: Priority.Low);

                if (GeosApplication.Instance.SelectedSalesOwnerUsersList != null)
                {
                    List<UserManagerDtl> salesOwners = GeosApplication.Instance.SelectedSalesOwnerUsersList.Cast<UserManagerDtl>().ToList();
                    var salesOwnersIds = string.Join(",", salesOwners.Select(i => i.IdUser));
                    PreviouslySelectedSalesOwners = salesOwnersIds;
                    #region Service Comments
                    //Companies = new ObservableCollection<Company>(CrmStartUp.GetSelectedUserCustomersBySalesOwnerId(salesOwnersIds));   //[rdixit][GEOS2-4324][11.04.2023]
                    //Companies = new ObservableCollection<Company>(CrmStartUp.GetSelectedUserCustomersBySalesOwnerId_V2380(salesOwnersIds));   //[rdixit][GEOS2-4279][05.05.2023]
                    //Companies = new ObservableCollection<Company>(CrmStartUp.GetSelectedUserCustomersBySalesOwnerId_V2390(salesOwnersIds)); //[rdixit][GEOS2-4712][01.08.2023]
                    //Companies = new ObservableCollection<Company>(CrmStartUp.GetSelectedUserCustomersBySalesOwnerId_V2420(salesOwnersIds));
                    //[Sudhir.Jangra][GEOS2-4664]
                    //Service GetSelectedUserCustomersBySalesOwnerId_V2430 updated with GetSelectedUserCustomersBySalesOwnerId_V2450 by [rdixit][GEOS2-4903][7.10.2023]
                    #endregion
                    //Companies = new ObservableCollection<Company>(CrmStartUp.GetSelectedUserCustomersBySalesOwnerId_V2450(salesOwnersIds));
                    //Companies = new ObservableCollection<Company>(CrmStartUp.GetSelectedUserCustomersBySalesOwnerId_V2630(salesOwnersIds));
                    //CrmStartUp = new CrmServiceController("localhost:6699");
                    //Companies = new ObservableCollection<Company>(CrmStartUp.GetSelectedUserCustomersBySalesOwnerId_V2670(salesOwnersIds));//[pallavi.kale][GEOS2-8949][15.09.2025]
                    Companies = new ObservableCollection<Company>(CrmStartUp.GetSelectedUserCustomersBySalesOwnerId_V2680(salesOwnersIds));//[pallavi.kale][GEOS2-9792][07.10.2025]
                    //[pallavi.kale][GEOS2-8949][15.09.2025]
                    foreach (Company item in Companies)
                    {
                        if (item.Latitude != null && item.Longitude != null)
                        {
                            item.IsCoordinatesNull = true;
                        }
                        else
                        {
                            item.IsCoordinatesNull = false;
                        }
                    }
                   
                    #region 
                    //SalesOwnerFill();//[rdixit][GEOS2-4279][05.05.2023]
                    //fill first customer from customers list.
                    //foreach (var item in Companies)
                    //{
                    //    item.Customer = item.Customers[0];
                    //}
                    #endregion
 
                    FillAccountAge();
                }
                else
                {
                    Companies = new ObservableCollection<Company>();
                }

                GeosApplication.Instance.Logger.Log("Method FillAccountListByUser executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillAccountListByUser() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillAccountListByUser() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
        }

        /// <summary>
        /// Method for fill account details from database by plant.
        /// </summary>
        private void FillAccountListByPlant()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillAccountListByPlant ...", category: Category.Info, priority: Priority.Low);
                if (GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
                {
                    List<Company> plantOwners = GeosApplication.Instance.SelectedPlantOwnerUsersList.Cast<Company>().ToList();
                    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.ConnectPlantId));
                    #region Service Comments
                    //Companies = CrmStartUp.GetCustomersBySalesOwnerId(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission);
                    //Companies = new ObservableCollection<Company>(CrmStartUp.GetSelectedUserCustomersByPlant(plantOwnersIds));   //[rdixit][GEOS2-4324][11.04.2023]
                    //Companies = new ObservableCollection<Company>(CrmStartUp.GetSelectedUserCustomersByPlant_V2380(plantOwnersIds));   //[rdixit][GEOS2-4279][05.05.2023]
                    //Service GetSelectedUserCustomersByPlant_V2390 updated with GetSelectedUserCustomersByPlant_V2420   //[rdixit][10.08.2023]
                    // Companies = new ObservableCollection<Company>(CrmStartUp.GetSelectedUserCustomersByPlant_V2420(plantOwnersIds));
                    //[Sudhir.jangra][GEOS2-4664]
                    //Service GetSelectedUserCustomersByPlant_V2430 Updated with GetSelectedUserCustomersByPlant_V2450 by 
                    #endregion
                    //Companies = new ObservableCollection<Company>(CrmStartUp.GetSelectedUserCustomersByPlant_V2450(plantOwnersIds));
                    //Companies = new ObservableCollection<Company>(CrmStartUp.GetSelectedUserCustomersByPlant_V2630(plantOwnersIds));
                    //CrmStartUp = new CrmServiceController("localhost:6699");
                    //Companies = new ObservableCollection<Company>(CrmStartUp.GetSelectedUserCustomersByPlant_V2670(plantOwnersIds));//[pallavi.kale][GEOS2-8949][15.09.2025]
                    Companies = new ObservableCollection<Company>(CrmStartUp.GetSelectedUserCustomersByPlant_V2680(plantOwnersIds));//[pallavi.kale][GEOS2-9792][07.10.2025]

                    //[pallavi.kale][GEOS2-8949][15.09.2025]
                    foreach (Company item in Companies)
                    {
                        if (item.Latitude != null && item.Longitude != null)
                        {
                            item.IsCoordinatesNull = true;
                        }
                        else
                        {
                            item.IsCoordinatesNull = false;
                        }
                    }
                  
                    #region
                    //SalesOwnerFill(); [rdixit][GEOS2-4279][05.05.2023]

                    //fill first customer from customers list.
                    //foreach (var item in Companies)
                    //{
                    //    item.Customer = item.Customers[0];
                    //}
                    #endregion
                    FillAccountAge();
                }
                else
                {
                    Companies = new ObservableCollection<Company>();
                }
                GeosApplication.Instance.Logger.Log("Method FillAccountListByPlant executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillAccountListByPlant() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillAccountListByPlant() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
        }
      
        /// <summary>
        /// Method for fill Account Age.
        /// </summary>
        private void FillAccountAge()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillAccountAge ...", category: Category.Info, priority: Priority.Low);
                int i = 0;
                if (Companies != null)
                {
                    foreach (Company age in Companies)
                    {
                        age.Customer = age.Customers[0];
                        Companies[i].Age = Math.Round((double)((GeosApplication.Instance.ServerDateTime.Month - Companies[i].CreatedIn.Value.Month) + 12 * (GeosApplication.Instance.ServerDateTime.Year - Companies[i].CreatedIn.Value.Year)) / 12, 1);
                        i++;
                    }
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillAccountAge() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillAccountAge() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillAccountAge() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Methdo for fill account details from database.
        /// </summary>
        private void FillAccountList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillAccountList ...", category: Category.Info, priority: Priority.Low);
                #region Service Comments
                //[rdixit][GEOS2-4324][11.04.2023]
                //Companies = new ObservableCollection<Company>(CrmStartUp.GetCustomersBySalesOwnerId(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission));
                //Companies = new ObservableCollection<Company>(CrmStartUp.GetCustomersBySalesOwnerId_V2380(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission)); [rdixit][GEOS2-4279][05.05.2023]
                //Companies = new ObservableCollection<Company>(CrmStartUp.GetCustomersBySalesOwnerId_V2390(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission)); [rdixit][GEOS2-4712][01.08.2023]                
                // Companies = new ObservableCollection<Company>(CrmStartUp.GetCustomersBySalesOwnerId_V2420(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission));
                //[Sudhir.jangra][GEOS2-4664]
                //Service GetCustomersBySalesOwnerId_V2430 updated with GetCustomersBySalesOwnerId_V2450 by [rdixit][GEOS2-4903][27.10.2023]
                #endregion
                //Companies = new ObservableCollection<Company>(CrmStartUp.GetCustomersBySalesOwnerId_V2630(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission));
                //CrmStartUp = new CrmServiceController("localhost:6699");
                //Companies = new ObservableCollection<Company>(CrmStartUp.GetCustomersBySalesOwnerId_V2670(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission));//[pallavi.kale][GEOS2-8949][15.09.2025]
                Companies = new ObservableCollection<Company>(CrmStartUp.GetCustomersBySalesOwnerId_V2680(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission));//[pallavi.kale][GEOS2-9792][07.10.2025]
                //[pallavi.kale][GEOS2-8949][15.09.2025]
                foreach (Company item in Companies)
                {
                    if (item.Latitude != null && item.Longitude != null)
                    {
                        item.IsCoordinatesNull = true;
                    }
                    else
                    {
                        item.IsCoordinatesNull = false;
                    }
                }
               
                #region
                //SalesOwnerFill(); [rdixit][GEOS2-4279][05.05.2023]
                //fill first customer from customers list.
                //foreach (var item in Companies)
                //{
                //    item.Customer = item.Customers[0];
                //}
                #endregion
 
                FillAccountAge();

                GeosApplication.Instance.Logger.Log("Method FillAccountList executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillAccountList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillAccountList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
        }

        /// <summary>
        /// Method for fill sales owner .
        /// </summary>
        private void SalesOwnerFill()
        {
            try
            {


                GeosApplication.Instance.Logger.Log("Method SalesOwnerFill ...", category: Category.Info, priority: Priority.Low);
                if (Companies != null)
                {
                    foreach (var item in Companies)
                    {
                        if (item.IdSalesResponsible != null)
                        {
                            item.People = new People();
                            PeopleDetails peopleDetails = new PeopleDetails();
                            peopleDetails = GeosApplication.Instance.PeopleList.Where(x => x.IdPerson == item.IdSalesResponsible).FirstOrDefault();
                            if (peopleDetails != null)
                            {
                                item.People.IdPerson = peopleDetails.IdPerson;
                                item.People.Name = peopleDetails.Name;
                                item.People.Surname = peopleDetails.Surname;
                                item.People.FullName = peopleDetails.FullName;
                                item.People.Email = peopleDetails.Email;
                            }

                        }

                        if (item.IdSalesResponsibleAssemblyBU != null)
                        {
                            PeopleDetails peopleDetails = new PeopleDetails();
                            item.PeopleSalesResponsibleAssemblyBU = new People();
                            peopleDetails = GeosApplication.Instance.PeopleList.Where(x => x.IdPerson == item.IdSalesResponsibleAssemblyBU).FirstOrDefault();
                            if (peopleDetails != null)
                            {

                                item.PeopleSalesResponsibleAssemblyBU.IdPerson = peopleDetails.IdPerson;
                                item.PeopleSalesResponsibleAssemblyBU.Name = peopleDetails.Name;
                                item.PeopleSalesResponsibleAssemblyBU.Surname = peopleDetails.Surname;
                                item.PeopleSalesResponsibleAssemblyBU.FullName = peopleDetails.FullName;
                                item.PeopleSalesResponsibleAssemblyBU.Email = peopleDetails.Email;
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            GeosApplication.Instance.Logger.Log("Method SalesOwnerFill() executed successfully", category: Category.Info, priority: Priority.Low);
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
        }

        /// <summary>
        /// This method is used to Display the Custom Filter for Sales Owner as 
        /// to display values of multiple properties in a single column.
        /// [001][2018-08-22][skhade] Solved bug in filter when testing.
        /// </summary>
        /// <param name="e">The FilterPopupEventArgs</param>
        private void SalesOwnerCustomShowFilterPopupAction(FilterPopupEventArgs e)
        {
            GeosApplication.Instance.Logger.Log(" Method SalesOwnerCustomShowFilterPopupAction ...", category: Category.Info, priority: Priority.Low);

            //[pramod.misal][GEOS2-1901][08-11-2023] 
            if (e.Column.FieldName != "SalesOwnerUnbound" && e.Column.FieldName != "BusinessProductString")
            {
                return;
            }

            try
            {
                List<object> filterItems = new List<object>();




                //SalesOwnerUnbound--
                //[pramod.misal][GEOS2-1901][08-11-2023]
                #region SalesOwnerUnbound
                if (e.Column.FieldName == "SalesOwnerUnbound")
                {
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("IsNullOrEmpty([SalesOwnerUnbound])")   // SalesOwner is equal to ' '
                    });
                    DevExpress.Xpf.Grid.TableView t = (DevExpress.Xpf.Grid.TableView)e.OriginalSource;
                    if (t.Grid.ActiveFilterInfo != null)
                    {
                        var temp = t.Grid.ActiveFilterInfo.FilterText;
                        var gridlist = t.Grid.VisibleItems;
                        HashSet<string> uniqueValues = new HashSet<string>();
                        foreach (Company item in gridlist)
                        {
                            if (item.SalesOwnerUnbound != null && item.SalesOwnerUnbound != "")
                            {
                                string[] sales = item.SalesOwnerUnbound.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                                foreach (string sale in sales)
                                {
                                    string displayValue = sale.Trim();
                                    if (uniqueValues.Add(displayValue))
                                    {
                                        CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                        customComboBoxItem.DisplayValue = displayValue;
                                        customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("SalesOwnerUnbound Like '%{0}%'", displayValue));
                                        filterItems.Add(customComboBoxItem);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        HashSet<string> uniqueValues = new HashSet<string>();
                        foreach (Company item in Companies)
                        {
                            if (item.SalesOwnerUnbound != null && item.SalesOwnerUnbound != "")
                            {
                                string[] sales = item.SalesOwnerUnbound.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                                foreach (string sale in sales)
                                {
                                    string displayValue = sale.Trim();
                                    if (uniqueValues.Add(displayValue))
                                    {
                                        CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                        customComboBoxItem.DisplayValue = displayValue;
                                        customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("SalesOwnerUnbound Like '%{0}%'", displayValue));
                                        filterItems.Add(customComboBoxItem);
                                    }
                                }
                            }
                        }

                    }

                }

                #endregion SalesOwnerUnbound

                //BusinessProductString//
                //[pramod.misal][GEOS2-1901][08-11-2023]
                #region  BusinessProductString
                else if (e.Column.FieldName == "BusinessProductString")
                {
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("BusinessProductString = ''")   // SalesOwner is equal to ' '
                    });
                    DevExpress.Xpf.Grid.TableView t = (DevExpress.Xpf.Grid.TableView)e.OriginalSource;
                    if (t.Grid.ActiveFilterInfo != null)
                    {
                        var temp = t.Grid.ActiveFilterInfo.FilterText;
                        var gridlist = t.Grid.VisibleItems;
                        HashSet<string> uniqueValues = new HashSet<string>();
                        foreach (Company item in gridlist)
                        {
                            if (!string.IsNullOrEmpty(item.BusinessProductString) && item.BusinessProductString != "")
                            {
                                string[] values = item.BusinessProductString.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                                foreach (string value in values)
                                {
                                    string displayValue = value.Trim();
                                    if (uniqueValues.Add(displayValue))
                                    {
                                        CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                        customComboBoxItem.DisplayValue = displayValue;
                                        customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("BusinessProductString Like '%{0}%'", displayValue));
                                        filterItems.Add(customComboBoxItem);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        HashSet<string> uniqueValues = new HashSet<string>();
                        foreach (Company item in Companies)
                        {
                            if (!string.IsNullOrEmpty(item.BusinessProductString) && item.BusinessProductString != "")
                            {
                                string[] values = item.BusinessProductString.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                                foreach (string value in values)
                                {
                                    string displayValue = value.Trim();
                                    if (uniqueValues.Add(displayValue))
                                    {
                                        CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                        customComboBoxItem.DisplayValue = displayValue;
                                        customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("BusinessProductString Like '%{0}%'", displayValue));
                                        filterItems.Add(customComboBoxItem);
                                    }
                                }
                            }
                        }

                    }

                }

                #endregion BusinessProductString



                e.ComboBoxEdit.ItemsSource = filterItems.OrderBy(sort => Convert.ToString(((CustomComboBoxItem)sort).DisplayValue)).ToList();
                GeosApplication.Instance.Logger.Log("Method SalesOwnerCustomShowFilterPopupAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SalesOwnerCustomShowFilterPopupAction() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void EditAccountViewWindowShow(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method LeadsEditViewWindowShow...", category: Category.Info, priority: Priority.Low);
            try
            {
                TableView detailView = (TableView)obj;
                GridControl gridControl = (detailView).Grid;

                if ((Company)detailView.FocusedRow != null)
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
                            return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                        }, null, null);
                    }

                    //List<Company> TempCompany =  Companies.Where(cpm => cpm.IdCompany == ((Company)obj).IdCompany).ToList();
                    List<Company> TempCompany = new List<Company>();
                    int companyId = Convert.ToInt32(((Company)detailView.FocusedRow).IdCompany);
                    // TempCompany.Add(CrmStartUp.GetCompanyDetailsById(companyId));
                    //[Sudhir.Jangra][GEOS2-4663][28/08/2023]
                    //TempCompany.Add(CrmStartUp.GetCompanyDetailsById_V2340(companyId));
                    //CrmStartUp = new CrmServiceController("localhost:6699");
                    //[pramod.misal][GEOS2-6462][19-11-2024]
                    //TempCompany.Add(CrmStartUp.GetCompanyDetailsById_V2580(companyId));
                    TempCompany.Add(CrmStartUp.GetCompanyDetailsById_V2630(companyId));


                    EditCustomerViewModel editCustomerViewModel = new EditCustomerViewModel();
                    EditCustomerView editCustomerView = new EditCustomerView();
                    editCustomerViewModel.InIt(TempCompany);
                    //[pallavi.kale][GEOS2-8953][18.09.2025]
                    if (ModuleName == "CRMModule")
                    {
                        editCustomerViewModel.IsContactEditAllowed = true;
                        if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 23))
                        {
                            editCustomerViewModel.IsPermissionManageCustomerContacts = true;
                            GeosApplication.Instance.IsPermissionEnabled = false;
                            editCustomerViewModel.IsPermissionEnableToAddActivity = false;
                            editCustomerViewModel.IsPermissionCustomerEdit = false;
                        }
                        else
                        {
                            editCustomerViewModel.IsPermissionManageCustomerContacts = false;
                            GeosApplication.Instance.IsPermissionEnabled = true;
                            editCustomerViewModel.IsPermissionEnableToAddActivity = true;
                            editCustomerViewModel.IsPermissionCustomerEdit = true;
                        }
                    }
                    else if (ModuleName == "TSMModule")
                    {
                        editCustomerViewModel.IsPermissionManageCustomerContacts = true;
                        editCustomerViewModel.IsPermissionCustomerReadOnly = true;
                        editCustomerViewModel.IsSourceButtonEnabled = false;
                        editCustomerViewModel.IsPermissionToManageCustomer = false;
                        editCustomerViewModel.IsPermissionEnableToAddActivity = false;
                        if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 148))
                        { 
                            editCustomerViewModel.IsContactEditAllowed = true;
                            GeosApplication.Instance.IsPermissionEnabled = true;
                            editCustomerViewModel.IsPermissionCustomerEdit = true;
                        }
                        else
                        {
                            editCustomerViewModel.IsContactEditAllowed = false;
                            GeosApplication.Instance.IsPermissionEnabled = false;
                            editCustomerViewModel.IsPermissionCustomerEdit = false;
                        }
                    }

                    EventHandler handle = delegate { editCustomerView.Close(); };
                    editCustomerViewModel.RequestClose += handle;
                    editCustomerView.DataContext = editCustomerViewModel;

                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                    {
                        DXSplashScreen.Close();
                    }
                    var ownerInfo = (obj as FrameworkElement);
                    editCustomerView.Owner = Window.GetWindow(ownerInfo);
                    editCustomerView.ShowDialog();

                    if (editCustomerViewModel.CustomerData != null)
                    {
                        //Company company = Companies.FirstOrDefault(cmpny => cmpny.IdCompany == companyId);
                        //[rdixit][GEOS2-4324][11.04.2023]
                        List<Company> co = new List<Company>(); Companies.ToList().ForEach(i => co.Add(i.Clone() as Company));
                        Company company = co.FirstOrDefault(cmpny => cmpny.IdCompany == companyId);

                        company.RegisteredName = editCustomerViewModel.CustomerData.RegisteredName;
                        company.CIF = editCustomerViewModel.CustomerData.CIF;//[GEOS2-3994][rdixit][17.11.2022]
                                                                             //   company.Size = editCustomerViewModel.CustomerData.Size;
                                                                             //  company.NumberOfEmployees = editCustomerViewModel.CustomerData.NumberOfEmployees;
                        company.SourceName = editCustomerViewModel.CustomerData.SourceName;//[Sudhir.jangra][GEOS2-4664]
                        company.Customer = editCustomerViewModel.CustomerData.Customers[0];
                        company.Customers = new List<Customer>(editCustomerViewModel.CustomerData.Customers);
                        company.IdCustomer = editCustomerViewModel.CustomerData.IdCustomer;
                        company.Name = editCustomerViewModel.CustomerData.Name;
                        company.IdBusinessCenter = editCustomerViewModel.CustomerData.IdBusinessCenter;
                        company.IdBusinessProduct = editCustomerViewModel.CustomerData.IdBusinessProduct;
                        company.IdBusinessField = editCustomerViewModel.CustomerData.IdBusinessField;
                        company.IdCountry = editCustomerViewModel.CustomerData.IdCountry;
                        company.Region = editCustomerViewModel.CustomerData.Region;
                        company.Website = editCustomerViewModel.CustomerData.Website;
                        company.Line = editCustomerViewModel.CustomerData.Line;
                        company.CuttingMachines = editCustomerViewModel.CustomerData.CuttingMachines;
                        company.Address = editCustomerViewModel.CustomerData.Address;
                        company.City = editCustomerViewModel.CustomerData.City;
                        company.ZipCode = editCustomerViewModel.CustomerData.ZipCode;
                        company.Telephone = editCustomerViewModel.CustomerData.Telephone;
                        company.Email = editCustomerViewModel.CustomerData.Email;
                        company.Fax = editCustomerViewModel.CustomerData.Fax;
                        company.BusinessField = editCustomerViewModel.CustomerData.BusinessField;
                        company.BusinessProductList = editCustomerViewModel.CustomerData.BusinessProductList;
                        company.BusinessProductString = editCustomerViewModel.CustomerData.BusinessProductString;
                        company.Latitude = editCustomerViewModel.CustomerData.Latitude;
                        company.Longitude = editCustomerViewModel.CustomerData.Longitude;
                        company.SalesOwnerUnbound = string.Join("\n", editCustomerViewModel.CustomerData.SalesOwnerList.Select(j => j.FullName).ToArray());//[rdixit][GEOS2-4279][05.05.2023]
                        company.SalesOwnerEnabled = string.Join("\n", editCustomerViewModel.CustomerData.SalesOwnerList.Select(j => j.FullName).ToArray());
                        company.SalesOwnerDisabled = null;
                        //[rdixit][GEOS2-4323][17.04.2023]
                        //editCustomerViewModel.CustomerData.Country.CountryIconBytes = Countries.FirstOrDefault(i => i.Name.ToLower() == editCustomerViewModel.CustomerData.Country.Name.ToLower()).CountryIconBytes;                
                        company.Country = editCustomerViewModel.CustomerData.Country;

                        //[pmisal][GEOS2-4324][27.04.2023]
                        company.Country.CountryIconUrl = "https://api.emdep.com/GEOS/Images?FilePath=/Images/Countries/" + Countries.FirstOrDefault(i => i.Name.ToLower() == editCustomerViewModel.CustomerData.Country.Name.ToLower()).Iso + ".png";

                        company.BusinessCenter = editCustomerViewModel.CustomerData.BusinessCenter;
                        company.ModifiedIn = editCustomerViewModel.CustomerData.ModifiedIn;

                        //if (editCustomerViewModel.CustomerData.People != null)
                        company.People = editCustomerViewModel.CustomerData.People;
                        #region //chitra.girigosavi[GEOS2-7242][10/04/2025]
                        if (company.Status == null)
                        {
                            company.Status = new LookupValue();
                        }
                        company.Status.Value = editCustomerViewModel.CustomerData.Status.Value;
                        company.Status.HtmlColor = editCustomerViewModel.CustomerData.Status.HtmlColor;
                        #endregion
                        //if (editCustomerViewModel.CustomerData.PeopleSalesResponsibleAssemblyBU != null)
                        company.PeopleSalesResponsibleAssemblyBU = editCustomerViewModel.CustomerData.PeopleSalesResponsibleAssemblyBU;
                        company.ContactCount = editCustomerViewModel.SiteContactsList.Count();//[pallavi.kale][GEOS2-8953][23.09.2025]
                        Companies = new ObservableCollection<Company>(co);  //[rdixit][GEOS2-4323][17.04.2023]
                        // Companies.Add(company);
                        SelectedObject = company;
                        gridControl.RefreshData();

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
                            GeosApplication.Instance.Logger.Log("Get an error in LeadsEditViewWindowShow()" + ex.Message, category: Category.Exception, priority: Priority.Low);
                        }

                    }
                    //[pallavi.kale][GEOS2-8953][23.09.2025]
                    else
                    {
                        List<Company> co = new List<Company>(); Companies.ToList().ForEach(i => co.Add(i.Clone() as Company));
                        Company company = co.FirstOrDefault(cmpny => cmpny.IdCompany == companyId);
                        company.ContactCount = editCustomerViewModel.SiteContactsList.Count();
                        Companies = new ObservableCollection<Company>(co);

                        SelectedObject = company;
                        gridControl.RefreshData();
                    }
                    // code for hide column chooser if empty

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
                        IsAccountColumnChooserVisible = true;
                    }
                    else
                    {
                        IsAccountColumnChooserVisible = false;
                    }
                }

                GeosApplication.Instance.Logger.Log("Method EditAccountViewWindowShow() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EditAccountViewWindowShow() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EditAccountViewWindowShow() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EditAccountViewWindowShow() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
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
                int visibleFalseCoulumn = 0;

                if (File.Exists(AccountGridSettingFilePath))
                {
                    ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.RestoreLayoutFromXml(AccountGridSettingFilePath);
                }

                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.AddHandler(DXSerializer.AllowPropertyEvent, new AllowPropertyEventHandler(OnAllowProperty));

                //This code for save grid layout...
                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.SaveLayoutToXml(AccountGridSettingFilePath);

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
                    IsAccountColumnChooserVisible = true;
                }
                else
                {
                    IsAccountColumnChooserVisible = false;
                }

                TableView datailView = ((TableView)((System.Windows.RoutedEventArgs)obj).OriginalSource);
                datailView.BestFitColumns();
               
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method CustomCellAppearanceGridControl() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in CustomCellAppearanceGridControl() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
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
                    ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(AccountGridSettingFilePath);
                }

                if (column.Visible == false)
                {
                    IsAccountColumnChooserVisible = true;
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
                ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(AccountGridSettingFilePath);

                GeosApplication.Instance.Logger.Log("Method VisibleIndexChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in VisibleIndexChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
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
                pcl.PaperKind = System.Drawing.Printing.PaperKind.A3;
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["AccountViewCustomPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["AccountViewCustomPrintFooterTemplate"];
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
        /// Method for add new customer.
        /// </summary>
        /// <param name="obj"></param>
        private void AddCustomerViewWindowShow(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method AddCustomerViewWindowShow...", category: Category.Info, priority: Priority.Low);

            try
            {
                if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    // DXSplashScreen.Show<SplashScreenView>(); 
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
                AddCustomerViewModel addCustomerViewModel = new AddCustomerViewModel();
                AddCustomerView addCustomerView = new AddCustomerView();
                EventHandler handle = delegate { addCustomerView.Close(); };
                addCustomerViewModel.RequestClose += handle;
                addCustomerView.DataContext = addCustomerViewModel;

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                //IsAccountColumnChooserVisible = false;
                var ownerInfo = (tableView as FrameworkElement);
                addCustomerView.Owner = Window.GetWindow(ownerInfo);
                addCustomerView.ShowDialog();

                if (addCustomerViewModel.CustomerData != null)
                {
                    List<Company> co = new List<Company>(); Companies.ToList().ForEach(i => co.Add(i.Clone() as Company));          //[rdixit][GEOS2-4323][17.04.2023]
                    Company company = new Company();
                    company.IdCompany = addCustomerViewModel.CustomerData.IdCompany;
                    company.Customer = addCustomerViewModel.CustomerData.Customers[0];
                    company.Customers = new List<Customer>(addCustomerViewModel.CustomerData.Customers);
                    company.Country = new Country();
                    company.Country = addCustomerViewModel.CustomerData.Country;
                    //company.Country.CountryIconBytes = Countries.FirstOrDefault(i => i.Name.ToLower() == company.Country.Name.ToLower()).CountryIconBytes;  //[rdixit][GEOS2-4323][17.04.2023]

                    //[pmisal][GEOS2-4324][26.04.2023]
                    company.Country.CountryIconUrl = "https://api.emdep.com/GEOS/Images?FilePath=/Images/Countries/" + Countries.FirstOrDefault(i => i.Name.ToLower() == company.Country.Name.ToLower()).Iso + ".png";

                    company.CIF = addCustomerViewModel.CustomerData.CIF;
                    company.RegisteredName = addCustomerViewModel.CustomerData.RegisteredName;
                    //company.Size = addCustomerViewModel.CustomerData.Size;
                    // company.NumberOfEmployees = addCustomerViewModel.CustomerData.NumberOfEmployees;


                    company.SourceName = addCustomerViewModel.CustomerData.SourceName;
                    company.IdCustomer = addCustomerViewModel.CustomerData.IdCustomer;
                    //CustomerData.IdCompany = Convert.ToInt32(CompanyPlantList[SelectedIndexCompanyPlant].IdCompany);
                    company.Name = addCustomerViewModel.CustomerData.Name;
                    company.IdBusinessCenter = addCustomerViewModel.CustomerData.IdBusinessCenter;
                    company.IdBusinessProduct = addCustomerViewModel.CustomerData.IdBusinessProduct;
                    company.IdBusinessField = addCustomerViewModel.CustomerData.IdBusinessField;
                    company.IdCountry = addCustomerViewModel.CustomerData.IdCountry;
                    company.Region = addCustomerViewModel.CustomerData.Region;
                    company.Website = addCustomerViewModel.CustomerData.Website;
                    company.Line = addCustomerViewModel.CustomerData.Line;
                    company.CuttingMachines = addCustomerViewModel.CustomerData.CuttingMachines;
                    company.Address = addCustomerViewModel.CustomerData.Address;
                    company.City = addCustomerViewModel.CustomerData.City;
                    company.ZipCode = addCustomerViewModel.CustomerData.ZipCode;
                    company.Telephone = addCustomerViewModel.CustomerData.Telephone;
                    company.Email = addCustomerViewModel.CustomerData.Email;
                    company.Fax = addCustomerViewModel.CustomerData.Fax;
                    company.BusinessProductString = addCustomerViewModel.CustomerData.BusinessProductString;
                    company.BusinessProductList = addCustomerViewModel.CustomerData.BusinessProductList;
                    company.BusinessField = addCustomerViewModel.CustomerData.BusinessField;
                    company.Latitude = addCustomerViewModel.CustomerData.Latitude;
                    company.Longitude = addCustomerViewModel.CustomerData.Longitude;
                    company.BusinessCenter = addCustomerViewModel.CustomerData.BusinessCenter;
                    company.CreatedIn = addCustomerViewModel.CustomerData.CreatedIn;
                    company.PeopleCreatedBy = new People();
                    company.PeopleCreatedBy.Name = addCustomerViewModel.CustomerData.PeopleCreatedBy.Name;
                    company.PeopleCreatedBy.Surname = addCustomerViewModel.CustomerData.PeopleCreatedBy.Surname;
                    company.SalesOwnerUnbound = string.Join("\n", addCustomerViewModel.CustomerData.SalesOwnerList.Select(j => j.FullName).ToArray());//[rdixit][GEOS2-4279][05.05.2023]
                    company.SalesOwnerEnabled = string.Join("\n", addCustomerViewModel.CustomerData.SalesOwnerList.Select(j => j.FullName).ToArray());
                    company.SalesOwnerDisabled = null;
                    if (addCustomerViewModel.CustomerData.People != null)
                        company.People = addCustomerViewModel.CustomerData.People;

                    #region //chitra.girigosavi[GEOS2-7242][10/04/2025]
                    if (company.Status == null)
                    {
                        company.Status = new LookupValue();
                    }
                    company.Status.Value = addCustomerViewModel.CustomerData.Status.Value;
                    company.Status.HtmlColor = addCustomerViewModel.CustomerData.Status.HtmlColor;
                    #endregion

                    co.Add(company);
                    Companies = new ObservableCollection<Company>(co);  //[rdixit][GEOS2-4323][17.04.2023]
                    SelectedObject = company;

                    // code for hide column chooser if empty
                    TableView detailView = ((TableView)obj);
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
                        IsAccountColumnChooserVisible = true;
                    }
                    else
                    {
                        IsAccountColumnChooserVisible = false;
                    }

                    detailView.Focus();

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

                            // CompanyGroupList = new ObservableCollection<Customer>(CrmStartUp.GetCompanyGroup(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission, true));
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
                        GeosApplication.Instance.Logger.Log("Method AddCustomerViewWindowShow() executed successfully" + ex.Message, category: Category.Exception, priority: Priority.Low);
                    }

                }

                GeosApplication.Instance.Logger.Log("Method AddCustomerViewWindowShow() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddCustomerViewWindowShow() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddCustomerViewWindowShow() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddCustomerViewWindowShow() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        private void DisableAccountRowCommandAction(object parameter)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DisableAccountRowCommandAction() ...", category: Category.Info, priority: Priority.Low);
                DXSplashScreen.Show<SplashScreenView>();
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                
                IsBusy = true;
                bool result = false;
                Company ObjCustomer = (Company)parameter;

                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["AccountDisableMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo, MessageBoxResult.No);
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    if (Companies != null && Companies.Count > 0)
                    {
                        ObjCustomer.IsStillActive = 0;

                        if (ObjCustomer.People != null)
                        {
                            ObjCustomer.People.OwnerImage = null;
                        }

                        if (ObjCustomer.PeopleSalesResponsibleAssemblyBU != null)
                        {
                            ObjCustomer.PeopleSalesResponsibleAssemblyBU.OwnerImage = null;
                        }

                        ChangedLogsEntries = new List<LogEntryBySite>();
                        ChangedLogsEntries.Add(new LogEntryBySite() { IdSite = ObjCustomer.IdCompany, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("AccountDisableChangeLog").ToString(), ObjCustomer.GroupPlantName), IdLogEntryBySite = 2 });
                        ObjCustomer.LogEntryBySites = ChangedLogsEntries;
                        ObjCustomer.OwnerImage = null; //[pallavi.kale][GEOS2-8949][15.09.2025]
                        ObjCustomer.OwnerImageBytes = null; //[pallavi.kale][GEOS2-8949][15.09.2025]
                        result = CrmStartUp.DisableAccount(ObjCustomer);
                        Companies.Remove((Company)ObjCustomer);
                        Listofitem = new ObservableCollection<TileBarFilters>();
                        AddCustomSetting();
                        FillTileBarList();
                    }
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AccountDisableSuccess").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    if (result)
                    {
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
                        IsBusy = false;
                        
                        GeosApplication.Instance.Logger.Log("Method DisableAccountRowCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
                    }
                    else
                    {
                        IsBusy = false;
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AccountDisableFailed").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                }
                else
                {
                    IsBusy = false;
                }
                
            }
            catch (FaultException<ServiceException> ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DisableAccountRowCommandAction() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DisableAccountRowCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DisableAccountRowCommandAction() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        // [nsatpute][25-06-2024][GEOS2-5701] Add new import accounts/contacts option (1/2) 
        private void ImportAccountViewWindowShow(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ImportContactViewWindowShow...", category: Category.Info, priority: Priority.Low);
                ShowPleaseWait();
                TableView detailView = (TableView)obj;
                ImportAccountViewModel importAccountViewModel = new ImportAccountViewModel();
                ImportAccountView importAccountView = new ImportAccountView();
                importAccountViewModel.Init(Companies);
                EventHandler handle = delegate { importAccountView.Close(); };
                importAccountViewModel.RequestClose += handle;
                importAccountView.DataContext = importAccountViewModel;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                var ownerInfo = (detailView as FrameworkElement);
                importAccountView.Owner = Window.GetWindow(ownerInfo);
                importAccountView.ShowDialogWindow();
                if (importAccountViewModel.IsSave)
                {
                    ShowPleaseWait();
                    RefreshAccountDetails(obj);
                    ScrollToEnd(obj);
                }
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method ImportContactViewWindowShow() executed successfully...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ImportContactViewWindowShow() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ScrollToEnd(object obj)
        {
            TableView view = ((TableView)obj);
            if (view != null)
            {
                int rowCount = view.DataControl.VisibleRowCount;
                if (rowCount > 0)
                {
                    view.FocusedRowHandle = rowCount - 1;
                    view.ScrollIntoView(rowCount - 1);
                }
            }
        }
        /// <summary>
        /// Method for refresh Account details.
        /// </summary>
        /// <param name="obj"></param>
        private void RefreshAccountDetails(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method RefreshAccountDetails...", category: Category.Info, priority: Priority.Low);
            Listofitem = new ObservableCollection<TileBarFilters>(); //[pallavi.kale][GEOS2-8957][29.09.2025]
            TableView detailView = (TableView)obj;
            GridControl gridControl = (detailView).Grid;

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
            FillTileBarList(); //[pallavi.kale][GEOS2-8957][29.09.2025]
            AddCustomSetting(); //[pallavi.kale][GEOS2-8957][29.09.2025]
            // code for hide column chooser if empty

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
                IsAccountColumnChooserVisible = true;
            }
            else
            {
                IsAccountColumnChooserVisible = false;
            }

            detailView.SearchString = null;
            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

            GeosApplication.Instance.Logger.Log("Method RefreshAccountDetails() executed successfully", category: Category.Info, priority: Priority.Low);
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

        private void ExportAccountsGridButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportAccountsGridButtonCommandAction ...", category: Category.Info, priority: Priority.Low);

                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Accounts";
                saveFile.Filter = "Excel Files (.xlsx)|*.xlsx|All Files (*.*)|*.*";
                saveFile.FilterIndex = 1;
                saveFile.Title = "Save Excel Report";
                DialogResult = (Boolean)saveFile.ShowDialog();

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

                    TableView accountTableView = ((TableView)obj);
                    accountTableView.ShowTotalSummary = false;
                    accountTableView.ShowFixedTotalSummary = false;

                    accountTableView.ExportToXlsx(ResultFileName);

                    IsBusy = false;
                    accountTableView.ShowTotalSummary = true;
                    accountTableView.ShowFixedTotalSummary = true;
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);
                }

                GeosApplication.Instance.Logger.Log("Method ExportAccountsGridButtonCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in ExportAccountsGridButtonCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[rdixit][GEOS2-4324][17.04.2023]
        public void fillCountries()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method fillCountries ...", category: Category.Info, priority: Priority.Low);
                Countries = CrmStartUp.GetAllCountriesDetails();
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
        public void Dispose()
        {
            throw new NotImplementedException();
        }
        // [nsatpute][03-07-204] GEOS2-5902 - Add new import accounts/contacts option (2/2) 
        private void ShowPleaseWait()
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
                    return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                }, null, null);
            }
        }
        //[pallavi.kale][GEOS2-8949][15.09.2025]
        private void AccountGridControlUnloadedCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AccountGridControlUnloadedCommandAction...", category: Category.Info, priority: Priority.Low);
                GridControl gridControl = obj as GridControl;
                TableView tableView = (TableView)gridControl.View;
                tableView.SearchString = string.Empty;
                if (gridControl.GroupCount > 0)
                    gridControl.ClearGrouping();
                gridControl.ClearSorting();
                gridControl.FilterString = null;
                gridControl.SaveLayoutToXml(AccountGridSettingFilePath);
                if (gridControl.DataContext is AccountViewModel vm)
                {
                    if (vm.ModuleName == "TSMModule")
                    {
                        GeosApplication.Instance.IsTSMCustomerViewVisible = false;
                        GeosApplication.Instance.CmbPlantOwnerUsers = Visibility.Collapsed;
                        GeosApplication.Instance.CmbSalesOwnerUsers = Visibility.Collapsed;
                    }
                    else if (vm.ModuleName == "CRMModule")
                    {
                        GeosApplication.Instance.IsTSMCustomerViewVisible = true;
                        if (GeosApplication.Instance.IdUserPermission == 21)
                        {
                            GeosApplication.Instance.SalesOwnerUsersList = WorkbenchStartUp.GetManagerUsers(GeosApplication.Instance.ActiveUser.IdUser);

                            GeosApplication.Instance.SelectedSalesOwnerUsersList = new List<object>();
                            UserManagerDtl usrDefault = GeosApplication.Instance.SalesOwnerUsersList.FirstOrDefault(x => x.IdUser == GeosApplication.Instance.ActiveUser.IdUser);

                            GeosApplication.Instance.CmbPlantOwnerUsers = Visibility.Hidden;
                            GeosApplication.Instance.CmbSalesOwnerUsers = Visibility.Visible;

                            if (usrDefault != null)
                            {
                                GeosApplication.Instance.SelectedSalesOwnerUsersList.Add(usrDefault);
                            }
                            else
                            {
                                GeosApplication.Instance.SelectedSalesOwnerUsersList.AddRange(GeosApplication.Instance.SalesOwnerUsersList);
                            }
                            GeosApplication.Instance.SelectedSalesOwnerUsersList = new List<object>(GeosApplication.Instance.SelectedSalesOwnerUsersList);
                        }
                        // End (SalesOwner)

                        // Start (PlantOwner) - Selected Plant Owners User list for CRM. 
                        if (GeosApplication.Instance.IdUserPermission == 22)
                        {

                            GeosApplication.Instance.PlantOwnerUsersList = CrmStartUp.GetAllCompaniesDetails_V2500(GeosApplication.Instance.ActiveUser.IdUser);
                            GeosApplication.Instance.PlantOwnerUsersList = GeosApplication.Instance.PlantOwnerUsersList.OrderBy(o => o.Country.IdCountry).ToList();
                            GeosApplication.Instance.SelectedPlantOwnerUsersList = new List<object>();

                            GeosApplication.Instance.CmbPlantOwnerUsers = Visibility.Visible;
                            GeosApplication.Instance.CmbSalesOwnerUsers = Visibility.Hidden;

                            string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.IsSelected == true).Select(xy => xy.Name).FirstOrDefault();
                            Company usrDefault = GeosApplication.Instance.PlantOwnerUsersList.FirstOrDefault(x => x.ShortName == serviceurl);
                            if (usrDefault != null)
                            {
                                GeosApplication.Instance.SelectedPlantOwnerUsersList.Add(usrDefault);
                            }
                            else
                            {
                                GeosApplication.Instance.SelectedPlantOwnerUsersList.AddRange(GeosApplication.Instance.PlantOwnerUsersList);
                            }
                            GeosApplication.Instance.SelectedPlantOwnerUsersList = new List<object>(GeosApplication.Instance.SelectedPlantOwnerUsersList);
                        }
                    }
                }
                GeosApplication.Instance.Logger.Log("Method AccountGridControlUnloadedCommandAction executed successfully...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error on AccountGridControlUnloadedCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
       
        //[pallavi.kale][GEOS2-8949][15.09.2025]
        private void LocationIconClickCommandAction(object obj)
        {
            try
            {
                if (obj is Company row && row.Latitude.HasValue && row.Longitude.HasValue)
                {
                    string url = $"https://www.google.com/maps?q={row.Latitude},{row.Longitude}";
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = url,
                        UseShellExecute = true
                    });
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error on LocationIconClickCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
               
            }
        }
        //[pallavi.kale][GEOS2-8957][29.09.2025]
        private void FillTileBarList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillTileBarList...", category: Category.Info, priority: Priority.Low);
                //CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                IList<LookupValue> temp = CrmStartUp.GetLookupValues(8).ToList();
                if (Listofitem == null)
                {
                    Listofitem = new ObservableCollection<TileBarFilters>();
                }
                if (Companies != null)
                {
                    foreach (var item in temp)
                    {
                        Listofitem.Add(new TileBarFilters()
                        {
                            Caption = item.Value,
                            Id = item.IdLookupValue,
                            BackColor = item.HtmlColor,
                            ForeColor = null,
                            FilterCriteria = $"[Country.Zone.Name] IN ('{item.Value}')",
                            EntitiesCount = Companies.Count(x => string.Equals(x.Country.Zone.Name.ToString(), item.Value, StringComparison.OrdinalIgnoreCase)),

                            EntitiesCountVisibility = Visibility.Visible,
                            Height = 60,
                            width = 230
                        });
                    }
                }

                Listofitem.Add(new TileBarFilters()
                {
                    Caption = (System.Windows.Application.Current.FindResource("CustomFilters").ToString()),
                    Id = 0,
                    BackColor = null,
                    ForeColor = null,
                    EntitiesCountVisibility = Visibility.Collapsed,
                    Height = 30,
                    width = 240,
                });

            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillTileBarList() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillTileBarList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillTileBarList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //[pallavi.kale][GEOS2-8957][29.09.2025]
        private void AccountShowFilterValue(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AccountShowFilterValue....", category: Category.Info, priority: Priority.Low);

                if (Listofitem.Count > 0)
                {
                    var temp = (System.Windows.Controls.SelectionChangedEventArgs)obj;
                    if (temp.AddedItems.Count > 0)
                    {
                        string str = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).DisplayText;
                        string Template = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).Type;
                        string _FilterString = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).FilterCriteria;
                        CustomFilterStringName = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).Caption;
                        CustomFilterHTMLColor = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).BackColor;

                        if (CustomFilterStringName.Equals(System.Windows.Application.Current.FindResource("CustomFilters").ToString()))
                            return;

                        if (str == null)
                        {
                            if (!string.IsNullOrEmpty(_FilterString))
                            {
                                if (!string.IsNullOrEmpty(_FilterString))
                                    MyFilterString = _FilterString;
                                else
                                    MyFilterString = string.Empty;
                            }
                            else
                                MyFilterString = string.Empty;
                        }
                        else
                        {

                            if (!string.IsNullOrEmpty(_FilterString))
                            {
                                if (!string.IsNullOrEmpty(_FilterString))
                                    MyFilterString = _FilterString;
                                else
                                    MyFilterString = string.Empty;
                            }
                            else
                                MyFilterString = string.Empty;

                        }
                    }

                }

                GeosApplication.Instance.Logger.Log("Method AccountShowFilterValue....executed successfully", category: Category.Info, priority: Priority.Low);
            }

            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AccountShowFilterValue...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //[pallavi.kale][GEOS2-8957][29.09.2025]
        private void CommandTileBarDoubleClickAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CommandTileBarDoubleClickAction()...", category: Category.Info, priority: Priority.Low);

                if (CustomFilterStringName == "CUSTOM FILTERS")
                {
                    return;
                }

                TableView table = (TableView)obj;
                GridControl gridControl = table.Grid;
                GridColumnList = gridControl.Columns.Where(x => x.Header != null).ToList();

                string filterContent = SelectedTileBarItem.FilterCriteria;

                string columnName = string.Empty;
                string columnValue = string.Empty;

                if (filterContent.StartsWith("StartsWith("))
                {
                    string filterContents = filterContent.Substring("StartsWith(".Length).TrimEnd(')');
                    string[] filterParts = filterContents.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    columnName = filterParts[0].Trim('[', ']');
                    columnValue = filterParts[1].Trim(' ', '\'');
                }
                else if (filterContent.StartsWith("EndsWith("))
                {
                    string filterContents = filterContent.Substring("EndsWith(".Length).TrimEnd(')');
                    string[] filterParts = filterContents.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    columnName = filterParts[0].Trim('[', ']');
                    columnValue = filterParts[1].Trim(' ', '\'');
                }
                else if (filterContent.Contains("Contains(") && !filterContent.Contains("Not Contains("))
                {
                    string filterContents = filterContent.Substring("Contains(".Length).TrimEnd(')');
                    string[] filterParts = filterContents.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    columnName = filterParts[0].Trim('[', ']');
                    columnValue = filterParts[1].Trim(' ', '\'');
                }
                else if (filterContent.Contains("Not Contains("))
                {
                    string filterContents = filterContent.Substring("Not Contains(".Length).TrimEnd(')');
                    string[] filterParts = filterContents.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    columnName = filterParts[0].Trim('[', ']');
                    columnValue = filterParts[1].Trim(' ', '\'');
                }
                else if (filterContent.Contains(" In ") && !(filterContent.Contains("Not") && filterContent.Contains("In")))
                {
                    int startColumnIndex = filterContent.IndexOf('[') + 1;
                    int endColumnIndex = filterContent.IndexOf(']');
                    columnName = filterContent.Substring(startColumnIndex, endColumnIndex - startColumnIndex).Trim();
                }
                else if (filterContent.Contains("Not") && filterContent.Contains("In"))
                {
                    int startIndex = filterContent.IndexOf('(') + 1;
                    int endIndex = filterContent.LastIndexOf(')');
                    string filterContents = filterContent.Substring(startIndex, endIndex - startIndex).Trim();

                    string[] filterParts = filterContent.Split(new[] { "Not", "In" }, StringSplitOptions.RemoveEmptyEntries);
                    columnName = filterParts[0].Trim('[', ']', ' ');

                }
                else if (filterContent.Contains("Not [") && filterContent.Contains("Between("))
                {
                    var filterContents = filterContent.Substring(filterContent.IndexOf("Not [") + "Not [".Length,
                        filterContent.IndexOf("Between(") - (filterContent.IndexOf("Not [") + "Not [".Length)).Trim();

                    var betweenContent = filterContent.Substring(filterContent.IndexOf("Between(") + "Between(".Length).TrimEnd(')');
                    var filterParts = betweenContent.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    if (filterParts.Length == 2)
                    {
                        columnName = filterContents.Trim(' ', '[', ']');
                        var lowerBound = filterParts[0].Trim().Trim('\'');
                        var upperBound = filterParts[1].Trim().Trim('\'');
                    }
                }
                else if (filterContent.Contains("Between("))
                {
                    string filterContents = filterContent.Substring(filterContent.IndexOf("Between(") + "Between(".Length).TrimEnd(')');

                    string[] filterParts = filterContents.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    if (filterParts.Length == 2)
                    {
                        columnName = filterContent.Substring(1, filterContent.IndexOf("]") - 1).Trim('[', ' ', ']');
                    }
                }
                else if (filterContent.Contains("<>"))
                {
                    var match = Regex.Match(filterContent, @"\[(.*?)\]\s*<>?\s*(-?\d+|'[^']*')", RegexOptions.IgnoreCase);
                    if (match.Success)
                    {
                        columnName = match.Groups[1].Value;
                        columnValue = match.Groups[2].Value;
                    }
                }
                else if (filterContent.Contains(">") && !(filterContent.Contains(">=")))
                {
                    var match = Regex.Match(filterContent, @"\[(.*?)\]\s*>\s*'(.*?)'", RegexOptions.IgnoreCase);

                    if (!match.Success)
                    {
                        match = Regex.Match(filterContent, @"\[(.*?)\]\s*>\s*(-?\d+)", RegexOptions.IgnoreCase);
                    }

                    if (!match.Success)
                    {
                        match = Regex.Match(filterContent, @"\[(.*?)\]\s*>\s*#(.*?)#", RegexOptions.IgnoreCase);
                    }
                    if (match.Success)
                    {
                        columnName = match.Groups[1].Value;
                        columnValue = match.Groups[2].Value;
                    }
                }

                else if (filterContent.Contains("<") && !(filterContent.Contains("<=")))
                {
                    var match = Regex.Match(filterContent, @"\[(.*?)\]\s*<\s*'(.*?)'", RegexOptions.IgnoreCase);

                    if (!match.Success)
                    {
                        match = Regex.Match(filterContent, @"\[(.*?)\]\s*<\s*(-?\d+)", RegexOptions.IgnoreCase);
                    }

                    if (!match.Success)
                    {
                        match = Regex.Match(filterContent, @"\[(.*?)\]\s*<\s*#(.*?)#", RegexOptions.IgnoreCase);
                    }
                    if (match.Success)
                    {
                        columnName = match.Groups[1].Value;
                        columnValue = match.Groups[2].Value;
                    }
                }
                else if (filterContent.Contains(">="))
                {
                    var match = Regex.Match(filterContent, @"\[(.*?)\]\s*>=\s*'(.*?)'", RegexOptions.IgnoreCase);

                    if (!match.Success)
                    {
                        match = Regex.Match(filterContent, @"\[(.*?)\]\s*>=\s*(-?\d+)", RegexOptions.IgnoreCase);
                    }

                    if (!match.Success)
                    {
                        match = Regex.Match(filterContent, @"\[(.*?)\]\s*>=\s*#(.*?)#", RegexOptions.IgnoreCase);
                    }
                    if (match.Success)
                    {
                        columnName = match.Groups[1].Value;
                        columnValue = match.Groups[2].Value;
                    }
                }
                else if (filterContent.Contains("<="))
                {
                    var match = Regex.Match(filterContent, @"\[(.*?)\]\s*<=\s*'(.*?)'", RegexOptions.IgnoreCase);

                    if (!match.Success)
                    {
                        match = Regex.Match(filterContent, @"\[(.*?)\]\s*<=\s*(-?\d+)", RegexOptions.IgnoreCase);
                    }

                    if (!match.Success)
                    {
                        match = Regex.Match(filterContent, @"\[(.*?)\]\s*<=\s*#(.*?)#", RegexOptions.IgnoreCase);
                    }
                    if (match.Success)
                    {
                        columnName = match.Groups[1].Value;
                        columnValue = match.Groups[2].Value;
                    }
                }
                else if (filterContent.Contains("Not IsNullOrEmpty"))
                {
                    columnName = filterContent.Split(new[] { '[' }, StringSplitOptions.RemoveEmptyEntries)[1].Split(']')[0].Trim();
                }
                else if (filterContent.Contains("IsNullOrEmpty"))
                {
                    columnName = filterContent.Split(new[] { '[' }, StringSplitOptions.RemoveEmptyEntries)[1].Split(']')[0].Trim();
                }
                else if (filterContent.Contains("="))
                {
                    var match = Regex.Match(filterContent, @"\[(.*?)\]\s*=\s*(?:'(.*?)'|(-?\d+(\.\d+)?)|#(.*?)#)", RegexOptions.IgnoreCase);
                    if (match.Success)
                    {
                        columnName = match.Groups[1].Value;
                        columnValue = match.Groups[2].Value;
                    }
                }
                else if (filterContent.Contains("!=") || filterContent.Contains("<>"))
                {
                    var match = Regex.Match(filterContent, @"\[(.*?)\]\s*(!=|<>)\s*'([^']*)'", RegexOptions.IgnoreCase);
                    if (match.Success)
                    {
                        columnName = match.Groups[1].Value;
                        columnValue = match.Groups[3].Value;
                    }
                }
                else if (filterContent.Contains(" Like ") || filterContent.Contains(" Not Like "))
                {
                    columnName = filterContent.Substring(1, filterContent.IndexOf("]") - 1).Trim('[', ' ', ']');
                }
                else if (filterContent.Contains("Is Null"))
                {
                    int startIndex = filterContent.IndexOf('[');
                    int endIndex = filterContent.IndexOf(']');
                    columnName = filterContent.Substring(startIndex + 1, endIndex - startIndex - 1).Trim();
                }

                else if (filterContent.Contains("Is Not Null"))
                {
                    int startIndex = filterContent.IndexOf('[');
                    int endIndex = filterContent.IndexOf(']');
                    columnName = filterContent.Substring(startIndex + 1, endIndex - startIndex - 1).Trim();
                }

                string normalizedColumnName = columnName.Replace(" ", "").ToLower();

                GridColumn column = GridColumnList.FirstOrDefault(x =>
                    x.Header.ToString().Replace(" ", "").ToLower().Equals(normalizedColumnName));

                if (column == null)
                {
                    column = GridColumnList.FirstOrDefault(x =>
                        x.FieldName.Replace(" ", "").ToLower().Equals(normalizedColumnName));
                }

                if (column != null)
                {
                    IsEdit = true;
                    table.ShowFilterEditor(column);
                }

                GeosApplication.Instance.Logger.Log("Method CommandTileBarDoubleClickAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CommandTileBarDoubleClickAction() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //[pallavi.kale][GEOS2-8957][29.09.2025]
        private void FilterEditorCreatedCommandAction(FilterEditorEventArgs obj)
        {
            obj.Handled = true;
            TableView table = (TableView)obj.OriginalSource;
            GridControl gridControl = (table).Grid;
            ShowFilterEditor(obj);
        }

        //[pallavi.kale][GEOS2-8957][29.09.2025]
        private void AddCustomSetting()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddCustomSetting()...", category: Category.Info, priority: Priority.Low);
                List<KeyValuePair<string, string>> tempUserSettings = new List<KeyValuePair<string, string>>();
                if (Listofitem == null)
                {
                    Listofitem = new ObservableCollection<TileBarFilters>();
                }

                tempUserSettings = GeosApplication.Instance.UserSettings.Where(x => x.Key.Contains(userSettingsKeyForAccount)).ToList();

                if (tempUserSettings != null && tempUserSettings.Count > 0)
                {

                    var colorMapping = new Dictionary<string, string>();

                    foreach (var item in tempUserSettings)
                    {
                        var parts = item.Key.Split(new[] { "_#" }, StringSplitOptions.None);

                        if (parts.Length > 1)
                        {
                            string baseKey = parts[0].Replace(userSettingsKeyForAccount, "");
                            string colorCode = "#" + parts[1];

                            colorMapping[baseKey] = colorCode;
                        }
                    }

                    foreach (var item in tempUserSettings)
                    {
                        try
                        {
                            string filter = item.Value;
                            var parts = item.Key.Split(new[] { "_#" }, StringSplitOptions.None);
                            string baseKey = parts[0].Replace(userSettingsKeyForAccount, "");

                            string backColor = colorMapping.ContainsKey(baseKey) ? colorMapping[baseKey] : null;

                            bool isDuplicate = Listofitem.Any(tile => tile.Caption == baseKey && tile.FilterCriteria == item.Value);

                            if (!isDuplicate)
                            {
                                CriteriaOperator op = CriteriaOperator.Parse(filter);

                                int count = 0;

                                if (filter.StartsWith("StartsWith("))
                                {
                                    string filterContent = filter.Substring("StartsWith(".Length).TrimEnd(')');
                                    string[] filterParts = filterContent.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                    string columnName = filterParts[0].Trim('[', ']');
                                    string columnValue = filterParts[1].Trim(' ', '\'');
                                    count = Companies.Count(ap =>
                                    {
                                        object obj = ap;

                                        foreach (var part in columnName.Split('.'))
                                        {
                                            if (obj == null) break;
                                            var prop = obj.GetType().GetProperty(part);
                                            obj = prop?.GetValue(obj);
                                        }

                                        var value = obj?.ToString();
                                        return value != null && value.StartsWith(columnValue, StringComparison.OrdinalIgnoreCase);
                                    });

                                }
                                else if (filter.StartsWith("EndsWith("))
                                {
                                    string filterContent = filter.Substring("EndsWith(".Length).TrimEnd(')');
                                    string[] filterParts = filterContent.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                    string columnName = filterParts[0].Trim('[', ']');
                                    string columnValue = filterParts[1].Trim(' ', '\'');
                                    count = Companies.Count(ap =>
                                    {
                                        object obj = ap;

                                        foreach (var part in columnName.Split('.'))
                                        {
                                            if (obj == null) break;
                                            var prop = obj.GetType().GetProperty(part);
                                            obj = prop?.GetValue(obj);
                                        }

                                        var value = obj?.ToString();
                                        return value != null && value.EndsWith(columnValue, StringComparison.OrdinalIgnoreCase);
                                    });
                                }
                                else if (filter.Contains("Contains(") && !filter.Contains("Not Contains("))
                                {
                                    string filterContent = filter.Substring("Contains(".Length).TrimEnd(')');
                                    string[] filterParts = filterContent.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                    string columnName = filterParts[0].Trim('[', ']');
                                    string columnValue = filterParts[1].Trim(' ', '\'');
                                    count = Companies.Count(ap =>
                                    {
                                        object obj = ap;

                                        foreach (var part in columnName.Split('.'))
                                        {
                                            if (obj == null) break;
                                            var prop = obj.GetType().GetProperty(part);
                                            obj = prop?.GetValue(obj);
                                        }

                                        var value = obj?.ToString();
                                        return value != null && value.IndexOf(columnValue, StringComparison.OrdinalIgnoreCase) >= 0;
                                    });
                                }
                                else if (filter.Contains("Not Contains("))
                                {
                                    string filterContent = filter.Substring("Not Contains(".Length).TrimEnd(')');
                                    string[] filterParts = filterContent.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                    string columnName = filterParts[0].Trim('[', ']');
                                    string columnValue = filterParts[1].Trim(' ', '\'');

                                    count = Companies.Count(ap =>
                                    {
                                        object obj = ap;

                                        foreach (var part in columnName.Split('.'))
                                        {
                                            if (obj == null) break;
                                            var prop = obj.GetType().GetProperty(part);
                                            obj = prop?.GetValue(obj);
                                        }

                                        var value = obj?.ToString();
                                        return value != null && value.IndexOf(columnValue, StringComparison.OrdinalIgnoreCase) < 0;
                                    });
                                }
                                else if (filter.Contains(" In ") && !(filter.Contains("Not") && filter.Contains("In")))
                                {
                                    int startColumnIndex = filter.IndexOf('[') + 1;
                                    int endColumnIndex = filter.IndexOf(']');
                                    string columnName = filter.Substring(startColumnIndex, endColumnIndex - startColumnIndex).Trim();

                                    int startValuesIndex = filter.IndexOf("In (", StringComparison.OrdinalIgnoreCase) + "In (".Length;
                                    int endValuesIndex = filter.IndexOf(')', startValuesIndex);
                                    string filterContent = filter.Substring(startValuesIndex, endValuesIndex - startValuesIndex).Trim();

                                    var columnValues = filterContent
          .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
          .Select(v => v.Trim().Trim('\'', '#'))
          .ToList();

                                    count = Companies.Count(ap =>
                                    {
                                        object obj = ap;
                                        foreach (var part in columnName.Split('.'))
                                        {
                                            if (obj == null) break;
                                            var prop = obj.GetType().GetProperty(part);
                                            obj = prop?.GetValue(obj);
                                        }

                                        if (obj == null) return false;
                                        var value = obj;
                                        var propertyType = value.GetType();

                                        if (value != null)
                                        {
                                            if (propertyType == typeof(string))
                                            {
                                                return columnValues.Contains(value.ToString(), StringComparer.OrdinalIgnoreCase);
                                            }
                                            else if (propertyType == typeof(DateTime) || propertyType == typeof(DateTime?))
                                            {
                                                var dateValue = (DateTime)value;
                                                return columnValues.Contains(dateValue.ToString("yyyy-MM-dd"), StringComparer.OrdinalIgnoreCase);
                                            }
                                            else if (typeof(IComparable).IsAssignableFrom(propertyType))
                                            {
                                                foreach (var columnValue in columnValues)
                                                {
                                                    try
                                                    {
                                                        var targetValue = Convert.ChangeType(columnValue, propertyType);
                                                        if (value.Equals(targetValue))
                                                            return true;
                                                    }
                                                    catch
                                                    {
                                                        // ignore conversion exceptions
                                                    }
                                                }
                                            }
                                        }

                                        return false;
                                    });
                                }


                                else if (filter.Contains("Not") && filter.Contains("In"))
                                {
                                    int startIndex = filter.IndexOf('(') + 1;
                                    int endIndex = filter.LastIndexOf(')');
                                    string filterContent = filter.Substring(startIndex, endIndex - startIndex).Trim();

                                    string[] filterParts = filter.Split(new[] { "Not", "In" }, StringSplitOptions.RemoveEmptyEntries);
                                    string columnName = filterParts[0].Trim('[', ']', ' ');

                                    var columnValues = filterContent
                                        .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                        .Select(v => v.Trim().Trim('\'', ' ', '#'))
                                        .ToArray();

                                    count = Companies.Count(ap =>
                                    {
                                        object obj = ap;

                                        foreach (var part in columnName.Split('.'))
                                        {
                                            if (obj == null) break;
                                            var prop = obj.GetType().GetProperty(part);
                                            obj = prop?.GetValue(obj);
                                        }

                                        if (obj == null) return true;

                                        var value = obj;
                                        var propertyType = value.GetType();

                                        if (value != null)
                                        {
                                            if (propertyType == typeof(string))
                                            {
                                                return !columnValues.Contains(value.ToString(), StringComparer.OrdinalIgnoreCase);
                                            }
                                            else if (propertyType == typeof(DateTime) || propertyType == typeof(DateTime?))
                                            {
                                                var dateValue = (DateTime)value;
                                                return !columnValues.Contains(dateValue.ToString("yyyy-MM-dd"), StringComparer.OrdinalIgnoreCase);
                                            }
                                            else if (typeof(IComparable).IsAssignableFrom(propertyType))
                                            {
                                                foreach (var columnValue in columnValues)
                                                {
                                                    try
                                                    {
                                                        var targetValue = Convert.ChangeType(columnValue, propertyType);
                                                        if (value.Equals(targetValue))
                                                        {
                                                            return false;
                                                        }
                                                    }
                                                    catch
                                                    {
                                                        // ignore conversion errors
                                                    }
                                                }
                                                return true;
                                            }
                                        }

                                        return true;
                                    });
                                }

                                else if (filter.Contains("Not [") && filter.Contains("Between("))
                                {
                                    var filterContent = filter.Substring(filter.IndexOf("Not [") + "Not [".Length,
                                        filter.IndexOf("Between(") - (filter.IndexOf("Not [") + "Not [".Length)).Trim();

                                    var betweenContent = filter.Substring(filter.IndexOf("Between(") + "Between(".Length).TrimEnd(')');
                                    var filterParts = betweenContent.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                                    if (filterParts.Length == 2)
                                    {
                                        var columnName = filterContent.Trim(' ', '[', ']');
                                        var lowerBound = filterParts[0].Trim().Trim('\'');
                                        var upperBound = filterParts[1].Trim().Trim('\'');

                                        count = Companies.Count(ap =>
                                        {
                                            object obj = ap;
                                            foreach (var part in columnName.Split('.'))
                                            {
                                                if (obj == null) break;
                                                var prop = obj.GetType().GetProperty(part);
                                                obj = prop?.GetValue(obj);
                                            }

                                            if (obj == null) return false;

                                            var value = obj;
                                            var propertyType = value.GetType();

                                            if (value != null)
                                            {
                                                if (propertyType == typeof(string))
                                                {
                                                    var stringValue = value as string;
                                                    return string.Compare(stringValue, lowerBound, StringComparison.OrdinalIgnoreCase) < 0 ||
                                                           string.Compare(stringValue, upperBound, StringComparison.OrdinalIgnoreCase) > 0;
                                                }
                                                else if (propertyType == typeof(DateTime) || propertyType == typeof(DateTime?))
                                                {
                                                    DateTime lowerDate, upperDate;

                                                    bool lowerParsed = DateTime.TryParse(lowerBound, System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.None, out lowerDate);
                                                    bool upperParsed = DateTime.TryParse(upperBound, System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.None, out upperDate);

                                                    if (value is DateTime?)
                                                    {
                                                        DateTime? nullableDateTime = (DateTime?)value;
                                                        return !nullableDateTime.HasValue ||
                                                               nullableDateTime.Value.CompareTo(lowerDate) < 0 ||
                                                               nullableDateTime.Value.CompareTo(upperDate) > 0;
                                                    }
                                                    else
                                                    {
                                                        return ((DateTime)value).CompareTo(lowerDate) < 0 ||
                                                               ((DateTime)value).CompareTo(upperDate) > 0;
                                                    }
                                                }
                                                else if (typeof(IComparable).IsAssignableFrom(propertyType))
                                                {
                                                    var comparable = (IComparable)value;

                                                    var lowerBoundValue = Convert.ChangeType(lowerBound, propertyType);
                                                    var upperBoundValue = Convert.ChangeType(upperBound, propertyType);

                                                    return comparable.CompareTo(lowerBoundValue) < 0 ||
                                                           comparable.CompareTo(upperBoundValue) > 0;
                                                }
                                            }

                                            return false;
                                        });
                                    }
                                }

                                else if (filter.Contains("Between("))
                                {
                                    string filterContent = filter.Substring(filter.IndexOf("Between(") + "Between(".Length).TrimEnd(')');

                                    string[] filterParts = filterContent.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                    if (filterParts.Length == 2)
                                    {
                                        string columnName = filter.Substring(1, filter.IndexOf("]") - 1).Trim('[', ' ', ']');

                                        var lowerBound = filterParts[0].Trim().Trim('\'', ' ');
                                        var upperBound = filterParts[1].Trim().Trim('\'', ' ');

                                        count = Companies.Count(ap =>
                                        {
                                            object obj = ap;

                                            foreach (var part in columnName.Split('.'))
                                            {
                                                if (obj == null) break;
                                                var prop = obj.GetType().GetProperty(part);
                                                obj = prop?.GetValue(obj);
                                            }

                                            if (obj == null) return false;

                                            var value = obj;
                                            var propertyType = Nullable.GetUnderlyingType(value.GetType()) ?? value.GetType();

                                            if (propertyType == typeof(string))
                                            {
                                                var stringValue = value as string;
                                                return string.Compare(stringValue, lowerBound, StringComparison.OrdinalIgnoreCase) >= 0 &&
                                                       string.Compare(stringValue, upperBound, StringComparison.OrdinalIgnoreCase) <= 0;
                                            }

                                            if (propertyType == typeof(DateTime))
                                            {
                                                if (!DateTime.TryParse(lowerBound, out var lowerDate) ||
                                                    !DateTime.TryParse(upperBound, out var upperDate))
                                                    return false;

                                                DateTime actualDate = (value is DateTime dt) ? dt : ((DateTime?)value ?? DateTime.MinValue);
                                                return actualDate >= lowerDate && actualDate <= upperDate;
                                            }

                                            if (value is IComparable comparable)
                                            {
                                                var lowerValue = Convert.ChangeType(lowerBound, propertyType);
                                                var upperValue = Convert.ChangeType(upperBound, propertyType);
                                                return comparable.CompareTo(lowerValue) >= 0 && comparable.CompareTo(upperValue) <= 0;
                                            }

                                            return false;
                                        });
                                    }
                                }
                                else if (filter.Contains("<>"))
                                {
                                    var match = Regex.Match(filter, @"\[(.*?)\]\s*<>\s*(-?\d+|'[^']*')", RegexOptions.IgnoreCase);
                                    if (match.Success)
                                    {
                                        string columnName = match.Groups[1].Value;
                                        string columnValue = match.Groups[2].Value.Trim('\'');

                                        count = Companies.Count(ap =>
                                        {
                                            object obj = ap;
                                            foreach (var part in columnName.Split('.'))
                                            {
                                                if (obj == null) break;
                                                var prop = obj.GetType().GetProperty(part);
                                                obj = prop?.GetValue(obj);
                                            }

                                            if (obj == null) return false;

                                            Type propertyType = Nullable.GetUnderlyingType(obj.GetType()) ?? obj.GetType();

                                            if (propertyType == typeof(string))
                                            {
                                                return !string.Equals(obj?.ToString(), columnValue, StringComparison.OrdinalIgnoreCase);
                                            }

                                            if (propertyType == typeof(DateTime))
                                            {
                                                if (!DateTime.TryParse(columnValue, out DateTime dateValue)) return false;

                                                DateTime actualDate;
                                                if (obj is DateTime dt)
                                                    actualDate = dt;
                                                else if (obj != null)
                                                    actualDate = (DateTime)obj;
                                                else
                                                    return false;

                                                return actualDate != dateValue;
                                            }

                                            if (obj is IComparable comparable)
                                            {
                                                var targetValue = Convert.ChangeType(columnValue, propertyType);
                                                return comparable.CompareTo(targetValue) != 0;
                                            }

                                            return false;
                                        });
                                    }
                                }
                                else if (filter.Contains(">") && !filter.Contains(">="))
                                {
                                    var match = Regex.Match(filter, @"\[(.*?)\]\s*>\s*'(.*?)'", RegexOptions.IgnoreCase)
                                                ?? Regex.Match(filter, @"\[(.*?)\]\s*>\s*(-?\d+)", RegexOptions.IgnoreCase)
                                                ?? Regex.Match(filter, @"\[(.*?)\]\s*>\s*#(.*?)#", RegexOptions.IgnoreCase);

                                    if (match.Success)
                                    {
                                        string columnName = match.Groups[1].Value;
                                        string columnValue = match.Groups[2].Value;
                                        var propertyPath = columnName.Split('.');

                                        count = Companies.Count(ap =>
                                        {
                                            object obj = ap;
                                            foreach (var propName in propertyPath)
                                            {
                                                if (obj == null) break;
                                                var propInfo = obj.GetType().GetProperty(propName);
                                                obj = propInfo?.GetValue(obj);
                                            }

                                            if (obj == null) return false;

                                            Type propertyType = Nullable.GetUnderlyingType(obj.GetType()) ?? obj.GetType();

                                            if (propertyType == typeof(string))
                                            {
                                                return string.Compare(obj?.ToString(), columnValue, StringComparison.OrdinalIgnoreCase) > 0;
                                            }

                                            if (propertyType == typeof(DateTime))
                                            {
                                                if (!DateTime.TryParse(columnValue, out DateTime dateValue)) return false;

                                                DateTime actualDate;
                                                if (obj is DateTime dt)
                                                    actualDate = dt;
                                                else if (obj != null)
                                                    actualDate = (DateTime)obj;
                                                else
                                                    return false;

                                                return actualDate > dateValue;
                                            }

                                            if (obj is IComparable comparable)
                                            {
                                                var targetValue = Convert.ChangeType(columnValue, propertyType);
                                                return comparable.CompareTo(targetValue) > 0;
                                            }

                                            return false;
                                        });
                                    }
                                }

                                else if (filter.Contains("<") && !filter.Contains("<="))
                                {
                                    var match = Regex.Match(filter, @"\[(.*?)\]\s*<\s*'(.*?)'", RegexOptions.IgnoreCase)
                                                ?? Regex.Match(filter, @"\[(.*?)\]\s*<\s*(-?\d+)", RegexOptions.IgnoreCase)
                                                ?? Regex.Match(filter, @"\[(.*?)\]\s*<\s*#(.*?)#", RegexOptions.IgnoreCase);

                                    if (match.Success)
                                    {
                                        string columnName = match.Groups[1].Value;
                                        string columnValue = match.Groups[2].Value;
                                        var propertyPath = columnName.Split('.');

                                        count = Companies.Count(ap =>
                                        {
                                            object obj = ap;

                                            foreach (var propName in propertyPath)
                                            {
                                                if (obj == null) break;
                                                var propInfo = obj.GetType().GetProperty(propName);
                                                obj = propInfo?.GetValue(obj);
                                            }

                                            if (obj == null) return false;

                                            Type propertyType = Nullable.GetUnderlyingType(obj.GetType()) ?? obj.GetType();

                                            if (propertyType == typeof(string))
                                            {
                                                return string.Compare(obj?.ToString(), columnValue, StringComparison.OrdinalIgnoreCase) < 0;
                                            }

                                            if (propertyType == typeof(DateTime))
                                            {
                                                if (!DateTime.TryParse(columnValue, out DateTime dateValue)) return false;

                                                DateTime actualDate;
                                                if (obj is DateTime dt)
                                                    actualDate = dt;
                                                else if (obj != null)
                                                    actualDate = (DateTime)obj;
                                                else
                                                    return false;

                                                return actualDate < dateValue;
                                            }

                                            if (obj is IComparable comparable)
                                            {
                                                var targetValue = Convert.ChangeType(columnValue, propertyType);
                                                return comparable.CompareTo(targetValue) < 0;
                                            }

                                            return false;
                                        });
                                    }
                                }

                                else if (filter.Contains(">="))
                                {
                                    var match = Regex.Match(filter, @"\[(.*?)\]\s*>=\s*'(.*?)'", RegexOptions.IgnoreCase)
                                                ?? Regex.Match(filter, @"\[(.*?)\]\s*>=\s*(-?\d+)", RegexOptions.IgnoreCase)
                                                ?? Regex.Match(filter, @"\[(.*?)\]\s*>=\s*#(.*?)#", RegexOptions.IgnoreCase);

                                    if (match.Success)
                                    {
                                        string columnName = match.Groups[1].Value;
                                        string columnValue = match.Groups[2].Value;
                                        var propertyPath = columnName.Split('.');

                                        count = Companies.Count(ap =>
                                        {
                                            object obj = ap;
                                            foreach (var propName in propertyPath)
                                            {
                                                if (obj == null) break;
                                                var propInfo = obj.GetType().GetProperty(propName);
                                                obj = propInfo?.GetValue(obj);
                                            }

                                            if (obj == null) return false;

                                            Type propertyType = Nullable.GetUnderlyingType(obj.GetType()) ?? obj.GetType();

                                            if (propertyType == typeof(string))
                                            {
                                                return string.Compare(obj?.ToString(), columnValue, StringComparison.OrdinalIgnoreCase) >= 0;
                                            }

                                            if (propertyType == typeof(DateTime))
                                            {
                                                if (!DateTime.TryParse(columnValue, out DateTime dateValue)) return false;

                                                DateTime actualDate;
                                                if (obj is DateTime dt)
                                                    actualDate = dt;
                                                else if (obj != null)
                                                    actualDate = (DateTime)obj;
                                                else
                                                    return false;

                                                return actualDate >= dateValue;
                                            }

                                            if (obj is IComparable comparable)
                                            {
                                                var targetValue = Convert.ChangeType(columnValue, propertyType);
                                                return comparable.CompareTo(targetValue) >= 0;
                                            }

                                            return false;
                                        });
                                    }
                                }
                                else if (filter.Contains("<="))
                                {
                                    var match = Regex.Match(filter, @"\[(.*?)\]\s*<=\s*'(.*?)'", RegexOptions.IgnoreCase)
                                                ?? Regex.Match(filter, @"\[(.*?)\]\s*<=\s*(-?\d+)", RegexOptions.IgnoreCase)
                                                ?? Regex.Match(filter, @"\[(.*?)\]\s*<=\s*#(.*?)#", RegexOptions.IgnoreCase);

                                    if (match.Success)
                                    {
                                        string columnName = match.Groups[1].Value;
                                        string columnValue = match.Groups[2].Value;
                                        var propertyPath = columnName.Split('.');

                                        count = Companies.Count(ap =>
                                        {
                                            object obj = ap;

                                            foreach (var propName in propertyPath)
                                            {
                                                if (obj == null) break;
                                                var propInfo = obj.GetType().GetProperty(propName);
                                                obj = propInfo?.GetValue(obj);
                                            }

                                            if (obj == null) return false;

                                            Type propertyType = Nullable.GetUnderlyingType(obj.GetType()) ?? obj.GetType();

                                            if (propertyType == typeof(string))
                                            {
                                                return string.Compare(obj?.ToString(), columnValue, StringComparison.OrdinalIgnoreCase) <= 0;
                                            }

                                            if (propertyType == typeof(DateTime))
                                            {
                                                if (!DateTime.TryParse(columnValue, out DateTime dateValue)) return false;

                                                DateTime actualDate;
                                                if (obj is DateTime dt)
                                                    actualDate = dt;
                                                else if (obj != null)
                                                    actualDate = (DateTime)obj;
                                                else
                                                    return false;

                                                return actualDate <= dateValue;
                                            }

                                            if (obj is IComparable comparable)
                                            {
                                                var targetValue = Convert.ChangeType(columnValue, propertyType);
                                                return comparable.CompareTo(targetValue) <= 0;
                                            }

                                            return false;
                                        });
                                    }
                                }
                                else if (filter.Contains("Not IsNullOrEmpty"))
                                {
                                    string columnName = filter.Split(new[] { '[' }, StringSplitOptions.RemoveEmptyEntries)[1]
                                                              .Split(']')[0].Trim();
                                    var propertyPath = columnName.Split('.');

                                    count = Companies.Count(ap =>
                                    {
                                        object obj = ap;
                                        foreach (var propName in propertyPath)
                                        {
                                            if (obj == null) break;
                                            var propInfo = obj.GetType().GetProperty(propName);
                                            obj = propInfo?.GetValue(obj);
                                        }

                                        var value = obj?.ToString();
                                        return !string.IsNullOrEmpty(value);
                                    });
                                }

                                else if (filter.Contains("IsNullOrEmpty"))
                                {
                                    string columnName = filter.Split(new[] { '[' }, StringSplitOptions.RemoveEmptyEntries)[1]
                                                              .Split(']')[0].Trim();
                                    var propertyPath = columnName.Split('.');

                                    count = Companies.Count(ap =>
                                    {
                                        object obj = ap;
                                        foreach (var propName in propertyPath)
                                        {
                                            if (obj == null) break;
                                            var propInfo = obj.GetType().GetProperty(propName);
                                            obj = propInfo?.GetValue(obj);
                                        }

                                        var value = obj?.ToString();
                                        return string.IsNullOrEmpty(value);
                                    });
                                }

                                else if (filter.Contains("Is Null"))
                                {
                                    int startIndex = filter.IndexOf('[');
                                    int endIndex = filter.IndexOf(']');

                                    string columnName = filter.Substring(startIndex + 1, endIndex - startIndex - 1).Trim();
                                    var propertyPath = columnName.Split('.');

                                    count = Companies.Count(ap =>
                                    {
                                        object obj = ap;
                                        foreach (var propName in propertyPath)
                                        {
                                            if (obj == null) break;
                                            var propInfo = obj.GetType().GetProperty(propName);
                                            obj = propInfo?.GetValue(obj);
                                        }

                                        return obj == null;
                                    });
                                }

                                else if (filter.Contains("Is Not Null"))
                                {
                                    int startIndex = filter.IndexOf('[');
                                    int endIndex = filter.IndexOf(']');

                                    string columnName = filter.Substring(startIndex + 1, endIndex - startIndex - 1).Trim();
                                    var propertyPath = columnName.Split('.');

                                    count = Companies.Count(ap =>
                                    {
                                        object obj = ap;

                                        foreach (var propName in propertyPath)
                                        {
                                            if (obj == null) break;
                                            var propInfo = obj.GetType().GetProperty(propName);
                                            obj = propInfo?.GetValue(obj);
                                        }

                                        return obj != null;
                                    });
                                }

                                else if (filter.Contains("="))
                                {
                                    var match = Regex.Match(filter, @"\[(.*?)\]\s*=\s*(?:'(.*?)'|(-?\d+(\.\d+)?)|#(.*?)#)", RegexOptions.IgnoreCase);

                                    if (match.Success)
                                    {
                                        string columnName = match.Groups[1].Value;
                                        string stringValue = match.Groups[2].Value;
                                        string numericValue = match.Groups[3].Value;
                                        string dateValue = match.Groups[5].Value;

                                        var propertyPath = columnName.Split('.');

                                        count = Companies.Count(ap =>
                                        {
                                            object obj = ap;
                                            foreach (var propName in propertyPath)
                                            {
                                                if (obj == null) break;
                                                var propInfo = obj.GetType().GetProperty(propName);
                                                obj = propInfo?.GetValue(obj);
                                            }

                                            if (obj == null) return false;
                                            Type propertyType = Nullable.GetUnderlyingType(obj.GetType()) ?? obj.GetType();
                                            if (propertyType == typeof(string))
                                            {
                                                return string.Equals(obj.ToString(), stringValue, StringComparison.OrdinalIgnoreCase);
                                            }

                                            if (propertyType == typeof(int) || propertyType == typeof(long) ||
                                                propertyType == typeof(decimal) || propertyType == typeof(double) || propertyType == typeof(float))
                                            {
                                                if (double.TryParse(numericValue, out double targetNumber))
                                                {
                                                    return Convert.ToDouble(obj) == targetNumber;
                                                }
                                            }
                                            if (propertyType == typeof(DateTime))
                                            {
                                                if (!DateTime.TryParse(dateValue, out DateTime targetDate)) return false;

                                                DateTime actualDate;

                                                if (obj is DateTime dt)
                                                {
                                                    actualDate = dt;
                                                }
                                                else if (obj is DateTime?)
                                                {
                                                    DateTime? nullableDate = (DateTime?)obj;
                                                    if (!nullableDate.HasValue) return false;
                                                    actualDate = nullableDate.Value;
                                                }
                                                else
                                                {
                                                    return false;
                                                }

                                                return actualDate == targetDate;
                                            }

                                            if (obj is IComparable comparable)
                                            {
                                                object targetValue = Convert.ChangeType(stringValue ?? numericValue ?? dateValue, propertyType);
                                                return comparable.CompareTo(targetValue) == 0;
                                            }

                                            return false;
                                        });
                                    }
                                }
                                else if (filter.Contains("!=") || filter.Contains("<>"))
                                {
                                    var match = Regex.Match(filter, @"\[(.*?)\]\s*(!=|<>)\s*'([^']*)'", RegexOptions.IgnoreCase);
                                    if (match.Success)
                                    {
                                        string columnName = match.Groups[1].Value;
                                        string columnValue = match.Groups[3].Value;
                                        var propertyPath = columnName.Split('.');

                                        count = Companies.Count(ap =>
                                        {
                                            object obj = ap;
                                            foreach (var propName in propertyPath)
                                            {
                                                if (obj == null) break;
                                                var propInfo = obj.GetType().GetProperty(propName);
                                                obj = propInfo?.GetValue(obj);
                                            }

                                            if (obj == null) return true;

                                            string value = obj.ToString();
                                            return !string.Equals(value, columnValue, StringComparison.OrdinalIgnoreCase);
                                        });
                                    }
                                }
                                else if (filter.Contains(" Like ") && !filter.Contains(" Not Like "))
                                {
                                    var columnNameStartIndex = filter.IndexOf("[") + 1;
                                    var columnNameEndIndex = filter.IndexOf("]", columnNameStartIndex);
                                    var columnName = filter.Substring(columnNameStartIndex, columnNameEndIndex - columnNameStartIndex).Trim();

                                    var likeValueStartIndex = filter.IndexOf("Like", columnNameEndIndex) + "Like".Length;
                                    var likeValue = filter.Substring(likeValueStartIndex).Trim().Trim('\'');

                                    var propertyPath = columnName.Split('.');

                                    count = Companies.Count(ap =>
                                    {
                                        object obj = ap;
                                        foreach (var propName in propertyPath)
                                        {
                                            if (obj == null) break;
                                            var propInfo = obj.GetType().GetProperty(propName);
                                            obj = propInfo?.GetValue(obj);
                                        }

                                        if (obj == null) return false;

                                        return obj.ToString().IndexOf(likeValue, StringComparison.OrdinalIgnoreCase) >= 0;

                                    });
                                }

                                else if (filter.Contains(" Not Like "))
                                {
                                    var columnNameStartIndex = filter.IndexOf("[") + 1;
                                    var columnNameEndIndex = filter.IndexOf("]", columnNameStartIndex);
                                    var columnName = filter.Substring(columnNameStartIndex, columnNameEndIndex - columnNameStartIndex).Trim();

                                    var notLikeValueStartIndex = filter.IndexOf("Not Like", columnNameEndIndex) + "Not Like".Length;
                                    var notLikeValue = filter.Substring(notLikeValueStartIndex).Trim().Trim('\'');

                                    var propertyPath = columnName.Split('.');

                                    count = Companies.Count(ap =>
                                    {
                                        object obj = ap;

                                        foreach (var propName in propertyPath)
                                        {
                                            if (obj == null) break;
                                            var propInfo = obj.GetType().GetProperty(propName);
                                            obj = propInfo?.GetValue(obj);
                                        }

                                        if (obj == null) return true;

                                        return obj.ToString().IndexOf(notLikeValue, StringComparison.OrdinalIgnoreCase) < 0;

                                    });
                                }



                                Listofitem.Add(
                                    new TileBarFilters()
                                    {
                                        Caption = baseKey,
                                        Id = 0,
                                        BackColor = !string.IsNullOrEmpty(backColor) ? backColor : null,
                                        ForeColor = null,
                                        FilterCriteria = item.Value,
                                        EntitiesCount = count,
                                        EntitiesCountVisibility = Visibility.Visible,
                                        Height = 60,
                                        width = 220
                                    });
                            }
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.Logger.Log(string.Format("Error in Method AddCustomSetting() {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }
                    }
                }


                GeosApplication.Instance.Logger.Log("Method AddCustomSetting() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddCustomSetting() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //[pallavi.kale][GEOS2-8957][29.09.2025]
        private void ShowFilterEditor(FilterEditorEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowFilterEditor()...", category: Category.Info, priority: Priority.Low);
                CustomFilterEditorView customFilterEditorView = new CustomFilterEditorView();
                CustomFilterEditorViewModel customFilterEditorViewModel = new CustomFilterEditorViewModel();
                string titleText = DevExpress.Xpf.Grid.GridControlLocalizer.Active.GetLocalizedString(GridControlStringId.FilterEditorTitle);
                if (IsEdit)
                {
                    if (!string.IsNullOrEmpty(CustomFilterStringName))
                    {
                        customFilterEditorViewModel.FilterName = CustomFilterStringName;
                    }
                    else
                    {
                        customFilterEditorViewModel.FilterName = SelectedTileBarItem.Caption;
                    }
                    if (!string.IsNullOrEmpty(CustomFilterHTMLColor))
                    {
                        customFilterEditorViewModel.HTMLColor = CustomFilterHTMLColor;
                    }
                    else
                    {
                        customFilterEditorViewModel.HTMLColor = SelectedTileBarItem.BackColor;
                    }


                    customFilterEditorViewModel.IsSave = true;
                    customFilterEditorViewModel.IsNew = false;
                    IsEdit = false;
                }
                else
                    customFilterEditorViewModel.IsNew = true;
                if (Listofitem == null)
                {
                    Listofitem = new ObservableCollection<TileBarFilters>();
                }


                customFilterEditorViewModel.Init(e.FilterControl, Listofitem);


                customFilterEditorView.DataContext = customFilterEditorViewModel;
                EventHandler handle = delegate { customFilterEditorView.Close(); };
                customFilterEditorViewModel.RequestClose += handle;
                customFilterEditorView.Title = titleText;
                customFilterEditorView.Icon = DevExpress.Xpf.Core.Native.ImageHelper.CreateImageFromCoreEmbeddedResource("Editors.Images.FilterControl.filter.png");

                customFilterEditorView.Grid.Children.Add(e.FilterControl);


                customFilterEditorView.ShowDialog();

                if (customFilterEditorViewModel.IsDelete && !string.IsNullOrEmpty(customFilterEditorViewModel.FilterName))
                {
                    if (Listofitem == null)
                    {
                        Listofitem = new ObservableCollection<TileBarFilters>();
                    }
                    TileBarFilters tileBarItem = Listofitem.FirstOrDefault(x => x.Caption.Equals(CustomFilterStringName));

                    if (tileBarItem != null)
                    {
                        Listofitem.Remove(tileBarItem);
                        List<Tuple<string, string>> lstUserConfiguration = new List<Tuple<string, string>>();
                        foreach (KeyValuePair<string, string> setting in GeosApplication.Instance.UserSettings)
                        {
                            string key = setting.Key;
                            string htmlColor = "";
                            string keyName = "";
                            if (setting.Key.Contains(userSettingsKeyForAccount))
                            {
                                key = setting.Key.Replace(userSettingsKeyForAccount, "");
                                string _html = key.Replace(tileBarItem.Caption, "");
                                htmlColor = _html.Replace("_", "");
                                if (!string.IsNullOrEmpty(htmlColor))
                                {
                                    keyName = key.Replace(_html, "");
                                }

                            }
                            if (keyName.Equals(tileBarItem.Caption) && !string.IsNullOrEmpty(htmlColor))
                            {
                                continue;
                            }



                            if (!key.Equals(tileBarItem.Caption))
                            {
                                lstUserConfiguration.Add(new Tuple<string, string>(setting.Key.ToString(), setting.Value.ToString()));
                            }

                        }
                        ApplicationOperation.CreateNewSetting(lstUserConfiguration, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                        GeosApplication.Instance.UserSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.UserSettingFilePath);
                        Listofitem = new ObservableCollection<TileBarFilters>();
                        FillTileBarList();
                        AddCustomSetting();
                        if (Listofitem.Count == 1)
                        {
                            Listofitem = new ObservableCollection<TileBarFilters>();
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(customFilterEditorViewModel.FilterName) && customFilterEditorViewModel.IsSave && !customFilterEditorViewModel.IsNew && customFilterEditorViewModel.IsCancel)
                {
                    if (Listofitem == null)
                    {
                        Listofitem = new ObservableCollection<TileBarFilters>();
                    }
                    TileBarFilters tileBarItem = Listofitem.FirstOrDefault(x => x.Caption.Equals(CustomFilterStringName));
                    if (tileBarItem != null)
                    {
                        CustomFilterStringName = customFilterEditorViewModel.FilterName;
                        CustomFilterHTMLColor = customFilterEditorViewModel.HTMLColor;
                        TableView table = (TableView)e.OriginalSource;
                        GridControl gridControl = (table).Grid;

                        if (!string.IsNullOrEmpty(customFilterEditorViewModel.FilterCriteria))
                        {
                            gridControl.FilterCriteria = CriteriaOperator.Parse(customFilterEditorViewModel.FilterCriteria);
                        }

                        VisibleRowCount = gridControl.VisibleRowCount;
                        string filterCaption = tileBarItem.Caption;
                        tileBarItem.Caption = customFilterEditorViewModel.FilterName;
                        tileBarItem.EntitiesCount = VisibleRowCount;
                        tileBarItem.EntitiesCountVisibility = Visibility.Visible;
                        tileBarItem.FilterCriteria = customFilterEditorViewModel.FilterCriteria;
                        List<Tuple<string, string>> lstUserConfiguration = new List<Tuple<string, string>>();
                        foreach (KeyValuePair<string, string> setting in GeosApplication.Instance.UserSettings)
                        {
                            string key = setting.Key;
                            string htmlColor = "";
                            string keyName = "";

                            if (setting.Key.Contains(userSettingsKeyForAccount))
                            {
                                key = setting.Key.Replace(userSettingsKeyForAccount, "");
                                string[] splitted = key.Split('_');
                                if (splitted != null && splitted.Length > 1)
                                {
                                    string _html = key.Replace(filterCaption, "");
                                    htmlColor = _html.Replace("_", "");
                                    if (!string.IsNullOrEmpty(htmlColor))
                                    {
                                        keyName = key.Replace(_html, "");
                                    }
                                }
                            }

                            if (keyName.Equals(filterCaption) && !string.IsNullOrEmpty(htmlColor))
                            {
                                Listofitem.Where(a => a.Caption == SelectedTileBarItem.Caption).ToList().ForEach(b => b.BackColor = CustomFilterHTMLColor);
                                PreviousSelectedTopTileBarItem = SelectedTileBarItem;
                                string name = userSettingsKeyForAccount + tileBarItem.Caption + "_" + CustomFilterHTMLColor;
                                lstUserConfiguration.Add(new Tuple<string, string>(name.ToString(), tileBarItem.FilterCriteria));
                                continue;
                            }


                            if (!key.Equals(filterCaption))
                                lstUserConfiguration.Add(new Tuple<string, string>(setting.Key.ToString(), setting.Value.ToString()));
                            else
                                lstUserConfiguration.Add(new Tuple<string, string>((userSettingsKeyForAccount + tileBarItem.Caption), tileBarItem.FilterCriteria));


                        }
                        ApplicationOperation.CreateNewSetting(lstUserConfiguration, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                        GeosApplication.Instance.UserSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.UserSettingFilePath);
                        Listofitem = new ObservableCollection<TileBarFilters>();

                        FillTileBarList();
                        AddCustomSetting();
                    }
                }
                else if (customFilterEditorViewModel.FilterName != null && !string.IsNullOrEmpty(customFilterEditorViewModel.FilterName) && customFilterEditorViewModel.IsSave && customFilterEditorViewModel.IsNew && customFilterEditorViewModel.IsCancel)
                {
                    TableView table = (TableView)e.OriginalSource;
                    GridControl gridControl = (table).Grid;

                    if (!string.IsNullOrEmpty(customFilterEditorViewModel.FilterCriteria))
                    {
                        gridControl.FilterCriteria = CriteriaOperator.Parse(customFilterEditorViewModel.FilterCriteria);
                    }

                    VisibleRowCount = gridControl.VisibleRowCount;
                    if (Listofitem == null)
                    {
                        Listofitem = new ObservableCollection<TileBarFilters>();
                    }
                    Listofitem.Add(new TileBarFilters()
                    {
                        Caption = customFilterEditorViewModel.FilterName,
                        Id = 0,
                        BackColor = customFilterEditorViewModel.HTMLColor,
                        EntitiesCountVisibility = Visibility.Visible,
                        FilterCriteria = customFilterEditorViewModel.FilterCriteria,
                        Height = 80,
                        width = 200,
                        EntitiesCount = VisibleRowCount
                    });

                    string filterName = "";

                    filterName = userSettingsKeyForAccount + customFilterEditorViewModel.FilterName;

                    GeosApplication.Instance.UserSettings[filterName] = customFilterEditorViewModel.FilterCriteria;
                    ApplicationOperation.CreateNewSetting(GeosApplication.Instance.UserSettings, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                    GeosApplication.Instance.UserSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.UserSettingFilePath);

                    if (customFilterEditorViewModel.HTMLColor != null)
                    {
                        string FilterName = "";
                        filterName = userSettingsKeyForAccount + customFilterEditorViewModel.FilterName + "_" + customFilterEditorViewModel.HTMLColor;
                        GeosApplication.Instance.UserSettings[filterName] = customFilterEditorViewModel.FilterCriteria;
                        ApplicationOperation.CreateNewSetting(GeosApplication.Instance.UserSettings, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                        GeosApplication.Instance.UserSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.UserSettingFilePath);
                    }

                    Listofitem = new ObservableCollection<TileBarFilters>();

                    FillTileBarList();
                    AddCustomSetting();

                }

                GeosApplication.Instance.Logger.Log("Method ShowFilterEditor() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ShowFilterEditor() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion // Methods.

    }

}
