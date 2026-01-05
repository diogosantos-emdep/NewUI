using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Spreadsheet;
using DevExpress.Xpf.Charts;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
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
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;

namespace Emdep.Geos.Modules.Crm.ViewModels
{
    public class DashboardEngineeringAnalysisViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Services

        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion  //Services

        #region Declaration
        bool isBusy;
        private List<string> failedPlants;
        private Boolean isShowFailedPlantWarning;
        private string warningFailedPlants;

        private ObservableCollection<Offer> allOffers;
        private ObservableCollection<Offer> openOffers;
        private ObservableCollection<Offer> closedOffers;

        private DataTable dataTableOpenOffers;
        private DataTable dataTableIndicators;
        private DataTable dataTableClosedOffers;
        private DataTable dataTableEnggAnalysisByMonth;
        private string openOffersText;
        private int openOffersCount;
        private string closedOffersText;
        private int closedOffersCount;
        private string currentMonthString;
        private string previousMonthString;
        private string periodString;
        #endregion  //Declaration

        #region public Properties
        public string PreviouslySelectedPlantOwners { get; set; }
        public string PreviouslySelectedSaleOwners { get; set; }

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
            }
        }

        //Failed plants
        public List<string> FailedPlants
        {
            get { return failedPlants; }
            set
            {
                failedPlants = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FailedPlants"));
            }
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

        public ObservableCollection<Offer> AllOffers
        {
            get { return allOffers; }
            set
            {
                allOffers = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AllOffers"));
            }
        }

        public ObservableCollection<Offer> OpenOffers
        {
            get { return openOffers; }
            set
            {
                openOffers = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OpenOffers"));
            }
        }

        public ObservableCollection<Offer> ClosedOffers
        {
            get { return closedOffers; }
            set
            {
                closedOffers = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ClosedOffers"));
            }
        }

        public DataTable DataTableIndicators
        {
            get { return dataTableIndicators; }
            set
            {
                dataTableIndicators = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DataTableIndicators"));
            }
        }

        public DataTable DataTableOpenOffers
        {
            get { return dataTableOpenOffers; }
            set
            {
                dataTableOpenOffers = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DataTableOpenOffers"));
            }
        }

        public DataTable DataTableClosedOffers
        {
            get { return dataTableClosedOffers; }
            set
            {
                dataTableClosedOffers = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DataTableClosedOffers"));
            }
        }

        public DataTable DataTableEnggAnalysisByMonth
        {
            get { return dataTableEnggAnalysisByMonth; }
            set
            {
                dataTableEnggAnalysisByMonth = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DataTableEnggAnalysisByMonth"));
            }
        }

        public int ClosedOffersCount
        {
            get { return closedOffersCount; }
            set { closedOffersCount = value; OnPropertyChanged(new PropertyChangedEventArgs("ClosedOffersCount")); }
        }

        public string ClosedOffersText
        {
            get { return closedOffersText; }
            set { closedOffersText = value; OnPropertyChanged(new PropertyChangedEventArgs("ClosedOffersText")); }
        }

        public int OpenOffersCount
        {
            get { return openOffersCount; }
            set { openOffersCount = value; OnPropertyChanged(new PropertyChangedEventArgs("OpenOffersCount")); }
        }
        public string OpenOffersText
        {
            get { return openOffersText; }
            set { openOffersText = value; OnPropertyChanged(new PropertyChangedEventArgs("OpenOffersText")); }
        }

        private ObservableCollection<Emdep.Geos.UI.Helper.Column> columns;
        public ObservableCollection<Emdep.Geos.UI.Helper.Column> Columns
        {
            get { return columns; }
            set
            {
                columns = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Columns"));
            }
        }

        public string PeriodString
        {
            get { return periodString; }
            set { periodString = value; OnPropertyChanged(new PropertyChangedEventArgs("PeriodString")); }
        }
        public string CurrentMonthString
        {
            get { return currentMonthString; }
            set { currentMonthString = value; OnPropertyChanged(new PropertyChangedEventArgs("CurrentMonthString")); }
        }

        public string PreviousMonthString
        {
            get { return previousMonthString; }
            set { previousMonthString = value; OnPropertyChanged(new PropertyChangedEventArgs("PreviousMonthString")); }
        }
        // Export Excel .xlsx
        public virtual string ResultFileName { get; set; }
        public virtual bool DialogResult { get; set; }

        #endregion  //Properties

        #region Commands
        public ICommand SalesOwnerPopupClosedCommand { get; private set; }
        public ICommand PlantOwnerPopupClosedCommand { get; set; }
        public ICommand RefreshDashboardEnggAnalysisCommand { get; set; }
        public ICommand ChartEngineeringAnalysisByMonthLoadCommand { get; set; }
        public ICommand DelayDaysOpenUnboundDataCommand { get; set; }
        public ICommand WeekandDelayDaysClosedUnboundDataCommand { get; set; }
        public ICommand ExportButtonCommand { get; set; }
        public ICommand OfferGridDoubleClickCommand { get; set; }
        #endregion  //Commands

        #region Constructor

        public DashboardEngineeringAnalysisViewModel()
        {
            GeosApplication.Instance.Logger.Log("Constructor DashBoardOperationsViewModel ...", category: Category.Info, priority: Priority.Low);

            FailedPlants = new List<string>();
            IsShowFailedPlantWarning = false;
            WarningFailedPlants = String.Empty;

            PlantOwnerPopupClosedCommand = new DevExpress.Mvvm.DelegateCommand<object>(PlantOwnerPopupClosedCommandAction);
            SalesOwnerPopupClosedCommand = new DevExpress.Mvvm.DelegateCommand<object>(SalesOwnerPopupClosedCommandAction);
            RefreshDashboardEnggAnalysisCommand = new DevExpress.Mvvm.DelegateCommand<object>(RefreshEngineeringAnalysisDashboard);

            ChartEngineeringAnalysisByMonthLoadCommand = new DelegateCommand<RoutedEventArgs>(ChartEngineeringAnalysisByMonthLoadCommandAction);
            ExportButtonCommand = new RelayCommand(new Action<object>(ExportOpenClosedEngineeringAnalysisAction));

            DelayDaysOpenUnboundDataCommand = new DevExpress.Mvvm.DelegateCommand<object>(DelayDaysOpenUnboundDataCommandAction);
            WeekandDelayDaysClosedUnboundDataCommand = new DevExpress.Mvvm.DelegateCommand<object>(WeekandDelayDaysClosedUnboundDataCommandAction);
            OfferGridDoubleClickCommand = new DelegateCommand<object>(OfferGridDoubleClickCommandAction);

            FillDashboardEngineeringAnalysisByPlant();
            //FillCmbSalesOwner();
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

        private void FillDashboardEngineeringAnalysisByPlant()
        {
            CreateDataTableEnggineeringAnalysisByMonth();
            FillCmbSalesOwner();
            //   FillListsFromServices();
            CreateDataTableOpenClosedOffers();
            FillOpenClosedEnggAnalysis();
            FillOpenClosedEnggAnalysisGraph();
            CreateDataTableIndicators();
            FillIndicators();
        }

        /// <summary>
        /// This method is used create and fill DataTable Enggineering Analysis By Month.
        /// </summary>
        private void CreateDataTableEnggineeringAnalysisByMonth()
        {
            GeosApplication.Instance.Logger.Log("Method CreateDataTableSalesStatusByMonth ...", category: Category.Info, priority: Priority.Low);

            //Create DataTable for Enggineering Analysis By Month
            DataTable dtTable = new DataTable();

            dtTable.Columns.Add("MonthNo", typeof(string)).DefaultValue = string.Empty;
            dtTable.Columns.Add("MonthName", typeof(string)).DefaultValue = string.Empty;
            //dtTable.Columns.Add("Month");
            dtTable.Columns.Add("Year");
            dtTable.Columns.Add("MonthYear");
            dtTable.Columns.Add("EngineeringAnalysisOpen", typeof(double)).DefaultValue = 0;
            dtTable.Columns.Add("EngineeringAnalysisClosed", typeof(double)).DefaultValue = 0;
            dtTable.Columns.Add("EngineeringAnalysisClosedAverage", typeof(double)).DefaultValue = 0;

            dtTable.Rows.Clear();
            if (Convert.ToInt32(GeosApplication.Instance.UserSettings["CustomPeriodOption"]) == 0)
            {

                //Fill Data for Enggineering Analysis By Month
                try
                {
                    int[] icol = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
                    foreach (var mt in icol)
                    {
                        DataRow dataRow = dtTable.NewRow();
                        dataRow["MonthNo"] = mt.ToString().PadLeft(2, '0');
                        dataRow["MonthName"] = mt.ToString().PadLeft(2, '0');
                        //System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(mt);
                        dataRow["Year"] = GeosApplication.Instance.CrmOfferYear;
                        dtTable.Rows.Add(dataRow);
                    }
                    GeosApplication.Instance.Logger.Log("Method CreateDataTableSalesStatusByMonth() executed successfully", category: Category.Info, priority: Priority.Low);
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in CreateDataTableSalesStatusByMonth() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
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



                foreach (var mty in diff)
                {
                    DataRow dr = dtTable.NewRow();
                    String[] substrings = mty.ToString().Split('-');
                    dr[0] = substrings[0];
                    dr[1] = substrings[1];
                    dr[2] = substrings[1] + substrings[0];
                    int monthstart = Convert.ToInt32(substrings[0]);
                    int yearstart = Convert.ToInt32(substrings[1]);
                    DataRow dataRow = dtTable.NewRow();
                    //dataRow["MonthNo"] = mt.ToString().PadLeft(2, '0');
                    dataRow["MonthNo"] = substrings[0];
                    dataRow["MonthName"] = substrings[1] + substrings[0];
                    dataRow["Year"] = substrings[1];
                    //System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(mt);
                    dtTable.Rows.Add(dataRow);

                }
            }


            DataTableEnggAnalysisByMonth = dtTable;
        }
        /// <summary>
        /// Method for Fill Dashboard Sales details.
        /// </summary>
        private void FillCmbSalesOwner()
        {
            GeosApplication.Instance.Logger.Log("Method FillCmbSalesOwner ...", category: Category.Info, priority: Priority.Low);

            if (GeosApplication.Instance.IdUserPermission == 21)
            {
                // Call for 1st time.
                FillEngineeringAnalysisByUsers();
            }
            else if (GeosApplication.Instance.IdUserPermission == 22)
            {
                FillEngineeringAnalysis();
            }
            else
            {
                FillEngineeringAnalysis();
            }

            GeosApplication.Instance.Logger.Log("Constructor FillCmbSalesOwner executed Successfully!", category: Category.Info, priority: Priority.Low);
        }

        private void FillEngineeringAnalysisByUsers()
        {
            List<Offer> tempAllOffers = new List<Offer>();
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillEngineeringAnalysisByUsers...", category: Category.Info, priority: Priority.Low);

                var salesOwnersIds = "";
                if (GeosApplication.Instance.SelectedSalesOwnerUsersList != null)
                {
                    List<UserManagerDtl> salesOwners = GeosApplication.Instance.SelectedSalesOwnerUsersList.Cast<UserManagerDtl>().ToList();
                    salesOwnersIds = string.Join(",", salesOwners.Select(i => i.IdUser));
                    PreviouslySelectedSaleOwners = salesOwnersIds;

                    // Continue loop although some plant is not available and Show error message.
                    foreach (var item in GeosApplication.Instance.CompanyList)
                    {
                        try
                        {
                            GeosApplication.Instance.SplashScreenMessage = "Connecting to " + item.Alias;

                            tempAllOffers.AddRange(CrmStartUp.GetSelectedOffersEngAnalysisDateWise(salesOwnersIds, GeosApplication.Instance.IdCurrencyByRegion,
                                                                                               GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone,
                                                                                               GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate,
                                                                                               item).ToList());
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

                    GeosApplication.Instance.SplashScreenMessage = string.Empty;
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log("Get an error in FillEngineeringAnalysisByUsers() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log("Get an error in FillEngineeringAnalysisByUsers() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log("Get an error in FillEngineeringAnalysisByUsers() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            AllOffers = new ObservableCollection<Offer>(tempAllOffers.ToList());
            GeosApplication.Instance.SplashScreenMessage = string.Empty;
        }


        private void FillEngineeringAnalysis()
        {
            List<Offer> tempAllOffers = new List<Offer>();
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillEngineeringAnalysis...", category: Category.Info, priority: Priority.Low);

                if (GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
                {
                    //For each plants
                    List<Company> plantOwners = GeosApplication.Instance.SelectedPlantOwnerUsersList.Cast<Company>().ToList();
                    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.ConnectPlantId));
                    PreviouslySelectedPlantOwners = plantOwnersIds;

                    foreach (Company item in plantOwners)
                    {
                        // Continue loop although some plant is not available and Show error message.
                        try
                        {
                            GeosApplication.Instance.SplashScreenMessage = "Connecting to " + item.Alias;

                            tempAllOffers.AddRange(CrmStartUp.GetOffersEngineeringAnalysisByPermission(GeosApplication.Instance.IdCurrencyByRegion,
                                                                                               GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone,
                                                                                               GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate,
                                                                                               item,
                                                                                               GeosApplication.Instance.IdUserPermission).ToList());
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

                    GeosApplication.Instance.SplashScreenMessage = string.Empty;
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log("Get an error in FillEngineeringAnalysis() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log("Get an error in FillEngineeringAnalysis() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log("Get an error in FillEngineeringAnalysis() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            AllOffers = new ObservableCollection<Offer>(tempAllOffers.ToList());
            GeosApplication.Instance.SplashScreenMessage = string.Empty;
        }

        /// <summary>
        /// This method is used to fill all lists from wcf services.
        /// </summary>
        private void FillListsFromServices()
        {
            GeosApplication.Instance.Logger.Log("In method FillListsFromServices() ...", category: Category.Info, priority: Priority.Low);
            List<Offer> tempAllOffers = new List<Offer>();
            if (GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
            {
                //For each plants
                List<Company> plantOwners = GeosApplication.Instance.SelectedPlantOwnerUsersList.Cast<Company>().ToList();
                var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.ConnectPlantId));
                PreviouslySelectedPlantOwners = plantOwnersIds;

                foreach (Company item in plantOwners)
                {
                    try
                    {
                        GeosApplication.Instance.SplashScreenMessage = "Connecting to " + item.ShortName;

                        tempAllOffers.AddRange(CrmStartUp.GetOffersEngineeringAnalysis(GeosApplication.Instance.IdCurrencyByRegion,
                                                       GeosApplication.Instance.ActiveUser.IdUser,
                                                       GeosApplication.Instance.ActiveUser.Company.Country.IdZone,
                                                       GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate,
                                                       item,
                                                       GeosApplication.Instance.IdUserPermission).ToList());
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

            AllOffers = new ObservableCollection<Offer>(tempAllOffers.ToList());

            GeosApplication.Instance.Logger.Log("Method FillListsFromServices() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// This method is used to create Indicators
        /// </summary>
        public void CreateDataTableIndicators()
        {
            GeosApplication.Instance.Logger.Log("Method CreateDataTableIndicators ...", category: Category.Info, priority: Priority.Low);
            try
            {
                DataTable dtTableOpen = new DataTable();

                string yearStarDate = GeosApplication.Instance.SelectedyearStarDate.ToShortDateString();
                string yearEndDate = GeosApplication.Instance.SelectedyearEndDate.ToShortDateString();
                PeriodString = yearStarDate + "-" + yearEndDate;
                int currentMonth = GeosApplication.Instance.ServerDateTime.Month;
                int previousMonth = currentMonth - 1;
                CurrentMonthString = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(currentMonth);

                if (previousMonth > 0)
                    PreviousMonthString = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(previousMonth);

                dtTableOpen.Columns.Add("Name", typeof(string)).DefaultValue = string.Empty;
                dtTableOpen.Columns.Add(PeriodString, typeof(string)).DefaultValue = string.Empty;
                dtTableOpen.Columns.Add(PreviousMonthString, typeof(string)).DefaultValue = string.Empty;
                dtTableOpen.Columns.Add(CurrentMonthString, typeof(string)).DefaultValue = string.Empty;
                dtTableOpen.Columns.Add("Trend(%)", typeof(string)).DefaultValue = string.Empty;
                dtTableOpen.Columns.Add("IsRedGreen", typeof(bool));

                DataTableIndicators = dtTableOpen;

                GeosApplication.Instance.Logger.Log("Method CreateDataTableIndicators() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CreateDataTableIndicators() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillIndicators()
        {
            GeosApplication.Instance.Logger.Log("In method FillIndicators() ...", category: Category.Info, priority: Priority.Low);
            if (GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
            {
                OpenOffers = new ObservableCollection<Offer>(AllOffers.Where(x => x.PartNumber.PartNumbersTracking.EndDate == null).ToList().OrderBy(s => s.DeliveryDate));
                List<string> listIndicator = new List<string> { "New Analysis", "Closed Analysis", "Close Avg(d)" };
                int i = 0;
                int currentMonth = GeosApplication.Instance.ServerDateTime.Month;

                foreach (var item in listIndicator)
                {
                    try
                    {
                        DataRow dr = DataTableIndicators.NewRow();
                        DataRow previousmonth = DataTableEnggAnalysisByMonth.AsEnumerable().FirstOrDefault(row => Convert.ToInt16(row["MonthNo"]) == currentMonth - 1);
                        DataRow currentmonth = DataTableEnggAnalysisByMonth.AsEnumerable().FirstOrDefault(row => Convert.ToInt16(row["MonthNo"]) == currentMonth);

                        dr["Name"] = listIndicator[i];
                        if (Convert.ToString(dr["Name"]) == "New Analysis")
                        {
                            double Open = DataTableEnggAnalysisByMonth.AsEnumerable().Sum(x => x.Field<double>("EngineeringAnalysisOpen"));
                            dr[PeriodString] = Open;
                            dr[CurrentMonthString] = currentmonth[2];

                            double previous = 0;

                            if (previousmonth != null)
                            {
                                dr[PreviousMonthString] = previousmonth[2];
                                previous = Convert.ToInt16(previousmonth[2]);
                            }

                            double current = Convert.ToInt16(currentmonth[2]);

                            int percentage = 0;

                            if (previous > 0)
                            {
                                percentage = Math.Abs(Convert.ToInt32(((current - previous) / previous) * 100));
                                dr["Trend(%)"] = percentage + "%";

                                if (current == previous)
                                    ;
                                else if (current > previous)
                                    dr["IsRedGreen"] = true;
                                else
                                    dr["IsRedGreen"] = false;
                            }
                            else
                            {
                                if (current == 0)
                                    dr["Trend(%)"] = 0 + "%";

                                else if (current > 0)
                                {
                                    dr["IsRedGreen"] = true;
                                    dr["Trend(%)"] = 100 + "%";
                                }
                            }
                        }

                        if (dr["Name"] == "Closed Analysis")
                        {
                            double Closed = DataTableEnggAnalysisByMonth.AsEnumerable().Sum(x => x.Field<double>("EngineeringAnalysisClosed"));
                            dr[PeriodString] = Closed;
                            dr[CurrentMonthString] = currentmonth[3];
                            double previous = 0;

                            if (previousmonth != null)
                            {
                                dr[PreviousMonthString] = previousmonth[3];
                                previous = Convert.ToInt16(previousmonth[3]);
                            }

                            double current = Convert.ToInt16(currentmonth[3]);

                            int percentage = 0;

                            if (previous > 0)
                            {
                                percentage = Math.Abs(Convert.ToInt32(((current - previous) / previous) * 100));
                                dr["Trend(%)"] = percentage + "%";

                                if (current == previous)
                                    ;
                                else if (current > previous)
                                    dr["IsRedGreen"] = true;
                                else
                                    dr["IsRedGreen"] = false;
                            }
                            else
                            {
                                if (current == 0)
                                    dr["Trend(%)"] = 0 + "%";

                                else if (current > 0)
                                {
                                    dr["IsRedGreen"] = true;
                                    dr["Trend(%)"] = 100 + "%";
                                }
                            }
                        }

                        if (Convert.ToString(dr["Name"]) == "Close Avg(d)")
                        {
                            double Average = DataTableEnggAnalysisByMonth.AsEnumerable().Sum(x => x.Field<double>("EngineeringAnalysisClosedAverage"));
                            dr[PeriodString] = Average;
                            dr[CurrentMonthString] = currentmonth[4];

                            double previous = 0;

                            if (previousmonth != null)
                            {
                                dr[PreviousMonthString] = previousmonth[4];
                                previous = Convert.ToInt16(previousmonth[4]);
                            }

                            double current = Convert.ToInt16(currentmonth[4]);

                            int percentage = 0;

                            if (previous > 0)
                            {
                                percentage = Math.Abs(Convert.ToInt32(((current - previous) / previous) * 100));
                                dr["Trend(%)"] = percentage + "%";

                                if (current == previous)
                                    ;
                                else if (current > previous)
                                    dr["IsRedGreen"] = true;
                                else
                                    dr["IsRedGreen"] = false;
                            }
                            else
                            {
                                if (current == 0)
                                    dr["Trend(%)"] = 0 + "%";

                                else if (current > 0)
                                {
                                    dr["IsRedGreen"] = true;
                                    dr["Trend(%)"] = 100 + "%";
                                }
                            }
                        }

                        i++;
                        DataTableIndicators.Rows.Add(dr);

                        GeosApplication.Instance.Logger.Log("Method FillIndicators() executed successfully", category: Category.Info, priority: Priority.Low);
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log("Get an error in FillIndicators() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                    }
                }
            }
            else
            {
                DataTableIndicators.Rows.Clear();
                GeosApplication.Instance.Logger.Log("In method FillIndicators() executed successfully ...", category: Category.Info, priority: Priority.Low);
            }
        }

        private void CreateDataTableOpenClosedOffers()
        {
            GeosApplication.Instance.Logger.Log("Method CreateDataTableOpenClosedOffers ...", category: Category.Info, priority: Priority.Low);

            //Fill Data for Enggineering Analysis By Month
            try
            {
                DataTable dtTableOpen = new DataTable();

                dtTableOpen.Columns.Add("IdOffer", typeof(Int64)).DefaultValue = 0;
                dtTableOpen.Columns.Add("ConnectPlantId", typeof(string)).DefaultValue = string.Empty;
                dtTableOpen.Columns.Add("OfferCode", typeof(string)).DefaultValue = string.Empty;
                dtTableOpen.Columns.Add("Description", typeof(string)).DefaultValue = string.Empty;
                dtTableOpen.Columns.Add("BusinessUnit", typeof(string)).DefaultValue = string.Empty;
                dtTableOpen.Columns.Add("OfferStatus", typeof(string)).DefaultValue = string.Empty;
                dtTableOpen.Columns.Add("RequestDate", typeof(DateTime));
                dtTableOpen.Columns.Add("DeliveryDate", typeof(DateTime));
                dtTableOpen.Columns.Add("Assigned", typeof(string)).DefaultValue = string.Empty;
                dtTableOpen.Columns.Add("HtmlColor", typeof(string));
                dtTableOpen.Columns.Add("IsOpenDeliveryDateRedColor", typeof(bool));

                DataTableOpenOffers = dtTableOpen;

                DataTable dtTableClosed = new DataTable();

                dtTableClosed.Columns.Add("IdOffer", typeof(Int64)).DefaultValue = 0;
                dtTableClosed.Columns.Add("ConnectPlantId", typeof(string)).DefaultValue = string.Empty;
                dtTableClosed.Columns.Add("OfferCode", typeof(string)).DefaultValue = string.Empty;
                dtTableClosed.Columns.Add("Description", typeof(string)).DefaultValue = string.Empty;
                dtTableClosed.Columns.Add("BusinessUnit", typeof(string)).DefaultValue = string.Empty;
                dtTableClosed.Columns.Add("OfferStatus", typeof(string)).DefaultValue = string.Empty;
                dtTableClosed.Columns.Add("RequestDate", typeof(DateTime));
                dtTableClosed.Columns.Add("DeliveryDate", typeof(DateTime));
                dtTableClosed.Columns.Add("CloseDate", typeof(DateTime));
                dtTableClosed.Columns.Add("Assigned", typeof(string)).DefaultValue = string.Empty;
                dtTableClosed.Columns.Add("HtmlColor", typeof(string));
                dtTableClosed.Columns.Add("WeekNo", typeof(Int32));
                dtTableClosed.Columns.Add("IsClosedDeliveryDateRedColor", typeof(bool));
                DataTableClosedOffers = dtTableClosed;

                GeosApplication.Instance.Logger.Log("Method CreateDataTableOpenClosedOffers() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CreateDataTableOpenClosedOffers() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        /// Fill open and closed Services
        /// </summary>
        private void FillOpenClosedEnggAnalysis()
        {
            GeosApplication.Instance.Logger.Log("In method FillOpenClosedEnggAnalysis() ...", category: Category.Info, priority: Priority.Low);

            // Engg analysis all open offers
            OpenOffers = new ObservableCollection<Offer>(AllOffers.Where(x => x.PartNumber.PartNumbersTracking.EndDate == null).ToList().OrderBy(s => s.DeliveryDate));

            foreach (Offer openOffer in OpenOffers)
            {
                try
                {
                    DataRow dr = DataTableOpenOffers.NewRow();

                    dr["IdOffer"] = openOffer.IdOffer;
                    dr["ConnectPlantId"] = openOffer.Site.ConnectPlantId;
                    dr["OfferCode"] = openOffer.Code;
                    dr["Description"] = openOffer.Description;
                    dr["BusinessUnit"] = openOffer.BusinessUnit != null ? openOffer.BusinessUnit.Value : string.Empty;
                    dr["OfferStatus"] = openOffer.GeosStatus != null ? openOffer.GeosStatus.Name : string.Empty;
                    dr["RequestDate"] = openOffer.PartNumber.PartNumbersTracking.StartDate;
                    dr["DeliveryDate"] = openOffer.DeliveryDate;
                    // dr["Assigned"] = openOffer.PartNumber.PartNumbersTracking.StageOperator.FullName;
                    dr["Assigned"] = (openOffer.TaskAssigned.Surname != null && openOffer.TaskAssigned.Name != null) ? openOffer.TaskAssigned.FullName : string.Empty;
                    dr["HtmlColor"] = openOffer.GeosStatus.HtmlColor;

                    if (GeosApplication.Instance.ServerDateTime.Date > openOffer.DeliveryDate)
                    {
                        dr["IsOpenDeliveryDateRedColor"] = true;
                    }
                    else
                    {
                        dr["IsOpenDeliveryDateRedColor"] = false;
                    }

                    DataTableOpenOffers.Rows.Add(dr);
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in FillOpenClosedEnggAnalysis() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                }

            }

            // Engg analysis all closed offers
            ClosedOffers = new ObservableCollection<Offer>(AllOffers.Where(x => x.PartNumber.PartNumbersTracking.EndDate != null).ToList().OrderBy(s => s.DeliveryDate));

            foreach (Offer closedOffer in ClosedOffers)
            {
                try
                {
                    DataRow dr = DataTableClosedOffers.NewRow();

                    dr["IdOffer"] = closedOffer.IdOffer;
                    dr["ConnectPlantId"] = closedOffer.Site.ConnectPlantId;
                    dr["OfferCode"] = closedOffer.Code;
                    dr["Description"] = closedOffer.Description;
                    dr["BusinessUnit"] = closedOffer.BusinessUnit != null ? closedOffer.BusinessUnit.Value : string.Empty;
                    dr["OfferStatus"] = closedOffer.GeosStatus != null ? closedOffer.GeosStatus.Name : string.Empty;
                    dr["RequestDate"] = closedOffer.PartNumber.PartNumbersTracking.StartDate;
                    dr["DeliveryDate"] = closedOffer.DeliveryDate;
                    dr["CloseDate"] = closedOffer.PartNumber.PartNumbersTracking.EndDate;
                    dr["Assigned"] = (closedOffer.PartNumber.PartNumbersTracking.StageOperator.Surname != null && closedOffer.PartNumber.PartNumbersTracking.StageOperator.Name != null) ? closedOffer.PartNumber.PartNumbersTracking.StageOperator.FullName : string.Empty;
                    dr["HtmlColor"] = closedOffer.GeosStatus.HtmlColor;

                    //var currentCulture = System.Globalization.CultureInfo.CurrentCulture;
                    //var weekNo = currentCulture.Calendar.GetWeekOfYear(
                    //                Convert.ToDateTime(dr["DeliveryDate"].ToString()),
                    //                currentCulture.DateTimeFormat.CalendarWeekRule,
                    //                currentCulture.DateTimeFormat.FirstDayOfWeek).ToString("00");

                    //Sprint36 Requirement display Year+Week------Swapnali [11-04-2018]
                    if (Convert.ToInt32(GeosApplication.Instance.UserSettings["CustomPeriodOption"]) == 0)
                    {
                        dr["WeekNo"] = Convert.ToInt32((CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(Convert.ToDateTime(dr["DeliveryDate"].ToString()), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday)).ToString("00"));
                    }

                    else
                    {
                        dr["WeekNo"] = Convert.ToInt32(Convert.ToString(CultureInfo.CurrentCulture.Calendar.GetYear(Convert.ToDateTime(dr["DeliveryDate"].ToString()))) + (CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(Convert.ToDateTime(dr["DeliveryDate"].ToString()), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday)).ToString("00"));
                    }

                    //dr["WeekNo"] = weekNo;
                    // if (GeosApplication.Instance.ServerDateTime.Date > closedOffer.DeliveryDate)

                    if (closedOffer.PartNumber.PartNumbersTracking.EndDate > closedOffer.DeliveryDate)
                    {
                        dr["IsClosedDeliveryDateRedColor"] = true;
                    }
                    else
                    {
                        dr["IsClosedDeliveryDateRedColor"] = false;
                    }
                    DataTableClosedOffers.Rows.Add(dr);
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in FillOpenClosedEnggAnalysis() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                }
            }

            if (DataTableOpenOffers.Rows.Count > 0)
            {
                OpenOffersCount = DataTableOpenOffers.Rows.Count;
                OpenOffersText = System.Windows.Application.Current.FindResource("DbEnggAnalysisOpen").ToString() + " " + "(" + OpenOffersCount + ")";
            }
            else
                OpenOffersText = System.Windows.Application.Current.FindResource("DbEnggAnalysisOpen").ToString();
            if (DataTableClosedOffers.Rows.Count > 0)
            {
                ClosedOffersCount = DataTableClosedOffers.Rows.Count;
                ClosedOffersText = System.Windows.Application.Current.FindResource("DbEnggAnalysisClosed").ToString() + " " + "(" + ClosedOffersCount + ")";
            }
            else
                ClosedOffersText = System.Windows.Application.Current.FindResource("DbEnggAnalysisClosed").ToString();

            GeosApplication.Instance.Logger.Log("Method FillOpenClosedEnggAnalysis() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Fill graph as per open and closed offers.
        /// </summary>
        private void FillOpenClosedEnggAnalysisGraph()
        {
            GeosApplication.Instance.Logger.Log("Method FillOpenClosedEnggAnalysisGraph ...", category: Category.Info, priority: Priority.Low);

            try
            {
                DataTable dtEnAnalysis = DataTableEnggAnalysisByMonth.Copy();
                ObservableCollection<Offer> ClosedOffersDaystemp;
                foreach (DataRow item in dtEnAnalysis.Rows)
                {
                    item["EngineeringAnalysisClosed"] = ClosedOffers.Where(x => x.PartNumber.PartNumbersTracking.EndDate.Value.Month == Convert.ToInt16(item["MonthNo"]) && x.PartNumber.PartNumbersTracking.EndDate.Value.Year == Convert.ToInt16(item["Year"])).ToList().Count();
                    item["EngineeringAnalysisOpen"] = OpenOffers.Where(x => x.PartNumber.PartNumbersTracking.StartDate.Value.Month == Convert.ToInt16(item["MonthNo"]) && x.PartNumber.PartNumbersTracking.StartDate.Value.Year == Convert.ToInt16(item["Year"])).ToList().Count();

                    int sum = 0;
                    ClosedOffersDaystemp = new ObservableCollection<Offer>(ClosedOffers.Where(x => x.PartNumber.PartNumbersTracking.EndDate.Value.Month == Convert.ToInt16(item["MonthNo"]) && x.PartNumber.PartNumbersTracking.EndDate.Value.Year == Convert.ToInt16(item["Year"])));

                    foreach (var closedOffers in ClosedOffersDaystemp)
                    {
                        var numberOfDays = (closedOffers.PartNumber.PartNumbersTracking.EndDate.Value - closedOffers.PartNumber.PartNumbersTracking.StartDate.Value).TotalDays;
                        sum = sum + Convert.ToInt16(numberOfDays);
                    }

                    if (ClosedOffersDaystemp.Count > 0)
                        item["EngineeringAnalysisClosedAverage"] = sum / ClosedOffersDaystemp.Count;
                }

                DataTableEnggAnalysisByMonth = new DataTable();
                DataTableEnggAnalysisByMonth = dtEnAnalysis;

                GeosApplication.Instance.Logger.Log("Method FillOpenClosedEnggAnalysisGraph() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillOpenClosedEnggAnalysisGraph() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// This method is used to get data by PlantOwner 
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

                FillDashboardEngineeringAnalysisByPlant();

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
                CreateDataTableEnggineeringAnalysisByMonth();
                OpenOffers = new ObservableCollection<Offer>();
                DataTableOpenOffers.Rows.Clear();
                ClosedOffers = new ObservableCollection<Offer>();
                DataTableClosedOffers.Rows.Clear();
                DataTableIndicators.Rows.Clear();
                FailedPlants.Clear();
                IsShowFailedPlantWarning = false;
                WarningFailedPlants = String.Empty;
            }

            GeosApplication.Instance.Logger.Log("Method PlantOwnerPopupClosedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
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

            if (GeosApplication.Instance.SelectedSalesOwnerUsersList != null && GeosApplication.Instance.SelectedSalesOwnerUsersList.Count > 0)
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

                FillDashboardEngineeringAnalysisByPlant();
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
                AllOffers = new ObservableCollection<Offer>();
                FailedPlants.Clear();
                IsShowFailedPlantWarning = false;
                WarningFailedPlants = String.Empty;
            }
        }

        /// <summary>
        /// Method for refresh DASHBOARD From database by refresh Button.
        /// </summary>
        /// <param name="obj"></param>
        private void RefreshEngineeringAnalysisDashboard(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method RefreshEngineeringAnalysisDashboard ...", category: Category.Info, priority: Priority.Low);

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

            FillDashboardEngineeringAnalysisByPlant();

            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

            GeosApplication.Instance.Logger.Log("Method RefreshEngineeringAnalysisDashboard() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// This method is used to load Engg analysis by month.
        /// </summary>
        /// <param name="obj"></param>
        private void ChartEngineeringAnalysisByMonthLoadCommandAction(RoutedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChartEngineeringAnalysisByMonthLoadCommandAction ...", category: Category.Info, priority: Priority.Low);

                ChartControl chartControl = (ChartControl)(obj.OriginalSource);
                chartControl.BeginInit();
                XYDiagram2D diagram = new XYDiagram2D();
                chartControl.Diagram = diagram;
                diagram.ActualAxisX.Title = new AxisTitle() { Content = "Months" };
                diagram.ActualAxisY.Title = new AxisTitle() { Content = "Offers" };
               
              

                GeosApplication.SetCurrencySymbol(GeosApplication.Instance.CurrentCurrencySymbol);
                diagram.ActualAxisY.Label = new AxisLabel();
                diagram.ActualAxisX.Label = new AxisLabel();
                
                diagram.ActualAxisX.QualitativeScaleOptions = new QualitativeScaleOptions();
                diagram.ActualAxisX.QualitativeScaleOptions.AutoGrid = false;

                LineSeries2D lineENAnalysisOpen = new LineSeries2D();
                lineENAnalysisOpen.LineStyle = new LineStyle(3);
                lineENAnalysisOpen.ArgumentScaleType = ScaleType.Auto;
                lineENAnalysisOpen.ValueScaleType = ScaleType.Numerical;
                lineENAnalysisOpen.DisplayName = "Open";
                lineENAnalysisOpen.ArgumentDataMember = "MonthName";
                lineENAnalysisOpen.ValueDataMember = "EngineeringAnalysisOpen";
                chartControl.Diagram.Series.Add(lineENAnalysisOpen);

                LineSeries2D lineENAnalysisClosed = new LineSeries2D();
                lineENAnalysisClosed.LineStyle = new LineStyle(3);
                lineENAnalysisClosed.ArgumentScaleType = ScaleType.Auto;
                lineENAnalysisClosed.ValueScaleType = ScaleType.Numerical;
                lineENAnalysisClosed.DisplayName = "Closed";
                lineENAnalysisClosed.ArgumentDataMember = "MonthName";
                lineENAnalysisClosed.ValueDataMember = "EngineeringAnalysisClosed";
                chartControl.Diagram.Series.Add(lineENAnalysisClosed);


                diagram.SecondaryAxesY.Clear();
                SecondaryAxisY2D Yaxis = new SecondaryAxisY2D();
                Yaxis.Title = new AxisTitle() { Content = "Close Avg(d)" };
                Yaxis.NumericScaleOptions = new ContinuousNumericScaleOptions();
                Yaxis.NumericScaleOptions.AutoGrid = false;
                Yaxis.Label = new AxisLabel();
                diagram.SecondaryAxesY.Add(Yaxis);
               
                LineSeries2D lineENAnalysisClosedAverage = new LineSeries2D();
                lineENAnalysisClosedAverage.LineStyle = new LineStyle(3);
                lineENAnalysisClosedAverage.ArgumentScaleType = ScaleType.Auto;
                lineENAnalysisClosedAverage.ValueScaleType = ScaleType.Numerical;
                lineENAnalysisClosedAverage.DisplayName = "Close Avg(d)";
                SolidColorBrush blueBrush = new SolidColorBrush();
                blueBrush.Color = Color.FromRgb(255, 201, 14);
                lineENAnalysisClosedAverage.Brush = blueBrush;
                lineENAnalysisClosedAverage.ArgumentDataMember = "MonthName";
                lineENAnalysisClosedAverage.ValueDataMember = "EngineeringAnalysisClosedAverage";
                lineENAnalysisClosedAverage.AxisY = Yaxis;
                chartControl.Diagram.Series.Add(lineENAnalysisClosedAverage);

                chartControl.AnimationMode = ChartAnimationMode.OnDataChanged;
                chartControl.Animate();
                chartControl.EndInit();

                GeosApplication.Instance.Logger.Log("Method ChartEngineeringAnalysisByMonthLoadCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ChartEngineeringAnalysisByMonthLoadCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ChartEngineeringAnalysisByMonthLoadCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ChartEngineeringAnalysisByMonthLoadCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        ///// <summary>
        ///// Display week number by date in Closed Engineering Analysis.
        ///// </summary>
        ///// <param name="obj"></param>
        //public void WeekUnboundDataCommandAction(object obj)
        //{
        //    if (obj == null) return;

        //    GridColumnDataEventArgs e = obj as GridColumnDataEventArgs;
        //    if (e.IsGetData)
        //    {
        //        var price = e.GetListSourceFieldValue("DeliveryDate");

        //        var currentCulture = System.Globalization.CultureInfo.CurrentCulture;
        //        e.Value = currentCulture.Calendar.GetWeekOfYear(
        //                    Convert.ToDateTime(price),
        //                    currentCulture.DateTimeFormat.CalendarWeekRule,
        //                    currentCulture.DateTimeFormat.FirstDayOfWeek).ToString("00");
        //    }
        //}

        /// <summary>
        /// Display week number by date in Closed Engineering Analysis.
        /// </summary>
        /// <param name="obj"></param>
        public void DelayDaysOpenUnboundDataCommandAction(object obj)
        {
            if (obj == null) return;

            GridColumnDataEventArgs e = obj as GridColumnDataEventArgs;
            if (e.IsGetData)
            {
                var date = e.GetListSourceFieldValue("DeliveryDate");
                e.Value = GeosApplication.Instance.ServerDateTime.Subtract(Convert.ToDateTime(date)).Negate().Days;
            }
        }

        /// <summary>
        /// Display week number and Delay days by date in Closed Engineering Analysis.
        /// </summary>
        /// <param name="obj"></param>
        public void WeekandDelayDaysClosedUnboundDataCommandAction(object obj)
        {
            if (obj == null) return;

            GridColumnDataEventArgs e = obj as GridColumnDataEventArgs;
            if (e.Column.FieldName == "Delay(d)" && e.IsGetData)
            {
                DateTime deliverydate = Convert.ToDateTime(e.GetListSourceFieldValue("DeliveryDate"));
                DateTime closedate = Convert.ToDateTime(e.GetListSourceFieldValue("CloseDate"));
                e.Value = deliverydate.Date.Subtract(Convert.ToDateTime(closedate.Date)).Days;
            }

            if (e.Column.FieldName == "Week" && e.IsGetData)
            {
                var price = e.GetListSourceFieldValue("DeliveryDate");

                var currentCulture = System.Globalization.CultureInfo.CurrentCulture;
                e.Value = currentCulture.Calendar.GetWeekOfYear(
                            Convert.ToDateTime(price),
                            currentCulture.DateTimeFormat.CalendarWeekRule,
                            currentCulture.DateTimeFormat.FirstDayOfWeek).ToString("00");
            }
        }

        /// <summary>
        /// This method is used to show offer after double click 
        /// [001][SP-67][skale][16-07-2019][GEOS2-1641]fechas vencidas en sistema CRM
        /// </summary>
        /// <param name="obj"></param>
        private void OfferGridDoubleClickCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OfferGridDoubleClickCommandAction...", category: Category.Info, priority: Priority.Low);

                if (obj == null) return;

                DataRowView dataRow = obj as DataRowView;
                long offerId = Convert.ToInt64(dataRow["IdOffer"].ToString());
                Int32 connectPlantId = Convert.ToInt32(dataRow["ConnectPlantId"].ToString());

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

                IList<Offer> TempLeadsList = new List<Offer>();
                //[001] added Change Method
                TempLeadsList.Add(CrmStartUp.GetOfferDetailsById_V2037(offerId, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, GeosApplication.Instance.CompanyList.Where(cmp => cmp.ConnectPlantId == connectPlantId.ToString()).FirstOrDefault()));

                LostReasonsByOffer lostReasonsByOffer = CrmStartUp.GetLostReasonsByOffer(offerId, GeosApplication.Instance.CompanyList.Where(cmp => cmp.ConnectPlantId == connectPlantId.ToString()).Select(x => x.ConnectPlantConstr).FirstOrDefault());
                bool IsPurchaseOrderDone = CrmStartUp.IsPurchaseOrderDoneByIdOffer(offerId, GeosApplication.Instance.CompanyList.Where(cmp => cmp.ConnectPlantId == connectPlantId.ToString()).FirstOrDefault());

                if (lostReasonsByOffer != null)
                {
                    TempLeadsList[0].LostReasonsByOffer = lostReasonsByOffer;
                }

                if (IsPurchaseOrderDone)    // true - Order
                {
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

                    orderEditView.ShowDialogWindow();

                    if (orderEditViewModel.OfferData != null)
                    {
                        dataRow["OfferCode"] = orderEditViewModel.OfferData.Code;
                        dataRow["Description"] = orderEditViewModel.OfferData.Description;

                        if (orderEditViewModel.OfferData.BusinessUnit != null)
                        {
                            dataRow["BusinessUnit"] = orderEditViewModel.OfferData.BusinessUnit.Value;
                            //openOffer.BusinessUnit.IdLookupValue = orderEditViewModel.OfferData.BusinessUnit.IdLookupValue;
                            //openOffer.BusinessUnit.Value = orderEditViewModel.OfferData.BusinessUnit.Value;
                        }

                        if (orderEditViewModel.OfferData.GeosStatus != null)
                        {
                            dataRow["OfferStatus"] = orderEditViewModel.OfferData.GeosStatus.Name;

                            //openOffer.GeosStatus.IdOfferStatusType = orderEditViewModel.OfferData.GeosStatus.IdOfferStatusType;
                            //openOffer.GeosStatus.Name = orderEditViewModel.OfferData.GeosStatus.Name;
                            //openOffer.GeosStatus.Position = orderEditViewModel.OfferData.GeosStatus.Position;

                            //openOffer.GeosStatus.IdSalesStatusType = orderEditViewModel.OfferData.GeosStatus.IdSalesStatusType;
                            //openOffer.GeosStatus.SalesStatusType = orderEditViewModel.OfferData.GeosStatus.SalesStatusType;
                        }
                    }
                }
                else // false - Offer
                {
                    LeadsEditViewModel leadsEditViewModel = new LeadsEditViewModel();
                    LeadsEditView leadsEditView = new LeadsEditView();

                    if (GeosApplication.Instance.IsPermissionReadOnly)
                    {
                        leadsEditViewModel.IsControlEnable = true;
                        leadsEditViewModel.IsAcceptControlEnableorder = false;
                        //leadsEditViewModel.IsControlEnableorder = false;
                        leadsEditViewModel.IsStatusChangeAction = true;

                        //[CRM-M040-02] (#49016) Validate Eng. Analysis 
                        //If user has engineering permission then he can edit eng analysis IsCompleted not other fields and save
                        leadsEditViewModel.IsAcceptEnable = false;
                        if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(x => x.IdPermission == 27))
                        {
                            leadsEditViewModel.IsAcceptEnable = true;
                        }

                        leadsEditViewModel.InItLeadsEditReadonly(TempLeadsList);
                    }
                    else
                    {
                        leadsEditViewModel.ForLeadOpen = true;
                        leadsEditViewModel.InIt(TempLeadsList);
                    }

                    EventHandler handle = delegate { leadsEditView.Close(); };
                    leadsEditViewModel.RequestClose += handle;

                    leadsEditView.DataContext = leadsEditViewModel;

                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                    leadsEditView.ShowDialogWindow();

                    if (leadsEditViewModel.OfferData != null)
                    {
                        dataRow["OfferCode"] = leadsEditViewModel.OfferData.Code;
                        dataRow["Description"] = leadsEditViewModel.OfferData.Description;

                        if (leadsEditViewModel.OfferData.BusinessUnit != null)
                        {
                            dataRow["BusinessUnit"] = leadsEditViewModel.OfferData.BusinessUnit.Value;
                            //openOffer.BusinessUnit.IdLookupValue = leadsEditViewModel.OfferData.BusinessUnit.IdLookupValue;
                            //openOffer.BusinessUnit.Value = leadsEditViewModel.OfferData.BusinessUnit.Value;
                        }

                        if (leadsEditViewModel.OfferData.GeosStatus != null)
                        {
                            dataRow["OfferStatus"] = leadsEditViewModel.OfferData.GeosStatus.Name;
                            dataRow["HtmlColor"] = leadsEditViewModel.OfferData.GeosStatus.HtmlColor;
                            //openOffer.GeosStatus.IdOfferStatusType = leadsEditViewModel.OfferData.GeosStatus.IdOfferStatusType;
                            //openOffer.GeosStatus.Name = leadsEditViewModel.OfferData.GeosStatus.Name;
                            //openOffer.GeosStatus.Position = leadsEditViewModel.OfferData.GeosStatus.Position;
                            //openOffer.GeosStatus.IdSalesStatusType = leadsEditViewModel.OfferData.GeosStatus.IdSalesStatusType;
                            //openOffer.GeosStatus.SalesStatusType = leadsEditViewModel.OfferData.GeosStatus.SalesStatusType;
                        }
                    }
                }

                GeosApplication.Instance.Logger.Log("Method OfferGridDoubleClickCommandAction executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OfferGridDoubleClickCommandAction() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OfferGridDoubleClickCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OfferGridDoubleClickCommandAction() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion  //Methods

        #region Engineering Analysis Export

        private void ExportOpenClosedEngineeringAnalysisAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportOpenClosedEngineeringAnalysisAction ...", category: Category.Info, priority: Priority.Low);

                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Engineering Analysis";
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

                    Workbook workbook = new Workbook();
                    workbook.Worksheets.Insert(0, System.Windows.Application.Current.FindResource("CrmMainViewModelDashboardEngineeringAnalysis").ToString());
                    Worksheet ws = workbook.Worksheets[System.Windows.Application.Current.FindResource("CrmMainViewModelDashboardEngineeringAnalysis").ToString()];

                    //Open Engineering Analysis

                    int rowOffset = 0;
                    string rangeOpenHeader = "A" + (rowOffset + 1) + ":H" + (rowOffset + 1);
                    ws.Range[rangeOpenHeader].Font.Bold = true;
                    ws.Range[rangeOpenHeader].Fill.BackgroundColor = System.Drawing.Color.LightGray;

                    //Engineering Analysis Open Header
                    ws.Cells[rowOffset, 0].Value = System.Windows.Application.Current.FindResource("DbEnggAnalysisOpen").ToString();
                    ws.Cells[rowOffset, 0].ColumnWidth = 350;
                    ws.Cells[rowOffset, 0].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                    ws.Cells[rowOffset, 0].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    ws.MergeCells(ws.Range[rangeOpenHeader]);

                    rowOffset = rowOffset + 2;
                    string rangeOpenColumns = "A" + (rowOffset + 1) + ":H" + (rowOffset + 1);
                    ws.Range[rangeOpenColumns].Font.Bold = true;
                    ws.Range[rangeOpenColumns].Fill.BackgroundColor = System.Drawing.Color.LightGray;

                    //Offer
                    ws.Cells[rowOffset, 0].Value = System.Windows.Application.Current.FindResource("DbEnggAnalysisOffer").ToString();
                    ws.Cells[rowOffset, 0].ColumnWidth = 350;

                    //Description
                    ws.Cells[rowOffset, 1].Value = System.Windows.Application.Current.FindResource("DbEnggAnalysisDescription").ToString();
                    ws.Cells[rowOffset, 1].ColumnWidth = 700;

                    //Business Unit
                    ws.Cells[rowOffset, 2].Value = System.Windows.Application.Current.FindResource("DbEnggAnalysisBU").ToString();
                    ws.Cells[rowOffset, 2].ColumnWidth = 300;

                    //Offer Status
                    ws.Cells[rowOffset, 3].Value = System.Windows.Application.Current.FindResource("DbEnggAnalysisOfferStatus").ToString();
                    ws.Cells[rowOffset, 3].ColumnWidth = 440;

                    //Request Date
                    ws.Cells[rowOffset, 4].Value = System.Windows.Application.Current.FindResource("DbEnggAnalysisRequestDate").ToString();
                    ws.Cells[rowOffset, 4].ColumnWidth = 300;

                    //Delivery Date
                    ws.Cells[rowOffset, 5].Value = System.Windows.Application.Current.FindResource("DbEnggAnalysisDeliveryDate").ToString();
                    ws.Cells[rowOffset, 5].ColumnWidth = 300;

                    //Delay(d)
                    ws.Cells[rowOffset, 6].Value = System.Windows.Application.Current.FindResource("DbEnggAnalysisDelay").ToString();
                    ws.Cells[rowOffset, 6].ColumnWidth = 300;

                    //Assigned
                    ws.Cells[rowOffset, 7].Value = System.Windows.Application.Current.FindResource("DbEnggAnalysisAssigned").ToString();
                    ws.Cells[rowOffset, 7].ColumnWidth = 400;

                    rowOffset = rowOffset + 1;

                    string openAllThinBorder = "A" + (rowOffset);
                    foreach (DataRow openOffer in DataTableOpenOffers.Rows)
                    {
                        ws.Cells[rowOffset, 0].Value = Convert.ToString(openOffer["OfferCode"]);                 // openOffer.Code;
                        ws.Cells[rowOffset, 1].Value = Convert.ToString(openOffer["Description"]);    //openOffer.Description;
                        ws.Cells[rowOffset, 2].Value = Convert.ToString(openOffer["BusinessUnit"]);   // (openOffer.BusinessUnit != null) ? openOffer.BusinessUnit.Value : string.Empty;
                        ws.Cells[rowOffset, 3].Value = Convert.ToString(openOffer["OfferStatus"]);    // openOffer.GeosStatus.Name;
                        ws.Cells[rowOffset, 4].Value = Convert.ToDateTime(openOffer["RequestDate"].ToString());  // openOffer.PartNumber.PartNumbersTracking.StartDate;
                        ws.Cells[rowOffset, 5].Value = Convert.ToDateTime(openOffer["DeliveryDate"].ToString()); //openOffer.DeliveryDate;
                        ws.Cells[rowOffset, 6].Value = GeosApplication.Instance.ServerDateTime.Subtract(Convert.ToDateTime(openOffer["DeliveryDate"])).Negate().Days;
                        ws.Cells[rowOffset, 7].Value = Convert.ToString(openOffer["Assigned"].ToString());       // openOffer.PartNumber.PartNumbersTracking.StageOperator.FullName;

                        rowOffset++;
                    }

                    openAllThinBorder = openAllThinBorder + ":H" + (rowOffset);
                    Formatting rangeOpenFormatting = ws.Range[openAllThinBorder].BeginUpdateFormatting();
                    Borders rangeOpenBorders = rangeOpenFormatting.Borders;
                    rangeOpenBorders.SetAllBorders(System.Drawing.Color.Black, BorderLineStyle.Thin);
                    ws.Range[openAllThinBorder].EndUpdateFormatting(rangeOpenFormatting);

                    rowOffset = rowOffset + 2;

                    //Closed Engineering Analysis

                    string rangeClosedHeader = "A" + (rowOffset + 1) + ":J" + (rowOffset + 1);
                    ws.Range[rangeClosedHeader].Font.Bold = true;
                    ws.Range[rangeClosedHeader].Fill.BackgroundColor = System.Drawing.Color.LightGray;

                    ws.Cells[rowOffset, 0].Value = System.Windows.Application.Current.FindResource("DbEnggAnalysisClosed").ToString();    // Engineering Analysis Closed;
                    ws.Cells[rowOffset, 0].ColumnWidth = 400;
                    ws.Cells[rowOffset, 0].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                    ws.Cells[rowOffset, 0].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    ws.MergeCells(ws.Range[rangeClosedHeader]);

                    rowOffset = rowOffset + 2;

                    string rangeClosedColumns = "A" + (rowOffset + 1) + ":J" + (rowOffset + 1);
                    ws.Range[rangeClosedColumns].Font.Bold = true;
                    ws.Range[rangeClosedColumns].Fill.BackgroundColor = System.Drawing.Color.LightGray;

                    //Offer
                    ws.Cells[rowOffset, 0].Value = System.Windows.Application.Current.FindResource("DbEnggAnalysisOffer").ToString();
                    //ws.Cells[rowOffset, 0].ColumnWidth = 350;

                    //Description
                    ws.Cells[rowOffset, 1].Value = System.Windows.Application.Current.FindResource("DbEnggAnalysisDescription").ToString();
                    //ws.Cells[rowOffset, 1].ColumnWidth = 700;

                    //Business Unit
                    ws.Cells[rowOffset, 2].Value = System.Windows.Application.Current.FindResource("DbEnggAnalysisBU").ToString();
                    //ws.Cells[rowOffset, 2].ColumnWidth = 300;

                    //Offer Status
                    ws.Cells[rowOffset, 3].Value = System.Windows.Application.Current.FindResource("DbEnggAnalysisOfferStatus").ToString();
                    //ws.Cells[rowOffset, 3].ColumnWidth = 440;

                    //Request Date
                    ws.Cells[rowOffset, 4].Value = System.Windows.Application.Current.FindResource("DbEnggAnalysisRequestDate").ToString();
                    //ws.Cells[rowOffset, 4].ColumnWidth = 300;

                    //Delivery Date
                    ws.Cells[rowOffset, 5].Value = System.Windows.Application.Current.FindResource("DbEnggAnalysisDeliveryDate").ToString();
                    //ws.Cells[rowOffset, 5].ColumnWidth = 300;

                    //Delay days
                    ws.Cells[rowOffset, 6].Value = System.Windows.Application.Current.FindResource("DbEnggAnalysisDelay").ToString();
                    //ws.Cells[rowOffset, 5].ColumnWidth = 300;

                    //Close Date
                    ws.Cells[rowOffset, 7].Value = System.Windows.Application.Current.FindResource("DbEnggAnalysisCloseDate").ToString();
                    //ws.Cells[rowOffset, 6].ColumnWidth = 300;

                    //Week
                    ws.Cells[rowOffset, 8].Value = System.Windows.Application.Current.FindResource("DbEnggAnalysisWeek").ToString();
                    //ws.Cells[rowOffset, 7].ColumnWidth = 400;

                    //Assigned
                    ws.Cells[rowOffset, 9].Value = System.Windows.Application.Current.FindResource("DbEnggAnalysisAssigned").ToString();
                    ws.Cells[rowOffset, 9].ColumnWidth = 400;

                    rowOffset = rowOffset + 1;

                    string closedAllThinBorder = "A" + (rowOffset);

                    //DataTable sortedDataTable = new DataTable();
                    //sortedDataTable = DataTableClosedOffers;
                    //sortedDataTable.DefaultView.Sort = "WeekNo desc";
                    //sortedDataTable = sortedDataTable.DefaultView.ToTable();
                    //DataTableClosedOffers = sortedDataTable;

                    DataTableClosedOffers.DefaultView.Sort = "WeekNo desc";
                    DataTableClosedOffers = DataTableClosedOffers.DefaultView.ToTable();


                    foreach (DataRow closedOffer in DataTableClosedOffers.Rows)
                    {
                        ws.Cells[rowOffset, 0].Value = Convert.ToString(closedOffer["OfferCode"]);      // closedOffer.Code;
                        ws.Cells[rowOffset, 1].Value = Convert.ToString(closedOffer["Description"]);    //closedOffer.Description;
                        ws.Cells[rowOffset, 2].Value = Convert.ToString(closedOffer["BusinessUnit"]);     // (closedOffer.BusinessUnit != null) ? closedOffer.BusinessUnit.Value : string.Empty;
                        ws.Cells[rowOffset, 3].Value = Convert.ToString(closedOffer["OfferStatus"]);      // closedOffer.GeosStatus.Name;
                        ws.Cells[rowOffset, 4].Value = Convert.ToDateTime(closedOffer["RequestDate"].ToString()); //closedOffer.PartNumber.PartNumbersTracking.StartDate;
                        ws.Cells[rowOffset, 5].Value = Convert.ToDateTime(closedOffer["DeliveryDate"].ToString());

                        DateTime deliverydate = Convert.ToDateTime(closedOffer["DeliveryDate"].ToString());
                        DateTime closedate = Convert.ToDateTime(closedOffer["CloseDate"].ToString());
                        var delay = closedate.Date.Subtract(Convert.ToDateTime(deliverydate.Date)).Days;

                        //var deliverydate = closedOffer["DeliveryDate"].ToString();
                        //DateTime closedate = Convert.ToDateTime(closedOffer["CloseDate"].ToString());
                        //var delay = closedate.Subtract(Convert.ToDateTime(deliverydate)).Negate().Days;

                        ws.Cells[rowOffset, 6].Value = delay;
                        ws.Cells[rowOffset, 7].Value = Convert.ToDateTime(closedOffer["CloseDate"].ToString());       //closedOffer.PartNumber.PartNumbersTracking. ;

                        //var currentCulture = System.Globalization.CultureInfo.CurrentCulture;
                        //var weekNo = currentCulture.Calendar.GetWeekOfYear(
                        //                Convert.ToDateTime(closedOffer["DeliveryDate"].ToString()),
                        //                currentCulture.DateTimeFormat.CalendarWeekRule,
                        //                currentCulture.DateTimeFormat.FirstDayOfWeek).ToString("00");

                        ws.Cells[rowOffset, 8].Value = Convert.ToInt32(closedOffer["weekNo"]);
                        ws.Cells[rowOffset, 9].Value = Convert.ToString(closedOffer["Assigned"].ToString()); // closedOffer.PartNumber.PartNumbersTracking.StageOperator.FullName;

                        rowOffset++;
                    }

                    closedAllThinBorder = closedAllThinBorder + ":J" + (rowOffset);

                    Formatting rangeClosedFormatting = ws.Range[closedAllThinBorder].BeginUpdateFormatting();
                    Borders rangeClosedBorders = rangeClosedFormatting.Borders;
                    rangeClosedBorders.SetAllBorders(System.Drawing.Color.Black, BorderLineStyle.Thin);
                    ws.Range[closedAllThinBorder].EndUpdateFormatting(rangeClosedFormatting);

                    IsBusy = false;
                    using (FileStream stream = new FileStream(ResultFileName, FileMode.Create, FileAccess.ReadWrite))
                    {
                        workbook.SaveDocument(stream, DocumentFormat.OpenXml);
                    }

                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    CustomMessageBox.Show(string.Format(string.Format(System.Windows.Application.Current.FindResource("DbEnggAnalysisExportedSuccessfully").ToString())), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    System.Diagnostics.Process.Start(ResultFileName);
                }

                GeosApplication.Instance.Logger.Log("Method ExportOpenClosedEngineeringAnalysisAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in ExportOpenClosedEngineeringAnalysisAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion  //Engineering Analysis

    }
}
