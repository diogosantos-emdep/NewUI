using DevExpress.Data;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors.Flyout;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.WindowsUI;
using Emdep.Geos.Data.Common.ERM;
using Emdep.Geos.Data.Common.PCM;
using Emdep.Geos.Modules.ERM.CommonClasses;
using Emdep.Geos.Modules.ERM.Views;
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
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.IO;
using System.Xml;

namespace Emdep.Geos.Modules.ERM.ViewModels
{
    public class PlantLoadAnalysisViewModel : NavigationViewModelBase, INotifyPropertyChanged
    {

        #region Service

        IERMService ERMService = new ERMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        public ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion

        #region Events
        public event EventHandler RequestClose;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
        #endregion

        #region Declaration

        DateTime fromDate;
        DateTime toDate;
        private ObservableCollection<Emdep.Geos.UI.Helper.Column> columnsLoadModules;
        private DataTable dataTableForGridLayoutProductionInTime;
        private ObservableCollection<BandItem> bandsDashboard = new ObservableCollection<BandItem>();
        List<string> failedPlants;
        private List<PlantLoadAnalysis> plantLoadAnalysisList;
        private List<PlantLoadAnalysis> plantLoadAnalysisList_Cloned;
        int isButtonStatus;
        Visibility isCalendarVisible;

        private bool isBusy;
        private bool isPeriod;
        private Duration _currentDuration;
        DateTime startDate;
        DateTime endDate;
        private GridControl FutureGridControl;
        private GridControl futureGridControlFilter;

        #region [GEOS2-5037][Rupali Sarode][21-12-2023]
        private ObservableCollection<BandItem> bandsDashboardLoadEquipment = new ObservableCollection<BandItem>(); //[GEOS2-5037][Rupali Sarode][21-12-2023]
        private DataTable dataTableForGridLayoutLoadEquipment; //[GEOS2-5037][Rupali Sarode][21-12-2023]
        private GridControl gridControlLoadEquipment;
        private GridControl gridControlLoadEquipmentFilter;
        #endregion [GEOS2-5037][Rupali Sarode][21-12-2023]

        #region [GEOS2-5039][Rupali Sarode][21-12-2023]
        private ObservableCollection<BandItem> bandsDashboardLoadWorkstation = new ObservableCollection<BandItem>();
        private DataTable dataTableForGridLayoutLoadWorkstation;
        private GridControl gridControlLoadWorkstation;
        private GridControl gridControlLoadWorkstationFilter;
        private List<DeliveryVisualManagementStages> loadWorkstationStageList;
        private ObservableCollection<Emdep.Geos.UI.Helper.Column> columnsLoadWorkstation;

        #endregion [GEOS2-5039][Rupali Sarode][21-12-2023]

        #region [GEOS2-5038][Aishwarya Ingale][22-12-2023]
        private ObservableCollection<BandItem> bandsDashboardLoadCustomers = new ObservableCollection<BandItem>();
        private DataTable dataTableForGridLayoutLoadCustomers; //[GEOS2-5038][Aishwarya Ingale][22-12-2023]
        private GridControl gridControlLoadCustomers; //[GEOS2-5038][Aishwarya Ingale][22-12-2023]
        private GridControl gridControlLoadCustomersFilter;
        private List<string> categoryColumns;
        private ObservableCollection<Emdep.Geos.UI.Helper.Column> columnsLoadCustomers;
        #endregion [GEOS2-5038][Aishwarya Ingale][22-12-2023]

        #region [GEOS2-5114][28-12-2023][Rupali Sarode]
        public string PlantLoadAnalysisFilterSettingFilePath = GeosApplication.Instance.UserSettingFolderName + "ERM_PlantLoadAnalysisFilterSetting.Xml";
        private List<string> TempXMLRegionsList;
        private List<Int32> TempXMLCustomerGroupsList;
        private List<string> TempXMLCustomerPlantsList;
        private List<string> TempXMLOTCodeList;
        private List<string> TempXMLProjectList;
        private List<string> TempXMLConnectorFamilyList;
        private List<string> TempXMLCptypeList;
        private List<string> TempXMLTemplatesList;
        private List<string> TempXMLOTItemStatussList;
        #endregion [GEOS2-5114][28-12-2023][Rupali Sarode]

        #endregion

        #region Property
        public DateTime FromDate
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
        public DateTime ToDate
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


        public ObservableCollection<Emdep.Geos.UI.Helper.Column> ColumnsLoadModules
        {
            get { return columnsLoadModules; }
            set
            {
                columnsLoadModules = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ColumnsLoadModules"));
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



        public ObservableCollection<BandItem> BandsDashboard
        {
            get { return bandsDashboard; }
            set
            {
                bandsDashboard = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BandsDashboard"));
            }
        }

        public List<string> FailedPlants
        {
            get { return failedPlants; }
            set { failedPlants = value; }
        }
        Boolean isShowFailedPlantWarning;
        string warningFailedPlants;


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


        public List<PlantLoadAnalysis> PlantLoadAnalysisList
        {
            get { return plantLoadAnalysisList; }
            set
            {
                plantLoadAnalysisList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PlantLoadAnalysisList"));
            }
        }
        public List<PlantLoadAnalysis> PlantLoadAnalysisList_Cloned
        {
            get { return plantLoadAnalysisList_Cloned; }
            set
            {
                plantLoadAnalysisList_Cloned = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PlantLoadAnalysisList_Cloned"));
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

        private List<PlantLoadAnalysis> groupByTempPlantLoadAnalysis;
        public List<PlantLoadAnalysis> GroupByTempPlantLoadAnalysis
        {
            get { return groupByTempPlantLoadAnalysis; }
            set
            {
                groupByTempPlantLoadAnalysis = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GroupByTempPlantLoadAnalysis"));
            }
        }



        private List<PlantLoadAnalysis> groupByTempPlantLoadAnalysisCopy;
        public List<PlantLoadAnalysis> GroupByTempPlantLoadAnalysisCopy
        {
            get { return groupByTempPlantLoadAnalysisCopy; }
            set
            {
                groupByTempPlantLoadAnalysisCopy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GroupByTempPlantLoadAnalysisCopy"));
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

        private object myGridControlFilter;
        public object MyGridControlFilter
        {
            get
            {
                return myGridControlFilter;
            }
            set
            {
                myGridControlFilter = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MyGridControlFilter"));
            }
        }


        public ObservableCollection<Emdep.Geos.UI.Helper.Summary> TotalSummary { get; private set; }
        public ObservableCollection<Emdep.Geos.UI.Helper.Summary> GroupSummary { get; private set; }

        private ObservableCollection<Emdep.Geos.UI.Helper.Column> columns;
        //public ObservableCollection<Emdep.Geos.UI.Helper.Column> Columns
        //{
        //    get { return columns; }
        //    set
        //    {
        //        columns = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("Columns"));
        //    }
        //}

        #region [GEOS2-5037][Rupali Sarode][21-12-2023]
        public ObservableCollection<BandItem> BandsDashboardLoadEquipment
        {
            get { return bandsDashboardLoadEquipment; }
            set
            {
                bandsDashboardLoadEquipment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BandsDashboardLoadEquipment"));
            }
        }

        public DataTable DataTableForGridLayoutLoadEquipment
        {
            get
            {
                return dataTableForGridLayoutLoadEquipment;
            }
            set
            {
                dataTableForGridLayoutLoadEquipment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DataTableForGridLayoutLoadEquipment"));
            }
        }

        private List<PlantLoadAnalysis> groupByTempPlantLoadAnalysisEquipment;
        public List<PlantLoadAnalysis> GroupByTempPlantLoadAnalysisEquipment
        {
            get { return groupByTempPlantLoadAnalysisEquipment; }
            set
            {
                groupByTempPlantLoadAnalysisEquipment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GroupByTempPlantLoadAnalysisEquipment"));
            }
        }
        private List<PlantLoadAnalysis> groupByTempPlantLoadAnalysisEquipmentCopy;
        public List<PlantLoadAnalysis> GroupByTempPlantLoadAnalysisEquipmentCopy
        {
            get { return groupByTempPlantLoadAnalysisEquipmentCopy; }
            set
            {
                groupByTempPlantLoadAnalysisEquipmentCopy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GroupByTempPlantLoadAnalysisEquipmentCopy"));
            }
        }

        public GridControl GridControlLoadEquipment
        {
            get
            {
                return gridControlLoadEquipment;
            }
            set
            {
                gridControlLoadEquipment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GridControlLoadEquipment"));
            }
        }

        public GridControl GridControlLoadEquipmentFilter
        {
            get
            {
                return gridControlLoadEquipmentFilter;
            }
            set
            {
                gridControlLoadEquipmentFilter = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GridControlLoadEquipmentFilter"));
            }
        }

        public ObservableCollection<Emdep.Geos.UI.Helper.Summary> TotalSummaryLoadEquipment { get; private set; }
        public ObservableCollection<Emdep.Geos.UI.Helper.Summary> GroupSummaryLoadEquipment { get; private set; }

        #endregion [GEOS2-5037][Rupali Sarode][21-12-2023]


        #region [GEOS2-5039][Rupali Sarode][22-12-2023]
        public ObservableCollection<BandItem> BandsDashboardLoadWorkstation
        {
            get { return bandsDashboardLoadWorkstation; }
            set
            {
                bandsDashboardLoadWorkstation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BandsDashboardLoadWorkstation"));
            }
        }

        public DataTable DataTableForGridLayoutLoadWorkstation
        {
            get
            {
                return dataTableForGridLayoutLoadWorkstation;
            }
            set
            {
                dataTableForGridLayoutLoadWorkstation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DataTableForGridLayoutLoadWorkstation"));
            }
        }

        public GridControl GridControlLoadWorkstation
        {
            get
            {
                return gridControlLoadWorkstation;
            }
            set
            {
                gridControlLoadWorkstation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GridControlLoadWorkstation"));
            }
        }

        public GridControl GridControlLoadWorkstationFilter
        {
            get
            {
                return gridControlLoadWorkstationFilter;
            }
            set
            {
                gridControlLoadWorkstationFilter = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GridControlLoadWorkstationFilter"));
            }
        }

        public ObservableCollection<Emdep.Geos.UI.Helper.Summary> TotalSummaryLoadWorkstation { get; private set; }
        public ObservableCollection<Emdep.Geos.UI.Helper.Summary> GroupSummaryLoadWorkstation { get; private set; }

        public List<DeliveryVisualManagementStages> LoadWorkstationStageList
        {
            get
            {
                return loadWorkstationStageList;
            }
            set
            {
                loadWorkstationStageList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LoadWorkstationStageList"));
            }
        }

        public ObservableCollection<Emdep.Geos.UI.Helper.Column> ColumnsLoadWorkstation
        {
            get { return columnsLoadWorkstation; }
            set
            {
                columnsLoadWorkstation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ColumnsLoadWorkstation"));
            }
        }

        #endregion [GEOS2-5039][Rupali Sarode][22-12-2023]

        #region [GEOS2-5038][Aishwarya Ingale][22-12-2023]


        public ObservableCollection<BandItem> BandsDashboardLoadCustomers
        {
            get { return bandsDashboardLoadCustomers; }
            set
            {
                bandsDashboardLoadCustomers = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BandsDashboardLoadCustomers"));
            }
        }

        public DataTable DataTableForGridLayoutLoadCustomers
        {
            get
            {
                return dataTableForGridLayoutLoadCustomers;
            }
            set
            {
                dataTableForGridLayoutLoadCustomers = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DataTableForGridLayoutLoadCustomers"));
            }
        }

        private List<PlantLoadAnalysis> groupByTempPlantLoadAnalysisCustomers;
        public List<PlantLoadAnalysis> GroupByTempPlantLoadAnalysisCustomers
        {
            get { return groupByTempPlantLoadAnalysisCustomers; }
            set
            {
                groupByTempPlantLoadAnalysisCustomers = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GroupByTempPlantLoadAnalysisCustomers"));
            }
        }

        private List<PlantLoadAnalysis> groupByTempPlantLoadAnalysisCustomers_Cloned;
        public List<PlantLoadAnalysis> GroupByTempPlantLoadAnalysisCustomers_Cloned
        {
            get
            {
                return groupByTempPlantLoadAnalysisCustomers_Cloned;
            }
            set
            {
                groupByTempPlantLoadAnalysisCustomers_Cloned = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GroupByTempPlantLoadAnalysisCustomers_Cloned"));
            }
        }
        private List<PlantLoadAnalysis> groupByTempPlantLoadAnalysisCustomersCopy;
        public List<PlantLoadAnalysis> GroupByTempPlantLoadAnalysisCustomersCopy
        {
            get { return groupByTempPlantLoadAnalysisCustomersCopy; }
            set
            {
                groupByTempPlantLoadAnalysisCustomersCopy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GroupByTempPlantLoadAnalysisCustomersCopy"));
            }
        }

        public GridControl GridControlLoadCustomers
        {
            get
            {
                return gridControlLoadCustomers;
            }
            set
            {
                gridControlLoadCustomers = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GridControlLoadCustomers"));
            }
        }

        public GridControl GridControlLoadCustomersFilter
        {
            get
            {
                return gridControlLoadCustomersFilter;
            }
            set
            {
                gridControlLoadCustomersFilter = value;
                OnPropertyChanged(new PropertyChangedEventArgs("gridControlLoadCustomersFilter"));
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

        public ObservableCollection<Emdep.Geos.UI.Helper.Column> ColumnsLoadCustomers
        {
            get { return columnsLoadCustomers; }
            set
            {
                columnsLoadCustomers = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ColumnsLoadCustomers"));
            }
        }


        public ObservableCollection<Emdep.Geos.UI.Helper.Summary> TotalSummaryLoadCustomers { get; private set; }
        public ObservableCollection<Emdep.Geos.UI.Helper.Summary> GroupSummaryLoadCustomers { get; private set; }

        #endregion [GEOS2-5038][Aishwarya Ingale][22-12-2023]



        #region Filter
        private List<PlantLoadAnalysis> regionList;
        public List<PlantLoadAnalysis> RegionList
        {
            get
            {
                return regionList;
            }

            set
            {
                regionList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RegionList"));
            }
        }

        private List<PlantLoadAnalysis> region_Cloned;
        public List<PlantLoadAnalysis> Region_Cloned
        {
            get
            {
                return region_Cloned;
            }

            set
            {
                region_Cloned = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Region_Cloned"));
            }
        }

        private List<PlantLoadAnalysis> customerGroupList;
        public List<PlantLoadAnalysis> CustomerGroupList
        {
            get
            {
                return customerGroupList;
            }

            set
            {
                customerGroupList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomerGroupList"));
            }
        }

        private List<PlantLoadAnalysis> customerPlantList;
        public List<PlantLoadAnalysis> CustomerPlantList
        {
            get
            {
                return customerPlantList;
            }

            set
            {
                customerPlantList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomerPlantList"));
            }
        }

        private List<PlantLoadAnalysis> customerGroup_Cloned;
        public List<PlantLoadAnalysis> CustomerGroup_Cloned
        {
            get
            {
                return customerGroup_Cloned;
            }

            set
            {
                customerGroup_Cloned = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomerGroup_Cloned"));
            }
        }

        private List<PlantLoadAnalysis> customerPlant_Cloned;
        public List<PlantLoadAnalysis> CustomerPlant_Cloned
        {
            get
            {
                return customerPlant_Cloned;
            }

            set
            {
                customerPlant_Cloned = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomerPlant_Cloned"));
            }
        }

        private List<PlantLoadAnalysis> oTCodeList;
        public List<PlantLoadAnalysis> OTCodeList
        {
            get
            {
                return oTCodeList;
            }

            set
            {
                oTCodeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OTCodeList"));
            }
        }
        private List<PlantLoadAnalysis> oTCodeList_Cloned;
        public List<PlantLoadAnalysis> OTCodeList_Cloned
        {
            get
            {
                return oTCodeList_Cloned;
            }

            set
            {
                oTCodeList_Cloned = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OTCodeList_Cloned"));
            }
        }
        private List<PlantLoadAnalysis> projectList;
        public List<PlantLoadAnalysis> ProjectList
        {
            get
            {
                return projectList;
            }

            set
            {
                projectList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProjectList"));
            }
        }

        private List<PlantLoadAnalysis> projectList_Cloned;
        public List<PlantLoadAnalysis> ProjectList_Cloned
        {
            get
            {
                return projectList_Cloned;
            }

            set
            {
                projectList_Cloned = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProjectList_Cloned"));
            }
        }

        private List<PlantLoadAnalysis> connectorFamilyList;
        public List<PlantLoadAnalysis> ConnectorFamilyList
        {
            get
            {
                return connectorFamilyList;
            }

            set
            {
                connectorFamilyList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ConnectorFamilyList"));
            }
        }

        private List<PlantLoadAnalysis> connectorFamilyList_Cloned;
        public List<PlantLoadAnalysis> ConnectorFamilyList_Cloned
        {
            get
            {
                return connectorFamilyList_Cloned;
            }

            set
            {
                connectorFamilyList_Cloned = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ConnectorFamilyList_Cloned"));
            }
        }


        private List<PlantLoadAnalysis> cPTypeList;
        public List<PlantLoadAnalysis> CPTypeList
        {
            get
            {
                return cPTypeList;
            }

            set
            {
                cPTypeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CPTypeList"));
            }
        }
        private List<PlantLoadAnalysis> cPTypeList_Cloned;
        public List<PlantLoadAnalysis> CPTypeList_Cloned
        {
            get
            {
                return cPTypeList_Cloned;
            }

            set
            {
                cPTypeList_Cloned = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CPTypeList_Cloned"));
            }
        }
        private List<PlantLoadAnalysis> templateList;
        public List<PlantLoadAnalysis> TemplateList
        {
            get
            {
                return templateList;
            }

            set
            {
                templateList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TemplateList"));
            }
        }

        private List<PlantLoadAnalysis> templateList_Cloned;
        public List<PlantLoadAnalysis> TemplateList_Cloned
        {
            get
            {
                return templateList_Cloned;
            }

            set
            {
                templateList_Cloned = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TemplateList_Cloned"));
            }
        }

        private List<PlantLoadAnalysis> oTItemStatusList;
        public List<PlantLoadAnalysis> OTItemStatusList
        {
            get
            {
                return oTItemStatusList;
            }

            set
            {
                oTItemStatusList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OTItemStatusList"));
            }
        }

        private List<PlantLoadAnalysis> oTItemStatusList_Cloned;
        public List<PlantLoadAnalysis> OTItemStatusList_Cloned
        {
            get
            {
                return oTItemStatusList_Cloned;
            }

            set
            {
                oTItemStatusList_Cloned = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OTItemStatusList_Cloned"));
            }
        }

        private List<object> selectedRegion;
        public List<object> SelectedRegion
        {
            get { return selectedRegion; }
            set
            {
                selectedRegion = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedRegion"));
                OnPropertyChanged(new PropertyChangedEventArgs("IsSelectedRegionNotEmpty"));//Aishwarya Ingale[Geos2-5749]
                UpdateColor();//Aishwarya Ingale[Geos2-5749]
            }
        }

        private List<object> selectedCustomerGroup;
        public List<object> SelectedCustomerGroup
        {
            get { return selectedCustomerGroup; }
            set
            {
                selectedCustomerGroup = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCustomerGroup"));
                OnPropertyChanged(new PropertyChangedEventArgs("IsSelectedCustomerGroupNotEmpty"));//Aishwarya Ingale[Geos2-5749]
                UpdateColor();//Aishwarya Ingale[Geos2-5749]
            }
        }

        private List<object> selectedCustomerPlant;
        public List<object> SelectedCustomerPlant
        {
            get { return selectedCustomerPlant; }
            set
            {
                selectedCustomerPlant = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCustomerPlant"));
                OnPropertyChanged(new PropertyChangedEventArgs("IsSelectedCustomerPlantsNotEmpty"));//Aishwarya Ingale[Geos2-5749]
                UpdateColor();//Aishwarya Ingale[Geos2-5749]
            }
        }
        private List<object> selectedOTCode;
        public List<object> SelectedOTCode
        {
            get { return selectedOTCode; }
            set
            {
                selectedOTCode = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedOTCode"));
                OnPropertyChanged(new PropertyChangedEventArgs("IsSelectedOTCodeNotEmpty"));
                UpdateColor();//Aishwarya Ingale[Geos2-5749]
            }
        }
        private List<object> selectedProject;
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
        private List<object> selectedConnectorFamily;
        public List<object> SelectedConnectorFamily
        {
            get { return selectedConnectorFamily; }
            set
            {
                selectedConnectorFamily = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedConnectorFamily"));
                OnPropertyChanged(new PropertyChangedEventArgs("IsSelectedConnectorFamilyNotEmpty"));//Aishwarya Ingale[Geos2-5749]
                UpdateColor();//Aishwarya Ingale[Geos2-5749]
            }
        }
        private List<object> selectedCPType;
        public List<object> SelectedCPType
        {
            get { return selectedCPType; }
            set
            {
                selectedCPType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCPType"));
                OnPropertyChanged(new PropertyChangedEventArgs("IsSelectedCPTypeNotEmpty"));//Aishwarya Ingale[Geos2-5749]
                UpdateColor();//Aishwarya Ingale[Geos2-5749]
            }
        }
        private List<object> selectedTemplate;
        public List<object> SelectedTemplate
        {
            get { return selectedTemplate; }
            set
            {
                selectedTemplate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedTemplate"));
                OnPropertyChanged(new PropertyChangedEventArgs("IsSelectedTemplateNotEmpty"));//Aishwarya Ingale[Geos2-5749]
                UpdateColor();//Aishwarya Ingale[Geos2-5749]
            }
        }
        private List<object> selectedOTItemStatus;
        public List<object> SelectedOTItemStatus
        {
            get { return selectedOTItemStatus; }
            set
            {
                selectedOTItemStatus = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedOTItemStatus"));
                OnPropertyChanged(new PropertyChangedEventArgs("IsSelectedOTItemStatusNotEmpty"));//Aishwarya Ingale[Geos2-5749]
                UpdateColor();//Aishwarya Ingale[Geos2-5749]
            }
        }
        #endregion

        private List<PlantLoadAnalysis> groupByTempPlantLoadAnalysisCopySummaryModule;
        public List<PlantLoadAnalysis> GroupByTempPlantLoadAnalysisCopySummaryModule
        {
            get { return groupByTempPlantLoadAnalysisCopySummaryModule; }
            set
            {
                groupByTempPlantLoadAnalysisCopySummaryModule = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GroupByTempPlantLoadAnalysisCopySummaryModule"));
            }
        }

        private List<PlantLoadAnalysis> groupByTempPlantLoadAnalysisCopySummaryEquipment;
        public List<PlantLoadAnalysis> GroupByTempPlantLoadAnalysisCopySummaryEquipment
        {
            get { return groupByTempPlantLoadAnalysisCopySummaryEquipment; }
            set
            {
                groupByTempPlantLoadAnalysisCopySummaryEquipment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GroupByTempPlantLoadAnalysisCopySummaryEquipment"));
            }
        }
        private bool isNotNull;
        public bool IsNotNull
        {
            get
            {
                return isNotNull;
            }
            set
            {
                isNotNull = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsNotNull"));
            }
        }

        #region Aishwarya Ingale[Geos2-5749]
        public bool IsSelectedRegionNotEmpty
        {
            get
            {
                return SelectedRegion != null && SelectedRegion.Count > 0 && SelectedRegion.Count != RegionList.Count;
            }

        }

        public bool IsSelectedTemplateNotEmpty
        {
            get
            {
                return SelectedTemplate != null && SelectedTemplate.Count > 0 && SelectedTemplate.Count != TemplateList.Count;
            }

        }

        public bool IsSelectedOTItemStatusNotEmpty
        {
            get
            {
                return SelectedOTItemStatus != null && SelectedOTItemStatus.Count > 0 && SelectedOTItemStatus.Count != OTItemStatusList.Count;
            }

        }

        public bool IsSelectedCustomerGroupNotEmpty
        {
            get
            {
                return SelectedCustomerGroup != null && SelectedCustomerGroup.Count > 0 && SelectedCustomerGroup.Count != CustomerGroupList.Count;
            }

        }

        public bool IsSelectedCustomerPlantsNotEmpty
        {
            get
            {
                return SelectedCustomerPlant != null && SelectedCustomerPlant.Count > 0 && SelectedCustomerPlant.Count != CustomerPlantList.Count;
            }

        }

        public bool IsSelectedProjectNotEmpty
        {
            get
            {
                return SelectedProject != null && SelectedProject.Count > 0 && SelectedProject.Count != ProjectList.Count;
            }

        }

        public bool IsSelectedOTCodeNotEmpty
        {
            get
            {
                return SelectedOTCode != null && SelectedOTCode.Count > 0 && SelectedOTCode.Count != OTCodeList.Count;
            }

        }

        public bool IsSelectedConnectorFamilyNotEmpty
        {
            get
            {
                return SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0 && SelectedConnectorFamily.Count != ConnectorFamilyList.Count;
            }

        }

        public bool IsSelectedCPTypeNotEmpty
        {
            get
            {
                return SelectedCPType != null && SelectedCPType.Count > 0 && SelectedCPType.Count != CPTypeList.Count;
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
            if (IsSelectedRegionNotEmpty || IsSelectedTemplateNotEmpty || IsSelectedOTItemStatusNotEmpty || IsSelectedCustomerGroupNotEmpty || IsSelectedCustomerPlantsNotEmpty|| IsSelectedProjectNotEmpty || IsSelectedOTCodeNotEmpty  || IsSelectedConnectorFamilyNotEmpty || IsSelectedCPTypeNotEmpty)
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

        #region  public Commands
        public ICommand ChangePlantCommand { get; set; }
        public ICommand PeriodCommand { get; set; }
        public ICommand ApplyCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand PeriodCustomRangeCommand { get; set; }
        public ICommand RefreshPlantLoadAnalysisCommand { get; set; }
        public ICommand GridActionLoadCommand { get; set; }
        public ICommand GridActionLoadEquipmentCommand { get; set; } // [GEOS2-5037][Rupali Sarode][1-2-2024]
        public ICommand GridActionLoadWorkstationCommand { get; set; } //[GEOS2-5039][Rupali Sarode][27-12-2023]

        public ICommand GridActionLoadCustomersCommand { get; set; } // [GEOS2-5038][Aishwarya Ingale][22-12-2023]


        #region Filter
        public ICommand ChangeRegionCommand { get; set; }
        public ICommand ChangeCustomerGroupCommand { get; set; }
        public ICommand ChangeCustomerPlantCommand { get; set; }
        public ICommand ChangeOTCodeCommand { get; set; }
        public ICommand ChangeProjectCommand { get; set; }
        public ICommand ChangeConnectorFamilyCommand { get; set; }
        public ICommand ChangeCPTypeCommand { get; set; }
        public ICommand ChangeTemplatesCommand { get; set; }
        public ICommand ChangeOTItemStatusCommand { get; set; }
        public ICommand PLAWorkstationGridControlUnloadedCommand { get; set; } //[GEOS2-5114][28-12-2023][Rupali Sarode]

        #endregion

        public ICommand ShowLoadModulesDialogWindowCommand { get; set; } //[GEOS2-5224][Rupali Sarode][24-01-2024]
        public ICommand ShowLoadEquipmentDialogWindowCommand { get; set; } //[GEOS2-5224][Rupali Sarode][24-01-2024]
        public ICommand ShowLoadCustomersDialogWindowCommand { get; set; } //[GEOS2-5224][Rupali Sarode][24-01-2024]
        public ICommand ShowLoadWorkStationDialogWindowCommand { get; set; } //[GEOS2-5224][Rupali Sarode][24-01-2024]

        #endregion

        #region Constructor

        public PlantLoadAnalysisViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor DashboardViewModel() ...", category: Category.Info, priority: Priority.Low);
                ApplyCommand = new DevExpress.Mvvm.DelegateCommand<object>(ApplyCommandAction);
                CancelCommand = new DevExpress.Mvvm.DelegateCommand<object>(CancelCommandAction);
                //  ChangePlantCommand = new DevExpress.Mvvm.DelegateCommand<object>(ChangePlantCommandAction);
                ChangePlantCommand = new DelegateCommand<object>(new Action<object>((obj) => { ChangePlantCommandAction(obj); }));
                PeriodCustomRangeCommand = new DevExpress.Mvvm.DelegateCommand<object>(PeriodCustomRangeCommandAction);
                PeriodCommand = new DevExpress.Mvvm.DelegateCommand<object>(PeriodCommandAction);
                RefreshPlantLoadAnalysisCommand = new DevExpress.Mvvm.DelegateCommand<object>(RefreshPlantDeliveryAnalysisAction);
                GridActionLoadCommand = new DevExpress.Mvvm.DelegateCommand<object>(GridActionLoadCommandAction);
                GridActionLoadEquipmentCommand = new DevExpress.Mvvm.DelegateCommand<object>(GridActionLoadEquipmentCommandAction);
                GridActionLoadWorkstationCommand = new DevExpress.Mvvm.DelegateCommand<object>(GridActionLoadWorkstationCommandAction);  //[GEOS2-5039][Rupali Sarode][27-12-2023]
                GridActionLoadCustomersCommand = new DevExpress.Mvvm.DelegateCommand<object>(GridActionLoadCustomersCommandAction);// [GEOS2-5038][Aishwarya Ingale][22-12-2023]
                #region Filter
                ChangeRegionCommand = new DevExpress.Mvvm.DelegateCommand<object>(ChangeRegionCommandAction);
                ChangeCustomerGroupCommand = new DevExpress.Mvvm.DelegateCommand<object>(ChangeCustomerGroupCommandAction);
                ChangeCustomerPlantCommand = new DevExpress.Mvvm.DelegateCommand<object>(ChangeCustomerPlantCommandAction);
                ChangeOTCodeCommand = new DevExpress.Mvvm.DelegateCommand<object>(ChangeOTCodeCommandAction);
                ChangeProjectCommand = new DevExpress.Mvvm.DelegateCommand<object>(ChangeProjectCommandAction);
                ChangeConnectorFamilyCommand = new DevExpress.Mvvm.DelegateCommand<object>(ChangeConnectorFamilyCommandAction);
                ChangeCPTypeCommand = new DevExpress.Mvvm.DelegateCommand<object>(ChangeCPTypeCommandAction);
                ChangeTemplatesCommand = new DevExpress.Mvvm.DelegateCommand<object>(ChangeTemplatesCommandAction);
                ChangeOTItemStatusCommand = new DevExpress.Mvvm.DelegateCommand<object>(ChangeOTItemStatusCommandAction);

                #endregion

                PLAWorkstationGridControlUnloadedCommand = new DevExpress.Mvvm.DelegateCommand<object>(PLAWorkstationGridControlUnloadedCommandAction); //[GEOS2-5114][28-12-2023][Rupali Sarode]

                ShowLoadModulesDialogWindowCommand = new DevExpress.Mvvm.DelegateCommand<object>(ShowLoadModulesDialogWindowCommandsAction);
                ShowLoadEquipmentDialogWindowCommand = new DevExpress.Mvvm.DelegateCommand<object>(ShowLoadEquipmentDialogWindowCommandsAction);
                ShowLoadCustomersDialogWindowCommand = new DevExpress.Mvvm.DelegateCommand<object>(ShowLoadCustomersDialogWindowCommandsAction);
                ShowLoadWorkStationDialogWindowCommand = new DevExpress.Mvvm.DelegateCommand<object>(ShowLoadWorkStationDialogWindowCommandsAction);

                GeosApplication.Instance.Logger.Log("Constructor DashboardViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Constructor DashboardViewModel()." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Method

        public void Init()
        {
            GeosApplication.Instance.Logger.Log("Method Init ...", category: Category.Info, priority: Priority.Low);
            try
            {

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                IsNotNull = true;
                StartDate = DateTime.Today.Date;
                EndDate = DateTime.Today.Date;
                setDefaultPeriod();
                FillLoadModulesAnalysis();
                AddColumnsToDataTableWithBandsinLoadModules();
                //  FillLoadModulesData();

                AddColumnsToDataTableWithBandsinLoadEquipments();//[GEOS2-5037][Rupali Sarode][21-12-2023]

                AddColumnsToDataTableWithBandsinLoadWorkstation(); //[GEOS2-5039][Rupali Sarode][27-12-2023]
                                                                   //  FillLoadWorkstationData(); //[GEOS2-5039][Rupali Sarode][27-12-2023]
                AddColumnsToDataTableWithBandsinLoadCustomers(); // [GEOS2-5038][Aishwarya Ingale][22-12-2023]
                                                                 //   FillLoadCustomerData(); // [GEOS2-5038][Aishwarya Ingale][22-12-2023]

                ReadXmlFile(); //[GEOS2-5114][Rupali Sarode][29-12-2023]

                FillFilter();
                FillAllselectedFilter();
                FillLoadModulesData();
                FillLoadEquipmentsData(); //[GEOS2-5037][Rupali Sarode][21-12-2023]
                GridControlLoadEquipmentFilter.GroupBy("CalendarWeekEquipment"); //[GEOS2-5037][Rupali Sarode][01-02-2024]
                FillLoadWorkstationData(); //[GEOS2-5039][Rupali Sarode][27-12-2023]
                FillLoadCustomerData(); // [GEOS2-5038][Aishwarya Ingale][22-12-2023]

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method Init() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init()", category: Category.Exception, priority: Priority.Low);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        private void setDefaultPeriod()
        {
            GeosApplication.Instance.Logger.Log("Method setDefaultPeriod ...", category: Category.Info, priority: Priority.Low);
            try
            {
                int year = DateTime.Now.Year;
                DateTime StartFromDate = new DateTime(year, 1, 1);
                DateTime EndToDate = new DateTime(year, 12, 31);
                //FromDate = StartFromDate.ToString("dd/MM/yyyy");
                //ToDate = EndToDate.ToString("dd/MM/yyyy");
                FromDate = StartFromDate;
                ToDate = EndToDate;
                GeosApplication.Instance.Logger.Log("Method setDefaultPeriod() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {

                GeosApplication.Instance.Logger.Log("Get an error in setDefaultPeriod()", category: Category.Exception, priority: Priority.Low);
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
            //  IsCalendarVisible = Visibility.Collapsed;

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
                    //FromDate = thisMonthStart.ToString("dd/MM/yyyy");
                    //ToDate = thisMonthEnd.ToString("dd/MM/yyyy");
                    FromDate = thisMonthStart;
                    ToDate = thisMonthEnd;
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
                    //FromDate = lastOneMonthStart.ToString("dd/MM/yyyy");
                    //ToDate = lastOneMonthEnd.ToString("dd/MM/yyyy");
                    FromDate = lastOneMonthStart;
                    ToDate = lastOneMonthEnd;
                }
                else if (IsButtonStatus == 3) //last month
                {
                    //FromDate = lastMonthStart.ToString("dd/MM/yyyy");
                    //ToDate = lastMonthEnd.ToString("dd/MM/yyyy");
                    FromDate = lastMonthStart;
                    ToDate = lastMonthEnd;
                }
                else if (IsButtonStatus == 4) //this week
                {
                    //FromDate = thisWeekStart.ToString("dd/MM/yyyy");
                    //ToDate = thisWeekEnd.ToString("dd/MM/yyyy");
                    FromDate = thisWeekStart;
                    ToDate = thisWeekEnd;
                }
                else if (IsButtonStatus == 5) //last one week
                {
                    //    FromDate = lastOneWeekStart.ToString("dd/MM/yyyy");
                    //    ToDate = lastOneWeekEnd.ToString("dd/MM/yyyy");
                    FromDate = thisWeekStart;
                    ToDate = thisWeekEnd;
                }
                else if (IsButtonStatus == 6) //last week
                {
                    //FromDate = lastWeekStart.ToString("dd/MM/yyyy");
                    //ToDate = lastWeekEnd.ToString("dd/MM/yyyy");
                    FromDate = lastWeekStart;
                    ToDate = lastWeekEnd;
                }
                else if (IsButtonStatus == 7) //custome range
                {
                    //FromDate = StartDate.ToString("dd/MM/yyyy");
                    //ToDate = EndDate.ToString("dd/MM/yyyy");
                    FromDate = StartDate;
                    ToDate = EndDate;
                }
                else if (IsButtonStatus == 8)//this year
                {
                    setDefaultPeriod();
                }
                else if (IsButtonStatus == 9)//last year
                {
                    //FromDate = StartFromDate.ToString("dd/MM/yyyy");
                    //ToDate = EndToDate.ToString("dd/MM/yyyy");
                    //FromDate = StartFromDate.ToString("dd/MM/yyyy");
                    //ToDate = EndToDate.ToString("dd/MM/yyyy");
                    FromDate = StartFromDate;
                    ToDate = EndToDate;
                }
                else if (IsButtonStatus == 10)//last 12 month
                {
                    DateTime Date_F = DateTime.Now.Date.AddMonths(-12);
                    DateTime Date_T = DateTime.Now.Date;
                    //FromDate = Date_F.ToString("dd/MM/yyyy");
                    //ToDate = Date_T.ToString("dd/MM/yyyy");
                    FromDate = Date_F;
                    ToDate = Date_T;

                }


                if (ERMCommon.Instance.SelectedAuthorizedPlantsList != null)
                {
                    GetPLAManagementProduction(ERMCommon.Instance.SelectedAuthorizedPlantsList, FromDate, ToDate);
                }
                IsNotNull = true;
                AddColumnsToDataTableWithBandsinLoadModules();
                //  FillLoadModulesData();
                //  GroupBY();
                AddColumnsToDataTableWithBandsinLoadEquipments();  //[GEOS2-5037][Rupali Sarode][01-02-2024]
                AddColumnsToDataTableWithBandsinLoadWorkstation(); //[GEOS2-5039][Rupali Sarode][27-12-2023]
                                                                   //   FillLoadWorkstationData(); //[GEOS2-5039][Rupali Sarode][27-12-2023]
                                                                   //   GroupBYWorkstation();  //[GEOS2-5039][Rupali Sarode][27-12-2023]

                AddColumnsToDataTableWithBandsinLoadCustomers(); // [GEOS2-5038][Aishwarya Ingale][22-12-2023]
                                                                 //   FillLoadCustomerData(); // [GEOS2-5038][Aishwarya Ingale][22-12-2023]

                // ReadXmlFile(); //[GEOS2-5114][Rupali Sarode][29-12-2023]
                FillFilter();

                SelectAllData();
                CreateXmlFile();
                ReadXmlFile(); //[GEOS2-5114][Rupali Sarode][29-12-2023]
                FillAllselectedFilter();

                FillLoadModulesData();
                GroupBY();
                FillLoadEquipmentsData(); //[GEOS2-5037][Rupali Sarode][21-12-2023]
                GridControlLoadEquipmentFilter.GroupBy("CalendarWeekEquipment"); //[GEOS2-5037][Rupali Sarode][01-02-2024]
                FillLoadWorkstationData(); //[GEOS2-5039][Rupali Sarode][27-12-2023]
                GroupBYWorkstation();  //[GEOS2-5039][Rupali Sarode][27-12-2023]
                FillLoadCustomerData(); // [GEOS2-5038][Aishwarya Ingale][22-12-2023]



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

        private void SelectAllData()
        {

            try
            {
                GeosApplication.Instance.Logger.Log("Constructor SelectAllData ...", category: Category.Info, priority: Priority.Low);

                SelectedRegion = new List<object>(RegionList.ToList());
                SelectedCustomerGroup = new List<object>(CustomerGroupList.ToList());
                SelectedCustomerPlant = new List<object>(CustomerPlantList.ToList());
                SelectedOTCode = new List<object>(OTCodeList.ToList());
                SelectedProject = new List<object>(ProjectList.ToList());
                SelectedConnectorFamily = new List<object>(ConnectorFamilyList.ToList());
                SelectedCPType = new List<object>(CPTypeList.ToList());
                SelectedTemplate = new List<object>(TemplateList.ToList());
                SelectedOTItemStatus = new List<object>(OTItemStatusList.ToList());

                GeosApplication.Instance.Logger.Log("Method SelectAllData....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SelectAllData() Method " + ex.Message, category: Category.Info, priority: Priority.Low);

            }
        }

        private void CancelCommandAction(object obj)
        {
            MenuFlyout menu = (MenuFlyout)obj;
            menu.IsOpen = false;
        }

        private void RefreshPlantDeliveryAnalysisAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshDashboardDetails ...", category: Category.Info, priority: Priority.Low);


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
                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned);
                plantLoadAnalysisList_Cloned = new List<PlantLoadAnalysis>();
                plantLoadAnalysisList_Cloned = PlantLoadAnalysisList.ToList();
                IsNotNull = true;
                AddColumnsToDataTableWithBandsinLoadModules();
                //FillLoadModulesData();
                //GroupBY();

                AddColumnsToDataTableWithBandsinLoadEquipments(); //[GEOS2-5037][Rupali Sarode][01-02-2024]
                AddColumnsToDataTableWithBandsinLoadWorkstation(); //[GEOS2-5039][Rupali Sarode][27-12-2023]
                //FillLoadWorkstationData(); //[GEOS2-5039][Rupali Sarode][27-12-2023]
                //GroupBYWorkstation();  //[GEOS2-5039][Rupali Sarode][27-12-2023]
                AddColumnsToDataTableWithBandsinLoadCustomers(); // [GEOS2-5038][Aishwarya Ingale][22-12-2023]

                // ReadXmlFile(); //[GEOS2-5114][Rupali Sarode][29-12-2023]
                FillFilter();

                //   SelectAllData();
                ReadXmlFile(); //[GEOS2-5114][Rupali Sarode][29-12-2023]

                FillAllselectedFilter();

                FillLoadModulesData();
                GroupBY();

                FillLoadEquipmentsData(); //[GEOS2-5037][Rupali Sarode][21-12-2023]
                GridControlLoadEquipmentFilter.GroupBy("CalendarWeekEquipment"); //[GEOS2-5037][Rupali Sarode][01-02-2024]
                FillLoadWorkstationData(); //[GEOS2-5039][Rupali Sarode][27-12-2023]
                GroupBYWorkstation();  //[GEOS2-5039][Rupali Sarode][27-12-2023]

                FillLoadCustomerData(); // [GEOS2-5038][Aishwarya Ingale][22-12-2023]

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

        #region Load-Modules

        private void FillLoadModulesAnalysis()
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Method FillLoadModulesAnalysis ...", category: Category.Info, priority: Priority.Low);
                if (ERMCommon.Instance.SelectedAuthorizedPlantsList != null)
                {
                    List<Site> plantOwners = ERMCommon.Instance.SelectedAuthorizedPlantsList.Cast<Site>().ToList();
                    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.Name));
                    if (ERMCommon.Instance.FailedPlants == null)
                    {
                        ERMCommon.Instance.FailedPlants = new List<string>();
                    }

                    FailedPlants = new List<string>();

                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>();
                    //[GEOS2-5039][Rupali Sarode][27-12-2023]
                    LoadWorkstationStageList = new List<DeliveryVisualManagementStages>();
                    List<DeliveryVisualManagementStages> TempLoadWorkstationStageList = new List<DeliveryVisualManagementStages>();

                    foreach (var itemPlantOwnerUsers in plantOwners)
                    {
                        try
                        {
                            string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.Name == itemPlantOwnerUsers.Name).Select(xy => xy.ServiceProviderUrl).FirstOrDefault();
                            ERMService = new ERMServiceController(serviceurl);
                            //

                            // ERMService = new ERMServiceController("localhost:6699");
                            GeosApplication.Instance.SplashScreenMessage = "Connecting to " + itemPlantOwnerUsers.Name;

                            Int32 IdSite = Convert.ToInt32(itemPlantOwnerUsers.IdSite);

                            var CurrencyNameFromSetting = String.Empty;

                            //[GEOS2-5037][Rupali Sarode][31-01-2023]
                            // PlantLoadAnalysisList.AddRange(ERMService.GetAllPlantLoadAnalysis_V2470( IdSite, FromDate,ToDate ));
                            // PlantLoadAnalysisList.AddRange(ERMService.GetAllPlantLoadAnalysis_V2480(IdSite, FromDate, ToDate));
                            PlantLoadAnalysisList.AddRange(ERMService.GetAllPlantLoadAnalysis_V2520(IdSite, FromDate, ToDate));


                            //[GEOS2-5039][Rupali Sarode][27-12-2023]
                            TempLoadWorkstationStageList.AddRange(ERMService.GetDVManagementRTMHRResourcesStage_V2440(IdSite));

                        }

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
                            GeosApplication.Instance.Logger.Log(string.Format("Error in method FillLoadModulesAnalysis() in filling list - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

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

                    LoadWorkstationStageList.AddRange(TempLoadWorkstationStageList.GroupBy(i => i.IdStage)
                                                                               .Select(grp => grp.First())
                                                                               .ToList().Distinct());

                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList);
                    plantLoadAnalysisList_Cloned = new List<PlantLoadAnalysis>();
                    plantLoadAnalysisList_Cloned = PlantLoadAnalysisList.ToList();


                    GeosApplication.Instance.SplashScreenMessage = string.Empty;
                    if (ERMCommon.Instance.FailedPlants == null || ERMCommon.Instance.FailedPlants.Count == 0)
                    {
                        ERMCommon.Instance.IsShowFailedPlantWarning = false;
                        ERMCommon.Instance.WarningFailedPlants = string.Empty;
                    }
                }

                GeosApplication.Instance.Logger.Log("Method FillLoadModulesAnalysis() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log("Get an error in FillLoadModulesAnalysis()", category: Category.Exception, priority: Priority.Low);
            }

        }

        private void AddColumnsToDataTableWithBandsinLoadModules()
        {
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTableWithBandsinLoadModules ...", category: Category.Info, priority: Priority.Low);

                if (FutureGridControlFilter == null)
                {
                    FutureGridControlFilter = new GridControl();
                }

                GridControlBand band = new GridControlBand
                {
                    Header = "Your Band Header",
                    Visible = false
                };
                GridColumn column = new GridColumn();
                BandsDashboard = new ObservableCollection<BandItem>(); BandsDashboard.Clear();
                BandItem band0 = new BandItem() { BandName = "FirstRow", BandHeader = "Plant", AllowBandMove = false, FixedStyle = FixedStyle.Left, Visible = true, AllowMoving = DevExpress.Utils.DefaultBoolean.False };
                band0.Columns = new ObservableCollection<ColumnItem>();
                band.Columns.Add(new DevExpress.Xpf.Grid.GridColumn() { FieldName = "CalenderWeek", Header = "", Width = 120, Visible = false });
                band.Columns.Add(new DevExpress.Xpf.Grid.GridColumn() { FieldName = "OTCode", Header = "", Width = 120, Visible = false });
                //band.Columns.Add(new DevExpress.Xpf.Grid.GridColumn() { FieldName = "CPType", Header = "", Width = 120, Visible = false });
                band0.Columns.Add(new ColumnItem() { ColumnFieldName = "CalenderWeek", HeaderText = "", Width = 120, IsVertical = false, PlantLoadAnalysisetting = PlantLoadAnalysisTemplateSelector.PlantLoadAnalysisSettingType.CalenderWeek, Visible = false });
                band0.Columns.Add(new ColumnItem() { ColumnFieldName = "OTCode", HeaderText = "", Width = 120, IsVertical = false, PlantLoadAnalysisetting = PlantLoadAnalysisTemplateSelector.PlantLoadAnalysisSettingType.OTCode, Visible = true });
                band0.Columns.Add(new ColumnItem() { ColumnFieldName = "CPType", HeaderText = "YearWeek", Width = 120, IsVertical = false, PlantLoadAnalysisetting = PlantLoadAnalysisTemplateSelector.PlantLoadAnalysisSettingType.CPType, Visible = false });
                // band0.Columns.Add(new ColumnItem() { ColumnFieldName = "OTCode", HeaderText = "", Width = 120, IsVertical = false, Settings = SettingsType.OTCode, Visible = true });

                BandsDashboard.Add(band0);
                GridSummaryItem GridSummaryItem = new GridSummaryItem();

                GroupSummary = new ObservableCollection<Emdep.Geos.UI.Helper.Summary>();
                TotalSummary = new ObservableCollection<Emdep.Geos.UI.Helper.Summary>();

                DataTableForGridLayoutProductionInTime = new DataTable();
                DataTableForGridLayoutProductionInTime.Columns.Add("CalenderWeek", typeof(string));
                DataTableForGridLayoutProductionInTime.Columns.Add("OTCode", typeof(string));
                DataTableForGridLayoutProductionInTime.Columns.Add("CPType", typeof(string));
                DataTableForGridLayoutProductionInTime.Columns.Add("GroupColor", typeof(bool));



                //if (GroupByTempPlantLoadAnalysis == null)
                //{
                GroupByTempPlantLoadAnalysisCopy = new List<PlantLoadAnalysis>();

                GroupByTempPlantLoadAnalysisCopySummaryModule = new List<PlantLoadAnalysis>();

                //   }
                if (GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
                {


                    FutureGridControlFilter.GroupSummary.Clear();
                    FutureGridControlFilter.TotalSummary.Clear();
                    List<Site> plantOwners = ERMCommon.Instance.SelectedAuthorizedPlantsList.Cast<Site>().ToList();
                    foreach (var SelectedPlant in plantOwners)
                    {

                        //if (IsNotNull)
                        //{
                        GridSummaryItem = new GridSummaryItem();
                        GroupByTempPlantLoadAnalysis = new List<PlantLoadAnalysis>();
                        // Outer BandItem
                        BandItem outerBand = new BandItem();



                        outerBand = new BandItem()
                        {
                            BandName = Convert.ToString(SelectedPlant.Name),
                            BandHeader = Convert.ToString(SelectedPlant.Name),
                            Visible = true
                        };

                        outerBand.Columns = new ObservableCollection<ColumnItem>();

                        BandsDashboard.Add(outerBand);


                        List<PlantLoadAnalysis> TempPlantLoadAnalysis = new List<PlantLoadAnalysis>();
                        TempPlantLoadAnalysis = PlantLoadAnalysisList.Where(i => i.ProductionIdSite == SelectedPlant.IdSite).ToList();
                        // var groupByTempPlantLoadAnalysis = TempPlantLoadAnalysis.GroupBy(a => a.OriginalIdSite).ToList();
                        GroupByTempPlantLoadAnalysis.AddRange(TempPlantLoadAnalysis.GroupBy(i => i.OriginalIdSite)
                                                                                 .Select(grp => grp.First())
                                                                                 .ToList().Distinct());

                        GroupByTempPlantLoadAnalysisCopy.AddRange(GroupByTempPlantLoadAnalysis);
                        //for (int i = 0; i < 5; i++)

                        foreach (PlantLoadAnalysis objOrigianlSite in GroupByTempPlantLoadAnalysis)
                        {
                            if (GroupByTempPlantLoadAnalysisCopySummaryModule.Where(i => i.OriginalIdSite == objOrigianlSite.OriginalIdSite).ToList().Count == 0)
                            {
                                GroupByTempPlantLoadAnalysisCopySummaryModule.Add(objOrigianlSite);
                            }
                        }

                        foreach (var item in GroupByTempPlantLoadAnalysis)
                        {


                            //if (IsNotNull)
                            //{

                            outerBand.Columns.Add(new ColumnItem() { ColumnFieldName = "Plant_" + item.OriginalIdSite + SelectedPlant.IdSite, HeaderText = item.OriginalSiteName.ToString(), Width = 120, Visible = true, IsVertical = false, PlantLoadAnalysisetting = PlantLoadAnalysisTemplateSelector.PlantLoadAnalysisSettingType.Plant });

                            DataTableForGridLayoutProductionInTime.Columns.Add("Plant_" + item.OriginalIdSite + SelectedPlant.IdSite, typeof(Int32));
                            //}
                            GroupSummary.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Sum, FieldName = "Plant_" + item.OriginalIdSite + SelectedPlant.IdSite, DisplayFormat = "{0}" });
                            TotalSummary.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Sum, FieldName = "Plant_" + item.OriginalIdSite + SelectedPlant.IdSite, DisplayFormat = "{0}" });

                            FutureGridControlFilter.GroupSummary.Add(new GridSummaryItem
                            {
                                FieldName = "Plant_" + item.OriginalIdSite + SelectedPlant.IdSite,
                                SummaryType = SummaryItemType.Sum,
                                DisplayFormat = "{0}"
                            });

                            FutureGridControlFilter.TotalSummary.Add(new GridSummaryItem
                            {
                                FieldName = "Plant_" + item.OriginalIdSite + SelectedPlant.IdSite,
                                SummaryType = SummaryItemType.Sum,
                                DisplayFormat = "{0}"
                            });
                        }

                        outerBand.Columns.Add(new ColumnItem() { ColumnFieldName = "Total" + SelectedPlant.IdSite, HeaderText = "Total", Width = 50, Visible = true, IsVertical = false, PlantLoadAnalysisetting = PlantLoadAnalysisTemplateSelector.PlantLoadAnalysisSettingType.Total });

                        DataTableForGridLayoutProductionInTime.Columns.Add("Total" + SelectedPlant.IdSite, typeof(Int32));




                        GroupSummary.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Sum, FieldName = "Total" + SelectedPlant.IdSite, DisplayFormat = "{0}" });
                        TotalSummary.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Sum, FieldName = "Total" + SelectedPlant.IdSite, DisplayFormat = "{0}" });


                        FutureGridControlFilter.TotalSummary.Add(new GridSummaryItem
                        {
                            FieldName = "Total" + SelectedPlant.IdSite,
                            SummaryType = SummaryItemType.Sum,
                            DisplayFormat = "{0}",

                        });

                        FutureGridControlFilter.GroupSummary.Add(new GridSummaryItem
                        {
                            FieldName = "Total" + SelectedPlant.IdSite,
                            SummaryType = SummaryItemType.Sum,
                            DisplayFormat = "{0}"
                        });
                        // }
                        //  }


                    }
                }
                BandItem TotaBand = new BandItem()
                {
                    BandName = "GrandTotal",
                    BandHeader = "Total",
                    Visible = true,
                    AllowBandMove = false,
                    FixedStyle = FixedStyle.Right,
                    Width = 70

                };
                TotaBand.Columns = new ObservableCollection<ColumnItem>();
                BandsDashboard.Add(TotaBand);
                TotaBand.Columns.Add(new ColumnItem() { ColumnFieldName = "GrandTotal", HeaderText = "", Width = 50, Visible = true, IsVertical = false, PlantLoadAnalysisetting = PlantLoadAnalysisTemplateSelector.PlantLoadAnalysisSettingType.Total });

                DataTableForGridLayoutProductionInTime.Columns.Add("GrandTotal", typeof(Int32));


                GroupSummary.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Sum, FieldName = "GrandTotal", DisplayFormat = "{0}" });
                TotalSummary.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Sum, FieldName = "GrandTotal", DisplayFormat = "{0}" });
                TotalSummary.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Count, FieldName = "CPType", DisplayFormat = "Total" });

