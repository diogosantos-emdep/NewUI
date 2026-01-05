using DevExpress.Data;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Charts;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.Crm.Views;
using Emdep.Geos.Services.Contracts;
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
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Emdep.Geos.Modules.Crm.ViewModels
{
    public class ItemForecastReportViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Services

        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService crmControl = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion  // Services

        #region Declaration

        private bool isBusy;
        private List<OfferOption> OfferOptions { get; set; }
        private List<Offer> offersItemsForecast;
        private DataTable dtItemsForecast;
        private DataTable graphDataTable;
        private bool isInit;
        private DataTable dt = new DataTable();
        public ObservableCollection<Column> Columns { get; private set; }
        public ObservableCollection<UI.Helper.Summary> TotalSummary { get; private set; }
        private ObservableCollection<SalesStatusType> collectionSalesStatusTypes;

        private string myFilterString;
        List<string> failedPlants;
        Boolean isShowFailedPlantWarning;
        string warningFailedPlants;

        #endregion  // Declaration

        #region Properties

        public string PreviouslySelectedSalesOwners { get; set; }
        public string PreviouslySelectedPlantOwners { get; set; }

        public List<string> FailedPlants
        {
            get { return failedPlants; }
            set { failedPlants = value; }
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
        public string MyFilterString
        {
            get { return myFilterString; }
            set
            {
                myFilterString = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MyFilterString"));
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

        public bool IsInit
        {
            get { return isInit; }
            set { isInit = value; }
        }

        public List<BandItem> Bands { get; set; }

        public ObservableCollection<SalesStatusType> CollectionSalesStatusTypes
        {
            get { return collectionSalesStatusTypes; }
            set
            {
                SetProperty(ref collectionSalesStatusTypes, value, () => CollectionSalesStatusTypes);
            }
        }

        public List<Offer> OffersItemsForecast
        {
            get { return offersItemsForecast; }
            set { offersItemsForecast = value; }
        }

        public DataTable DtItemsForecast
        {
            get { return dtItemsForecast; }
            set
            {
                dtItemsForecast = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DtItemsForecast"));
            }
        }

        public DataTable GraphDataTable
        {
            get { return graphDataTable; }
            set
            {
                graphDataTable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GraphDataTable"));
            }
        }


        #endregion  // Properties

        #region Public event

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

        public ICommand ChartItemsForecastLoadCommand { get; set; }
        public ICommand ChartDashboardSaleCustomDrawCrosshairCommand { get; set; }
        public ICommand PrintButtonCommand { get; set; }
        public ICommand SalesOwnerPopupClosedCommand { get; private set; }
        public ICommand PlantOwnerPopupClosedCommand { get; private set; }
        public ICommand RefreshItemForecastReportViewCommand { get; private set; }

        #endregion  // Public Commands

        #region Constructor

        public ItemForecastReportViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor ItemForecastReportViewModel ...", category: Category.Info, priority: Priority.Low);
                //IsInit = true;
                FailedPlants = new List<string>();
                IsShowFailedPlantWarning = false;
                WarningFailedPlants = String.Empty;

                ChartItemsForecastLoadCommand = new DevExpress.Mvvm.DelegateCommand<object>(ChartItemsForecastAction);
                ChartDashboardSaleCustomDrawCrosshairCommand = new DevExpress.Mvvm.DelegateCommand<object>(ChartDashboardSaleCustomDrawCrosshairCommandAction);
                PrintButtonCommand = new DelegateCommand<object>(PrintGrid);
                SalesOwnerPopupClosedCommand = new DelegateCommand<object>(SalesOwnerPopupClosedCommandAction);
                PlantOwnerPopupClosedCommand = new DelegateCommand<object>(PlantOwnerPopupClosedCommandAction);
                RefreshItemForecastReportViewCommand = new DelegateCommand<object>(RefreshItemForecastReporDetails);

                FillCmbSalesOwner();
                //IsInit = false;
                GeosApplication.Instance.Logger.Log("Constructor ItemForecastReportViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in ItemForecastReportViewModel() Constructor " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in ItemForecastReportViewModel() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ItemForecastReportViewModel() Constructor " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

            //IsInit = false;
        }

        #endregion  // Constructor

        #region Methods

        private void FillCmbSalesOwner()
        {
            /* This Section for Sales Owner */
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCmbSalesOwner ...", category: Category.Info, priority: Priority.Low);

                CollectionSalesStatusTypes = new ObservableCollection<SalesStatusType>(crmControl.GetAllSalesStatusType().AsEnumerable());
                AddColumnsToDataTable();

                GeosApplication.Instance.Logger.Log("Method FillCmbSalesOwner() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillCmbSalesOwner() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillCmbSalesOwner() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in FillCmbSalesOwner()", category: Category.Exception, priority: Priority.Low);
            }

            if (GeosApplication.Instance.IdUserPermission == 21)
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

                // Called for 1st Time.
                // FillItemsForecastGraph();
                FillItemForecastReportDetailsByUser();
                FillItemsForecastGrid();
                FillItemsForecastGraph();
            }
            else if (GeosApplication.Instance.IdUserPermission == 22)
            {
                FillItemForecastReportDetailsByPlant();
                FillItemsForecastGrid();
                FillItemsForecastGraph();
            }
            else
            {
                FillItemForecastReportDetailsByActiveUser();
                FillItemsForecastGrid();
                FillItemsForecastGraph();
            }

            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

            /* This Section for Sales Owner, Period and Remaining Day Header*/
        }

        private void FillItemForecastReportDetailsByActiveUser()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillItemForecastReportDetailsByActiveUser ...", category: Category.Info, priority: Priority.Low);

                OffersItemsForecast = new List<Offer>();

                foreach (var companyDetails in GeosApplication.Instance.CompanyList)
                {
                    try
                    {
                        GeosApplication.Instance.SplashScreenMessage = "Connecting to " + companyDetails.Alias;

                        OffersItemsForecast.AddRange(crmControl.GetOfferQuantitySalesStatusByMonthCompanyWise(GeosApplication.Instance.IdCurrencyByRegion,
                                                                                                              GeosApplication.Instance.ActiveUser.IdUser,
                                                                                                              GeosApplication.Instance.ActiveUser.Company.Country.IdZone,
                                                                                                              GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate,
                                                                                                              companyDetails, GeosApplication.Instance.IdUserPermission));
                    }
                    catch (FaultException<ServiceException> ex)
                    {
                        IsBusy = false;
                        GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", companyDetails.Alias, " Failed");
                        if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                            FailedPlants.Add(companyDetails.ShortName);
                        if (FailedPlants != null && FailedPlants.Count > 0)
                        {
                            IsShowFailedPlantWarning = true;
                            WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                            WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                        }
                        System.Threading.Thread.Sleep(1000);
                        GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", companyDetails.Alias, " Failed ", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                    }
                    catch (ServiceUnexceptedException ex)
                    {
                        IsBusy = false;
                        GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", companyDetails.Alias, " Failed");
                        if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                            FailedPlants.Add(companyDetails.ShortName);
                        if (FailedPlants != null && FailedPlants.Count > 0)
                        {
                            IsShowFailedPlantWarning = true;
                            WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                            WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                        }
                        System.Threading.Thread.Sleep(1000);
                        GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", companyDetails.Alias, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                    }
                    catch (Exception ex)
                    {
                        IsBusy = false;
                        GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", companyDetails.Alias, " Failed");
                        if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                            FailedPlants.Add(companyDetails.ShortName);
                        if (FailedPlants != null && FailedPlants.Count > 0)
                        {
                            IsShowFailedPlantWarning = true;
                            WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                            WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                        }
                        System.Threading.Thread.Sleep(1000);
                        GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", companyDetails.Alias, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                    }
                }

                GeosApplication.Instance.SplashScreenMessage = string.Empty;

                if (FailedPlants == null || FailedPlants.Count == 0)
                {
                    IsShowFailedPlantWarning = false;
                    WarningFailedPlants = string.Empty;
                }

                GeosApplication.Instance.Logger.Log("Method FillItemForecastReportDetailsByActiveUser() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillItemForecastReportDetailsByActiveUser()" + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for refresh temForecast Report details.
        /// </summary>
        private void FillItemForecastReportDetailsByUser()
        {
            GeosApplication.Instance.Logger.Log("Method FillItemForecastReportDetailsByUser ...", category: Category.Info, priority: Priority.Low);
            try
            {
                if (GeosApplication.Instance.SelectedSalesOwnerUsersList != null)
                {
                    OffersItemsForecast = new List<Offer>();
                    List<UserManagerDtl> salesOwners = GeosApplication.Instance.SelectedSalesOwnerUsersList.Cast<UserManagerDtl>().ToList();
                    var salesOwnersIds = string.Join(",", salesOwners.Select(i => i.IdUser));
                    PreviouslySelectedSalesOwners = salesOwnersIds;

                    foreach (var companyDetails in GeosApplication.Instance.CompanyList)
                    {
                        try
                        {
                            GeosApplication.Instance.SplashScreenMessage = "Connecting to " + companyDetails.Alias;

                            OffersItemsForecast.AddRange(crmControl.GetSelectedUsersOfferQuantitySalesStatusByMonthCompanyWise(GeosApplication.Instance.IdCurrencyByRegion,
                                                                                                 GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate,
                                                                                                 salesOwnersIds,
                                                                                                 companyDetails, GeosApplication.Instance.ActiveUser.IdUser));
                        }
                        catch (FaultException<ServiceException> ex)
                        {
                            IsBusy = false;
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", companyDetails.Alias, " Failed");
                            if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                FailedPlants.Add(companyDetails.ShortName);
                            if (FailedPlants != null && FailedPlants.Count > 0)
                            {
                                IsShowFailedPlantWarning = true;
                                WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                                WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                            }
                            System.Threading.Thread.Sleep(1000);
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", companyDetails.Alias, " Failed ", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                        }
                        catch (ServiceUnexceptedException ex)
                        {
                            IsBusy = false;
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", companyDetails.Alias, " Failed");
                            if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                FailedPlants.Add(companyDetails.ShortName);
                            if (FailedPlants != null && FailedPlants.Count > 0)
                            {
                                IsShowFailedPlantWarning = true;
                                WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                                WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                            }
                            System.Threading.Thread.Sleep(1000);
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", companyDetails.Alias, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }
                        catch (Exception ex)
                        {
                            IsBusy = false;
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", companyDetails.Alias, " Failed");
                            if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                FailedPlants.Add(companyDetails.ShortName);
                            if (FailedPlants != null && FailedPlants.Count > 0)
                            {
                                IsShowFailedPlantWarning = true;
                                WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                                WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                            }
                            System.Threading.Thread.Sleep(1000);
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", companyDetails.Alias, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }
                    }

                    GeosApplication.Instance.SplashScreenMessage = string.Empty;

                    if (FailedPlants == null || FailedPlants.Count == 0)
                    {
                        IsShowFailedPlantWarning = false;
                        WarningFailedPlants = string.Empty;
                    }

                    GeosApplication.Instance.Logger.Log("Method FillItemForecastReportDetailsByUser() executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillItemForecastReportDetailsByUser()" + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for refresh temForecast Report details.
        /// </summary>
        private void FillItemForecastReportDetailsByPlant()
        {
            try
            {
                OffersItemsForecast = new List<Offer>();
                if (GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
                {
                    List<Company> plantOwners = GeosApplication.Instance.SelectedPlantOwnerUsersList.Cast<Company>().ToList();
                    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.ShortName));
                    PreviouslySelectedPlantOwners = string.Join(",", plantOwners.Select(i => i.ConnectPlantId));

                    foreach (var item in plantOwners)
                    {
                        try
                        {
                            GeosApplication.Instance.SplashScreenMessage = "Connecting to " + item.ShortName;

                            OffersItemsForecast.AddRange(crmControl.GetOfferQuantitySalesStatusByMonthCompanyWise(GeosApplication.Instance.IdCurrencyByRegion,
                                                                                                                  GeosApplication.Instance.ActiveUser.IdUser,
                                                                                                                  GeosApplication.Instance.ActiveUser.Company.Country.IdZone,
                                                                                                                  GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate,
                                                                                                                  item, GeosApplication.Instance.IdUserPermission));
                        }
                        catch (FaultException<ServiceException> ex)
                        {
                            IsBusy = false;
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
                            IsBusy = false;
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
                            IsBusy = false;
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

                    GeosApplication.Instance.SplashScreenMessage = string.Empty;

                    if (FailedPlants == null || FailedPlants.Count == 0)
                    {
                        IsShowFailedPlantWarning = false;
                        WarningFailedPlants = string.Empty;
                    }
                }

                //OffersItemsForecast = new List<Offer>(crmControl.GetOfferQuantitySalesStatusByMonth(GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.CrmOfferYear));
                //FillItemsForecastGraph();
                //FillItemsForecastGrid();
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillItemForecastReportDetailsByActiveUser() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for refresh temForecast Report From database by refresh Button.
        /// </summary>
        /// <param name="obj"></param>
        private void RefreshItemForecastReporDetails(object obj)
        {
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
            FillCmbSalesOwner();

            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
        }


        /// <summary>
        /// This method is used to get data by salesowner group
        /// </summary>
        /// <param name="obj"></param>
        private void SalesOwnerPopupClosedCommandAction(object obj)
        {
            if (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
            {
                return;
            }

            if (GeosApplication.Instance.SelectedSalesOwnerUsersList != null)
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

                FailedPlants = new List<string>();
                IsShowFailedPlantWarning = false;
                WarningFailedPlants = String.Empty;

                FillItemsForecastGraph();
                FillItemForecastReportDetailsByUser();
                FillItemsForecastGrid();

                if (FailedPlants != null && FailedPlants.Count > 0)
                {
                    IsShowFailedPlantWarning = true;
                    WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                    WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                }

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
            }
            else
            {
                FillItemsForecastGraph();
                OffersItemsForecast = new List<Offer>();
                DtItemsForecast.Rows.Clear();

                FailedPlants.Clear();
                IsShowFailedPlantWarning = false;
                WarningFailedPlants = String.Empty;
            }
        }

        /// <summary>
        /// This method is used to get data by salesowner group
        /// </summary>
        /// <param name="obj"></param>
        private void PlantOwnerPopupClosedCommandAction(object obj)
        {
            if (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
            {
                return;
            }

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
                        win.Topmost = false;
                        return win;
                    }, x =>
                    {
                        return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }

                FailedPlants = new List<string>();
                IsShowFailedPlantWarning = false;
                WarningFailedPlants = String.Empty;

                FillItemsForecastGraph();
                FillItemForecastReportDetailsByPlant();
                FillItemsForecastGrid();
                if (FailedPlants != null && FailedPlants.Count > 0)
                {
                    IsShowFailedPlantWarning = true;
                    WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                    WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                }

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
            }
            else
            {
                FillItemsForecastGraph();
                OffersItemsForecast = new List<Offer>();
                DtItemsForecast.Rows.Clear();

                FailedPlants.Clear();
                IsShowFailedPlantWarning = false;
                WarningFailedPlants = String.Empty;
            }
        }

        private void ChartItemsForecastAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChartDashboardSalebyCustomerAction ...", category: Category.Info, priority: Priority.Low);

                ChartControl chartcontrol = (ChartControl)obj;
                chartcontrol.BeginInit();
                XYDiagram2D diagram = new XYDiagram2D();
                chartcontrol.Diagram = diagram;
                diagram.ActualAxisX.Title = new AxisTitle() { Content = "Months" };
                diagram.ActualAxisY.Title = new AxisTitle() { Content = "Quantity" };

                // Changed list as per requirement
                List<SalesStatusType> CopyListSalesStatusType = new List<SalesStatusType>();
                CopyListSalesStatusType.Add(CollectionSalesStatusTypes.Where(item => item.IdSalesStatusType == 5).SingleOrDefault());
                foreach (var itemsst in CollectionSalesStatusTypes.Where(item => item.IdSalesStatusType != 5).ToList())
                {
                    CopyListSalesStatusType.Add(itemsst);
                }
                CopyListSalesStatusType.Reverse();

                foreach (var item in CopyListSalesStatusType)
                {
                    if (item != null)    // <> LOST
                    {
                        BarSideBySideStackedSeries2D barSideBySideStackedSeries2D = new BarSideBySideStackedSeries2D();
                        barSideBySideStackedSeries2D.ArgumentScaleType = ScaleType.Auto;
                        barSideBySideStackedSeries2D.ValueScaleType = ScaleType.Numerical;
                        barSideBySideStackedSeries2D.DisplayName = item.Name;
                        barSideBySideStackedSeries2D.Brush = (SolidColorBrush)(new BrushConverter().ConvertFrom(item.HtmlColor.ToString()));
                        barSideBySideStackedSeries2D.ArgumentDataMember = "MonthYear";
                        barSideBySideStackedSeries2D.ValueDataMember = item.Name;
                        barSideBySideStackedSeries2D.ShowInLegend = false;
                        barSideBySideStackedSeries2D.Model = new DevExpress.Xpf.Charts.SimpleBar2DModel();

                        barSideBySideStackedSeries2D.AnimationAutoStartMode = AnimationAutoStartMode.SetStartState;
                        Bar2DSlideFromTopAnimation seriesAnimation = new Bar2DSlideFromTopAnimation();
                        seriesAnimation.Duration = new TimeSpan(0, 0, 2);
                        barSideBySideStackedSeries2D.PointAnimation = seriesAnimation;

                        chartcontrol.Diagram.Series.Add(barSideBySideStackedSeries2D);

                        BarSideBySideStackedSeries2D barSideBySideStackedSeries2Dhidden = new BarSideBySideStackedSeries2D();
                        barSideBySideStackedSeries2Dhidden.Brush = (SolidColorBrush)(new BrushConverter().ConvertFrom(item.HtmlColor.ToString()));
                        chartcontrol.Diagram.Series.Insert(0, barSideBySideStackedSeries2Dhidden);
                        barSideBySideStackedSeries2Dhidden.DisplayName = item.Name;
                    }
                }

                chartcontrol.EndInit();

                chartcontrol.AnimationMode = ChartAnimationMode.OnDataChanged;
                chartcontrol.Animate();

                GeosApplication.Instance.Logger.Log("Method ChartDashboardSalebyCustomerAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ChartDashboardSalebyCustomerAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// This method is used to display data on hover of Bar. (order of CrosshairElements is reversed)
        /// </summary>
        /// <param name="obj"></param>
        private void ChartDashboardSaleCustomDrawCrosshairCommandAction(object obj)
        {
            try
            {
                CustomDrawCrosshairEventArgs e = (CustomDrawCrosshairEventArgs)obj;
                foreach (var group in e.CrosshairElementGroups)
                {
                    var reverseList = group.CrosshairElements.ToList();
                    group.CrosshairElements.Clear();
                    foreach (var item in reverseList)
                    {
                        if (item.Series.DisplayName == "TARGET")
                            group.CrosshairElements.Add(item);
                        else
                            group.CrosshairElements.Insert(0, item);
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ChartDashboardSaleCustomDrawCrosshairCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillItemsForecastGraph()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GraphCreateTable ...", category: Category.Info, priority: Priority.Low);

                dt = new DataTable();
                dt.Columns.Add("Month");
                dt.Columns.Add("Year");
                dt.Columns.Add("MonthYear");
                foreach (var item in CollectionSalesStatusTypes)
                {
                    //if (item.IdSalesStatusType != 5)    // <> LOST
                    dt.Columns.Add(item.Name, typeof(double)).DefaultValue = 0;
                }
                dt.Clear();

                if (Convert.ToInt32(GeosApplication.Instance.UserSettings["CustomPeriodOption"]) == 0)
                {
                    int[] icol = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
                    // diff.ToArray();

                    foreach (var mt in icol)
                    {
                        DataRow dr = dt.NewRow();
                        dr[0] = null;
                        //mt.ToString().PadLeft(2, '0');
                        dr[1] = null;
                        dr[2] = mt.ToString().PadLeft(2, '0');
                        int k = 3;
                        foreach (var item in dt.Columns)
                        {
                            if (item.ToString() != "Month" && item.ToString() != "Year" && item.ToString() != "MonthYear")
                            {
                                dr[k] = dr[k] = OffersItemsForecast.Where(m => m.GeosStatus.SalesStatusType.Name == item.ToString() && m.CurrentMonth == mt).Select(mv => mv.OptionsByOffers[0].Quantity != null ? mv.OptionsByOffers[0].Quantity : 0).ToList().Sum();
                                //dr[k] = SalesStatusByMonthList.Where(m => m.Status == item.ToString() && m.CurrentMonth == monthstart && m.CurrentYear == yearstart).Select(mv => mv.Value).ToList().Sum();
                                k++;
                            }
                        }
                        dt.Rows.Add(dr);
                    }
                }
                else
                {
                    var start = GeosApplication.Instance.SelectedyearStarDate;
                    var end = GeosApplication.Instance.SelectedyearEndDate;

                    // set end-date to end of month
                    end = new DateTime(end.Year, end.Month, DateTime.DaysInMonth(end.Year, end.Month));

                    var diff = Enumerable.Range(0, Int32.MaxValue)
                                         .Select(e => start.AddMonths(e))
                                         .TakeWhile(e => e <= end)
                                         .Select(e => e.ToString("MM-yyyy"));

                    int i = dt.Columns.Count;

                    //int[] icol = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
                    //diff.ToArray();

                    foreach (var mty in diff)
                    {
                        DataRow dr = dt.NewRow();
                        String[] substrings = mty.ToString().Split('-');
                        dr[0] = substrings[0];
                        dr[1] = substrings[1];
                        dr[2] = substrings[1] + substrings[0];
                        int monthstart = Convert.ToInt32(substrings[0]);
                        int yearstart = Convert.ToInt32(substrings[1]);
                        int k = 3;

                        foreach (var item in dt.Columns)
                        {
                            if (item.ToString() != "Month" && item.ToString() != "Year" && item.ToString() != "MonthYear")
                            {
                                dr[k] = OffersItemsForecast.Where(m => m.GeosStatus.SalesStatusType.Name == item.ToString() && m.CurrentMonth == monthstart && m.CurrentYear == yearstart).Select(mv => mv.OptionsByOffers[0].Quantity != null ? mv.OptionsByOffers[0].Quantity : 0).ToList().Sum();
                                k++;
                            }
                        }
                        dt.Rows.Add(dr);
                    }
                }
                GeosApplication.Instance.Logger.Log("Method GraphCreateTable() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GraphCreateTable() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            GraphDataTable = dt;
        }

        private void AddColumnsToDataTable()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTable ...", category: Category.Exception, priority: Priority.Low);

                Columns = new ObservableCollection<Column>() {
                    new Column() { FieldName="Month", HeaderText="Month",   Settings = SettingsType.Default, AllowCellMerge = true, Width = 85,  AllowEditing = false, Visible = true, IsVertical = false },
                    new Column() { FieldName="Week",  HeaderText="Week",    Settings = SettingsType.Default, AllowCellMerge = true, Width = 85,  AllowEditing = false, Visible = true, IsVertical = false },
                    new Column() { FieldName="GROUP", HeaderText="Group",   Settings = SettingsType.Default, AllowCellMerge = true, Width = 200, AllowEditing = false, Visible = true, IsVertical = false },
                    new Column() { FieldName="Plant", HeaderText="Plant",   Settings = SettingsType.Default, AllowCellMerge = true, Width = 150, AllowEditing = false, Visible = true, IsVertical = false },
                };

                DtItemsForecast = new DataTable();
                // DtItemsForecast.Columns.Add("Month", typeof(double));
                DtItemsForecast.Columns.Add("Month", typeof(string));
                DtItemsForecast.Columns.Add("Week", typeof(double));
                DtItemsForecast.Columns.Add("GROUP", typeof(string));
                DtItemsForecast.Columns.Add("Plant", typeof(string));

                Bands = new List<BandItem>();

                BandItem band1 = new BandItem() { BandHeader = "" };
                band1.Columns = new List<ColumnItem>();
                // band1.Columns.Add(new ColumnItem() { ColumnFieldName = "Month", HeaderText = "Month", Width = 80, Visible = false });
                band1.Columns.Add(new ColumnItem() { ColumnFieldName = "Week", HeaderText = "Week", Width = 85 });
                band1.Columns.Add(new ColumnItem() { ColumnFieldName = "GROUP", HeaderText = "Group", Width = 150 });
                band1.Columns.Add(new ColumnItem() { ColumnFieldName = "Plant", HeaderText = "Plant", Width = 150 });
                Bands.Add(band1);

                TotalSummary = new ObservableCollection<UI.Helper.Summary>();

                OfferOptions = crmControl.GetAllOfferOptions();
                foreach (OfferOption offerOption in OfferOptions)
                {
                    BandItem band2 = new BandItem() { BandHeader = offerOption.Name };
                    band2.Columns = new List<ColumnItem>();
                    foreach (SalesStatusType salesStatusType in CollectionSalesStatusTypes)
                    {
                        if (salesStatusType != null)
                        {
                            Columns.Add(new Column() { FieldName = string.Concat(offerOption.Name, "_", salesStatusType.Name), HeaderText = string.Concat(offerOption.Name, "_", salesStatusType.Name), Settings = SettingsType.Default, AllowCellMerge = false, Width = 70, AllowEditing = false, Visible = true, IsVertical = false });
                            DtItemsForecast.Columns.Add(offerOption.Name + "_" + salesStatusType.Name, typeof(double));
                            band2.Columns.Add(new ColumnItem() { ColumnFieldName = string.Concat(offerOption.Name, "_", salesStatusType.Name), HeaderText = salesStatusType.Name, Width = 90 });
                            TotalSummary.Add(new UI.Helper.Summary() { Type = SummaryItemType.Sum, FieldName = string.Concat(offerOption.Name, "_", salesStatusType.Name) });
                        }
                    }
                    Bands.Add(band2);
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddColumnsToDataTable() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddColumnsToDataTable() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddColumnsToDataTable() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// This method is used to fill Grid of ItemsForecast.
        /// </summary>
        private void FillItemsForecastGrid()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillItemsForecastGrid ...", category: Category.Info, priority: Priority.Low);
                DtItemsForecast.Rows.Clear();

                if (Convert.ToInt32(GeosApplication.Instance.UserSettings["CustomPeriodOption"]) == 0)
                {
                    //OffersItemsForecast.ToList().ForEach(u => u.CurrentWeek = Convert.ToInt32(u.CurrentYear.ToString() + u.CurrentWeek.ToString().PadLeft(2, '0')));
                    OffersItemsForecast = OffersItemsForecast.OrderBy(c => c.CurrentYear).ThenBy(c => c.CurrentMonth).ThenBy(c => c.CurrentWeek).ToList();
                }
                else
                {
                    OffersItemsForecast = OffersItemsForecast.OrderBy(c => c.CurrentYear).ThenBy(c => c.CurrentMonth).ThenBy(c => c.CurrentWeek).ToList();
                    OffersItemsForecast.ToList().ForEach(u => u.CurrentWeek = Convert.ToInt32(u.CurrentYear.ToString() + u.CurrentWeek.ToString().PadLeft(2, '0')));
                }

                foreach (Offer offer in OffersItemsForecast)
                {
                    //if (offer.GeosStatus.SalesStatusType.Name == "LOST")
                    //    continue;

                    // Graph data.
                    //var updateGraphRow = (from row in GraphDataTable.Rows.OfType<DataRow>()
                    //                      where
                    //                          row["Month"].ToString() == CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(offer.CurrentMonth).ToString()
                    //                      select row).FirstOrDefault();

                    //if (updateGraphRow != null)
                    //{
                    //    // Row is present
                    //    updateGraphRow[offer.GeosStatus.SalesStatusType.Name] = Convert.ToDouble(updateGraphRow[offer.GeosStatus.SalesStatusType.Name]) + Convert.ToDouble(offer.OptionsByOffers[0].Quantity != null ? offer.OptionsByOffers[0].Quantity : 0);
                    //}
                    //else
                    //{
                    //    DataRow newGraphRow = GraphDataTable.NewRow();
                    //    //newGraphRow["Month"] = offer.CurrentMonth;
                    //    newGraphRow["Month"] = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(offer.CurrentMonth).ToString();
                    //    try
                    //    {
                    //        newGraphRow[offer.GeosStatus.SalesStatusType.Name] = offer.OptionsByOffers[0].Quantity != null ? offer.OptionsByOffers[0].Quantity : 0;
                    //    }
                    //    catch
                    //    { }

                    //    GraphDataTable.Rows.Add(newGraphRow);
                    //}

                    if (Convert.ToInt32(GeosApplication.Instance.UserSettings["CustomPeriodOption"]) == 0)
                    {
                        var updateRow = (from row in DtItemsForecast.Rows.OfType<DataRow>()
                                         where
                                             row["Month"].ToString() == offer.CurrentMonth.ToString() &&
                                             row["Week"].ToString() == offer.CurrentWeek.ToString() &&
                                             row["GROUP"].ToString() == offer.Site.Customers[0].CustomerName.ToString() &&
                                             row["Plant"].ToString() == offer.Site.Name.ToString()
                                         select row).FirstOrDefault();

                        if (updateRow != null)
                        {
                            // Row is present
                            updateRow[string.Concat(offer.OptionsByOffers[0].OfferOption.Name, "_", offer.GeosStatus.SalesStatusType.Name)] = offer.OptionsByOffers[0].Quantity != null ? offer.OptionsByOffers[0].Quantity : 0;
                        }
                        else
                        {
                            DataRow newRow = DtItemsForecast.NewRow();
                            //newRow["Month"] = offer.CurrentMonth;
                            newRow["Month"] = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(offer.CurrentMonth).ToString();
                            newRow["Week"] = offer.CurrentWeek.ToString();
                            newRow["GROUP"] = offer.Site.Customers[0].CustomerName;
                            newRow["Plant"] = offer.Site.Name;

                            try
                            {
                                newRow[string.Concat(offer.OptionsByOffers[0].OfferOption.Name, "_", offer.GeosStatus.SalesStatusType.Name)] = offer.OptionsByOffers[0].Quantity != null ? offer.OptionsByOffers[0].Quantity : 0;
                            }
                            catch
                            {
                            }

                            DtItemsForecast.Rows.Add(newRow);
                        }
                    }
                    else
                    {
                        var updateRow = (from row in DtItemsForecast.Rows.OfType<DataRow>()
                                         where
                                             row["Month"].ToString() == offer.CurrentMonth.ToString() &&
                                             row["Week"].ToString() == offer.CurrentWeek.ToString() &&
                                             row["GROUP"].ToString() == offer.Site.Customers[0].CustomerName.ToString() &&
                                             row["Plant"].ToString() == offer.Site.Name.ToString()
                                         select row).FirstOrDefault();

                        if (updateRow != null)
                        {
                            // Row is present
                            updateRow[string.Concat(offer.OptionsByOffers[0].OfferOption.Name, "_", offer.GeosStatus.SalesStatusType.Name)] = offer.OptionsByOffers[0].Quantity != null ? offer.OptionsByOffers[0].Quantity : 0;
                        }
                        else
                        {
                            DataRow newRow = DtItemsForecast.NewRow();
                            //newRow["Month"] = offer.CurrentMonth;
                            newRow["Month"] = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(offer.CurrentMonth).ToString();
                            newRow["Week"] = offer.CurrentWeek.ToString();
                            newRow["GROUP"] = offer.Site.Customers[0].CustomerName;
                            newRow["Plant"] = offer.Site.Name;

                            try
                            {
                                newRow[string.Concat(offer.OptionsByOffers[0].OfferOption.Name, "_", offer.GeosStatus.SalesStatusType.Name)] = offer.OptionsByOffers[0].Quantity != null ? offer.OptionsByOffers[0].Quantity : 0;
                            }
                            catch
                            {
                            }

                            DtItemsForecast.Rows.Add(newRow);
                        }
                    }
                    // Grid data.
                }

                //DataTable temp = GraphDataTable.Copy();
                //GraphDataTable = temp;

                GeosApplication.Instance.Logger.Log("Method FillItemsForecastGrid() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillItemsForecastGrid() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillItemsForecastGrid() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillItemsForecastGrid() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void PrintGrid(object obj)
        {
            try
            {
                IsBusy = true;
                GeosApplication.Instance.Logger.Log("Method PrintLeadsGrid ...", category: Category.Info, priority: Priority.Low);

                PrintableControlLink pcl = new PrintableControlLink((TableView)obj);
                pcl.Margins.Bottom = 5;
                pcl.Margins.Top = 5;
                pcl.Margins.Left = 5;
                pcl.Margins.Right = 5;
                //pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["PageHeader"];
                //pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["PageFooter"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);
                pcl.Landscape = true;
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

        #endregion  // Methods
    }
}
