using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Charts;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.ServiceModel;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.OptimizedClass;
namespace Emdep.Geos.Modules.Crm.Views
{
    public class DashBoardOperationsViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Services

        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
		//ICrmService CrmStartUp = new CrmServiceController("localhost:6699");
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion  //Services

        #region Declaration

        private List<SalesStatusTypeDetail> listAllSalesStatusType;
        //Sales status by month.
        private List<OfferMonthDetail> salesStatusByMonthList;
        private List<SalesUserWon> salesUserWon;
        private DataTable dataTableSalesStatusByMonthGraph;
        // private PlantBusinessUnitSalesQuota plantBusinessUnitSalesQuota;
        //Top 5 Customers.
        private List<CustomerDetail> customersFromPlant;
        private List<Customer> topFiveCustomersList;
        private List<TargetDetail> companies;
        private DataTable dataTableCustomerGraph;
        //Sales users list.
        private ObservableCollection<SalesUserQuota> finalSalesUsersList;

        //Business Unit.
        private ObservableCollection<BusinessUnitDetail> businessUnitList;
        private List<BusinessUnitDetail> businessUnitStatusWise;
        private List<BusinessUnitDetail> businessUnitPlantQuotaAmountBUWise;

        private List<string> failedPlants;
        private Boolean isShowFailedPlantWarning;
        private string warningFailedPlants;

        #endregion  //Declaration

        #region public Properties

        //public string PreviouslySelectedSalesOwners { get; set; }
        public string PreviouslySelectedPlantOwners { get; set; }

        public List<SalesStatusTypeDetail> ListAllSalesStatusType
        {
            get { return listAllSalesStatusType; }
            set { listAllSalesStatusType = value; }
        }

        //Sales status by month
        public List<OfferMonthDetail> SalesStatusByMonthList
        {
            get { return salesStatusByMonthList; }
            set { salesStatusByMonthList = value; }
        }

        //public PlantBusinessUnitSalesQuota PlantBusinessUnitSalesQuota
        //{
        //    get { return plantBusinessUnitSalesQuota; }
        //    set { plantBusinessUnitSalesQuota = value; }
        //}

        public DataTable DataTableSalesStatusByMonthGraph
        {
            get { return dataTableSalesStatusByMonthGraph; }
            set
            {
                dataTableSalesStatusByMonthGraph = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DataTableSalesStatusByMonthGraph"));
            }
        }

        //Customers
        public List<CustomerDetail> CustomersFromPlant
        {
            get { return customersFromPlant; }
            set { customersFromPlant = value; }
        }

        public List<SalesUserWon> SalesUserWon
        {
            get { return salesUserWon; }
            set { salesUserWon = value; }
        }

        public List<Customer> TopCustomersList
        {
            get { return topFiveCustomersList; }
            set { topFiveCustomersList = value; }
        }

        public List<TargetDetail> Companies
        {
            get { return companies; }
            set { companies = value; }
        }

        public DataTable DataTableCustomerGraph
        {
            get { return dataTableCustomerGraph; }
            set
            {
                dataTableCustomerGraph = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DataTableCustomerGraph"));
            }
        }

        //Sales users list.
        public ObservableCollection<SalesUserQuota> FinalSalesUsersList
        {
            get { return finalSalesUsersList; }
            set
            {
                finalSalesUsersList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FinalSalesUsersList"));
            }
        }

        //Business Unit
        public ObservableCollection<BusinessUnitDetail> BusinessUnitList
        {
            get { return businessUnitList; }
            set
            {
                businessUnitList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BusinessUnitList"));
            }
        }

        public List<BusinessUnitDetail> BusinessUnitStatusWise
        {
            get { return businessUnitStatusWise; }
            set { businessUnitStatusWise = value; }
        }

        public List<BusinessUnitDetail> BusinessUnitPlantQuotaAmountBUWise
        {
            get { return businessUnitPlantQuotaAmountBUWise; }
            set { businessUnitPlantQuotaAmountBUWise = value; }
        }

        //Failed plants
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

        #endregion  //Properties

        #region Commands

        public ICommand ChartSalesStatusLoadCommand { get; set; }
        public ICommand ChartSalesStatusCustomDrawCrosshairCommand { get; set; }
        public ICommand ChartCustomersLoadCommand { get; set; }
        public ICommand ChartCustomersCustomDrawCrosshairCommand { get; set; }
        public ICommand PlantOwnerPopupClosedCommand { get; set; }
        public ICommand RefreshDashboardOperationsViewCommand { get; set; }
        public ICommand PieChartSalesTeamLoadCommand { get; set; }
        public ICommand PieChartDrawSeriesPointCommand { get; set; }

        #endregion  //Commands

        #region Constructor

