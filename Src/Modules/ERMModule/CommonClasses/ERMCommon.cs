using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.ServiceProcess;
using DevExpress.Mvvm;
using Emdep.Geos.UI.Common;
using DevExpress.Mvvm.POCO;
using DevExpress.Mvvm.UI;
using System.Collections.ObjectModel;
using Emdep.Geos.Data.Common;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Media;
using System.Drawing;
using Prism.Logging;
using DevExpress.Xpf.LayoutControl;
using System.ComponentModel;
using Emdep.Geos.Utility;
using System.Windows;
using Emdep.Geos.Data.Common.PCM;
using System.Windows.Controls;
using Emdep.Geos.Data.Common.ERM;
using System.Data;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.Data.Common.Hrm;

namespace Emdep.Geos.Modules.ERM.CommonClasses
{
    /// <summary>
    ///
    /// </summary>
    public sealed class ERMCommon : Prism.Mvvm.BindableBase
    {
        #region  Declaration
        private string eRM_Appearance;
        MaximizedElementPosition maximizedElementPosition;
        private static readonly ERMCommon instance = new ERMCommon();
        private long selectedPeriod;
        private List<object> selectedAuthorizedPlantsList;
        private bool unloadAsyncTimetracking;  //[gulab lakade][27 03 2023][Async fail message issue]

        private ObservableCollection<Site> userAuthorizedPlantsList;
        private bool plantVisibleFlag;  //[gulab lakade][21 04 2023][Plant dropdown disable]
        DateTime startDate;
        DateTime endDate;
        #region [GEOS2-5883][gulab lakade][27 06 2024]
        private List<ERMWorkStageWiseJobDescription> workStageWiseJobDescription;
        private ObservableCollection<PlanningDateReviewStages> stageList;
        #endregion
        private string currencySymbolFromSetting;//Aishwarya ingale[Geos2-6431]
        private List<ERM_EMP_ProductionTimeline> emp_Productiontime;//[GEOS2-6738][gulab lakade][02 01 2025]
        private ERM_EMP_ProductionTimeline select_emp_Productiontime;//[GEOS2-6738][gulab lakade][02 01 2025]
        DateTime planningDate; //[GEOS2-6885][Daivshala Vighne][29-01-2025]
        private List<ERM_Warehouses> warehouseList;//[GEOS2-6885][Daivshala Vighne][30-01-2025]
        private ERM_Warehouses selectedwarehouse;//[GEOS2-6885][Daivshala Vighne][30-01-2025]
        List<ERMEmployeeLeave> employeeLeave;//[GEOS2-6965][rani dhamankar][10-03-2025]
        private List<Company> plantOwnerUsersList;//[GEOS2-8698][rani dhamankar][15-07-2025]
        #region [GEOS2-9220][gulab lakade][08 08 2025]
        private ERM_Main_productiontimeline main_Productiontimeline ;
        private List<ERM_EmployeeDetails> eRM_EmployeeDetails;
        private List<ERM_EmployeeDetails> eRM_EmployeeDetailsList;
        private List<ERM_Employee_Attendance> eRM_Employee_Attendance;
        private List<ERM_Employee_Attendance> eRM_Employee_MAX_Attendance;
        private List<ERMEmployeeLeave> eRMEmployeeLeave;
        private List<ERMCompanyHoliday> eRMCompanyHoliday;
        private List<Counterpartstracking> eRM_Counterpartstracking;
        private List<ERM_NO_OT_Time> eRM_NO_OT_Time;
        private string selectedproductionTimeline;
        private List<DesignSharedItemsEmployeeDetails> designSharedItemsEmployeeDetails;//[GEOS2-7091][rani dhamankar][11 09 2025]
        private List<Counterpartstracking> eRM_SharedOtCounterpartstracking; //[GEOS2-7091][rani dhamankar][11 09 2025]

        private ERM_WorkOrder_Other_ProductionTimeline wOO_ProductionTimeline;
        private List<ERM_OT_Working_Times> eRM_OT_Working_Times;//[GEOS2-9393][pallavi jadhav][12 11 2025]
        #endregion
        #endregion

        #region Public Properties

