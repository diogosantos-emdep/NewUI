using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Charts;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Serialization;
using DevExpress.Xpf.Editors.Flyout;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.TreeList;
using DevExpress.Xpf.LayoutControl;
using DevExpress.Xpf.Printing;
using DevExpress.Xpf.Scheduler;
using DevExpress.Xpf.Scheduler.Menu;
using DevExpress.Xpf.Scheduler.UI;
using DevExpress.Xpf.WindowsUI;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.ERM;
using Emdep.Geos.Data.Common.OptimizedClass;
using Emdep.Geos.Data.Common.PCM;
using Emdep.Geos.Modules.ERM.CommonClasses;
using Emdep.Geos.Modules.ERM.Views;
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
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using DevExpress.Data;
using DevExpress.Utils;
using System.Xml;

namespace Emdep.Geos.Modules.ERM.ViewModels
{
    public class DashboardViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Service

        IERMService ERMService = new ERMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        public ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion

        #region Declaration
        bool isOptionExpand;
        public string RealTimeMonitorFilterSettingFilePath = GeosApplication.Instance.UserSettingFolderName + "ERM_RealTimeMonitorFilterSetting.Xml";
        XYDiagram2D diagram = new XYDiagram2D();
        private ChartControl chartControl;
        private DataTable dt = new DataTable();
        private DataTable graphDataTable;
        //      private ObservableCollection<ERMDeliveeryVisualManagementStages> eRMDeliveeryVisualManagementStagesList;
        private ObservableCollection<ERMDeliveryVisualManagement> eRMDeliveryVisualManagementList;
        private AppointmentLabelCollection labels;
        private DataTable dp = new DataTable();
        private DataTable actiongraphDataTable;
        string fromDate;
        string toDate;
        private List<object> selectedPlantold;
        int isButtonStatus;
        DateTime startDate;
        DateTime endDate;
        Visibility isCalendarVisible;
        private Duration _currentDuration;
        private bool isBusy;
        private bool isPeriod;
        private UInt64 totalStagesQTY;
        #region [GEOS2-4342][Rupali Sarode][11-05-2023]
        private UInt64 conditionQuality;
        private UInt64 conditionGreaterThan7Days;
        private UInt64 conditionDelay;
        private UInt64 conditionLessThanEqual7Day;
        private UInt64 conditionOnHoldTS;
        private UInt64 conditionComponents;
        private UInt64 conditionGoAhead;
        private UInt64 totalQuantity;

        #endregion
        private List<DeliveryVisualManagementStages> eRMDeliveryVisualManagementStagesList;
        string[] ArrActiveInPlants;
        public bool FlagPresentInActivePlants = false;

        #region [GEOS2-4351][Rupali Sarode][29-05-2023]

        TableView tableViewInstance;
        public string ERM_RealTimeMonitorGrid_SettingFilePath = GeosApplication.Instance.UserSettingFolderName + "ERM_RealTimeMonitorGrid_Setting.Xml";
        private bool isColumnChooserVisibleForGrid;
        private ObservableCollection<BandItem> bands = new ObservableCollection<BandItem>();
        private DataTable dataTableForGridLayout;
        private DataTable dataTableRealTimeMonitor;
        private ObservableCollection<Emdep.Geos.UI.Helper.Column> columns;
        private ObservableCollection<Emdep.Geos.UI.Helper.Column> columnsProductionsInTime;
        bool IsSelectedWeekExistsInPlantWeekList;
        #endregion [GEOS2-4351][Rupali Sarode][29-05-2023]
        List<string> failedPlants;

        #region HR Resource [GEOS2-4729][rupali sarode][04-08-2023]
        private DataTable dtDashboard;
        private ObservableCollection<BandItem> bandsDashboard = new ObservableCollection<BandItem>();
        private DataTable dataTableForGridLayoutDashboard;
        private List<string> customers;
        private string loadsxWorkstation;
        private string futureLoad;
        private string hRResources;
        private string idstages;
        private string jobDescriptionID;
        private List<GeosAppSetting> workStages = new List<GeosAppSetting>();
        private List<ERMWorkStageWiseJobDescription> workStageWiseJobDescription = new List<ERMWorkStageWiseJobDescription>();
        private List<PlantOperationWeek> plantWeekList = new List<PlantOperationWeek>();
        private PlantOperationWeek plantWeek = new PlantOperationWeek();
        DataTable dtPlantOperation;
        private List<ERMEmployeePlantOperation> employeeplantOperationalList = new List<ERMEmployeePlantOperation>();
        private ERMEmployeePlantOperation employeeplantOperational = new ERMEmployeePlantOperation();
        private List<ERMEmployeePlantOperation> employeeplantOperationalListForRealTime = new List<ERMEmployeePlantOperation>(); //[GEOS2-4553][Rupali Sarode][19-06-2023]
        private List<ERMPlantOperationalPlanning> plantOperationalPlanning = new List<ERMPlantOperationalPlanning>();
        private DateTime hRResourceStartDate;
        private DateTime hRResourceEndDate;
        private List<RTMHRResourcesExpectedTime> rTMHRResourcesExpectedTimeList;
        private TableView FutureGridControl1;
        private GridControl FutureGridControl;
        private GridControl futureGridControlFilter;
        private GridControl ProductionGridControl;
        private GridControl productionGridControlFilter;
        private List<ERMDeliveryVisualManagement> eRMDeliveryVisualManagementList_Cloned;
        private List<RTMHRResourcesExpectedTime> rTMHRResourcesExpectedTimeList_Cloned;
        private List<RTMFutureLoad> rTMFutureLoadList_Cloned;
        private List<ERMProductionTime> productionTimeList_Cloned;

        private List<Object> selectedDeliveryCW;
        private List<string> deliveryCWList;

        private List<ERMRTMFilters> customerList;
        private List<object> selectedCustomer;

        private List<ERMRTMFilters> customerPlantList;
        private List<object> selectedCustomerPlant;

        private List<ERMRTMFilters> workOrderList;
        private List<object> selectedWorkOrder;

        private List<ERMRTMFilters> projectList;
        private List<object> selectedProject;
        private ObservableCollection<ERMDeliveryVisualManagement> filterDataList;
        private List<ERMCustomerPlant> allCustomerPlantList;
        private List<ERMCustomerPlant> selectedCustomerData;

        private string testString;
        private string legendConditionLessThan7Day;
        private List<string> categoryColumns;

        #endregion

        #region Future Load [rupali sarode][28-08-2023]
        private List<OfferOption> offerOptions;
        public DataRowView selectedObject;
        private List<RTMFutureLoad> rTMFutureLoadList;

        #endregion

        private List<DeliveryVisualManagementStages> eRMRTMHRResourcesStageList;
        private DataTable dataTableForGridLayoutProductionInTime;
        private ObservableCollection<BandItem> bandsDashboard1 = new ObservableCollection<BandItem>();
        private List<ERMProductionTime> productionTimeList;
        private List<GeosAppSetting> geosAppSettingList;


        #region [GEOS2-5030][Aishwarya Ingale][07-12-2023]

        private List<string> TempXMLDeliveryCWList;
        private List<string> TempXMLCustomerList;
        private List<int> TempXMLCustomerPlantList;
        private List<string> TempXMLWorkOrderList;
        private List<string> TempXMLProjectList;

        #endregion [GEOS2-5030][Aishwarya Ingale][07-12-2023]
        #endregion
        #region  public Commands
        public ICommand ChartLoadCommand { get; set; }
        public ICommand ChartDashboardSaleCustomDrawCrosshairCommand { get; set; }
        public ICommand ActionLoadCommand { get; set; }
        public ICommand PeriodCommand { get; set; }
        public ICommand ApplyCommand { get; set; }
        public ICommand PeriodCustomRangeCommand { get; set; }
        public ICommand PrintButtonCommand { get; set; }
        public ICommand PlantOwnerPopupClosedCommand { get; set; }
        public ICommand ChangePlantCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand RefreshDashboardCommand { get; set; }
        public ICommand TableViewLoadedCommand { get; set; }  //[GEOS2-4351][Rupali Sarode][29-05-2023]
        public ICommand TableViewUnloadedCommand { get; set; }  //[GEOS2-4351][Rupali Sarode][29-05-2023]
        public ICommand GridActionLoadCommand { get; set; }
        public ICommand GridActionProductionLoadCommand { get; set; }
        public ICommand ExpandDataCommand { get; set; }
        public ICommand CollapseDataCommand { get; set; }

        public ICommand ChangeDeliveryCWCommand { get; set; }
        public ICommand ChangeCustomerCommand { get; set; }

        public ICommand ChangeCustomerPlantCommand { get; set; }
        public ICommand ChangeWorkOrderCommand { get; set; }
        public ICommand ChangeProjectCommand { get; set; }

        public ICommand FilterExpandedCommand { get; set; }

        public ICommand ShowChartDialogWindowCommand { get; set; }

        public ICommand ShowHRResourceGridDialogWindowCommand { get; set; }
        //public ICommand ExpandAndCollapseOptionsCommand { get; set; }
        public ICommand ShowCRMExpectedLoadGridDialogWindowCommand { get; set; }
        public ICommand ShowProductionLoadGridDialogWindowCommand { get; set; }
        public ICommand LoadsxWorkstationLayoutCommand { get; set; }

        public ICommand RealTimeMonitorGridControlUnloadedCommand { get; set; }

