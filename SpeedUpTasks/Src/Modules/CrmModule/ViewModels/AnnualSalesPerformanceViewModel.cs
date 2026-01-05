using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Charts;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.TreeList;
using DevExpress.Xpf.LayoutControl;
using DevExpress.Xpf.Scheduler;
using DevExpress.Xpf.Scheduler.Menu;
using DevExpress.Xpf.Scheduler.UI;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.OptimizedClass;
using Emdep.Geos.Modules.Crm.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.Utility.Text;
using Prism.Commands;
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
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Emdep.Geos.Modules.Crm.ViewModels
{
    public class AnnualSalesPerformanceViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable
    {
        #region Services

        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion // Services

        #region Declaration

        private DateTime fromDate;
        private DateTime toDate;

        private string topOfferNumber;
        private int todaysTotalAppointment;

        private double gaugeSuccessRate;
        private double gaugeTargetAchieved;
        private string plannedVisitsLable;
        //private string offerTotal;
        private double offerTotal;

        private Visibility todaysTotalAppointmentVisibility = Visibility.Hidden;

        private bool isRefreshClicked;
        private bool isInit;

        private double remainingAmmount;
        private double tempRemainingAmmount;

        private List<OfferDetail> salesPerformanceList;
        private IList<Offer> tempSalesPerformanceList;
        private List<SalesTargetBySite> salesPerformanceTargetList;

        private DataTable dataTableTopProjects;
        private DataTable dtTopProjects;
        private List<CarProjectDetail> finalListCarProject;

        private IList<Offer> leadsList;
        private List<OptionsByOffer> leadsOptionsByOfferList;
        private List<OfferDetail> salesStatusByMonthList;

        private ChartControl chartControl;
        private DataTable dt = new DataTable();
        private DataTable graphDataTable;
        private List<SalesStatusType> listSalesStatusType = new List<SalesStatusType>();
        private List<SalesStatusType> listAllSalesStatusType = new List<SalesStatusType>();

        private ObservableCollection<AppointmentModel> OffersWithoutPurchaseOrdeList = new ObservableCollection<AppointmentModel>();
        public ObservableCollection<AppointmentModel> ProjectTasksDeliveryList;
        private ObservableCollection<Dictionary<String, Object>> salePerfomanceDictionary;
        private AppointmentLabelCollection labels;
        public List<object> SelectedFilters { get; set; }
        public ObservableCollection<DateTime> SelectedDates { get; set; }
        public DateTime SelectedDates1 { get; set; }
        public Double TargetAmount { get; set; }
        //public List<ModelAppointment> Appointments { get; set; }
        private List<TempAppointment> appointments; // { get; set; }
        private List<TempAppointment> filterAppointments; // { get; set; }
        private double targetAchieved;
        private double successRate;
        private bool isMyValueNegative = false;
        private string tempCurrentCurrencySymbol;
        private string myFilterString;
        private List<string> failedPlants;
        private Boolean isShowFailedPlantWarning;
        private string warningFailedPlants;

        private List<CarProjectDetail> listCarProject;
        int srNoProject = 1;
        private List<DailyEventActivity> activityList;
        private List<object> selectedAppointmentType;
        private List<string> appointmentType;
        private PlannedVisitDetail plannedVisitDetails;
        private ObservableCollection<PieChartValuesPlannedVisit> plannedVisitsSeries;
        private double period;
        private double yTD;
        private string yIDForeground;

        private List<LookupValue> statusList;
        private ObservableCollection<MainStatus> mainStatusList;

        #endregion // Declaration

        #region  public Properties
        public List<LookupValue> GetLookupValues { get; set; }
        /// <summary>
        /// added code for filter the appointment.
        /// </summary>
        public List<object> SelectedAppointmentType
        {
            get
            {
                return selectedAppointmentType;
            }

            set
            {
                selectedAppointmentType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedAppointmentType"));

                List<TempAppointment> TempFilterAppointments = new List<TempAppointment>();

                if (selectedAppointmentType != null && selectedAppointmentType.Count > 0)
                {
                    foreach (var item in selectedAppointmentType)
                    {
                        if (Appointments != null)
                        {
                            List<TempAppointment> tm = Appointments.Where(app => ((string)app.Tag.ToString()).Contains(item.ToString())).Select(ap => ap).ToList();
                            if (tm != null && tm.Count > 0)
                                TempFilterAppointments.AddRange(tm);
                        }
                        // FilterAppointments
                    }
                }
                else
                {
                    TempFilterAppointments = new List<TempAppointment>();
                }

                FilterAppointments = new List<TempAppointment>(TempFilterAppointments);
            }
        }


        public string PlannedVisitsLable
        {
            get { return plannedVisitsLable; }
            set
            {
                plannedVisitsLable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PlannedVisitsLable"));

            }
        }

        public ObservableCollection<PieChartValuesPlannedVisit> PlannedVisitsSeries
        {
            get { return plannedVisitsSeries; }
            set
            {
                plannedVisitsSeries = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PlannedVisitsSeries"));
            }
        }

        public double Period
        {
            get { return period; }
            set
            {
                period = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Period"));
            }
        }
        public double YTD
        {
            get { return yTD; }
            set
            {
                yTD = value;
                OnPropertyChanged(new PropertyChangedEventArgs("YTD"));
            }
        }

        public PlannedVisitDetail PlannedVisitDetails
        {
            get
            {
                return plannedVisitDetails;
            }
            set
            {
                plannedVisitDetails = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PlannedVisitDetails"));

            }

        }


        public List<string> AppointmentType
        {
            get
            {
                return appointmentType;
            }

            set
            {
                appointmentType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AppointmentType"));
            }
        }

        public AppointmentLabelCollection Labels
        {
            get
            {
                return labels;
            }

            set
            {
                labels = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Labels"));
            }
        }

        public List<DailyEventActivity> ActivityList
        {
            get
            {
                return activityList;
            }

            set
            {
                activityList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ActivityList"));
            }
        }

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
        public List<TempAppointment> Appointments
        {
            get { return appointments; }
            set
            {
                appointments = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Appointments"));
            }
        }

        public List<TempAppointment> FilterAppointments
        {
            get { return filterAppointments; }
            set
            {
                filterAppointments = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FilterAppointments"));
            }
        }


        public string TempCurrentCurrencySymbol
        {
            get { return tempCurrentCurrencySymbol; }
            set
            {
                tempCurrentCurrencySymbol = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TempCurrentCurrencySymbol"));
            }
        }

        public bool IsInit
        {
            get { return isInit; }
            set { isInit = value; }
        }

        public bool IsMyValueNegative
        {
            get { return isMyValueNegative; }
            set
            {
                isMyValueNegative = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsMyValueNegative"));
            }
        }

        public DataTable GraphDataTable
        {
            get { return graphDataTable; }
            set { graphDataTable = value; OnPropertyChanged(new PropertyChangedEventArgs("GraphDataTable")); }
        }

        public List<OfferDetail> SalesPerformanceList
        {
            get { return salesPerformanceList; }
            set { salesPerformanceList = value; }
        }

        public IList<Offer> TempSalesPerformanceList
        {
            get { return tempSalesPerformanceList; }
            set { tempSalesPerformanceList = value; }
        }

        public List<OfferDetail> SalesStatusByMonthList
        {
            get { return salesStatusByMonthList; }
            set { salesStatusByMonthList = value; }
        }

        public ObservableCollection<Dictionary<String, Object>> SalePerfomanceDictionary
        {
            get { return salePerfomanceDictionary; }
            set
            {
                salePerfomanceDictionary = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SalePerfomanceDictionary"));
            }
        }

        public double GaugeSuccessRate
        {
            get { return gaugeSuccessRate; }
            set
            {
                SetProperty(ref gaugeSuccessRate, value, () => GaugeSuccessRate);
            }
        }

        public double GaugeTargetAchieved
        {
            get { return gaugeTargetAchieved; }
            set
            {
                SetProperty(ref gaugeTargetAchieved, value, () => GaugeTargetAchieved);
            }
        }

        public List<SalesStatusType> ListSalesStatusType
        {
            get { return listSalesStatusType; }
            set { listSalesStatusType = value; }
        }

        public List<SalesStatusType> ListAllSalesStatusType
        {
            get { return listAllSalesStatusType; }
            set
            {
                listAllSalesStatusType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListAllSalesStatusType"));
            }
        }

        public List<SalesTargetBySite> SalesPerformanceTargetList
        {
            get { return salesPerformanceTargetList; }
            set { salesPerformanceTargetList = value; }
        }

        public DataTable DataTableTopProjects
        {
            get { return dataTableTopProjects; }
            set
            {
                dataTableTopProjects = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DataTableTopProjects"));
            }
        }

        public DataTable DtTopProjects
        {
            get { return dtTopProjects; }
            set
            {
                dtTopProjects = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DtTopProjects"));
            }
        }

        public List<CarProjectDetail> FinalListCarProject
        {
            get { return finalListCarProject; }
            set
            {
                finalListCarProject = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FinalListCarProject"));
            }
        }

        public List<CarProjectDetail> ListCarProject
        {
            get { return listCarProject; }
            set { listCarProject = value; }
        }

        public IList<Offer> LeadsList
        {
            get { return leadsList; }
            set { leadsList = value; }
        }

        public List<OptionsByOffer> LeadsOptionsByOfferList
        {
            get { return leadsOptionsByOfferList; }
            set { leadsOptionsByOfferList = value; OnPropertyChanged(new PropertyChangedEventArgs("LeadsOptionsByOfferList")); }
        }

        public double OfferTotal
        {
            get { return offerTotal; }
            set
            {
                offerTotal = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OfferTotal"));
            }
        }

        public string TopOfferNumber
        {
            get { return topOfferNumber; }
            set
            {
                topOfferNumber = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TopOfferNumber"));
            }
        }

        public int TodaysTotalAppointment
        {
            get { return todaysTotalAppointment; }
            set
            {
                todaysTotalAppointment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TodaysTotalAppointment"));
            }
        }

        public Visibility TodaysTotalAppointmentVisibility
        {
            get { return todaysTotalAppointmentVisibility; }
            set
            {
                todaysTotalAppointmentVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TodaysTotalAppointmentVisibility"));
            }
        }

        public bool IsRefreshClicked
        {
            get { return isRefreshClicked; }
            set
            {
                isRefreshClicked = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsRefreshClicked"));
            }
        }


        public double RemainingAmmount
        {
            get { return remainingAmmount; }
            set
            {
                remainingAmmount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RemainingAmmount"));
            }
        }

        public double TempRemainingAmmount
        {
            get { return tempRemainingAmmount; }
            set
            {
                tempRemainingAmmount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TempRemainingAmmount"));
            }
        }

        public double TargetAchieved
        {
            get { return targetAchieved; }
            set
            {
                targetAchieved = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TargetAchieved"));
                OnPropertyChanged(new PropertyChangedEventArgs("StrTargetAchieved"));
            }
        }

        public string StrTargetAchieved
        {
            get { return TargetAchieved.ToString() + "%"; }

        }


        public double SuccessRate
        {
            get { return successRate; }
            set
            {
                successRate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SuccessRate"));
                OnPropertyChanged(new PropertyChangedEventArgs("StrSuccessRate"));
            }
        }
        public string StrSuccessRate
        {
            get { return SuccessRate.ToString() + "%"; }

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




        public string YIDForeground
        {
            get { return yIDForeground; }
            set { yIDForeground = value; OnPropertyChanged(new PropertyChangedEventArgs("YIDForeground")); }
        }

        public List<LookupValue> StatusList
        {
            get
            {
                return statusList;
            }

            set
            {
                statusList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StatusList"));
            }
        }

        public ObservableCollection<MainStatus> MainStatusList
        {
            get
            {
                return mainStatusList;
            }

            set
            {
                mainStatusList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MainStatusList"));
            }
        }


        #endregion // Properties

        #region  public Commands
        public ICommand LayoutGroupSalesStatusLoadCommand { get; set; }
        public ICommand SelectDateCommand { get; private set; }
        public ICommand ChartLoadCommand { get; set; }
        public ICommand ChartDashboardSaleCustomDrawCrosshairCommand { get; set; }
        public ICommand GaugeLoadCommand { get; set; }
        public ICommand LoadCommand { get; private set; }
        public ICommand AllowEditAppointmentCommand { get; private set; }
        public ICommand PopupMenuShowingCommand { get; private set; }
        public ICommand CommandGridDoubleClick { get; private set; }
        public ICommand SalesOwnerPopupClosedCommand { get; private set; }
        public ICommand PlantOwnerPopupClosedCommand { get; private set; }
        public ICommand RefreshAnnualSalesPerformanceViewCommand { get; private set; }
        public ICommand CustomColumnSortCommand { get; set; }
        public ICommand CustomNumberUnboundColumnDataCommand { get; set; }

        public ICommand GroupBoxExpandedCommand { get; set; }

        public ICommand ListAllSalesStatusTypeItemsSourceChangedCommand { get; set; }


        #endregion // Command

        #region  public event

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion // Event

        #region Constructor

        public AnnualSalesPerformanceViewModel()
        {
            try
            {
                IsInit = true;

                FailedPlants = new List<string>();
                IsShowFailedPlantWarning = false;
                WarningFailedPlants = String.Empty;

                GeosApplication.Instance.Logger.Log("Constructor AnnualSalesPerformanceViewModel ...", category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.CurrentCurrencySymbol = GeosApplication.Instance.Currencies.Where(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency"].ToString()).Select(cur => cur.Symbol).SingleOrDefault();
                GeosApplication.SetCurrencySymbol(GeosApplication.Instance.CurrentCurrencySymbol);

                TempCurrentCurrencySymbol = GeosApplication.Instance.CurrentCurrencySymbol;

                ChartLoadCommand = new DevExpress.Mvvm.DelegateCommand<object>(ChartLoadAction);
                ChartDashboardSaleCustomDrawCrosshairCommand = new DevExpress.Mvvm.DelegateCommand<object>(ChartDashboardSaleCustomDrawCrosshairCommandAction);
                LoadCommand = new DevExpress.Mvvm.DelegateCommand<object>(LoadAction);
                SelectDateCommand = new DevExpress.Mvvm.DelegateCommand<EventArgs>(SelectDatesAction);
                PopupMenuShowingCommand = new DevExpress.Mvvm.DelegateCommand<SchedulerMenuEventArgs>(PopupMenuShowingAction);
                CommandGridDoubleClick = new DevExpress.Mvvm.DelegateCommand<object>(LeadsEditViewWindowShow);
                SalesOwnerPopupClosedCommand = new DevExpress.Mvvm.DelegateCommand<object>(SalesOwnerPopupClosedCommandAction);
                ListAllSalesStatusTypeItemsSourceChangedCommand = new DevExpress.Mvvm.DelegateCommand<object>(ListAllSalesStatusTypeChanged);
                PlantOwnerPopupClosedCommand = new DevExpress.Mvvm.DelegateCommand<object>(PlantOwnerPopupClosedCommandAction);
                RefreshAnnualSalesPerformanceViewCommand = new DevExpress.Mvvm.DelegateCommand<object>(RefreshAnnualSalesDetails);
                CustomColumnSortCommand = new DevExpress.Mvvm.DelegateCommand<TreeListCustomColumnSortEventArgs>(tableView1_CustomColumnSort);
                CustomNumberUnboundColumnDataCommand = new DevExpress.Mvvm.DelegateCommand<TreeListUnboundColumnDataEventArgs>(tableView1_CustomUnboundColumnData);
                LayoutGroupSalesStatusLoadCommand = new DevExpress.Mvvm.DelegateCommand<object>(LayoutGroupSalesStatusLoad);
                AllowEditAppointmentCommand = new DevExpress.Mvvm.DelegateCommand<EditAppointmentFormEventArgs>(AllowEditAppointmentAction);
                GroupBoxExpandedCommand = new DevExpress.Mvvm.DelegateCommand<object>(GroupBoxExpandAction);

                FillCmbSalesOwner();
                FillAppointmentType();
                //BindActionBarChartList();
                // FillRemainingAmmount(ListAllSalesStatusType);

                SelectedAppointmentType = new List<object>(AppointmentType);

                GeosApplication.Instance.Logger.Log("Constructor AnnualSalesPerformanceViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                    GeosApplication.Instance.SplashScreenMessage = string.Empty;
                }
                GeosApplication.Instance.Logger.Log("Get an error in AnnualSalesPerformanceViewModel() Constructor " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                    GeosApplication.Instance.SplashScreenMessage = string.Empty;
                }
                GeosApplication.Instance.Logger.Log("Get an error in AnnualSalesPerformanceViewModel() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log("Get an error in AnnualSalesPerformanceViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            GeosApplication.Instance.SplashScreenMessage = string.Empty;
            IsInit = false;
        }

        #endregion // Constructor

        #region  Methods

        private void FillRemainingAmmount(List<SalesStatusType> SalesStatusTypes)
        {
            GeosApplication.Instance.Logger.Log("FillRemainingAmmount ...", category: Category.Info, priority: Priority.Low);

            try
            {
                SalesStatusType SalesStatusTypeRemaining;
                if (!SalesStatusTypes.Any(X => X.Name == "REMAINING"))
                {
                    var SalesStatus = SalesStatusTypes.FirstOrDefault(x => x.Name == "TARGET");
                    SalesStatusTypeRemaining = new SalesStatusType
                    {
                        Currency = SalesStatus.Currency,
                        Name = "REMAINING",
                        TotalAmount = RemainingAmmount,
                        HtmlColor = SalesStatus.HtmlColor

                    };


                    SalesStatusTypes.Add(SalesStatusTypeRemaining);
                }
                else
                {
                    var SalesStatus = SalesStatusTypes.FirstOrDefault(x => x.Name == "REMAINING");
                    SalesStatus.TotalAmount = RemainingAmmount;

                }
                SalesStatusTypes.ForEach(x =>
                {
                    if (x.Name == "REMAINING" || x.Name == "TARGET")
                        SetColourForSalesStatus(x);

                });

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillRemainingAmmount() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            GeosApplication.Instance.Logger.Log("FillRemainingAmmount executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// To set the colour for SalesStatus based on ThemeName 
        /// </summary>
        /// <param name="salesStatusType"></param>
        public void SetColourForSalesStatus(SalesStatusType salesStatusType)
        {
            //if (GeosApplication.Instance.UserSettings.FirstOrDefault(x => x.Key == "ThemeName").Value == "WhiteAndBlue")
            //{
            //    salesStatusType.HtmlColor = "#FFFFFFFF";
            //}
            //if (GeosApplication.Instance.UserSettings.FirstOrDefault(x => x.Key == "ThemeName").Value == "BlackAndBlue")
            //{
            //    salesStatusType.HtmlColor = "#FF000000";
            //}
            salesStatusType.HtmlColor = "#00FFFFFF";
        }


        /// <summary>
        /// To fill PlantsVisited
        /// </summary>
        private void FillPlantsVisited()
        {
            GeosApplication.Instance.Logger.Log("FillPlantsVisited ...", category: Category.Info, priority: Priority.Low);
            PlannedVisitsSeries = new ObservableCollection<PieChartValuesPlannedVisit>();
            try
            {
                //fill account as per user conditions.

                if (GeosApplication.Instance.IdUserPermission == 21)
                {
                    if (GeosApplication.Instance.SelectedSalesOwnerUsersList != null)
                    {
                        List<UserManagerDtl> salesOwners = GeosApplication.Instance.SelectedSalesOwnerUsersList.Cast<UserManagerDtl>().ToList();
                        var salesOwnersIds = string.Join(",", salesOwners.Select(i => i.IdUser));

                        //==========================================================================================
                        PlannedVisitDetails = CrmStartUp.GetPlannedVisitDetail(GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, salesOwnersIds, GeosApplication.Instance.IdUserPermission, "0");
                        //==========================================================================================

                    }
                    else
                    {
                        PlannedVisitDetails = new PlannedVisitDetail();
                    }
                }
                else if (GeosApplication.Instance.IdUserPermission == 22)
                {
                    if (GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
                    {
                        List<Company> plantOwners = GeosApplication.Instance.SelectedPlantOwnerUsersList.Cast<Company>().ToList();
                        string plantOwnersIds = string.Join(",", plantOwners.Select(i => i.ConnectPlantId));
                        //==========================================================================================
                        PlannedVisitDetails = CrmStartUp.GetPlannedVisitDetail(GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, GeosApplication.Instance.ActiveUser.IdUser.ToString(), GeosApplication.Instance.IdUserPermission, plantOwnersIds);

                        //sw.Stop();
                        //GeosApplication.Instance.Logger.Log("Activity Data Received from Server In- " + (sw.ElapsedMilliseconds/1000).ToString() + " Secounds", Category.Info, Priority.Medium);
                        //==========================================================================================

                    }
                    else
                    {
                        PlannedVisitDetails = new PlannedVisitDetail();
                    }
                }
                else
                {
                    PlannedVisitDetails = CrmStartUp.GetPlannedVisitDetail(GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, GeosApplication.Instance.ActiveUser.IdUser.ToString(), GeosApplication.Instance.IdUserPermission, "0");

                }

                if (PlannedVisitDetails.PlannedAppInCurrentDate != 0)
                    YTD = Math.Round((Convert.ToDouble(PlannedVisitDetails.PlannedAppCompletedTillCurrentDate) / Convert.ToDouble(PlannedVisitDetails.PlannedAppInCurrentDate)) * 100, 2);
                else
                    YTD = 0;

                if (PlannedVisitDetails.PlannedAppInCurrentPeriod != 0)
                    Period = Math.Round((Convert.ToDouble(PlannedVisitDetails.PlannedAppCompletedTillCurrentPeriod) / Convert.ToDouble(PlannedVisitDetails.PlannedAppInCurrentPeriod)) * 100, 2);
                else
                    Period = 0;

                for (int i = 0; i < 2; i++)
                {
                    PieChartValuesPlannedVisit pieChartValuesPlannedVisit = new PieChartValuesPlannedVisit { Level = "level" + i };
                    if (i == 1)
                    {

                        pieChartValuesPlannedVisit.Argument = "YTD";
                        pieChartValuesPlannedVisit.Value = YTD;


                        if (GeosApplication.Instance.UserSettings.FirstOrDefault(x => x.Key == "ThemeName").Value == "WhiteAndBlue")
                        {
                            pieChartValuesPlannedVisit.HtmlColor = "#FF083493";
                            YIDForeground = "#FF083493";
                        }
                        if (GeosApplication.Instance.UserSettings.FirstOrDefault(x => x.Key == "ThemeName").Value == "BlackAndBlue")
                        {
                            pieChartValuesPlannedVisit.HtmlColor = "#FF2AB7FF ";
                            YIDForeground = "#FF2AB7FF";
                        }
                        PlannedVisitsSeries.Add(pieChartValuesPlannedVisit);

                        pieChartValuesPlannedVisit = new PieChartValuesPlannedVisit { Level = "level" + i };
                        pieChartValuesPlannedVisit.Argument = "YTD";
                        pieChartValuesPlannedVisit.Value = 100 - YTD;
                        pieChartValuesPlannedVisit.HtmlColor = "#DCDCDC ";
                        PlannedVisitsSeries.Add(pieChartValuesPlannedVisit);


                    }
                    if (i == 0)
                    {

                        pieChartValuesPlannedVisit.Argument = "Period";
                        pieChartValuesPlannedVisit.Value = Period;

                        pieChartValuesPlannedVisit.HtmlColor = "#696969";
                        PlannedVisitsSeries.Add(pieChartValuesPlannedVisit);
                        pieChartValuesPlannedVisit = new PieChartValuesPlannedVisit { Level = "level" + i };
                        pieChartValuesPlannedVisit.Argument = "Period";
                        pieChartValuesPlannedVisit.HtmlColor = "#DCDCDC ";
                        pieChartValuesPlannedVisit.Value = 100 - Period;
                        PlannedVisitsSeries.Add(pieChartValuesPlannedVisit);

                    }
                }

                PlannedVisitsLable = "YID : " + YTD + " %" + "\n" + "Period : " + Period + " %";


            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillPlantsVisited() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            GeosApplication.Instance.Logger.Log("FillPlantsVisited executed successfully", category: Category.Info, priority: Priority.Low);
        }

        private void FillAppointmentType()
        {
            AppointmentType = new List<string>();


            AppointmentType = GetLookupValues.Select(ap => ap.Value).ToList();

            AppointmentType.Add("CloseDate");
            AppointmentType.Add("DeliveryDate");
            AppointmentType.Add("Quote Sent Date");
            AppointmentType.Add("RFQ Date");


        }
        /// <summary>
        /// 
        /// To handel the focused row event
        /// </summary>
        /// <param name="obj"></param>
        private void ListAllSalesStatusTypeChanged(object obj)
        {
            GeosApplication.Instance.Logger.Log("ListAllSalesStatusTypeChanged ...", category: Category.Info, priority: Priority.Low);
            try
            {
                ((TableView)obj).FocusedRowHandle = GridControl.InvalidRowHandle;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ListAllSalesStatusTypeChanged " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            GeosApplication.Instance.Logger.Log("ListAllSalesStatusTypeChanged executed successfully", category: Category.Info, priority: Priority.Low);

        }

        /// <summary>
        /// Method for fill data as per user permission.
        /// </summary>
        private void FillCmbSalesOwner()
        {
            ListAllSalesStatusType = new List<SalesStatusType>();
            ListAllSalesStatusType = CrmStartUp.GetAllSalesStatusType().ToList();


            ListAllSalesStatusType.Insert(0, ListAllSalesStatusType.ElementAt(ListAllSalesStatusType.Count - 1));
            ListAllSalesStatusType.RemoveAt(ListAllSalesStatusType.Count - 1);

            Labels = new AppointmentLabelCollection();
            Labels.Clear();

            foreach (var item in ListAllSalesStatusType)
            {
                Labels.Add(Labels.CreateNewLabel(item.IdSalesStatusType, item.Name, item.Name, (Color)ColorConverter.ConvertFromString(item.HtmlColor != null ? item.HtmlColor : "#FFFFFF")));
            }

            GetLookupValues = CrmStartUp.GetLookupValues(9).ToList();
            foreach (var item in GetLookupValues)
            {
                Labels.Add(Labels.CreateNewLabel(item.IdLookupValue, item.Value, item.Value, (Color)ColorConverter.ConvertFromString(item.HtmlColor != null ? item.HtmlColor : "#FFFFFF")));
            }


            if (GeosApplication.Instance.IdUserPermission == 21)
            {
                if (GeosApplication.Instance.SelectedSalesOwnerUsersList != null)
                {
                    //List<UserManagerDtl> salesOwners = GeosApplication.Instance.SelectedSalesOwnerUsersList.Cast<UserManagerDtl>().ToList();
                    //var salesOwnersIds = string.Join(",", salesOwners.Select(i => i.IdUser));
                    //Account

                    FillAnnualSalesDetailsByUsers();
                }
                else
                {
                    //SelectedAppointmentType.Clear();
                    SelectedAppointmentType = new List<object>();
                }
            }
            else if (GeosApplication.Instance.IdUserPermission == 22)
            {

                if (GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
                {
                    //List<Company> plantOwners = GeosApplication.Instance.SelectedPlantOwnerUsersList.Cast<Company>().ToList();
                    //var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.ConnectPlantId));
                    //Account

                    FillAnnualSalesDetailsByPlant();
                }
                else
                {
                    //SelectedAppointmentType.Clear();
                    SelectedAppointmentType = new List<object>();
                }
            }
            else
            {
                FillAnnualSalesDetails();
            }
        }

        /// <summary>
        /// Method for fill annual sales detailes by users.
        /// </summary>
        private void FillAnnualSalesDetailsByUsers()
        {
            GeosApplication.Instance.Logger.Log("FillAnnualSalesDetailsByUsers ...", category: Category.Info, priority: Priority.Low);


            ListSalesStatusType = new List<SalesStatusType>();
            dt = new DataTable();


            ListSalesStatusType = ListAllSalesStatusType.Where(trg => trg.Name != "TARGET").ToList();
            dt.Columns.Add("Month");
            dt.Columns.Add("Year");
            dt.Columns.Add("MonthYear");

            foreach (var item in ListSalesStatusType)
            {
                dt.Columns.Add(item.Name, typeof(double));

            }

            List<UserManagerDtl> salesOwners = GeosApplication.Instance.SelectedSalesOwnerUsersList.Cast<UserManagerDtl>().ToList();
            var salesOwnersIds = string.Join(",", salesOwners.Select(i => i.IdUser));
            PreviouslySelectedSalesOwners = salesOwnersIds;

            ActivityList = CrmStartUp.GetDailyEventActivities(salesOwnersIds, 21, "0");

            //StatusList = new List<LookupValue>();

            StatusList = new List<LookupValue>(CrmStartUp.GetActionsCountOfStatus(salesOwnersIds,
                                                                                    null,
                                                                                    GeosApplication.Instance.ActiveUser.IdUser,
                                                                                    GeosApplication.Instance.IdUserPermission,
                                                                                    GeosApplication.Instance.SelectedyearStarDate,
                                                                                    GeosApplication.Instance.SelectedyearEndDate).ToList().OrderBy(a => a.Position));
            BindActionBarChartList();

            ListCarProject = new List<CarProjectDetail>();
            SalesPerformanceList = new List<OfferDetail>();
            SalesStatusByMonthList = new List<OfferDetail>();
            List<TempAppointment> modelAppointmentList = new List<TempAppointment>();

            // Continue loop although some plant is not available and Show error message.
            foreach (var item in CrmStartUp.GetAllCompaniesDetails_V2040(GeosApplication.Instance.ActiveUser.IdUser))
            {
                try
                {
                    GeosApplication.Instance.SplashScreenMessage = "Connecting to " + item.Alias;

                    ListCarProject.AddRange(CrmStartUp.GetTopCarProjectOffersOptimization(GeosApplication.Instance.IdCurrencyByRegion,
                                             salesOwnersIds,
                                             GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate,
                                             GeosApplication.Instance.CrmTopOffers,
                                             GeosApplication.Instance.IdUserPermission,
                                             item,
                                             GeosApplication.Instance.ActiveUser.IdUser));

                    SalesPerformanceList.AddRange(CrmStartUp.GetSalesStatusWithTarget_V2035(GeosApplication.Instance.IdCurrencyByRegion, salesOwnersIds, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, item, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.IdUserPermission));

                    SalesStatusByMonthList.AddRange(CrmStartUp.GetSalesStatusByMonthAllPermission_V2035(GeosApplication.Instance.IdCurrencyByRegion, salesOwnersIds, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, item, GeosApplication.Instance.IdUserPermission, GeosApplication.Instance.ActiveUser.IdUser));

                    modelAppointmentList.AddRange(CrmStartUp.GetDailyEvents_V2035(GeosApplication.Instance.IdCurrencyByRegion, salesOwnersIds, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, item, GeosApplication.Instance.IdUserPermission, GeosApplication.Instance.ActiveUser.IdUser));

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

            if (FailedPlants == null || FailedPlants.Count == 0)
            {
                IsShowFailedPlantWarning = false;
                WarningFailedPlants = string.Empty;
            }

            TopOfferNumber = string.Format(System.Windows.Application.Current.FindResource("AnnualSalesPerformanceViewTopProjects").ToString(), GeosApplication.Instance.CrmTopOffers);

            FillTopProjects();
            FillSalesPerformanceTargetByUser();
            FillAppointments(modelAppointmentList);
            CreateTable();
            FillPlannedVisits();
            GeosApplication.Instance.Logger.Log("FillAnnualSalesDetailsByUsers executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method for fill annual sales details by plant
        /// </summary>
        private void FillAnnualSalesDetailsByPlant()
        {
            GeosApplication.Instance.Logger.Log("FillAnnualSalesDetailsByPlant ...", category: Category.Info, priority: Priority.Low);

            //ListAllSalesStatusType = new List<SalesStatusType>();
            ListSalesStatusType = new List<SalesStatusType>();
            dt = new DataTable();

            //ListAllSalesStatusType = CrmStartUp.GetAllSalesStatusType().ToList();

            ListSalesStatusType = ListAllSalesStatusType.Where(trg => trg.Name != "TARGET" && trg.Name != "Remaining".ToUpper()).ToList();
            dt.Columns.Add("Month");
            dt.Columns.Add("Year");
            dt.Columns.Add("MonthYear");
            foreach (var item in ListSalesStatusType)
            {
                dt.Columns.Add(item.Name, typeof(double));
            }

            //FillOrder();
            //List<Offer> TopOffersList = new List<Offer>();

            ListCarProject = new List<CarProjectDetail>();
            SalesPerformanceList = new List<OfferDetail>();
            SalesStatusByMonthList = new List<OfferDetail>();
            List<TempAppointment> modelAppointmentList = new List<TempAppointment>();

            if (GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
            {
                List<Company> plantOwners = GeosApplication.Instance.SelectedPlantOwnerUsersList.Cast<Company>().ToList();
                var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.ConnectPlantId));
                PreviouslySelectedPlantOwners = string.Join(",", plantOwners.Select(i => i.ConnectPlantId));

                ActivityList = CrmStartUp.GetDailyEventActivities(GeosApplication.Instance.ActiveUser.IdUser.ToString(), 22, plantOwnersIds);

                StatusList = new List<LookupValue>(CrmStartUp.GetActionsCountOfStatus(null,
                                                                               plantOwnersIds,
                                                                               GeosApplication.Instance.ActiveUser.IdUser,
                                                                               GeosApplication.Instance.IdUserPermission,
                                                                               GeosApplication.Instance.SelectedyearStarDate,
                                                                               GeosApplication.Instance.SelectedyearEndDate).ToList().OrderBy(a => a.Position));
                BindActionBarChartList();

                // Continue loop although some plant is not available and Show error message.
                foreach (var item in plantOwners)
                {
                    try
                    {
                        GeosApplication.Instance.SplashScreenMessage = "Connecting to " + item.ShortName;

                        ListCarProject.AddRange(CrmStartUp.GetTopCarProjectOffersOptimization(GeosApplication.Instance.IdCurrencyByRegion,
                                             Convert.ToString(GeosApplication.Instance.ActiveUser.IdUser),
                                             GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate,
                                             GeosApplication.Instance.CrmTopOffers,
                                             GeosApplication.Instance.IdUserPermission,
                                             item,
                                             GeosApplication.Instance.ActiveUser.IdUser));


                        SalesPerformanceList.AddRange(CrmStartUp.GetSalesStatusWithTarget_V2035(GeosApplication.Instance.IdCurrencyByRegion,
                                                                        GeosApplication.Instance.ActiveUser.IdUser.ToString(),

                                                                        GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, item, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.IdUserPermission).ToList());


                        SalesStatusByMonthList.AddRange(CrmStartUp.GetSalesStatusByMonthAllPermission_V2035(GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.ActiveUser.IdUser.ToString(), GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, item, GeosApplication.Instance.IdUserPermission, GeosApplication.Instance.ActiveUser.IdUser).ToList());

                        modelAppointmentList.AddRange(CrmStartUp.GetDailyEvents_V2035(GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.ActiveUser.IdUser.ToString(), GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, item, GeosApplication.Instance.IdUserPermission, GeosApplication.Instance.ActiveUser.IdUser));
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


            TopOfferNumber = string.Format(System.Windows.Application.Current.FindResource("AnnualSalesPerformanceViewTopProjects").ToString(), GeosApplication.Instance.CrmTopOffers);

            FillTopProjects();
            FillSalesPerformanceTargetByUser();

            FillAppointments(modelAppointmentList);
            CreateTable();

            GeosApplication.Instance.SplashScreenMessage = string.Empty;
            GeosApplication.Instance.Logger.Log("FillAnnualSalesDetailsByPlant executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method for refresh AnnualSales Grid details. 20
        /// </summary>
        private void FillAnnualSalesDetails()
        {
            GeosApplication.Instance.Logger.Log("FillAnnualSalesDetails ...", category: Category.Info, priority: Priority.Low);

            //ListAllSalesStatusType = new List<SalesStatusType>();
            ListSalesStatusType = new List<SalesStatusType>();
            dt = new DataTable();

            //ListAllSalesStatusType = CrmStartUp.GetAllSalesStatusType().ToList();
            ListSalesStatusType = ListAllSalesStatusType.Where(trg => trg.Name != "TARGET").ToList();
            dt.Columns.Add("Month");
            dt.Columns.Add("Year");
            dt.Columns.Add("MonthYear");

            foreach (var item in ListSalesStatusType)
            {
                dt.Columns.Add(item.Name, typeof(double));
            }

            ActivityList = CrmStartUp.GetDailyEventActivities(GeosApplication.Instance.ActiveUser.IdUser.ToString(), 20, "0");

            StatusList = new List<LookupValue>(CrmStartUp.GetActionsCountOfStatus(null,
                                                                            null,
                                                                            GeosApplication.Instance.ActiveUser.IdUser,
                                                                            GeosApplication.Instance.IdUserPermission,
                                                                            GeosApplication.Instance.SelectedyearStarDate,
                                                                            GeosApplication.Instance.SelectedyearEndDate).ToList().OrderBy(a => a.Position));
            BindActionBarChartList();

            //List<Offer> TopOffersList = new List<Offer>();
            ListCarProject = new List<CarProjectDetail>();
            SalesPerformanceList = new List<OfferDetail>();
            SalesStatusByMonthList = new List<OfferDetail>();
            List<TempAppointment> modelAppointmentList = new List<TempAppointment>();


            // Continue loop although some plant is not available and Show error message.
            foreach (var item in GeosApplication.Instance.CompanyList)
            {
                try
                {
                    GeosApplication.Instance.SplashScreenMessage = "Connecting to " + item.Alias;

                    ListCarProject.AddRange(CrmStartUp.GetTopCarProjectOffersOptimization(GeosApplication.Instance.IdCurrencyByRegion,
                                             Convert.ToString(GeosApplication.Instance.ActiveUser.IdUser),
                                             GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate,
                                             GeosApplication.Instance.CrmTopOffers,
                                             GeosApplication.Instance.IdUserPermission,
                                             item,
                                             GeosApplication.Instance.ActiveUser.IdUser));


                    SalesPerformanceList.AddRange(CrmStartUp.GetSalesStatusWithTarget_V2035(GeosApplication.Instance.IdCurrencyByRegion,
                                                                           GeosApplication.Instance.ActiveUser.IdUser.ToString(),

                                                                           GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, item, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.IdUserPermission).ToList());


                    SalesStatusByMonthList.AddRange(CrmStartUp.GetSalesStatusByMonthAllPermission_V2035(GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.ActiveUser.IdUser.ToString(), GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, item, GeosApplication.Instance.IdUserPermission, GeosApplication.Instance.ActiveUser.IdUser).ToList());

                    modelAppointmentList.AddRange(CrmStartUp.GetDailyEvents_V2035(GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.ActiveUser.IdUser.ToString(), GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, item, GeosApplication.Instance.IdUserPermission, GeosApplication.Instance.ActiveUser.IdUser));
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

                GeosApplication.Instance.SplashScreenMessage = string.Empty;

                if (FailedPlants == null || FailedPlants.Count == 0)
                {
                    IsShowFailedPlantWarning = false;
                    WarningFailedPlants = string.Empty;
                }
            }

            TopOfferNumber = string.Format(System.Windows.Application.Current.FindResource("AnnualSalesPerformanceViewTopProjects").ToString(), GeosApplication.Instance.CrmTopOffers);

            FillTopProjects();
            FillSalesPerformanceTarget();
            FillAppointments(modelAppointmentList);
            CreateTable();

            GeosApplication.Instance.Logger.Log("FillAnnualSalesDetails executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method for fill appointments (daily events).
        /// </summary>
        /// <param name="modelAppointmentList"></param>
        private void FillAppointments(List<TempAppointment> modelAppointmentList)
        {
            GeosApplication.Instance.Logger.Log("Method FillAppointments ...", category: Category.Info, priority: Priority.Low);

            List<SalesStatusType> copyListAllSalesStatusType = ListAllSalesStatusType.ToList();
            foreach (SalesStatusType item in copyListAllSalesStatusType)
            {
                if (item.Name != "TARGET" && item.Name != "REMAINING")
                {
                    item.TotalAmount = SalesPerformanceList.Where(spf => spf.SaleStatusName == item.Name).Select(ofr => ofr.Value).ToList().Sum();
                }
            }

            if (copyListAllSalesStatusType.Exists(x => x.Name == "TARGET"))
            {
                //if user roll is 22 then take all selected plants target total.
                if (GeosApplication.Instance.IdUserPermission == 22)
                {
                    if (GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
                    {
                        List<Company> plantOwners = GeosApplication.Instance.SelectedPlantOwnerUsersList.Cast<Company>().ToList();
                        var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.ConnectPlantId));

                        //PlantBusinessUnitSalesQuota plantBusinessUnitSalesQuota = CrmStartUp.GetTotalPlantQuotaSelectedPlantWiseAndYear(plantOwnersIds, GeosApplication.Instance.IdCurrencyByRegion, Convert.ToInt32(GeosApplication.Instance.CrmOfferYear), GeosApplication.Instance.ActiveUser.IdUser);
                        PlantBusinessUnitSalesQuota plantBusinessUnitSalesQuota = CrmStartUp.GetTotalPlantQuotaSelectedPlantWiseAndYearWithExchangeDate(plantOwnersIds, GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, GeosApplication.Instance.ActiveUser.IdUser);

                        SalesStatusType salesStatusType = copyListAllSalesStatusType.FirstOrDefault(x => x.Name == "TARGET");
                        salesStatusType.TotalAmount = plantBusinessUnitSalesQuota.QuotaAmountWithExchangeRate;


                    }
                }

                //if user roll is 20 and 21 then take all selected salesOwner's target total.
                else if (GeosApplication.Instance.IdUserPermission == 20 || GeosApplication.Instance.IdUserPermission == 21)
                {
                    var salesOwnersIds = "";
                    if (GeosApplication.Instance.IdUserPermission == 21)
                    {
                        if (GeosApplication.Instance.SelectedSalesOwnerUsersList != null)
                        {
                            List<UserManagerDtl> salesOwners = GeosApplication.Instance.SelectedSalesOwnerUsersList.Cast<UserManagerDtl>().ToList();
                            salesOwnersIds = string.Join(",", salesOwners.Select(i => i.IdUser));
                        }
                    }
                    if (GeosApplication.Instance.IdUserPermission == 20)
                    {
                        salesOwnersIds = GeosApplication.Instance.ActiveUser.IdUser.ToString();
                    }

                    // SalesUserQuota salesUser = CrmStartUp.GetTotalSalesQuotaBySelectedUserIdAndYear(salesOwnersIds.ToString(), GeosApplication.Instance.IdCurrencyByRegion, Convert.ToInt32(GeosApplication.Instance.CrmOfferYear));
                    SalesUserQuota salesUser = CrmStartUp.GetTotalSalesQuotaBySelectedUserIdAndYearWithExchangeRate(salesOwnersIds.ToString(), GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate);
                    SalesStatusType salesStatusType = copyListAllSalesStatusType.FirstOrDefault(x => x.Name == "TARGET");
                    salesStatusType.TotalAmount = salesUser.SalesQuotaAmountWithExchangeRate;
                    salesStatusType = copyListAllSalesStatusType.FirstOrDefault(x => x.Name == "REMAINING");
                    salesStatusType.TotalAmount = RemainingAmmount;
                }
            }

            else
            {
                //if user roll is 22 then take all selected plants target total.
                if (GeosApplication.Instance.IdUserPermission == 22)
                {
                    if (GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
                    {
                        List<Company> plantOwners = GeosApplication.Instance.SelectedPlantOwnerUsersList.Cast<Company>().ToList();
                        var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.ConnectPlantId));

                        //PlantBusinessUnitSalesQuota plantBusinessUnitSalesQuota = CrmStartUp.GetTotalPlantQuotaSelectedPlantWiseAndYear(plantOwnersIds.ToString(), GeosApplication.Instance.IdCurrencyByRegion, Convert.ToInt32(GeosApplication.Instance.CrmOfferYear), GeosApplication.Instance.ActiveUser.IdUser);
                        PlantBusinessUnitSalesQuota plantBusinessUnitSalesQuota = CrmStartUp.GetTotalPlantQuotaSelectedPlantWiseAndYearWithExchangeDate(plantOwnersIds.ToString(), GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, GeosApplication.Instance.ActiveUser.IdUser);

                        SalesStatusType salesStatusType = new SalesStatusType();
                        salesStatusType.Name = "TARGET";
                        salesStatusType.IdImage = 0;
                        salesStatusType.HtmlColor = "#FFFFFF";
                        salesStatusType.TotalAmount = plantBusinessUnitSalesQuota.QuotaAmountWithExchangeRate;

                        copyListAllSalesStatusType.Add(salesStatusType);
                    }
                }

                //if user roll is 20 and 21 then take all selected salesOwner's target total.
                else if (GeosApplication.Instance.IdUserPermission == 20 || GeosApplication.Instance.IdUserPermission == 21)
                {
                    var salesOwnersIds = "";
                    if (GeosApplication.Instance.IdUserPermission == 21)
                    {
                        if (GeosApplication.Instance.SelectedSalesOwnerUsersList != null)
                        {
                            List<UserManagerDtl> salesOwners = GeosApplication.Instance.SelectedSalesOwnerUsersList.Cast<UserManagerDtl>().ToList();
                            salesOwnersIds = string.Join(",", salesOwners.Select(i => i.IdUser));
                        }
                    }
                    if (GeosApplication.Instance.IdUserPermission == 20)
                    {
                        salesOwnersIds = GeosApplication.Instance.ActiveUser.IdUser.ToString();
                    }

                    //SalesUserQuota salesUser = CrmStartUp.GetTotalSalesQuotaBySelectedUserIdAndYear(salesOwnersIds.ToString(), GeosApplication.Instance.IdCurrencyByRegion, Convert.ToInt32(GeosApplication.Instance.CrmOfferYear));

                    SalesUserQuota salesUser = CrmStartUp.GetTotalSalesQuotaBySelectedUserIdAndYearWithExchangeRate(salesOwnersIds.ToString(), GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate);

                    SalesStatusType salesStatusType = new SalesStatusType();
                    salesStatusType.Name = "TARGET";
                    salesStatusType.IdImage = 0;
                    salesStatusType.HtmlColor = "#FFFFFF";
                    salesStatusType.TotalAmount = salesUser.SalesQuotaAmountWithExchangeRate;

                    copyListAllSalesStatusType.Add(salesStatusType);
                }

            }
            FillRemainingAmmount(copyListAllSalesStatusType);
            FillPlantsVisited();
            ListAllSalesStatusType = copyListAllSalesStatusType.ToList();
            modelAppointmentList = FillActivityInModelAppointment(ActivityList, modelAppointmentList);
            GeosApplication.Instance.SplashScreenMessage = string.Empty;
            ObservableCollection<TempAppointment> TempAppointments;

            //Temporary copy of list for do operation on local list and pass it to main list to increase performance.
            ObservableCollection<TempAppointment> AppointmentsCopy = new ObservableCollection<TempAppointment>(modelAppointmentList);

            // ***[Start] create list for defferent datetype and merge it in main "Appointments" for show appointment
            // as per defferent date types.

            TempAppointments = new ObservableCollection<TempAppointment>();
            foreach (TempAppointment item in AppointmentsCopy)
            {
                if (item.Status == -1)
                {

                }

                else
                if (!item.IsOfferDonePO)
                {

                    if (item.GeosStatus.IdOfferStatusType == 1 || item.GeosStatus.IdOfferStatusType == 2)
                    {
                        if (item.SendIn != null && item.SendIn > DateTime.MinValue)
                        {
                            TempAppointment modelAppointment_Onlyquoted = new TempAppointment();
                            modelAppointment_Onlyquoted = (TempAppointment)item.Clone();
                            modelAppointment_Onlyquoted.StartTime = item.SendIn;
                            modelAppointment_Onlyquoted.EndTime = item.SendIn;
                            modelAppointment_Onlyquoted.Tag = "RFQ Date";
                            TempAppointments.Add(modelAppointment_Onlyquoted);
                        }

                        if (item.RfqReception != null && item.RfqReception > DateTime.MinValue)
                        {
                            TempAppointment modelAppointment_Waitingforquote = new TempAppointment();
                            modelAppointment_Waitingforquote = (TempAppointment)item.Clone();
                            modelAppointment_Waitingforquote.StartTime = item.RfqReception;
                            modelAppointment_Waitingforquote.EndTime = item.RfqReception;
                            modelAppointment_Waitingforquote.Tag = "Quote Sent Date";
                            TempAppointments.Add(modelAppointment_Waitingforquote);
                        }
                    }

                    item.Tag = "CloseDate";
                }
                else
                {
                    TempAppointment modelAppointment_DeliveryDate = new TempAppointment();
                    modelAppointment_DeliveryDate = (TempAppointment)item.Clone();
                    modelAppointment_DeliveryDate.StartTime = item.OtsDeliveryDate;
                    modelAppointment_DeliveryDate.EndTime = item.OtsDeliveryDate;
                    modelAppointment_DeliveryDate.Tag = "DeliveryDate";
                    TempAppointments.Add(modelAppointment_DeliveryDate);

                    if (item.IsGoAhead == 0)
                        item.Tag = "PODateGreen";
                    else if (item.IsGoAhead == 1)
                        item.Tag = "PODateRed";
                }
            }

            if (TempAppointments != null && TempAppointments.Count > 0)
            {
                AppointmentsCopy = new ObservableCollection<TempAppointment>(AppointmentsCopy.Concat(TempAppointments));
            }

            Appointments = new List<TempAppointment>(AppointmentsCopy);
            FilterAppointments = new List<TempAppointment>(Appointments);

            //// **[End]

            TodaysTotalAppointment = Appointments.Where(n => Convert.ToDateTime(n.StartTime).Date >= GeosApplication.Instance.ServerDateTime.Date && Convert.ToDateTime(n.EndTime).Date <= GeosApplication.Instance.ServerDateTime).ToList().Count;

            if (TodaysTotalAppointment > 0)
            {
                TodaysTotalAppointmentVisibility = Visibility.Visible;
            }
            else
            {
                TodaysTotalAppointmentVisibility = Visibility.Hidden;
            }

            GeosApplication.Instance.Logger.Log("Method FillAppointments() executed successfully", category: Category.Info, priority: Priority.Low);
        }





        /// <summary>
        /// filling the chart values for
        /// </summary>
        private void FillPlannedVisits()
        {

        }




        /// <summary>
        /// Method for fill activity on daily event.
        /// </summary>
        /// <param name="activityList"></param>
        /// <param name="modelAppointmentList"></param>
        /// <returns></returns>
        private List<TempAppointment> FillActivityInModelAppointment(List<DailyEventActivity> activityList, List<TempAppointment> modelAppointmentList)
        {
            GeosApplication.Instance.Logger.Log("Method FillActivityInModelAppointment ...", category: Category.Info, priority: Priority.Low);

            try
            {

                foreach (DailyEventActivity item in activityList)
                {
                    TempAppointment modelAppointment = new TempAppointment();

                    modelAppointment.IdOffer = item.IdActivity;
                    modelAppointment.Description = item.Subject;
                    modelAppointment.Subject = item.Subject;
                    modelAppointment.StartTime = item.FromDate;
                    modelAppointment.EndTime = item.ToDate;
                    modelAppointment.Label = item.IdActivityType;
                    modelAppointment.Status = -1;

                    if (item.IdActivityType > 0)
                    {
                        modelAppointment.Tag = item.ActivityType;
                    }

                    modelAppointmentList.Add(modelAppointment);
                }

                return modelAppointmentList;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillActivityInModelAppointment() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            GeosApplication.Instance.Logger.Log("Method FillActivityInModelAppointment() executed successfully", category: Category.Info, priority: Priority.Low);

            return modelAppointmentList;
        }

        /// <summary>
        /// Method for action after choose plants owners.
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

            if (GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
            {
                FailedPlants = new List<string>();
                IsShowFailedPlantWarning = false;
                WarningFailedPlants = String.Empty;

                FillAnnualSalesDetailsByPlant();
                //BindActionBarChartList();

                if (FailedPlants != null && FailedPlants.Count > 0)
                {
                    IsShowFailedPlantWarning = true;
                    WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                    WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                }
            }
            else
            {
                List<SalesStatusType> copyListAllSalesStatusType = ListAllSalesStatusType.ToList();
                foreach (SalesStatusType salesStatusType in copyListAllSalesStatusType)
                {
                    salesStatusType.TotalAmount = 0.0;
                }
                ListAllSalesStatusType = copyListAllSalesStatusType.ToList();

                SuccessRate = 0.0;
                TargetAchieved = 0.0;
                RemainingAmmount = 0.0;

                DataTableTopProjects.Rows.Clear();
                OfferTotal = 0.0;

                GraphDataTable.Rows.Clear();
                if (chartControl != null) { chartControl.UpdateData(); }
                Appointments = new List<TempAppointment>();
                TodaysTotalAppointment = Appointments.Where(n => Convert.ToDateTime(n.StartTime).Date >= GeosApplication.Instance.ServerDateTime.Date && Convert.ToDateTime(n.EndTime).Date <= GeosApplication.Instance.ServerDateTime).ToList().Count;
                FailedPlants.Clear();
                IsShowFailedPlantWarning = false;
                WarningFailedPlants = String.Empty;
                //SelectedAppointmentType.Clear();
                SelectedAppointmentType = new List<object>();
                //SelectedAppointmentType.AddRange(AppointmentType);
                FilterAppointments = new List<TempAppointment>();
                if (TodaysTotalAppointment > 0)
                {
                    TodaysTotalAppointmentVisibility = Visibility.Visible;
                }
                else
                {
                    TodaysTotalAppointmentVisibility = Visibility.Hidden;
                }
            }

            //SelectedAppointmentType.Clear();
            //SelectedAppointmentType.AddRange(AppointmentType);

            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

            GeosApplication.Instance.Logger.Log("Method PlantOwnerPopupClosedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method for action after choose sales owners.
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

            if (GeosApplication.Instance.SelectedSalesOwnerUsersList != null)
            {
                FailedPlants = new List<string>();
                IsShowFailedPlantWarning = false;
                WarningFailedPlants = String.Empty;

                FillAnnualSalesDetailsByUsers();

                if (FailedPlants != null && FailedPlants.Count > 0)
                {
                    IsShowFailedPlantWarning = true;
                    WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                    WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                }
            }
            else
            {
                List<SalesStatusType> copyListAllSalesStatusType = ListAllSalesStatusType.ToList();
                foreach (SalesStatusType salesStatusType in copyListAllSalesStatusType)
                {
                    salesStatusType.TotalAmount = 0.0;
                }
                ListAllSalesStatusType = copyListAllSalesStatusType.ToList();

                SuccessRate = 0.0;
                TargetAchieved = 0.0;
                RemainingAmmount = 0.0;

                DataTableTopProjects.Rows.Clear();
                OfferTotal = 0.0;

                GraphDataTable.Rows.Clear();
                if (chartControl != null) { chartControl.UpdateData(); }
                Appointments = new List<TempAppointment>();
                TodaysTotalAppointment = Appointments.Where(n => Convert.ToDateTime(n.StartTime).Date >= GeosApplication.Instance.ServerDateTime.Date && Convert.ToDateTime(n.EndTime).Date <= GeosApplication.Instance.ServerDateTime).ToList().Count;

                FailedPlants.Clear();
                IsShowFailedPlantWarning = false;
                WarningFailedPlants = String.Empty;
                //SelectedAppointmentType.Clear();
                SelectedAppointmentType = new List<object>();
                //SelectedAppointmentType.AddRange(AppointmentType);
                FilterAppointments = new List<TempAppointment>();
                if (TodaysTotalAppointment > 0)
                {
                    TodaysTotalAppointmentVisibility = Visibility.Visible;
                }
                else
                {
                    TodaysTotalAppointmentVisibility = Visibility.Hidden;
                }
            }



            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

            GeosApplication.Instance.Logger.Log("Method SalesOwnerPopupClosedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method for refresh Annual Sales From database by refresh Button.
        /// </summary>
        /// <param name="obj"></param>
        private void RefreshAnnualSalesDetails(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method RefreshAnnualSalesDetails ...", category: Category.Info, priority: Priority.Low);

            FailedPlants = new List<string>();
            IsShowFailedPlantWarning = false;
            WarningFailedPlants = String.Empty;

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

            //List<object> oldSelectedAppointmentType = new List<object>();
            //oldSelectedAppointmentType = SelectedAppointmentType;

            GeosApplication.Instance.CurrentCurrencySymbol = GeosApplication.Instance.Currencies.Where(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency"].ToString()).Select(cur => cur.Symbol).SingleOrDefault();
            GeosApplication.SetCurrencySymbol(GeosApplication.Instance.CurrentCurrencySymbol);
            TempCurrentCurrencySymbol = GeosApplication.Instance.CurrentCurrencySymbol;
            MyFilterString = string.Empty;
            FillCmbSalesOwner();
            FillRemainingAmmount(listAllSalesStatusType);
            FillPlantsVisited();
            SelectedAppointmentType = new List<object>(AppointmentType);

            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

            GeosApplication.Instance.Logger.Log("Method RefreshAnnualSalesDetails() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method for edit offer details.
        /// [001][SP-67][skale][16-07-2019][GEOS2-1641]fechas vencidas en sistema CRM
        /// </summary>
        /// <param name="obj"></param>
        private void LeadsEditViewWindowShow(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method LeadsEditViewWindowShow ...", category: Category.Info, priority: Priority.Low);

                if (obj == null) return;
                //var abc = ((System.Data.DataRowView)((DevExpress.Xpf.Grid.DataViewBase)obj).FocusedRow).Row.ItemArray[4].ToString();

               // DataRow dataCurrentRow = ((System.Data.DataRowView)((DevExpress.Xpf.Grid.DataViewBase)obj).FocusedRow).DataView as DataRow;
                DataRow dataCurrentRow =((System.Data.DataRowView)(((DevExpress.Xpf.Grid.TreeListView)obj).FocusedNode.Content)).Row;  //((System.Data.DataRowView)(obj)).Row;

                if (dataCurrentRow != null)
                {
                    if (dataCurrentRow["ChildId"].ToString().StartsWith("Project")
                        && string.IsNullOrEmpty(dataCurrentRow["ParentId"].ToString()))
                    {
                        return;
                    }

                    //DXSplashScreen.Show<SplashScreenView>();
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

                    long IdOffer = Convert.ToUInt32(dataCurrentRow["IdOffer"].ToString());
                    string ConnectPlantId = Convert.ToString(dataCurrentRow["ConnectPlantId"]);

                    //Offer OfferToEdit = (Offer)obj;

                    List<Offer> LeadsList = new List<Offer>();
                    //[001] added Change Method
                    LeadsList.Add(CrmStartUp.GetOfferDetailsById_V2037(IdOffer,
                                                                 GeosApplication.Instance.ActiveUser.IdUser,
                                                                 GeosApplication.Instance.IdCurrencyByRegion,
                                                                 GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate,
                                                                 GeosApplication.Instance.CompanyList.Where(cmp => cmp.ConnectPlantId == ConnectPlantId).FirstOrDefault()));

                    LostReasonsByOffer lostReasonsByOffer = CrmStartUp.GetLostReasonsByOffer(Convert.ToInt64(LeadsList[0].IdOffer),
                                                                                             GeosApplication.Instance.CompanyList.Where(cmp => cmp.ConnectPlantId == ConnectPlantId).Select(x => x.ConnectPlantConstr).FirstOrDefault());

                    if (lostReasonsByOffer != null)
                    {
                        LeadsList[0].LostReasonsByOffer = lostReasonsByOffer;
                    }

                    LeadsEditViewModel leadsEditViewModel = new LeadsEditViewModel();
                    LeadsEditView leadsEditView = new LeadsEditView();

                    if (GeosApplication.Instance.IsPermissionReadOnly)
                    {
                        leadsEditViewModel.IsControlEnable = true;
                        leadsEditViewModel.IsStatusChangeAction = true;
                        leadsEditViewModel.IsAcceptControlEnableorder = false;

                        //[CRM-M040-02] (#49016) Validate Eng. Analysis 
                        //If user has engineering permission then he can edit eng analysis IsCompleted not other fields and save
                        leadsEditViewModel.IsAcceptEnable = false;
                        if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(x => x.IdPermission == 27))
                        {
                            leadsEditViewModel.IsAcceptEnable = true;
                        }

                        leadsEditViewModel.InItLeadsEditReadonly(LeadsList);
                    }
                    else
                    {
                        leadsEditViewModel.ForLeadOpen = true;
                        leadsEditViewModel.InIt(LeadsList);
                    }

                    EventHandler handle = delegate { leadsEditView.Close(); };
                    leadsEditViewModel.RequestClose += handle;

                    leadsEditView.DataContext = leadsEditViewModel;

                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                    {
                        DXSplashScreen.Close();
                    }

                    var ownerInfo = (obj as FrameworkElement);
                    leadsEditView.Owner = Window.GetWindow(ownerInfo);
                    leadsEditView.ShowDialogWindow();

                    // This code is used to update treelist control if any offer changed.
                    if (leadsEditViewModel.OfferData != null)
                    {
                        //Update current row info.
                        dataCurrentRow["OfferCode"] = leadsEditViewModel.OfferData.Code;
                        dataCurrentRow["Plant"] = leadsEditViewModel.OfferData.Site.SiteNameWithoutCountry;
                        dataCurrentRow["Country"] = leadsEditViewModel.OfferData.Site.Country.Name;
                        dataCurrentRow["Title"] = leadsEditViewModel.OfferData.Description;
                        dataCurrentRow["Date"] = leadsEditViewModel.OfferData.DeliveryDate;
                        if (leadsEditViewModel.OfferData.CarOEM != null)
                        {
                            dataCurrentRow["CarOEM"] = leadsEditViewModel.OfferData.CarOEM.Name;
                        }

                        DataRow dataCurrentRowParent = DataTableTopProjects.AsEnumerable().FirstOrDefault(row => Convert.ToString(row["ChildId"]) == Convert.ToString(dataCurrentRow["ParentId"]));

                        if (dataCurrentRow["Group"].ToString().Trim() != leadsEditViewModel.OfferData.Site.Customers[0].CustomerName)
                        {
                            DataRow existingRowParent = DataTableTopProjects.AsEnumerable().FirstOrDefault(row => Convert.ToString(row["SrNo"]) == leadsEditViewModel.OfferData.CarProject.Name
                                                                             && (row["IdProjectCustomer"] != null ? Convert.ToInt32(row["IdProjectCustomer"]) : 0) == leadsEditViewModel.OfferData.Site.Customers[0].IdCustomer);
                            if (existingRowParent != null)
                            {
                                string currentParent = dataCurrentRow["ParentId"].ToString();

                                dataCurrentRow["ParentId"] = existingRowParent["ChildId"];
                                existingRowParent["OfferCode"] = string.Empty;
                                existingRowParent["Group"] = string.Empty;
                                existingRowParent["Plant"] = string.Empty;
                                existingRowParent["Country"] = string.Empty;
                                existingRowParent["Title"] = string.Empty;
                                existingRowParent["Date"] = DBNull.Value;
                                existingRowParent["CarOEM"] = string.Empty;

                                dataCurrentRow["Amount"] = leadsEditViewModel.OfferData.Value;
                                existingRowParent["Amount"] = Convert.ToDouble(existingRowParent["Amount"]) + leadsEditViewModel.OfferData.Value;
                                dataCurrentRowParent["Amount"] = Convert.ToDouble(dataCurrentRowParent["Amount"]) - leadsEditViewModel.OfferData.Value;

                                //Change current parent data
                                var temp = DataTableTopProjects.AsEnumerable().Where(x => Convert.ToString(x["ParentId"]) == currentParent);
                                if (temp.Count() <= 0)
                                {
                                    dataCurrentRowParent.Delete();
                                }
                                else if (temp.Count() == 1)
                                {
                                    DataRow tempRow = DataTableTopProjects.AsEnumerable().FirstOrDefault(x => Convert.ToString(x["ParentId"]) == currentParent);
                                    if (tempRow != null)
                                    {
                                        dataCurrentRowParent["OfferCode"] = tempRow["OfferCode"];
                                        dataCurrentRowParent["Group"] = tempRow["Group"];
                                        dataCurrentRowParent["Plant"] = tempRow["Plant"];
                                        dataCurrentRowParent["Country"] = tempRow["Country"];
                                        dataCurrentRowParent["Title"] = tempRow["Title"];
                                        dataCurrentRowParent["Date"] = tempRow["Date"];
                                        dataCurrentRowParent["CarOEM"] = tempRow["CarOEM"];
                                    }
                                }
                                else if (temp.Count() > 1)
                                {
                                    DataRow tempRow = DataTableTopProjects.AsEnumerable().FirstOrDefault(x => Convert.ToString(x["ParentId"]) == currentParent);
                                    if (tempRow != null)
                                    {
                                        dataCurrentRowParent["OfferCode"] = string.Empty;
                                        dataCurrentRowParent["Group"] = tempRow["Group"];
                                        dataCurrentRowParent["Plant"] = tempRow["Plant"];
                                        dataCurrentRowParent["Country"] = tempRow["Country"];
                                        dataCurrentRowParent["Title"] = string.Empty;
                                        dataCurrentRowParent["Date"] = DBNull.Value;
                                        dataCurrentRowParent["CarOEM"] = string.Empty;
                                    }
                                }

                                //Update existing New parent row. 
                                var tempRows = DataTableTopProjects.AsEnumerable().Where(x => Convert.ToString(x["ParentId"]) == Convert.ToString(existingRowParent["ChildId"]));
                                if (tempRows.Count() > 1)
                                {
                                    DataRow tempRow = DataTableTopProjects.AsEnumerable().FirstOrDefault(x => Convert.ToString(x["ParentId"]) == Convert.ToString(existingRowParent["ChildId"]));
                                    if (tempRow != null)
                                    {
                                        existingRowParent["Group"] = tempRow["Group"];
                                        existingRowParent["Plant"] = tempRow["Plant"];
                                        existingRowParent["Country"] = tempRow["Country"];
                                    }
                                }

                                //var tempRow = DataTableTopProjects.AsEnumerable().Where(x => Convert.ToString(x["ParentId"]) == Convert.ToString(dataRow["ParentId"]));
                                //dataRow["SrNo"] = tempRow.Count() + 1;
                            }
                            else
                            {
                                string currentParent = dataCurrentRow["ParentId"].ToString();
                                //Add new parent if not exists.
                                DataRow newParent = DataTableTopProjects.NewRow();
                                DataTableTopProjects.Rows.Add(newParent);
                                newParent["SrNo"] = leadsEditViewModel.OfferData.CarProject.Name;
                                newParent["ChildId"] = string.Format("Project_" + (srNoProject + 1));
                                newParent["IdOffer"] = leadsEditViewModel.OfferData.IdOffer;
                                newParent["OfferCode"] = leadsEditViewModel.OfferData.Code;
                                newParent["Group"] = leadsEditViewModel.OfferData.Site.Customers[0].CustomerName;
                                newParent["Plant"] = leadsEditViewModel.OfferData.Site.SiteNameWithoutCountry;
                                newParent["Country"] = leadsEditViewModel.OfferData.Site.Country.Name;
                                newParent["Title"] = leadsEditViewModel.OfferData.Description;
                                newParent["Amount"] = leadsEditViewModel.OfferData.Value;
                                newParent["Date"] = leadsEditViewModel.OfferData.OfferExpectedDate;
                                newParent["IdProjectCustomer"] = leadsEditViewModel.OfferData.Site.Customers[0].IdCustomer;

                                if (leadsEditViewModel.OfferData.CarOEM != null)
                                {
                                    newParent["CarOEM"] = leadsEditViewModel.OfferData.CarOEM.Name;
                                }

                                dataCurrentRowParent["Amount"] = Convert.ToDouble(dataCurrentRowParent["Amount"]) - leadsEditViewModel.OfferData.Value;
                                dataCurrentRow["Amount"] = leadsEditViewModel.OfferData.Value;
                                dataCurrentRow["ParentId"] = string.Format("Project_" + (srNoProject + 1));
                                dataCurrentRow["SrNo"] = "1";

                                var temp = DataTableTopProjects.AsEnumerable().Where(x => Convert.ToString(x["ParentId"]) == currentParent);
                                if (temp.Count() <= 0)      //if prev parent has zero child then delete it.
                                {
                                    dataCurrentRowParent.Delete();
                                }
                                else if (temp.Count() == 1) // if prev parent has one child then set first row data to parent.
                                {
                                    var tempRow = DataTableTopProjects.AsEnumerable().FirstOrDefault(x => Convert.ToString(x["ParentId"]) == currentParent);
                                    dataCurrentRowParent["OfferCode"] = tempRow["OfferCode"];  // leadsEditViewModel.OfferData.Code;
                                    dataCurrentRowParent["Group"] = tempRow["Group"];      //leadsEditViewModel.OfferData.Site.Customers[0].CustomerName;
                                    dataCurrentRowParent["Plant"] = tempRow["Plant"];      //leadsEditViewModel.OfferData.Site.SiteNameWithoutCountry;
                                    dataCurrentRowParent["Country"] = tempRow["Country"];    //leadsEditViewModel.OfferData.Site.Country.Name;
                                    dataCurrentRowParent["Title"] = tempRow["Title"];      //leadsEditViewModel.OfferData.Description;
                                    dataCurrentRowParent["Amount"] = tempRow["Amount"];     //leadsEditViewModel.OfferData.Value;
                                    dataCurrentRowParent["Date"] = tempRow["Date"];       //leadsEditViewModel.OfferData.OfferExpectedDate;
                                    dataCurrentRowParent["CarOEM"] = tempRow["CarOEM"]; //leadsEditViewModel.OfferData.CarOEM.Name;
                                }

                                srNoProject++;
                            }

                            dataCurrentRow["Group"] = leadsEditViewModel.OfferData.Site.Customers[0].CustomerName;
                        }
                        else // else part is group not changed
                        {
                            dataCurrentRow["Group"] = leadsEditViewModel.OfferData.Site.Customers[0].CustomerName;

                            int childCount = DataTableTopProjects.AsEnumerable().Where(row => Convert.ToString(row["ParentId"]) == Convert.ToString(dataCurrentRow["ParentId"])).Count();
                            if (childCount == 1 && dataCurrentRowParent != null)        // Update parent if it have only one child
                            {
                                if (Convert.ToString(dataCurrentRowParent["ParentType"]) == "CarProjectType")    //For project, if they have only one child, then display same info in parent row.
                                {
                                    dataCurrentRowParent["OfferCode"] = leadsEditViewModel.OfferData.Code;
                                }
                                else if (Convert.ToString(dataCurrentRowParent["ParentType"]) == "OfferType")    //For offers put same info also in the parent row.
                                {
                                    dataCurrentRowParent["SrNo"] = leadsEditViewModel.OfferData.Code;
                                }

                                if (leadsEditViewModel.OfferData.CarOEM != null)
                                {
                                    dataCurrentRowParent["CarOEM"] = leadsEditViewModel.OfferData.CarOEM.Name;
                                }

                                dataCurrentRowParent["Group"] = leadsEditViewModel.OfferData.Site.Customers[0].CustomerName;
                                dataCurrentRowParent["Plant"] = leadsEditViewModel.OfferData.Site.SiteNameWithoutCountry;
                                dataCurrentRowParent["Country"] = leadsEditViewModel.OfferData.Site.Country.Name;
                                dataCurrentRowParent["Title"] = leadsEditViewModel.OfferData.Description;
                                dataCurrentRowParent["Date"] = leadsEditViewModel.OfferData.OfferExpectedDate;
                            }

                            //Update amount for all.
                            if (dataCurrentRowParent != null)
                            {
                                if (Convert.ToDouble(dataCurrentRow["Amount"]) < leadsEditViewModel.OfferData.Value)
                                {
                                    double difference = leadsEditViewModel.OfferData.Value - Convert.ToDouble(dataCurrentRow["Amount"]);
                                    dataCurrentRowParent["Amount"] = Convert.ToDouble(dataCurrentRowParent["Amount"]) + difference;
                                    OfferTotal = OfferTotal + difference;
                                }
                                else
                                {
                                    double difference = Convert.ToDouble(dataCurrentRow["Amount"]) - leadsEditViewModel.OfferData.Value;
                                    dataCurrentRowParent["Amount"] = Convert.ToDouble(dataCurrentRowParent["Amount"]) - difference;
                                    OfferTotal = OfferTotal - difference;
                                }
                            }

                            dataCurrentRow["Amount"] = leadsEditViewModel.OfferData.Value;
                        }
                    }
                }

                GeosApplication.Instance.Logger.Log("Method LeadsEditViewWindowShow() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in LeadsEditViewWindowShow() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in LeadsEditViewWindowShow() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in LeadsEditViewWindowShow() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void PopupMenuShowingAction(SchedulerMenuEventArgs parameter)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PopupMenuShowingAction ...", category: Category.Info, priority: Priority.Low);

                if (parameter.Menu.Name == SchedulerMenuItemName.AppointmentMenu)
                {
                    object open = parameter.Menu.Items.FirstOrDefault(x => x is SchedulerBarItem && (string)((SchedulerBarItem)x).Content == "Open");

                    parameter.Menu.Items.Clear();
                    parameter.Menu.Items.Add((SchedulerBarItem)open);

                }

                GeosApplication.Instance.Logger.Log("Method PopupMenuShowingAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in PopupMenuShowingAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for scroll daily event down when expanded it.
        /// </summary>
        /// <param name="obj"></param>
        private void GroupBoxExpandAction(object obj)
        {

            try
            {
                System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(

                new Action(() =>
                {
                    (obj as LayoutGroup).BringIntoView();
                })
            , DispatcherPriority.Loaded);
            }
            catch (Exception ex)
            {

            }
        }

        private void LayoutGroupSalesStatusLoad(object obj)
        {
            ((TableView)obj).FocusedRowHandle = GridControl.InvalidRowHandle;
        }


        /// <summary>
        /// Method for edit offer or order from daily events.
        /// [001][SP-67][skale][16-07-2019][GEOS2-1641]fechas vencidas en sistema CRM
        /// </summary>
        /// <param name="obj"></param>
        private void AllowEditAppointmentAction(EditAppointmentFormEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AllowEditAppointmentAction ...", category: Category.Info, priority: Priority.Low);

                obj.Cancel = true;
                if (Convert.ToInt64(obj.Appointment.Id) > 0)
                {
                    //DXSplashScreen.Show<SplashScreenView>();
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

                    // if open activity then this code exacute.
                    if (Convert.ToInt32(obj.Appointment.StatusKey) == -1)
                    {
                        Activity tempActivity = new Activity();

                        tempActivity = CrmStartUp.GetActivityByIdActivity_V2035(Convert.ToInt64(obj.Appointment.Id));
                        EditActivityViewModel editActivityViewModel = new EditActivityViewModel();
                        EditActivityView editActivityView = new EditActivityView();
                        editActivityViewModel.Init(tempActivity);

                        EventHandler handle = delegate { editActivityView.Close(); };
                        editActivityViewModel.RequestClose += handle;
                        editActivityView.DataContext = editActivityViewModel;

                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                        editActivityView.ShowDialogWindow();

                        if (editActivityViewModel.objActivity != null)
                        {

                            TempAppointment modelAppointment = Appointments.FirstOrDefault(x => x.IdOffer == Convert.ToInt64(obj.Appointment.Id)
                                                                                              && Convert.ToInt32(obj.Appointment.StatusKey) == -1);

                            TempAppointment filterModelAppointment = FilterAppointments.FirstOrDefault(x => x.IdOffer == Convert.ToInt64(obj.Appointment.Id)
                                                                                               && Convert.ToInt32(obj.Appointment.StatusKey) == -1);

                            modelAppointment.Tag = editActivityViewModel.objActivity.LookupValue.Value;
                            modelAppointment.Label = editActivityViewModel.objActivity.LookupValue.IdLookupValue;
                            modelAppointment.Subject = editActivityViewModel.objActivity.Subject;
                            modelAppointment.Description = editActivityViewModel.objActivity.Subject;
                            modelAppointment.StartTime = editActivityViewModel.objActivity.FromDate;
                            modelAppointment.EndTime = editActivityViewModel.objActivity.ToDate;

                            filterModelAppointment = modelAppointment;

                        }
                    }


                    else
                    {
                        //*****Appointment.Subject using for ConnectPlantId.*****

                        bool IsOfferDonePO = Appointments.Where(apnt => apnt.IdOffer == Convert.ToInt64(obj.Appointment.Id) && apnt.ConnectPlantId == obj.Appointment.Subject).Select(apnts => apnts.IsOfferDonePO).FirstOrDefault();
                        Int32 offerPlantId = Convert.ToInt32(obj.Appointment.Subject);

                        List<Offer> LeadsList = new List<Offer>();
                        //[001] added Change Method
                        LeadsList.Add(CrmStartUp.GetOfferDetailsById_V2037(Convert.ToInt64(obj.Appointment.Id), GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, GeosApplication.Instance.CompanyList.Where(cmp => cmp.ConnectPlantId == offerPlantId.ToString()).FirstOrDefault()));

                        LostReasonsByOffer lostReasonsByOffer = CrmStartUp.GetLostReasonsByOffer(Convert.ToInt64(LeadsList[0].IdOffer), GeosApplication.Instance.CompanyList.Where(cmp => cmp.ConnectPlantId == offerPlantId.ToString()).Select(x => x.ConnectPlantConstr).FirstOrDefault());

                        if (lostReasonsByOffer != null)
                        {
                            LeadsList[0].LostReasonsByOffer = lostReasonsByOffer;
                        }

                        // if open order then this code exacute.
                        if (IsOfferDonePO == true)
                        {
                            OrderEditViewModel orderEditViewModel = new OrderEditViewModel();
                            OrderEditView orderEditView = new OrderEditView();

                            orderEditViewModel.IsControlEnable = true;
                            orderEditViewModel.IsControlEnableorder = false;
                            orderEditViewModel.IsStatusChangeAction = true;
                            orderEditViewModel.IsAcceptControlEnableorder = false;
                            orderEditViewModel.InItOrder(LeadsList);

                            EventHandler handle = delegate { orderEditView.Close(); };
                            orderEditViewModel.RequestClose += handle;
                            orderEditView.DataContext = orderEditViewModel;

                            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                            {
                                DXSplashScreen.Close();
                            }

                            orderEditView.ShowDialogWindow();

                            if (orderEditViewModel.OfferData != null)
                            {
                                TempAppointment modelAppointment = Appointments.FirstOrDefault(x => x.IdOffer == Convert.ToInt64(obj.Appointment.Id)
                                                                                                && Convert.ToInt32(obj.Appointment.StatusKey) != -1);

                                TempAppointment filterModelAppointment = FilterAppointments.FirstOrDefault(x => x.IdOffer == Convert.ToInt64(obj.Appointment.Id)
                                                                                                && Convert.ToInt32(obj.Appointment.StatusKey) != -1);

                                modelAppointment.Subject = orderEditViewModel.OfferData.Description;
                                modelAppointment.Description = orderEditViewModel.OfferData.Description;
                                modelAppointment.StartTime = orderEditViewModel.OfferData.DeliveryDate;
                                modelAppointment.EndTime = orderEditViewModel.OfferData.DeliveryDate;

                                filterModelAppointment = modelAppointment;
                            }
                        }

                        // if open Offer then this code exacute.
                        else
                        {
                            LeadsEditViewModel leadsEditViewModel = new LeadsEditViewModel();
                            LeadsEditView leadsEditView = new LeadsEditView();

                            if (GeosApplication.Instance.IsPermissionReadOnly)
                            {
                                leadsEditViewModel.IsControlEnable = true;
                                leadsEditViewModel.IsAcceptControlEnableorder = false;

                                leadsEditViewModel.IsControlEnableorder = false;
                                leadsEditViewModel.IsStatusChangeAction = true;

                                //[CRM-M040-02] (#49016) Validate Eng. Analysis 
                                //If user has engineering permission then he can edit eng analysis IsCompleted not other fields and save
                                leadsEditViewModel.IsAcceptEnable = false;
                                if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(x => x.IdPermission == 27))
                                {
                                    leadsEditViewModel.IsAcceptEnable = true;
                                }

                                leadsEditViewModel.InItLeadsEditReadonly(LeadsList);

                            }
                            else
                            {
                                leadsEditViewModel.ForLeadOpen = true;
                                leadsEditViewModel.InIt(LeadsList);
                            }

                            EventHandler handle = delegate { leadsEditView.Close(); };
                            leadsEditViewModel.RequestClose += handle;

                            leadsEditView.DataContext = leadsEditViewModel;

                            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                            {
                                DXSplashScreen.Close();
                            }

                            leadsEditView.ShowDialogWindow();

                            if (leadsEditViewModel.OfferData != null)
                            {
                                TempAppointment modelAppointment = Appointments.FirstOrDefault(x => x.IdOffer == Convert.ToInt64(obj.Appointment.Id)
                                                                                                 && Convert.ToInt32(obj.Appointment.StatusKey) != -1);

                                TempAppointment filterModelAppointment = FilterAppointments.FirstOrDefault(x => x.IdOffer == Convert.ToInt64(obj.Appointment.Id)
                                                                                                && Convert.ToInt32(obj.Appointment.StatusKey) != -1);

                                modelAppointment.Subject = leadsEditViewModel.OfferData.Description;
                                modelAppointment.Description = leadsEditViewModel.OfferData.Description;
                                modelAppointment.StartTime = leadsEditViewModel.OfferData.DeliveryDate;
                                modelAppointment.EndTime = leadsEditViewModel.OfferData.DeliveryDate;

                                filterModelAppointment = modelAppointment;
                            }

                        }
                    }
                }

                GeosApplication.Instance.Logger.Log("Method AllowEditAppointmentAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in AllowEditAppointmentAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in AllowEditAppointmentAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in AllowEditAppointmentAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for fill after form load.
        /// </summary>
        /// <param name="obj"></param>
        private void LoadAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method LoadAction ...", category: Category.Info, priority: Priority.Low);

                SelectedDates1 = GeosApplication.Instance.ServerDateTime.Date;
                LeadsList = new List<Offer>();

                OffersWithoutPurchaseOrdeList = new ObservableCollection<AppointmentModel>();
                TempAppointment apt = null;

                DateTime baseTime = GeosApplication.Instance.ServerDateTime.Date; ;

                GeosApplication.Instance.Logger.Log("Method LoadAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in LoadAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        /// <summary>
        /// method for select date action.
        /// </summary>
        /// <param name="parameter"></param>
        private void SelectDatesAction(EventArgs parameter)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SelectDatesAction ...", category: Category.Info, priority: Priority.Low);

                if (SelectedDates.Count > 0)
                {
                    fromDate = SelectedDates[0].Date;
                    toDate = SelectedDates[0].Date;

                    if (SelectedDates.Count > 1)
                    {
                        toDate = SelectedDates[SelectedDates.Count - 1].Date;
                    }

                    ProjectTasksDeliveryList.Clear();

                    if (SelectedFilters != null)
                    {
                        foreach (var titem in SelectedFilters)
                        {
                            DataHelper data = titem as DataHelper;
                            LeadsList = new List<Offer>();
                            LeadsOptionsByOfferList = new List<OptionsByOffer>();
                            List<Offer> offerList = new List<Offer>();
                            List<OptionsByOffer> OptionsByOfferList = new List<OptionsByOffer>();

                            // Continue loop although some plant is not available and Show error message.
                            foreach (var itemCompaniesDetails in GeosApplication.Instance.CompanyList)
                            {
                                try
                                {
                                    //offerList.AddRange(CrmStartUp.GetOffersWithoutPurchaseOrder(itemCompaniesDetails.IdCompany, GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.CrmOfferYear,itemCompaniesDetails).ToList());
                                    //OptionsByOfferList.AddRange(CrmStartUp.GetoptionsByOffer(itemCompaniesDetails.IdCompany, GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.CrmOfferYear, itemCompaniesDetails).ToList());
                                }
                                catch (FaultException<ServiceException> ex)
                                {
                                    GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemCompaniesDetails.Alias, " Failed");
                                    if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                        FailedPlants.Add(itemCompaniesDetails.ShortName);
                                    if (FailedPlants != null && FailedPlants.Count > 0)
                                    {
                                        IsShowFailedPlantWarning = true;
                                        WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                                        WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                                    }
                                    System.Threading.Thread.Sleep(1000);
                                    GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemCompaniesDetails.Alias, " Failed ", ex.Detail.ErrorMessage), category: Category.Info, priority: Priority.Low);
                                }
                                catch (ServiceUnexceptedException ex)
                                {
                                    GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemCompaniesDetails.Alias, " Failed");
                                    if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                        FailedPlants.Add(itemCompaniesDetails.ShortName);
                                    if (FailedPlants != null && FailedPlants.Count > 0)
                                    {
                                        IsShowFailedPlantWarning = true;
                                        WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                                        WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                                    }
                                    System.Threading.Thread.Sleep(1000);
                                    GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemCompaniesDetails.Alias, " Failed ", ex.Message), category: Category.Info, priority: Priority.Low);
                                }
                                catch (Exception ex)
                                {
                                    GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemCompaniesDetails.Alias, " Failed");
                                    if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                        FailedPlants.Add(itemCompaniesDetails.ShortName);
                                    if (FailedPlants != null && FailedPlants.Count > 0)
                                    {
                                        IsShowFailedPlantWarning = true;
                                        WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                                        WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                                    }
                                    System.Threading.Thread.Sleep(1000);
                                    GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemCompaniesDetails.Alias, " Failed ", ex.Message), category: Category.Info, priority: Priority.Low);
                                }
                            }

                            if (FailedPlants == null || FailedPlants.Count == 0)
                            {

                                IsShowFailedPlantWarning = false;
                                WarningFailedPlants = string.Empty;
                            }

                            LeadsList = offerList;
                            LeadsOptionsByOfferList = OptionsByOfferList;
                            foreach (var item in LeadsList)
                            {
                                OffersWithoutPurchaseOrdeList.Add(new AppointmentModel() { Data = item, Subject = item.Description, ResourceId = item.IdOffer, StartTime = item.DeliveryDate.Value, Label = 1, EndTime = item.DeliveryDate.Value, Description = item.Description, AppointmentType = 1 });
                            }
                        }
                    }
                }

                GeosApplication.Instance.Logger.Log("Method SelectDatesAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in SelectDatesAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in SelectDatesAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SelectDatesAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for chart load action.
        /// </summary>
        /// <param name="obj"></param>
        private void ChartLoadAction(object obj)
        {
            // CultureInfo.GetCultureInfo(cultureName).NumberFormat.CurrencySymbol;
            //var d = CultureInfo.GetCultureInfo(GeosApplication.Instance.CurrentCulture).NumberFormat.CurrencySymbol;
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChartLoadAction ...", category: Category.Info, priority: Priority.Low);

                chartControl = (ChartControl)obj;
                // chartcontrol.DataSource = GraphDataTable;
                chartControl.BeginInit();
                XYDiagram2D diagram = new XYDiagram2D();
                chartControl.Diagram = diagram;
                diagram.ActualAxisX.Title = new AxisTitle() { Content = "Months" };
                diagram.ActualAxisY.Title = new AxisTitle() { Content = "Amount" };
                diagram.ActualAxisX.QualitativeScaleOptions = new QualitativeScaleOptions();
                diagram.ActualAxisX.QualitativeScaleOptions.AutoGrid = false;
                GeosApplication.SetCurrencySymbol(GeosApplication.Instance.CurrentCurrencySymbol);
                diagram.ActualAxisY.NumericOptions = new NumericOptions();
                diagram.ActualAxisY.NumericOptions.Format = NumericFormat.Currency;

                // Changed list as per requirement
                List<SalesStatusType> CopyListSalesStatusType = new List<SalesStatusType>();
                CopyListSalesStatusType.Add(ListAllSalesStatusType.Where(item => item.IdSalesStatusType == 5).SingleOrDefault());
                foreach (var itemsst in ListAllSalesStatusType.Where(item => item.IdSalesStatusType != 5 && item.IdSalesStatusType != 0).ToList())
                {
                    CopyListSalesStatusType.Add(itemsst);
                }
                CopyListSalesStatusType.Reverse();

                foreach (var item in CopyListSalesStatusType)
                {
                    if (item != null)
                    {
                        BarSideBySideStackedSeries2D barSideBySideStackedSeries2D = new BarSideBySideStackedSeries2D();
                        barSideBySideStackedSeries2D.ArgumentScaleType = ScaleType.Auto;
                        barSideBySideStackedSeries2D.ValueScaleType = ScaleType.Numerical;
                        barSideBySideStackedSeries2D.DisplayName = item.Name;
                        barSideBySideStackedSeries2D.Brush = (SolidColorBrush)(new BrushConverter().ConvertFrom(item.HtmlColor.ToString()));
                        // barSideBySideStackedSeries2D.ArgumentDataMember = "Month";
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

                chartControl.EndInit();
                chartControl.AnimationMode = ChartAnimationMode.OnDataChanged;
                chartControl.Animate();

                GeosApplication.Instance.Logger.Log("Method ChartLoadAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in ChartLoadAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in ChartLoadAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ChartLoadAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
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
                GeosApplication.Instance.Logger.Log("Get an error in ChartDashboardSaleCustomDrawCrosshairCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        /// <summary>
        /// ******
        /// </summary>
        /// <returns></returns>
        private void CreateTable()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CreateTable ...", category: Category.Info, priority: Priority.Low);

                dt.Rows.Clear();
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
                                dr[k] = SalesStatusByMonthList.Where(m => m.SaleStatusName == item.ToString() && m.CurrentMonth == mt).Select(mv => mv.Value).ToList().Sum();
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
                                dr[k] = SalesStatusByMonthList.Where(m => m.SaleStatusName == item.ToString() && m.CurrentMonth == monthstart && m.CurrentYear == yearstart).Select(mv => mv.Value).ToList().Sum();
                                k++;
                            }
                        }
                        dt.Rows.Add(dr);
                    }
                }

                GeosApplication.Instance.Logger.Log("Method CreateTable() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CreateTable() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

            GraphDataTable = dt;
            if (chartControl != null) { chartControl.UpdateData(); }
        }

        /// <summary>
        /// Method for fill sales performance target.
        /// </summary>
        private void FillSalesPerformanceTarget()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method fillSalesPerformanceTarget ...", category: Category.Info, priority: Priority.Low);

                //SalesPerformanceTargetList = new List<SalesTargetBySite>();
                //SalesPerformanceTargetList.AddRange(CrmStartUp.GetSalesStatusTargetDashboard(GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.CrmOfferYear, GeosApplication.Instance.IdUserPermission));
                //TargetAmount = Convert.ToDouble(SalesPerformanceTargetList[0].TargetAmount);

                // SalesUserQuota salesUser = CrmStartUp.GetTotalSalesQuotaBySelectedUserIdAndYear(GeosApplication.Instance.ActiveUser.IdUser.ToString(), GeosApplication.Instance.IdCurrencyByRegion, Convert.ToInt32(GeosApplication.Instance.CrmOfferYear));
                SalesUserQuota salesUser = CrmStartUp.GetTotalSalesQuotaBySelectedUserIdAndYearWithExchangeRate(GeosApplication.Instance.ActiveUser.IdUser.ToString(), GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate);
                TargetAmount = salesUser.SalesQuotaAmountWithExchangeRate;

                double Won = SalesPerformanceList.Where(s => s.SaleStatusName == "WON").Select(x => x.Value).ToList().Sum();

                if (TargetAmount == 0)
                {
                    TargetAchieved = Math.Round(0.00, 2);
                }
                else
                {
                    TargetAchieved = Math.Round(((Won / TargetAmount) * 100), 2);
                }

                double Lost = SalesPerformanceList.Where(s => s.SaleStatusName == "LOST").Select(x => x.Value).ToList().Sum();
                SuccessRate = ((Won + Lost) > 0) ? Math.Round(((Won / (Won + Lost)) * 100), 2) : 0;

                TempRemainingAmmount = Math.Round(Won - TargetAmount);
                RemainingAmmount = Math.Abs(TempRemainingAmmount);

                if (TempRemainingAmmount >= 0)
                {
                    IsMyValueNegative = false;
                }
                else
                {
                    IsMyValueNegative = true;
                }



                SalePerfomanceDictionary = new ObservableCollection<Dictionary<string, object>>();
                Dictionary<String, Object> SalePerfomanceDictionaryItems = new Dictionary<string, object>();
                SalePerfomanceDictionaryItems.Add("Name", "SUCCESS RATE");
                SalePerfomanceDictionaryItems.Add("Text", SuccessRate);
                SalePerfomanceDictionary.Add(SalePerfomanceDictionaryItems);

                Dictionary<String, Object> SalePerfomanceDictionaryItems1 = new Dictionary<string, object>();
                SalePerfomanceDictionaryItems1.Add("Name", "TARGET ACHIEVED");
                SalePerfomanceDictionaryItems1.Add("Text", TargetAchieved);
                SalePerfomanceDictionary.Add(SalePerfomanceDictionaryItems1);

                Dictionary<String, Object> SalePerfomanceDictionaryItems3 = new Dictionary<string, object>();
                SalePerfomanceDictionaryItems3.Add("Name", "REMAINING");
                SalePerfomanceDictionaryItems3.Add("Text", RemainingAmmount);
                SalePerfomanceDictionary.Add(SalePerfomanceDictionaryItems3);

                GaugeSuccessRate = SuccessRate;
                GaugeTargetAchieved = TargetAchieved;

                GeosApplication.Instance.Logger.Log("Method fillSalesPerformanceTarget() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in fillSalesPerformanceTarget() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in fillSalesPerformanceTarget() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in fillSalesPerformanceTarget() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for fill sales performance target as per user.
        /// </summary>
        private void FillSalesPerformanceTargetByUser()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method fillSalesPerformanceTarget ...", category: Category.Info, priority: Priority.Low);

                //if User Permission in 22
                if (GeosApplication.Instance.IdUserPermission == 22)
                {
                    List<Company> plantOwners = GeosApplication.Instance.SelectedPlantOwnerUsersList.Cast<Company>().ToList();
                    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.ConnectPlantId));

                    //PlantBusinessUnitSalesQuota plantBusinessUnitSalesQuota = CrmStartUp.GetTotalPlantQuotaSelectedPlantWiseAndYear(plantOwnersIds, GeosApplication.Instance.IdCurrencyByRegion, Convert.ToInt32(GeosApplication.Instance.CrmOfferYear), GeosApplication.Instance.ActiveUser.IdUser);
                    PlantBusinessUnitSalesQuota plantBusinessUnitSalesQuota = CrmStartUp.GetTotalPlantQuotaSelectedPlantWiseAndYearWithExchangeDate(plantOwnersIds, GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, GeosApplication.Instance.ActiveUser.IdUser);
                    TargetAmount = plantBusinessUnitSalesQuota.QuotaAmountWithExchangeRate;
                }

                //if user User Permission in 21
                if (GeosApplication.Instance.IdUserPermission == 21)
                {
                    List<UserManagerDtl> salesOwners = GeosApplication.Instance.SelectedSalesOwnerUsersList.Cast<UserManagerDtl>().ToList();
                    var salesOwnersIds = string.Join(",", salesOwners.Select(i => i.IdUser));
                    //SalesUserQuota salesUser = CrmStartUp.GetTotalSalesQuotaBySelectedUserIdAndYear(salesOwnersIds.ToString(), GeosApplication.Instance.IdCurrencyByRegion, Convert.ToInt32(GeosApplication.Instance.CrmOfferYear));
                    SalesUserQuota salesUser = CrmStartUp.GetTotalSalesQuotaBySelectedUserIdAndYearWithExchangeRate(salesOwnersIds.ToString(), GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate);
                    TargetAmount = salesUser.SalesQuotaAmountWithExchangeRate;
                }

                //if user User Permission in 20
                if (GeosApplication.Instance.IdUserPermission == 20)
                {
                    //SalesUserQuota salesUser = CrmStartUp.GetTotalSalesQuotaBySelectedUserIdAndYear(GeosApplication.Instance.ActiveUser.IdUser.ToString(),
                    //                                                                   GeosApplication.Instance.IdCurrencyByRegion, Convert.ToInt32(GeosApplication.Instance.CrmOfferYear));
                    SalesUserQuota salesUser = CrmStartUp.GetTotalSalesQuotaBySelectedUserIdAndYearWithExchangeRate(GeosApplication.Instance.ActiveUser.IdUser.ToString(), GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate);
                    TargetAmount = salesUser.SalesQuotaAmountWithExchangeRate;
                }

                double Won = SalesPerformanceList.Where(s => s.SaleStatusName == "WON").Select(x => x.Value).ToList().Sum();

                if (TargetAmount == 0)
                {
                    TargetAchieved = Math.Round(0.00, 2);
                }
                else
                {
                    TargetAchieved = Math.Round(((Won / TargetAmount) * 100), 2);
                }

                double Lost = SalesPerformanceList.Where(s => s.SaleStatusName == "LOST").Select(x => x.Value).ToList().Sum();
                SuccessRate = ((Won + Lost) > 0) ? Math.Round(((Won / (Won + Lost)) * 100), 2) : 0;

                TempRemainingAmmount = Math.Round(Won - TargetAmount);
                RemainingAmmount = Math.Abs(TempRemainingAmmount);
                //  ListAllSalesStatusType.FirstOrDefault(x => x.Name == "REMAINING").TotalAmount = RemainingAmmount;

                if (TempRemainingAmmount >= 0)
                {
                    IsMyValueNegative = false;
                }
                else
                {
                    IsMyValueNegative = true;
                }

                SalePerfomanceDictionary = new ObservableCollection<Dictionary<string, object>>();
                Dictionary<String, Object> SalePerfomanceDictionaryItems = new Dictionary<string, object>();
                SalePerfomanceDictionaryItems.Add("Name", "SUCCESS RATE");
                SalePerfomanceDictionaryItems.Add("Text", SuccessRate);
                SalePerfomanceDictionary.Add(SalePerfomanceDictionaryItems);

                Dictionary<String, Object> SalePerfomanceDictionaryItems1 = new Dictionary<string, object>();
                SalePerfomanceDictionaryItems1.Add("Name", "TARGET ACHIEVED");
                SalePerfomanceDictionaryItems1.Add("Text", TargetAchieved);
                SalePerfomanceDictionary.Add(SalePerfomanceDictionaryItems1);

                Dictionary<String, Object> SalePerfomanceDictionaryItems3 = new Dictionary<string, object>();
                SalePerfomanceDictionaryItems3.Add("Name", "REMAINING");
                SalePerfomanceDictionaryItems3.Add("Text", RemainingAmmount);
                SalePerfomanceDictionary.Add(SalePerfomanceDictionaryItems3);

                GaugeSuccessRate = SuccessRate;
                GaugeTargetAchieved = TargetAchieved;

                GeosApplication.Instance.Logger.Log("Method fillSalesPerformanceTarget() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in fillSalesPerformanceTarget() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in fillSalesPerformanceTarget() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in fillSalesPerformanceTarget() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Fill and show top 10 projects or offers.
        /// </summary>
        private void FillTopProjects()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillTopProjects ...", category: Category.Info, priority: Priority.Low);

                if (DataTableTopProjects == null)
                {
                    DataTableTopProjects = new DataTable();
                    DataTableTopProjects.Columns.Add("SrNo", typeof(string)).DefaultValue = 0;
                    DataTableTopProjects.Columns.Add("ChildId", typeof(string)).DefaultValue = string.Empty;
                    DataTableTopProjects.Columns.Add("ParentId", typeof(string)).DefaultValue = string.Empty;
                    DataTableTopProjects.Columns.Add("CarOEM", typeof(string)).DefaultValue = string.Empty;
                    DataTableTopProjects.Columns.Add("IdOffer", typeof(string)).DefaultValue = string.Empty;
                    DataTableTopProjects.Columns.Add("OfferCode", typeof(string)).DefaultValue = string.Empty;
                    DataTableTopProjects.Columns.Add("Group", typeof(string)).DefaultValue = string.Empty;
                    DataTableTopProjects.Columns.Add("Plant", typeof(string)).DefaultValue = string.Empty;
                    DataTableTopProjects.Columns.Add("Country", typeof(string)).DefaultValue = string.Empty;
                    DataTableTopProjects.Columns.Add("Title", typeof(string)).DefaultValue = string.Empty;
                    DataTableTopProjects.Columns.Add("Amount", typeof(double)).DefaultValue = 0;
                    DataTableTopProjects.Columns.Add("Date", typeof(DateTime));
                    DataTableTopProjects.Columns.Add("ParentType", typeof(string)).DefaultValue = string.Empty;
                    DataTableTopProjects.Columns.Add("IdProjectCustomer", typeof(int)).DefaultValue = 0;
                    DataTableTopProjects.Columns.Add("ConnectPlantId", typeof(string)).DefaultValue = string.Empty;
                }

                DataTableTopProjects.Rows.Clear();
                DtTopProjects = DataTableTopProjects.Copy();

                //Create final list of car projects
                FinalListCarProject = new List<CarProjectDetail>();
                foreach (CarProjectDetail itemCarProject in ListCarProject)
                {
                    if (FinalListCarProject.Any(x => x.IdCarProject == itemCarProject.IdCarProject))
                    {
                        CarProjectDetail carProject = FinalListCarProject.FirstOrDefault(x => x.IdCarProject == itemCarProject.IdCarProject);
                        carProject.ProjectOfferAmount = carProject.ProjectOfferAmount + itemCarProject.ProjectOfferAmount;
                        carProject.Offers.AddRange(itemCarProject.Offers);
                        carProject.Offers = carProject.Offers.OrderByDescending(x => x.Value).ToList();
                    }
                    else
                    {
                        FinalListCarProject.Add(itemCarProject);
                    }
                }

                //Get unnamed car project and lists.
                CarProjectDetail UnNamedCarProject = FinalListCarProject.FirstOrDefault(x => x.IdCarProject == 0 && x.Name == "UNNAMED PROJECT");
                List<OfferDetail> UnNamedProjectOffersList = new List<OfferDetail>();
                if (UnNamedCarProject != null)
                {
                    UnNamedProjectOffersList = UnNamedCarProject.Offers.Take(GeosApplication.Instance.CrmTopOffers).ToList();
                    FinalListCarProject.Remove(UnNamedCarProject);
                }

                //Get Top Projects.
                FinalListCarProject = FinalListCarProject.OrderByDescending(x => x.ProjectOfferAmount).Take(GeosApplication.Instance.CrmTopOffers).ToList();

                //The offers linked to some project should be grouped by customer 
                //group in order to identify the top projects.
                List<CarProjectDetail> FinalListOfProjects = new List<CarProjectDetail>();
                foreach (var item in FinalListCarProject)
                {
                    var groups = item.Offers.GroupBy(x => x.IdCustomer).ToList();

                    foreach (var offer in groups)
                    {
                        CarProjectDetail tempCarProject = new CarProjectDetail();
                        tempCarProject.Name = item.Name;
                        tempCarProject.Offers = new List<OfferDetail>();
                        tempCarProject.Offers.AddRange(offer.ToList());
                        tempCarProject.ProjectOfferAmount = tempCarProject.Offers.Sum(x => x.Value);

                        tempCarProject.IdCustomer = offer.Key;
                        FinalListOfProjects.Add(tempCarProject);
                    }
                }

                FinalListOfProjects = FinalListOfProjects.OrderByDescending(x => x.ProjectOfferAmount).ToList();

                List<object> FinalListOfProjectsOrOffers = new List<object>();

                foreach (CarProjectDetail carProject in FinalListOfProjects)
                {
                    var offersList = UnNamedProjectOffersList.Where(x => x.Value > carProject.ProjectOfferAmount);

                    if (offersList.Any())
                    {
                        FinalListOfProjectsOrOffers.AddRange(offersList);
                        UnNamedProjectOffersList.RemoveAll(x => x.Value > carProject.ProjectOfferAmount);
                        FinalListOfProjectsOrOffers.Add(carProject);
                    }
                    else
                    {
                        FinalListOfProjectsOrOffers.Add(carProject);
                    }
                }

                if (UnNamedProjectOffersList != null && UnNamedProjectOffersList.Count > 0)
                {
                    FinalListOfProjectsOrOffers.AddRange(UnNamedProjectOffersList);
                }

                FinalListOfProjectsOrOffers = FinalListOfProjectsOrOffers.Take(GeosApplication.Instance.CrmTopOffers).ToList();

                //Add Project as Parent Node
                OfferTotal = 0.0;
                int srNoChild = 1;

                foreach (var item in FinalListOfProjectsOrOffers)
                {
                    DataRow dataRowParent = DtTopProjects.NewRow();
                    dataRowParent["ChildId"] = string.Format("Project_" + srNoProject);
                    DtTopProjects.Rows.Add(dataRowParent);       //Add Parent row.

                    if (item is CarProjectDetail)
                    {
                        CarProjectDetail itemCarProject = (CarProjectDetail)item;
                        dataRowParent["SrNo"] = itemCarProject.Name;
                        dataRowParent["ParentType"] = "CarProjectType";
                        dataRowParent["IdProjectCustomer"] = itemCarProject.IdCustomer;

                        OfferTotal = OfferTotal + (itemCarProject.ProjectOfferAmount.HasValue ? itemCarProject.ProjectOfferAmount.Value : 0.0);

                        //Add Offers as Child Nodes.
                        int SrNo = 1;
                        //bool isUpdatedParent = false;

                        if (itemCarProject.Offers != null && itemCarProject.Offers.Count > 0)
                        {
                            if (itemCarProject.Offers.GroupBy(i => i.Code).Count() == 1)
                            {
                                dataRowParent["OfferCode"] = itemCarProject.Offers[0].Code;
                            }

                            if (itemCarProject.Offers.GroupBy(i => i.CustomerName).Count() == 1)
                            {
                                dataRowParent["Group"] = itemCarProject.Offers[0].CustomerName;
                            }

                            if (itemCarProject.Offers.GroupBy(i => i.SiteName).Count() == 1)
                            {
                                dataRowParent["Plant"] = itemCarProject.Offers[0].SiteName;
                            }

                            if (itemCarProject.Offers.GroupBy(i => i.CountryName).Count() == 1)
                            {
                                dataRowParent["Country"] = itemCarProject.Offers[0].CountryName;
                            }

                            if (itemCarProject.Offers.GroupBy(i => i.OfferExpectedDate).Count() == 1)
                            {
                                dataRowParent["Date"] = itemCarProject.Offers[0].OfferExpectedDate;
                            }

                            if (itemCarProject.Offers.GroupBy(i => i.Description).Count() == 1)
                            {
                                dataRowParent["Title"] = itemCarProject.Offers[0].Description;
                            }

                            if (itemCarProject.Offers.GroupBy(i => i.CarOEMName == "" ? null : i.CarOEMName).Count() == 1)
                            {
                                if (!string.IsNullOrEmpty(itemCarProject.Offers[0].CarOEMName))
                                    dataRowParent["CarOEM"] = itemCarProject.Offers[0].CarOEMName;
                            }
                        }

                        foreach (OfferDetail offer in itemCarProject.Offers)
                        {
                            try
                            {
                                // Add sum (amount) of child to parent.
                                dataRowParent["Amount"] = Convert.ToDouble(dataRowParent["Amount"]) + offer.Value;

                                DataRow dataRowChild = DtTopProjects.NewRow();       //Add Child row as Offer.
                                dataRowChild["SrNo"] = SrNo;
                                dataRowChild["ChildId"] = srNoChild;
                                dataRowChild["IdOffer"] = offer.IdOffer;
                                dataRowChild["ParentId"] = string.Format("Project_" + srNoProject);
                                if (!string.IsNullOrEmpty(offer.CarOEMName))
                                {
                                    dataRowChild["CarOEM"] = offer.CarOEMName;
                                }
                                dataRowChild["OfferCode"] = offer.Code;
                                dataRowChild["Group"] = offer.CustomerName;
                                dataRowChild["Plant"] = offer.SiteName;
                                dataRowChild["Country"] = offer.CountryName;
                                dataRowChild["Title"] = offer.Description;
                                dataRowChild["Amount"] = offer.Value;
                                dataRowChild["Date"] = offer.OfferExpectedDate;
                                dataRowChild["ConnectPlantId"] = offer.ConnectPlantId;

                                DtTopProjects.Rows.Add(dataRowChild);
                                SrNo++;
                                srNoChild++;
                            }
                            catch (Exception ex)
                            {
                                GeosApplication.Instance.Logger.Log("Error in Dashboard 1 - FillTopProjects - Adding offers in projects" + ex.Message, category: Category.Info, priority: Priority.Low);
                            }
                        }
                    }
                    else if (item is OfferDetail)     // Offer always have single child node.
                    {
                        OfferDetail itemOffer = (OfferDetail)item;
                        dataRowParent["SrNo"] = itemOffer.Code;
                        //For Offers - Parent node same info as child node as there is only one child.
                        dataRowParent["Amount"] = itemOffer.Value;
                        dataRowParent["Group"] = itemOffer.CustomerName;
                        dataRowParent["Plant"] = itemOffer.SiteName;
                        dataRowParent["Country"] = itemOffer.CountryName;
                        dataRowParent["Title"] = itemOffer.Description;
                        dataRowParent["Date"] = itemOffer.OfferExpectedDate;
                        dataRowParent["ParentType"] = "OfferType";
                        if (!string.IsNullOrEmpty(itemOffer.CarOEMName))
                        {
                            dataRowParent["CarOEM"] = itemOffer.CarOEMName;
                        }


                        //Total amount for Top projects/offers.
                        OfferTotal = OfferTotal + itemOffer.Value;

                        DataRow dataRowChild = DtTopProjects.NewRow();       //Add Child row for Offer.
                        dataRowChild["SrNo"] = 1;
                        dataRowChild["ChildId"] = srNoChild;
                        dataRowChild["IdOffer"] = itemOffer.IdOffer;
                        dataRowChild["ParentId"] = string.Format("Project_" + srNoProject);
                        if (!string.IsNullOrEmpty(itemOffer.CarOEMName))
                        {
                            dataRowParent["CarOEM"] = itemOffer.CarOEMName;
                        }
                        dataRowChild["OfferCode"] = itemOffer.Code;
                        dataRowChild["Group"] = itemOffer.CustomerName;
                        dataRowChild["Plant"] = itemOffer.SiteName;
                        dataRowChild["Country"] = itemOffer.CountryName;
                        dataRowChild["Title"] = itemOffer.Description;
                        dataRowChild["Amount"] = itemOffer.Value;
                        dataRowChild["Date"] = itemOffer.OfferExpectedDate;
                        dataRowChild["ConnectPlantId"] = itemOffer.ConnectPlantId;

                        DtTopProjects.Rows.Add(dataRowChild);
                        srNoChild++;
                    }

                    srNoProject++;
                }

                DataTableTopProjects = DtTopProjects;
                GeosApplication.Instance.Logger.Log("Method FillTopProjects() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillTopProjects() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for sort top offers as per amount.
        /// </summary>
        /// <param name="e"></param>
        private void tableView1_CustomColumnSort(DevExpress.Xpf.Grid.TreeList.TreeListCustomColumnSortEventArgs e)
        {
            GeosApplication.Instance.Logger.Log("Method tableView1_CustomColumnSort ...", category: Category.Info, priority: Priority.Low);

            try
            {
                if (e.Node1.HasChildren || e.Node2.HasChildren)
                {
                    if (e.Column.FieldName == "SrNo")
                    {
                        e.Result = string.Compare(Convert.ToString((e.Node1.Content as DataRowView).Row[(e.Column).FieldName]),
                                              Convert.ToString((e.Node2.Content as DataRowView).Row[(e.Column).FieldName]));
                    }
                    else if (e.Column.FieldName == "Amount")
                    {
                        e.Result = Convert.ToDouble((e.Node1.Content as DataRowView).Row[(e.Column).FieldName]) <
                            Convert.ToDouble((e.Node2.Content as DataRowView).Row[(e.Column).FieldName]) ? -1 : 1;
                    }
                }
                else    //Sort Child nodes by Descending
                {
                    if (e.SortOrder == DevExpress.Data.ColumnSortOrder.Ascending)
                    {
                        e.Result = Convert.ToInt32((e.Node1.Content as DataRowView).Row["SrNo"]) >
                                Convert.ToInt32((e.Node2.Content as DataRowView).Row["SrNo"]) ? 1 : -1;
                    }
                    else if (e.SortOrder == DevExpress.Data.ColumnSortOrder.Descending)
                    {
                        e.Result = Convert.ToInt32((e.Node1.Content as DataRowView).Row["SrNo"]) >
                                Convert.ToInt32((e.Node2.Content as DataRowView).Row["SrNo"]) ? -1 : 1;
                    }
                }
                e.Handled = true;

                GeosApplication.Instance.Logger.Log("Method tableView1_CustomColumnSort() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in tableView1_CustomColumnSort() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        /// <summary>
        /// This method is used to Display Custom numbers for offers in Car Project
        /// </summary>
        /// <param name="e"></param>
        private void tableView1_CustomUnboundColumnData(TreeListUnboundColumnDataEventArgs e)
        {
            var nd = e.Node;
            var rh = nd.RowHandle;
            var p = nd.ParentNode;
            if (p != null)
            {
                var prh = p.RowHandle;
                var diff = rh - prh;
                e.Value = diff;
            }
            else
            {
                e.Value = ((System.Data.DataRowView)(e.Node.Content)).Row["SrNo"];
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion

        public void BindActionBarChartList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method BindActionBarChartList()...", category: Category.Info, priority: Priority.Low);

                MainStatusList = new ObservableCollection<MainStatus>();

                //StatusList = new List<LookupValue>();
                //if (GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
                //{
                //    List<Company> plantOwners = GeosApplication.Instance.SelectedPlantOwnerUsersList.Cast<Company>().ToList();
                //    string plantOwnersIds = string.Join(",", plantOwners.Select(i => i.ConnectPlantId));

                //    StatusList = new List<LookupValue>(CrmStartUp.GetActionsCountOfStatus(GeosApplication.Instance.ActiveUser.IdUser.ToString(),
                //                                                                            plantOwnersIds,
                //                                                                            GeosApplication.Instance.ActiveUser.IdUser,
                //                                                                            GeosApplication.Instance.IdUserPermission,
                //                                                                            GeosApplication.Instance.SelectedyearStarDate,
                //                                                                            GeosApplication.Instance.SelectedyearEndDate).ToList().OrderBy(a => a.Position));
                //}

                if (StatusList != null && StatusList.Count > 0)
                {
                    foreach (LookupValue value in StatusList.OrderByDescending(a => a.Position))
                    {
                        MainStatus status = new MainStatus();
                        status.Name = value.Value;
                        status.Color = value.HtmlColor;
                        status.CountValues = new ObservableCollection<LookupValue> {
                                new LookupValue {  Value = "Actions",          Count = value.Count}
                            };

                        MainStatusList.Add(status);
                    }
                }

                GeosApplication.Instance.Logger.Log("Method BindActionBarChartList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in BindActionBarChartList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in BindActionBarChartList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method BindActionBarChartList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
    }



    public class PieChartValuesPlannedVisit
    {
        public string Level { get; set; }
        public double Value { get; set; }
        public string Argument { get; set; }
        public string HtmlColor { get; set; }
    }

    public class MainStatus
    {
        public string Name { get; set; }
        public ObservableCollection<LookupValue> CountValues { get; set; }
        public string Color { get; set; }
    }

}