        public static ERMCommon Instance
        {
            get { return instance; }
        }
        string timeTrackingLoadingMessage = string.Empty;
        string currentYearONTimeDeliveryAVG = string.Empty;
        string lastYearONTimeDeliveryAVG = string.Empty;
        string currentYearDeliveryAVG = string.Empty;
        string lastYearDeliveryAVG = string.Empty;
        public string TimeTrackingLoadingMessage
        {
            get
            {
                return timeTrackingLoadingMessage;
            }
            set
            {
                timeTrackingLoadingMessage = value;
                OnPropertyChanged(nameof(TimeTrackingLoadingMessage));
            }
        }
        public bool UnloadAsyncTimetracking  //[gulab lakade][27 03 2023][Async fail message issue]
        {
            get
            {
                return unloadAsyncTimetracking;
            }
            set
            {
                unloadAsyncTimetracking = value;
                OnPropertyChanged(nameof(UnloadAsyncTimetracking));
            }
        }
        public bool PlantVisibleFlag  //[gulab lakade][21 04 2023][Plant dropdown disable]
        {
            get
            {
                return plantVisibleFlag;
            }
            set
            {
                plantVisibleFlag = value;
                OnPropertyChanged(nameof(PlantVisibleFlag));
            }
        }

        public string CurrentYearONTimeDeliveryAVG
        {
            get
            {
                return currentYearONTimeDeliveryAVG;
            }
            set
            {
                currentYearONTimeDeliveryAVG = value;
                OnPropertyChanged(nameof(CurrentYearONTimeDeliveryAVG));
            }
        }
        public string LastYearONTimeDeliveryAVG
        {
            get
            {
                return lastYearONTimeDeliveryAVG;
            }
            set
            {
                lastYearONTimeDeliveryAVG = value;
                OnPropertyChanged(nameof(LastYearONTimeDeliveryAVG));
            }
        }


        public string CurrentYearDeliveryAVG
        {
            get
            {
                return currentYearDeliveryAVG;
            }
            set
            {
                currentYearDeliveryAVG = value;
                OnPropertyChanged(nameof(CurrentYearDeliveryAVG));
            }
        }
        public string LastYearDeliveryAVG
        {
            get
            {
                return lastYearDeliveryAVG;
            }
            set
            {
                lastYearDeliveryAVG = value;
                OnPropertyChanged(nameof(LastYearDeliveryAVG));
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
                OnPropertyChanged("StartDate");
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
                OnPropertyChanged("EndDate");
            }
        }


        //[Aishwarya Ingale][4920][22/11/2023]
        public string ERM_Appearance
        {
            get { return eRM_Appearance; }
            set
            {
                eRM_Appearance = value;
                OnPropertyChanged("ERM_Appearance");
            }
        }

        //[Aishwarya Ingale][4920][22/11/2023]
        public MaximizedElementPosition MaximizedElementPosition
        {
            get { return maximizedElementPosition; }
            set
            {
                maximizedElementPosition = value;
                OnPropertyChanged("MaximizedElementPosition");
            }
        }

        #region [GEOS2-5883][gulab lakade][27 06 2024]
        public List<ERMWorkStageWiseJobDescription> WorkStageWiseJobDescription
        {
            get { return workStageWiseJobDescription; }
            set
            {
                workStageWiseJobDescription = value;
                OnPropertyChanged("WorkStageWiseJobDescription");
            }
        }
        public ObservableCollection<PlanningDateReviewStages> StageList
        {
            get { return stageList; }
            set
            {
                stageList = value;
                OnPropertyChanged("StageList");
            }
        }
        #endregion

        //Aishwarya ingale[Geos2-6431]
        public string CurrencySymbolFromSetting
        {
            get
            {
                return currencySymbolFromSetting;
            }

            set
            {
                currencySymbolFromSetting = value;
                OnPropertyChanged("CurrencySymbolFromSetting");
            }
        }
        public List<int> DetailsList_IDLookup = new List<int>();//[GEOS2-6716][gulab lakade][13 01 2025]
        #region [GEOS2-6885][Daivshala Vighne][29 01 2025]
        public DateTime PlanningDate
        {
            get
            {
                return planningDate;
            }

            set
            {
                planningDate = value;
                OnPropertyChanged("PlanningDate");
            }
        }

