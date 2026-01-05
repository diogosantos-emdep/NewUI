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
using System.Globalization;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Emdep.Geos.Modules.Crm.ViewModels
{
    public class TargetAndForecastViewModel : NavigationViewModelBase, INotifyPropertyChanged
    {
        #region Services

        ICrmService crmControl = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //ICrmService crmControl = new CrmServiceController("localhost:6699");
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        protected ISaveFileDialogService SaveFileDialogService { get { return this.GetService<ISaveFileDialogService>(); } }
        #endregion // Services

        #region Declaration
        List<Country> countries;
        DataTable dtTargetForecast;
        DataTable dtTargetForecastCopy;
        public string TargetandForecastGridSettingFilePath = GeosApplication.Instance.UserSettingFolderName + "TargetandForecastGridSetting.Xml";
        bool isTargetandforecastColumnChooserVisible;
        bool isBusy;
        private bool isInit;
        private string myFilterString;
        private ObservableCollection<Emdep.Geos.UI.Helper.Column> columns;
        public ObservableCollection<Summary> totalSummary;



        #endregion // Declaration

        #region Public Properties
        public List<Country> Countries
        {
            get { return countries; }
            set
            {
                countries = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Countries"));
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
        public Int64 StartYear { get; set; }
        public Int64 EndYear { get; set; }
        public virtual string ResultFileName { get; protected set; }
        public virtual bool DialogResult { get; protected set; }

        public string PreviouslySelectedSalesOwners { get; set; }
        //public string PreviouslySelectedPlantOwners { get; set; }

        public List<BandItem> Bands { get; set; }
        public IList<Offer> OfferTargetForecast { get; set; }

        public DataTable DtTargetForecast
        {
            get { return dtTargetForecast; }
            set
            {
                dtTargetForecast = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DtTargetForecast"));
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
        public DataTable DtTargetForecastCopy
        {
            get { return dtTargetForecastCopy; }
            set
            {
                dtTargetForecastCopy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DtTargetForecastCopy"));
            }
        }

        public bool IsTargetandforecastColumnChooserVisible
        {
            get { return isTargetandforecastColumnChooserVisible; }
            set
            {
                isTargetandforecastColumnChooserVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsTargetandforecastColumnChooserVisible"));
            }
        }
        public bool IsInit
        {
            get { return isInit; }
            set { isInit = value; }
        }
        public ObservableCollection<Summary> TotalSummary
        {
            get { return totalSummary; }
            set
            {
                totalSummary = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TotalSummary"));
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


        #endregion  // Properties

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

        #region Commands

        public ICommand PrintButtonCommand { get; set; }
        public ICommand CustomCellAppearanceCommand { get; set; }
        public ICommand CommandGridDoubleClick { get; set; }
        public ICommand RefreshTargetAndForecastViewCommand { get; set; }
        public ICommand SalesOwnerPopupClosedCommand { get; private set; }
        public ICommand PlantOwnerPopupClosedCommand { get; private set; }
        public ICommand ExportButtonCommand { get;set; }
        

        #endregion // Commands

        #region Constructor

        public TargetAndForecastViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor TargetAndForecastViewModel ...", category: Category.Info, priority: Priority.Low);
                IsInit = true;
                GeosApplication.Instance.CurrentCurrencySymbol = GeosApplication.Instance.Currencies.Where(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency"].ToString()).Select(cur => cur.Symbol).SingleOrDefault();
                GeosApplication.SetCurrencySymbol(GeosApplication.Instance.CurrentCurrencySymbol);
                StartYear= GeosApplication.Instance.SelectedyearStarDate.Year;
                EndYear = GeosApplication.Instance.SelectedyearEndDate.Year;
                PrintButtonCommand = new RelayCommand(new Action<object>(PrintGrid));
                //CustomCellAppearanceCommand = new DelegateCommand<RoutedEventArgs>(CustomCellAppearanceGridControl);
                CommandGridDoubleClick = new DelegateCommand<RowDoubleClickEventArgs>(EditTargetAndForecastViewWindowShow);
                RefreshTargetAndForecastViewCommand = new RelayCommand(new Action<object>(RefreshTargetAndForecast));
                SalesOwnerPopupClosedCommand = new DevExpress.Mvvm.DelegateCommand<object>(SalesOwnerPopupClosedCommandAction);
                PlantOwnerPopupClosedCommand = new DevExpress.Mvvm.DelegateCommand<object>(PlantOwnerPopupClosedCommandAction);
                ExportButtonCommand = new RelayCommand(new Action<object>(ExportTargetAndForecastButtonCommandAction));
                //fillCountries();
                TargetAndForecastDetails();
                //IsTargetandforecastColumnChooserVisible = true;
                IsInit = false;
                GeosApplication.Instance.Logger.Log("Constructor TargetAndForecastViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in TargetAndForecastViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            IsInit = false;
        }
        #endregion // Constructor

        #region Methods

        /// <summary>
        /// Method for export to excel
        /// </summary>
        /// <param name="obj"></param>
        public void ExportTargetAndForecastButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor ExportTargetAndForecastButtonCommandAction ...", category: Category.Info, priority: Priority.Low);

                SaveFileDialogService.DefaultExt = "xlsx";
                SaveFileDialogService.DefaultFileName = "Target and Forecast";
                SaveFileDialogService.Filter = "Excel Files (.xlsx)|*.xlsx|All Files (.)|*.*";
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
                    TableView targetAndForecastView = ((TableView)obj);

                    targetAndForecastView.ShowTotalSummary = false;
                    targetAndForecastView.ExportToXlsx(ResultFileName);

                    SpreadsheetControl control = new SpreadsheetControl();
                    control.LoadDocument(ResultFileName);
                    Worksheet worksheet = control.ActiveWorksheet;

                    DevExpress.Spreadsheet.CellRange colRangeRow1 = worksheet.Rows[0];
                    colRangeRow1.Font.Bold = true;
                    control.SaveDocument();
                    IsBusy = false;
                    targetAndForecastView.ShowTotalSummary = true;
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);
                }
                    GeosApplication.Instance.Logger.Log("Constructor ExportTargetAndForecastButtonCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch(Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in ExportTargetAndForecastButtonCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);

            }

        }


        /// <summary>
        /// Method for fill Target And Forecast Details
        /// </summary>
        private void TargetAndForecastDetails()
        {
            if (GeosApplication.Instance.IdUserPermission == 21)
            {
                // shubham[skadam] GEOS2-3158 Improvement in Target & forecast  (better visibility) - 1  01 Sep 2022
                // Called for 1st Time
                TargetAndForecastDetailsByUser();
            }
            else if (GeosApplication.Instance.IdUserPermission == 22)
            {
                // shubham[skadam] GEOS2-3158 Improvement in Target & forecast  (better visibility) - 1  30 Aug 2022
                TargetAndForecastDetailsByPlant();
            }
            else
            {
                // shubham[skadam] GEOS2-3158 Improvement in Target & forecast  (better visibility) - 1  01 Sep 2022
                TargetAndForecastDetailsByActiveUser();
            }
        }

        private void TargetAndForecastDetailsByUser()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor TargetAndForecastDetailsByUser ...", category: Category.Info, priority: Priority.Low);
 
                if (GeosApplication.Instance.SelectedSalesOwnerUsersList != null)
                {
                    List<UserManagerDtl> salesOwners = GeosApplication.Instance.SelectedSalesOwnerUsersList.Cast<UserManagerDtl>().ToList();
                    string salesOwnersIds = string.Join(",", salesOwners.Select(i => i.IdUser));
                    PreviouslySelectedSalesOwners = salesOwnersIds;
                    #region
                    // OfferTargetForecast = crmControl.GetSelectedUsersTargetForecastDate(GeosApplication.Instance.IdCurrencyByRegion, salesOwnersIds, GeosApplication.Instance.CrmOfferYear - 1, GeosApplication.Instance.CrmOfferYear);

                    // shubham[skadam] GEOS2-3158 Improvement in Target & forecast  (better visibility) - 1  30 Aug 2022
                    //OfferTargetForecast = crmControl.GetSelectedUsersTargetForecastDate(GeosApplication.Instance.IdCurrencyByRegion, salesOwnersIds, StartYear, EndYear);
                    //OfferTargetForecast = crmControl.GetSelectedUsersTargetForecastDate_V2301(GeosApplication.Instance.IdCurrencyByRegion, salesOwnersIds, StartYear, EndYear);
                    //OfferTargetForecast = crmControl.GetSelectedUsersTargetForecastDate_V2301(GeosApplication.Instance.IdCurrencyByRegion, salesOwnersIds, StartYear, EndYear);
                    //Service V2301 updated with V2380 by [rdixit][26.04.2023][GEOS2-4324]
                    #endregion
                    OfferTargetForecast = crmControl.GetSelectedUsersTargetForecastDate_V2380(GeosApplication.Instance.IdCurrencyByRegion, salesOwnersIds, StartYear, EndYear);


                }
                else
                {
                    OfferTargetForecast = new List<Offer>();
                }
                AddColumnsToDataTable();
                FillDashboard();
                GeosApplication.Instance.Logger.Log("Constructor TargetAndForecastDetailsByUser() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in TargetAndForecastDetailsByUser() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in TargetAndForecastDetailsByUser() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception)
            {
                GeosApplication.Instance.Logger.Log("Get an error in TargetAndForecastDetailsByUser()", category: Category.Exception, priority: Priority.Low);
            }
        }

        private void TargetAndForecastDetailsByPlant()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor TargetAndForecastDetailsByPlant ...", category: Category.Info, priority: Priority.Low);
               
                if (GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
                {
                    List<Company> plantOwners = GeosApplication.Instance.SelectedPlantOwnerUsersList.Cast<Company>().ToList();
                    string plantOwnersIds = string.Join(",", plantOwners.Select(i => i.ConnectPlantId));
                    #region
                    //DateTime accountingYearFrom = GeosApplication.Instance.SelectedyearStarDate.AddYears(-1);
                    // OfferTargetForecast = crmControl.GetTargetForecastByPlantDate(GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.SelectedyearStarDate.AddYears(-1), GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.IdUserPermission, plantOwnersIds);

                    // shubham[skadam] GEOS2-3158 Improvement in Target & forecast  (better visibility) - 1  30 Aug 2022
                    //OfferTargetForecast = crmControl.GetTargetForecastByPlantDate(GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, GeosApplication.Instance.IdUserPermission, plantOwnersIds);
                    //OfferTargetForecast = crmControl.GetTargetForecastByPlantDate_V2301(GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, GeosApplication.Instance.IdUserPermission, plantOwnersIds);
                    //Service V2301 updated with V2380 by [rdixit][26.04.2023][GEOS2-4324]
                    #endregion
                    OfferTargetForecast = crmControl.GetTargetForecastByPlantDate_V2380(GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, GeosApplication.Instance.IdUserPermission, plantOwnersIds);

                }
                else
                {
                    OfferTargetForecast = new List<Offer>();
                }
                //OfferTargetForecast = crmControl.GetTargetForecast(GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.CrmOfferYear - 1, GeosApplication.Instance.CrmOfferYear, GeosApplication.Instance.IdUserPermission);
                AddColumnsToDataTable();
                FillDashboard();
                GeosApplication.Instance.Logger.Log("Constructor TargetAndForecastDetailsByPlant() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in TargetAndForecastDetailsByPlant() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in TargetAndForecastDetailsByPlant() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in TargetAndForecastDetailsByPlant()", category: Category.Exception, priority: Priority.Low);
            }
        }

        private void TargetAndForecastDetailsByActiveUser()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor TargetAndForecastDetailsByActiveUser ...", category: Category.Info, priority: Priority.Low);
                #region
                //OfferTargetForecast = crmControl.GetTargetForecastNewCurrencyConversion(GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.SelectedyearStarDate.AddYears(-1), GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.IdUserPermission);

                // shubham[skadam] GEOS2-3158 Improvement in Target & forecast  (better visibility) - 1  01 Sep 2022
                //OfferTargetForecast = crmControl.GetTargetForecastNewCurrencyConversion(GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, GeosApplication.Instance.IdUserPermission);
                //OfferTargetForecast = crmControl.GetTargetForecastNewCurrencyConversion_V2301(GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, GeosApplication.Instance.IdUserPermission);
                //Service V2301 updated with V2380 by [rdixit][26.04.2023][GEOS2-4324]
                #endregion
                OfferTargetForecast = crmControl.GetTargetForecastNewCurrencyConversion_V2380(GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, GeosApplication.Instance.IdUserPermission);
                AddColumnsToDataTable();
                FillDashboard();
                GeosApplication.Instance.Logger.Log("Constructor TargetAndForecastDetailsByActiveUser() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in TargetAndForecastDetailsByActiveUser() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in TargetAndForecastDetailsByActiveUser() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in TargetAndForecastDetailsByActiveUser()", category: Category.Exception, priority: Priority.Low);
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
                List<UserManagerDtl> salesOwners = GeosApplication.Instance.SelectedSalesOwnerUsersList.Cast<UserManagerDtl>().ToList();
                string salesOwnersIds = string.Join(",", salesOwners.Select(i => i.IdUser));
                //OfferTargetForecast = crmControl.GetSelectedUsersTargetForecast(GeosApplication.Instance.IdCurrencyByRegion, salesOwnersIds, StartYear, EndYear);
                OfferTargetForecast = crmControl.GetSelectedUsersTargetForecastDate(GeosApplication.Instance.IdCurrencyByRegion, salesOwnersIds, StartYear, EndYear);
                FillDashboard();
            }
            else
            {
                OfferTargetForecast = new List<Offer>();
                DtTargetForecast.Rows.Clear();
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
            if (GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
            {
                TargetAndForecastDetailsByPlant();
                // FillDashboard();
            }
            else
            {
                OfferTargetForecast = new List<Offer>();
                DtTargetForecast.Rows.Clear();
            }
            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

            GeosApplication.Instance.Logger.Log("Method PlantOwnerPopupClosedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method for fill Target And Forecast Details from database.
        /// </summary>
        private void RefreshTargetAndForecast(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method RefreshTargetAndForecast ...", category: Category.Info, priority: Priority.Low);

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
            

            StartYear = GeosApplication.Instance.SelectedyearStarDate.Year;
            EndYear = GeosApplication.Instance.SelectedyearEndDate.Year;
            TargetAndForecastDetails();

            // code for hide column chooser if empty
            //TableView detailView = (TableView)obj;
            //GridControl gridControl = (detailView).Grid;

            //int visibleFalseCoulumn = 0;
            //foreach (GridColumn column in gridControl.Columns)
            //{
            //    DependencyPropertyDescriptor descriptor = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleProperty, typeof(GridColumn));
            //    if (descriptor != null)
            //    {
            //        descriptor.AddValueChanged(column, VisibleChanged);
            //    }
            //    if (column.Visible == false)
            //    {
            //        visibleFalseCoulumn++;
            //    }
            //}
            //if (visibleFalseCoulumn > 0)
            //{
            //    IsTargetandforecastColumnChooserVisible = true;
            //}
            //else
            //{
            //    IsTargetandforecastColumnChooserVisible = false;
            //}
            //detailView.SearchString = null;

            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

            GeosApplication.Instance.Logger.Log("Method RefreshTargetAndForecast() executed successfully", category: Category.Info, priority: Priority.Low);
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

                if (File.Exists(TargetandForecastGridSettingFilePath))
                {
                    ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.RestoreLayoutFromXml(TargetandForecastGridSettingFilePath);
                }
                
                //((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.AddHandler(DXSerializer.AllowPropertyEvent, new AllowPropertyEventHandler(OnAllowProperty));

                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.SaveLayoutToXml(TargetandForecastGridSettingFilePath);

                GridControl gridControl = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;
                foreach (GridColumn column in gridControl.Columns)
                {
                    DependencyPropertyDescriptor descriptor = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleProperty, typeof(GridColumn));
                    if (descriptor != null)
                    {
                        descriptor.AddValueChanged(column, VisibleChanged);
                    }

                    //DependencyPropertyDescriptor descriptorColumnPosition = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleIndexProperty, typeof(GridColumn));
                    //if (descriptorColumnPosition != null)
                    //{
                    //    descriptorColumnPosition.AddValueChanged(column, VisibleIndexChanged);
                    //}
                    if (column.Visible == false)
                    {
                        visibleFalseCoulumn++;
                    }
                }

                if (visibleFalseCoulumn > 0)
                {
                    IsTargetandforecastColumnChooserVisible = true;
                }
                else
                {
                    IsTargetandforecastColumnChooserVisible = false;
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

        //private void OnAllowProperty(object sender, AllowPropertyEventArgs e)
        //{
        //    if (e.DependencyProperty == GridControl.ColumnGeneratorTemplateSelectorProperty)
        //        e.Allow = false;

        //}
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
                    ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(TargetandForecastGridSettingFilePath);
                }
                if (column.Visible == false)
                {
                    IsTargetandforecastColumnChooserVisible = true;
                }

                GeosApplication.Instance.Logger.Log("Method VisibleChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in VisibleChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //void VisibleIndexChanged(object sender, EventArgs args)
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method VisibleIndexChanged ...", category: Category.Info, priority: Priority.Low);

        //        GridColumn column = sender as GridColumn;
        //        ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(TargetandForecastGridSettingFilePath);

        //        GeosApplication.Instance.Logger.Log("Method VisibleIndexChanged() executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in VisibleIndexChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}
        private void AddColumnsToDataTableOldMethod()
        {
            GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTable ...", category: Category.Info, priority: Priority.Low);

            CultureInfo ci = new CultureInfo(Thread.CurrentThread.CurrentCulture.Name);
            Columns = new ObservableCollection<Emdep.Geos.UI.Helper.Column>()
            {
              new Emdep.Geos.UI.Helper.Column() { FieldName="CustomerName",HeaderText="Group", Settings = SettingsType.Default, AllowCellMerge=false, Width=200,AllowEditing=false,IsVertical= false,FixedWidth=true,AllowBestFit=true,Visible=true},
              new Emdep.Geos.UI.Helper.Column() { FieldName="SiteName",HeaderText="Plant", Settings = SettingsType.Default, AllowCellMerge=false, Width=200,AllowEditing=false,IsVertical= false,FixedWidth=true,AllowBestFit=true,Visible=true},
              new Emdep.Geos.UI.Helper.Column() { FieldName="CountryName",HeaderText="Country", Settings = SettingsType.Default, AllowCellMerge=false, Width=200,AllowEditing=false,IsVertical= false,FixedWidth=true,AllowBestFit=true,Visible=true},
              new Emdep.Geos.UI.Helper.Column() { FieldName="ZoneName",HeaderText="Region", Settings = SettingsType.Default, AllowCellMerge=false, Width=150,AllowEditing=false,IsVertical= false,FixedWidth=true,AllowBestFit=true,Visible=true},
              new Emdep.Geos.UI.Helper.Column() { FieldName="IdSite",HeaderText="IdSite", Settings = SettingsType.Hidden, AllowCellMerge=false, Width=150,AllowEditing=false,IsVertical= false,FixedWidth=true,AllowBestFit=true,Visible=true},
              new Emdep.Geos.UI.Helper.Column() { FieldName="IdCurrency",HeaderText="IdCurrency", Settings = SettingsType.Hidden, AllowCellMerge=false, Width=150,AllowEditing=false,IsVertical= false,FixedWidth=true,AllowBestFit=true,Visible=true},

            };

            DtTargetForecast = new DataTable();
            DtTargetForecast.Columns.Add("CustomerName", typeof(string));   //group
            DtTargetForecast.Columns.Add("SiteName", typeof(string));       //plant
            DtTargetForecast.Columns.Add("CountryName", typeof(string));
            DtTargetForecast.Columns.Add("ZoneName", typeof(string));
            DtTargetForecast.Columns.Add("IdSite", typeof(Int32));
            DtTargetForecast.Columns.Add("IdCurrency", typeof(Int16));

            TotalSummary = new ObservableCollection<Summary>();
            TotalSummary.Add(new Summary() { Type = SummaryItemType.Count, FieldName = "CustomerName", DisplayFormat = "Total: {0}" });

            for (Int64 i = EndYear; i >= StartYear; i--)
            {       
                DtTargetForecast.Columns.Add(i.ToString(), typeof(double));
                DtTargetForecast.Columns.Add("TargetWithExchange"+i.ToString(), typeof(double));
                for (int j = 0; j < 1; j++)
                {
                    Columns.Add(new Emdep.Geos.UI.Helper.Column() { FieldName = string.Concat(i), HeaderText = string.Concat("Target" + i), Settings = SettingsType.Amount, AllowCellMerge = false, Width = 200, AllowEditing = false, IsVertical = false, FixedWidth = true, AllowBestFit = true });
                    Columns.Add(new Emdep.Geos.UI.Helper.Column() { FieldName = string.Concat(i), HeaderText = string.Concat("TargetWithExchange" + i), Settings = SettingsType.Hidden, AllowCellMerge = false, Width = 200, AllowEditing = false, IsVertical = false, FixedWidth = true, AllowBestFit = true,Visible=false });
                    TotalSummary.Add(new Summary() { Type = SummaryItemType.Sum, FieldName = i.ToString(), DisplayFormat = " {0:C}" });
                }
            }    
            GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTable() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        #region GEOS2-3158 Improvement in Target & forecast  (better visibility) - 1
        private ObservableCollection<ParentBandItem> bands_FirstLevel = new ObservableCollection<ParentBandItem>();
        public ObservableCollection<ParentBandItem> Bands_FirstLevel
        {
            get { return bands_FirstLevel; }
            set
            {
                bands_FirstLevel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Bands_FirstLevel"));
            }
        }

        private void AddColumnsToDataTable()
        {
            GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTable ...", category: Category.Info, priority: Priority.Low);

            try
            {
                CultureInfo ci = new CultureInfo(Thread.CurrentThread.CurrentCulture.Name);
                TotalSummary = new ObservableCollection<Summary>();
                if (Bands_FirstLevel == null)
                {
                    Bands_FirstLevel = new ObservableCollection<ParentBandItem>();
                }
                else
                {
                    Bands_FirstLevel.Clear();
                }
                ParentBandItem ParentBandItem1 = new ParentBandItem()
                {
                    Header = "",
                    Name = "ParentBandItem1",
                    HeaderToolTip = ""
                };
                Bands_FirstLevel.Add(ParentBandItem1);

                BandItem band1 = new BandItem() { BandHeader = "" };
                band1.Columns = new ObservableCollection<ColumnItem>();
                band1.Columns.Add(new ColumnItem() { ColumnFieldName = "CustomerName", HeaderText = "Group", Width = 200, IsVertical = false, Settings = SettingsType.Default, Visible = true });
                band1.Columns.Add(new ColumnItem() { ColumnFieldName = "SiteName", HeaderText = "Plant", Width = 200, IsVertical = false, Settings = SettingsType.Default, Visible = true });
                band1.Columns.Add(new ColumnItem() { ColumnFieldName = "CountryName", HeaderText = "Country", Width = 200, IsVertical = false, Settings = SettingsType.CountryWithImage, Visible = true });//[rdixit][GEOS2-4258][18.04.2023]
                band1.Columns.Add(new ColumnItem() { ColumnFieldName = "ZoneName", HeaderText = "Region", Width = 200, IsVertical = false, Settings = SettingsType.Default, Visible = true });
                band1.Columns.Add(new ColumnItem() { ColumnFieldName = "IdSite", HeaderText = "IdSite", Width = 200, IsVertical = false, Settings = SettingsType.Hidden, Visible = false });
                band1.Columns.Add(new ColumnItem() { ColumnFieldName = "IdCurrency", HeaderText = "IdCurrency", Width = 200, IsVertical = false, Settings = SettingsType.Hidden, Visible = false });
                /// band1.Columns.Add(new ColumnItem() { ColumnFieldName = "CountryImageBytes", HeaderText = "CountryImageBytes", Settings = SettingsType.Hidden, Width = 10, Visible = false, IsVertical = false });//[GEOS2-4258][18.04.2023][rdixit] 
                band1.Columns.Add(new ColumnItem() { ColumnFieldName = "CountryImageUrl", HeaderText = "CountryImageUrl", Settings = SettingsType.Hidden, Width = 10, Visible = false, IsVertical = false });//[GEOS2-4258][18.04.2023][rdixit] 
                Bands_FirstLevel[0].Bands.Add(band1);
                DtTargetForecast = new DataTable();
                DtTargetForecast.Columns.Add("CustomerName", typeof(string));   //group
                DtTargetForecast.Columns.Add("SiteName", typeof(string));       //plant
                DtTargetForecast.Columns.Add("CountryName", typeof(string));
                DtTargetForecast.Columns.Add("ZoneName", typeof(string));
                DtTargetForecast.Columns.Add("IdSite", typeof(Int32));
                DtTargetForecast.Columns.Add("IdCurrency", typeof(Int16));


                //Add year wise bands Bands_FirstLevel_Year
                List<int> YearsList = new List<int>();

                for (Int64 i = EndYear; i >= StartYear; i--)
                {
                    if (!YearsList.Contains(Convert.ToInt32(i)) && i != 0)
                    {
                        YearsList.Add(Convert.ToInt32(i));
                    }
                }

                foreach (var item in YearsList)
                {
                    ParentBandItem ParentBandItem = new ParentBandItem()
                    {
                        Header = $"{item.ToString()}",
                        Name = $"Name{item.ToString()}",
                        HeaderToolTip = $"{item.ToString()}"
                    };
                    Bands_FirstLevel.Add(ParentBandItem);
                }



                string WMSCurrencyName = GeosApplication.Instance.Currencies.FirstOrDefault(x => x.IdCurrency == GeosApplication.Instance.IdCurrencyByRegion).Name;
                foreach (var item in YearsList)
                {
                    BandItem bandExchangeRate = new BandItem()
                    {
                        BandName = "Band_ExchangeRate",
                        BandHeader = "Exchange Rate"
                    };

                    BandItem bandTotal = new BandItem()
                    {
                        BandName = "Band_Total",
                        BandHeader = "Amount"
                    };
                    bandTotal.Columns = new ObservableCollection<ColumnItem>();
                    bandExchangeRate.Columns = new ObservableCollection<ColumnItem>();
                    string totalSummaryField = "Total" + "-" + item.ToString();
                    if (!DtTargetForecast.Columns.Contains(("Total" + "-" + item).ToString()))
                    {
                        DtTargetForecast.Columns.Add("Target" + item.ToString(), typeof(string)).AllowDBNull = true;
                        bandTotal.Columns.Add(new ColumnItem() { ColumnFieldName = "Target" + item.ToString(), HeaderText = "Target" + item.ToString(), Width = 120, IsVertical = false, Settings = SettingsType.Amount, Visible = false });

                        DtTargetForecast.Columns.Add(totalSummaryField, typeof(double)).AllowDBNull = true;
                        bandTotal.Columns.Add(new ColumnItem() { ColumnFieldName = totalSummaryField, HeaderText = item.ToString() +" " + "(" + WMSCurrencyName + ")", Width = 120, IsVertical = false, Settings = SettingsType.Amount, Visible = true });

                        string totalSummaryFieldConvertedAmount = "Total" + "-" + item.ToString() + "ConvertedAmount";
                        DtTargetForecast.Columns.Add(totalSummaryFieldConvertedAmount, typeof(double)).AllowDBNull = true;

                        DtTargetForecast.Columns.Add(totalSummaryFieldConvertedAmount + "WithSymbol", typeof(string)).AllowDBNull = true;
                       

                        bandTotal.Columns.Add(new ColumnItem()
                        {
                            ColumnFieldName = totalSummaryFieldConvertedAmount + "WithSymbol",
                            HeaderText = item.ToString(),
                            Width = 120,
                            IsVertical = false,
                            Settings = SettingsType.Amount,
                            Visible = true,
                            Tag = totalSummaryFieldConvertedAmount
                        });
                        
                    }

                    if (!DtTargetForecast.Columns.Contains(("ExchangeRateDate" + "-" + item).ToString()))
                    {
                        string ExchangeRateDateField = "ExchangeRateDate" + "-" + item.ToString();
                        DtTargetForecast.Columns.Add(ExchangeRateDateField, typeof(DateTime)).AllowDBNull = true;
                        bandExchangeRate.Columns.Add(new ColumnItem() { ColumnFieldName = ExchangeRateDateField, HeaderText = "Date", Width = 100, IsVertical = false, Settings = SettingsType.CloseDate, Visible = true });
                    }

                    if (!DtTargetForecast.Columns.Contains(("ExchangeRate4D" + "-" + item).ToString()))
                    {
                        string ExchangeRateField = "ExchangeRate4D" + "-" + item.ToString();
                        DtTargetForecast.Columns.Add(ExchangeRateField, typeof(double)).AllowDBNull = true;
                        bandExchangeRate.Columns.Add(new ColumnItem() { ColumnFieldName = ExchangeRateField, HeaderText = "Rate", Width = 80, IsVertical = false, Settings = SettingsType.Amount, Visible = true });
                    }

                    if (bandTotal.Columns.Count > 0)
                    {
                        if (item != 0)
                        {
                            var parentBandItem1 = Bands_FirstLevel.FirstOrDefault(
                            x => (x.Header.ToString() == item.ToString()));

                            if (parentBandItem1 != null)
                            {
                                parentBandItem1.Bands.Add(bandTotal);
                            }
                        }
                    }

                    if (bandExchangeRate.Columns.Count > 0)
                    {
                        if (item != 0)
                        {
                            var parentBandItem1 = Bands_FirstLevel.First(
                            x => (x.Header.ToString() == item.ToString()));

                            if (parentBandItem1 != null)
                            {
                                parentBandItem1.Bands.Add(bandExchangeRate);
                            }
                        }
                    }
                    TotalSummary.Add(new Summary() { Type = SummaryItemType.Sum, FieldName = totalSummaryField, DisplayFormat = " {0:C}" });
                }
                //TotalSummary = new ObservableCollection<Summary>();
                //DtTargetForecast.Columns.Add("CountryImageBytes", typeof(byte[])); //[GEOS2-4258][18.04.2023][rdixit]
                DtTargetForecast.Columns.Add("CountryImageUrl", typeof(string)); //[GEOS2-4258][26.04.2023][rdixit]
                TotalSummary.Add(new Summary() { Type = SummaryItemType.Count, FieldName = "CustomerName", DisplayFormat = "Total: {0}" });
                #region OldCoode
                //Columns = new ObservableCollection<Emdep.Geos.UI.Helper.Column>()
                //{
                //  new Emdep.Geos.UI.Helper.Column() { FieldName="CustomerName",HeaderText="Group", Settings = SettingsType.Default, AllowCellMerge=false, Width=200,AllowEditing=false,IsVertical= false,FixedWidth=true,AllowBestFit=true,Visible=true},
                //  new Emdep.Geos.UI.Helper.Column() { FieldName="SiteName",HeaderText="Plant", Settings = SettingsType.Default, AllowCellMerge=false, Width=200,AllowEditing=false,IsVertical= false,FixedWidth=true,AllowBestFit=true,Visible=true},
                //  new Emdep.Geos.UI.Helper.Column() { FieldName="CountryName",HeaderText="Country", Settings = SettingsType.Default, AllowCellMerge=false, Width=200,AllowEditing=false,IsVertical= false,FixedWidth=true,AllowBestFit=true,Visible=true},
                //  new Emdep.Geos.UI.Helper.Column() { FieldName="ZoneName",HeaderText="Region", Settings = SettingsType.Default, AllowCellMerge=false, Width=150,AllowEditing=false,IsVertical= false,FixedWidth=true,AllowBestFit=true,Visible=true},
                //  new Emdep.Geos.UI.Helper.Column() { FieldName="IdSite",HeaderText="IdSite", Settings = SettingsType.Hidden, AllowCellMerge=false, Width=150,AllowEditing=false,IsVertical= false,FixedWidth=true,AllowBestFit=true,Visible=true},
                //  new Emdep.Geos.UI.Helper.Column() { FieldName="IdCurrency",HeaderText="IdCurrency", Settings = SettingsType.Hidden, AllowCellMerge=false, Width=150,AllowEditing=false,IsVertical= false,FixedWidth=true,AllowBestFit=true,Visible=true},

                //};

                //DtTargetForecast = new DataTable();
                //DtTargetForecast.Columns.Add("CustomerName", typeof(string));   //group
                //DtTargetForecast.Columns.Add("SiteName", typeof(string));       //plant
                //DtTargetForecast.Columns.Add("CountryName", typeof(string));
                //DtTargetForecast.Columns.Add("ZoneName", typeof(string));
                //DtTargetForecast.Columns.Add("IdSite", typeof(Int32));
                //DtTargetForecast.Columns.Add("IdCurrency", typeof(Int16));

                //TotalSummary = new ObservableCollection<Summary>();
                //TotalSummary.Add(new Summary() { Type = SummaryItemType.Count, FieldName = "CustomerName", DisplayFormat = "Total: {0}" });

                //for (Int64 i = EndYear; i >= StartYear; i--)
                //{
                //    DtTargetForecast.Columns.Add(i.ToString(), typeof(double));
                //    DtTargetForecast.Columns.Add("TargetWithExchange" + i.ToString(), typeof(double));
                //    for (int j = 0; j < 1; j++)
                //    {
                //        Columns.Add(new Emdep.Geos.UI.Helper.Column() { FieldName = string.Concat(i), HeaderText = string.Concat("Target" + i), Settings = SettingsType.Amount, AllowCellMerge = false, Width = 200, AllowEditing = false, IsVertical = false, FixedWidth = true, AllowBestFit = true });
                //        Columns.Add(new Emdep.Geos.UI.Helper.Column() { FieldName = string.Concat(i), HeaderText = string.Concat("TargetWithExchange" + i), Settings = SettingsType.Hidden, AllowCellMerge = false, Width = 200, AllowEditing = false, IsVertical = false, FixedWidth = true, AllowBestFit = true, Visible = false });
                //        TotalSummary.Add(new Summary() { Type = SummaryItemType.Sum, FieldName = i.ToString(), DisplayFormat = " {0:C}" });
                //    }
                //}
                #endregion
                GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTable() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {

            }
        }
        #endregion
        private void FillDashboard()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillDashboard ...", category: Category.Info, priority: Priority.Low);
                if (OfferTargetForecast != null && OfferTargetForecast.Count > 0)
                {
                    DtTargetForecast.Rows.Clear();
                    DtTargetForecastCopy = DtTargetForecast.Copy();
                    OfferTargetForecast = OfferTargetForecast.OrderBy(x => x.Site.Customers[0].CustomerName).ToList();

                    foreach (Offer item in OfferTargetForecast)
                    {
                        DataRow dr = DtTargetForecastCopy.NewRow();
                        dr["CountryName"] = item.Site.Country.Name.ToString();
                        dr["ZoneName"] = item.Site.Country.Zone.Name.ToString();
                        dr["CustomerName"] = item.Site.Customers[0].CustomerName.ToString();
                        dr["SiteName"] = item.Site.Name.ToString();
                        dr["IdSite"] = item.Site.IdCompany;

                        if (item.Site.SalesTargetBySiteLst != null || item.Site.SalesTargetBySiteLst.Count != 0)
                        {
                            foreach (SalesTargetBySite itemSalesTargetBySite in item.Site.SalesTargetBySiteLst)
                            {
                                dr["IdCurrency"] = itemSalesTargetBySite.Currency.IdCurrency;
                                dr["Total-" + itemSalesTargetBySite.Year.ToString()] = itemSalesTargetBySite.TargetAmountWithExchangeRate;
                                dr["Target" + itemSalesTargetBySite.Year.ToString()] = itemSalesTargetBySite.TargetAmount.ToString("N2");
                                dr["Total-" + itemSalesTargetBySite.Year.ToString() + "ConvertedAmount"] =  itemSalesTargetBySite.TargetAmountWithExchangeRate.ToString("N2");
                                dr["Total-" + itemSalesTargetBySite.Year.ToString() + "ConvertedAmountWithSymbol"] = GeosApplication.Instance.Currencies.FirstOrDefault(
                                                                                                                     i => i.IdCurrency == itemSalesTargetBySite.TargetCurrencyId).Symbol
                                                                                                                     + " " +
                                                                                                                     itemSalesTargetBySite.TargetAmount.ToString("N2");
                                dr["ExchangeRateDate-" + itemSalesTargetBySite.Year.ToString()] = itemSalesTargetBySite.CurrencyConversionDate;
                                dr["ExchangeRate4D-" + itemSalesTargetBySite.Year.ToString()] = itemSalesTargetBySite.CurrencyConversionRate.ToString("N4");
                            }
                        }
                        //dr["CountryImageBytes"] = Countries.FirstOrDefault(j => j.Name.ToLower() == item.Site.Country.Name.ToString().ToLower()).CountryIconBytes; //[rdixit][GEOS2-4258][18.04.2023]
                        dr["CountryImageUrl"] =  item.Site.Country.CountryIconUrl;
                        DtTargetForecastCopy.Rows.Add(dr);
                    }
                    DtTargetForecast = DtTargetForecastCopy;
                }
                GeosApplication.Instance.Logger.Log("Method FillDashboard() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillDashboard() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillDashboard() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillDashboard() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void PrintGrid(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintGrid ...", category: Category.Info, priority: Priority.Low);

                IsBusy = true;
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

                GeosApplication.Instance.Logger.Log("Method PrintGrid() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in PrintGrid() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void EditTargetAndForecastViewWindowShow(RowDoubleClickEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditTargetAndForecastViewWindowShow...", category: Category.Info, priority: Priority.Low);

                if (GeosApplication.Instance.IsPermissionAdminOnly)
                {
                    TableView detailView = e.HitInfo.Column.View as TableView;
                    DataRowView data = (DataRowView)(detailView.DataControl as GridControl).GetRow(e.HitInfo.RowHandle);
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

                    EditTargetAndForecastViewModel editTargetAndForecastViewModel = new EditTargetAndForecastViewModel();
                    EditTargetAndForecastView editTargetAndForecastView = new EditTargetAndForecastView();
                    if (((ISupportParameter)editTargetAndForecastViewModel).Parameter == null)
                        ((ISupportParameter)editTargetAndForecastViewModel).Parameter = detailView;
                    EventHandler handle = delegate { editTargetAndForecastView.Close(); };
                    editTargetAndForecastViewModel.RequestClose += handle;
                    editTargetAndForecastView.DataContext = editTargetAndForecastViewModel;
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    var ownerInfo = (e.OriginalSource as FrameworkElement);
                    editTargetAndForecastView.Owner = Window.GetWindow(ownerInfo);
                    editTargetAndForecastView.ShowDialogWindow();

                    //Code to update Target in the Grid.
                    if (editTargetAndForecastViewModel.IsUpdated)   //True - Admin modified Target
                    {
                        foreach (SalesTargetBySite item in editTargetAndForecastViewModel.TargetAndForecast)
                        {
                            DataRow dataRow = DtTargetForecast.AsEnumerable().FirstOrDefault(row => Convert.ToInt64(row["IdSite"]) == item.IdSite);

                            if (dataRow != null)
                            {
                                //dataRow[string.Concat(item.Year)] = item.TargetAmountWithExchangeRate;
                                //dataRow[string.Concat("TargetWithExchange"+item.Year)] = item.TargetAmount;
                                dataRow["Total-" + item.Year.ToString()] = item.TargetAmountWithExchangeRate;
                                dataRow["Target" + item.Year.ToString()] = item.TargetAmount.ToString("N2");
                                dataRow["Total-" + item.Year.ToString() + "ConvertedAmount"] = item.TargetAmountWithExchangeRate.ToString("N2");
                                dataRow["Total-" + item.Year.ToString() + "ConvertedAmountWithSymbol"] = GeosApplication.Instance.Currencies.FirstOrDefault(
                                                                                                                     i => i.IdCurrency == item.IdCurrency).Symbol
                                                                                                                     + " " +
                                                                                                                     item.TargetAmount.ToString("N2");
                                dataRow["ExchangeRateDate-" + item.Year.ToString()] = item.ExchangeRateDate;
                            }
                        }
                    }
                }

                GeosApplication.Instance.Logger.Log("Method EditTargetAndForecastViewWindowShow() executed successfully...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EditTargetAndForecastViewWindowShow() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //[rdixit][GEOS2-4258][18.04.2023]
        public void fillCountries()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method fillCountries ...", category: Category.Info, priority: Priority.Low);
                Countries = crmControl.GetAllCountriesDetails();
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
        #endregion // Methods
    }

}
