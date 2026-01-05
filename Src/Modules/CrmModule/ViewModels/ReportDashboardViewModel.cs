using DevExpress.Data;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
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
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Emdep.Geos.Modules.Crm.ViewModels
{
    public class ReportDashboardViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Services

        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion  // Services

        #region Declaration

        private ObservableCollection<SalesStatusType> salesStatusTypes;
        DataTable dataTableReport2;
        bool isBusy;

        private bool isInit;
        private string myFilterString;
        private List<string> failedPlants;
        private Boolean isShowFailedPlantWarning;
        private string warningFailedPlants;

        #endregion  // Declaration

        #region  public Properties

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


        public List<Offer> OfferDashboard { get; set; }
        public ObservableCollection<Column> Columns { get; private set; }
        public ObservableCollection<Summary> TotalSummary { get; private set; }
        public List<BandItem> Bands { get; set; }

        public ObservableCollection<SalesStatusType> SalesStatusTypes
        {
            get { return salesStatusTypes; }
            set
            {
                salesStatusTypes = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SalesStatusTypes"));
            }
        }
        public bool IsInit
        {
            get { return isInit; }
            set { isInit = value; }
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

        public DataTable DataTableReport2
        {
            get { return dataTableReport2; }
            set
            {
                dataTableReport2 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DataTableReport2"));
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

        #endregion  //public Properties

        #region  public event

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion   // public event

        #region Commands

        public ICommand CustomCellAppearanceCommand { get; private set; }
        public ICommand PrintButtonCommand { get; set; }
        public ICommand SalesOwnerPopupClosedCommand { get; private set; }
        public ICommand PlantOwnerPopupClosedCommand { get; private set; }
        public ICommand RefreshReportDashboardViewCommand { get; private set; }

        #endregion  // Commands

        #region  Constructor

        public ReportDashboardViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor ReportDashboardViewModel() ...", category: Category.Info, priority: Priority.Low);

                FailedPlants = new List<string>();
                IsShowFailedPlantWarning = false;
                WarningFailedPlants = String.Empty;

                GeosApplication.Instance.CurrentCurrencySymbol = GeosApplication.Instance.Currencies.Where(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency"].ToString()).Select(cur => cur.Symbol).SingleOrDefault();
                GeosApplication.SetCurrencySymbol(GeosApplication.Instance.CurrentCurrencySymbol);

                CustomCellAppearanceCommand = new DevExpress.Mvvm.DelegateCommand<RoutedEventArgs>(CustomCellAppearance);
                PrintButtonCommand = new DelegateCommand<object>(PrintGrid);
                SalesOwnerPopupClosedCommand = new DelegateCommand<object>(SalesOwnerPopupClosedCommandAction);
                PlantOwnerPopupClosedCommand = new DelegateCommand<object>(PlantOwnerPopupClosedCommandAction);
                RefreshReportDashboardViewCommand = new DevExpress.Mvvm.DelegateCommand<object>(RefreshReportDashboard2Details);

                FillCmbSalesOwner();

                GeosApplication.Instance.Logger.Log("Constructor ReportDashboardViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Constructor ReportDashboardViewModel()", category: Category.Info, priority: Priority.Low);
            }

        }

        #endregion  // Constructor

        #region  Methods

        /// <summary>
        /// Method for refresh Report Dashboard2 details.
        /// </summary>
        private void FillCmbSalesOwner()
        {
            GeosApplication.Instance.Logger.Log("Method FillCmbSalesOwner ...", category: Category.Info, priority: Priority.Low);
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

                AddColumnsToDataTable();

                if (GeosApplication.Instance.IdUserPermission == 21)
                {
                   
                    // Called for 1st Time.
                    FillDashboardByUser();

                }
                else if (GeosApplication.Instance.IdUserPermission == 22)
                {
                   
                    FillDashboardByPlant();
                }
                else
                {
                    
                    FillDashboardByActiveUser();
                }

            }
            catch (Exception)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCmbSalesOwner()", category: Category.Info, priority: Priority.Low);
            }

            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
        }

        private void FillDashboardByActiveUser()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillDashboardByActiveUser ...", category: Category.Info, priority: Priority.Low);

                OfferDashboard = new List<Offer>();

                List<Offer> OfferList = new List<Offer>();

                foreach (var companyDetails in GeosApplication.Instance.CompanyList)
                {
                    try
                    {
                        GeosApplication.Instance.SplashScreenMessage = "Connecting to " + companyDetails.Alias;
                        OfferList.Add(CrmStartUp.GetDashboardDetailsCompanyWise(GeosApplication.Instance.IdCurrencyByRegion,
                                                                                GeosApplication.Instance.ActiveUser.IdUser,
                                                                                GeosApplication.Instance.ActiveUser.Company.Country.IdZone,
                                                                                GeosApplication.Instance.CrmOfferYear,
                                                                                companyDetails, GeosApplication.Instance.IdUserPermission));
                    }
                    catch (FaultException<ServiceException> ex)
                    {
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
                        GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", companyDetails.Alias, " Failed ", ex.Detail.ErrorMessage), category: Category.Info, priority: Priority.Low);
                    }
                    catch (ServiceUnexceptedException ex)
                    {
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
                        GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", companyDetails.Alias, " Failed ", ex.Message), category: Category.Info, priority: Priority.Low);
                    }
                    catch (Exception ex)
                    {
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
                        GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", companyDetails.Alias, " Failed ", ex.Message), category: Category.Info, priority: Priority.Low);
                    }
                }
                if (FailedPlants == null || FailedPlants.Count == 0)
                {

                    IsShowFailedPlantWarning = false;
                    WarningFailedPlants = string.Empty;
                }


                GeosApplication.Instance.SplashScreenMessage = "";

                OfferDashboard = CrmStartUp.GetTargetByCustomer(GeosApplication.Instance.IdCurrencyByRegion,
                                                                             GeosApplication.Instance.ActiveUser.IdUser,
                                                                             GeosApplication.Instance.ActiveUser.Company.Country.IdZone,
                                                                             GeosApplication.Instance.CrmOfferYear, GeosApplication.Instance.IdUserPermission);

                FillCompleteListForReportDashboard(OfferList);

                FillDashboard();
                GeosApplication.Instance.Logger.Log("Method FillDashboardByActiveUser() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillDashboardByActiveUser() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillDashboardByActiveUser() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in FillDashboardByActiveUser() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        private void FillDashboardByUser()
        {
            GeosApplication.Instance.Logger.Log("Method FillDashboardByUser ...", category: Category.Info, priority: Priority.Low);
            
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
                var salesOwnersIds = string.Join(",", salesOwners.Select(i => i.IdUser));
                PreviouslySelectedSalesOwners = salesOwnersIds;

                List<Offer> OfferList = new List<Offer>();

                try
                {
                    OfferDashboard = new List<Offer>();

                    foreach (var companyDetails in GeosApplication.Instance.CompanyList)
                    {
                        try
                        {
                            GeosApplication.Instance.SplashScreenMessage = "Connecting to " + companyDetails.Alias;

                            OfferList.Add(CrmStartUp.GetSelectedUsersDashboardDetailsCompanyWise(GeosApplication.Instance.IdCurrencyByRegion,
                                                                                                 GeosApplication.Instance.CrmOfferYear,
                                                                                                 salesOwnersIds,
                                                                                                 companyDetails));
                        }
                        catch (FaultException<ServiceException> ex)
                        {
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
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", companyDetails.Alias, " Failed ", ex.Detail.ErrorMessage), category: Category.Info, priority: Priority.Low);
                        }
                        catch (ServiceUnexceptedException ex)
                        {
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
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", companyDetails.Alias, " Failed ", ex.Message), category: Category.Info, priority: Priority.Low);
                        }
                        catch (Exception ex)
                        {
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
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", companyDetails.Alias, " Failed ", ex.Message), category: Category.Info, priority: Priority.Low);
                        }
                    }
                    if (FailedPlants == null || FailedPlants.Count == 0)
                    {

                        IsShowFailedPlantWarning = false;
                        WarningFailedPlants = string.Empty;
                    }


                    GeosApplication.Instance.SplashScreenMessage = "";

                    OfferDashboard = CrmStartUp.GetSelectedUsersTargetByCustomer(GeosApplication.Instance.IdCurrencyByRegion,
                                                                                 salesOwnersIds,
                                                                                 GeosApplication.Instance.CrmOfferYear);

                    FillCompleteListForReportDashboard(OfferList);

                    FillDashboard();
                }
                catch (FaultException<ServiceException> ex)
                {
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error in FillDashboardByUser() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                    CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                catch (ServiceUnexceptedException ex)
                {
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error in FillDashboardByUser() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                    GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                }
                catch (Exception ex)
                {
                    IsBusy = false;
                    GeosApplication.Instance.Logger.Log("Get an error in FillDashboardByUser() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                }
                GeosApplication.Instance.Logger.Log("Method FillDashboardByUser() executed successfully", category: Category.Info, priority: Priority.Low);
            }

            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
        }


        private void FillDashboardByPlant()
        {
            GeosApplication.Instance.Logger.Log("Method FillDashboardByPlant ...", category: Category.Info, priority: Priority.Low);
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
                OfferDashboard = new List<Offer>();

                List<Offer> OfferList = new List<Offer>();

                if (GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
                {
                    List<Company> plantOwners = GeosApplication.Instance.SelectedPlantOwnerUsersList.Cast<Company>().ToList();
                    string plantOwnersIds = string.Join(",", plantOwners.Select(i => i.ConnectPlantId));
                    PreviouslySelectedPlantOwners = string.Join(",", plantOwners.Select(i => i.ConnectPlantId));

                    foreach (var item in plantOwners)
                    {
                        try
                        {
                            GeosApplication.Instance.SplashScreenMessage = "Connecting to " + item.ShortName;
                            OfferList.Add(CrmStartUp.GetDashboardDetailsCompanyWise(GeosApplication.Instance.IdCurrencyByRegion,
                                                                                    GeosApplication.Instance.ActiveUser.IdUser,
                                                                                    GeosApplication.Instance.ActiveUser.Company.Country.IdZone,
                                                                                    GeosApplication.Instance.CrmOfferYear,
                                                                                    item, GeosApplication.Instance.IdUserPermission));
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
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", item.ShortName, " Failed ", ex.Detail.ErrorMessage), category: Category.Info, priority: Priority.Low);
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
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", item.ShortName, " Failed ", ex.Message), category: Category.Info, priority: Priority.Low);
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
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", item.ShortName, " Failed ", ex.Message), category: Category.Info, priority: Priority.Low);
                        }
                    }
                    if (FailedPlants == null || FailedPlants.Count == 0)
                    {

                        IsShowFailedPlantWarning = false;
                        WarningFailedPlants = string.Empty;
                    }


                    GeosApplication.Instance.SplashScreenMessage = "";

                    //OfferDashboard = CrmStartUp.GetTargetByCustomer(GeosApplication.Instance.IdCurrencyByRegion,
                    //                                                             GeosApplication.Instance.ActiveUser.IdUser,
                    //                                                             GeosApplication.Instance.ActiveUser.Company.Country.IdZone,
                    //                                                             GeosApplication.Instance.CrmOfferYear, GeosApplication.Instance.IdUserPermission);

                    OfferDashboard = CrmStartUp.GetTargetByCustomerByPlant(GeosApplication.Instance.IdCurrencyByRegion,
                                                                            GeosApplication.Instance.ActiveUser.IdUser,
                                                                            GeosApplication.Instance.ActiveUser.Company.Country.IdZone,
                                                                            GeosApplication.Instance.CrmOfferYear, 
                                                                            plantOwnersIds);
                }

                FillCompleteListForReportDashboard(OfferList);

                FillDashboard();
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillDashboardByPlant() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillDashboardByPlant() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in FillDashboardByPlant() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
            GeosApplication.Instance.Logger.Log("Method FillDashboardByPlant() executed successfully", category: Category.Info, priority: Priority.Low);
            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
        }

        private void FillCompleteListForReportDashboard(List<Offer> OfferList)
        {
            foreach (Offer offerPlant in OfferList)
            {
                GeosApplication.Instance.Logger.Log("Method FillCompleteListForReportDashboard ...", category: Category.Info, priority: Priority.Low);
                foreach (Offer offerTarget in OfferDashboard)
                {
                    offerTarget.QualifiedByCustomers.AddRange(offerPlant.QualifiedByCustomers.Where(customerqualified => customerqualified.Offer.IdCustomer == offerTarget.Site.IdCompany));
                    if (offerTarget.QualifiedByCustomers.Count > 0)
                    {
                        offerTarget.TotalQualified = offerTarget.QualifiedByCustomers.Sum(i => i.Value);
                    }

                    offerTarget.RFQByCustomers.AddRange(offerPlant.RFQByCustomers.Where(customerrfq => customerrfq.Offer.IdCustomer == offerTarget.Site.IdCompany));
                    if (offerTarget.RFQByCustomers.Count > 0)
                    {
                        offerTarget.TotalRFQ = offerTarget.RFQByCustomers.Sum(i => i.Value);
                    }

                    offerTarget.ForecastedByCustomers.AddRange(offerPlant.ForecastedByCustomers.Where(customerforecast => customerforecast.Offer.IdCustomer == offerTarget.Site.IdCompany));
                    if (offerTarget.ForecastedByCustomers.Count > 0)
                    {
                        offerTarget.TotalForecasted = offerTarget.ForecastedByCustomers.Sum(i => i.Value);
                    }

                    offerTarget.QuotedByCustomers.AddRange(offerPlant.QuotedByCustomers.Where(customerQuote => customerQuote.Offer.IdCustomer == offerTarget.Site.IdCompany));
                    if (offerTarget.QuotedByCustomers.Count > 0)
                    {
                        offerTarget.TotalQuotated = offerTarget.QuotedByCustomers.Sum(i => i.Value);
                    }

                    offerTarget.SalesByCustomers.AddRange(offerPlant.SalesByCustomers.Where(customerSale => customerSale.Offer.IdCustomer == offerTarget.Site.IdCompany));
                    if (offerTarget.SalesByCustomers.Count > 0)
                    {
                        offerTarget.TotalSales = offerTarget.SalesByCustomers.Sum(i => i.Value);
                    }

                    if (offerTarget.Site != null && offerTarget.Site.SalesTargetBySite != null)
                    {
                        double sales = offerTarget.TotalSales.Value;
                        double targetAmount = Convert.ToDouble(offerTarget.Site.SalesTargetBySite.TargetAmount);
                        if (targetAmount != 0)
                            offerTarget.Site.SalesTargetBySite.TargetPercentage = Math.Round((sales / targetAmount) * 100, 2);
                        else
                            offerTarget.Site.SalesTargetBySite.TargetPercentage = 0;
                    }
                }
                GeosApplication.Instance.Logger.Log("Method FillCompleteListForReportDashboard() executed successfully", category: Category.Info, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for refresh Report Dashboard2 From database by refresh Button.
        /// </summary>
        /// <param name="obj"></param>
        private void RefreshReportDashboard2Details(object obj)
        {
            FailedPlants = new List<string>();
            IsShowFailedPlantWarning = false;
            WarningFailedPlants = String.Empty;
            GeosApplication.Instance.Logger.Log("Method RefreshReportDashboard2Details ...", category: Category.Info, priority: Priority.Low);
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
            GeosApplication.Instance.Logger.Log("Method RefreshReportDashboard2Details() executed successfully", category: Category.Info, priority: Priority.Low);
            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
        }

        private void CustomCellAppearance(RoutedEventArgs obj)
        {
            TableView detailView = ((TableView)((System.Windows.RoutedEventArgs)obj).OriginalSource);
            detailView.BestFitColumns();
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

            if (GeosApplication.Instance.SelectedSalesOwnerUsersList != null)
            {
                FailedPlants = new List<string>();
                IsShowFailedPlantWarning = false;
                WarningFailedPlants = String.Empty;

                FillDashboardByUser();

                if (FailedPlants != null && FailedPlants.Count > 0)
                {
                    IsShowFailedPlantWarning = true;
                    WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                    WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                }
            }
            else
            {
                OfferDashboard = new List<Offer>();
                DataTableReport2.Rows.Clear();
                FailedPlants.Clear();
                IsShowFailedPlantWarning = false;
                WarningFailedPlants = String.Empty;
            }
            GeosApplication.Instance.Logger.Log("Method SalesOwnerPopupClosedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// This method is used to get data by plantowner group
        /// </summary>
        /// <param name="obj"></param>
        private void PlantOwnerPopupClosedCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method PlantOwnerPopupClosedCommandAction ...", category: Category.Info, priority: Priority.Low);
            if (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
            {
                return;
            }

            if (GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
            {
                FailedPlants = new List<string>();
                IsShowFailedPlantWarning = false;
                WarningFailedPlants = String.Empty;

                FillDashboardByPlant();

                if (FailedPlants != null && FailedPlants.Count > 0)
                {
                    IsShowFailedPlantWarning = true;
                    WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                    WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                }
            }
            else
            {
                OfferDashboard = new List<Offer>();
                DataTableReport2.Rows.Clear();
                FailedPlants.Clear();
                IsShowFailedPlantWarning = false;
                WarningFailedPlants = String.Empty;
            }
            GeosApplication.Instance.Logger.Log("Method PlantOwnerPopupClosedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        private void AddColumnsToDataTable()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Report2 AddColumnsToDataTable ...", category: Category.Info, priority: Priority.Low);

                Columns = new ObservableCollection<Column>() {
                    new Column() { FieldName="Group",   HeaderText="Group",   Settings = SettingsType.Default, AllowCellMerge = true, Width = 100,  AllowEditing = false, Visible = true, IsVertical = false },
                    new Column() { FieldName="Plant",   HeaderText="Plant",   Settings = SettingsType.Default, AllowCellMerge = true, Width = 120,  AllowEditing = false, Visible = true, IsVertical = false },
                    new Column() { FieldName="Country", HeaderText="Country", Settings = SettingsType.Default, AllowCellMerge = true, Width = 100,  AllowEditing = false, Visible = true, IsVertical = false },
                    new Column() { FieldName="Region",  HeaderText="Region",  Settings = SettingsType.Default, AllowCellMerge = true, Width = 100,  AllowEditing = false, Visible = true, IsVertical = false },
                };

                DataTableReport2 = new DataTable();
                DataTableReport2.Columns.Add("Group", typeof(string));
                DataTableReport2.Columns.Add("Plant", typeof(string));
                DataTableReport2.Columns.Add("Country", typeof(string));
                DataTableReport2.Columns.Add("Region", typeof(string));

                Bands = new List<BandItem>();
                BandItem band1 = new BandItem() { BandHeader = "" };
                band1.Columns = new ObservableCollection<ColumnItem>();
                band1.Columns.Add(new ColumnItem() { ColumnFieldName = "Group", HeaderText = "Group", Width = 100 });
                band1.Columns.Add(new ColumnItem() { ColumnFieldName = "Plant", HeaderText = "Plant", Width = 120 });
                band1.Columns.Add(new ColumnItem() { ColumnFieldName = "Country", HeaderText = "Country", Width = 100 });
                band1.Columns.Add(new ColumnItem() { ColumnFieldName = "Region", HeaderText = "Region", Width = 100 });
                Bands.Add(band1);

                // Total Summary
                TotalSummary = new ObservableCollection<Summary>();

                // Target
                Columns.Add(new Column() { FieldName = "TARGET_Percentage", HeaderText = "TARGET_Percentage", Settings = SettingsType.Amount, AllowCellMerge = true, Width = 180, AllowEditing = false, Visible = true, IsVertical = false });
                Columns.Add(new Column() { FieldName = "TARGET_Amount", HeaderText = "TARGET_Amount", Settings = SettingsType.Amount, AllowCellMerge = true, Width = 180, AllowEditing = false, Visible = true, IsVertical = false });
                DataTableReport2.Columns.Add("TARGET_Percentage", typeof(double)).DefaultValue = 0;
                DataTableReport2.Columns.Add("TARGET_Amount", typeof(double)).DefaultValue = 0;
                BandItem band2 = new BandItem() { BandHeader = "TARGET" };
                band2.Columns = new ObservableCollection<ColumnItem>();
                band2.Columns.Add(new ColumnItem() { ColumnFieldName = "TARGET_Percentage", HeaderText = "", Width = 150 });
                band2.Columns.Add(new ColumnItem() { ColumnFieldName = "TARGET_Amount", HeaderText = "", Width = 150 });
                Bands.Add(band2);
                TotalSummary.Add(new Summary() { Type = SummaryItemType.Custom, FieldName = string.Concat("TARGET_Percentage"), DisplayFormat = " {0}" });
                TotalSummary.Add(new Summary() { Type = SummaryItemType.Sum, FieldName = string.Concat("TARGET_Amount"), DisplayFormat = " {0:C}" });
                // Status
                SalesStatusTypes = new ObservableCollection<SalesStatusType>(CrmStartUp.GetAllSalesStatusType().AsEnumerable());

                foreach (var item in SalesStatusTypes)
                {
                    BandItem band3 = new BandItem() { BandHeader = item.Name };
                    band3.Columns = new ObservableCollection<ColumnItem>();

                    if (item != null && (item.IdSalesStatusType != 5))
                    {
                        Columns.Add(new Column() { FieldName = string.Concat(item.Name.ToString(), "_Total"), HeaderText = string.Concat(item.Name.ToString(), "_Total"), Settings = SettingsType.Amount, AllowCellMerge = true, Width = 150, AllowEditing = false, Visible = true, IsVertical = false });
                        Columns.Add(new Column() { FieldName = string.Concat(item.Name.ToString(), "_Q1"), HeaderText = string.Concat(item.Name.ToString(), "_Total"), Settings = SettingsType.Amount, AllowCellMerge = true, Width = 120, AllowEditing = false, Visible = true, IsVertical = false });
                        Columns.Add(new Column() { FieldName = string.Concat(item.Name.ToString(), "_Q2"), HeaderText = string.Concat(item.Name.ToString(), "_Total"), Settings = SettingsType.Amount, AllowCellMerge = true, Width = 120, AllowEditing = false, Visible = true, IsVertical = false });
                        Columns.Add(new Column() { FieldName = string.Concat(item.Name.ToString(), "_Q3"), HeaderText = string.Concat(item.Name.ToString(), "_Total"), Settings = SettingsType.Amount, AllowCellMerge = true, Width = 120, AllowEditing = false, Visible = true, IsVertical = false });
                        Columns.Add(new Column() { FieldName = string.Concat(item.Name.ToString(), "_Q4"), HeaderText = string.Concat(item.Name.ToString(), "_Total"), Settings = SettingsType.Amount, AllowCellMerge = true, Width = 120, AllowEditing = false, Visible = true, IsVertical = false });

                        DataTableReport2.Columns.Add(string.Concat(item.Name.ToString(), "_Total"), typeof(double)).DefaultValue = 0;
                        DataTableReport2.Columns.Add(string.Concat(item.Name.ToString(), "_Q1"), typeof(double)).DefaultValue = 0;
                        DataTableReport2.Columns.Add(string.Concat(item.Name.ToString(), "_Q2"), typeof(double)).DefaultValue = 0;
                        DataTableReport2.Columns.Add(string.Concat(item.Name.ToString(), "_Q3"), typeof(double)).DefaultValue = 0;
                        DataTableReport2.Columns.Add(string.Concat(item.Name.ToString(), "_Q4"), typeof(double)).DefaultValue = 0;

                        band3.Columns.Add(new ColumnItem() { ColumnFieldName = string.Concat(item.Name.ToString(), "_Total"), HeaderText = "Total", Width = 150 });
                        band3.Columns.Add(new ColumnItem() { ColumnFieldName = string.Concat(item.Name.ToString(), "_Q1"), HeaderText = "Q1", Width = 120 });
                        band3.Columns.Add(new ColumnItem() { ColumnFieldName = string.Concat(item.Name.ToString(), "_Q2"), HeaderText = "Q2", Width = 120 });
                        band3.Columns.Add(new ColumnItem() { ColumnFieldName = string.Concat(item.Name.ToString(), "_Q3"), HeaderText = "Q3", Width = 120 });
                        band3.Columns.Add(new ColumnItem() { ColumnFieldName = string.Concat(item.Name.ToString(), "_Q4"), HeaderText = "Q4", Width = 120 });

                        TotalSummary.Add(new Summary() { Type = SummaryItemType.Sum, FieldName = string.Concat(item.Name.ToString(), "_Total"), DisplayFormat = " {0:C}" });
                        TotalSummary.Add(new Summary() { Type = SummaryItemType.Sum, FieldName = string.Concat(item.Name.ToString(), "_Q1"), DisplayFormat = " {0:C}" });
                        TotalSummary.Add(new Summary() { Type = SummaryItemType.Sum, FieldName = string.Concat(item.Name.ToString(), "_Q2"), DisplayFormat = " {0:C}" });
                        TotalSummary.Add(new Summary() { Type = SummaryItemType.Sum, FieldName = string.Concat(item.Name.ToString(), "_Q3"), DisplayFormat = " {0:C}" });
                        TotalSummary.Add(new Summary() { Type = SummaryItemType.Sum, FieldName = string.Concat(item.Name.ToString(), "_Q4"), DisplayFormat = " {0:C}" });

                        Bands.Add(band3);
                    }
                }

                GeosApplication.Instance.Logger.Log("Method Report2 AddColumnsToDataTable() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddColumnsToDataTable() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddColumnsToDataTable() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception e)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Report2 AddColumnsToDataTable()", category: Category.Info, priority: Priority.Low);
            }
        }

        private void FillDashboard()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Report2 FillDashboard ...", category: Category.Info, priority: Priority.Low);
                DataTableReport2.Rows.Clear();

                foreach (Offer offer in OfferDashboard)
                {
                    DataRow dataRow = DataTableReport2.NewRow();
                    dataRow["Country"] = offer.Site != null && offer.Site.Country != null ? offer.Site.Country.Name : "";
                    dataRow["Group"] = offer.Site != null && offer.Site.Customers != null && offer.Site.Customers.Count > 0 ? offer.Site.Customers[0].CustomerName : "";
                    dataRow["Plant"] = offer.Site != null ? offer.Site.Name : "";
                    dataRow["Region"] = offer.Site.Country != null && offer.Site.Country.Zone != null ? offer.Site.Country.Zone.Name : "";
                    dataRow["TARGET_Percentage"] = offer.Site != null && offer.Site.SalesTargetBySite != null ? offer.Site.SalesTargetBySite.TargetPercentage : 0;
                    dataRow["TARGET_Amount"] = offer.Site != null && offer.Site.SalesTargetBySite != null ? offer.Site.SalesTargetBySite.TargetAmount : 0;

                    if (offer.ForecastedByCustomers != null && offer.ForecastedByCustomers.Count > 0)   // Forecasted
                    {
                        dataRow[string.Concat("FORECASTED_Total")] = offer.TotalForecasted;
                        foreach (CustomerPurchaseOrder cpOrder in offer.ForecastedByCustomers)
                        {
                            dataRow[string.Concat("FORECASTED_", cpOrder.Quarter)] = Convert.ToDouble(dataRow[string.Concat("FORECASTED_", cpOrder.Quarter)]) + cpOrder.Value;
                        }
                    }

                    if (offer.QuotedByCustomers != null && offer.QuotedByCustomers.Count > 0)   // Quoted
                    {
                        dataRow[string.Concat("QUOTED_Total")] = offer.TotalQuotated;
                        foreach (CustomerPurchaseOrder cpOrder in offer.QuotedByCustomers)
                        {
                            dataRow[string.Concat("QUOTED_", cpOrder.Quarter)] = Convert.ToDouble(dataRow[string.Concat("QUOTED_", cpOrder.Quarter)]) + cpOrder.Value;
                        }
                    }

                    if (offer.SalesByCustomers != null && offer.SalesByCustomers.Count > 0)     // WON
                    {
                        dataRow[string.Concat("WON_Total")] = offer.TotalSales;
                        foreach (CustomerPurchaseOrder cpOrder in offer.SalesByCustomers)
                        {
                            dataRow[string.Concat("WON_", cpOrder.Quarter)] = Convert.ToDouble(dataRow[string.Concat("WON_", cpOrder.Quarter)]) + cpOrder.Value;
                        }
                    }

                    if (offer.QualifiedByCustomers != null && offer.QualifiedByCustomers.Count > 0)     // Qualified
                    {
                        dataRow[string.Concat("Qualified_Total")] = offer.TotalQualified;
                        foreach (CustomerPurchaseOrder cpOrder in offer.QualifiedByCustomers)
                        {
                            dataRow[string.Concat("Qualified_", cpOrder.Quarter)] = Convert.ToDouble(dataRow[string.Concat("Qualified_", cpOrder.Quarter)]) + cpOrder.Value;
                        }
                    }

                    if (offer.RFQByCustomers != null && offer.RFQByCustomers.Count > 0)     // Qualified
                    {
                        dataRow[string.Concat("RFQ_Total")] = offer.TotalRFQ;
                        foreach (CustomerPurchaseOrder cpOrder in offer.RFQByCustomers)
                        {
                            dataRow[string.Concat("RFQ_", cpOrder.Quarter)] = Convert.ToDouble(dataRow[string.Concat("RFQ_", cpOrder.Quarter)]) + cpOrder.Value;
                        }
                    }

                    DataTableReport2.Rows.Add(dataRow);

                    GeosApplication.Instance.Logger.Log("Method Report2 FillDashboard() executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillDashboard() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillDashboard() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception e)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Report2 FillDashboard()", category: Category.Info, priority: Priority.Low);
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
                GeosApplication.Instance.Logger.Log("Get an error in PrintGrid() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        #endregion  // Methods
    }
}