        public List<ERM_Warehouses> WarehouseList
        {
            get { return warehouseList; }
            set
            {
                warehouseList = value;
                this.OnPropertyChanged("WarehouseList");
            }
        }

        public ERM_Warehouses Selectedwarehouse
        {
            get { return selectedwarehouse; }
            set
            {
                selectedwarehouse = value;
                OnPropertyChanged("Selectedwarehouse");
            }
        }
        #endregion
        #region [GEOS2-9220][gulab lakade][08 08 2025]

        public ERM_Main_productiontimeline Main_Productiontimeline
        {
            get { return main_Productiontimeline; }
            set
            {
                main_Productiontimeline = value;
                OnPropertyChanged("Main_Productiontimeline");
            }
        }
        private ERM_Main_productiontimeline main_ProductionTimeList_Clone;
        public ERM_Main_productiontimeline Main_ProductionTimeList_Clone
        {

            get
            {
                return main_ProductionTimeList_Clone;
            }

            set
            {
                main_ProductionTimeList_Clone = value;
                OnPropertyChanged("Main_ProductionTimeList_Clone");
            }

        }
        public List<ERM_EmployeeDetails> ERM_EmployeeDetails
        {
            get { return eRM_EmployeeDetails; }
            set
            {
                eRM_EmployeeDetails = value;
                OnPropertyChanged("ERM_EmployeeDetails");
            }
        }
        public List<ERM_Employee_Attendance> ERM_Employee_Attendance
        {
            get { return eRM_Employee_Attendance; }
            set
            {
                eRM_Employee_Attendance = value;
                OnPropertyChanged("ERM_Employee_Attendance");
            }
        }
        public List<ERM_Employee_Attendance> ERM_Employee_MAX_Attendance
        {
            get { return eRM_Employee_MAX_Attendance; }
            set
            {
                eRM_Employee_MAX_Attendance = value;
                OnPropertyChanged("ERM_Employee_MAX_Attendance");
            }
        }
        public List<ERMEmployeeLeave> ERMEmployeeLeave
        {
            get { return eRMEmployeeLeave; }
            set
            {
                eRMEmployeeLeave = value;
                OnPropertyChanged("ERMEmployeeLeave");
            }
        }
        public List<ERMCompanyHoliday> ERMCompanyHoliday
        {
            get { return eRMCompanyHoliday; }
            set
            {
                eRMCompanyHoliday = value;
                OnPropertyChanged("ERMCompanyHoliday");
            }
        }
        public List<Counterpartstracking> ERM_Counterpartstracking
        {
            get { return eRM_Counterpartstracking; }
            set
            {
                eRM_Counterpartstracking = value;
                OnPropertyChanged("ERM_Counterpartstrackingy");
            }
        }
        public List<ERM_NO_OT_Time> ERM_NO_OT_Time
        {
            get { return eRM_NO_OT_Time; }
            set
            {
                eRM_NO_OT_Time = value;
                OnPropertyChanged("ERM_NO_OT_Time");
            }
        }
        public List<ERM_EmployeeDetails> ERM_EmployeeDetailsList
        {
            get { return eRM_EmployeeDetailsList; }
            set
            {
                eRM_EmployeeDetailsList = value;
                OnPropertyChanged("ERM_EmployeeDetailsList");
            }
        }
        public string SelectedproductionTimeline
        {
            get { return selectedproductionTimeline; }
            set
            {
                selectedproductionTimeline = value;
                OnPropertyChanged("SelectedproductionTimeline");
            }
        }

        #region [GEOS2-7091][rani dhamankar][11 09 2025]
        public List<DesignSharedItemsEmployeeDetails> DesignSharedItemsEmployeeDetails
        {
            get { return designSharedItemsEmployeeDetails; }
            set
            {
                designSharedItemsEmployeeDetails = value;
                OnPropertyChanged("DesignSharedItemsEmployeeDetails");
            }
        }

        public List<Counterpartstracking> ERM_SharedOtCounterpartstracking
        {
            get { return eRM_SharedOtCounterpartstracking; }
            set
            {
                eRM_SharedOtCounterpartstracking = value;
                OnPropertyChanged("ERM_SharedOtCounterpartstracking");
            }
        }
        #endregion
        #endregion
        #endregion