                FutureGridControlFilter.TotalSummary.Add(new GridSummaryItem
                {
                    FieldName = "CPType",
                    SummaryType = SummaryItemType.Count,
                    DisplayFormat = "Total"
                });

                FutureGridControlFilter.TotalSummary.Add(new GridSummaryItem
                {
                    FieldName = "GrandTotal",
                    SummaryType = SummaryItemType.Sum,
                    DisplayFormat = "{0}"
                });

                FutureGridControlFilter.GroupSummary.Add(new GridSummaryItem
                {
                    FieldName = "GrandTotal",
                    SummaryType = SummaryItemType.Sum,
                    DisplayFormat = "{0}"
                });
                BandsDashboard = new ObservableCollection<BandItem>(BandsDashboard);

                FutureGridControlFilter.Bands.Add(band);


                GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTableWithBandsinLoadModules executed Successfully", category: Category.Info, priority: Priority.Low);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in AddColumnsToDataTableWithBandsinLoadModules() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        //private void FillLoadModulesData()
        //{
        //    try
        //    {
        //        if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
        //        GeosApplication.Instance.Logger.Log("Method FillLoadModulesData ...", category: Category.Info, priority: Priority.Low);
        //        DataTableForGridLayoutProductionInTime.Clear();
        //        DataTable DataTableForGridLayoutPlantLoadAnalysisCopy = DataTableForGridLayoutProductionInTime.Copy();
        //        // List<PlantLoadAnalysis> PlantLoadAnalysisListTemp = new List<PlantLoadAnalysis>();
        //        //var PlantLoadAnalysisListTemp = PlantLoadAnalysisList.GroupBy(a => a.IdOTItem).Select(group => group.ToList()).ToList();

        //        //// Flattening the grouped items
        //        //var flattenedList = PlantLoadAnalysisListTemp.SelectMany(group => group).ToList();

        //        //// Creating a new List<PlantLoadAnalysis>
        //        //PlantLoadAnalysisList = new List<PlantLoadAnalysis>(flattenedList);
        //        for (int i = 0; i < PlantLoadAnalysisList.Count; i++)
        //        {


        //            List<Site> plantOwners = ERMCommon.Instance.SelectedAuthorizedPlantsList.Cast<Site>().ToList();

        //            //foreach (var SelectedPlant in plantOwners)
        //            //{
        //                int GrandTotal = 0;
        //                int totalQTY = 0;
        //                //foreach (var item in groupbyWeek)
        //                //{
        //                try
        //                {



        //                    DataTable dataTable = DataTableForGridLayoutPlantLoadAnalysisCopy;



        //                        DataRow[] foundRows = dataTable.Select($"OTCode = '{PlantLoadAnalysisList[i]?.OTCode}' AND CalenderWeek = '{PlantLoadAnalysisList[i]?.DeliveryWeek}' AND CPType = '{PlantLoadAnalysisList[i]?.ConnectorFamily}'");

        //                var groupBY = PlantLoadAnalysisList.GroupBy(a => a.IdOTItem).First();

        //                // int groupedData = groupBY.Where(a => a.DeliveryWeek == PlantLoadAnalysisList[i].DeliveryWeek && a.OriginalIdSite == PlantLoadAnalysisList[i].OriginalIdSite && a.CPType == PlantLoadAnalysisList[i].CPType && a.OTCode == PlantLoadAnalysisList[i].OTCode).Sum(a => a.QTY);
        //                // int groupedData = PlantLoadAnalysisList.Where(a => a.DeliveryWeek == PlantLoadAnalysisList[i].DeliveryWeek && a.OriginalIdSite == PlantLoadAnalysisList[i].OriginalIdSite && a.CPType == PlantLoadAnalysisList[i].CPType && a.OTCode == PlantLoadAnalysisList[i].OTCode && a.OTItemStatus == PlantLoadAnalysisList[i].OTItemStatus).GroupBy(a => new { a.IdOTItem }).Sum(group => group.Sum(a => a.QTY)); 

        //                int groupedData = PlantLoadAnalysisList.Where(a => a.DeliveryWeek == PlantLoadAnalysisList[i].DeliveryWeek && a.OriginalIdSite == PlantLoadAnalysisList[i].OriginalIdSite && a.ConnectorFamily == PlantLoadAnalysisList[i].ConnectorFamily && a.OTCode == PlantLoadAnalysisList[i].OTCode).GroupBy(a => new { a.IdOTItem }).Sum(group => group.Sum(a => a.QTY));

        //                if (foundRows.Length > 0)
        //                        {

        //                            foundRows[0]["Plant_" + PlantLoadAnalysisList[i].OriginalIdSite + PlantLoadAnalysisList[i].ProductionIdSite] = groupedData;
        //                            totalQTY = totalQTY + groupedData;



        //                            foundRows[0]["Total" + PlantLoadAnalysisList[i].ProductionIdSite] = totalQTY;
        //                            GrandTotal = GrandTotal + totalQTY;
        //                            foundRows[0]["GrandTotal"] = GrandTotal;

        //                        }
        //                        else
        //                        {

        //                            DataRow dr = DataTableForGridLayoutPlantLoadAnalysisCopy.NewRow();
        //                            dr["CalenderWeek"] = PlantLoadAnalysisList[i].DeliveryWeek;
        //                            dr["OTCode"] = PlantLoadAnalysisList[i].OTCode;
        //                            dr["CPType"] = PlantLoadAnalysisList[i].ConnectorFamily;
        //                            dr["Plant_" + PlantLoadAnalysisList[i].OriginalIdSite + PlantLoadAnalysisList[i].ProductionIdSite] = groupedData;
        //                            totalQTY = totalQTY + groupedData;



        //                            dr["Total" + PlantLoadAnalysisList[i].ProductionIdSite] = totalQTY;
        //                            GrandTotal = GrandTotal + totalQTY;
        //                            dr["GrandTotal"] = GrandTotal;
        //                            DataTableForGridLayoutPlantLoadAnalysisCopy.Rows.Add(dr);
        //                        }
        //                }

        //                catch (Exception ex)
        //                {

        //                    throw;
        //                }
        //        }
        //        DataTableForGridLayoutProductionInTime = DataTableForGridLayoutPlantLoadAnalysisCopy;

        //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //    }
        //    catch (Exception ex)
        //    {
        //         if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //        throw;
        //    }
        //}

        #region [Rupali Sarode][For performance][08-02-2024]
        private void FillLoadModulesData()
        {
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                GeosApplication.Instance.Logger.Log("Method FillLoadModulesData ...", category: Category.Info, priority: Priority.Low);
                DataTableForGridLayoutProductionInTime.Clear();
                DataTable DataTableForGridLayoutPlantLoadAnalysisCopy = DataTableForGridLayoutProductionInTime.Copy();

                DataTable TempTable = DataTableForGridLayoutProductionInTime.Copy();

                // List<PlantLoadAnalysis> PlantLoadAnalysisListTemp = new List<PlantLoadAnalysis>();
                //var PlantLoadAnalysisListTemp = PlantLoadAnalysisList.GroupBy(a => a.IdOTItem).Select(group => group.ToList()).ToList();

                //// Flattening the grouped items
                //var flattenedList = PlantLoadAnalysisListTemp.SelectMany(group => group).ToList();

                //// Creating a new List<PlantLoadAnalysis>
                //PlantLoadAnalysisList = new List<PlantLoadAnalysis>(flattenedList);


                var groupedDataModule = PlantLoadAnalysisList
                       .GroupBy(item => new { item.OTCode, item.DeliveryWeek, item.OriginalIdSite, item.ConnectorFamily, item.ProductionIdSite })
                       .Select(group => new
                       {
                           OTCode = group.Key.OTCode,
                           DeliveryWeek = group.Key.DeliveryWeek,
                           OriginalIdSite = group.Key.OriginalIdSite,
                           ConnectorFamily = group.Key.ConnectorFamily,
                           ProductionIdSite = group.Key.ProductionIdSite
                           //, TotalQTY = group.Sum(item => item.QTY)
                       });

                DateTime currentDate = DateTime.Now;
                int currentWeek = (int)(currentDate.DayOfYear / 7) + 1;
                int year = DateTime.Now.Year;
                string Week = year + "CW" + currentWeek;
                List<int> ValueOfColumns = new List<int>();
                foreach (var TempGroup in groupedDataModule)
                {
                    int GrandTotal = 0;
                    int totalQTY = 0;

                    DataRow[] foundRows = DataTableForGridLayoutPlantLoadAnalysisCopy.Select($"OTCode = '{TempGroup?.OTCode}' AND CalenderWeek = '{TempGroup?.DeliveryWeek}' AND CPType = '{TempGroup?.ConnectorFamily}'");

                    var groupBY = PlantLoadAnalysisList.GroupBy(a => a.IdOTItem).First();

                    int groupedData = PlantLoadAnalysisList.Where(a => a.DeliveryWeek == TempGroup.DeliveryWeek && a.OriginalIdSite == TempGroup.OriginalIdSite && a.ConnectorFamily == TempGroup.ConnectorFamily && a.OTCode == TempGroup.OTCode).GroupBy(a => new { a.IdOTItem }).Sum(group => group.Sum(a => a.QTY));

                    if (foundRows.Length > 0)
                    {
                        foundRows[0]["Plant_" + TempGroup.OriginalIdSite + TempGroup.ProductionIdSite] = groupedData;
                        totalQTY = totalQTY + groupedData;

                        foundRows[0]["Total" + TempGroup.ProductionIdSite] = totalQTY;
                        GrandTotal = GrandTotal + totalQTY;
                        foundRows[0]["GrandTotal"] = GrandTotal;
                    }
                    else
                    {

                        DataRow dr = DataTableForGridLayoutPlantLoadAnalysisCopy.NewRow();
                        dr["CalenderWeek"] = TempGroup.DeliveryWeek;
                        dr["OTCode"] = TempGroup.OTCode;
                        dr["CPType"] = TempGroup.ConnectorFamily;
                        dr["Plant_" + TempGroup.OriginalIdSite + TempGroup.ProductionIdSite] = groupedData;
                        totalQTY = totalQTY + groupedData;

                        dr["Total" + TempGroup.ProductionIdSite] = totalQTY;
                        GrandTotal = GrandTotal + totalQTY;
                        dr["GrandTotal"] = GrandTotal;
                        dr["GroupColor"] = false;
                        if (TempGroup.DeliveryWeek == Week)
                        {
                            // string curWeek = rowsInCurrentWeek.Select(a => a.ToString()).FirstOrDefault();
                            //GroupColor = Week;
                            dr["GroupColor"] = true;
                        }
                        DataTableForGridLayoutPlantLoadAnalysisCopy.Rows.Add(dr);
                    }

                    //foreach (DataRow row in DataTableForGridLayoutPlantLoadAnalysisCopy.Rows)
                    //{
                    //    var value = row["Plant_" + TempGroup.OriginalIdSite + TempGroup.ProductionIdSite];
                    //    ValueOfColumns.Add(Convert.ToInt32(value));
                    //}
                }


                //for (int i = 0; i < PlantLoadAnalysisList.Count; i++)
                //{


                //    List<Site> plantOwners = ERMCommon.Instance.SelectedAuthorizedPlantsList.Cast<Site>().ToList();

                //    //foreach (var SelectedPlant in plantOwners)
                //    //{
                //    int GrandTotal = 0;
                //    int totalQTY = 0;
                //    //foreach (var item in groupbyWeek)
                //    //{
                //    try
                //    {



                //        DataTable dataTable = DataTableForGridLayoutPlantLoadAnalysisCopy;



                //        DataRow[] foundRows = dataTable.Select($"OTCode = '{PlantLoadAnalysisList[i]?.OTCode}' AND CalenderWeek = '{PlantLoadAnalysisList[i]?.DeliveryWeek}' AND CPType = '{PlantLoadAnalysisList[i]?.ConnectorFamily}'");

                //        var groupBY = PlantLoadAnalysisList.GroupBy(a => a.IdOTItem).First();

                //        // int groupedData = groupBY.Where(a => a.DeliveryWeek == PlantLoadAnalysisList[i].DeliveryWeek && a.OriginalIdSite == PlantLoadAnalysisList[i].OriginalIdSite && a.CPType == PlantLoadAnalysisList[i].CPType && a.OTCode == PlantLoadAnalysisList[i].OTCode).Sum(a => a.QTY);
                //        // int groupedData = PlantLoadAnalysisList.Where(a => a.DeliveryWeek == PlantLoadAnalysisList[i].DeliveryWeek && a.OriginalIdSite == PlantLoadAnalysisList[i].OriginalIdSite && a.CPType == PlantLoadAnalysisList[i].CPType && a.OTCode == PlantLoadAnalysisList[i].OTCode && a.OTItemStatus == PlantLoadAnalysisList[i].OTItemStatus).GroupBy(a => new { a.IdOTItem }).Sum(group => group.Sum(a => a.QTY)); 

                //        int groupedData = PlantLoadAnalysisList.Where(a => a.DeliveryWeek == PlantLoadAnalysisList[i].DeliveryWeek && a.OriginalIdSite == PlantLoadAnalysisList[i].OriginalIdSite && a.ConnectorFamily == PlantLoadAnalysisList[i].ConnectorFamily && a.OTCode == PlantLoadAnalysisList[i].OTCode).GroupBy(a => new { a.IdOTItem }).Sum(group => group.Sum(a => a.QTY));

                //        if (foundRows.Length > 0)
                //        {

                //            foundRows[0]["Plant_" + PlantLoadAnalysisList[i].OriginalIdSite + PlantLoadAnalysisList[i].ProductionIdSite] = groupedData;
                //            totalQTY = totalQTY + groupedData;



                //            foundRows[0]["Total" + PlantLoadAnalysisList[i].ProductionIdSite] = totalQTY;
                //            GrandTotal = GrandTotal + totalQTY;
                //            foundRows[0]["GrandTotal"] = GrandTotal;

                //        }
                //        else
                //        {

                //            DataRow dr = DataTableForGridLayoutPlantLoadAnalysisCopy.NewRow();
                //            dr["CalenderWeek"] = PlantLoadAnalysisList[i].DeliveryWeek;
                //            dr["OTCode"] = PlantLoadAnalysisList[i].OTCode;
                //            dr["CPType"] = PlantLoadAnalysisList[i].ConnectorFamily;
                //            dr["Plant_" + PlantLoadAnalysisList[i].OriginalIdSite + PlantLoadAnalysisList[i].ProductionIdSite] = groupedData;
                //            totalQTY = totalQTY + groupedData;



                //            dr["Total" + PlantLoadAnalysisList[i].ProductionIdSite] = totalQTY;
                //            GrandTotal = GrandTotal + totalQTY;
                //            dr["GrandTotal"] = GrandTotal;
                //            DataTableForGridLayoutPlantLoadAnalysisCopy.Rows.Add(dr);
                //        }
                //    }

                //    catch (Exception ex)
                //    {

                //        throw;
                //    
                //}
                List<string> columnNames = DataTableForGridLayoutPlantLoadAnalysisCopy.Columns.Cast<DataColumn>().Select(column => column.ColumnName).ToList();

                var columnsToRemove = DataTableForGridLayoutPlantLoadAnalysisCopy.Columns.Cast<DataColumn>()
           .Where(col => DataTableForGridLayoutPlantLoadAnalysisCopy.AsEnumerable().All(row => row.IsNull(col)))
           .ToList();


                foreach (var col in columnsToRemove)
                {
                    if (!col.ColumnName.Contains("Total"))
                    {

                       // int colIndex = DataTableForGridLayoutPlantLoadAnalysisCopy.Columns.IndexOf(col);

                        DataTableForGridLayoutPlantLoadAnalysisCopy.Columns.Remove(col);
                      //  DataTableForGridLayoutPlantLoadAnalysisCopy.Columns.RemoveAt(colIndex);
                    }
                    //     DataTableForGridLayoutPlantLoadAnalysisCopy.Columns.RemoveAt(0);
                }
                if (columnsToRemove.Count>0)
                {
                    AddColumnsToDataTableWithBandsinLoadModules();
                }

                DataTableForGridLayoutProductionInTime = DataTableForGridLayoutPlantLoadAnalysisCopy;

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method FillLoadModulesData()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillLoadModulesData() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                throw;
            }
        }

