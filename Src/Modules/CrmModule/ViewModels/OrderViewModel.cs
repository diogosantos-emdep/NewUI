using DevExpress.Data;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Spreadsheet;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Serialization;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using DevExpress.Xpf.Spreadsheet;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.Crm.Views;
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
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using DevExpress.Mvvm.UI.Interactivity;
using DevExpress.Utils;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Utility;
using System.Net;
using System.Runtime.Serialization.Json;
using DevExpress.Data.Filtering;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
namespace Emdep.Geos.Modules.Crm.ViewModels
{
    public class OrderViewModel : NavigationViewModelBase, INotifyPropertyChanged //, ISupportServices
    {
        #region Services

        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //ICrmService CrmStartUp = new CrmServiceController("localhost:6699");
        #endregion // Services

        #region Declaration

        private DataTable dttable;
        private DataTable dttableCopy;
        private DataTable dataTable;
        private float probabilityOfSuccess = .0F;
        private IList<Offer> orderList;
        private List<OptionsByOffer> orderOptionsByOfferList;
        private IList<Template> templates;
        private ObservableCollection<Emdep.Geos.UI.Helper.Column> columns;

        public string OrderGridSettingFilePath = GeosApplication.Instance.UserSettingFolderName + "OrderGridSetting.Xml";

        private Timer tmrOnce;
        public ObservableCollection<Summary> TotalSummary { get; private set; }
        public ObservableCollection<Summary> GroupSummary { get; private set; }
        string myFilterString;
        private DateTime startSelectionDate;
        private DateTime finishSelectionDate;

        private bool isBusy;

        private bool isViewRangeControl;
        private string gridRowHeight;
        private bool isOrderColumnChooserVisible;

        List<string> failedPlants;
        List<string> successPlantList; //[cpatil][13.09.2025][GEOS2-8367]
        Boolean isShowFailedPlantWarning;
        string warningFailedPlants;
        IServiceContainer serviceContainer = null;


        private ObservableCollection<LookupValue> businessUnitList;
        private ObservableCollection<LookupValue> leadSourceList;
        private List<CarProject> carProjectList;
        private List<CarOEM> carOemList;
        private List<LogEntryByOffer> changeLogsEntry;
        List<OfferOption> offerOptions;
        TableView view;

        private bool isFromFilterString = false;
        private bool isFilterEnabled;
        private DateTime? old_startDate;
        private DateTime? old_endDate;
        private List<OrderGrid> orderGridList;
        ObservableCollection<GeosStatus> geosStatusList;
        List<Country> countries; //[pmisal][GEOS2-4322][17.04.2023]
        #endregion // Declaration

        #region Public Properties
        public ObservableCollection<GeosStatus> GeosStatusList
        {
            get { return geosStatusList; }
            set { geosStatusList = value; }
        }

        public ObservableCollection<LookupValue> BusinessUnitList
        {
            get { return businessUnitList; }
            set
            {
                businessUnitList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BusinessUnitList"));
            }
        }
        public ObservableCollection<LookupValue> LeadSourceList
        {
            get { return leadSourceList; }
            set
            {
                leadSourceList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LeadSourceList"));
            }
        }
        public List<CarProject> CarProjectList
        {
            get { return carProjectList; }
            set
            {
                carProjectList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CarProjectList"));
            }
        }
        public List<CarOEM> CarOemList
        {
            get { return carOemList; }
            set
            {
                carOemList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CarOemList"));
            }
        }
        public List<LogEntryByOffer> ChangeLogsEntry
        {
            get { return changeLogsEntry; }
            set { changeLogsEntry = value; }
        }

        public string PreviouslySelectedSalesOwners { get; set; }
        public string PreviouslySelectedPlantOwners { get; set; }
        public List<string> FailedPlants
        {
            get { return failedPlants; }
            set { failedPlants = value; }
        }
        //[cpatil][13.09.2025][GEOS2-8367]
        public List<string> SuccessPlantList
        {
            get { return successPlantList; }
            set { successPlantList = value; }
        }
        public Boolean IsShowFailedPlantWarning
        {
            get { return isShowFailedPlantWarning; }
            set
            {
                isShowFailedPlantWarning = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsShowFailedPlantWarning"));
            }
        }

        public string WarningFailedPlants
        {
            get { return warningFailedPlants; }
            set
            {
                warningFailedPlants = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WarningFailedPlants"));
            }
        }

        public IList<Offer> OrderList
        {
            get { return orderList; }
            set { orderList = value; OnPropertyChanged(new PropertyChangedEventArgs("OrderList")); }
        }

        public List<OptionsByOffer> OrderOptionsByOfferList
        {
            get { return orderOptionsByOfferList; }
            set { orderOptionsByOfferList = value; }
        }

        public DataTable DataTable
        {
            get { return dataTable; }
            set { dataTable = value; }
        }

        public float ProbabilityOfSuccess
        {
            get { return probabilityOfSuccess; }
            set { probabilityOfSuccess = value; }
        }

        public DataTable Dttable
        {
            get { return dttable; }
            set
            {
                dttable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Dttable"));
            }
        }

        public DataTable DttableCopy
        {
            get { return dttableCopy; }
            set
            {
                dttableCopy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DttableCopy"));
            }
        }

        public IList<Template> Templates
        {
            get { return templates; }
            set { templates = value; }
        }

        public string MyFilterString
        {
            get { return myFilterString; }
            set
            {
                myFilterString = value;

                //This condition for if user clear the filter then clear date also.
                if (string.IsNullOrEmpty(myFilterString))
                {
                    isFromFilterString = true;
                    StartSelectionDate = DateTime.MinValue;
                    FinishSelectionDate = DateTime.MinValue;
                }
                else
                {
                    if (!myFilterString.Contains("[PODate]"))
                    {
                        StartSelectionDate = DateTime.MinValue;
                        FinishSelectionDate = DateTime.MinValue;
                    }
                }

                isFromFilterString = false;

                OnPropertyChanged(new PropertyChangedEventArgs("MyFilterString"));
            }
        }


        public DateTime FinishSelectionDate
        {
            get { return finishSelectionDate; }
            set
            {
                finishSelectionDate = value;

                if (!isFromFilterString)
                    UpdateFilterString();
                OnPropertyChanged(new PropertyChangedEventArgs("FinishSelectionDate"));
            }
        }

        public DateTime StartSelectionDate
        {
            get { return startSelectionDate; }
            set
            {
                startSelectionDate = value;

                if (!isFromFilterString)
                    UpdateFilterString();
                OnPropertyChanged(new PropertyChangedEventArgs("StartSelectionDate"));
            }
        }