        #region [GEOS2-9393][pallavi jadhav][12 11 2025]
        public List<ERM_OT_Working_Times> ERM_OT_Working_Times
        {
            get { return eRM_OT_Working_Times; }
            set
            {
                eRM_OT_Working_Times = value;
                OnPropertyChanged("ERM_OT_Working_Times");
            }
        }
        #endregion


        #region Constructor

        public ERMCommon()
        {
        }

        #endregion

        #region Common methods

        #endregion

        private Visibility isVisibleLabelTimetrackingInBackground;
        public Visibility IsVisibleLabelTimetrackingInBackground
        {
            get
            {
                return isVisibleLabelTimetrackingInBackground;
            }
            set
            {
                isVisibleLabelTimetrackingInBackground = value;
                OnPropertyChanged(nameof(IsVisibleLabelTimetrackingInBackground));
            }
        }

        List<string> failedPlants;
        public List<string> FailedPlants
        {
            get { return failedPlants; }
            set { failedPlants = value; }
        }
        Boolean isShowFailedPlantWarning;

        public Boolean IsShowFailedPlantWarning
        {
            get { return isShowFailedPlantWarning; }
            set
            {
                isShowFailedPlantWarning = value;
                OnPropertyChanged(nameof(IsShowFailedPlantWarning));
                //OnPropertyChanged(new PropertyChangedEventArgs("IsShowFailedPlantWarning"));
            }
        }

        private void OnPropertyChanged(PropertyChangedEventArgs propertyChangedEventArgs)
        {
            throw new NotImplementedException();
        }

        //string warningFailedPlants = string.Empty;
        //public string WarningFailedPlants
        //{
        //    get
        //    {
        //        return warningFailedPlants;
        //    }
        //    set
        //    {
        //        warningFailedPlants = value;
        //        OnPropertyChanged(nameof(WarningFailedPlants));
        //    }
        //}
        string warningFailedPlants;
        public string WarningFailedPlants
        {
            get { return warningFailedPlants; }
            set
            {
                warningFailedPlants = value;
                OnPropertyChanged("WarningFailedPlants");
                //OnPropertyChanged(new PropertyChangedEventArgs("WarningFailedPlants"));
            }
        }

        public long SelectedPeriod
        {
            get { return selectedPeriod; }
            set
            {
                if (selectedPeriod != value)
                {
                    selectedPeriod = value;
                    OnPropertyChanged("SelectedPeriod");
                }
            }

        }

        public ObservableCollection<Site> UserAuthorizedPlantsList
        {
            get
            {
                return userAuthorizedPlantsList;
            }

            set
            {
                userAuthorizedPlantsList = value;
                OnPropertyChanged("UserAuthorizedPlantsList");
            }
        }

        public List<object> SelectedAuthorizedPlantsList
        {
            get
            {
                return selectedAuthorizedPlantsList;
            }

            set
            {
                var bothlistsItemsAreEqual = true;

                if (value == null)
                {
                    selectedAuthorizedPlantsList = null;
                }
                else if (selectedAuthorizedPlantsList == null ||
                    selectedAuthorizedPlantsList.Count != value.Count
                    )
                {
                    bothlistsItemsAreEqual = false;
                }
                else
                {
                    foreach (var item1 in selectedAuthorizedPlantsList)
                    {
                        var item1IdCompany = ((Site)item1).IdSite;
                        var founditem1IdCompanyInValueList = false;
                        foreach (var item2 in value)
                        {
                            var item2IdCompany = ((Site)item2).IdSite;
                            if (item1IdCompany == item2IdCompany)
                            {
                                founditem1IdCompanyInValueList = true;
                            }

                        }
                        if (!founditem1IdCompanyInValueList)
                        {
                            bothlistsItemsAreEqual = false;
                            break;
                        }

                    }

                    if (!bothlistsItemsAreEqual)
                    {
                        foreach (var item1 in value)
                        {
                            var item1IdCompany = ((Site)item1).IdSite;
                            var founditem1IdCompanyInValueList = false;
                            foreach (var item2 in selectedAuthorizedPlantsList)
                            {
                                var item2IdCompany = ((Site)item2).IdSite;
                                if (item1IdCompany == item2IdCompany)
                                {
                                    founditem1IdCompanyInValueList = true;
                                }

                            }
                            if (!founditem1IdCompanyInValueList)
                            {
                                bothlistsItemsAreEqual = false;
                                break;
                            }

                        }
                    }
                }

                if (!bothlistsItemsAreEqual)
                {
                    selectedAuthorizedPlantsList = value;
                    OnPropertyChanged("SelectedAuthorizedPlantsList");
                }

            }
        }