        #endregion
        #region Property
        public DataTable GraphDataTable
        {
            get { return graphDataTable; }
            set { graphDataTable = value; OnPropertyChanged(new PropertyChangedEventArgs("GraphDataTable")); }
        }
        //public ObservableCollection<ERMDeliveeryVisualManagementStages> ERMDeliveeryVisualManagementStagesList
        //{
        //    get {
        //        return eRMDeliveeryVisualManagementStagesList;
        //    }
        //    set {
        //        eRMDeliveeryVisualManagementStagesList = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("ERMDeliveeryVisualManagementStagesList"));
        //    }
        //}
        public ObservableCollection<ERMDeliveryVisualManagement> ERMDeliveryVisualManagementList
        {
            get
            {
                return eRMDeliveryVisualManagementList;
            }
            set
            {
                eRMDeliveryVisualManagementList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ERMDeliveryVisualManagementList"));
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
        public DataTable ActionGraphDataTable
        {
            get { return actiongraphDataTable; }
            set { actiongraphDataTable = value; OnPropertyChanged(new PropertyChangedEventArgs("ActionGraphDataTable")); }
        }
        public string FromDate
        {
            get
            {
                return fromDate;
            }

            set
            {
                fromDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FromDate"));
            }
        }
        public string ToDate
        {
            get
            {
                return toDate;
            }

            set
            {
                toDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ToDate"));
            }
        }
        public int IsButtonStatus
        {
            get
            {
                return isButtonStatus;
            }

            set
            {
                isButtonStatus = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsButtonStatus"));
            }
        }
        public DateTime StartDate
        {
            get
            {
                return startDate;
            }

            set
            {
                startDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StartDate"));
            }
        }
        public DateTime EndDate
        {
            get
            {
                return endDate;
            }

            set
            {
                endDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EndDate"));
            }
        }
        public Visibility IsCalendarVisible
        {
            get
            {
                return isCalendarVisible;
            }

            set
            {
                isCalendarVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCalendarVisible"));
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
        public bool IsPeriod
        {
            get { return isPeriod; }
            set { isPeriod = value; }
        }

        public UInt64 TotalStagesQTY
        {
            get
            {
                return totalStagesQTY;
            }

            set
            {
                totalStagesQTY = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TotalStagesQTY"));
            }
        }
        #region [GEOS2-4342][Rupali Sarode][11-05-2023]

        public UInt64 ConditionQuality
        {
            get { return conditionQuality; }
            set
            {
                conditionQuality = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ConditionQuality"));
            }
        }

        public UInt64 ConditionGreaterThan7Days
        {
            get { return conditionGreaterThan7Days; }
            set
            {
                conditionGreaterThan7Days = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ConditionGreaterThan7Days"));
            }
        }

        public UInt64 ConditionDelay
        {
            get { return conditionDelay; }
            set
            {
                conditionDelay = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ConditionDelay"));
            }
        }
        public UInt64 ConditionLessThanEqual7Day
        {
            get { return conditionLessThanEqual7Day; }
            set
            {
                conditionLessThanEqual7Day = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ConditionLessThanEqual7Day"));
            }
        }
        public UInt64 ConditionOnHoldTS
        {
            get { return conditionOnHoldTS; }
            set
            {
                conditionOnHoldTS = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ConditionOnHoldTS"));
            }
        }
        public UInt64 ConditionComponents
        {
            get { return conditionComponents; }
            set
            {
                conditionComponents = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ConditionComponents"));
            }
        }
        public UInt64 ConditionGoAhead
        {
            get { return conditionGoAhead; }
            set
            {
                conditionGoAhead = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ConditionGoAhead"));
            }
        }

        public UInt64 TotalQuantity
        {
            get { return totalQuantity; }
            set
            {
                totalQuantity = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TotalQuantity"));
            }
        }

        #endregion

        public List<DeliveryVisualManagementStages> ERMDeliveryVisualManagementStagesList
        {
            get
            {
                return eRMDeliveryVisualManagementStagesList;
            }
            set
            {
                eRMDeliveryVisualManagementStagesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ERMDeliveryVisualManagementStagesList"));
            }
        }


        #region [GEOS2-4351][Rupali Sarode][29-05-2023]
        public bool IsColumnChooserVisibleForGrid
        {
            get
            {
                return isColumnChooserVisibleForGrid;
            }

            set
            {
                isColumnChooserVisibleForGrid = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsColumnChooserVisibleForGrid"));
            }
        }

        public ObservableCollection<BandItem> Bands
        {
            get { return bands; }
            set
            {
                bands = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Bands"));
            }
        }

        public DataTable DataTableForGridLayout
        {
            get
            {
                return dataTableForGridLayout;
            }
            set
            {
                dataTableForGridLayout = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DataTableForGridLayout"));
            }
        }

        public DataTable DataTableRealTimeMonitor
        {
            get
            {
                return dataTableRealTimeMonitor;
            }
            set
            {
                dataTableRealTimeMonitor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DataTableRealTimeMonitor"));
            }
        }

        public ObservableCollection<Emdep.Geos.UI.Helper.Summary> TotalSummary { get; private set; }
        public ObservableCollection<Emdep.Geos.UI.Helper.Summary> GroupSummary { get; private set; }

        public ObservableCollection<Emdep.Geos.UI.Helper.Summary> TotalSummaryTestboardsInProduction { get; private set; }
        public ObservableCollection<Emdep.Geos.UI.Helper.Summary> GroupSummaryTestboardsInProduction { get; private set; }

        #endregion

        public List<string> FailedPlants
        {
            get { return failedPlants; }
            set { failedPlants = value; }
        }
        Boolean isShowFailedPlantWarning;
        string warningFailedPlants;
        private List<ERMRTMFilters> allFiltersList;

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

        #region HR Resource [GEOS2-4729][rupali sarode][04-08-2023]
        public DataTable DtDashboard
        {
            get { return dtDashboard; }
            set
            {
                dtDashboard = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DtDashboard"));
            }
        }

        public ObservableCollection<BandItem> BandsDashboard
        {
            get { return bandsDashboard; }
            set
            {
                bandsDashboard = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BandsDashboard"));
            }
        }

        public DataTable DataTableForGridLayoutDashboard
        {
            get
            {
                return dataTableForGridLayoutDashboard;
            }
            set
            {
                dataTableForGridLayoutDashboard = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DataTableForGridLayoutDashboard"));
            }
        }
        public string LoadsxWorkstation
        {
            get
            {
                return loadsxWorkstation;
            }
            set
            {
                loadsxWorkstation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LoadsxWorkstation"));
            }
        }

        public string FutureLoad
        {
            get
            {
                return futureLoad;
            }
            set
            {
                futureLoad = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FutureLoad"));
            }
        }
        public string HRResources
        {
            get
            {
                return hRResources;
            }
            set
            {
                hRResources = value;
                OnPropertyChanged(new PropertyChangedEventArgs("HRResources"));
            }
        }

        public string Idstages
        {
            get { return idstages; }
            set
            {
                idstages = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Idstages"));
            }
        }
        public List<GeosAppSetting> WorkStages
        {
            get { return workStages; }
            set
            {
                workStages = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkStages"));
            }
        }

        public List<ERMWorkStageWiseJobDescription> WorkStageWiseJobDescription
        {
            get { return workStageWiseJobDescription; }
            set
            {
                workStageWiseJobDescription = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkStageWiseJobDescription"));
            }
        }
        public List<PlantOperationWeek> PlantWeekList
        {
            get { return plantWeekList; }
            set
            {
                plantWeekList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PlantWeekList"));
            }
        }

        public DataTable DtPlantOperation
        {
            get { return dtPlantOperation; }
            set
            {
                dtPlantOperation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DtPlantOperation"));
            }
        }

        public List<ERMEmployeePlantOperation> EmployeeplantOperationallist
        {
            get { return employeeplantOperationalList; }
            set
            {
                employeeplantOperationalList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeplantOperationallist"));
            }
        }

        public ERMEmployeePlantOperation EmployeeplantOperational
        {
            get { return employeeplantOperational; }
            set
            {
                employeeplantOperational = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeplantOperational"));
            }
        }

        public List<ERMEmployeePlantOperation> EmployeeplantOperationalListForRealTime
        {
            get { return employeeplantOperationalListForRealTime; }
            set
            {
                employeeplantOperationalListForRealTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeplantOperationalListForRealTime"));
            }
        }

        public List<ERMPlantOperationalPlanning> PlantOperationalPlanning
        {
            get { return plantOperationalPlanning; }
            set
            {
                plantOperationalPlanning = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PlantOperationalPlanning"));
            }
        }

        public List<PlantOperationProductionStage> PlantOperationProductionStage { get; set; }

        public DateTime HRResourceStartDate
        {
            get { return hRResourceStartDate; }
            set
            {
                hRResourceStartDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("HRResourceStartDate"));
            }
        }
        public DateTime HRResourceEndDate
        {
            get { return hRResourceEndDate; }
            set
            {
                hRResourceEndDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("HRResourceEndDate"));
            }
        }

        public List<RTMHRResourcesExpectedTime> RTMHRResourcesExpectedTimeList
        {
            get { return rTMHRResourcesExpectedTimeList; }
            set
            {
                rTMHRResourcesExpectedTimeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RTMHRResourcesExpectedTimeList"));
            }
        }

        #endregion HR Resource [GEOS2-4729][rupali sarode][04-08-2023]

        #region Future Load [rupali sarode][28-08-2023]

        public List<OfferOption> OfferOptions
        {
            get { return offerOptions; }
            set { offerOptions = value; }
        }
        public DataRowView SelectedObject
        {
            get { return selectedObject; }
            set
            {
                selectedObject = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedObject"));
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

        public ObservableCollection<Emdep.Geos.UI.Helper.Column> ColumnsProductionsInTime
        {
            get { return columnsProductionsInTime; }
            set
            {
                columnsProductionsInTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ColumnsProductionsInTime"));
            }
        }

        public List<RTMFutureLoad> RTMFutureLoadList
        {
            get { return rTMFutureLoadList; }
            set { rTMFutureLoadList = value; }
        }

        #endregion Future Load [rupali sarode][28-08-2023]


        public List<DeliveryVisualManagementStages> ERMRTMHRResourcesStageList
        {
            get
            {
                return eRMRTMHRResourcesStageList;
            }
            set
            {
                eRMRTMHRResourcesStageList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ERMRTMHRResourcesStageList"));
            }
        }

        #region [GEOS2-4862][Rupali Sarode][30-09-2023]
        public List<Object> SelectedDeliveryCW
        {
            get { return selectedDeliveryCW; }
            set
            {
                selectedDeliveryCW = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedDeliveryCW"));
                OnPropertyChanged(new PropertyChangedEventArgs("IsSelectedDeliveryCWNotEmpty")); //Aishwarya Ingale[Geos2-5749]
                UpdateColor();//Aishwarya Ingale[Geos2-5749]
            }
        }



        public List<string> DeliveryCWList
        {
            get
            {
                return deliveryCWList;
            }
            set
            {
                deliveryCWList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DeliveryCWList"));
            }
        }

        public List<ERMDeliveryVisualManagement> ERMDeliveryVisualManagementList_Cloned
        {
            get
            {
                return eRMDeliveryVisualManagementList_Cloned;
            }
            set
            {
                eRMDeliveryVisualManagementList_Cloned = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ERMDeliveryVisualManagementList_Cloned"));
            }
        }

        public List<ERMRTMFilters> CustomerList
        {
            get
            {
                return customerList;
            }
            set
            {
                customerList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomerList"));
            }
        }

        public List<object> SelectedCustomer
        {
            get { return selectedCustomer; }
            set
            {
                selectedCustomer = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCustomer"));
                OnPropertyChanged(new PropertyChangedEventArgs("IsSelectedCustomerNotEmpty"));//Aishwarya Ingale[Geos2-5749]
                UpdateColor();//Aishwarya Ingale[Geos2-5749]
            }
        }

        public List<ERMRTMFilters> CustomerPlantList
        {
            get { return customerPlantList; }
            set
            {
                customerPlantList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomerPlantList"));
            }
        }



        public List<object> SelectedCustomerPlant
        {
            get { return selectedCustomerPlant; }
            set
            {
                selectedCustomerPlant = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCustomerPlant"));
                OnPropertyChanged(new PropertyChangedEventArgs("IsSelectedCustomerPlantNotEmpty"));//Aishwarya Ingale[Geos2-5749]
                UpdateColor();//Aishwarya Ingale[Geos2-5749]
            }
        }

        public List<ERMRTMFilters> WorkOrderList
        {
            get { return workOrderList; }
            set
            {
                workOrderList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkOrderList"));
            }
        }

        public List<object> SelectedWorkOrder
        {
            get { return selectedWorkOrder; }
            set
            {
                selectedWorkOrder = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedWorkOrder"));
                OnPropertyChanged(new PropertyChangedEventArgs("IsSelectedWorkOrderNotEmpty"));//Aishwarya Ingale[Geos2-5749]
                UpdateColor();//Aishwarya Ingale[Geos2-5749]
            }
        }

        public List<ERMRTMFilters> ProjectList
        {
            get { return projectList; }
            set
            {
                projectList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProjectList"));
            }
        }

        public List<object> SelectedProject
        {
            get { return selectedProject; }
            set
            {
                selectedProject = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedProject"));
                OnPropertyChanged(new PropertyChangedEventArgs("IsSelectedProjectNotEmpty"));//Aishwarya Ingale[Geos2-5749]
                UpdateColor();//Aishwarya Ingale[Geos2-5749]
            }
        }

        public ObservableCollection<ERMDeliveryVisualManagement> FilterDataList
        {
            get
            {
                return filterDataList;
            }
            set
            {
                filterDataList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FilterDataList"));
            }
        }

        public List<RTMHRResourcesExpectedTime> RTMHRResourcesExpectedTimeList_Cloned
        {
            get
            {
                return rTMHRResourcesExpectedTimeList_Cloned;
            }
            set
            {
                rTMHRResourcesExpectedTimeList_Cloned = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RTMHRResourcesExpectedTimeList_Cloned"));
            }
        }

        public List<RTMFutureLoad> RTMFutureLoadList_Cloned
        {
            get
            {
                return rTMFutureLoadList_Cloned;
            }
            set
            {
                rTMFutureLoadList_Cloned = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RTMFutureLoadList_Cloned"));
            }
        }

        public List<ERMProductionTime> ProductionTimeList_Cloned
        {
            get
            {
                return productionTimeList_Cloned;
            }
            set
            {
                productionTimeList_Cloned = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProductionTimeList_Cloned"));
            }
        }

        public List<ERMCustomerPlant> AllCustomerPlantList
        {
            get { return allCustomerPlantList; }
            set
            {
                allCustomerPlantList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AllCustomerPlantList"));
            }
        }

        public List<ERMCustomerPlant> SelectedCustomerData
        {
            get { return selectedCustomerData; }
            set
            {
                selectedCustomerData = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCustomerData"));
            }
        }

        public string TestString
        {
            get
            {
                return testString;
            }
            set
            {
                testString = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TestString"));
            }
        }

        #endregion [GEOS2-4862][Rupali Sarode][30-09-2023]

        public string LegendConditionLessThan7Day
        {
            get
            {
                return legendConditionLessThan7Day;
            }
            set
            {
                legendConditionLessThan7Day = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LegendConditionLessThan7Day"));
            }
        }

        public GridControl FutureGridControlFilter
        {
            get
            {
                return futureGridControlFilter;
            }
            set
            {
                futureGridControlFilter = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FutureGridControlFilter"));
            }
        }

        public List<ERMRTMFilters> AllFiltersList
        {
            get
            {
                return allFiltersList;
            }
            set
            {
                allFiltersList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AllFiltersList"));
            }
        }

        public GridControl ProductionGridControlFilter
        {
            get
            {
                return productionGridControlFilter;
            }
            set
            {
                productionGridControlFilter = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProductionGridControlFilter"));
            }
        }

        public DataTable DataTableForGridLayoutProductionInTime
        {
            get
            {
                return dataTableForGridLayoutProductionInTime;
            }
            set
            {
                dataTableForGridLayoutProductionInTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DataTableForGridLayoutProductionInTime"));
            }
        }

        public List<ERMProductionTime> ProductionTimeList
        {
            get
            {
                return productionTimeList;
            }
            set
            {
                productionTimeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProductionTimeList"));
            }
        }
        public ObservableCollection<BandItem> BandsDashboard1
        {
            get { return bandsDashboard1; }
            set
            {
                bandsDashboard1 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BandsDashboard1"));
            }
        }

        public List<string> CategoryColumns
        {
            get { return categoryColumns; }
            set
            {
                categoryColumns = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CategoryColumns"));
            }
        }

        public List<GeosAppSetting> GeosAppSettingList
        {
            get { return geosAppSettingList; }
            set
            {
                geosAppSettingList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GeosAppSettingList"));
            }
        }

        public bool IsOptionExpand
        {
            get
            {
                return isOptionExpand;
            }

            set
            {
                isOptionExpand = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsOptionExpand"));
            }
        }

        #region Aishwarya Ingale[Geos2-5749]
        public bool IsSelectedDeliveryCWNotEmpty
        {
            get
            {
                return selectedDeliveryCW != null && selectedDeliveryCW.Count > 0 && selectedDeliveryCW.Count != DeliveryCWList.Count;
            }

        }

        public bool IsSelectedCustomerNotEmpty
        {
            get
            {
                return SelectedCustomer != null && SelectedCustomer.Count > 0 && SelectedCustomer.Count != CustomerList.Count;
            }

        }

        public bool IsSelectedCustomerPlantNotEmpty
        {
            get
            {
                return SelectedCustomerPlant != null && SelectedCustomerPlant.Count > 0 && SelectedCustomerPlant.Count != CustomerPlantList.Count;
            }

        }

        public bool IsSelectedWorkOrderNotEmpty
        {
            get
            {
                return SelectedWorkOrder != null && SelectedWorkOrder.Count > 0 && SelectedWorkOrder.Count != WorkOrderList.Count;
            }

        }

        public bool IsSelectedProjectNotEmpty
        {
            get
            {
                return SelectedProject != null && SelectedProject.Count > 0 && SelectedProject.Count != ProjectList.Count;
            }

        }

        //Aishwarya Ingale[Geos2-5749]
        private string color;
        public string Color
        {
            get { return color; }
            set
            {
                color = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Color"));
            }
        }

        //Aishwarya Ingale[Geos2-5749]
        private void UpdateColor()
        {
            if (IsSelectedDeliveryCWNotEmpty || IsSelectedCustomerNotEmpty || IsSelectedCustomerPlantNotEmpty || IsSelectedWorkOrderNotEmpty || IsSelectedProjectNotEmpty)
            {
                Color = "Black";
            }
            else
            {
                Color = "White"; // or any other default color
            }
        }
        #endregion

        #endregion
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

        public DashboardViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor DashboardViewModel() ...", category: Category.Info, priority: Priority.Low);

                PeriodCommand = new DevExpress.Mvvm.DelegateCommand<object>(PeriodCommandAction);
                PeriodCustomRangeCommand = new DevExpress.Mvvm.DelegateCommand<object>(PeriodCustomRangeCommandAction);
                ApplyCommand = new DevExpress.Mvvm.DelegateCommand<object>(ApplyCommandAction);
                ActionLoadCommand = new DevExpress.Mvvm.DelegateCommand<object>(ChartLoadAction);
                PrintButtonCommand = new DevExpress.Mvvm.DelegateCommand<object>(PrintChart);
                ChangePlantCommand = new DevExpress.Mvvm.DelegateCommand<object>(ChangePlantCommandAction);
                CancelCommand = new DevExpress.Mvvm.DelegateCommand<object>(CancelCommandAction);
                ChartDashboardSaleCustomDrawCrosshairCommand = new DevExpress.Mvvm.DelegateCommand<object>(ChartDashboardSaleCustomDrawCrosshairCommandAction);
                RefreshDashboardCommand = new DevExpress.Mvvm.DelegateCommand<object>(RefreshDashboardDetails);
                TableViewLoadedCommand = new DevExpress.Mvvm.DelegateCommand<RoutedEventArgs>(TableViewLoadedCommandAction);
                TableViewUnloadedCommand = new DevExpress.Mvvm.DelegateCommand<RoutedEventArgs>(TableViewUnloadedCommandAction);
                GridActionLoadCommand = new DevExpress.Mvvm.DelegateCommand<object>(GridActionLoadCommandAction);
                GridActionProductionLoadCommand = new DevExpress.Mvvm.DelegateCommand<object>(GridActionProductionLoadCommandAction);
                ExpandDataCommand = new DevExpress.Mvvm.DelegateCommand<object>(ExpandDataCommandAction);
                CollapseDataCommand = new DevExpress.Mvvm.DelegateCommand<object>(CollapseDataCommandAction);
                RealTimeMonitorGridControlUnloadedCommand = new DevExpress.Mvvm.DelegateCommand<object>(RealTimeMonitorGridControlUnloadedCommandAction);
                ChangeDeliveryCWCommand = new DevExpress.Mvvm.DelegateCommand<object>(ChangeDeliveryCWCommandAction);
                ChangeCustomerCommand = new DevExpress.Mvvm.DelegateCommand<object>(ChangeCustomerCommandAction);
                ChangeCustomerPlantCommand = new DevExpress.Mvvm.DelegateCommand<object>(ChangeCustomerPlantCommandAction);
                ChangeWorkOrderCommand = new DevExpress.Mvvm.DelegateCommand<object>(ChangeWorkOrderCommandAction);
                ChangeProjectCommand = new DevExpress.Mvvm.DelegateCommand<object>(ChangeProjectCommandAction);
                FilterExpandedCommand = new DevExpress.Mvvm.DelegateCommand<object>(FilterExpandedCommandAction);
                ShowChartDialogWindowCommand = new DevExpress.Mvvm.DelegateCommand<object>(ShowChartDialogWindowCommandAction);
                ShowHRResourceGridDialogWindowCommand = new DevExpress.Mvvm.DelegateCommand<object>(ShowHRResourceGridDialogWindowCommandAction);
                // ExpandAndCollapseOptionsCommand = new DevExpress.Mvvm.DelegateCommand<object>(ExpandAndCollapseOptionsCommandAction);
                ShowCRMExpectedLoadGridDialogWindowCommand = new DevExpress.Mvvm.DelegateCommand<object>(ShowCRMExpectedLoadGridDialogWindowCommandAction);
                ShowProductionLoadGridDialogWindowCommand = new DevExpress.Mvvm.DelegateCommand<object>(ShowProductionLoadGridDialogWindowCommandAction);
                LoadsxWorkstationLayoutCommand = new DevExpress.Mvvm.DelegateCommand<object>(LoadsxWorkstationLayoutCommandAction);

                GeosApplication.Instance.Logger.Log("Constructor DashboardViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Constructor DashboardViewModel()." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
        public void Init()
        {
            GeosApplication.Instance.Logger.Log("Method Init ...", category: Category.Info, priority: Priority.Low);
            try
            {
                ERMCommon.Instance.IsShowFailedPlantWarning = false;
                ERMCommon.Instance.WarningFailedPlants = string.Empty;
                ERMCommon.Instance.FailedPlants = new List<string>();
                FailedPlants = new List<string>();
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                IsCalendarVisible = Visibility.Collapsed;
                StartDate = DateTime.Today.Date;
                EndDate = DateTime.Today.Date;
                setDefaultPeriod();
                FillStages();  //[GEOS2-4343][Pallavi Jadhav][25 05 2023]
                ResetConditionValues(); //[GEOS2-4342][Rupali Sarode][11-05-2023]

                FillDVManagementProduction();
                FailedPlants = new List<string>();

                dt = new DataTable();
                dt.Columns.Add("Month");
                dt.Columns.Add("Year");
                dt.Columns.Add("MonthYear");
                dt.Columns.Add("Components");
                dt.Columns.Add("OnHold-T-S");
                dt.Columns.Add(">7Day");
                dt.Columns.Add("<=7Day");
                dt.Columns.Add("Delay");
                dt.Columns.Add("Quality");
                dt.Columns.Add("GoAhead");
                CreateTable();  // Fill data for Stages other than COM & CAD


                dp = new DataTable();
                dp.Columns.Add("Month");
                dp.Columns.Add("Year");
                dp.Columns.Add("MonthYear");
                dp.Columns.Add("Components");
                dp.Columns.Add("OnHold-T-S");
                dp.Columns.Add(">7Day");
                dp.Columns.Add("<=7Day");
                dp.Columns.Add("Delay");
                dp.Columns.Add("Quality");
                dp.Columns.Add("GoAhead");
                ActionCreateTable();  // Fill data for COM & CAD Stages 

                TotalQuantity = ConditionGreaterThan7Days + ConditionLessThanEqual7Day + ConditionDelay + ConditionQuality + ConditionGoAhead;

                #region [GEOS2-4730][rupali sarode][10/08/2023]
                GetIdStageAndJobDescriptionByAppSetting();
                GetWeekList();

                RTMFutureLoadList = new List<RTMFutureLoad>();
                AddColumnsToDataTableWithBands(); //[GEOS2-4351][Rupali Sarode][29-05-2023]
                FillRTMFutureLoadList();
                FillDashboard();
                RTMHRResourcesExpectedTimeList = new List<RTMHRResourcesExpectedTime>();

                FillRTMData();

                AddColumnsToDataTableWithBandsForHrResources();
                LoadsxWorkstation = System.Windows.Application.Current.FindResource("LoadsxWorkstation").ToString();
                FutureLoad = System.Windows.Application.Current.FindResource("FutureLoad").ToString();
                HRResources = System.Windows.Application.Current.FindResource("HRResources").ToString();
                LegendConditionLessThan7Day = "<=7Day";
                FillListOfColor();
                FillProductionIntime();
                ReadXmlFile();  //[GEOS2-5030][Aishwarya Ingale][07-12-2023]
                FillFilters(); //[GEOS2-4862][Rupali Sarode][30-09-2023]

                FillDashboardHRResources();
                AddColumnsToDataTableWithBandsinTestboardProduction();
                FillDashboardProductionIntime();
                #endregion [GEOS2-4730][rupali sarode][10/08/2023]

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method Init() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init()", category: Category.Exception, priority: Priority.Low);
            }

        }
        #region Method

        private void ChartLoadAction(object obj)
        {

            try
            {
                GeosApplication.Instance.Logger.Log("Method ChartLoadAction ...", category: Category.Info, priority: Priority.Low);

                chartControl = (ChartControl)obj;
                // chartcontrol.DataSource = GraphDataTable;
                chartControl.BeginInit();
                XYDiagram2D diagram = new XYDiagram2D();
                chartControl.Diagram = diagram;
                //diagram.ActualAxisX.Title = new AxisTitle() { Content = "Months" };
                //diagram.ActualAxisY.Title = new AxisTitle() { Content = "Amount" };
                diagram.ActualAxisX.QualitativeScaleOptions = new QualitativeScaleOptions();
                diagram.ActualAxisX.QualitativeScaleOptions.AutoGrid = false;
                // GeosApplication.SetCurrencySymbol(GeosApplication.Instance.CurrentCurrencySymbol);
                diagram.ActualAxisY.NumericOptions = new NumericOptions();
                //     diagram.ActualAxisY.GridSpacing = 2000; // Set the interval of 50 values on the y-axis
                diagram.ActualAxisY.NumericOptions.Format = NumericFormat.General;




                List<string> monthNameList = new List<string>();
                // monthNameList.Add("Components");
                monthNameList.Add("OnHold-T-S");
                monthNameList.Add(">7Day");
                monthNameList.Add("<=7Day");
                monthNameList.Add("Delay");
                monthNameList.Add("Quality");
                monthNameList.Add("GoAhead");

                Dictionary<string, string> LegentwithColorList = new Dictionary<string, string>();

                LegentwithColorList.Add("OnHold-T-S", "#808080");
                LegentwithColorList.Add(">7Day", "#3A9BDC");
                LegentwithColorList.Add("<=7Day", "#72cc50");
                LegentwithColorList.Add("Delay", "#FFFF00");
                LegentwithColorList.Add("Quality", "#ff0000");
                LegentwithColorList.Add("GoAhead", "#FFA500");

                LineSeries2D lineDashedTargetPlant = new LineSeries2D();
                lineDashedTargetPlant.LineStyle = new LineStyle();
                lineDashedTargetPlant.LineStyle.DashStyle = new DashStyle();
                lineDashedTargetPlant.LineStyle.Thickness = 2;
                lineDashedTargetPlant.LineStyle.DashStyle.Dashes = new DoubleCollection(new double[] { 3, 3, 3, 3 });
                lineDashedTargetPlant.ArgumentScaleType = ScaleType.Auto;
                lineDashedTargetPlant.Brush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#808080"));
                lineDashedTargetPlant.ValueScaleType = ScaleType.Numerical;
                lineDashedTargetPlant.DisplayName = "Components";
                lineDashedTargetPlant.CrosshairLabelPattern = "{S} : {V:n0}";
                lineDashedTargetPlant.ArgumentDataMember = "Year";
                lineDashedTargetPlant.ArgumentDataMember = "MonthYear";
                lineDashedTargetPlant.ValueDataMember = "Components";
                chartControl.Diagram.Series.Add(lineDashedTargetPlant);

                // LineSeries2D lineDashedTargetPlant1 = new LineSeries2D();
                // lineDashedTargetPlant1.LineStyle = new LineStyle();
                // // lineDashedTargetPlant1.LineStyle.DashStyle = new DashStyle();
                // lineDashedTargetPlant1.LineStyle.Thickness = 1;
                // //lineDashedTargetPlant1.LineStyle.DashStyle.Dashes = new DoubleCollection(new double[] { 3, 3, 3, 3 });
                // lineDashedTargetPlant1.ArgumentScaleType = ScaleType.Auto;
                //// lineDashedTargetPlant1.Brush = new SolidColorBrush(Colors.Transparent);
                // lineDashedTargetPlant1.Brush = (SolidColorBrush)Application.Current.Resources["ColorMain"];
                // SolidColorBrush blackBrush = new SolidColorBrush(Colors.Black);
                // SolidColorBrush whiteBrush = new SolidColorBrush(Colors.White);
                // //   lineDashedTargetPlant1.Brush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#808080"));
                // lineDashedTargetPlant1.ValueScaleType = ScaleType.Numerical;
                // lineDashedTargetPlant1.DisplayName = "Year";
                // lineDashedTargetPlant1.ArgumentDataMember = "MonthYear";
                // lineDashedTargetPlant1.ValueDataMember = "Year";
                // lineDashedTargetPlant1.LabelsVisibility = true;
                // lineDashedTargetPlant1.Legend = false;
                // chartControl.Diagram.Series.Add(lineDashedTargetPlant1);

                foreach (var item in monthNameList)
                {

                    BarSideBySideStackedSeries2D barSideBySideStackedSeries2Dhidden = new BarSideBySideStackedSeries2D();
                    barSideBySideStackedSeries2Dhidden.Brush = (SolidColorBrush)(new BrushConverter().ConvertFrom(LegentwithColorList.FirstOrDefault(i => i.Key.ToLower() == item.ToLower()).Value));
                    chartControl.Diagram.Series.Add(barSideBySideStackedSeries2Dhidden);
                    barSideBySideStackedSeries2Dhidden.DisplayName = item;
                    //if (TotalStagesQTY != 0)
                    //{
                    //    diagram.ActualAxisX.Label.TextPattern = Convert.ToString(TotalStagesQTY);

                    //}
                    if (item != null)
                    {
                        BarSideBySideStackedSeries2D barSideBySideStackedSeries2D = new BarSideBySideStackedSeries2D();
                        barSideBySideStackedSeries2D.ArgumentScaleType = ScaleType.Auto;
                        barSideBySideStackedSeries2D.ValueScaleType = ScaleType.Numerical;
                        barSideBySideStackedSeries2D.DisplayName = item;
                        barSideBySideStackedSeries2D.BarWidth = 0.8;

                        if (barSideBySideStackedSeries2D.DisplayName == "OnHold-T-S")
                        {
                            barSideBySideStackedSeries2D.Brush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#808080"));
                            // barSideBySideStackedSeries2D.ArgumentDataMember = "Year";
                            barSideBySideStackedSeries2D.ArgumentDataMember = "MonthYear";
                            barSideBySideStackedSeries2D.ValueDataMember = item;
                            barSideBySideStackedSeries2D.CrosshairLabelPattern = "{S} : {V:n0}";
                            barSideBySideStackedSeries2D.Model = new DevExpress.Xpf.Charts.SimpleBar2DModel();
                            barSideBySideStackedSeries2D.ShowInLegend = false;
                            barSideBySideStackedSeries2D.LabelsVisibility = false;
                            barSideBySideStackedSeries2D.AnimationAutoStartMode = AnimationAutoStartMode.SetStartState;
                            Bar2DSlideFromTopAnimation seriesAnimation = new Bar2DSlideFromTopAnimation();
                            seriesAnimation.Duration = new TimeSpan(0, 0, 2);
                            barSideBySideStackedSeries2D.PointAnimation = seriesAnimation;

                            diagram.Series.Add(barSideBySideStackedSeries2D);


                        }
                        else if (barSideBySideStackedSeries2D.DisplayName == "Quality")
                        {
                            barSideBySideStackedSeries2D.Brush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#ff0000"));
                            barSideBySideStackedSeries2D.ArgumentDataMember = "Month";
                            // barSideBySideStackedSeries2D.ArgumentDataMember = "Year";
                            barSideBySideStackedSeries2D.ArgumentDataMember = "MonthYear";
                            barSideBySideStackedSeries2D.ValueDataMember = item;
                            barSideBySideStackedSeries2D.CrosshairLabelPattern = "{S} : {V:n0}";
                            barSideBySideStackedSeries2D.Model = new DevExpress.Xpf.Charts.SimpleBar2DModel();
                            barSideBySideStackedSeries2D.ShowInLegend = false;

                            barSideBySideStackedSeries2D.AnimationAutoStartMode = AnimationAutoStartMode.SetStartState;
                            Bar2DSlideFromTopAnimation seriesAnimation = new Bar2DSlideFromTopAnimation();
                            seriesAnimation.Duration = new TimeSpan(0, 0, 2);
                            barSideBySideStackedSeries2D.PointAnimation = seriesAnimation;

                            diagram.Series.Add(barSideBySideStackedSeries2D);


                        }
                        else if (barSideBySideStackedSeries2D.DisplayName == ">7Day")
                        {
                            barSideBySideStackedSeries2D.Brush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#3A9BDC"));
                            barSideBySideStackedSeries2D.ArgumentDataMember = "Month";
                            // barSideBySideStackedSeries2D.ArgumentDataMember = "Year";
                            barSideBySideStackedSeries2D.ArgumentDataMember = "MonthYear";
                            barSideBySideStackedSeries2D.ValueDataMember = item;
                            barSideBySideStackedSeries2D.CrosshairLabelPattern = "{S} : {V:n0}";
                            barSideBySideStackedSeries2D.Model = new DevExpress.Xpf.Charts.SimpleBar2DModel();

                            barSideBySideStackedSeries2D.ShowInLegend = false;
                            barSideBySideStackedSeries2D.LabelsVisibility = false;
                            SeriesLabel SeriesLabel = new SeriesLabel();
                            SeriesLabel.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#ff0000"));
                            barSideBySideStackedSeries2D.ColorDataMember = SeriesLabel.Foreground.ToString();
                            barSideBySideStackedSeries2D.AnimationAutoStartMode = AnimationAutoStartMode.SetStartState;
                            Bar2DSlideFromTopAnimation seriesAnimation = new Bar2DSlideFromTopAnimation();
                            seriesAnimation.Duration = new TimeSpan(0, 0, 2);
                            barSideBySideStackedSeries2D.PointAnimation = seriesAnimation;

                            diagram.Series.Add(barSideBySideStackedSeries2D);


                        }
                        else if (barSideBySideStackedSeries2D.DisplayName == "<=7Day")
                        {
                            barSideBySideStackedSeries2D.Brush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#72cc50"));
                            barSideBySideStackedSeries2D.ArgumentDataMember = "Month";
                            //  barSideBySideStackedSeries2D.ArgumentDataMember = "Year"; 
                            barSideBySideStackedSeries2D.ArgumentDataMember = "MonthYear";
                            barSideBySideStackedSeries2D.ValueDataMember = item;

                            barSideBySideStackedSeries2D.CrosshairLabelPattern = "{S} : {V:n0}";
                            barSideBySideStackedSeries2D.Model = new DevExpress.Xpf.Charts.SimpleBar2DModel();
                            barSideBySideStackedSeries2D.ShowInLegend = false;
                            barSideBySideStackedSeries2D.LabelsVisibility = false;

                            //SeriesLabel SeriesLabel = new SeriesLabel();
                            //  SeriesLabel.Foreground.Transform = Labels.p;
                            //  barSideBySideStackedSeries2D.Label.Contains(SeriesLabel.Foreground) = SeriesLabel.Foreground ;
                            barSideBySideStackedSeries2D.AnimationAutoStartMode = AnimationAutoStartMode.SetStartState;
                            Bar2DSlideFromTopAnimation seriesAnimation = new Bar2DSlideFromTopAnimation();
                            seriesAnimation.Duration = new TimeSpan(0, 0, 2);
                            barSideBySideStackedSeries2D.PointAnimation = seriesAnimation;

                            diagram.Series.Add(barSideBySideStackedSeries2D);


                        }
                        else if (barSideBySideStackedSeries2D.DisplayName == "Delay")
                        {
                            barSideBySideStackedSeries2D.Brush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFF00"));
                            barSideBySideStackedSeries2D.ArgumentDataMember = "Month";
                            //barSideBySideStackedSeries2D.ArgumentDataMember = "Year";
                            barSideBySideStackedSeries2D.ArgumentDataMember = "MonthYear";
                            barSideBySideStackedSeries2D.ValueDataMember = item;
                            barSideBySideStackedSeries2D.CrosshairLabelPattern = "{S} : {V:n0}";
                            barSideBySideStackedSeries2D.Model = new DevExpress.Xpf.Charts.SimpleBar2DModel();
                            barSideBySideStackedSeries2D.ShowInLegend = false;

                            barSideBySideStackedSeries2D.AnimationAutoStartMode = AnimationAutoStartMode.SetStartState;
                            Bar2DSlideFromTopAnimation seriesAnimation = new Bar2DSlideFromTopAnimation();
                            seriesAnimation.Duration = new TimeSpan(0, 0, 2);
                            barSideBySideStackedSeries2D.PointAnimation = seriesAnimation;

                            diagram.Series.Add(barSideBySideStackedSeries2D);


                        }
                        else if (barSideBySideStackedSeries2D.DisplayName == "GoAhead")
                        {
                            barSideBySideStackedSeries2D.Brush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFA500"));
                            barSideBySideStackedSeries2D.ArgumentDataMember = "Month";
                            // barSideBySideStackedSeries2D.ArgumentDataMember = "Year";
                            barSideBySideStackedSeries2D.ArgumentDataMember = "MonthYear";
                            barSideBySideStackedSeries2D.ValueDataMember = item;
                            barSideBySideStackedSeries2D.CrosshairLabelPattern = "{S} : {V:n0}";
                            barSideBySideStackedSeries2D.Model = new DevExpress.Xpf.Charts.SimpleBar2DModel();
                            barSideBySideStackedSeries2D.ShowInLegend = false;

                            barSideBySideStackedSeries2D.AnimationAutoStartMode = AnimationAutoStartMode.SetStartState;
                            Bar2DSlideFromTopAnimation seriesAnimation = new Bar2DSlideFromTopAnimation();
                            seriesAnimation.Duration = new TimeSpan(0, 0, 2);
                            barSideBySideStackedSeries2D.PointAnimation = seriesAnimation;

                            diagram.Series.Add(barSideBySideStackedSeries2D);


                        }
                    }

                }

                diagram.ActualAxisX.ActualLabel.Angle = 270;

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
                        if (item.Series.DisplayName == "Components")
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

        private void CreateTable()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CreateTable ...", category: Category.Info, priority: Priority.Low);

                dt.Rows.Clear();
                FilterDataList = new ObservableCollection<ERMDeliveryVisualManagement>();

                //var AllStages = ERMDeliveryVisualManagementList.GroupBy(x => x.IdStage)
                //    .Select(group => new
                //    {
                //        IdStage = ERMDeliveryVisualManagementList.FirstOrDefault(a => a.IdStage == group.Key).IdStage,
                //        //  Sequence  = ERMDeliveryVisualManagementList.FirstOrDefault(a => a.IdStage == group.Key).Sequence,
                //        NewSequence = ERMDeliveryVisualManagementList.FirstOrDefault(a => a.IdStage == group.Key).NewSequence,
                //        StageCode = ERMDeliveryVisualManagementList.FirstOrDefault(a => a.IdStage == group.Key).StageCode,
                //        Count = ERMDeliveryVisualManagementList.Where(b => b.IdStage == 0).Count(),
                //    }).ToList().OrderBy(i => i.NewSequence);


                //DateTime? tempDate = DateTime.Now.AddDays(7);

                DateTime TodaysDate = DateTime.Now.Date; //[GEOS2-4528][Rupali Sarode][30-05-2023]

                //foreach (var stage in AllStages.Where(x => x.StageCode != "COM" && x.StageCode != "CAD").ToList())
                #region [Pallavi Jadhav][GEOS2-4343][23-05-2023] -- Show stage only if present in ActiveInPlants
                foreach (var stage in ERMDeliveryVisualManagementStagesList.Where(x => x.StageCode != "COM" && x.StageCode != "CAD").ToList())
                {

                    if (stage.ActiveInPlants != null && stage.ActiveInPlants != "")
                    {
                        ArrActiveInPlants = stage.ActiveInPlants.Split(',');
                        List<Site> tmpSelectedPlant = ERMCommon.Instance.SelectedAuthorizedPlantsList.Cast<Site>().ToList();
                        FlagPresentInActivePlants = false;
                        foreach (Site itemSelectedPlant in tmpSelectedPlant)
                        {
                            if (FlagPresentInActivePlants == true)
                            {
                                break;
                            }

                            foreach (var iPlant in ArrActiveInPlants)
                            {
                                if (Convert.ToUInt16(iPlant) == itemSelectedPlant.IdSite)
                                {
                                    FlagPresentInActivePlants = true;
                                    break;
                                }
                            }

                        }
                    }

                    if (FlagPresentInActivePlants == true || stage.ActiveInPlants == null || stage.ActiveInPlants == "")
                    {

                        DataRow dr = dt.NewRow();



                        if (stage.StageCode == "COM" || stage.StageCode == "CAD")
                        {
                        }
                        else
                        {
                            // DataRow dr = dt.NewRow();
                            dr[0] = null;
                            dr[1] = null;
                            dr[2] = stage.StageCode.ToString().PadLeft(2, '0');

                            dr["Quality"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.IdOfferType == 2 || i.IdOfferType == 3) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).Select(x => x.QTY).Sum();

                            FilterDataList.AddRange(ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.IdOfferType == 2 || i.IdOfferType == 3) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).ToList());

                            ConditionQuality = ConditionQuality + Convert.ToUInt64(dr["Quality"]);
                            int Quality = Convert.ToInt32(dr["Quality"]);

                            DateTime? tempDate = DateTime.Now.AddDays(7);
                            // dr[">7Day"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.CounterPSSateDate.Value.Date > tempDate.Value.Date) && i.CurrentWorkStation == stage.StageCode).Select(x => x.QTY).Sum();
                            //[GEOS2-4528][Rupali Sarode][30-05-2023]
                            // dr[">7Day"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.DeliveryDate.Value.Date > tempDate.Value.Date) && i.CurrentWorkStation == stage.StageCode).Select(x => x.QTY).Sum();
                            dr[">7Day"] = ERMDeliveryVisualManagementList.Where(i => (i.PlannedDeliveryDate != null ? i.PlannedDeliveryDate.Value.Date : i.DeliveryDate.Value.Date) > tempDate.Value.Date && i.IdStage == stage.IdStage && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).Select(x => x.QTY).Sum();
                            FilterDataList.AddRange(ERMDeliveryVisualManagementList.Where(i => (i.PlannedDeliveryDate != null ? i.PlannedDeliveryDate.Value.Date : i.DeliveryDate.Value.Date) > tempDate.Value.Date && i.IdStage == stage.IdStage && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).ToList());

                            ConditionGreaterThan7Days = ConditionGreaterThan7Days + Convert.ToUInt64(dr[">7Day"]);
                            int Day7 = Convert.ToInt32(dr[">7Day"]);

                            tempDate = DateTime.Now;
                            // dr["Delay"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.CounterPSSateDate.Value.Date <= tempDate.Value.Date) && i.CurrentWorkStation == stage.StageCode).Select(x => x.QTY).Sum();
                            //[GEOS2-4528][Rupali Sarode][30-05-2023]
                            dr["Delay"] = ERMDeliveryVisualManagementList.Where(i => (i.PlannedDeliveryDate != null ? i.PlannedDeliveryDate.Value.Date : i.DeliveryDate.Value.Date) <= tempDate.Value.Date && i.IdStage == stage.IdStage && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).Select(x => x.QTY).Sum();
                            FilterDataList.AddRange(ERMDeliveryVisualManagementList.Where(i => (i.PlannedDeliveryDate != null ? i.PlannedDeliveryDate.Value.Date : i.DeliveryDate.Value.Date) <= tempDate.Value.Date && i.IdStage == stage.IdStage && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).ToList());

                            ConditionDelay = ConditionDelay + Convert.ToUInt64(dr["Delay"]);
                            int Delay = Convert.ToInt32(dr["Delay"]);

                            tempDate = DateTime.Now.AddDays(7);
                            //dr["<=7Day"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.CounterPSSateDate.Value.Date <= tempDate.Value.Date) && i.CurrentWorkStation == stage.StageCode).Select(x => x.QTY).Sum();
                            //[GEOS2-4528][Rupali Sarode][30-05-2023]
                            dr["<=7Day"] = ERMDeliveryVisualManagementList.Where(i => (i.PlannedDeliveryDate != null ? i.PlannedDeliveryDate.Value.Date : i.DeliveryDate.Value.Date) > TodaysDate && i.DeliveryDate.Value.Date <= tempDate.Value.Date && i.IdStage == stage.IdStage && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).Select(x => x.QTY).Sum();
                            FilterDataList.AddRange(ERMDeliveryVisualManagementList.Where(i => (i.PlannedDeliveryDate != null ? i.PlannedDeliveryDate.Value.Date : i.DeliveryDate.Value.Date) > TodaysDate && i.DeliveryDate.Value.Date <= tempDate.Value.Date && i.IdStage == stage.IdStage && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).ToList());

                            ConditionLessThanEqual7Day = ConditionLessThanEqual7Day + Convert.ToUInt64(dr["<=7Day"]);
                            int Day7lessthanequalto = Convert.ToInt32(dr["<=7Day"]);

                            dr["OnHold-T-S"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.IdOfferStatusType == 18) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).Select(x => x.QTY).Sum();
                            FilterDataList.AddRange(ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.IdOfferStatusType == 18) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).ToList());

                            ConditionOnHoldTS = ConditionOnHoldTS + Convert.ToUInt64(dr["OnHold-T-S"]);
                            int OnHold = Convert.ToInt32(dr["OnHold-T-S"]);

                            dr["Components"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.IdTemplate == 9) && i.CurrentWorkStation == stage.StageCode).Select(x => x.QTY).Sum();
                            FilterDataList.AddRange(ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.IdTemplate == 9) && i.CurrentWorkStation == stage.StageCode).ToList());

                            ConditionComponents = ConditionComponents + Convert.ToUInt64(dr["Components"]);
                            int Components = Convert.ToInt32(dr["Components"]);

                            //  dr["GoAhead"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.GoAheadDate != null && i.PODate == null) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).Select(x => x.QTY).Sum();
                            dr["GoAhead"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.GoAheadDate != null) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).Select(x => x.QTY).Sum();
                            FilterDataList.AddRange(ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.GoAheadDate != null) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).ToList());

                            ConditionGoAhead = ConditionGoAhead + Convert.ToUInt64(dr["GoAhead"]);
                            int GoAhead = Convert.ToInt32(dr["GoAhead"]);

                            dr["Year"] = Quality + Day7 + Delay + Day7lessthanequalto + OnHold + GoAhead;
                            //if (dr["Year"].ToString().Length == 1)
                            //{
                            //    dr[2] = dr[2].ToString() + "\n   " + dr["Year"].ToString();
                            //}
                            //else if (dr["Year"].ToString().Length == 2)
                            //{
                            //    dr[2] = dr[2].ToString() + "\n " + dr["Year"].ToString();
                            //}

                            //else
                            //{
                            //    dr[2] = dr[2].ToString() + "\n" + dr["Year"].ToString();
                            //}


                            if (dr["Year"].ToString().Length == 1)
                            {
                                dr[2] = "(" + dr["Year"].ToString() + ") " + dr[2].ToString();
                            }
                            else if (dr["Year"].ToString().Length == 2)
                            {
                                dr[2] = "(" + dr["Year"].ToString() + ") " + dr[2].ToString();
                            }

                            else
                            {
                                dr[2] = "(" + dr["Year"].ToString() + ") " + dr[2].ToString();
                            }

                        }
                        dt.Rows.Add(dr);



                    }


                    //if (TotalStagesQTY != 0)
                    //{
                    //    diagram.ActualAxisX.Title = new AxisTitle()
                    //    {
                    //        Content = TotalStagesQTY
                    //    };
                    //}
                }
                #endregion


                GeosApplication.Instance.Logger.Log("Method CreateTable() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CreateTable() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

            GraphDataTable = dt;

            if (chartControl != null) { chartControl.UpdateData(); }
        }



        private void FillDVManagementProduction()
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Method FillDVManagementProduction ...", category: Category.Info, priority: Priority.Low);
                if (ERMCommon.Instance.SelectedAuthorizedPlantsList != null)
                {
                    List<Site> plantOwners = ERMCommon.Instance.SelectedAuthorizedPlantsList.Cast<Site>().ToList();
                    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.Name));
                    if (ERMCommon.Instance.FailedPlants == null)
                    {
                        ERMCommon.Instance.FailedPlants = new List<string>();
                    }

                    FailedPlants = new List<string>();

                    ERMDeliveryVisualManagementList = new ObservableCollection<ERMDeliveryVisualManagement>();

                    foreach (var itemPlantOwnerUsers in plantOwners)
                    {
                        try
                        {
                            string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.Name == itemPlantOwnerUsers.Name).Select(xy => xy.ServiceProviderUrl).FirstOrDefault();
                            ERMService = new ERMServiceController(serviceurl); // [Rupali Sarode][08-06-2023]

                            GeosApplication.Instance.SplashScreenMessage = "Connecting to " + itemPlantOwnerUsers.Name;

                            string IdSite = Convert.ToString(itemPlantOwnerUsers.IdSite);

                            var CurrencyNameFromSetting = String.Empty;

                            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("ERM_SelectedCurrency"))
                            {
                                CurrencyNameFromSetting = GeosApplication.Instance.UserSettings["ERM_SelectedCurrency"];
                            }

                            //ERMService = new ERMServiceController("localhost:6699");
                            //ERMDeliveryVisualManagementList.AddRange(ERMService.GetDVManagementProduction_V2390(IdSite, CurrencyNameFromSetting, DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),
                            //       DateTime.ParseExact(ToDate, "dd/MM/yyyy", null)));
                            //ERMDeliveryVisualManagementList.AddRange(ERMService.GetDVManagementProduction_V2410(IdSite, DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),
                            //      DateTime.ParseExact(ToDate, "dd/MM/yyyy", null))); // [GEOS2-4606][gulab lakade]

                            ERMDeliveryVisualManagementList.AddRange(ERMService.GetDVManagementProduction_V2440(IdSite, DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),
                                 DateTime.ParseExact(ToDate, "dd/MM/yyyy", null))); //[GEOS2-4821][Rupali Sarode][14-09-2023]
                        }
                        //catch (Exception ex)
                        //{
                        //    GeosApplication.Instance.Logger.Log("Get an error in FillDVManagementProduction()", category: Category.Exception, priority: Priority.Low);
                        //}
                        catch (FaultException<ServiceException> ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = string.Empty;
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed");
                            if (!ERMCommon.Instance.FailedPlants.Contains(itemPlantOwnerUsers.Name))
                            {
                                ERMCommon.Instance.FailedPlants.Add(itemPlantOwnerUsers.Name);
                                if (ERMCommon.Instance.FailedPlants != null && ERMCommon.Instance.FailedPlants.Count > 0)
                                {
                                    ERMCommon.Instance.IsShowFailedPlantWarning = true;
                                    ERMCommon.Instance.WarningFailedPlants = string.Join(",", ERMCommon.Instance.FailedPlants.Select(x => x.ToString()).ToArray());
                                    ERMCommon.Instance.WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), ERMCommon.Instance.WarningFailedPlants.ToString());
                                }
                            }
                            System.Threading.Thread.Sleep(1000);
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed ", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                        }
                        catch (ServiceUnexceptedException ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = string.Empty;
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed");
                            if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                if (!ERMCommon.Instance.FailedPlants.Contains(itemPlantOwnerUsers.Name))
                                {
                                    ERMCommon.Instance.FailedPlants.Add(itemPlantOwnerUsers.Name);
                                    if (ERMCommon.Instance.FailedPlants != null && ERMCommon.Instance.FailedPlants.Count > 0)
                                    {
                                        ERMCommon.Instance.IsShowFailedPlantWarning = true;
                                        ERMCommon.Instance.WarningFailedPlants = string.Join(",", ERMCommon.Instance.FailedPlants.Select(x => x.ToString()).ToArray());
                                        ERMCommon.Instance.WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), ERMCommon.Instance.WarningFailedPlants.ToString());
                                    }
                                }
                            System.Threading.Thread.Sleep(1000);
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = string.Empty;
                            GeosApplication.Instance.Logger.Log(string.Format("Error in method AcceptButtonCommandAction() in filling list - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed");
                            if (!ERMCommon.Instance.FailedPlants.Contains(itemPlantOwnerUsers.Name))
                            {
                                ERMCommon.Instance.FailedPlants.Add(itemPlantOwnerUsers.Name);
                                if (ERMCommon.Instance.FailedPlants != null && ERMCommon.Instance.FailedPlants.Count > 0)
                                {
                                    ERMCommon.Instance.IsShowFailedPlantWarning = true;
                                    ERMCommon.Instance.WarningFailedPlants = string.Join(",", ERMCommon.Instance.FailedPlants.Select(x => x.ToString()).ToArray());
                                    ERMCommon.Instance.WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), ERMCommon.Instance.WarningFailedPlants.ToString());
                                }
                            }
                            System.Threading.Thread.Sleep(1000);
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }
                    }

                    #region [GEOS2-4862][Rupali Sarode][30-09-2023]
                    ERMDeliveryVisualManagementList_Cloned = new List<ERMDeliveryVisualManagement>();
                    ERMDeliveryVisualManagementList_Cloned = ERMDeliveryVisualManagementList.ToList();

                    #endregion [GEOS2-4862][Rupali Sarode][30-09-2023]


                    GeosApplication.Instance.SplashScreenMessage = string.Empty;
                    if (ERMCommon.Instance.FailedPlants == null || ERMCommon.Instance.FailedPlants.Count == 0)
                    {
                        ERMCommon.Instance.IsShowFailedPlantWarning = false;
                        ERMCommon.Instance.WarningFailedPlants = string.Empty;
                    }
                }

                GeosApplication.Instance.Logger.Log("Method FillDVManagementProduction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log("Get an error in FillDVManagementProduction()", category: Category.Exception, priority: Priority.Low);
            }

        }


        private void ActionCreateTable()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ActionCreateTable ...", category: Category.Info, priority: Priority.Low);

                dp.Rows.Clear();
                //string[] actions = { "COM", "CAD" };

                //  foreach (var actionIndex in Enumerable.Range(0, actions.Length))

                //var AllStages = ERMDeliveryVisualManagementList.GroupBy(x => x.IdStage)
                //   .Select(group => new
                //   {
                //       Sequence = ERMDeliveryVisualManagementList.FirstOrDefault(a => a.IdStage == group.Key).Sequence,
                //       NewSequence = ERMDeliveryVisualManagementList.FirstOrDefault(a => a.IdStage == group.Key).NewSequence,
                //       IdStage = ERMDeliveryVisualManagementList.FirstOrDefault(a => a.IdStage == group.Key).IdStage,
                //       StageCode = ERMDeliveryVisualManagementList.FirstOrDefault(a => a.IdStage == group.Key).StageCode,
                //       Count = ERMDeliveryVisualManagementList.Where(b => b.IdStage == 0).Count(),
                //   }).ToList().OrderBy(i => i.NewSequence);


                DateTime TodaysDate = DateTime.Now.Date; //[GEOS2-4528][Rupali Sarode][30-05-2023]

                //foreach (var stage in AllStages)
                // foreach (var stage in AllStages.Where(x => x.StageCode == "COM" || x.StageCode == "CAD").ToList())
                #region [Pallavi Jadhav][GEOS2-4343][23-05-2023] -- Show stage only if present in ActiveInPlants
                foreach (var stage in ERMDeliveryVisualManagementStagesList.Where(x => x.StageCode == "COM" || x.StageCode == "CAD").ToList())
                {

                    if (stage.ActiveInPlants != null && stage.ActiveInPlants != "")
                    {
                        ArrActiveInPlants = stage.ActiveInPlants.Split(',');
                        List<Site> tmpSelectedPlant = ERMCommon.Instance.SelectedAuthorizedPlantsList.Cast<Site>().ToList();
                        FlagPresentInActivePlants = false;
                        foreach (Site itemSelectedPlant in tmpSelectedPlant)
                        {
                            if (FlagPresentInActivePlants == true)
                            {
                                break;
                            }

                            foreach (var iPlant in ArrActiveInPlants)
                            {
                                if (Convert.ToUInt16(iPlant) == itemSelectedPlant.IdSite)
                                {
                                    FlagPresentInActivePlants = true;
                                    break;
                                }
                            }

                        }
                    }

                    if (FlagPresentInActivePlants == true || stage.ActiveInPlants == null || stage.ActiveInPlants == "")
                    {

                        DataRow dr = dp.NewRow();

                        //if (stage.StageCode == "COM" || stage.StageCode == "CAD")
                        //{
                        //    //DataRow dr = dp.NewRow();
                        //    dr[0] = null;
                        //    dr[1] = null;
                        //    dr[2] = stage.StageCode.ToString().PadLeft(2, '0');

                        //    dr["Quality"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.IdOfferType == 2 || i.IdOfferType == 3) && i.CurrentWorkStation == stage.StageCode).Select(x => x.QTY).Sum();
                        //    ConditionQuality = ConditionQuality + Convert.ToUInt64(dr["Quality"]);
                        //    int QualityComCad = Convert.ToInt32(dr["Quality"]);

                        //    DateTime? tempDate = DateTime.Now.AddDays(7);
                        //    //  dr[">7Day"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.CounterPSSateDate.Value.Date > tempDate.Value.Date) && i.CurrentWorkStation == stage.StageCode).Select(x => x.QTY).Sum();
                        //    //[GEOS2-4528][Rupali Sarode][30-05-2023]
                        //    dr[">7Day"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.DeliveryDate.Value.Date > tempDate.Value.Date) && i.CurrentWorkStation == stage.StageCode).Select(x => x.QTY).Sum();
                        //    ConditionGreaterThan7Days = ConditionGreaterThan7Days + Convert.ToUInt64(dr[">7Day"]);
                        //    int day7ComCad = Convert.ToInt32(dr[">7Day"]);

                        //    tempDate = DateTime.Now;
                        //    // dr["Delay"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.CounterPSSateDate.Value.Date <= tempDate.Value.Date) && i.CurrentWorkStation == stage.StageCode).Select(x => x.QTY).Sum();
                        //    //[GEOS2-4528][Rupali Sarode][30-05-2023]
                        //    dr["Delay"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.DeliveryDate.Value.Date <= tempDate.Value.Date) && i.CurrentWorkStation == stage.StageCode).Select(x => x.QTY).Sum();
                        //    ConditionDelay = ConditionDelay + Convert.ToUInt64(dr["Delay"]);
                        //    int DelayComCad = Convert.ToInt32(dr["Delay"]);

                        //    tempDate = DateTime.Now.AddDays(7);
                        //    // dr["<=7Day"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.CounterPSSateDate.Value.Date <= tempDate.Value.Date) && i.CurrentWorkStation == stage.StageCode).Select(x => x.QTY).Sum();
                        //    //[GEOS2-4528][Rupali Sarode][30-05-2023]
                        //    dr["<=7Day"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.DeliveryDate.Value.Date > TodaysDate && i.DeliveryDate.Value.Date <= tempDate.Value.Date) && i.CurrentWorkStation == stage.StageCode).Select(x => x.QTY).Sum();
                        //    ConditionLessThanEqual7Day = ConditionLessThanEqual7Day + Convert.ToUInt64(dr["<=7Day"]);
                        //    int Day7lessthanorEqualtoComCad = Convert.ToInt32(dr["<=7Day"]);

                        //    dr["OnHold-T-S"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.IdOfferStatusType == 18) && i.CurrentWorkStation == stage.StageCode).Select(x => x.QTY).Sum();
                        //    ConditionOnHoldTS = ConditionOnHoldTS + Convert.ToUInt64(dr["OnHold-T-S"]);
                        //    int OnHoldTSComCad = Convert.ToInt32(dr["OnHold-T-S"]);

                        //    dr["Components"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.IdOfferType == 8) && i.CurrentWorkStation == stage.StageCode).Select(x => x.QTY).Sum();
                        //    ConditionComponents = ConditionComponents + Convert.ToUInt64(dr["Components"]);
                        //    int ComponentsComCad = Convert.ToInt32(dr["Components"]);

                        //    dr["GoAhead"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.GoAheadDate != null && i.PODate == null) && i.CurrentWorkStation == stage.StageCode).Select(x => x.QTY).Sum();
                        //    ConditionGoAhead = ConditionGoAhead + Convert.ToUInt64(dr["GoAhead"]);
                        //    int GoAheadComCad = Convert.ToInt32(dr["GoAhead"]);

                        //    dr["Year"] = QualityComCad + day7ComCad + DelayComCad + Day7lessthanorEqualtoComCad + OnHoldTSComCad + GoAheadComCad;

                        //    //dr[2] = dr[2].ToString() + "\n" + dr["Year"].ToString();
                        //    if (dr["Year"].ToString().Length == 1)
                        //    {
                        //        dr[2] = dr[2].ToString() + "\n   " + dr["Year"].ToString();
                        //    }
                        //    else if (dr["Year"].ToString().Length == 2)
                        //    {
                        //        dr[2] = dr[2].ToString() + "\n " + dr["Year"].ToString();
                        //    }

                        //    else
                        //    {
                        //        dr[2] = dr[2].ToString() + "\n" + dr["Year"].ToString();
                        //    }
                        //    dp.Rows.Add(dr);
                        //}
                        if (stage.StageCode == "COM" || stage.StageCode == "CAD")
                        {
                            //DataRow dr = dp.NewRow();
                            dr[0] = null;
                            dr[1] = null;
                            dr[2] = stage.StageCode.ToString().PadLeft(2, '0');

                            dr["Quality"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.IdOfferType == 2 || i.IdOfferType == 3) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).Select(x => x.QTY).Sum();
                            FilterDataList.AddRange(ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.IdOfferType == 2 || i.IdOfferType == 3) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).ToList());

                            ConditionQuality = ConditionQuality + Convert.ToUInt64(dr["Quality"]);
                            int QualityComCad = Convert.ToInt32(dr["Quality"]);

                            DateTime? tempDate = DateTime.Now.AddDays(7);
                            //  dr[">7Day"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.CounterPSSateDate.Value.Date > tempDate.Value.Date) && i.CurrentWorkStation == stage.StageCode).Select(x => x.QTY).Sum();
                            //[GEOS2-4528][Rupali Sarode][30-05-2023]
                            dr[">7Day"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.DeliveryDate.Value.Date > tempDate.Value.Date) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).Select(x => x.QTY).Sum();
                            FilterDataList.AddRange(ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.DeliveryDate.Value.Date > tempDate.Value.Date) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).ToList());

                            ConditionGreaterThan7Days = ConditionGreaterThan7Days + Convert.ToUInt64(dr[">7Day"]);
                            int day7ComCad = Convert.ToInt32(dr[">7Day"]);

                            tempDate = DateTime.Now;
                            // dr["Delay"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.CounterPSSateDate.Value.Date <= tempDate.Value.Date) && i.CurrentWorkStation == stage.StageCode).Select(x => x.QTY).Sum();
                            //[GEOS2-4528][Rupali Sarode][30-05-2023]
                            dr["Delay"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.DeliveryDate.Value.Date <= tempDate.Value.Date) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).Select(x => x.QTY).Sum();
                            FilterDataList.AddRange(ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.DeliveryDate.Value.Date <= tempDate.Value.Date) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).ToList());

                            ConditionDelay = ConditionDelay + Convert.ToUInt64(dr["Delay"]);
                            int DelayComCad = Convert.ToInt32(dr["Delay"]);

                            tempDate = DateTime.Now.AddDays(7);
                            // dr["<=7Day"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.CounterPSSateDate.Value.Date <= tempDate.Value.Date) && i.CurrentWorkStation == stage.StageCode).Select(x => x.QTY).Sum();
                            //[GEOS2-4528][Rupali Sarode][30-05-2023]
                            dr["<=7Day"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.DeliveryDate.Value.Date > TodaysDate && i.DeliveryDate.Value.Date <= tempDate.Value.Date) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).Select(x => x.QTY).Sum();
                            FilterDataList.AddRange(ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.DeliveryDate.Value.Date > TodaysDate && i.DeliveryDate.Value.Date <= tempDate.Value.Date) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).ToList());

                            ConditionLessThanEqual7Day = ConditionLessThanEqual7Day + Convert.ToUInt64(dr["<=7Day"]);
                            int Day7lessthanorEqualtoComCad = Convert.ToInt32(dr["<=7Day"]);

                            dr["OnHold-T-S"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.IdOfferStatusType == 18) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).Select(x => x.QTY).Sum();
                            FilterDataList.AddRange(ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.IdOfferStatusType == 18) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).ToList());

                            ConditionOnHoldTS = ConditionOnHoldTS + Convert.ToUInt64(dr["OnHold-T-S"]);
                            int OnHoldTSComCad = Convert.ToInt32(dr["OnHold-T-S"]);

                            dr["Components"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.IdTemplate == 9) && i.CurrentWorkStation == stage.StageCode).Select(x => x.QTY).Sum();
                            FilterDataList.AddRange(ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.IdTemplate == 9) && i.CurrentWorkStation == stage.StageCode).ToList());

                            ConditionComponents = ConditionComponents + Convert.ToUInt64(dr["Components"]);
                            int ComponentsComCad = Convert.ToInt32(dr["Components"]);

                            //  dr["GoAhead"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.GoAheadDate != null && i.PODate == null) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).Select(x => x.QTY).Sum();
                            dr["GoAhead"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.GoAheadDate != null) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).Select(x => x.QTY).Sum();
                            FilterDataList.AddRange(ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.GoAheadDate != null) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).ToList());

                            ConditionGoAhead = ConditionGoAhead + Convert.ToUInt64(dr["GoAhead"]);
                            int GoAheadComCad = Convert.ToInt32(dr["GoAhead"]);

                            dr["Year"] = QualityComCad + day7ComCad + DelayComCad + Day7lessthanorEqualtoComCad + OnHoldTSComCad + GoAheadComCad;

                            //dr[2] = dr[2].ToString() + "\n" + dr["Year"].ToString();


                            //if (dr["Year"].ToString().Length == 1)
                            //{
                            //    dr[2] = dr[2].ToString() + "\n   " + dr["Year"].ToString();
                            //}
                            //else if (dr["Year"].ToString().Length == 2)
                            //{
                            //    dr[2] = dr[2].ToString() + "\n " + dr["Year"].ToString();
                            //}

                            //else
                            //{
                            //    dr[2] = dr[2].ToString() + "\n" + dr["Year"].ToString();
                            //}

                            if (dr["Year"].ToString().Length == 1)
                            {
                                dr[2] = "(" + dr["Year"].ToString() + ") " + dr[2].ToString();
                            }
                            else if (dr["Year"].ToString().Length == 2)
                            {
                                dr[2] = "(" + dr["Year"].ToString() + ") " + dr[2].ToString();
                            }

                            else
                            {
                                dr[2] = "(" + dr["Year"].ToString() + ") " + dr[2].ToString();
                            }


                            dp.Rows.Add(dr);
                        }
                        //else
                        //     if (stage.StageCode == "CAD")
                        //{
                        //    //DataRow dr = dp.NewRow();
                        //    dr[0] = null;
                        //    dr[1] = null;
                        //    dr[2] = stage.StageCode.ToString().PadLeft(2, '0');

                        //    dr["Quality"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.IdOfferType == 2 || i.IdOfferType == 3) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).Select(x => x.QTY).Sum();
                        //    FilterDataList.AddRange(ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.IdOfferType == 2 || i.IdOfferType == 3) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).ToList());

                        //    ConditionQuality = ConditionQuality + Convert.ToUInt64(dr["Quality"]);
                        //    int QualityComCad = Convert.ToInt32(dr["Quality"]);

                        //    DateTime? tempDate = DateTime.Now.AddDays(7);
                        //    //  dr[">7Day"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.CounterPSSateDate.Value.Date > tempDate.Value.Date) && i.CurrentWorkStation == stage.StageCode).Select(x => x.QTY).Sum();
                        //    //[GEOS2-4528][Rupali Sarode][30-05-2023]
                        //    dr[">7Day"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.DeliveryDate.Value.Date > tempDate.Value.Date) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).Select(x => x.QTY).Sum();
                        //    FilterDataList.AddRange(ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.DeliveryDate.Value.Date > tempDate.Value.Date) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).ToList());

                        //    ConditionGreaterThan7Days = ConditionGreaterThan7Days + Convert.ToUInt64(dr[">7Day"]);
                        //    int day7ComCad = Convert.ToInt32(dr[">7Day"]);

                        //    tempDate = DateTime.Now;
                        //    // dr["Delay"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.CounterPSSateDate.Value.Date <= tempDate.Value.Date) && i.CurrentWorkStation == stage.StageCode).Select(x => x.QTY).Sum();
                        //    //[GEOS2-4528][Rupali Sarode][30-05-2023]
                        //    dr["Delay"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.DeliveryDate.Value.Date <= tempDate.Value.Date) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).Select(x => x.QTY).Sum();
                        //    FilterDataList.AddRange(ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.DeliveryDate.Value.Date <= tempDate.Value.Date) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).ToList());

                        //    ConditionDelay = ConditionDelay + Convert.ToUInt64(dr["Delay"]);
                        //    int DelayComCad = Convert.ToInt32(dr["Delay"]);

                        //    tempDate = DateTime.Now.AddDays(7);
                        //    // dr["<=7Day"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.CounterPSSateDate.Value.Date <= tempDate.Value.Date) && i.CurrentWorkStation == stage.StageCode).Select(x => x.QTY).Sum();
                        //    //[GEOS2-4528][Rupali Sarode][30-05-2023]
                        //    dr["<=7Day"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.DeliveryDate.Value.Date > TodaysDate && i.DeliveryDate.Value.Date <= tempDate.Value.Date) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).Select(x => x.QTY).Sum();
                        //    FilterDataList.AddRange(ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.DeliveryDate.Value.Date > TodaysDate && i.DeliveryDate.Value.Date <= tempDate.Value.Date) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).ToList());

                        //    ConditionLessThanEqual7Day = ConditionLessThanEqual7Day + Convert.ToUInt64(dr["<=7Day"]);
                        //    int Day7lessthanorEqualtoComCad = Convert.ToInt32(dr["<=7Day"]);

                        //    dr["OnHold-T-S"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.IdOfferStatusType == 18) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).Select(x => x.QTY).Sum();
                        //    FilterDataList.AddRange(ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.IdOfferStatusType == 18) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).ToList());

                        //    ConditionOnHoldTS = ConditionOnHoldTS + Convert.ToUInt64(dr["OnHold-T-S"]);
                        //    int OnHoldTSComCad = Convert.ToInt32(dr["OnHold-T-S"]);

                        //    dr["Components"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.IdTemplate == 9) && i.CurrentWorkStation == stage.StageCode).Select(x => x.QTY).Sum();
                        //    FilterDataList.AddRange(ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.IdTemplate == 9) && i.CurrentWorkStation == stage.StageCode).ToList());
                        //    ConditionComponents = ConditionComponents + Convert.ToUInt64(dr["Components"]);
                        //    int ComponentsComCad = Convert.ToInt32(dr["Components"]);

                        //    //  dr["GoAhead"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.GoAheadDate != null && i.PODate == null) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).Select(x => x.QTY).Sum();
                        //    dr["GoAhead"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.GoAheadDate != null) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).Select(x => x.QTY).Sum();
                        //    FilterDataList.AddRange(ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.GoAheadDate != null) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).ToList());

                        //    ConditionGoAhead = ConditionGoAhead + Convert.ToUInt64(dr["GoAhead"]);
                        //    int GoAheadComCad = Convert.ToInt32(dr["GoAhead"]);

                        //    dr["Year"] = QualityComCad + day7ComCad + DelayComCad + Day7lessthanorEqualtoComCad + OnHoldTSComCad + GoAheadComCad;

                        //    //dr[2] = dr[2].ToString() + "\n" + dr["Year"].ToString();
                        //    if (dr["Year"].ToString().Length == 1)
                        //    {
                        //        dr[2] = dr[2].ToString() + "\n   " + dr["Year"].ToString();
                        //    }
                        //    else if (dr["Year"].ToString().Length == 2)
                        //    {
                        //        dr[2] = dr[2].ToString() + "\n " + dr["Year"].ToString();
                        //    }

                        //    else
                        //    {
                        //        dr[2] = dr[2].ToString() + "\n" + dr["Year"].ToString();
                        //    }
                        //    dp.Rows.Add(dr);
                        //    #region [4606] [gulab lakade]
                        //    //DataRow dr1 = dp.NewRow();
                        //    //dr1[0] = null;
                        //    //dr1[1] = null;
                        //    //dr1[2] = "3D-" + stage.StageCode.ToString().PadLeft(2, '0');
                        //    //dr1["Quality"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.IdOfferType == 2 || i.IdOfferType == 3) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate == 27).Select(x => x.QTY).Sum();
                        //    //ConditionQuality = ConditionQuality + Convert.ToUInt64(dr1["Quality"]);
                        //    //int QualityComCad1 = Convert.ToInt32(dr1["Quality"]);

                        //    //DateTime? tempDate1 = DateTime.Now.AddDays(7);
                        //    ////  dr[">7Day"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.CounterPSSateDate.Value.Date > tempDate.Value.Date) && i.CurrentWorkStation == stage.StageCode).Select(x => x.QTY).Sum();
                        //    ////[GEOS2-4528][Rupali Sarode][30-05-2023]
                        //    //dr1[">7Day"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.DeliveryDate.Value.Date > tempDate.Value.Date) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate == 27).Select(x => x.QTY).Sum();
                        //    //ConditionGreaterThan7Days = ConditionGreaterThan7Days + Convert.ToUInt64(dr1[">7Day"]);
                        //    //int day7ComCad1 = Convert.ToInt32(dr1[">7Day"]);

                        //    //tempDate1 = DateTime.Now;
                        //    //// dr["Delay"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.CounterPSSateDate.Value.Date <= tempDate.Value.Date) && i.CurrentWorkStation == stage.StageCode).Select(x => x.QTY).Sum();
                        //    ////[GEOS2-4528][Rupali Sarode][30-05-2023]
                        //    //dr1["Delay"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.DeliveryDate.Value.Date <= tempDate.Value.Date) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate == 27).Select(x => x.QTY).Sum();
                        //    //ConditionDelay = ConditionDelay + Convert.ToUInt64(dr1["Delay"]);
                        //    //int DelayComCad1 = Convert.ToInt32(dr1["Delay"]);

                        //    //tempDate1 = DateTime.Now.AddDays(7);
                        //    //// dr["<=7Day"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.CounterPSSateDate.Value.Date <= tempDate.Value.Date) && i.CurrentWorkStation == stage.StageCode).Select(x => x.QTY).Sum();
                        //    ////[GEOS2-4528][Rupali Sarode][30-05-2023]
                        //    //dr1["<=7Day"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.DeliveryDate.Value.Date > TodaysDate && i.DeliveryDate.Value.Date <= tempDate.Value.Date) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate == 27).Select(x => x.QTY).Sum();
                        //    //ConditionLessThanEqual7Day = ConditionLessThanEqual7Day + Convert.ToUInt64(dr1["<=7Day"]);
                        //    //int Day7lessthanorEqualtoComCad1 = Convert.ToInt32(dr1["<=7Day"]);

                        //    //dr1["OnHold-T-S"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.IdOfferStatusType == 18) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate == 27).Select(x => x.QTY).Sum();
                        //    //ConditionOnHoldTS = ConditionOnHoldTS + Convert.ToUInt64(dr1["OnHold-T-S"]);
                        //    //int OnHoldTSComCad1 = Convert.ToInt32(dr1["OnHold-T-S"]);

                        //    //dr1["Components"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.IdOfferType == 8) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate == 27).Select(x => x.QTY).Sum();
                        //    //ConditionComponents = ConditionComponents + Convert.ToUInt64(dr1["Components"]);
                        //    //int ComponentsComCad1 = Convert.ToInt32(dr1["Components"]);

                        //    //// dr1["GoAhead"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.GoAheadDate != null && i.PODate == null) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate == 27).Select(x => x.QTY).Sum();
                        //    //dr1["GoAhead"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.GoAheadDate != null) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate == 27).Select(x => x.QTY).Sum();
                        //    //ConditionGoAhead = ConditionGoAhead + Convert.ToUInt64(dr1["GoAhead"]);
                        //    //int GoAheadComCad1 = Convert.ToInt32(dr1["GoAhead"]);

                        //    //dr1["Year"] = QualityComCad1 + day7ComCad1 + DelayComCad1 + Day7lessthanorEqualtoComCad1 + OnHoldTSComCad1 + GoAheadComCad1;

                        //    ////dr1[2] = dr1[2].ToString() + "\n" + dr1["Year"].ToString();
                        //    //if (dr1["Year"].ToString().Length == 1)
                        //    //{
                        //    //    dr1[2] = dr1[2].ToString() + "\n   " + dr1["Year"].ToString();
                        //    //}
                        //    //else if (dr1["Year"].ToString().Length == 2)
                        //    //{
                        //    //    dr1[2] = dr1[2].ToString() + "\n " + dr1["Year"].ToString();
                        //    //}

                        //    //else
                        //    //{
                        //    //    dr1[2] = dr1[2].ToString() + "\n" + dr1["Year"].ToString();
                        //    //}
                        //    //dp.Rows.Add(dr1);
                        //    #endregion
                        //}

                    }
                }

                #endregion

                GeosApplication.Instance.Logger.Log("Method ActionCreateTable() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ActionCreateTable() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

            ActionGraphDataTable = dp;
            if (chartControl != null) { chartControl.UpdateData(); }
        }

        private void PrintChart(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintChart ...", category: Category.Info, priority: Priority.Low);

                IsBusy = true;

                PrintableControlLink pcl = new PrintableControlLink((ChartControl)obj);

                pcl.Margins.Bottom = 5;
                pcl.Margins.Top = 5;
                pcl.Margins.Left = 5;
                pcl.Margins.Right = 5;
                pcl.PageHeaderTemplate = (DataTemplate)((ChartControl)obj).Resources["PageHeader"];
                pcl.PageFooterTemplate = (DataTemplate)((ChartControl)obj).Resources["PageFooter"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);
                pcl.Landscape = true;
                pcl.CreateDocument(false);

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                IsBusy = false;
                window.Show();

                GeosApplication.Instance.Logger.Log("Method PrintChart() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in PrintChart() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }
        #region Period Chanegs
        private void setDefaultPeriod()
        {

            int year = DateTime.Now.Year;
            DateTime StartFromDate = new DateTime(year, 1, 1);
            DateTime EndToDate = new DateTime(year, 12, 31);
            FromDate = StartFromDate.ToString("dd/MM/yyyy");
            ToDate = EndToDate.ToString("dd/MM/yyyy");

        }

        private void PeriodCommandAction(object obj)
        {
            if (obj == null)
                return;

            Button button = (Button)obj;
            if (button.Name == "ThisMonth")
            {
                IsButtonStatus = 1;
            }
            else if (button.Name == "LastOneMonth")
            {
                IsButtonStatus = 2;
            }
            else if (button.Name == "LastMonth")
            {
                IsButtonStatus = 3;
            }
            else if (button.Name == "ThisWeek")
            {
                IsButtonStatus = 4;
            }
            else if (button.Name == "LastOneWeek")
            {
                IsButtonStatus = 5;
            }
            else if (button.Name == "LastWeek")
            {
                IsButtonStatus = 6;
            }
            else if (button.Name == "CustomRange")
            {
                IsButtonStatus = 7;
            }
            else if (button.Name == "ThisYear")
            {
                IsButtonStatus = 8;
            }
            else if (button.Name == "LastYear")
            {
                IsButtonStatus = 9;
            }
            else if (button.Name == "Last12Months")
            {
                IsButtonStatus = 10;
            }
            IsCalendarVisible = Visibility.Collapsed;
        }
        private void PeriodCustomRangeCommandAction(object obj)
        {
            IsButtonStatus = 7;
            IsCalendarVisible = Visibility.Visible;
        }
        private void ApplyCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor ApplyCommandAction ...", category: Category.Info, priority: Priority.Low);

                MenuFlyout menu = (MenuFlyout)obj;
                _currentDuration = menu.FlyoutControl.AnimationDuration;
                menu.FlyoutControl.AnimationDuration = new System.Windows.Duration(TimeSpan.FromMilliseconds(1));
                menu.FlyoutControl.Closed += FlyoutControl_Closed;
                menu.IsOpen = false;
                GeosApplication.Instance.Logger.Log("Method ApplyCommandAction....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ApplyCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CancelCommandAction(object obj)
        {
            MenuFlyout menu = (MenuFlyout)obj;
            menu.IsOpen = false;
        }


        private void FlyoutControl_Closed(object sender, EventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor FlyoutControl_Closed ...", category: Category.Info, priority: Priority.Low);
                var flyout = (sender as FlyoutControl);
                flyout.AnimationDuration = _currentDuration;
                flyout.Closed -= FlyoutControl_Closed;
                Processing();

                DateTime baseDate = DateTime.Today;
                var today = baseDate;
                //this week
                var thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek);
                var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
                //Last week
                var lastWeekStart = thisWeekStart.AddDays(-7);
                var lastWeekEnd = thisWeekStart.AddSeconds(-1);
                //this month
                var thisMonthStart = baseDate.AddDays(1 - baseDate.Day);
                var thisMonthEnd = thisMonthStart.AddMonths(1).AddSeconds(-1);
                //last month
                var lastMonthStart = thisMonthStart.AddMonths(-1);
                var lastMonthEnd = thisMonthStart.AddSeconds(-1);
                //last one month
                var lastOneMonthStart = baseDate.AddMonths(-1);
                var lastOneMonthEnd = baseDate;
                //Last one week
                var lastOneWeekStart = baseDate.AddDays(-7);
                var lastOneWeekEnd = baseDate;
                //Last Year
                int year = DateTime.Now.Year - 1;
                DateTime StartFromDate = new DateTime(year, 1, 1);
                DateTime EndToDate = new DateTime(year, 12, 31);

                if (IsButtonStatus == 0)
                {
                    setDefaultPeriod();
                }
                else if (IsButtonStatus == 1)//this month
                {
                    FromDate = thisMonthStart.ToString("dd/MM/yyyy");
                    ToDate = thisMonthEnd.ToString("dd/MM/yyyy");
                    PlanningSchedulerControl scheduler = new PlanningSchedulerControl();
                    DateTime start = Convert.ToDateTime(FromDate);
                    DateTime end = Convert.ToDateTime(FromDate).AddDays(1);
                    scheduler.SelectedInterval = new DevExpress.Mvvm.DateTimeRange(start, end);

                    DateTime startDate = new DateTime(Convert.ToInt32(Convert.ToDateTime(FromDate).Year), Convert.ToDateTime(FromDate).Month, 1);
                    scheduler.Month = startDate;
                    scheduler.SelectedInterval = new DevExpress.Mvvm.DateTimeRange(Convert.ToDateTime(FromDate), Convert.ToDateTime(FromDate).AddDays(1));
                }
                else if (IsButtonStatus == 2)//last one month
                {
                    FromDate = lastOneMonthStart.ToString("dd/MM/yyyy");
                    ToDate = lastOneMonthEnd.ToString("dd/MM/yyyy");

                }
                else if (IsButtonStatus == 3) //last month
                {
                    FromDate = lastMonthStart.ToString("dd/MM/yyyy");
                    ToDate = lastMonthEnd.ToString("dd/MM/yyyy");
                }
                else if (IsButtonStatus == 4) //this week
                {
                    FromDate = thisWeekStart.ToString("dd/MM/yyyy");
                    ToDate = thisWeekEnd.ToString("dd/MM/yyyy");
                }
                else if (IsButtonStatus == 5) //last one week
                {
                    FromDate = lastOneWeekStart.ToString("dd/MM/yyyy");
                    ToDate = lastOneWeekEnd.ToString("dd/MM/yyyy");
                }
                else if (IsButtonStatus == 6) //last week
                {
                    FromDate = lastWeekStart.ToString("dd/MM/yyyy");
                    ToDate = lastWeekEnd.ToString("dd/MM/yyyy");
                }
                else if (IsButtonStatus == 7) //custome range
                {
                    FromDate = StartDate.ToString("dd/MM/yyyy");
                    ToDate = EndDate.ToString("dd/MM/yyyy");
                }
                else if (IsButtonStatus == 8)//this year
                {
                    setDefaultPeriod();
                }
                else if (IsButtonStatus == 9)//last year
                {
                    FromDate = StartFromDate.ToString("dd/MM/yyyy");
                    ToDate = EndToDate.ToString("dd/MM/yyyy");
                }
                else if (IsButtonStatus == 10)//last 12 month
                {
                    DateTime Date_F = DateTime.Now.Date.AddMonths(-12);
                    DateTime Date_T = DateTime.Now.Date;
                    FromDate = Date_F.ToString("dd/MM/yyyy");
                    ToDate = Date_T.ToString("dd/MM/yyyy");
                }


                if (ERMCommon.Instance.SelectedAuthorizedPlantsList != null)
                {
                    GetDVManagementProduction(ERMCommon.Instance.SelectedAuthorizedPlantsList, FromDate, ToDate);
                    FillProductionIntime();
                    AddColumnsToDataTableWithBandsinTestboardProduction();
                    FillDashboardProductionIntime();
                    ReadXmlFile();
                    FillFilters();
                    FillRTMFutureLoadList();
                }
                //   FillDVManagementProduction();
                IsBusy = false;
                IsPeriod = true;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method FlyoutControl_Closed....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in FlyoutControl_Closed() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in FlyoutControl_Closed() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in FlyoutControl_Closed() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        private Action Processing()
        {
            IsBusy = true;
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
            return null;
        }

        #endregion


        private void GetDVManagementProduction(List<object> SelectedPlant, string FromDate, string ToDate)
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Method GetDVManagementProduction ...", category: Category.Info, priority: Priority.Low);
                ERMCommon.Instance.IsShowFailedPlantWarning = false;
                ERMCommon.Instance.WarningFailedPlants = string.Empty;
                if (ERMCommon.Instance.SelectedAuthorizedPlantsList != null)
                {
                    List<Site> plantOwners = ERMCommon.Instance.SelectedAuthorizedPlantsList.Cast<Site>().ToList();
                    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.Name));
                    if (ERMCommon.Instance.FailedPlants == null)
                    {
                        ERMCommon.Instance.FailedPlants = new List<string>();
                    }

                    FailedPlants = new List<string>();
                    ERMDeliveryVisualManagementList = new ObservableCollection<ERMDeliveryVisualManagement>();

                    foreach (var itemPlantOwnerUsers in plantOwners)
                    {
                        try
                        {
                            string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.Name == itemPlantOwnerUsers.Name).Select(xy => xy.ServiceProviderUrl).FirstOrDefault();
                            ERMService = new ERMServiceController(serviceurl); // [Rupali Sarode][08-06-2023]

                            GeosApplication.Instance.SplashScreenMessage = "Connecting to " + itemPlantOwnerUsers.Name;

                            string IdSite = Convert.ToString(itemPlantOwnerUsers.IdSite);


                            var CurrencyNameFromSetting = String.Empty;

                            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("ERM_SelectedCurrency"))
                            {
                                CurrencyNameFromSetting = GeosApplication.Instance.UserSettings["ERM_SelectedCurrency"];
                            }
                            //ERMService = new ERMServiceController("localhost:6699");
                            //ERMDeliveryVisualManagementList.AddRange(ERMService.GetDVManagementProduction_V2390(IdSite, CurrencyNameFromSetting, DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),
                            //       DateTime.ParseExact(ToDate, "dd/MM/yyyy", null)));
                            //ERMDeliveryVisualManagementList.AddRange(ERMService.GetDVManagementProduction_V2410(IdSite, DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),
                            //       DateTime.ParseExact(ToDate, "dd/MM/yyyy", null)));//[4606]

                            ERMDeliveryVisualManagementList.AddRange(ERMService.GetDVManagementProduction_V2440(IdSite, DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),
                                  DateTime.ParseExact(ToDate, "dd/MM/yyyy", null))); //[GEOS2-4821][Rupali Sarode][14-09-2023]
                        }

                        //catch (Exception ex)
                        //{
                        //    GeosApplication.Instance.Logger.Log("Get an error in GetDVManagementProduction()", category: Category.Exception, priority: Priority.Low);
                        //}
                        catch (FaultException<ServiceException> ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = string.Empty;
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed");
                            if (!ERMCommon.Instance.FailedPlants.Contains(itemPlantOwnerUsers.Name))
                            {
                                ERMCommon.Instance.FailedPlants.Add(itemPlantOwnerUsers.Name);
                                if (ERMCommon.Instance.FailedPlants != null && ERMCommon.Instance.FailedPlants.Count > 0)
                                {
                                    ERMCommon.Instance.IsShowFailedPlantWarning = true;
                                    ERMCommon.Instance.WarningFailedPlants = string.Join(",", ERMCommon.Instance.FailedPlants.Select(x => x.ToString()).ToArray());
                                    ERMCommon.Instance.WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), ERMCommon.Instance.WarningFailedPlants.ToString());
                                }
                            }
                            System.Threading.Thread.Sleep(1000);
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed ", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                        }
                        catch (ServiceUnexceptedException ex)
                        {

                            GeosApplication.Instance.SplashScreenMessage = string.Empty;
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed");
                            if (!ERMCommon.Instance.FailedPlants.Contains(itemPlantOwnerUsers.Name))
                            {
                                ERMCommon.Instance.FailedPlants.Add(itemPlantOwnerUsers.Name);
                                if (ERMCommon.Instance.FailedPlants != null && ERMCommon.Instance.FailedPlants.Count > 0)
                                {
                                    ERMCommon.Instance.IsShowFailedPlantWarning = true;
                                    ERMCommon.Instance.WarningFailedPlants = string.Join(",", ERMCommon.Instance.FailedPlants.Select(x => x.ToString()).ToArray());
                                    ERMCommon.Instance.WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), ERMCommon.Instance.WarningFailedPlants.ToString());
                                }
                            }
                            System.Threading.Thread.Sleep(1000);
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }
                        catch (Exception ex)
                        {

                            GeosApplication.Instance.SplashScreenMessage = string.Empty;
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed");
                            if (!ERMCommon.Instance.FailedPlants.Contains(itemPlantOwnerUsers.Name))
                            {
                                ERMCommon.Instance.FailedPlants.Add(itemPlantOwnerUsers.Name);
                                if (ERMCommon.Instance.FailedPlants != null && ERMCommon.Instance.FailedPlants.Count > 0)
                                {
                                    ERMCommon.Instance.IsShowFailedPlantWarning = true;
                                    ERMCommon.Instance.WarningFailedPlants = string.Join(",", ERMCommon.Instance.FailedPlants.Select(x => x.ToString()).ToArray());
                                    ERMCommon.Instance.WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), ERMCommon.Instance.WarningFailedPlants.ToString());
                                }
                            }
                            System.Threading.Thread.Sleep(1000);
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }
                    }

                    #region [GEOS2-4862][Rupali Sarode][30-09-2023]
                    ERMDeliveryVisualManagementList_Cloned = new List<ERMDeliveryVisualManagement>();
                    ERMDeliveryVisualManagementList_Cloned = ERMDeliveryVisualManagementList.ToList();
                    #endregion [GEOS2-4862][Rupali Sarode][30-09-2023]


                    GeosApplication.Instance.SplashScreenMessage = string.Empty;
                    if (FailedPlants == null || FailedPlants.Count == 0)
                    {
                        IsShowFailedPlantWarning = false;
                        WarningFailedPlants = string.Empty;
                    }

                    ResetConditionValues(); //[GEOS2-4342][Rupali Sarode][11-05-2023]

                    CreateTable();
                    ActionCreateTable();

                    TotalQuantity = ConditionGreaterThan7Days + ConditionLessThanEqual7Day + ConditionDelay + ConditionQuality + ConditionGoAhead; //[GEOS2-4342][Rupali Sarode][11-05-2023]

                }
                GeosApplication.Instance.Logger.Log("Method GetDVManagementProduction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log("Get an error in GetDVManagementProduction()", category: Category.Exception, priority: Priority.Low);
            }

        }
        private void ChangePlantCommandAction(object obj)
        {
            try
            {


                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                ERMRTMHRResourcesStageList = new List<DeliveryVisualManagementStages>();
                if (obj == null)
                {


                }
                else
                {

                    DevExpress.Xpf.Editors.ClosePopupEventArgs CloseModetemp = (DevExpress.Xpf.Editors.ClosePopupEventArgs)obj;
                    // DevExpress.Xpf.Editors.PopupCloseMode closetemp=(DevExpress.Xpf.Editors.PopupCloseMode)obj;
                    //if (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
                    if (CloseModetemp.CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
                    {
                        //return;
                    }
                    else
                    {
                        GeosApplication.Instance.Logger.Log("Method ChangePlantCommandAction()...", category: Category.Info, priority: Priority.Low);

                        var TempSelectedPlant = (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).EditValue);
                        List<Object> TmpSelectedPlant = new List<object>();

                        foreach (var tmpPlant in (dynamic)TempSelectedPlant)
                        {
                            TmpSelectedPlant.Add(tmpPlant);
                        }

                        ERMCommon.Instance.SelectedAuthorizedPlantsList = new List<object>();
                        ERMCommon.Instance.SelectedAuthorizedPlantsList = TmpSelectedPlant;

                        if (ERMCommon.Instance.SelectedAuthorizedPlantsList != null)
                        {
                            try
                            {
                                GetDVManagementProduction(ERMCommon.Instance.SelectedAuthorizedPlantsList, FromDate, ToDate);
                                FillRTMData();
                                AddColumnsToDataTableWithBandsForHrResources(); //[pallavi jadhav][GEOS2-4869][9 27 2023]

                                FillDashboardHRResources();

                                FillRTMFutureLoadList();
                                //  AddColumnsToDataTableWithBands();
                                FillDashboard();

                                //FutureGridControlFilter.GroupBy("OfferDeliveryDateYear");
                                //FutureGridControlFilter.GroupBy("Site");
                                //FutureGridControlFilter.GroupBy("CalendarWeek");
                                //FutureGridControlFilter.GroupBy("Code");
                                //FutureGridControlFilter.GroupBy("Group");
                                //FutureGridControlFilter.GroupBy("CustomerSite");
                                //FutureGridControlFilter.GroupBy("BusinessUnit");
                                //FutureGridControlFilter.GroupBy("SalesOwner");

                                FillProductionIntime();

                                AddColumnsToDataTableWithBandsinTestboardProduction();
                                FillDashboardProductionIntime();
                                //ProductionGridControlFilter.GroupBy("DeliveryWeek");
                                //ProductionGridControlFilter.GroupBy("OTCode");
                                ReadXmlFile();
                                FillFilters();
                            }
                            catch (FaultException<ServiceException> ex)
                            {
                            }
                        }
                        else
                        {

                        }


                    }
                }


                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method ChangePlantCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method ChangePlantCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        #region [GEOS2-4342][Rupali Sarode][11-05-2023]
        private void ResetConditionValues()
        {
            ConditionQuality = 0;
            ConditionGreaterThan7Days = 0;
            ConditionDelay = 0;
            ConditionLessThanEqual7Day = 0;
            ConditionOnHoldTS = 0;
            ConditionComponents = 0;
            ConditionGoAhead = 0;
        }
        #endregion

        private void RefreshDashboardDetails(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshDashboardDetails ...", category: Category.Info, priority: Priority.Low);


                if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
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
                }

                FillDVManagementProduction();
                ResetConditionValues(); //[GEOS2-4342][Rupali Sarode][11-05-2023]

                CreateTable();
                ActionCreateTable();
                TotalQuantity = ConditionGreaterThan7Days + ConditionLessThanEqual7Day + ConditionDelay + ConditionQuality + ConditionGoAhead; //[GEOS2-4342][Rupali Sarode][11-05-2023]
                FillRTMData();
                AddColumnsToDataTableWithBandsForHrResources();//[gulab lakade][beta changes][24 02 2025]
                FillDashboardHRResources();
                FillRTMFutureLoadList();
                FillDashboard();
                FillProductionIntime();
                AddColumnsToDataTableWithBandsinTestboardProduction();
                FillDashboardProductionIntime();
                ReadXmlFile();
                FillFilters();

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }

                GeosApplication.Instance.Logger.Log("Method RefreshDashboardDetails() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in RefreshDashboardDetails() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        //[GEOS2-4343][Pallavi Jadhav][25 05 2023]
        private void FillStages()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillStages ...", category: Category.Info, priority: Priority.Low);

                ERMDeliveryVisualManagementStagesList = new List<DeliveryVisualManagementStages>();
                //ERMService = new ERMServiceController("localhost:6699");
                // ERMDeliveryVisualManagementStagesList.AddRange(ERMService.GetDVManagementProductionStage_V2400());

                ERMDeliveryVisualManagementStagesList.AddRange(ERMService.GetDVManagementProductionStage_V2440());

                GeosApplication.Instance.Logger.Log("Method FillStages() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillStages() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        #region [GEOS2-4351][Rupali Sarode][29-05-2023]
        private void TableViewLoadedCommandAction(RoutedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method TableViewLoadedCommandAction ...", category: Category.Info, priority: Priority.Low);

                int visibleFalseColumnsCount = 0;

                if (File.Exists(ERM_RealTimeMonitorGrid_SettingFilePath))
                {
                    ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.RestoreLayoutFromXml(ERM_RealTimeMonitorGrid_SettingFilePath);
                    GridControl gridControlInstance = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;
                    TableView tableView = (TableView)gridControlInstance.View;
                    this.tableViewInstance = tableView;

                    if (tableView.SearchString != null)
                    {
                        tableView.SearchString = null;
                    }
                }

                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.AddHandler(DXSerializer.AllowPropertyEvent, new AllowPropertyEventHandler(OnAllowProperty));

                //This code for save grid layout.
                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.SaveLayoutToXml(ERM_RealTimeMonitorGrid_SettingFilePath);

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
                        visibleFalseColumnsCount++;
                    }
                }

                if (visibleFalseColumnsCount > 0)
                {
                    IsColumnChooserVisibleForGrid = true;
                }
                else
                {
                    IsColumnChooserVisibleForGrid = false;
                }
                TableView datailView = ((TableView)((System.Windows.RoutedEventArgs)obj).OriginalSource);
                //  datailView.ShowTotalSummary = true;


                GeosApplication.Instance.Logger.Log("Method TableViewLoadedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in TableViewLoadedCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        private void OnAllowProperty(object sender, AllowPropertyEventArgs e)
        {
            try
            {
                if (e.DependencyProperty == TreeListControl.FilterStringProperty)
                    e.Allow = false;

                if (e.Property.Name == "GroupCount")
                    e.Allow = false;

                if (e.DependencyProperty == TableViewEx.SearchStringProperty)
                    e.Allow = false;

                if (e.Property.Name == "FormatConditions")
                    e.Allow = false;
            }
            catch (Exception ex)
            {

            }
        }

        void VisibleChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method VisibleChanged ...", category: Category.Info, priority: Priority.Low);

                GridColumn column = sender as GridColumn;
                if (column.ShowInColumnChooser)
                {
                    ((GridControl)((GridControlBand)((System.Windows.FrameworkContentElement)sender).Parent).Parent).SaveLayoutToXml(ERM_RealTimeMonitorGrid_SettingFilePath);
                    //((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(ERM_TimeTrackinggrid_SettingFilePath);

                }

                if (column.Visible == false)
                {
                    IsColumnChooserVisibleForGrid = true;
                }

                GeosApplication.Instance.Logger.Log("Method VisibleChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in VisibleChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        void VisibleIndexChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method VisibleIndexChanged ...", category: Category.Info, priority: Priority.Low);

                GridColumn column = sender as GridColumn;
                ((GridControl)((GridControlBand)((System.Windows.FrameworkContentElement)sender).Parent).Parent).SaveLayoutToXml(ERM_RealTimeMonitorGrid_SettingFilePath);
                //((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(ERM_TimeTrackinggrid_SettingFilePath);

                GeosApplication.Instance.Logger.Log("Method VisibleIndexChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in VisibleIndexChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void TableViewUnloadedCommandAction(RoutedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method TableViewUnloadedCommandAction ...", category: Category.Info, priority: Priority.Low);

                GridControl gridControl = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;
                TableView tableView = (TableView)gridControl.View;
                tableView.SearchString = string.Empty;
                if (gridControl.GroupCount > 0)
                    gridControl.ClearGrouping();
                gridControl.ClearSorting();
                gridControl.FilterString = null;
                gridControl.SaveLayoutToXml(ERM_RealTimeMonitorGrid_SettingFilePath);

                GeosApplication.Instance.Logger.Log("Method TableViewUnloadedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in TableViewUnloadedCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }


        //private void AddColumnsToDataTableWithBands()
        //{
        //    try
        //    {
        //        if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
        //        GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTableWithBands ...", category: Category.Info, priority: Priority.Low);
        //        Bands = new ObservableCollection<BandItem>();
        //        Bands.Clear();
        //        BandItem band0 = new BandItem() { BandName = "FirstRow", AllowBandMove = false, FixedStyle = FixedStyle.Left, Visible = true, AllowMoving = DevExpress.Utils.DefaultBoolean.False };
        //        band0.Columns = new ObservableCollection<ColumnItem>();
        //        band0.Columns.Add(new ColumnItem() { ColumnFieldName = "CalendarWeek", HeaderText = "Week", Width = 60, IsVertical = false, RealTimeMonitorSetting = RealTimeMonitorColumnTemplateSelector.RealTimeMonitorSettingType.CalendarWeek, Visible = true });


        //        Bands.Add(band0);


        //        BandItem band1 = new BandItem() { BandName = "Customer", BandHeader = "Cust.", Visible = true };
        //        band1.Columns = new ObservableCollection<ColumnItem>();
        //        band1.Columns.Add(new ColumnItem() { ColumnFieldName = "CustomerETD", HeaderText = "ETD", Width = 60, IsVertical = false, RealTimeMonitorSetting = RealTimeMonitorColumnTemplateSelector.RealTimeMonitorSettingType.CustomerETD, Visible = true });

        //        Bands.Add(band1);


        //        BandItem band2 = new BandItem() { BandName = "Production", BandHeader = "Prod.", Visible = true };
        //        band2.Columns = new ObservableCollection<ColumnItem>();
        //        band2.Columns.Add(new ColumnItem() { ColumnFieldName = "ProductionPlan", HeaderText = "Plan", Width = 60, IsVertical = false, RealTimeMonitorSetting = RealTimeMonitorColumnTemplateSelector.RealTimeMonitorSettingType.ProductionPlan, Visible = true });
        //        band2.Columns.Add(new ColumnItem() { ColumnFieldName = "Vision", HeaderText = "V", Width = 60, IsVertical = false, RealTimeMonitorSetting = RealTimeMonitorColumnTemplateSelector.RealTimeMonitorSettingType.ProductionVision, Visible = true });
        //        band2.Columns.Add(new ColumnItem() { ColumnFieldName = "Tightening", HeaderText = "T", Width = 60, IsVertical = false, RealTimeMonitorSetting = RealTimeMonitorColumnTemplateSelector.RealTimeMonitorSettingType.ProductionTightening, Visible = true });
        //        band2.Columns.Add(new ColumnItem() { ColumnFieldName = "ProductionO", HeaderText = "O", Width = 60, IsVertical = false, RealTimeMonitorSetting = RealTimeMonitorColumnTemplateSelector.RealTimeMonitorSettingType.ProductionO, Visible = true });

        //        Bands.Add(band2);


        //        BandItem band3 = new BandItem() { BandName = "RFQForecast", BandHeader = "RFQ + Forecast", Visible = true };
        //        band3.Columns = new ObservableCollection<ColumnItem>();
        //        band3.Columns.Add(new ColumnItem() { ColumnFieldName = "RFQ", HeaderText = "RFQ", Width = 60, IsVertical = false, RealTimeMonitorSetting = RealTimeMonitorColumnTemplateSelector.RealTimeMonitorSettingType.RFQ, Visible = true });
        //        band3.Columns.Add(new ColumnItem() { ColumnFieldName = "SpecialEquipments", HeaderText = "SP", Width = 60, IsVertical = false, RealTimeMonitorSetting = RealTimeMonitorColumnTemplateSelector.RealTimeMonitorSettingType.SpecialEquipments, Visible = true });
        //        band3.Columns.Add(new ColumnItem() { ColumnFieldName = "Forecast", HeaderText = "For.", Width = 60, IsVertical = false, RealTimeMonitorSetting = RealTimeMonitorColumnTemplateSelector.RealTimeMonitorSettingType.Forecast, Visible = true });
        //        Bands.Add(band3);

        //        DataTableForGridLayout = new DataTable();
        //        DataTableForGridLayout.Columns.Add("CalendarWeek", typeof(string));
        //        DataTableForGridLayout.Columns.Add("CustomerETD", typeof(string));
        //        DataTableForGridLayout.Columns.Add("ProductionPlan", typeof(string));
        //        DataTableForGridLayout.Columns.Add("Vision", typeof(string));
        //        DataTableForGridLayout.Columns.Add("Tightening", typeof(string));
        //        DataTableForGridLayout.Columns.Add("ProductionO", typeof(string));
        //        DataTableForGridLayout.Columns.Add("RFQ", typeof(string));
        //        DataTableForGridLayout.Columns.Add("SpecialEquipments", typeof(string));
        //        DataTableForGridLayout.Columns.Add("Forecast", typeof(string));

        //        Bands = new ObservableCollection<BandItem>(Bands);

        //        GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTableWithBands executed Successfully", category: Category.Info, priority: Priority.Low);
        //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Error in AddColumnsToDataTableWithBands() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}

        private void AddColumnsToDataTableWithBands()
        {
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTableWithBands ...", category: Category.Info, priority: Priority.Low);
                //Bands = new ObservableCollection<BandItem>();
                //Bands.Clear();
                //BandItem band0 = new BandItem() { BandName = "FirstRow", AllowBandMove = false, FixedStyle = FixedStyle.Left, Visible = true, AllowMoving = DevExpress.Utils.DefaultBoolean.False };
                //band0.Columns = new ObservableCollection<ColumnItem>();
                //band0.Columns.Add(new ColumnItem() { ColumnFieldName = "CalendarWeek", HeaderText = "Week", Width = 60, IsVertical = false, RealTimeMonitorSetting = RealTimeMonitorColumnTemplateSelector.RealTimeMonitorSettingType.CalendarWeek, Visible = true });
                Columns = new ObservableCollection<Emdep.Geos.UI.Helper.Column>()
                {
                    new Emdep.Geos.UI.Helper.Column() { FieldName="OfferDeliveryDateYear",HeaderText="", Settings = SettingsType.OfferDeliveryDateYear, AllowCellMerge=false, Width=60, AllowEditing = false, Visible= true, IsVertical = false, FixedWidth=true, IsReadOnly = true  },
                    new Emdep.Geos.UI.Helper.Column() { FieldName="Site",HeaderText="", Settings = SettingsType.SiteName, AllowCellMerge=false, Width=60, AllowEditing = false, Visible= true, IsVertical = false, FixedWidth=true, IsReadOnly = true  },
                    new Emdep.Geos.UI.Helper.Column() { FieldName="CalendarWeek",HeaderText="", Settings = SettingsType.CalendarWeek, AllowCellMerge=false, Width=120, AllowEditing = false, Visible= true, IsVertical = false, FixedWidth=true, IsReadOnly = true  },
                    new Emdep.Geos.UI.Helper.Column() { FieldName="Plant",HeaderText="Offer Close Year", Settings = SettingsType.CalendarWeek, AllowCellMerge=false, Width=120, AllowEditing = false, Visible= true, IsVertical = false, FixedWidth=true, IsReadOnly = true  },
                    new Emdep.Geos.UI.Helper.Column() { FieldName="Code",HeaderText="Offer Number", Settings = SettingsType.CalendarWeek, AllowCellMerge=false, Width=120, AllowEditing = false, Visible= true, IsVertical = false, FixedWidth=true, IsReadOnly = true  },
                    new Emdep.Geos.UI.Helper.Column() { FieldName="Group",HeaderText="Customer", Settings = SettingsType.CalendarWeek, AllowCellMerge=false, Width=120, AllowEditing = false, Visible= true, IsVertical = false, FixedWidth=true, IsReadOnly = true  },
                    new Emdep.Geos.UI.Helper.Column() { FieldName="CustomerSite",HeaderText="Plant", Settings = SettingsType.CalendarWeek, AllowCellMerge=false, Width=120, AllowEditing = false, Visible= true, IsVertical = false, FixedWidth=true, IsReadOnly = true  },
                    new Emdep.Geos.UI.Helper.Column() { FieldName="BusinessUnit",HeaderText="Business Unit", Settings = SettingsType.CalendarWeek, AllowCellMerge=false, Width=120, AllowEditing = false, Visible= true, IsVertical = false, FixedWidth=true, IsReadOnly = true  },
                    new Emdep.Geos.UI.Helper.Column() { FieldName="SalesOwner",HeaderText="Sales Owner", Settings = SettingsType.CalendarWeek, AllowCellMerge=false, Width=120, AllowEditing = false, Visible= true, IsVertical = false, FixedWidth=true, IsReadOnly = true  },

                };

                GroupSummary = new ObservableCollection<Emdep.Geos.UI.Helper.Summary>();
                TotalSummary = new ObservableCollection<Emdep.Geos.UI.Helper.Summary>();

                DataTableForGridLayout = new DataTable();
                //DataTableForGridLayout.Columns.Add("SalesOwner", typeof(string));
                // DataTableForGridLayout.Columns.Add("Plant", typeof(string));
                DataTableForGridLayout.Columns.Add("BusinessUnit", typeof(string));
                // DataTableForGridLayout.Columns.Add("Region", typeof(string));
                DataTableForGridLayout.Columns.Add("CustomerSite", typeof(string));
                DataTableForGridLayout.Columns.Add("Group", typeof(string));
                DataTableForGridLayout.Columns.Add("Code", typeof(string));
                DataTableForGridLayout.Columns.Add("CalendarWeek", typeof(string));
                DataTableForGridLayout.Columns.Add("Site", typeof(string));
                DataTableForGridLayout.Columns.Add("OfferDeliveryDateYear", typeof(string));
                DataTableForGridLayout.Columns.Add("SalesOwner", typeof(string));
                DataTableForGridLayout.Columns.Add("GroupColor", typeof(bool));

                //Bands.Add(band0);


                //BandItem band1 = new BandItem() { BandName = "", BandHeader = "Cust.", Visible = true };
                //band1.Columns = new ObservableCollection<ColumnItem>();
                //CrmStartUp = new CrmServiceController("localhost:6699");
                OfferOptions = CrmStartUp.GetAllOfferOptions();
                for (int i = 0; i < OfferOptions.Count; i++)
                {
                    if (!DataTableForGridLayout.Columns.Contains(OfferOptions[i].Name))
                    {

                        Columns.Add(new Emdep.Geos.UI.Helper.Column() { FieldName = OfferOptions[i].Name.ToString(), HeaderText = OfferOptions[i].Name.ToString(), Settings = SettingsType.ArrayOfferOption, AllowCellMerge = false, Width = 45, AllowEditing = false, Visible = false, IsVertical = true, FixedWidth = true });
                        // Columns.Add(new Emdep.Geos.UI.Helper.Column() { FieldName = "OfferOption_" + OfferOptions[i].Name.ToString(), HeaderText = OfferOptions[i].Name.ToString(), RealTimeMonitorSetting = SettingsType.Array, AllowCellMerge = false, Width = 45, AllowEditing = false, Visible = false, IsVertical = true, FixedWidth = true });
                        //  band0.Columns.Add(new ColumnItem() { ColumnFieldName = "OfferOption_" + OfferOptions[i].Name.ToString(), HeaderText = OfferOptions[i].Name.ToString(), Width = 45, IsVertical = true, RealTimeMonitorSetting = RealTimeMonitorColumnTemplateSelector.RealTimeMonitorSettingType.Array, Visible = true });
                        //TotalSummary.Add(new Summary() { Type = SummaryItemType.Sum, FieldName = OfferOptions[i].Name.ToString(), DisplayFormat = " {0}" });
                        DataTableForGridLayout.Columns.Add(OfferOptions[i].Name.ToString(), typeof(string));

                        GroupSummary.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Sum, FieldName = OfferOptions[i].Name, DisplayFormat = "{0}" });
                        TotalSummary.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Sum, FieldName = OfferOptions[i].Name, DisplayFormat = "{0}" });


                    }
                }

                //DataTableForGridLayout.Columns.Add("CustomerETD", typeof(string));
                //DataTableForGridLayout.Columns.Add("ProductionPlan", typeof(string));
                //DataTableForGridLayout.Columns.Add("Vision", typeof(string));
                //DataTableForGridLayout.Columns.Add("Tightening", typeof(string));
                //DataTableForGridLayout.Columns.Add("ProductionO", typeof(string));
                //DataTableForGridLayout.Columns.Add("RFQ", typeof(string));
                //DataTableForGridLayout.Columns.Add("SpecialEquipments", typeof(string));
                //DataTableForGridLayout.Columns.Add("Forecast", typeof(string));

                // Bands = new ObservableCollection<BandItem>(Bands);
                //  TotalSummary.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Count, FieldName = "CalendarWeek", DisplayFormat = "Total" });
                TotalSummary.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Count, FieldName = "OfferDeliveryDateYear", DisplayFormat = "Total" });

                GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTableWithBands executed Successfully", category: Category.Info, priority: Priority.Low);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in AddColumnsToDataTableWithBands() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillRTMFutureLoadList()
        {

            try
            {
                GeosApplication.Instance.Logger.Log("Method FillRTMFutureLoadList ...", category: Category.Info, priority: Priority.Low);

                if (GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
                {
                    // List<Company> plantOwners = GeosApplication.Instance.SelectedPlantOwnerUsersList.Cast<Company>().ToList();
                    List<Site> plantOwners = ERMCommon.Instance.SelectedAuthorizedPlantsList.Cast<Site>().ToList();
                    //var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.ShortName));
                    //PreviouslySelectedPlantOwners = string.Join(",", plantOwners.Select(i => i.ConnectPlantId));

                    RTMFutureLoadList = new List<RTMFutureLoad>();

                    foreach (var itemPlantOwnerUsers in plantOwners)
                    {
                        try
                        {
                            GeosApplication.Instance.SplashScreenMessage = "Connecting to " + itemPlantOwnerUsers.Name;

                            string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.Name == itemPlantOwnerUsers.Name).Select(xy => xy.ServiceProviderUrl).FirstOrDefault();
                            ERMService = new ERMServiceController(serviceurl);

                            //   Company itemPlantOwnerUsers = GeosApplication.Instance.SelectedPlantOwnerUsersList.Cast<Company>().FirstOrDefault();
                            List<RTMFutureLoad> FutureLoadList = new List<RTMFutureLoad>();
                            RTMFutureLoadParams objRTMFutureLoadParams = new RTMFutureLoadParams();

                            objRTMFutureLoadParams.idCurrency = GeosApplication.Instance.IdCurrencyByRegion;
                            objRTMFutureLoadParams.idUser = GeosApplication.Instance.ActiveUser.IdUser;
                            objRTMFutureLoadParams.idsSelectedUser = GeosApplication.Instance.ActiveUser.IdUser.ToString();
                            objRTMFutureLoadParams.idZone = GeosApplication.Instance.ActiveUser.Company.Country.IdZone;

                            // objRTMFutureLoadParams.activeSite = new ActiveSite { IdSite = Convert.ToInt32(itemPlantOwnerUsers.ConnectPlantId), SiteAlias = Convert.ToString(itemPlantOwnerUsers.Alias), SiteServiceProvider = itemPlantOwnerUsers.ServiceProviderUrl };
                            objRTMFutureLoadParams.activeSite = new ActiveSite { IdSite = Convert.ToInt32(itemPlantOwnerUsers.IdSite), SiteAlias = Convert.ToString(itemPlantOwnerUsers.Name), SiteServiceProvider = "" };
                            //  GeosApplication.Instance.SelectedyearStarDate = Convert.ToDateTime("01/01/2021");
                            //objRTMFutureLoadParams.accountingYearFrom = GeosApplication.Instance.SelectedyearStarDate;
                            //objRTMFutureLoadParams.accountingYearTo = Convert.ToDateTime(DateTime.MaxValue.ToString("yyyy-MM-dd"));

                            objRTMFutureLoadParams.accountingYearFrom = HRResourceStartDate;
                            objRTMFutureLoadParams.accountingYearTo = HRResourceEndDate;

                            objRTMFutureLoadParams.Roles = RoleType.SalesGlobalManager;

                            //ERMService = new ERMServiceController("localhost:6699");

                            //  FutureLoadList = ERMService.GetRTMFutureLoadDetails_V2430(objRTMFutureLoadParams).ToList();
                           // FutureLoadList = ERMService.GetRTMFutureLoadDetails_V2440(objRTMFutureLoadParams).ToList();
                            FutureLoadList = ERMService.GetRTMFutureLoadDetails_V2540(objRTMFutureLoadParams).ToList(); //[pallavi jadhav][GEOS2-5907][17 07 2024]


                            RTMFutureLoadList.AddRange(FutureLoadList);
                        }
                        catch (FaultException<ServiceException> ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed");
                            if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                FailedPlants.Add(itemPlantOwnerUsers.Name);
                            if (FailedPlants != null && FailedPlants.Count > 0)
                            {
                                IsShowFailedPlantWarning = true;
                                WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                                WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                            }
                            System.Threading.Thread.Sleep(1000);
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed ", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                        }
                        catch (ServiceUnexceptedException ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed");
                            if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                FailedPlants.Add(itemPlantOwnerUsers.Name);
                            if (FailedPlants != null && FailedPlants.Count > 0)
                            {
                                IsShowFailedPlantWarning = true;
                                WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                                WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                            }
                            System.Threading.Thread.Sleep(1000);
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed");
                            if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                FailedPlants.Add(itemPlantOwnerUsers.Name);

                            if (FailedPlants != null && FailedPlants.Count > 0)
                            {
                                IsShowFailedPlantWarning = true;
                                WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                                WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                            }

                            System.Threading.Thread.Sleep(1000);
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }

                    }

                    #region [GEOS2-4862][Rupali Sarode][30-09-2023]
                    RTMFutureLoadList_Cloned = new List<RTMFutureLoad>();
                    RTMFutureLoadList_Cloned = RTMFutureLoadList.ToList();

                    #endregion [GEOS2-4862][Rupali Sarode][30-09-2023]


                    GeosApplication.Instance.SplashScreenMessage = string.Empty;

                    if (FailedPlants == null || FailedPlants.Count == 0)
                    {
                        IsShowFailedPlantWarning = false;
                        WarningFailedPlants = string.Empty;
                    }
                }

                GeosApplication.Instance.Logger.Log("Method FillRTMFutureLoadList executed Successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in FillRTMFutureLoadList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        private void FillDashboard()
        {

            try
            {
                GeosApplication.Instance.Logger.Log("Method FillDashboard ...", category: Category.Info, priority: Priority.Low);
                List<RTMFutureLoad> RTMFutureLoadWeekwiseList = new List<RTMFutureLoad>();

                DataTableForGridLayout.Rows.Clear();

                DataTable DataTableForGridLayoutCopy = new DataTable();

                DataTableForGridLayoutCopy = DataTableForGridLayout.Copy();

                PlantWeekList = PlantWeekList.OrderBy(a => a.CalenderWeek).ToList();
                List<string> YearOfDate;
                string[] Seperator = { "CW" };

                //    RTMFutureLoadList = RTMFutureLoadList.Where(i => i.Offer == "OP22RO00699").ToList();
                DateTime currentDate = DateTime.Now;
                int currentWeek = (int)(currentDate.DayOfYear / 7) + 1;
                int year = DateTime.Now.Year;
                string Week = year + "CW" + currentWeek;
                foreach (PlantOperationWeek weekitem in PlantWeekList)
                {


                    //dr["CalendarWeek"] = weekitem.CalenderWeek;

                    RTMFutureLoadWeekwiseList = RTMFutureLoadList.Where(i => i.DeliveryWeek == weekitem.CalenderWeek).ToList();

                    foreach (RTMFutureLoad RTMFutureLoadWeekwiseItem in RTMFutureLoadWeekwiseList)
                    {
                        DataRow dr = DataTableForGridLayoutCopy.NewRow();
                        if (RTMFutureLoadWeekwiseList == null || RTMFutureLoadWeekwiseList.Count == 0)
                            dr["CalendarWeek"] = "";
                        else
                        {
                            dr["CalendarWeek"] = weekitem.CalenderWeek;
                            YearOfDate = weekitem.CalenderWeek.Split(Seperator, StringSplitOptions.None).ToList();
                            dr["OfferDeliveryDateYear"] = YearOfDate[0].ToString();
                            dr["GroupColor"] = false;
                            if (weekitem.CalenderWeek == Week)
                            {
                                // string curWeek = rowsInCurrentWeek.Select(a => a.ToString()).FirstOrDefault();
                                //GroupColor = Week;
                                dr["GroupColor"] = true;
                            }
                        }
                        // dr["CalendarWeek"] = weekitem.CalenderWeek;
                        dr["Site"] = RTMFutureLoadWeekwiseItem.ActiveSite.SiteAlias;


                        #region RND
                        dr["Code"] = RTMFutureLoadWeekwiseItem.Code;
                        dr["Group"] = RTMFutureLoadWeekwiseItem.Group;
                        // dr["Region"] = RTMFutureLoadWeekwiseItem.Region;
                        dr["CustomerSite"] = RTMFutureLoadWeekwiseItem.CustomerSite;
                        dr["BusinessUnit"] = RTMFutureLoadWeekwiseItem.BusinessUnit;
                        if (RTMFutureLoadWeekwiseItem.SalesOwnerList != null)
                        {

                            string item = RTMFutureLoadWeekwiseItem.SalesOwnerList.FirstOrDefault(i => i.IdSite == RTMFutureLoadWeekwiseItem.IdSite).SalesOwner;
                            dr["SalesOwner"] = item;
                        }

                        //RTMFutureLoadWeekwiseItem.SalesOwnerList
                        #endregion
                        foreach (OptionsByOfferGrid item in RTMFutureLoadWeekwiseItem.OptionsByOffers)
                        {
                            if (item.OfferOption != null)
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
                                else if (DataTableForGridLayoutCopy.Columns.Contains(item.OfferOption.ToString()))
                                {
                                    if (DataTableForGridLayoutCopy.Columns[item.OfferOption.ToString()].ToString() == item.OfferOption.ToString())
                                    {
                                        var column = Columns.FirstOrDefault(c => c.FieldName.Trim().ToUpper() == item.OfferOption.ToString().Trim().ToUpper());
                                        int indexc = Columns.IndexOf(column);
                                        Columns[indexc].Visible = true;

                                        if (string.IsNullOrEmpty(Convert.ToString(dr[item.OfferOption]))) dr[item.OfferOption] = 0;

                                        dr[item.OfferOption] = Convert.ToInt32(dr[item.OfferOption]) + item.Quantity;
                                    }
                                }
                            }
                        }
                        //DataTableForGridLayoutCopy.Rows.Add(dr);
                        if (!string.IsNullOrEmpty(Convert.ToString(dr["CalendarWeek"])))
                        {

                            if (SelectedDeliveryCW == null) SelectedDeliveryCW = new List<object>();

                            if (SelectedDeliveryCW.Count > 0)
                            {
                                if (selectedDeliveryCW.Contains(Convert.ToString(dr["CalendarWeek"])))
                                {
                                    DataTableForGridLayoutCopy.Rows.Add(dr);
                                }
                            }
                            else
                            {
                                DataTableForGridLayoutCopy.Rows.Add(dr);
                            }

                        }
                    }

                    //if (!string.IsNullOrEmpty(Convert.ToString(dr["CalendarWeek"])))
                    //{
                    //    DataTableForGridLayoutCopy.Rows.Add(dr);
                    //}

                    //[GEOS2-4862][Aishwarya Ingale][06-10-2023]

                    //if (!string.IsNullOrEmpty(Convert.ToString(dr["CalendarWeek"])))
                    //{
                    //    if (IsSelectedWeekExistsInPlantWeekList == true)
                    //    {
                    //        if (SelectedDeliveryCW == null) SelectedDeliveryCW = new List<object>();

                    //        if (selectedDeliveryCW.Contains(Convert.ToString(dr["CalendarWeek"])))
                    //        {
                    //            DataTableForGridLayoutCopy.Rows.Add(dr);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        DataTableForGridLayoutCopy.Rows.Add(dr);
                    //    }
                    //}


                }

                DataTableForGridLayout = DataTableForGridLayoutCopy;
                SelectedObject = DataTableForGridLayout.DefaultView[(DataTableForGridLayout.Rows.Count - 1)];
                GeosApplication.Instance.Logger.Log("Method FillDashboard executed Successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in FillDashboard() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        #endregion [GEOS2-4351][Rupali Sarode][29-05-2023]


        #region HR Resources [GEOS2-4729][rupali sarode][04-08-2023]

        private void AddColumnsToDataTableWithBandsForHrResources()
        {
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTableWithBandsForHrResources ...", category: Category.Info, priority: Priority.Low);
                DtDashboard = new DataTable();//[GEOS2-4708][gulab lakade][25 07 2023]
                BandsDashboard = new ObservableCollection<BandItem>(); BandsDashboard.Clear();
                BandItem band0 = new BandItem() { BandName = "FirstRow", AllowBandMove = false, FixedStyle = FixedStyle.Left, Visible = true, AllowMoving = DevExpress.Utils.DefaultBoolean.False };
                band0.Columns = new ObservableCollection<ColumnItem>();

                band0.Columns.Add(new ColumnItem() { ColumnFieldName = "CalenderWeek", HeaderText = "Week", Width = 120, IsVertical = false, RealTimeMonitorHRResourcesSetting = RealTimeMonitorHRResourcesColumnTemplateSelector.RealTimeMonitorHRResourcesSettingType.CalenderWeek, Visible = true });

                BandsDashboard.Add(band0);
                DataTableForGridLayoutDashboard = new DataTable();

                DataTableForGridLayoutDashboard.Columns.Add("CalenderWeek", typeof(string));
                DataTableForGridLayoutDashboard.Columns.Add("GroupColor", typeof(bool));
                List<string> tempCurrentstage = new List<string>();


                #region //[pallavi jadhav][GEOS2-4869][9 27 2023]
                //var TempIdStageList = WorkStageWiseJobDescription.Where(x => x.IdJobDescription.Contains(Convert.ToString(item.IdJobDescription))).ToList();
                List<string> IdStages = WorkStageWiseJobDescription.Select(a => Convert.ToString(a.IdWorkStage)).ToList(); //[pallavi jadhav][GEOS2-6529][24 10 2024]
                ERMRTMHRResourcesStageList = ERMRTMHRResourcesStageList.Where(a => a.StageCode != "{null}").ToList();
                ERMRTMHRResourcesStageList = ERMRTMHRResourcesStageList.GroupBy(x => x.IdStage, (key, group) => group.First()).ToList();
                ERMRTMHRResourcesStageList = ERMRTMHRResourcesStageList.Where(a => IdStages.Contains(Convert.ToString(a.IdStage))).ToList(); //[pallavi jadhav][GEOS2-6529][24 10 2024]
                #endregion
                if (ERMRTMHRResourcesStageList.Count > 0) //[pallavi jadhav][GEOS2-4869][9 27 2023]
                {
                    // foreach (var item in PlantOperationProductionStage)
                    foreach (var item in ERMRTMHRResourcesStageList) //[pallavi jadhav][GEOS2-4869][9 27 2023]
                    {
                        if (Convert.ToString(item) != null)
                        {

                            tempCurrentstage.Add(Convert.ToString(item));
                            BandItem band1 = new BandItem()
                            {
                                BandName = Convert.ToString(item.StageCode),
                                BandHeader = Convert.ToString(item.StageCode),
                                Visible = true
                            };

                            band1.Columns = new ObservableCollection<ColumnItem>();


                            DataTableForGridLayoutDashboard.Columns.Add("HRExpected_" + Convert.ToString(item.IdStage), typeof(string));

                            band1.Columns.Add(new ColumnItem() { ColumnFieldName = "HRExpected_" + Convert.ToString(item.IdStage), HeaderText = "HRE", Width = 70, Visible = true, IsVertical = false, RealTimeMonitorHRResourcesSetting = RealTimeMonitorHRResourcesColumnTemplateSelector.RealTimeMonitorHRResourcesSettingType.HRExpected });
                            DataTableForGridLayoutDashboard.Columns.Add("HRPlan_" + Convert.ToString(item.IdStage), typeof(string));
                            band1.Columns.Add(new ColumnItem() { ColumnFieldName = "HRPlan_" + Convert.ToString(item.IdStage), HeaderText = "HRP", Width = 70, Visible = true, IsVertical = false, RealTimeMonitorHRResourcesSetting = RealTimeMonitorHRResourcesColumnTemplateSelector.RealTimeMonitorHRResourcesSettingType.HRPlan });

                            DataTableForGridLayoutDashboard.Columns.Add("ProductionExpectedTime_" + Convert.ToString(item.IdStage), typeof(string));
                            DataTableForGridLayoutDashboard.Columns.Add("Tempcolorflag_" + Convert.ToString(item.IdStage), typeof(bool));
                            DataTableForGridLayoutDashboard.Columns.Add("Tempcolor_" + Convert.ToString(item.IdStage), typeof(string));

                            switch (item.IdStage)
                            {

                                case 1:

                                    band1.Columns.Add(new ColumnItem() { ColumnFieldName = "ProductionExpectedTime_" + Convert.ToString(item.IdStage), HeaderText = "PET", Width = 70, Visible = true, IsVertical = false, RealTimeMonitorHRResourcesSetting = RealTimeMonitorHRResourcesColumnTemplateSelector.RealTimeMonitorHRResourcesSettingType.ProductionExpectedTime1 });

                                    break;

                                case 2:
                                    band1.Columns.Add(new ColumnItem() { ColumnFieldName = "ProductionExpectedTime_" + Convert.ToString(item.IdStage), HeaderText = "PET", Width = 70, Visible = true, IsVertical = false, RealTimeMonitorHRResourcesSetting = RealTimeMonitorHRResourcesColumnTemplateSelector.RealTimeMonitorHRResourcesSettingType.ProductionExpectedTime2 });
                                    break;

                                case 3:

                                    band1.Columns.Add(new ColumnItem() { ColumnFieldName = "ProductionExpectedTime_" + Convert.ToString(item.IdStage), HeaderText = "PET", Width = 70, Visible = true, IsVertical = false, RealTimeMonitorHRResourcesSetting = RealTimeMonitorHRResourcesColumnTemplateSelector.RealTimeMonitorHRResourcesSettingType.ProductionExpectedTime3 });
                                    break;

                                case 4:

                                    band1.Columns.Add(new ColumnItem() { ColumnFieldName = "ProductionExpectedTime_" + Convert.ToString(item.IdStage), HeaderText = "PET", Width = 70, Visible = true, IsVertical = false, RealTimeMonitorHRResourcesSetting = RealTimeMonitorHRResourcesColumnTemplateSelector.RealTimeMonitorHRResourcesSettingType.ProductionExpectedTime4 });
                                    break;

                                case 5:

                                    band1.Columns.Add(new ColumnItem() { ColumnFieldName = "ProductionExpectedTime_" + Convert.ToString(item.IdStage), HeaderText = "PET", Width = 70, Visible = true, IsVertical = false, RealTimeMonitorHRResourcesSetting = RealTimeMonitorHRResourcesColumnTemplateSelector.RealTimeMonitorHRResourcesSettingType.ProductionExpectedTime5 });
                                    break;

                                case 6:

                                    band1.Columns.Add(new ColumnItem() { ColumnFieldName = "ProductionExpectedTime_" + Convert.ToString(item.IdStage), HeaderText = "PET", Width = 70, Visible = true, IsVertical = false, RealTimeMonitorHRResourcesSetting = RealTimeMonitorHRResourcesColumnTemplateSelector.RealTimeMonitorHRResourcesSettingType.ProductionExpectedTime6 });
                                    break;

                                case 7:

                                    band1.Columns.Add(new ColumnItem() { ColumnFieldName = "ProductionExpectedTime_" + Convert.ToString(item.IdStage), HeaderText = "PET", Width = 70, Visible = true, IsVertical = false, RealTimeMonitorHRResourcesSetting = RealTimeMonitorHRResourcesColumnTemplateSelector.RealTimeMonitorHRResourcesSettingType.ProductionExpectedTime7 });
                                    break;

                                case 8:
                                    band1.Columns.Add(new ColumnItem() { ColumnFieldName = "ProductionExpectedTime_" + Convert.ToString(item.IdStage), HeaderText = "PET", Width = 70, Visible = true, IsVertical = false, RealTimeMonitorHRResourcesSetting = RealTimeMonitorHRResourcesColumnTemplateSelector.RealTimeMonitorHRResourcesSettingType.ProductionExpectedTime8 });
                                    break;

                                case 9:

                                    band1.Columns.Add(new ColumnItem() { ColumnFieldName = "ProductionExpectedTime_" + Convert.ToString(item.IdStage), HeaderText = "PET", Width = 70, Visible = true, IsVertical = false, RealTimeMonitorHRResourcesSetting = RealTimeMonitorHRResourcesColumnTemplateSelector.RealTimeMonitorHRResourcesSettingType.ProductionExpectedTime9 });
                                    break;

                                case 10:
                                    band1.Columns.Add(new ColumnItem() { ColumnFieldName = "ProductionExpectedTime_" + Convert.ToString(item.IdStage), HeaderText = "PET", Width = 70, Visible = true, IsVertical = false, RealTimeMonitorHRResourcesSetting = RealTimeMonitorHRResourcesColumnTemplateSelector.RealTimeMonitorHRResourcesSettingType.ProductionExpectedTime10 });
                                    break;

                                case 11:
                                    band1.Columns.Add(new ColumnItem() { ColumnFieldName = "ProductionExpectedTime_" + Convert.ToString(item.IdStage), HeaderText = "PET", Width = 70, Visible = true, IsVertical = false, RealTimeMonitorHRResourcesSetting = RealTimeMonitorHRResourcesColumnTemplateSelector.RealTimeMonitorHRResourcesSettingType.ProductionExpectedTime11 });
                                    break;

                                case 12:
                                    band1.Columns.Add(new ColumnItem() { ColumnFieldName = "ProductionExpectedTime_" + Convert.ToString(item.IdStage), HeaderText = "PET", Width = 70, Visible = true, IsVertical = false, RealTimeMonitorHRResourcesSetting = RealTimeMonitorHRResourcesColumnTemplateSelector.RealTimeMonitorHRResourcesSettingType.ProductionExpectedTime12 });
                                    break;

                                case 21:
                                    band1.Columns.Add(new ColumnItem() { ColumnFieldName = "ProductionExpectedTime_" + Convert.ToString(item.IdStage), HeaderText = "PET", Width = 70, Visible = true, IsVertical = false, RealTimeMonitorHRResourcesSetting = RealTimeMonitorHRResourcesColumnTemplateSelector.RealTimeMonitorHRResourcesSettingType.ProductionExpectedTime21 });
                                    break;

                                case 26:
                                    band1.Columns.Add(new ColumnItem() { ColumnFieldName = "ProductionExpectedTime_" + Convert.ToString(item.IdStage), HeaderText = "PET", Width = 70, Visible = true, IsVertical = false, RealTimeMonitorHRResourcesSetting = RealTimeMonitorHRResourcesColumnTemplateSelector.RealTimeMonitorHRResourcesSettingType.ProductionExpectedTime26 });
                                    break;

                                case 27:
                                    band1.Columns.Add(new ColumnItem() { ColumnFieldName = "ProductionExpectedTime_" + Convert.ToString(item.IdStage), HeaderText = "PET", Width = 70, Visible = true, IsVertical = false, RealTimeMonitorHRResourcesSetting = RealTimeMonitorHRResourcesColumnTemplateSelector.RealTimeMonitorHRResourcesSettingType.ProductionExpectedTime27 });
                                    break;
                                case 28:
                                    band1.Columns.Add(new ColumnItem() { ColumnFieldName = "ProductionExpectedTime_" + Convert.ToString(item.IdStage), HeaderText = "PET", Width = 70, Visible = true, IsVertical = false, RealTimeMonitorHRResourcesSetting = RealTimeMonitorHRResourcesColumnTemplateSelector.RealTimeMonitorHRResourcesSettingType.ProductionExpectedTime28 });
                                    break;

                                case 29:
                                    band1.Columns.Add(new ColumnItem() { ColumnFieldName = "ProductionExpectedTime_" + Convert.ToString(item.IdStage), HeaderText = "PET", Width = 70, Visible = true, IsVertical = false, RealTimeMonitorHRResourcesSetting = RealTimeMonitorHRResourcesColumnTemplateSelector.RealTimeMonitorHRResourcesSettingType.ProductionExpectedTime29 });
                                    break;

                                case 32:
                                    band1.Columns.Add(new ColumnItem() { ColumnFieldName = "ProductionExpectedTime_" + Convert.ToString(item.IdStage), HeaderText = "PET", Width = 70, Visible = true, IsVertical = false, RealTimeMonitorHRResourcesSetting = RealTimeMonitorHRResourcesColumnTemplateSelector.RealTimeMonitorHRResourcesSettingType.ProductionExpectedTime32 });
                                    break;
                                case 33:
                                    band1.Columns.Add(new ColumnItem() { ColumnFieldName = "ProductionExpectedTime_" + Convert.ToString(item.IdStage), HeaderText = "PET", Width = 70, Visible = true, IsVertical = false, RealTimeMonitorHRResourcesSetting = RealTimeMonitorHRResourcesColumnTemplateSelector.RealTimeMonitorHRResourcesSettingType.ProductionExpectedTime33 });
                                    break;
                                case 35:
                                    band1.Columns.Add(new ColumnItem() { ColumnFieldName = "ProductionExpectedTime_" + Convert.ToString(item.IdStage), HeaderText = "PET", Width = 70, Visible = true, IsVertical = false, RealTimeMonitorHRResourcesSetting = RealTimeMonitorHRResourcesColumnTemplateSelector.RealTimeMonitorHRResourcesSettingType.ProductionExpectedTime35 });
                                    break;

                                case 37:
                                    band1.Columns.Add(new ColumnItem() { ColumnFieldName = "ProductionExpectedTime_" + Convert.ToString(item.IdStage), HeaderText = "PET", Width = 70, Visible = true, IsVertical = false, RealTimeMonitorHRResourcesSetting = RealTimeMonitorHRResourcesColumnTemplateSelector.RealTimeMonitorHRResourcesSettingType.ProductionExpectedTime37 });
                                    break;

                                default:


                                    band1.Columns.Add(new ColumnItem() { ColumnFieldName = "ProductionExpectedTime_" + Convert.ToString(item.IdStage), HeaderText = "PET", Width = 70, Visible = true, IsVertical = false, RealTimeMonitorHRResourcesSetting = RealTimeMonitorHRResourcesColumnTemplateSelector.RealTimeMonitorHRResourcesSettingType.ProductionExpectedTime });

                                    break;

                            }

                            band1.Columns.Add(new ColumnItem() { ColumnFieldName = "IdStage_", HeaderText = "IdStage", Width = 0, IsVertical = false, RealTimeMonitorHRResourcesSetting = RealTimeMonitorHRResourcesColumnTemplateSelector.RealTimeMonitorHRResourcesSettingType.Hidden, Visible = false });
                            if (!DataTableForGridLayoutDashboard.Columns.Contains("IdStage_"))
                            {
                                DataTableForGridLayoutDashboard.Columns.Add("IdStage_" + Convert.ToString(item.IdStage), typeof(string));
                            }


                            BandsDashboard.Add(band1);
                            #region [GEOS2-4549][gulablakade][08 06 2023]


                            #endregion

                        }
                    }
                }

                BandsDashboard = new ObservableCollection<BandItem>(BandsDashboard);

                GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTableWithBandsForHrResources executed Successfully", category: Category.Info, priority: Priority.Low);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in AddColumnsToDataTableWithBandsForHrResources() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in AddColumnsToDataTableWithBandsForHrResources() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in AddColumnsToDataTableWithBandsForHrResources() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void GetIdStageAndJobDescriptionByAppSetting()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GetIdStageAndJobDescriptionByAppSetting ...", category: Category.Info, priority: Priority.Low);

                Idstages = string.Empty;
                jobDescriptionID = string.Empty;
                WorkStages = new List<GeosAppSetting>();
                WorkStages = WorkbenchStartUp.GetSelectedGeosAppSettings("98");
                if (workStages.Count > 0)
                {
                    List<string> tempWorkStageList = new List<string>();
                    foreach (var item in workStages)
                    {
                        string tempstring = Convert.ToString(item.DefaultValue.Replace('(', ' '));
                        tempWorkStageList = Convert.ToString(tempstring).Split(')').ToList();
                    }
                    if (tempWorkStageList.Count > 0)
                    {
                        if (WorkStageWiseJobDescription != null)
                        {
                            WorkStageWiseJobDescription = new List<ERMWorkStageWiseJobDescription>();
                        }
                        List<string> TempJobDescriptionID = new List<string>();
                        foreach (var item in tempWorkStageList)
                        {
                            List<string> tempIDStageList = Convert.ToString(item.Trim()).Split(';').ToList();
                            if (tempIDStageList.Count > 0)
                            {
                                if (!string.IsNullOrEmpty(tempIDStageList[0]) && !string.IsNullOrEmpty(tempIDStageList[1]))
                                {
                                    ERMWorkStageWiseJobDescription IDStage = new ERMWorkStageWiseJobDescription();
                                    string tempstring = Convert.ToString(tempIDStageList[0].Replace(',', ' '));
                                    IDStage.IdWorkStage = Convert.ToInt32(tempstring.Trim());
                                    IDStage.IdJobDescription = new List<string>();
                                    TempJobDescriptionID.AddRange(Convert.ToString(tempIDStageList[1].Trim()).Split(',').ToList());
                                    IDStage.IdJobDescription = Convert.ToString(tempIDStageList[1].Trim()).Split(',').ToList();
                                    WorkStageWiseJobDescription.Add(IDStage);
                                    if (string.IsNullOrEmpty(Idstages))
                                    {
                                        Idstages = Convert.ToString(tempstring.Trim());
                                    }
                                    else
                                    {
                                        Idstages = Idstages + "," + Convert.ToString(tempstring.Trim());
                                    }
                                }
                            }

                        }
                        if (TempJobDescriptionID.Count > 0)
                        {
                            var Temp = TempJobDescriptionID.Distinct().ToList();
                            if (Temp.Count > 0)
                            {

                                foreach (var Tempitem in Temp)
                                {
                                    if (string.IsNullOrEmpty(jobDescriptionID))
                                    {
                                        jobDescriptionID = Convert.ToString(Tempitem);
                                    }
                                    else
                                    {
                                        jobDescriptionID = jobDescriptionID + "," + Convert.ToString(Tempitem);
                                    }
                                }

                            }
                        }

                    }
                }
                GeosApplication.Instance.Logger.Log("Method GetIdStageAndJobDescriptionByAppSetting() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetIdStageAndJobDescriptionByAppSetting() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void GetWeekList()
        {
            try
            {

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                GeosApplication.Instance.Logger.Log("Method GetWeekList ...", category: Category.Info, priority: Priority.Low);

                #region

                //CultureInfo CultureEnglish = new CultureInfo("en-GB");

                var culture = System.Threading.Thread.CurrentThread.CurrentCulture;
                CultureInfo CultureEnglish = new CultureInfo(culture.Name);
                DateTime TodaysDate;
                TodaysDate = DateTime.Now.Date;
                // TodaysDate = Convert.ToDateTime("01/01/2023");
                HRResourceStartDate = TodaysDate;

                DateTime TempFromDate = DateTime.Parse(Convert.ToString(TodaysDate), CultureEnglish, DateTimeStyles.AdjustToUniversal);

                int MinweekNum = CultureEnglish.Calendar.GetWeekOfYear(Convert.ToDateTime(TempFromDate).Date, CalendarWeekRule.FirstFourDayWeek, culture.DateTimeFormat.FirstDayOfWeek);

                var diff = Convert.ToDateTime(TempFromDate).Date.DayOfWeek - culture.DateTimeFormat.FirstDayOfWeek;

                if (diff < 0)
                {
                    diff += 7;
                }

                DateTime FirstDateOfWeek = Convert.ToDateTime(TempFromDate).Date.AddDays(-diff).Date;

                DateTime LastDateOfWeek = FirstDateOfWeek.AddDays(6);
                PlantWeekList = new List<PlantOperationWeek>();
                //DateTime HRResourceStartDate = DateTime.Parse(ToDate, CultureEnglish, DateTimeStyles.AdjustToUniversal); 


                //while (FirstDateOfWeek.Date < EndDate.Date)


                while (PlantWeekList.Count < 12)
                {
                    plantWeek = new PlantOperationWeek();
                    int Year = Convert.ToInt32(FirstDateOfWeek.Year);
                    int weekNum = CultureEnglish.Calendar.GetWeekOfYear(FirstDateOfWeek, CalendarWeekRule.FirstFourDayWeek, culture.DateTimeFormat.FirstDayOfWeek);
                    string CalendarWeek = Year + "CW" + weekNum.ToString("00");
                    DateTime LastDate = FirstDateOfWeek.AddDays(6);
                    plantWeek.CalenderWeek = CalendarWeek;
                    plantWeek.FirstDateofweek = FirstDateOfWeek;
                    plantWeek.LastDateofWeek = LastDate;
                    PlantWeekList.Add(plantWeek);
                    FirstDateOfWeek = LastDate.AddDays(1);
                }

                //#region testing
                //while (PlantWeekList.Count < 52)
                //{
                //    plantWeek = new PlantOperationWeek();
                //    int Year = Convert.ToInt32(FirstDateOfWeek.Year);
                //    int weekNum = CultureEnglish.Calendar.GetWeekOfYear(FirstDateOfWeek, CalendarWeekRule.FirstFourDayWeek, culture.DateTimeFormat.FirstDayOfWeek);
                //    string CalendarWeek = Year + "CW" + weekNum.ToString("00");
                //    DateTime LastDate = FirstDateOfWeek.AddDays(6);
                //    plantWeek.CalenderWeek = CalendarWeek;
                //    plantWeek.FirstDateofweek = FirstDateOfWeek;
                //    plantWeek.LastDateofWeek = LastDate;
                //    PlantWeekList.Add(plantWeek);
                //    FirstDateOfWeek = LastDate.AddDays(1);
                //}
                //#endregion

                HRResourceEndDate = PlantWeekList[PlantWeekList.Count - 1].LastDateofWeek;

                #endregion
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method GetWeekList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {

            }
        }

        private void FillRTMData()
        {
            try
            {

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                GeosApplication.Instance.Logger.Log("Method FillRTMData ...", category: Category.Info, priority: Priority.Low);

                PlantOperationalPlanning = new List<ERMPlantOperationalPlanning>();
                RTMHRResourcesExpectedTimeList = new List<RTMHRResourcesExpectedTime>();
                if (GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
                {
                    List<Site> plantOwners = ERMCommon.Instance.SelectedAuthorizedPlantsList.Cast<Site>().ToList();
                    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.Name));
                    ERMCommon.Instance.FailedPlants = new List<string>();
                    ERMCommon.Instance.IsShowFailedPlantWarning = false;
                    ERMCommon.Instance.WarningFailedPlants = string.Empty;
                    foreach (var itemPlantOwnerUsers in plantOwners)
                    {

                        // DateTime tempFromyear = DateTime.Parse(FromDate.ToString());
                        // string year = Convert.ToString(tempFromyear.Year);
                        string PlantName = Convert.ToString(itemPlantOwnerUsers.Name);
                        try
                        {
                            string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.Name == itemPlantOwnerUsers.Name).Select(xy => xy.ServiceProviderUrl).FirstOrDefault();
                            ERMService = new ERMServiceController(serviceurl);
                            //ERMService = new ERMServiceController("localhost:6699");
                            FillRTMHRResourcesStage(Convert.ToInt32(itemPlantOwnerUsers.IdSite)); //[pallavi jadhav][GEOS2-4869][9 27 2023]
                            //PlantOperationalPlanning.AddRange(ERMService.GetAllRTM_HRResources_V2420(Convert.ToInt32(itemPlantOwnerUsers.IdSite),
                            //HRResourceStartDate,
                            //HRResourceEndDate, jobDescriptionID));

                            //[GEOS2-5001][rupali sarode][28-10-2023]

                            //PlantOperationalPlanning.AddRange(ERMService.GetAllRTM_HRResources_V2450(Convert.ToInt32(itemPlantOwnerUsers.IdSite),
                            //HRResourceStartDate,
                            //HRResourceEndDate, jobDescriptionID));

                            //[GEOS2-5546][pallavi jadhav][29 03 2024]
                          //  PlantOperationalPlanning.AddRange(ERMService.GetAllRTM_HRResources_V2500(Convert.ToInt32(itemPlantOwnerUsers.IdSite),
                          //HRResourceStartDate,
                          //HRResourceEndDate, jobDescriptionID));
                            PlantOperationalPlanning.AddRange(ERMService.GetAllRTM_HRResources_V2540(Convert.ToInt32(itemPlantOwnerUsers.IdSite),
                          HRResourceStartDate,
                          HRResourceEndDate, jobDescriptionID)); //[pallavi jadhav][GEOS2-5907][17 07 2024]


                            //[GEOS2-4862][Rupali Sarode][04-10-2023]
                            //   RTMHRResourcesExpectedTimeList.AddRange(ERMService.GetRTM_HRResourcesExpectedTime_V2420(HRResourceStartDate, HRResourceEndDate));
                            // RTMHRResourcesExpectedTimeList.AddRange(ERMService.GetRTM_HRResourcesExpectedTime_V2440(HRResourceStartDate, HRResourceEndDate));
                            RTMHRResourcesExpectedTimeList.AddRange(ERMService.GetRTM_HRResourcesExpectedTime_V2540(HRResourceStartDate, HRResourceEndDate)); //[pallavi jadhav][GEOS2-5907][17 07 2024]



                            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        }


                        catch (FaultException<ServiceException> ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", PlantName, " Failed");
                            if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                if (!ERMCommon.Instance.FailedPlants.Contains(PlantName))
                                {
                                    ERMCommon.Instance.FailedPlants.Add(PlantName);
                                    if (ERMCommon.Instance.FailedPlants != null && ERMCommon.Instance.FailedPlants.Count > 0)
                                    {
                                        ERMCommon.Instance.IsShowFailedPlantWarning = true;
                                        ERMCommon.Instance.WarningFailedPlants = string.Join(",", ERMCommon.Instance.FailedPlants.Select(x => x.ToString()).ToArray());
                                        ERMCommon.Instance.WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), ERMCommon.Instance.WarningFailedPlants.ToString());
                                    }
                                }
                            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", PlantName, " Failed ", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                        }
                        catch (ServiceUnexceptedException ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", PlantName, " Failed");
                            if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                if (!ERMCommon.Instance.FailedPlants.Contains(PlantName))
                                {
                                    ERMCommon.Instance.FailedPlants.Add(PlantName);
                                    if (ERMCommon.Instance.FailedPlants != null && ERMCommon.Instance.FailedPlants.Count > 0)
                                    {
                                        ERMCommon.Instance.IsShowFailedPlantWarning = true;
                                        ERMCommon.Instance.WarningFailedPlants = string.Join(",", ERMCommon.Instance.FailedPlants.Select(x => x.ToString()).ToArray());
                                        ERMCommon.Instance.WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), ERMCommon.Instance.WarningFailedPlants.ToString());
                                    }
                                }
                            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", PlantName, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", PlantName, " Failed");
                            if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                if (!ERMCommon.Instance.FailedPlants.Contains(PlantName))
                                {
                                    ERMCommon.Instance.FailedPlants.Add(PlantName);
                                    if (ERMCommon.Instance.FailedPlants != null && ERMCommon.Instance.FailedPlants.Count > 0)
                                    {
                                        ERMCommon.Instance.IsShowFailedPlantWarning = true;
                                        ERMCommon.Instance.WarningFailedPlants = string.Join(",", ERMCommon.Instance.FailedPlants.Select(x => x.ToString()).ToArray());
                                        ERMCommon.Instance.WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), ERMCommon.Instance.WarningFailedPlants.ToString());
                                    }
                                }
                            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", PlantName, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }
                    }

                    FillEmployeeData();

                    #region [GEOS2-4862][Rupali Sarode][30-09-2023]
                    RTMHRResourcesExpectedTimeList_Cloned = new List<RTMHRResourcesExpectedTime>();
                    RTMHRResourcesExpectedTimeList_Cloned = RTMHRResourcesExpectedTimeList.ToList();

                    #endregion [GEOS2-4862][Rupali Sarode][30-09-2023]

                    GeosApplication.Instance.SplashScreenMessage = string.Empty;
                    if (ERMCommon.Instance.FailedPlants == null || ERMCommon.Instance.FailedPlants.Count == 0)
                    {
                        ERMCommon.Instance.IsShowFailedPlantWarning = false;
                        ERMCommon.Instance.WarningFailedPlants = string.Empty;
                    }
                }


                GeosApplication.Instance.Logger.Log("Method FillRTMData() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
        }

        #endregion HR Resources [GEOS2-4729][rupali sarode][04-08-2023]

        #region HR Resources [GEOS2-4730][rupali sarode][08-08-2023]
        private void FillDashboardHRResources()
        {
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                GeosApplication.Instance.Logger.Log("Method FillDashboardHRResources ...", category: Category.Info, priority: Priority.Low);
                //  int rowCounter = 0;

                List<string> tempCurrentstage = new List<string>();
                var currentculter = CultureInfo.CurrentCulture;
                string DecimalSeperator = currentculter.NumberFormat.NumberDecimalSeparator.ToString();
                DataTableForGridLayoutDashboard.Clear();
                // DataTableForGridLayout.Clear();
                DtPlantOperation = null;
                // ERMService = new ERMServiceController("localhost:6699");
                //List<RTMHRResourcesExpectedTime> RTMHRResourcesExpectedTimeList = new List<RTMHRResourcesExpectedTime>();
                //RTMHRResourcesExpectedTimeList = ERMService.GetRTM_HRResourcesExpectedTime_V2420(HRResourceStartDate, HRResourceEndDate);

                EmployeeplantOperationallist = EmployeeplantOperationallist.OrderBy(a => a.CalenderWeek).ToList();
                var TempCalenderWeek1 = EmployeeplantOperationallist.GroupBy(a => a.CalenderWeek).ToList();
                var TempCalenderWeek = TempCalenderWeek1.OrderBy(a => a.Key).ToList();
                Int64 TotalHRExpected = 0;
                float TotalReal = 0;
                DateTime currentDate = DateTime.Now;
                int currentWeek = (int)(currentDate.DayOfYear / 7) + 1;
                int year = DateTime.Now.Year;
                string Week = year + "CW" + currentWeek;

                foreach (var calendar in TempCalenderWeek)
                {
                    List<ERMEmployeePlantOperation> TempEmployeeplantOperationallist = new List<ERMEmployeePlantOperation>();
                    TempEmployeeplantOperationallist = EmployeeplantOperationallist.Where(a => a.CalenderWeek == Convert.ToString(calendar.Key)).ToList();

                    int count = 0;// = TempEmployeeplantOperationallist.Count();
                    count++;
                    DataRow dr = DataTableForGridLayoutDashboard.NewRow();

                    foreach (var Employeedata in TempEmployeeplantOperationallist.GroupBy(a => a.IdEmployee))
                    {
                        //count++;
                        //DataRow dr = DataTableForGridLayoutDashboard.NewRow();
                        // DataRow dr = DataTableForGridLayout.NewRow();
                        dr["CalenderWeek"] = Convert.ToString(calendar.Key);
                        dr["GroupColor"] = false;
                        if (calendar.Key == Week)
                        {
                            // string curWeek = rowsInCurrentWeek.Select(a => a.ToString()).FirstOrDefault();
                            //GroupColor = Week;
                            dr["GroupColor"] = true;
                        }

                        List<ERMEmployeePlantOperation> TempEmployeelist = new List<ERMEmployeePlantOperation>();
                        TempEmployeelist = TempEmployeeplantOperationallist.Where(a => a.IdEmployee == Employeedata.Key && a.CalenderWeek == calendar.Key).ToList();
                        // TempEmployeelist = TempEmployeeplantOperationallist.ToList();
                        if (TempEmployeelist.Count > 0)
                        {
                            List<TempIdStage> IdStageList = new List<TempIdStage>();

                            foreach (ERMEmployeePlantOperation item in TempEmployeelist)
                            {

                                var TempIdStageList = WorkStageWiseJobDescription.Where(x => x.IdJobDescription.Contains(Convert.ToString(item.IdJobDescription))).ToList();
                                if (TempIdStageList.Count > 0)
                                {
                                    foreach (var itemIdStage in TempIdStageList)
                                    {

                                        var IsExist_IdStage = ERMRTMHRResourcesStageList.Where(x => x.IdStage == itemIdStage.IdWorkStage).ToList();
                                        //   var IsExist_IdStage = PlantOperationProductionStage.Where(x => x.IdStage == itemIdStage.IdWorkStage).ToList();
                                        if (IsExist_IdStage.Count > 0)
                                        {
                                            TempIdStage tempIdStage = new TempIdStage();
                                            tempIdStage.IdStage = Convert.ToInt32(itemIdStage.IdWorkStage);
                                            tempIdStage.IdJobDescription = Convert.ToInt32(item.IdJobDescription);
                                            tempIdStage.JobDescriptionUse = Convert.ToDecimal(item.JobDescriptionUsage);
                                            IdStageList.Add(tempIdStage);
                                        }
                                    }

                                }

                            }
                            if (IdStageList.Count > 0)
                            {

                                var tempIdStage = IdStageList.GroupBy(a => a.IdStage).ToList();
                                if (tempIdStage.Count > 0)
                                {
                                    foreach (var item in tempIdStage)
                                    {
                                        List<TempIdStage> TempidStageList = new List<TempIdStage>();
                                        TempidStageList = IdStageList.Where(a => a.IdStage == item.Key).ToList();
                                        if (TempidStageList.Count > 0)
                                        {
                                            int TempHRExpectedValue = 0;
                                            int TempHRPlanValue = 0;
                                            string TempTimeType = string.Empty;
                                            float TempRealTime = 0;

                                            decimal TempJobDescriptionUsage = 0;
                                            foreach (var item1 in TempidStageList)
                                            {
                                                var tempvalue = TempEmployeeplantOperationallist.Where(a => a.IdEmployee == Employeedata.Key && a.CalenderWeek == calendar.Key && a.IdJobDescription == item1.IdJobDescription).FirstOrDefault();

                                                if (TempJobDescriptionUsage == 0)
                                                {
                                                    TempJobDescriptionUsage = Convert.ToDecimal(item1.JobDescriptionUse);
                                                    if (tempvalue != null)
                                                    {
                                                        TempHRExpectedValue = Convert.ToInt32(tempvalue.HRExpected);
                                                        TempHRPlanValue = Convert.ToInt32(tempvalue.HRPlan);
                                                        if (tempvalue.EmployeePlantOperationalRealTimeList.Count() > 0)
                                                        {
                                                            var tempReal = tempvalue.EmployeePlantOperationalRealTimeList.Where(x => x.CalenderWeek == calendar.Key && x.IdEmployee == Employeedata.Key && x.Idstage == item1.IdStage).FirstOrDefault();
                                                            if (tempReal != null)
                                                            {
                                                                TempRealTime = (float)(tempReal.TimeDifferenceInMinutes);
                                                            }
                                                        }

                                                    }

                                                }
                                                else
                                                {
                                                    TempJobDescriptionUsage = TempJobDescriptionUsage + Convert.ToDecimal(item1.JobDescriptionUse);
                                                    if (tempvalue != null)
                                                    {
                                                        TempHRExpectedValue = TempHRExpectedValue + Convert.ToInt32(tempvalue.HRExpected);
                                                        TempHRPlanValue = TempHRPlanValue + Convert.ToInt32(tempvalue.HRPlan);
                                                        if (tempvalue.EmployeePlantOperationalRealTimeList.Count() > 0)
                                                        {
                                                            var tempReal = tempvalue.EmployeePlantOperationalRealTimeList.Where(x => x.CalenderWeek == calendar.Key && x.IdEmployee == Employeedata.Key && x.Idstage == item1.IdStage).FirstOrDefault();
                                                            if (tempReal != null)
                                                            {
                                                                TempRealTime = TempRealTime + (float)(tempReal.TimeDifferenceInMinutes);
                                                            }
                                                        }
                                                    }
                                                }


                                            }

                                            //TotalHRExpected = TotalHRExpected + TempHRExpectedValue;
                                            //TotalReal = TotalReal + TempRealTime;

                                            string IdStage = "IdStage_" + Convert.ToString(item.Key);
                                            dr[IdStage] = Convert.ToString(item.Key);
                                            string HRExpected = "HRExpected_" + Convert.ToString(item.Key);
                                            if (string.IsNullOrEmpty(Convert.ToString(dr[HRExpected]))) dr[HRExpected] = 0;

                                            dr[HRExpected] = Convert.ToInt32(dr[HRExpected]) + Convert.ToInt32(TempHRExpectedValue.ToString("0"));

                                            string HRPlan = "HRPlan_" + Convert.ToString(item.Key);

                                            if (string.IsNullOrEmpty(Convert.ToString(dr[HRPlan]))) dr[HRPlan] = 0;
                                            dr[HRPlan] = Convert.ToInt32(dr[HRPlan]) + Math.Round(Convert.ToDouble(TempHRPlanValue), 0);


                                        }

                                    }
                                }
                            }

                        }
                    }

                    //Add Expected Time data
                    if (RTMHRResourcesExpectedTimeList != null)
                    {
                        foreach (RTMHRResourcesExpectedTime ExpectedTime in RTMHRResourcesExpectedTimeList.Where(i => i.DeliveryWeek == Convert.ToString(calendar.Key)).ToList())
                        {
                            if (ExpectedTime.RTMCurrentStageList != null)
                            {
                                for (int iItem = 0; iItem < ExpectedTime.RTMCurrentStageList.Count; iItem++)
                                {
                                    try
                                    {

                                        string expected = "ProductionExpectedTime_" + Convert.ToString(ExpectedTime.RTMCurrentStageList[iItem].IdStage);
                                        TimeSpan Tempexpected = TimeSpan.Parse("0");
                                        if (!string.IsNullOrEmpty(Convert.ToString(ExpectedTime.RTMCurrentStageList[iItem].Expected)) && Convert.ToString(ExpectedTime.RTMCurrentStageList[iItem].Expected) != "0")
                                        {
                                            //Tempexpected = ConvertfloattoTimespan(Convert.ToString(TimeTrackingList[i].TimeTrackingStage[iItem].Expected));
                                            Tempexpected = TimeSpan.FromMinutes(Convert.ToDouble(ExpectedTime.RTMCurrentStageList[iItem].Expected));
                                            if (DataTableForGridLayoutDashboard.Columns.Contains(expected))
                                            {
                                                if (string.IsNullOrEmpty(Convert.ToString(dr[expected])))
                                                    dr[expected] = TimeSpan.Parse("0");
                                                dr[expected] = TimeSpan.Parse(Convert.ToString(dr[expected])) + Tempexpected;
                                            }
                                        }
                                        else
                                        {
                                            Tempexpected = TimeSpan.FromMinutes(Convert.ToDouble(0.0));
                                            if (DataTableForGridLayoutDashboard.Columns.Contains(expected))
                                            {
                                                if (string.IsNullOrEmpty(Convert.ToString(dr[expected])))
                                                    dr[expected] = TimeSpan.Parse("0");
                                                dr[expected] = TimeSpan.Parse(Convert.ToString(dr[expected])) + Tempexpected;
                                            }
                                        }

                                        //TempTotalExpected += Tempexpected;// Convert.ToDouble(item1.Expected);


                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                }
                            }
                        }

                    }

                    //DataTableForGridLayout.Rows.Add(dr);

                    if (ERMRTMHRResourcesStageList.Count > 0)
                    {
                        foreach (var item in ERMRTMHRResourcesStageList)
                        {
                            string tempcolorFlag = "Tempcolorflag_" + Convert.ToString(item.IdStage);
                            string HRPlan = "HRPlan_" + Convert.ToString(item.IdStage);
                            string ProductionExpectedTime = "ProductionExpectedTime_" + Convert.ToString(item.IdStage);
                            string TempColor = "Tempcolor_" + Convert.ToString(item.IdStage);
                            string HRExpected = "HRExpected_" + Convert.ToString(item.IdStage);

                            Int64 ProductionExpectedTimeHours = 0;
                            Int64 HRPlanHours = 0;

                            if (string.IsNullOrEmpty(Convert.ToString(dr[ProductionExpectedTime])))
                                ProductionExpectedTimeHours = 0;
                            else
                                ProductionExpectedTimeHours = TimeSpan.Parse(Convert.ToString(dr[ProductionExpectedTime])).Hours;

                            if (ProductionExpectedTimeHours == 0)
                                dr[ProductionExpectedTime] = "";
                            else
                                dr[ProductionExpectedTime] = ProductionExpectedTimeHours;

                            if (string.IsNullOrEmpty(Convert.ToString(dr[HRPlan])))
                                HRPlanHours = 0;
                            else
                                HRPlanHours = Convert.ToInt64(dr[HRPlan]);

                            if (ProductionExpectedTimeHours > HRPlanHours)
                            {
                                if (DataTableForGridLayoutDashboard.Columns.Contains(tempcolorFlag))
                                {
                                    dr[tempcolorFlag] = true;
                                    dr[TempColor] = "Red";

                                }
                            }
                            else
                            {
                                if (DataTableForGridLayoutDashboard.Columns.Contains(tempcolorFlag))
                                {
                                    dr[tempcolorFlag] = false;
                                    dr[TempColor] = "";
                                }
                            }
                            if (!String.IsNullOrEmpty(Convert.ToString(dr[HRPlan])))
                            {
                                if (Convert.ToInt64(dr[HRPlan]) == 0)
                                {
                                    dr[HRPlan] = "";
                                }
                            }

                            if (!String.IsNullOrEmpty(Convert.ToString(dr[HRExpected])))
                            {
                                if (Convert.ToInt64(dr[HRExpected]) == 0)
                                {
                                    dr[HRExpected] = "";
                                }
                            }
                        }
                    }
                    //if (IsSelectedWeekExistsInPlantWeekList == true)
                    //{
                    //    if (SelectedDeliveryCW == null) SelectedDeliveryCW = new List<object>();

                    //    if (selectedDeliveryCW.Contains(Convert.ToString(calendar.Key)))
                    //    {
                    //        DataTableForGridLayoutDashboard.Rows.Add(dr);
                    //    }
                    //}
                    //else
                    //{
                    //    DataTableForGridLayoutDashboard.Rows.Add(dr);
                    //}

                    DataTableForGridLayoutDashboard.Rows.Add(dr);



                    #region [Aishwarya]
                    //if (SelectedDeliveryCW == null) SelectedDeliveryCW = new List<object>();
                    //if (SelectedCustomer == null) SelectedCustomer = new List<object>();
                    //if (SelectedCustomerPlant == null) SelectedCustomerPlant = new List<object>();
                    //if (SelectedWorkOrder == null) SelectedWorkOrder = new List<object>();
                    //if (SelectedProject == null) SelectedProject = new List<object>();


                    //List<string> SelectedCWFromFilter = RTMHRResourcesExpectedTimeList.Select(i => i.DeliveryWeek).Distinct().ToList();
                    //if (SelectedCWFromFilter == null) SelectedCWFromFilter = new List<string>();

                    //if (SelectedDeliveryCW.Count == DeliveryCWList.Count && SelectedCustomer.Count == CustomerList.Count && SelectedCustomerPlant.Count == CustomerPlantList.Count && SelectedWorkOrder.Count == WorkOrderList.Count && SelectedProject.Count == ProjectList.Count)
                    //{
                    //    DataTableForGridLayoutDashboard.Rows.Add(dr);
                    //}
                    //else
                    //{
                    //    if ((SelectedDeliveryCW.Count == 0 || SelectedDeliveryCW.Count == DeliveryCWList.Count) && 
                    //        (SelectedCustomer.Count == 0 || SelectedCustomer.Count == CustomerList.Count) &&
                    //        (SelectedCustomerPlant.Count == 0 || SelectedCustomerPlant.Count == CustomerPlantList.Count) &&
                    //        (SelectedWorkOrder.Count == 0  || SelectedWorkOrder.Count == WorkOrderList.Count) &&
                    //        (SelectedProject.Count == 0 || SelectedProject.Count == ProjectList.Count))
                    //    {
                    //        DataTableForGridLayoutDashboard.Rows.Add(dr);
                    //    }
                    //    else
                    //    {
                    //        if (SelectedDeliveryCW.Count > 0 && SelectedDeliveryCW.Count < DeliveryCWList.Count)
                    //        {
                    //            if (selectedDeliveryCW.Contains(Convert.ToString(calendar.Key)))
                    //            {
                    //                if ((SelectedCustomer.Count > 0 && SelectedCustomer.Count < CustomerList.Count) || 
                    //                    (SelectedCustomerPlant.Count > 0 && SelectedCustomerPlant.Count < CustomerPlantList.Count) || 
                    //                    (SelectedWorkOrder.Count > 0 && SelectedWorkOrder.Count < WorkOrderList.Count) || 
                    //                    (SelectedProject.Count > 0 && SelectedProject.Count < ProjectList.Count))
                    //                {
                    //                    if (SelectedCWFromFilter.Contains(Convert.ToString(calendar.Key)))
                    //                    {
                    //                        DataTableForGridLayoutDashboard.Rows.Add(dr);
                    //                    }
                    //                }
                    //                else
                    //                {
                    //                    DataTableForGridLayoutDashboard.Rows.Add(dr);
                    //                }
                    //            }

                    //        }
                    //        else
                    //        {
                    //            if ((SelectedCustomer.Count > 0 && SelectedCustomer.Count < CustomerList.Count) || 
                    //                (SelectedCustomerPlant.Count > 0 && SelectedCustomerPlant.Count < CustomerPlantList.Count) || 
                    //                (SelectedWorkOrder.Count > 0  && SelectedWorkOrder.Count < WorkOrderList.Count) || 
                    //                (SelectedProject.Count > 0 && SelectedProject.Count < ProjectList.Count))
                    //            {

                    //                if (SelectedCWFromFilter.Contains(Convert.ToString(calendar.Key)))
                    //                {
                    //                    DataTableForGridLayoutDashboard.Rows.Add(dr);
                    //                }
                    //            }
                    //            else
                    //            {
                    //                DataTableForGridLayoutDashboard.Rows.Add(dr);
                    //            }
                    //        }
                    //    }

                    //}
                    #endregion [Aishwarya]

                    // rowCounter += 1;   
                }
                DtPlantOperation = new DataTable();
                // DtPlantOperation = DataTableForGridLayout;
                DtPlantOperation = DataTableForGridLayoutDashboard;


                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method FillDashboardHRResources()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillDashboardHRResources() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillEmployeeData()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillEmployeeData()...", category: Category.Info, priority: Priority.Low);

                string TempEmployeeName = string.Empty;
                EmployeeplantOperationallist = new List<ERMEmployeePlantOperation>();
                EmployeeplantOperationalListForRealTime = new List<ERMEmployeePlantOperation>();
                ERMEmployeePlantOperation EmployeeplantOperationalForRealTime = new ERMEmployeePlantOperation();
                if (PlantOperationalPlanning != null)
                {
                    #region rajashri GEOS2-5859[18-06-2024]
                    //List<GeosAppSetting> CompaniesNotDeductBreak = new List<GeosAppSetting>();
                    //CompaniesNotDeductBreak = WorkbenchStartUp.GetSelectedGeosAppSettings("120");

                    //List<Int32> CNDB_PlantId = new List<Int32>();
                    //if (CompaniesNotDeductBreak != null)
                    //{
                    //    string TempCNDB_PlantId = CompaniesNotDeductBreak.Select(s => s.DefaultValue).FirstOrDefault();
                    //    if (!string.IsNullOrEmpty(TempCNDB_PlantId))
                    //    {
                    //        CNDB_PlantId = new List<Int32>();
                    //        CNDB_PlantId = TempCNDB_PlantId.Split(';').Select(int.Parse).ToList();
                    //    }

                    //}
                    #endregion
                    foreach (ERMPlantOperationalPlanning item in PlantOperationalPlanning)
                    {
                        if (PlantWeekList.Count > 0)
                        {
                            List<PlantOperationWeek> TempPlantWeekList = new List<PlantOperationWeek>();
                            TempPlantWeekList = PlantWeekList.OrderBy(a => a.CalenderWeek).ToList();
                            foreach (PlantOperationWeek weekitem in TempPlantWeekList)
                            {
                                //if (Convert.ToDateTime(item.JobDescriptionStartDate) <= weekitem.FirstDateofweek && Convert.ToDateTime(item.EndDate) >= weekitem.LastDateofWeek)


                                if ((Convert.ToDateTime(item.JobDescriptionStartDate) <= weekitem.LastDateofWeek) && (Convert.ToDateTime(item.EndDate) >= weekitem.FirstDateofweek) && (Convert.ToDateTime(item.JobDescriptionStartDate).Date != Convert.ToDateTime(item.EndDate).Date))
                                {
                                    int TotalWorkingDayCount = 0;       ////[gulab lakade][05 05 2023]
                                    EmployeeplantOperational = new ERMEmployeePlantOperation();
                                    EmployeeplantOperational.CalenderWeek = Convert.ToString(weekitem.CalenderWeek);
                                    EmployeeplantOperational.IdEmployee = Convert.ToInt32(item.IdEmployee);
                                    EmployeeplantOperational.IdCompany = Convert.ToInt32(item.IdCompany);
                                    EmployeeplantOperational.IdCompany = Convert.ToInt32(item.IdCompany);
                                    //if(item.IdEmployee==1935 && weekitem.CalenderWeek=="2023CW02")//only test
                                    //{

                                    //}
                                    #region JobDescription
                                    EmployeeplantOperational.IdJobDescription = Convert.ToInt32(item.IdJobDescription);
                                    EmployeeplantOperational.JobDescriptionUsage = Convert.ToDecimal(item.JobDescriptionUsage);
                                    if (item.JobDescriptionStartDate != null)
                                    {
                                        EmployeeplantOperational.JobDescriptionStartDate = Convert.ToDateTime(item.JobDescriptionStartDate);
                                    }
                                    if (item.EndDate != null)
                                    {
                                        EmployeeplantOperational.EndDate = Convert.ToDateTime(item.EndDate);
                                    }
                                    #endregion
                                    var Employee = item.EmployeeInformation.FirstOrDefault();
                                    if (Employee != null)
                                    {
                                        TempEmployeeName = Convert.ToString(Employee.EmployeeName);
                                        EmployeeplantOperational.EmployeeName = Convert.ToString(Employee.EmployeeName);

                                        //Find First day and last day of week
                                        //var culture = System.Threading.Thread.CurrentThread.CurrentCulture;
                                        //var diff = Convert.ToDateTime(item.JobDescriptionStartDate).DayOfWeek - culture.DateTimeFormat.FirstDayOfWeek;

                                        //if (diff < 0)
                                        //{
                                        //    diff += 7;
                                        //}

                                        DateTime FirstDateOfWeek = weekitem.FirstDateofweek;//DateTime.Now.AddDays(-diff).Date;

                                        DateTime LastDateOfWeek = weekitem.LastDateofWeek; //FirstDateOfWeek.AddDays(6);

                                        try
                                        {
                                            #region [gulab lakade][05 05 2023][date condtion]

                                            if (Employee.IdEmployee == 242 && item.JobDescriptionUsage == 75)        //only testing
                                            {

                                            }
                                            if (Convert.ToDateTime(item.JobDescriptionStartDate) <= weekitem.FirstDateofweek && Convert.ToDateTime(item.EndDate) >= weekitem.LastDateofWeek)
                                            {
                                                TotalWorkingDayCount = 5;
                                            }
                                            else
                                            if (Convert.ToDateTime(item.JobDescriptionStartDate) > weekitem.FirstDateofweek &&
                                                Convert.ToDateTime(item.JobDescriptionStartDate) <= weekitem.LastDateofWeek
                                                && Convert.ToDateTime(item.EndDate) >= weekitem.LastDateofWeek)
                                            {
                                                DateTime tempStartDate = Convert.ToDateTime(item.JobDescriptionStartDate);
                                                while (tempStartDate <= weekitem.LastDateofWeek)
                                                {
                                                    DayOfWeek StartDateDay = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(tempStartDate);
                                                    if (StartDateDay <= DayOfWeek.Friday && StartDateDay != DayOfWeek.Saturday && StartDateDay != DayOfWeek.Sunday)
                                                    {
                                                        TotalWorkingDayCount = TotalWorkingDayCount + 1;

                                                    }
                                                    tempStartDate = tempStartDate.AddDays(1);
                                                }
                                            }
                                            else
                                            if (Convert.ToDateTime(item.JobDescriptionStartDate) > weekitem.FirstDateofweek &&
                                                Convert.ToDateTime(item.JobDescriptionStartDate) <= weekitem.LastDateofWeek
                                                && Convert.ToDateTime(item.EndDate) <= weekitem.LastDateofWeek)
                                            {
                                                DateTime tempStartDate = Convert.ToDateTime(item.JobDescriptionStartDate);
                                                while (tempStartDate <= Convert.ToDateTime(item.EndDate))
                                                {
                                                    DayOfWeek StartDateDay = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(tempStartDate);
                                                    if (StartDateDay <= DayOfWeek.Friday && StartDateDay != DayOfWeek.Saturday && StartDateDay != DayOfWeek.Sunday)
                                                    {
                                                        TotalWorkingDayCount = TotalWorkingDayCount + 1;

                                                    }
                                                    tempStartDate = tempStartDate.AddDays(1);

                                                }
                                            }
                                            else
                                            if (Convert.ToDateTime(item.JobDescriptionStartDate) <= weekitem.FirstDateofweek &&
                                                Convert.ToDateTime(item.EndDate) >= weekitem.FirstDateofweek
                                                && Convert.ToDateTime(item.EndDate) <= weekitem.LastDateofWeek)
                                            {
                                                DateTime tempStartDate = Convert.ToDateTime(weekitem.FirstDateofweek);
                                                while (tempStartDate <= Convert.ToDateTime(item.EndDate))
                                                {
                                                    DayOfWeek StartDateDay = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(tempStartDate);
                                                    if (StartDateDay <= DayOfWeek.Friday && StartDateDay != DayOfWeek.Saturday && StartDateDay != DayOfWeek.Sunday)
                                                    {
                                                        TotalWorkingDayCount = TotalWorkingDayCount + 1;

                                                    }
                                                    tempStartDate = tempStartDate.AddDays(1);

                                                }
                                            }
                                            #endregion

                                            #region HRExpected
                                            if (item.CompanyHoliday.Count > 0)
                                            {
                                                var Holiday = item.CompanyHoliday.Where(x => x.StartDate >= FirstDateOfWeek && x.EndDate <= LastDateOfWeek).FirstOrDefault();
                                                if (Holiday != null)
                                                {
                                                    if (Holiday.IsAllDayEvent == 1)
                                                    {
                                                        double TotalHolidayCount = 0;
                                                        TotalHolidayCount = (Convert.ToDateTime(Holiday.EndDate).Date - Convert.ToDateTime(Holiday.StartDate).Date).TotalDays;
                                                        //EmployeeplantOperational.HRExpected = ((Convert.ToDecimal(Employee.WeeklyHoursCount) - (Convert.ToDecimal(Employee.DailyHoursCount) * Convert.ToDecimal(TotalWorkingDayCount))) * (Convert.ToDecimal(item.JobDescriptionUsage) / 100));  ////[gulab lakade][05 05 2023]
                                                        double TotalDayHour = (Convert.ToDouble(Employee.DailyHoursCount) * Convert.ToDouble(TotalWorkingDayCount));            ////[gulab lakade][05 05 2023]
                                                        EmployeeplantOperational.HRExpected = ((Convert.ToDecimal(TotalDayHour) - Convert.ToDecimal(Employee.DailyHoursCount)) * (Convert.ToDecimal(item.JobDescriptionUsage) / 100));  ////[gulab lakade][05 05 2023]
                                                    }
                                                    else
                                                        if (Holiday.IsAllDayEvent == 0)
                                                    {
                                                        double TotalHolidayHours = 0;
                                                        TimeSpan StartDateTime = Convert.ToDateTime(Holiday.StartDate).TimeOfDay;
                                                        TimeSpan EndDateTime = Convert.ToDateTime(Holiday.EndDate).TimeOfDay;
                                                        TotalHolidayHours = Convert.ToDouble((EndDateTime - StartDateTime).TotalHours);
                                                        double TotalDayHour = (Convert.ToDouble(Employee.DailyHoursCount) * Convert.ToDouble(TotalWorkingDayCount));            ////[gulab lakade][05 05 2023]
                                                                                                                                                                                // TotalHolidayHours = TotalHolidayHours + TotalDayHour;       ////[gulab lakade][05 05 2023]
                                                        EmployeeplantOperational.HRExpected = ((Convert.ToDecimal(TotalDayHour) - Convert.ToDecimal(TotalHolidayHours)) * (Convert.ToDecimal(item.JobDescriptionUsage) / 100));
                                                    }
                                                    else
                                                    {
                                                        double TotalDayHour = (Convert.ToDouble(Employee.DailyHoursCount) * Convert.ToDouble(TotalWorkingDayCount));            ////[gulab lakade][05 05 2023]

                                                        //EmployeeplantOperational.HRExpected = ((Convert.ToDecimal(Employee.WeeklyHoursCount) - Convert.ToDecimal(TotalDayHour)) * (Convert.ToDecimal(item.JobDescriptionUsage) / 100)); ////[gulab lakade][05 05 2023]
                                                        EmployeeplantOperational.HRExpected = ((Convert.ToDecimal(TotalDayHour)) * (Convert.ToDecimal(item.JobDescriptionUsage) / 100)); ////[gulab lakade][05 05 2023]
                                                    }
                                                }
                                                else
                                                {
                                                    double TotalDayHour = (Convert.ToDouble(Employee.DailyHoursCount) * Convert.ToDouble(TotalWorkingDayCount));            ////[gulab lakade][05 05 2023]
                                                                                                                                                                            //EmployeeplantOperational.HRExpected = (Convert.ToDecimal(Employee.WeeklyHoursCount) * (Convert.ToDecimal(item.JobDescriptionUsage) / 100));
                                                                                                                                                                            //EmployeeplantOperational.HRExpected = ((Convert.ToDecimal(Employee.WeeklyHoursCount) - Convert.ToDecimal(TotalDayHour)) * (Convert.ToDecimal(item.JobDescriptionUsage) / 100));  ////[gulab lakade][05 05 2023]
                                                    EmployeeplantOperational.HRExpected = ((Convert.ToDecimal(TotalDayHour)) * (Convert.ToDecimal(item.JobDescriptionUsage) / 100));  ////[gulab lakade][05 05 2023]
                                                }
                                            }
                                            else
                                            {
                                                double TotalDayHour = (Convert.ToDouble(Employee.DailyHoursCount) * Convert.ToDouble(TotalWorkingDayCount));        ////[gulab lakade][05 05 2023]
                                                                                                                                                                    //EmployeeplantOperational.HRExpected = (Convert.ToDecimal(Employee.WeeklyHoursCount) * (Convert.ToDecimal(item.JobDescriptionUsage) / 100));
                                                                                                                                                                    //EmployeeplantOperational.HRExpected = ((Convert.ToDecimal(Employee.WeeklyHoursCount) - Convert.ToDecimal(TotalDayHour)) * (Convert.ToDecimal(item.JobDescriptionUsage) / 100));    ////[gulab lakade][05 05 2023]
                                                EmployeeplantOperational.HRExpected = ((Convert.ToDecimal(TotalDayHour)) * (Convert.ToDecimal(item.JobDescriptionUsage) / 100));    ////[gulab lakade][05 05 2023]
                                            }

                                            #endregion


                                        }
                                        catch (Exception ex)
                                        {
                                            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                                            GeosApplication.Instance.Logger.Log("Get an error in FillEmployeeData() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                                        }
                                        try
                                        {
                                            #region HRPlan

                                            if (item.EmployeeLeave.Count > 0)
                                            {
                                                var Leave = item.EmployeeLeave.Where(x => x.StartDate >= FirstDateOfWeek && x.EndDate <= LastDateOfWeek).FirstOrDefault();
                                                if (Leave != null)
                                                {
                                                    if (Leave.IsAllDayEvent == 1)
                                                    {
                                                        double TotalLeaveCount = 0;
                                                        TotalLeaveCount = 1 + (Convert.ToDateTime(Leave.EndDate).Date - Convert.ToDateTime(Leave.StartDate).Date).TotalDays;
                                                        var Holiday = item.CompanyHoliday.Where(x => x.StartDate >= FirstDateOfWeek && x.EndDate <= LastDateOfWeek).FirstOrDefault();
                                                        if (Holiday != null)
                                                        {
                                                            if (Holiday.IsAllDayEvent == 1)
                                                            {
                                                                if (Convert.ToDateTime(Leave.StartDate).Date <= Convert.ToDateTime(Holiday.StartDate).Date && Convert.ToDateTime(Leave.EndDate).Date >= Convert.ToDateTime(Holiday.EndDate).Date)
                                                                {
                                                                    double TotalHolidayCount = 0;
                                                                    TotalHolidayCount = 1 + (Convert.ToDateTime(Holiday.EndDate).Date - Convert.ToDateTime(Holiday.StartDate).Date).TotalDays;
                                                                    TotalLeaveCount = TotalLeaveCount - TotalHolidayCount;
                                                                }
                                                            }
                                                        }
                                                        decimal TotalWeekHours = (Convert.ToDecimal(Employee.DailyHoursCount) * Convert.ToDecimal(TotalLeaveCount));
                                                        //rajashri GEOS2-5859[19-06-2024]
                                                        //if (!CNDB_PlantId.Contains(item.IdCompany))
                                                        //{
                                                        //    EmployeeplantOperational.HRPlan = Convert.ToDecimal(EmployeeplantOperational.HRExpected) - ((Convert.ToDecimal(TotalWeekHours) * (Convert.ToDecimal(item.JobDescriptionUsage) / 100)) + Convert.ToDecimal(totalBreakHoursDecimal));
                                                        //}
                                                        //else
                                                        //{
                                                        EmployeeplantOperational.HRPlan = Convert.ToDecimal(EmployeeplantOperational.HRExpected) - (Convert.ToDecimal(TotalWeekHours) * (Convert.ToDecimal(item.JobDescriptionUsage) / 100));
                                                        //}
                                                    }
                                                    else
                                                        if (Leave.IsAllDayEvent == 0)
                                                    {
                                                        double TotalLeaveHours = 0;
                                                        TimeSpan StartDateTime = Convert.ToDateTime(Leave.StartDate).TimeOfDay;
                                                        TimeSpan EndDateTime = Convert.ToDateTime(Leave.EndDate).TimeOfDay;
                                                        TotalLeaveHours = Convert.ToDouble((EndDateTime - StartDateTime).TotalHours);

                                                        //rajashri GEOS2-5859
                                                        //      if (!CNDB_PlantId.Contains(item.IdCompany))
                                                        //      {
                                                        //          EmployeeplantOperational.HRPlan = Convert.ToDecimal(EmployeeplantOperational.HRExpected) -
                                                        //((Convert.ToDecimal(TotalLeaveHours) * (Convert.ToDecimal(item.JobDescriptionUsage) / 100)) + Convert.ToDecimal(totalBreakHoursDecimal));
                                                        //      }
                                                        //      else
                                                        //      {
                                                        EmployeeplantOperational.HRPlan = Convert.ToDecimal(EmployeeplantOperational.HRExpected) -
                                              (Convert.ToDecimal(TotalLeaveHours) * (Convert.ToDecimal(item.JobDescriptionUsage) / 100));
                                                        // }
                                                    }
                                                    else
                                                    {
                                                        EmployeeplantOperational.HRPlan = (Convert.ToDecimal(EmployeeplantOperational.HRExpected));
                                                    }
                                                }
                                                else
                                                {
                                                    EmployeeplantOperational.HRPlan = (Convert.ToDecimal(EmployeeplantOperational.HRExpected));
                                                }
                                            }
                                            else
                                            {
                                                EmployeeplantOperational.HRPlan = (Convert.ToDecimal(EmployeeplantOperational.HRExpected));
                                            }
                                            #endregion
                                        }

                                        catch (Exception ex)
                                        {
                                            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                                            GeosApplication.Instance.Logger.Log("Get an error in FillEmployeeData() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                                        }

                                    }


                                    #region RealTime

                                    var TempIdStageList = WorkStageWiseJobDescription.Where(x => x.IdJobDescription.Contains(Convert.ToString(item.IdJobDescription))).ToList();
                                    if (TempIdStageList.Count() > 0)
                                    {
                                        EmployeeplantOperational.EmployeePlantOperationalRealTimeList = new List<PlantOperationalPlanningRealInfo>();
                                        foreach (var IDstageitem in TempIdStageList)
                                        {
                                            //if (item.IdEmployee == 242 && IDstageitem.IdWorkStage==7 && EmployeeplantOperational.CalenderWeek=="2023CW03")
                                            //{

                                            //}
                                            if (item.PlantOperationalPlanningRealInfo.Count() > 0)
                                            {

                                                PlantOperationalPlanningRealInfo EmployeeRealTime = new PlantOperationalPlanningRealInfo();
                                                float SumRealTime = 0;
                                                float NotRegisteredTime = 0;
                                                float FinalRealTime = 0;
                                                foreach (var Realitem in item.PlantOperationalPlanningRealInfo.Where(x => x.Idstage == IDstageitem.IdWorkStage && x.CalenderWeek == weekitem.CalenderWeek && x.IdEmployee == item.IdEmployee).ToList())
                                                {
                                                    TimeSpan TSReal = Realitem.EndTime - Realitem.StartTime;
                                                    SumRealTime = SumRealTime + (float)TSReal.TotalMinutes;

                                                }

                                                float temp1 = (float)TimeSpan.FromHours(Convert.ToDouble(EmployeeplantOperational.HRPlan)).TotalMinutes;
                                                NotRegisteredTime = (float)TimeSpan.FromHours(Convert.ToDouble(EmployeeplantOperational.HRPlan)).TotalMinutes - (float)Convert.ToDouble(SumRealTime);
                                                FinalRealTime = NotRegisteredTime + (float)Convert.ToDouble(SumRealTime);
                                                EmployeeRealTime.CalenderWeek = Convert.ToString(weekitem.CalenderWeek);
                                                EmployeeRealTime.IdEmployee = item.IdEmployee;
                                                EmployeeRealTime.Idstage = IDstageitem.IdWorkStage;
                                                EmployeeRealTime.TimeDifferenceInMinutes = (float)Convert.ToDouble(TimeSpan.FromMinutes(FinalRealTime).TotalHours);
                                                EmployeeplantOperational.EmployeePlantOperationalRealTimeList.Add(EmployeeRealTime);
                                            }
                                            else
                                            {
                                                EmployeeplantOperational.EmployeePlantOperationalRealTimeList = new List<PlantOperationalPlanningRealInfo>();
                                                PlantOperationalPlanningRealInfo EmployeeRealTime = new PlantOperationalPlanningRealInfo();
                                                EmployeeRealTime.CalenderWeek = Convert.ToString(weekitem.CalenderWeek);
                                                EmployeeRealTime.IdEmployee = item.IdEmployee;
                                                EmployeeRealTime.Idstage = IDstageitem.IdWorkStage;
                                                EmployeeRealTime.TimeDifferenceInMinutes = (float)Convert.ToDouble(EmployeeplantOperational.HRPlan);
                                                EmployeeplantOperational.EmployeePlantOperationalRealTimeList.Add(EmployeeRealTime);
                                            }
                                        }


                                    }


                                    #endregion


                                    EmployeeplantOperationallist.Add(EmployeeplantOperational);

                                }
                            }

                        }

                    }

                }
                GeosApplication.Instance.Logger.Log("Method FillEmployeeData()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillEmployeeData() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private class TempIdStage
        {
            public Int32 IdStage;
            public Int32 IdJobDescription;
            public decimal JobDescriptionUse;
        }

        #endregion


        private void GridActionLoadCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GridActionLoadCommandAction()...", category: Category.Info, priority: Priority.Low);

                FutureGridControl = (GridControl)obj;

                FutureGridControlFilter = FutureGridControl;

                FutureGridControl.GroupBy("OfferDeliveryDateYear");
                FutureGridControl.GroupBy("Site");
                FutureGridControl.GroupBy("CalendarWeek");
                //FutureGridControl.GroupBy("Code");
                //FutureGridControl.GroupBy("Group");
                ////      FutureGridControl.GroupBy("Region");
                //FutureGridControl.GroupBy("CustomerSite");
                //FutureGridControl.GroupBy("BusinessUnit");
                //FutureGridControl.GroupBy("SalesOwner");

                GeosApplication.Instance.Logger.Log("Method GridActionLoadCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GridActionLoadCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region //[pallavi jadhav][GEOS2-4869][9 27 2023]
        private void FillRTMHRResourcesStage(Int32 IdSite)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillRTMHRResourcesStage ...", category: Category.Info, priority: Priority.Low);
                if (ERMRTMHRResourcesStageList == null)
                {
                    ERMRTMHRResourcesStageList = new List<DeliveryVisualManagementStages>();
                }
                // ERMService = new ERMServiceController("localhost:6699");
                ERMRTMHRResourcesStageList.AddRange(ERMService.GetDVManagementRTMHRResourcesStage_V2440(IdSite));

                GeosApplication.Instance.Logger.Log("Method FillRTMHRResourcesStage() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillRTMHRResourcesStage() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }
        #endregion

        private void ExpandDataCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExpandDataCommandAction()...", category: Category.Info, priority: Priority.Low);

                //FutureGridControl1 = (TableView)obj;
                FutureGridControl = (GridControl)obj;
                if (FutureGridControl.SelectedItem != null)
                {
                    FutureGridControl.ExpandAllGroups();
                }
                else
                {
                    var item = FutureGridControl.View.FocusedRowHandle;


                    FutureGridControl.ExpandGroupRow(item);
                }
                //   FutureGridControl.ExpandGroupRow(-1);
                //FutureGridControl1.ExpandFocusedRow();
                //  FutureGridControl1.expand
                //if(SelectedObject==null)
                //{
                //    SelectedObject = DataTableForGridLayout.DefaultView[(DataTableForGridLayout.Rows.Count - 1)];
                //}
                // //if(FutureGridControl1.ExpandFocusedRow()==true)
                // //{
                //    FutureGridControl.ExpandGroupRow(-1);
                //    // FutureGridControl1.ExpandFocusedRow();
                // //}
                // //else if(FutureGridControl1.ExpandFocusedRow()==false)
                // //{


                //   //  FutureGridControl.ExpandAllGroups();
                //// }
                GeosApplication.Instance.Logger.Log("Method ExpandDataCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ExpandDataCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        private void CollapseDataCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CollapseDataCommandAction()...", category: Category.Info, priority: Priority.Low);
                FutureGridControl = (GridControl)obj;
                if (FutureGridControl.SelectedItem != null)
                {
                    FutureGridControl.CollapseAllGroups();
                }
                else
                {
                    var item = FutureGridControl.View.FocusedRowHandle;


                    FutureGridControl.CollapseGroupRow(item);
                }
                //FutureGridControl1 = (TableView)obj;
                //FutureGridControl1.CollapseFocusedRow();
                GeosApplication.Instance.Logger.Log("Method CollapseDataCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CollapseDataCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }



        private void ChangeDeliveryCWCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChangeDeliveryCWCommandAction()...", category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                if (obj == null)
                {
                }
                else
                {

                    DevExpress.Xpf.Editors.ClosePopupEventArgs CloseModetemp = (DevExpress.Xpf.Editors.ClosePopupEventArgs)obj;

                    if (CloseModetemp.CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
                    {
                    }
                    else
                    {

                        var TempSelectedDeliveryCW = (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).EditValue);
                        List<Object> TmpSelectedDeliveryCW = new List<object>();

                        if (TempSelectedDeliveryCW != null)
                        {
                            foreach (var tmpDeliveryCW in (dynamic)TempSelectedDeliveryCW)
                            {
                                TmpSelectedDeliveryCW.Add(tmpDeliveryCW);
                            }

                            SelectedDeliveryCW = new List<object>();
                            SelectedDeliveryCW = TmpSelectedDeliveryCW;
                        }

                        if (SelectedDeliveryCW == null) SelectedDeliveryCW = new List<object>();

                        //var CalenderWeekInPlantWeekList = PlantWeekList.Where(i => SelectedDeliveryCW.Contains(i.CalenderWeek)).FirstOrDefault();

                        //if (CalenderWeekInPlantWeekList != null)
                        //{
                        //    IsSelectedWeekExistsInPlantWeekList = true;
                        //}
                        //else
                        //{
                        //    IsSelectedWeekExistsInPlantWeekList = false;
                        //}

                        //  List<int> CustomerIds = SelectedCustomer.Select(i => (i as ERMCustomers).IdCustomer).Distinct().ToList();

                        List<ERMRTMFilters> FilteredDeliveryCW = new List<ERMRTMFilters>(AllFiltersList.Where(i => SelectedDeliveryCW.Contains(i.DeliveryWeek)).Distinct().ToList());

                        CustomerList = new List<ERMRTMFilters>(FillCustomerList(FilteredDeliveryCW));
                        CustomerPlantList = new List<ERMRTMFilters>(FillCustomerPlantList(FilteredDeliveryCW));

                        WorkOrderList = new List<ERMRTMFilters>(FillWorkOrderList(FilteredDeliveryCW));
                        ProjectList = new List<ERMRTMFilters>(FillProjectList(FilteredDeliveryCW));

                        ApplyFilterConditions();

                        ResetConditionValues();
                        CreateTable();
                        ActionCreateTable();
                        TotalQuantity = ConditionGreaterThan7Days + ConditionLessThanEqual7Day + ConditionDelay + ConditionQuality + ConditionGoAhead;

                        // FillDashboardHRResources();
                        FillDashboard();
                        AddColumnsToDataTableWithBandsinTestboardProduction();
                        FillDashboardProductionIntime();

                        FutureGridControlFilter.GroupBy("OfferDeliveryDateYear");
                        FutureGridControlFilter.GroupBy("Site");
                        FutureGridControlFilter.GroupBy("CalendarWeek");
                        //FutureGridControlFilter.GroupBy("Code");
                        //FutureGridControlFilter.GroupBy("Group");
                        //FutureGridControlFilter.GroupBy("CustomerSite");
                        //FutureGridControlFilter.GroupBy("BusinessUnit");
                        //FutureGridControlFilter.GroupBy("SalesOwner");

                        //ProductionGridControlFilter.GroupBy("DeliveryWeek");
                        //ProductionGridControlFilter.GroupBy("OTCode");

                    }
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method ChangeDeliveryCWCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ChangeDeliveryCWCommandAction()-2 Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ApplyFilterConditions()
        {

            try
            {
                GeosApplication.Instance.Logger.Log("Method ApplyFilterConditions()...", category: Category.Info, priority: Priority.Low);

                if (SelectedDeliveryCW == null) SelectedDeliveryCW = new List<object>();
                if (SelectedCustomer == null) SelectedCustomer = new List<object>();
                if (SelectedCustomerPlant == null) SelectedCustomerPlant = new List<object>();
                if (SelectedWorkOrder == null) SelectedWorkOrder = new List<object>();
                if (SelectedProject == null) SelectedProject = new List<object>();

                ERMDeliveryVisualManagementList = new ObservableCollection<ERMDeliveryVisualManagement>(ERMDeliveryVisualManagementList_Cloned.ToList());
                // RTMHRResourcesExpectedTimeList = new List<RTMHRResourcesExpectedTime>(RTMHRResourcesExpectedTimeList_Cloned.ToList());
                RTMFutureLoadList = new List<RTMFutureLoad>(RTMFutureLoadList_Cloned.ToList());
                ProductionTimeList = new List<ERMProductionTime>(productionTimeList_Cloned.ToList());

                if (SelectedDeliveryCW.Count == DeliveryCWList.Count && SelectedCustomer.Count == CustomerList.Count && SelectedCustomerPlant.Count == CustomerPlantList.Count && SelectedWorkOrder.Count == WorkOrderList.Count && SelectedProject.Count == ProjectList.Count)
                {
                    ERMDeliveryVisualManagementList = new ObservableCollection<ERMDeliveryVisualManagement>(ERMDeliveryVisualManagementList_Cloned.ToList());
                    //  RTMHRResourcesExpectedTimeList = new List<RTMHRResourcesExpectedTime>(RTMHRResourcesExpectedTimeList_Cloned.ToList());
                    RTMFutureLoadList = new List<RTMFutureLoad>(RTMFutureLoadList_Cloned.ToList());
                    ProductionTimeList = new List<ERMProductionTime>(productionTimeList_Cloned.ToList());
                } // If Selected ALL filters

                else
                {
                    if (SelectedDeliveryCW != null && SelectedDeliveryCW.Count > 0 && SelectedDeliveryCW.Count < DeliveryCWList.Count)
                    {
                        //try
                        //{
                        ERMDeliveryVisualManagementList = new ObservableCollection<ERMDeliveryVisualManagement>(ERMDeliveryVisualManagementList_Cloned.Where(i => SelectedDeliveryCW.Contains(i.DeliveryWeek)).ToList());
                        //  RTMHRResourcesExpectedTimeList = new List<RTMHRResourcesExpectedTime>(RTMHRResourcesExpectedTimeList_Cloned.Where(i => SelectedDeliveryCW.Contains(i.DeliveryWeek)).ToList());
                        RTMFutureLoadList = new List<RTMFutureLoad>(RTMFutureLoadList_Cloned.Where(i => SelectedDeliveryCW.Contains(i.DeliveryWeek)).ToList());
                        ProductionTimeList = new List<ERMProductionTime>(ProductionTimeList_Cloned.Where(i => SelectedDeliveryCW.Contains(i.DeliveryWeek)).ToList());

                        if (SelectedCustomer.Count > 0 && SelectedCustomer.Count < CustomerList.Count)
                        {
                            List<int> CustomerIds = SelectedCustomer.Select(i => (i as ERMRTMFilters).IdCustomer).Distinct().ToList();

                            ERMDeliveryVisualManagementList = new ObservableCollection<ERMDeliveryVisualManagement>(ERMDeliveryVisualManagementList_Cloned.Where(i => SelectedDeliveryCW.Contains(i.DeliveryWeek) && CustomerIds.Contains(i.IdCustomer)).ToList());
                            // RTMHRResourcesExpectedTimeList = RTMHRResourcesExpectedTimeList_Cloned.Where(i => SelectedDeliveryCW.Contains(i.DeliveryWeek) && CustomerIds.Contains(i.IdCustomer)).ToList();
                            RTMFutureLoadList = RTMFutureLoadList_Cloned.Where(i => SelectedDeliveryCW.Contains(i.DeliveryWeek) && CustomerIds.Contains(i.IdCustomer)).ToList();
                            ProductionTimeList = ProductionTimeList_Cloned.Where(i => SelectedDeliveryCW.Contains(i.DeliveryWeek) && CustomerIds.Contains(i.IdCustomer)).ToList();

                            if (SelectedCustomerPlant.Count > 0 && SelectedCustomerPlant.Count < CustomerPlantList.Count)
                            {
                                List<int> CustomerPlantIds = SelectedCustomerPlant.Select(i => (i as ERMRTMFilters).IdOfferSite).Distinct().ToList();
                                ERMDeliveryVisualManagementList = new ObservableCollection<ERMDeliveryVisualManagement>(ERMDeliveryVisualManagementList_Cloned.Where(i => SelectedDeliveryCW.Contains(i.DeliveryWeek) && CustomerIds.Contains(i.IdCustomer) && CustomerPlantIds.Contains(i.IdOfferSite)).ToList());
                                // RTMHRResourcesExpectedTimeList = RTMHRResourcesExpectedTimeList_Cloned.Where(i => SelectedDeliveryCW.Contains(i.DeliveryWeek) && CustomerIds.Contains(i.IdCustomer) && CustomerPlantIds.Contains(i.IdOfferSite)).ToList();
                                RTMFutureLoadList = RTMFutureLoadList_Cloned.Where(i => SelectedDeliveryCW.Contains(i.DeliveryWeek) && CustomerIds.Contains(i.IdCustomer) && CustomerPlantIds.Contains(i.IdOfferSite)).ToList();
                                ProductionTimeList = ProductionTimeList_Cloned.Where(i => SelectedDeliveryCW.Contains(i.DeliveryWeek) && CustomerIds.Contains(i.IdCustomer) && CustomerPlantIds.Contains(i.IDOfferSite)).ToList();

                                if (SelectedWorkOrder.Count > 0 && SelectedWorkOrder.Count < WorkOrderList.Count)
                                {
                                    List<string> WorkOrders = SelectedWorkOrder.Select(i => (i as ERMRTMFilters).WorkOrder).Distinct().ToList();

                                    ERMDeliveryVisualManagementList = new ObservableCollection<ERMDeliveryVisualManagement>(ERMDeliveryVisualManagementList_Cloned.Where(i => SelectedDeliveryCW.Contains(i.DeliveryWeek) && CustomerIds.Contains(i.IdCustomer) && CustomerPlantIds.Contains(i.IdOfferSite) && WorkOrders.Contains(i.Offer)).ToList());

                                    if (SelectedProject.Count > 0 && SelectedProject.Count < ProjectList.Count)
                                    {
                                        List<int> ProjectIds = SelectedProject.Select(i => (i as ERMRTMFilters).IdProject).Distinct().ToList();
                                        ERMDeliveryVisualManagementList = new ObservableCollection<ERMDeliveryVisualManagement>(ERMDeliveryVisualManagementList_Cloned.Where(i => SelectedDeliveryCW.Contains(i.DeliveryWeek) && CustomerIds.Contains(i.IdCustomer) && CustomerPlantIds.Contains(i.IdOfferSite) && WorkOrders.Contains(i.Offer) && ProjectIds.Contains(i.IdProject)).ToList());
                                        //   RTMHRResourcesExpectedTimeList = RTMHRResourcesExpectedTimeList_Cloned.Where(i => SelectedDeliveryCW.Contains(i.DeliveryWeek) && CustomerIds.Contains(i.IdCustomer) && CustomerPlantIds.Contains(i.IdOfferSite) && WorkOrders.Contains(i.Offer) && ProjectIds.Contains(i.IdProject)).ToList();
                                        RTMFutureLoadList = RTMFutureLoadList_Cloned.Where(i => SelectedDeliveryCW.Contains(i.DeliveryWeek) && CustomerIds.Contains(i.IdCustomer) && CustomerPlantIds.Contains(i.IdOfferSite) && WorkOrders.Contains(i.Offer) && ProjectIds.Contains(i.IdProject)).ToList();
                                        ProductionTimeList = ProductionTimeList_Cloned.Where(i => SelectedDeliveryCW.Contains(i.DeliveryWeek) && CustomerIds.Contains(i.IdCustomer) && CustomerPlantIds.Contains(i.IDOfferSite) && WorkOrders.Contains(i.Code) && ProjectIds.Contains(i.IdProject)).ToList();
                                    }
                                } //
                                else
                                {
                                    if (SelectedProject.Count > 0 && SelectedProject.Count < ProjectList.Count)
                                    {
                                        List<int> ProjectIds = SelectedProject.Select(i => (i as ERMRTMFilters).IdProject).Distinct().ToList();
                                        ERMDeliveryVisualManagementList = new ObservableCollection<ERMDeliveryVisualManagement>(ERMDeliveryVisualManagementList_Cloned.Where(i => SelectedDeliveryCW.Contains(i.DeliveryWeek) && CustomerIds.Contains(i.IdCustomer) && CustomerPlantIds.Contains(i.IdOfferSite) && ProjectIds.Contains(i.IdProject)).ToList());
                                        //  RTMHRResourcesExpectedTimeList = RTMHRResourcesExpectedTimeList_Cloned.Where(i => SelectedDeliveryCW.Contains(i.DeliveryWeek) && CustomerIds.Contains(i.IdCustomer) && CustomerPlantIds.Contains(i.IdOfferSite) && ProjectIds.Contains(i.IdProject)).ToList();
                                        RTMFutureLoadList = RTMFutureLoadList_Cloned.Where(i => SelectedDeliveryCW.Contains(i.DeliveryWeek) && CustomerIds.Contains(i.IdCustomer) && CustomerPlantIds.Contains(i.IdOfferSite) && ProjectIds.Contains(i.IdProject)).ToList();
                                        ProductionTimeList = ProductionTimeList_Cloned.Where(i => SelectedDeliveryCW.Contains(i.DeliveryWeek) && CustomerIds.Contains(i.IdCustomer) && CustomerPlantIds.Contains(i.IDOfferSite) && ProjectIds.Contains(i.IdProject)).ToList();
                                    }
                                }
                            }
                            else
                            {
                                if (SelectedWorkOrder.Count > 0 && SelectedWorkOrder.Count < WorkOrderList.Count)
                                {
                                    List<string> WorkOrders = SelectedWorkOrder.Select(i => (i as ERMRTMFilters).WorkOrder).Distinct().ToList();
                                    ERMDeliveryVisualManagementList = new ObservableCollection<ERMDeliveryVisualManagement>(ERMDeliveryVisualManagementList_Cloned.Where(i => SelectedDeliveryCW.Contains(i.DeliveryWeek) && CustomerIds.Contains(i.IdCustomer) && WorkOrders.Contains(i.Offer)).ToList());
                                    // RTMHRResourcesExpectedTimeList = RTMHRResourcesExpectedTimeList_Cloned.Where(i => SelectedDeliveryCW.Contains(i.DeliveryWeek) && CustomerIds.Contains(i.IdCustomer) && WorkOrders.Contains(i.Offer)).ToList();
                                    RTMFutureLoadList = RTMFutureLoadList_Cloned.Where(i => SelectedDeliveryCW.Contains(i.DeliveryWeek) && CustomerIds.Contains(i.IdCustomer) && WorkOrders.Contains(i.Offer)).ToList();
                                    ProductionTimeList = ProductionTimeList_Cloned.Where(i => SelectedDeliveryCW.Contains(i.DeliveryWeek) && CustomerIds.Contains(i.IdCustomer) && WorkOrders.Contains(i.Code)).ToList();

                                    if (SelectedProject.Count > 0 && SelectedProject.Count < ProjectList.Count)
                                    {
                                        List<int> ProjectIds = SelectedProject.Select(i => (i as ERMRTMFilters).IdProject).Distinct().ToList();
                                        ERMDeliveryVisualManagementList = new ObservableCollection<ERMDeliveryVisualManagement>(ERMDeliveryVisualManagementList_Cloned.Where(i => SelectedDeliveryCW.Contains(i.DeliveryWeek) && CustomerIds.Contains(i.IdCustomer) && WorkOrders.Contains(i.Offer) && ProjectIds.Contains(i.IdProject)).ToList());
                                        //   RTMHRResourcesExpectedTimeList = RTMHRResourcesExpectedTimeList_Cloned.Where(i => SelectedDeliveryCW.Contains(i.DeliveryWeek) && CustomerIds.Contains(i.IdCustomer) && WorkOrders.Contains(i.Offer) && ProjectIds.Contains(i.IdProject)).ToList();
                                        RTMFutureLoadList = RTMFutureLoadList_Cloned.Where(i => SelectedDeliveryCW.Contains(i.DeliveryWeek) && CustomerIds.Contains(i.IdCustomer) && WorkOrders.Contains(i.Offer) && ProjectIds.Contains(i.IdProject)).ToList();
                                        ProductionTimeList = ProductionTimeList_Cloned.Where(i => SelectedDeliveryCW.Contains(i.DeliveryWeek) && CustomerIds.Contains(i.IdCustomer) && WorkOrders.Contains(i.Code) && ProjectIds.Contains(i.IdProject)).ToList();
                                    }
                                }
                                else
                                {
                                    if (SelectedProject.Count > 0 && SelectedProject.Count < ProjectList.Count)
                                    {
                                        List<int> ProjectIds = SelectedProject.Select(i => (i as ERMRTMFilters).IdProject).Distinct().ToList();
                                        ERMDeliveryVisualManagementList = new ObservableCollection<ERMDeliveryVisualManagement>(ERMDeliveryVisualManagementList_Cloned.Where(i => SelectedDeliveryCW.Contains(i.DeliveryWeek) && CustomerIds.Contains(i.IdCustomer) && ProjectIds.Contains(i.IdProject)).ToList());
                                        //   RTMHRResourcesExpectedTimeList = RTMHRResourcesExpectedTimeList_Cloned.Where(i => SelectedDeliveryCW.Contains(i.DeliveryWeek) && CustomerIds.Contains(i.IdCustomer) && ProjectIds.Contains(i.IdProject)).ToList();
                                        RTMFutureLoadList = RTMFutureLoadList_Cloned.Where(i => SelectedDeliveryCW.Contains(i.DeliveryWeek) && CustomerIds.Contains(i.IdCustomer) && ProjectIds.Contains(i.IdProject)).ToList();
                                        ProductionTimeList = ProductionTimeList_Cloned.Where(i => SelectedDeliveryCW.Contains(i.DeliveryWeek) && CustomerIds.Contains(i.IdCustomer) && ProjectIds.Contains(i.IdProject)).ToList();
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (SelectedCustomerPlant.Count > 0 && SelectedCustomerPlant.Count < CustomerPlantList.Count)
                            {
                                List<int> CustomerPlantIds = SelectedCustomerPlant.Select(i => (i as ERMRTMFilters).IdOfferSite).Distinct().ToList();
                                ERMDeliveryVisualManagementList = new ObservableCollection<ERMDeliveryVisualManagement>(ERMDeliveryVisualManagementList_Cloned.Where(i => SelectedDeliveryCW.Contains(i.DeliveryWeek) && CustomerPlantIds.Contains(i.IdOfferSite)).ToList());
                                //   RTMHRResourcesExpectedTimeList = RTMHRResourcesExpectedTimeList_Cloned.Where(i => SelectedDeliveryCW.Contains(i.DeliveryWeek) && CustomerPlantIds.Contains(i.IdOfferSite)).ToList();
                                RTMFutureLoadList = RTMFutureLoadList_Cloned.Where(i => SelectedDeliveryCW.Contains(i.DeliveryWeek) && CustomerPlantIds.Contains(i.IdOfferSite)).ToList();
                                ProductionTimeList = ProductionTimeList_Cloned.Where(i => SelectedDeliveryCW.Contains(i.DeliveryWeek) && CustomerPlantIds.Contains(i.IDOfferSite)).ToList();

                                if (SelectedWorkOrder.Count > 0 && SelectedWorkOrder.Count < WorkOrderList.Count)
                                {
                                    List<string> WorkOrders = SelectedWorkOrder.Select(i => (i as ERMRTMFilters).WorkOrder).Distinct().ToList();
                                    ERMDeliveryVisualManagementList = new ObservableCollection<ERMDeliveryVisualManagement>(ERMDeliveryVisualManagementList_Cloned.Where(i => SelectedDeliveryCW.Contains(i.DeliveryWeek) && CustomerPlantIds.Contains(i.IdOfferSite) && WorkOrders.Contains(i.Offer)).ToList());
                                    //  RTMHRResourcesExpectedTimeList = RTMHRResourcesExpectedTimeList_Cloned.Where(i => SelectedDeliveryCW.Contains(i.DeliveryWeek) && CustomerPlantIds.Contains(i.IdOfferSite) && WorkOrders.Contains(i.Offer)).ToList();
                                    RTMFutureLoadList = RTMFutureLoadList_Cloned.Where(i => SelectedDeliveryCW.Contains(i.DeliveryWeek) && CustomerPlantIds.Contains(i.IdOfferSite) && WorkOrders.Contains(i.Offer)).ToList();
                                    ProductionTimeList = ProductionTimeList_Cloned.Where(i => SelectedDeliveryCW.Contains(i.DeliveryWeek) && CustomerPlantIds.Contains(i.IDOfferSite) && WorkOrders.Contains(i.Code)).ToList();

                                    if (SelectedProject.Count > 0 && SelectedProject.Count < ProjectList.Count)
                                    {
                                        List<int> ProjectIds = SelectedProject.Select(i => (i as ERMRTMFilters).IdProject).Distinct().ToList();
                                        ERMDeliveryVisualManagementList = new ObservableCollection<ERMDeliveryVisualManagement>(ERMDeliveryVisualManagementList_Cloned.Where(i => SelectedDeliveryCW.Contains(i.DeliveryWeek) && CustomerPlantIds.Contains(i.IdOfferSite) && WorkOrders.Contains(i.Offer) && ProjectIds.Contains(i.IdProject)).ToList());
                                        //  RTMHRResourcesExpectedTimeList = RTMHRResourcesExpectedTimeList_Cloned.Where(i => SelectedDeliveryCW.Contains(i.DeliveryWeek) && CustomerPlantIds.Contains(i.IdOfferSite) && WorkOrders.Contains(i.Offer) && ProjectIds.Contains(i.IdProject)).ToList();
                                        RTMFutureLoadList = RTMFutureLoadList_Cloned.Where(i => SelectedDeliveryCW.Contains(i.DeliveryWeek) && CustomerPlantIds.Contains(i.IdOfferSite) && WorkOrders.Contains(i.Offer) && ProjectIds.Contains(i.IdProject)).ToList();
                                        ProductionTimeList = ProductionTimeList_Cloned.Where(i => SelectedDeliveryCW.Contains(i.DeliveryWeek) && CustomerPlantIds.Contains(i.IDOfferSite) && WorkOrders.Contains(i.Code) && ProjectIds.Contains(i.IdProject)).ToList();

                                    }
                                }
                                else
                                {
                                    if (SelectedProject.Count > 0 && SelectedProject.Count < ProjectList.Count)
                                    {
                                        List<int> ProjectIds = SelectedProject.Select(i => (i as ERMRTMFilters).IdProject).Distinct().ToList();
                                        ERMDeliveryVisualManagementList = new ObservableCollection<ERMDeliveryVisualManagement>(ERMDeliveryVisualManagementList_Cloned.Where(i => SelectedDeliveryCW.Contains(i.DeliveryWeek) && CustomerPlantIds.Contains(i.IdOfferSite) && ProjectIds.Contains(i.IdProject)).ToList());
                                        // RTMHRResourcesExpectedTimeList = RTMHRResourcesExpectedTimeList_Cloned.Where(i => SelectedDeliveryCW.Contains(i.DeliveryWeek) && CustomerPlantIds.Contains(i.IdOfferSite) && ProjectIds.Contains(i.IdProject)).ToList();
                                        RTMFutureLoadList = RTMFutureLoadList_Cloned.Where(i => SelectedDeliveryCW.Contains(i.DeliveryWeek) && CustomerPlantIds.Contains(i.IdOfferSite) && ProjectIds.Contains(i.IdProject)).ToList();
                                        ProductionTimeList = ProductionTimeList_Cloned.Where(i => SelectedDeliveryCW.Contains(i.DeliveryWeek) && CustomerPlantIds.Contains(i.IDOfferSite) && ProjectIds.Contains(i.IdProject)).ToList();
                                    }
                                }
                            }
                            else
                            {
                                if (SelectedWorkOrder.Count > 0 && SelectedWorkOrder.Count < WorkOrderList.Count)
                                {
                                    List<string> WorkOrders = SelectedWorkOrder.Select(i => (i as ERMRTMFilters).WorkOrder).Distinct().ToList();
                                    ERMDeliveryVisualManagementList = new ObservableCollection<ERMDeliveryVisualManagement>(ERMDeliveryVisualManagementList_Cloned.Where(i => SelectedDeliveryCW.Contains(i.DeliveryWeek) && WorkOrders.Contains(i.Offer)).ToList());
                                    // RTMHRResourcesExpectedTimeList = RTMHRResourcesExpectedTimeList_Cloned.Where(i => SelectedDeliveryCW.Contains(i.DeliveryWeek) && WorkOrders.Contains(i.Offer)).ToList();
                                    RTMFutureLoadList = RTMFutureLoadList_Cloned.Where(i => SelectedDeliveryCW.Contains(i.DeliveryWeek) && WorkOrders.Contains(i.Offer)).ToList();
                                    ProductionTimeList = ProductionTimeList_Cloned.Where(i => SelectedDeliveryCW.Contains(i.DeliveryWeek) && WorkOrders.Contains(i.Code)).ToList();

                                    if (SelectedProject.Count > 0 && SelectedProject.Count < ProjectList.Count)
                                    {
                                        List<int> ProjectIds = SelectedProject.Select(i => (i as ERMRTMFilters).IdProject).Distinct().ToList();
                                        ERMDeliveryVisualManagementList = new ObservableCollection<ERMDeliveryVisualManagement>(ERMDeliveryVisualManagementList_Cloned.Where(i => SelectedDeliveryCW.Contains(i.DeliveryWeek) && WorkOrders.Contains(i.Offer) && ProjectIds.Contains(i.IdProject)).ToList());
                                        //  RTMHRResourcesExpectedTimeList = RTMHRResourcesExpectedTimeList_Cloned.Where(i => SelectedDeliveryCW.Contains(i.DeliveryWeek) && WorkOrders.Contains(i.Offer) && ProjectIds.Contains(i.IdProject)).ToList();
                                        RTMFutureLoadList = RTMFutureLoadList_Cloned.Where(i => SelectedDeliveryCW.Contains(i.DeliveryWeek) && WorkOrders.Contains(i.Offer) && ProjectIds.Contains(i.IdProject)).ToList();
                                        ProductionTimeList = ProductionTimeList_Cloned.Where(i => SelectedDeliveryCW.Contains(i.DeliveryWeek) && WorkOrders.Contains(i.Code) && ProjectIds.Contains(i.IdProject)).ToList();

                                    }
                                }
                                else
                                {
                                    if (SelectedProject.Count > 0 && SelectedProject.Count < ProjectList.Count)
                                    {
                                        List<int> ProjectIds = SelectedProject.Select(i => (i as ERMRTMFilters).IdProject).Distinct().ToList();
                                        ERMDeliveryVisualManagementList = new ObservableCollection<ERMDeliveryVisualManagement>(ERMDeliveryVisualManagementList_Cloned.Where(i => SelectedDeliveryCW.Contains(i.DeliveryWeek) && ProjectIds.Contains(i.IdProject)).ToList());
                                        //   RTMHRResourcesExpectedTimeList = RTMHRResourcesExpectedTimeList_Cloned.Where(i => SelectedDeliveryCW.Contains(i.DeliveryWeek) && ProjectIds.Contains(i.IdProject)).ToList();
                                        RTMFutureLoadList = RTMFutureLoadList_Cloned.Where(i => SelectedDeliveryCW.Contains(i.DeliveryWeek) && ProjectIds.Contains(i.IdProject)).ToList();
                                        ProductionTimeList = ProductionTimeList_Cloned.Where(i => SelectedDeliveryCW.Contains(i.DeliveryWeek) && ProjectIds.Contains(i.IdProject)).ToList();
                                    }

                                }
                            }

                        }

                    }

                    else  // if Delivery week is not selected, check for other filters
                    {

                        if (SelectedCustomer.Count > 0 && SelectedCustomer.Count < CustomerList.Count)
                        {
                            List<int> CustomerIds = SelectedCustomer.Select(i => (i as ERMRTMFilters).IdCustomer).Distinct().ToList();

                            ERMDeliveryVisualManagementList = new ObservableCollection<ERMDeliveryVisualManagement>(ERMDeliveryVisualManagementList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer)).ToList());
                            //  RTMHRResourcesExpectedTimeList = RTMHRResourcesExpectedTimeList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer)).ToList();
                            RTMFutureLoadList = RTMFutureLoadList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer)).ToList();
                            ProductionTimeList = ProductionTimeList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer)).ToList();

                            if (SelectedCustomerPlant.Count > 0 && SelectedCustomerPlant.Count < CustomerPlantList.Count)
                            {
                                List<int> CustomerPlantIds = SelectedCustomerPlant.Select(i => (i as ERMRTMFilters).IdOfferSite).Distinct().ToList();
                                ERMDeliveryVisualManagementList = new ObservableCollection<ERMDeliveryVisualManagement>(ERMDeliveryVisualManagementList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerPlantIds.Contains(i.IdOfferSite)).ToList());
                                //   RTMHRResourcesExpectedTimeList = RTMHRResourcesExpectedTimeList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerPlantIds.Contains(i.IdOfferSite)).ToList();
                                RTMFutureLoadList = RTMFutureLoadList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerPlantIds.Contains(i.IdOfferSite)).ToList();
                                ProductionTimeList = ProductionTimeList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerPlantIds.Contains(i.IDOfferSite)).ToList();

                                if (SelectedWorkOrder.Count > 0 && SelectedWorkOrder.Count < WorkOrderList.Count)
                                {
                                    List<string> WorkOrders = SelectedWorkOrder.Select(i => (i as ERMRTMFilters).WorkOrder).Distinct().ToList();
                                    ERMDeliveryVisualManagementList = new ObservableCollection<ERMDeliveryVisualManagement>(ERMDeliveryVisualManagementList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerPlantIds.Contains(i.IdOfferSite) && WorkOrders.Contains(i.Offer)).ToList());
                                    // RTMHRResourcesExpectedTimeList = RTMHRResourcesExpectedTimeList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerPlantIds.Contains(i.IdOfferSite) && WorkOrders.Contains(i.Offer)).ToList();
                                    RTMFutureLoadList = RTMFutureLoadList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerPlantIds.Contains(i.IdOfferSite) && WorkOrders.Contains(i.Offer)).ToList();
                                    ProductionTimeList = ProductionTimeList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerPlantIds.Contains(i.IDOfferSite) && WorkOrders.Contains(i.Code)).ToList();

                                    if (SelectedProject.Count > 0 && SelectedProject.Count < ProjectList.Count)
                                    {
                                        List<int> ProjectIds = SelectedProject.Select(i => (i as ERMRTMFilters).IdProject).Distinct().ToList();
                                        ERMDeliveryVisualManagementList = new ObservableCollection<ERMDeliveryVisualManagement>(ERMDeliveryVisualManagementList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerPlantIds.Contains(i.IdOfferSite) && WorkOrders.Contains(i.Offer) && ProjectIds.Contains(i.IdProject)).ToList());
                                        //  RTMHRResourcesExpectedTimeList = RTMHRResourcesExpectedTimeList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerPlantIds.Contains(i.IdOfferSite) && WorkOrders.Contains(i.Offer) && ProjectIds.Contains(i.IdProject)).ToList();
                                        RTMFutureLoadList = RTMFutureLoadList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerPlantIds.Contains(i.IdOfferSite) && WorkOrders.Contains(i.Offer) && ProjectIds.Contains(i.IdProject)).ToList();
                                        ProductionTimeList = ProductionTimeList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerPlantIds.Contains(i.IDOfferSite) && WorkOrders.Contains(i.Code) && ProjectIds.Contains(i.IdProject)).ToList();

                                    }
                                }
                                else
                                {
                                    if (SelectedProject.Count > 0 && SelectedProject.Count < ProjectList.Count)
                                    {
                                        List<int> ProjectIds = SelectedProject.Select(i => (i as ERMRTMFilters).IdProject).Distinct().ToList();
                                        ERMDeliveryVisualManagementList = new ObservableCollection<ERMDeliveryVisualManagement>(ERMDeliveryVisualManagementList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerPlantIds.Contains(i.IdOfferSite) && ProjectIds.Contains(i.IdProject)).ToList());
                                        // RTMHRResourcesExpectedTimeList = RTMHRResourcesExpectedTimeList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerPlantIds.Contains(i.IdOfferSite) && ProjectIds.Contains(i.IdProject)).ToList();
                                        RTMFutureLoadList = RTMFutureLoadList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerPlantIds.Contains(i.IdOfferSite) && ProjectIds.Contains(i.IdProject)).ToList();
                                        ProductionTimeList = ProductionTimeList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerPlantIds.Contains(i.IDOfferSite) && ProjectIds.Contains(i.IdProject)).ToList();
                                    }
                                }
                            }
                            else
                            {
                                if (SelectedWorkOrder.Count > 0 && SelectedWorkOrder.Count < WorkOrderList.Count)
                                {
                                    List<string> WorkOrders = SelectedWorkOrder.Select(i => (i as ERMRTMFilters).WorkOrder).Distinct().ToList();
                                    ERMDeliveryVisualManagementList = new ObservableCollection<ERMDeliveryVisualManagement>(ERMDeliveryVisualManagementList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && WorkOrders.Contains(i.Offer)).ToList());
                                    //  RTMHRResourcesExpectedTimeList = RTMHRResourcesExpectedTimeList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && WorkOrders.Contains(i.Offer)).ToList();
                                    RTMFutureLoadList = RTMFutureLoadList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && WorkOrders.Contains(i.Offer)).ToList();
                                    ProductionTimeList = ProductionTimeList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && WorkOrders.Contains(i.Code)).ToList();

                                    if (SelectedProject.Count > 0 && SelectedProject.Count < ProjectList.Count)
                                    {
                                        List<int> ProjectIds = SelectedProject.Select(i => (i as ERMRTMFilters).IdProject).Distinct().ToList();
                                        ERMDeliveryVisualManagementList = new ObservableCollection<ERMDeliveryVisualManagement>(ERMDeliveryVisualManagementList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && WorkOrders.Contains(i.Offer) && ProjectIds.Contains(i.IdProject)).ToList());
                                        //    RTMHRResourcesExpectedTimeList = RTMHRResourcesExpectedTimeList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && WorkOrders.Contains(i.Offer) && ProjectIds.Contains(i.IdProject)).ToList();
                                        RTMFutureLoadList = RTMFutureLoadList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && WorkOrders.Contains(i.Offer) && ProjectIds.Contains(i.IdProject)).ToList();
                                        ProductionTimeList = ProductionTimeList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && WorkOrders.Contains(i.Code) && ProjectIds.Contains(i.IdProject)).ToList();
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (SelectedCustomerPlant.Count > 0 && SelectedCustomerPlant.Count < CustomerPlantList.Count)
                            {
                                List<int> CustomerPlantIds = SelectedCustomerPlant.Select(i => (i as ERMRTMFilters).IdOfferSite).Distinct().ToList();
                                ERMDeliveryVisualManagementList = new ObservableCollection<ERMDeliveryVisualManagement>(ERMDeliveryVisualManagementList_Cloned.Where(i => CustomerPlantIds.Contains(i.IdOfferSite)).ToList());
                                //  RTMHRResourcesExpectedTimeList = RTMHRResourcesExpectedTimeList_Cloned.Where(i => CustomerPlantIds.Contains(i.IdOfferSite)).ToList();
                                RTMFutureLoadList = RTMFutureLoadList_Cloned.Where(i => CustomerPlantIds.Contains(i.IdOfferSite)).ToList();
                                ProductionTimeList = ProductionTimeList_Cloned.Where(i => CustomerPlantIds.Contains(i.IDOfferSite)).ToList();

                                if (SelectedWorkOrder.Count > 0 && SelectedWorkOrder.Count < WorkOrderList.Count)
                                {
                                    List<string> WorkOrders = SelectedWorkOrder.Select(i => (i as ERMRTMFilters).WorkOrder).Distinct().ToList();
                                    ERMDeliveryVisualManagementList = new ObservableCollection<ERMDeliveryVisualManagement>(ERMDeliveryVisualManagementList_Cloned.Where(i => CustomerPlantIds.Contains(i.IdOfferSite) && WorkOrders.Contains(i.Offer)).ToList());
                                    //  RTMHRResourcesExpectedTimeList = RTMHRResourcesExpectedTimeList_Cloned.Where(i => CustomerPlantIds.Contains(i.IdOfferSite) && WorkOrders.Contains(i.Offer)).ToList();
                                    RTMFutureLoadList = RTMFutureLoadList_Cloned.Where(i => CustomerPlantIds.Contains(i.IdOfferSite) && WorkOrders.Contains(i.Offer)).ToList();
                                    ProductionTimeList = ProductionTimeList_Cloned.Where(i => CustomerPlantIds.Contains(i.IDOfferSite) && WorkOrders.Contains(i.Code)).ToList();

                                    if (SelectedProject.Count > 0 && SelectedProject.Count < ProjectList.Count)
                                    {
                                        List<int> ProjectIds = SelectedProject.Select(i => (i as ERMRTMFilters).IdProject).Distinct().ToList();
                                        ERMDeliveryVisualManagementList = new ObservableCollection<ERMDeliveryVisualManagement>(ERMDeliveryVisualManagementList_Cloned.Where(i => CustomerPlantIds.Contains(i.IdOfferSite) && WorkOrders.Contains(i.Offer) && ProjectIds.Contains(i.IdProject)).ToList());
                                        //     RTMHRResourcesExpectedTimeList = RTMHRResourcesExpectedTimeList_Cloned.Where(i => CustomerPlantIds.Contains(i.IdOfferSite) && WorkOrders.Contains(i.Offer) && ProjectIds.Contains(i.IdProject)).ToList();
                                        RTMFutureLoadList = RTMFutureLoadList_Cloned.Where(i => CustomerPlantIds.Contains(i.IdOfferSite) && WorkOrders.Contains(i.Offer) && ProjectIds.Contains(i.IdProject)).ToList();
                                        ProductionTimeList = ProductionTimeList_Cloned.Where(i => CustomerPlantIds.Contains(i.IDOfferSite) && WorkOrders.Contains(i.Code) && ProjectIds.Contains(i.IdProject)).ToList();
                                    }
                                }
                                else
                                {
                                    if (SelectedProject.Count > 0 && SelectedProject.Count < ProjectList.Count)
                                    {
                                        List<int> ProjectIds = SelectedProject.Select(i => (i as ERMRTMFilters).IdProject).Distinct().ToList();
                                        ERMDeliveryVisualManagementList = new ObservableCollection<ERMDeliveryVisualManagement>(ERMDeliveryVisualManagementList_Cloned.Where(i => CustomerPlantIds.Contains(i.IdOfferSite) && ProjectIds.Contains(i.IdProject)).ToList());
                                        //   RTMHRResourcesExpectedTimeList = RTMHRResourcesExpectedTimeList_Cloned.Where(i => CustomerPlantIds.Contains(i.IdOfferSite) && ProjectIds.Contains(i.IdProject)).ToList();
                                        RTMFutureLoadList = RTMFutureLoadList_Cloned.Where(i => CustomerPlantIds.Contains(i.IdOfferSite) && ProjectIds.Contains(i.IdProject)).ToList();
                                        ProductionTimeList = ProductionTimeList_Cloned.Where(i => CustomerPlantIds.Contains(i.IDOfferSite) && ProjectIds.Contains(i.IdProject)).ToList();
                                    }
                                }
                            }

                            else
                            {
                                if (SelectedWorkOrder.Count > 0 && SelectedWorkOrder.Count < WorkOrderList.Count)
                                {
                                    List<string> WorkOrders = SelectedWorkOrder.Select(i => (i as ERMRTMFilters).WorkOrder).Distinct().ToList();
                                    ERMDeliveryVisualManagementList = new ObservableCollection<ERMDeliveryVisualManagement>(ERMDeliveryVisualManagementList_Cloned.Where(i => WorkOrders.Contains(i.Offer)).ToList());
                                    //   RTMHRResourcesExpectedTimeList = RTMHRResourcesExpectedTimeList_Cloned.Where(i => WorkOrders.Contains(i.Offer)).ToList();
                                    RTMFutureLoadList = RTMFutureLoadList_Cloned.Where(i => WorkOrders.Contains(i.Offer)).ToList();
                                    ProductionTimeList = ProductionTimeList_Cloned.Where(i => WorkOrders.Contains(i.Code)).ToList();

                                    if (SelectedProject.Count > 0 && SelectedProject.Count < ProjectList.Count)
                                    {
                                        List<int> ProjectIds = SelectedProject.Select(i => (i as ERMRTMFilters).IdProject).Distinct().ToList();
                                        ERMDeliveryVisualManagementList = new ObservableCollection<ERMDeliveryVisualManagement>(ERMDeliveryVisualManagementList_Cloned.Where(i => WorkOrders.Contains(i.Offer) && ProjectIds.Contains(i.IdProject)).ToList());
                                        //    RTMHRResourcesExpectedTimeList = RTMHRResourcesExpectedTimeList_Cloned.Where(i => WorkOrders.Contains(i.Offer) && ProjectIds.Contains(i.IdProject)).ToList();
                                        RTMFutureLoadList = RTMFutureLoadList_Cloned.Where(i => WorkOrders.Contains(i.Offer) && ProjectIds.Contains(i.IdProject)).ToList();
                                        ProductionTimeList = ProductionTimeList_Cloned.Where(i => WorkOrders.Contains(i.Code) && ProjectIds.Contains(i.IdProject)).ToList();

                                    }
                                }
                                else
                                {
                                    if (SelectedProject.Count > 0 && SelectedProject.Count < ProjectList.Count)
                                    {
                                        List<int> ProjectIds = SelectedProject.Select(i => (i as ERMRTMFilters).IdProject).Distinct().ToList();
                                        ERMDeliveryVisualManagementList = new ObservableCollection<ERMDeliveryVisualManagement>(ERMDeliveryVisualManagementList_Cloned.Where(i => ProjectIds.Contains(i.IdProject)).ToList());
                                        //   RTMHRResourcesExpectedTimeList = RTMHRResourcesExpectedTimeList_Cloned.Where(i => ProjectIds.Contains(i.IdProject)).ToList();
                                        RTMFutureLoadList = RTMFutureLoadList_Cloned.Where(i => ProjectIds.Contains(i.IdProject)).ToList();
                                        ProductionTimeList = ProductionTimeList_Cloned.Where(i => ProjectIds.Contains(i.IdProject)).ToList();
                                    }
                                }

                            }
                        }

                    }

                }

                GeosApplication.Instance.Logger.Log("Method ApplyFilterConditions()....executed successfully", category: Category.Info, priority: Priority.Low);
            }

            catch (Exception ex)
            {

                GeosApplication.Instance.Logger.Log("Get an error in ApplyFilterConditions() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }



        private void ChangeCustomerCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChangeCustomerCommandAction()...", category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                if (obj == null)
                {
                }
                else
                {

                    DevExpress.Xpf.Editors.ClosePopupEventArgs CloseModetemp = (DevExpress.Xpf.Editors.ClosePopupEventArgs)obj;

                    if (CloseModetemp.CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
                    {
                    }
                    else
                    {

                        var TempSelectedCustomer = (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).EditValue);
                        List<object> TmpSelectedCustomer = new List<object>();
                        if (TempSelectedCustomer != null)
                        {
                            foreach (var tmpCustomer in (dynamic)TempSelectedCustomer)
                            {
                                TmpSelectedCustomer.Add(tmpCustomer);
                            }

                            SelectedCustomer = new List<object>();
                            SelectedCustomer = TmpSelectedCustomer;
                        }
                        if (SelectedCustomer == null)
                        {
                            SelectedCustomer = new List<object>();
                        }


                        //Aishwarya Ingale [Geos2-4862][3/10/2023]                    
                        //List<ERMRTMFilters> filteredData = new List<ERMRTMFilters>(AllCustomerPlantList.Where(i => CustomerIds.Contains(i.IdCustomer)).Distinct().ToList());
                        //CustomerPlantList = new List<ERMRTMFilters>(filteredData);
                        //SelectedCustomerPlant = new List<object>();
                        //SelectedCustomerData = filteredData;
                        List<int> CustomerIds = SelectedCustomer.Select(i => (i as ERMRTMFilters).IdCustomer).Distinct().ToList();

                        if (DeliveryCWList == null) DeliveryCWList = new List<string>();
                        List<ERMRTMFilters> FilteredCustomers = new List<ERMRTMFilters>(AllFiltersList.Where(i => CustomerIds.Contains(i.IdCustomer) && DeliveryCWList.Contains(i.DeliveryWeek)).Distinct().ToList());

                        CustomerPlantList = FillCustomerPlantList(FilteredCustomers);
                        WorkOrderList = FillWorkOrderList(FilteredCustomers);
                        ProjectList = FillProjectList(FilteredCustomers);

                        CreateXmlFile();
                        ApplyFilterConditions();

                        ResetConditionValues();
                        CreateTable();
                        ActionCreateTable();
                        TotalQuantity = ConditionGreaterThan7Days + ConditionLessThanEqual7Day + ConditionDelay + ConditionQuality + ConditionGoAhead;

                        //FillDashboardHRResources();
                        FillDashboard();
                        AddColumnsToDataTableWithBandsinTestboardProduction();
                        FillDashboardProductionIntime();

                        FutureGridControlFilter.GroupBy("OfferDeliveryDateYear");
                        FutureGridControlFilter.GroupBy("Site");
                        FutureGridControlFilter.GroupBy("CalendarWeek");
                        //FutureGridControlFilter.GroupBy("Code");
                        //FutureGridControlFilter.GroupBy("Group");
                        //FutureGridControlFilter.GroupBy("CustomerSite");
                        //FutureGridControlFilter.GroupBy("BusinessUnit");
                        //FutureGridControlFilter.GroupBy("SalesOwner");

                        //ProductionGridControlFilter.GroupBy("DeliveryWeek");
                        //ProductionGridControlFilter.GroupBy("OTCode");
                    }
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method ChangeCustomerCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ChangeCustomerCommandAction()-2 Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ChangeCustomerPlantCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChangeCustomerPlantCommandAction()...", category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                if (obj == null)
                {
                }
                else
                {

                    DevExpress.Xpf.Editors.ClosePopupEventArgs CloseModetemp = (DevExpress.Xpf.Editors.ClosePopupEventArgs)obj;

                    if (CloseModetemp.CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
                    {
                    }
                    else
                    {

                        var TempSelectedCustomerPlant = (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).EditValue);
                        List<object> TmpSelectedCustomerPlant = new List<object>();
                        if (TempSelectedCustomerPlant != null)
                        {
                            foreach (var tmpCustomerPlant in (dynamic)TempSelectedCustomerPlant)
                            {
                                TmpSelectedCustomerPlant.Add(tmpCustomerPlant);
                            }

                            SelectedCustomerPlant = new List<object>();
                            SelectedCustomerPlant = TmpSelectedCustomerPlant;
                        }


                        List<int> OfferSiteIds = SelectedCustomerPlant.Select(i => (i as ERMRTMFilters).IdOfferSite).Distinct().ToList();
                        List<ERMRTMFilters> FilteredCustomerPlants = new List<ERMRTMFilters>(AllFiltersList.Where(i => OfferSiteIds.Contains(i.IdOfferSite) && DeliveryCWList.Contains(i.DeliveryWeek)).Distinct().ToList());

                        WorkOrderList = FillWorkOrderList(FilteredCustomerPlants);
                        ProjectList = FillProjectList(FilteredCustomerPlants);

                        CreateXmlFile();
                        ApplyFilterConditions();

                        ResetConditionValues();
                        CreateTable();
                        ActionCreateTable();
                        TotalQuantity = ConditionGreaterThan7Days + ConditionLessThanEqual7Day + ConditionDelay + ConditionQuality + ConditionGoAhead;

                        // FillDashboardHRResources();
                        FillDashboard();
                        AddColumnsToDataTableWithBandsinTestboardProduction();
                        FillDashboardProductionIntime();

                        FutureGridControlFilter.GroupBy("OfferDeliveryDateYear");
                        FutureGridControlFilter.GroupBy("Site");
                        FutureGridControlFilter.GroupBy("CalendarWeek");
                        //FutureGridControlFilter.GroupBy("Code");
                        //FutureGridControlFilter.GroupBy("Group");
                        //FutureGridControlFilter.GroupBy("CustomerSite");
                        //FutureGridControlFilter.GroupBy("BusinessUnit");
                        //FutureGridControlFilter.GroupBy("SalesOwner");

                        //ProductionGridControlFilter.GroupBy("DeliveryWeek");
                        //ProductionGridControlFilter.GroupBy("OTCode");
                    }
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method ChangeCustomerPlantCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ChangeCustomerPlantCommandAction()-2 Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ChangeWorkOrderCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChangeWorkOrderCommandAction()...", category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }


                if (obj == null)
                {
                }
                else
                {
                    DevExpress.Xpf.Editors.ClosePopupEventArgs CloseModetemp = (DevExpress.Xpf.Editors.ClosePopupEventArgs)obj;

                    if (CloseModetemp.CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
                    {
                    }
                    else
                    {

                        var TempSelectedWorkOrder = (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).EditValue);
                        List<object> TmpSelectedWorkOrder = new List<object>();
                        if (TempSelectedWorkOrder != null)
                        {
                            foreach (var tmpCustomerPlant in (dynamic)TempSelectedWorkOrder)
                            {
                                TmpSelectedWorkOrder.Add(tmpCustomerPlant);
                            }

                            SelectedWorkOrder = new List<object>();
                            SelectedWorkOrder = TmpSelectedWorkOrder;
                        }


                        List<string> tmpWorkOrders = SelectedWorkOrder.Select(i => (i as ERMRTMFilters).WorkOrder).Distinct().ToList();
                        List<ERMRTMFilters> FilteredCustomerPlants = new List<ERMRTMFilters>(AllFiltersList.Where(i => tmpWorkOrders.Contains(i.WorkOrder) && DeliveryCWList.Contains(i.DeliveryWeek)).Distinct().ToList());

                        ProjectList = FillProjectList(FilteredCustomerPlants);
                        CreateXmlFile();
                        ApplyFilterConditions();

                        ResetConditionValues();
                        CreateTable();
                        ActionCreateTable();
                        TotalQuantity = ConditionGreaterThan7Days + ConditionLessThanEqual7Day + ConditionDelay + ConditionQuality + ConditionGoAhead;

                        //FillDashboardHRResources();
                        FillDashboard();
                        FillDashboardProductionIntime();

                        FutureGridControlFilter.GroupBy("OfferDeliveryDateYear");
                        FutureGridControlFilter.GroupBy("Site");
                        FutureGridControlFilter.GroupBy("CalendarWeek");
                        //FutureGridControlFilter.GroupBy("Code");
                        //FutureGridControlFilter.GroupBy("Group");
                        //FutureGridControlFilter.GroupBy("CustomerSite");
                        //FutureGridControlFilter.GroupBy("BusinessUnit");
                        //FutureGridControlFilter.GroupBy("SalesOwner");

                        //ProductionGridControlFilter.GroupBy("DeliveryWeek");
                        //ProductionGridControlFilter.GroupBy("OTCode");
                    }
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method ChangeWorkOrderCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ChangeWorkOrderCommandAction()-2 Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        private void ChangeProjectCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChangeProjectCommandAction()...", category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                if (obj == null)
                {
                }
                else
                {

                    DevExpress.Xpf.Editors.ClosePopupEventArgs CloseModetemp = (DevExpress.Xpf.Editors.ClosePopupEventArgs)obj;

                    if (CloseModetemp.CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
                    {
                    }
                    else
                    {

                        var TempSelectedProject = ((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).EditValue;
                        List<object> TmpSelectedProject = new List<object>();
                        if (TempSelectedProject != null)
                        {
                            foreach (var tmpProject in (dynamic)TempSelectedProject)
                            {
                                TmpSelectedProject.Add(tmpProject);
                            }
                            SelectedProject = new List<object>();
                            SelectedProject = TmpSelectedProject;
                        }
                        CreateXmlFile();
                        ApplyFilterConditions();

                        ResetConditionValues();
                        CreateTable();
                        ActionCreateTable();
                        TotalQuantity = ConditionGreaterThan7Days + ConditionLessThanEqual7Day + ConditionDelay + ConditionQuality + ConditionGoAhead;

                        //FillDashboardHRResources();
                        FillDashboard();
                        AddColumnsToDataTableWithBandsinTestboardProduction();
                        FillDashboardProductionIntime();

                        FutureGridControlFilter.GroupBy("OfferDeliveryDateYear");
                        FutureGridControlFilter.GroupBy("Site");
                        FutureGridControlFilter.GroupBy("CalendarWeek");
                        //FutureGridControlFilter.GroupBy("Code");
                        //FutureGridControlFilter.GroupBy("Group");
                        //FutureGridControlFilter.GroupBy("CustomerSite");
                        //FutureGridControlFilter.GroupBy("BusinessUnit");
                        //FutureGridControlFilter.GroupBy("SalesOwner");

                        //ProductionGridControlFilter.GroupBy("DeliveryWeek");
                        //ProductionGridControlFilter.GroupBy("OTCode");

                    }
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method ChangeProjectCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ChangeProjectCommandAction()-2 Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }



        private void FillFilters()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillFilters()...", category: Category.Info, priority: Priority.Low);
                List<string> TempDeliveryCWList = new List<string>();
                if (FilterDataList == null)
                    FilterDataList = new ObservableCollection<ERMDeliveryVisualManagement>();
                if (PlantWeekList == null)
                    PlantWeekList = new List<PlantOperationWeek>();
                if (ProductionTimeList == null)
                    ProductionTimeList = new List<ERMProductionTime>();

                TempDeliveryCWList.AddRange(new List<string>(FilterDataList.Select(item => item.DeliveryWeek).Distinct().OrderBy(i => i).ToList()));
                TempDeliveryCWList.AddRange(new List<string>(PlantWeekList.Select(item => item.CalenderWeek).Distinct().OrderBy(i => i).ToList()));
                TempDeliveryCWList.AddRange(new List<string>(ProductionTimeList.Select(item => item.DeliveryWeek).Distinct().OrderBy(i => i).ToList()));

                //   TempDeliveryCWList.AddRange(new List<string>(RTMHRResourcesExpectedTimeList.Select(item => item.DeliveryWeek).Distinct().OrderBy(i => i).ToList()));

                //    TempDeliveryCWList.AddRange(new List<string>(RTMFutureLoadList.Select(item => item.DeliveryWeek).Distinct().OrderBy(i => i).ToList()));

                DeliveryCWList = new List<string>();
                DeliveryCWList = TempDeliveryCWList.Distinct().OrderBy(i => i).ToList();

                //  List<ERMCustomers> obj = new List<ERMCustomers>();

                var objFilterDataList = FilterDataList.Select(i => new { i.DeliveryWeek, i.IdCustomer, i.Customer, i.IdOfferSite, i.OfferSiteName, i.Offer, i.Project, i.IdProject }).Distinct().ToList();

                //   var objRTMHRResourcesExpectedTimeList = RTMHRResourcesExpectedTimeList.Select(i => new { i.DeliveryWeek, i.IdCustomer, i.Customer, i.IdOfferSite, i.OfferSiteName, i.Offer, i.Project, i.IdProject }).Distinct().ToList();

                var objRTMFutureLoadList = RTMFutureLoadList.Select(i => new { i.DeliveryWeek, i.IdCustomer, i.Customer, i.IdOfferSite, i.OfferSiteName, i.Offer, i.Project, i.IdProject }).Distinct().ToList();

                var objRTMProductionTimeList = ProductionTimeList.Select(i => new { i.DeliveryWeek, i.IdCustomer, i.Customer, i.IDOfferSite, i.OfferSiteName, i.Code, i.Project, i.IdProject }).Distinct().ToList();

                AllFiltersList = new List<ERMRTMFilters>();

                foreach (var item in objFilterDataList)
                {
                    ERMRTMFilters objERMRTMFilters = new ERMRTMFilters();
                    objERMRTMFilters.DeliveryWeek = item.DeliveryWeek;
                    objERMRTMFilters.IdCustomer = item.IdCustomer;
                    objERMRTMFilters.Customer = item.Customer;
                    objERMRTMFilters.IdOfferSite = item.IdOfferSite;
                    objERMRTMFilters.OfferSiteName = item.OfferSiteName;
                    objERMRTMFilters.WorkOrder = item.Offer;
                    objERMRTMFilters.IdProject = item.IdProject;
                    objERMRTMFilters.Project = item.Project;

                    AllFiltersList.Add(objERMRTMFilters);
                }

                //foreach (var item in objRTMHRResourcesExpectedTimeList)
                //{
                //    ERMRTMFilters objERMRTMFilters = new ERMRTMFilters();
                //    objERMRTMFilters.DeliveryWeek = item.DeliveryWeek;
                //    objERMRTMFilters.IdCustomer = item.IdCustomer;
                //    objERMRTMFilters.Customer = item.Customer;
                //    objERMRTMFilters.IdOfferSite = item.IdOfferSite;
                //    objERMRTMFilters.OfferSiteName = item.OfferSiteName;
                //    objERMRTMFilters.WorkOrder = item.Offer;
                //    objERMRTMFilters.IdProject = item.IdProject;
                //    objERMRTMFilters.Project = item.Project;
                //    AllFiltersList.Add(objERMRTMFilters);
                //}

                foreach (var item in objRTMFutureLoadList)
                {
                    ERMRTMFilters objERMRTMFilters = new ERMRTMFilters();
                    objERMRTMFilters.DeliveryWeek = item.DeliveryWeek;
                    objERMRTMFilters.IdCustomer = item.IdCustomer;
                    objERMRTMFilters.Customer = item.Customer;
                    objERMRTMFilters.IdOfferSite = item.IdOfferSite;
                    objERMRTMFilters.OfferSiteName = item.OfferSiteName;
                    objERMRTMFilters.WorkOrder = item.Offer;
                    objERMRTMFilters.IdProject = item.IdProject;
                    objERMRTMFilters.Project = item.Project;
                    AllFiltersList.Add(objERMRTMFilters);
                }

                foreach (var item in objRTMProductionTimeList)
                {
                    ERMRTMFilters objERMRTMFilters = new ERMRTMFilters();
                    objERMRTMFilters.DeliveryWeek = item.DeliveryWeek;
                    objERMRTMFilters.IdCustomer = item.IdCustomer;
                    objERMRTMFilters.Customer = item.Customer;
                    objERMRTMFilters.IdOfferSite = item.IDOfferSite;
                    objERMRTMFilters.OfferSiteName = item.OfferSiteName;
                    objERMRTMFilters.WorkOrder = item.Code;
                    objERMRTMFilters.IdProject = item.IdProject;
                    objERMRTMFilters.Project = item.Project;
                    AllFiltersList.Add(objERMRTMFilters);
                }

                CustomerList = new List<ERMRTMFilters>(FillCustomerList(AllFiltersList));

                CustomerPlantList = new List<ERMRTMFilters>(FillCustomerPlantList(AllFiltersList));

                WorkOrderList = new List<ERMRTMFilters>(FillWorkOrderList(AllFiltersList));
                ProjectList = new List<ERMRTMFilters>(FillProjectList(AllFiltersList));


                //Aishwarya Ingale [Geos2-4862][4/10/2023]

                //var FutureCutomerdata = RTMFutureLoadList.GroupBy(item => item.Customer).Select(group => group.First()).ToList();
                //foreach (var item in FutureCutomerdata)
                //{
                //    ERMCustomers selectcustomer = new ERMCustomers();
                //    var RtmCustomer = CustomerList.Where(i => i.IdCustomer == item.IdCustomer).FirstOrDefault();
                //    if (RtmCustomer == null)
                //    {
                //        selectcustomer.CustomerName = item.Customer;
                //        selectcustomer.IdCustomer = item.IdCustomer;
                //        if (CustomerList == null)
                //        {
                //            CustomerList = new List<ERMCustomers>();
                //        }
                //        CustomerList.Add(selectcustomer);
                //    }
                //}

                //Aishwarya Ingale [Geos2-4862][4/10/2023]

                //var FutureCustomerPlantdata = RTMFutureLoadList.GroupBy(item => item.OfferSiteName).Select(group => group.First()).ToList();
                //foreach (var item in FutureCustomerPlantdata)
                //{
                //    ERMCustomerPlant selectCustomerPlant = new ERMCustomerPlant();
                //    var RtmCustomerPlant = CustomerPlantList.Where(i => i.IdCustomer == item.IdCustomer).FirstOrDefault();
                //    if (RtmCustomerPlant == null)
                //    {
                //        selectCustomerPlant.IdOfferSite = item.IdOfferSite;
                //        selectCustomerPlant.OfferSiteName = item.OfferSiteName;
                //        selectCustomerPlant.IdCustomer = item.IdCustomer;
                //        if (CustomerPlantList == null)
                //        {
                //            CustomerPlantList = new List<ERMCustomerPlant>();
                //        }
                //        CustomerPlantList.Add(selectCustomerPlant);
                //    }
                //}

                //   AllCustomerPlantList = new List<ERMCustomerPlant>(CustomerPlantList.Distinct().ToList());

                //Aishwarya Ingale [Geos2-4862][4/10/2023]             
                //var FutureWorkOrderdata = RTMFutureLoadList.GroupBy(item => item.Offer).Select(group => group.First()).ToList();
                //List<string> offerValues = FutureWorkOrderdata.Select(item => item.Offer).ToList();
                //foreach (var offer in offerValues)
                //{
                //    if (!WorkOrderList.Contains(offer))
                //    {
                //        WorkOrderList.Add(offer);
                //    }
                //}

                //Aishwarya Ingale [Geos2-4862][4/10/2023]             
                //var FutureProjectdata = RTMFutureLoadList.GroupBy(item => item.Project).Select(group => group.First()).ToList();
                //var tempFutureProjectdata = (from item in RTMFutureLoadList
                //                             where item.Project != null
                //                             select new
                //                             {
                //                                 IdProject = item.IdProject,
                //                                 Project = item.Project

                //                             }

                //                        ).Distinct().OrderBy(a => a.Project).ToList();

                //foreach (var item in tempFutureProjectdata)
                //{
                //    bool projectExists = ProjectList.Any(p => p.ProjectName == item.Project);

                //    if (!projectExists)
                //    {
                //        ERMProject objProjectdata = new ERMProject();
                //        objProjectdata.ProjectName = item.Project;
                //        objProjectdata.IdProject = item.IdProject;
                //        ProjectList.Add(objProjectdata);
                //    }
                //}
                //Aishwarya Ingale [Geos2-4862][4/10/2023]            
                //var FutureDeliveryCWdata = RTMFutureLoadList.GroupBy(item => item.DeliveryWeek).Select(group => group.First()).ToList();
                //List<string> DeliveryCWValues = FutureDeliveryCWdata.Select(item => item.DeliveryWeek).ToList();
                //foreach (var DeliveryWeek in DeliveryCWValues)
                //{
                //    if (!DeliveryCWList.Contains(DeliveryWeek))
                //    {
                //        DeliveryCWList.Add(DeliveryWeek);
                //    }
                //}

                CustomerList = CustomerList.Distinct().OrderBy(a => a.Customer).ToList();
                CustomerPlantList = CustomerPlantList.Distinct().OrderBy(a => a.OfferSiteName).ToList();
                ProjectList = ProjectList.Distinct().OrderBy(a => a.Project).ToList();
                WorkOrderList = WorkOrderList.Distinct().OrderBy(a => a.WorkOrder).ToList();

                if (SelectedDeliveryCW == null) SelectedDeliveryCW = new List<object>();
                if (SelectedCustomer == null) SelectedCustomer = new List<object>();
                if (SelectedCustomerPlant == null) SelectedCustomerPlant = new List<object>();
                if (SelectedWorkOrder == null) SelectedWorkOrder = new List<object>();
                if (SelectedProject == null) SelectedProject = new List<object>();

                #region [GEOS2-5030][Aishwarya Ingale][12-12-2023]
                if (!File.Exists(RealTimeMonitorFilterSettingFilePath))
                {
                    SelectedDeliveryCW.AddRange(DeliveryCWList.ToList());
                    SelectedCustomer.AddRange(CustomerList.ToList());
                    SelectedCustomerPlant.AddRange(CustomerPlantList.ToList());
                    SelectedWorkOrder.AddRange(WorkOrderList.ToList());
                    SelectedProject.AddRange(ProjectList.ToList());

                }
                else
                {

                    if (TempXMLDeliveryCWList.Count == 0)//If nothing is selected previously
                        SelectedDeliveryCW = new List<object>();
                    else if (!DeliveryCWList.Intersect(TempXMLDeliveryCWList).Any())
                    {
                        SelectedDeliveryCW = new List<object>(DeliveryCWList);
                    }
                    else
                    {
                        SelectedDeliveryCW = new List<object>(TempXMLDeliveryCWList.Distinct().ToList());
                        //SelectedDeliveryCW.AddRange(TempXMLDeliveryCWList.Distinct().ToList());
                        List<ERMRTMFilters> FilteredDeliveryCW = new List<ERMRTMFilters>(AllFiltersList.Where(i => SelectedDeliveryCW.Contains(i.DeliveryWeek)).Distinct().ToList());
                        CustomerList = new List<ERMRTMFilters>(FillCustomerList(FilteredDeliveryCW));
                        CustomerPlantList = new List<ERMRTMFilters>(FillCustomerPlantList(FilteredDeliveryCW));
                        WorkOrderList = new List<ERMRTMFilters>(FillWorkOrderList(FilteredDeliveryCW));
                        ProjectList = new List<ERMRTMFilters>(FillProjectList(FilteredDeliveryCW));
                    }


                    if (TempXMLCustomerList.Count == 0) //If nothing is selected previously
                        SelectedCustomer = new List<object>();
                    else if (CustomerList.Where(i => TempXMLCustomerList.Contains(i.Customer)).Distinct().Count() == 0)
                        SelectedCustomer = new List<object>(CustomerList);
                    else
                    {
                        SelectedCustomer = new List<object>(CustomerList.Where(i => TempXMLCustomerList.Contains(i.Customer)).Distinct().ToList());
                        List<string> CustomerIds = SelectedCustomer.Select(i => (i as ERMRTMFilters).Customer).Distinct().ToList();
                        List<ERMRTMFilters> FilteredCustomers = new List<ERMRTMFilters>(AllFiltersList.Where(i => CustomerIds.Contains(i.Customer) && DeliveryCWList.Contains(i.DeliveryWeek)).Distinct().ToList());
                        CustomerPlantList = FillCustomerPlantList(FilteredCustomers);
                        WorkOrderList = FillWorkOrderList(FilteredCustomers);
                        ProjectList = FillProjectList(FilteredCustomers);
                    }


                    if (TempXMLCustomerPlantList.Count == 0) // If nothing is selected previously
                        SelectedCustomerPlant = new List<object>();
                    else if (CustomerPlantList.Where(i => TempXMLCustomerPlantList.Contains(i.IdOfferSite)).Distinct().Count() == 0)
                        SelectedCustomerPlant = new List<object>(CustomerPlantList.ToList());
                    else
                    {
                        SelectedCustomerPlant = new List<object>(CustomerPlantList.Where(i => TempXMLCustomerPlantList.Contains(i.IdOfferSite)).Distinct().ToList());
                        List<int> OfferSiteIds = SelectedCustomerPlant.Select(i => (i as ERMRTMFilters).IdOfferSite).Distinct().ToList();
                        List<ERMRTMFilters> FilteredCustomerPlants = new List<ERMRTMFilters>(AllFiltersList.Where(i => OfferSiteIds.Contains(i.IdOfferSite) && DeliveryCWList.Contains(i.DeliveryWeek)).Distinct().ToList());
                        WorkOrderList = FillWorkOrderList(FilteredCustomerPlants);
                        ProjectList = FillProjectList(FilteredCustomerPlants);

                    }


                    if (TempXMLWorkOrderList.Count == 0) // If nothing is selected previously
                        SelectedWorkOrder = new List<object>();
                    else if (WorkOrderList.Where(i => TempXMLWorkOrderList.Contains(i.WorkOrder)).Distinct().Count() == 0)
                        SelectedWorkOrder = new List<object>(WorkOrderList.ToList());
                    else
                    {
                        SelectedWorkOrder = new List<object>(WorkOrderList.Where(i => TempXMLWorkOrderList.Contains(i.WorkOrder)).Distinct().ToList());
                        List<string> tmpWorkOrders = SelectedWorkOrder.Select(i => (i as ERMRTMFilters).WorkOrder).Distinct().ToList();
                        List<ERMRTMFilters> FilteredCustomerPlants = new List<ERMRTMFilters>(AllFiltersList.Where(i => tmpWorkOrders.Contains(i.WorkOrder) && DeliveryCWList.Contains(i.DeliveryWeek)).Distinct().ToList());
                        ProjectList = FillProjectList(FilteredCustomerPlants);

                    }

                    if (TempXMLProjectList.Count == 0) // If nothing is selected previously
                        SelectedProject = new List<object>();
                    else if (ProjectList.Where(i => TempXMLProjectList.Contains(i.Project)).Distinct().Count() == 0)
                        SelectedProject = new List<object>(ProjectList.ToList());
                    else
                    {
                        SelectedProject = new List<object>(ProjectList.Where(i => TempXMLProjectList.Contains(i.Project)).Distinct().ToList());

                        List<object> TmpSelectedProject = new List<object>();
                        SelectedProject = new List<object>();
                        SelectedProject = TmpSelectedProject;
                    }
                    ApplyFilterConditions();

                    ResetConditionValues();
                    CreateTable();
                    ActionCreateTable();
                    TotalQuantity = ConditionGreaterThan7Days + ConditionLessThanEqual7Day + ConditionDelay + ConditionQuality + ConditionGoAhead;

                    //FillDashboardHRResources();
                    FillDashboard();
                    FillDashboardProductionIntime();
                }
                #endregion [GEOS2-5030][Aishwarya Ingale][12-12-2023]
                GeosApplication.Instance.Logger.Log("Method FillFilters()....executed successfully", category: Category.Info, priority: Priority.Low);
            }

            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillFilters() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        private void FilterExpandedCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FilterExpandedCommandAction()...", category: Category.Info, priority: Priority.Low);
                LayoutGroup filterLayout = (LayoutGroup)obj;
                filterLayout.Height = 80;
                GeosApplication.Instance.Logger.Log("Method FilterExpandedCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FilterExpandedCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        private List<ERMRTMFilters> FillCustomerList(List<ERMRTMFilters> FiltersCustomerList)
        {
            List<ERMRTMFilters> objCustomerList = new List<ERMRTMFilters>();

            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCustomerList()...", category: Category.Info, priority: Priority.Low);

                var tempCustomerList = (from item in FiltersCustomerList
                                        select new
                                        {
                                            Id = item.IdCustomer,
                                            Name = item.Customer
                                        }

                                       ).Distinct().OrderBy(a => a.Name).ToList();




                foreach (var item in tempCustomerList)
                {
                    if (!objCustomerList.Exists(i => i.IdCustomer == item.Id))
                    {
                        ERMRTMFilters objCustomer = new ERMRTMFilters();
                        objCustomer.IdCustomer = item.Id;
                        objCustomer.Customer = item.Name;
                        objCustomerList.Add(objCustomer);
                    }
                }

                objCustomerList = objCustomerList.Distinct().OrderBy(a => a.Customer).ToList();

                GeosApplication.Instance.Logger.Log("Method FillCustomerList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCustomerList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            return objCustomerList;
        }


        private List<ERMRTMFilters> FillCustomerPlantList(List<ERMRTMFilters> FiltersCustomerPlantList)
        {
            List<ERMRTMFilters> objCustomerPlantList = new List<ERMRTMFilters>();
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCustomerPlantList()...", category: Category.Info, priority: Priority.Low);

                var tempCustomerPlantList = (from item in FiltersCustomerPlantList
                                             select new
                                             {
                                                 IdCustomer = item.IdCustomer,
                                                 IdOfferSite = item.IdOfferSite,
                                                 OfferSiteName = item.OfferSiteName
                                             }

                                        ).Distinct().OrderBy(a => a.OfferSiteName).ToList();



                foreach (var item in tempCustomerPlantList)
                {

                    if (!objCustomerPlantList.Exists(i => i.IdOfferSite == item.IdOfferSite))
                    {
                        ERMRTMFilters objCustomerPlant = new ERMRTMFilters();
                        objCustomerPlant.IdOfferSite = item.IdOfferSite;
                        objCustomerPlant.OfferSiteName = item.OfferSiteName;
                        objCustomerPlant.IdCustomer = item.IdCustomer;
                        objCustomerPlantList.Add(objCustomerPlant);
                    }
                }

                objCustomerPlantList = objCustomerPlantList.Distinct().OrderBy(a => a.OfferSiteName).ToList();

                GeosApplication.Instance.Logger.Log("Method FillCustomerPlantList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCustomerPlantList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            return objCustomerPlantList;
        }

        private List<ERMRTMFilters> FillWorkOrderList(List<ERMRTMFilters> FiltersWorkOrderList)
        {
            List<ERMRTMFilters> objWorkOrderList = new List<ERMRTMFilters>();
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillWorkOrderList()...", category: Category.Info, priority: Priority.Low);


                var tempWorkOrderList = (from item in FiltersWorkOrderList
                                         select new
                                         {
                                             WorkOrder = item.WorkOrder,
                                         }

                                        ).Distinct().OrderBy(a => a.WorkOrder).ToList();

                foreach (var item in tempWorkOrderList)
                {

                    if (!objWorkOrderList.Exists(i => i.WorkOrder == item.WorkOrder))
                    {
                        ERMRTMFilters objWorkOrder = new ERMRTMFilters();
                        objWorkOrder.WorkOrder = item.WorkOrder;
                        objWorkOrderList.Add(objWorkOrder);
                    }
                }

                objWorkOrderList = objWorkOrderList.Distinct().OrderBy(a => a.Project).ToList();

                GeosApplication.Instance.Logger.Log("Method FillWorkOrderList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillWorkOrderList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            return objWorkOrderList;
        }

        private List<ERMRTMFilters> FillProjectList(List<ERMRTMFilters> FiltersProjectList)
        {
            List<ERMRTMFilters> objProjectList = new List<ERMRTMFilters>();
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillProjectList()...", category: Category.Info, priority: Priority.Low);

                var tempProjectList = (from item in FiltersProjectList
                                       select new
                                       {
                                           IdProject = item.IdProject,
                                           Project = item.Project
                                       }

                                       ).Distinct().OrderBy(a => a.Project).ToList();

                foreach (var item in tempProjectList.Where(item => item.Project != null))
                {
                    if (!objProjectList.Exists(i => i.IdProject == item.IdProject))
                    {
                        ERMRTMFilters objProject = new ERMRTMFilters();
                        objProject.IdProject = item.IdProject;
                        objProject.Project = item.Project;
                        objProjectList.Add(objProject);
                    }
                }
                objProjectList = objProjectList.Distinct().OrderBy(a => a.WorkOrder).ToList();

                GeosApplication.Instance.Logger.Log("Method FillProjectList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillProjectList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            return objProjectList;
        }

        //[GEOS2-4909][Aishwarya Ingale][27-10-2023]
        private void FillProductionIntime()
        {
            try
            {

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                GeosApplication.Instance.Logger.Log("Method FillProductionIntime ...", category: Category.Info, priority: Priority.Low);

                ProductionTimeList = new List<ERMProductionTime>();
                if (GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
                {
                    List<Site> plantOwners = ERMCommon.Instance.SelectedAuthorizedPlantsList.Cast<Site>().ToList();
                    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.Name));
                    ERMCommon.Instance.FailedPlants = new List<string>();
                    ERMCommon.Instance.IsShowFailedPlantWarning = false;
                    ERMCommon.Instance.WarningFailedPlants = string.Empty;
                    foreach (var itemPlantOwnerUsers in plantOwners)
                    {

                        string PlantName = Convert.ToString(itemPlantOwnerUsers.Name);
                        try
                        {
                            string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.Name == itemPlantOwnerUsers.Name).Select(xy => xy.ServiceProviderUrl).FirstOrDefault();
                            ERMService = new ERMServiceController(serviceurl);
                            //ERMService = new ERMServiceController("localhost:6699");
                            //ProductionTimeList.AddRange(ERMService.GetRTMTestBoardsInProduction_V2450(itemPlantOwnerUsers.IdSite, DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),
                            //     DateTime.ParseExact(ToDate, "dd/MM/yyyy", null), GeosAppSettingList));
                            ProductionTimeList.AddRange(ERMService.GetRTMTestBoardsInProduction_V2540(itemPlantOwnerUsers.IdSite, DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),
                                DateTime.ParseExact(ToDate, "dd/MM/yyyy", null), GeosAppSettingList)); //[pallavi jadhav][GEOS2-5907][17 07 2024]

                            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        }
                        catch (FaultException<ServiceException> ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", PlantName, " Failed");
                            if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                if (!ERMCommon.Instance.FailedPlants.Contains(PlantName))
                                {
                                    ERMCommon.Instance.FailedPlants.Add(PlantName);
                                    if (ERMCommon.Instance.FailedPlants != null && ERMCommon.Instance.FailedPlants.Count > 0)
                                    {
                                        ERMCommon.Instance.IsShowFailedPlantWarning = true;
                                        ERMCommon.Instance.WarningFailedPlants = string.Join(",", ERMCommon.Instance.FailedPlants.Select(x => x.ToString()).ToArray());
                                        ERMCommon.Instance.WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), ERMCommon.Instance.WarningFailedPlants.ToString());
                                    }
                                }
                            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", PlantName, " Failed ", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                        }
                        catch (ServiceUnexceptedException ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", PlantName, " Failed");
                            if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                if (!ERMCommon.Instance.FailedPlants.Contains(PlantName))
                                {
                                    ERMCommon.Instance.FailedPlants.Add(PlantName);
                                    if (ERMCommon.Instance.FailedPlants != null && ERMCommon.Instance.FailedPlants.Count > 0)
                                    {
                                        ERMCommon.Instance.IsShowFailedPlantWarning = true;
                                        ERMCommon.Instance.WarningFailedPlants = string.Join(",", ERMCommon.Instance.FailedPlants.Select(x => x.ToString()).ToArray());
                                        ERMCommon.Instance.WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), ERMCommon.Instance.WarningFailedPlants.ToString());
                                    }
                                }
                            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", PlantName, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", PlantName, " Failed");
                            if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                if (!ERMCommon.Instance.FailedPlants.Contains(PlantName))
                                {
                                    ERMCommon.Instance.FailedPlants.Add(PlantName);
                                    if (ERMCommon.Instance.FailedPlants != null && ERMCommon.Instance.FailedPlants.Count > 0)
                                    {
                                        ERMCommon.Instance.IsShowFailedPlantWarning = true;
                                        ERMCommon.Instance.WarningFailedPlants = string.Join(",", ERMCommon.Instance.FailedPlants.Select(x => x.ToString()).ToArray());
                                        ERMCommon.Instance.WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), ERMCommon.Instance.WarningFailedPlants.ToString());
                                    }
                                }
                            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", PlantName, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }
                    }

                    ProductionTimeList_Cloned = new List<ERMProductionTime>();
                    ProductionTimeList_Cloned = ProductionTimeList.ToList();

                    GeosApplication.Instance.SplashScreenMessage = string.Empty;
                    if (ERMCommon.Instance.FailedPlants == null || ERMCommon.Instance.FailedPlants.Count == 0)
                    {
                        ERMCommon.Instance.IsShowFailedPlantWarning = false;
                        ERMCommon.Instance.WarningFailedPlants = string.Empty;
                    }
                }

                GeosApplication.Instance.Logger.Log("Method FillProductionIntime() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
        }


        //[GEOS2-4909][Aishwarya Ingale][27-10-2023]
        private void FillDashboardProductionIntime()
        {
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                GeosApplication.Instance.Logger.Log("Method FillDashboardProductionIntime ...", category: Category.Info, priority: Priority.Low);
                DataTableForGridLayoutProductionInTime.Clear();

                DataTable DataTableForGridLayoutProductionInTimeCopy = DataTableForGridLayoutProductionInTime.Copy();
                string CategoryName = string.Empty;
                var distinctByIdoffer = ProductionTimeList
                                .GroupBy(pt => pt.IdOffer)
                                .Select(g => g.First())
                                .ToList();                              // [pallavi jadhav] [GEOS2-5831][03-07-2024]
                ProductionTimeList = new List<ERMProductionTime>(distinctByIdoffer.ToList());
                if (ProductionTimeList.Count > 0)
                {
                    Int32 GrandTotalProductionInTime = 0;
                    Int64 InProductionCount = 0;
                    Int64 ModulesCount = 0;
                    Int64 ProducedModules = 0;
                    DateTime currentDate = DateTime.Now;
                    int currentWeek = (int)(currentDate.DayOfYear / 7) + 1;
                    int year = DateTime.Now.Year;
                    string Week = year + "CW" + currentWeek;
                    foreach (ERMProductionTime item in ProductionTimeList)
                    {
                        try
                        {
                            DataRow dr = DataTableForGridLayoutProductionInTimeCopy.NewRow();
                            //dr["RowLabels"] = item.MergeCode;
                            dr["RowLabels"] = item.Code;  // [pallavi jadhav] [GEOS2-5831][03-07-2024]
                            dr["Deliveryweek"] = item.DeliveryWeek;
                            dr["OTCode"] = item.OtCode;
                            dr["Modules"] = item.Modules;
                            dr["ProducedModules"] = item.ProducedModules;
                            dr["GroupColor"] = false;
                            if (item.DeliveryWeek == Week)
                            {
                                // string curWeek = rowsInCurrentWeek.Select(a => a.ToString()).FirstOrDefault();
                                //GroupColor = Week;
                                dr["GroupColor"] = true;
                            }

                            if (String.IsNullOrEmpty(Convert.ToString(item.Modules))) ModulesCount = 0;
                            else
                                ModulesCount = item.Modules;

                            if (String.IsNullOrEmpty(Convert.ToString(item.ProducedModules))) ProducedModules = 0;
                            else
                                ProducedModules = item.ProducedModules;

                            InProductionCount = ModulesCount - ProducedModules;

                            dr["InProduction"] = InProductionCount;

                            //dr["DeliveryDate"] = Convert.ToDateTime(item.DeliveryDate);
                            dr["DeliveryDateHtmlColor"] = item.DeliveryDateHtmlColor;

                            //   item.ProductionList.FirstOrDefault().IdProductCategory
                            int CategoryQuantity = 0;
                            int CategoryCount = 0;
                            List<ERMProductionTime> objMergeCode = new List<ERMProductionTime>();

                            CategoryCount = ProductionTimeList.Where(i => i.Code == item.Code).ToList().Count(); // [pallavi jadhav] [GEOS2-5831][03-07-2024]

                            if (item.ProductCategoryGrid != null)
                            {
                                if (item.ProductCategoryGrid.Category != null)
                                {
                                    CategoryName = item.ProductCategoryGrid.Category.Name;

                                }
                                else
                                {
                                    CategoryName = item.ProductCategoryGrid.Name;
                                }

                                //if (item.ProductCategoryGrid.Category != null)
                                //{
                                //CategoryName = item.ProductCategoryGrid.Category.Name;
                                if (!string.IsNullOrEmpty(CategoryName))
                                {
                                    if (dr[CategoryName] != null)
                                    {
                                        CategoryQuantity = String.IsNullOrEmpty(Convert.ToString(dr[CategoryName])) ? 0 : Convert.ToInt32(dr[CategoryName]);
                                    }

                                    dr[CategoryName] = CategoryQuantity + CategoryCount;
                                }
                                //}
                            }

                            GrandTotalProductionInTime = 0;
                            foreach (string str in CategoryColumns)
                            {
                                if (!String.IsNullOrEmpty(Convert.ToString(dr[str])))
                                {
                                    GrandTotalProductionInTime = GrandTotalProductionInTime + Convert.ToInt32(dr[str]);
                                }
                            }

                            dr["GrandTotal"] = GrandTotalProductionInTime;
                            DataTableForGridLayoutProductionInTimeCopy.Rows.Add(dr);

                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }


                DataTableForGridLayoutProductionInTime = DataTableForGridLayoutProductionInTimeCopy;

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method FillDashboardProductionIntime()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillDashboardProductionIntime() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }



        //[GEOS2-4909][Aishwarya Ingale][27-10-2023]
        private void GridActionProductionLoadCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GridActionProductionLoadCommandAction()...", category: Category.Info, priority: Priority.Low);

                ProductionGridControl = (GridControl)obj;

                ProductionGridControlFilter = ProductionGridControl;

                // ProductionGridControl.GroupBy("DeliveryWeek");
                // ProductionGridControl.GroupBy("OTCode");

                GeosApplication.Instance.Logger.Log("Method GridActionProductionLoadCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GridActionProductionLoadCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //[GEOS2-4909][Aishwarya Ingale][27-10-2023]
        private void AddColumnsToDataTableWithBandsinTestboardProduction()
        {
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTableWithBandsinTestboardProduction ...", category: Category.Info, priority: Priority.Low);

                CategoryColumns = new List<string>();

                ColumnsProductionsInTime = new ObservableCollection<Emdep.Geos.UI.Helper.Column>()
                {
                    new Emdep.Geos.UI.Helper.Column() { FieldName="RowLabels",HeaderText="Row Labels", Settings = SettingsType.RowLabels, AllowCellMerge=false, Width=120, AllowEditing = false, Visible= true, IsVertical = false, FixedWidth=true, IsReadOnly = true  },
                    new Emdep.Geos.UI.Helper.Column() { FieldName="DeliveryWeek",HeaderText="", Settings = SettingsType.DeliveryWeek, AllowCellMerge =false, Width=70, AllowEditing = false, Visible= true, IsVertical = false, FixedWidth=true, IsReadOnly = true  },
                    new Emdep.Geos.UI.Helper.Column() { FieldName="Modules",HeaderText="#Modules", Settings = SettingsType.Modules, AllowCellMerge=false, Width=60, AllowEditing = false, Visible= true, IsVertical = false, FixedWidth=true, IsReadOnly = true  },
                    new Emdep.Geos.UI.Helper.Column() { FieldName="ProducedModules",HeaderText="Produced Modules", Settings = SettingsType.ProducedModules, AllowCellMerge=false, Width=145, AllowEditing = false, Visible= true, IsVertical = false, FixedWidth=true, IsReadOnly = true  },
                    new Emdep.Geos.UI.Helper.Column() { FieldName="InProduction",HeaderText="In Production", Settings = SettingsType.InProduction, AllowCellMerge=false, Width=150, AllowEditing = false, Visible= true, IsVertical = false, FixedWidth=true, IsReadOnly = true  },
                    new Emdep.Geos.UI.Helper.Column() { FieldName="OTCode",HeaderText="OTCode", Settings = SettingsType.OTCode, AllowCellMerge=false, Width=60, AllowEditing = false, Visible= false, IsVertical = false, FixedWidth=true, IsReadOnly = true  },
                    //new Emdep.Geos.UI.Helper.Column() { FieldName="DeliveryDate",HeaderText="DeliveryDate", Settings = SettingsType.DeliveryDate, AllowCellMerge=false, Width=120, AllowEditing = false, Visible= false, IsVertical = false, FixedWidth=true, IsReadOnly = true  },

                };


                GroupSummaryTestboardsInProduction = new ObservableCollection<Emdep.Geos.UI.Helper.Summary>();
                TotalSummaryTestboardsInProduction = new ObservableCollection<Emdep.Geos.UI.Helper.Summary>();

                DataTableForGridLayoutProductionInTime = new DataTable();
                DataTableForGridLayoutProductionInTime.Columns.Add("RowLabels", typeof(string));
                DataTableForGridLayoutProductionInTime.Columns.Add("DeliveryWeek", typeof(string));
                DataTableForGridLayoutProductionInTime.Columns.Add("OTCode", typeof(string));
                DataTableForGridLayoutProductionInTime.Columns.Add("Modules", typeof(Int32));
                DataTableForGridLayoutProductionInTime.Columns.Add("ProducedModules", typeof(Int32));
                DataTableForGridLayoutProductionInTime.Columns.Add("InProduction", typeof(Int32));
                //DataTableForGridLayoutProductionInTime.Columns.Add("DeliveryDate", typeof(DateTime));
                DataTableForGridLayoutProductionInTime.Columns.Add("DeliveryDateHtmlColor", typeof(string));
                DataTableForGridLayoutProductionInTime.Columns.Add("GroupColor", typeof(bool));

                for (int i = 0; i < ProductionTimeList.Count; i++)
                {
                    try
                    {
                        if (ProductionTimeList[i].ProductCategoryGrid != null)
                        {
                            if (ProductionTimeList[i].ProductCategoryGrid.Category != null)
                            {
                                if (!DataTableForGridLayoutProductionInTime.Columns.Contains(ProductionTimeList[i].ProductCategoryGrid.Category.Name))
                                {

                                    ColumnsProductionsInTime.Add(new Emdep.Geos.UI.Helper.Column() { FieldName = ProductionTimeList[i].ProductCategoryGrid.Category.Name.ToString(), HeaderText = ProductionTimeList[i].ProductCategoryGrid.Category.Name.ToString(), Settings = SettingsType.ArrayOfferOptionProduction, AllowCellMerge = false, Width = 50, AllowEditing = false, Visible = true, IsVertical = true, FixedWidth = true });
                                    DataTableForGridLayoutProductionInTime.Columns.Add(ProductionTimeList[i].ProductCategoryGrid.Category.Name.ToString(), typeof(string));
                                    CategoryColumns.Add(ProductionTimeList[i].ProductCategoryGrid.Category.Name.ToString());

                                    GroupSummaryTestboardsInProduction.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Sum, FieldName = ProductionTimeList[i].ProductCategoryGrid.Category.Name, DisplayFormat = "{0}" });
                                    TotalSummaryTestboardsInProduction.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Sum, FieldName = ProductionTimeList[i].ProductCategoryGrid.Category.Name, DisplayFormat = "{0}" });
                                }
                            }
                            else
                            {
                                if (!DataTableForGridLayoutProductionInTime.Columns.Contains(ProductionTimeList[i].ProductCategoryGrid.Name))
                                {

                                    ColumnsProductionsInTime.Add(new Emdep.Geos.UI.Helper.Column() { FieldName = ProductionTimeList[i].ProductCategoryGrid.Name.ToString(), HeaderText = ProductionTimeList[i].ProductCategoryGrid.Name.ToString(), Settings = SettingsType.ArrayOfferOptionProduction, AllowCellMerge = false, Width = 50, AllowEditing = false, Visible = true, IsVertical = true, FixedWidth = true });
                                    DataTableForGridLayoutProductionInTime.Columns.Add(ProductionTimeList[i].ProductCategoryGrid.Name.ToString(), typeof(string));
                                    CategoryColumns.Add(ProductionTimeList[i].ProductCategoryGrid.Name.ToString());

                                    GroupSummaryTestboardsInProduction.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Sum, FieldName = ProductionTimeList[i].ProductCategoryGrid.Name, DisplayFormat = "{0}" });
                                    TotalSummaryTestboardsInProduction.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Sum, FieldName = ProductionTimeList[i].ProductCategoryGrid.Name, DisplayFormat = "{0}" });
                                }
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log("Error in AddColumnsToDataTableWithBandsinTestboardProduction()- 1 Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                    }
                    // }
                }



                ColumnsProductionsInTime.Add(new Emdep.Geos.UI.Helper.Column() { FieldName = "GrandTotal", HeaderText = "GrandTotal", Settings = SettingsType.GrandTotal, AllowCellMerge = false, Width = 120, AllowEditing = false, Visible = true, IsVertical = false, FixedWidth = true, IsReadOnly = true });
                DataTableForGridLayoutProductionInTime.Columns.Add("GrandTotal", typeof(Int32));

                GroupSummaryTestboardsInProduction.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Sum, FieldName = "GrandTotal", DisplayFormat = "{0}" });
                TotalSummaryTestboardsInProduction.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Count, FieldName = "DeliveryWeek", DisplayFormat = "GrandTotal" });
                TotalSummaryTestboardsInProduction.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Sum, FieldName = "GrandTotal", DisplayFormat = "{0}" });
                // TotalSummaryTestboardsInProduction.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Count, FieldName = "RowLabels", DisplayFormat = "Grand Total"  });
                GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTableWithBandsinTestboardProduction executed Successfully", category: Category.Info, priority: Priority.Low);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in AddColumnsToDataTableWithBandsinTestboardProduction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        private void ShowChartDialogWindowCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowChartDialogWindowCommandAction()...", category: Category.Info, priority: Priority.Low);

                RTMShowChartViewModel RTMShowChartViewModel = new RTMShowChartViewModel();
                RTMShowChartView RTMShowChartView = new RTMShowChartView();
                EventHandler handle = delegate { RTMShowChartView.Close(); };
                RTMShowChartViewModel.ERMDeliveryVisualManagementList = ERMDeliveryVisualManagementList;
                RTMShowChartViewModel.ERMDeliveryVisualManagementStagesList = ERMDeliveryVisualManagementStagesList;

                RTMShowChartViewModel.RequestClose += handle;
                RTMShowChartView.DataContext = RTMShowChartViewModel;
                RTMShowChartViewModel.Init();
                RTMShowChartView.ShowDialog();

                GeosApplication.Instance.Logger.Log("Method ShowChartDialogWindowCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ShowChartDialogWindowCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ShowHRResourceGridDialogWindowCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowHRResourceGridDialogWindowCommandAction()...", category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                RTMShowHRResourcesViewModel RTMShowHRResourcesViewModel = new RTMShowHRResourcesViewModel();
                RTMShowHRResourcesView RTMShowHRResourcesView = new RTMShowHRResourcesView();
                EventHandler handle = delegate { RTMShowHRResourcesView.Close(); };
                RTMShowHRResourcesViewModel.EmployeeplantOperationallist = EmployeeplantOperationallist;
                RTMShowHRResourcesViewModel.RTMHRResourcesExpectedTimeList = RTMHRResourcesExpectedTimeList;
                RTMShowHRResourcesViewModel.ERMRTMHRResourcesStageList = ERMRTMHRResourcesStageList;
                RTMShowHRResourcesViewModel.WorkStageWiseJobDescription = WorkStageWiseJobDescription;

                RTMShowHRResourcesViewModel.RequestClose += handle;
                RTMShowHRResourcesView.DataContext = RTMShowHRResourcesViewModel;
                RTMShowHRResourcesViewModel.Init();
                RTMShowHRResourcesView.ShowDialog();

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method ShowHRResourceGridDialogWindowCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ShowHRResourceGridDialogWindowCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        private void ShowCRMExpectedLoadGridDialogWindowCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowCRMExpectedLoadGridDialogWindowCommandAction()...", category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                RTMShowCRMExpectedLoadsViewModel RTMShowCRMExpectedLoadsViewModel = new RTMShowCRMExpectedLoadsViewModel();
                RTMShowCRMExpectedLoadsView RTMShowCRMExpectedLoadsView = new RTMShowCRMExpectedLoadsView();
                EventHandler handle = delegate { RTMShowCRMExpectedLoadsView.Close(); };
                RTMShowCRMExpectedLoadsViewModel.OfferOptions = OfferOptions;
                RTMShowCRMExpectedLoadsViewModel.PlantWeekList = PlantWeekList;
                RTMShowCRMExpectedLoadsViewModel.RTMFutureLoadList = RTMFutureLoadList;

                RTMShowCRMExpectedLoadsViewModel.RequestClose += handle;
                RTMShowCRMExpectedLoadsView.DataContext = RTMShowCRMExpectedLoadsViewModel;
                RTMShowCRMExpectedLoadsViewModel.Init();
                RTMShowCRMExpectedLoadsView.ShowDialog();

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method ShowCRMExpectedLoadGridDialogWindowCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ShowCRMExpectedLoadGridDialogWindowCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ShowProductionLoadGridDialogWindowCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowProductionLoadGridDialogWindowCommandAction()...", category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                RTMShowTestboardsInProductionViewModel RTMShowTestboardsInProductionViewModel = new RTMShowTestboardsInProductionViewModel();
                RTMShowTestboardsInProductionView RTMShowTestboardsInProductionView = new RTMShowTestboardsInProductionView();
                EventHandler handle = delegate { RTMShowTestboardsInProductionView.Close(); };
                RTMShowTestboardsInProductionViewModel.ProductionTimeList = ProductionTimeList;

                RTMShowTestboardsInProductionViewModel.RequestClose += handle;
                RTMShowTestboardsInProductionView.DataContext = RTMShowTestboardsInProductionViewModel;
                RTMShowTestboardsInProductionViewModel.Init();
                RTMShowTestboardsInProductionView.ShowDialog();

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method ShowProductionLoadGridDialogWindowCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ShowProductionLoadGridDialogWindowCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillListOfColor()
        {
            try
            {
                GeosAppSettingList = WorkbenchStartUp.GetSelectedGeosAppSettings("14,15,16,17");
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in Method FillListOfColor() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in  Method FillListOfColor() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(String.Format("Error in FillListOfColor - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void LoadsxWorkstationLayoutCommandAction(object obj)
        {
            ScrollViewer varScroll = (ScrollViewer)obj;
            varScroll.ScrollToBottom();

        }

        #region [GEOS2-5030][Aishwarya Ingale][07-12-2023]
        private void CreateXmlFile()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CreateXmlFile ...", category: Category.Info, priority: Priority.Low);

                XmlDocument doc = new XmlDocument();
                XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                XmlElement root = doc.DocumentElement;
                doc.InsertBefore(xmlDeclaration, root);

                XmlElement element1 = doc.CreateElement(string.Empty, "body", string.Empty);
                doc.AppendChild(element1);

                if (SelectedDeliveryCW != null)
                {
                    List<string> listDeliveryCW = SelectedDeliveryCW.OfType<ERMRTMFilters>().Select(i => Convert.ToString(i.DeliveryWeek)).Distinct().ToList();

                    XmlElement ParentNodeDeliveryCW = doc.CreateElement(string.Empty, "DeliveryCW", string.Empty);
                    element1.AppendChild(ParentNodeDeliveryCW);

                    foreach (string ObjDeliveryCW in SelectedDeliveryCW)
                    {
                        XmlElement childNode1 = doc.CreateElement("DeliveryWeek");
                        XmlText DeliveryWeek = doc.CreateTextNode(ObjDeliveryCW);
                        childNode1.AppendChild(DeliveryWeek);
                        ParentNodeDeliveryCW.AppendChild(childNode1);
                    }
                }


                if (SelectedCustomer != null)
                {
                    List<string> listCustomer = SelectedCustomer
                        .OfType<ERMRTMFilters>()
                        .Select(i => Convert.ToString(i.Customer))
                          .Distinct()
                            .ToList();

                    XmlElement ParentNodeCustomer = doc.CreateElement("Customer");
                    element1.AppendChild(ParentNodeCustomer);

                    foreach (string ObjCustomer in listCustomer)
                    {
                        XmlElement childNode1 = doc.CreateElement("CustomerId");
                        XmlText CustomerId = doc.CreateTextNode(ObjCustomer);
                        childNode1.AppendChild(CustomerId);
                        ParentNodeCustomer.AppendChild(childNode1);
                    }
                }

                if (SelectedCustomerPlant != null)
                {
                    List<string> ListCustomerPlant = SelectedCustomerPlant
                    .OfType<ERMRTMFilters>()
                    .Select(i => Convert.ToString(i.IdOfferSite))
                      .Distinct()
                       .ToList();

                    XmlElement ParentNodeCustomerPlant = doc.CreateElement("CustomerPlant");
                    element1.AppendChild(ParentNodeCustomerPlant);

                    foreach (string ObjCustomerPlant in ListCustomerPlant)
                    {
                        XmlElement childNode1 = doc.CreateElement("IDOfferSite");
                        XmlText IDOfferSite = doc.CreateTextNode(ObjCustomerPlant);
                        childNode1.AppendChild(IDOfferSite);
                        ParentNodeCustomerPlant.AppendChild(childNode1);
                    }
                }

                if (SelectedWorkOrder != null)
                {
                    List<string> ListWorkOrder = SelectedWorkOrder.OfType<ERMRTMFilters>().Select(i => Convert.ToString(i.WorkOrder)).Distinct().ToList();
                    XmlElement ParentNodeWorkOrder = doc.CreateElement("WorkOrder");
                    element1.AppendChild(ParentNodeWorkOrder);

                    foreach (string ObjWorkOrder in ListWorkOrder)
                    {
                        XmlElement childNode1 = doc.CreateElement("Code");
                        XmlText Code = doc.CreateTextNode(Convert.ToString(ObjWorkOrder));
                        childNode1.AppendChild(Code);
                        ParentNodeWorkOrder.AppendChild(childNode1);
                    }
                }

                if (SelectedProject != null)
                {
                    List<string> ListProject = SelectedProject.OfType<ERMRTMFilters>().Select(i => Convert.ToString(i.IdProject)).Distinct().ToList();
                    XmlElement ParentNodeProject = doc.CreateElement("Project");
                    element1.AppendChild(ParentNodeProject);

                    foreach (string ObjProject in ListProject)
                    {
                        XmlElement childNode1 = doc.CreateElement("IdProject");
                        XmlText IdProject = doc.CreateTextNode(ObjProject);
                        childNode1.AppendChild(IdProject);
                        ParentNodeProject.AppendChild(childNode1);
                    }
                }


                doc.Save(RealTimeMonitorFilterSettingFilePath);

                GeosApplication.Instance.Logger.Log("Method CreateXmlFile() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CreateXmlFile() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

        }

        private void ReadXmlFile()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ReadXmlFile ...", category: Category.Info, priority: Priority.Low);

                TempXMLDeliveryCWList = new List<string>();
                TempXMLCustomerList = new List<string>();
                TempXMLCustomerPlantList = new List<int>();
                TempXMLWorkOrderList = new List<string>();
                TempXMLProjectList = new List<string>();


                if (File.Exists(RealTimeMonitorFilterSettingFilePath))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(RealTimeMonitorFilterSettingFilePath);

                    XmlNodeList NodeListDeliveryCW = doc.SelectNodes("/body/DeliveryCW");

                    foreach (XmlNode node in NodeListDeliveryCW)
                    {
                        foreach (XmlNode child in node.ChildNodes)
                        {
                            TempXMLDeliveryCWList.Add(child.InnerText);
                        }
                    }

                    XmlNodeList NodeListCustomer = doc.SelectNodes("/body/Customer");

                    foreach (XmlNode node in NodeListCustomer)
                    {
                        foreach (XmlNode child in node.ChildNodes)
                        {
                            TempXMLCustomerList.Add(child.InnerText);
                        }
                    }

                    XmlNodeList NodeListCustomerPlant = doc.SelectNodes("/body/CustomerPlant");

                    foreach (XmlNode node in NodeListCustomerPlant)
                    {
                        foreach (XmlNode child in node.ChildNodes)
                        {
                            TempXMLCustomerPlantList.Add(Convert.ToInt32(child.InnerText));
                        }
                    }

                    XmlNodeList NodeListWorkOrder = doc.SelectNodes("/body/WorkOrder");

                    foreach (XmlNode node in NodeListWorkOrder)
                    {
                        foreach (XmlNode child in node.ChildNodes)
                        {
                            TempXMLWorkOrderList.Add(child.InnerText);
                        }
                    }

                    XmlNodeList NodeListProject = doc.SelectNodes("/body/Project");

                    foreach (XmlNode node in NodeListProject)
                    {
                        foreach (XmlNode child in node.ChildNodes)
                        {
                            TempXMLProjectList.Add(child.InnerText);
                        }
                    }
                }

                GeosApplication.Instance.Logger.Log("Method ReadXmlFile() executed successfully", category: Category.Info, priority: Priority.Low);
            }

            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ReadXmlFile() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        private void RealTimeMonitorGridControlUnloadedCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RealTimeMonitorGridControlUnloadedCommandAction ...", category: Category.Info, priority: Priority.Low);

                CreateXmlFile();

                GeosApplication.Instance.Logger.Log("Method RealTimeMonitorGridControlUnloadedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in RealTimeMonitorGridControlUnloadedCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }
        #endregion [GEOS2-5030][Aishwarya Ingale][07-12-2023]
    }
}