        public bool IsFilterEnabled
        {
            get { return isFilterEnabled; }
            set
            {
                if (isFilterEnabled == value)
                    return;
                isFilterEnabled = value;
                isFromFilterString = true;
                if (!value)
                {
                    old_startDate = StartSelectionDate;
                    old_endDate = FinishSelectionDate;
                    StartSelectionDate = DateTime.MinValue;
                    FinishSelectionDate = DateTime.MinValue;


                }
                else if (old_startDate.HasValue)
                {
                    if (old_startDate.Value > DateTime.MinValue)
                    {
                        if (old_startDate.Value == StartSelectionDate)
                        {

                            StartSelectionDate = old_startDate.Value;
                            FinishSelectionDate = old_endDate.Value;
                            old_startDate = null;
                            old_endDate = null;
                        }
                        else
                        {
                            if (StartSelectionDate == DateTime.MinValue && FinishSelectionDate == DateTime.MinValue)
                            {
                                StartSelectionDate = old_startDate.Value;
                                FinishSelectionDate = old_endDate.Value;
                                old_startDate = null;
                                old_endDate = null;
                            }


                        }
                    }
                    else
                    {
                        old_startDate = StartSelectionDate;
                        old_endDate = FinishSelectionDate;
                    }
                }
                isFromFilterString = false;
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


        public bool IsViewRangeControl
        {
            get { return isViewRangeControl; }
            set
            {
                isViewRangeControl = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsViewRangeControl"));
            }
        }

        public string GridRowHeight
        {
            get { return gridRowHeight; }
            set
            {
                gridRowHeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GridRowHeight"));
            }
        }



        public List<OfferOption> OfferOptions
        {
            get { return offerOptions; }
            set { offerOptions = value; }
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

        public bool IsOrderColumnChooserVisible
        {
            get
            {
                return isOrderColumnChooserVisible;
            }

            set
            {
                isOrderColumnChooserVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsOrderColumnChooserVisible"));
            }
        }

        // Export Excel .xlsx
        public virtual string ResultFileName { get; protected set; }
        public virtual bool DialogResult { get; protected set; }
        protected ISaveFileDialogService SaveFileDialogService { get { return this.GetService<ISaveFileDialogService>(); } }

        public ICustomGridService GridService
        {
            get
            {
                return ServiceContainer.GetService<ICustomGridService>();
            }
        }

        public List<OrderGrid> OrderGridList
        {
            get { return orderGridList; }
            set { orderGridList = value; }
        }

        //[pmisal][GEOS2-4322][17.04.2023]
        public List<Country> Countries
        {
            get { return countries; }
            set
            {
                countries = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Countries"));
            }
        }

        #endregion // Properties

        #region Events

        public event EventHandler RequestClose;

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler ContentChanged;

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion // Events

        #region Commands

        public ICommand PrintButtonCommand { get; set; }
        public ICommand CustomCellAppearanceCommand { get; set; }
        public ICommand CommandGridDoubleClick { get; set; }
        public ICommand SalesOwnerPopupClosedCommand { get; private set; }
        public ICommand PlantOwnerPopupClosedCommand { get; private set; }
        public ICommand ViewHideRangeControlCommand { get; set; }
        public ICommand RefreshOrderViewCommand { get; set; }
        public ICommand ExportButtonCommand { get; set; }
        public ICommand UpdateMultipleRowsOrderViewCommand { get; set; }
        public ICommand CommandShowFilterPopupClick { get; set; }




        #endregion // Commands

        #region Constructor

        public OrderViewModel()
        {

            try
            {
                GeosApplication.Instance.Logger.Log("Constructor OrderViewModel ...", category: Category.Info, priority: Priority.Low);

                FailedPlants = new List<string>();
                IsShowFailedPlantWarning = false;
                WarningFailedPlants = String.Empty;
                SuccessPlantList = new List<string>(); //[cpatil][13.09.2025][GEOS2-8367]
                CustomCellAppearanceCommand = new DevExpress.Mvvm.DelegateCommand<RoutedEventArgs>(CustomCellAppearanceGridControl);
                PrintButtonCommand = new RelayCommand(new Action<object>(PrintLeadsGrid));
                CommandGridDoubleClick = new DelegateCommand<object>(OrderEditViewWindowShow);
                ViewHideRangeControlCommand = new DelegateCommand<object>(ViewHideRangeControl);
                SalesOwnerPopupClosedCommand = new DevExpress.Mvvm.DelegateCommand<object>(SalesOwnerPopupClosedCommandAction);
                PlantOwnerPopupClosedCommand = new DevExpress.Mvvm.DelegateCommand<object>(PlantOwnerPopupClosedCommandAction);
                RefreshOrderViewCommand = new RelayCommand(new Action<object>(RefreshOrderDetails));
                ExportButtonCommand = new RelayCommand(new Action<object>(ExportLeadsGridButtonCommandAction));

                CommandShowFilterPopupClick = new DelegateCommand<FilterPopupEventArgs>(CustomShowFilterPopup);

                IsViewRangeControl = true;
                GridRowHeight = "100";
                var tasks = new List<Task>();
                tasks.Add(Task.Run(() => FillCarProjectList()));
                tasks.Add(Task.Run(() => FillStatusList()));
                tasks.Add(Task.Run(() => FillCarOemList()));
                tasks.Add(Task.Run(() => FillBusinessUnitList()));
                tasks.Add(Task.Run(() => FillLeadSourceList()));
                tasks.Add(Task.Run(() => fillCountries()));//[pmisal][GESO2-4322][17.04.2023]
                //tasks.Add(Task.Run(() => FillCmbSalesOwner()));
                Task.WaitAll(tasks.ToArray());
                FillCmbSalesOwner();


                UpdateMultipleRowsOrderViewCommand = new DelegateCommand<object>(UpdateMultipleRowsCommandAction);

                GeosApplication.Instance.Logger.Log("Constructor OrderViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in OrderViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion // Constructor

        #region Methods

        /// <summary>
        /// Method for fill data as per user permission.
        /// </summary>
        private void FillCmbSalesOwner()
        {
            GeosApplication.Instance.Logger.Log("Method FillCmbSalesOwner ...", category: Category.Info, priority: Priority.Low);

            if (GeosApplication.Instance.IdUserPermission == 21)
            {
                if (GeosApplication.Instance.SelectedSalesOwnerUsersList != null)
                {
                    GeosApplication.Instance.SelectedPlantOwnerUsersList = null;

                    FillOrderGridDetailsByUsers();
                }
                else
                {
                    Dttable.Rows.Clear();
                }

                // Called for 1st Time

            }
            else if (GeosApplication.Instance.IdUserPermission == 22)
            {
                if (GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
                {
                    GeosApplication.Instance.SelectedSalesOwnerUsersList = null;

                    // Called for 1st Time
                    FillOrderGridDetailsByPlant();
                }
                else
                {
                    Dttable.Rows.Clear();
                }
            }
            else
            {
                FillOrderGridDetails();
            }
            GeosApplication.Instance.Logger.Log("Method FillCmbSalesOwner() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method for refresh Order Grid details.
        /// </summary>
        private void FillOrderGridDetailsByPlant()
        {
            GeosApplication.Instance.Logger.Log("Method FillOrderGridDetailsByPlant ...", category: Category.Info, priority: Priority.Low);

            GeosApplication.Instance.CurrentCurrencySymbol = GeosApplication.Instance.Currencies.Where(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency"].ToString()).Select(cur => cur.Symbol).SingleOrDefault();
            GeosApplication.SetCurrencySymbol(GeosApplication.Instance.CurrentCurrencySymbol);

            try
            {
                Fillgrid();
                FillOrderPlants();
                FillDataTable();
                //Fillgrid();
                GeosApplication.Instance.Logger.Log("Method FillOrderGridDetailsByPlant() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillOrderGridDetailsByPlant() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for refresh Order Grid details.
        /// </summary>
        private void FillOrderGridDetails()
        {
            GeosApplication.Instance.Logger.Log("Method FillOrderGridDetails ...", category: Category.Info, priority: Priority.Low);
            GeosApplication.Instance.CurrentCurrencySymbol = GeosApplication.Instance.Currencies.Where(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency"].ToString()).Select(cur => cur.Symbol).SingleOrDefault();
            GeosApplication.SetCurrencySymbol(GeosApplication.Instance.CurrentCurrencySymbol);
            try
            {
                Fillgrid();
                FillOrder();
                FillDataTable();
                GeosApplication.Instance.Logger.Log("Method FillOrderGridDetails() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillOrderGridDetails() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillOrderGridDetailsByUsers()
        {
            GeosApplication.Instance.Logger.Log("Method FillOrderGridDetailsByUsers ...", category: Category.Info, priority: Priority.Low);

            GeosApplication.Instance.CurrentCurrencySymbol = GeosApplication.Instance.Currencies.Where(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency"].ToString()).Select(cur => cur.Symbol).SingleOrDefault();
            GeosApplication.SetCurrencySymbol(GeosApplication.Instance.CurrentCurrencySymbol);

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
                        return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }
                Fillgrid();
                FillOrderByUsers();
                FillDataTable();

                //Fillgrid();

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method FillOrderGridDetailsByUsers() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillOrderGridDetailsByUsers() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            GeosApplication.Instance.Logger.Log("Method FillOrderGridDetailsByUsers() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        private void FillOrderByUsers()
        {
            try
            {
                //[cpatil][13.09.2025][GEOS2-8367]
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
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
                    return new SplashScreenCustomView() { DataContext = new SplashScreenViewModel() };
                }, null, null);

                GeosApplication.Instance.Logger.Log("Method FillOrderByUsers ...", category: Category.Info, priority: Priority.Low);
                OrderGridList = new List<OrderGrid>();
                if (GeosApplication.Instance.SelectedSalesOwnerUsersList == null) return;

                List<UserManagerDtl> salesOwners = GeosApplication.Instance.SelectedSalesOwnerUsersList.Cast<UserManagerDtl>().ToList();
                var salesOwnersIds = string.Join(",", salesOwners.Select(i => i.IdUser));
                PreviouslySelectedSalesOwners = salesOwnersIds;
                //[cpatil][13.09.2025][GEOS2-8367]
                GeosApplication.Instance.CustomeSplashScreenMessage = "Please wait";
                GeosApplication.Instance.CustomeSplashScreenInformationMessage = "Collecting the information from plants :";
                GeosApplication.Instance.StatusMessages = new ObservableCollection<Data.Common.Crm.StatusMessage>();
                foreach (Company com in GeosApplication.Instance.CompanyList)
                {
                    GeosApplication.Instance.StatusMessages.Add(new Data.Common.Crm.StatusMessage() { Symbol = "", Message = com.ShortName, IsSuccess = 0 });
                }
                if (GeosApplication.Instance.CompanyList.Count == 1)
                {

                    // Continue loop although some plant is not available and Show error message.
                    foreach (var item in GeosApplication.Instance.CompanyList)
                    {
                        try
                        {
                            GeosApplication.Instance.SplashScreenMessage = "Connecting to " + item.Alias;
                            //offerOrderList.

                            //==========================================================================================

                            OrderParams objOrderParams = new OrderParams();

                            objOrderParams.idCurrency = GeosApplication.Instance.IdCurrencyByRegion;
                            objOrderParams.idsSelectedUser = salesOwnersIds;
                            objOrderParams.idUser = GeosApplication.Instance.ActiveUser.IdUser;
                            objOrderParams.idZone = GeosApplication.Instance.ActiveUser.Company.Country.IdZone;
                            objOrderParams.connectSiteDetailParams = new ConnectSiteDetailParams { idSite = Convert.ToInt32(item.ConnectPlantId), ConnectionString = item.ConnectPlantConstr };
                            objOrderParams.accountingYearFrom = GeosApplication.Instance.SelectedyearStarDate;
                            objOrderParams.accountingYearTo = GeosApplication.Instance.SelectedyearEndDate;
                            objOrderParams.Roles = RoleType.SalesPlantManager;
                            objOrderParams.activeSite = new ActiveSite { IdSite = Convert.ToInt32(item.ConnectPlantId), SiteAlias = Convert.ToString(item.Alias), SiteServiceProvider = item.ServiceProviderUrl }; //[nsatpute][20.11.2025][GEOS2-8367]
                            var orderGridListtemp = GetOrderGridData(objOrderParams).ToList();
                            //==========================================================================================
                            OrderGridList.AddRange(orderGridListtemp);

                            #region OldCode
                            //OffersOptionsList offersOptionsList = CrmStartUp.GetSelectedUsersOrdersDatatable(
                            //                           GeosApplication.Instance.IdCurrencyByRegion,
                            //                           salesOwnersIds,
                            //                             GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate,
                            //                           item,
                            //                           GeosApplication.Instance.ActiveUser.IdUser);


                            //offerOrderList.AddRange(offersOptionsLst.Offers);
                            //OptionsByOfferOrder.AddRange(offersOptionsLst.OptionsByOffers);
                            #endregion
                        }
                        catch (FaultException<ServiceException> ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", item.Alias, " Failed");
                            if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                FailedPlants.Add(item.ShortName);
                            if (FailedPlants != null && FailedPlants.Count > 0)
                            {
                                IsShowFailedPlantWarning = true;
                                WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                                WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                            }
                            System.Threading.Thread.Sleep(1000);
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", item.Alias, " Failed ", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                        }
                        catch (ServiceUnexceptedException ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", item.Alias, " Failed");
                            if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                FailedPlants.Add(item.ShortName);
                            if (FailedPlants != null && FailedPlants.Count > 0)
                            {
                                IsShowFailedPlantWarning = true;
                                WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                                WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                            }
                            System.Threading.Thread.Sleep(1000);
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", item.Alias, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", item.Alias, " Failed");
                            if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                FailedPlants.Add(item.ShortName);
                            if (FailedPlants != null && FailedPlants.Count > 0)
                            {
                                IsShowFailedPlantWarning = true;
                                WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                                WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                            }
                            System.Threading.Thread.Sleep(1000);
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", item.Alias, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }
                    }
                }
                else
                {
                    Parallel.ForEach(GeosApplication.Instance.CompanyList, itemCompaniesDetails =>
                    {
                        try
                        {

                            OrderParams objOrderParams = new OrderParams();

                            objOrderParams.idCurrency = GeosApplication.Instance.IdCurrencyByRegion;
                            objOrderParams.idsSelectedUser = salesOwnersIds;
                            objOrderParams.idUser = GeosApplication.Instance.ActiveUser.IdUser;
                            objOrderParams.idZone = GeosApplication.Instance.ActiveUser.Company.Country.IdZone;
                            objOrderParams.connectSiteDetailParams = new ConnectSiteDetailParams { idSite = Convert.ToInt32(itemCompaniesDetails.ConnectPlantId), ConnectionString = itemCompaniesDetails.ConnectPlantConstr };
                            objOrderParams.accountingYearFrom = GeosApplication.Instance.SelectedyearStarDate;
                            objOrderParams.accountingYearTo = GeosApplication.Instance.SelectedyearEndDate;
                            objOrderParams.Roles = RoleType.SalesPlantManager;
                            objOrderParams.activeSite = new ActiveSite { IdSite = Convert.ToInt32(itemCompaniesDetails.ConnectPlantId), SiteAlias = Convert.ToString(itemCompaniesDetails.Alias), SiteServiceProvider = itemCompaniesDetails.ServiceProviderUrl }; //[nsatpute][20.11.2025][GEOS2-8367]
                            var orderGridListtemp = GetOrderGridData(objOrderParams).ToList();
                            //==========================================================================================
                            //OrderGridList.AddRange(orderGridListtemp);

                            lock (OrderGridList)
                            {
                                OrderGridList.AddRange(orderGridListtemp);
                            }
                            lock (SuccessPlantList)
                            {
                                SuccessPlantList.Add(itemCompaniesDetails.ShortName);
                                if (GeosApplication.Instance.StatusMessages.Any(x => x.Message == itemCompaniesDetails.ShortName))
                                {
                                    GeosApplication.Instance.StatusMessages.FirstOrDefault(x => x.Message == itemCompaniesDetails.ShortName).IsSuccess = 1;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            string errorMessage = ex is FaultException<ServiceException> faultEx
                                ? faultEx.Detail.ErrorMessage
                                : ex.Message;

                            //GeosApplication.Instance.SplashScreenMessage = $"Connecting to {itemCompaniesDetails.Alias} Failed";

                            lock (FailedPlants)
                            {
                                FailedPlants.Add(itemCompaniesDetails.ShortName);
                            }
                            lock (FailedPlants)
                            {
                                if (FailedPlants != null && FailedPlants.Count > 0)
                                {
                                    IsShowFailedPlantWarning = true;
                                    WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                                    WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                                }
                                if (GeosApplication.Instance.StatusMessages.Any(x => x.Message == itemCompaniesDetails.ShortName))
                                {
                                    GeosApplication.Instance.StatusMessages.FirstOrDefault(x => x.Message == itemCompaniesDetails.ShortName).IsSuccess = 2;
                                }
                            }

                            System.Threading.Thread.Sleep(1000);
                            // GeosApplication.Instance.Logger.Log($"Connecting to {itemCompaniesDetails.Alias} Failed {errorMessage}", Category.Exception, Priority.Low);
                        }
                    });
                }
                GeosApplication.Instance.SplashScreenMessage = string.Empty;

                IsShowFailedPlantWarning = FailedPlants?.Count > 0;
                WarningFailedPlants = IsShowFailedPlantWarning
                    ? string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(),
                        string.Join(",", FailedPlants))
                    : string.Empty;

                #region OldCode
                //OrderList = offerOrderList;
                //OrderOptionsByOfferList = OptionsByOfferOrder;
                //var kksdj = OrderOptionsByOfferList.Where(oob => oob.IdOffer == 13034).FirstOrDefault();
                #endregion
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log("Get an error in FillOrderByUser() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log("Get an error in FillOrderByUser() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log("Get an error on FillOrderByUser() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            GeosApplication.Instance.SplashScreenMessage = string.Empty;
        }
       
        /// [001][GEOS2-3157][cpatil][11-10-2021]Improvement in the discount option. (Set the discount null by default)
        private ObservableCollection<OrderGrid> GetOrderGridData(OrderParams objOrderParams)
        {
            ObservableCollection<OrderGrid> orderGridList = new ObservableCollection<OrderGrid>();
            //string ServicePath = GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString();
            string ServicePath = objOrderParams.activeSite.SiteServiceProvider; //[nsatpute][20.11.2025][GEOS2-8367]
            
                //string ServiceUrl = "http://" + ServicePath + "/CrmRestService.svc" + "/GetOrderGridDetails_V2035";
                //[001]Service method changed
                //Service updated from GetOrderGridDetails_V2200 to GetOrderGridDetails_V2390
                //Service updated from GetOrderGridDetails_V2390 to GetOrderGridDetails_V2420
                string ServiceUrl = null;
            if (ServicePath.Contains("http://") || ServicePath.Contains("https://"))
            {
                ServiceUrl = ServicePath + "/CrmRestService.svc" + "/GetOrderGridDetails_V2670";

                if (ServiceUrl.Contains("https://"))
                {
                    //if (ServiceUrl.Contains("9.7") || ServiceUrl.Contains("9.202"))
                    System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                }
            }
            else
                ServiceUrl = "http://" + ServicePath + "/CrmRestService.svc" + "/GetOrderGridDetails_V2670";

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(ServiceUrl);
            request.Method = "POST";
            request.ContentType = "application/json";

            DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(OrderParams));

            json.WriteObject(request.GetRequestStream(), objOrderParams);
            System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();

            DataContractJsonSerializer jsonResp = new DataContractJsonSerializer(typeof(ObservableCollection<OrderGrid>));

            orderGridList = (ObservableCollection<OrderGrid>)jsonResp.ReadObject(stream);

            stream.Flush();
            stream.Close();


            return orderGridList;
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
            if (MultipleCellEditHelper.IsValueChanged)
            {
                MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["LeadseditUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    UpdateMultipleRowsCommandAction(MultipleCellEditHelper.Viewtableview);

                }
                MultipleCellEditHelper.IsValueChanged = false;
            }
            MultipleCellEditHelper.IsValueChanged = false;
            if (GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
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
                        win.Topmost = true;
                        return win;
                    }, x =>
                    {
                        return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }

                FailedPlants = new List<string>();
                IsShowFailedPlantWarning = false;
                WarningFailedPlants = String.Empty;

                FillOrderGridDetailsByPlant();

                if (FailedPlants != null && FailedPlants.Count > 0)
                {

                    IsShowFailedPlantWarning = true;
                    WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                    WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                }

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
            }
            else
            {
                OrderList = new List<Offer>();
                OrderOptionsByOfferList = new List<OptionsByOffer>();
                Dttable.Rows.Clear();
                FailedPlants.Clear();
                IsShowFailedPlantWarning = false;
                WarningFailedPlants = String.Empty;
            }

            Timer tmrOnce = new Timer();
            tmrOnce.Tick += tmrOnce_Tick;
            tmrOnce.Interval = new TimeSpan(001);
            tmrOnce.Start();

            GeosApplication.Instance.Logger.Log("Method PlantOwnerPopupClosedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
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
            if (MultipleCellEditHelper.IsValueChanged)
            {
                MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["LeadseditUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    UpdateMultipleRowsCommandAction(MultipleCellEditHelper.Viewtableview);

                }
                MultipleCellEditHelper.IsValueChanged = false;
            }
            MultipleCellEditHelper.IsValueChanged = false;
            if (GeosApplication.Instance.SelectedSalesOwnerUsersList != null)
            {
                FailedPlants = new List<string>();
                SuccessPlantList = new List<string>(); //[cpatil][13.09.2025][GEOS2-8367]
                IsShowFailedPlantWarning = false;
                WarningFailedPlants = String.Empty;

                FillOrderGridDetailsByUsers();

                if (FailedPlants != null && FailedPlants.Count > 0)
                {

                    IsShowFailedPlantWarning = true;
                    WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                    WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                }
            }
            else
            {
                OrderList = new List<Offer>();
                OrderOptionsByOfferList = new List<OptionsByOffer>();
                Dttable.Rows.Clear();

                FailedPlants.Clear();
                IsShowFailedPlantWarning = false;
                WarningFailedPlants = String.Empty;
            }

            Timer tmrOnce = new Timer();
            tmrOnce.Tick += tmrOnce_Tick;
            tmrOnce.Interval = new TimeSpan(001);
            tmrOnce.Start();

            GeosApplication.Instance.Logger.Log("Method SalesOwnerPopupClosedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method for refresh Order Grid From database by refresh Button.
        /// </summary>
        /// <param name="obj"></param>
        private void RefreshOrderDetails(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method RefreshOrderDetails ...", category: Category.Info, priority: Priority.Low);
            view = MultipleCellEditHelper.Viewtableview;

            if (MultipleCellEditHelper.IsValueChanged)
            {
                MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["LeadseditUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    UpdateMultipleRowsCommandAction(MultipleCellEditHelper.Viewtableview);

                }
                MultipleCellEditHelper.IsValueChanged = false;
            }
            MultipleCellEditHelper.IsValueChanged = false;

            if (view != null)
            {
                MultipleCellEditHelper.SetIsValueChanged(view, false);
            }
            FailedPlants = new List<string>();
            IsShowFailedPlantWarning = false;
            WarningFailedPlants = String.Empty;

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

            // code for hide column chooser if empty
            GridControl detailView = (GridControl)obj;
            GridControl gridControl = detailView;
            TableView tableView = (TableView)gridControl.View;
            //GridControl gridControl = (detailView).Grid;
            int visibleFalseCoulumn = 0;
            foreach (GridColumn column in gridControl.Columns)
            {
                if (column.Visible == false
                  && ((Emdep.Geos.UI.Helper.Column)column.DataContext).Settings.ToString() != "IsChecked"
                  && ((Emdep.Geos.UI.Helper.Column)column.DataContext).Settings.ToString() != "Array"
                  && ((Emdep.Geos.UI.Helper.Column)column.DataContext).Settings.ToString() != "Hidden")
                {
                    visibleFalseCoulumn++;
                }
            }
            if (visibleFalseCoulumn > 0)
            {
                IsOrderColumnChooserVisible = true;
            }
            else
            {
                IsOrderColumnChooserVisible = false;
            }
            var tasks = new List<Task>();
            tasks.Add(Task.Run(() => FillCmbSalesOwner()));
            tasks.Add(Task.Run(() => FillBusinessUnitList()));
            tasks.Add(Task.Run(() => FillLeadSourceList()));
            Task.WaitAll(tasks.ToArray());

            tableView.SearchString = null;

            Timer tmrOnce = new Timer();
            tmrOnce.Tick += tmrOnce_Tick;
            tmrOnce.Interval = new TimeSpan(001);
            tmrOnce.Start();

            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
            {
                DXSplashScreen.Close();
            }

            GeosApplication.Instance.Logger.Log("Method RefreshOrderDetails() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// This ticker for restore grid layout on refresh button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tmrOnce_Tick(object sender, EventArgs e)
        {
            GridService.Refresh();

            ((Timer)sender).IsEnabled = false;
            ((Timer)sender).Dispose();
        }

        /// <summary>
        /// [001][SP-67][skale][16-07-2019][GEOS2-1641]fechas vencidas en sistema CRM
        /// [002][skale][20/11/2019][GEOS2-1806]- Add new field "Offer Owner" and "Offered To" in Opportunities
        /// [003][cpatil][02/10/2023][GEOS2-4743]- Categoria High Voltage Duplicada
        /// </summary>
        /// <param name="obj"></param>
        private void OrderEditViewWindowShow(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OrderEditViewWindowShow...", category: Category.Info, priority: Priority.Low);

                TableView detailView = null;
                if (obj is TableView)
                    detailView = (TableView)obj;

                if (detailView != null && (System.Data.DataRowView)detailView.FocusedRow != null)
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
                                WindowStartupLocation = WindowStartupLocation.CenterScreen
                            };
                            WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                            win.Topmost = false;
                            return win;
                        }, x =>
                        {
                            return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                        }, null, null);
                    }


                    long idOffer = Convert.ToInt64(((System.Data.DataRowView)detailView.FocusedRow).Row.ItemArray[1].ToString());
                    Int32 ConnectPlantId = Convert.ToInt32(((System.Data.DataRowView)detailView.FocusedRow).Row.ItemArray[0].ToString());
                    IList<Offer> TempLeadsList = new List<Offer>();
                    ActiveSite offerActiveSite = new ActiveSite();
                    if (GeosApplication.Instance.CompanyList.Any(cl => cl.IdCompany.ToString() == ConnectPlantId.ToString()))
                    {
                        Company company = GeosApplication.Instance.CompanyList.Where(cmp => cmp.ConnectPlantId == ConnectPlantId.ToString()).FirstOrDefault();
                        offerActiveSite = new ActiveSite { IdSite = company.IdCompany, SiteAlias = company.Alias, SiteServiceProvider = company.ServiceProviderUrl };
                    }
                    ICrmService CrmStartUpOfferActiveSite = new CrmServiceController(offerActiveSite.SiteServiceProvider);
                    //[001] added Change Method
                    TempLeadsList.Add(CrmStartUpOfferActiveSite.GetOfferDetailsById_V2120(idOffer, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, offerActiveSite));

                    // TempLeadsList[0].DeliveryDate = Convert.ToDateTime(((System.Data.DataRowView)obj).Row.ItemArray[11].ToString());
                    LostReasonsByOffer lostReasonsByOffer = CrmStartUpOfferActiveSite.GetLostReasonsByOffer(Convert.ToInt64(TempLeadsList[0].IdOffer), GeosApplication.Instance.CompanyList.Where(cmp => cmp.ConnectPlantId == ConnectPlantId.ToString()).Select(x => x.ConnectPlantConstr).FirstOrDefault());

                    if (lostReasonsByOffer != null)
                    {
                        TempLeadsList[0].LostReasonsByOffer = lostReasonsByOffer;
                    }

                    OrderEditViewModel orderEditViewModel = new OrderEditViewModel();
                    OrderEditView orderEditView = new OrderEditView();
                    orderEditViewModel.IsControlEnable = true;
                    orderEditViewModel.IsControlEnableorder = false;
                    orderEditViewModel.IsStatusChangeAction = true;
                    orderEditViewModel.IsAcceptControlEnableorder = false;
                    orderEditViewModel.InItOrder(TempLeadsList);

                    EventHandler handle = delegate { orderEditView.Close(); };
                    orderEditViewModel.RequestClose += handle;
                    orderEditView.DataContext = orderEditViewModel;


                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                    IsOrderColumnChooserVisible = false;
                    var ownerInfo = (detailView as FrameworkElement);
                    orderEditView.Owner = Window.GetWindow(ownerInfo);
                    orderEditView.ShowDialogWindow();

                    IsOrderColumnChooserVisible = true;

                    if (orderEditViewModel.OfferData != null)
                    {
                       
                        FillCarProjectList();
                        DataRow dataRow = Dttable.AsEnumerable().FirstOrDefault(row => Convert.ToInt64(row["Idoffer"]) == idOffer && Convert.ToInt32(row["ConnectPlantId"]) == ConnectPlantId);
                        if (orderEditViewModel.OfferData.Discount != 0)//[rdixit][GEOS2-2911][06.07.2022]
                            dataRow["Discount"] = orderEditViewModel.OfferData.Discount;
                        else
                            dataRow["Discount"] = 0;
                        if (orderEditViewModel.OfferData.CarProject != null)
                            dataRow["Project"] = orderEditViewModel.OfferData.CarProject.Name;
                        if (orderEditViewModel.OfferData.CarOEM != null)
                            dataRow["OEM"] = orderEditViewModel.OfferData.CarOEM.Name;
                        if (orderEditViewModel.OfferData.Source != null)
                            dataRow["Source"] = orderEditViewModel.OfferData.Source.Value;
                        if (orderEditViewModel.OfferData.BusinessUnit != null)
                            dataRow["BusinessUnit"] = orderEditViewModel.OfferData.BusinessUnit.Value;

                        Int64? idCategory = CrmStartUp.GetIdProductCategoryByIdOffer(orderEditViewModel.OfferData.IdOffer, GeosApplication.Instance.CompanyList.Where(cmp => cmp.ConnectPlantId == ConnectPlantId.ToString()).FirstOrDefault().ConnectPlantConstr);

                        if (idCategory > 0)
                        {

                            ProductCategory objProCat = new ProductCategory();
                            objProCat = orderEditViewModel.ListProductCategory.Where(i => i.IdProductCategory == idCategory.Value).FirstOrDefault();

                            if (objProCat.MergedCategoryAndProduct != null)
                            {
                                objProCat.MergedCategoryAndProduct = objProCat.MergedCategoryAndProduct.Replace("-", "");
                                string[] strArray = objProCat.MergedCategoryAndProduct.Split('>');
                                if (strArray.Count() < 2)
                                {
                                    dataRow["Category1"] = strArray[0] != null ? strArray[0].Trim() : strArray[0];// [003]
                                    dataRow["Category2"] = null;
                                }
                                else
                                {
                                    dataRow["Category1"] = strArray[0] != null ? strArray[0].Trim() : strArray[0];// [003]
                                    dataRow["Category2"] = strArray[1] != null ? strArray[1].Trim() : strArray[1];// [003]
                                }

                            }

                        }

                        //[002] added
                        if (orderEditViewModel.ListAddedOfferContact != null)
                        {
                            List<OfferContact> OfferContactlist = orderEditViewModel.ListAddedOfferContact.Where(x => x.IsDeleted != true).ToList();

                            if (OfferContactlist.Count > 0)
                            {
                                dataRow["OfferedTo"] = String.Join("\n", OfferContactlist.Select(x => x.People.FullName).Distinct().ToArray());
                            }
                            else
                            {
                                dataRow["OfferedTo"] = null;
                            }
                        }
                        else
                            dataRow["OfferedTo"] = null;

                        if (orderEditViewModel.OfferOwnerList.Count > 0)
                        {
                            User user = orderEditViewModel.OfferOwnerList.Where(x => x.IdUser == orderEditViewModel.OfferData.OfferedBy).FirstOrDefault();

                            if (!string.IsNullOrEmpty(user.FullName))
                            {
                                dataRow["OfferOwner"] = user.FullName;

                            }
                            else
                                dataRow["OfferOwner"] = null;
                        }


                    }

                    // code for hide column chooser if empty
                    GridControl gridControl = (detailView).Grid;
                    try
                    {
                        //Shubham[skadam] GEOS2-5170 Crear nuevos TAG en Oportunities/Orders y Acciones 13 02 2024
                        gridControl.RefreshData();
                        gridControl.UpdateLayout();
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log("Get an error in OrderEditViewWindowShow() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                    }
                    int visibleFalseCoulumn = 0;
                    foreach (GridColumn column in gridControl.Columns)
                    {
                        if (column.Visible == false && ((Emdep.Geos.UI.Helper.Column)column.DataContext).Settings.ToString() != "IsChecked"
                        && ((Emdep.Geos.UI.Helper.Column)column.DataContext).Settings.ToString() != "Array"
                        && ((Emdep.Geos.UI.Helper.Column)column.DataContext).Settings.ToString() != "Hidden")
                        {
                            visibleFalseCoulumn++;
                        }
                    }

                    if (visibleFalseCoulumn > 0)
                    {
                        IsOrderColumnChooserVisible = true;
                    }
                    else
                    {
                        IsOrderColumnChooserVisible = false;
                    }
                }
                FillLeadSourceList();//[Sudhir.jangra][GEOS2-5170]
                GeosApplication.Instance.Logger.Log("Method OrderEditViewWindowShow executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OrderEditViewWindowShow() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OrderEditViewWindowShow() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OrderEditViewWindowShow() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void CustomCellAppearanceGridControl(RoutedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CustomCellAppearanceGridControl ...", category: Category.Info, priority: Priority.Low);

                int visibleFalseCoulumn = 0;

                if (File.Exists(OrderGridSettingFilePath))
                {

                    ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.RestoreLayoutFromXml(OrderGridSettingFilePath);
                }

                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.AddHandler(DXSerializer.AllowPropertyEvent, new AllowPropertyEventHandler(OnAllowProperty));
                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.View.AddHandler(DXSerializer.AllowPropertyEvent, new AllowPropertyEventHandler(OnAllowProperty));

                //This code for save grid layout...
                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.SaveLayoutToXml(OrderGridSettingFilePath);

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

                    if (column.Visible == false
                        && ((Emdep.Geos.UI.Helper.Column)column.DataContext).Settings.ToString() != "IsChecked"
                        && ((Emdep.Geos.UI.Helper.Column)column.DataContext).Settings.ToString() != "Array"
                        && ((Emdep.Geos.UI.Helper.Column)column.DataContext).Settings.ToString() != "Hidden")
                    {
                        visibleFalseCoulumn++;
                    }
                }

                if (visibleFalseCoulumn > 0)
                {
                    IsOrderColumnChooserVisible = true;
                }
                else
                {
                    IsOrderColumnChooserVisible = false;
                }

                TableView datailView = ((TableView)((System.Windows.RoutedEventArgs)obj).OriginalSource);
                datailView.BestFitColumns();

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

            if (e.DependencyProperty == TableViewEx.SearchStringProperty)
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
                    ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(OrderGridSettingFilePath);
                }

                if (column.Visible == false)
                {
                    IsOrderColumnChooserVisible = true;
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
                ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(OrderGridSettingFilePath);

                GeosApplication.Instance.Logger.Log("Method VisibleIndexChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in VisibleIndexChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void UpdateFilterString()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method UpdateFilterString ...", category: Category.Info, priority: Priority.Low);

                if (StartSelectionDate > new DateTime(2000, 1, 1) && FinishSelectionDate > new DateTime(2000, 1, 1))
                {
                    string st = string.Format("[PODate] >= #{0}# And [PODate] < #{1}#", StartSelectionDate.ToString("MM-dd-yyyy"), FinishSelectionDate.ToString("MM-dd-yyyy"));
                    MyFilterString = st;
                }

                GeosApplication.Instance.Logger.Log("Method UpdateFilterString() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in UpdateFilterString() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillOrder()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillOrder ...", category: Category.Info, priority: Priority.Low);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
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
                    return new SplashScreenCustomView() { DataContext = new SplashScreenViewModel() };
                }, null, null);
                OrderGridList = new List<OrderGrid>();
                bool isShowFailedPlantWarning = false;
                string warningFailedPlants = string.Empty;
                GeosApplication.Instance.CustomeSplashScreenMessage = "Please wait";
                GeosApplication.Instance.CustomeSplashScreenInformationMessage = "Collecting the information from plants :";
                GeosApplication.Instance.StatusMessages = new ObservableCollection<Data.Common.Crm.StatusMessage>();
                foreach (Company com in GeosApplication.Instance.CompanyList)
                {
                    GeosApplication.Instance.StatusMessages.Add(new Data.Common.Crm.StatusMessage() { Symbol = "", Message = com.ShortName, IsSuccess = 0 });
                }
                if (GeosApplication.Instance.CompanyList.Count == 1)
                {
                    // Continue loop although some plant is not available and Show error message.
                    foreach (var item in GeosApplication.Instance.CompanyList)
                    {
                        try
                        {
                            GeosApplication.Instance.SplashScreenMessage = "Connecting to " + item.Alias;

                            //==========================================================================================
                         
                            OrderParams objOrderParams = new OrderParams();

                            objOrderParams.idCurrency = GeosApplication.Instance.IdCurrencyByRegion;
                            objOrderParams.idsSelectedUser = GeosApplication.Instance.ActiveUser.IdUser.ToString();
                            objOrderParams.idUser = GeosApplication.Instance.ActiveUser.IdUser;
                            objOrderParams.idZone = GeosApplication.Instance.ActiveUser.Company.Country.IdZone;
                            objOrderParams.connectSiteDetailParams = new ConnectSiteDetailParams { idSite = Convert.ToInt32(item.ConnectPlantId), ConnectionString = item.ConnectPlantConstr };
                            objOrderParams.accountingYearFrom = GeosApplication.Instance.SelectedyearStarDate;
                            objOrderParams.accountingYearTo = GeosApplication.Instance.SelectedyearEndDate;
                            objOrderParams.Roles = RoleType.SalesAssistant;
							//[nsatpute][20.11.2025][GEOS2-8367]
                            objOrderParams.activeSite = new ActiveSite { IdSite = Convert.ToInt32(item.ConnectPlantId), SiteAlias = Convert.ToString(item.Alias), SiteServiceProvider = item.ServiceProviderUrl };
                            var orderGridListtemp = GetOrderGridData(objOrderParams).ToList();
                            //==========================================================================================
                            //offlst = CrmStartUp.GetOffersWithoutPurchaseOrderReturnListDatatable(GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.CrmOfferYear, itemCompaniesDetails, GeosApplication.Instance.IdUserPermission);

                            OrderGridList.AddRange(orderGridListtemp);

                            #region OldCode
                            //offersOptionsLst = CrmStartUp.GetOrdersDatatable(GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, item, GeosApplication.Instance.IdUserPermission);

                            //offerOrderList.AddRange(offersOptionsLst.Offers);
                            //OptionsByOfferOrder.AddRange(offersOptionsLst.OptionsByOffers);
                            #endregion
                        }
                        catch (FaultException<ServiceException> ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", item.Alias, " Failed");
                            if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                FailedPlants.Add(item.ShortName);
                            if (FailedPlants != null && FailedPlants.Count > 0)
                            {
                                IsShowFailedPlantWarning = true;
                                WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                                WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                            }
                            System.Threading.Thread.Sleep(1000);
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", item.Alias, " Failed ", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                        }
                        catch (ServiceUnexceptedException ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", item.Alias, " Failed");
                            if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                FailedPlants.Add(item.ShortName);
                            if (FailedPlants != null && FailedPlants.Count > 0)
                            {
                                IsShowFailedPlantWarning = true;
                                WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                                WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                            }
                            System.Threading.Thread.Sleep(1000);
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", item.Alias, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", item.Alias, " Failed");
                            if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                FailedPlants.Add(item.ShortName);
                            if (FailedPlants != null && FailedPlants.Count > 0)
                            {
                                IsShowFailedPlantWarning = true;
                                WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                                WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                            }
                            System.Threading.Thread.Sleep(1000);
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", item.Alias, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }
                    }
                }
                else
                {
                    Parallel.ForEach(GeosApplication.Instance.CompanyList, itemCompaniesDetails =>
                    {

                        try
                        {
                         
                            OrderParams objOrderParams = new OrderParams();

                            objOrderParams.idCurrency = GeosApplication.Instance.IdCurrencyByRegion;
                            objOrderParams.idsSelectedUser = GeosApplication.Instance.ActiveUser.IdUser.ToString();
                            objOrderParams.idUser = GeosApplication.Instance.ActiveUser.IdUser;
                            objOrderParams.idZone = GeosApplication.Instance.ActiveUser.Company.Country.IdZone;
                            objOrderParams.connectSiteDetailParams = new ConnectSiteDetailParams { idSite = Convert.ToInt32(itemCompaniesDetails.ConnectPlantId), ConnectionString = itemCompaniesDetails.ConnectPlantConstr };
                            objOrderParams.accountingYearFrom = GeosApplication.Instance.SelectedyearStarDate;
                            objOrderParams.accountingYearTo = GeosApplication.Instance.SelectedyearEndDate;
                            objOrderParams.Roles = RoleType.SalesAssistant;
							//[nsatpute][20.11.2025][GEOS2-8367]
                            objOrderParams.activeSite = new ActiveSite { IdSite = Convert.ToInt32(itemCompaniesDetails.ConnectPlantId), SiteAlias = Convert.ToString(itemCompaniesDetails.Alias), SiteServiceProvider = itemCompaniesDetails.ServiceProviderUrl };
                            var orderGridListtemp = GetOrderGridData(objOrderParams).ToList();
                            //==========================================================================================
                            //offlst = CrmStartUp.GetOffersWithoutPurchaseOrderReturnListDatatable(GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.CrmOfferYear, itemCompaniesDetails, GeosApplication.Instance.IdUserPermission);

                       
                            lock (OrderGridList)
                            {
                                OrderGridList.AddRange(orderGridListtemp);
                            }
                            lock (SuccessPlantList)
                            {
                                SuccessPlantList.Add(itemCompaniesDetails.ShortName);
                                if (GeosApplication.Instance.StatusMessages.Any(x => x.Message == itemCompaniesDetails.ShortName))
                                {
                                    GeosApplication.Instance.StatusMessages.FirstOrDefault(x => x.Message == itemCompaniesDetails.ShortName).IsSuccess = 1;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            string errorMessage = ex is FaultException<ServiceException> faultEx
                                ? faultEx.Detail.ErrorMessage
                                : ex.Message;

                            //GeosApplication.Instance.SplashScreenMessage = $"Connecting to {itemCompaniesDetails.Alias} Failed";

                            lock (FailedPlants)
                            {
                                FailedPlants.Add(itemCompaniesDetails.ShortName);
                            }
                            lock (FailedPlants)
                            {
                                if (FailedPlants != null && FailedPlants.Count > 0)
                                {
                                    IsShowFailedPlantWarning = true;
                                    WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                                    WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                                }
                                if (GeosApplication.Instance.StatusMessages.Any(x => x.Message == itemCompaniesDetails.ShortName))
                                {
                                    GeosApplication.Instance.StatusMessages.FirstOrDefault(x => x.Message == itemCompaniesDetails.ShortName).IsSuccess = 2;
                                }
                            }

                            System.Threading.Thread.Sleep(1000);
                            // GeosApplication.Instance.Logger.Log($"Connecting to {itemCompaniesDetails.Alias} Failed {errorMessage}", Category.Exception, Priority.Low);
                        }
                    });
                }


                GeosApplication.Instance.SplashScreenMessage = string.Empty;

                if (FailedPlants == null || FailedPlants.Count == 0)
                {
                    IsShowFailedPlantWarning = false;
                    WarningFailedPlants = string.Empty;
                }

                //OrderList = offerOrderList;
                //OrderOptionsByOfferList = OptionsByOfferOrder;

                GeosApplication.Instance.Logger.Log("Method FillOrder() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log("Get an error in FillOrder() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log("Get an error in FillOrder() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }

            GeosApplication.Instance.SplashScreenMessage = string.Empty;
        }

        private void FillOrderPlants()
        {
            try
            {
                //[cpatil][13.09.2025][GEOS2-8367]
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
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
                    return new SplashScreenCustomView() { DataContext = new SplashScreenViewModel() };
                }, null, null);

                GeosApplication.Instance.Logger.Log("Method FillOrderPlants ...", category: Category.Info, priority: Priority.Low);
                OrderGridList = new List<OrderGrid>();
                if (GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
                {
                    List<Company> plantOwners = GeosApplication.Instance.SelectedPlantOwnerUsersList.Cast<Company>().ToList();
                    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.ShortName));
                    PreviouslySelectedPlantOwners = string.Join(",", plantOwners.Select(i => i.ConnectPlantId));
                    //[cpatil][13.09.2025][GEOS2-8367]
                    GeosApplication.Instance.CustomeSplashScreenMessage = "Please wait";
                    GeosApplication.Instance.CustomeSplashScreenInformationMessage = "Collecting the information from plants :";
                    GeosApplication.Instance.StatusMessages = new ObservableCollection<Data.Common.Crm.StatusMessage>();

                    foreach (Company com in plantOwners)
                    {
                        GeosApplication.Instance.StatusMessages.Add(new Data.Common.Crm.StatusMessage() { Symbol = "", Message = com.ShortName, IsSuccess = 0 });
                    }
                    if (plantOwners.Count == 1)
                    {
                        // Continue loop although some plant is not available and Show error message.
                        foreach (var item in plantOwners)
                        {
                            try
                            {
                                GeosApplication.Instance.SplashScreenMessage = "Connecting to " + item.ShortName;

                                //==========================================================================================
                              
                                OrderParams objOrderParams = new OrderParams();

                                objOrderParams.idCurrency = GeosApplication.Instance.IdCurrencyByRegion;
                                objOrderParams.idsSelectedUser = GeosApplication.Instance.ActiveUser.IdUser.ToString();
                                objOrderParams.idUser = GeosApplication.Instance.ActiveUser.IdUser;
                                objOrderParams.idZone = GeosApplication.Instance.ActiveUser.Company.Country.IdZone;
                                objOrderParams.connectSiteDetailParams = new ConnectSiteDetailParams { idSite = Convert.ToInt32(item.ConnectPlantId), ConnectionString = item.ConnectPlantConstr };
                                objOrderParams.accountingYearFrom = GeosApplication.Instance.SelectedyearStarDate;
                                objOrderParams.accountingYearTo = GeosApplication.Instance.SelectedyearEndDate;
                                objOrderParams.Roles = RoleType.SalesGlobalManager;
								//[nsatpute][20.11.2025][GEOS2-8367]
                                objOrderParams.activeSite = new ActiveSite { IdSite = Convert.ToInt32(item.ConnectPlantId), SiteAlias = Convert.ToString(item.Alias), SiteServiceProvider = item.ServiceProviderUrl };
                                var  orderGridListtemp = GetOrderGridData(objOrderParams).ToList();
                                //==========================================================================================


                                OrderGridList.AddRange(orderGridListtemp);

                                #region OldCode
                                //offersOptionsLst = CrmStartUp.GetOrdersDatatable(GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, item, GeosApplication.Instance.IdUserPermission);

                                //offerOrderList.AddRange(offersOptionsLst.Offers);
                                //OptionsByOfferOrder.AddRange(offersOptionsLst.OptionsByOffers);
                                #endregion
                            }
                            catch (FaultException<ServiceException> ex)
                            {
                                GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", item.ShortName, " Failed");
                                if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                    FailedPlants.Add(item.ShortName);
                                if (FailedPlants != null && FailedPlants.Count > 0)
                                {
                                    IsShowFailedPlantWarning = true;
                                    WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                                    WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                                }
                                System.Threading.Thread.Sleep(1000);
                                GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", item.ShortName, " Failed ", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                            }
                            catch (ServiceUnexceptedException ex)
                            {
                                GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", item.ShortName, " Failed");
                                if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                    FailedPlants.Add(item.ShortName);
                                if (FailedPlants != null && FailedPlants.Count > 0)
                                {
                                    IsShowFailedPlantWarning = true;
                                    WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                                    WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                                }
                                System.Threading.Thread.Sleep(1000);
                                GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", item.ShortName, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                            }
                            catch (Exception ex)
                            {
                                GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", item.ShortName, " Failed");
                                if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                    FailedPlants.Add(item.ShortName);
                                if (FailedPlants != null && FailedPlants.Count > 0)
                                {
                                    IsShowFailedPlantWarning = true;
                                    WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                                    WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                                }
                                System.Threading.Thread.Sleep(1000);
                                GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", item.ShortName, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                            }
                        }
                    }
                    else
                    {
                        Parallel.ForEach(plantOwners, itemPlantOwnerUsers =>
                        {
                            try
                            {
                                //==========================================================================================
                               
                                OrderParams objOrderParams = new OrderParams();

                                objOrderParams.idCurrency = GeosApplication.Instance.IdCurrencyByRegion;
                                objOrderParams.idsSelectedUser = GeosApplication.Instance.ActiveUser.IdUser.ToString();
                                objOrderParams.idUser = GeosApplication.Instance.ActiveUser.IdUser;
                                objOrderParams.idZone = GeosApplication.Instance.ActiveUser.Company.Country.IdZone;
                                objOrderParams.connectSiteDetailParams = new ConnectSiteDetailParams { idSite = Convert.ToInt32(itemPlantOwnerUsers.ConnectPlantId), ConnectionString = itemPlantOwnerUsers.ConnectPlantConstr };
                                objOrderParams.accountingYearFrom = GeosApplication.Instance.SelectedyearStarDate;
                                objOrderParams.accountingYearTo = GeosApplication.Instance.SelectedyearEndDate;
                                objOrderParams.Roles = RoleType.SalesGlobalManager;
								//[nsatpute][20.11.2025][GEOS2-8367]
                                objOrderParams.activeSite = new ActiveSite { IdSite = Convert.ToInt32(itemPlantOwnerUsers.ConnectPlantId), SiteAlias = Convert.ToString(itemPlantOwnerUsers.Alias), SiteServiceProvider = itemPlantOwnerUsers.ServiceProviderUrl };
                                var  orderGridListtemp = GetOrderGridData(objOrderParams).ToList();
                                //==========================================================================================

                                                            
                                lock (OrderGridList)
                                {
                                    OrderGridList.AddRange(orderGridListtemp);
                                }
                                lock (SuccessPlantList)
                                {
                                    SuccessPlantList.Add(itemPlantOwnerUsers.ShortName);

                                    if (GeosApplication.Instance.StatusMessages.Any(x => x.Message == itemPlantOwnerUsers.ShortName))
                                    {
                                        GeosApplication.Instance.StatusMessages.FirstOrDefault(x => x.Message == itemPlantOwnerUsers.ShortName).IsSuccess = 1;
                                    }
                                }
                            }
                            catch (FaultException<ServiceException> ex)
                            {
                                GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemPlantOwnerUsers.ShortName, " Failed");

                                if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                    lock (FailedPlants)
                                    {
                                        FailedPlants.Add(itemPlantOwnerUsers.ShortName);
                                    }

                                lock (FailedPlants)
                                {
                                    if (FailedPlants != null && FailedPlants.Count > 0)
                                    {
                                        IsShowFailedPlantWarning = true;
                                        WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                                        WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());

                                        if (GeosApplication.Instance.StatusMessages.Any(x => x.Message == itemPlantOwnerUsers.ShortName))
                                        {
                                            GeosApplication.Instance.StatusMessages.FirstOrDefault(x => x.Message == itemPlantOwnerUsers.ShortName).IsSuccess = 2;
                                        }
                                    }
                                }
                                System.Threading.Thread.Sleep(1000);
                                GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemPlantOwnerUsers.ShortName, " Failed ", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                            }
                            catch (ServiceUnexceptedException ex)
                            {
                                GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemPlantOwnerUsers.ShortName, " Failed");
                                if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                    lock (FailedPlants)
                                    {
                                        FailedPlants.Add(itemPlantOwnerUsers.ShortName);
                                    }

                                lock (FailedPlants)
                                {
                                    if (FailedPlants != null && FailedPlants.Count > 0)
                                    {
                                        IsShowFailedPlantWarning = true;
                                        WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                                        WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                                        if (GeosApplication.Instance.StatusMessages.Any(x => x.Message == itemPlantOwnerUsers.ShortName))
                                        {
                                            GeosApplication.Instance.StatusMessages.FirstOrDefault(x => x.Message == itemPlantOwnerUsers.ShortName).IsSuccess = 2;
                                        }
                                    }
                                }
                                System.Threading.Thread.Sleep(1000);
                                GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemPlantOwnerUsers.ShortName, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                            }
                            catch (Exception ex)
                            {
                                GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemPlantOwnerUsers.ShortName, " Failed");
                                if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                    lock (FailedPlants)
                                    {
                                        FailedPlants.Add(itemPlantOwnerUsers.ShortName);
                                    }

                                lock (FailedPlants)
                                {
                                    if (FailedPlants != null && FailedPlants.Count > 0)
                                    {
                                        IsShowFailedPlantWarning = true;
                                        WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                                        WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                                    }
                                    if (GeosApplication.Instance.StatusMessages.Any(x => x.Message == itemPlantOwnerUsers.ShortName))
                                    {
                                        GeosApplication.Instance.StatusMessages.FirstOrDefault(x => x.Message == itemPlantOwnerUsers.ShortName).IsSuccess = 2;
                                    }
                                }
                                System.Threading.Thread.Sleep(1000);
                                GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemPlantOwnerUsers.ShortName, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                            }
                        });
                    }


                    GeosApplication.Instance.SplashScreenMessage = string.Empty;

                    if (FailedPlants == null || FailedPlants.Count == 0)
                    {
                        IsShowFailedPlantWarning = false;
                        WarningFailedPlants = string.Empty;
                    }

                    //OrderList = offerOrderList;
                    //OrderOptionsByOfferList = OptionsByOfferOrder;

                    GeosApplication.Instance.Logger.Log("Method FillOrderPlants() executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log("Get an error in FillOrderPlants() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log("Get an error in FillOrderPlants() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }

            GeosApplication.Instance.SplashScreenMessage = string.Empty;
        }
        /// <summary>
        /// [lsharma][07/06/2018][Sprint 40][CRM-M040-06] Invoice amount in Red if is less than offer amount]
        /// Created a new column template InvoiceAmount in viewmodel as well as in view
        /// [001][skale][20/11/2019][GEOS2-1806]- Add new field "Offer Owner" and "Offered To" in Opportunities
        /// </summary>
        private void Fillgrid()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Fillgrid ...", category: Category.Info, priority: Priority.Low);

                Columns = new ObservableCollection<Emdep.Geos.UI.Helper.Column>()
                {
                    new Emdep.Geos.UI.Helper.Column() { FieldName = "IdOffer", HeaderText = "IdOffer", Settings = SettingsType.Hidden, AllowCellMerge = false, Width = 120, AllowEditing = false, Visible = false, IsVertical = false,FixedWidth=false  },
                    new Emdep.Geos.UI.Helper.Column() { FieldName = "ConnectPlantId", HeaderText = "ConnectPlantId", Settings = SettingsType.Hidden, AllowCellMerge = false, Width = 120, AllowEditing = false, Visible = false, IsVertical = false ,FixedWidth=false },
                    new Emdep.Geos.UI.Helper.Column() { FieldName = "Code", HeaderText = "Code", Settings = SettingsType.OfferCode, AllowCellMerge = false, Width = 120, AllowEditing = false, Visible = true, IsVertical = false ,FixedWidth=false },
                    new Emdep.Geos.UI.Helper.Column() { FieldName = "IsChecked",HeaderText="IsChecked", Settings = SettingsType.IsChecked, AllowCellMerge=false, Width=120,AllowEditing=false,Visible=false,IsVertical= false,FixedWidth=false  },
                    new Emdep.Geos.UI.Helper.Column() { FieldName = "Group", HeaderText = "Group", Settings = SettingsType.Default, AllowCellMerge = false, Width = 100, AllowEditing = false, Visible = true, IsVertical = false,FixedWidth=false },
                    new Emdep.Geos.UI.Helper.Column() { FieldName = "Plant", HeaderText = "Plant", Settings = SettingsType.Default, AllowCellMerge = false, Width = 150, AllowEditing = false, Visible = true, IsVertical = false,FixedWidth=false },
                    new Emdep.Geos.UI.Helper.Column() { FieldName=  "Project",HeaderText="Project", Settings = SettingsType.Project, AllowCellMerge=false, Width=120,AllowEditing=true,Visible=true,IsVertical= false,FixedWidth=false  },
                    new Emdep.Geos.UI.Helper.Column() { FieldName = "Country", HeaderText = "Country", Settings = SettingsType.CountryWithImage, AllowCellMerge = false, Width = 150, AllowEditing = false, Visible = true, IsVertical = false,FixedWidth=false },

                    new Emdep.Geos.UI.Helper.Column() { FieldName = "Region", HeaderText = "Region", Settings = SettingsType.Default, AllowCellMerge = false, Width = 100, AllowEditing = false, Visible = true, IsVertical = false,FixedWidth=false },
                    new Emdep.Geos.UI.Helper.Column() { FieldName = "Description", HeaderText = "Description", Settings = SettingsType.Default, AllowCellMerge = false, Width = 250, AllowEditing = false, Visible = true, IsVertical = false,FixedWidth=false },

                    new Emdep.Geos.UI.Helper.Column() { FieldName = "Status", HeaderText = "Status", Settings = SettingsType.Status, AllowCellMerge = false, Width = 200, AllowEditing = false, Visible = true, IsVertical = false,FixedWidth=false },
                    new Emdep.Geos.UI.Helper.Column() { FieldName = "HtmlColor", HeaderText = "HtmlColor", Settings = SettingsType.Hidden, AllowCellMerge = false, Width = 200, AllowEditing = false, Visible = false, IsVertical = false,FixedWidth=false },
                    new Emdep.Geos.UI.Helper.Column() { FieldName = "Amount", HeaderText = "Amount", Settings = SettingsType.Amount, AllowCellMerge = false, Width = 170, AllowEditing = false, Visible = true, IsVertical = false,FixedWidth=false },
                    new Emdep.Geos.UI.Helper.Column() { FieldName = "Discount", HeaderText = "Discount", Settings = SettingsType.Discount, AllowCellMerge = false, Width = 170, AllowEditing = true, Visible = true, IsVertical = false,FixedWidth=false },
                    new Emdep.Geos.UI.Helper.Column() { FieldName = "InvoiceAmount", HeaderText = "Invoice Amount", Settings = SettingsType.InvoiceAmount, AllowCellMerge = false, Width = 170, AllowEditing = false, Visible = true, IsVertical = false,FixedWidth=false },
                    new Emdep.Geos.UI.Helper.Column() { FieldName = "DeliveryDate", HeaderText = "Delivery Date", Settings = SettingsType.Default, AllowCellMerge = false, Width = 90, AllowEditing = false, Visible = true, IsVertical = false,FixedWidth=false },
                    new Emdep.Geos.UI.Helper.Column() { FieldName = "PODate", HeaderText = "PODate", Settings = SettingsType.Default, AllowCellMerge = false, Width = 90, AllowEditing = false, Visible = true, IsVertical = false,FixedWidth=true },
                    new Emdep.Geos.UI.Helper.Column() { FieldName = "SalesOwner", HeaderText = "Sales Owner", Settings = SettingsType.SalesOwner, AllowCellMerge = false, Width = 150, AllowEditing = false, Visible = true, IsVertical = false,FixedWidth=false },
                    new Emdep.Geos.UI.Helper.Column() { FieldName = "IsSaleOwnerNull", HeaderText = "IsSaleOwnerNull", Settings = SettingsType.Hidden, AllowCellMerge=false,Width=10,AllowEditing=false,Visible=false,IsVertical= false,FixedWidth=false },
                    new Emdep.Geos.UI.Helper.Column() { FieldName="Category1",HeaderText="Category1", Settings = SettingsType.Category1, AllowCellMerge=false, Width=120,AllowEditing=true,Visible=false,IsVertical= false,FixedWidth=false  },
                    new Emdep.Geos.UI.Helper.Column() { FieldName="Category2",HeaderText="Category2", Settings = SettingsType.Category2, AllowCellMerge=false, Width=120,AllowEditing=true,Visible=false,IsVertical= false,FixedWidth=false  },
                    new Emdep.Geos.UI.Helper.Column() { FieldName = "ShippingDate", HeaderText = "Shipping Date", Settings = SettingsType.OthersFields, AllowCellMerge = false, Width = 90, AllowEditing = false, Visible = false, IsVertical = false,FixedWidth=false },
                };
                //Added extra columns in column chooser. 
                {
                    Columns.Add(new Emdep.Geos.UI.Helper.Column() { FieldName = "OEM", HeaderText = "OEM", Settings = SettingsType.OEM, AllowCellMerge = false, Width = 120, AllowEditing = true, Visible = false, IsVertical = false, FixedWidth = false });
                    Columns.Add(new Emdep.Geos.UI.Helper.Column() { FieldName = "Source", HeaderText = "Source", Settings = SettingsType.Source, AllowCellMerge = false, Width = 120, AllowEditing = true, Visible = false, IsVertical = false, FixedWidth = false });
                    Columns.Add(new Emdep.Geos.UI.Helper.Column() { FieldName = "BusinessUnit", HeaderText = "Business Unit", Settings = SettingsType.BusinessUnit, AllowCellMerge = false, Width = 120, AllowEditing = true, Visible = false, IsVertical = false, FixedWidth = false });
                    Columns.Add(new Emdep.Geos.UI.Helper.Column() { FieldName = "RFQReceptionDate", HeaderText = "RFQ Reception Date", Settings = SettingsType.RFQDate, AllowCellMerge = false, Width = 90, AllowEditing = false, Visible = false, IsVertical = false, FixedWidth = false });
                    Columns.Add(new Emdep.Geos.UI.Helper.Column() { FieldName = "QuoteSentDate", HeaderText = "Quote Sent Date", Settings = SettingsType.QuoteSentDate, AllowCellMerge = false, Width = 90, AllowEditing = false, Visible = false, IsVertical = false, FixedWidth = false });
                    Columns.Add(new Emdep.Geos.UI.Helper.Column() { FieldName = "CustomerPOCodes", HeaderText = "PO", Settings = SettingsType.PO, AllowCellMerge = false, Width = 120, AllowEditing = false, Visible = false, IsVertical = false, FixedWidth = false });
                    //[001] added
                    Columns.Add(new Emdep.Geos.UI.Helper.Column() { FieldName = "OfferOwner", HeaderText = "Offer Owner", Settings = SettingsType.OfferOwner, AllowCellMerge = false, Width = 70, AllowEditing = false, Visible = false, IsVertical = false, FixedWidth = false });
                    Columns.Add(new Emdep.Geos.UI.Helper.Column() { FieldName = "OfferedTo", HeaderText = "Offered To", Settings = SettingsType.OfferedTo, AllowCellMerge = false, Width = 70, AllowEditing = false, Visible = false, IsVertical = false, FixedWidth = false });

                }

                Dttable = new DataTable();
                Dttable.Columns.Add("ConnectPlantId", typeof(Int32));
                Dttable.Columns.Add("IdOffer", typeof(long));
                Dttable.Columns.Add("Code", typeof(string));
                Dttable.Columns.Add("IsChecked", typeof(bool));
                Dttable.Columns.Add("Country", typeof(string));
                Dttable.Columns.Add("Region", typeof(string));
                Dttable.Columns.Add("Group", typeof(string));
                Dttable.Columns.Add("Plant", typeof(string));
                Dttable.Columns.Add("Site", typeof(string));
                Dttable.Columns.Add("Description", typeof(string));
                Dttable.Columns.Add("Status", typeof(string));
                Dttable.Columns.Add("HtmlColor", typeof(string));
                Dttable.Columns.Add("Amount", typeof(double));
                Dttable.Columns.Add("Discount", typeof(double));
                Dttable.Columns.Add("InvoiceAmount", typeof(double));
                Dttable.Columns.Add("DeliveryDate", typeof(DateTime));
                Dttable.Columns.Add("PODate", typeof(DateTime));
                Dttable.Columns.Add("SalesOwner", typeof(string));
                Dttable.Columns.Add("IsSaleOwnerNull", typeof(bool));
                Dttable.Columns.Add("Project", typeof(string));
                Dttable.Columns.Add("Category1", typeof(string));
                Dttable.Columns.Add("Category2", typeof(string));
                Dttable.Columns.Add("ShippingDate", typeof(DateTime));
                {
                    Dttable.Columns.Add("OEM", typeof(string));
                    Dttable.Columns.Add("Source", typeof(string));
                    Dttable.Columns.Add("BusinessUnit", typeof(string));
                    Dttable.Columns.Add("RFQReceptionDate", typeof(DateTime));
                    Dttable.Columns.Add("QuoteSentDate", typeof(DateTime));
                    Dttable.Columns.Add("CustomerPOCodes", typeof(string));
                    //[001] added
                    Dttable.Columns.Add("OfferOwner", typeof(string));
                    Dttable.Columns.Add("OfferedTo", typeof(string));
                }

                Templates = new List<Template>();
                // Total Summary
                TotalSummary = new ObservableCollection<Summary>();
                TotalSummary.Add(new Summary() { Type = SummaryItemType.Sum, FieldName = "Amount", DisplayFormat = " {0:C}" });
                //TotalSummary.Add(new Summary() { Type = SummaryItemType.Sum, FieldName = "Discount", DisplayFormat = " {0:C}" });
                TotalSummary.Add(new Summary() { Type = SummaryItemType.Sum, FieldName = "InvoiceAmount", DisplayFormat = " {0:C}" });
                TotalSummary.Add(new Summary() { Type = SummaryItemType.Count, FieldName = "Code", DisplayFormat = "Total: {0}" });
                OfferOptions = CrmStartUp.GetAllOfferOptions();

                for (int i = 0; i < OfferOptions.Count; i++)
                {
                    if (!Dttable.Columns.Contains(OfferOptions[i].Name))
                    {
                        Columns.Add(new Emdep.Geos.UI.Helper.Column() { FieldName = OfferOptions[i].Name.ToString(), HeaderText = OfferOptions[i].Name.ToString(), Settings = SettingsType.Array, AllowCellMerge = false, Width = 45, AllowEditing = false, Visible = false, IsVertical = true, FixedWidth = true });
                        TotalSummary.Add(new Summary() { Type = SummaryItemType.Sum, FieldName = OfferOptions[i].Name.ToString(), DisplayFormat = " {0}" });
                        Dttable.Columns.Add(OfferOptions[i].Name.ToString(), typeof(string));
                    }
                }

                //Group Summery
                GroupSummary = new ObservableCollection<Summary>();
                GroupSummary.Add(new Summary() { Type = SummaryItemType.Sum, FieldName = "Amount", DisplayFormat = "Amount: {0:C}" });
                //TotalSummary.Add(new Summary() { Type = SummaryItemType.Sum, FieldName = "Discount", DisplayFormat = " {0:C}" });
                GroupSummary.Add(new Summary() { Type = SummaryItemType.Sum, FieldName = "InvoiceAmount", DisplayFormat = "Invoice Amount: {0:C}" });
                GroupSummary.Add(new Summary() { Type = SummaryItemType.Count, FieldName = "Code", DisplayFormat = "Total: {0}" });

                //added site column in last on the grid.
                Columns.Add(new Emdep.Geos.UI.Helper.Column() { FieldName = "Site", HeaderText = "Site", Settings = SettingsType.Fixed, AllowCellMerge = false, Width = 70, AllowEditing = false, Visible = true, IsVertical = false, FixedWidth = true });
                Columns.Add(new Emdep.Geos.UI.Helper.Column() { FieldName = "CountryImageBytes", HeaderText = "CountryImageBytes", Settings = SettingsType.Hidden, AllowCellMerge = false, Width = 10, AllowEditing = false, Visible = false, IsVertical = false, FixedWidth = true });//[GEOS2-4322][17.04.2023][pmisal]
                Dttable.Columns.Add("CountryImageBytes", typeof(string)); //[GEOS2-4322][26.04.2023][pmisal]
                GeosApplication.Instance.Logger.Log("Method Fillgrid() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Fillgrid() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Fillgrid() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Fillgrid() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// [001][skale][20/11/2019][GEOS2-1806]- Add new field "Offer Owner" and "Offered To" in Opportunities
        /// [002][cpatil][12/10/2021][GEOS2-3157]- Improvement in the discount option. (Set the discount null by default)
        /// </summary>
        public void FillDataTable()
        {
            GeosApplication.Instance.Logger.Log("Method FillDataTable ...", category: Category.Info, priority: Priority.Low);
            Dttable.Rows.Clear();

            DttableCopy = Dttable.Copy();

            for (int i = 0; i < OrderGridList.Count; i++)
            {
                var converter = new System.Windows.Media.BrushConverter();
                System.Windows.Media.Brush brush = (System.Windows.Media.Brush)converter.ConvertFromString(OrderGridList[i].HtmlColor.ToString());
                OrderGridList[i].HtmlBrushColor = brush;

                DataRow dr = DttableCopy.NewRow();
                dr["IsSaleOwnerNull"] = false;
                dr["SalesOwner"] = string.Empty;
                dr["BusinessUnit"] = string.Empty;
                dr["Source"] = string.Empty;
                dr["Project"] = string.Empty;
                dr["OEM"] = string.Empty;
                dr["Category1"] = string.Empty;
                dr["Category2"] = string.Empty;
                dr["IdOffer"] = OrderGridList[i].IdOffer.ToString();
                dr["Code"] = OrderGridList[i].Code.ToString();
                dr["IsChecked"] = false;
                dr["Country"] = OrderGridList[i].Country.ToString();
                dr["Region"] = OrderGridList[i].Region.ToString();
                dr["Group"] = OrderGridList[i].Group.ToString();
                dr["Plant"] = OrderGridList[i].Plant.ToString();
                dr["Description"] = OrderGridList[i].Description.ToString();
                dr["Status"] = OrderGridList[i].Status.ToString();
                dr["HtmlColor"] = OrderGridList[i].HtmlColor.ToString();
                dr["Amount"] = OrderGridList[i].Amount;
                //[002]
                if (OrderGridList[i].DiscountNullable != null)
                    dr["Discount"] = OrderGridList[i].DiscountNullable;
                else
                    dr["Discount"] = DBNull.Value;

                dr["InvoiceAmount"] = OrderGridList[i].InvoiceAmount;

                // [001] added
                if (OrderGridList[i].OfferOwner != null)
                    dr["OfferOwner"] = OrderGridList[i].OfferOwner;
                else
                    dr["OfferOwner"] = DBNull.Value;

                if (OrderGridList[i].OfferedTo != null)
                    dr["OfferedTo"] = OrderGridList[i].OfferedTo;
                else
                    dr["OfferedTo"] = DBNull.Value;


                if (OrderGridList[i].CustomerPOCodes != null)
                    dr["CustomerPOCodes"] = OrderGridList[i].CustomerPOCodes;
                else
                    dr["CustomerPOCodes"] = DBNull.Value;
                if (OrderGridList[i].OfferCloseDate != null)
                {
                    dr["DeliveryDate"] = OrderGridList[i].OfferCloseDate;
                }
                if (OrderGridList[i].PoReceptionDate != null)
                {
                    dr["PODate"] = OrderGridList[i].PoReceptionDate;
                }
                if (OrderGridList[i].ShipmentDate != null)
                {
                    dr["ShippingDate"] = OrderGridList[i].ShipmentDate;
                }

                if (OrderGridList[i].Plant != null)
                {
                    dr["ConnectPlantId"] = OrderGridList[i].ConnectPlantId.ToString();

                    OrderGridList[i].Alias = GeosApplication.Instance.CompanyList.Where(cmplst => cmplst.ConnectPlantId == OrderGridList[i].ConnectPlantId.ToString()).Select(slt => slt.Alias).FirstOrDefault();
                    dr["Site"] = OrderGridList[i].Alias;
                }

                if (OrderGridList[i].IdCarProject != null)
                {
                    if (CarProjectList.Any(cp => cp.IdCarProject == OrderGridList[i].IdCarProject.Value))
                        dr["Project"] = CarProjectList.Where(cp => cp.IdCarProject == OrderGridList[i].IdCarProject.Value).FirstOrDefault().Name;
                }


                if (OrderGridList[i].IdCarOEM != null)
                {
                    if (CarOemList.Any(co => co.IdCarOEM == OrderGridList[i].IdCarOEM.Value))
                        dr["OEM"] = CarOemList.Where(co => co.IdCarOEM == OrderGridList[i].IdCarOEM.Value).FirstOrDefault().Name;
                }


                if (OrderGridList[i].IdSource != null)
                {
                    if (LeadSourceList.Any(ls => ls.IdLookupValue == OrderGridList[i].IdSource.Value))
                        dr["Source"] = LeadSourceList.Where(ls => ls.IdLookupValue == OrderGridList[i].IdSource.Value).FirstOrDefault().Value;

                }


                if (OrderGridList[i].IdBusinessUnit != null)
                {
                    if (BusinessUnitList.Any(bu => bu.IdLookupValue == OrderGridList[i].IdBusinessUnit.Value))
                        dr["BusinessUnit"] = BusinessUnitList.Where(bu => bu.IdLookupValue == OrderGridList[i].IdBusinessUnit.Value).FirstOrDefault().Value;
                }


                if (OrderGridList[i].RfqReceptionDate != null)
                {
                    if (Convert.ToDateTime(OrderGridList[i].RfqReceptionDate) == DateTime.MinValue)
                        dr["RFQReceptionDate"] = DBNull.Value;
                    else
                        dr["RFQReceptionDate"] = OrderGridList[i].RfqReceptionDate;
                }
                else
                {
                    dr["RFQReceptionDate"] = DBNull.Value;
                }

                if (OrderGridList[i].QuoteSentDate != null)
                {
                    if (Convert.ToDateTime(OrderGridList[i].QuoteSentDate) == DateTime.MinValue)
                        dr["QuoteSentDate"] = DBNull.Value;
                    else
                        dr["QuoteSentDate"] = OrderGridList[i].QuoteSentDate;
                }
                else
                {
                    dr["QuoteSentDate"] = DBNull.Value;
                }

                // *Below code for fill SalesOwner --Start
                #region Previous Code
                //if (OrderGridList[i].IdSalesOwner != null)
                //    {
                //        dr["SalesOwner"] = GeosApplication.Instance.PeopleList.Where(x => x.IdPerson == OrderGridList[i].IdSalesOwner).Select(slt => slt.FullName).FirstOrDefault();//OrderList[i].SalesOwner.FullName;
                //        dr["IsSaleOwnerNull"] = false;
                //    }
                //    else if (OrderGridList[i].IdSalesResponsible != null && OrderGridList[i].IdSalesResponsibleAssemblyBU != null)
                //    {
                //        dr["SalesOwner"] = "";
                //        dr["IsSaleOwnerNull"] = false;
                //    }
                //    else if (OrderGridList[i].IdSalesResponsible != null && OrderGridList[i].IdSalesResponsibleAssemblyBU == null)
                //    {
                //        dr["SalesOwner"] = GeosApplication.Instance.PeopleList.Where(x => x.IdPerson == OrderGridList[i].IdSalesResponsible).Select(slt => slt.FullName).FirstOrDefault();
                //        dr["IsSaleOwnerNull"] = true;
                //    }
                //    else if (OrderGridList[i].IdSalesResponsible == null && OrderGridList[i].IdSalesResponsibleAssemblyBU != null)
                //    {
                //        dr["SalesOwner"] = GeosApplication.Instance.PeopleList.Where(x => x.IdPerson == OrderGridList[i].IdSalesResponsibleAssemblyBU).Select(slt => slt.FullName).FirstOrDefault();
                //        dr["IsSaleOwnerNull"] = true;
                //    }
                #endregion

                #region rdixit30.05.2023
                if (OrderGridList[i].IdSalesOwner != null)
                {
                    dr["SalesOwner"] = GeosApplication.Instance.PeopleList.Where(x => x.IdPerson == OrderGridList[i].IdSalesOwner).Select(slt => slt.FullName).FirstOrDefault();
                    dr["IsSaleOwnerNull"] = false;
                }
                else if (OrderGridList[i].SalesOwnerList != null)
                {
                    if (OrderGridList[i].SalesOwnerList.Count == 1)
                    {
                        dr["SalesOwner"] = OrderGridList[i].SalesOwnerList.FirstOrDefault().SalesOwner;
                        dr["IsSaleOwnerNull"] = true;
                    }
                    else
                    {
                        dr["SalesOwner"] = "";
                        dr["IsSaleOwnerNull"] = false;
                    }
                }
                #endregion
                try
                {
                    // *Below code for fill SalesOwner --End
                    // OrderGridList[i].OptionsByOffers = OrderGridList.Where(option => option.IdOffer == OrderList[i].IdOffer && option.IdOfferPlant == Convert.ToInt32(OrderList[i].Site.ConnectPlantId)).ToList();

                    //LeadsList[i].OptionsByOffers = OptionsByOfferLeadsList.Where(oboll => oboll.IdOffer == LeadsList[i].IdOffer && oboll.IdOfferPlant == Convert.ToInt32(LeadsList[i].Site.ConnectPlantId)).ToList();
                    foreach (OptionsByOfferGrid item in OrderGridList[i].OptionsByOffers)
                    {
                        if (item.IdOption.ToString() == "6" ||
                                 item.IdOption.ToString() == "19" ||
                                 item.IdOption.ToString() == "21" ||
                                 item.IdOption.ToString() == "23" ||
                                 item.IdOption.ToString() == "25" ||
                                 item.IdOption.ToString() == "27")
                        {
                            var column = Columns.FirstOrDefault(c => c.FieldName.Trim().ToUpper() == item.OfferOption.ToString().Trim().ToUpper());
                            int indexc = Columns.IndexOf(column);
                            Columns[indexc].Visible = false;
                        }
                        else if (DttableCopy.Columns[item.OfferOption.ToString()].ToString() == item.OfferOption.ToString())
                        {
                            var column = Columns.FirstOrDefault(c => c.FieldName.Trim().ToUpper() == item.OfferOption.ToString().Trim().ToUpper());
                            int indexc = Columns.IndexOf(column);
                            Columns[indexc].Visible = true;


                            dr[item.OfferOption] = item.Quantity;
                        }
                    }
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in FillDataTable() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                }

                if (OrderGridList[i].IdProductCategory > 0)
                {
                    if (OrderGridList[i].ProductCategory.Category != null)
                        dr["Category1"] = OrderGridList[i].ProductCategory.Category.Name;

                    if (OrderGridList[i].ProductCategory.IdParent == 0)
                    {
                        dr["Category1"] = OrderGridList[i].ProductCategory.Name;
                    }
                    else
                    {
                        dr["Category2"] = OrderGridList[i].ProductCategory.Name;
                    }

                }

                //dr["CountryImageBytes"] = Countries.FirstOrDefault(j => j.Name.ToLower() == OrderGridList[i].Country.ToString().ToLower()).CountryIconBytes;
                //DttableCopy.Rows.Add(dr);



                //[pmisal][GEOS_4322][26.04.2023]
                dr["CountryImageBytes"] = "https://api.emdep.com/GEOS/Images?FilePath=/Images/Countries/" + Countries.FirstOrDefault(j => j.Name.ToLower() == OrderGridList[i].Country.ToString().ToLower()).Iso + ".png";
                DttableCopy.Rows.Add(dr);


            }

            Dttable = DttableCopy;
            GeosApplication.Instance.Logger.Log("Method FillDataTable() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method for resize Grid Row on design.
        /// </summary>
        /// <param name="obj"></param>
        private void ViewHideRangeControl(object obj)
        {
            if (IsViewRangeControl)
            {
                GridRowHeight = "0";
                IsViewRangeControl = false;
            }
            else
            {
                GridRowHeight = "100";
                IsViewRangeControl = true;
            }
        }

        private void PrintLeadsGrid(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintLeadsGrid ...", category: Category.Info, priority: Priority.Low);

                IsBusy = true;

                //if (MultipleCellEditHelper.IsValueChanged)
                //{
                //    MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["LeadseditUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                //    if (MessageBoxResult == MessageBoxResult.Yes)
                //    {
                //        UpdateMultipleRowsCommandAction(MultipleCellEditHelper.Viewtableview);
                //    }
                //    MultipleCellEditHelper.IsValueChanged = false;
                //}
                //MultipleCellEditHelper.IsValueChanged = false;

                PrintableControlLink pcl = new PrintableControlLink((TableView)obj);
                pcl.Margins.Bottom = 5;
                pcl.Margins.Top = 5;
                pcl.Margins.Left = 5;
                pcl.Margins.Right = 5;
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["PageHeader"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["PageFooter"];
                pcl.Landscape = true;
                pcl.PaperKind = System.Drawing.Printing.PaperKind.A3;
                pcl.CreateDocument(false);

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                IsBusy = false;
                window.Show();

                GeosApplication.Instance.Logger.Log("Method PrintLeadsGrid() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in PrintLeadsGrid() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ExportLeadsGridButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportLeadsGridButtonCommandAction ...", category: Category.Info, priority: Priority.Low);

                SaveFileDialogService.DefaultExt = "xlsx";
                SaveFileDialogService.DefaultFileName = "Orders";
                SaveFileDialogService.Filter = "Excel Files (.xlsx)|*.xlsx|All Files (*.*)|*.*";
                SaveFileDialogService.FilterIndex = 1;
                DialogResult = SaveFileDialogService.ShowDialog();

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

                    ResultFileName = (SaveFileDialogService.File).DirectoryName + "\\" + (SaveFileDialogService.File).Name;

                    TableView _OrdersTableView = ((TableView)obj);
                    _OrdersTableView.ShowTotalSummary = false;
                    _OrdersTableView.ExportToXlsx(ResultFileName);

                    SpreadsheetControl control = new SpreadsheetControl();
                    control.LoadDocument(ResultFileName);
                    Worksheet worksheet = control.ActiveWorksheet;

                    // Split all merged cells in the worksheet. 
                    foreach (var item in worksheet.Cells.GetMergedRanges())
                    {
                        item.UnMerge();
                    }

                    // Rotate only the OfferOptions.
                    DevExpress.Spreadsheet.ColumnCollection worksheetColumns = worksheet.Columns;
                    int countTechCol = 0;

                    foreach (GridColumn gridColumn in _OrdersTableView.VisibleColumns)
                    {
                        if (OfferOptions.Any(op => op.Name == gridColumn.FieldName))
                        {
                            string text = worksheetColumns[countTechCol].Heading + 1;

                            Cell cell = worksheet.Cells[text];
                            cell.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                            cell.Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                            cell.Alignment.RotationAngle = 90;
                        }

                        countTechCol++;
                    }

                    DevExpress.Spreadsheet.CellRange supplierNamesRange = worksheet.Range["A1:AZ1"];
                    supplierNamesRange.Font.Bold = true;
                    control.SaveDocument();

                    IsBusy = false;
                    _OrdersTableView.ShowTotalSummary = true;
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);
                }

                GeosApplication.Instance.Logger.Log("Method ExportLeadsGridButtonCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in ExportLeadsGridButtonCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //StatusList
        private void FillStatusList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillStatusList ...", category: Category.Info, priority: Priority.Low);
                GeosStatusList = new ObservableCollection<GeosStatus>(CrmStartUp.GetGeosOfferStatus().AsEnumerable());
                for (int i = GeosStatusList.Count - 1; i >= 0; i--)
                {
                    if (GeosStatusList[i].IdOfferStatusType == 4)
                    {
                        GeosStatusList.RemoveAt(i);
                    }
                }

                GeosApplication.Instance.Logger.Log("Method FillStatusList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillStatusList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillStatusList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillStatusList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for fill Car Project list.
        /// </summary>
        private void FillCarProjectList()
        {
            try
            {
                if (GeosApplication.Instance.GeosCarProjectsList == null)
                {
                    GeosApplication.Instance.Logger.Log("Method FillCarProjectList ...", category: Category.Info, priority: Priority.Low);
                    CarProjectList = CrmStartUp.GetCarProject(0);
                    GeosApplication.Instance.GeosCarProjectsList = CarProjectList;
                    GeosApplication.Instance.Logger.Log("Method FillCarProjectList() executed successfully", category: Category.Info, priority: Priority.Low);
                }
                else
                {
                    CarProjectList = GeosApplication.Instance.GeosCarProjectsList;
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillCarProjectList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillCarProjectList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCarProjectList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for fill Car OEM list.
        /// </summary>
        private void FillCarOemList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCarOemList ...", category: Category.Info, priority: Priority.Low);
                CarOemList = CrmStartUp.GetCarOEM();
                CarOemList.Insert(0, new CarOEM() { Name = "---" });
                GeosApplication.Instance.Logger.Log("Method FillCarOemList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillCarOemList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillCarOemList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCarOemList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
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
                BusinessUnitList = new ObservableCollection<LookupValue>(CrmStartUp.GetLookupvaluesWithoutRestrictedBU(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.IdUserPermission).AsEnumerable());
                // BusinessUnitList = new ObservableCollection<LookupValue>(CrmStartUp.GetLookupValues(2).AsEnumerable());
                BusinessUnitList.Insert(0, new LookupValue() { Value = "---", InUse = true });
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

        /// <summary>
        /// Method for Fill Lead Source list.  
        /// </summary>
        private void FillLeadSourceList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillLeadSourceList ...", category: Category.Info, priority: Priority.Low);

                LeadSourceList = new ObservableCollection<LookupValue>(CrmStartUp.GetLookupValues(4).AsEnumerable());
                LeadSourceList.Insert(0, new LookupValue() { Value = "---", InUse = true });

                GeosApplication.Instance.Logger.Log("Method FillLeadSourceList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillLeadSourceList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillLeadSourceList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillLeadSourceList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        /// Method For Multiple Row Edit
        /// [001][SP-67][skale][16-07-2019][GEOS2-1641]fechas vencidas en sistema CRM
        /// [002][GEOS2-3157][cpatil][11-10-2021]Improvement in the discount option. (Set the discount null by default)
        /// [003][GEOS2-3595][cpatil][10-03-2022]Can´t update date
        /// </summary>
        /// <param name="obj"></param>
        public void UpdateMultipleRowsCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method UpdateMultipleRowsCommandAction ...", category: Category.Info, priority: Priority.Low);
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
                        return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }

                view = obj as TableView;
                ObservableCollection<object> selectedRows = (ObservableCollection<object>)view.SelectedRows;
                Offer OfferData = new Offer();
                bool isoffersave = false;
                bool isAllSave = false;
                string offerCode = string.Empty;
                long offerId = 0;
                Int32 ConnectPlantId = 0;
                IList<Offer> FinalOfferList = new List<Offer>();
                byte? cellIdBusinessUnit = null;
                byte? cellIdSource = null;
                int? cellIdOEM = null;
                long? CellIdCarProject = null;
                string discount = null;
                cellIdBusinessUnit = Convert.ToByte(BusinessUnitList.Where(b => b.Value == MultipleCellEditHelper.BusinessUnit).Select(u => u.IdLookupValue).FirstOrDefault());
                cellIdSource = Convert.ToByte(LeadSourceList.Where(b => b.Value == MultipleCellEditHelper.LeadSource).Select(u => u.IdLookupValue).FirstOrDefault());
                cellIdOEM = CarOemList.Where(b => b.Name == MultipleCellEditHelper.OEM).Select(u => u.IdCarOEM).FirstOrDefault();
                CellIdCarProject = CarProjectList.Where(b => b.Name == MultipleCellEditHelper.Project).Select(u => u.IdCarProject).FirstOrDefault();
                discount = MultipleCellEditHelper.LeadDicount;
                DataRow[] foundRow = Dttable.AsEnumerable().Where(row => Convert.ToBoolean(row["IsChecked"]) == true).ToArray();

                foreach (DataRow item in foundRow)
                {
                    ChangeLogsEntry = new List<LogEntryByOffer>();
                    DataRow dr1 = item;
                    //((System.Data.DataRowView)(item)).Row;
                    ConnectPlantId = Convert.ToInt32(dr1["ConnectPlantId"].ToString());
                    offerId = Convert.ToInt64(dr1["Idoffer"].ToString());
                    offerCode = dr1["Code"].ToString();
                    IList<Offer> TempOfferList = new List<Offer>();
                    ActiveSite offerActiveSite = new ActiveSite();
                    if (GeosApplication.Instance.CompanyList.Any(cl => cl.IdCompany.ToString() == ConnectPlantId.ToString()))
                    {
                        Company company = GeosApplication.Instance.CompanyList.Where(cmp => cmp.ConnectPlantId == ConnectPlantId.ToString()).FirstOrDefault();
                        offerActiveSite = new ActiveSite { IdSite = company.IdCompany, SiteAlias = company.Alias, SiteServiceProvider = company.ServiceProviderUrl };
                    }
                    ICrmService CrmStartUpOfferActiveSite = new CrmServiceController(offerActiveSite.SiteServiceProvider);
                    //[001] added Change Method
                    //[002] added Change Method
                    TempOfferList.Add(CrmStartUpOfferActiveSite.GetOfferDetailsById_V2200(offerId, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, offerActiveSite));

                    OfferData = TempOfferList[0];
                    bool IsEditableOffer = false;
                    foreach (var CurrentStatus in GeosStatusList)
                    {
                        if (OfferData.GeosStatus.IdOfferStatusType == CurrentStatus.IdOfferStatusType)
                            IsEditableOffer = true;
                    }
                    if (IsEditableOffer == true)
                    {
                        //[002]
                        if (dr1["Discount"] == DBNull.Value && OfferData.DiscountNullable != null)
                        {
                            string DiscountOld = OfferData.DiscountNullable.ToString();
                            OfferData.DiscountNullable = null;

                            ChangeLogsEntry.Add(new LogEntryByOffer() { IdOffer = OfferData.IdOffer, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewDiscountChanged").ToString(), DiscountOld + "%", "None"), IdLogEntryType = 8 });
                        }
                        else if (dr1["Discount"] != DBNull.Value && OfferData.DiscountNullable == null)//[003]
                        {

                            OfferData.DiscountNullable = Convert.ToSingle(dr1["Discount"]);
                            string DiscountNew = OfferData.DiscountNullable.ToString();
                            ChangeLogsEntry.Add(new LogEntryByOffer() { IdOffer = OfferData.IdOffer, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewDiscountChanged").ToString(), "None", DiscountNew + "%"), IdLogEntryType = 8 });
                        }
                        else if (dr1["Discount"] != DBNull.Value && OfferData.DiscountNullable != (double)dr1["Discount"])//[003]
                        {
                            string DiscountOld = OfferData.DiscountNullable.ToString();
                            OfferData.DiscountNullable = Convert.ToSingle(dr1["Discount"]);
                            string DiscountNew = OfferData.DiscountNullable.ToString();
                            ChangeLogsEntry.Add(new LogEntryByOffer() { IdOffer = OfferData.IdOffer, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewDiscountChanged").ToString(), DiscountOld + "%", DiscountNew + "%"), IdLogEntryType = 8 });
                        }
                        if (cellIdBusinessUnit.HasValue)
                        {
                            Int32? businessUnitIdCurrent = Convert.ToInt32(BusinessUnitList.Where(bus => bus.Value == dr1["BusinessUnit"].ToString()).Select(bunn => bunn.IdLookupValue).FirstOrDefault());
                            if (OfferData.IdBusinessUnitNew == null)
                                OfferData.IdBusinessUnitNew = 0;
                            if (OfferData.IdBusinessUnitNew != businessUnitIdCurrent)
                            {
                                string businessUnitNameOld = string.Empty;
                                string businessUnitNameNew = string.Empty;
                                if (OfferData.IdBusinessUnit != null)
                                {
                                    businessUnitNameOld = BusinessUnitList.Where(bu => bu.IdLookupValue == Convert.ToInt32(OfferData.IdBusinessUnitNew.ToString())).Select(bun => bun.Value).SingleOrDefault();
                                }
                                OfferData.IdBusinessUnitNew = businessUnitIdCurrent;
                                if (OfferData.IdBusinessUnitNew != null)
                                {
                                    businessUnitNameNew = BusinessUnitList.Where(bu => bu.IdLookupValue == Convert.ToInt32(OfferData.IdBusinessUnitNew.ToString())).Select(bun => bun.Value).SingleOrDefault();
                                }
                                ChangeLogsEntry.Add(new LogEntryByOffer() { IdOffer = OfferData.IdOffer, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewBusinessUnitChanged").ToString(), businessUnitNameOld, businessUnitNameNew), IdLogEntryType = 8 });
                            }
                        }

                        if (cellIdSource.HasValue)
                        {
                            Int32? leadSourceIdCurrent = Convert.ToInt32(LeadSourceList.Where(bus => bus.Value == dr1["Source"].ToString()).Select(bunn => bunn.IdLookupValue).FirstOrDefault());
                            if (OfferData.IdSourceNew == null)
                                OfferData.IdSourceNew = 0;
                            if (OfferData.IdSourceNew != leadSourceIdCurrent)
                            {
                                string leadSourceNameOld = string.Empty;
                                string leadSourceNameNew = string.Empty;
                                if (OfferData.IdSource != null)
                                {
                                    leadSourceNameOld = LeadSourceList.Where(ls => ls.IdLookupValue == Convert.ToInt32(OfferData.IdSourceNew.ToString())).Select(bun => bun.Value).SingleOrDefault();
                                }
                                OfferData.IdSourceNew = leadSourceIdCurrent;
                                if (OfferData.IdSourceNew != null)
                                {
                                    leadSourceNameNew = LeadSourceList.Where(ls => ls.IdLookupValue == Convert.ToInt32(OfferData.IdSourceNew.ToString())).Select(bun => bun.Value).SingleOrDefault();
                                }
                                ChangeLogsEntry.Add(new LogEntryByOffer() { IdOffer = OfferData.IdOffer, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewSourceChanged").ToString(), leadSourceNameOld, leadSourceNameNew), IdLogEntryType = 8 });
                            }
                        }

                        if (cellIdOEM.HasValue)
                        {
                            int? CaroemIdCurrent = CarOemList.Where(bus => bus.Name == dr1["OEM"].ToString()).Select(bunn => bunn.IdCarOEM).FirstOrDefault();
                            if (OfferData.IdCarOEM == null)
                                OfferData.IdCarOEM = 0;

                            if (OfferData.IdCarOEM != CaroemIdCurrent)
                            {
                                string CaroemNameOld = string.Empty;
                                string CaroemNameNew = string.Empty;
                                if (OfferData.IdCarOEM != null)
                                {
                                    CaroemNameOld = CarOemList.Where(coem => coem.IdCarOEM == OfferData.IdCarOEM).Select(gcoem => gcoem.Name).SingleOrDefault();
                                }
                                OfferData.IdCarOEM = CaroemIdCurrent;

                                if (OfferData.IdCarOEM != null)
                                {
                                    CaroemNameNew = CarOemList.Where(coem => coem.IdCarOEM == OfferData.IdCarOEM).Select(gcoem => gcoem.Name).SingleOrDefault();
                                }
                                ChangeLogsEntry.Add(new LogEntryByOffer() { IdOffer = OfferData.IdOffer, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewCarsOemChanged").ToString(), CaroemNameOld, CaroemNameNew), IdLogEntryType = 8 });
                            }
                        }
                        if (CellIdCarProject.HasValue)
                        {
                            long? CarProjectIdCurrent = CarProjectList.Where(bus => bus.Name == dr1["Project"].ToString()).Select(bunn => bunn.IdCarProject).FirstOrDefault();
                            if (OfferData.IdCarProject == null)
                                OfferData.IdCarProject = 0;

                            if (OfferData.IdCarProject != CarProjectIdCurrent)
                            {
                                string CarProjectNameOld = string.Empty;
                                string CarProjectNameNew = string.Empty;
                                if (OfferData.IdCarProject != null)
                                {
                                    CarProjectNameOld = CarProjectList.Where(carproj => carproj.IdCarProject == OfferData.IdCarProject).Select(carproject => carproject.Name).SingleOrDefault();
                                    if (CarProjectNameOld == null)
                                        CarProjectNameOld = "---";
                                }

                                OfferData.IdCarProject = CarProjectIdCurrent;

                                if (OfferData.IdCarProject != null)
                                {
                                    CarProjectNameNew = CarProjectList.Where(carproj => carproj.IdCarProject == OfferData.IdCarProject).Select(carproject => carproject.Name).SingleOrDefault();
                                    if (CarProjectNameNew == null)
                                        CarProjectNameNew = "---";
                                }
                                ChangeLogsEntry.Add(new LogEntryByOffer() { IdOffer = OfferData.IdOffer, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewProjectChanged").ToString(), CarProjectNameOld, CarProjectNameNew), IdLogEntryType = 8 });
                            }
                        }


                        if (ChangeLogsEntry != null)
                            OfferData.LogEntryByOffers = ChangeLogsEntry;
                        OfferData.ModifiedIn = DateTime.Now;
                        OfferData.ModifiedBy = GeosApplication.Instance.ActiveUser.IdUser;
                        OfferData.Site.ConnectPlantConstr = GeosApplication.Instance.CompanyList.Where(cmp => cmp.ConnectPlantId == TempOfferList[0].Site.ConnectPlantId).Select(x => x.ConnectPlantConstr).FirstOrDefault();
                        FinalOfferList.Add(OfferData);
                    }
                    dr1["ischecked"] = false;
                }

                foreach (Offer item in FinalOfferList)
                {
                    //[002] added Change Method
                    isoffersave = CrmStartUp.UpdateOrderForParticularColumn_V2200(item);
                    if (isoffersave)
                        isAllSave = true;
                    else
                        isAllSave = false;
                }

                if (isAllSave)
                {
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("OrderMsgUpdateOrderSuccessfull").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                }
                else
                {
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("OrderMsgUpdateOrderFail").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }

                MultipleCellEditHelper.SetIsValueChanged(view, false);
                MultipleCellEditHelper.IsValueChanged = false;
                GeosApplication.Instance.Logger.Log("Method UpdateMultipleRowsCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in UpdateMultipleRowsCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// [001][skale][20/11/2019][GEOS2-1806]- Add new field "Offer Owner" and "Offered To" in Opportunities
        /// </summary>
        /// <param name="e"></param>
        private void CustomShowFilterPopup(FilterPopupEventArgs e)
        {

            try
            {
                GeosApplication.Instance.Logger.Log(" Method CustomShowFilterPopup ...", category: Category.Info, priority: Priority.Low);

                if (e.Column.FieldName != "OfferedTo")
                {
                    return;
                }

                List<object> filterItems = new List<object>();

                if (e.Column.FieldName == "OfferedTo")
                {
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("OfferedTo = ''")
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("OfferedTo <> ''")
                    });

                    foreach (DataRow row in Dttable.Rows)
                    {
                        string OfferedTo = row.Field<string>("OfferedTo");

                        if (OfferedTo == null)
                        {
                            continue;
                        }
                        else if (OfferedTo != null)
                        {

                            if (OfferedTo.Contains("\n"))
                            {
                                string tempOfferedTo = OfferedTo;

                                for (int index = 0; index < tempOfferedTo.Length; index++)
                                {
                                    string offeredTo = tempOfferedTo.Split('\n').First();

                                    if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == offeredTo))
                                    {
                                        CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                        customComboBoxItem.DisplayValue = offeredTo;
                                        customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("OfferedTo Like '%{0}%'", offeredTo));
                                        filterItems.Add(customComboBoxItem);
                                    }
                                    if (tempOfferedTo.Contains("\n"))
                                        tempOfferedTo = tempOfferedTo.Remove(0, offeredTo.Length + 1);
                                    else
                                        break;
                                }
                            }
                            else
                            {
                                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == OfferedTo))
                                {
                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                    customComboBoxItem.DisplayValue = OfferedTo;
                                    customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("OfferedTo Like '%{0}%'", OfferedTo));
                                    filterItems.Add(customComboBoxItem);
                                }
                            }

                        }

                    }
                }
                e.ComboBoxEdit.ItemsSource = filterItems.OrderBy(sort => Convert.ToString(((CustomComboBoxItem)sort).DisplayValue)).ToList();

                GeosApplication.Instance.Logger.Log("Method CustomShowFilterPopup() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CustomShowFilterPopup() method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }


        //[pmisal][GESO2-4322][17.04.2023]
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

        #endregion
    }
}