        //[Aishwarya Ingale][4920][22/11/2023]
        public MaximizedElementPosition SetMaximizedElementPosition()
        {

            if (GeosApplication.Instance.UserSettings != null)
            {
                if (GeosApplication.Instance.UserSettings.ContainsKey("ERM_Appearance"))
                {
                    if (string.IsNullOrEmpty(GeosApplication.Instance.UserSettings["ERM_Appearance"].ToString()))
                    {
                        MaximizedElementPosition = MaximizedElementPosition.Right;
                        return MaximizedElementPosition;
                    }
                    else
                    {
                        MaximizedElementPosition = (MaximizedElementPosition)Enum.Parse(typeof(MaximizedElementPosition), GeosApplication.Instance.UserSettings["ERM_Appearance"].ToString(), true);
                        return MaximizedElementPosition;
                    }
                }
                else
                {
                    MaximizedElementPosition = MaximizedElementPosition.Right;
                    return MaximizedElementPosition;
                }
            }
            return MaximizedElementPosition;
        }


        #region[GEOS2-4867][gulab lakade][25 10 2023]
        private Grid grid;
        public Grid Grid
        {
            get { return grid; }
            set
            {
                grid = value;
                OnPropertyChanged("Grid");
            }
        }
        private List<ERM_ProductionTimeline> productionTimeList;
        public List<ERM_ProductionTimeline> ProductionTimeList
        {

            get
            {
                return productionTimeList;
            }

            set
            {
                productionTimeList = value;
                OnPropertyChanged("ProductionTimeList");
            }

        }

        private List<ERM_ProductionTimeline> allProductionTimeList;
        public List<ERM_ProductionTimeline> AllProductionTimeList
        {

            get
            {
                return allProductionTimeList;
            }

            set
            {
                allProductionTimeList = value;
                OnPropertyChanged("AllProductionTimeList");
            }

        }

        private Site selectedPlant;
        public Site SelectedPlant
        {
            get
            {
                return selectedPlant;
            }

            set
            {
                selectedPlant = value;
                OnPropertyChanged("SelectedPlant");
            }
        }

        private ObservableCollection<ERMProductionTimelineSprint> productionTimelineSprint;
        public ObservableCollection<ERMProductionTimelineSprint> ProductionTimelineSprint
        {
            get { return productionTimelineSprint; }
            set
            {
                productionTimelineSprint = value;

                OnPropertyChanged("ProductionTimelineSprint");
            }
        }
        Visibility isEmployeeVisible;
        public Visibility IsEmployeeVisible
        {
            get
            {
                return isEmployeeVisible;
            }

            set
            {
                isEmployeeVisible = value;
                OnPropertyChanged("IsEmployeeVisible");
            }
        }

        private List<ERM_ProductionTimeline> employeeProductionTimeList;
        public List<ERM_ProductionTimeline> EmployeeProductionTimeList
        {

            get
            {
                return employeeProductionTimeList;
            }

            set
            {
                employeeProductionTimeList = value;
                OnPropertyChanged("EmployeeProductionTimeList");
            }

        }
        private List<object> selectedEmployee;
        public List<object> SelectedEmployee
        {

            get
            {
                return selectedEmployee;
            }

            set
            {
                selectedEmployee = value;
                OnPropertyChanged("SelectedEmployee");
            }

        }
        private List<ERM_ProductionTimeline> productionTimeList_Clone;
        public List<ERM_ProductionTimeline> ProductionTimeList_Clone
        {

            get
            {
                return productionTimeList_Clone;
            }

            set
            {
                productionTimeList_Clone = value;
                OnPropertyChanged("ProductionTimeList_Clone");
            }

        }
        private List<ProductionTimeLineToolTipdata> productionToolTip; //[gulab lakade][tool tips][24 01 2024]