        private void RemoveColumn(string idSite)
        {
            try
            {
                var columnsToRemove = DataTableForGridLayoutProductionInTime.Columns.Cast<DataColumn>()
           .Where(col => DataTableForGridLayoutProductionInTime.AsEnumerable().All(row => row.IsNull(col)) && col.ColumnName.Contains(idSite))
           .ToList();
                IsNotNull = true;
                if (columnsToRemove.Count > 0)
                {
                    IsNotNull = false;
                    //  AddColumnsToDataTableWithBandsinLoadModules();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        private void ChangePlantCommandAction(object obj)
        {
            try
            {
                var value = (object[])obj;

                FutureGridControlFilter = (DevExpress.Xpf.Grid.GridControl)value[1];

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                if (obj == null)
                {


                }
                else
                {

                    DevExpress.Xpf.Editors.ComboBoxEdit CloseModetemp = (DevExpress.Xpf.Editors.ComboBoxEdit)value[0];
                    GeosApplication.Instance.Logger.Log("Method ChangePlantCommandAction()...", category: Category.Info, priority: Priority.Low);

                    var TempSelectedPlant = (((DevExpress.Xpf.Editors.ComboBoxEdit)value[0]).EditValue);
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
                            GetPLAManagementProduction(ERMCommon.Instance.SelectedAuthorizedPlantsList, FromDate, ToDate);
                            IsNotNull = true;
                            AddColumnsToDataTableWithBandsinLoadModules();

                            //FillLoadModulesData();
                            //GroupBY();
                            AddColumnsToDataTableWithBandsinLoadEquipments(); //[GEOS2-5037][Rupali Sarode][01-02-2024]
                            AddColumnsToDataTableWithBandsinLoadWorkstation(); //[GEOS2-5039][Rupali Sarode][27-12-2023]
                            //FillLoadWorkstationData(); //[GEOS2-5039][Rupali Sarode][27-12-2023]
                            //GroupBYWorkstation();  //[GEOS2-5039][Rupali Sarode][27-12-2023]
                            AddColumnsToDataTableWithBandsinLoadCustomers();// [GEOS2-5038][Aishwarya Ingale][22-12-2023]
                                                                            //FillLoadCustomerData();// [GEOS2-5038][Aishwarya Ingale][22-12-2023]

                            // ReadXmlFile(); //[GEOS2-5114][Rupali Sarode][29-12-2023]
                            FillFilter();
                            SelectAllData();

                            CreateXmlFile();
                            ReadXmlFile(); //[GEOS2-5114][Rupali Sarode][29-12-2023]

                            FillAllselectedFilter();

                            FillLoadModulesData();
                            GroupBY();
                            FillLoadEquipmentsData(); //[GEOS2-5037][Rupali Sarode][01-02-2024]
                            GridControlLoadEquipmentFilter.GroupBy("CalendarWeekEquipment");  //[GEOS2-5037][Rupali Sarode][01-02-2024]
                            FillLoadWorkstationData(); //[GEOS2-5039][Rupali Sarode][27-12-2023]
                            GroupBYWorkstation();  //[GEOS2-5039][Rupali Sarode][27-12-2023]
                            FillLoadCustomerData();// [GEOS2-5038][Aishwarya Ingale][22-12-2023]
                            List<Site> plantOwners = ERMCommon.Instance.SelectedAuthorizedPlantsList.Cast<Site>().ToList();
                            //foreach (var selectedplant in plantOwners)
                            //{
                            //                  var columnsToRemove = DataTableForGridLayoutProductionInTime.Columns.Cast<DataColumn>()
                            //.Where(col => DataTableForGridLayoutProductionInTime.AsEnumerable().All(row => row.IsNull(col)) && col.ColumnName.Contains(Convert.ToString(selectedplant.IdSite)))
                            //.ToList();
                            //                  if (columnsToRemove.Count>0)
                            //                  {
                            //                      foreach (var col in columnsToRemove)
                            //                      {
                            //                          DataTableForGridLayoutProductionInTime.Columns.Remove(col);
                            //                          //DataTableForGridLayoutProductionInTime.Columns.RemoveAt(0);
                            //                      }
                            //                    //  return;
                            //                  }

                            // }
                            FutureGridControlFilter.RefreshData();
                            FutureGridControlFilter.UpdateLayout(); }
                        catch (FaultException<ServiceException> ex)
                        {
                        }
                    }
                    else
                    {

                    }


                    //}
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

        private void GetPLAManagementProduction(List<object> SelectedPlant, DateTime FromDate, DateTime ToDate)
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
                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>();

                    LoadWorkstationStageList = new List<DeliveryVisualManagementStages>();
                    List<DeliveryVisualManagementStages> TempLoadWorkstationStageList = new List<DeliveryVisualManagementStages>();

                    foreach (var itemPlantOwnerUsers in plantOwners)
                    {
                        try
                        {
                            string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.Name == itemPlantOwnerUsers.Name).Select(xy => xy.ServiceProviderUrl).FirstOrDefault();
                            ERMService = new ERMServiceController(serviceurl);
                            // ERMService = new ERMServiceController("localhost:6699");
                            GeosApplication.Instance.SplashScreenMessage = "Connecting to " + itemPlantOwnerUsers.Name;

                            Int32 IdSite = Convert.ToInt32(itemPlantOwnerUsers.IdSite);


                            var CurrencyNameFromSetting = String.Empty;

                            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("ERM_SelectedCurrency"))
                            {
                                CurrencyNameFromSetting = GeosApplication.Instance.UserSettings["ERM_SelectedCurrency"];
                            }

                            //[GEOS2-5037][Rupali Sarode][31-01-2023]
                            // PlantLoadAnalysisList.AddRange(ERMService.GetAllPlantLoadAnalysis_V2470(IdSite, FromDate,ToDate) );
                            // PlantLoadAnalysisList.AddRange(ERMService.GetAllPlantLoadAnalysis_V2480(IdSite, FromDate, ToDate));
                            PlantLoadAnalysisList.AddRange(ERMService.GetAllPlantLoadAnalysis_V2520(IdSite, FromDate, ToDate));

                            TempLoadWorkstationStageList.AddRange(ERMService.GetDVManagementRTMHRResourcesStage_V2440(IdSite));

                        }

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

                    // Get distinct data from list
                    LoadWorkstationStageList.AddRange(TempLoadWorkstationStageList.GroupBy(i => i.IdStage)
                                                                                 .Select(grp => grp.First())
                                                                                 .ToList().Distinct());


                    GeosApplication.Instance.SplashScreenMessage = string.Empty;
                    if (FailedPlants == null || FailedPlants.Count == 0)
                    {
                        IsShowFailedPlantWarning = false;
                        WarningFailedPlants = string.Empty;
                    }
                    plantLoadAnalysisList_Cloned = new List<PlantLoadAnalysis>();
                    plantLoadAnalysisList_Cloned = PlantLoadAnalysisList.ToList();


                }
                GeosApplication.Instance.Logger.Log("Method GetDVManagementProduction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log("Get an error in GetDVManagementProduction()", category: Category.Exception, priority: Priority.Low);
            }

        }


        private void GridActionLoadCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GridActionLoadCommandAction()...", category: Category.Info, priority: Priority.Low);

                FutureGridControl = (GridControl)obj;

                FutureGridControlFilter = FutureGridControl;
                FutureGridControl.GroupBy("CalenderWeek");
                FutureGridControl.GroupBy("OTCode");


                GeosApplication.Instance.Logger.Log("Method GridActionLoadCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GridActionLoadCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void GroupBY()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GroupBY()...", category: Category.Info, priority: Priority.Low);
                FutureGridControlFilter.GroupBy("CalenderWeek");
                FutureGridControlFilter.GroupBy("OTCode");
                GeosApplication.Instance.Logger.Log("Method GroupBY()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {

                GeosApplication.Instance.Logger.Log("Get an error in GroupBY() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion


        #region Load-Equipments

        //private void AddColumnsToDataTableWithBandsinLoadEquipments()
        //{
        //    try
        //    {
        //        if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
        //        GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTableWithBandsinLoadEquipments ...", category: Category.Info, priority: Priority.Low);

        //        //   CategoryColumns = new List<string>();
        //        // BandsDashboardLoadEquipment
        //        BandsDashboardLoadEquipment = new ObservableCollection<BandItem>(); BandsDashboardLoadEquipment.Clear();
        //        BandItem band0 = new BandItem() { BandName = "FirstRow", BandHeader = "Plant", AllowBandMove = false, FixedStyle = FixedStyle.Left, Visible = true, AllowMoving = DevExpress.Utils.DefaultBoolean.False };
        //        band0.Columns = new ObservableCollection<ColumnItem>();

        //        band0.Columns.Add(new ColumnItem() { ColumnFieldName = "CalenderWeek", HeaderText = "", Width = 120, IsVertical = false, PlantLoadAnalysisetting = PlantLoadAnalysisTemplateSelector.PlantLoadAnalysisSettingType.CalenderWeek, Visible = true });
        //        // band0.Columns.Add(new ColumnItem() { ColumnFieldName = "OTCode", HeaderText = "OTCode", Width = 120, IsVertical = false, PlantLoadAnalysisetting = PlantLoadAnalysisTemplateSelector.PlantLoadAnalysisSettingType.OTCodeTemplate, Visible = true });
        //        band0.Columns.Add(new ColumnItem() { ColumnFieldName = "CPType", HeaderText = "Year Week", Width = 120, IsVertical = false, PlantLoadAnalysisetting = PlantLoadAnalysisTemplateSelector.PlantLoadAnalysisSettingType.CPType, Visible = true });
        //        band0.Columns.Add(new ColumnItem() { ColumnFieldName = "OTCode", HeaderText = "", Width = 120, IsVertical = false, Settings = SettingsType.OTCode, Visible = true });

        //        BandsDashboardLoadEquipment.Add(band0);
        //        //DataTableForGridLayoutLoadEquipment
        //        DataTableForGridLayoutLoadEquipment = new DataTable();
        //        DataTableForGridLayoutLoadEquipment.Columns.Add("CalenderWeek", typeof(string));
        //        DataTableForGridLayoutLoadEquipment.Columns.Add("OTCode", typeof(string));
        //        DataTableForGridLayoutLoadEquipment.Columns.Add("CPType", typeof(string));

        //        GroupByTempPlantLoadAnalysisEquipmentCopy = new List<PlantLoadAnalysis>();
        //        GroupSummaryLoadEquipment = new ObservableCollection<Emdep.Geos.UI.Helper.Summary>();
        //        TotalSummaryLoadEquipment = new ObservableCollection<Emdep.Geos.UI.Helper.Summary>();

        //        if (GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
        //        {
        //            List<Site> plantOwners = ERMCommon.Instance.SelectedAuthorizedPlantsList.Cast<Site>().ToList();
        //            foreach (var SelectedPlant in plantOwners)
        //            {
        //                GroupByTempPlantLoadAnalysisEquipment = new List<PlantLoadAnalysis>();

        //                // Outer BandItem
        //                BandItem outerBand = new BandItem()
        //                {
        //                    BandName = Convert.ToString(SelectedPlant.Name),
        //                    BandHeader = Convert.ToString(SelectedPlant.Name),
        //                    Visible = true
        //                };

        //                outerBand.Columns = new ObservableCollection<ColumnItem>();
        //                BandsDashboardLoadEquipment.Add(outerBand);


        //                List<PlantLoadAnalysis> TempPlantLoadAnalysis = new List<PlantLoadAnalysis>();
        //                TempPlantLoadAnalysis = PlantLoadAnalysisList.Where(i => i.ProductionIdSite == SelectedPlant.IdSite).ToList();
        //                // var groupByTempPlantLoadAnalysis = TempPlantLoadAnalysis.GroupBy(a => a.OriginalIdSite).ToList();
        //                GroupByTempPlantLoadAnalysisEquipment.AddRange(TempPlantLoadAnalysis.GroupBy(i => i.OriginalIdSite)
        //                                                                         .Select(grp => grp.First())
        //                                                                         .ToList().Distinct());

        //                GroupByTempPlantLoadAnalysisEquipmentCopy.AddRange(GroupByTempPlantLoadAnalysisEquipment);

        //                //for (int i = 0; i < 5; i++)
        //                string PlantName = string.Empty;

        //                foreach (var item in GroupByTempPlantLoadAnalysisEquipment)
        //                {
        //                    PlantName = "Plant_" + item.OriginalIdSite + "_" + SelectedPlant.IdSite;
        //                    outerBand.Columns.Add(new ColumnItem() { ColumnFieldName = "Plant_" + item.OriginalIdSite + "_" + SelectedPlant.IdSite, HeaderText = item.OriginalSiteName.ToString(), Width = 70, Visible = true, IsVertical = false, PlantLoadAnalysisetting = PlantLoadAnalysisTemplateSelector.PlantLoadAnalysisSettingType.Plant });

        //                    DataTableForGridLayoutLoadEquipment.Columns.Add("Plant_" + item.OriginalIdSite + "_" + SelectedPlant.IdSite, typeof(string));

        //                    GroupSummaryLoadEquipment.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Sum, FieldName = PlantName, DisplayFormat = "{0}" });
        //                    TotalSummaryLoadEquipment.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Sum, FieldName = PlantName, DisplayFormat = "{0}" });


        //                }
        //                outerBand.Columns.Add(new ColumnItem() { ColumnFieldName = "Total" + SelectedPlant.IdSite, HeaderText = "Total", Width = 70, Visible = true, IsVertical = false, PlantLoadAnalysisetting = PlantLoadAnalysisTemplateSelector.PlantLoadAnalysisSettingType.Total });
        //                DataTableForGridLayoutLoadEquipment.Columns.Add("Total" + SelectedPlant.IdSite, typeof(string));

        //                GroupSummaryLoadEquipment.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Sum, FieldName = "Total" + SelectedPlant.IdSite, DisplayFormat = "{0}" });
        //                TotalSummaryLoadEquipment.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Sum, FieldName = "Total" + SelectedPlant.IdSite, DisplayFormat = "{0}" });

        //            }


        //        }
        //        BandItem TotalBand = new BandItem()
        //        {
        //            BandName = "GrandTotal",
        //            BandHeader = "Total",
        //            Visible = true,
        //            AllowBandMove = false,
        //            FixedStyle = FixedStyle.Right
        //        };
        //        TotalBand.Columns = new ObservableCollection<ColumnItem>();
        //        BandsDashboardLoadEquipment.Add(TotalBand);
        //        TotalBand.Columns.Add(new ColumnItem() { ColumnFieldName = "GrandTotal", HeaderText = "", Width = 70, Visible = true, IsVertical = false, PlantLoadAnalysisetting = PlantLoadAnalysisTemplateSelector.PlantLoadAnalysisSettingType.Total });
        //        DataTableForGridLayoutLoadEquipment.Columns.Add("GrandTotal", typeof(Int32));

        //        GroupSummaryLoadEquipment.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Sum, FieldName = "GrandTotal", DisplayFormat = "{0}" });
        //        TotalSummaryLoadEquipment.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Sum, FieldName = "GrandTotal", DisplayFormat = "{0}" });

        //        TotalSummaryLoadEquipment.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Count, FieldName = "CPType", DisplayFormat = "Total" });

        //        BandsDashboardLoadEquipment = new ObservableCollection<BandItem>(BandsDashboardLoadEquipment);

        //        GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTableWithBandsinLoadEquipments executed Successfully", category: Category.Info, priority: Priority.Low);
        //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Error in AddColumnsToDataTableWithBandsinLoadEquipments() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}

        private void AddColumnsToDataTableWithBandsinLoadEquipments()
        {
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTableWithBandsinLoadEquipments ...", category: Category.Info, priority: Priority.Low);

                if (GridControlLoadEquipmentFilter == null)
                {
                    GridControlLoadEquipmentFilter = new GridControl();
                }

                GridControlBand band = new GridControlBand
                {
                    Header = "Your Band Header",
                    Visible = false
                };
                GridColumn column = new GridColumn();
                BandsDashboardLoadEquipment = new ObservableCollection<BandItem>(); BandsDashboardLoadEquipment.Clear();
                BandItem band0 = new BandItem() { BandName = "FirstRow", BandHeader = "Plant", AllowBandMove = false, FixedStyle = FixedStyle.Left, Visible = true, AllowMoving = DevExpress.Utils.DefaultBoolean.False };

                band0.Columns = new ObservableCollection<ColumnItem>();
                // band.Columns.Add(new DevExpress.Xpf.Grid.GridColumn() { FieldName = "CalenderWeek", Header = "", Width = 120, Visible = false });
                band.Columns.Add(new DevExpress.Xpf.Grid.GridColumn() { FieldName = "CalendarWeekEquipment", Header = "", Width = 120, Visible = false });

                band.Columns.Add(new DevExpress.Xpf.Grid.GridColumn() { FieldName = "otCodeEquipment", Header = "", Width = 120, Visible = false });
                //band.Columns.Add(new DevExpress.Xpf.Grid.GridColumn() { FieldName = "CPType", Header = "", Width = 120, Visible = false });

                //  band0.Columns.Add(new ColumnItem() { ColumnFieldName = "CalenderWeek", HeaderText = "Equipment", Width = 120, IsVertical = false, PlantLoadAnalysisetting = PlantLoadAnalysisTemplateSelector.PlantLoadAnalysisSettingType.CalenderWeek, Visible = false });
                band0.Columns.Add(new ColumnItem() { ColumnFieldName = "CalendarWeekEquipment", HeaderText = "", Width = 120, IsVertical = false, PlantLoadAnalysisetting = PlantLoadAnalysisTemplateSelector.PlantLoadAnalysisSettingType.CalendarWeekEquipment, Visible = true });

                band0.Columns.Add(new ColumnItem() { ColumnFieldName = "otCodeEquipment", HeaderText = "CODE", Width = 120, IsVertical = false, PlantLoadAnalysisetting = PlantLoadAnalysisTemplateSelector.PlantLoadAnalysisSettingType.otCodeEquipment, Visible = true });
                //  band0.Columns.Add(new ColumnItem() { ColumnFieldName = "CPType", HeaderText = "YearWeek", Width = 120, IsVertical = false, PlantLoadAnalysisetting = PlantLoadAnalysisTemplateSelector.PlantLoadAnalysisSettingType.CPType, Visible = true });
                // band0.Columns.Add(new ColumnItem() { ColumnFieldName = "OTCode", HeaderText = "", Width = 120, IsVertical = false, Settings = SettingsType.OTCode, Visible = true });

                BandsDashboardLoadEquipment.Add(band0);
                GridSummaryItem GridSummaryItem = new GridSummaryItem();

                GroupSummaryLoadEquipment = new ObservableCollection<Emdep.Geos.UI.Helper.Summary>();
                TotalSummaryLoadEquipment = new ObservableCollection<Emdep.Geos.UI.Helper.Summary>();

                DataTableForGridLayoutLoadEquipment = new DataTable();
                DataTableForGridLayoutLoadEquipment.Columns.Add("CalendarWeekEquipment", typeof(string));
                DataTableForGridLayoutLoadEquipment.Columns.Add("otCodeEquipment", typeof(string));
                DataTableForGridLayoutLoadEquipment.Columns.Add("GroupColor", typeof(bool));

                //  DataTableForGridLayoutLoadEquipment.Columns.Add("CPType", typeof(string));

                //if (GroupByTempPlantLoadAnalysis == null)
                //{
                // GroupByTempPlantLoadAnalysisCopy = new List<PlantLoadAnalysis>();
                //   }
                if (GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
                {


                    GridControlLoadEquipmentFilter.GroupSummary.Clear();
                    GridControlLoadEquipmentFilter.TotalSummary.Clear();

                    GroupByTempPlantLoadAnalysisCopySummaryEquipment = new List<PlantLoadAnalysis>();

                    List<Site> plantOwners = ERMCommon.Instance.SelectedAuthorizedPlantsList.Cast<Site>().ToList();
                    foreach (var SelectedPlant in plantOwners)
                    {

                        //  GridSummaryItem = new GridSummaryItem();
                        GroupByTempPlantLoadAnalysisEquipment = new List<PlantLoadAnalysis>();
                        // Outer BandItem
                        BandItem outerBand = new BandItem()
                        {
                            BandName = Convert.ToString(SelectedPlant.Name),
                            BandHeader = Convert.ToString(SelectedPlant.Name),
                            Visible = true
                        };

                        outerBand.Columns = new ObservableCollection<ColumnItem>();


                        List<PlantLoadAnalysis> TempPlantLoadAnalysis = new List<PlantLoadAnalysis>();
                        // take only structure data : idTemplate = 24
                        TempPlantLoadAnalysis = PlantLoadAnalysisList.Where(i => i.ProductionIdSite == SelectedPlant.IdSite && i.IdTemplate == 24).ToList();

                        GroupByTempPlantLoadAnalysisEquipment.AddRange(TempPlantLoadAnalysis.GroupBy(i => i.OriginalIdSite)
                                                                                 .Select(grp => grp.First())
                                                                                 .ToList().Distinct());

                        //  GroupByTempPlantLoadAnalysisCopy.AddRange(GroupByTempPlantLoadAnalysis);
                        //for (int i = 0; i < 5; i++)

                        foreach (PlantLoadAnalysis objOrigianlSite in GroupByTempPlantLoadAnalysisEquipment)
                        {
                            if (GroupByTempPlantLoadAnalysisCopySummaryEquipment.Where(i => i.OriginalIdSite == objOrigianlSite.OriginalIdSite).ToList().Count == 0)
                            {
                                GroupByTempPlantLoadAnalysisCopySummaryEquipment.Add(objOrigianlSite);
                            }
                        }


                        foreach (var item in GroupByTempPlantLoadAnalysisEquipment)
                        {
                            outerBand.Columns.Add(new ColumnItem() { ColumnFieldName = "Plant_" + item.OriginalIdSite + "_" + SelectedPlant.IdSite, HeaderText = item.OriginalSiteName.ToString(), Width = 70, Visible = true, IsVertical = false, PlantLoadAnalysisetting = PlantLoadAnalysisTemplateSelector.PlantLoadAnalysisSettingType.Plant });

                            DataTableForGridLayoutLoadEquipment.Columns.Add("Plant_" + item.OriginalIdSite + "_" + SelectedPlant.IdSite, typeof(Int32));

                            GroupSummaryLoadEquipment.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Sum, FieldName = "Plant_" + item.OriginalIdSite + "_" + SelectedPlant.IdSite, DisplayFormat = "{0}" });
                            TotalSummaryLoadEquipment.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Sum, FieldName = "Plant_" + item.OriginalIdSite + "_" + SelectedPlant.IdSite, DisplayFormat = "{0}" });

                            GridControlLoadEquipmentFilter.GroupSummary.Add(new GridSummaryItem
                            {
                                FieldName = "Plant_" + item.OriginalIdSite + "_" + SelectedPlant.IdSite,
                                SummaryType = SummaryItemType.Sum,
                                DisplayFormat = "{0}"
                            });

                            GridControlLoadEquipmentFilter.TotalSummary.Add(new GridSummaryItem
                            {
                                FieldName = "Plant_" + item.OriginalIdSite + "_" + SelectedPlant.IdSite,
                                SummaryType = SummaryItemType.Sum,
                                DisplayFormat = "{0}"
                            });
                        }
                        outerBand.Columns.Add(new ColumnItem() { ColumnFieldName = "Total_" + SelectedPlant.IdSite, HeaderText = "Total", Width = 70, Visible = true, IsVertical = false, PlantLoadAnalysisetting = PlantLoadAnalysisTemplateSelector.PlantLoadAnalysisSettingType.Total });
                        DataTableForGridLayoutLoadEquipment.Columns.Add("Total_" + SelectedPlant.IdSite, typeof(Int32));
                        GroupSummaryLoadEquipment.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Sum, FieldName = "Total_" + SelectedPlant.IdSite, DisplayFormat = "{0}" });
                        TotalSummaryLoadEquipment.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Sum, FieldName = "Total_" + SelectedPlant.IdSite, DisplayFormat = "{0}" });
                        BandsDashboardLoadEquipment.Add(outerBand);

                        GridControlLoadEquipmentFilter.TotalSummary.Add(new GridSummaryItem
                        {
                            FieldName = "Total_" + SelectedPlant.IdSite,
                            SummaryType = SummaryItemType.Sum,
                            DisplayFormat = "{0}"
                        });

                        GridControlLoadEquipmentFilter.GroupSummary.Add(new GridSummaryItem
                        {
                            FieldName = "Total_" + SelectedPlant.IdSite,
                            SummaryType = SummaryItemType.Sum,
                            DisplayFormat = "{0}"
                        });
                    }


                }
                BandItem TotalBand = new BandItem()
                {
                    BandName = "GrandTotal",
                    BandHeader = "Total",
                    Visible = true,
                    AllowBandMove = false,
                    FixedStyle = FixedStyle.Right,

                };
                TotalBand.Columns = new ObservableCollection<ColumnItem>();
                TotalBand.Columns.Add(new ColumnItem() { ColumnFieldName = "GrandTotal", HeaderText = "", Width = 70, Visible = true, IsVertical = false, PlantLoadAnalysisetting = PlantLoadAnalysisTemplateSelector.PlantLoadAnalysisSettingType.Total });
                BandsDashboardLoadEquipment.Add(TotalBand);
                DataTableForGridLayoutLoadEquipment.Columns.Add("GrandTotal", typeof(Int32));
                GroupSummaryLoadEquipment.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Sum, FieldName = "GrandTotal", DisplayFormat = "{0}" });
                TotalSummaryLoadEquipment.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Sum, FieldName = "GrandTotal", DisplayFormat = "{0}" });
                TotalSummaryLoadEquipment.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Count, FieldName = "otCodeEquipment", DisplayFormat = "Total" });


                // TotalSummaryLoadEquipment.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Count, FieldName = "CPType", DisplayFormat = "Total" });

                GridControlLoadEquipmentFilter.TotalSummary.Add(new GridSummaryItem
                {
                    FieldName = "otCodeEquipment",
                    SummaryType = SummaryItemType.Count,
                    DisplayFormat = "Total"
                });

                GridControlLoadEquipmentFilter.TotalSummary.Add(new GridSummaryItem
                {
                    FieldName = "GrandTotal",
                    SummaryType = SummaryItemType.Sum,
                    DisplayFormat = "{0}"
                });

                GridControlLoadEquipmentFilter.GroupSummary.Add(new GridSummaryItem
                {
                    FieldName = "GrandTotal",
                    SummaryType = SummaryItemType.Sum,
                    DisplayFormat = "{0}"
                });
                BandsDashboardLoadEquipment = new ObservableCollection<BandItem>(BandsDashboardLoadEquipment);

                GridControlLoadEquipmentFilter.Bands.Add(band);

                GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTableWithBandsinLoadEquipments executed Successfully", category: Category.Info, priority: Priority.Low);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in AddColumnsToDataTableWithBandsinLoadEquipments() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
        }


        private void FillLoadEquipmentsData()
        {
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                GeosApplication.Instance.Logger.Log("Method FillLoadEquipmentsData ...", category: Category.Info, priority: Priority.Low);
                DataTableForGridLayoutLoadEquipment.Clear();
                DataTable DataTableForGridLayoutLoadEquipmentCopy = DataTableForGridLayoutLoadEquipment.Copy();
                DateTime currentDate = DateTime.Now;
                int currentWeek = (int)(currentDate.DayOfYear / 7) + 1;
                int year = DateTime.Now.Year;
                string Week = year + "CW" + currentWeek;
                foreach (PlantLoadAnalysis objPlantLoadAnalysis in PlantLoadAnalysisList.Where(j => j.IdTemplate == 24).ToList())
                {
                    List<Site> plantOwners = ERMCommon.Instance.SelectedAuthorizedPlantsList.Cast<Site>().ToList();

                    int GrandTotal = 0;
                    int TotalQTY = 1;

                    try
                    {
                        DataRow dr;

                        string Condition = "CalendarWeekEquipment = '" + objPlantLoadAnalysis.DeliveryWeek + "' AND otCodeEquipment = '" + objPlantLoadAnalysis.OTCode + "'";
                        DataRow[] drExists = DataTableForGridLayoutLoadEquipmentCopy.Select(Condition);
                        if (drExists.Length == 0)
                            dr = DataTableForGridLayoutLoadEquipmentCopy.NewRow();
                        else
                            dr = drExists.FirstOrDefault();

                        dr["CalendarWeekEquipment"] = objPlantLoadAnalysis.DeliveryWeek;
                        dr["otCodeEquipment"] = objPlantLoadAnalysis.OTCode;
                        dr["Plant_" + objPlantLoadAnalysis.OriginalIdSite + "_" + objPlantLoadAnalysis.ProductionIdSite] = 1;
                        dr["Total_" + objPlantLoadAnalysis.ProductionIdSite] = TotalQTY;
                        dr["GroupColor"] = false;
                        if (objPlantLoadAnalysis.DeliveryWeek == Week)
                        {
                            // string curWeek = rowsInCurrentWeek.Select(a => a.ToString()).FirstOrDefault();
                            //GroupColor = Week;
                            dr["GroupColor"] = true;
                        }
                        foreach (DataColumn col in DataTableForGridLayoutLoadEquipmentCopy.Columns)
                        {
                            if (col.ColumnName.Contains("Total_"))
                            {
                                if (dr[col.ColumnName] == DBNull.Value)
                                    GrandTotal = GrandTotal + 0;
                                else
                                    GrandTotal = GrandTotal + Convert.ToInt32(dr[col.ColumnName]);
                            }

                        }

                        dr["GrandTotal"] = GrandTotal;

                        if (drExists.Length == 0)
                        {
                            DataTableForGridLayoutLoadEquipmentCopy.Rows.Add(dr);
                        }

                    }

                    catch (Exception ex)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Error in FillLoadEquipmentsData()- 1 Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                    }
                }

                var columnsToRemove = DataTableForGridLayoutLoadEquipmentCopy.Columns.Cast<DataColumn>()
           .Where(col => DataTableForGridLayoutLoadEquipmentCopy.AsEnumerable().All(row => row.IsNull(col)))
           .ToList();


                foreach (var col in columnsToRemove)
                {
                    if (!col.ColumnName.Contains("Total"))
                    {


                        DataTableForGridLayoutLoadEquipmentCopy.Columns.Remove(col);
                    }
                    //     DataTableForGridLayoutPlantLoadAnalysisCopy.Columns.RemoveAt(0);
                }
                if (columnsToRemove.Count>0)
                {
                    AddColumnsToDataTableWithBandsinLoadEquipments();
                }
                DataTableForGridLayoutLoadEquipment = DataTableForGridLayoutLoadEquipmentCopy;

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method FillLoadEquipmentsData executed Successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in FillLoadEquipmentsData()- 2 Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void GridActionLoadEquipmentCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GridActionLoadEquipmentCommandAction()...", category: Category.Info, priority: Priority.Low);

                GridControlLoadEquipment = (GridControl)obj;

                GridControlLoadEquipmentFilter = GridControlLoadEquipment;

                GridControlLoadEquipmentFilter.GroupBy("CalendarWeekEquipment");
                //GridControlLoadEquipment.GroupBy("OTCode");

                GeosApplication.Instance.Logger.Log("Method GridActionLoadEquipmentCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GridActionLoadEquipmentCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion Load-Equipments

        #region Load-Customers  

        private void FillLoadCustomerData()
        {
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                GeosApplication.Instance.Logger.Log("Method FillLoadCustomerData ...", category: Category.Info, priority: Priority.Low);
                DataTableForGridLayoutLoadCustomers.Clear();
                DataTable DataTableForGridLayoutLoadCustomerCopy = DataTableForGridLayoutLoadCustomers.Copy();
                string CategoryName = string.Empty;

                if (PlantLoadAnalysisList.Count > 0)
                {

                    var groupedData = PlantLoadAnalysisList
                        .GroupBy(item => new { item.CustomerGroup, item.CustomerPlant })
                        .Select(group => new
                        {
                            CustomerGroup = group.Key.CustomerGroup,
                            CustomerPlant = group.Key.CustomerPlant,
                            TotalQTY = group.Sum(item => item.QTY)
                        });

                    foreach (var groupItem in groupedData)
                    {
                        try
                        {
                            DataRow dr = DataTableForGridLayoutLoadCustomerCopy.NewRow();

                            dr["CustomerName"] = $"{groupItem.CustomerGroup} - {groupItem.CustomerPlant}";
                            dr["QTY"] = groupItem.TotalQTY;

                            DataTableForGridLayoutLoadCustomerCopy.Rows.Add(dr);
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }


                DataTableForGridLayoutLoadCustomers = DataTableForGridLayoutLoadCustomerCopy;

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method FillLoadCustomerData()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillLoadCustomerData() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        private void GridActionLoadCustomersCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GridActionLoadCustomersCommandAction()...", category: Category.Info, priority: Priority.Low);

                GridControlLoadCustomers = (GridControl)obj;

                GridControlLoadCustomersFilter = GridControlLoadCustomers;


                GeosApplication.Instance.Logger.Log("Method GridActionLoadCustomersCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GridActionLoadCustomersCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AddColumnsToDataTableWithBandsinLoadCustomers()
        {
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTableWithBandsinLoadCustomers ...", category: Category.Info, priority: Priority.Low);

                CategoryColumns = new List<string>();

                ColumnsLoadCustomers = new ObservableCollection<Emdep.Geos.UI.Helper.Column>()
                {
                    new Emdep.Geos.UI.Helper.Column() { FieldName="CustomerName",HeaderText="CUSTOMER NAME", Settings = SettingsType.CustomerName, AllowCellMerge=false, Width=450, AllowEditing = false, Visible= true, IsVertical = false, FixedWidth=true, IsReadOnly = true  },
                    new Emdep.Geos.UI.Helper.Column() { FieldName="QTY",HeaderText="CP's QTY", Settings = SettingsType.QTY, AllowCellMerge =false, Width=92, AllowEditing = false, Visible= true, IsVertical = false, FixedWidth=true, IsReadOnly = true  },
                };


                GroupSummaryLoadCustomers = new ObservableCollection<Emdep.Geos.UI.Helper.Summary>();
                TotalSummaryLoadCustomers = new ObservableCollection<Emdep.Geos.UI.Helper.Summary>();

                DataTableForGridLayoutLoadCustomers = new DataTable();
                DataTableForGridLayoutLoadCustomers.Columns.Add("CustomerName", typeof(string));
                DataTableForGridLayoutLoadCustomers.Columns.Add("QTY", typeof(Int32));



                TotalSummaryLoadCustomers.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Sum, FieldName = "QTY", DisplayFormat = "{0}" });
                TotalSummaryLoadCustomers.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Count, FieldName = "CustomerName", DisplayFormat = "Total" });



                GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTableWithBandsinLoadCustomers executed Successfully", category: Category.Info, priority: Priority.Low);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in AddColumnsToDataTableWithBandsinLoadCustomers() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        #endregion Load-Customers

        #region Load-Workstation
        private void AddColumnsToDataTableWithBandsinLoadWorkstation()
        {
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTableWithBandsinLoadWorkstation ...", category: Category.Info, priority: Priority.Low);

                ColumnsLoadWorkstation = new ObservableCollection<Emdep.Geos.UI.Helper.Column>()
                {
                    new Emdep.Geos.UI.Helper.Column() { FieldName="CalenderWeek", HeaderText="", Settings = SettingsType.CalendarWeekPDAWorkstation, AllowCellMerge=false, Width=60, AllowEditing = false, Visible= true, IsVertical = false, FixedWidth=true, IsReadOnly = true  },
                    new Emdep.Geos.UI.Helper.Column() { FieldName="CustomerWithPlant", HeaderText="", Settings = SettingsType.CalendarWeekPDAWorkstation, AllowCellMerge=false, Width=60, AllowEditing = false, Visible= true, IsVertical = false, FixedWidth=true, IsReadOnly = true  },
                    new Emdep.Geos.UI.Helper.Column() { FieldName="Project", HeaderText="", Settings = SettingsType.CalendarWeekPDAWorkstation, AllowCellMerge=false, Width=120, AllowEditing = false, Visible= true, IsVertical = false, FixedWidth=true, IsReadOnly = true  },
                    new Emdep.Geos.UI.Helper.Column() { FieldName="OTCode", HeaderText="", Settings = SettingsType.PDAOTCode, AllowCellMerge=false, Width=120, AllowEditing = false, Visible= true, IsVertical = false, FixedWidth=true, IsReadOnly = true  },
                     new Emdep.Geos.UI.Helper.Column() { FieldName="ItemNumber", HeaderText="YearWeek", Settings = SettingsType.PDAOTCode, AllowCellMerge=false, Width=120, AllowEditing = false, Visible= true, IsVertical = false, FixedWidth=true, IsReadOnly = true  },

                };


                if (GridControlLoadWorkstationFilter != null)
                {
                    GridControlLoadWorkstationFilter.Columns.Add(new GridColumn { FieldName = "CalenderWeek", Visible = false });
                    GridControlLoadWorkstationFilter.Columns.Add(new GridColumn { FieldName = "CustomerWithPlant", Visible = false });
                    GridControlLoadWorkstationFilter.Columns.Add(new GridColumn { FieldName = "Project", Visible = false });
                    GridControlLoadWorkstationFilter.Columns.Add(new GridColumn { FieldName = "OTCode", Visible = false });
                    GridControlLoadWorkstationFilter.Columns.Add(new GridColumn { FieldName = "ItemNumber", Visible = false });

                }


                DataTableForGridLayoutLoadWorkstation = new DataTable();
                DataTableForGridLayoutLoadWorkstation.Columns.Add("CalenderWeek", typeof(string));
                DataTableForGridLayoutLoadWorkstation.Columns.Add("CustomerWithPlant", typeof(string));
                DataTableForGridLayoutLoadWorkstation.Columns.Add("Project", typeof(string));
                DataTableForGridLayoutLoadWorkstation.Columns.Add("OTCode", typeof(string));
                DataTableForGridLayoutLoadWorkstation.Columns.Add("ItemNumber", typeof(string));
                DataTableForGridLayoutLoadWorkstation.Columns.Add("GroupColor", typeof(bool));
                GroupSummaryLoadWorkstation = new ObservableCollection<Emdep.Geos.UI.Helper.Summary>();
                TotalSummaryLoadWorkstation = new ObservableCollection<Emdep.Geos.UI.Helper.Summary>();

                ColumnsLoadWorkstation.Add(new Emdep.Geos.UI.Helper.Column() { FieldName = "TempColumn", HeaderText = "TempColumn", Settings = SettingsType.ArrayOfferOption, AllowCellMerge = false, Width = 120, AllowEditing = false, Visible = false, IsVertical = false, FixedWidth = true, IsReadOnly = true });

                string StageName = string.Empty;
                //int j = 0;
                foreach (DeliveryVisualManagementStages item in LoadWorkstationStageList)
                {

                    StageName = "Plant_" + item.IdStage;

                    //       outerBand.Columns.Add(new ColumnItem() { ColumnFieldName = "Plant_" + item.IdStage + j, HeaderText = item.StageName.ToString(), Width = 70, Visible = true, IsVertical = false, PlantLoadAnalysisetting = PlantLoadAnalysisTemplateSelector.PlantLoadAnalysisSettingType.DefaultTemplate });
                    //       DataTableForGridLayoutLoadWorkstation.Columns.Add("Plant_" + item.IdStage + j, typeof(string));

                    ColumnsLoadWorkstation.Add(new Emdep.Geos.UI.Helper.Column() { FieldName = "Plant_" + item.IdStage, HeaderText = item.StageCode, Settings = SettingsType.ArrayOfferOption, AllowCellMerge = false, Width = 90, AllowEditing = false, Visible = true, IsVertical = false, FixedWidth = true, IsReadOnly = true });

                    DataTableForGridLayoutLoadWorkstation.Columns.Add("Plant_" + item.IdStage, typeof(string));

                    GroupSummaryLoadWorkstation.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Sum, FieldName = StageName, DisplayFormat = "{0}" });
                    TotalSummaryLoadWorkstation.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Sum, FieldName = StageName, DisplayFormat = "{0}" });

                }
                TotalSummaryLoadWorkstation.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Count, FieldName = "ItemNumber", DisplayFormat = "Total" });
                GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTableWithBandsinLoadWorkstation executed Successfully", category: Category.Info, priority: Priority.Low);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in AddColumnsToDataTableWithBandsinLoadWorkstation() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        private void FillLoadWorkstationData()
        {
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                GeosApplication.Instance.Logger.Log("Method FillLoadWorkstationData ...", category: Category.Info, priority: Priority.Low);

                // Clear the original DataTable
                // Clear the original DataTable
                // Clear the original DataTable
                DataTableForGridLayoutLoadWorkstation.Clear();

                // Create a copy of the DataTable
                DataTable DataTableForGridLayoutLoadWorkstationCopy = DataTableForGridLayoutLoadWorkstation.Copy();
                var groupedDataWorkStation = PlantLoadAnalysisList
                      .GroupBy(item => new { item.DeliveryWeek, item.CustomerWithPlant, item.Project, item.OTCode, item.ItemNumber, item.IdStage })
                      .Select(group => new
                      {
                          DeliveryWeek = group.Key.DeliveryWeek,
                          CustomerWithPlant = group.Key.CustomerWithPlant,
                          //CustomerPlant = group.Key.CustomerPlant,
                          Project = group.Key.Project,
                          OTCode = group.Key.OTCode,
                          IdStage = group.Key.IdStage,
                          ItemNumber = group.Key.ItemNumber,
                          TotalQTY = group.Sum(item => item.QTY)
                      }).OrderBy(a => a.ItemNumber);
                //// Group the data by DeliveryWeek, ItemNumber, and IdStage
                //var groupedDataWorkStation = PlantLoadAnalysisList
                //    .GroupBy(item => new { item.DeliveryWeek, item.ItemNumber, item.IdStage })
                //    .Select(grp => grp.First()).OrderBy(a=>a.ItemNumber).ToList();
                DateTime currentDate = DateTime.Now;
                int currentWeek = (int)(currentDate.DayOfYear / 7) + 1;
                int year = DateTime.Now.Year;
                string Week = year + "CW" + currentWeek;
                foreach (var groupItem in groupedDataWorkStation)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(groupItem.IdStage.ToString()) || groupItem.IdStage == 0)
                        {
                            continue; // Skip items with invalid IdStage
                        }

                        string ColumnName = "Plant_" + groupItem.IdStage;
                        if (!DataTableForGridLayoutLoadWorkstationCopy.Columns.Contains(ColumnName))
                        {
                            DataTableForGridLayoutLoadWorkstationCopy.Columns.Add(ColumnName);
                        }

                        // Process each item separately
                        DataRow dr;
                        string Condition = "CalenderWeek = '" + groupItem.DeliveryWeek +
                                           "' and CustomerWithPlant = '" + groupItem.CustomerWithPlant +
                                           "' and Project = '" + groupItem.Project +
                                           "' and OTCode = '" + groupItem.OTCode +
                                           "' and ItemNumber = '" + groupItem.ItemNumber + "'";

                        DataRow[] ExistedRow = DataTableForGridLayoutLoadWorkstationCopy.Select(Condition);

                        if (ExistedRow != null && ExistedRow.Length > 0)
                        {
                            dr = ExistedRow.FirstOrDefault(); // Use existing row
                        }
                        else
                        {
                            dr = DataTableForGridLayoutLoadWorkstationCopy.NewRow(); // Create new row if no existing row is found
                        }

                        // Set common fields
                        dr["CalenderWeek"] = groupItem.DeliveryWeek;
                        dr["CustomerWithPlant"] = groupItem.CustomerWithPlant;
                        dr["Project"] = groupItem.Project;
                        dr["OTCode"] = groupItem.OTCode;
                        dr["ItemNumber"] = groupItem.ItemNumber;
                        dr["GroupColor"] = false;
                        if (groupItem.DeliveryWeek == Week)
                        {
                            // string curWeek = rowsInCurrentWeek.Select(a => a.ToString()).FirstOrDefault();
                            //GroupColor = Week;
                            dr["GroupColor"] = true;
                        }
                        // Handle Quantity
                        if (DataTableForGridLayoutLoadWorkstationCopy.Columns.Contains(ColumnName))
                        {
                            dr[ColumnName] = groupItem.TotalQTY; // Set Quantity
                        }

                        // Add or update the row in the DataTable
                        if (ExistedRow == null || ExistedRow.Length == 0)
                        {
                            DataTableForGridLayoutLoadWorkstationCopy.Rows.Add(dr); // Add new row if it doesn't exist
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }

                // Update the original DataTable with the changes
                DataTableForGridLayoutLoadWorkstation = DataTableForGridLayoutLoadWorkstationCopy.Copy();


                //}
                //catch (Exception ex)
                //        {
                //            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                //            GeosApplication.Instance.Logger.Log("Error in FillLoadWorkstationData() - 1 Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                //        }
                //    }

                //   DataTableForGridLayoutLoadWorkstation = DataTableForGridLayoutLoadWorkstationCopy;

                GeosApplication.Instance.Logger.Log("Method FillLoadWorkstationData executed Successfully", category: Category.Info, priority: Priority.Low);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }

            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in FillLoadWorkstationData() - 2 Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        private void GridActionLoadWorkstationCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GridActionLoadWorkstationCommandAction()...", category: Category.Info, priority: Priority.Low);

                GridControlLoadWorkstation = (GridControl)obj;

                GridControlLoadWorkstationFilter = GridControlLoadWorkstation;

                GridControlLoadWorkstation.GroupBy("CalenderWeek");
                GridControlLoadWorkstation.GroupBy("CustomerWithPlant");
                GridControlLoadWorkstation.GroupBy("Project");
                GridControlLoadWorkstation.GroupBy("OTCode");

                GeosApplication.Instance.Logger.Log("Method GridActionLoadWorkstationCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GridActionLoadEquipmentCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void GroupBYWorkstation()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GroupBYWorkstation()...", category: Category.Info, priority: Priority.Low);

                GridControlLoadWorkstationFilter.GroupBy("CalenderWeek");
                GridControlLoadWorkstationFilter.GroupBy("CustomerWithPlant");
                GridControlLoadWorkstationFilter.GroupBy("Project");
                //  GridControlLoadWorkstationFilter.GroupBy("OTCode");

                GeosApplication.Instance.Logger.Log("Method GroupBYWorkstation()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {

                GeosApplication.Instance.Logger.Log("Get an error in GroupBYWorkstation() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion Load-Workstation

        #region Filter
        private void FillFilter()
        {
            try
            {
                FillRegion();
                FillCustomerGroup();
                FillCustomerPlant();
                FillOTCode();
                FillProject();
                FillConectorFamily();
                FillCPType();
                FillTemplate();
                FillOTItemStatus();
                // FillAllselectedFilter();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void FillRegion()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillRegion()...", category: Category.Info, priority: Priority.Low);


                RegionList = new List<PlantLoadAnalysis>();

                foreach (var item in PlantLoadAnalysisList)
                {
                    PlantLoadAnalysis selectedRegion = new PlantLoadAnalysis();
                    // selectedRegion.OTStatus = item.OTStatus;
                    selectedRegion.Region = item.Region;
                    //   selectedRegion.IdProductCategory = item.IdProductCategory;
                    RegionList.Add(selectedRegion);

                }


                var TempPlantDeliveryAnalysisList1 = (from a in RegionList
                                                      select new
                                                      {
                                                          a.Region


                                                      }).Distinct().ToList();
                RegionList = new List<PlantLoadAnalysis>();

                foreach (var item1 in TempPlantDeliveryAnalysisList1)
                {
                    PlantLoadAnalysis selectedvalue = new PlantLoadAnalysis();
                    selectedvalue.Region = item1.Region;
                    //  selectedvalue.OfferStatusType = PlantLoadAnalysisList.Where(a => a.Region == item1.Region).FirstOrDefault().OfferStatusType;

                    RegionList.Add(selectedvalue);

                }
                RegionList = new List<PlantLoadAnalysis>(RegionList);
                Region_Cloned = new List<PlantLoadAnalysis>();
                Region_Cloned = RegionList.ToList();
                GeosApplication.Instance.Logger.Log("Method FillRegion()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillRegion() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillCustomerGroup()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCustomerGroup()...", category: Category.Info, priority: Priority.Low);

                CustomerGroupList = new List<PlantLoadAnalysis>();

                foreach (var item in PlantLoadAnalysisList)
                {
                    PlantLoadAnalysis selectedCustomerGroup = new PlantLoadAnalysis();
                    selectedCustomerGroup.CustomerGroup = item.CustomerGroup;
                    // selectedCustomerGroup.IdProductCategory = item.IdProductCategory;
                    selectedCustomerGroup.IdCustomer = item.IdCustomer;
                    CustomerGroupList.Add(selectedCustomerGroup);

                }
                //var TempCustomerGroupList = (from a in CustomerGroupList
                //                             select new
                //                             {
                //                                 // a.CustomerGroup,
                //                                 // a.IdProductCategory,
                //                                 a.IdCustomer

                //                             }).Distinct().ToList();
                //CustomerGroupList = new List<PlantLoadAnalysis>();

                //foreach (var item1 in TempCustomerGroupList)
                //{
                //    PlantLoadAnalysis selectedvalue = new PlantLoadAnalysis();
                //    selectedvalue.CustomerGroup = PlantLoadAnalysisList.Where(i => i.IdCustomer == item1.IdCustomer).FirstOrDefault().CustomerGroup;
                //    // selectedvalue.IdProductCategory = PlantLoadAnalysisList.Where(i => i.IdCustomer == item1.IdCustomer).FirstOrDefault().IdProductCategory;
                //    selectedvalue.IdCustomer = item1.IdCustomer;
                //    selectedvalue.CustomerPlant = PlantLoadAnalysisList.Where(i => i.IdCustomer == item1.IdCustomer).FirstOrDefault().CustomerPlant;
                //    //   selectedvalue.OTStatus = PlantDeliveryAnalysisList.Where(i => i.IdCustomer == item1.IdCustomer).FirstOrDefault().OTStatus;
                //    CustomerGroupList.Add(selectedvalue);

                //}
                //CustomerGroupList = new List<PlantLoadAnalysis>(CustomerGroupList);
                //CustomerGroup_Cloned = new List<PlantLoadAnalysis>();
                //CustomerGroup_Cloned = CustomerGroupList.ToList();

                List<PlantLoadAnalysis> TempCustomerGroupList = new List<PlantLoadAnalysis>();
                TempCustomerGroupList.AddRange(CustomerGroupList.GroupBy(i => i.IdCustomer)
                                                                                 .Select(grp => grp.First())
                                                                                 .ToList().Distinct());

                CustomerGroupList = new List<PlantLoadAnalysis>(TempCustomerGroupList);
                GeosApplication.Instance.Logger.Log("Method FillCustomerGroup()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCustomerGroup() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillCustomerPlant()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCustomerPlant()...", category: Category.Info, priority: Priority.Low);

                CustomerPlantList = new List<PlantLoadAnalysis>();

                foreach (var item in PlantLoadAnalysisList)
                {
                    PlantLoadAnalysis selectedCustomerGroup = new PlantLoadAnalysis();
                    selectedCustomerGroup.CustomerPlant = item.CustomerPlant;
                    selectedCustomerGroup.IdCustomer = item.IdCustomer;
                    CustomerPlantList.Add(selectedCustomerGroup);

                }
                var TempCustomerPlantList = (from a in CustomerPlantList
                                             select new
                                             {
                                                 a.CustomerPlant,
                                                 a.IdCustomer

                                             }).Distinct().ToList();
                CustomerPlantList = new List<PlantLoadAnalysis>();

                foreach (var item1 in TempCustomerPlantList)
                {
                    PlantLoadAnalysis selectedvalue = new PlantLoadAnalysis();
                    selectedvalue.CustomerPlant = item1.CustomerPlant;
                    selectedvalue.IdCustomer = item1.IdCustomer;

                    CustomerPlantList.Add(selectedvalue);

                }
                CustomerPlantList = new List<PlantLoadAnalysis>(CustomerPlantList);
                CustomerPlant_Cloned = new List<PlantLoadAnalysis>();
                CustomerPlant_Cloned = CustomerPlantList.ToList();
                GeosApplication.Instance.Logger.Log("Method FillCustomerPlant()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCustomerPlant() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillOTCode()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillOTCode()...", category: Category.Info, priority: Priority.Low);

                OTCodeList = new List<PlantLoadAnalysis>();

                foreach (var item in PlantLoadAnalysisList)
                {
                    PlantLoadAnalysis selectedCustomerGroup = new PlantLoadAnalysis();
                    selectedCustomerGroup.OTCode = item.OTCode;
                    //  selectedCustomerGroup.IdCustomer = item.IdCustomer;
                    OTCodeList.Add(selectedCustomerGroup);

                }
                var TempOTCodeList = (from a in OTCodeList
                                      select new
                                      {
                                          a.OTCode,
                                          //a.IdCustomer

                                      }).Distinct().ToList();
                OTCodeList = new List<PlantLoadAnalysis>();

                foreach (var item1 in TempOTCodeList)
                {
                    PlantLoadAnalysis selectedvalue = new PlantLoadAnalysis();
                    selectedvalue.OTCode = item1.OTCode;
                    //   selectedvalue.IdCustomer = item1.IdCustomer;

                    OTCodeList.Add(selectedvalue);

                }
                OTCodeList = new List<PlantLoadAnalysis>(OTCodeList);
                OTCodeList_Cloned = new List<PlantLoadAnalysis>();
                OTCodeList_Cloned = OTCodeList.ToList();
                GeosApplication.Instance.Logger.Log("Method FillOTCode()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillOTCode() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillProject()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillProject()...", category: Category.Info, priority: Priority.Low);

                ProjectList = new List<PlantLoadAnalysis>();

                foreach (var item in PlantLoadAnalysisList.Where(a => a.Project != null).ToList())
                {
                    PlantLoadAnalysis selectedCustomerGroup = new PlantLoadAnalysis();
                    selectedCustomerGroup.Project = item.Project;
                    //  selectedCustomerGroup.IdCustomer = item.IdCustomer;
                    ProjectList.Add(selectedCustomerGroup);

                }
                var TempProjectList = (from a in ProjectList
                                       select new
                                       {
                                           a.Project,
                                           //a.IdCustomer

                                       }).Distinct().ToList();
                ProjectList = new List<PlantLoadAnalysis>();

                foreach (var item1 in TempProjectList)
                {
                    PlantLoadAnalysis selectedvalue = new PlantLoadAnalysis();
                    selectedvalue.Project = item1.Project;
                    //   selectedvalue.IdCustomer = item1.IdCustomer;

                    ProjectList.Add(selectedvalue);

                }
                ProjectList = new List<PlantLoadAnalysis>(ProjectList);
                ProjectList_Cloned = new List<PlantLoadAnalysis>();
                ProjectList_Cloned = ProjectList.ToList();
                GeosApplication.Instance.Logger.Log("Method FillProject()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillProject() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillConectorFamily()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillConectorFamily()...", category: Category.Info, priority: Priority.Low);

                ConnectorFamilyList = new List<PlantLoadAnalysis>();

                foreach (var item in PlantLoadAnalysisList)
                {
                    PlantLoadAnalysis selectedConnectorFamily = new PlantLoadAnalysis();
                    selectedConnectorFamily.ConnectorFamily = item.ConnectorFamily;
                    selectedConnectorFamily.IdFamily = item.IdFamily;
                    //  selectedCustomerGroup.IdCustomer = item.IdCustomer;
                    ConnectorFamilyList.Add(selectedConnectorFamily);

                }
                List<PlantLoadAnalysis> TempConnectorFamilyList = new List<PlantLoadAnalysis>();
                TempConnectorFamilyList.AddRange(ConnectorFamilyList.GroupBy(i => i.IdFamily)
                                                                                 .Select(grp => grp.First())
                                                                                 .ToList().Distinct());

                ConnectorFamilyList = new List<PlantLoadAnalysis>(TempConnectorFamilyList.Where(i => i.ConnectorFamily != null).ToList());
                GeosApplication.Instance.Logger.Log("Method FillConectorFamily()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillConectorFamily() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillCPType()
        {

            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCPType()...", category: Category.Info, priority: Priority.Low);

                CPTypeList = new List<PlantLoadAnalysis>();

                foreach (var item in PlantLoadAnalysisList)
                {
                    PlantLoadAnalysis selectedCustomerGroup = new PlantLoadAnalysis();
                    selectedCustomerGroup.CPType = item.CPType;
                    //  selectedCustomerGroup.IdCustomer = item.IdCustomer;
                    CPTypeList.Add(selectedCustomerGroup);

                }
                var TempCPTypeList = (from a in CPTypeList
                                      select new
                                      {
                                          a.CPType,
                                          //a.IdCustomer

                                      }).Distinct().ToList();
                CPTypeList = new List<PlantLoadAnalysis>();

                foreach (var item1 in TempCPTypeList)
                {
                    PlantLoadAnalysis selectedvalue = new PlantLoadAnalysis();
                    selectedvalue.CPType = item1.CPType;
                    //   selectedvalue.IdCustomer = item1.IdCustomer;

                    CPTypeList.Add(selectedvalue);

                }
                CPTypeList = new List<PlantLoadAnalysis>(CPTypeList);
                CPTypeList_Cloned = new List<PlantLoadAnalysis>();
                CPTypeList_Cloned = CPTypeList.ToList();
                GeosApplication.Instance.Logger.Log("Method FillCPType()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCPType() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillTemplate()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillTemplate()...", category: Category.Info, priority: Priority.Low);


                TemplateList = new List<PlantLoadAnalysis>();

                foreach (var item in PlantLoadAnalysisList)
                {
                    PlantLoadAnalysis selectedOTStatus = new PlantLoadAnalysis();
                    selectedOTStatus.Template = item.Template;
                    //     selectedOTStatus.IdProductCategory = item.IdProductCategory;
                    TemplateList.Add(selectedOTStatus);

                }
                //List<PlantDeliveryAnalysis> TempPlantDeliveryAnalysisList = new List<PlantDeliveryAnalysis>();
                //TempPlantDeliveryAnalysisList = OTStatusList.Select(a=>a.OTStatus).Distinct().ToList();
                //OTStatusList = new List<PlantDeliveryAnalysis>(TempPlantDeliveryAnalysisList);

                var TempTemplateList = (from a in TemplateList
                                        select new
                                        {
                                            a.Template,


                                        }).Distinct().ToList();
                TemplateList = new List<PlantLoadAnalysis>();

                foreach (var item1 in TempTemplateList)
                {
                    PlantLoadAnalysis selectedvalue = new PlantLoadAnalysis();
                    selectedvalue.Template = item1.Template;
                    // selectedvalue.IdProductCategory = PlantDeliveryAnalysisList.Where(a => a.TemplateName == item1.TemplateName).FirstOrDefault().IdProductCategory;
                    // selectedvalue.OTStatus = PlantDeliveryAnalysisList.Where(b => b.TemplateName == item1.TemplateName).FirstOrDefault().OTStatus;

                    TemplateList.Add(selectedvalue);

                }
                TemplateList = new List<PlantLoadAnalysis>(TemplateList);
                TemplateList_Cloned = new List<PlantLoadAnalysis>();
                TemplateList_Cloned = TemplateList.ToList();
                GeosApplication.Instance.Logger.Log("Method FillTemplate()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillTemplate() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillOTItemStatus()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillOTItemStatus()...", category: Category.Info, priority: Priority.Low);


                OTItemStatusList = new List<PlantLoadAnalysis>();
                foreach (var item in PlantLoadAnalysisList)
                {
                    PlantLoadAnalysis selectedOTStatus = new PlantLoadAnalysis();
                    selectedOTStatus.OTItemStatus = item.OTItemStatus;
                    //    selectedOTStatus.IdProductCategory = item.IdProductCategory;
                    OTItemStatusList.Add(selectedOTStatus);

                }
                //List<PlantDeliveryAnalysis> TempPlantDeliveryAnalysisList = new List<PlantDeliveryAnalysis>();
                //TempPlantDeliveryAnalysisList = OTStatusList.Select(a=>a.OTStatus).Distinct().ToList();
                //OTStatusList = new List<PlantDeliveryAnalysis>(TempPlantDeliveryAnalysisList);

                var TempPlantDeliveryAnalysisList1 = (from a in OTItemStatusList
                                                      select new
                                                      {
                                                          a.OTItemStatus,


                                                      }).Distinct().ToList();
                OTItemStatusList = new List<PlantLoadAnalysis>();

                foreach (var item1 in TempPlantDeliveryAnalysisList1)
                {
                    PlantLoadAnalysis selectedvalue = new PlantLoadAnalysis();
                    selectedvalue.OTItemStatus = item1.OTItemStatus;
                    //  selectedvalue.TemplateName = PlantDeliveryAnalysisList.Where(a => a.OTStatus == item1.OTStatus).FirstOrDefault().TemplateName;
                    //  selectedvalue.IdCustomer = PlantDeliveryAnalysisList.Where(b => b.OTStatus == item1.OTStatus).FirstOrDefault().IdCustomer;

                    OTItemStatusList.Add(selectedvalue);

                }
                OTItemStatusList = new List<PlantLoadAnalysis>(OTItemStatusList);
                OTItemStatusList_Cloned = new List<PlantLoadAnalysis>();
                OTItemStatusList_Cloned = OTItemStatusList.ToList();
                GeosApplication.Instance.Logger.Log("Method FillOTItemStatus()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillTemplate() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ChangeRegionCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChangeRegionCommandAction()...", category: Category.Info, priority: Priority.Low);

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

                        var TempSelectedRegion = (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).EditValue);
                        List<Object> TmpSelectedRegion = new List<object>();

                        if (TempSelectedRegion != null)
                        {
                            foreach (var tmpRegion in (dynamic)TempSelectedRegion)
                            {
                                TmpSelectedRegion.Add(tmpRegion);
                            }

                            SelectedRegion = new List<object>();
                            SelectedRegion = TmpSelectedRegion;
                        }

                        if (SelectedRegion == null) SelectedRegion = new List<object>();

                        List<string> RegionIds = SelectedRegion.Select(i => Convert.ToString((i as PlantLoadAnalysis).Region)).Distinct().ToList();
                        // List<PlantDeliveryAnalysis> filteredData = new List<PlantDeliveryAnalysis>(PlantDeliveryAnalysisList.Where(i => SelectedRegion.Contains(i.Region)).Distinct().ToList());

                        List<PlantLoadAnalysis> filteredData = new List<PlantLoadAnalysis>(plantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(Convert.ToString(i.Region))).ToList());
                        TemplateList = new List<PlantLoadAnalysis>(filteredData);
                        OTItemStatusList = new List<PlantLoadAnalysis>(filteredData);
                        CustomerGroupList = new List<PlantLoadAnalysis>(filteredData);
                        CustomerPlantList = new List<PlantLoadAnalysis>(filteredData);
                        ProjectList = new List<PlantLoadAnalysis>(filteredData);
                        OTCodeList = new List<PlantLoadAnalysis>(filteredData);
                        ConnectorFamilyList = new List<PlantLoadAnalysis>(filteredData);
                        CPTypeList = new List<PlantLoadAnalysis>(filteredData);
                        List<PlantLoadAnalysis> TempTemplateList = new List<PlantLoadAnalysis>();
                        List<PlantLoadAnalysis> TempOtStatusList = new List<PlantLoadAnalysis>();
                        List<PlantLoadAnalysis> TempcustomergroupList = new List<PlantLoadAnalysis>();
                        List<PlantLoadAnalysis> TempcustomerPlantList = new List<PlantLoadAnalysis>();
                        List<PlantLoadAnalysis> TempProjectList = new List<PlantLoadAnalysis>();
                        List<PlantLoadAnalysis> TempOTcodeList = new List<PlantLoadAnalysis>();
                        List<PlantLoadAnalysis> TempConnectorFamilyList = new List<PlantLoadAnalysis>();
                        List<PlantLoadAnalysis> TempCPTypeList = new List<PlantLoadAnalysis>();
                        TempTemplateList.AddRange(TemplateList.GroupBy(i => i.Template)
                                                                                .Select(grp => grp.First())
                                                                                .ToList().Distinct());

                        TempOtStatusList.AddRange(OTItemStatusList.GroupBy(i => i.OTItemStatus)
                                                                                .Select(grp => grp.First())
                                                                                .ToList().Distinct());
                        TempcustomergroupList.AddRange(CustomerGroupList.GroupBy(i => i.CustomerGroup)
                                                                              .Select(grp => grp.First())
                                                                              .ToList().Distinct());
                        TempcustomerPlantList.AddRange(CustomerPlantList.GroupBy(i => i.CustomerPlant)
                                                                              .Select(grp => grp.First())
                                                                              .ToList().Distinct());
                        TempProjectList.AddRange(ProjectList.GroupBy(i => i.Project)
                                                                          .Select(grp => grp.First())
                                                                          .ToList().Distinct());
                        TempOTcodeList.AddRange(OTCodeList.GroupBy(i => i.OTCode)
                                                                         .Select(grp => grp.First())
                                                                         .ToList().Distinct());
                        TempConnectorFamilyList.AddRange(ConnectorFamilyList.GroupBy(i => i.ConnectorFamily)
                                                                       .Select(grp => grp.First())
                                                                       .ToList().Distinct());
                        TempCPTypeList.AddRange(CPTypeList.GroupBy(i => i.CPType)
                                                                       .Select(grp => grp.First())
                                                                       .ToList().Distinct());
                        TemplateList = new List<PlantLoadAnalysis>(TempTemplateList);
                        OTItemStatusList = new List<PlantLoadAnalysis>(TempOtStatusList);
                        CustomerGroupList = new List<PlantLoadAnalysis>(TempcustomergroupList);
                        CustomerPlantList = new List<PlantLoadAnalysis>(TempcustomerPlantList);
                        ProjectList = new List<PlantLoadAnalysis>(TempProjectList);
                        OTCodeList = new List<PlantLoadAnalysis>(TempOTcodeList);
                        ConnectorFamilyList = new List<PlantLoadAnalysis>(TempConnectorFamilyList);
                        CPTypeList = new List<PlantLoadAnalysis>(TempCPTypeList);
                        // SelectAllData();
                        SelectedTemplate = new List<object>(TemplateList.ToList());
                        SelectedOTItemStatus = new List<object>(OTItemStatusList.ToList());
                        SelectedCustomerGroup = new List<object>(CustomerGroupList.ToList());
                        SelectedCustomerPlant = new List<object>(CustomerPlantList.ToList());
                        SelectedOTCode = new List<object>(OTCodeList.ToList());
                        SelectedProject = new List<object>(ProjectList.ToList());
                        SelectedConnectorFamily = new List<object>(ConnectorFamilyList.ToList());
                        SelectedCPType = new List<object>(CPTypeList.ToList());


                        ApplyFilterConditions();
                        AddColumnsToDataTableWithBandsinLoadModules();
                        FillLoadModulesData();
                        GroupBY();
                        AddColumnsToDataTableWithBandsinLoadEquipments();
                        FillLoadEquipmentsData(); //[GEOS2-5037][Rupali Sarode][21-12-2023]
                        GridControlLoadEquipmentFilter.GroupBy("CalendarWeekEquipment");
                        FillLoadWorkstationData();
                        FillLoadCustomerData();
                        CreateXmlFile(); //[GEOS2-5114][Rupali Sarode][29-12-2023]

                    }
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method ChangeRegionCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ChangeRegionCommandAction()-2 Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        private void ChangeCustomerGroupCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChangeCustomerGroupCommandAction()...", category: Category.Info, priority: Priority.Low);

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

                        var TmpSelectedCustomer = (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).EditValue);
                        List<Object> TmpSelectedCustomerGroup = new List<object>();

                        if (TmpSelectedCustomer != null)
                        {
                            foreach (var tmpRegion in (dynamic)TmpSelectedCustomer)
                            {
                                TmpSelectedCustomerGroup.Add(tmpRegion);
                            }

                            SelectedCustomerGroup = new List<object>();
                            SelectedCustomerGroup = TmpSelectedCustomerGroup;
                        }

                        if (SelectedCustomerGroup == null) SelectedCustomerGroup = new List<object>();

                        List<int> CustomerIds = SelectedCustomerGroup.Select(i => Convert.ToInt32((i as PlantLoadAnalysis).IdCustomer)).Distinct().ToList();
                        // List<PlantDeliveryAnalysis> filteredData = new List<PlantDeliveryAnalysis>(PlantDeliveryAnalysisList.Where(i => SelectedRegion.Contains(i.Region)).Distinct().ToList());

                        List<PlantLoadAnalysis> filteredData = new List<PlantLoadAnalysis>(
                            plantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(Convert.ToInt32(i.IdCustomer)) &&
                            SelectedOTItemStatus.Select(a => (a as PlantLoadAnalysis).OTItemStatus).Contains(Convert.ToString(i.OTItemStatus)) &&
                            SelectedTemplate.Select(b => (b as PlantLoadAnalysis).Template).Contains(Convert.ToString(i.Template)) &&
                            SelectedRegion.Select(c => (c as PlantLoadAnalysis).Region).Contains(Convert.ToString(i.Region))).ToList());

                        CustomerPlantList = new List<PlantLoadAnalysis>(filteredData);
                        ProjectList = new List<PlantLoadAnalysis>(filteredData);
                        OTCodeList = new List<PlantLoadAnalysis>(filteredData);
                        ConnectorFamilyList = new List<PlantLoadAnalysis>(filteredData);
                        CPTypeList = new List<PlantLoadAnalysis>(filteredData);

                        List<PlantLoadAnalysis> TempProjectList = new List<PlantLoadAnalysis>();
                        List<PlantLoadAnalysis> TempOTcodeList = new List<PlantLoadAnalysis>();
                        List<PlantLoadAnalysis> TempConnectorFamilyList = new List<PlantLoadAnalysis>();
                        List<PlantLoadAnalysis> TempCPTypeList = new List<PlantLoadAnalysis>();

                        List<PlantLoadAnalysis> TempCustomerPlantList = new List<PlantLoadAnalysis>();
                        TempCustomerPlantList.AddRange(CustomerPlantList.GroupBy(i => i.CustomerPlant)
                                                                                  .Select(grp => grp.First())
                                                                                  .ToList().Distinct());

                        CustomerPlantList = new List<PlantLoadAnalysis>(TempCustomerPlantList);

                        TempProjectList.AddRange(ProjectList.GroupBy(i => i.Project)
                                                                          .Select(grp => grp.First())
                                                                          .ToList().Distinct());
                        TempOTcodeList.AddRange(OTCodeList.GroupBy(i => i.OTCode)
                                                                         .Select(grp => grp.First())
                                                                         .ToList().Distinct());
                        TempConnectorFamilyList.AddRange(ConnectorFamilyList.GroupBy(i => i.ConnectorFamily)
                                                                       .Select(grp => grp.First())
                                                                       .ToList().Distinct());
                        TempCPTypeList.AddRange(CPTypeList.GroupBy(i => i.CPType)
                                                                       .Select(grp => grp.First())
                                                                       .ToList().Distinct());
                        // TemplateList = new List<PlantLoadAnalysis>(TempTemplateList);
                        //OTItemStatusList = new List<PlantLoadAnalysis>(TempOtStatusList);
                        //     CustomerGroupList = new List<PlantLoadAnalysis>(TempcustomergroupList);
                        //CustomerPlantList = new List<PlantLoadAnalysis>(TempcustomerPlantList);
                        ProjectList = new List<PlantLoadAnalysis>(TempProjectList);
                        OTCodeList = new List<PlantLoadAnalysis>(TempOTcodeList);
                        ConnectorFamilyList = new List<PlantLoadAnalysis>(TempConnectorFamilyList);
                        CPTypeList = new List<PlantLoadAnalysis>(TempCPTypeList);
                        //  SelectAllData();
                        SelectedCustomerPlant = new List<object>(CustomerPlantList.ToList());
                        SelectedOTCode = new List<object>(OTCodeList.ToList());
                        SelectedProject = new List<object>(ProjectList.ToList());
                        SelectedConnectorFamily = new List<object>(ConnectorFamilyList.ToList());
                        SelectedCPType = new List<object>(CPTypeList.ToList());
                        ApplyFilterConditions();
                        AddColumnsToDataTableWithBandsinLoadModules();
                        FillLoadModulesData();
                        GroupBY();
                        AddColumnsToDataTableWithBandsinLoadEquipments();
                        FillLoadEquipmentsData(); //[GEOS2-5037][Rupali Sarode][21-12-2023]
                        GridControlLoadEquipmentFilter.GroupBy("CalendarWeekEquipment");
                        FillLoadWorkstationData();
                        FillLoadCustomerData();
                        CreateXmlFile(); //[GEOS2-5114][Rupali Sarode][29-12-2023]
                    }
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method ChangeCustomerGroupCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ChangeCustomerGroupCommandAction()-2 Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
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

                        var TmpSelectedPlant = (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).EditValue);
                        List<Object> TmpSelectedCustomerPlant = new List<object>();

                        if (TmpSelectedPlant != null)
                        {
                            foreach (var tmpRegion in (dynamic)TmpSelectedPlant)
                            {
                                TmpSelectedCustomerPlant.Add(tmpRegion);
                            }

                            SelectedCustomerPlant = new List<object>();
                            SelectedCustomerPlant = TmpSelectedCustomerPlant;
                        }

                        if (SelectedCustomerPlant == null) SelectedCustomerPlant = new List<object>();

                        List<string> CustomerPlant = SelectedCustomerPlant.Select(i => Convert.ToString((i as PlantLoadAnalysis).CustomerPlant)).Distinct().ToList();
                        // List<PlantDeliveryAnalysis> filteredData = new List<PlantDeliveryAnalysis>(PlantDeliveryAnalysisList.Where(i => SelectedRegion.Contains(i.Region)).Distinct().ToList());

                        List<PlantLoadAnalysis> filteredData = new List<PlantLoadAnalysis>(plantLoadAnalysisList_Cloned.Where(i => CustomerPlant.Contains(Convert.ToString(i.CustomerPlant)) &&
                        SelectedCustomerGroup.Select(a => (a as PlantLoadAnalysis).CustomerGroup).Contains(Convert.ToString(i.CustomerGroup)) &&
                        SelectedOTItemStatus.Select(b => (b as PlantLoadAnalysis).OTItemStatus).Contains(Convert.ToString(i.OTItemStatus)) &&
                        SelectedTemplate.Select(c => (c as PlantLoadAnalysis).Template).Contains(Convert.ToString(i.Template)) &&
                        SelectedRegion.Select(d => (d as PlantLoadAnalysis).Region).Contains(Convert.ToString(i.Region))).ToList());

                        ProjectList = new List<PlantLoadAnalysis>(filteredData.Where(i => i.Project != null).ToList());
                        // ProjectList = new List<PlantLoadAnalysis>(filteredData);
                        List<PlantLoadAnalysis> TempProjectList = new List<PlantLoadAnalysis>();
                        TempProjectList.AddRange(ProjectList.GroupBy(i => i.Project)
                                                                                  .Select(grp => grp.First())
                                                                                  .ToList().Distinct());

                        ProjectList = new List<PlantLoadAnalysis>(TempProjectList);
                        OTCodeList = new List<PlantLoadAnalysis>(filteredData);
                        ConnectorFamilyList = new List<PlantLoadAnalysis>(filteredData);
                        CPTypeList = new List<PlantLoadAnalysis>(filteredData);
                        List<PlantLoadAnalysis> TempOTcodeList = new List<PlantLoadAnalysis>();
                        List<PlantLoadAnalysis> TempConnectorFamilyList = new List<PlantLoadAnalysis>();
                        List<PlantLoadAnalysis> TempCPTypeList = new List<PlantLoadAnalysis>();

                        List<PlantLoadAnalysis> TempCustomerPlantList = new List<PlantLoadAnalysis>();


                        TempOTcodeList.AddRange(OTCodeList.GroupBy(i => i.OTCode)
                                                                         .Select(grp => grp.First())
                                                                         .ToList().Distinct());
                        TempConnectorFamilyList.AddRange(ConnectorFamilyList.GroupBy(i => i.ConnectorFamily)
                                                                       .Select(grp => grp.First())
                                                                       .ToList().Distinct());
                        TempCPTypeList.AddRange(CPTypeList.GroupBy(i => i.CPType)
                                                                       .Select(grp => grp.First())
                                                                       .ToList().Distinct());
                        // TemplateList = new List<PlantLoadAnalysis>(TempTemplateList);
                        //OTItemStatusList = new List<PlantLoadAnalysis>(TempOtStatusList);
                        //     CustomerGroupList = new List<PlantLoadAnalysis>(TempcustomergroupList);
                        //  CustomerPlantList = new List<PlantLoadAnalysis>(TempcustomerPlantList);
                        // ProjectList = new List<PlantLoadAnalysis>(TempProjectList);
                        OTCodeList = new List<PlantLoadAnalysis>(TempOTcodeList);
                        ConnectorFamilyList = new List<PlantLoadAnalysis>(TempConnectorFamilyList);
                        CPTypeList = new List<PlantLoadAnalysis>(TempCPTypeList);
                        if (ProjectList.Count == 0)
                        {
                            List<PlantLoadAnalysis> OTCodeListTemp = new List<PlantLoadAnalysis>();
                            // OTCodeListTemp = plantLoadAnalysisList_Cloned.Where(a => a.Project == null).ToList();
                            OTCodeListTemp = plantLoadAnalysisList_Cloned.Where(a => a.Project == null && CustomerPlant.Contains(Convert.ToString(a.CustomerPlant)) && SelectedCustomerGroup.Select(b => (b as PlantLoadAnalysis).CustomerGroup).Contains(Convert.ToString(a.CustomerGroup)) && SelectedRegion.Select(c => (c as PlantLoadAnalysis).Region).Contains(Convert.ToString(a.Region))).ToList();

                            OTCodeList = new List<PlantLoadAnalysis>(OTCodeListTemp);
                            List<PlantLoadAnalysis> OTCodeListGroupBy = new List<PlantLoadAnalysis>();
                            OTCodeListGroupBy.AddRange(OTCodeList.GroupBy(i => i.OTCode)
                                                                                 .Select(grp => grp.First())
                                                                                 .ToList().Distinct());
                            OTCodeList = new List<PlantLoadAnalysis>(OTCodeListGroupBy);
                        }
                        //   SelectAllData();
                        SelectedProject = new List<object>(ProjectList.ToList());
                        SelectedOTCode = new List<object>(OTCodeList.ToList());
                        SelectedConnectorFamily = new List<object>(ConnectorFamilyList.ToList());
                        SelectedCPType = new List<object>(CPTypeList.ToList());
                        ApplyFilterConditions();
                        AddColumnsToDataTableWithBandsinLoadModules();
                        FillLoadModulesData();
                        GroupBY();
                        AddColumnsToDataTableWithBandsinLoadEquipments();
                        FillLoadEquipmentsData(); //[GEOS2-5037][Rupali Sarode][21-12-2023]
                        GridControlLoadEquipmentFilter.GroupBy("CalendarWeekEquipment");
                        FillLoadWorkstationData();
                        FillLoadCustomerData();
                        CreateXmlFile(); //[GEOS2-5114][Rupali Sarode][29-12-2023]
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

        private void ChangeOTCodeCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChangeOTCodeCommandAction()...", category: Category.Info, priority: Priority.Low);

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

                        var TmpSelectedOTCode = (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).EditValue);
                        List<Object> TmpSelectedCode = new List<object>();

                        if (TmpSelectedOTCode != null)
                        {
                            foreach (var tmpRegion in (dynamic)TmpSelectedOTCode)
                            {
                                TmpSelectedCode.Add(tmpRegion);
                            }

                            SelectedOTCode = new List<object>();
                            SelectedOTCode = TmpSelectedCode;
                        }

                        if (SelectedOTCode == null) SelectedOTCode = new List<object>();

                        List<string> OTCode = SelectedOTCode.Select(i => Convert.ToString((i as PlantLoadAnalysis).OTCode)).Distinct().ToList();
                        // List<PlantDeliveryAnalysis> filteredData = new List<PlantDeliveryAnalysis>(PlantDeliveryAnalysisList.Where(i => SelectedRegion.Contains(i.Region)).Distinct().ToList());

                        List<PlantLoadAnalysis> filteredData = new List<PlantLoadAnalysis>(plantLoadAnalysisList_Cloned.Where(i => OTCode.Contains(Convert.ToString(i.OTCode)) &&
                        SelectedProject.Select(a => (a as PlantLoadAnalysis).Project).Contains(Convert.ToString(i.Project)) &&
                        SelectedCustomerPlant.Select(b => (b as PlantLoadAnalysis).CustomerPlant).Contains(Convert.ToString(i.CustomerPlant)) &&
                        SelectedCustomerGroup.Select(c => (c as PlantLoadAnalysis).CustomerGroup).Contains(Convert.ToString(i.CustomerGroup)) &&
                        SelectedOTItemStatus.Select(d => (d as PlantLoadAnalysis).OTItemStatus).Contains(Convert.ToString(i.OTItemStatus)) &&
                        SelectedTemplate.Select(e => (e as PlantLoadAnalysis).Template).Contains(Convert.ToString(i.Template)) &&
                        SelectedRegion.Select(f => (f as PlantLoadAnalysis).Region).Contains(Convert.ToString(i.Region))).ToList());



                        ConnectorFamilyList = new List<PlantLoadAnalysis>(filteredData);
                        CPTypeList = new List<PlantLoadAnalysis>(filteredData);
                        List<PlantLoadAnalysis> TempCPTypeList = new List<PlantLoadAnalysis>();
                        List<PlantLoadAnalysis> TempConnectorFamilyList = new List<PlantLoadAnalysis>();
                        TempConnectorFamilyList.AddRange(ConnectorFamilyList.GroupBy(i => i.ConnectorFamily)
                                                                                  .Select(grp => grp.First())
                                                                                  .ToList().Distinct());

                        ConnectorFamilyList = new List<PlantLoadAnalysis>(TempConnectorFamilyList.Where(a => a.ConnectorFamily != null).ToList());

                        TempCPTypeList.AddRange(CPTypeList.GroupBy(i => i.CPType)
                                                                       .Select(grp => grp.First())
                                                                       .ToList().Distinct());
                        CPTypeList = new List<PlantLoadAnalysis>(TempCPTypeList);
                        if (ConnectorFamilyList.Count == 0)
                        {
                            CPTypeList = new List<PlantLoadAnalysis>();
                        }
                        SelectedConnectorFamily = new List<object>(ConnectorFamilyList.ToList());
                        SelectedCPType = new List<object>(CPTypeList.ToList());
                        // SelectAllData();
                        ApplyFilterConditions();
                        AddColumnsToDataTableWithBandsinLoadModules();
                        FillLoadModulesData();
                        GroupBY();
                        AddColumnsToDataTableWithBandsinLoadEquipments();
                        FillLoadEquipmentsData(); //[GEOS2-5037][Rupali Sarode][21-12-2023]
                        GridControlLoadEquipmentFilter.GroupBy("CalendarWeekEquipment");
                        FillLoadWorkstationData();
                        FillLoadCustomerData();
                        CreateXmlFile(); //[GEOS2-5114][Rupali Sarode][29-12-2023]
                    }
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method ChangeOTCodeCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ChangeOTCodeCommandAction()-2 Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
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

                        var TmpSelectedProjet = (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).EditValue);
                        List<Object> TmpSelectedprojects = new List<object>();

                        if (TmpSelectedProjet != null)
                        {
                            foreach (var tmpRegion in (dynamic)TmpSelectedProjet)
                            {
                                TmpSelectedprojects.Add(tmpRegion);
                            }

                            SelectedProject = new List<object>();
                            SelectedProject = TmpSelectedprojects;
                        }

                        if (SelectedProject == null) SelectedProject = new List<object>();

                        List<string> Projects = SelectedProject.Select(i => Convert.ToString((i as PlantLoadAnalysis).Project)).Distinct().ToList();
                        // List<PlantDeliveryAnalysis> filteredData = new List<PlantDeliveryAnalysis>(PlantDeliveryAnalysisList.Where(i => SelectedRegion.Contains(i.Region)).Distinct().ToList());

                        List<PlantLoadAnalysis> filteredData = new List<PlantLoadAnalysis>(plantLoadAnalysisList_Cloned.Where(i => Projects.Contains(Convert.ToString(i.Project)) &&
                        SelectedCustomerPlant.Select(a => (a as PlantLoadAnalysis).CustomerPlant).Contains(Convert.ToString(i.CustomerPlant)) &&
                        SelectedCustomerGroup.Select(b => (b as PlantLoadAnalysis).CustomerGroup).Contains(Convert.ToString(i.CustomerGroup)) &&
                        SelectedOTItemStatus.Select(c => (c as PlantLoadAnalysis).OTItemStatus).Contains(Convert.ToString(i.OTItemStatus)) &&
                        SelectedTemplate.Select(d => (d as PlantLoadAnalysis).Template).Contains(Convert.ToString(i.Template)) &&
                        SelectedRegion.Select(e => (e as PlantLoadAnalysis).Region).Contains(Convert.ToString(i.Region))).ToList());

                        OTCodeList = new List<PlantLoadAnalysis>(filteredData);
                        ConnectorFamilyList = new List<PlantLoadAnalysis>(filteredData);
                        CPTypeList = new List<PlantLoadAnalysis>(filteredData);
                        List<PlantLoadAnalysis> TempConnectorFamilyList = new List<PlantLoadAnalysis>();
                        List<PlantLoadAnalysis> TempCPTypeList = new List<PlantLoadAnalysis>();
                        List<PlantLoadAnalysis> TempOTCodeList = new List<PlantLoadAnalysis>();
                        TempOTCodeList.AddRange(OTCodeList.GroupBy(i => i.OTCode)
                                                                                  .Select(grp => grp.First())
                                                                                  .ToList().Distinct());

                        OTCodeList = new List<PlantLoadAnalysis>(TempOTCodeList);

                        TempConnectorFamilyList.AddRange(ConnectorFamilyList.GroupBy(i => i.ConnectorFamily)
                                                                       .Select(grp => grp.First())
                                                                       .ToList().Distinct());
                        TempCPTypeList.AddRange(CPTypeList.GroupBy(i => i.CPType)
                                                                       .Select(grp => grp.First())
                                                                       .ToList().Distinct());
                        // TemplateList = new List<PlantLoadAnalysis>(TempTemplateList);
                        //OTItemStatusList = new List<PlantLoadAnalysis>(TempOtStatusList);
                        //     CustomerGroupList = new List<PlantLoadAnalysis>(TempcustomergroupList);
                        //  CustomerPlantList = new List<PlantLoadAnalysis>(TempcustomerPlantList);
                        // ProjectList = new List<PlantLoadAnalysis>(TempProjectList);

                        ConnectorFamilyList = new List<PlantLoadAnalysis>(TempConnectorFamilyList);
                        CPTypeList = new List<PlantLoadAnalysis>(TempCPTypeList);
                        if (OTCodeList.Count == 0)
                        {
                            ConnectorFamilyList = new List<PlantLoadAnalysis>();
                            CPTypeList = new List<PlantLoadAnalysis>();
                        }
                        //  SelectAllData();
                        SelectedOTCode = new List<object>(OTCodeList.ToList());
                        SelectedConnectorFamily = new List<object>(ConnectorFamilyList.ToList());
                        SelectedCPType = new List<object>(CPTypeList.ToList());
                        ApplyFilterConditions();
                        AddColumnsToDataTableWithBandsinLoadModules();
                        FillLoadModulesData();
                        GroupBY();
                        AddColumnsToDataTableWithBandsinLoadEquipments();
                        FillLoadEquipmentsData(); //[GEOS2-5037][Rupali Sarode][21-12-2023]
                        GridControlLoadEquipmentFilter.GroupBy("CalendarWeekEquipment");
                        FillLoadWorkstationData();
                        FillLoadCustomerData();
                        CreateXmlFile(); //[GEOS2-5114][Rupali Sarode][29-12-2023]
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

        private void ChangeConnectorFamilyCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChangeConnectorFamilyCommandAction()...", category: Category.Info, priority: Priority.Low);

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

                        var TmpSelectedConnectorFamily = (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).EditValue);
                        List<Object> TmpSelectedConnectorFamilys = new List<object>();

                        if (TmpSelectedConnectorFamily != null)
                        {
                            foreach (var tmpRegion in (dynamic)TmpSelectedConnectorFamily)
                            {
                                TmpSelectedConnectorFamilys.Add(tmpRegion);
                            }

                            SelectedConnectorFamily = new List<object>();
                            SelectedConnectorFamily = TmpSelectedConnectorFamilys;
                        }

                        if (SelectedConnectorFamily == null) SelectedConnectorFamily = new List<object>();

                        List<string> ConnectorFamily = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                        // List<PlantDeliveryAnalysis> filteredData = new List<PlantDeliveryAnalysis>(PlantDeliveryAnalysisList.Where(i => SelectedRegion.Contains(i.Region)).Distinct().ToList());

                        List<PlantLoadAnalysis> filteredData = new List<PlantLoadAnalysis>(plantLoadAnalysisList_Cloned.Where(i => ConnectorFamily.Contains(Convert.ToString(i.ConnectorFamily)) &&
                        SelectedOTCode.Select(a => (a as PlantLoadAnalysis).OTCode).Contains(Convert.ToString(i.OTCode)) &&
                        SelectedProject.Select(b => (b as PlantLoadAnalysis).Project).Contains(Convert.ToString(i.Project)) &&
                        SelectedCustomerPlant.Select(c => (c as PlantLoadAnalysis).CustomerPlant).Contains(Convert.ToString(i.CustomerPlant)) &&
                        SelectedCustomerGroup.Select(d => (d as PlantLoadAnalysis).CustomerGroup).Contains(Convert.ToString(i.CustomerGroup)) &&
                        SelectedOTItemStatus.Select(e => (e as PlantLoadAnalysis).OTItemStatus).Contains(Convert.ToString(i.OTItemStatus)) &&
                        SelectedTemplate.Select(f => (f as PlantLoadAnalysis).Template).Contains(Convert.ToString(i.Template)) &&
                        SelectedRegion.Select(g => (g as PlantLoadAnalysis).Region).Contains(Convert.ToString(i.Region))).ToList());

                        CPTypeList = new List<PlantLoadAnalysis>(filteredData);
                        List<PlantLoadAnalysis> TempCPTypeList = new List<PlantLoadAnalysis>();
                        TempCPTypeList.AddRange(CPTypeList.GroupBy(i => i.CPType)
                                                                                  .Select(grp => grp.First())
                                                                                  .ToList().Distinct());

                        CPTypeList = new List<PlantLoadAnalysis>(TempCPTypeList.Where(b => b.CPType != null).ToList());
                        // SelectAllData();
                        // SelectedConnectorFamily = new List<object>(ConnectorFamilyList.ToList());
                        SelectedCPType = new List<object>(CPTypeList.ToList());
                        ApplyFilterConditions();
                        AddColumnsToDataTableWithBandsinLoadModules();
                        FillLoadModulesData();
                        GroupBY();
                        AddColumnsToDataTableWithBandsinLoadEquipments();
                        FillLoadEquipmentsData(); //[GEOS2-5037][Rupali Sarode][21-12-2023]
                        GridControlLoadEquipmentFilter.GroupBy("CalendarWeekEquipment");
                        FillLoadWorkstationData();
                        FillLoadCustomerData();
                        CreateXmlFile(); //[GEOS2-5114][Rupali Sarode][29-12-2023]
                    }
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method ChangeConnectorFamilyCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ChangeConnectorFamilyCommandAction()-2 Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ChangeCPTypeCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChangeCPTypeCommandAction()...", category: Category.Info, priority: Priority.Low);

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

                        var TmpSelectedCPType = (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).EditValue);
                        List<Object> TmpSelectedCpTypes = new List<object>();

                        if (TmpSelectedCPType != null)
                        {
                            foreach (var tmpRegion in (dynamic)TmpSelectedCPType)
                            {
                                TmpSelectedCpTypes.Add(tmpRegion);
                            }

                            SelectedCPType = new List<object>();
                            SelectedCPType = TmpSelectedCpTypes;
                        }

                        if (SelectedCPType == null) SelectedCPType = new List<object>();
                        //  SelectAllData();
                        // SelectedCPType = new List<object>(CPTypeList.ToList());
                        ApplyFilterConditions();
                        AddColumnsToDataTableWithBandsinLoadModules();
                        FillLoadModulesData();
                        GroupBY();
                        AddColumnsToDataTableWithBandsinLoadEquipments();
                        FillLoadEquipmentsData(); //[GEOS2-5037][Rupali Sarode][21-12-2023]
                        GridControlLoadEquipmentFilter.GroupBy("CalendarWeekEquipment");
                        FillLoadWorkstationData();
                        FillLoadCustomerData();
                        CreateXmlFile(); //[GEOS2-5114][Rupali Sarode][29-12-2023]

                    }
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method ChangeCPTypeCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ChangeCPTypeCommandAction()-2 Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ChangeTemplatesCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChangeTemplatesCommandAction()...", category: Category.Info, priority: Priority.Low);

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

                        var TmpSelectedTemplate = (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).EditValue);
                        List<Object> TmpSelectedTemplates = new List<object>();

                        if (TmpSelectedTemplate != null)
                        {
                            foreach (var tmpRegion in (dynamic)TmpSelectedTemplate)
                            {
                                TmpSelectedTemplates.Add(tmpRegion);
                            }

                            SelectedTemplate = new List<object>();
                            SelectedTemplate = TmpSelectedTemplates;
                        }

                        if (SelectedTemplate == null) SelectedTemplate = new List<object>();

                        List<string> Templates = SelectedTemplate.Select(i => Convert.ToString((i as PlantLoadAnalysis).Template)).Distinct().ToList();
                        // List<PlantDeliveryAnalysis> filteredData = new List<PlantDeliveryAnalysis>(PlantDeliveryAnalysisList.Where(i => SelectedRegion.Contains(i.Region)).Distinct().ToList());

                        List<PlantLoadAnalysis> filteredData = new List<PlantLoadAnalysis>(plantLoadAnalysisList_Cloned.Where(i => Templates.Contains(Convert.ToString(i.Template)) && SelectedRegion.Select(j => Convert.ToString((j as PlantLoadAnalysis).Region)).Contains(Convert.ToString(i.Region))).ToList());
                        OTItemStatusList = new List<PlantLoadAnalysis>(filteredData);
                        CustomerGroupList = new List<PlantLoadAnalysis>(filteredData);
                        CustomerPlantList = new List<PlantLoadAnalysis>(filteredData);
                        ProjectList = new List<PlantLoadAnalysis>(filteredData);
                        OTCodeList = new List<PlantLoadAnalysis>(filteredData);
                        ConnectorFamilyList = new List<PlantLoadAnalysis>(filteredData);
                        CPTypeList = new List<PlantLoadAnalysis>(filteredData);
                        List<PlantLoadAnalysis> TempOTItemStatusList = new List<PlantLoadAnalysis>();
                        TempOTItemStatusList.AddRange(OTItemStatusList.GroupBy(i => i.OTItemStatus)
                                                                                  .Select(grp => grp.First())
                                                                                  .ToList().Distinct());

                        OTItemStatusList = new List<PlantLoadAnalysis>(TempOTItemStatusList.Where(a => a.OTItemStatus != null).ToList());

                        List<PlantLoadAnalysis> TempOtStatusList = new List<PlantLoadAnalysis>();
                        List<PlantLoadAnalysis> TempcustomergroupList = new List<PlantLoadAnalysis>();
                        List<PlantLoadAnalysis> TempcustomerPlantList = new List<PlantLoadAnalysis>();
                        List<PlantLoadAnalysis> TempProjectList = new List<PlantLoadAnalysis>();
                        List<PlantLoadAnalysis> TempOTcodeList = new List<PlantLoadAnalysis>();
                        List<PlantLoadAnalysis> TempConnectorFamilyList = new List<PlantLoadAnalysis>();
                        List<PlantLoadAnalysis> TempCPTypeList = new List<PlantLoadAnalysis>();


                        TempOtStatusList.AddRange(OTItemStatusList.GroupBy(i => i.OTItemStatus)
                                                                                .Select(grp => grp.First())
                                                                                .ToList().Distinct());
                        TempcustomergroupList.AddRange(CustomerGroupList.GroupBy(i => i.CustomerGroup)
                                                                              .Select(grp => grp.First())
                                                                              .ToList().Distinct());
                        TempcustomerPlantList.AddRange(CustomerPlantList.GroupBy(i => i.CustomerPlant)
                                                                              .Select(grp => grp.First())
                                                                              .ToList().Distinct());
                        TempProjectList.AddRange(ProjectList.GroupBy(i => i.Project)
                                                                          .Select(grp => grp.First())
                                                                          .ToList().Distinct());
                        TempOTcodeList.AddRange(OTCodeList.GroupBy(i => i.OTCode)
                                                                         .Select(grp => grp.First())
                                                                         .ToList().Distinct());
                        TempConnectorFamilyList.AddRange(ConnectorFamilyList.GroupBy(i => i.ConnectorFamily)
                                                                       .Select(grp => grp.First())
                                                                       .ToList().Distinct());
                        TempCPTypeList.AddRange(CPTypeList.GroupBy(i => i.CPType)
                                                                       .Select(grp => grp.First())
                                                                       .ToList().Distinct());
                        // TemplateList = new List<PlantLoadAnalysis>(TempTemplateList);
                        OTItemStatusList = new List<PlantLoadAnalysis>(TempOtStatusList);
                        CustomerGroupList = new List<PlantLoadAnalysis>(TempcustomergroupList);
                        CustomerPlantList = new List<PlantLoadAnalysis>(TempcustomerPlantList);
                        ProjectList = new List<PlantLoadAnalysis>(TempProjectList);
                        OTCodeList = new List<PlantLoadAnalysis>(TempOTcodeList);
                        ConnectorFamilyList = new List<PlantLoadAnalysis>(TempConnectorFamilyList);
                        CPTypeList = new List<PlantLoadAnalysis>(TempCPTypeList);
                        // SelectAllData();
                        SelectedOTItemStatus = new List<object>(OTItemStatusList.ToList());
                        SelectedCustomerGroup = new List<object>(CustomerGroupList.ToList());
                        SelectedCustomerPlant = new List<object>(CustomerPlantList.ToList());
                        SelectedOTCode = new List<object>(OTCodeList.ToList());
                        SelectedProject = new List<object>(ProjectList.ToList());
                        SelectedConnectorFamily = new List<object>(ConnectorFamilyList.ToList());
                        SelectedCPType = new List<object>(CPTypeList.ToList());
                        ApplyFilterConditions();
                        AddColumnsToDataTableWithBandsinLoadModules();
                        FillLoadModulesData();
                        GroupBY();
                        AddColumnsToDataTableWithBandsinLoadEquipments();
                        FillLoadEquipmentsData(); //[GEOS2-5037][Rupali Sarode][21-12-2023]
                        GridControlLoadEquipmentFilter.GroupBy("CalendarWeekEquipment");
                        FillLoadWorkstationData();
                        FillLoadCustomerData();
                        CreateXmlFile(); //[GEOS2-5114][Rupali Sarode][29-12-2023]
                    }
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method ChangeTemplatesCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ChangeTemplatesCommandAction()-2 Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ChangeOTItemStatusCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChangeOTItemStatusCommandAction()...", category: Category.Info, priority: Priority.Low);

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

                        var TmpSelectedCustomerGroup = (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).EditValue);
                        List<Object> TmpSelectedCustomerGroups = new List<object>();

                        if (TmpSelectedCustomerGroup != null)
                        {
                            foreach (var tmpRegion in (dynamic)TmpSelectedCustomerGroup)
                            {
                                TmpSelectedCustomerGroups.Add(tmpRegion);
                            }

                            SelectedOTItemStatus = new List<object>();
                            SelectedOTItemStatus = TmpSelectedCustomerGroups;
                        }

                        if (SelectedOTItemStatus == null) SelectedOTItemStatus = new List<object>();

                        List<string> OTItemStatus = SelectedOTItemStatus.Select(i => Convert.ToString((i as PlantLoadAnalysis).OTItemStatus)).Distinct().ToList();
                        // List<PlantDeliveryAnalysis> filteredData = new List<PlantDeliveryAnalysis>(PlantDeliveryAnalysisList.Where(i => SelectedRegion.Contains(i.Region)).Distinct().ToList());

                        List<PlantLoadAnalysis> filteredData = new List<PlantLoadAnalysis>(plantLoadAnalysisList_Cloned.Where(i => OTItemStatus.Contains(Convert.ToString(i.OTItemStatus)) && SelectedTemplate.Select(j => (j as PlantLoadAnalysis).Template).Contains(Convert.ToString(i.Template)) && SelectedRegion.Select(k => (k as PlantLoadAnalysis).Region).Contains(Convert.ToString(i.Region))).ToList());
                        CustomerGroupList = new List<PlantLoadAnalysis>(filteredData);
                        CustomerPlantList = new List<PlantLoadAnalysis>(filteredData);
                        ProjectList = new List<PlantLoadAnalysis>(filteredData);
                        OTCodeList = new List<PlantLoadAnalysis>(filteredData);
                        ConnectorFamilyList = new List<PlantLoadAnalysis>(filteredData);
                        CPTypeList = new List<PlantLoadAnalysis>(filteredData);

                        List<PlantLoadAnalysis> TempCustomerGroupList = new List<PlantLoadAnalysis>();
                        List<PlantLoadAnalysis> TempcustomergroupList = new List<PlantLoadAnalysis>();
                        List<PlantLoadAnalysis> TempcustomerPlantList = new List<PlantLoadAnalysis>();
                        List<PlantLoadAnalysis> TempProjectList = new List<PlantLoadAnalysis>();
                        List<PlantLoadAnalysis> TempOTcodeList = new List<PlantLoadAnalysis>();
                        List<PlantLoadAnalysis> TempConnectorFamilyList = new List<PlantLoadAnalysis>();
                        List<PlantLoadAnalysis> TempCPTypeList = new List<PlantLoadAnalysis>();
                        TempCustomerGroupList.AddRange(CustomerGroupList.GroupBy(i => i.CustomerGroup)
                                                                                  .Select(grp => grp.First())
                                                                                  .ToList().Distinct());

                        CustomerGroupList = new List<PlantLoadAnalysis>(TempCustomerGroupList);

                        TempcustomergroupList.AddRange(CustomerGroupList.GroupBy(i => i.CustomerGroup)
                                                                              .Select(grp => grp.First())
                                                                              .ToList().Distinct());
                        TempcustomerPlantList.AddRange(CustomerPlantList.GroupBy(i => i.CustomerPlant)
                                                                              .Select(grp => grp.First())
                                                                              .ToList().Distinct());
                        TempProjectList.AddRange(ProjectList.GroupBy(i => i.Project)
                                                                          .Select(grp => grp.First())
                                                                          .ToList().Distinct());
                        TempOTcodeList.AddRange(OTCodeList.GroupBy(i => i.OTCode)
                                                                         .Select(grp => grp.First())
                                                                         .ToList().Distinct());
                        TempConnectorFamilyList.AddRange(ConnectorFamilyList.GroupBy(i => i.ConnectorFamily)
                                                                       .Select(grp => grp.First())
                                                                       .ToList().Distinct());
                        TempCPTypeList.AddRange(CPTypeList.GroupBy(i => i.CPType)
                                                                       .Select(grp => grp.First())
                                                                       .ToList().Distinct());
                        // TemplateList = new List<PlantLoadAnalysis>(TempTemplateList);
                        //OTItemStatusList = new List<PlantLoadAnalysis>(TempOtStatusList);
                        //CustomerGroupList = new List<PlantLoadAnalysis>(TempcustomergroupList);
                        CustomerPlantList = new List<PlantLoadAnalysis>(TempcustomerPlantList);
                        ProjectList = new List<PlantLoadAnalysis>(TempProjectList);
                        OTCodeList = new List<PlantLoadAnalysis>(TempOTcodeList);
                        ConnectorFamilyList = new List<PlantLoadAnalysis>(TempConnectorFamilyList);
                        CPTypeList = new List<PlantLoadAnalysis>(TempCPTypeList);
                        SelectedCustomerGroup = new List<object>(CustomerGroupList.ToList());
                        SelectedCustomerPlant = new List<object>(CustomerPlantList.ToList());
                        SelectedOTCode = new List<object>(OTCodeList.ToList());
                        SelectedProject = new List<object>(ProjectList.ToList());
                        SelectedConnectorFamily = new List<object>(ConnectorFamilyList.ToList());
                        SelectedCPType = new List<object>(CPTypeList.ToList());
                        // SelectAllData();
                        ApplyFilterConditions();
                        AddColumnsToDataTableWithBandsinLoadModules();
                        FillLoadModulesData();
                        GroupBY();
                        AddColumnsToDataTableWithBandsinLoadEquipments();
                        FillLoadEquipmentsData(); //[GEOS2-5037][Rupali Sarode][21-12-2023]
                        GridControlLoadEquipmentFilter.GroupBy("CalendarWeekEquipment");
                        FillLoadWorkstationData();
                        FillLoadCustomerData();
                        CreateXmlFile(); //[GEOS2-5114][Rupali Sarode][29-12-2023]
                    }
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method ChangeOTItemStatusCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ChangeOTItemStatusCommandAction()-2 Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillAllselectedFilter()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillAllselectedFilter()...", category: Category.Info, priority: Priority.Low);
                if (SelectedRegion == null) SelectedRegion = new List<object>();
                if (SelectedCustomerGroup == null) SelectedCustomerGroup = new List<object>();
                if (SelectedCustomerPlant == null) SelectedCustomerPlant = new List<object>();
                if (SelectedOTCode == null) SelectedOTCode = new List<object>();
                if (SelectedProject == null) SelectedProject = new List<object>();
                if (SelectedConnectorFamily == null) SelectedConnectorFamily = new List<object>();
                if (SelectedCPType == null) SelectedCPType = new List<object>();
                if (SelectedTemplate == null) SelectedTemplate = new List<object>();
                if (SelectedOTItemStatus == null) SelectedOTItemStatus = new List<object>();


                #region [GEOS2-5114][28-12-2023][Rupali Sarode]

                if (!File.Exists(PlantLoadAnalysisFilterSettingFilePath))
                {
                    //SelectedRegion.AddRange(RegionList.ToList());
                    //SelectedCustomerGroup.AddRange(CustomerGroupList.ToList());
                    //SelectedCustomerPlant.AddRange(CustomerPlantList.ToList());
                    //SelectedOTCode.AddRange(OTCodeList.ToList());
                    //SelectedProject.AddRange(ProjectList.ToList());
                    //SelectedConnectorFamily.AddRange(ConnectorFamilyList.ToList());
                    //SelectedCPType.AddRange(CPTypeList.ToList());
                    //SelectedTemplate.AddRange(TemplateList.ToList());
                    //SelectedOTItemStatus.AddRange(OTItemStatusList.ToList());

                    SelectedRegion = new List<object>(RegionList.ToList());
                    SelectedCustomerGroup = new List<object>(CustomerGroupList.ToList());
                    SelectedCustomerPlant = new List<object>(CustomerPlantList.ToList());
                    SelectedOTCode = new List<object>(OTCodeList.ToList());
                    SelectedProject = new List<object>(ProjectList.ToList());
                    SelectedConnectorFamily = new List<object>(ConnectorFamilyList.ToList());
                    SelectedCPType = new List<object>(CPTypeList.ToList());
                    SelectedTemplate = new List<object>(TemplateList.ToList());
                    SelectedOTItemStatus = new List<object>(OTItemStatusList.ToList());
                }
                else
                {
                    // Region
                    if (TempXMLRegionsList.Count == 0)  // If nothing is selected previously 
                        SelectedRegion = new List<object>();
                    else if (RegionList.Where(i => TempXMLRegionsList.Contains(i.Region)).Distinct().Count() == 0)  // (6)	 If the user selection values are not present in the DDL must be selected ALL per default
                        SelectedRegion = new List<object>(RegionList.ToList());
                    else
                    {
                        SelectedRegion = new List<object>(RegionList.Where(i => TempXMLRegionsList.Contains(i.Region)).Distinct().ToList());

                        List<string> Regions = SelectedRegion.Select(i => Convert.ToString((i as PlantLoadAnalysis).Region)).Distinct().ToList();

                        List<PlantLoadAnalysis> filteredData = new List<PlantLoadAnalysis>(plantLoadAnalysisList_Cloned.Where(i => Regions.Contains(Convert.ToString(i.Region))).ToList());
                        TemplateList = new List<PlantLoadAnalysis>(filteredData);
                        List<PlantLoadAnalysis> TempTemplateList = new List<PlantLoadAnalysis>();

                        TempTemplateList.AddRange(TemplateList.GroupBy(i => i.Template)
                                                                                .Select(grp => grp.First())
                                                                                .ToList().Distinct());

                        TemplateList = new List<PlantLoadAnalysis>(TempTemplateList);

                    }

                    //Template
                    if (TempXMLTemplatesList.Count == 0)
                        SelectedTemplate = new List<object>();
                    else if (TemplateList.Where(i => TempXMLTemplatesList.Contains(i.Template)).Distinct().Count() == 0)  // (6)	 If the user selection values are not present in the DDL must be selected ALL per default
                        SelectedTemplate = new List<object>(TemplateList.ToList());
                    else
                    {
                        SelectedTemplate = new List<object>(TemplateList.Where(i => TempXMLTemplatesList.Contains(i.Template)).Distinct().ToList());

                        List<string> Templates = SelectedTemplate.Select(i => Convert.ToString((i as PlantLoadAnalysis).Template)).Distinct().ToList();

                        //List<PlantLoadAnalysis> filteredData = new List<PlantLoadAnalysis>(plantLoadAnalysisList_Cloned.Where(i => Templates.Contains(Convert.ToString(i.Template))).ToList());
                        List<PlantLoadAnalysis> filteredData = new List<PlantLoadAnalysis>(plantLoadAnalysisList_Cloned.Where(i => Templates.Contains(Convert.ToString(i.Template)) && SelectedRegion.Select(j => Convert.ToString((j as PlantLoadAnalysis).Region)).Contains(Convert.ToString(i.Region))).ToList());

                        OTItemStatusList = new List<PlantLoadAnalysis>(filteredData);
                        List<PlantLoadAnalysis> TempOTItemStatusList = new List<PlantLoadAnalysis>();
                        TempOTItemStatusList.AddRange(OTItemStatusList.GroupBy(i => i.OTItemStatus)
                                                                                  .Select(grp => grp.First())
                                                                                  .ToList().Distinct());

                        OTItemStatusList = new List<PlantLoadAnalysis>(TempOTItemStatusList.Where(a => a.OTItemStatus != null).ToList());

                    }

                    if (TempXMLOTItemStatussList.Count == 0)
                        SelectedOTItemStatus = new List<object>();
                    else if (OTItemStatusList.Where(i => TempXMLOTItemStatussList.Contains(i.OTItemStatus)).Distinct().Count() == 0)  // (6)	 If the user selection values are not present in the DDL must be selected ALL per default
                        SelectedOTItemStatus = new List<object>(OTItemStatusList.ToList());
                    else
                    {
                        SelectedOTItemStatus = new List<object>(OTItemStatusList.Where(i => TempXMLOTItemStatussList.Contains(i.OTItemStatus)).Distinct().ToList());

                        List<string> OTItemStatus = SelectedOTItemStatus.Select(i => Convert.ToString((i as PlantLoadAnalysis).OTItemStatus)).Distinct().ToList();

                        // List<PlantLoadAnalysis> filteredData = new List<PlantLoadAnalysis>(plantLoadAnalysisList_Cloned.Where(i => OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());
                        List<PlantLoadAnalysis> filteredData = new List<PlantLoadAnalysis>(plantLoadAnalysisList_Cloned.Where(i => OTItemStatus.Contains(Convert.ToString(i.OTItemStatus)) && SelectedTemplate.Select(j => (j as PlantLoadAnalysis).Template).Contains(Convert.ToString(i.Template)) && SelectedRegion.Select(k => (k as PlantLoadAnalysis).Region).Contains(Convert.ToString(i.Region))).ToList());

                        CustomerGroupList = new List<PlantLoadAnalysis>(filteredData);
                        List<PlantLoadAnalysis> TempCustomerGroupList = new List<PlantLoadAnalysis>();
                        TempCustomerGroupList.AddRange(CustomerGroupList.GroupBy(i => i.CustomerGroup)
                                                                                  .Select(grp => grp.First())
                                                                                  .ToList().Distinct());

                        CustomerGroupList = new List<PlantLoadAnalysis>(TempCustomerGroupList);
                    }


                    // Customer group
                    if (TempXMLCustomerGroupsList.Count == 0)
                        SelectedCustomerGroup = new List<object>();
                    else if (CustomerGroupList.Where(i => TempXMLCustomerGroupsList.Contains(i.IdCustomer)).Distinct().Count() == 0)
                        SelectedCustomerGroup = new List<object>(CustomerGroupList.ToList());
                    else
                    {
                        SelectedCustomerGroup = new List<object>(CustomerGroupList.Where(i => TempXMLCustomerGroupsList.Contains(i.IdCustomer)).Distinct().ToList());

                        List<int> CustomerIds = SelectedCustomerGroup.Select(i => Convert.ToInt32((i as PlantLoadAnalysis).IdCustomer)).Distinct().ToList();

                        //  List<PlantLoadAnalysis> filteredData = new List<PlantLoadAnalysis>(plantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(Convert.ToInt32(i.IdCustomer))).ToList());

                        List<PlantLoadAnalysis> filteredData = new List<PlantLoadAnalysis>(
                           plantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(Convert.ToInt32(i.IdCustomer)) &&
                           SelectedOTItemStatus.Select(a => (a as PlantLoadAnalysis).OTItemStatus).Contains(Convert.ToString(i.OTItemStatus)) &&
                           SelectedTemplate.Select(b => (b as PlantLoadAnalysis).Template).Contains(Convert.ToString(i.Template)) &&
                           SelectedRegion.Select(c => (c as PlantLoadAnalysis).Region).Contains(Convert.ToString(i.Region))).ToList());

                        CustomerPlantList = new List<PlantLoadAnalysis>(filteredData);
                        List<PlantLoadAnalysis> TempCustomerPlantList = new List<PlantLoadAnalysis>();
                        TempCustomerPlantList.AddRange(CustomerPlantList.GroupBy(i => i.CustomerPlant)
                                                                                  .Select(grp => grp.First())
                                                                                  .ToList().Distinct());

                        CustomerPlantList = new List<PlantLoadAnalysis>(TempCustomerPlantList);
                    }

                    // Customer plant
                    if (TempXMLCustomerPlantsList.Count == 0)
                        SelectedCustomerPlant = new List<object>();
                    else if (CustomerPlantList.Where(i => TempXMLCustomerPlantsList.Contains(i.CustomerPlant)).Distinct().Count() == 0)
                        SelectedCustomerPlant = new List<object>(CustomerPlantList.ToList());
                    else
                    {
                        SelectedCustomerPlant = new List<object>(CustomerPlantList.Where(i => TempXMLCustomerPlantsList.Contains(i.CustomerPlant)).Distinct().ToList());

                        List<string> CustomerPlant = SelectedCustomerPlant.Select(i => Convert.ToString((i as PlantLoadAnalysis).CustomerPlant)).Distinct().ToList();

                        //   List<PlantLoadAnalysis> filteredData = new List<PlantLoadAnalysis>(plantLoadAnalysisList_Cloned.Where(i => CustomerPlant.Contains(Convert.ToString(i.CustomerPlant))).ToList());
                        List<PlantLoadAnalysis> filteredData = new List<PlantLoadAnalysis>(plantLoadAnalysisList_Cloned.Where(i => CustomerPlant.Contains(Convert.ToString(i.CustomerPlant)) &&
                     SelectedCustomerGroup.Select(a => (a as PlantLoadAnalysis).CustomerGroup).Contains(Convert.ToString(i.CustomerGroup)) &&
                     SelectedOTItemStatus.Select(b => (b as PlantLoadAnalysis).OTItemStatus).Contains(Convert.ToString(i.OTItemStatus)) &&
                     SelectedTemplate.Select(c => (c as PlantLoadAnalysis).Template).Contains(Convert.ToString(i.Template)) &&
                     SelectedRegion.Select(d => (d as PlantLoadAnalysis).Region).Contains(Convert.ToString(i.Region))).ToList());

                        ProjectList = new List<PlantLoadAnalysis>(filteredData);
                        List<PlantLoadAnalysis> TempProjectList = new List<PlantLoadAnalysis>();
                        TempProjectList.AddRange(ProjectList.GroupBy(i => i.Project)
                                                                                  .Select(grp => grp.First())
                                                                                  .ToList().Distinct());

                        ProjectList = new List<PlantLoadAnalysis>(TempProjectList.Where(a => a.Project != null).ToList());

                        if (ProjectList.Count == 0)
                        {
                            List<PlantLoadAnalysis> OTCodeListTemp = new List<PlantLoadAnalysis>();
                            OTCodeListTemp = plantLoadAnalysisList_Cloned.Where(a => a.Project == null).ToList();
                            OTCodeList = new List<PlantLoadAnalysis>(OTCodeListTemp);
                            List<PlantLoadAnalysis> OTCodeListGroupBy = new List<PlantLoadAnalysis>();
                            OTCodeListGroupBy.AddRange(OTCodeList.GroupBy(i => i.OTCode)
                                                                                 .Select(grp => grp.First())
                                                                                 .ToList().Distinct());
                            OTCodeList = new List<PlantLoadAnalysis>(OTCodeListGroupBy);
                        }
                    }

                    // Project
                    if (TempXMLProjectList.Count == 0)
                        SelectedProject = new List<object>();
                    else if (ProjectList.Where(i => TempXMLProjectList.Contains(i.Project)).Distinct().Count() == 0)
                        SelectedProject = new List<object>(ProjectList.ToList());
                    else
                    {
                        SelectedProject = new List<object>(ProjectList.Where(i => TempXMLProjectList.Contains(i.Project)).Distinct().ToList());

                        List<string> Projects = SelectedProject.Select(i => Convert.ToString((i as PlantLoadAnalysis).Project)).Distinct().ToList();

                        //  List<PlantLoadAnalysis> filteredData = new List<PlantLoadAnalysis>(plantLoadAnalysisList_Cloned.Where(i => Projects.Contains(Convert.ToString(i.Project))).ToList());
                        List<PlantLoadAnalysis> filteredData = new List<PlantLoadAnalysis>(plantLoadAnalysisList_Cloned.Where(i => (Projects.Contains(Convert.ToString(i.Project)) || i.Project == null) &&
                       SelectedCustomerPlant.Select(a => (a as PlantLoadAnalysis).CustomerPlant).Contains(Convert.ToString(i.CustomerPlant)) &&
                       SelectedCustomerGroup.Select(b => (b as PlantLoadAnalysis).CustomerGroup).Contains(Convert.ToString(i.CustomerGroup)) &&
                       SelectedOTItemStatus.Select(c => (c as PlantLoadAnalysis).OTItemStatus).Contains(Convert.ToString(i.OTItemStatus)) &&
                       SelectedTemplate.Select(d => (d as PlantLoadAnalysis).Template).Contains(Convert.ToString(i.Template)) &&
                       SelectedRegion.Select(e => (e as PlantLoadAnalysis).Region).Contains(Convert.ToString(i.Region))).ToList());

                        OTCodeList = new List<PlantLoadAnalysis>(filteredData);
                        List<PlantLoadAnalysis> TempOTCodeList = new List<PlantLoadAnalysis>();
                        TempOTCodeList.AddRange(OTCodeList.GroupBy(i => i.OTCode)
                                                                                  .Select(grp => grp.First())
                                                                                  .ToList().Distinct());

                        OTCodeList = new List<PlantLoadAnalysis>(TempOTCodeList);

                    }

                    //OTcode

                    if (TempXMLOTCodeList.Count == 0)
                        SelectedOTCode = new List<object>();
                    else if (OTCodeList.Where(i => TempXMLOTCodeList.Contains(i.OTCode)).Distinct().Count() == 0)
                        SelectedOTCode = new List<object>(OTCodeList.ToList());
                    else
                    {
                        SelectedOTCode = new List<object>(OTCodeList.Where(i => TempXMLOTCodeList.Contains(i.OTCode)).Distinct().ToList());

                        List<string> OTCode = SelectedOTCode.Select(i => Convert.ToString((i as PlantLoadAnalysis).OTCode)).Distinct().ToList();

                        // List<PlantLoadAnalysis> filteredData = new List<PlantLoadAnalysis>(plantLoadAnalysisList_Cloned.Where(i => OTCode.Contains(Convert.ToString(i.OTCode))).ToList());
                        List<PlantLoadAnalysis> filteredData = new List<PlantLoadAnalysis>(plantLoadAnalysisList_Cloned.Where(i => OTCode.Contains(Convert.ToString(i.OTCode)) && (
                       SelectedProject.Select(a => (a as PlantLoadAnalysis).Project).Contains(Convert.ToString(i.Project)) || i.Project == null) &&
                       SelectedCustomerPlant.Select(b => (b as PlantLoadAnalysis).CustomerPlant).Contains(Convert.ToString(i.CustomerPlant)) &&
                       SelectedCustomerGroup.Select(c => (c as PlantLoadAnalysis).CustomerGroup).Contains(Convert.ToString(i.CustomerGroup)) &&
                       SelectedOTItemStatus.Select(d => (d as PlantLoadAnalysis).OTItemStatus).Contains(Convert.ToString(i.OTItemStatus)) &&
                       SelectedTemplate.Select(e => (e as PlantLoadAnalysis).Template).Contains(Convert.ToString(i.Template)) &&
                       SelectedRegion.Select(f => (f as PlantLoadAnalysis).Region).Contains(Convert.ToString(i.Region))).ToList());

                        ConnectorFamilyList = new List<PlantLoadAnalysis>(filteredData);
                        List<PlantLoadAnalysis> TempConnectorFamilyList = new List<PlantLoadAnalysis>();
                        TempConnectorFamilyList.AddRange(ConnectorFamilyList.GroupBy(i => i.ConnectorFamily)
                                                                                  .Select(grp => grp.First())
                                                                                  .ToList().Distinct());

                        ConnectorFamilyList = new List<PlantLoadAnalysis>(TempConnectorFamilyList.Where(a => a.ConnectorFamily != null).ToList());
                        if (ConnectorFamilyList.Count == 0)
                        {
                            CPTypeList = new List<PlantLoadAnalysis>();
                        }
                    }




                    // Connector Family
                    if (TempXMLConnectorFamilyList.Count == 0)
                        SelectedConnectorFamily = new List<object>();
                    else if (ConnectorFamilyList.Where(i => TempXMLConnectorFamilyList.Contains(i.ConnectorFamily)).Distinct().Count() == 0)  // (6)	 If the user selection values are not present in the DDL must be selected ALL per default
                        SelectedConnectorFamily = new List<object>(ConnectorFamilyList.ToList());
                    else
                    {
                        SelectedConnectorFamily = new List<object>(ConnectorFamilyList.Where(i => TempXMLConnectorFamilyList.Contains(i.ConnectorFamily)).Distinct().ToList());

                        List<string> ConnectorFamily = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();

                        //  List<PlantLoadAnalysis> filteredData = new List<PlantLoadAnalysis>(plantLoadAnalysisList_Cloned.Where(i => ConnectorFamily.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                        List<PlantLoadAnalysis> filteredData = new List<PlantLoadAnalysis>(plantLoadAnalysisList_Cloned.Where(i => ConnectorFamily.Contains(Convert.ToString(i.ConnectorFamily)) &&
                       SelectedOTCode.Select(a => (a as PlantLoadAnalysis).OTCode).Contains(Convert.ToString(i.OTCode)) &&
                       (SelectedProject.Select(b => (b as PlantLoadAnalysis).Project).Contains(Convert.ToString(i.Project)) || i.Project == null) &&
                       SelectedCustomerPlant.Select(c => (c as PlantLoadAnalysis).CustomerPlant).Contains(Convert.ToString(i.CustomerPlant)) &&
                       SelectedCustomerGroup.Select(d => (d as PlantLoadAnalysis).CustomerGroup).Contains(Convert.ToString(i.CustomerGroup)) &&
                       SelectedOTItemStatus.Select(e => (e as PlantLoadAnalysis).OTItemStatus).Contains(Convert.ToString(i.OTItemStatus)) &&
                       SelectedTemplate.Select(f => (f as PlantLoadAnalysis).Template).Contains(Convert.ToString(i.Template)) &&
                       SelectedRegion.Select(g => (g as PlantLoadAnalysis).Region).Contains(Convert.ToString(i.Region))).ToList());


                        CPTypeList = new List<PlantLoadAnalysis>(filteredData);
                        List<PlantLoadAnalysis> TempCPTypeList = new List<PlantLoadAnalysis>();
                        TempCPTypeList.AddRange(CPTypeList.GroupBy(i => i.CPType)
                                                                                  .Select(grp => grp.First())
                                                                                  .ToList().Distinct());

                        CPTypeList = new List<PlantLoadAnalysis>(TempCPTypeList.Where(a => a.CPType != null).ToList());
                    }


                    // Cptype
                    if (TempXMLCptypeList.Count == 0)
                        SelectedCPType = new List<object>();
                    else if (CPTypeList.Where(i => TempXMLCptypeList.Contains(i.CPType)).Distinct().Count() == 0)  // (6)	 If the user selection values are not present in the DDL must be selected ALL per default
                        SelectedCPType = new List<object>(CPTypeList.ToList());
                    else
                        SelectedCPType = new List<object>(CPTypeList.Where(i => TempXMLCptypeList.Contains(i.CPType)).Distinct().ToList());


                    ApplyFilterConditions();


                }
                #endregion [GEOS2-5114][28-12-2023][Rupali Sarode]

                GeosApplication.Instance.Logger.Log("Method FillAllselectedFilter()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {

                GeosApplication.Instance.Logger.Log("Get an error in FillAllselectedFilter() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //private void FillAllselectedFilter()
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method FillAllselectedFilter()...", category: Category.Info, priority: Priority.Low);
        //        if (SelectedRegion == null) SelectedRegion = new List<object>();
        //        if (SelectedCustomerGroup == null) SelectedCustomerGroup = new List<object>();
        //        if (SelectedCustomerPlant == null) SelectedCustomerPlant = new List<object>();
        //        if (SelectedOTCode == null) SelectedOTCode = new List<object>();
        //        if (SelectedProject == null) SelectedProject = new List<object>();
        //        if (SelectedConnectorFamily == null) SelectedConnectorFamily = new List<object>();
        //        if (SelectedCPType == null) SelectedCPType = new List<object>();
        //        if (SelectedTemplate == null) SelectedTemplate = new List<object>();
        //        if (SelectedOTItemStatus == null) SelectedOTItemStatus = new List<object>();




        //        SelectedRegion.AddRange(RegionList.ToList());
        //        SelectedCustomerGroup.AddRange(CustomerGroupList.ToList());
        //        SelectedCustomerPlant.AddRange(CustomerPlantList.ToList());
        //        SelectedOTCode.AddRange(OTCodeList.ToList());
        //        SelectedProject.AddRange(ProjectList.ToList());
        //        SelectedConnectorFamily.AddRange(ConnectorFamilyList.ToList());
        //        SelectedCPType.AddRange(CPTypeList.ToList());
        //        SelectedTemplate.AddRange(TemplateList.ToList());
        //        SelectedOTItemStatus.AddRange(OTItemStatusList.ToList());
        //        GeosApplication.Instance.Logger.Log("Method FillAllselectedFilter()....executed successfully", category: Category.Info, priority: Priority.Low);

        //    }
        //    catch (Exception ex)
        //    {
        //       GeosApplication.Instance.Logger.Log("Get an error in FillAllselectedFilter() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);

        //    }
        //}

        private void ApplyFilterConditions()
        {

            try
            {
                GeosApplication.Instance.Logger.Log("Method ApplyFilterConditions()...", category: Category.Info, priority: Priority.Low);

                if (SelectedRegion == null) SelectedRegion = new List<object>();
                if (SelectedCustomerGroup == null) SelectedCustomerPlant = new List<object>();
                if (SelectedCustomerPlant == null) SelectedCustomerPlant = new List<object>();
                if (SelectedOTCode == null) SelectedOTCode = new List<object>();
                if (SelectedProject == null) SelectedProject = new List<object>();
                if (SelectedConnectorFamily == null) SelectedConnectorFamily = new List<object>();
                if (SelectedCPType == null) SelectedCPType = new List<object>();
                if (SelectedTemplate == null) SelectedTemplate = new List<object>();
                if (SelectedOTItemStatus == null) SelectedOTItemStatus = new List<object>();

                if (PlantLoadAnalysisList_Cloned != null)
                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.ToList());
                if (SelectedRegion.Count == RegionList.Count && SelectedCustomerGroup.Count == CustomerGroupList.Count && SelectedCustomerPlant.Count == CustomerPlantList.Count && SelectedOTCode.Count == OTCodeList.Count && SelectedProject.Count == ProjectList.Count && SelectedConnectorFamily.Count == ConnectorFamilyList.Count && SelectedCPType.Count == CPTypeList.Count && SelectedTemplate.Count == TemplateList.Count && SelectedOTItemStatus.Count == OTItemStatusList.Count)
                {
                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.ToList());

                } // If Selected ALL filters
                else if (SelectedRegion != null && SelectedRegion.Count > 0)
                {
                    List<string> RegionIds = SelectedRegion.Select(i => Convert.ToString((i as PlantLoadAnalysis).Region)).Distinct().ToList();

                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region)).ToList());

                    if (SelectedTemplate != null && SelectedTemplate.Count > 0)
                    {

                        List<string> Templates = SelectedTemplate.Select(i => Convert.ToString((i as PlantLoadAnalysis).Template)).Distinct().ToList();
                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && Templates.Contains(Convert.ToString(i.Template))).ToList());

                        if (SelectedOTItemStatus != null && SelectedOTItemStatus.Count > 0)
                        {
                            List<string> OTItemStatus = SelectedOTItemStatus.Select(i => Convert.ToString((i as PlantLoadAnalysis).OTItemStatus)).Distinct().ToList();
                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && Templates.Contains(Convert.ToString(i.Template)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());
                            if (SelectedCustomerGroup != null && SelectedCustomerGroup.Count > 0)
                            {
                                List<int> CustomerIds = SelectedCustomerGroup.Select(i => Convert.ToInt32((i as PlantLoadAnalysis).IdCustomer)).Distinct().ToList();

                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerIds.Contains(i.IdCustomer) && Templates.Contains(Convert.ToString(i.Template)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());
                                if (SelectedCustomerPlant != null && SelectedCustomerPlant.Count > 0)
                                {
                                    List<string> CustomerplantIds = SelectedCustomerPlant.Select(i => Convert.ToString((i as PlantLoadAnalysis).CustomerPlant)).Distinct().ToList();
                                    // List<int> OTStatusCategoryIds = SelectedOTStatus.Select(i => Convert.ToInt32((i as PlantDeliveryAnalysis).IdProductCategory)).Distinct().ToList();

                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && Templates.Contains(Convert.ToString(i.Template)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());
                                    if (SelectedProject != null && SelectedProject.Count > 0)
                                    {
                                        List<string> Projects = SelectedProject.Select(i => Convert.ToString((i as PlantLoadAnalysis).Project)).Distinct().ToList();
                                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && Projects.Contains(Convert.ToString(i.Project)) && Templates.Contains(Convert.ToString(i.Template)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());
                                        if (SelectedOTCode != null && SelectedOTCode.Count > 0)
                                        {

                                            List<string> OTCodes = SelectedOTCode.Select(i => Convert.ToString((i as PlantLoadAnalysis).OTCode)).Distinct().ToList();
                                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Templates.Contains(Convert.ToString(i.Template)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());

                                            if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                                            {
                                                List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());

                                                if (SelectedCPType != null && SelectedCPType.Count > 0)
                                                {
                                                    List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());

                                                }
                                            }
                                            else if (SelectedCPType != null && SelectedCPType.Count > 0)
                                            {
                                                List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && CPTypes.Contains(Convert.ToString(i.CPType)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());

                                            }
                                        }
                                        else if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                                        {
                                            List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());

                                            if (SelectedCPType != null && SelectedCPType.Count > 0)
                                            {
                                                List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());

                                            }
                                        }
                                    }
                                    else if (SelectedOTCode != null && SelectedOTCode.Count > 0)
                                    {

                                        List<string> OTCodes = SelectedOTCode.Select(i => Convert.ToString((i as PlantLoadAnalysis).OTCode)).Distinct().ToList();
                                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Templates.Contains(Convert.ToString(i.Template)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());

                                        if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                                        {
                                            List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());

                                            if (SelectedCPType != null && SelectedCPType.Count > 0)
                                            {
                                                List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());

                                            }
                                        }
                                        else if (SelectedCPType != null && SelectedCPType.Count > 0)
                                        {
                                            List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && CPTypes.Contains(Convert.ToString(i.CPType)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());

                                        }

                                    }
                                    else if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                                    {
                                        List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());

                                        if (SelectedCPType != null && SelectedCPType.Count > 0)
                                        {
                                            List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());

                                        }
                                    }
                                }
                                else if (SelectedProject != null && SelectedProject.Count > 0)
                                {
                                    List<string> Projects = SelectedProject.Select(i => Convert.ToString((i as PlantLoadAnalysis).Project)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerIds.Contains(i.IdCustomer) && Projects.Contains(Convert.ToString(i.Project)) && Templates.Contains(Convert.ToString(i.Template)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());
                                    if (SelectedOTCode != null && SelectedOTCode.Count > 0)
                                    {

                                        List<string> OTCodes = SelectedOTCode.Select(i => Convert.ToString((i as PlantLoadAnalysis).OTCode)).Distinct().ToList();
                                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerIds.Contains(i.IdCustomer) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Templates.Contains(Convert.ToString(i.Template)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());

                                        if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                                        {
                                            List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerIds.Contains(i.IdCustomer) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());

                                            if (SelectedCPType != null && SelectedCPType.Count > 0)
                                            {
                                                List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerIds.Contains(i.IdCustomer) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());

                                            }
                                        }
                                        else if (SelectedCPType != null && SelectedCPType.Count > 0)
                                        {
                                            List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerIds.Contains(i.IdCustomer) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && CPTypes.Contains(Convert.ToString(i.CPType)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());

                                        }
                                    }
                                    else if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                                    {
                                        List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerIds.Contains(i.IdCustomer) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());

                                        if (SelectedCPType != null && SelectedCPType.Count > 0)
                                        {
                                            List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerIds.Contains(i.IdCustomer) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());

                                        }
                                    }
                                }
                            }
                            else if (SelectedCustomerPlant != null && SelectedCustomerPlant.Count > 0)
                            {
                                List<string> CustomerplantIds = SelectedCustomerPlant.Select(i => Convert.ToString((i as PlantLoadAnalysis).CustomerPlant)).Distinct().ToList();
                                // List<int> OTStatusCategoryIds = SelectedOTStatus.Select(i => Convert.ToInt32((i as PlantDeliveryAnalysis).IdProductCategory)).Distinct().ToList();

                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && Templates.Contains(Convert.ToString(i.Template)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());
                                if (SelectedProject != null && SelectedProject.Count > 0)
                                {
                                    List<string> Projects = SelectedProject.Select(i => Convert.ToString((i as PlantLoadAnalysis).Project)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && Projects.Contains(Convert.ToString(i.Project)) && Templates.Contains(Convert.ToString(i.Template)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());
                                    if (SelectedOTCode != null && SelectedOTCode.Count > 0)
                                    {

                                        List<string> OTCodes = SelectedOTCode.Select(i => Convert.ToString((i as PlantLoadAnalysis).OTCode)).Distinct().ToList();
                                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Templates.Contains(Convert.ToString(i.Template)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());

                                        if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                                        {
                                            List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());

                                            if (SelectedCPType != null && SelectedCPType.Count > 0)
                                            {
                                                List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());

                                            }
                                        }
                                        else if (SelectedCPType != null && SelectedCPType.Count > 0)
                                        {
                                            List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && CPTypes.Contains(Convert.ToString(i.CPType)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());

                                        }
                                    }
                                    else if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                                    {
                                        List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());

                                        if (SelectedCPType != null && SelectedCPType.Count > 0)
                                        {
                                            List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());

                                        }
                                    }
                                }
                                else if (SelectedOTCode != null && SelectedOTCode.Count > 0)
                                {

                                    List<string> OTCodes = SelectedOTCode.Select(i => Convert.ToString((i as PlantLoadAnalysis).OTCode)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Templates.Contains(Convert.ToString(i.Template)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());

                                    if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                                    {
                                        List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());

                                        if (SelectedCPType != null && SelectedCPType.Count > 0)
                                        {
                                            List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());

                                        }
                                    }
                                    else if (SelectedCPType != null && SelectedCPType.Count > 0)
                                    {
                                        List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && CPTypes.Contains(Convert.ToString(i.CPType)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());

                                    }
                                }
                                else if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                                {
                                    List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());

                                    if (SelectedCPType != null && SelectedCPType.Count > 0)
                                    {
                                        List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());

                                    }
                                }
                                else if (SelectedCPType != null && SelectedCPType.Count > 0)
                                {
                                    List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && CPTypes.Contains(Convert.ToString(i.CPType)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());

                                }
                            }
                            else if (SelectedProject != null && SelectedProject.Count > 0)
                            {
                                List<string> Projects = SelectedProject.Select(i => Convert.ToString((i as PlantLoadAnalysis).Project)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && Projects.Contains(Convert.ToString(i.Project)) && Templates.Contains(Convert.ToString(i.Template)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());
                                if (SelectedOTCode != null && SelectedOTCode.Count > 0)
                                {

                                    List<string> OTCodes = SelectedOTCode.Select(i => Convert.ToString((i as PlantLoadAnalysis).OTCode)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Templates.Contains(Convert.ToString(i.Template)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());

                                    if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                                    {
                                        List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());

                                        if (SelectedCPType != null && SelectedCPType.Count > 0)
                                        {
                                            List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());

                                        }
                                    }
                                    else if (SelectedCPType != null && SelectedCPType.Count > 0)
                                    {
                                        List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && CPTypes.Contains(Convert.ToString(i.CPType)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());

                                    }
                                }
                                else if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                                {
                                    List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());

                                    if (SelectedCPType != null && SelectedCPType.Count > 0)
                                    {
                                        List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());

                                    }
                                }
                            }
                            else if (SelectedOTCode != null && SelectedOTCode.Count > 0)
                            {

                                List<string> OTCodes = SelectedOTCode.Select(i => Convert.ToString((i as PlantLoadAnalysis).OTCode)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Templates.Contains(Convert.ToString(i.Template)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());

                                if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                                {
                                    List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && OTCodes.Contains(Convert.ToString(i.OTCode)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());

                                    if (SelectedCPType != null && SelectedCPType.Count > 0)
                                    {
                                        List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && OTCodes.Contains(Convert.ToString(i.OTCode)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());

                                    }
                                }
                                else if (SelectedCPType != null && SelectedCPType.Count > 0)
                                {
                                    List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && OTCodes.Contains(Convert.ToString(i.OTCode)) && CPTypes.Contains(Convert.ToString(i.CPType)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());

                                }
                            }
                            else if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                            {
                                List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());

                                if (SelectedCPType != null && SelectedCPType.Count > 0)
                                {
                                    List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());

                                }
                            }
                            else if (SelectedCPType != null && SelectedCPType.Count > 0)
                            {
                                List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CPTypes.Contains(Convert.ToString(i.CPType)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());

                            }
                        }
                        else if (SelectedCustomerGroup != null && SelectedCustomerGroup.Count > 0)
                        {
                            List<int> CustomerIds = SelectedCustomerGroup.Select(i => Convert.ToInt32((i as PlantLoadAnalysis).IdCustomer)).Distinct().ToList();

                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerIds.Contains(i.IdCustomer) && Templates.Contains(Convert.ToString(i.Template))).ToList());
                            if (SelectedCustomerPlant != null && SelectedCustomerPlant.Count > 0)
                            {
                                List<string> CustomerplantIds = SelectedCustomerPlant.Select(i => Convert.ToString((i as PlantLoadAnalysis).CustomerPlant)).Distinct().ToList();
                                // List<int> OTStatusCategoryIds = SelectedOTStatus.Select(i => Convert.ToInt32((i as PlantDeliveryAnalysis).IdProductCategory)).Distinct().ToList();

                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && Templates.Contains(Convert.ToString(i.Template))).ToList());
                                if (SelectedProject != null && SelectedProject.Count > 0)
                                {
                                    List<string> Projects = SelectedProject.Select(i => Convert.ToString((i as PlantLoadAnalysis).Project)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && Projects.Contains(Convert.ToString(i.Project)) && Templates.Contains(Convert.ToString(i.Template))).ToList());
                                    if (SelectedOTCode != null && SelectedOTCode.Count > 0)
                                    {

                                        List<string> OTCodes = SelectedOTCode.Select(i => Convert.ToString((i as PlantLoadAnalysis).OTCode)).Distinct().ToList();
                                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Templates.Contains(Convert.ToString(i.Template))).ToList());

                                        if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                                        {
                                            List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                                            if (SelectedCPType != null && SelectedCPType.Count > 0)
                                            {
                                                List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                            }
                                        }
                                        else if (SelectedCPType != null && SelectedCPType.Count > 0)
                                        {
                                            List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                        }
                                    }
                                    else if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                                    {
                                        List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                                        if (SelectedCPType != null && SelectedCPType.Count > 0)
                                        {
                                            List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                        }
                                    }
                                }
                                else if (SelectedOTCode != null && SelectedOTCode.Count > 0)
                                {

                                    List<string> OTCodes = SelectedOTCode.Select(i => Convert.ToString((i as PlantLoadAnalysis).OTCode)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Templates.Contains(Convert.ToString(i.Template))).ToList());

                                    if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                                    {
                                        List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                                        if (SelectedCPType != null && SelectedCPType.Count > 0)
                                        {
                                            List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                        }
                                    }
                                    else if (SelectedCPType != null && SelectedCPType.Count > 0)
                                    {
                                        List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                    }
                                }
                                else if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                                {
                                    List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                                    if (SelectedCPType != null && SelectedCPType.Count > 0)
                                    {
                                        List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                    }
                                }
                                else if (SelectedCPType != null && SelectedCPType.Count > 0)
                                {
                                    List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                }
                            }
                            else if (SelectedProject != null && SelectedProject.Count > 0)
                            {
                                List<string> Projects = SelectedProject.Select(i => Convert.ToString((i as PlantLoadAnalysis).Project)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerIds.Contains(i.IdCustomer) && Projects.Contains(Convert.ToString(i.Project)) && Templates.Contains(Convert.ToString(i.Template))).ToList());
                                if (SelectedOTCode != null && SelectedOTCode.Count > 0)
                                {

                                    List<string> OTCodes = SelectedOTCode.Select(i => Convert.ToString((i as PlantLoadAnalysis).OTCode)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerIds.Contains(i.IdCustomer) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Templates.Contains(Convert.ToString(i.Template))).ToList());

                                    if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                                    {
                                        List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerIds.Contains(i.IdCustomer) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                                        if (SelectedCPType != null && SelectedCPType.Count > 0)
                                        {
                                            List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerIds.Contains(i.IdCustomer) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                        }
                                    }
                                    else if (SelectedCPType != null && SelectedCPType.Count > 0)
                                    {
                                        List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerIds.Contains(i.IdCustomer) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                    }
                                }
                                else if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                                {
                                    List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerIds.Contains(i.IdCustomer) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                                    if (SelectedCPType != null && SelectedCPType.Count > 0)
                                    {
                                        List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerIds.Contains(i.IdCustomer) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                    }
                                }
                                else if (SelectedCPType != null && SelectedCPType.Count > 0)
                                {
                                    List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerIds.Contains(i.IdCustomer) && Projects.Contains(Convert.ToString(i.Project)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                }
                            }
                            else if (SelectedOTCode != null && SelectedOTCode.Count > 0)
                            {

                                List<string> OTCodes = SelectedOTCode.Select(i => Convert.ToString((i as PlantLoadAnalysis).OTCode)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerIds.Contains(i.IdCustomer) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Templates.Contains(Convert.ToString(i.Template))).ToList());

                                if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                                {
                                    List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerIds.Contains(i.IdCustomer) && OTCodes.Contains(Convert.ToString(i.OTCode)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                                    if (SelectedCPType != null && SelectedCPType.Count > 0)
                                    {
                                        List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerIds.Contains(i.IdCustomer) && OTCodes.Contains(Convert.ToString(i.OTCode)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                    }
                                }
                                else if (SelectedCPType != null && SelectedCPType.Count > 0)
                                {
                                    List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerIds.Contains(i.IdCustomer) && OTCodes.Contains(Convert.ToString(i.OTCode)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                }
                            }
                            else if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                            {
                                List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerIds.Contains(i.IdCustomer) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                                if (SelectedCPType != null && SelectedCPType.Count > 0)
                                {
                                    List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerIds.Contains(i.IdCustomer) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                }
                            }
                        }
                        else if (SelectedCustomerPlant != null && SelectedCustomerPlant.Count > 0)
                        {
                            List<string> CustomerplantIds = SelectedCustomerPlant.Select(i => Convert.ToString((i as PlantLoadAnalysis).CustomerPlant)).Distinct().ToList();
                            // List<int> OTStatusCategoryIds = SelectedOTStatus.Select(i => Convert.ToInt32((i as PlantDeliveryAnalysis).IdProductCategory)).Distinct().ToList();

                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && Templates.Contains(Convert.ToString(i.Template))).ToList());
                            if (SelectedProject != null && SelectedProject.Count > 0)
                            {
                                List<string> Projects = SelectedProject.Select(i => Convert.ToString((i as PlantLoadAnalysis).Project)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && Projects.Contains(Convert.ToString(i.Project)) && Templates.Contains(Convert.ToString(i.Template))).ToList());
                                if (SelectedOTCode != null && SelectedOTCode.Count > 0)
                                {

                                    List<string> OTCodes = SelectedOTCode.Select(i => Convert.ToString((i as PlantLoadAnalysis).OTCode)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Templates.Contains(Convert.ToString(i.Template))).ToList());

                                    if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                                    {
                                        List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                                        if (SelectedCPType != null && SelectedCPType.Count > 0)
                                        {
                                            List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                        }
                                    }
                                    else if (SelectedCPType != null && SelectedCPType.Count > 0)
                                    {
                                        List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                    }
                                }
                                else if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                                {
                                    List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                                    if (SelectedCPType != null && SelectedCPType.Count > 0)
                                    {
                                        List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                    }
                                }
                            }
                            else if (SelectedOTCode != null && SelectedOTCode.Count > 0)
                            {

                                List<string> OTCodes = SelectedOTCode.Select(i => Convert.ToString((i as PlantLoadAnalysis).OTCode)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Templates.Contains(Convert.ToString(i.Template))).ToList());

                                if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                                {
                                    List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                                    if (SelectedCPType != null && SelectedCPType.Count > 0)
                                    {
                                        List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                    }
                                }
                                else if (SelectedCPType != null && SelectedCPType.Count > 0)
                                {
                                    List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                }
                            }
                            else if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                            {
                                List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                                if (SelectedCPType != null && SelectedCPType.Count > 0)
                                {
                                    List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                }
                            }
                            else if (SelectedCPType != null && SelectedCPType.Count > 0)
                            {
                                List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                            }
                        }
                        else if (SelectedProject != null && SelectedProject.Count > 0)
                        {
                            List<string> Projects = SelectedProject.Select(i => Convert.ToString((i as PlantLoadAnalysis).Project)).Distinct().ToList();
                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && Projects.Contains(Convert.ToString(i.Project)) && Templates.Contains(Convert.ToString(i.Template))).ToList());
                            if (SelectedOTCode != null && SelectedOTCode.Count > 0)
                            {

                                List<string> OTCodes = SelectedOTCode.Select(i => Convert.ToString((i as PlantLoadAnalysis).OTCode)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Templates.Contains(Convert.ToString(i.Template))).ToList());

                                if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                                {
                                    List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                                    if (SelectedCPType != null && SelectedCPType.Count > 0)
                                    {
                                        List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                    }
                                }
                                else if (SelectedCPType != null && SelectedCPType.Count > 0)
                                {
                                    List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                }
                            }
                            else if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                            {
                                List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                                if (SelectedCPType != null && SelectedCPType.Count > 0)
                                {
                                    List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                }
                            }
                            else if (SelectedCPType != null && SelectedCPType.Count > 0)
                            {
                                List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && Projects.Contains(Convert.ToString(i.Project)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                            }
                        }
                        else if (SelectedOTCode != null && SelectedOTCode.Count > 0)
                        {

                            List<string> OTCodes = SelectedOTCode.Select(i => Convert.ToString((i as PlantLoadAnalysis).OTCode)).Distinct().ToList();
                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Templates.Contains(Convert.ToString(i.Template))).ToList());

                            if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                            {
                                List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && OTCodes.Contains(Convert.ToString(i.OTCode)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                                if (SelectedCPType != null && SelectedCPType.Count > 0)
                                {
                                    List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && OTCodes.Contains(Convert.ToString(i.OTCode)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                }
                            }
                            else if (SelectedCPType != null && SelectedCPType.Count > 0)
                            {
                                List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && OTCodes.Contains(Convert.ToString(i.OTCode)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                            }
                        }
                        else if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                        {
                            List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                            if (SelectedCPType != null && SelectedCPType.Count > 0)
                            {
                                List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                            }
                        }

                    }


                }
                else if (SelectedTemplate != null && SelectedTemplate.Count > 0)
                {

                    List<string> Templates = SelectedTemplate.Select(i => Convert.ToString((i as PlantLoadAnalysis).Template)).Distinct().ToList();
                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => Templates.Contains(Convert.ToString(i.Template))).ToList());

                    if (SelectedOTItemStatus != null && SelectedOTItemStatus.Count > 0)
                    {
                        List<string> OTItemStatus = SelectedOTItemStatus.Select(i => Convert.ToString((i as PlantLoadAnalysis).OTItemStatus)).Distinct().ToList();
                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => Templates.Contains(Convert.ToString(i.Template)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());
                        if (SelectedCustomerGroup != null && SelectedCustomerGroup.Count > 0)
                        {
                            List<int> CustomerIds = SelectedCustomerGroup.Select(i => Convert.ToInt32((i as PlantLoadAnalysis).IdCustomer)).Distinct().ToList();

                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && Templates.Contains(Convert.ToString(i.Template)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());
                            if (SelectedCustomerPlant != null && SelectedCustomerPlant.Count > 0)
                            {
                                List<string> CustomerplantIds = SelectedCustomerPlant.Select(i => Convert.ToString((i as PlantLoadAnalysis).CustomerPlant)).Distinct().ToList();
                                // List<int> OTStatusCategoryIds = SelectedOTStatus.Select(i => Convert.ToInt32((i as PlantDeliveryAnalysis).IdProductCategory)).Distinct().ToList();

                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && Templates.Contains(Convert.ToString(i.Template)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());
                                if (SelectedProject != null && SelectedProject.Count > 0)
                                {
                                    List<string> Projects = SelectedProject.Select(i => Convert.ToString((i as PlantLoadAnalysis).Project)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && Projects.Contains(Convert.ToString(i.Project)) && Templates.Contains(Convert.ToString(i.Template)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());
                                    if (SelectedOTCode != null && SelectedOTCode.Count > 0)
                                    {

                                        List<string> OTCodes = SelectedOTCode.Select(i => Convert.ToString((i as PlantLoadAnalysis).OTCode)).Distinct().ToList();
                                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Templates.Contains(Convert.ToString(i.Template)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());

                                        if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                                        {
                                            List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                                            if (SelectedCPType != null && SelectedCPType.Count > 0)
                                            {
                                                List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                            }
                                        }
                                        else if (SelectedCPType != null && SelectedCPType.Count > 0)
                                        {
                                            List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                        }
                                    }
                                    else if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                                    {
                                        List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                                        if (SelectedCPType != null && SelectedCPType.Count > 0)
                                        {
                                            List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                        }
                                    }
                                }
                                else if (SelectedOTCode != null && SelectedOTCode.Count > 0)
                                {

                                    List<string> OTCodes = SelectedOTCode.Select(i => Convert.ToString((i as PlantLoadAnalysis).OTCode)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Templates.Contains(Convert.ToString(i.Template)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());

                                    if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                                    {
                                        List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                                        if (SelectedCPType != null && SelectedCPType.Count > 0)
                                        {
                                            List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                        }
                                    }
                                    else if (SelectedCPType != null && SelectedCPType.Count > 0)
                                    {
                                        List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                    }

                                }
                                else if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                                {
                                    List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                                    if (SelectedCPType != null && SelectedCPType.Count > 0)
                                    {
                                        List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                    }
                                }
                            }
                            else if (SelectedProject != null && SelectedProject.Count > 0)
                            {
                                List<string> Projects = SelectedProject.Select(i => Convert.ToString((i as PlantLoadAnalysis).Project)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && Projects.Contains(Convert.ToString(i.Project)) && Templates.Contains(Convert.ToString(i.Template)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());
                                if (SelectedOTCode != null && SelectedOTCode.Count > 0)
                                {

                                    List<string> OTCodes = SelectedOTCode.Select(i => Convert.ToString((i as PlantLoadAnalysis).OTCode)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Templates.Contains(Convert.ToString(i.Template)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());

                                    if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                                    {
                                        List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                                        if (SelectedCPType != null && SelectedCPType.Count > 0)
                                        {
                                            List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                        }
                                    }
                                    else if (SelectedCPType != null && SelectedCPType.Count > 0)
                                    {
                                        List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                    }
                                }
                                else if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                                {
                                    List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                                    if (SelectedCPType != null && SelectedCPType.Count > 0)
                                    {
                                        List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                    }
                                }
                            }
                        }
                        else if (SelectedCustomerPlant != null && SelectedCustomerPlant.Count > 0)
                        {
                            List<string> CustomerplantIds = SelectedCustomerPlant.Select(i => Convert.ToString((i as PlantLoadAnalysis).CustomerPlant)).Distinct().ToList();
                            // List<int> OTStatusCategoryIds = SelectedOTStatus.Select(i => Convert.ToInt32((i as PlantDeliveryAnalysis).IdProductCategory)).Distinct().ToList();

                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && Templates.Contains(Convert.ToString(i.Template)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());
                            if (SelectedProject != null && SelectedProject.Count > 0)
                            {
                                List<string> Projects = SelectedProject.Select(i => Convert.ToString((i as PlantLoadAnalysis).Project)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && Projects.Contains(Convert.ToString(i.Project)) && Templates.Contains(Convert.ToString(i.Template)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());
                                if (SelectedOTCode != null && SelectedOTCode.Count > 0)
                                {

                                    List<string> OTCodes = SelectedOTCode.Select(i => Convert.ToString((i as PlantLoadAnalysis).OTCode)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Templates.Contains(Convert.ToString(i.Template)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());

                                    if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                                    {
                                        List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                                        if (SelectedCPType != null && SelectedCPType.Count > 0)
                                        {
                                            List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                        }
                                    }
                                    else if (SelectedCPType != null && SelectedCPType.Count > 0)
                                    {
                                        List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                    }
                                }
                                else if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                                {
                                    List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                                    if (SelectedCPType != null && SelectedCPType.Count > 0)
                                    {
                                        List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                    }
                                }
                            }
                            else if (SelectedOTCode != null && SelectedOTCode.Count > 0)
                            {

                                List<string> OTCodes = SelectedOTCode.Select(i => Convert.ToString((i as PlantLoadAnalysis).OTCode)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Templates.Contains(Convert.ToString(i.Template)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());

                                if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                                {
                                    List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                                    if (SelectedCPType != null && SelectedCPType.Count > 0)
                                    {
                                        List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                    }
                                }
                                else if (SelectedCPType != null && SelectedCPType.Count > 0)
                                {
                                    List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                }
                            }
                            else if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                            {
                                List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                                if (SelectedCPType != null && SelectedCPType.Count > 0)
                                {
                                    List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                }
                            }
                            else if (SelectedCPType != null && SelectedCPType.Count > 0)
                            {
                                List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                            }
                        }
                        else if (SelectedProject != null && SelectedProject.Count > 0)
                        {
                            List<string> Projects = SelectedProject.Select(i => Convert.ToString((i as PlantLoadAnalysis).Project)).Distinct().ToList();
                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => Projects.Contains(Convert.ToString(i.Project)) && Templates.Contains(Convert.ToString(i.Template)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());
                            if (SelectedOTCode != null && SelectedOTCode.Count > 0)
                            {

                                List<string> OTCodes = SelectedOTCode.Select(i => Convert.ToString((i as PlantLoadAnalysis).OTCode)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => OTCodes.Contains(Convert.ToString(i.OTCode)) && Templates.Contains(Convert.ToString(i.Template)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());

                                if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                                {
                                    List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                                    if (SelectedCPType != null && SelectedCPType.Count > 0)
                                    {
                                        List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                    }
                                }
                                else if (SelectedCPType != null && SelectedCPType.Count > 0)
                                {
                                    List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                }
                            }
                            else if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                            {
                                List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                                if (SelectedCPType != null && SelectedCPType.Count > 0)
                                {
                                    List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                }
                            }
                        }
                        else if (SelectedOTCode != null && SelectedOTCode.Count > 0)
                        {

                            List<string> OTCodes = SelectedOTCode.Select(i => Convert.ToString((i as PlantLoadAnalysis).OTCode)).Distinct().ToList();
                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => OTCodes.Contains(Convert.ToString(i.OTCode)) && Templates.Contains(Convert.ToString(i.Template)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());

                            if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                            {
                                List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => OTCodes.Contains(Convert.ToString(i.OTCode)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                                if (SelectedCPType != null && SelectedCPType.Count > 0)
                                {
                                    List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => OTCodes.Contains(Convert.ToString(i.OTCode)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                }
                            }
                            else if (SelectedCPType != null && SelectedCPType.Count > 0)
                            {
                                List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => OTCodes.Contains(Convert.ToString(i.OTCode)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                            }
                        }
                        else if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                        {
                            List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                            if (SelectedCPType != null && SelectedCPType.Count > 0)
                            {
                                List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                            }
                        }
                        else if (SelectedCPType != null && SelectedCPType.Count > 0)
                        {
                            List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                        }
                    }
                    else if (SelectedCustomerGroup != null && SelectedCustomerGroup.Count > 0)
                    {
                        List<int> CustomerIds = SelectedCustomerGroup.Select(i => Convert.ToInt32((i as PlantLoadAnalysis).IdCustomer)).Distinct().ToList();

                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && Templates.Contains(Convert.ToString(i.Template))).ToList());
                        if (SelectedCustomerPlant != null && SelectedCustomerPlant.Count > 0)
                        {
                            List<string> CustomerplantIds = SelectedCustomerPlant.Select(i => Convert.ToString((i as PlantLoadAnalysis).CustomerPlant)).Distinct().ToList();
                            // List<int> OTStatusCategoryIds = SelectedOTStatus.Select(i => Convert.ToInt32((i as PlantDeliveryAnalysis).IdProductCategory)).Distinct().ToList();

                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && Templates.Contains(Convert.ToString(i.Template))).ToList());
                            if (SelectedProject != null && SelectedProject.Count > 0)
                            {
                                List<string> Projects = SelectedProject.Select(i => Convert.ToString((i as PlantLoadAnalysis).Project)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && Projects.Contains(Convert.ToString(i.Project)) && Templates.Contains(Convert.ToString(i.Template))).ToList());
                                if (SelectedOTCode != null && SelectedOTCode.Count > 0)
                                {

                                    List<string> OTCodes = SelectedOTCode.Select(i => Convert.ToString((i as PlantLoadAnalysis).OTCode)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Templates.Contains(Convert.ToString(i.Template))).ToList());

                                    if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                                    {
                                        List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                                        if (SelectedCPType != null && SelectedCPType.Count > 0)
                                        {
                                            List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                        }
                                    }
                                    else if (SelectedCPType != null && SelectedCPType.Count > 0)
                                    {
                                        List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                    }
                                }
                                else if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                                {
                                    List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                                    if (SelectedCPType != null && SelectedCPType.Count > 0)
                                    {
                                        List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                    }
                                }
                            }
                            else if (SelectedOTCode != null && SelectedOTCode.Count > 0)
                            {

                                List<string> OTCodes = SelectedOTCode.Select(i => Convert.ToString((i as PlantLoadAnalysis).OTCode)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Templates.Contains(Convert.ToString(i.Template))).ToList());

                                if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                                {
                                    List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                                    if (SelectedCPType != null && SelectedCPType.Count > 0)
                                    {
                                        List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                    }
                                }
                                else if (SelectedCPType != null && SelectedCPType.Count > 0)
                                {
                                    List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                }
                            }
                            else if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                            {
                                List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                                if (SelectedCPType != null && SelectedCPType.Count > 0)
                                {
                                    List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                }
                            }
                            else if (SelectedCPType != null && SelectedCPType.Count > 0)
                            {
                                List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                            }
                        }
                        else if (SelectedProject != null && SelectedProject.Count > 0)
                        {
                            List<string> Projects = SelectedProject.Select(i => Convert.ToString((i as PlantLoadAnalysis).Project)).Distinct().ToList();
                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && Projects.Contains(Convert.ToString(i.Project)) && Templates.Contains(Convert.ToString(i.Template))).ToList());
                            if (SelectedOTCode != null && SelectedOTCode.Count > 0)
                            {

                                List<string> OTCodes = SelectedOTCode.Select(i => Convert.ToString((i as PlantLoadAnalysis).OTCode)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Templates.Contains(Convert.ToString(i.Template))).ToList());

                                if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                                {
                                    List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                                    if (SelectedCPType != null && SelectedCPType.Count > 0)
                                    {
                                        List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                    }
                                }
                                else if (SelectedCPType != null && SelectedCPType.Count > 0)
                                {
                                    List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                }
                            }
                            else if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                            {
                                List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                                if (SelectedCPType != null && SelectedCPType.Count > 0)
                                {
                                    List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                }
                            }
                            else if (SelectedCPType != null && SelectedCPType.Count > 0)
                            {
                                List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && Projects.Contains(Convert.ToString(i.Project)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                            }
                        }
                        else if (SelectedOTCode != null && SelectedOTCode.Count > 0)
                        {

                            List<string> OTCodes = SelectedOTCode.Select(i => Convert.ToString((i as PlantLoadAnalysis).OTCode)).Distinct().ToList();
                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Templates.Contains(Convert.ToString(i.Template))).ToList());

                            if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                            {
                                List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && OTCodes.Contains(Convert.ToString(i.OTCode)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                                if (SelectedCPType != null && SelectedCPType.Count > 0)
                                {
                                    List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && OTCodes.Contains(Convert.ToString(i.OTCode)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                }
                            }
                            else if (SelectedCPType != null && SelectedCPType.Count > 0)
                            {
                                List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && OTCodes.Contains(Convert.ToString(i.OTCode)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                            }
                        }
                        else if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                        {
                            List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                            if (SelectedCPType != null && SelectedCPType.Count > 0)
                            {
                                List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                            }
                        }
                    }
                    else if (SelectedCustomerPlant != null && SelectedCustomerPlant.Count > 0)
                    {
                        List<string> CustomerplantIds = SelectedCustomerPlant.Select(i => Convert.ToString((i as PlantLoadAnalysis).CustomerPlant)).Distinct().ToList();
                        // List<int> OTStatusCategoryIds = SelectedOTStatus.Select(i => Convert.ToInt32((i as PlantDeliveryAnalysis).IdProductCategory)).Distinct().ToList();

                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && Templates.Contains(Convert.ToString(i.Template))).ToList());
                        if (SelectedProject != null && SelectedProject.Count > 0)
                        {
                            List<string> Projects = SelectedProject.Select(i => Convert.ToString((i as PlantLoadAnalysis).Project)).Distinct().ToList();
                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && Projects.Contains(Convert.ToString(i.Project)) && Templates.Contains(Convert.ToString(i.Template))).ToList());
                            if (SelectedOTCode != null && SelectedOTCode.Count > 0)
                            {

                                List<string> OTCodes = SelectedOTCode.Select(i => Convert.ToString((i as PlantLoadAnalysis).OTCode)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Templates.Contains(Convert.ToString(i.Template))).ToList());

                                if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                                {
                                    List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                                    if (SelectedCPType != null && SelectedCPType.Count > 0)
                                    {
                                        List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                    }
                                }
                                else if (SelectedCPType != null && SelectedCPType.Count > 0)
                                {
                                    List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                }
                            }
                            else if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                            {
                                List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                                if (SelectedCPType != null && SelectedCPType.Count > 0)
                                {
                                    List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                }
                            }
                        }
                        else if (SelectedOTCode != null && SelectedOTCode.Count > 0)
                        {

                            List<string> OTCodes = SelectedOTCode.Select(i => Convert.ToString((i as PlantLoadAnalysis).OTCode)).Distinct().ToList();
                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Templates.Contains(Convert.ToString(i.Template))).ToList());

                            if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                            {
                                List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                                if (SelectedCPType != null && SelectedCPType.Count > 0)
                                {
                                    List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                }
                            }
                            else if (SelectedCPType != null && SelectedCPType.Count > 0)
                            {
                                List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                            }
                        }
                        else if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                        {
                            List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                            if (SelectedCPType != null && SelectedCPType.Count > 0)
                            {
                                List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                            }
                        }
                        else if (SelectedCPType != null && SelectedCPType.Count > 0)
                        {
                            List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                        }
                    }
                    else if (SelectedProject != null && SelectedProject.Count > 0)
                    {
                        List<string> Projects = SelectedProject.Select(i => Convert.ToString((i as PlantLoadAnalysis).Project)).Distinct().ToList();
                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => Projects.Contains(Convert.ToString(i.Project)) && Templates.Contains(Convert.ToString(i.Template))).ToList());
                        if (SelectedOTCode != null && SelectedOTCode.Count > 0)
                        {

                            List<string> OTCodes = SelectedOTCode.Select(i => Convert.ToString((i as PlantLoadAnalysis).OTCode)).Distinct().ToList();
                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => OTCodes.Contains(Convert.ToString(i.OTCode)) && Templates.Contains(Convert.ToString(i.Template))).ToList());

                            if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                            {
                                List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                                if (SelectedCPType != null && SelectedCPType.Count > 0)
                                {
                                    List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                }
                            }
                            else if (SelectedCPType != null && SelectedCPType.Count > 0)
                            {
                                List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                            }
                        }
                        else if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                        {
                            List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                            if (SelectedCPType != null && SelectedCPType.Count > 0)
                            {
                                List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                            }
                        }
                        else if (SelectedCPType != null && SelectedCPType.Count > 0)
                        {
                            List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => Projects.Contains(Convert.ToString(i.Project)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                        }
                    }
                    else if (SelectedOTCode != null && SelectedOTCode.Count > 0)
                    {

                        List<string> OTCodes = SelectedOTCode.Select(i => Convert.ToString((i as PlantLoadAnalysis).OTCode)).Distinct().ToList();
                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => OTCodes.Contains(Convert.ToString(i.OTCode)) && Templates.Contains(Convert.ToString(i.Template))).ToList());

                        if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                        {
                            List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => OTCodes.Contains(Convert.ToString(i.OTCode)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                            if (SelectedCPType != null && SelectedCPType.Count > 0)
                            {
                                List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => OTCodes.Contains(Convert.ToString(i.OTCode)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                            }
                        }
                        else if (SelectedCPType != null && SelectedCPType.Count > 0)
                        {
                            List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => OTCodes.Contains(Convert.ToString(i.OTCode)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                        }
                    }
                    else if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                    {
                        List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                        if (SelectedCPType != null && SelectedCPType.Count > 0)
                        {
                            List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                        }
                    }

                }

                //otitemm
                else if (SelectedOTItemStatus != null && SelectedOTItemStatus.Count > 0)
                {
                    List<string> OTItemStatus = SelectedOTItemStatus.Select(i => Convert.ToString((i as PlantLoadAnalysis).OTItemStatus)).Distinct().ToList();
                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());
                    if (SelectedCustomerGroup != null && SelectedCustomerGroup.Count > 0)
                    {
                        List<int> CustomerIds = SelectedCustomerGroup.Select(i => Convert.ToInt32((i as PlantLoadAnalysis).IdCustomer)).Distinct().ToList();

                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());
                        if (SelectedCustomerPlant != null && SelectedCustomerPlant.Count > 0)
                        {
                            List<string> CustomerplantIds = SelectedCustomerPlant.Select(i => Convert.ToString((i as PlantLoadAnalysis).CustomerPlant)).Distinct().ToList();
                            // List<int> OTStatusCategoryIds = SelectedOTStatus.Select(i => Convert.ToInt32((i as PlantDeliveryAnalysis).IdProductCategory)).Distinct().ToList();

                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());
                            if (SelectedProject != null && SelectedProject.Count > 0)
                            {
                                List<string> Projects = SelectedProject.Select(i => Convert.ToString((i as PlantLoadAnalysis).Project)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && Projects.Contains(Convert.ToString(i.Project)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());
                                if (SelectedOTCode != null && SelectedOTCode.Count > 0)
                                {

                                    List<string> OTCodes = SelectedOTCode.Select(i => Convert.ToString((i as PlantLoadAnalysis).OTCode)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());

                                    if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                                    {
                                        List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                                        if (SelectedCPType != null && SelectedCPType.Count > 0)
                                        {
                                            List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                        }
                                    }
                                    else if (SelectedCPType != null && SelectedCPType.Count > 0)
                                    {
                                        List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                    }
                                }
                                else if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                                {
                                    List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                                    if (SelectedCPType != null && SelectedCPType.Count > 0)
                                    {
                                        List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                    }
                                }
                            }
                            else if (SelectedOTCode != null && SelectedOTCode.Count > 0)
                            {

                                List<string> OTCodes = SelectedOTCode.Select(i => Convert.ToString((i as PlantLoadAnalysis).OTCode)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());

                                if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                                {
                                    List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                                    if (SelectedCPType != null && SelectedCPType.Count > 0)
                                    {
                                        List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                    }
                                }
                                else if (SelectedCPType != null && SelectedCPType.Count > 0)
                                {
                                    List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                }

                            }
                            else if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                            {
                                List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                                if (SelectedCPType != null && SelectedCPType.Count > 0)
                                {
                                    List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                }
                            }
                        }
                        else if (SelectedProject != null && SelectedProject.Count > 0)
                        {
                            List<string> Projects = SelectedProject.Select(i => Convert.ToString((i as PlantLoadAnalysis).Project)).Distinct().ToList();
                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && Projects.Contains(Convert.ToString(i.Project)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());
                            if (SelectedOTCode != null && SelectedOTCode.Count > 0)
                            {

                                List<string> OTCodes = SelectedOTCode.Select(i => Convert.ToString((i as PlantLoadAnalysis).OTCode)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && OTCodes.Contains(Convert.ToString(i.OTCode)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());

                                if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                                {
                                    List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                                    if (SelectedCPType != null && SelectedCPType.Count > 0)
                                    {
                                        List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                    }
                                }
                                else if (SelectedCPType != null && SelectedCPType.Count > 0)
                                {
                                    List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                }
                            }
                            else if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                            {
                                List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                                if (SelectedCPType != null && SelectedCPType.Count > 0)
                                {
                                    List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                }
                            }
                        }
                    }
                    else if (SelectedCustomerPlant != null && SelectedCustomerPlant.Count > 0)
                    {
                        List<string> CustomerplantIds = SelectedCustomerPlant.Select(i => Convert.ToString((i as PlantLoadAnalysis).CustomerPlant)).Distinct().ToList();
                        // List<int> OTStatusCategoryIds = SelectedOTStatus.Select(i => Convert.ToInt32((i as PlantDeliveryAnalysis).IdProductCategory)).Distinct().ToList();

                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());
                        if (SelectedProject != null && SelectedProject.Count > 0)
                        {
                            List<string> Projects = SelectedProject.Select(i => Convert.ToString((i as PlantLoadAnalysis).Project)).Distinct().ToList();
                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && Projects.Contains(Convert.ToString(i.Project)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());
                            if (SelectedOTCode != null && SelectedOTCode.Count > 0)
                            {

                                List<string> OTCodes = SelectedOTCode.Select(i => Convert.ToString((i as PlantLoadAnalysis).OTCode)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());

                                if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                                {
                                    List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                                    if (SelectedCPType != null && SelectedCPType.Count > 0)
                                    {
                                        List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                    }
                                }
                                else if (SelectedCPType != null && SelectedCPType.Count > 0)
                                {
                                    List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                }
                            }
                            else if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                            {
                                List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                                if (SelectedCPType != null && SelectedCPType.Count > 0)
                                {
                                    List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                }
                            }
                        }
                        else if (SelectedOTCode != null && SelectedOTCode.Count > 0)
                        {

                            List<string> OTCodes = SelectedOTCode.Select(i => Convert.ToString((i as PlantLoadAnalysis).OTCode)).Distinct().ToList();
                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());

                            if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                            {
                                List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                                if (SelectedCPType != null && SelectedCPType.Count > 0)
                                {
                                    List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                }
                            }
                            else if (SelectedCPType != null && SelectedCPType.Count > 0)
                            {
                                List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                            }
                        }
                        else if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                        {
                            List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                            if (SelectedCPType != null && SelectedCPType.Count > 0)
                            {
                                List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                            }
                        }
                        else if (SelectedCPType != null && SelectedCPType.Count > 0)
                        {
                            List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                        }
                    }
                    else if (SelectedProject != null && SelectedProject.Count > 0)
                    {
                        List<string> Projects = SelectedProject.Select(i => Convert.ToString((i as PlantLoadAnalysis).Project)).Distinct().ToList();
                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => Projects.Contains(Convert.ToString(i.Project)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());
                        if (SelectedOTCode != null && SelectedOTCode.Count > 0)
                        {

                            List<string> OTCodes = SelectedOTCode.Select(i => Convert.ToString((i as PlantLoadAnalysis).OTCode)).Distinct().ToList();
                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => OTCodes.Contains(Convert.ToString(i.OTCode)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());

                            if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                            {
                                List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                                if (SelectedCPType != null && SelectedCPType.Count > 0)
                                {
                                    List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                }
                            }
                            else if (SelectedCPType != null && SelectedCPType.Count > 0)
                            {
                                List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                            }
                        }
                        else if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                        {
                            List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                            if (SelectedCPType != null && SelectedCPType.Count > 0)
                            {
                                List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                            }
                        }
                    }
                    else if (SelectedOTCode != null && SelectedOTCode.Count > 0)
                    {

                        List<string> OTCodes = SelectedOTCode.Select(i => Convert.ToString((i as PlantLoadAnalysis).OTCode)).Distinct().ToList();
                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => OTCodes.Contains(Convert.ToString(i.OTCode)) && OTItemStatus.Contains(Convert.ToString(i.OTItemStatus))).ToList());

                        if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                        {
                            List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => OTCodes.Contains(Convert.ToString(i.OTCode)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                            if (SelectedCPType != null && SelectedCPType.Count > 0)
                            {
                                List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => OTCodes.Contains(Convert.ToString(i.OTCode)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                            }
                        }
                        else if (SelectedCPType != null && SelectedCPType.Count > 0)
                        {
                            List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => OTCodes.Contains(Convert.ToString(i.OTCode)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                        }
                    }
                    else if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                    {
                        List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                        if (SelectedCPType != null && SelectedCPType.Count > 0)
                        {
                            List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                        }
                    }
                    else if (SelectedCPType != null && SelectedCPType.Count > 0)
                    {
                        List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                    }
                }
                else if (SelectedCustomerGroup != null && SelectedCustomerGroup.Count > 0)
                {
                    List<int> CustomerIds = SelectedCustomerGroup.Select(i => Convert.ToInt32((i as PlantLoadAnalysis).IdCustomer)).Distinct().ToList();

                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer)).ToList());
                    if (SelectedCustomerPlant != null && SelectedCustomerPlant.Count > 0)
                    {
                        List<string> CustomerplantIds = SelectedCustomerPlant.Select(i => Convert.ToString((i as PlantLoadAnalysis).CustomerPlant)).Distinct().ToList();
                        // List<int> OTStatusCategoryIds = SelectedOTStatus.Select(i => Convert.ToInt32((i as PlantDeliveryAnalysis).IdProductCategory)).Distinct().ToList();

                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant))).ToList());
                        if (SelectedProject != null && SelectedProject.Count > 0)
                        {
                            List<string> Projects = SelectedProject.Select(i => Convert.ToString((i as PlantLoadAnalysis).Project)).Distinct().ToList();
                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && Projects.Contains(Convert.ToString(i.Project))).ToList());
                            if (SelectedOTCode != null && SelectedOTCode.Count > 0)
                            {

                                List<string> OTCodes = SelectedOTCode.Select(i => Convert.ToString((i as PlantLoadAnalysis).OTCode)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode))).ToList());

                                if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                                {
                                    List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                                    if (SelectedCPType != null && SelectedCPType.Count > 0)
                                    {
                                        List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                    }
                                }
                                else if (SelectedCPType != null && SelectedCPType.Count > 0)
                                {
                                    List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                }
                            }
                            else if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                            {
                                List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                                if (SelectedCPType != null && SelectedCPType.Count > 0)
                                {
                                    List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                }
                            }
                        }
                        else if (SelectedOTCode != null && SelectedOTCode.Count > 0)
                        {

                            List<string> OTCodes = SelectedOTCode.Select(i => Convert.ToString((i as PlantLoadAnalysis).OTCode)).Distinct().ToList();
                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode))).ToList());

                            if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                            {
                                List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                                if (SelectedCPType != null && SelectedCPType.Count > 0)
                                {
                                    List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                }
                            }
                            else if (SelectedCPType != null && SelectedCPType.Count > 0)
                            {
                                List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                            }
                        }
                        else if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                        {
                            List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                            if (SelectedCPType != null && SelectedCPType.Count > 0)
                            {
                                List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                            }
                        }
                        else if (SelectedCPType != null && SelectedCPType.Count > 0)
                        {
                            List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                        }
                    }
                    else if (SelectedProject != null && SelectedProject.Count > 0)
                    {
                        List<string> Projects = SelectedProject.Select(i => Convert.ToString((i as PlantLoadAnalysis).Project)).Distinct().ToList();
                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && Projects.Contains(Convert.ToString(i.Project))).ToList());
                        if (SelectedOTCode != null && SelectedOTCode.Count > 0)
                        {

                            List<string> OTCodes = SelectedOTCode.Select(i => Convert.ToString((i as PlantLoadAnalysis).OTCode)).Distinct().ToList();
                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && OTCodes.Contains(Convert.ToString(i.OTCode))).ToList());

                            if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                            {
                                List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                                if (SelectedCPType != null && SelectedCPType.Count > 0)
                                {
                                    List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                }
                            }
                            else if (SelectedCPType != null && SelectedCPType.Count > 0)
                            {
                                List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                            }
                        }
                        else if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                        {
                            List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                            if (SelectedCPType != null && SelectedCPType.Count > 0)
                            {
                                List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                            }
                        }
                        else if (SelectedCPType != null && SelectedCPType.Count > 0)
                        {
                            List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && Projects.Contains(Convert.ToString(i.Project)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                        }
                    }
                    else if (SelectedOTCode != null && SelectedOTCode.Count > 0)
                    {

                        List<string> OTCodes = SelectedOTCode.Select(i => Convert.ToString((i as PlantLoadAnalysis).OTCode)).Distinct().ToList();
                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && OTCodes.Contains(Convert.ToString(i.OTCode))).ToList());

                        if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                        {
                            List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && OTCodes.Contains(Convert.ToString(i.OTCode)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                            if (SelectedCPType != null && SelectedCPType.Count > 0)
                            {
                                List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && OTCodes.Contains(Convert.ToString(i.OTCode)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                            }
                        }
                        else if (SelectedCPType != null && SelectedCPType.Count > 0)
                        {
                            List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && OTCodes.Contains(Convert.ToString(i.OTCode)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                        }
                    }
                    else if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                    {
                        List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                        if (SelectedCPType != null && SelectedCPType.Count > 0)
                        {
                            List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerIds.Contains(i.IdCustomer) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                        }
                    }
                }
                else if (SelectedCustomerPlant != null && SelectedCustomerPlant.Count > 0)
                {
                    List<string> CustomerplantIds = SelectedCustomerPlant.Select(i => Convert.ToString((i as PlantLoadAnalysis).CustomerPlant)).Distinct().ToList();
                    // List<int> OTStatusCategoryIds = SelectedOTStatus.Select(i => Convert.ToInt32((i as PlantDeliveryAnalysis).IdProductCategory)).Distinct().ToList();

                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant))).ToList());
                    if (SelectedProject != null && SelectedProject.Count > 0)
                    {
                        List<string> Projects = SelectedProject.Select(i => Convert.ToString((i as PlantLoadAnalysis).Project)).Distinct().ToList();
                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && Projects.Contains(Convert.ToString(i.Project))).ToList());
                        if (SelectedOTCode != null && SelectedOTCode.Count > 0)
                        {

                            List<string> OTCodes = SelectedOTCode.Select(i => Convert.ToString((i as PlantLoadAnalysis).OTCode)).Distinct().ToList();
                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode))).ToList());

                            if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                            {
                                List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                                if (SelectedCPType != null && SelectedCPType.Count > 0)
                                {
                                    List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                                }
                            }
                            else if (SelectedCPType != null && SelectedCPType.Count > 0)
                            {
                                List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                            }
                        }
                        else if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                        {
                            List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                            if (SelectedCPType != null && SelectedCPType.Count > 0)
                            {
                                List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                            }
                        }
                    }
                    else if (SelectedOTCode != null && SelectedOTCode.Count > 0)
                    {

                        List<string> OTCodes = SelectedOTCode.Select(i => Convert.ToString((i as PlantLoadAnalysis).OTCode)).Distinct().ToList();
                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode))).ToList());

                        if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                        {
                            List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                            if (SelectedCPType != null && SelectedCPType.Count > 0)
                            {
                                List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                            }
                        }
                        else if (SelectedCPType != null && SelectedCPType.Count > 0)
                        {
                            List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && OTCodes.Contains(Convert.ToString(i.OTCode)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                        }
                    }
                    else if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                    {
                        List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                        if (SelectedCPType != null && SelectedCPType.Count > 0)
                        {
                            List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                        }
                    }
                    else if (SelectedCPType != null && SelectedCPType.Count > 0)
                    {
                        List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CustomerplantIds.Contains(Convert.ToString(i.CustomerPlant)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                    }
                }
                else if (SelectedProject != null && SelectedProject.Count > 0)
                {
                    List<string> Projects = SelectedProject.Select(i => Convert.ToString((i as PlantLoadAnalysis).Project)).Distinct().ToList();
                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => Projects.Contains(Convert.ToString(i.Project))).ToList());
                    if (SelectedOTCode != null && SelectedOTCode.Count > 0)
                    {

                        List<string> OTCodes = SelectedOTCode.Select(i => Convert.ToString((i as PlantLoadAnalysis).OTCode)).Distinct().ToList();
                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => OTCodes.Contains(Convert.ToString(i.OTCode))).ToList());

                        if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                        {
                            List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                            if (SelectedCPType != null && SelectedCPType.Count > 0)
                            {
                                List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                            }
                        }
                        else if (SelectedCPType != null && SelectedCPType.Count > 0)
                        {
                            List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => OTCodes.Contains(Convert.ToString(i.OTCode)) && Projects.Contains(Convert.ToString(i.Project)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                        }
                    }
                    else if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                    {
                        List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                        if (SelectedCPType != null && SelectedCPType.Count > 0)
                        {
                            List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => Projects.Contains(Convert.ToString(i.Project)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                        }
                    }
                    else if (SelectedCPType != null && SelectedCPType.Count > 0)
                    {
                        List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => Projects.Contains(Convert.ToString(i.Project)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                    }
                }
                else if (SelectedOTCode != null && SelectedOTCode.Count > 0)
                {

                    List<string> OTCodes = SelectedOTCode.Select(i => Convert.ToString((i as PlantLoadAnalysis).OTCode)).Distinct().ToList();
                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => OTCodes.Contains(Convert.ToString(i.OTCode))).ToList());

                    if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                    {
                        List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => OTCodes.Contains(Convert.ToString(i.OTCode)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                        if (SelectedCPType != null && SelectedCPType.Count > 0)
                        {
                            List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                            PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => OTCodes.Contains(Convert.ToString(i.OTCode)) && ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                        }
                    }
                    else if (SelectedCPType != null && SelectedCPType.Count > 0)
                    {
                        List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => OTCodes.Contains(Convert.ToString(i.OTCode)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                    }
                }
                else if (SelectedConnectorFamily != null && SelectedConnectorFamily.Count > 0)
                {
                    List<string> ConnectorFamilys = SelectedConnectorFamily.Select(i => Convert.ToString((i as PlantLoadAnalysis).ConnectorFamily)).Distinct().ToList();
                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily))).ToList());

                    if (SelectedCPType != null && SelectedCPType.Count > 0)
                    {
                        List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                        PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => ConnectorFamilys.Contains(Convert.ToString(i.ConnectorFamily)) && CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                    }
                }
                else if (SelectedCPType != null && SelectedCPType.Count > 0)
                {
                    List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                    PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList_Cloned.Where(i => CPTypes.Contains(Convert.ToString(i.CPType))).ToList());

                }

                PlantLoadAnalysisList = new List<PlantLoadAnalysis>(PlantLoadAnalysisList);


                GeosApplication.Instance.Logger.Log("Method ApplyFilterConditions()....executed successfully", category: Category.Info, priority: Priority.Low);
            }

            catch (Exception ex)
            {

                GeosApplication.Instance.Logger.Log("Get an error in ApplyFilterConditions() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        #region Save Filters  [GEOS2-5114][28-12-2023][Rupali Sarode]

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

                if (SelectedRegion != null)
                {
                    XmlElement ParentNodeRegions = doc.CreateElement(string.Empty, "Regions", string.Empty);
                    element1.AppendChild(ParentNodeRegions);

                    foreach (PlantLoadAnalysis ObjRegion in SelectedRegion)
                    {
                        XmlElement childNode1 = doc.CreateElement("Region");
                        XmlText RegionId = doc.CreateTextNode(ObjRegion.Region);
                        childNode1.AppendChild(RegionId);
                        ParentNodeRegions.AppendChild(childNode1);
                    }

                }

                if (SelectedTemplate != null)
                {
                    List<string> TemplateName = SelectedTemplate.Select(i => Convert.ToString((i as PlantLoadAnalysis).Template)).Distinct().ToList();
                    XmlElement ParentNodeTemplates = doc.CreateElement("Templates");
                    element1.AppendChild(ParentNodeTemplates);

                    foreach (string ObjTemplate in TemplateName)
                    {
                        XmlElement childNode1 = doc.CreateElement("Template");
                        XmlText Template = doc.CreateTextNode(ObjTemplate);
                        childNode1.AppendChild(Template);
                        ParentNodeTemplates.AppendChild(childNode1);
                    }
                }

                if (SelectedOTItemStatus != null)
                {
                    List<string> OTItemStatus = SelectedOTItemStatus.Select(i => Convert.ToString((i as PlantLoadAnalysis).OTItemStatus)).Distinct().ToList();
                    XmlElement ParentNodeOTItemStatus = doc.CreateElement("OTItemStatuss");
                    element1.AppendChild(ParentNodeOTItemStatus);
                    foreach (string ObjOTItemStatus in OTItemStatus)
                    {
                        XmlElement childNode1 = doc.CreateElement("OTItemStatus");
                        XmlText xmlOTItemStatus = doc.CreateTextNode(ObjOTItemStatus);
                        childNode1.AppendChild(xmlOTItemStatus);
                        ParentNodeOTItemStatus.AppendChild(childNode1);
                    }
                }

                if (SelectedCustomerGroup != null)
                {
                    List<string> CustomerGroupIds = SelectedCustomerGroup.Select(i => Convert.ToString((i as PlantLoadAnalysis).IdCustomer)).Distinct().ToList();
                    XmlElement ParentNodeCustomerGroups = doc.CreateElement("CustomerGroups");
                    element1.AppendChild(ParentNodeCustomerGroups);

                    foreach (string ObjCustomerGroup in CustomerGroupIds)
                    {
                        XmlElement childNode1 = doc.CreateElement("CustomerGroup");
                        XmlText CustomerGroup = doc.CreateTextNode(ObjCustomerGroup);
                        childNode1.AppendChild(CustomerGroup);
                        ParentNodeCustomerGroups.AppendChild(childNode1);
                    }
                }

                if (SelectedCustomerPlant != null)
                {
                    List<string> CustomerPlantIds = SelectedCustomerPlant.Select(i => Convert.ToString((i as PlantLoadAnalysis).CustomerPlant)).Distinct().ToList();
                    XmlElement ParentNodeCustomerPlants = doc.CreateElement("CustomerPlants");
                    element1.AppendChild(ParentNodeCustomerPlants);

                    foreach (string ObjCustomerPlant in CustomerPlantIds)
                    {
                        XmlElement childNode1 = doc.CreateElement("CustomerPlant");
                        XmlText CustomerGroup = doc.CreateTextNode(ObjCustomerPlant);
                        childNode1.AppendChild(CustomerGroup);
                        ParentNodeCustomerPlants.AppendChild(childNode1);
                    }
                }




                if (SelectedProject != null && SelectedProject.Count > 0)
                {
                    List<string> ProjectIds = SelectedProject.Select(i => Convert.ToString((i as PlantLoadAnalysis).Project)).Distinct().ToList();
                    XmlElement ParentNodeProject = doc.CreateElement("Projects");
                    element1.AppendChild(ParentNodeProject);

                    foreach (string ObjProject in ProjectIds)
                    {
                        XmlElement childNode1 = doc.CreateElement("ProjectId");
                        XmlText Project = doc.CreateTextNode(ObjProject);
                        childNode1.AppendChild(Project);
                        ParentNodeProject.AppendChild(childNode1);
                    }
                }

                if (SelectedOTCode != null)
                {
                    List<string> OTCode = SelectedOTCode.Select(i => Convert.ToString((i as PlantLoadAnalysis).OTCode)).Distinct().ToList();
                    XmlElement ParentNodeOTStatus = doc.CreateElement("OTCodes");
                    element1.AppendChild(ParentNodeOTStatus);

                    foreach (string ObjOTCode in OTCode)
                    {
                        XmlElement childNode1 = doc.CreateElement("OTCode");
                        XmlText OtCode = doc.CreateTextNode(ObjOTCode);
                        childNode1.AppendChild(OtCode);
                        ParentNodeOTStatus.AppendChild(childNode1);
                    }
                }

                if (SelectedConnectorFamily != null)
                {
                    List<string> ConnectorFamilies = SelectedConnectorFamily.Select(i => (i as PlantLoadAnalysis).ConnectorFamily).Distinct().ToList();
                    XmlElement ParentNodeConnectorFamily = doc.CreateElement("ConnectorFamily");
                    element1.AppendChild(ParentNodeConnectorFamily);

                    foreach (string ObjConnectorFamily in ConnectorFamilies)
                    {
                        XmlElement childNode1 = doc.CreateElement("ConnectorFamilyId");
                        XmlText ConnectorFamily = doc.CreateTextNode(Convert.ToString(ObjConnectorFamily));
                        childNode1.AppendChild(ConnectorFamily);
                        ParentNodeConnectorFamily.AppendChild(childNode1);
                    }
                }

                if (SelectedCPType != null)
                {
                    List<string> CPTypes = SelectedCPType.Select(i => Convert.ToString((i as PlantLoadAnalysis).CPType)).Distinct().ToList();
                    XmlElement ParentNodeCPType = doc.CreateElement("CPTypes");
                    element1.AppendChild(ParentNodeCPType);

                    foreach (string ObjCPType in CPTypes)
                    {
                        XmlElement childNode1 = doc.CreateElement("CPType");
                        XmlText CPTypeText = doc.CreateTextNode(ObjCPType);
                        childNode1.AppendChild(CPTypeText);
                        ParentNodeCPType.AppendChild(childNode1);
                    }
                }

                doc.Save(PlantLoadAnalysisFilterSettingFilePath);

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

                TempXMLRegionsList = new List<string>();
                TempXMLCustomerGroupsList = new List<Int32>();
                TempXMLCustomerPlantsList = new List<string>();
                TempXMLOTCodeList = new List<string>();
                TempXMLProjectList = new List<string>();
                TempXMLConnectorFamilyList = new List<string>();
                TempXMLCptypeList = new List<string>();
                TempXMLTemplatesList = new List<string>();
                TempXMLOTItemStatussList = new List<string>();



                if (File.Exists(PlantLoadAnalysisFilterSettingFilePath))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(PlantLoadAnalysisFilterSettingFilePath);

                    XmlNodeList NodeListRegions = doc.SelectNodes("/body/Regions");

                    foreach (XmlNode node in NodeListRegions)
                    {
                        foreach (XmlNode child in node.ChildNodes)
                        {
                            TempXMLRegionsList.Add(child.InnerText);
                        }
                    }

                    XmlNodeList NodeListTemplates = doc.SelectNodes("/body/Templates");

                    foreach (XmlNode node in NodeListTemplates)
                    {
                        foreach (XmlNode child in node.ChildNodes)
                        {
                            TempXMLTemplatesList.Add(child.InnerText);
                        }
                    }


                    XmlNodeList NodeListOTItemStatuss = doc.SelectNodes("/body/OTItemStatuss");

                    foreach (XmlNode node in NodeListOTItemStatuss)
                    {
                        foreach (XmlNode child in node.ChildNodes)
                        {
                            TempXMLOTItemStatussList.Add(child.InnerText);
                        }
                    }

                    XmlNodeList NodeListCustomerGroups = doc.SelectNodes("/body/CustomerGroups");

                    foreach (XmlNode node in NodeListCustomerGroups)
                    {
                        foreach (XmlNode child in node.ChildNodes)
                        {
                            TempXMLCustomerGroupsList.Add(Convert.ToInt32(child.InnerText));
                        }
                    }

                    XmlNodeList NodeListCustomerPlants = doc.SelectNodes("/body/CustomerPlants");

                    foreach (XmlNode node in NodeListCustomerPlants)
                    {
                        foreach (XmlNode child in node.ChildNodes)
                        {
                            TempXMLCustomerPlantsList.Add(child.InnerText);
                        }
                    }


                   

                    XmlNodeList NodeListProjects = doc.SelectNodes("/body/Projects");

                    foreach (XmlNode node in NodeListProjects)
                    {
                        
                            foreach (XmlNode child in node.ChildNodes)
                        {
                           
                                TempXMLProjectList.Add(child.InnerText);
                            
                        }
                    }

                    XmlNodeList NodeListOTCodes = doc.SelectNodes("/body/OTCodes");

                    foreach (XmlNode node in NodeListOTCodes)
                    {
                        foreach (XmlNode child in node.ChildNodes)
                        {
                            TempXMLOTCodeList.Add(child.InnerText);
                        }
                    }

                    XmlNodeList NodeListConnectorFamily = doc.SelectNodes("/body/ConnectorFamily");

                    foreach (XmlNode node in NodeListConnectorFamily)
                    {
                        foreach (XmlNode child in node.ChildNodes)
                        {
                            TempXMLConnectorFamilyList.Add(child.InnerText);
                        }
                    }

                    XmlNodeList NodeListCPTypes = doc.SelectNodes("/body/CPTypes");

                    foreach (XmlNode node in NodeListCPTypes)
                    {
                        foreach (XmlNode child in node.ChildNodes)
                        {
                            TempXMLCptypeList.Add(child.InnerText);
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

        private void PLAWorkstationGridControlUnloadedCommandAction(object obj)
        {

            try
            {
                GeosApplication.Instance.Logger.Log("Method PLAWorkstationGridControlUnloadedCommandAction ...", category: Category.Info, priority: Priority.Low);

                CreateXmlFile();

                GeosApplication.Instance.Logger.Log("Method PLAWorkstationGridControlUnloadedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in PLAWorkstationGridControlUnloadedCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        #endregion Save Filters [GEOS2-5114][28-12-2023][Rupali Sarode]
        #endregion

        private void ShowLoadModulesDialogWindowCommandsAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowLoadModulesDialogWindowCommandsAction()...", category: Category.Info, priority: Priority.Low);

                PLAShowLoadModulesViewModel PLAShowLoadModulesViewModel = new PLAShowLoadModulesViewModel();
                PLAShowLoadModulesView PLAShowLoadModulesView = new PLAShowLoadModulesView();

                EventHandler handle = delegate { PLAShowLoadModulesView.Close(); };

                PLAShowLoadModulesViewModel.PlantLoadAnalysisList = PlantLoadAnalysisList;
                PLAShowLoadModulesViewModel.DataTableForGridLayoutProductionInTime = DataTableForGridLayoutProductionInTime;
                PLAShowLoadModulesViewModel.BandsDashboard = BandsDashboard;
                PLAShowLoadModulesViewModel.GroupByTempPlantLoadAnalysis = GroupByTempPlantLoadAnalysisCopySummaryModule;
                PLAShowLoadModulesViewModel.RequestClose += handle;
                PLAShowLoadModulesView.DataContext = PLAShowLoadModulesViewModel;
                PLAShowLoadModulesViewModel.Init();
                PLAShowLoadModulesView.ShowDialog();

                GeosApplication.Instance.Logger.Log("Method ShowLoadModulesDialogWindowCommandsAction()....executed successfully", category: Category.Info, priority: Priority.Low);


            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ShowLoadModulesDialogWindowCommandsAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);

            }
        }

        private void ShowLoadEquipmentDialogWindowCommandsAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowLoadEquipmentDialogWindowCommandsAction()...", category: Category.Info, priority: Priority.Low);

                PLAShowLoadEquipmentViewModel PLAShowLoadEquipmentViewModel = new PLAShowLoadEquipmentViewModel();
                PLAShowLoadEquipmentView PLAShowLoadEquipmentView = new PLAShowLoadEquipmentView();

                EventHandler handle = delegate { PLAShowLoadEquipmentView.Close(); };

                PLAShowLoadEquipmentViewModel.PlantLoadAnalysisList = PlantLoadAnalysisList;
                PLAShowLoadEquipmentViewModel.DataTableForGridLayoutLoadEquipment = DataTableForGridLayoutLoadEquipment;
                PLAShowLoadEquipmentViewModel.BandsDashboardLoadEquipment = BandsDashboardLoadEquipment;
                PLAShowLoadEquipmentViewModel.GroupByTempPlantLoadAnalysisEquipment = GroupByTempPlantLoadAnalysisCopySummaryEquipment;
                PLAShowLoadEquipmentViewModel.RequestClose += handle;
                PLAShowLoadEquipmentView.DataContext = PLAShowLoadEquipmentViewModel;
                PLAShowLoadEquipmentViewModel.Init();
                PLAShowLoadEquipmentView.ShowDialog();

                GeosApplication.Instance.Logger.Log("Method ShowLoadEquipmentDialogWindowCommandsAction()....executed successfully", category: Category.Info, priority: Priority.Low);


            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ShowLoadEquipmentDialogWindowCommandsAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);

            }
        }

        private void ShowLoadCustomersDialogWindowCommandsAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowLoadCustomersDialogWindowCommandsAction()...", category: Category.Info, priority: Priority.Low);

                PLAShowLoadCustomersViewModel PLAShowLoadCustomersViewModel = new PLAShowLoadCustomersViewModel();
                PLAShowLoadCustomersView PLAShowLoadCustomersView = new PLAShowLoadCustomersView();

                EventHandler handle = delegate { PLAShowLoadCustomersView.Close(); };

                PLAShowLoadCustomersViewModel.PlantLoadAnalysisList = PlantLoadAnalysisList;

                PLAShowLoadCustomersViewModel.RequestClose += handle;
                PLAShowLoadCustomersView.DataContext = PLAShowLoadCustomersViewModel;
                PLAShowLoadCustomersViewModel.Init();
                PLAShowLoadCustomersView.ShowDialog();

                GeosApplication.Instance.Logger.Log("Method ShowLoadCustomersDialogWindowCommandsAction()....executed successfully", category: Category.Info, priority: Priority.Low);


            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ShowLoadCustomersDialogWindowCommandsAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);

            }
        }

        private void ShowLoadWorkStationDialogWindowCommandsAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowLoadWorkStationDialogWindowCommandsAction()...", category: Category.Info, priority: Priority.Low);

                PLAShowLoadWorkStationViewModel PLAShowLoadWorkStationViewModel = new PLAShowLoadWorkStationViewModel();
                PLAShowLoadWorkStationView PLAShowLoadWorkStationView = new PLAShowLoadWorkStationView();

                EventHandler handle = delegate { PLAShowLoadWorkStationView.Close(); };

                PLAShowLoadWorkStationViewModel.PlantLoadAnalysisList = PlantLoadAnalysisList;
                PLAShowLoadWorkStationViewModel.LoadWorkstationStageList = LoadWorkstationStageList;

                PLAShowLoadWorkStationViewModel.RequestClose += handle;
                PLAShowLoadWorkStationView.DataContext = PLAShowLoadWorkStationViewModel;
                PLAShowLoadWorkStationViewModel.Init();
                PLAShowLoadWorkStationView.ShowDialog();

                GeosApplication.Instance.Logger.Log("Method ShowLoadWorkStationDialogWindowCommandsAction()....executed successfully", category: Category.Info, priority: Priority.Low);


            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ShowLoadWorkStationDialogWindowCommandsAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);

            }
        }
        #endregion

    }

    public class Helper
    {
        public static readonly DependencyProperty ShowCustomCaptionProperty =
            DependencyProperty.RegisterAttached("ShowCustomCaption", typeof(bool), typeof(Helper), new UIPropertyMetadata(false));
        public static bool GetShowCustomCaption(DependencyObject obj)
        {
            return (bool)obj.GetValue(ShowCustomCaptionProperty);
        }
        public static void SetShowCustomCaption(DependencyObject obj, bool value)
        {
            obj.SetValue(ShowCustomCaptionProperty, value);
        }
    }
}