        public DashBoardOperationsViewModel()
        {
            GeosApplication.Instance.Logger.Log("Constructor DashBoardOperationsViewModel ...", category: Category.Info, priority: Priority.Low);

            FailedPlants = new List<string>();
            IsShowFailedPlantWarning = false;
            WarningFailedPlants = String.Empty;

            ChartSalesStatusLoadCommand = new DelegateCommand<RoutedEventArgs>(ChartSalesStatusLoadCommandAction);
            ChartSalesStatusCustomDrawCrosshairCommand = new DelegateCommand<object>(ChartSalesStatusCustomDrawCrosshairCommandAction);
            ChartCustomersLoadCommand = new DelegateCommand<RoutedEventArgs>(ChartCustomersLoadCommandAction);
            ChartCustomersCustomDrawCrosshairCommand = new DelegateCommand<object>(ChartCustomersCustomDrawCrosshairCommandAction);
            PlantOwnerPopupClosedCommand = new DevExpress.Mvvm.DelegateCommand<object>(PlantOwnerPopupClosedCommandAction);
            RefreshDashboardOperationsViewCommand = new DevExpress.Mvvm.DelegateCommand<object>(RefreshDashboardOperationsDetails);
            PieChartSalesTeamLoadCommand = new DelegateCommand<RoutedEventArgs>(PieChartSalesTeamLoadCommandAction);
            PieChartDrawSeriesPointCommand = new DelegateCommand<RoutedEventArgs>(PieChartDrawSeriesPointCommandAction);

            FillDashboardOperationsByPlant();

            GeosApplication.Instance.Logger.Log("Constructor DashBoardOperationsViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        #endregion  //Constructor

        #region public event

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion  //public event

        #region Methods

        /// <summary>
        /// This method is used to fill dashboard operations by plant owners.
        /// </summary>
        private void FillDashboardOperationsByPlant()
        {

            BusinessUnitList = new ObservableCollection<BusinessUnitDetail>(CrmStartUp.GetBusinessUnitsDetails(GeosApplication.Instance.ActiveUser.IdUser) as List<BusinessUnitDetail>);


            FillListsFromServices();
            CreateDataTableSalesStatusByMonth();
            FillDataTableCustomers();
            FillSalesTeam();
            FillBusinessUnits();
        }

        /// <summary>
        /// This method is used to fill all lists from wcf services.
        /// </summary>
        private void FillListsFromServices()
        {
            GeosApplication.Instance.Logger.Log("In method FillListsFromServices() ...", category: Category.Info, priority: Priority.Low);
            if (GeosApplication.Instance.SalesStatusTypeList == null)
            {
                GeosApplication.Instance.SalesStatusTypeList = new List<SalesStatusTypeDetail>(CrmStartUp.GetSalesStatusTypeDetail().ToList());
            }
            ListAllSalesStatusType = GeosApplication.Instance.SalesStatusTypeList;
            // Changed list as per requirement
            List<SalesStatusTypeDetail> CopyListSalesStatusType = new List<SalesStatusTypeDetail>();
            CopyListSalesStatusType.Add(ListAllSalesStatusType.Where(item => item.IdSalesStatusType == 5).SingleOrDefault());

            foreach (var salesStatus in ListAllSalesStatusType.Where(item => item.IdSalesStatusType != 5).ToList())
            {
                CopyListSalesStatusType.Add(salesStatus);
            }
            CopyListSalesStatusType.Reverse();

            ListAllSalesStatusType = CopyListSalesStatusType.ToList();

            SalesStatusByMonthList = new List<OfferMonthDetail>();
            CustomersFromPlant = new List<CustomerDetail>();
            BusinessUnitStatusWise = new List<BusinessUnitDetail>();
            SalesUserWon = new List<SalesUserWon>();

            if (GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
            {
                //For each plants
                List<Company> plantOwners = GeosApplication.Instance.SelectedPlantOwnerUsersList.Cast<Company>().ToList();
                var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.ConnectPlantId));
                PreviouslySelectedPlantOwners = plantOwnersIds;

                //get all Sales User list from main db.
                try
                {
                    FinalSalesUsersList = new ObservableCollection<SalesUserQuota>(CrmStartUp.GetAllSalesTeamUserDetail(GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, plantOwnersIds).ToList());
                }
                catch (FaultException<ServiceException> ex)
                {
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log(string.Concat("Get an error in FillListsFromServices() Method ", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                }
                catch (ServiceUnexceptedException ex)
                {
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log(string.Concat("Get an error in FillListsFromServices() Method ", ex.Message), category: Category.Exception, priority: Priority.Low);
                }
                catch (Exception ex)
                {
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log(string.Concat("Get an error in FillListsFromServices() Method ", ex.Message), category: Category.Exception, priority: Priority.Low);
                }

                bool isTarget = true;
                foreach (Company item in plantOwners)
                {
                    try
                    {
                        GeosApplication.Instance.SplashScreenMessage = "Connecting to " + item.ShortName;
                        //Sales by Month
                        #region ServicesLog
                        //SalesStatusByMonthList.AddRange(CrmStartUp.GetSalesStatusByMonthWithTarget_V2110(GeosApplication.Instance.IdCurrencyByRegion,
                        //                                plantOwnersIds.ToString(),
                        //                                 GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, item,
                        //                                 GeosApplication.Instance.IdUserPermission, GeosApplication.Instance.ActiveUser.IdUser, isTarget).ToList());

                        //[rdixit][GEOS2-3518][30.06.2022][Ignore the “To” date in the “DASHBOARD->Operations Review” section for Opportunities info]
                        //SalesStatusByMonthList.AddRange(CrmStartUp.GetSalesStatusByMonthWithTarget_IgnoreOfferCloseDate(GeosApplication.Instance.IdCurrencyByRegion,
                        //                               plantOwnersIds.ToString(),
                        //                                GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, item,
                        //                                GeosApplication.Instance.IdUserPermission, GeosApplication.Instance.ActiveUser.IdUser, isTarget).ToList());
                        #endregion
                        //[GEOS2-4224][rdixit][28.02.2023]
                        SalesStatusByMonthList.AddRange(CrmStartUp.GetSalesStatusByMonthWithTarget_IgnoreOfferCloseDate_V2360(GeosApplication.Instance.IdCurrencyByRegion,
                                                plantOwnersIds.ToString(), GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, item,
                                                 GeosApplication.Instance.IdUserPermission, GeosApplication.Instance.ActiveUser.IdUser, isTarget).ToList());

                        //Top 5 Customers
                        if (isTarget)
                        {
                            CustomerTargetDetail customerTargetDetail = new CustomerTargetDetail();
                            #region ServicesLog
                            //customerTargetDetail = CrmStartUp.GetTop5CustomersDashboardDetails_V2110(GeosApplication.Instance.IdCurrencyByRegion,
                            //                                                                         GeosApplication.Instance.ActiveUser.IdUser,
                            //                                                                        GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate,
                            //                                                                         item, plantOwnersIds.ToString(), isTarget);

                            //[rdixit][GEOS2-3518][30.06.2022][Ignore the “To” date in the “DASHBOARD->Operations Review” section for Opportunities info]
                            //customerTargetDetail = CrmStartUp.GetTop5CustomersDashboardDetails_IgnoreOfferCloseDate(GeosApplication.Instance.IdCurrencyByRegion,
                            //                                                                        GeosApplication.Instance.ActiveUser.IdUser,
                            //                                                                       GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate,
                            //                                                                        item, plantOwnersIds.ToString(), isTarget);
                            #endregion
                            //[GEOS2-4223][rdixit][28.02.2023]
                            customerTargetDetail = CrmStartUp.GetTop5CustomersDashboardDetails_IgnoreOfferCloseDate_V2360(GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.ActiveUser.IdUser,
                                                   GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, item, plantOwnersIds.ToString(), isTarget);

                            CustomersFromPlant.AddRange(customerTargetDetail.CustomerDetail.ToList());
                            Companies = new List<TargetDetail>();
                            Companies.AddRange(customerTargetDetail.TargetDetail.ToList());

                        }
                        else
                        {
                            #region ServicesLog
                            //CustomersFromPlant.AddRange(CrmStartUp.GetTop5CustomersDashboardDetails_V2110(GeosApplication.Instance.IdCurrencyByRegion,
                            //                                                                         GeosApplication.Instance.ActiveUser.IdUser,
                            //                                                                        GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate,
                            //                                                                         item, plantOwnersIds.ToString(), isTarget).CustomerDetail.ToList());

                            //[rdixit][GEOS2-3518][30.06.2022][Ignore the “To” date in the “DASHBOARD->Operations Review” section for Opportunities info]
                            //CustomersFromPlant.AddRange(CrmStartUp.GetTop5CustomersDashboardDetails_IgnoreOfferCloseDate(GeosApplication.Instance.IdCurrencyByRegion,
                            //                                                                         GeosApplication.Instance.ActiveUser.IdUser,
                            //                                                                        GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate,
                            //                                                                         item, plantOwnersIds.ToString(), isTarget).CustomerDetail.ToList());
                            #endregion
                            //[GEOS2-4223][rdixit][28.02.2023]
                            CustomersFromPlant.AddRange(CrmStartUp.GetTop5CustomersDashboardDetails_IgnoreOfferCloseDate_V2360(GeosApplication.Instance.IdCurrencyByRegion,
                                                                   GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate,
                                                                   item, plantOwnersIds.ToString(), isTarget).CustomerDetail.ToList());
                        }

                        // Fill Sales User Card View
                        #region ServicesLog
                        //SalesUserWon.AddRange(CrmStartUp.GetAllSalesTeamUserWonDetail_V2110(item,
                        //                                                         GeosApplication.Instance.IdCurrencyByRegion,
                        //                                                         string.Join(",", FinalSalesUsersList.Select(x => x.IdSalesUser)),
                        //                                                         GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, GeosApplication.Instance.ActiveUser.IdUser));
                        #endregion
                        //[GEOS2-4224][rdixit][28.02.2023]
                        //SalesUserWon.AddRange(CrmStartUp.GetAllSalesTeamUserWonDetail_V2360(item, GeosApplication.Instance.IdCurrencyByRegion, string.Join(",", FinalSalesUsersList.Select(x => x.IdSalesUser)),
                        //   GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, GeosApplication.Instance.ActiveUser.IdUser));

                        //rajashri [GEOS2-5014][6-12-2023]

                        //SalesUserWon.AddRange(CrmStartUp.GetAllSalesTeamUserWonDetail_V2460(item, GeosApplication.Instance.IdCurrencyByRegion, string.Join(",", FinalSalesUsersList.Select(x => x.IdSalesUser)),
                        //                             GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, GeosApplication.Instance.ActiveUser.IdUser));
                        //CrmStartUp = new CrmServiceController("localhost:6699");
                        //[Rahul.Gadhave][GEOS2-7650][Date:04-01-2025]
                        SalesUserWon.AddRange(CrmStartUp.GetAllSalesTeamUserWonDetail_V2630(item, GeosApplication.Instance.IdCurrencyByRegion, string.Join(",", FinalSalesUsersList.Select(x => x.IdSalesUser)),
                                                         GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, GeosApplication.Instance.ActiveUser.IdUser));
                        //Fill business unit section
                        if (isTarget)
                        {
                            BusinessUnitTargetDetail businessUnitTargetDetail = new BusinessUnitTargetDetail();
                            #region ServicesLog
                            //[rdixit][GEOS2-3518][30.06.2022][Ignore the “To” date in the “DASHBOARD->Operations Review” section for Opportunities info]

                            //businessUnitTargetDetail = CrmStartUp.GetBusinessUnitStatusWithTarget_V2110(GeosApplication.Instance.IdCurrencyByRegion,
                            //                                                        plantOwnersIds.ToString(),
                            //                                                        GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate,
                            //                                                        GeosApplication.Instance.ActiveUser.IdUser,
                            //                                                        item, isTarget);
                            //businessUnitTargetDetail = CrmStartUp.GetBusinessUnitStatusWithTarget_IgnoreOfferCloseDate(GeosApplication.Instance.IdCurrencyByRegion,
                            //                                                        plantOwnersIds.ToString(),
                            //                                                        GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate,
                            //                                                        GeosApplication.Instance.ActiveUser.IdUser,
                            //                                                        item, isTarget);
                            #endregion
                            //[GEOS2-4224][rdixit][28.02.2023]
                            businessUnitTargetDetail = CrmStartUp.GetBusinessUnitStatusWithTarget_IgnoreOfferCloseDate_V2360(GeosApplication.Instance.IdCurrencyByRegion, plantOwnersIds.ToString(),
                                                       GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, GeosApplication.Instance.ActiveUser.IdUser, item, isTarget);

                            BusinessUnitStatusWise.AddRange(businessUnitTargetDetail.BusinessUnitDetail);
                            BusinessUnitPlantQuotaAmountBUWise = new List<BusinessUnitDetail>();
                            BusinessUnitPlantQuotaAmountBUWise.AddRange(businessUnitTargetDetail.TargetDetail);

                        }
                        else
                        {
                            #region ServicesLog
                            //BusinessUnitStatusWise.AddRange(CrmStartUp.GetBusinessUnitStatusWithTarget_V2110(GeosApplication.Instance.IdCurrencyByRegion,
                            //                                                        plantOwnersIds.ToString(),
                            //                                                        GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate,
                            //                                                      GeosApplication.Instance.ActiveUser.IdUser,
                            //                                                        item, isTarget).BusinessUnitDetail.ToList());

                            //[rdixit][GEOS2-3518][30.06.2022][Ignore the “To” date in the “DASHBOARD->Operations Review” section for Opportunities info]
                            //BusinessUnitStatusWise.AddRange(CrmStartUp.GetBusinessUnitStatusWithTarget_IgnoreOfferCloseDate(GeosApplication.Instance.IdCurrencyByRegion,
                            //                                                        plantOwnersIds.ToString(),
                            //                                                        GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate,
                            //                                                      GeosApplication.Instance.ActiveUser.IdUser,
                            //                                                        item, isTarget).BusinessUnitDetail.ToList());
                            #endregion
                            //[GEOS2-4224][rdixit][28.02.2023]
                            BusinessUnitStatusWise.AddRange(CrmStartUp.GetBusinessUnitStatusWithTarget_IgnoreOfferCloseDate_V2360(GeosApplication.Instance.IdCurrencyByRegion,
                                                               plantOwnersIds.ToString(), GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate,
                                                               GeosApplication.Instance.ActiveUser.IdUser, item, isTarget).BusinessUnitDetail.ToList());
                        }

                        isTarget = false;
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

                GeosApplication.Instance.SplashScreenMessage = string.Empty;

                if (FailedPlants == null || FailedPlants.Count == 0)
                {
                    IsShowFailedPlantWarning = false;
                    WarningFailedPlants = string.Empty;
                }


            }
            else
            {
                IsShowFailedPlantWarning = false;
                WarningFailedPlants = String.Empty;
            }

            GeosApplication.Instance.SplashScreenMessage = string.Empty;
            GeosApplication.Instance.Logger.Log("Method FillListsFromServices() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// This method is used create and fill DataTable Sales Status By Month.
        /// </summary>
        private void CreateDataTableSalesStatusByMonth()
        {
            GeosApplication.Instance.Logger.Log("Method CreateDataTableSalesStatusByMonth ...", category: Category.Info, priority: Priority.Low);

            //Create DataTable for Sales Status By Month.
            DataTable dtTable = new DataTable();

            dtTable.Columns.Add("MonthNo", typeof(string)).DefaultValue = string.Empty;
            dtTable.Columns.Add("Year");
            dtTable.Columns.Add("MonthYear");
            //dtTable.Columns.Add("MonthName", typeof(string)).DefaultValue = string.Empty;
            foreach (var item in ListAllSalesStatusType)
            {
                dtTable.Columns.Add(item.Name, typeof(double)).DefaultValue = 0;
            }
            dtTable.Columns.Add("TargetPlantQuota", typeof(double)).DefaultValue = 0;
            dtTable.Columns.Add("AccumulatedSales", typeof(double)).DefaultValue = 0;

            //Fill Data for Status By Month.
            try
            {
                dtTable.Rows.Clear();

                double TargetPlantQuotaPrevMonth = 0.0;
                double AccumulatedSalesPrevMonth = 0.0;
                double AccumulatedSales = 0.0;

                if (Convert.ToInt32(GeosApplication.Instance.UserSettings["CustomPeriodOption"]) == 0)
                {
                    double TargetPlantQuota = SalesStatusByMonthList[0].QuotaAmountWithExchangeRate / 12;
                    int[] icol = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

                    foreach (var mt in icol)
                    {
                        DataRow dataRow = dtTable.NewRow();

                        dataRow[0] = mt.ToString().PadLeft(2, '0');

                        dataRow[1] = GeosApplication.Instance.CrmOfferYear;
                        dataRow[2] = mt.ToString().PadLeft(2, '0');
                        int monthstart = Convert.ToInt32(dataRow[2]);
                        int yearstart = Convert.ToInt32(GeosApplication.Instance.CrmOfferYear);


                        foreach (var item in ListAllSalesStatusType)
                        {
                            dataRow[item.Name] = SalesStatusByMonthList.Where(m => m.SaleStatusName == item.Name && m.CurrentMonth == monthstart && m.CurrentYear == yearstart).Select(mv => mv.Value).ToList().Sum();

                            if (item.Name == "WON")
                            {
                                AccumulatedSales = AccumulatedSalesPrevMonth + SalesStatusByMonthList.Where(m => m.SaleStatusName == item.Name && m.CurrentMonth == monthstart && m.CurrentYear == yearstart).Select(mv => mv.Value).ToList().Sum();
                                dataRow["AccumulatedSales"] = AccumulatedSalesPrevMonth = AccumulatedSales;
                            }
                        }

                        //TargetPlantQuota
                        dataRow["TargetPlantQuota"] = TargetPlantQuotaPrevMonth + TargetPlantQuota;
                        TargetPlantQuotaPrevMonth = Convert.ToDouble(dataRow["TargetPlantQuota"]);

                        dtTable.Rows.Add(dataRow);
                    }
                }
                else
                {
                    var start = GeosApplication.Instance.SelectedyearStarDate;
                    var end = GeosApplication.Instance.SelectedyearEndDate;

                    end = new DateTime(end.Year, end.Month, DateTime.DaysInMonth(end.Year, end.Month));

                    var diff = Enumerable.Range(0, Int32.MaxValue)
                                         .Select(e => start.AddMonths(e))
                                         .TakeWhile(e => e <= end)
                                         .Select(e => e.ToString("MM-yyyy"));

                    double TargetPlantQuota = SalesStatusByMonthList[0].QuotaAmountWithExchangeRate / diff.Count();

                    foreach (var mty in diff)
                    {
                        DataRow dataRow = dtTable.NewRow();
                        String[] substrings = mty.ToString().Split('-');
                        dataRow[0] = substrings[0];
                        dataRow[1] = substrings[1];
                        dataRow[2] = substrings[1] + substrings[0];
                        int monthstart = Convert.ToInt32(substrings[0]);
                        int yearstart = Convert.ToInt32(substrings[1]);
                        // int k = 3;
                        foreach (var item in ListAllSalesStatusType)
                        {
                            dataRow[item.Name] = SalesStatusByMonthList.Where(m => m.SaleStatusName == item.Name && m.CurrentMonth == monthstart && m.CurrentYear == yearstart).Select(mv => mv.Value).ToList().Sum();

                            if (item.Name == "WON")
                            {
                                AccumulatedSales = AccumulatedSalesPrevMonth + SalesStatusByMonthList.Where(m => m.SaleStatusName == item.Name && m.CurrentMonth == monthstart && m.CurrentYear == yearstart).Select(mv => mv.Value).ToList().Sum();
                                dataRow["AccumulatedSales"] = AccumulatedSalesPrevMonth = AccumulatedSales;
                            }
                        }

                        //TargetPlantQuota
                        dataRow["TargetPlantQuota"] = TargetPlantQuotaPrevMonth + TargetPlantQuota;
                        TargetPlantQuotaPrevMonth = Convert.ToDouble(dataRow["TargetPlantQuota"]);

                        dtTable.Rows.Add(dataRow);
                    }
                }

                GeosApplication.Instance.Logger.Log("Method CreateDataTableSalesStatusByMonth() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CreateDataTableSalesStatusByMonth() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            DataTableSalesStatusByMonthGraph = dtTable;
        }

        /// <summary>
        /// Create DataTable for Customers.
        /// </summary>
        private void FillGraphDataTableCustomers()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillGraphDataTableCustomers ...", category: Category.Info, priority: Priority.Low);

                DataTableCustomerGraph = new DataTable();
                DataTableCustomerGraph.Columns.Add("IdCustomer", typeof(Int16));
                DataTableCustomerGraph.Columns.Add("CustomerName");
                DataTableCustomerGraph.Columns.Add("TARGET", typeof(double)).DefaultValue = 0;

                foreach (var item in ListAllSalesStatusType)
                {
                    DataTableCustomerGraph.Columns.Add(item.Name, typeof(double)).DefaultValue = 0;
                }

                GeosApplication.Instance.Logger.Log("Method FillGraphDataTableCustomers() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillGraphDataTableCustomers() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Fill DataTable for Customers.
        /// </summary>
        private void FillDataTableCustomers()
        {
            FillGraphDataTableCustomers();

            try
            {
                GeosApplication.Instance.Logger.Log("Method FillDataTableCustomers ...", category: Category.Info, priority: Priority.Low);

                foreach (var item in CustomersFromPlant)
                {

                    DataRow dataRow = DataTableCustomerGraph.AsEnumerable().FirstOrDefault(row => Convert.ToInt16(row["IdCustomer"]) == item.IdCustomer);

                    if (dataRow != null)
                    {

                        foreach (var itemCompany in item.Companies)
                        {
                            foreach (var itemSalesStatusType in itemCompany.SalesStatusTypeDetail)
                            {
                                dataRow[itemSalesStatusType.Name] = Convert.ToDouble(dataRow[itemSalesStatusType.Name]) + itemSalesStatusType.TotalAmount;
                            }

                        }
                    }
                    else
                    {
                        dataRow = DataTableCustomerGraph.NewRow();
                        dataRow["IdCustomer"] = item.IdCustomer;
                        dataRow["CustomerName"] = item.CustomerName;
                        List<Int32> idSites = item.Companies.Select(ii => ii.IdCompany).ToList();

                        dataRow["TARGET"] = Companies.Where(x => idSites.Contains(x.IdSite)).Select(x => x.TargetAmountWithExchangeRate).ToList().Sum();
                        foreach (var itemCompany in item.Companies)
                        {
                            foreach (var itemSalesStatusType in itemCompany.SalesStatusTypeDetail)
                            {
                                dataRow[itemSalesStatusType.Name] = Convert.ToDouble(dataRow[itemSalesStatusType.Name]) + itemSalesStatusType.TotalAmount;
                            }
                        }

                        DataTableCustomerGraph.Rows.Add(dataRow);
                    }
                }


                //Sort by WON column
                DataView dv = DataTableCustomerGraph.DefaultView;
                dv.Sort = "WON DESC";
                DataTable sortedDT = dv.ToTable();
                DataTableCustomerGraph = sortedDT;

                ////Take first five rows.
                DataTableCustomerGraph = DataTableCustomerGraph.AsEnumerable().Take(5).CopyToDataTable();

                GeosApplication.Instance.Logger.Log("Method FillDataTableCustomers() Successfully ...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Get Error in Method FillDataTableCustomers() ..." + ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillSalesTeam()
        {
            GeosApplication.Instance.Logger.Log("Method FillSalesTeam ...", category: Category.Info, priority: Priority.Low);

            DataTable dtTableSUQ = new DataTable();
            dtTableSUQ.Columns.Add("Name", typeof(string)).DefaultValue = string.Empty;
            dtTableSUQ.Columns.Add("Percentage", typeof(double)).DefaultValue = 0.0;
            dtTableSUQ.Rows.Add(new Object[] { "Achieved", 0 });        //
            dtTableSUQ.Rows.Add(new Object[] { "Not achieved", 100 });  //, "WhiteSmoke"


            foreach (SalesUserQuota salesUser in FinalSalesUsersList)
            {
                DataTable dtTableSalesUserQuota = dtTableSUQ.Copy();
           
                Double amount = SalesUserWon.Where(i => (i.Idsalesresponsible == salesUser.IdSalesUser || i.IdSalesResponsibleAssemblyBU == salesUser.IdSalesUser) && (i.IdSalesOwner == salesUser.IdSalesUser || i.IdSalesOwner == null)).ToList().Select(i => i.Amount).Sum();

                salesUser.WonValue = salesUser.WonValue + amount;

                if (salesUser.SalesQuotaAmount != 0)
                {
                    salesUser.Percentage = Math.Round((salesUser.WonValue / salesUser.SalesQuotaAmount) * 100, 2);
                }
                else
                {
                    salesUser.Percentage = 0;
                }

                if (salesUser.Percentage != null)
                {
                    dtTableSalesUserQuota.Rows[0]["Percentage"] = salesUser.Percentage;
                    if (salesUser.Percentage < 100)
                    {
                        dtTableSalesUserQuota.Rows[1]["Percentage"] = 100 - salesUser.Percentage;
                    }
                    else
                    {
                        dtTableSalesUserQuota.Rows[1]["Percentage"] = 0;
                    }
                }
                salesUser.Tag = dtTableSalesUserQuota;

                if (GeosApplication.Instance.UserCommonDetailList == null)
                {
                    GeosApplication.Instance.UserCommonDetailList = new List<UserCommonDetail>();
                }

                //Fill sales user image. 
                if (!GeosApplication.Instance.UserCommonDetailList.Any(i => i.IdUser == salesUser.IdSalesUser))
                {
                    User user = WorkbenchStartUp.GetUserById(salesUser.IdSalesUser);

                    if (user != null)
                    {
                        byte[] UserProfileImageByte = null;
                        try
                        {
                            UserProfileImageByte = GeosRepositoryServiceController.GetUserProfileImageWithoutException(user.Login);
                            if (UserProfileImageByte != null)
                            {
                                salesUser.People.OwnerImage = byteArrayToBitmapImage(UserProfileImageByte);
                            }
                            else
                            {
                                salesUser.People.IdPersonGender = user.IdUserGender;
                            }
                        }
                        catch (Exception)
                        {
                        }

                        UserCommonDetail userCommonDetail = new UserCommonDetail();
                        userCommonDetail.IdUser = user.IdUser;
                        userCommonDetail.IdPersonGender = user.IdUserGender;
                        userCommonDetail.ImageBytes = UserProfileImageByte;
                        userCommonDetail.OwnerImage = byteArrayToBitmapImage(UserProfileImageByte);

                        GeosApplication.Instance.UserCommonDetailList.Add(userCommonDetail);
                    }
                    else
                    {

                        UserCommonDetail userCommonDetail = new UserCommonDetail();
                        userCommonDetail.IdUser = salesUser.IdSalesUser;
                        userCommonDetail.IdPersonGender = null;
                        userCommonDetail.ImageBytes = null;
                        userCommonDetail.OwnerImage = null;

                        GeosApplication.Instance.UserCommonDetailList.Add(userCommonDetail);
                    }
                }
                else
                {
                    UserCommonDetail userCommonDetail = GeosApplication.Instance.UserCommonDetailList.Where(i => i.IdUser == salesUser.IdSalesUser).FirstOrDefault();
                    if (userCommonDetail.ImageBytes != null)
                    {
                        salesUser.People.OwnerImage = byteArrayToBitmapImage(userCommonDetail.ImageBytes);
                    }
                    else
                    {
                        salesUser.People.IdPersonGender = userCommonDetail.IdPersonGender;
                    }
                }
            }

            GeosApplication.Instance.Logger.Log("Method FillSalesTeam() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// This method is used to 
        /// </summary>
        private void FillBusinessUnits()
        {
            GeosApplication.Instance.Logger.Log("Method FillBusinessUnits ...", category: Category.Info, priority: Priority.Low);
            if (GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
            {
                //Merge Business units from all plants
                List<BusinessUnitDetail> FinalBusinessUnitStatusWise = new List<BusinessUnitDetail>();

                if (GeosApplication.Instance.SalesStatusTypeList == null)
                {
                    GeosApplication.Instance.SalesStatusTypeList = new List<SalesStatusTypeDetail>(CrmStartUp.GetSalesStatusTypeDetail().ToList());
                }


                DataTable dtTableBU = new DataTable();
                dtTableBU.Columns.Add("Name", typeof(string)).DefaultValue = string.Empty;
                dtTableBU.Columns.Add("Percentage", typeof(double)).DefaultValue = 0.0;
                dtTableBU.Rows.Add(new Object[] { "Achieved", 0 });         // , "#19A57C"
                dtTableBU.Rows.Add(new Object[] { "Not achieved", 100 });   //, "WhiteSmoke"

                foreach (BusinessUnitDetail item in BusinessUnitList)
                {
                    //Clone all objects and add to mainlist.
                    item.Percentage = 0.0;

                    item.SalesStatusTypeDetail = new List<SalesStatusTypeDetail>();

                    foreach (var item12 in GeosApplication.Instance.SalesStatusTypeList.Where(x => x.IdSalesStatusType == 3 || x.IdSalesStatusType == 4).ToList())
                    {

                        item.SalesStatusTypeDetail.Add((SalesStatusTypeDetail)item12.Clone());
                    }
                    DataTable dtTableBU2 = dtTableBU.Copy();

                    //Fill Target
                    BusinessUnitDetail LookupValue = businessUnitPlantQuotaAmountBUWise.FirstOrDefault(x => x.IdLookupValue == item.IdLookupValue);
                    if (LookupValue != null)
                    {
                        item.Amount = LookupValue.Amount;
                        //item.Amount = 0;
                    }

                    //Quoted and Won
                    List<BusinessUnitDetail> BusinessUnitStatusWiseById = BusinessUnitStatusWise.Where(x => x.IdLookupValue == item.IdLookupValue).ToList();
                    foreach (BusinessUnitDetail LookupValueInner in BusinessUnitStatusWiseById)
                    {
                        foreach (var salesStatus in item.SalesStatusTypeDetail)
                        {
                            SalesStatusTypeDetail temp = LookupValueInner.SalesStatusTypeDetail.FirstOrDefault(x => x.IdSalesStatusType == salesStatus.IdSalesStatusType);
                            if (temp != null)
                            {
                                salesStatus.TotalAmount = salesStatus.TotalAmount + temp.TotalAmount;

                                if (temp.IdSalesStatusType == 4 && item.Amount != 0) //Achieved percentage
                                {
                                    item.Percentage = Math.Round((salesStatus.TotalAmount / item.Amount) * 100, 2);
                                    dtTableBU2.Rows[0]["Percentage"] = item.Percentage;
                                    if (item.Percentage < 100)
                                    {
                                        dtTableBU2.Rows[1]["Percentage"] = 100 - item.Percentage;
                                    }
                                    else
                                    {
                                        dtTableBU2.Rows[1]["Percentage"] = 0;
                                    }
                                }

                            }
                        }
                    }

                    item.Tag = dtTableBU2;
                    //Add Target in the list.
                    SalesStatusTypeDetail target = new SalesStatusTypeDetail();
                    target.Name = "Target";
                    target.TotalAmount = item.Amount;

                    item.SalesStatusTypeDetail.Add(target);


                }
            }

            OnPropertyChanged(new PropertyChangedEventArgs("BusinessUnitList"));
            GeosApplication.Instance.Logger.Log("Method FillBusinessUnits() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Get image by path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public ImageSource GetImage(string path)
        {
            return new BitmapImage(new Uri(path, UriKind.Relative));
        }

        /// <summary>
        /// This method is for to convert from Bytearray to ImageSource
        /// </summary>
        /// <param name="byteArrayIn"></param>
        /// <returns></returns>
        public ImageSource byteArrayToBitmapImage(byte[] byteArrayIn)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method convert byteArrayToImage ...", category: Category.Info, priority: Priority.Low);

                if (byteArrayIn == null || byteArrayIn.Length == 0) return null;
                var image = new BitmapImage();

                using (var mem = new MemoryStream(byteArrayIn))
                {
                    mem.Position = 0;
                    image.BeginInit();
                    image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.UriSource = null;
                    image.StreamSource = mem;
                    image.EndInit();
                }

                image.Freeze();
                return image;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in byteArrayToImage() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            return null;
        }

        /// <summary>
        /// Method for refresh DahsBoard Performance From database by refresh Button.
        /// </summary>
        /// <param name="obj"></param>
        private void RefreshDashboardOperationsDetails(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method RefreshDashboardOperationsDetails ...", category: Category.Info, priority: Priority.Low);

            FailedPlants = new List<string>();
            IsShowFailedPlantWarning = false;
            WarningFailedPlants = String.Empty;
            if (GeosApplication.Instance.SelectedPlantOwnerUsersList != null && GeosApplication.Instance.SelectedPlantOwnerUsersList.Count > 0)
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

                FillDashboardOperationsByPlant();
            }
            else
            {
                DataTableSalesStatusByMonthGraph = new DataTable();
                DataTableCustomerGraph = new DataTable();
                FinalSalesUsersList = new ObservableCollection<SalesUserQuota>();
                BusinessUnitList = null;
                FailedPlants.Clear();
                IsShowFailedPlantWarning = false;
                WarningFailedPlants = String.Empty;
            }

            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

            GeosApplication.Instance.Logger.Log("Method OpenDashboardSaleView() executed successfully", category: Category.Info, priority: Priority.Low);
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

            if (GeosApplication.Instance.SelectedPlantOwnerUsersList != null && GeosApplication.Instance.SelectedPlantOwnerUsersList.Count > 0)
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

                FillDashboardOperationsByPlant();

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

                DataTableSalesStatusByMonthGraph = new DataTable();
                DataTableCustomerGraph = new DataTable();
                FinalSalesUsersList = new ObservableCollection<SalesUserQuota>();
                BusinessUnitList = null;
                FailedPlants.Clear();
                IsShowFailedPlantWarning = false;
                WarningFailedPlants = String.Empty;
            }

            GeosApplication.Instance.Logger.Log("Method PlantOwnerPopupClosedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        //This method is used to load Sales Status Charts.
        private void ChartSalesStatusLoadCommandAction(RoutedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChartSalesStatusLoadCommandAction ...", category: Category.Info, priority: Priority.Low);

                ChartControl chartControl = (ChartControl)(obj.OriginalSource);
                chartControl.BeginInit();
                XYDiagram2D diagram = new XYDiagram2D();
                chartControl.Diagram = diagram;
                diagram.ActualAxisX.Title = new AxisTitle() { Content = "Months" };
                diagram.ActualAxisY.Title = new AxisTitle() { Content = "Amount" };
                diagram.ActualAxisX.QualitativeScaleOptions = new QualitativeScaleOptions();
                diagram.ActualAxisX.QualitativeScaleOptions.AutoGrid = false;
                GeosApplication.SetCurrencySymbol(GeosApplication.Instance.CurrentCurrencySymbol);

                diagram.ActualAxisY.Label = new AxisLabel();
                diagram.ActualAxisY.Label.TextPattern = "{V:c2}";

                foreach (var item in ListAllSalesStatusType)
                {
                    if (item != null)
                    {
                        BarSideBySideStackedSeries2D barSideBySideStackedSeries2D = new BarSideBySideStackedSeries2D();
                        barSideBySideStackedSeries2D.ArgumentScaleType = ScaleType.Auto;
                        barSideBySideStackedSeries2D.ValueScaleType = ScaleType.Numerical;
                        barSideBySideStackedSeries2D.DisplayName = item.Name;
                        barSideBySideStackedSeries2D.Brush = (SolidColorBrush)(new BrushConverter().ConvertFrom(item.HtmlColor.ToString()));

                        barSideBySideStackedSeries2D.ArgumentDataMember = "MonthYear";
                        barSideBySideStackedSeries2D.ValueDataMember = item.Name;
                        barSideBySideStackedSeries2D.CrosshairLabelPattern = "{S} : {V:c2}";
                        barSideBySideStackedSeries2D.Model = new DevExpress.Xpf.Charts.SimpleBar2DModel();
                        barSideBySideStackedSeries2D.ShowInLegend = false;

                        barSideBySideStackedSeries2D.AnimationAutoStartMode = AnimationAutoStartMode.SetStartState;
                        Bar2DSlideFromTopAnimation seriesAnimation = new Bar2DSlideFromTopAnimation();
                        seriesAnimation.Duration = new TimeSpan(0, 0, 2);
                        barSideBySideStackedSeries2D.PointAnimation = seriesAnimation;
                        diagram.Series.Add(barSideBySideStackedSeries2D);

                        BarSideBySideStackedSeries2D barSideBySideStackedSeries2Dhidden = new BarSideBySideStackedSeries2D();
                        barSideBySideStackedSeries2Dhidden.Brush = (SolidColorBrush)(new BrushConverter().ConvertFrom(item.HtmlColor.ToString()));
                        chartControl.Diagram.Series.Insert(0, barSideBySideStackedSeries2Dhidden);
                        barSideBySideStackedSeries2Dhidden.DisplayName = item.Name;
                    }
                }

                LineSeries2D lineDashedTargetPlant = new LineSeries2D();
                lineDashedTargetPlant.LineStyle = new LineStyle();
                lineDashedTargetPlant.LineStyle.DashStyle = new DashStyle();
                lineDashedTargetPlant.LineStyle.Thickness = 2;
                lineDashedTargetPlant.LineStyle.DashStyle.Dashes = new DoubleCollection(new double[] { 3, 3, 3, 3 });
                lineDashedTargetPlant.ArgumentScaleType = ScaleType.Auto;
                lineDashedTargetPlant.ValueScaleType = ScaleType.Numerical;
                lineDashedTargetPlant.DisplayName = "Accumulated plant target";
                lineDashedTargetPlant.CrosshairLabelPattern = "{S} : {V:c2}";

                lineDashedTargetPlant.ArgumentDataMember = "MonthYear";
                lineDashedTargetPlant.ValueDataMember = "TargetPlantQuota";
                chartControl.Diagram.Series.Add(lineDashedTargetPlant);

                SplineSeries2D lineAccumulatedSales = new SplineSeries2D();
                lineAccumulatedSales.ArgumentScaleType = ScaleType.Auto;
                lineAccumulatedSales.ValueScaleType = ScaleType.Numerical;
                lineAccumulatedSales.DisplayName = "Accumulated sales";
                lineAccumulatedSales.CrosshairLabelPattern = "{S} : {V:c2}";
                lineAccumulatedSales.LineTension = 1;

                lineAccumulatedSales.ArgumentDataMember = "MonthYear";
                lineAccumulatedSales.ValueDataMember = "AccumulatedSales";
                chartControl.Diagram.Series.Add(lineAccumulatedSales);

                chartControl.EndInit();
                chartControl.AnimationMode = ChartAnimationMode.OnDataChanged;
                chartControl.Animate();

                GeosApplication.Instance.Logger.Log("Method ChartSalesStatusLoadCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ChartSalesStatusLoadCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ChartSalesStatusLoadCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ChartSalesStatusLoadCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// This method is used to change order of all status types.
        /// </summary>
        /// <param name="obj"></param>
        private void ChartSalesStatusCustomDrawCrosshairCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method ChartSalesStatusCustomDrawCrosshairCommandAction ...", category: Category.Info, priority: Priority.Low);

            try
            {
                CustomDrawCrosshairEventArgs e = (CustomDrawCrosshairEventArgs)obj;
                foreach (var group in e.CrosshairElementGroups)
                {
                    var reverseList = group.CrosshairElements.ToList();
                    group.CrosshairElements.Clear();
                    foreach (var item in reverseList)
                    {
                        if (item.Series.DisplayName == "Accumulated plant target" || item.Series.DisplayName == "Accumulated sales")
                            group.CrosshairElements.Add(item);
                        else
                            group.CrosshairElements.Insert(0, item);
                    }
                }

                GeosApplication.Instance.Logger.Log("Method ChartSalesStatusCustomDrawCrosshairCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ChartSalesStatusCustomDrawCrosshairCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// This method is used to load customers chart.
        /// </summary>
        /// <param name="obj"></param>
        private void ChartCustomersLoadCommandAction(RoutedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChartCustomersLoadCommandAction ...", category: Category.Info, priority: Priority.Low);

                ChartControl chartControl = (ChartControl)(obj.OriginalSource);

                chartControl.BeginInit();
                XYDiagram2D diagram = new XYDiagram2D();
                chartControl.Diagram = diagram;
                diagram.ActualAxisX.Title = new AxisTitle() { Content = "Customers" };
                diagram.ActualAxisY.Title = new AxisTitle() { Content = "Amount" };

                GeosApplication.SetCurrencySymbol(GeosApplication.Instance.CurrentCurrencySymbol);
                diagram.ActualAxisY.NumericOptions = new NumericOptions();
                diagram.ActualAxisY.NumericOptions.Format = NumericFormat.Currency;

                foreach (var item in ListAllSalesStatusType)
                {
                    if (item != null)
                    {
                        BarSideBySideStackedSeries2D barSideBySideStackedSeries2D = new BarSideBySideStackedSeries2D();
                        barSideBySideStackedSeries2D.ArgumentScaleType = ScaleType.Auto;
                        barSideBySideStackedSeries2D.ValueScaleType = ScaleType.Numerical;
                        barSideBySideStackedSeries2D.DisplayName = item.Name;
                        barSideBySideStackedSeries2D.Brush = (SolidColorBrush)(new BrushConverter().ConvertFrom(item.HtmlColor.ToString()));
                        barSideBySideStackedSeries2D.ArgumentDataMember = "CustomerName";
                        barSideBySideStackedSeries2D.ValueDataMember = item.Name;
                        barSideBySideStackedSeries2D.CrosshairLabelPattern = "{S} : {V:c2}";
                        barSideBySideStackedSeries2D.Model = new DevExpress.Xpf.Charts.SimpleBar2DModel();
                        barSideBySideStackedSeries2D.ShowInLegend = false;

                        barSideBySideStackedSeries2D.AnimationAutoStartMode = AnimationAutoStartMode.SetStartState;
                        Bar2DSlideFromTopAnimation seriesAnimation = new Bar2DSlideFromTopAnimation();
                        seriesAnimation.Duration = new TimeSpan(0, 0, 2);
                        barSideBySideStackedSeries2D.PointAnimation = seriesAnimation;

                        diagram.Series.Add(barSideBySideStackedSeries2D);

                        BarSideBySideStackedSeries2D barSideBySideStackedSeries2Dhidden = new BarSideBySideStackedSeries2D();
                        barSideBySideStackedSeries2Dhidden.Brush = (SolidColorBrush)(new BrushConverter().ConvertFrom(item.HtmlColor.ToString()));
                        chartControl.Diagram.Series.Insert(0, barSideBySideStackedSeries2Dhidden);
                        barSideBySideStackedSeries2Dhidden.DisplayName = item.Name;
                    }
                }

                LineStackedSeries2D lineStackedSeries2D = new DevExpress.Xpf.Charts.LineStackedSeries2D();
                lineStackedSeries2D.ArgumentScaleType = ScaleType.Auto;
                lineStackedSeries2D.ValueScaleType = ScaleType.Numerical;
                lineStackedSeries2D.DisplayName = "TARGET";
                lineStackedSeries2D.CrosshairLabelPattern = "{S} : {V:c2}";
                lineStackedSeries2D.Brush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF7F50"));
                lineStackedSeries2D.ArgumentDataMember = "CustomerName";
                lineStackedSeries2D.ValueDataMember = "TARGET";
                chartControl.Diagram.Series.Add(lineStackedSeries2D);

                chartControl.EndInit();
                chartControl.AnimationMode = ChartAnimationMode.OnDataChanged;
                chartControl.Animate();

                GeosApplication.Instance.Logger.Log("Method ChartCustomersLoadCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ChartCustomersLoadCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ChartCustomersLoadCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ChartCustomersLoadCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// This method is used change the order of status types.
        /// </summary>
        /// <param name="obj"></param>
        private void ChartCustomersCustomDrawCrosshairCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method ChartCustomersCustomDrawCrosshairCommandAction ...", category: Category.Info, priority: Priority.Low);

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

                GeosApplication.Instance.Logger.Log("Method ChartCustomersCustomDrawCrosshairCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ChartCustomersCustomDrawCrosshairCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// This method is used to change color of the series points.
        /// </summary>
        /// <param name="obj"></param>
        private void PieChartDrawSeriesPointCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method PieChartDrawSeriesPointCommandAction ...", category: Category.Info, priority: Priority.Low);

            try
            {
                CustomDrawSeriesPointEventArgs e = (CustomDrawSeriesPointEventArgs)obj;

                if (ThemeManager.ApplicationThemeName == "BlackAndBlue")
                {
                    if (e.SeriesPoint.Argument == "Achieved")
                        e.DrawOptions.Color = (Color)ColorConverter.ConvertFromString("#2AB7FF");
                }
                else
                {
                    if (e.SeriesPoint.Argument == "Achieved")
                        e.DrawOptions.Color = (Color)ColorConverter.ConvertFromString("#083493");
                }

                if (ThemeManager.ApplicationThemeName == "BlackAndBlue")
                {
                    if (e.SeriesPoint.Argument == "Not achieved")
                    {
                        e.DrawOptions.Color = (Color)ColorConverter.ConvertFromString("#F5F5F5");//gray   // "WhiteSmoke"
                    }
                }
                else
                {
                    if (e.SeriesPoint.Argument == "Not achieved")
                    {
                        e.DrawOptions.Color = (Color)ColorConverter.ConvertFromString("#A9A9A9");//gray   // "WhiteSmoke"
                    }
                }

                GeosApplication.Instance.Logger.Log("Method PieChartDrawSeriesPointCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in PieChartDrawSeriesPointCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// This event is used to load Pie chart Sales team - added animation.
        /// </summary>
        /// <param name="obj"></param>
        private void PieChartSalesTeamLoadCommandAction(RoutedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PieChartSalesTeamLoadCommandAction ...", category: Category.Info, priority: Priority.Low);

                ChartControl chartControl = (ChartControl)(obj.OriginalSource);
                chartControl.BeginInit();

                PieSeries2D series = (PieSeries2D)chartControl.Diagram.Series[0];
                series.AnimationAutoStartMode = AnimationAutoStartMode.SetStartState;
                Pie2DFlyInAnimation pie2DFlyInAnimation = new Pie2DFlyInAnimation();
                pie2DFlyInAnimation.Duration = new TimeSpan(0, 0, 3);
                pie2DFlyInAnimation.PointOrder = PointAnimationOrder.Random;
                series.PointAnimation = pie2DFlyInAnimation;

                chartControl.EndInit();
                chartControl.AnimationMode = ChartAnimationMode.OnDataChanged;
                chartControl.Animate();

                GeosApplication.Instance.Logger.Log("Method PieChartSalesTeamLoadCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in PieChartSalesTeamLoadCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in PieChartSalesTeamLoadCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in PieChartSalesTeamLoadCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion  //Methods
    }
}