        public List<ProductionTimeLineToolTipdata> ProductionToolTip   //[gulab lakade][tool tips][24 01 2024]
        {
            get
            {
                return productionToolTip;
            }

            set
            {
                productionToolTip = value;
                OnPropertyChanged("ProductionToolTip");
            }
        }
        #region [GEOS2-5238] [gulab lakade][29 01 2024]
        ObservableCollection<ProductionTimelineAccordian> productionTimelineWeek;
        //ObservableCollection<TemplateWithCPTypes> templateWithCPTypes;
        public ObservableCollection<ProductionTimelineAccordian> ProductionTimelineWeek
        {
            get { return productionTimelineWeek; }
            set
            {
                productionTimelineWeek = value;
                OnPropertyChanged("ProductionTimelineWeek");
            }
        }
        #region [GEOS2-5238][gulab lakade][31 01 2024]
        Visibility isLeftToggleVisible;
        public Visibility IsLeftToggleVisible
        {
            get
            {
                return isLeftToggleVisible;
            }

            set
            {
                isLeftToggleVisible = value;
                OnPropertyChanged("IsLeftToggleVisible");
            }
        }

        Visibility isLegendVisible;
        public Visibility IsLegendVisible
        {
            get
            {
                return isLegendVisible;
            }

            set
            {
                isLegendVisible = value;
                OnPropertyChanged("IsLegendVisible");
            }
        }

        private List<ProductionTimeReportLegend> productionTimeReportLegendloggedColorList;
        public List<ProductionTimeReportLegend> ProductionTimeReportLegendloggedColorList
        {

            get
            {
                return productionTimeReportLegendloggedColorList;
            }

            set
            {
                productionTimeReportLegendloggedColorList = value;
                OnPropertyChanged("ProductionTimeReportLegendloggedColorList");
            }

        }

        private List<ProductionTimeReportLegend> productionTimeReportLegendAttendanceColorList;
        public List<ProductionTimeReportLegend> ProductionTimeReportLegendAttendanceColorList
        {

            get
            {
                return productionTimeReportLegendAttendanceColorList;
            }

            set
            {
                productionTimeReportLegendAttendanceColorList = value;
                OnPropertyChanged("ProductionTimeReportLegendAttendanceColorList");
            }

        }
        private List<object> selectedProductionTimeStagesList;
        public List<object> SelectedProductionTimeStagesList
        {

            get
            {
                return selectedProductionTimeStagesList;
            }

            set
            {
                selectedProductionTimeStagesList = value;
                OnPropertyChanged("SelectedProductionTimeStagesList");
            }

        }

        private DataTable dataTableForProductionTimeLine;

        public DataTable DataTableForProductionTimeLine
        {
            get
            {
                return dataTableForProductionTimeLine;
            }
            set
            {
                dataTableForProductionTimeLine = value;
                OnPropertyChanged("DataTableForProductionTimeLine");
            }
        }

        private DataTable dataTableForProductionTimeLineGrid;

        public DataTable DataTableForProductionTimeLineGrid
        {
            get
            {
                return dataTableForProductionTimeLineGrid;
            }
            set
            {
                dataTableForProductionTimeLineGrid = value;
                OnPropertyChanged("DataTableForProductionTimeLineGrid");
            }
        }
        private ObservableCollection<BandItem> productionBands = new ObservableCollection<BandItem>();
        public ObservableCollection<BandItem> ProductionBands
        {
            get { return productionBands; }
            set
            {
                productionBands = value;
                OnPropertyChanged("ProductionBands");
            }
        }
        #endregion
        #endregion
        private List<ProductionTimeReportLegend> productionTimeReportWorkingColor;
        public List<ProductionTimeReportLegend> ProductionTimeReportWorkingColor
        {

            get
            {
                return productionTimeReportWorkingColor;
            }

            set
            {
                productionTimeReportWorkingColor = value;
                OnPropertyChanged("ProductionTimeReportWorkingColor");
            }

        }
        private Visibility isRightToggleVisible;
        public Visibility IsRightToggleVisible
        {
            get
            {
                return isRightToggleVisible;
            }

            set
            {
                isRightToggleVisible = value;
                OnPropertyChanged("IsRightToggleVisible");
            }
        }
        //start [GEOS2-5558][gulab lakade]
        private StageByOTItemAndIDDrawing productionStagesList; // [Rupali Sarode][GEOS2-5522][21-03-2024]
        public StageByOTItemAndIDDrawing ProductionStagesList
        {
            get
            {
                return productionStagesList;
            }

            set
            {
                productionStagesList = value;
                OnPropertyChanged("ProductionStagesList");
            }
        }
        private List<int> otItemStagesList; // [Rupali Sarode][GEOS2-5522][21-03-2024]


        public List<int> OtItemStagesList
        {
            get
            {
                return otItemStagesList;
            }

            set
            {
                otItemStagesList = value;
                OnPropertyChanged("OtItemStagesList");
            }
        }
        private List<int> drawingIdStagesList; // [Rupali Sarode][GEOS2-5522][21-03-2024]
        public List<int> DrawingIdStagesList
        {
            get
            {
                return drawingIdStagesList;
            }

            set
            {
                drawingIdStagesList = value;
                OnPropertyChanged("DrawingIdStagesList");
            }
        }


        //end [GEOS2-5558][gulab lakade]

        #region  //Aishwarya Ingale[Geos2-5853]
        private List<ProductionTimeReportLegend> productionTimeReportManagementLegendColorList;
        public List<ProductionTimeReportLegend> ProductionTimeReportManagementLegendColorList
        {

            get
            {
                return productionTimeReportManagementLegendColorList;
            }

            set
            {
                productionTimeReportManagementLegendColorList = value;
                OnPropertyChanged("ProductionTimeReportManagementLegendColorList");
            }

        }

        private List<ProductionTimeReportLegend> productionTimeReportManagementColor;
        public List<ProductionTimeReportLegend> ProductionTimeReportManagementColor
        {

            get
            {
                return productionTimeReportManagementColor;
            }

            set
            {
                productionTimeReportManagementColor = value;
                OnPropertyChanged("ProductionTimeReportManagementColor");
            }

        }
        #endregion

        #region [GEOS2-6738][gulab lakade][02 01 2025]
        public List<ERM_EMP_ProductionTimeline> Emp_Productiontime
        {

            get
            {
                return emp_Productiontime;
            }

            set
            {
                emp_Productiontime = value;
                OnPropertyChanged("Emp_Productiontime");
            }

        }
        public ERM_EMP_ProductionTimeline Select_emp_Productiontime
        {

            get
            {
                return select_emp_Productiontime;
            }

            set
            {
                select_emp_Productiontime = value;
                OnPropertyChanged("Select_emp_Productiontime");
            }

        }
        #endregion
        #endregion

        #region [GEOS2-6965][rani dhamankar][10-03-2025]
        public List<ERMEmployeeLeave> EmployeeLeave
        {
            get
            {
                return employeeLeave;
            }

            set
            {
                employeeLeave = value;
                OnPropertyChanged("EmployeeLeave");
            }
        }
        #endregion

        #region [GEOS2-8698][rani dhamankar][15-07-2025]
        public List<Company> PlantOwnerUsersList
        {
            get { return plantOwnerUsersList; }
            set
            {
                plantOwnerUsersList = value;
                OnPropertyChanged("PlantOwnerUsersList");
            }
        }
        #endregion

        public ERM_WorkOrder_Other_ProductionTimeline WOO_ProductionTimeline
        {

            get
            {
                return wOO_ProductionTimeline;
            }

            set
            {
                wOO_ProductionTimeline = value;
                OnPropertyChanged("WOO_ProductionTimeline");
            }

        }
       
        public Dictionary<Int32, string> MeetingsList = new Dictionary<Int32, string>();
        public Dictionary<Int32, string> MaintenanceList = new Dictionary<Int32, string>();
        public Dictionary<string, BitmapImage> ImagesDictionary = new Dictionary<string, BitmapImage>();
    }
}
