using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
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
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DevExpress.Data;

using DevExpress.Mvvm.UI;

using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;

using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using System.Windows.Media;
using Emdep.Geos.Data.Common.PCM;
using Emdep.Geos.Data.Common.ERM;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.Modules.ERM.Views;
using DevExpress.XtraPrinting;
using DevExpress.Export.Xl;
using System.Globalization;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using DevExpress.Xpf.Grid.TreeList;
using DevExpress.Xpf.Core.Serialization;
using Emdep.Geos.UI.Converters;
using Emdep.Geos.Modules.ERM.CommonClasses;
using Emdep.Geos.Utility;
using System.Windows.Controls;
using System.Runtime.InteropServices.ComTypes;
using DevExpress.Xpf.WindowsUI;
using DevExpress.Xpf.Editors.Flyout;
using DevExpress.XtraEditors.Filtering.Templates;
using DevExpress.Xpf.CodeView;
using DevExpress.Xpf.Printing.PreviewControl.Native;
using DevExpress.Xpf.Editors;
using DevExpress.Data.Helpers;
using DevExpress.Charts.Native;

namespace Emdep.Geos.Modules.ERM.ViewModels
{
    public class CAMCADTimeTrackingViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Service
        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        IPLMService PLMService = new PLMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IPLMService PLMService = new PLMServiceController("localhost:6699");
        //IERMService ERMService = new ERMServiceController("localhost:6699");
        IERMService ERMService = new ERMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        protected ISaveFileDialogService SaveFileDialogService { get { return this.GetService<ISaveFileDialogService>(); } }
        #endregion
        #region Declaration
        TreeListControl treeListControlInstance;
        TableView tableViewInstance;
        public string ERM_TimeTrackinggrid_SettingFilePath = GeosApplication.Instance.UserSettingFolderName + "ERM_CAM_CAD_TimeTrackingGrid_Setting.Xml";


        DataTable dtTimetracking;
        DataTable dtTimetrackingCopy;
        GridControl GridControl1;

        bool isBusy;
        private bool isInit;
        private string myFilterString;

        private ObservableCollection<Emdep.Geos.UI.Helper.Column> columns;
        private ObservableCollection<BandItem> bands = new ObservableCollection<BandItem>();
        private ObservableCollection<ParentBandItem> bands_FirstLevel_Year =
            new ObservableCollection<ParentBandItem>();

        private DataTable dataTableForGridLayout;
        public string ToolTipText = "Real Rework";
        //private Currency currentSelectedCurrency;


        public List<TimeTracking> TimeTrackingList { get; set; }
        public List<Site> AllPlantList { get; set; } //[GEOS2-4173][Rupali Sarode][06-03-2023]
        private ERMCamCadTimeTrackingGetData.CustomObservableCollection<TimeTracking> TimeTrackingList1;
        public List<TimeTracking> AllPlantTimeTrackingList { get; set; }
        public List<TimeTracking> TimeTrackingListCopy { get; set; }
        private bool isColumnChooserVisibleForGrid;
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

        public List<TimeTrackingProductionStage> TimeTrackingProductionStage { get; set; }

        private Visibility isAccordionControlVisible;
        public Visibility IsAccordionControlVisible
        {
            get { return isAccordionControlVisible; }
            set
            {
                isAccordionControlVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAccordionControlVisible"));
            }
        }
        private bool isBandOpen;
        public bool IsBandOpen
        {
            get
            {
                return isBandOpen;
            }
            set
            {
                isBandOpen = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBandOpen"));
            }
        }
        private bool isGridOpen;
        public bool IsGridOpen
        {
            get
            {
                return isGridOpen;
            }
            set
            {
                isGridOpen = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsGridOpen"));
            }
        }

        private List<GeosAppSetting> geosAppSettingList; //[GEOS2-4069][Rupali Sarode][07-12-2022]
        private List<int> appSettingData;      //[GEOS2-4252][Gulabrao lakade][06 03 2023]
        public List<int> AppSettingData
        {
            get
            {
                return appSettingData;
            }
            set
            {
                appSettingData = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AppSettingData"));
            }
        }

        private List<int> otItemStagesList; // [Rupali Sarode][GEOS2-5522][21-03-2024]
        private List<int> drawingIdStagesList; // [Rupali Sarode][GEOS2-5522][21-03-2024]
                                               // private TimeTrackingStageByOTItemAndIDDrawing timetrackingStagesList; // [Rupali Sarode][GEOS2-5522][21-03-2024]
        private StageByOTItemAndIDDrawing timetrackingStagesList; // [Rupali Sarode][GEOS2-5522][21-03-2024]
        private bool FlagCalculateRework;
        private List<ERM_CADCAMTimePerDesignType> cADCAMDesignTypeList;//[GEOS2-5854][gulab lakade][18 07 2024]

        public List<TimeTracking> timeTrackingmismatch { get; set; }//rajashri GEOS2-5988[22-08-2024]

        #region [pallavi jadhav][GEOS2-6081][16 09 2024]
        private List<StandardOperationsDictionaryModules> lstStandardOperationsDictionaryModules;
        private List<StandardOperationsDictionaryDetection> lstStandardOperationsDictionaryDetection;
        private List<StandardOperationsDictionaryOption> lstStandardOperationsDictionaryOption;
        private List<StandardOperationsDictionaryWays> lstStandardOperationsDictionaryWays;
        private ObservableCollection<Stages> getStages;
        private ObservableCollection<WorkOperationByStages> workOperationMenulist;
        private WorkOperationByStages selectedWorkOperationMenulist;
        public TimeTracking TimeTrackingListCP { get; set; }
        #endregion

        public List<ERM_Timetracking_rework_ows> reworkOWSList { get; set; }//[GEOS2-6620][gulab lakade][12 12 2024]
        private bool ischeckplantInSetting;//[GEOS2-6811][gulab lakade][31 12 2024]

        #region [pallavi jadhav][GEOS2-7060][25-03-2025]

        string fromDate;
        string toDate;
        int isButtonStatus;
        Visibility isCalendarVisible;
        DateTime startDate;
        DateTime endDate;
        private Duration _currentDuration;
        private bool isPeriod;
        #endregion

        #endregion // Declaration

        #region Public Properties
        public virtual string ResultFileName { get; protected set; }
        public virtual bool DialogResult { get; protected set; }
        //public Currency CurrentSelectedCurrency
        //{
        //    get
        //    {
        //        return currentSelectedCurrency;
        //    }

        //    set
        //    {
        //        currentSelectedCurrency = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("CurrentSelectedCurrency"));
        //    }
        //}


        public ObservableCollection<Emdep.Geos.UI.Helper.Column> Columns
        {
            get { return columns; }
            set
            {
                columns = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Columns"));
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

        public ObservableCollection<ParentBandItem> Bands_FirstLevel_Year
        {
            get { return bands_FirstLevel_Year; }
            set
            {
                bands_FirstLevel_Year = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Bands_FirstLevel_Year"));
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



        public bool IsInit
        {
            get { return isInit; }
            set { isInit = value; }
        }
        public ObservableCollection<Summary> TotalSummary { get; private set; }

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
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
        private ObservableCollection<Site> plantList;
        public ObservableCollection<Site> PlantList
        {
            get
            {
                return plantList;
            }

            set
            {
                plantList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PlantList"));
            }
        }


        //private object selectedPlant;
        //public virtual object SelectedPlant
        //{
        //    get
        //    {
        //        return selectedPlant;
        //    }

        //    set
        //    {
        //        selectedPlant = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("SelectedPlant"));
        //    }
        //}
        //private ObservableCollection<Site> selectedPlant;
        //public ObservableCollection<Site> SelectedPlant
        //{
        //    get
        //    {
        //        return selectedPlant;
        //    }

        //    set
        //    {
        //        selectedPlant = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("SelectedPlant"));
        //    }
        //}
        private List<object> selectedPlant;
        public List<object> SelectedPlant
        {
            get
            {
                return selectedPlant;
            }

            set
            {
                selectedPlant = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPlant"));
            }
        }
        private List<object> selectedPlantold;
        public List<object> SelectedPlantold
        {
            get
            {
                return selectedPlantold;
            }

            set
            {
                selectedPlantold = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPlantold"));
            }
        }

        //private ObservableCollection<Site> selectedPlant;
        //public ObservableCollection<Site> SelectedPlant
        //{
        //    get
        //    {
        //        return selectedPlant;
        //    }

        //    set
        //    {
        //        selectedPlant = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("SelectedPlant"));
        //    }
        //}
        public DataTable DtTimetracking
        {
            get { return dtTimetracking; }
            set
            {
                dtTimetracking = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DtTimetracking"));
            }
        }
        public DataTable DtTimetrackingCopy
        {
            get { return dtTimetrackingCopy; }
            set
            {
                dtTimetrackingCopy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DtTimetrackingCopy"));
            }
        }
        private object selectedItem;
        public virtual object SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;

                GetRecordbyDeliveryDate();
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedItem"));
            }
        }


        private List<TrackingAccordian> timeTracking;
        public List<TrackingAccordian> TimeTracking
        {
            get
            {
                return timeTracking;
            }

            set
            {
                timeTracking = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TimeTracking"));
            }
        }

        private bool remainingtimecolor;

        public bool Remainingtimecolor
        {
            get { return remainingtimecolor; }
            set
            {
                remainingtimecolor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Remainingtimecolor"));
            }
        }
        private bool plannedDatecolor;

        public bool PlannedDatecolor
        {
            get { return plannedDatecolor; }
            set
            {
                plannedDatecolor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PlannedDatecolor"));
            }
        }

        #region [GEOS2-4069][Rupali Sarode][07-12-2022]
        public List<GeosAppSetting> GeosAppSettingList
        {
            get { return geosAppSettingList; }
            set
            {
                geosAppSettingList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GeosAppSettingList"));
            }
        }
        #endregion

        //[GEOS2-4149][Rupali Sarode][30-01-2023]
        private ObservableCollection<Site> plantListForTrackingData;
        public ObservableCollection<Site> PlantListForTrackingData
        {
            get
            {
                return plantListForTrackingData;
            }

            set
            {
                plantListForTrackingData = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PlantListForTrackingData"));
            }
        }
        #region [GEOS2-4252] [gulab lakade][06 03 2023]

        private TableView view;
        #endregion
        //[GEOS2-5098][gulab lakade][30 11 2023]
        private List<ERMTimetrackingSite> timetrackingProductionList;
        public List<ERMTimetrackingSite> TimetrackingProductionList
        {
            get
            {
                return timetrackingProductionList;
            }

            set
            {
                timetrackingProductionList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TimetrackingProductionList"));
            }
        }
        //[GEOS2-5098][gulab lakade][30 11 2023]

        private bool isColunm;
        public bool IsColunm
        {
            get
            {
                return isColunm;
            }

            set
            {
                isColunm = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsColunm"));
            }
        }

        #region  [Rupali Sarode][GEOS2-5522][21-03-2024]
        public List<int> OtItemStagesList
        {
            get
            {
                return otItemStagesList;
            }

            set
            {
                otItemStagesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OtItemStagesList"));
            }
        }

        public List<int> DrawingIdStagesList
        {
            get
            {
                return drawingIdStagesList;
            }

            set
            {
                drawingIdStagesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DrawingIdStagesList"));
            }
        }

        //public TimeTrackingStageByOTItemAndIDDrawing TimetrackingStagesList
        //{
        //    get
        //    {
        //        return timetrackingStagesList;
        //    }

        //    set
        //    {
        //        timetrackingStagesList = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("TimetrackingStagesList"));
        //    }
        //}
        public StageByOTItemAndIDDrawing TimetrackingStagesList
        {
            get
            {
                return timetrackingStagesList;
            }

            set
            {
                timetrackingStagesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TimetrackingStagesList"));
            }
        }
        #endregion  [Rupali Sarode][GEOS2-5522][21-03-2024]
        //[GEOS2-5854][gulab lakade][18 07 2024]
        public List<ERM_CADCAMTimePerDesignType> CADCAMDesignTypeList
        {
            get
            {
                return cADCAMDesignTypeList;
            }

            set
            {
                cADCAMDesignTypeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CADCAMDesignTypeList"));
            }
        }
        //[GEOS2-5854][gulab lakade][18 07 2024]

        #region [pallavi jadhav][GEOS2-6081][16 09 2024]

        public ObservableCollection<BandItem> BandsCPOperation
        {
            get { return bands; }
            set
            {
                bands = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BandsCPOperation"));
            }
        }
        public List<StandardOperationsDictionaryModules> LstStandardOperationsDictionaryModules
        {
            get { return lstStandardOperationsDictionaryModules; }
            set
            {
                lstStandardOperationsDictionaryModules = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LstStandardOperationsDictionaryModules"));
            }
        }

        public List<StandardOperationsDictionaryDetection> LstStandardOperationsDictionaryDetection
        {
            get { return lstStandardOperationsDictionaryDetection; }
            set
            {
                lstStandardOperationsDictionaryDetection = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LstStandardOperationsDictionaryDetection"));
            }
        }
        public List<StandardOperationsDictionaryOption> LstStandardOperationsDictionaryOption
        {
            get { return lstStandardOperationsDictionaryOption; }
            set
            {
                lstStandardOperationsDictionaryOption = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LstStandardOperationsDictionaryOption"));
            }
        }
        public List<StandardOperationsDictionaryWays> LstStandardOperationsDictionaryWays
        {
            get { return lstStandardOperationsDictionaryWays; }
            set
            {
                lstStandardOperationsDictionaryWays = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LstStandardOperationsDictionaryWays"));
            }
        }

        ObservableCollection<ERMSOPModule> ModuleMenulistupdated = new ObservableCollection<ERMSOPModule>();

        private object selectedItemWay;

        public ObservableCollection<Stages> GetStages
        {
            get
            {
                return getStages;
            }
            set
            {
                getStages = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GetStages"));
            }
        }

        public ObservableCollection<WorkOperationByStages> WorkOperationMenulist
        {
            get
            {
                return workOperationMenulist;
            }

            set
            {
                workOperationMenulist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkOperationMenulist"));
            }
        }

        public WorkOperationByStages SelectedWorkOperationMenulist
        {
            get { return selectedWorkOperationMenulist; }
            set
            {
                selectedWorkOperationMenulist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedWorkOperationMenulist"));
            }

        }

        List<Tuple<string, float?>> allsupplementsBoxMenu;
        public List<Tuple<string, float?>> AllSupplementsBoxMenu
        {
            get { return allsupplementsBoxMenu; }
            set
            {
                allsupplementsBoxMenu = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AllSupplementsBoxMenu"));
            }
        }
        private ObservableCollection<TreeListColumn> columnsWayForSupplements;
        public ObservableCollection<TreeListColumn> ColumnsWayForSupplements
        {
            get { return columnsWayForSupplements; }
            set
            {
                columnsWayForSupplements = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ColumnsWayForSupplements"));
            }
        }
        private ObservableCollection<TreeListColumn> columnsWayOperationTime;
        public ObservableCollection<TreeListColumn> ColumnsWayOperationTime
        {
            get { return columnsWayOperationTime; }
            set
            {
                columnsWayOperationTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ColumnsWayOperationTime"));
            }
        }
        ObservableCollection<WorkOperationByStages> operationWayMenulist = new ObservableCollection<WorkOperationByStages>();
        private List<ERMSOPWays> tempWorkOperationClonedWays = new List<ERMSOPWays>();

        ObservableCollection<ERMSOPWays> waysMenulist;
        public ObservableCollection<ERMSOPWays> WaysMenulist
        {
            get { return waysMenulist; }
            set
            {
                waysMenulist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WaysMenulist"));
            }
        }
        List<StandardOperationsDictionarySupplement> lstStandardOperationsDictionarySupplement;
        public List<StandardOperationsDictionarySupplement> LstStandardOperationsDictionarySupplement
        {
            get { return lstStandardOperationsDictionarySupplement; }
            set
            {
                lstStandardOperationsDictionarySupplement = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LstStandardOperationsDictionarySupplement"));
            }
        }
        #endregion

        //[GEOS2-6620][gulab lakade][12 12 2024]
        public List<ERM_Timetracking_rework_ows> ReworkOWSList
        {
            get
            {
                return reworkOWSList;
            }

            set
            {
                reworkOWSList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ReworkOWSList"));
            }
        }
        ////[GEOS2-6620][gulab lakade][12 12 2024]

        public List<GeosAppSetting> ActivePlantList = new List<GeosAppSetting>();//Aishwarya Ingale[Geos2-6786]

        //[GEOS2-6811][gulab lakade][31 12 2024]
        public bool IscheckplantInSetting
        {
            get
            {
                return ischeckplantInSetting;
            }

            set
            {
                ischeckplantInSetting = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IscheckplantInSetting"));
            }
        }
        //[GEOS2-6811][gulab lakade][31 12 2024]


        #region [pallavi jadhav][GEOS2-7060][25-03-2025]
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

        public bool IsPeriod
        {
            get { return isPeriod; }
            set { isPeriod = value; }
        }

        private List<object> selectedOTNumber;
        public List<object> SelectedOTNumber
        {
            get { return selectedOTNumber; }
            set
            {
                selectedOTNumber = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedOTNumber"));
                // OnPropertyChanged(new PropertyChangedEventArgs("IsSelectedOTNumberNotEmpty"));//Aishwarya Ingale[geos2-5749]
                //  UpdateColor();//Aishwarya Ingale[Geos2-5749]
            }
        }

        private List<object> selectedItems;
        public List<object> SelectedItems
        {
            get { return selectedItems; }
            set
            {
                selectedItems = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedItems"));
                // OnPropertyChanged(new PropertyChangedEventArgs("IsSelectedOTNumberNotEmpty"));//Aishwarya Ingale[geos2-5749]
                // UpdateColor();//Aishwarya Ingale[Geos2-5749]
            }
        }

        public bool IsSelectedOTNumberNotEmpty
        {
            get
            {
                return SelectedOTNumber != null && SelectedOTNumber.Count > 0 && SelectedOTNumber.Count != OTNumberList.Count;
            }

        }

        private List<TimeTracking> oTNumberList;
        public List<TimeTracking> OTNumberList
        {
            get
            {
                return oTNumberList;
            }

            set
            {
                oTNumberList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OTNumberList"));
            }
        }

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

        private List<TimeTracking> timeTrackingList_Cloned;
        public List<TimeTracking> TimeTrackingList_Cloned
        {
            get
            {
                return timeTrackingList_Cloned;
            }
            set
            {
                timeTrackingList_Cloned = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TimeTrackingList_Cloned"));
            }
        }

        CAMCADTimeTrackingView TimeTrackingView = new CAMCADTimeTrackingView();

        #endregion


        private string _enteredText;

        public string EnteredText
        {
            get => _enteredText;
            set
            {
                _enteredText = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EnteredText"));
            }
        }


        private string _oTCode;

        public string OTCode
        {
            get => _oTCode;
            set
            {
                _oTCode = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OTCode"));
            }
        }


        private string _item;

        public string Item
        {
            get => _item;
            set
            {
                _item = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Item"));
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
        public ICommand RefreshTimeTrackingCommand { get; set; }
        public ICommand PrintTimeTrackingCommand { get; set; }
        public ICommand ExportTimeTrackingCommand { get; set; }
        public ICommand ChangePlantCommand { get; set; }
        public ICommand TableViewLoadedCommand { get; set; }
        public ICommand TableViewUnloadedCommand { get; set; }

        public ICommand ItemListTableViewLoadedCommand { get; set; }
        public ICommand CustomSummaryCommand { get; set; }
        public ICommand CommandAccordionControl_Drop { get; set; }
        public ICommand HidePanelCommand { get; set; }
        public ICommand VerifyTheSamplePopupWindowCommand { get; set; } //[pallavi jadhav][26-08-2024][GEOS2-6081]

        #region [pallavi jadhav][GEOS2-7060][25-03-2025]
        public ICommand PeriodCommand { get; set; }
        public ICommand PeriodCustomRangeCommand { get; set; }
        public ICommand ApplyCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand SearchCommand { get; set; }

        //public ICommand ChangeOTNumberCommand { get; set; }
        //public ICommand ChangeItemCommand { get; set; }
        #endregion

        #endregion

        #region Constructor

        public CAMCADTimeTrackingViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor TimeTrackingViewModel()...", category: Category.Info, priority: Priority.Low);
                IsInit = true;
                RefreshTimeTrackingCommand = new RelayCommand(new Action<object>(RefreshTimeTrackingCommandAction));
                PrintTimeTrackingCommand = new RelayCommand(new Action<object>(PrintTimeTrackingCommandAction));
                ExportTimeTrackingCommand = new RelayCommand(new Action<object>(ExportTimeTrackingCommandAction));
                ChangePlantCommand = new DelegateCommand<object>(ChangePlantCommandAction);
                CustomSummaryCommand = new DelegateCommand<object>(CustomSummaryCommandAction);
                TableViewLoadedCommand = new DelegateCommand<RoutedEventArgs>(TableViewLoadedCommandAction);
                TableViewUnloadedCommand = new DelegateCommand<RoutedEventArgs>(TableViewUnloadedCommandAction);
                ItemListTableViewLoadedCommand = new DelegateCommand<object>(ItemListTableViewLoadedAction);
                VerifyTheSamplePopupWindowCommand = new DelegateCommand<object>(VerifyTheSamplePopupWindowCommandAction);//[pallavi jadhav][26-08-2024][GEOS2-6081]
                HidePanelCommand = new RelayCommand(new Action<object>(HidePanel));
                //CommandAccordionControl_Drop = new DelegateCommand<DragEventArgs>(AccordionControl_Drop);

                #region [pallavi jadhav][GEOS2-7060][25-03-2025] 

                PeriodCommand = new DelegateCommand<object>(PeriodCommandAction);
                PeriodCustomRangeCommand = new DelegateCommand<object>(PeriodCustomRangeCommandAction);
                ApplyCommand = new DelegateCommand<object>(ApplyCommandAction);
                CancelCommand = new DelegateCommand<object>(CancelCommandAction);
                SearchCommand = new RelayCommand(new Action<object>(SearchCommandCommandAction));
                // ChangeItemCommand = new RelayCommand(new Action<object>(ChangeItemCommandAction));
                #endregion

                FillListOfColor();  // Called only once for colors  //[GEOS2-4069][Rupali Sarode][07-12-2022]

                GeosApplication.Instance.Logger.Log("Constructor Constructor CAMCADTimeTrackingViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Constructor CAMCADTimeTrackingViewModel()." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
        public void Init()
        {
            GeosApplication.Instance.Logger.Log("Method Init ...", category: Category.Info, priority: Priority.Low);
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                IscheckplantInSetting = false;//[GEOS2-6811][gulab lakade][31 12 2024]
                List<Tuple<string, string>> userConfigurations = new List<Tuple<string, string>>();
                if (GeosApplication.Instance.UserSettings.ContainsKey("ERM_CAM_CAD_TimeTraking_IsFileDeleted_V2640"))
                {
                    if (GeosApplication.Instance.UserSettings["ERM_CAM_CAD_TimeTraking_IsFileDeleted_V2640"].ToString() == "0")
                    {
                        if (File.Exists(@ERM_TimeTrackinggrid_SettingFilePath))
                        {
                            File.Delete(@ERM_TimeTrackinggrid_SettingFilePath);
                            GeosApplication.Instance.UserSettings["ERM_CAM_CAD_TimeTraking_IsFileDeleted_V2640"] = "1";
                            foreach (KeyValuePair<string, string> setting in GeosApplication.Instance.UserSettings)
                            {
                                userConfigurations.Add(new Tuple<string, string>(setting.Key.ToString(), setting.Value.ToString()));
                            }
                            ApplicationOperation.CreateNewSetting(userConfigurations, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                        }
                    }
                }
                //    FloatToTimeSpanConverter temp = new FloatToTimeSpanConverter();

                //ERM_TimeTrackinggrid_SettingFilePath = GeosApplication.Instance.UserSettingFolderName + "ERM_TimeTrackinggrid_Setting.Xml";

                ActivePlantList = WorkbenchStartUp.GetSelectedGeosAppSettings("134");//Aishwarya Ingale[Geos2-6786]

               

                IsColunm = new bool();
                IsColunm = true;
                FailedPlants = new List<string>();
                AllPlantList = new List<Site>();  //[GEOS2-4173][Rupali Sarode][06-03-2023]
                IsGridOpen = true;
                IsBandOpen = false;
                ERMCommon.Instance.PlantVisibleFlag = true;  //[gulab lakade][21 04 2023][Plant dropdown disable]
                                                             //  GetPlants()
                #region [pallavi jadhav][GEOS2-7060][25 - 03 - 2025]



                StartDate = DateTime.Today.Date;
                EndDate = DateTime.Today.Date;
                setDefaultPeriod();
                #endregion

                FillCADCAMDesignTypeList();
                AllPlantList = ERMCommon.Instance.UserAuthorizedPlantsList.ToList();
                FillProductionStage();
                FillDataByPlant();
                AddColumnsToDataTableWithoutBands();
                FillDeliveryweek();
                FlagCalculateRework = false; // If this flag is false then only reworks will be calculated.
                FillDashboard();
                var CurrencyNameFromSetting = String.Empty;

                //TimeTrackingList.AddRange(ERMService.GetAllTimeTracking_V2340());
                if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("ERM_SelectedCurrency"))
                {
                    CurrencyNameFromSetting = GeosApplication.Instance.UserSettings["ERM_SelectedCurrency"];
                }
                if (ERMCommon.Instance.SelectedAuthorizedPlantsList != null)
                {
                    List<Site> plantOwners = ERMCommon.Instance.SelectedAuthorizedPlantsList.Cast<Site>().ToList();
                    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.Name));
                    PreviouslySelectedPlantOwners = string.Join(",", plantOwners.Select(i => i.Name));
                    //TimeTrackingList1 = new CommonClasses.ERMTimeTrackingGetData.CustomObservableCollection<Data.Common.ERM.TimeTracking>();
                    //TimeTrackingList1.AddRange(TimeTrackingList);

                    //[GEOS2-4149][Rupali Sarode][30-01-2023]
                    //ERMTimeTrackingGetData.GetERMTimeTrackingFromServiceAsync(ERMService, PLMService, TimeTrackingList, GridControl1, CurrencyNameFromSetting,
                    //    PreviouslySelectedPlantOwners, DtTimetracking, DataTableForGridLayout, GeosAppSettingList, PlantList.ToList(), TemplateWithTimeTracking, WarningFailedPlants, IsShowFailedPlantWarning);
                    if (WarningFailedPlants == null)
                    {
                        WarningFailedPlants = "";
                    }
                    ////start[GEOS2-5098][gulab lakade][30 11 2023]
                    //ERMTimeTrackingGetData.GetERMTimeTrackingFromServiceAsync(ERMService, PLMService, TimeTrackingList, GridControl1, CurrencyNameFromSetting,
                    //    PreviouslySelectedPlantOwners, DtTimetracking, DataTableForGridLayout, GeosAppSettingList, PlantListForTrackingData.ToList(), TemplateWithTimeTracking, WarningFailedPlants, IsShowFailedPlantWarning, AllPlantList);

                    //ERMTimeTrackingGetData.GetERMTimeTrackingFromServiceAsync(ERMService, PLMService, TimeTrackingList, GridControl1, CurrencyNameFromSetting,
                    //        PreviouslySelectedPlantOwners, DtTimetracking, DataTableForGridLayout, GeosAppSettingList, TimetrackingProductionList.ToList(), TemplateWithTimeTracking, WarningFailedPlants, IsShowFailedPlantWarning, AllPlantList);
                    //ERMTimeTrackingGetData.GetERMTimeTrackingFromServiceAsync(ERMService, PLMService, TimeTrackingList, GridControl1, CurrencyNameFromSetting,
                    //        PreviouslySelectedPlantOwners, DtTimetracking, DataTableForGridLayout, GeosAppSettingList, TimetrackingProductionList.ToList(), TemplateWithTimeTracking, WarningFailedPlants, IsShowFailedPlantWarning, AllPlantList, TimeTrackingProductionStage);//[gulab lakade][11 03 2024][GEOS2-5466]

                    //ERMTimeTrackingGetData.GetERMTimeTrackingFromServiceAsync(ERMService, PLMService, TimeTrackingList, GridControl1, CurrencyNameFromSetting,
                    //                           PreviouslySelectedPlantOwners, DtTimetracking, DataTableForGridLayout, GeosAppSettingList, TimetrackingProductionList.ToList(), TemplateWithTimeTracking, WarningFailedPlants, IsShowFailedPlantWarning, AllPlantList, TimeTrackingProductionStage, TimetrackingStagesList); // [Rupali Sarode][GEOS2-5522][21-03-2024]



                    //ERMTimeTrackingGetData.GetERMTimeTrackingFromServiceAsync(ERMService, PLMService, TimeTrackingList, GridControl1, CurrencyNameFromSetting,
                    //                           PreviouslySelectedPlantOwners, DtTimetracking, DataTableForGridLayout, GeosAppSettingList, TimetrackingProductionList.ToList(), TemplateWithTimeTracking, WarningFailedPlants, IsShowFailedPlantWarning, AllPlantList, TimeTrackingProductionStage, TimetrackingStagesList, CADCAMDesignTypeList); //[GEOS2-5854][gulab lakade][19 07 2024]


                    ERMCamCadTimeTrackingGetData.GetERMCamCadTimeTrackingFromServiceAsync(ERMService, PLMService, TimeTrackingList, GridControl1, CurrencyNameFromSetting,
                                              PreviouslySelectedPlantOwners, DtTimetracking, DataTableForGridLayout, GeosAppSettingList, TimetrackingProductionList.ToList(), TemplateWithTimeTracking, WarningFailedPlants, IsShowFailedPlantWarning, AllPlantList, TimeTrackingProductionStage, TimetrackingStagesList, CADCAMDesignTypeList, AppSettingData, FromDate, ToDate); //  [GEOS2-7094][dhawal bhalerao][05 05 2025]



                    ////end[GEOS2-5098][gulab lakade][30 11 2023]

                }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method Init() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Init()", category: Category.Exception, priority: Priority.Low);
            }

        }

        private void FillProductionStage()
        {
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                GeosApplication.Instance.Logger.Log("Method FillProductionStage ...", category: Category.Info, priority: Priority.Low);
                TimeTrackingProductionStage = new List<TimeTrackingProductionStage>();
                    //ERMService = new ERMServiceController("localhost:6699");
                //TimeTrackingProductionStage.AddRange(ERMService.GetAllTimeTrackingProductioStage_V2320());

                //[Rupali Sarode][GEOS2-4347][05-05-2023]
                // TimeTrackingProductionStage.AddRange(ERMService.GetAllTimeTrackingProductioStage_V2340());
                TimeTrackingProductionStage.AddRange(ERMService.GetAllTimeTrackingProductioStage_V2390());

                // [Rupali Sarode][GEOS2-5522][21-03-2024]

                //TimetrackingStagesList = ERMService.GetAllTimeTrackingProductionStage_V2500();

                TimetrackingStagesList = new StageByOTItemAndIDDrawing();
                TimetrackingStagesList = ERMService.GetAllStagesPerIDOTItemAndIDDrawing_V2500();
                OtItemStagesList = new List<int>();
                DrawingIdStagesList = new List<int>();

                //  TimeTrackingProductionStage.AddRange(TimetrackingStagesList.AllStagesList);
                OtItemStagesList = TimetrackingStagesList.OTITemStagesList.Select(i => i.IdStage).ToList();
                DrawingIdStagesList = TimetrackingStagesList.DrawingIdStagesList.Select(i => i.IdStage).ToList();

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method FillProductionStage() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillProductionStage() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

            }
        }

        public string PreviouslySelectedPlantOwners { get; set; }
        List<string> failedPlants;
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
        private void FillDataByPlant()
        {
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                GeosApplication.Instance.Logger.Log("Method FillDataByPlant ...", category: Category.Info, priority: Priority.Low);
                TimeTrackingList = new List<TimeTracking>();
                OffersOptionsList offersOptionsLst = new OffersOptionsList();
                if (ERMCommon.Instance.SelectedAuthorizedPlantsList != null)
                {
                    List<Site> plantOwners = ERMCommon.Instance.SelectedAuthorizedPlantsList.Cast<Site>().ToList();
                    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.Name));
                    PreviouslySelectedPlantOwners = string.Join(",", plantOwners.Select(i => i.IdSite));
                    if (ERMCommon.Instance.FailedPlants == null)
                    {
                        ERMCommon.Instance.FailedPlants = new List<string>();
                    }

                    foreach (var itemPlantOwnerUsers in plantOwners)
                    {
                        try
                        {

                            GeosApplication.Instance.SplashScreenMessage = "Connecting to " + itemPlantOwnerUsers.Name;
                            //if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                            //==========================================================================================
                            string PlantName = Convert.ToString(itemPlantOwnerUsers.Name);
                            string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.Name == itemPlantOwnerUsers.Name).Select(xy => xy.ServiceProviderUrl).FirstOrDefault();
                            ERMService = new ERMServiceController(serviceurl);
                            //ERMService = new ERMServiceController("localhost:6699");
                            //TimeTrackingList.AddRange(ERMService.GetAllTimeTracking_V2330());

                            #region [GEOS2-4059][Rupali Sarode][05-12-2022]
                            var CurrencyNameFromSetting = String.Empty;

                            //TimeTrackingList.AddRange(ERMService.GetAllTimeTracking_V2340());
                            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("ERM_SelectedCurrency"))
                            {
                                CurrencyNameFromSetting = GeosApplication.Instance.UserSettings["ERM_SelectedCurrency"];
                            }
                            //ERMService = new ERMServiceController("localhost:6699");
                            //TimeTrackingList.AddRange(ERMService.GetAllTimeTracking_V2340(CurrencyNameFromSetting, PlantName, GeosAppSettingList));
                            //[GEOS2-4093][Ruupali Sarode][26-12-2022]
                            //TimeTrackingList.AddRange(ERMService.GetAllTimeTracking_V2350(CurrencyNameFromSetting, PlantName, GeosAppSettingList));

                            #region [GEOS2-4149][Rupali Sarode][30-01-2023]

                            TimeTrackingWithSites timeTrackingSite = new TimeTrackingWithSites();

                            // timeTrackingSite = ERMService.GetAllTimeTracking_V2360(CurrencyNameFromSetting, PlantName, GeosAppSettingList);
                            //timeTrackingSite = ERMService.GetAllTimeTrackings_V2360(CurrencyNameFromSetting, PlantName, GeosAppSettingList);
                            //timeTrackingSite = ERMService.GetAllTimeTrackings_V2370(CurrencyNameFromSetting, PlantName, GeosAppSettingList);
                            //timeTrackingSite = ERMService.GetAllTimeTrackings_V2380(CurrencyNameFromSetting, PlantName, GeosAppSettingList);  //[Origin and Production][gulab lakade][17 04 2023]
                            // timeTrackingSite = ERMService.GetAllTimeTrackings_V2400(CurrencyNameFromSetting, PlantName, GeosAppSettingList); //[gulab lakade][GEOS2-4494-batch][26 05 2023]
                            //  timeTrackingSite = ERMService.GetAllTimeTrackings_V2420(CurrencyNameFromSetting, PlantName, GeosAppSettingList); //[pallavi jadhav][Origin Plant][16 08 2023]
                            //   timeTrackingSite = ERMService.GetAllTimeTrackings_V2430(CurrencyNameFromSetting, PlantName, GeosAppSettingList); //[Pallavi Jadhav][14-09-2023][GEOS2-4818]
                            //timeTrackingSite = ERMService.GetAllTimeTrackings_V2460(CurrencyNameFromSetting, PlantName, GeosAppSettingList); // [Pallavi Jadhav][24-11-2023][GEOS2-5053]
                            // timeTrackingSite = ERMService.GetAllTimeTrackings_V2500(CurrencyNameFromSetting, PlantName, GeosAppSettingList); // [gulab lakade][GEOS2-5466][12 03 2024]
                            //    timeTrackingSite = ERMService.GetAllTimeTrackings_V2540(CurrencyNameFromSetting, PlantName, GeosAppSettingList); // [pallavi jadhav][GEOS2-5907][18 07 2024]

                            //    timeTrackingSite = ERMService.GetAllTimeTrackings_V2550(CurrencyNameFromSetting, PlantName, GeosAppSettingList); // [Aishwarya Ingale][GEOS2-6034][08 08 2024]
                            //timeTrackingSite = ERMService.GetAllTimeTrackings_V2560(CurrencyNameFromSetting, PlantName, GeosAppSettingList); // [pallavi jadhav][GEOS2-6081][16 09 2024]
                            //timeTrackingSite = ERMService.GetAllTimeTrackings_V2580(CurrencyNameFromSetting, PlantName, GeosAppSettingList); // [pallavi jadhav][GEOS2-5320][23 10 2024]
                            //timeTrackingSite = ERMService.GetAllTimeTrackings_V2590(CurrencyNameFromSetting, PlantName, GeosAppSettingList); // [pallavi jadhav][GEOS2-5320][23 10 2024]

                            // timeTrackingSite = ERMService.GetAllTimeTrackings_V2600(CurrencyNameFromSetting, PlantName, GeosAppSettingList); // [GEOS2-6646][pallavi jadhav][04-02-2025]
                            //   timeTrackingSite = ERMService.GetCamCadTimeTrackings_V2640(CurrencyNameFromSetting, PlantName, GeosAppSettingList, DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),
                            //DateTime.ParseExact(ToDate, "dd/MM/yyyy", null)); // [pallavi jadhav][GEOS2-7060][25-03-2025] 
                           // timeTrackingSite = ERMService.GetCamCadTimeTrackings_V2660(CurrencyNameFromSetting, PlantName, GeosAppSettingList, DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),
                          // DateTime.ParseExact(ToDate, "dd/MM/yyyy", null)); // [GEOS2-8868][gulab lakade][10 07 2025] 

                              timeTrackingSite = ERMService.GetCamCadTimeTrackings_V2670(CurrencyNameFromSetting, PlantName, GeosAppSettingList, DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),
                             DateTime.ParseExact(ToDate, "dd/MM/yyyy", null)); // [GEOS2-8309][rani dhamankar][29 08 2025] 


                            var tempSiteID = timeTrackingSite.TimeTrackingList.GroupBy(a => a.ProductionIdSite).ToList();




                            //PlantListForTrackingData = new ObservableCollection<Site>();
                            TimetrackingProductionList = new List<ERMTimetrackingSite>();
                            foreach (var item in tempSiteID)
                            {
                                ERMTimetrackingSite tempSiteList = new ERMTimetrackingSite();

                                var TempSite = timeTrackingSite.TimeTrackingList.Where(x => x.ProductionIdSite == item.Key).FirstOrDefault();
                                if (TempSite != null)
                                {
                                    tempSiteList.ProductionIdSite = Convert.ToInt32(item.Key);
                                    tempSiteList.ProductionSite = TempSite.ProductionPlant;
                                    tempSiteList.OriginalIdSite = TempSite.IdProductionPlant;
                                    tempSiteList.OriginalSite = TempSite.OriginalPlant;
                                    TimetrackingProductionList.Add(tempSiteList);
                                }

                            }
                            //PlantListForTrackingData = new ObservableCollection<Site>();
                            //PlantListForTrackingData.AddRange(timeTrackingSite.siteList);

                            TimeTrackingList.AddRange(timeTrackingSite.TimeTrackingList);
                            TimeTrackingList_Cloned = new List<TimeTracking>(TimeTrackingList.ToList());// [pallavi jadhav][GEOS2-7060][25-03-2025] 
                            #region [GEOS2-4252] [gulab lakade][06 03 2023]
                            try
                            {
                                AppSettingData = new List<int>();
                                AppSettingData = timeTrackingSite.AppSettingData;

                            }
                            catch (Exception ex)
                            { }
                            #endregion
                            #endregion [GEOS2-4149][Rupali Sarode][30-01-2023]



                            #endregion

                        }
                        catch (FaultException<ServiceException> ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed");
                            if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                if (!ERMCommon.Instance.FailedPlants.Contains(itemPlantOwnerUsers.Name))
                                {
                                    ERMCommon.Instance.FailedPlants.Add(itemPlantOwnerUsers.Name);
                                    if (ERMCommon.Instance.FailedPlants != null && FailedPlants.Count > 0)
                                    {
                                        ERMCommon.Instance.IsShowFailedPlantWarning = true;
                                        ERMCommon.Instance.WarningFailedPlants = string.Join(",", ERMCommon.Instance.FailedPlants.Select(x => x.ToString()).ToArray());
                                        ERMCommon.Instance.WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                                    }
                                }

                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed ", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                        }
                        catch (ServiceUnexceptedException ex)
                        {
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

                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }
                        catch (Exception ex)
                        {
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
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }
                    }

                    GeosApplication.Instance.SplashScreenMessage = string.Empty;

                   // FillOTNumber();
                }

                GeosApplication.Instance.Logger.Log("Method FillDataByPlant() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log("Get an error in FillDataByPlant() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log("Get an error in FillDataByPlant() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log("Get an error on FillDataByPlant() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            GeosApplication.Instance.SplashScreenMessage = string.Empty;
        }

        public void FillAllDataByPlant()
        {
            try
            {
                ERMCommon.Instance.FailedPlants = new List<string>();
                ERMCommon.Instance.IsShowFailedPlantWarning = false;
                ERMCommon.Instance.WarningFailedPlants = string.Empty;
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                GeosApplication.Instance.Logger.Log("Method FillAllDataByPlant ...", category: Category.Info, priority: Priority.Low);
                AllPlantTimeTrackingList = new List<TimeTracking>();
                FailedPlants = new List<string>();
                List<object> TempSelectedPlant = new List<object>();
                TempSelectedPlant = new List<object>();

                var CurrencyNameFromSetting = String.Empty;

                //TimeTrackingList.AddRange(ERMService.GetAllTimeTracking_V2340());
                if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("ERM_SelectedCurrency"))
                {
                    CurrencyNameFromSetting = GeosApplication.Instance.UserSettings["ERM_SelectedCurrency"];
                }
                foreach (Site itemPlant in SelectedPlant)
                {
                    int counter = 0;
                    var TempRemainingPlant = PlantList.Where(x => x.IdSite != itemPlant.IdSite).ToList();
                    foreach (Site itemPlantOwnerUsers in TempRemainingPlant)
                    {
                        try
                        {
                            if (counter < 1)
                            {
                                counter++;
                                UInt32 PlantID = Convert.ToUInt32(itemPlant.IdSite);
                                string PlantName = Convert.ToString(itemPlantOwnerUsers.Name);
                                GeosApplication.Instance.SplashScreenMessage = "Connecting to " + itemPlantOwnerUsers.Name;
                                //if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                                //==========================================================================================

                                string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.Name == itemPlantOwnerUsers.Name).Select(xy => xy.ServiceProviderUrl).FirstOrDefault();
                                ERMService = new ERMServiceController(serviceurl);
                                //ERMService = new ERMServiceController("localhost:6699");
                                //ERMService = new ERMServiceController("localhost:6699458");

                                // ERMTimeTrackingGetData.GetERMTimeTrackingFromServiceAsync(ERMService, PLMService, timeTrackingList1, GridControl1, CurrencyNameFromSetting, PlantName);
                                //FillDashboard();
                            }

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

                    GeosApplication.Instance.SplashScreenMessage = string.Empty;

                }

                GeosApplication.Instance.Logger.Log("Method FillAllDataByPlant() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log("Get an error in FillAllDataByPlant() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log("Get an error in FillAllDataByPlant() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log("Get an error on FillAllDataByPlant() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            GeosApplication.Instance.SplashScreenMessage = string.Empty;
        }
        List<Site> TempSelectedOldPlant = new List<Site>();
        private void FillSelectedPlant()
        {
            try
            {
                ERMCommon.Instance.FailedPlants = new List<string>();
                ERMCommon.Instance.IsShowFailedPlantWarning = false;
                ERMCommon.Instance.WarningFailedPlants = string.Empty;
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                GeosApplication.Instance.Logger.Log("Method FillSelectedPlant ...", category: Category.Info, priority: Priority.Low);
                TimeTrackingList = new List<TimeTracking>();
                foreach (Site itemPlantOwnerUsers in SelectedPlant)
                {
                    try
                    {
                        GeosApplication.Instance.SplashScreenMessage = "Connecting to " + itemPlantOwnerUsers.Name;
                        //if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                        //==========================================================================================

                        string PlantName = Convert.ToString(itemPlantOwnerUsers.Name);
                        string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.Name == itemPlantOwnerUsers.Name).Select(xy => xy.ServiceProviderUrl).FirstOrDefault();
                        ERMService = new ERMServiceController(serviceurl);
                        //ERMService = new ERMServiceController("localhost:6699");

                        #region [GEOS2-4059][Rupali Sarode][05-12-2022]
                        var CurrencyNameFromSetting = String.Empty;
                        if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("ERM_SelectedCurrency"))
                        {
                            CurrencyNameFromSetting = GeosApplication.Instance.UserSettings["ERM_SelectedCurrency"];
                        }
                        //[GEOS2-4093][Rupali Sarode][26-12-2022]
                        //ERMService = new ERMServiceController("localhost:6699");
                        #region [GEOS2-4149][Rupali Sarode][30-01-2023]
                        TimeTrackingWithSites timeTrackingSite = new TimeTrackingWithSites();
                        //timeTrackingSite = ERMService.GetAllTimeTrackings_V2360(CurrencyNameFromSetting, PlantName, GeosAppSettingList);
                        //timeTrackingSite = ERMService.GetAllTimeTrackings_V2370(CurrencyNameFromSetting, PlantName, GeosAppSettingList);
                        //timeTrackingSite = ERMService.GetAllTimeTrackings_V2380(CurrencyNameFromSetting, PlantName, GeosAppSettingList);  //[Origin and Production][gulab lakade][17 04 2023]
                        //   timeTrackingSite = ERMService.GetAllTimeTrackings_V2400(CurrencyNameFromSetting, PlantName, GeosAppSettingList);  //[gulab lakade][GEOS2-4494-batch][26 05 2023]
                        //timeTrackingSite = ERMService.GetAllTimeTrackings_V2420(CurrencyNameFromSetting, PlantName, GeosAppSettingList); //[pallavi jadhav][Origin Plant][16 08 2023]
                        //timeTrackingSite = ERMService.GetAllTimeTrackings_V2430(CurrencyNameFromSetting, PlantName, GeosAppSettingList);//[Pallavi Jadhav][14-09-2023][GEOS2-4818]
                        // timeTrackingSite = ERMService.GetAllTimeTrackings_V2460(CurrencyNameFromSetting, PlantName, GeosAppSettingList); // [Pallavi Jadhav][24-11-2023][GEOS2-5053]
                        // timeTrackingSite = ERMService.GetAllTimeTrackings_V2500(CurrencyNameFromSetting, PlantName, GeosAppSettingList); // [gulab lakade][GEOS2-5466][12 03 2024]
                        //timeTrackingSite = ERMService.GetAllTimeTrackings_V2540(CurrencyNameFromSetting, PlantName, GeosAppSettingList); // [pallavi jadhav][GEOS2-5907][18 07 2024]
                        // timeTrackingSite = ERMService.GetAllTimeTrackings_V2550(CurrencyNameFromSetting, PlantName, GeosAppSettingList); // [Aishwarya Ingale][GEOS2-6034][20 08 2024]
                        //timeTrackingSite = ERMService.GetAllTimeTrackings_V2560(CurrencyNameFromSetting, PlantName, GeosAppSettingList); // [pallavi jadhav][GEOS2-6081][16 09 2024]
                        //timeTrackingSite = ERMService.GetAllTimeTrackings_V2580(CurrencyNameFromSetting, PlantName, GeosAppSettingList); // [pallavi jadhav][GEOS2-6081][16 09 2024]
                        //timeTrackingSite = ERMService.GetAllTimeTrackings_V2590(CurrencyNameFromSetting, PlantName, GeosAppSettingList); // [pallavi jadhav][GEOS2-6081][16 09 2024]
                        //  timeTrackingSite = ERMService.GetAllTimeTrackings_V2600(CurrencyNameFromSetting, PlantName, GeosAppSettingList); // [GEOS2-6646][pallavi jadhav][04-02-2025]
                        //timeTrackingSite = ERMService.GetAllTimeTrackings_V2630(CurrencyNameFromSetting, PlantName, GeosAppSettingList, DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),
                        // DateTime.ParseExact(ToDate, "dd/MM/yyyy", null)); // [pallavi jadhav][GEOS2-7060][25-03-2025] 

                        //timeTrackingSite = ERMService.GetCamCadTimeTrackings_V2660(CurrencyNameFromSetting, PlantName, GeosAppSettingList, DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),
                        //   DateTime.ParseExact(ToDate, "dd/MM/yyyy", null)); // [GEOS2-8868][gulab lakade][10 07 2025] 

                        timeTrackingSite = ERMService.GetCamCadTimeTrackings_V2670(CurrencyNameFromSetting, PlantName, GeosAppSettingList, DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),
                          DateTime.ParseExact(ToDate, "dd/MM/yyyy", null)); // [GEOS2-8309][rani dhamankar][29 08 2025] 


                        TimeTrackingList.AddRange(timeTrackingSite.TimeTrackingList);
                        TimeTrackingList_Cloned = new List<TimeTracking>(TimeTrackingList.ToList());// [pallavi jadhav][GEOS2-7060][25-03-2025] 
                        #region [GEOS2-4252] [gulab lakade][06 03 2023]
                        try
                        {
                            AppSettingData = new List<int>();
                            AppSettingData = timeTrackingSite.AppSettingData;

                        }
                        catch (Exception ex)
                        { }
                        #endregion
                        #endregion [GEOS2-4149][Rupali Sarode][30-01-2023]


                        #endregion [GEOS2-4059][Rupali Sarode][05-12-2022]
                    }
                    catch (FaultException<ServiceException> ex)
                    {
                        GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed");
                        if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                            if (!ERMCommon.Instance.FailedPlants.Contains(itemPlantOwnerUsers.Name))
                            {
                                ERMCommon.Instance.FailedPlants.Add(itemPlantOwnerUsers.Name);
                                if (ERMCommon.Instance.FailedPlants != null && ERMCommon.Instance.FailedPlants.Count > 0)
                                {
                                    ERMCommon.Instance.IsShowFailedPlantWarning = true;
                                    ERMCommon.Instance.WarningFailedPlants = string.Join(",", ERMCommon.Instance.FailedPlants.Select(x => x.ToString()).ToArray());
                                    ERMCommon.Instance.WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                                }
                            }
                        // System.Threading.Thread.Sleep(1000);
                        GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed ", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                    }
                    catch (ServiceUnexceptedException ex)
                    {
                        GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed");
                        if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                            if (!ERMCommon.Instance.FailedPlants.Contains(itemPlantOwnerUsers.Name))
                            {
                                ERMCommon.Instance.FailedPlants.Add(itemPlantOwnerUsers.Name);
                                if (ERMCommon.Instance.FailedPlants != null && ERMCommon.Instance.FailedPlants.Count > 0)
                                {
                                    ERMCommon.Instance.IsShowFailedPlantWarning = true;
                                    ERMCommon.Instance.WarningFailedPlants = string.Join(",", ERMCommon.Instance.FailedPlants.Select(x => x.ToString()).ToArray());
                                    ERMCommon.Instance.WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                                }
                            }
                        //System.Threading.Thread.Sleep(1000);
                        GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed");
                        if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                            if (!ERMCommon.Instance.FailedPlants.Contains(itemPlantOwnerUsers.Name))
                            {
                                ERMCommon.Instance.FailedPlants.Add(itemPlantOwnerUsers.Name);

                                if (ERMCommon.Instance.FailedPlants != null && ERMCommon.Instance.FailedPlants.Count > 0)
                                {
                                    ERMCommon.Instance.IsShowFailedPlantWarning = true;
                                    ERMCommon.Instance.WarningFailedPlants = string.Join(",", ERMCommon.Instance.FailedPlants.Select(x => x.ToString()).ToArray());
                                    ERMCommon.Instance.WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                                }
                            }
                        //System.Threading.Thread.Sleep(1000);
                        GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                    }
                }
                #region ////[GEOS2-5098] [gulab lakade][30 11 2023]
                var tempSiteID = TimeTrackingList.GroupBy(a => a.ProductionIdSite).ToList();
                //PlantListForTrackingData = new ObservableCollection<Site>();
                TimetrackingProductionList = new List<ERMTimetrackingSite>();
                foreach (var item in tempSiteID)
                {
                    ERMTimetrackingSite tempSiteList = new ERMTimetrackingSite();

                    var TempSite = TimeTrackingList.Where(x => x.ProductionIdSite == item.Key).FirstOrDefault();
                    if (TempSite != null)
                    {
                        tempSiteList.ProductionIdSite = Convert.ToInt32(item.Key);
                        tempSiteList.ProductionSite = TempSite.ProductionPlant;
                        tempSiteList.OriginalIdSite = TempSite.IdProductionPlant;
                        tempSiteList.OriginalSite = TempSite.OriginalPlant;
                        TimetrackingProductionList.Add(tempSiteList);
                    }

                }
                #endregion

                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                //FillOTNumber();
                FillDashboard();
                if (FailedPlants == null || FailedPlants.Count == 0)
                {
                    IsShowFailedPlantWarning = false;
                    WarningFailedPlants = string.Empty;
                }

                GeosApplication.Instance.Logger.Log("Method FillSelectedPlant() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log("Get an error in FillSelectedPlant() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log("Get an error in FillSelectedPlant() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log("Get an error on FillSelectedPlant() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            GeosApplication.Instance.SplashScreenMessage = string.Empty;
        }
        //private void FillDashboard()
        //{
        //    try
        //    {
        //        if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
        //        GeosApplication.Instance.Logger.Log("Method FillDashboard ...", category: Category.Info, priority: Priority.Low);
        //        int rowCounter = 0;
        //        double? totalsum = null;
        //        double totalsumConvertedAmount = 0;
        //        int bandvalue = 3;
        //        List<string> tempCurrentstage = new List<string>();
        //        var currentculter = CultureInfo.CurrentCulture;
        //        string DecimalSeperator = currentculter.NumberFormat.NumberDecimalSeparator.ToString();
        //        foreach (TimeTracking timeTrackingStage in TimeTrackingList)
        //        {
        //            DataRow dr = DataTableForGridLayout.NewRow();
        //            dr["DeliveryWeek"] = Convert.ToString(timeTrackingStage.DeliveryWeek);
        //            dr["DeliveryDate"] = Convert.ToString(timeTrackingStage.DeliveryDate);
        //            if (timeTrackingStage.POType != null)
        //                dr["POType"] = Convert.ToString(timeTrackingStage.POType);
        //            if (timeTrackingStage.Customer != null)
        //                dr["Customer"] = Convert.ToString(timeTrackingStage.Customer);
        //            if (timeTrackingStage.Project != null)
        //                dr["Project"] = Convert.ToString(timeTrackingStage.Project);
        //            if (timeTrackingStage.Offer != null)
        //                dr["Offer"] = Convert.ToString(timeTrackingStage.Offer);
        //            if (timeTrackingStage.OTCode != null)
        //                dr["OTCode"] = Convert.ToString(timeTrackingStage.OTCode);

        //            dr["OriginPlant"] = Convert.ToString(timeTrackingStage.OriginalPlant);
        //            if (timeTrackingStage.ProductionPlant != null)TemplateWithTimeTracking
        //                dr["ProductionPlant"] = Convert.ToString(timeTrackingStage.ProductionPlant);
        //            if (timeTrackingStage.Reference != null)
        //                dr["Reference"] = Convert.ToString(timeTrackingStage.Reference);
        //            if (timeTrackingStage.Template != null)
        //                dr["ReferenceTemplate"] = Convert.ToString(timeTrackingStage.Template);
        //            if (timeTrackingStage.Type != null)
        //                dr["Type"] = Convert.ToString(timeTrackingStage.Type);
        //            if (timeTrackingStage.QTY != null)
        //                dr["QTY"] = timeTrackingStage.QTY;
        //            if (timeTrackingStage.Unit != null)
        //                dr["UnitPrice"] = timeTrackingStage.Unit;
        //            if (timeTrackingStage.SerialNumber != null)
        //                dr["SerialNumber"] = Convert.ToString(timeTrackingStage.SerialNumber);
        //            if (timeTrackingStage.ItemStatus != null)
        //                dr["ItemStatus"] = Convert.ToString(timeTrackingStage.ItemStatus);
        //            if (timeTrackingStage.CurrentWorkStation != null)
        //            {
        //                dr["CurrentWorkStation"] = Convert.ToString(timeTrackingStage.CurrentWorkStation);
        //            }
        //            if (timeTrackingStage.QTY != null && timeTrackingStage.Unit != null)
        //                timeTrackingStage.TotalSalePrice = timeTrackingStage.QTY * timeTrackingStage.Unit;
        //            if (timeTrackingStage.TotalSalePrice != null)
        //                dr["Total"] = timeTrackingStage.TotalSalePrice;
        //            if (timeTrackingStage.TRework != null)
        //                dr["TRework"] = timeTrackingStage.TRework;
        //            //float? TempExpectedTime=0;
        //            TimeSpan TempTotalReal = TimeSpan.Parse("0");
        //            TimeSpan TempTotalExpected = TimeSpan.Parse("0");
        //            TimeSpan TempTotalRemaianing = TimeSpan.Parse("0");
        //            //double TempTotalReal = 0;

        //            //double TempTotalExpected = 0;
        //            //double TempTotalReamianing = 0;
        //            foreach (TimeTrackingCurrentStage item1 in timeTrackingStage.TimeTrackingStage)
        //            {
        //                string real = "Real_" + Convert.ToString(item1.IdStage);
        //                TimeSpan Tempreal = TimeSpan.Parse("0");
        //                if (!string.IsNullOrEmpty(Convert.ToString(item1.Real)) && Convert.ToString(item1.Real) != "0")
        //                {
        //                    Tempreal = ConvertfloattoTimespan(Convert.ToString(item1.Real));
        //                }
        //                dr[real] = Tempreal;
        //                //if (Tempreal.Hours == 0)
        //                //{
        //                //    dr[real] = Tempreal.ToString(@"mm\:ss");
        //                //}
        //                //else
        //                //{
        //                //    dr[real] = Tempreal;
        //                //}
        //                //dr[real] = Convert.ToString(item1.Real);
        //                //if (!string.IsNullOrEmpty(Convert.ToString(item1.Real)))
        //                //{
        //                TempTotalReal += Tempreal;// Convert.ToDouble(item1.Real);
        //                //}
        //                string expected = "Expected_" + Convert.ToString(item1.IdStage);
        //                TimeSpan Tempexpected = TimeSpan.Parse("0");
        //                if (!string.IsNullOrEmpty(Convert.ToString(item1.Expected)) && Convert.ToString(item1.Expected) != "0")
        //                {
        //                    Tempexpected = ConvertfloattoTimespan(Convert.ToString(item1.Expected));
        //                }
        //                if (Tempexpected.Hours == 0)
        //                {
        //                    dr[expected] = Tempexpected.ToString(@"mm\:ss");
        //                }
        //                else
        //                {
        //                    dr[expected] = Tempexpected;
        //                }
        //                //dr[expected] = Convert.ToString(item1.Expected);
        //                //if (!string.IsNullOrEmpty(Convert.ToString(item1.Expected)))
        //                //{
        //                TempTotalExpected += Tempexpected;// Convert.ToDouble(item1.Expected);
        //                //}
        //                string remaining = "Remaining_" + Convert.ToString(item1.IdStage);
        //                //double TempExpected = Convert.ToDouble(item1.Expected);
        //                //if (TempExpected == null)
        //                //{
        //                //    TempExpected = 0;
        //                //}
        //                //double TempReal = Convert.ToDouble(item1.Real);
        //                //if (TempReal == null)
        //                //{
        //                //    TempReal = 0;
        //                //}
        //                if (Convert.ToDouble(item1.Real) <= Convert.ToDouble(item1.Expected))
        //                {
        //                    Remainingtimecolor = true;
        //                    timeTrackingStage.Tempcolor = false;
        //                    dr["Tempcolor"] = timeTrackingStage.Tempcolor;
        //                    if ((Tempexpected - Tempreal).Hours == 0)
        //                    {
        //                        dr[remaining] = (Tempexpected - Tempreal).ToString(@"mm\:ss"); //ConvertfloattoTimespan(Convert.ToString((TempExpected - TempReal))).ToString(@"mm\:ss");
        //                    }
        //                    else
        //                    {
        //                        dr[remaining] = (Tempexpected - Tempreal);// ConvertfloattoTimespan(Convert.ToString((TempExpected - TempReal)));

        //                    }
        //                    TempTotalRemaianing += (Tempexpected - Tempreal);
        //                    //dr[remaining] = Convert.ToString((TempExpected - TempReal));
        //                    // Convert.ToDouble(TempExpected - TempReal);

        //                }
        //                else
        //                {
        //                    timeTrackingStage.Tempcolor = true;
        //                    dr["Tempcolor"] = timeTrackingStage.Tempcolor;
        //                    //if(ConvertfloattoTimespan(Convert.ToString((TempReal - TempExpected))).Hours==0)
        //                    //{
        //                    //    dr[remaining] = ConvertfloattoTimespan(Convert.ToString((TempReal - TempExpected))).ToString(@"mm\:ss");
        //                    //}
        //                    //else
        //                    //{
        //                    //    dr[remaining] = ConvertfloattoTimespan(Convert.ToString((TempReal - TempExpected)));
        //                    //}
        //                    dr[remaining] = (Tempreal - Tempexpected);
        //                    TempTotalRemaianing += (Tempreal - Tempexpected);
        //                }
        //            }

        //            if (TempTotalReal != null)
        //            {
        //                if (TempTotalReal.Hours == 0)
        //                {
        //                    dr["Real"] = TempTotalReal.ToString(@"mm\:ss");
        //                }
        //                else
        //                {
        //                    dr["Real"] = TempTotalReal;
        //                }
        //                // dr["Real"] = Convert.ToString(TempTotalReal);
        //            }
        //            if (TempTotalExpected != null)
        //            {
        //                if (TempTotalExpected.Hours == 0)
        //                {
        //                    dr["Expected"] = TempTotalExpected.ToString(@"mm\:ss");
        //                }
        //                else
        //                {
        //                    dr["Expected"] = TempTotalExpected;
        //                }
        //                // dr["Expected"] = Convert.ToString(TempTotalExpected);
        //            }
        //            if (TempTotalRemaianing != null)
        //            {
        //                if (TempTotalRemaianing.Hours == 0)
        //                {
        //                    dr["Remaining"] = TempTotalRemaianing.ToString(@"mm\:ss");
        //                }
        //                else
        //                {
        //                    dr["Remaining"] = TempTotalRemaianing;
        //                }
        //                //dr["Remaining"] = Convert.ToString(TempTotalReamianing);
        //            }

        //            DataTableForGridLayout.Rows.Add(dr);
        //            rowCounter += 1;

        //        }
        //        DtTimetracking = DataTableForGridLayout;

        //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //    }
        //    catch (Exception ex) { }
        //}

        private void FillDashboard()
        {
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                GeosApplication.Instance.Logger.Log("Method FillDashboard ...", category: Category.Info, priority: Priority.Low);
                int rowCounter = 0;
                double? totalsum = null;
                double totalsumConvertedAmount = 0;
                int bandvalue = 3;
                List<string> tempCurrentstage = new List<string>();
                var currentculter = CultureInfo.CurrentCulture;
                string DecimalSeperator = currentculter.NumberFormat.NumberDecimalSeparator.ToString();
                DataTableForGridLayout.Clear();
                DtTimetracking = null;
                //TimeTrackingList = TimeTrackingList.Where(a=>a.OTCode== "2024-24369H OT 1").OrderBy(a => a.DeliveryDate).ToList();
                TimeTrackingList = TimeTrackingList.OrderBy(a => a.DeliveryDate).ToList();
               // TimeTrackingList = TimeTrackingList.OrderBy(a => a.DeliveryDate).ToList();

                List<ReworkData> OTsWithIDOTItemList = new List<ReworkData>();
                List<ReworkData> OTsWithIDDrawingList = new List<ReworkData>();
                //List<int> App_Stage = new List<int>();
                //App_Stage = new List<int>();
                //try
                //{
                //    GeosAppSetting Setting = CrmStartUp.GetGeosAppSettings(96);
                //    if (!string.IsNullOrEmpty(Setting.DefaultValue))
                //    {
                //        List<string> tempApp_Stage = Convert.ToString(Setting.DefaultValue).Split(',').ToList<string>();

                //        foreach (var timeTrackingStage in tempApp_Stage)
                //        {
                //            App_Stage.Add(Convert.ToInt32(timeTrackingStage));
                //        }
                //    }
                //}
                //catch (Exception ex) { }
                for (int i = 0; i < TimeTrackingList.Count; i++)
                {

                    if (Convert.ToString(TimeTrackingList[i].SerialNumber) == "E240439P001001")
                    {

                    }
                    DataRow dr = DataTableForGridLayout.NewRow();
                    ReworkOWSList = new List<ERM_Timetracking_rework_ows>();////[GEOS2-6620][gulab lakade][12 12 2024]
                    //if (Convert.ToDateTime(TimeTrackingList[i].DeliveryDate) < DateTime.Now)
                    //{
                    //    dr["DeliveryDateColor"] = true;
                    //}
                    long idcounterpart = TimeTrackingList[i].IdCounterpart;
                    dr["DeliveryWeek"] = Convert.ToString(TimeTrackingList[i].DeliveryWeek);
                    dr["DeliveryDate"] = Convert.ToString(TimeTrackingList[i].DeliveryDate);
                    if (!DataTableForGridLayout.Columns.Contains("IdCounterpart"))
                        DataTableForGridLayout.Columns.Add("IdCounterpart", typeof(Int64));
                    dr["IdCounterpart"] = Convert.ToInt64(TimeTrackingList[i].IdCounterpart);
                    //dr["DeliveryDateHtmlColor"] = Convert.ToString(TimeTrackingList[i].DeliveryDateHtmlColor);
                    // PlannedDatecolor = false;

                    if (TimeTrackingList[i].PlannedDeliveryDate != null) //[GEOS2-4217] [Pallavi Jadhav] [27 02 2023]
                    {
                        //PlannedDatecolor = true;

                        dr["PlannedDeliveryDate"] = Convert.ToDateTime(TimeTrackingList[i].PlannedDeliveryDate);
                        dr["PlannedDeliveryDateHtmlColor"] = Convert.ToString(TimeTrackingList[i].PlannedDeliveryDateHtmlColor);
                    }


                    #region [GEOS2-4093][Rupali Sarode][26-12-2022]

                    if (TimeTrackingList[i].QuoteSendDate != null)
                        dr["QuoteSendDate"] = Convert.ToString(TimeTrackingList[i].QuoteSendDate);
                    // dr["QuoteSendDate"] = TimeTrackingList[i].QuoteSendDate.ToString("dd-MM-YYYY");
                    if (TimeTrackingList[i].GoAheadDate != null)
                        dr["GoAheadDate"] = Convert.ToString(TimeTrackingList[i].GoAheadDate);
                    if (TimeTrackingList[i].PODate != null)
                        dr["PODate"] = Convert.ToString(TimeTrackingList[i].PODate);

                    //#region [GEOS2-4145][Pallavi Jadhav][02-03-2023]
                    if (TimeTrackingList[i].Samples != null && TimeTrackingList[i].Samples != "")
                    {
                        dr["SamplesTemplate"] = Convert.ToString(TimeTrackingList[i].Samples);
                        string samples = Convert.ToString(TimeTrackingList[i].Samples);
                        switch (samples)
                        {
                            case "E":
                                dr["SamplesTooltip"] = Convert.ToString("E-EMDEP");
                                break;
                            case "C":
                                dr["SamplesTooltip"] = Convert.ToString("C-CUSTOMER");
                                break;
                            case "H":
                                dr["SamplesTooltip"] = Convert.ToString("H-HARNESS");
                                break;
                            case "M":
                                dr["SamplesTooltip"] = Convert.ToString("M-MISSING");
                                break;
                            case "W":
                                dr["SamplesTooltip"] = Convert.ToString("W-WIRES");
                                break;

                            default:
                                break;
                        }


                        if (TimeTrackingList[i].SamplesDate != null)
                            dr["SamplesDateTemplate"] = Convert.ToString(TimeTrackingList[i].SamplesDate);
                        string samplesColor = Convert.ToString(TimeTrackingList[i].SamplesColor);
                        switch (samplesColor)
                        {
                            case "True":
                                dr["SamplesColor"] = Convert.ToString("#008000");
                                break;
                            case "False":
                                dr["SamplesColor"] = Convert.ToString("#FF0000");
                                break;
                            default:
                                break;
                        }

                    }


                    //if (TimeTrackingList[i].SamplesDate != null)
                    //    dr["SamplesDateTemplate"] = Convert.ToString(TimeTrackingList[i].SamplesDate);


                    //#endregion
                    if (TimeTrackingList[i].AvailbleForDesignDate != null)
                        dr["AvailbleForDesignDate"] = Convert.ToString(TimeTrackingList[i].AvailbleForDesignDate);

                    #endregion

                    dr["DeliveryDateHtmlColor"] = Convert.ToString(TimeTrackingList[i].DeliveryDateHtmlColor);
                    if (TimeTrackingList[i].POType != null)
                        dr["POType"] = Convert.ToString(TimeTrackingList[i].POType);
                    if (TimeTrackingList[i].Customer != null)
                        dr["Customer"] = Convert.ToString(TimeTrackingList[i].Customer);
                    if (TimeTrackingList[i].Project != null)
                        dr["Project"] = Convert.ToString(TimeTrackingList[i].Project);
                    if (TimeTrackingList[i].Offer != null)
                        dr["Offer"] = Convert.ToString(TimeTrackingList[i].Offer);
                    if (TimeTrackingList[i].OTCode != null)
                        dr["OTCode"] = Convert.ToString(TimeTrackingList[i].OTCode);

                    dr["OriginPlant"] = Convert.ToString(TimeTrackingList[i].OriginalPlant);
                    if (TimeTrackingList[i].ProductionPlant != null)
                        dr["ProductionPlant"] = Convert.ToString(TimeTrackingList[i].ProductionPlant);
                    if (TimeTrackingList[i].Reference != null)
                        dr["Reference"] = Convert.ToString(TimeTrackingList[i].Reference);
                    if (TimeTrackingList[i].Template != null)
                        dr["ReferenceTemplate"] = Convert.ToString(TimeTrackingList[i].Template);
                    if (TimeTrackingList[i].Type != null)
                        dr["Type"] = Convert.ToString(TimeTrackingList[i].Type);
                    if (TimeTrackingList[i].QTY != null)
                        dr["QTY"] = TimeTrackingList[i].QTY;
                    //if (TimeTrackingList[i].QTY != null && TimeTrackingList[i].Unit != null)
                    //    TimeTrackingList[i].TotalSalePrice =(float?) Math.Round(Convert.ToDecimal(TimeTrackingList[i].QTY) * Convert.ToDecimal(TimeTrackingList[i].Unit),2);
                    //double tempTotalPrice = 

                    if (TimeTrackingList[i].Unit != null)
                        dr["UnitPrice"] = Convert.ToDecimal(Convert.ToDouble(TimeTrackingList[i].Unit).ToString("N", CultureInfo.CurrentCulture));

                    //[Rupali Sarode][04-04-2024][GEOS2-5577]
                    if (TimeTrackingList[i].IdDrawing > 0)
                        dr["IdDrawing"] = Convert.ToString(TimeTrackingList[i].IdDrawing);

                    //[Aishwarya Ingale][09-08-2024][GEOS2-6034]
                    if (TimeTrackingList[i].Workbookdrawing != null)
                        dr["workbook_drawing"] = Convert.ToString(TimeTrackingList[i].Workbookdrawing);

                    if (TimeTrackingList[i].SerialNumber != null)
                        dr["SerialNumber"] = Convert.ToString(TimeTrackingList[i].SerialNumber);
                    if (TimeTrackingList[i].ItemStatus != null)
                        dr["ItemStatus"] = Convert.ToString(TimeTrackingList[i].ItemStatus);

                    #region [Gulab Lakade][geso2-4173][02-03 -2023]
                    if (TimeTrackingList[i].DrawingType != null)
                    {
                        dr["DrawingType"] = Convert.ToString(TimeTrackingList[i].DrawingType);
                        string DrawingTypecolor = Convert.ToString(TimeTrackingList[i].DrawingType);
                        switch (DrawingTypecolor)
                        {
                            case "M":
                                dr["DrawingTypeForColor"] = Convert.ToString("#A020F0");
                                break;
                            case "U":
                                dr["DrawingTypeForColor"] = Convert.ToString("#00FF00");
                                break;
                            case "N":
                                dr["DrawingTypeForColor"] = Convert.ToString("#FF0000");
                                break;
                            case "C":
                                dr["DrawingTypeForColor"] = Convert.ToString("#0000FF");
                                break;
                            case "X":
                                dr["DrawingTypeForColor"] = Convert.ToString("#000000");
                                break;
                            default:
                                break;
                        }
                        //  dr["DrawingType"] = Convert.ToString(TimeTrackingList[i].DrawingType);
                        string DrawingTypetooltip = Convert.ToString(TimeTrackingList[i].DrawingType);
                        switch (DrawingTypetooltip)
                        {
                            case "M":
                                dr["DrawingTypeTooltip"] = Convert.ToString("M-Modification");
                                break;
                            case "U":
                                dr["DrawingTypeTooltip"] = Convert.ToString("U-Update");
                                break;
                            case "N":
                                dr["DrawingTypeTooltip"] = Convert.ToString("N-New");
                                break;
                            case "C":
                                dr["DrawingTypeTooltip"] = Convert.ToString("C-Copy");
                                break;
                            case "X":
                                dr["DrawingTypeTooltip"] = Convert.ToString("X–Drawing assigned by the commercial team");
                                break;
                            default:
                                break;
                        }
                    }
                    if (TimeTrackingList[i].TrayName != null)
                        dr["TrayName"] = Convert.ToString(TimeTrackingList[i].TrayName);
                    if (TimeTrackingList[i].TrayColor != null)
                        dr["TrayColor"] = Convert.ToString(TimeTrackingList[i].TrayColor);
                    #endregion

                    #region [Rupali Sarode][geso2-4173][10-03-2023]
                    if (TimeTrackingList[i].FirstDeliveryDate != null)
                        dr["FirstDeliveryDate"] = Convert.ToString(TimeTrackingList[i].FirstDeliveryDate);
                    #endregion


                    if (TimeTrackingList[i].HtmlColor != null)
                        dr["HtmlColor"] = Convert.ToString(TimeTrackingList[i].HtmlColor);
                    if (TimeTrackingList[i].NumItem != null)
                    {
                        dr["ItemNumber"] = Convert.ToString(TimeTrackingList[i].NumItem);
                    }

                    if (TimeTrackingList[i].CurrentWorkStation != null)
                    {
                        dr["CurrentWorkStation"] = Convert.ToString(TimeTrackingList[i].CurrentWorkStation);
                    }

                    if (TimeTrackingList[i].DesignSystem != null)
                    {
                        dr["DesignSystem"] = Convert.ToString(TimeTrackingList[i].DesignSystem);
                    }

                    var OperatorNames = string.Join(",", TimeTrackingList[i].TimeTrackingAddingPostServer.Where(a => a.TimeTrackIdCounterpart == TimeTrackingList[i].IdCounterpart).Select(x => x.OperatorName.Trim()).Distinct(StringComparer.OrdinalIgnoreCase).ToList());
                    string[] names = OperatorNames.Split(',').Select(name => name.Trim()).Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
                    dr["Designer"] = string.Join(", ", names);

                    dr["StartRev"] = Convert.ToString(TimeTrackingList[i].StartRevision);

                    dr["LastRev"] = Convert.ToString(TimeTrackingList[i].LastRevision);

                    if (TimeTrackingList[i].TotalSalePrice != null)
                        dr["TotalPrice"] = Convert.ToDecimal(Convert.ToDouble(TimeTrackingList[i].TotalSalePrice).ToString("N2", CultureInfo.CurrentCulture));// Math.Round(Convert.ToDouble(TimeTrackingList[i].TotalSalePrice),2).ToString("N", CultureInfo.CurrentCulture);
                                                                                                                                                              //dr["Total"] = Convert.ToDouble(tempTotalPrice).ToString("N", CultureInfo.CurrentCulture);



                    #region [Rupali Sarode][GEOS2-5522][21-03-2024] -- Rework As per New algorithm

                    //if (TimeTrackingList[i].TRework != null)
                    //{
                    //    dr["TRework"] = TimeTrackingList[i].TRework;
                    //}

                    List<TimeTrackingCurrentStage> StageReworksList = new List<TimeTrackingCurrentStage>();

                    try
                    {
                        dr["TRework"] = TimeTrackingList[i].TRework;
                    }
                    catch (Exception ex)
                    {

                    }
                    if (FlagCalculateRework == false)

                    {
                        if (TimeTrackingList[i].IsBatch == false && TimeTrackingList[i].TRework > 0)
                        {
                            if (TimeTrackingList[i].IdOTItem != 0)
                            {
                                var TempIdOTItem = OTsWithIDOTItemList.Where(j => j.IdOT == TimeTrackingList[i].IdOt && j.IdOTItem == TimeTrackingList[i].IdOTItem).FirstOrDefault();

                                if (TempIdOTItem == null)
                                {
                                    ReworkData TempReworkOTItem = new ReworkData();

                                    TempReworkOTItem.IdOT = TimeTrackingList[i].IdOt;
                                    TempReworkOTItem.IdOTItem = TimeTrackingList[i].IdOTItem;
                                    TempReworkOTItem.IdCounterpart = TimeTrackingList[i].IdCounterpart;
                                    OTsWithIDOTItemList.Add(TempReworkOTItem);
                                    //FlagNewAlgorithmCOM = false;
                                }
                                else
                                {
                                    //FlagNewAlgorithmCOM = true;

                                    // Update rework for COM stage in Time tracking Lists to display in Time tracking grid 
                                    if (TimeTrackingList[i].TimeTrackingStage != null)
                                    {
                                        StageReworksList = TimeTrackingList[i].TimeTrackingStage.Where(j => OtItemStagesList.Contains(j.NewIdStage) && j.Rework == 1).ToList();

                                        if (StageReworksList.Count > 0)  // Update rework only if rework is related to specific stage.
                                        {
                                            if (TimeTrackingList[i].TRework > 0)
                                            {
                                                TimeTrackingList[i].TRework = TimeTrackingList[i].TRework - (ulong?)StageReworksList.Count;
                                                dr["TRework"] = TimeTrackingList[i].TRework;
                                            }

                                        }
                                    }
                                }
                            }

                            if (TimeTrackingList[i].IdDrawing != 0 && TimeTrackingList[i].IdWorkbookOfCpProducts != 0) //Aishwarya Ingale[Geos2-6034]
                            {
                                var TempIdDrawing = OTsWithIDDrawingList.Where(j => j.IdOT == TimeTrackingList[i].IdOt && j.IdDrawing == TimeTrackingList[i].IdDrawing && j.IdWorkbookOfCpProducts == TimeTrackingList[i].IdWorkbookOfCpProducts).FirstOrDefault();
                                if (TempIdDrawing == null)
                                {
                                    ReworkData TempReworkDrawing = new ReworkData();

                                    TempReworkDrawing.IdOT = TimeTrackingList[i].IdOt;
                                    TempReworkDrawing.IdOTItem = TimeTrackingList[i].IdOTItem;
                                    TempReworkDrawing.IdCounterpart = TimeTrackingList[i].IdCounterpart;
                                    TempReworkDrawing.IdDrawing = TimeTrackingList[i].IdDrawing;
                                    TempReworkDrawing.IdWorkbookOfCpProducts = TimeTrackingList[i].IdWorkbookOfCpProducts; //Aishwarya Ingale[Geos2-6034]
                                    OTsWithIDDrawingList.Add(TempReworkDrawing);
                                }
                                else
                                {
                                    // Update rework for CAD & CAM stage in ProductionOutPutReport & AllPlantWeeklyReworksMail Lists to display in mail 

                                    StageReworksList = new List<TimeTrackingCurrentStage>();
                                    if (TimeTrackingList[i].TimeTrackingStage != null)
                                    {
                                        StageReworksList = TimeTrackingList[i].TimeTrackingStage.Where(j => DrawingIdStagesList.Contains(j.NewIdStage) && j.Rework == 1).ToList();

                                        if (StageReworksList.Count > 0)  // Update rework only if rework is related to specific stage.
                                        {
                                            TimeTrackingList[i].TRework = TimeTrackingList[i].TRework - (ulong?)StageReworksList.Count;
                                            dr["TRework"] = TimeTrackingList[i].TRework;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    #endregion [Rupali Sarode][GEOS2-5522][21-03-2024] 




                    TimeSpan TempTotalReal = TimeSpan.Parse("0");
                    TimeSpan TempTotalExpected = TimeSpan.Parse("0");
                    TimeSpan TempTotalRemaianing = TimeSpan.Parse("0");
                    if (TimeTrackingList[i].TimeTrackingStage != null)
                    {
                        //if(TimeTrackingList[i].SerialNumber== "E240439P001001")
                        //{

                        //}
                        #region [gulab lakade][11 03 2024][GEOS2-5466]
                        //if (dr["SerialNumber"].ToString() == "2403001P011002")// R210008H006001   2309708P001001  2403001P002001  2324401P060001  2403001P004001 2403001P011002 2309708P001001
                        //{

                        //}
                        //if (dr["SerialNumber"].ToString() == "2403001P011001")// R210008H006001   2309708P001001  2403001P002001  2324401P060001  2403001P004001 2403001P011002
                        //{

                        //}
                        ////if (dr["SerialNumber"].ToString() == "E240336H003002")// R210008H006001   2309708P001001  2403001P002001  2324401P060001  2403001P004001 2403001P011002
                        ////{

                        ////}
                        //if (dr["SerialNumber"].ToString() == "2309708P001001")// R210008H006001   2309708P001001  2403001P002001  2324401P060001  2403001P004001 2403001P011002 
                        //{

                        //}
                        //if (dr["SerialNumber"].ToString() == "2309708P011002")// R210008H006001   2309708P001001  2403001P002001  2324401P060001  2403001P004001 2403001P011002 
                        //{

                        //}
                        //if (dr["OTCode"].ToString() == "IE240336H OT 1")// R210008H006001   2309708P001001  2403001P002001  2324401P060001  2403001P004001 2403001P011002 
                        //{

                        //}
                        //if (dr["SerialNumber"].ToString() == "2309708P012002")// R210008H006001   2309708P001001  2403001P002001  2324401P060001  2403001P004001 2403001P011002
                        //{
                        //}
                        try
                        {
                            List<TimeTrackingCurrentStage> TempTimeTrackingByStageList = new List<TimeTrackingCurrentStage>();

                            var tempTimeTrackingStages = TimeTrackingList[i].TimeTrackingStage.OrderBy(a => a.IdCounterparttracking).ToList();


                            var timeTrackingStages = tempTimeTrackingStages
                                .OrderBy(r => r.Startdate)
                                .ThenBy(r => r.Enddate)
                                .ToList();
                            #region [GEOS2-6620][gulab lakade][05 12 2024]
                            int? checkIdstage_first = 0;
                            string Production = string.Empty;

                            //string Rework = string.Empty;

                            string Rework_First = string.Empty;
                            string Rework_Second = string.Empty;
                            string Rework_Third = string.Empty;
                            string Rework_Fourth = string.Empty;

                            string POWS_First = string.Empty;
                            string POWS_Second = string.Empty;
                            string POWS_Third = string.Empty;
                            string POWS_Fourth = string.Empty;


                            string ROWS_First = string.Empty;
                            string ROWS_Second = string.Empty;
                            string ROWS_Third = string.Empty;
                            string ROWS_Fourth = string.Empty;


                            TimeSpan ProductionTime = new TimeSpan();

                            TimeSpan ReworkTime = new TimeSpan();
                            double ReworkTimeIndouble = 0;
                            TimeSpan TotalProductiontime = new TimeSpan();
                            TimeSpan TotalReworktime = new TimeSpan();


                            int? DetectedIdStage_first = 0;

                            string Rework = string.Empty;
                            ////rajashri start GEOS2-6054
                            //double ProductionOWStime = 0;
                            string POWS = string.Empty;
                            string ROWS = string.Empty;
                            TimeSpan TotalProductionOwstime = new TimeSpan();
                            TimeSpan TotalReworkOwstime = new TimeSpan();
                            TimeSpan OWSprodandReworkTime = new TimeSpan();
                            //TimeSpan tempstoreOWSvalues = new TimeSpan();
                            #endregion

                            #region [rani dhamankar] [23 - 05 - 2025][GEOS2 - 8131]

                            TimeSpan tmptimespanvalue = new TimeSpan();
                            TimeSpan tmpAdding_time = new TimeSpan();
                            TimeSpan tmpPostServer_time = new TimeSpan();
                            TimeSpan tmpEDS_time = new TimeSpan();
                            TimeSpan tmpDownload_time = new TimeSpan();
                            TimeSpan tmpTransferred_time = new TimeSpan();

                            #endregion

                            List<TimeTrackingCurrentStage> ReworkTimeStageList = new List<TimeTrackingCurrentStage>();

                            //end

                            #region [pallavi jadhav][GEOS2-7066][10-04-2025]
                            TimeSpan TempAddInPostServer = TimeSpan.Parse("0");
                            TimeSpan AddintotalTimeDifference = TimeSpan.Zero;
                            TimeSpan PostservertotalTimeDifference = TimeSpan.Zero;
                            #endregion

                            #region [dhawal bhalerao][24-04-2025]
                            TimeSpan EDStotalTimeDifference = TimeSpan.Zero;                            
                            TimeSpan ProductionTimeDifference = TimeSpan.Zero;
                            #endregion

                            foreach (var timeTrackingStage in timeTrackingStages)
                            {
                                #region [GEOS2-6620][gulab lakade][12 12 2024]
                                if (ReworkOWSList == null)
                                {
                                    ReworkOWSList = new List<ERM_Timetracking_rework_ows>();
                                }
                                #endregion

                                bool FlagPresentInActivePlants = false;
                                var timeTrackingProdStage = TimeTrackingProductionStage.Where(x => x.IdStage == timeTrackingStage.IdStage).FirstOrDefault();
                                if (timeTrackingProdStage != null)
                                {
                                    //TimeTrackingCurrentStage TempTimeTrackingByStage = new TimeTrackingCurrentStage();
                                    #region [Rupali Sarode][GEOS2-4347][05-05-2023] -- Show stage only if present in ActiveInPlants

                                    string[] ArrActiveInPlants;

                                    if (timeTrackingProdStage.ActiveInPlants != null && timeTrackingProdStage.ActiveInPlants != "")
                                    {
                                        ArrActiveInPlants = timeTrackingProdStage.ActiveInPlants.Split(',');
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
                                    #endregion
                                    if (FlagPresentInActivePlants == true || timeTrackingProdStage.ActiveInPlants == null || timeTrackingProdStage.ActiveInPlants == "")
                                    {
                                        if (TempTimeTrackingByStageList.Count() > 0)
                                        {
                                            var Real = TempTimeTrackingByStageList.Where(x => x.IdStage == timeTrackingStage.IdStage).FirstOrDefault();
                                            decimal? RealTime = 0;
                                            decimal? ExpectedTime = 0;
                                            if (Real != null)
                                            {
                                                if (Real.Real != null)
                                                {
                                                    RealTime = Real.Real;
                                                }
                                                if (Real.Expected == null)
                                                {
                                                    if (timeTrackingStage.Expected == null || timeTrackingStage.Expected == 0)
                                                    {
                                                        TempTimeTrackingByStageList.Where(x => x.IdStage == timeTrackingStage.IdStage).ToList().ForEach(a => a.Expected = 0);
                                                    }
                                                    else
                                                    {
                                                        TempTimeTrackingByStageList.Where(x => x.IdStage == timeTrackingStage.IdStage).ToList().ForEach(a => a.Expected = timeTrackingStage.Expected);
                                                    }

                                                }
                                                else
                                                {
                                                    TempTimeTrackingByStageList.Where(x => x.IdStage == timeTrackingStage.IdStage).ToList().ForEach(a => a.Expected = Real.Expected);
                                                }

                                            }
                                            else
                                            {
                                                TimeTrackingCurrentStage TempTimeTrackingByStage = new TimeTrackingCurrentStage();
                                                //var tempexpexcted = tempTimeTrackingStages.Where(X => X.IdStage == timeTrackingStage.IdStage && X.Expected!=null).FirstOrDefault();

                                                if (timeTrackingStage.Expected != null)
                                                {
                                                    TempTimeTrackingByStage.Expected = timeTrackingStage.Expected;
                                                }
                                                else
                                                {
                                                    TempTimeTrackingByStage.Expected = 0;
                                                }
                                                TempTimeTrackingByStage.IdStage = timeTrackingStage.IdStage;
                                                TempTimeTrackingByStage.Real = 0;// Convert.ToDecimal(timeTrackingStage.TimeDifference);
                                                TempTimeTrackingByStage.PlannedDeliveryDateByStage = timeTrackingStage.PlannedDeliveryDateByStage;
                                                TempTimeTrackingByStage.PlannedDeliveryDateHtmlColor = timeTrackingStage.PlannedDeliveryDateHtmlColor;
                                                TempTimeTrackingByStage.Days = timeTrackingStage.Days;
                                                TempTimeTrackingByStage.ProductionActivityTimeType = timeTrackingStage.ProductionActivityTimeType;
                                                TempTimeTrackingByStage.TimeTrackDifference = timeTrackingStage.TimeTrackDifference;

                                                TempTimeTrackingByStageList.Add(TempTimeTrackingByStage);
                                            }
                                        }
                                        else
                                        {
                                            TimeTrackingCurrentStage TempTimeTrackingByStage = new TimeTrackingCurrentStage();
                                            // var tempexpexcted = tempTimeTrackingStages.Where(X => X.IdStage == timeTrackingStage.IdStage).FirstOrDefault();

                                            if (timeTrackingStage.Expected != null)
                                            {
                                                TempTimeTrackingByStage.Expected = timeTrackingStage.Expected;
                                            }
                                            else
                                            {
                                                TempTimeTrackingByStage.Expected = 0;
                                            }
                                            TempTimeTrackingByStage.IdStage = timeTrackingStage.IdStage;
                                            TempTimeTrackingByStage.Real = 0;// Convert.ToDecimal(timeTrackingStage.TimeDifference);
                                            TempTimeTrackingByStage.PlannedDeliveryDateByStage = timeTrackingStage.PlannedDeliveryDateByStage;
                                            TempTimeTrackingByStage.PlannedDeliveryDateHtmlColor = timeTrackingStage.PlannedDeliveryDateHtmlColor;
                                            TempTimeTrackingByStage.Days = timeTrackingStage.Days;
                                            TempTimeTrackingByStage.ProductionActivityTimeType = timeTrackingStage.ProductionActivityTimeType;
                                            TempTimeTrackingByStage.TimeTrackDifference = timeTrackingStage.TimeTrackDifference;

                                            TempTimeTrackingByStageList.Add(TempTimeTrackingByStage);

                                        }
                                        #region First Rework is true
                                        //if (reworkflag_First == true)
                                       
                                        if (ReworkOWSList.Count() > 0)
                                        {
                                            #region check reworkflag_Second 
                                            //if (reworkflag_Second == false)
                                            if (ReworkOWSList.Count() == 1)
                                            {
                                                var tempR_P_OWS = ReworkOWSList.Where(a => a.OWS_ID == 1).FirstOrDefault();
                                                if (tempR_P_OWS != null)
                                                {
                                                    checkIdstage_first = tempR_P_OWS.CheckStageId;
                                                    DetectedIdStage_first = tempR_P_OWS.DetectedStageId;
                                                }
                                                #region reworkflag_First data check and fill only
                                                #region checkIdstage_first == item.IdStage
                                                if (checkIdstage_first == timeTrackingStage.IdStage)
                                                {
                                                    try
                                                    {
                                                        #region reworkflag_First completed
                                                        string production_first = "Production_" + checkIdstage_first;
                                                        if (!DataTableForGridLayout.Columns.Contains(Production))
                                                        {
                                                            dr[production_first] = TimeSpan.Zero;
                                                        }
                                                        if (DataTableForGridLayout.Columns.Contains(production_first))
                                                        {
                                                            if (Convert.ToString(dr[production_first]) != null)
                                                            {
                                                                string reworkowsInstring = Convert.ToString(dr["Production_" + checkIdstage_first]);
                                                                if (string.IsNullOrEmpty(Convert.ToString(reworkowsInstring)) || reworkowsInstring == "0" || reworkowsInstring == "00:00:00")
                                                                {
                                                                    reworkowsInstring = "0.00:00:00";
                                                                }
                                                                string timeString = Convert.ToString(reworkowsInstring); // Example time format string
                                                                string[] format = { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" }; // Specify the format of the time string
                                                                TimeSpan TempproductionTime = TimeSpan.ParseExact(timeString, format, null);

                                                                if (DataTableForGridLayout.Columns.Contains(production_first))
                                                                {
                                                                    #region[GEOS2-7880][gulab lakade][16 04 2025]
                                                                    //dr[production_first] = (TempproductionTime + TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference)));

                                                                    //TotalProductiontime = TotalProductiontime + TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference));
                                                                    if (timeTrackingStage.IdStage == 2 && TimeTrackingList[i].DesignSystem == "EDS")
                                                                    {
                                                                        #region [GEOS2-7094][dhawal bhalerao][05-05-2025]                                                                        
                                                                        var tempAddingPosServer = TimeTrackingList[i].TimeTrackingAddingPostServer.Where(x => x.TimeTrackIdCounterparttracking == timeTrackingStage.IdCounterparttracking && (x.ProductionActivityTimeType == 0 || x.ProductionActivityTimeType == 2228 || x.ProductionActivityTimeType == 2229 || x.ProductionActivityTimeType == 2230 || x.ProductionActivityTimeType == 1920 || x.ProductionActivityTimeType == 1921)).ToList();

                                                                        if (tempAddingPosServer.Count() > 0)
                                                                        {
                                                                            //TimeSpan tmptimespanvalue = new TimeSpan();
                                                                            //TimeSpan tmpAdding_time = new TimeSpan();
                                                                            //TimeSpan tmpPostServer_time = new TimeSpan();
                                                                            //TimeSpan tmpEDS_time = new TimeSpan();
                                                                            //TimeSpan tmpDownload_time = new TimeSpan();
                                                                            //TimeSpan tmpTransferred_time = new TimeSpan();
                                                                            foreach (var ttime in tempAddingPosServer)
                                                                            {
                                                                                if (ttime.ProductionActivityTimeType == 0)
                                                                                {
                                                                                    tmptimespanvalue = tmptimespanvalue + ttime.TimeTrackDifference;
                                                                                }
                                                                                else if(ttime.ProductionActivityTimeType == 2228)
                                                                                {
                                                                                    tmptimespanvalue = tmptimespanvalue + ttime.TimeTrackDifference;
                                                                                    tmpAdding_time = tmpAdding_time + ttime.TimeTrackDifference;
                                                                                    dr["Addin_" + Convert.ToString(timeTrackingStage.IdStage)] = tmpAdding_time;
                                                                                }
                                                                                else if (ttime.ProductionActivityTimeType == 2229)
                                                                                {
                                                                                    tmptimespanvalue = tmptimespanvalue + ttime.TimeTrackDifference;
                                                                                    tmpPostServer_time = tmpPostServer_time + ttime.TimeTrackDifference;
                                                                                    dr["PostServer_" + Convert.ToString(timeTrackingStage.IdStage)] = tmpPostServer_time;
                                                                                }
                                                                                else if (ttime.ProductionActivityTimeType == 2230)
                                                                                {
                                                                                    tmpEDS_time = tmpEDS_time + ttime.TimeTrackDifference;
                                                                                    dr["EDS_" + Convert.ToString(timeTrackingStage.IdStage)] = tmpEDS_time;
                                                                                }
                                                                                else if (ttime.ProductionActivityTimeType == 1920)
                                                                                {
                                                                                    tmpDownload_time = tmpDownload_time + ttime.TimeTrackDifference;
                                                                                    dr["Download_" + Convert.ToString(timeTrackingStage.IdStage)] = tmpDownload_time;
                                                                                }
                                                                                else if (ttime.ProductionActivityTimeType == 1921)
                                                                                {
                                                                                    tmpTransferred_time = tmpTransferred_time + ttime.TimeTrackDifference;
                                                                                    dr["Transferred_" + Convert.ToString(timeTrackingStage.IdStage)] = tmpTransferred_time;
                                                                                }
                                                                            }
                                                                            dr[production_first] = TempproductionTime + tmptimespanvalue;
                                                                            TotalProductiontime = TotalProductiontime + tmptimespanvalue;
                                                                        }

                                                                        #endregion
                                                                    }
                                                                    else if (timeTrackingStage.IdStage == 2 && TimeTrackingList[i].DesignSystem == "GSM")
                                                                    {
                                                                        #region [GEOS2-7094][dhawal bhalerao][15-05-2025]                                                                        
                                                                        var tempAddingPosServer = TimeTrackingList[i].TimeTrackingAddingPostServer.Where(x => x.TimeTrackIdCounterparttracking == timeTrackingStage.IdCounterparttracking && (x.ProductionActivityTimeType == 0 || x.ProductionActivityTimeType == 1920 || x.ProductionActivityTimeType == 1921)).ToList();

                                                                        if (tempAddingPosServer.Count() > 0)
                                                                        {
                                                                            //TimeSpan tmptimespanvalue = new TimeSpan();                                                                            
                                                                            //TimeSpan tmpDownload_time = new TimeSpan();
                                                                            //TimeSpan tmpTransferred_time = new TimeSpan();
                                                                            foreach (var ttime in tempAddingPosServer)
                                                                            {
                                                                                if (ttime.ProductionActivityTimeType == 0)
                                                                                {
                                                                                    tmptimespanvalue = tmptimespanvalue + ttime.TimeTrackDifference;
                                                                                }                                                                                
                                                                                else if (ttime.ProductionActivityTimeType == 1920)
                                                                                {
                                                                                    tmpDownload_time = tmpDownload_time + ttime.TimeTrackDifference;
                                                                                    dr["Download_" + Convert.ToString(timeTrackingStage.IdStage)] = tmpDownload_time;
                                                                                }
                                                                                else if (ttime.ProductionActivityTimeType == 1921)
                                                                                {
                                                                                    tmpTransferred_time = tmpTransferred_time + ttime.TimeTrackDifference;
                                                                                    dr["Transferred_" + Convert.ToString(timeTrackingStage.IdStage)] = tmpTransferred_time;
                                                                                }
                                                                            }
                                                                            dr[production_first] = TempproductionTime + tmptimespanvalue;
                                                                            TotalProductiontime = TotalProductiontime + tmptimespanvalue;
                                                                        }

                                                                        #endregion
                                                                    }
                                                                    else
                                                                    {
                                                                        dr[production_first] = (TempproductionTime + TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference)));
                                                                        TotalProductiontime = TotalProductiontime + TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference));
                                                                    }
                                                                    #endregion 
                                                                }

                                                            }
                                                            else
                                                            {
                                                                if (DataTableForGridLayout.Columns.Contains(production_first))
                                                                {
                                                                    dr[production_first] = TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference));

                                                                    TotalProductiontime = TotalProductiontime + TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference));
                                                                    #region[GEOS2-7880][gulab lakade][16 04 2025]
                                                                    //dr[production_first] = (TempproductionTime + TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference)));

                                                                    //TotalProductiontime = TotalProductiontime + TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference));
                                                                    if (timeTrackingStage.IdStage == 2 && TimeTrackingList[i].DesignSystem == "EDS")
                                                                    {
                                                                        #region [GEOS2-7094][dhawal bhalerao][05-05-2025] 
                                                                        
                                                                        var tempAddingPosServer = TimeTrackingList[i].TimeTrackingAddingPostServer.Where(x => x.TimeTrackIdCounterparttracking == timeTrackingStage.IdCounterparttracking && (x.ProductionActivityTimeType == 0 || x.ProductionActivityTimeType == 2228 || x.ProductionActivityTimeType == 2229 || x.ProductionActivityTimeType == 2230 || x.ProductionActivityTimeType == 1920 || x.ProductionActivityTimeType == 1921)).ToList();

                                                                        if (tempAddingPosServer.Count() > 0)
                                                                        {
                                                                            //TimeSpan tmptimespanvalue = new TimeSpan();
                                                                            //TimeSpan tmpAdding_time = new TimeSpan();
                                                                            //TimeSpan tmpPostServer_time = new TimeSpan();
                                                                            //TimeSpan tmpEDS_time = new TimeSpan();
                                                                            //TimeSpan tmpDownload_time = new TimeSpan();
                                                                            //TimeSpan tmpTransferred_time = new TimeSpan();
                                                                            foreach (var ttime in tempAddingPosServer)
                                                                            {
                                                                                if (ttime.ProductionActivityTimeType == 0)
                                                                                {
                                                                                    tmptimespanvalue = tmptimespanvalue + ttime.TimeTrackDifference;
                                                                                }
                                                                                else if(ttime.ProductionActivityTimeType == 2228)
                                                                                {
                                                                                    tmptimespanvalue = tmptimespanvalue + ttime.TimeTrackDifference;
                                                                                }
                                                                                else if (ttime.ProductionActivityTimeType == 2228)
                                                                                {
                                                                                    tmptimespanvalue = tmptimespanvalue + ttime.TimeTrackDifference;
                                                                                    tmpAdding_time = tmpAdding_time + ttime.TimeTrackDifference;
                                                                                    dr["Addin_" + Convert.ToString(timeTrackingStage.IdStage)] = tmpAdding_time;
                                                                                }
                                                                                else if (ttime.ProductionActivityTimeType == 2229)
                                                                                {
                                                                                    tmptimespanvalue = tmptimespanvalue + ttime.TimeTrackDifference;
                                                                                    tmpPostServer_time = tmpPostServer_time + ttime.TimeTrackDifference;
                                                                                    dr["PostServer_" + Convert.ToString(timeTrackingStage.IdStage)] = tmpPostServer_time;
                                                                                }
                                                                                else if (ttime.ProductionActivityTimeType == 2230)
                                                                                {
                                                                                    tmpEDS_time = tmpEDS_time + ttime.TimeTrackDifference;
                                                                                    dr["EDS_" + Convert.ToString(timeTrackingStage.IdStage)] = tmpEDS_time;
                                                                                }
                                                                                else if (ttime.ProductionActivityTimeType == 1920)
                                                                                {
                                                                                    tmpDownload_time = tmpDownload_time + ttime.TimeTrackDifference;
                                                                                    dr["Download_" + Convert.ToString(timeTrackingStage.IdStage)] = tmpDownload_time;
                                                                                }
                                                                                else if (ttime.ProductionActivityTimeType == 1921)
                                                                                {
                                                                                    tmpTransferred_time = tmpTransferred_time + ttime.TimeTrackDifference;
                                                                                    dr["Transferred_" + Convert.ToString(timeTrackingStage.IdStage)] = tmpTransferred_time;
                                                                                }
                                                                            }
                                                                            dr[production_first] = tmptimespanvalue;
                                                                            TotalProductiontime = TotalProductiontime + tmptimespanvalue;
                                                                        }

                                                                        #endregion
                                                                    }
                                                                    else if (timeTrackingStage.IdStage == 2 && TimeTrackingList[i].DesignSystem == "GSM")
                                                                    {
                                                                        #region [GEOS2-7094][dhawal bhalerao][15-05-2025] 

                                                                        var tempAddingPosServer = TimeTrackingList[i].TimeTrackingAddingPostServer.Where(x => x.TimeTrackIdCounterparttracking == timeTrackingStage.IdCounterparttracking && (x.ProductionActivityTimeType == 0 || x.ProductionActivityTimeType == 1920 || x.ProductionActivityTimeType == 1921)).ToList();

                                                                        if (tempAddingPosServer.Count() > 0)
                                                                        {
                                                                            //TimeSpan tmptimespanvalue = new TimeSpan();                                                                            
                                                                            //TimeSpan tmpDownload_time = new TimeSpan();
                                                                            //TimeSpan tmpTransferred_time = new TimeSpan();
                                                                            foreach (var ttime in tempAddingPosServer)
                                                                            {
                                                                                if (ttime.ProductionActivityTimeType == 0)
                                                                                {
                                                                                    tmptimespanvalue = tmptimespanvalue + ttime.TimeTrackDifference;
                                                                                }                                                                                
                                                                                else if (ttime.ProductionActivityTimeType == 1920)
                                                                                {
                                                                                    tmpDownload_time = tmpDownload_time + ttime.TimeTrackDifference;
                                                                                    dr["Download_" + Convert.ToString(timeTrackingStage.IdStage)] = tmpDownload_time;
                                                                                }
                                                                                else if (ttime.ProductionActivityTimeType == 1921)
                                                                                {
                                                                                    tmpTransferred_time = tmpTransferred_time + ttime.TimeTrackDifference;
                                                                                    dr["Transferred_" + Convert.ToString(timeTrackingStage.IdStage)] = tmpTransferred_time;
                                                                                }
                                                                            }
                                                                            dr[production_first] = tmptimespanvalue;
                                                                            TotalProductiontime = TotalProductiontime + tmptimespanvalue;
                                                                        }

                                                                        #endregion
                                                                    }
                                                                    else
                                                                    {
                                                                        dr[production_first] = TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference));
                                                                        TotalProductiontime = TotalProductiontime + TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference));
                                                                    }
                                                                    #endregion 

                                                                }

                                                            }
                                                        }
                                                                                                                                                                        

                                                        // reworkflag_First = false;
                                                        checkIdstage_first = 0;
                                                        ReworkTimeIndouble = 0;
                                                        DetectedIdStage_first = 0;
                                                        //ProductionOWStime_First = 0;//GEOS2-6054
                                                        // reworkflag_First = false;
                                                        // checkIdstage_first = 0;
                                                        ReworkOWSList.RemoveAll(a => a.OWS_ID == 1);
                                                        #endregion

                                                        #region reworkflag_First again true
                                                        // gulab lakade 19 04 2024
                                                        if (timeTrackingStage.Rework == 1)
                                                        {
                                                            ERM_Timetracking_rework_ows selected_rework_ows = new ERM_Timetracking_rework_ows();
                                                            selected_rework_ows.OWS_ID = 1;
                                                            selected_rework_ows.CheckStageId = Convert.ToInt32(timeTrackingStage.IdStage);
                                                            //reworkflag_First = true;
                                                            checkIdstage_first = timeTrackingStage.IdStage;
                                                            var DetectedStageIndex = timeTrackingStages.IndexOf(timeTrackingStage);
                                                            int lastindex = timeTrackingStages.Count();
                                                            if (lastindex - 1 > DetectedStageIndex)
                                                            {
                                                                var Detectedrecord = timeTrackingStages[DetectedStageIndex + 1];
                                                                if (Detectedrecord != null)
                                                                {
                                                                    DetectedIdStage_first = Detectedrecord.IdStage;// detected rework stage
                                                                    selected_rework_ows.DetectedStageId = Convert.ToInt32(Detectedrecord.IdStage);
                                                                }
                                                                TimeTrackingCurrentStage TempReworkTime = new TimeTrackingCurrentStage();
                                                                TempReworkTime.IdStage = Detectedrecord.IdStage;
                                                                TempReworkTime.IdCounterparttracking = Detectedrecord.IdCounterparttracking;
                                                                TempReworkTime.TimeDifference = Detectedrecord.TimeDifference;
                                                                ReworkTimeStageList.Add(TempReworkTime);

                                                            }

                                                            if (checkIdstage_first == DetectedIdStage_first)
                                                            {
                                                                // reworkflag_First = false;
                                                                checkIdstage_first = 0;
                                                                DetectedIdStage_first = 0;
                                                            }
                                                            else
                                                            {
                                                                ReworkOWSList.Add(selected_rework_ows);
                                                            }

                                                        }
                                                        #endregion
                                                        //end gulab lakade 19 04 2024
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        GeosApplication.Instance.Logger.Log(string.Format("Error in method FillDashboard() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                                                    }
                                                }
                                                else
                                                {
                                                    try
                                                    {


                                                        #region only Add reworkflag_First data not other  
                                                        #region  if (item.Rework == 1) DetectedIdStage_Second
                                                        #region if(checkIdstage_Second==DetectedIdStage_Second)
                                                        Int32 Max_owsID = ReworkOWSList.Max(a => a.OWS_ID);
                                                        ERM_Timetracking_rework_ows selected_rework_ows = new ERM_Timetracking_rework_ows();
                                                        selected_rework_ows.OWS_ID = Max_owsID + 1;
                                                        bool sameCheck_Detect = false;
                                                        if (timeTrackingStage.Rework == 1)
                                                        {
                                                            var DetectedStageIndex = timeTrackingStages.IndexOf(timeTrackingStage);
                                                            if ((timeTrackingStages.Count() - 1) > DetectedStageIndex)
                                                            {
                                                                var Detectedrecord = timeTrackingStages[DetectedStageIndex + 1];
                                                                if (Detectedrecord != null)
                                                                {
                                                                    //DetectedIdStage_Second = Detectedrecord.IdStage;
                                                                    selected_rework_ows.DetectedStageId = Convert.ToInt32(Detectedrecord.IdStage);
                                                                }
                                                            }
                                                            selected_rework_ows.CheckStageId = Convert.ToInt32(timeTrackingStage.IdStage);

                                                            // checkIdstage_Second = timeTrackingStage.IdStage;
                                                            if (selected_rework_ows.CheckStageId == selected_rework_ows.DetectedStageId)
                                                            {
                                                                sameCheck_Detect = true;
                                                                //checkIdstage_Second = 0;
                                                                //DetectedIdStage_Second = 0;
                                                                //reworkflag_Second = false;
                                                            }
                                                            else
                                                            {
                                                                ReworkOWSList.Add(selected_rework_ows);

                                                            }

                                                        }

                                                        #endregion

                                                        if (timeTrackingStage.Rework == 1 && sameCheck_Detect == false)
                                                        {
                                                            //var DetectedStageIndex = timeTrackingStages.IndexOf(timeTrackingStage);
                                                            //if ((timeTrackingStages.Count() - 1) > DetectedStageIndex)
                                                            //{
                                                            //    var Detectedrecord = timeTrackingStages[DetectedStageIndex + 1];
                                                            //    if (Detectedrecord != null)
                                                            //    {
                                                            //        DetectedIdStage_Second = Detectedrecord.IdStage;
                                                            //    }
                                                            //   // reworkflag_Second = true;
                                                            //    checkIdstage_Second = timeTrackingStage.IdStage;
                                                            Int32 Max_owsID_Add = ReworkOWSList.Max(a => a.OWS_ID);
                                                            Int32 Max_owsID_Add_minus = Max_owsID_Add - 1;
                                                            var R_P_OWSMax = ReworkOWSList.Where(a => a.OWS_ID == Max_owsID_Add).FirstOrDefault();
                                                            var R_P_OWSMax_minus = ReworkOWSList.Where(a => a.OWS_ID == Max_owsID_Add_minus).FirstOrDefault();

                                                            #region Rework==1 and DetectedIdStage_first == item.IdStage
                                                            //if (DetectedIdStage_first == timeTrackingStage.IdStage)
                                                            if (R_P_OWSMax_minus.DetectedStageId == timeTrackingStage.IdStage)
                                                            {

                                                                string Rework_second = "Rework_" + timeTrackingStage.IdStage;
                                                                if (!DataTableForGridLayout.Columns.Contains(Rework_second))
                                                                {
                                                                    dr[Rework_second] = TimeSpan.Zero;
                                                                }
                                                                if (Convert.ToString(dr[Rework_second]) != null)
                                                                {
                                                                    string reworkowsInstring = Convert.ToString(dr[Rework_second]);
                                                                    if (string.IsNullOrEmpty(Convert.ToString(reworkowsInstring)) || reworkowsInstring == "0" || reworkowsInstring == "00:00:00")
                                                                    {
                                                                        reworkowsInstring = "00:00:00";
                                                                    }
                                                                    string timeString = Convert.ToString(reworkowsInstring); // Example time format string
                                                                    string[] format = { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" };// Specify the format of the time string
                                                                    TimeSpan TempReworkTime = TimeSpan.ParseExact(timeString, format, null);

                                                                    if (DataTableForGridLayout.Columns.Contains(Rework_second))
                                                                    {
                                                                        dr[Rework_second] = (TempReworkTime + TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference)));

                                                                        TotalReworktime = TotalReworktime + TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference));
                                                                    }

                                                                }
                                                                else
                                                                {
                                                                    dr[Rework_second] = TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference));

                                                                    TotalReworktime = TotalReworktime + TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference));

                                                                }

                                                            }
                                                            #endregion
                                                            else
                                                            {
                                                                #region Rework OWS
                                                                string ROWS_second = "ROWS_" + R_P_OWSMax_minus.DetectedStageId;
                                                                if (!DataTableForGridLayout.Columns.Contains(ROWS_second))
                                                                {
                                                                    dr[ROWS_second] = TimeSpan.Zero;
                                                                }

                                                                if (Convert.ToString(dr[ROWS_second]) != null)
                                                                {
                                                                    string reworkowsInstring = Convert.ToString(dr[ROWS_second]);
                                                                    if (string.IsNullOrEmpty(Convert.ToString(reworkowsInstring)) || reworkowsInstring == "0" || reworkowsInstring == "00:00:00")
                                                                    {
                                                                        reworkowsInstring = "00:00:00";
                                                                    }
                                                                    string timeString = Convert.ToString(reworkowsInstring); // Example time format string
                                                                    string[] format = { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" };// Specify the format of the time string
                                                                    TimeSpan TempReworkTime = TimeSpan.ParseExact(timeString, format, null);

                                                                    if (DataTableForGridLayout.Columns.Contains(ROWS_second))
                                                                    {
                                                                        dr[ROWS_second] = (TempReworkTime + TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference)));

                                                                        TotalReworkOwstime = TotalReworkOwstime + TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference));
                                                                    }

                                                                }
                                                                else
                                                                {
                                                                    dr[ROWS_second] = TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference));

                                                                    TotalReworkOwstime = TotalReworkOwstime + TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference));

                                                                }
                                                                #endregion
                                                                #region Production OWS
                                                                string POWS_second = "POWS_" + timeTrackingStage.IdStage;
                                                                if (!DataTableForGridLayout.Columns.Contains(POWS_second))
                                                                {
                                                                    dr[POWS_second] = TimeSpan.Zero;
                                                                }

                                                                if (Convert.ToString(dr[POWS_second]) != null)
                                                                {
                                                                    string P_owsInstring = Convert.ToString(dr[POWS_second]);
                                                                    if (string.IsNullOrEmpty(Convert.ToString(P_owsInstring)) || P_owsInstring == "0" || P_owsInstring == "00:00:00")
                                                                    {
                                                                        P_owsInstring = "00:00:00";
                                                                    }
                                                                    string timeString = Convert.ToString(P_owsInstring); // Example time format string
                                                                    string[] format = { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" };// Specify the format of the time string
                                                                    TimeSpan TempReworkTime = TimeSpan.ParseExact(timeString, format, null);

                                                                    if (DataTableForGridLayout.Columns.Contains(POWS_second))
                                                                    {
                                                                        dr[POWS_second] = (TempReworkTime + TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference)));

                                                                        TotalProductionOwstime = TotalProductionOwstime + TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference));
                                                                    }

                                                                }
                                                                else
                                                                {
                                                                    dr[POWS_second] = TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference));

                                                                    TotalProductionOwstime = TotalProductionOwstime + TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference));

                                                                }

                                                                #endregion
                                                            }


                                                            // }

                                                        }
                                                        #endregion
                                                        else
                                                        {


                                                            #region if (DetectedIdStage_first != item.IdStage)
                                                            Int32 Max_owsID_Add = ReworkOWSList.Max(a => a.OWS_ID);
                                                            //Int32 Max_owsID_Add_minus = Max_owsID_Add - 1;
                                                            var R_P_OWSMax = ReworkOWSList.Where(a => a.OWS_ID == Max_owsID_Add).FirstOrDefault();
                                                            //var R_P_OWSMax_minus = ReworkOWSList.Where(a => a.OWS_ID == Max_owsID_Add_minus).FirstOrDefault();

                                                            //if (DetectedIdStage_first == timeTrackingStage.IdStage)
                                                            if (R_P_OWSMax.DetectedStageId == timeTrackingStage.IdStage)
                                                            {

                                                                string Rework_second = "Rework_" + timeTrackingStage.IdStage;
                                                                if (!DataTableForGridLayout.Columns.Contains(Rework_second))
                                                                {
                                                                    dr[Rework_second] = TimeSpan.Zero;
                                                                }
                                                                if (Convert.ToString(dr[Rework_second]) != null)
                                                                {
                                                                    string reworkowsInstring = Convert.ToString(dr[Rework_second]);
                                                                    if (string.IsNullOrEmpty(Convert.ToString(reworkowsInstring)) || reworkowsInstring == "0" || reworkowsInstring == "00:00:00")
                                                                    {
                                                                        reworkowsInstring = "00:00:00";
                                                                    }
                                                                    string timeString = Convert.ToString(reworkowsInstring); // Example time format string
                                                                    string[] format = { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" };// Specify the format of the time string
                                                                    TimeSpan TempReworkTime = TimeSpan.ParseExact(timeString, format, null);

                                                                    if (DataTableForGridLayout.Columns.Contains(Rework_second))
                                                                    {
                                                                        dr[Rework_second] = (TempReworkTime + TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference)));

                                                                        TotalReworktime = TotalReworktime + TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference));
                                                                    }

                                                                }
                                                                else
                                                                {
                                                                    dr[Rework_second] = TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference));

                                                                    TotalReworktime = TotalReworktime + TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference));

                                                                }

                                                            }
                                                            else
                                                            {
                                                                #region  rework ows and production ows


                                                                POWS_First = "POWS_" + timeTrackingStage.IdStage;
                                                                ROWS_First = "ROWS_" + R_P_OWSMax.DetectedStageId;
                                                                if (!DataTableForGridLayout.Columns.Contains(POWS_First))
                                                                {
                                                                    dr[POWS_First] = TimeSpan.Zero;
                                                                }
                                                                if (!DataTableForGridLayout.Columns.Contains(ROWS_First))
                                                                {
                                                                    dr[ROWS_First] = TimeSpan.Zero;
                                                                }
                                                                if (DataTableForGridLayout.Columns.Contains(ROWS_First))
                                                                {
                                                                    if (Convert.ToString(dr[ROWS_First]) != null)
                                                                    {
                                                                        string ROWSInstring = Convert.ToString(dr[ROWS_First]);
                                                                        if (string.IsNullOrEmpty(Convert.ToString(ROWSInstring)) || ROWSInstring == "0" || ROWSInstring == "00:00:00")
                                                                        {
                                                                            ROWSInstring = "00:00:00";
                                                                        }
                                                                        string timeString = Convert.ToString(ROWSInstring); // Example time format string
                                                                        string[] format = { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" };// Specify the format of the time string
                                                                        TimeSpan tempROWSInstring = TimeSpan.ParseExact(timeString, format, null);
                                                                        TimeSpan timespan = tempROWSInstring + TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference));
                                                                        //tempstoreOWSvalues = timespan;
                                                                        // TotalProductionOwstime += timespan;
                                                                        if (DataTableForGridLayout.Columns.Contains(ROWS_First))
                                                                        {
                                                                            dr[ROWS_First] = timespan;

                                                                        }

                                                                    }
                                                                    else
                                                                    {
                                                                        if (DataTableForGridLayout.Columns.Contains(ROWS_First))
                                                                        {
                                                                            dr[ROWS_First] = TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference));

                                                                        }

                                                                    }
                                                                }
                                                                TotalReworkOwstime = TotalReworkOwstime + TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference));
                                                                TotalProductionOwstime = TotalProductionOwstime + TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference));
                                                                if (DataTableForGridLayout.Columns.Contains(POWS_First))
                                                                {
                                                                    if (Convert.ToString(dr[POWS_First]) != null)
                                                                    {
                                                                        string POWInstring = Convert.ToString(dr[POWS_First]);
                                                                        if (string.IsNullOrEmpty(Convert.ToString(POWInstring)) || POWInstring == "0" || POWInstring == "00:00:00")
                                                                        {
                                                                            POWInstring = "00:00:00";
                                                                        }
                                                                        string timeString = Convert.ToString(POWInstring); // Example time format string
                                                                        string[] format = { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" }; // Specify the format of the time string
                                                                        TimeSpan POWInTimespan = TimeSpan.ParseExact(timeString, format, null);
                                                                        TimeSpan timespan = POWInTimespan + TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference));

                                                                        if (DataTableForGridLayout.Columns.Contains(POWS_First))
                                                                        {
                                                                            dr[POWS_First] = timespan;
                                                                        }

                                                                    }
                                                                    else
                                                                    {
                                                                        if (DataTableForGridLayout.Columns.Contains(POWS_First))
                                                                        {
                                                                            dr[POWS_First] = TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference));
                                                                        }

                                                                    }
                                                                }

                                                                #endregion
                                                            }

                                                            #endregion
                                                        }
                                                        #endregion
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        GeosApplication.Instance.Logger.Log(string.Format("Error in method FillDashboard() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                                                    }
                                                }
                                                #endregion
                                                #endregion
                                            }
                                            else
                                            {
                                                try
                                                {

                                                    #region check reworkflag_Third 
                                                    //if (reworkflag_Third == false)
                                                    if (ReworkOWSList.Count() > 1)
                                                    {
                                                        Int32 Max_owsID = ReworkOWSList.Max(a => a.OWS_ID);
                                                        Int32 Max_owsID_minus = Max_owsID - 1;
                                                        var tempR_P_OWSMax = ReworkOWSList.Where(a => a.OWS_ID == Max_owsID).FirstOrDefault();
                                                        var tempR_P_OWSMax_minusone = ReworkOWSList.Where(a => a.OWS_ID == Max_owsID_minus).FirstOrDefault();
                                                        #region reworkflag_Second data check and fill only
                                                        #region checkIdstage_Second  == item.IdStage
                                                        //if (checkIdstage_Second == timeTrackingStage.IdStage)
                                                        if (tempR_P_OWSMax.CheckStageId == timeTrackingStage.IdStage)
                                                        {
                                                            #region   if(checkIdstage_Second==DetectedIdStage_first)
                                                            //if (checkIdstage_Second == DetectedIdStage_first)
                                                            if (tempR_P_OWSMax.CheckStageId == tempR_P_OWSMax_minusone.DetectedStageId)
                                                            {
                                                                string Rework_second = "Rework_" + timeTrackingStage.IdStage;
                                                                if (!DataTableForGridLayout.Columns.Contains(Rework_second))
                                                                {
                                                                    dr[Rework_second] = TimeSpan.Zero;
                                                                }


                                                                if (Convert.ToString(dr[Rework_second]) != null)
                                                                {
                                                                    string reworkowsInstring = Convert.ToString(dr[Rework_second]);
                                                                    if (string.IsNullOrEmpty(Convert.ToString(reworkowsInstring)) || reworkowsInstring == "0" || reworkowsInstring == "00:00:00")
                                                                    {
                                                                        reworkowsInstring = "00:00:00";
                                                                    }
                                                                    string timeString = Convert.ToString(reworkowsInstring); // Example time format string
                                                                    string[] format = { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" }; // Specify the format of the time string
                                                                    TimeSpan TempReworkTime = TimeSpan.ParseExact(timeString, format, null);

                                                                    if (DataTableForGridLayout.Columns.Contains(Rework_second))
                                                                    {
                                                                        dr[Rework_second] = (TempReworkTime + TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference)));

                                                                        TotalReworktime = TotalReworktime + TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference));
                                                                    }

                                                                }
                                                                else
                                                                {
                                                                    dr[Rework_second] = TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference));

                                                                    TotalReworktime = TotalReworktime + TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference));

                                                                }
                                                            }
                                                            #endregion
                                                            else
                                                            {
                                                                #region Rework OWS

                                                                //string ROWS_second = "ROWS_" + DetectedIdStage_first;
                                                                string ROWS_second = "ROWS_" + tempR_P_OWSMax_minusone.DetectedStageId;
                                                                if (!DataTableForGridLayout.Columns.Contains(ROWS_second))
                                                                {
                                                                    dr[ROWS_second] = TimeSpan.Zero;
                                                                }

                                                                if (Convert.ToString(dr[ROWS_second]) != null)
                                                                {
                                                                    string reworkowsInstring = Convert.ToString(dr[ROWS_second]);
                                                                    if (string.IsNullOrEmpty(Convert.ToString(reworkowsInstring)) || reworkowsInstring == "0" || reworkowsInstring == "00:00:00")
                                                                    {
                                                                        reworkowsInstring = "00:00:00";
                                                                    }
                                                                    string timeString = Convert.ToString(reworkowsInstring); // Example time format string
                                                                    string[] format = { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" };// Specify the format of the time string
                                                                    TimeSpan TempReworkTime = TimeSpan.ParseExact(timeString, format, null);

                                                                    if (DataTableForGridLayout.Columns.Contains(ROWS_second))
                                                                    {
                                                                        dr[ROWS_second] = (TempReworkTime + TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference)));

                                                                        TotalReworkOwstime = TotalReworkOwstime + TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference));
                                                                    }

                                                                }
                                                                else
                                                                {
                                                                    dr[ROWS_second] = TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference));

                                                                    TotalReworkOwstime = TotalReworkOwstime + TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference));

                                                                }
                                                                #endregion
                                                                #region Production OWS
                                                                string POWS_second = "POWS_" + timeTrackingStage.IdStage;
                                                                if (!DataTableForGridLayout.Columns.Contains(POWS_second))
                                                                {
                                                                    dr[POWS_second] = TimeSpan.Zero;
                                                                }

                                                                if (Convert.ToString(dr[POWS_second]) != null)
                                                                {
                                                                    string P_owsInstring = Convert.ToString(dr[POWS_second]);
                                                                    if (string.IsNullOrEmpty(Convert.ToString(P_owsInstring)) || P_owsInstring == "0" || P_owsInstring == "00:00:00")
                                                                    {
                                                                        P_owsInstring = "00:00:00";
                                                                    }
                                                                    string timeString = Convert.ToString(P_owsInstring); // Example time format string
                                                                    string[] format = { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" };// Specify the format of the time string
                                                                    TimeSpan TempReworkTime = TimeSpan.ParseExact(timeString, format, null);

                                                                    if (DataTableForGridLayout.Columns.Contains(POWS_second))
                                                                    {
                                                                        dr[POWS_second] = (TempReworkTime + TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference)));

                                                                        TotalProductionOwstime = TotalProductionOwstime + TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference));
                                                                    }

                                                                }
                                                                else
                                                                {
                                                                    dr[POWS_second] = TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference));

                                                                    TotalProductionOwstime = TotalProductionOwstime + TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference));

                                                                }

                                                                #endregion

                                                            }
                                                            #region reworkflag_Second completed
                                                            ReworkOWSList.RemoveAll(a => a.OWS_ID == Max_owsID);

                                                            //ReworkTimeIndouble = 0;
                                                            //DetectedIdStage_Second = 0;
                                                            //ProductionOWStime_Second = 0;//GEOS2-6054
                                                            //reworkflag_Second = false;
                                                            //checkIdstage_Second = 0;
                                                            #endregion


                                                            #region reworkflag_Second again true
                                                            // gulab lakade 19 04 2024
                                                            if (timeTrackingStage.Rework == 1)
                                                            {
                                                                Int32 Max_owsID_Add = ReworkOWSList.Max(a => a.OWS_ID);
                                                                var tempR_P_OWSMax_add = ReworkOWSList.Where(a => a.OWS_ID == Max_owsID_Add).FirstOrDefault();
                                                                ERM_Timetracking_rework_ows selected_rework_ows = new ERM_Timetracking_rework_ows();
                                                                selected_rework_ows.OWS_ID = Max_owsID_Add + 1;
                                                                selected_rework_ows.CheckStageId = Convert.ToInt32(timeTrackingStage.IdStage);
                                                                //reworkflag_Second = true;
                                                                //checkIdstage_Second = timeTrackingStage.IdStage;
                                                                var DetectedStageIndex = timeTrackingStages.IndexOf(timeTrackingStage);
                                                                int lastindex = timeTrackingStages.Count();
                                                                if (lastindex - 1 > DetectedStageIndex)
                                                                {
                                                                    var Detectedrecord = timeTrackingStages[DetectedStageIndex + 1];
                                                                    if (Detectedrecord != null)
                                                                    {
                                                                        //DetectedIdStage_Second = Detectedrecord.IdStage;// detected rework stage
                                                                        selected_rework_ows.DetectedStageId = Convert.ToInt32(Detectedrecord.IdStage);
                                                                    }
                                                                    TimeTrackingCurrentStage TempReworkTime = new TimeTrackingCurrentStage();
                                                                    TempReworkTime.IdStage = Detectedrecord.IdStage;
                                                                    TempReworkTime.IdCounterparttracking = Detectedrecord.IdCounterparttracking;
                                                                    TempReworkTime.TimeDifference = Detectedrecord.TimeDifference;
                                                                    ReworkTimeStageList.Add(TempReworkTime);
                                                                }
                                                                ReworkOWSList.Add(selected_rework_ows);


                                                            }
                                                            #endregion
                                                            //end gulab lakade 19 04 2024
                                                        }
                                                        else
                                                        {
                                                            #region only Add reworkflag_Second data not other  
                                                            #region  if (item.Rework == 1) DetectedIdStage_Third
                                                            #region if(checkIdstage_Second==DetectedIdStage_Second)
                                                            Int32 Max_owsID_1 = ReworkOWSList.Max(a => a.OWS_ID);
                                                            var tempR_P_OWSMax_1 = ReworkOWSList.Where(a => a.OWS_ID == Max_owsID_1).FirstOrDefault();
                                                            Int32 Max_owsID_1_minus = Max_owsID_1 - 1;
                                                            var tempR_P_OWSMax_1_minus = ReworkOWSList.Where(a => a.OWS_ID == Max_owsID_1_minus).FirstOrDefault();
                                                            bool sameCheck_Detect = false;
                                                            if (timeTrackingStage.Rework == 1)
                                                            {
                                                                ERM_Timetracking_rework_ows selected_rework_ows = new ERM_Timetracking_rework_ows();
                                                                selected_rework_ows.OWS_ID = Max_owsID_1 + 1;
                                                                var DetectedStageIndex = timeTrackingStages.IndexOf(timeTrackingStage);
                                                                if ((timeTrackingStages.Count() - 1) > DetectedStageIndex)
                                                                {
                                                                    var Detectedrecord = timeTrackingStages[DetectedStageIndex + 1];
                                                                    if (Detectedrecord != null)
                                                                    {
                                                                        //DetectedIdStage_Third = Detectedrecord.IdStage;
                                                                        selected_rework_ows.DetectedStageId = Convert.ToInt32(Detectedrecord.IdStage);
                                                                    }
                                                                }
                                                                //checkIdstage_Third = timeTrackingStage.IdStage;
                                                                selected_rework_ows.CheckStageId = Convert.ToInt32(timeTrackingStage.IdStage);
                                                                //if (checkIdstage_Third == DetectedIdStage_Third )
                                                                if (selected_rework_ows.CheckStageId == selected_rework_ows.DetectedStageId)
                                                                {
                                                                    sameCheck_Detect = true;
                                                                    //checkIdstage_Third = 0;
                                                                    //DetectedIdStage_Third = 0;
                                                                    //reworkflag_Third = false;
                                                                }
                                                                else
                                                                {
                                                                    ReworkOWSList.Add(selected_rework_ows);
                                                                }

                                                            }

                                                            #endregion

                                                            if (timeTrackingStage.Rework == 1 && sameCheck_Detect == false)
                                                            {
                                                                //var DetectedStageIndex = timeTrackingStages.IndexOf(timeTrackingStage);
                                                                //if ((timeTrackingStages.Count() - 1) > DetectedStageIndex)
                                                                //{
                                                                //var Detectedrecord = timeTrackingStages[DetectedStageIndex + 1];
                                                                //if (Detectedrecord != null)
                                                                //{
                                                                //    DetectedIdStage_Third = Detectedrecord.IdStage;
                                                                //}

                                                                //reworkflag_Third = true;
                                                                //checkIdstage_Third = timeTrackingStage.IdStage;
                                                                #region item.Rework == 1 and  if (DetectedIdStage_Second != item.IdStage)
                                                                //if (DetectedIdStage_Second == timeTrackingStage.IdStage)
                                                                if (tempR_P_OWSMax_1_minus.DetectedStageId == timeTrackingStage.IdStage)
                                                                {

                                                                    string Rework_third = "Rework_" + timeTrackingStage.IdStage;
                                                                    if (!DataTableForGridLayout.Columns.Contains(Rework_third))
                                                                    {
                                                                        dr[Rework_third] = TimeSpan.Zero;
                                                                    }
                                                                    if (Convert.ToString(dr[Rework_third]) != null)
                                                                    {
                                                                        string reworkowsInstring = Convert.ToString(dr[Rework_third]);
                                                                        if (string.IsNullOrEmpty(Convert.ToString(reworkowsInstring)) || reworkowsInstring == "0" || reworkowsInstring == "00:00:00")
                                                                        {
                                                                            reworkowsInstring = "00:00:00";
                                                                        }
                                                                        string timeString = Convert.ToString(reworkowsInstring); // Example time format string
                                                                        string[] format = { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" };// Specify the format of the time string
                                                                        TimeSpan TempReworkTime = TimeSpan.ParseExact(timeString, format, null);

                                                                        if (DataTableForGridLayout.Columns.Contains(Rework_third))
                                                                        {
                                                                            dr[Rework_third] = (TempReworkTime + TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference)));

                                                                            TotalReworktime = TotalReworktime + TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference));
                                                                        }

                                                                    }
                                                                    else
                                                                    {
                                                                        dr[Rework_third] = TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference));

                                                                        TotalReworktime = TotalReworktime + TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference));

                                                                    }

                                                                }
                                                                #endregion
                                                                else
                                                                {
                                                                    #region Rework OWS
                                                                    //string ROWS_second = "ROWS_" + DetectedIdStage_Second;
                                                                    string ROWS_second = "ROWS_" + tempR_P_OWSMax_1_minus.DetectedStageId;
                                                                    if (!DataTableForGridLayout.Columns.Contains(ROWS_second))
                                                                    {
                                                                        dr[ROWS_second] = TimeSpan.Zero;
                                                                    }

                                                                    if (Convert.ToString(dr[ROWS_second]) != null)
                                                                    {
                                                                        string reworkowsInstring = Convert.ToString(dr[ROWS_second]);
                                                                        if (string.IsNullOrEmpty(Convert.ToString(reworkowsInstring)) || reworkowsInstring == "0" || reworkowsInstring == "00:00:00")
                                                                        {
                                                                            reworkowsInstring = "00:00:00";
                                                                        }
                                                                        string timeString = Convert.ToString(reworkowsInstring); // Example time format string
                                                                        string[] format = { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" };// Specify the format of the time string
                                                                        TimeSpan TempReworkTime = TimeSpan.ParseExact(timeString, format, null);

                                                                        if (DataTableForGridLayout.Columns.Contains(ROWS_second))
                                                                        {
                                                                            dr[ROWS_second] = (TempReworkTime + TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference)));

                                                                            TotalReworkOwstime = TotalReworkOwstime + TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference));
                                                                        }

                                                                    }
                                                                    else
                                                                    {
                                                                        dr[ROWS_second] = TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference));

                                                                        TotalReworkOwstime = TotalReworkOwstime + TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference));

                                                                    }
                                                                    #endregion
                                                                    #region Production OWS
                                                                    string POWS_second = "POWS_" + timeTrackingStage.IdStage;
                                                                    if (!DataTableForGridLayout.Columns.Contains(POWS_second))
                                                                    {
                                                                        dr[POWS_second] = TimeSpan.Zero;
                                                                    }

                                                                    if (Convert.ToString(dr[POWS_second]) != null)
                                                                    {
                                                                        string P_owsInstring = Convert.ToString(dr[POWS_second]);
                                                                        if (string.IsNullOrEmpty(Convert.ToString(P_owsInstring)) || P_owsInstring == "0" || P_owsInstring == "00:00:00")
                                                                        {
                                                                            P_owsInstring = "00:00:00";
                                                                        }
                                                                        string timeString = Convert.ToString(P_owsInstring); // Example time format string
                                                                        string[] format = { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" };// Specify the format of the time string
                                                                        TimeSpan TempReworkTime = TimeSpan.ParseExact(timeString, format, null);

                                                                        if (DataTableForGridLayout.Columns.Contains(POWS_second))
                                                                        {
                                                                            dr[POWS_second] = (TempReworkTime + TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference)));

                                                                            TotalProductionOwstime = TotalProductionOwstime + TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference));
                                                                        }

                                                                    }
                                                                    else
                                                                    {
                                                                        dr[POWS_second] = TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference));

                                                                        TotalProductionOwstime = TotalProductionOwstime + TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference));

                                                                    }

                                                                    #endregion
                                                                }



                                                                //}

                                                            }
                                                            #endregion
                                                            else
                                                            {

                                                                #region if (DetectedIdStage_Second != item.IdStage)
                                                                //if (DetectedIdStage_Second == timeTrackingStage.IdStage)
                                                                if (tempR_P_OWSMax_1.DetectedStageId == timeTrackingStage.IdStage)
                                                                {

                                                                    string Rework_third = "Rework_" + timeTrackingStage.IdStage;
                                                                    if (!DataTableForGridLayout.Columns.Contains(Rework_third))
                                                                    {
                                                                        dr[Rework_third] = TimeSpan.Zero;
                                                                    }
                                                                    if (Convert.ToString(dr[Rework_third]) != null)
                                                                    {
                                                                        string reworkowsInstring = Convert.ToString(dr[Rework_third]);
                                                                        if (string.IsNullOrEmpty(Convert.ToString(reworkowsInstring)) || reworkowsInstring == "0" || reworkowsInstring == "00:00:00")
                                                                        {
                                                                            reworkowsInstring = "00:00:00";
                                                                        }
                                                                        string timeString = Convert.ToString(reworkowsInstring); // Example time format string
                                                                        string[] format = { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" };// Specify the format of the time string
                                                                        TimeSpan TempReworkTime = TimeSpan.ParseExact(timeString, format, null);

                                                                        if (DataTableForGridLayout.Columns.Contains(Rework_third))
                                                                        {
                                                                            dr[Rework_third] = (TempReworkTime + TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference)));

                                                                            TotalReworktime = TotalReworktime + TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference));
                                                                        }

                                                                    }
                                                                    else
                                                                    {
                                                                        dr[Rework_third] = TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference));

                                                                        TotalReworktime = TotalReworktime + TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference));

                                                                    }

                                                                }
                                                                else
                                                                {
                                                                    #region  rework ows and production ows


                                                                    POWS_Second = "POWS_" + timeTrackingStage.IdStage;
                                                                    //ROWS_Second = "ROWS_" + DetectedIdStage_Second;
                                                                    ROWS_Second = "ROWS_" + tempR_P_OWSMax_1.DetectedStageId;
                                                                    if (!DataTableForGridLayout.Columns.Contains(POWS_Second))
                                                                    {
                                                                        dr[POWS_Second] = TimeSpan.Zero;
                                                                    }
                                                                    if (!DataTableForGridLayout.Columns.Contains(ROWS_Second))
                                                                    {
                                                                        dr[ROWS_Second] = TimeSpan.Zero;
                                                                    }
                                                                    if (DataTableForGridLayout.Columns.Contains(ROWS_Second))
                                                                    {
                                                                        if (Convert.ToString(dr[ROWS_Second]) != null)
                                                                        {
                                                                            string ROWSInstring = Convert.ToString(dr[ROWS_Second]);
                                                                            if (string.IsNullOrEmpty(Convert.ToString(ROWSInstring)) || ROWSInstring == "0" || ROWSInstring == "00:00:00")
                                                                            {
                                                                                ROWSInstring = "00:00:00";
                                                                            }
                                                                            string timeString = Convert.ToString(ROWSInstring); // Example time format string
                                                                            string[] format = { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" }; // Specify the format of the time string
                                                                            TimeSpan tempROWSInstring = TimeSpan.ParseExact(timeString, format, null);
                                                                            TimeSpan timespan = tempROWSInstring + TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference));
                                                                            //tempstoreOWSvalues = timespan;
                                                                            // TotalProductionOwstime += timespan;
                                                                            if (DataTableForGridLayout.Columns.Contains(ROWS_Second))
                                                                            {
                                                                                dr[ROWS_Second] = timespan;

                                                                            }

                                                                        }
                                                                        else
                                                                        {
                                                                            if (DataTableForGridLayout.Columns.Contains(ROWS_Second))
                                                                            {
                                                                                dr[ROWS_Second] = TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference));

                                                                            }

                                                                        }
                                                                    }
                                                                    TotalReworkOwstime = TotalReworkOwstime + TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference));
                                                                    TotalProductionOwstime = TotalProductionOwstime + TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference));
                                                                    if (DataTableForGridLayout.Columns.Contains(POWS_Second))
                                                                    {
                                                                        if (Convert.ToString(dr[POWS_Second]) != null)
                                                                        {
                                                                            string POWInstring = Convert.ToString(dr[POWS_Second]);
                                                                            if (string.IsNullOrEmpty(Convert.ToString(POWInstring)) || POWInstring == "0" || POWInstring == "00:00:00")
                                                                            {
                                                                                POWInstring = "00:00:00";
                                                                            }
                                                                            string timeString = Convert.ToString(POWInstring); // Example time format string
                                                                            string[] format = { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" }; // Specify the format of the time string
                                                                            TimeSpan POWInTimespan = TimeSpan.ParseExact(timeString, format, null);
                                                                            TimeSpan timespan = POWInTimespan + TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference));

                                                                            if (DataTableForGridLayout.Columns.Contains(POWS_Second))
                                                                            {
                                                                                dr[POWS_Second] = timespan;
                                                                            }

                                                                        }
                                                                        else
                                                                        {
                                                                            if (DataTableForGridLayout.Columns.Contains(POWS_Second))
                                                                            {
                                                                                dr[POWS_Second] = TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference));
                                                                            }

                                                                        }
                                                                    }

                                                                    #endregion
                                                                }

                                                                #endregion
                                                            }
                                                            #endregion
                                                        }

                                                        #endregion
                                                        #endregion
                                                    }

                                                    #endregion
                                                }
                                                catch (Exception ex)
                                                {
                                                    GeosApplication.Instance.Logger.Log(string.Format("Error in method FillDashboard() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                                                }
                                            }
                                            #endregion
                                        }
                                        #endregion
                                        else
                                        {
                                            try
                                            {

                                                #region if (timeTrackingStage.Rework == 1) Firstrework==true
                                                if (timeTrackingStage.Rework == 1)
                                                {
                                                    ERM_Timetracking_rework_ows selected_rework_ows = new ERM_Timetracking_rework_ows();
                                                    selected_rework_ows.OWS_ID = 1;
                                                    var DetectedStageIndex = timeTrackingStages.IndexOf(timeTrackingStage);
                                                    int lastindex = timeTrackingStages.Count();
                                                    if (lastindex - 1 > DetectedStageIndex)
                                                    {
                                                        try
                                                        {
                                                            var Detectedrecord = timeTrackingStages[DetectedStageIndex + 1];
                                                            if (Detectedrecord != null)
                                                            {
                                                                // DetectedIdStage_first = Detectedrecord.IdStage;// detected rework stage

                                                                selected_rework_ows.DetectedStageId = Convert.ToInt32(Detectedrecord.IdStage);// detected rework stage

                                                            }

                                                        }
                                                        catch (Exception ex)
                                                        {

                                                        }
                                                        //end

                                                        // reworkflag_First = true;
                                                        // checkIdstage_first = timeTrackingStage.IdStage;
                                                        selected_rework_ows.CheckStageId = Convert.ToInt32(timeTrackingStage.IdStage);
                                                        ReworkOWSList.Add(selected_rework_ows);
                                                        Production = "Production_" + timeTrackingStage.IdStage;
                                                        //Rework_First = "Rework_" + DetectedIdStage_first;
                                                        //POWS_First = "POWS_" + DetectedIdStage_first;
                                                        //ROWS_First = "ROWS_" + DetectedIdStage_first;
                                                        Rework_First = "Rework_" + selected_rework_ows.DetectedStageId;
                                                        POWS_First = "POWS_" + selected_rework_ows.DetectedStageId;
                                                        ROWS_First = "ROWS_" + selected_rework_ows.DetectedStageId;

                                                        #region[GEOS2-7880][gulab lakade][16 04 2025]
                                                        //ProductionTime = TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference));
                                                        if (timeTrackingStage.IdStage == 2 && TimeTrackingList[i].DesignSystem == "EDS")
                                                        {
                                                            #region[GEOS2-7094][dhawal bhalerao][29 04 2025]                                                           
                                                            var tempAddingPosServer = TimeTrackingList[i].TimeTrackingAddingPostServer.Where(x => x.TimeTrackIdCounterparttracking == timeTrackingStage.IdCounterparttracking && (x.ProductionActivityTimeType == 0 || x.ProductionActivityTimeType == 2228 || x.ProductionActivityTimeType == 2229 || x.ProductionActivityTimeType == 2230 || x.ProductionActivityTimeType == 1920 || x.ProductionActivityTimeType == 1921)).ToList();

                                                            if (tempAddingPosServer.Count() > 0)
                                                            {
                                                                //TimeSpan tmptimespanvalue = new TimeSpan();                                                              
                                                                //TimeSpan tmpAdding_time = new TimeSpan();
                                                                //TimeSpan tmpPostServer_time = new TimeSpan();
                                                                //TimeSpan tmpEDS_time = new TimeSpan();
                                                                //TimeSpan tmpDownload_time = new TimeSpan();
                                                                //TimeSpan tmpTransferred_time = new TimeSpan();
                                                                foreach (var ttime in tempAddingPosServer)
                                                                {
                                                                    if (ttime.ProductionActivityTimeType == 0)
                                                                    {
                                                                        tmptimespanvalue = tmptimespanvalue + ttime.TimeTrackDifference;
                                                                    }
                                                                    else if (ttime.ProductionActivityTimeType == 2228)
                                                                    {
                                                                        tmptimespanvalue = tmptimespanvalue + ttime.TimeTrackDifference;
                                                                        tmpAdding_time = tmpAdding_time + ttime.TimeTrackDifference;
                                                                        dr["Addin_" + Convert.ToString(timeTrackingStage.IdStage)] = tmpAdding_time;
                                                                    }
                                                                    else if (ttime.ProductionActivityTimeType == 2229)
                                                                    {
                                                                        tmptimespanvalue = tmptimespanvalue + ttime.TimeTrackDifference;
                                                                        tmpPostServer_time = tmpPostServer_time + ttime.TimeTrackDifference;
                                                                        dr["PostServer_" + Convert.ToString(timeTrackingStage.IdStage)] = tmpPostServer_time;
                                                                    }
                                                                    else if (ttime.ProductionActivityTimeType == 2230)
                                                                    {
                                                                        tmpEDS_time = tmpEDS_time + ttime.TimeTrackDifference;
                                                                        dr["EDS_" + Convert.ToString(timeTrackingStage.IdStage)] = tmpEDS_time;
                                                                    }
                                                                    else if (ttime.ProductionActivityTimeType == 1920)
                                                                    {
                                                                        tmpDownload_time = tmpDownload_time + ttime.TimeTrackDifference;
                                                                        dr["Download_" + Convert.ToString(timeTrackingStage.IdStage)] = tmpDownload_time;
                                                                    }
                                                                    else if (ttime.ProductionActivityTimeType == 1921)
                                                                    {
                                                                        tmpTransferred_time = tmpTransferred_time + ttime.TimeTrackDifference;
                                                                        dr["Transferred_" + Convert.ToString(timeTrackingStage.IdStage)] = tmpTransferred_time;
                                                                    }
                                                                }
                                                                ProductionTime = tmptimespanvalue;
                                                            }
                                                            #endregion
                                                        }
                                                        else if (timeTrackingStage.IdStage == 2 && TimeTrackingList[i].DesignSystem == "GSM")
                                                        {
                                                            #region[GEOS2-7094][dhawal bhalerao][15 05 2025]                                                           
                                                            var tempAddingPosServer = TimeTrackingList[i].TimeTrackingAddingPostServer.Where(x => x.TimeTrackIdCounterparttracking == timeTrackingStage.IdCounterparttracking && (x.ProductionActivityTimeType == 0 || x.ProductionActivityTimeType == 1920 || x.ProductionActivityTimeType == 1921)).ToList();

                                                            if (tempAddingPosServer.Count() > 0)
                                                            {
                                                                //TimeSpan tmptimespanvalue = new TimeSpan();                                                             
                                                                //TimeSpan tmpDownload_time = new TimeSpan();
                                                                //TimeSpan tmpTransferred_time = new TimeSpan();
                                                                foreach (var ttime in tempAddingPosServer)
                                                                {
                                                                    if (ttime.ProductionActivityTimeType == 0)
                                                                    {
                                                                        tmptimespanvalue = tmptimespanvalue + ttime.TimeTrackDifference;
                                                                    }                                                                    
                                                                    else if (ttime.ProductionActivityTimeType == 1920)
                                                                    {
                                                                        tmpDownload_time = tmpDownload_time + ttime.TimeTrackDifference;
                                                                        dr["Download_" + Convert.ToString(timeTrackingStage.IdStage)] = tmpDownload_time;
                                                                    }
                                                                    else if (ttime.ProductionActivityTimeType == 1921)
                                                                    {
                                                                        tmpTransferred_time = tmpTransferred_time + ttime.TimeTrackDifference;
                                                                        dr["Transferred_" + Convert.ToString(timeTrackingStage.IdStage)] = tmpTransferred_time;
                                                                    }
                                                                }
                                                                ProductionTime = tmptimespanvalue;
                                                            }
                                                            #endregion
                                                        }
                                                        else
                                                        {
                                                            ProductionTime = TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference));
                                                        }
                                                        #endregion
                                                        TotalProductiontime += ProductionTime;
                                                        //dr[Production] = ProductionTime;
                                                        if (DataTableForGridLayout.Columns.Contains(Production))  // [Rupali Sarode][GEOS2-4347][05-05-2023]
                                                        {
                                                            if (!string.IsNullOrEmpty(Convert.ToString(timeTrackingStage.TimeDifference)) && Convert.ToString(timeTrackingStage.TimeDifference) != "0")
                                                            {

                                                                if (AppSettingData.Count > 0)
                                                                {
                                                                    if (AppSettingData.Contains(Convert.ToInt32(timeTrackingStage.IdStage)))
                                                                    {
                                                                        if (TimeTrackingList[i].QTY == 1)
                                                                        {
                                                                            if (TimeTrackingList[i].SerialNumber.EndsWith("1"))
                                                                            {
                                                                                if (DataTableForGridLayout.Columns.Contains(Production))
                                                                                {
                                                                                    if (ProductionTime != TimeSpan.Zero)//Aishwarya Ingale[Geos2-6069]
                                                                                    {
                                                                                        if (dr[Rework_First] == DBNull.Value)
                                                                                            dr[Rework_First] = TimeSpan.Zero;
                                                                                        dr[Production] = ProductionTime;                                                                                        
                                                                                    }
                                                                                    TimeTrackingList[i].ProductionHtmlColorFlag = false;
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                ProductionTime = TimeSpan.FromMinutes(Convert.ToDouble(0.0));
                                                                                if (DataTableForGridLayout.Columns.Contains(Production))
                                                                                {
                                                                                    if (ProductionTime != TimeSpan.Zero)//Aishwarya Ingale[Geos2-6069]
                                                                                    {
                                                                                        if (dr[Rework_First] == DBNull.Value)
                                                                                            dr[Rework_First] = TimeSpan.Zero;
                                                                                        dr[Production] = ProductionTime;
                                                                                    }
                                                                                    TimeTrackingList[i].ProductionHtmlColorFlag = true;
                                                                                }
                                                                            }

                                                                        }
                                                                        else
                                                                        {
                                                                            if (ProductionTime != TimeSpan.Zero)//Aishwarya Ingale[Geos2-6069]
                                                                            {
                                                                                if (dr[Rework_First] == DBNull.Value)
                                                                                    dr[Rework_First] = TimeSpan.Zero;
                                                                                dr[Production] = ProductionTime;
                                                                            }
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (ProductionTime != TimeSpan.Zero)//Aishwarya Ingale[Geos2-6069]
                                                                        {
                                                                            if (dr[Rework_First] == DBNull.Value)
                                                                                dr[Rework_First] = TimeSpan.Zero;
                                                                            dr[Production] = ProductionTime;
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (ProductionTime != TimeSpan.Zero)//Aishwarya Ingale[Geos2-6069]
                                                                    {
                                                                        if (dr[Rework_First] == DBNull.Value)
                                                                            dr[Rework_First] = TimeSpan.Zero;
                                                                        dr[Production] = ProductionTime;
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (ProductionTime != TimeSpan.Zero)//Aishwarya Ingale[Geos2-6069]
                                                                {
                                                                    if (dr[Rework_First] == DBNull.Value)
                                                                        dr[Rework_First] = TimeSpan.Zero;
                                                                    dr[Production] = ProductionTime;
                                                                }
                                                            }
                                                            //decimal? tempProduction = Convert.ToDecimal(timeTrackingStage.TimeDifference);
                                                            //TempTimeTrackingByStageList.Where(x => x.IdStage == timeTrackingStage.IdStage).ToList().ForEach(a => a.Production = tempProduction);
                                                        }                                                        
                                                    }
                                                }
                                                #endregion
                                                else
                                                {
                                                    Production = "Production_" + timeTrackingStage.IdStage;
                                                    Rework = "Rework_" + timeTrackingStage.IdStage;

                                                    #region[GEOS2-7094][dhawal bhalerao][29 04 2025]
                                                    //ProductionTime = TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference));
                                                    if (timeTrackingStage.IdStage == 2 && TimeTrackingList[i].DesignSystem == "EDS")
                                                    {
                                                        var tempAddingPosServer = TimeTrackingList[i].TimeTrackingAddingPostServer.Where(x => x.TimeTrackIdCounterparttracking == timeTrackingStage.IdCounterparttracking && (x.ProductionActivityTimeType == 0 || x.ProductionActivityTimeType == 2228 || x.ProductionActivityTimeType == 2229 || x.ProductionActivityTimeType == 2230 || x.ProductionActivityTimeType == 1920 || x.ProductionActivityTimeType == 1921)).ToList();

                                                        if (tempAddingPosServer.Count() > 0)
                                                        {
                                                            //TimeSpan tmptimespanvalue = new TimeSpan();
                                                            //TimeSpan tmpAdding_time = new TimeSpan();
                                                            //TimeSpan tmpPostServer_time = new TimeSpan();
                                                            //TimeSpan tmpEDS_time = new TimeSpan();
                                                            //TimeSpan tmpDownload_time = new TimeSpan();
                                                            //TimeSpan tmpTransferred_time = new TimeSpan();
                                                            foreach (var ttime in tempAddingPosServer)
                                                            {
                                                                if (ttime.ProductionActivityTimeType == 0)
                                                                {
                                                                    tmptimespanvalue = tmptimespanvalue + ttime.TimeTrackDifference;
                                                                }
                                                                else if (ttime.ProductionActivityTimeType == 2228)
                                                                {
                                                                    tmptimespanvalue = tmptimespanvalue + ttime.TimeTrackDifference;
                                                                    tmpAdding_time = tmpAdding_time + ttime.TimeTrackDifference;
                                                                    dr["Addin_" + Convert.ToString(timeTrackingStage.IdStage)] = tmpAdding_time;
                                                                }
                                                                else if (ttime.ProductionActivityTimeType == 2229)
                                                                {
                                                                    tmptimespanvalue = tmptimespanvalue + ttime.TimeTrackDifference;
                                                                    tmpPostServer_time = tmpPostServer_time + ttime.TimeTrackDifference;
                                                                    dr["PostServer_" + Convert.ToString(timeTrackingStage.IdStage)] = tmpPostServer_time;
                                                                }
                                                                else if (ttime.ProductionActivityTimeType == 2230)
                                                                {
                                                                    tmpEDS_time = tmpEDS_time + ttime.TimeTrackDifference;
                                                                    dr["EDS_" + Convert.ToString(timeTrackingStage.IdStage)] = tmpEDS_time;
                                                                }
                                                                else if (ttime.ProductionActivityTimeType == 1920)
                                                                {
                                                                    tmpDownload_time = tmpDownload_time + ttime.TimeTrackDifference;
                                                                    dr["Download_" + Convert.ToString(timeTrackingStage.IdStage)] = tmpDownload_time;
                                                                }
                                                                else if (ttime.ProductionActivityTimeType == 1921)
                                                                {
                                                                    tmpTransferred_time = tmpTransferred_time + ttime.TimeTrackDifference;
                                                                    dr["Transferred_" + Convert.ToString(timeTrackingStage.IdStage)] = tmpTransferred_time;
                                                                }
                                                            }                                                            
                                                            ProductionTime = tmptimespanvalue;
                                                        }
                                                    }
                                                    else if (timeTrackingStage.IdStage == 2 && TimeTrackingList[i].DesignSystem == "GSM")
                                                    {
                                                        var tempAddingPosServer = TimeTrackingList[i].TimeTrackingAddingPostServer.Where(x => x.TimeTrackIdCounterparttracking == timeTrackingStage.IdCounterparttracking && (x.ProductionActivityTimeType == 0 || x.ProductionActivityTimeType == 1920 || x.ProductionActivityTimeType == 1921)).ToList();

                                                        if (tempAddingPosServer.Count() > 0)
                                                        {
                                                            //TimeSpan tmptimespanvalue = new TimeSpan();                                                            
                                                            //TimeSpan tmpDownload_time = new TimeSpan();
                                                            //TimeSpan tmpTransferred_time = new TimeSpan();
                                                            foreach (var ttime in tempAddingPosServer)
                                                            {
                                                                if (ttime.ProductionActivityTimeType == 0)
                                                                {
                                                                    tmptimespanvalue = tmptimespanvalue + ttime.TimeTrackDifference;
                                                                }                                                                
                                                                else if (ttime.ProductionActivityTimeType == 1920)
                                                                {
                                                                    tmpDownload_time = tmpDownload_time + ttime.TimeTrackDifference;
                                                                    dr["Download_" + Convert.ToString(timeTrackingStage.IdStage)] = tmpDownload_time;
                                                                }
                                                                else if (ttime.ProductionActivityTimeType == 1921)
                                                                {
                                                                    tmpTransferred_time = tmpTransferred_time + ttime.TimeTrackDifference;
                                                                    dr["Transferred_" + Convert.ToString(timeTrackingStage.IdStage)] = tmpTransferred_time;
                                                                }
                                                            }
                                                            ProductionTime = tmptimespanvalue;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        ProductionTime = TimeSpan.FromSeconds(Convert.ToDouble(timeTrackingStage.TimeDifference));
                                                    }
                                                    #endregion
                                                    ReworkTime = TimeSpan.FromSeconds(Convert.ToDouble(0));
                                                    POWS = "POWS_" + timeTrackingStage.IdStage;
                                                    ROWS = "ROWS_" + timeTrackingStage.IdStage;
                                                    OWSprodandReworkTime = TimeSpan.FromSeconds(Convert.ToDouble(0));
                                                    TotalProductiontime += ProductionTime;
                                                    #region old production time get
                                                    string oldproductiontime = Convert.ToString(dr[Production]);

                                                    if (string.IsNullOrEmpty(Convert.ToString(oldproductiontime)) || oldproductiontime == "0" || oldproductiontime == "00:00:00")
                                                    {
                                                        oldproductiontime = "0.00:00:00";
                                                    }
                                                    string oldprod_string = Convert.ToString(oldproductiontime); // Example time format string
                                                    string[] format_Prod = { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" }; // Specify the format of the time string
                                                    TimeSpan OldproductionTime = TimeSpan.ParseExact(oldprod_string, format_Prod, null);
                                                    #endregion
                                                    if (!string.IsNullOrEmpty(Convert.ToString(timeTrackingStage.TimeDifference)) && Convert.ToString(timeTrackingStage.TimeDifference) != "0")
                                                    {
                                                        if (AppSettingData.Count > 0)
                                                        {
                                                            if (AppSettingData.Contains(Convert.ToInt32(timeTrackingStage.IdStage)))
                                                            {
                                                                if (TimeTrackingList[i].QTY == 1)
                                                                {
                                                                    if (TimeTrackingList[i].SerialNumber.EndsWith("1"))
                                                                    {
                                                                        if (DataTableForGridLayout.Columns.Contains(Production))
                                                                        {
                                                                            if (ProductionTime != TimeSpan.Zero)//Aishwarya Ingale[Geos2-6069]
                                                                            {
                                                                                if (dr[Rework] == DBNull.Value)
                                                                                    dr[Rework] = TimeSpan.Zero;
                                                                                dr[Production] = ProductionTime + OldproductionTime;
                                                                                // insert ading and piost se
                                                                                dr[POWS] = OWSprodandReworkTime;
                                                                                dr[ROWS] = OWSprodandReworkTime;
                                                                            }
                                                                            TimeTrackingList[i].ProductionHtmlColorFlag = false;

                                                                        }

                                                                    }
                                                                    else
                                                                    {
                                                                        ProductionTime = TimeSpan.FromMinutes(Convert.ToDouble(0.0));
                                                                        if (DataTableForGridLayout.Columns.Contains(Production))
                                                                        {
                                                                            if (ProductionTime != TimeSpan.Zero)//Aishwarya Ingale[Geos2-6069]
                                                                            {
                                                                                if (dr[Rework] == DBNull.Value)
                                                                                    dr[Rework] = TimeSpan.Zero;
                                                                                dr[Production] = ProductionTime + OldproductionTime;
                                                                                // insert ading and piost se
                                                                                dr[POWS] = OWSprodandReworkTime;
                                                                                dr[ROWS] = OWSprodandReworkTime;
                                                                            }
                                                                            TimeTrackingList[i].ProductionHtmlColorFlag = true;

                                                                        }
                                                                    }

                                                                }
                                                                else
                                                                {
                                                                    if (ProductionTime != TimeSpan.Zero)//Aishwarya Ingale[Geos2-6069]
                                                                    {
                                                                        if (dr[Rework] == DBNull.Value)
                                                                            dr[Rework] = TimeSpan.Zero;
                                                                        dr[Production] = ProductionTime + OldproductionTime;
                                                                        // insert ading and piost se
                                                                        dr[POWS] = OWSprodandReworkTime;
                                                                        dr[ROWS] = OWSprodandReworkTime;
                                                                    }

                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (ProductionTime != TimeSpan.Zero)//Aishwarya Ingale[Geos2-6069]
                                                                {
                                                                    if (dr[Rework] == DBNull.Value)
                                                                        dr[Rework] = TimeSpan.Zero;
                                                                    dr[Production] = ProductionTime + OldproductionTime;
                                                                    // insert ading and piost se
                                                                    dr[POWS] = OWSprodandReworkTime;
                                                                    dr[ROWS] = OWSprodandReworkTime;
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (ProductionTime != TimeSpan.Zero)//Aishwarya Ingale[Geos2-6069]
                                                            {
                                                                if (dr[Rework] == DBNull.Value)
                                                                    dr[Rework] = TimeSpan.Zero;
                                                                dr[Production] = ProductionTime + OldproductionTime;
                                                                // insert ading and piost se
                                                                dr[POWS] = OWSprodandReworkTime;
                                                                dr[ROWS] = OWSprodandReworkTime;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (ProductionTime != TimeSpan.Zero)//Aishwarya Ingale[Geos2-6069]
                                                        {
                                                            if (dr[Rework] == DBNull.Value)
                                                                dr[Rework] = TimeSpan.Zero;
                                                            dr[Production] = ProductionTime + OldproductionTime;
                                                            // insert ading and piost se
                                                            dr[POWS] = OWSprodandReworkTime;
                                                            dr[ROWS] = OWSprodandReworkTime;
                                                        }
                                                    }                                                    


                                                    decimal? tempProduction = Convert.ToDecimal(timeTrackingStage.TimeDifference);
                                                    TempTimeTrackingByStageList.Where(x => x.IdStage == timeTrackingStage.IdStage).ToList().ForEach(a => a.Production = tempProduction);
                                                    TotalReworktime += ReworkTime;
                                                    TotalProductionOwstime += OWSprodandReworkTime;
                                                    if (DataTableForGridLayout.Columns.Contains(Rework))  // [Rupali Sarode][GEOS2-4347][05-05-2023]
                                                    {
                                                        #region Aishwarya Ingale[Geos2-6069]
                                                        if (ProductionTime != TimeSpan.Zero && ReworkTime != TimeSpan.Zero)
                                                        {
                                                            dr[Rework] = ReworkTime;
                                                        }
                                                        else if (ProductionTime == TimeSpan.Zero && ReworkTime == TimeSpan.Zero)
                                                        {
                                                            dr[Rework] = DBNull.Value;
                                                            TimeTrackingList[i].ProductionHtmlColorFlag = false;
                                                        }
                                                        else if (ProductionTime == TimeSpan.Zero && ReworkTime != TimeSpan.Zero)
                                                        {
                                                            dr[Rework] = ReworkTime;
                                                            TimeTrackingList[i].ProductionHtmlColorFlag = false;
                                                        }
                                                        else if (ProductionTime == TimeSpan.Zero)
                                                        {
                                                            dr[Rework] = DBNull.Value;
                                                            TimeTrackingList[i].ProductionHtmlColorFlag = false;
                                                        }

                                                        #endregion


                                                        if (Convert.ToString(dr[Rework]) != null)
                                                        {
                                                            string ReworkInstring = Convert.ToString(dr[Rework]);
                                                            if (string.IsNullOrEmpty(Convert.ToString(ReworkInstring)))
                                                            {
                                                                ReworkInstring = "00:00:00";
                                                            }
                                                            string timeString = Convert.ToString(ReworkInstring); // Example time format string
                                                            string[] format = { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" }; // Specify the format of the time string
                                                            TimeSpan TempReworkInTimespan = TimeSpan.ParseExact(timeString, format, null);
                                                            TimeSpan timespan = TempReworkInTimespan + ReworkTime;

                                                            if (DataTableForGridLayout.Columns.Contains(Rework))  // [Rupali Sarode][GEOS2-4347][05-05-2023]
                                                            {
                                                                #region Aishwarya Ingale[Geos2-6069]
                                                                if (ProductionTime != TimeSpan.Zero && timespan != TimeSpan.Zero)
                                                                {
                                                                    dr[Rework] = timespan;
                                                                }
                                                                else if (ProductionTime == TimeSpan.Zero && timespan == TimeSpan.Zero)
                                                                {
                                                                    dr[Rework] = DBNull.Value;
                                                                    TimeTrackingList[i].ProductionHtmlColorFlag = false;
                                                                }
                                                                else if (ProductionTime == TimeSpan.Zero && timespan != TimeSpan.Zero)
                                                                {
                                                                    dr[Rework] = timespan;
                                                                    TimeTrackingList[i].ProductionHtmlColorFlag = false;
                                                                }
                                                                else if (ProductionTime == TimeSpan.Zero)
                                                                {
                                                                    dr[Rework] = DBNull.Value;
                                                                    TimeTrackingList[i].ProductionHtmlColorFlag = false;
                                                                }

                                                                #endregion
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (DataTableForGridLayout.Columns.Contains(Rework))  // [Rupali Sarode][GEOS2-4347][05-05-2023]
                                                            {
                                                                #region Aishwarya Ingale[Geos2-6069]
                                                                if (ProductionTime != TimeSpan.Zero && ReworkTime != TimeSpan.Zero)
                                                                {
                                                                    dr[Rework] = ReworkTime;
                                                                }
                                                                else if (ProductionTime == TimeSpan.Zero && ReworkTime == TimeSpan.Zero)
                                                                {
                                                                    dr[Rework] = DBNull.Value;
                                                                    TimeTrackingList[i].ProductionHtmlColorFlag = false;
                                                                }
                                                                else if (ProductionTime == TimeSpan.Zero && ReworkTime != TimeSpan.Zero)
                                                                {
                                                                    dr[Rework] = ReworkTime;
                                                                    TimeTrackingList[i].ProductionHtmlColorFlag = false;
                                                                }
                                                                else if (ProductionTime == TimeSpan.Zero)
                                                                {
                                                                    dr[Rework] = DBNull.Value;
                                                                    TimeTrackingList[i].ProductionHtmlColorFlag = false;
                                                                }

                                                                #endregion
                                                            }

                                                        }
                                                    }
                                                    //rajashri
                                                    if (DataTableForGridLayout.Columns.Contains(POWS))
                                                    {
                                                        string productionowsInstring = Convert.ToString(dr[POWS]);
                                                        if (string.IsNullOrEmpty(Convert.ToString(productionowsInstring)) || productionowsInstring == "0" || productionowsInstring == "00:00:00")
                                                        {
                                                            productionowsInstring = "00:00:00";
                                                        }
                                                        string timeString = Convert.ToString(productionowsInstring); // Example time format string
                                                        string[] format = { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" }; // Specify the format of the time string

                                                        TimeSpan TempReworkInTimespan = TimeSpan.ParseExact(timeString, format, null);
                                                        TimeSpan timespan = OWSprodandReworkTime;
                                                        if (DataTableForGridLayout.Columns.Contains(POWS))  // [Rupali Sarode][GEOS2-4347][05-05-2023]
                                                        {
                                                            dr[POWS] = timespan;
                                                            dr[ROWS] = timespan;

                                                        }
                                                    }
                                                    //end


                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillDashboard() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                                            }
                                        }

                                    }
                                    else
                                    {
                                        //if (timeTrackingStage.Rework == 1)
                                        //{
                                        //    reworkflag_First = true;
                                        //    checkIdstage_first = timeTrackingStage.IdStage;
                                        //}
                                    }
                                }
                                else
                                {
                                    //if (timeTrackingStage.Rework == 1)
                                    //{
                                    //    reworkflag_First = true;
                                    //    checkIdstage_first = timeTrackingStage.IdStage;
                                    //}
                                }
                            }
                            foreach (var stageItem in TempTimeTrackingByStageList)
                            {
                                string real = "Real_" + Convert.ToString(stageItem.IdStage);
                                double TempExpectedTime = 0;//gulab lakade  mismatch total
                                TimeSpan Tempreal = TimeSpan.Parse("0");

                                #region Real value
                                string[] format = { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" };  // Specify the format of the time string
                                string tempproduction = Convert.ToString(dr["Production_" + stageItem.IdStage]);
                                if (string.IsNullOrEmpty(Convert.ToString(tempproduction)) || tempproduction == "0" || tempproduction == "00:00:00")
                                {
                                    tempproduction = "00:00:00";
                                }
                                string PRO_timeString = Convert.ToString(tempproduction); // Example time format string

                                TimeSpan Pro_InTimespan = TimeSpan.ParseExact(PRO_timeString, format, null);

                                string tempRework = Convert.ToString(dr["Rework_" + stageItem.IdStage]);
                                if (string.IsNullOrEmpty(Convert.ToString(tempRework)) || tempRework == "0" || tempRework == "00:00:00")
                                {
                                    tempRework = "00:00:00";
                                }
                                string R_timeString = Convert.ToString(tempRework); // Example time format string

                                TimeSpan R_InTimespan = TimeSpan.ParseExact(R_timeString, format, null);
                                string tempP_OWS = Convert.ToString(dr["POWS_" + stageItem.IdStage]);
                                if (string.IsNullOrEmpty(Convert.ToString(tempP_OWS)) || tempP_OWS == "0" || tempP_OWS == "00:00:00")
                                {
                                    tempP_OWS = "00:00:00";
                                }
                                string POWS_timeString = Convert.ToString(tempP_OWS); // Example time format string

                                TimeSpan POWS_InTimespan = TimeSpan.ParseExact(POWS_timeString, format, null);
                                Tempreal = Pro_InTimespan + R_InTimespan + POWS_InTimespan;
                                if (DataTableForGridLayout.Columns.Contains(real))  // [Rupali Sarode][GEOS2-4347][05-05-2023]
                                {

                                    #region [GEOS2-6742][gulab lakade][30 12 2024]
                                    //dr[real] = Tempreal;
                                    if (Tempreal == TimeSpan.Zero)
                                    {
                                        dr[real] = DBNull.Value;
                                    }
                                    else
                                    {
                                        dr[real] = Tempreal;
                                    }

                                    #endregion
                                }
                                #endregion

                                #region [GEOS2-6742][gulab lakade][30 12 2024]
                                if (Convert.ToString(TimeTrackingList[i].SerialNumber) == "2306630P009001")
                                {

                                }
                                string tempR_OWS = Convert.ToString(dr["ROWS_" + stageItem.IdStage]);
                                if (string.IsNullOrEmpty(Convert.ToString(tempR_OWS)) || tempR_OWS == "0" || tempR_OWS == "00:00:00")
                                {
                                    tempR_OWS = "00:00:00";
                                }
                                string ROWS_timeString = Convert.ToString(tempR_OWS); // Example time format string

                                TimeSpan ROWS_InTimespan = TimeSpan.ParseExact(ROWS_timeString, format, null);
                                if (!string.IsNullOrEmpty(Convert.ToString(dr["Production_" + stageItem.IdStage])))
                                {
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["Rework_" + stageItem.IdStage])))
                                    {
                                        dr["Rework_" + stageItem.IdStage] = TimeSpan.Zero;
                                    }
                                    else
                                    if (!string.IsNullOrEmpty(Convert.ToString(dr["Rework_" + stageItem.IdStage])) && Convert.ToString(dr["Rework_" + stageItem.IdStage]) == "00:00:00")
                                    {
                                        dr["Rework_" + stageItem.IdStage] = TimeSpan.Zero;
                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["POWS_" + stageItem.IdStage])))
                                    {
                                        dr["POWS_" + stageItem.IdStage] = TimeSpan.Zero;
                                    }
                                    else
                                    if (!string.IsNullOrEmpty(Convert.ToString(dr["POWS_" + stageItem.IdStage])) && Convert.ToString(dr["POWS_" + stageItem.IdStage]) == "00:00:00")
                                    {
                                        dr["POWS_" + stageItem.IdStage] = TimeSpan.Zero;
                                    }

                                    if (string.IsNullOrEmpty(Convert.ToString(dr["ROWS_" + stageItem.IdStage])))
                                    {
                                        dr["ROWS_" + stageItem.IdStage] = TimeSpan.Zero;
                                    }
                                    else
                                         if (!string.IsNullOrEmpty(Convert.ToString(dr["ROWS_" + stageItem.IdStage])) && Convert.ToString(dr["ROWS_" + stageItem.IdStage]) == "00:00:00")//[rani dhamankar][27-05-2025][GEOS2-8196]
                                    {
                                        dr["ROWS_" + stageItem.IdStage] = TimeSpan.Zero;
                                    }
                                }
                                else
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["Production_" + stageItem.IdStage])))
                                {
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["Rework_" + stageItem.IdStage])))
                                    {
                                        dr["Rework_" + stageItem.IdStage] = DBNull.Value;
                                    }
                                    else
                                     if (!string.IsNullOrEmpty(Convert.ToString(dr["Rework_" + stageItem.IdStage])) && Convert.ToString(dr["Rework_" + stageItem.IdStage]) == "00:00:00")
                                    {
                                        dr["Rework_" + stageItem.IdStage] = DBNull.Value;
                                    }

                                    if (string.IsNullOrEmpty(Convert.ToString(dr["POWS_" + stageItem.IdStage])))
                                    {
                                        dr["POWS_" + stageItem.IdStage] = DBNull.Value;
                                    }
                                    else
                                     if (!string.IsNullOrEmpty(Convert.ToString(dr["POWS_" + stageItem.IdStage])) && Convert.ToString(dr["POWS_" + stageItem.IdStage]) == "00:00:00")
                                    {
                                        dr["POWS_" + stageItem.IdStage] = DBNull.Value;
                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["ROWS_" + stageItem.IdStage])))
                                    {
                                        dr["ROWS_" + stageItem.IdStage] = DBNull.Value;
                                    }
                                    else
                                         if (!string.IsNullOrEmpty(Convert.ToString(dr["ROWS_" + stageItem.IdStage])) && Convert.ToString(dr["ROWS_" + stageItem.IdStage]) == "00:00:00")
                                    {
                                        dr["ROWS_" + stageItem.IdStage] = DBNull.Value;
                                    }
                                }
                                #endregion
                                string expected = "Expected_" + Convert.ToString(stageItem.IdStage);
                                TimeSpan Tempexpected = TimeSpan.Parse("0");


                                if (!string.IsNullOrEmpty(Convert.ToString(stageItem.Expected)) && Convert.ToString(stageItem.Expected) != "0")
                                {
                                    Tempexpected = TimeSpan.FromMinutes(Convert.ToDouble(stageItem.Expected));

                                }
                                else
                                {
                                    Tempexpected = TimeSpan.FromMinutes(Convert.ToDouble(0.0));

                                }

                                #region [GEOS2-5854][gulab lakade][18 07 2024]
                                if (CADCAMDesignTypeList.Count() > 0)
                                {
                                    var TempCADCAM = CADCAMDesignTypeList.Where(x => x.IdStage == stageItem.IdStage && x.DesignType == Convert.ToString(TimeTrackingList[i].DrawingType)).FirstOrDefault();
                                    if (TempCADCAM != null)
                                    {
                                        if (TempCADCAM.RoleValue == "C")
                                        {
                                            Tempexpected = TimeSpan.FromSeconds(TempCADCAM.DesignValue);
                                            TempExpectedTime = Convert.ToDouble(Tempexpected.TotalMinutes);//gulab lakade  mismatch total
                                        }
                                        else
                                        {

                                            double doubleExpected = Tempexpected.TotalSeconds;
                                            doubleExpected = doubleExpected * Convert.ToDouble(Convert.ToDouble(TempCADCAM.DesignValue) / Convert.ToDouble(100));
                                            Tempexpected = TimeSpan.FromSeconds(doubleExpected);
                                            TempExpectedTime = Convert.ToDouble(Tempexpected.TotalMinutes); ;//gulab lakade  mismatch total 14 08 2024
                                        }

                                    }
                                    else
                                    {

                                        double doubleExpected = Tempexpected.TotalSeconds;
                                        Tempexpected = TimeSpan.FromSeconds(doubleExpected);
                                        TempExpectedTime = Convert.ToDouble(Tempexpected.TotalMinutes);



                                    }
                                }
                                if (DataTableForGridLayout.Columns.Contains(expected))
                                {
                                    dr[expected] = Tempexpected;
                                    //gulab lakade  mismatch total 14 08 2024
                                    if (TempExpectedTime != 0)
                                    {
                                        stageItem.Expected = Convert.ToDecimal(TempExpectedTime);
                                    }
                                    //gulab lakade  mismatch total 14 08 2024
                                }
                                #endregion

                                string remaining = "Remaining_" + Convert.ToString(stageItem.IdStage);
                                #region [pallavi jadhav][GEOS2-5320][05 11 2024]
                                string plannedDeliveryDate = "PlannedDeliveryDate_" + Convert.ToString(stageItem.IdStage);
                                string plannedDeliveryDateHtmlColor = "PlannedDeliveryDateHtmlColor_" + Convert.ToString(stageItem.IdStage);
                                if (DataTableForGridLayout.Columns.Contains(plannedDeliveryDate))
                                {
                                    if (stageItem.PlannedDeliveryDateByStage != null)
                                    {
                                        dr[plannedDeliveryDateHtmlColor] = stageItem.PlannedDeliveryDateHtmlColor;
                                        dr[plannedDeliveryDate] = stageItem.PlannedDeliveryDateByStage;
                                    }
                                }
                                string days = "Days_" + Convert.ToString(stageItem.IdStage);
                                if (DataTableForGridLayout.Columns.Contains(days))
                                {
                                    if (stageItem.Days != null && stageItem.Days != 0)
                                    {
                                        dr[days] = stageItem.Days;
                                    }
                                }
                                #endregion
                                string tempcolor = "Tempcolor_" + Convert.ToString(stageItem.IdStage);
                                TimeSpan TempProduction = Pro_InTimespan;
                                //if (!string.IsNullOrEmpty(Convert.ToString(stageItem.Production)) && Convert.ToString(stageItem.Production) != "0")
                                //{
                                //    TempProduction = TimeSpan.FromSeconds(Convert.ToDouble(stageItem.Production));
                                //}

                                //if (Tempreal <= Tempexpected)
                                if (TempProduction <= Tempexpected)
                                {

                                    if (DataTableForGridLayout.Columns.Contains(tempcolor))
                                    {
                                        dr[tempcolor] = true;
                                    }

                                    if (DataTableForGridLayout.Columns.Contains(remaining))
                                    {
                                        dr[remaining] = (Tempexpected - TempProduction);
                                    }
                                    TempTotalRemaianing += (Tempexpected - TempProduction);

                                }
                                else
                                {
                                    if (DataTableForGridLayout.Columns.Contains(tempcolor))
                                    {
                                        dr[tempcolor] = false;
                                    }
                                    if (DataTableForGridLayout.Columns.Contains(remaining))
                                    {
                                        dr[remaining] = (Tempexpected - TempProduction);
                                    }
                                    TempTotalRemaianing += (Tempexpected - TempProduction);
                                }

                                


                            }

                            double TotalExpectedIndouble = Convert.ToDouble(TempTimeTrackingByStageList.Sum(a => a.Expected));
                            // DateTime PDD = TempTimeTrackingByStageList.Select(a=>a.PlannedDeliveryDateByStage).


                            //TempTotalReal = TimeSpan.FromSeconds(TotalRealIndouble);
                            TempTotalReal = TotalProductiontime + TotalProductionOwstime + TotalReworktime;
                            dr["Real"] = TempTotalReal;


                            if (TotalExpectedIndouble != 0)
                            {
                                //TempTotalExpected = TimeSpan.FromSeconds(TotalExpectedIndouble);
                                TempTotalExpected = TimeSpan.FromMinutes(TotalExpectedIndouble); //gulab lakade  mismatch total 14 08 2024
                                dr["Expected"] = TempTotalExpected;

                            }
                            if (TotalProductiontime != TimeSpan.Parse("0"))
                            {
                                dr["Production"] = TotalProductiontime;

                            }
                            if (TotalProductionOwstime != TimeSpan.Parse("0"))
                            {
                                dr["POWS"] = TotalProductionOwstime;

                            }
                            if (TotalReworktime != TimeSpan.Parse("0"))
                            {
                                dr["Rework"] = TotalReworktime;

                            }
                            if (TotalProductionOwstime != TimeSpan.Parse("0"))
                            {
                                dr["ROWS"] = TotalReworkOwstime;

                            }
                            if (TempTotalRemaianing != null)
                            {
                                if (TempTotalReal <= TempTotalExpected)
                                {

                                    dr["Tempcolor"] = true;
                                    dr["Remaining"] = TempTotalRemaianing;
                                }
                                else
                                {
                                    dr["Tempcolor"] = false;
                                    dr["Remaining"] = TempTotalRemaianing;
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                        }

                        //    for (int iItem = 0; iItem < TimeTrackingList[i].TimeTrackingStage.GroupBy(x => x.IdStage).ToList().Count; iItem++)
                        //    {
                        //        try
                        //        {
                        //            string real = "Real_" + Convert.ToString(TimeTrackingList[i].TimeTrackingStage[iItem].IdStage);
                        //            TimeSpan Tempreal = TimeSpan.Parse("0");

                        //            if (!string.IsNullOrEmpty(Convert.ToString(TimeTrackingList[i].TimeTrackingStage[iItem].Real)) && Convert.ToString(TimeTrackingList[i].TimeTrackingStage[iItem].Real) != "0")
                        //            {
                        //                //Tempreal = ConvertfloattoTimespan(Convert.ToString(TimeTrackingList[i].TimeTrackingStage[iItem].Real));
                        //                Tempreal = TimeSpan.FromSeconds(Convert.ToDouble(TimeTrackingList[i].TimeTrackingStage[iItem].Real));
                        //                if (DataTableForGridLayout.Columns.Contains(real))  // [Rupali Sarode][GEOS2-4347][05-05-2023]
                        //                {
                        //                    dr[real] = Tempreal;
                        //                }
                        //            }

                        //            TempTotalReal += Tempreal;// Convert.ToDouble(item1.Real);

                        //            string expected = "Expected_" + Convert.ToString(TimeTrackingList[i].TimeTrackingStage[iItem].IdStage);
                        //            TimeSpan Tempexpected = TimeSpan.Parse("0");
                        //            if (!string.IsNullOrEmpty(Convert.ToString(TimeTrackingList[i].TimeTrackingStage[iItem].Expected)) && Convert.ToString(TimeTrackingList[i].TimeTrackingStage[iItem].Expected) != "0")
                        //            {
                        //                //Tempexpected = ConvertfloattoTimespan(Convert.ToString(TimeTrackingList[i].TimeTrackingStage[iItem].Expected));
                        //                Tempexpected = TimeSpan.FromMinutes(Convert.ToDouble(TimeTrackingList[i].TimeTrackingStage[iItem].Expected));
                        //                if (DataTableForGridLayout.Columns.Contains(expected))  // [Rupali Sarode][GEOS2-4347][05-05-2023]
                        //                {
                        //                    dr[expected] = Tempexpected;
                        //                }
                        //            }
                        //            else    //[GEOS2-4150] [Gulab Lakade] [30 01 2023]
                        //            {
                        //                Tempexpected = TimeSpan.FromMinutes(Convert.ToDouble(0.0));
                        //                if (DataTableForGridLayout.Columns.Contains(expected)) // [Rupali Sarode][GEOS2-4347][05-05-2023]
                        //                {
                        //                    dr[expected] = Tempexpected;
                        //                }
                        //            }

                        //            TempTotalExpected += Tempexpected;// Convert.ToDouble(item1.Expected);

                        //            string remaining = "Remaining_" + Convert.ToString(TimeTrackingList[i].TimeTrackingStage[iItem].IdStage);

                        //            string tempcolor = "Tempcolor_" + Convert.ToString(TimeTrackingList[i].TimeTrackingStage[iItem].IdStage);
                        //            //if (Convert.ToDouble(TimeTrackingList[i].TimeTrackingStage[iItem].Real) <= Convert.ToDouble(TimeTrackingList[i].TimeTrackingStage[iItem].Expected))
                        //            if (Tempreal <= Tempexpected)
                        //            {
                        //                //Remainingtimecolor = true;
                        //                //TimeTrackingList[i].Tempcolor = false;
                        //                if (DataTableForGridLayout.Columns.Contains(tempcolor)) // [Rupali Sarode][GEOS2-4347][05-05-2023]
                        //                {
                        //                    dr[tempcolor] = true;
                        //                }

                        //                if (DataTableForGridLayout.Columns.Contains(remaining)) // [Rupali Sarode][GEOS2-4347][05-05-2023]
                        //                {
                        //                    dr[remaining] = (Tempexpected - Tempreal);
                        //                }
                        //                TempTotalRemaianing += (Tempexpected - Tempreal);

                        //            }
                        //            else
                        //            {
                        //                //TimeTrackingList[i].Tempcolor = true;
                        //                if (DataTableForGridLayout.Columns.Contains(tempcolor)) // [Rupali Sarode][GEOS2-4347][05-05-2023]
                        //                {
                        //                    dr[tempcolor] = false;
                        //                }
                        //                if (DataTableForGridLayout.Columns.Contains(remaining)) // [Rupali Sarode][GEOS2-4347][05-05-2023]
                        //                {
                        //                    dr[remaining] = (Tempexpected - Tempreal);
                        //                }
                        //                TempTotalRemaianing += (Tempexpected - Tempreal);
                        //            }
                        //        }
                        //        catch (Exception ex)
                        //        {
                        //        }
                        //    }
                        //}
                        //if (TempTotalReal != null)
                        //{
                        //    dr["Real"] = TempTotalReal;

                        //}
                        //if (TempTotalExpected != null)
                        //{
                        //    dr["Expected"] = TempTotalExpected;

                        //}
                        //if (TempTotalRemaianing != null)
                        //{
                        //    if (TempTotalReal <= TempTotalExpected)
                        //    {

                        //        dr["Tempcolor"] = true;
                        //        dr["Remaining"] = TempTotalRemaianing;
                        //    }
                        //    else
                        //    {
                        //        dr["Tempcolor"] = false;
                        //        dr["Remaining"] = TempTotalRemaianing;
                        //    }
                        //}
                        #endregion
                    }
                    //[GEOS2-4150] [Gulab Lakade] [30 01 2023]
                    if (TimeTrackingList[i].ExpectedHtmlColorFlag == true)
                    {
                        #region [GEOS2-8868][gulab lakade][10 07 2025]

                        if (!DataTableForGridLayout.Columns.Contains("ExpectedHtmlColorFlag"))
                            DataTableForGridLayout.Columns.Add("ExpectedHtmlColorFlag", typeof(string));
                        if (!DataTableForGridLayout.Columns.Contains("POWSHtmlColorFlag"))
                            DataTableForGridLayout.Columns.Add("POWSHtmlColorFlag", typeof(string));
                        if (!DataTableForGridLayout.Columns.Contains("ProductionHtmlColorFlag"))
                            DataTableForGridLayout.Columns.Add("ProductionHtmlColorFlag", typeof(string));
                        if (!DataTableForGridLayout.Columns.Contains("ROWSHtmlColorFlag"))
                            DataTableForGridLayout.Columns.Add("ROWSHtmlColorFlag", typeof(string));
                        if (!DataTableForGridLayout.Columns.Contains("ReworkHtmlColorFlag"))
                            DataTableForGridLayout.Columns.Add("ReworkHtmlColorFlag", typeof(string));
                        if (!DataTableForGridLayout.Columns.Contains("RealHtmlColorFlag"))
                            DataTableForGridLayout.Columns.Add("RealHtmlColorFlag", typeof(string));
                        if (!DataTableForGridLayout.Columns.Contains("RemainingHtmlColorFlag1"))
                            DataTableForGridLayout.Columns.Add("RemainingHtmlColorFlag1", typeof(string));
                        if (!DataTableForGridLayout.Columns.Contains("RemainingHtmlColorFlag2"))
                            DataTableForGridLayout.Columns.Add("RemainingHtmlColorFlag2", typeof(string));
                        if (!DataTableForGridLayout.Columns.Contains("RemainingHtmlColorFlag6"))
                            DataTableForGridLayout.Columns.Add("RemainingHtmlColorFlag6", typeof(string));
                        #endregion
                        #region [rajashri.telvekar] [07 11 2025] [GEOS2-8309]
                        if (!DataTableForGridLayout.Columns.Contains("ExpectedHtmlColorFlag1"))
                            DataTableForGridLayout.Columns.Add("ExpectedHtmlColorFlag1", typeof(string));
                        if (!DataTableForGridLayout.Columns.Contains("POWSHtmlColorFlag1"))
                            DataTableForGridLayout.Columns.Add("POWSHtmlColorFlag1", typeof(string));
                        if (!DataTableForGridLayout.Columns.Contains("ProductionHtmlColorFlag1"))
                            DataTableForGridLayout.Columns.Add("ProductionHtmlColorFlag1", typeof(string));
                        if (!DataTableForGridLayout.Columns.Contains("ROWSHtmlColorFlag1"))
                            DataTableForGridLayout.Columns.Add("ROWSHtmlColorFlag1", typeof(string));
                        if (!DataTableForGridLayout.Columns.Contains("ReworkHtmlColorFlag1"))
                            DataTableForGridLayout.Columns.Add("ReworkHtmlColorFlag1", typeof(string));
                        if (!DataTableForGridLayout.Columns.Contains("RealHtmlColorFlag1"))
                            DataTableForGridLayout.Columns.Add("RealHtmlColorFlag1", typeof(string));
                        #endregion
                        #region GEOS2-8309 rajashri 17.10.2025
                        if (!DataTableForGridLayout.Columns.Contains("DownloadHtmlColorFlag"))
                            DataTableForGridLayout.Columns.Add("DownloadHtmlColorFlag", typeof(string));
                        if (!DataTableForGridLayout.Columns.Contains("TransfferdHtmlColorFlag"))
                            DataTableForGridLayout.Columns.Add("TransfferdHtmlColorFlag", typeof(string));
                        if (!DataTableForGridLayout.Columns.Contains("AddinHtmlColorFlag"))
                            DataTableForGridLayout.Columns.Add("AddinHtmlColorFlag", typeof(string));
                        if (!DataTableForGridLayout.Columns.Contains("PostServerHtmlColorFlag"))
                            DataTableForGridLayout.Columns.Add("PostServerHtmlColorFlag", typeof(string));
                        if (!DataTableForGridLayout.Columns.Contains("EDSHtmlColorFlag"))
                            DataTableForGridLayout.Columns.Add("EDSHtmlColorFlag", typeof(string));

                        #endregion
                        dr["ExpectedHtmlColorFlag"] = "#808080";
                        #region[rani dhamankar] [20 - 02 - 2025][GEOS2 - 6685]
                        dr["POWSHtmlColorFlag"] = "#808080";
                        dr["ProductionHtmlColorFlag"] = "#808080";
                        dr["ROWSHtmlColorFlag"] = "#808080";
                        dr["ReworkHtmlColorFlag"] = "#808080";
                        dr["RealHtmlColorFlag"] = "#808080";
                        dr["RemainingHtmlColorFlag1"] = "#808080";
                        dr["RemainingHtmlColorFlag2"] = "#808080";
                        dr["RemainingHtmlColorFlag6"] = "#808080";

                        #region [rajashri.telvekar] [07 11 2025] [GEOS2-8309]
                        dr["ExpectedHtmlColorFlag1"] = "#808080";
                        dr["POWSHtmlColorFlag1"] = "#808080";
                        dr["ProductionHtmlColorFlag1"] = "#808080";
                        dr["ROWSHtmlColorFlag1"] = "#808080";
                        dr["ReworkHtmlColorFlag1"] = "#808080";
                        dr["RealHtmlColorFlag1"] = "#808080";
                        #region GEOS2-8309 rajashri 
                        dr["DownloadHtmlColorFlag"] = "#808080";
                        dr["TransfferdHtmlColorFlag"] = "#808080";
                        dr["AddinHtmlColorFlag"] = "#808080";
                        dr["PostServerHtmlColorFlag"] = "#808080";
                        dr["EDSHtmlColorFlag"] = "#808080";
                        #endregion
                        #endregion

                        #endregion
                    }
                    if (TimeTrackingList[i].ProductionHtmlColorFlag == true)
                    {
                        dr["ProductionHtmlColorFlag"] = "#808080";
                    }
                    //if (TimeTrackingList[i].POWSHtmlColorFlag == true)
                    //{
                    //    dr["POWSHtmlColorFlag"] = "#808080";
                    //}

                    DataTableForGridLayout.Rows.Add(dr);
                    rowCounter += 1;

                }
                #region GEOS2[8309][7/11/2025 ] [pallavi.jadhav][rajashri.telvekar]
                // Allowed Expected columns
                var expectedColumns = DataTableForGridLayout.Columns.Cast<DataColumn>()
                    .Select(c => c.ColumnName)
                    .Where(c =>
                        c.StartsWith("Expected_", StringComparison.OrdinalIgnoreCase) &&
                        (c.EndsWith("_1") || c.EndsWith("_2") || c.EndsWith("_6")))
                    .ToList();

                foreach (DataRow row in DataTableForGridLayout.Rows)
                {
                    string serialNumber = row["SerialNumber"].ToString();
                    string itemNumber = row["ItemNumber"].ToString();
                    string otCode = row["OTCode"].ToString();
                    string idDrawing = row["IdDrawing"].ToString();


                    // Loop only Expected_1, Expected_2, Expected_6
                    foreach (string expectedColumn in expectedColumns)
                    {
                        if (string.IsNullOrEmpty(expectedColumn))
                            continue;

                        TimeSpan expectedTime = TimeSpan.Zero;
                        if (row[expectedColumn] != DBNull.Value)
                            TimeSpan.TryParse(row[expectedColumn].ToString(), out expectedTime);

                        int expectedIndex = 0;
                        if (expectedColumn.Contains("_"))
                            int.TryParse(expectedColumn.Split('_')[1], out expectedIndex);

                        if (expectedIndex != 1)
                        {
                            // ✅ Logic for Expected_2 and Expected_6
                            var sameOtSameDrawingList = DataTableForGridLayout.AsEnumerable()
                                .Where(r => r["OTCode"].ToString() == otCode &&
                                            r["IdDrawing"].ToString() == idDrawing)
                                .OrderBy(r => Convert.ToInt64(r["IdCounterpart"]))
                                .ToList();

                            var sameOtDiffDrawingList = DataTableForGridLayout.AsEnumerable()
                                .Where(r => r["OTCode"].ToString() == otCode &&
                                            r["IdDrawing"].ToString() != idDrawing)
                                .OrderBy(r => Convert.ToInt64(r["IdCounterpart"]))
                                .ToList();

                            if (sameOtSameDrawingList.Count > 1)
                            {
                                var firstRow = sameOtSameDrawingList
                                    .FirstOrDefault(r => r["SerialNumber"].ToString().EndsWith("001"))
                                    ?? sameOtSameDrawingList.First();

                                if (firstRow != null && serialNumber == firstRow["SerialNumber"].ToString())
                                {
                                    row[expectedColumn] = expectedTime;

                                    row["ExpectedHtmlColorFlag"] = "";
                                    row["POWSHtmlColorFlag"] = "";
                                    row["ProductionHtmlColorFlag"] = "";
                                    row["ROWSHtmlColorFlag"] = "";
                                    row["ReworkHtmlColorFlag"] = "";
                                    row["RealHtmlColorFlag"] = "";
                                    row["RemainingHtmlColorFlag2"] = "";
                                    row["RemainingHtmlColorFlag6"] = "";
                                    row["DownloadHtmlColorFlag"] = "";
                                    row["TransfferdHtmlColorFlag"] = "";
                                    row["AddinHtmlColorFlag"] = "";
                                    row["PostServerHtmlColorFlag"] = "";
                                    row["EDSHtmlColorFlag"] = "";
                                }
                                else
                                {
                                    row[expectedColumn] = TimeSpan.Zero;
                                    row["ExpectedHtmlColorFlag"] = "#808080";
                                    row["POWSHtmlColorFlag"] = "#808080";
                                    row["ProductionHtmlColorFlag"] = "#808080";
                                    row["ROWSHtmlColorFlag"] = "#808080";
                                    row["ReworkHtmlColorFlag"] = "#808080";
                                    row["RealHtmlColorFlag"] = "#808080";
                                    row["RemainingHtmlColorFlag2"] = "#808080";
                                    row["RemainingHtmlColorFlag6"] = "#808080";
                                    row["DownloadHtmlColorFlag"] = "#808080";
                                    row["TransfferdHtmlColorFlag"] = "#808080";
                                    row["AddinHtmlColorFlag"] = "#808080";
                                    row["PostServerHtmlColorFlag"] = "#808080";
                                    row["EDSHtmlColorFlag"] = "#808080";
                                }
                            }
                            else if (sameOtDiffDrawingList.Any())
                            {
                                row[expectedColumn] = expectedTime;
                                row["ExpectedHtmlColorFlag"] = "";
                                row["POWSHtmlColorFlag"] = "";
                                row["ProductionHtmlColorFlag"] = "";
                                row["ROWSHtmlColorFlag"] = "";
                                row["ReworkHtmlColorFlag"] = "";
                                row["RealHtmlColorFlag"] = "";
                                row["RemainingHtmlColorFlag1"] = "";
                                row["RemainingHtmlColorFlag2"] = "";
                                row["RemainingHtmlColorFlag6"] = "";
                                row["DownloadHtmlColorFlag"] = "";
                                row["TransfferdHtmlColorFlag"] = "";
                                row["AddinHtmlColorFlag"] = "";
                                row["PostServerHtmlColorFlag"] = "";
                                row["EDSHtmlColorFlag"] = "";
                            }
                            else
                            {
                                row[expectedColumn] = expectedTime;
                                row["ExpectedHtmlColorFlag"] = "";
                                row["POWSHtmlColorFlag"] = "";
                                row["ProductionHtmlColorFlag"] = "";
                                row["ROWSHtmlColorFlag"] = "";
                                row["ReworkHtmlColorFlag"] = "";
                                row["RealHtmlColorFlag"] = "";
                                row["RemainingHtmlColorFlag1"] = "";
                                row["RemainingHtmlColorFlag2"] = "";
                                row["RemainingHtmlColorFlag6"] = "";
                                row["DownloadHtmlColorFlag"] = "";
                                row["TransfferdHtmlColorFlag"] = "";
                                row["AddinHtmlColorFlag"] = "";
                                row["PostServerHtmlColorFlag"] = "";
                                row["EDSHtmlColorFlag"] = "";
                            }
                        }
                        else
                        {
                            // ✅ Logic for Expected_1

                            if (expectedIndex == 1)
                            {
                                if (serialNumber.EndsWith("001"))
                                {
                                    row[expectedColumn] = expectedTime;
                                    row["ExpectedHtmlColorFlag1"] = "";
                                    row["POWSHtmlColorFlag1"] = "";
                                    row["ProductionHtmlColorFlag1"] = "";
                                    row["ROWSHtmlColorFlag1"] = "";
                                    row["ReworkHtmlColorFlag1"] = "";
                                    row["RealHtmlColorFlag1"] = "";
                                    row["RemainingHtmlColorFlag1"] = "";
                                 
                                }
                                else
                                {
                                    row[expectedColumn] = TimeSpan.Zero;
                                    row["ExpectedHtmlColorFlag1"] = "#808080";
                                    row["POWSHtmlColorFlag1"] = "#808080";
                                    row["ProductionHtmlColorFlag1"] = "#808080";
                                    row["ROWSHtmlColorFlag1"] = "#808080";
                                    row["ReworkHtmlColorFlag1"] = "#808080";
                                    row["RealHtmlColorFlag1"] = "#808080";
                                    row["RemainingHtmlColorFlag1"] = "#808080";
                                   

                                }
                            }
                            else
                            {
                                row[expectedColumn] = expectedTime;
                                row["ExpectedHtmlColorFlag"] = "";
                            }
                        }
                    }
                }
                #endregion

                DtTimetracking = DataTableForGridLayout;

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                //GeosApplication.Instance.Logger.Log("Method FillDashboard()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                //GeosApplication.Instance.Logger.Log(string.Format("Error in method FillDashboard() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

            }
        }

        public TimeSpan ConvertfloattoTimespan(string observedtime)
        {
            TimeSpan UITempobservedTime;
            try
            {
                #region GEOS2-3954 Time format HH:MM:SS
                var currentculter = CultureInfo.CurrentCulture;
                string culterseparator = currentculter.NumberFormat.NumberDecimalSeparator.ToString();
                string tempd = Convert.ToString(observedtime);
                string[] parts = new string[2];
                int i1 = 0;
                int i2 = 0;
                if (tempd.Contains(culterseparator))
                {
                    parts = tempd.Split(Convert.ToChar(culterseparator));
                    i1 = int.Parse(parts[0]);
                    i2 = int.Parse(parts[1]);

                    if (Convert.ToString(parts[1]).Length == 1)
                    {
                        i1 = (i1 * 60) + i2 * 10;
                    }
                    else
                    {
                        i1 = (i1 * 60) + i2;
                    }
                    UITempobservedTime = TimeSpan.FromSeconds(i1);
                    int ts1 = UITempobservedTime.Hours;
                    int ts2 = UITempobservedTime.Minutes;
                    int ts3 = UITempobservedTime.Seconds;
                }
                else
                {
                    //parts = tempd.Split(Convert.ToChar(culterseparator));
                    //i1 = int.Parse(parts[0]);
                    //i1 = (i1 * 60);

                    UITempobservedTime = TimeSpan.FromSeconds(Convert.ToInt64(tempd) * 60);  //GEOS2-4045 Gulab Lakade time coversio issue
                    int ts1 = UITempobservedTime.Hours;
                    int ts2 = UITempobservedTime.Minutes;
                    int ts3 = UITempobservedTime.Seconds;
                }

                #endregion
                return UITempobservedTime;
            }
            catch (Exception ex)
            {
                UITempobservedTime = TimeSpan.FromSeconds(0);
                return UITempobservedTime;
            }

        }


        public String ConvertTimespantoFloat(String observedtime)
        {

            var currentculter = CultureInfo.CurrentCulture;
            string culterseparator = currentculter.NumberFormat.NumberDecimalSeparator.ToString();


            string[] NormaltimeArr = new string[2];
            int nt1 = 0;
            int nt2 = 0;
            int nt3 = 0;
            if (observedtime.Contains(":"))
            {
                NormaltimeArr = observedtime.Split(':');
                nt1 = int.Parse(NormaltimeArr[0]);
                nt2 = int.Parse(NormaltimeArr[1]);
                nt3 = int.Parse(NormaltimeArr[2]);
                nt1 = (nt1 * 60) + nt2;
            }
            string tempstring = string.Empty;
            if (Convert.ToString(nt3).Length == 1)
            {
                tempstring = Convert.ToString(nt1) + culterseparator + "0" + Convert.ToString(nt3);
            }
            else
            {
                tempstring = Convert.ToString(nt1) + culterseparator + Convert.ToString(nt3);
            }

            return tempstring;
        }

        private void CustomSummaryCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CustomSummaryCommandAction()...", category: Category.Info, priority: Priority.Low);


                TreeListCustomSummaryEventArgs e = (TreeListCustomSummaryEventArgs)obj;
                if (e.SummaryItem.FieldName == "Expected_COM")
                {
                    if (e.SummaryProcess == CustomSummaryProcess.Calculate)
                    {
                        if (e.FieldValue != null && e.Node != null)
                        {
                            e.TotalValue = Convert.ToDouble(e.TotalValue) + Convert.ToDouble(e.FieldValue);
                            if (e.TotalValue != null)
                                e.TotalValue = Math.Round(Convert.ToDouble(e.TotalValue), 2);
                            //ERMSOPModule temp = (ERMSOPModule)(e.Node.Content);
                            //if (GetStages.Any(a => a != null && a.Code.Trim().Equals(temp.Name.Trim())))
                            //{
                            //    e.TotalValue = Convert.ToDouble(e.TotalValue) + Convert.ToDouble(e.FieldValue);
                            //    if (e.TotalValue != null)

                            //}

                        }

                    }
                }
                GeosApplication.Instance.Logger.Log("Method CustomSummaryCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {

                GeosApplication.Instance.Logger.Log(string.Format("Error in method CustomSummaryCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

            }
        }

        #region Method
        private void AddColumnsToDataTableWithoutBands()
        {
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTableWithoutBands ...", category: Category.Info, priority: Priority.Low);
                Bands = new ObservableCollection<BandItem>(); Bands.Clear();
                BandItem band0 = new BandItem() { BandName = "FirstRow", AllowBandMove = false, FixedStyle = FixedStyle.Left, Visible = true, AllowMoving = DevExpress.Utils.DefaultBoolean.False };
                band0.Columns = new ObservableCollection<ColumnItem>();
                band0.Columns.Add(new ColumnItem() { ColumnFieldName = "DeliveryWeek", HeaderText = "Delivery Week", Width = 120, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DeliveryWeek, Visible = true });
                band0.Columns.Add(new ColumnItem() { ColumnFieldName = "DeliveryDate", HeaderText = "Delivery Date", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DeliveryDate, Visible = true });
                band0.Columns.Add(new ColumnItem() { ColumnFieldName = "PlannedDeliveryDate", HeaderText = "Planned Delivery Date", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PlannedDeliveryDate, Visible = true });



                Bands.Add(band0);
                DataTableForGridLayout = new DataTable();

                DataTableForGridLayout.Columns.Add("DeliveryWeek", typeof(string));
                DataTableForGridLayout.Columns.Add("DeliveryDate", typeof(DateTime));

                DataTableForGridLayout.Columns.Add("PlannedDeliveryDate", typeof(DateTime));
                DataTableForGridLayout.Columns.Add("DeliveryDateColor", typeof(bool));
                DataTableForGridLayout.Columns.Add("DeliveryDateHtmlColor", typeof(string));
                DataTableForGridLayout.Columns.Add("SamplesTooltip", typeof(string));
                DataTableForGridLayout.Columns.Add("DrawingTypeTooltip", typeof(string));
                DataTableForGridLayout.Columns.Add("PlannedDeliveryDateHtmlColor", typeof(string));
                BandItem band1 = new BandItem() { BandName = "all", Visible = true };
                band1.Columns = new ObservableCollection<ColumnItem>();

                //[Rupali Sarode][geso2-4173][10-03-2023]
                band1.Columns.Add(new ColumnItem() { ColumnFieldName = "FirstDeliveryDate", HeaderText = "First Delivery Date", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.FirstDeliveryDate, Visible = true });



                #region [GEOS2-4093][Rupali Sarode][26-12-2022]
                band1.Columns.Add(new ColumnItem() { ColumnFieldName = "QuoteSendDate", HeaderText = "Quote Send Date", Width = 90, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DatesTemplate, Visible = true });
                band1.Columns.Add(new ColumnItem() { ColumnFieldName = "GoAheadDate", HeaderText = "Go Ahead Date", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DatesTemplate, Visible = true });
                band1.Columns.Add(new ColumnItem() { ColumnFieldName = "PODate", HeaderText = "PO Date", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DatesTemplate, Visible = true });
                band1.Columns.Add(new ColumnItem() { ColumnFieldName = "SamplesTemplate", HeaderText = "Samples", Width = 80, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.SamplesTemplate, Visible = true });
                band1.Columns.Add(new ColumnItem() { ColumnFieldName = "SamplesDateTemplate", HeaderText = "Samples Date", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.SamplesDateTemplate, Visible = true });
                band1.Columns.Add(new ColumnItem() { ColumnFieldName = "AvailbleForDesignDate", HeaderText = "Available For Design Date", Width = 140, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DatesTemplate, Visible = true });
                #endregion

                band1.Columns.Add(new ColumnItem() { ColumnFieldName = "POType", HeaderText = "PO Type", Width = 80, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DefaultTemplate, Visible = true });
                band1.Columns.Add(new ColumnItem() { ColumnFieldName = "Customer", HeaderText = "Customer", Width = 130, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DefaultTemplate, Visible = true });
                band1.Columns.Add(new ColumnItem() { ColumnFieldName = "Project", HeaderText = "Project", Width = 100, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DefaultTemplate, Visible = true });
                band1.Columns.Add(new ColumnItem() { ColumnFieldName = "Offer", HeaderText = "Offer", Width = 90, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DefaultTemplate, Visible = true });
                band1.Columns.Add(new ColumnItem() { ColumnFieldName = "OTCode", HeaderText = "OT Code", Width = 130, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.OTCode, Visible = true });
                //[GEOS2-3972] [Rupali Sarode] [31/10/2022]
                //band1.Columns.Add(new ColumnItem() { ColumnFieldName = "OTNumber", HeaderText = "OT Number", Width = 80, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.OTNumber, Visible = true });  //[Rupali Sarode] To remove OTNumber column
                band1.Columns.Add(new ColumnItem() { ColumnFieldName = "OriginPlant", HeaderText = "Origin Plant", Width = 100, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Plant, Visible = true });
                band1.Columns.Add(new ColumnItem() { ColumnFieldName = "ProductionPlant", HeaderText = "Production Plant", Width = 120, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Plant, Visible = true });
                band1.Columns.Add(new ColumnItem() { ColumnFieldName = "Reference", HeaderText = "Customer Ref.", Width = 120, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Reference, Visible = true });
                band1.Columns.Add(new ColumnItem() { ColumnFieldName = "ReferenceTemplate", HeaderText = "Template", Width = 100, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.ReferenceTemplate, Visible = true });
                band1.Columns.Add(new ColumnItem() { ColumnFieldName = "Type", HeaderText = "Type", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Type, Visible = true });
                band1.Columns.Add(new ColumnItem() { ColumnFieldName = "QTY", HeaderText = "QTY", Width = 55, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.QTY, Visible = true });
                // Bands[0].Bands.Add(band1);
                Bands.Add(band1);

                #region [GEOS2-4093][Rupali Sarode][26-12-2022]
                DataTableForGridLayout.Columns.Add("FirstDeliveryDate", typeof(DateTime));
                DataTableForGridLayout.Columns.Add("QuoteSendDate", typeof(DateTime));
                DataTableForGridLayout.Columns.Add("GoAheadDate", typeof(DateTime));
                DataTableForGridLayout.Columns.Add("PODate", typeof(DateTime));
                DataTableForGridLayout.Columns.Add("SamplesTemplate", typeof(string));
                DataTableForGridLayout.Columns.Add("SamplesColor", typeof(string));
                DataTableForGridLayout.Columns.Add("SamplesDateTemplate", typeof(DateTime));
                DataTableForGridLayout.Columns.Add("AvailbleForDesignDate", typeof(DateTime));
                #endregion

                DataTableForGridLayout.Columns.Add("POType", typeof(string));
                DataTableForGridLayout.Columns.Add("Customer", typeof(string));
                DataTableForGridLayout.Columns.Add("Project", typeof(string));
                DataTableForGridLayout.Columns.Add("Offer", typeof(string));
                DataTableForGridLayout.Columns.Add("OTCode", typeof(string));
                //[GEOS2-3972] [Rupali Sarode] [31/10/2022]
                //DataTableForGridLayout.Columns.Add("OTNumber", typeof(string));
                DataTableForGridLayout.Columns.Add("OriginPlant", typeof(string));
                DataTableForGridLayout.Columns.Add("ProductionPlant", typeof(string));
                DataTableForGridLayout.Columns.Add("Reference", typeof(string));
                DataTableForGridLayout.Columns.Add("ReferenceTemplate", typeof(string));
                DataTableForGridLayout.Columns.Add("Type", typeof(string));
                DataTableForGridLayout.Columns.Add("QTY", typeof(string));

                //SalePrice
                BandItem band2 = new BandItem() { BandName = "SalePrice", BandHeader = "Sale Price", Visible = true };
                band2.Columns = new ObservableCollection<ColumnItem>();

                string fieldName3 = "UnitPrice";
                DataTableForGridLayout.Columns.Add(fieldName3, typeof(decimal));
                #region  [GEOS2-4059][Rupali Sarode][06-12-2022]  Header as per selected currency
                Currency CurrentSelectedCurrency;
                List<Currency> Currencies;
                string CurrencySymbol = string.Empty;
                try
                {
                    if (GeosApplication.Instance.UserSettings.ContainsKey("ERM_SelectedCurrency"))
                    {
                        Currencies = GeosApplication.Instance.Currencies.ToList();
                        if (GeosApplication.Instance.UserSettings["ERM_SelectedCurrency"] != null)
                        {
                            CurrentSelectedCurrency = Currencies.Where(x => x.Name == GeosApplication.Instance.UserSettings["ERM_SelectedCurrency"]).FirstOrDefault();
                            if (CurrentSelectedCurrency != null)
                            {
                                CurrencySymbol = CurrentSelectedCurrency.Symbol;
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Error in AddColumnsToDataTableWithoutBands() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                }
                //band2.Columns.Add(new ColumnItem() { ColumnFieldName = fieldName3, HeaderText = "Unit Price(€)", Width = 90, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.SalePrice });
                band2.Columns.Add(new ColumnItem() { ColumnFieldName = fieldName3, HeaderText = "Unit Price(" + Convert.ToString(CurrencySymbol) + ")", Width = 90, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.SalePrice });
                #endregion

                string fieldName4 = "TotalPrice";
                DataTableForGridLayout.Columns.Add(fieldName4, typeof(decimal));
                //[GEOS2-4059][Rupali Sarode][06-12-2022]  Header as per selected currency
                //band2.Columns.Add(new ColumnItem() { ColumnFieldName = fieldName4, HeaderText = "Total Price(€)", Width = 90, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.SalePrice });
                band2.Columns.Add(new ColumnItem() { ColumnFieldName = fieldName4, HeaderText = "Total Price(" + Convert.ToString(CurrencySymbol) + ")", Width = 90, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.SalePrice });

                #region //[Pallavi jadhav][GEOS2-5910][17-07-2024]
                if (GeosApplication.Instance.IsViewSupervisorERM)
                {

                }
                else
                {
                    Bands.Add(band2);
                }
                #endregion

                //ItemInformation
                BandItem band3 = new BandItem() { BandName = "ItemInformation", BandHeader = "Item Information", Visible = true };
                band3.Columns = new ObservableCollection<ColumnItem>();

                //[Rupali Sarode][04-04-2024][GEOS2-5577]
                string fieldIdDrawing = "IdDrawing";
                DataTableForGridLayout.Columns.Add(fieldIdDrawing, typeof(string));
                band3.Columns.Add(new ColumnItem() { ColumnFieldName = fieldIdDrawing, HeaderText = "Id Drawing", Width = 130, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.IdDrawing });

                //[Aishwarya Ingale][09-08-2024][GEOS2-6034]
                string fieldworkbook_drawing = "workbook_drawing";
                DataTableForGridLayout.Columns.Add(fieldworkbook_drawing, typeof(string));
                band3.Columns.Add(new ColumnItem() { ColumnFieldName = fieldworkbook_drawing, HeaderText = "workbook_drawing", Width = 130, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.workbook_drawing });


                string fieldName5 = "SerialNumber";
                DataTableForGridLayout.Columns.Add(fieldName5, typeof(string));
                band3.Columns.Add(new ColumnItem() { ColumnFieldName = fieldName5, HeaderText = "Serial Number", Width = 130, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.ItemInformation });

                DataTableForGridLayout.Columns.Add("ItemNumber", typeof(string));
                band3.Columns.Add(new ColumnItem() { ColumnFieldName = "ItemNumber", HeaderText = "Item Number", Width = 110, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.TotalRework });

                string fieldName6 = "ItemStatus";
                DataTableForGridLayout.Columns.Add(fieldName6, typeof(string));
                band3.Columns.Add(new ColumnItem() { ColumnFieldName = fieldName6, HeaderText = "Item Status", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.ItemStatus });
                #region [Gulab Lakade][geso2-4173][02-03 -2023]
                DataTableForGridLayout.Columns.Add("DrawingType", typeof(string));
                band3.Columns.Add(new ColumnItem() { ColumnFieldName = "DrawingType", HeaderText = "Design Type", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DrawingType });
                DataTableForGridLayout.Columns.Add("DrawingTypeForColor", typeof(string));
                DataTableForGridLayout.Columns.Add("TrayName", typeof(string));
                band3.Columns.Add(new ColumnItem() { ColumnFieldName = "TrayName", HeaderText = "Tray", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.TrayName });
                DataTableForGridLayout.Columns.Add("TrayColor", typeof(string));
                #endregion
                DataTableForGridLayout.Columns.Add("HtmlColor", typeof(string));
                //band3.Columns.Add(new ColumnItem() { ColumnFieldName = "HtmlColor", HeaderText = "HtmlColor", Width = 10, Visible = false, IsVertical = false, });


                string fieldName7 = "CurrentWorkStation";
                DataTableForGridLayout.Columns.Add(fieldName7, typeof(string));
                band3.Columns.Add(new ColumnItem() { ColumnFieldName = fieldName7, HeaderText = "Current WorkStation", Width = 145, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.ItemInformation });

                string fieldName23 = "DesignSystem";
                DataTableForGridLayout.Columns.Add(fieldName23, typeof(string));
                band3.Columns.Add(new ColumnItem() { ColumnFieldName = fieldName23, HeaderText = "Design System", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DesignSystem });

                string fieldName24 = "Designer";
                DataTableForGridLayout.Columns.Add(fieldName24, typeof(string));
                band3.Columns.Add(new ColumnItem() { ColumnFieldName = fieldName24, HeaderText = "Designer", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Designer });

                string fieldName25 = "StartRev";
                DataTableForGridLayout.Columns.Add(fieldName25, typeof(string));
                band3.Columns.Add(new ColumnItem() { ColumnFieldName = fieldName25, HeaderText = "StartRev.", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.StartRev });

                string fieldName26 = "LastRev";
                DataTableForGridLayout.Columns.Add(fieldName26, typeof(string));
                band3.Columns.Add(new ColumnItem() { ColumnFieldName = fieldName26, HeaderText = "LastRev.", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.LastRev });

                string fieldName8 = "TRework";
                DataTableForGridLayout.Columns.Add(fieldName8, typeof(int));
                band3.Columns.Add(new ColumnItem() { ColumnFieldName = fieldName8, HeaderText = "T. Reworks", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.TotalRework });

                Bands.Add(band3);
                TotalSummary = new ObservableCollection<Summary>()
                {
                    new Summary() { Type = SummaryItemType.Count, FieldName="DeliveryWeek",  DisplayFormat = "Count: {0}" }

                };
                List<string> tempCurrentstage = new List<string>();
                // var tempTimeTrackingList = TimeTrackingList.Select(x => x.CurrentWorkStation).Distinct().ToList();

                string[] ArrActiveInPlants;
                bool FlagPresentInActivePlants = false;
                // int[] a;


                if (TimeTrackingProductionStage.Count > 0)
                {
                    foreach (var item in TimeTrackingProductionStage)
                    {

                        #region [Rupali Sarode][GEOS2-4347][05-05-2023] -- Show stage only if present in ActiveInPlants


                        if (item.ActiveInPlants != null && item.ActiveInPlants != "")
                        {
                            ArrActiveInPlants = item.ActiveInPlants.Split(',');
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
                        #endregion

                        #region Aishwarya[Geos2-6786]
                        List<Site> plantOwners = ERMCommon.Instance.SelectedAuthorizedPlantsList.Cast<Site>().ToList();
                        string ActivePlantString = ActivePlantList.Select(a => a.DefaultValue).FirstOrDefault();
                        var activePlantIds = ActivePlantString?.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(id => Convert.ToUInt32(id)).ToList();
                        var plantOwnerNames = plantOwners.Select(plantOwner => plantOwner.IdSite).ToList();
                        bool isMatch = activePlantIds != null && activePlantIds.Any(id => plantOwnerNames.Contains(id));
                        #endregion


                        if (FlagPresentInActivePlants == true || item.ActiveInPlants == null || item.ActiveInPlants == "")
                        {
                            if (Convert.ToString(item) != null)
                            {

                                tempCurrentstage.Add(Convert.ToString(item));
                                BandItem band4 = new BandItem()
                                {
                                    BandName = Convert.ToString(item.StageCode),
                                    BandHeader = Convert.ToString(item.StageCode),
                                    Visible = true
                                };

                                band4.Columns = new ObservableCollection<ColumnItem>();

                                switch (item.IdStage)
                                {

                                    case 1:
                                        #region [GEOS2-5320][pallavi jadhav][15 10 2024]
                                        #region [Geos2-6786][gulab lakade][30 12 2024]
                                        if (isMatch == false && IscheckplantInSetting == true)
                                        {
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDateHtmlColor_" + Convert.ToString(item.IdStage), typeof(string));
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDate_" + Convert.ToString(item.IdStage), typeof(DateTime));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "PlannedDeliveryDate_" + Convert.ToString(item.IdStage), HeaderText = "Planned Delivery Date", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PlannedDeliveryDate1, Visible = false });
                                            DataTableForGridLayout.Columns.Add("Days_" + Convert.ToString(item.IdStage), typeof(int));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Days_" + Convert.ToString(item.IdStage), HeaderText = "Days", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Days, Visible = false });

                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDateHtmlColor_" + Convert.ToString(item.IdStage), typeof(string));
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDate_" + Convert.ToString(item.IdStage), typeof(DateTime));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "PlannedDeliveryDate_" + Convert.ToString(item.IdStage), HeaderText = "Planned Delivery Date", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PlannedDeliveryDate1, Visible = true });
                                            DataTableForGridLayout.Columns.Add("Days_" + Convert.ToString(item.IdStage), typeof(int));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Days_" + Convert.ToString(item.IdStage), HeaderText = "Days", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Days, Visible = true });

                                        }


                                        #endregion
                                        #endregion
                                        #region [GEOS2-4252] [gulab lakade][06 03 2023]
                                        if (AppSettingData.Contains(Convert.ToInt32(item.IdStage)))
                                        {
                                            if (Convert.ToInt32(item.IdStage) == 1)
                                            {
                                                DataTableForGridLayout.Columns.Add("Expected_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                                band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Expected_" + Convert.ToString(item.IdStage), HeaderText = "Expected", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.ExpectedTime1 });

                                            }
                                            else
                                            {
                                                DataTableForGridLayout.Columns.Add("Expected_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                                band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Expected_" + Convert.ToString(item.IdStage), HeaderText = "Expected", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.ExpectedTime });
                                            }
                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("Expected_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Expected_" + Convert.ToString(item.IdStage), HeaderText = "Expected", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProductionExpected });
                                        }
                                        #endregion
                                        #region [gulab lakade][11 03 2024][GEOS2-5466]
                                        //rajashri[GEOS2-5799]
                                        if (AppSettingData.Contains(Convert.ToInt32(item.IdStage)))
                                        {
                                            if (Convert.ToInt32(item.IdStage) == 1)
                                            {
                                                DataTableForGridLayout.Columns.Add("Production_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                                band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Production_" + Convert.ToString(item.IdStage), HeaderText = "Production", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Production1 });
                                            }
                                            else
                                            {
                                                DataTableForGridLayout.Columns.Add("Production_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Production_" + Convert.ToString(item.IdStage), HeaderText = "Production", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Production });
                                            }
                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("Production_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Production_" + Convert.ToString(item.IdStage), HeaderText = "Production", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProductionWithoutCOMCADCAM });
                                        }
                                        //DataTableForGridLayout.Columns.Add("POWS_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "POWS_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Production-OWS", Width = 116, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PrOWS });

                                        #region [rani dhamankar][20-02-2025][GEOS2-6685]
                                        if (AppSettingData.Contains(Convert.ToInt32(item.IdStage)))
                                        {
                                            if (Convert.ToInt32(item.IdStage) == 1)
                                            {
                                                DataTableForGridLayout.Columns.Add("POWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                                band4.Columns.Add(new ColumnItem() { ColumnFieldName = "POWS_" + Convert.ToString(item.IdStage), HeaderText = "Production-OWS", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PrOWS1 });

                                                DataTableForGridLayout.Columns.Add("Rework_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                                band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Rework_" + Convert.ToString(item.IdStage), HeaderText = "Rework", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Rework1 });

                                                DataTableForGridLayout.Columns.Add("ROWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                                band4.Columns.Add(new ColumnItem() { ColumnFieldName = "ROWS_" + Convert.ToString(item.IdStage), HeaderText = "Rework-OWS", Width = 95, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.ReOWS1 });

                                                #endregion
                                                //string fieldName10 = "Real_" + Convert.ToString(item.IdStage);
                                                DataTableForGridLayout.Columns.Add("Real_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                                band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Real_" + Convert.ToString(item.IdStage), HeaderText = "Real", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProduction1 });

                                            }
                                            else
                                            {
                                                DataTableForGridLayout.Columns.Add("POWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "POWS_" + Convert.ToString(item.IdStage), HeaderText = "Production-OWS", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PrOWS });

                                            DataTableForGridLayout.Columns.Add("Rework_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Rework_" + Convert.ToString(item.IdStage), HeaderText = "Rework", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Rework });

                                            DataTableForGridLayout.Columns.Add("ROWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "ROWS_" + Convert.ToString(item.IdStage), HeaderText = "Rework-OWS", Width = 95, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.ReOWS });

                                            #endregion
                                            //string fieldName10 = "Real_" + Convert.ToString(timeTrackingStage.IdStage);
                                            DataTableForGridLayout.Columns.Add("Real_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Real_" + Convert.ToString(item.IdStage), HeaderText = "Real", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProduction });
                                        }
                                            //DataTableForGridLayout.Columns.Add("Remaining_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                            //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Remaining_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Remaining", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.RemainingTime1 });
                                            //DataTableForGridLayout.Columns.Add("Tempcolor_" + Convert.ToString(timeTrackingStage.IdStage), typeof(bool));

                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("POWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "POWS_" + Convert.ToString(item.IdStage), HeaderText = "Production-OWS", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicPOWSWithoutCOMCADCAM });
                                            DataTableForGridLayout.Columns.Add("Rework_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Rework_" + Convert.ToString(item.IdStage), HeaderText = "Rework", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicReworkWithoutCOMCADCAM });

                                            DataTableForGridLayout.Columns.Add("ROWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "ROWS_" + Convert.ToString(item.IdStage), HeaderText = "Rework-OWS", Width = 95, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicROWSWithoutCOMCADCAM });

                                            #endregion
                                            //string fieldName10 = "Real_" + Convert.ToString(timeTrackingStage.IdStage);
                                            DataTableForGridLayout.Columns.Add("Real_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Real_" + Convert.ToString(item.IdStage), HeaderText = "Real", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicRealWithoutCOMCADCAM });

                                            //DataTableForGridLayout.Columns.Add("Remaining_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                            //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Remaining_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Remaining", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.RemainingTime1WithoutCOMCADCA });
                                            //DataTableForGridLayout.Columns.Add("Tempcolor_" + Convert.ToString(timeTrackingStage.IdStage), typeof(bool));


                                        }
                                        


                                        //DataTableForGridLayout.Columns.Add("Rework_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Rework_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Rework", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Rework });

                                        //DataTableForGridLayout.Columns.Add("ROWS_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "ROWS_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Rework-OWS", Width = 95, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.ReOWS });

                                        //#endregion
                                        ////string fieldName10 = "Real_" + Convert.ToString(timeTrackingStage.IdStage);
                                        //DataTableForGridLayout.Columns.Add("Real_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Real_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Real", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProduction });

                                        //string fieldName11 = "Remaining_" + Convert.ToString(timeTrackingStage.IdStage);
                                        DataTableForGridLayout.Columns.Add("Remaining_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                        band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Remaining_" + Convert.ToString(item.IdStage), HeaderText = "Remaining", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.RemainingTime1 });
                                        DataTableForGridLayout.Columns.Add("Tempcolor_" + Convert.ToString(item.IdStage), typeof(bool));


                                        break;

                                    case 2:
                                        #region [GEOS2-5320][pallavi jadhav][15 10 2024]
                                        #region [Geos2-6786][gulab lakade][30 12 2024]
                                        if (isMatch == false && IscheckplantInSetting == true)
                                        {
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDateHtmlColor_" + Convert.ToString(item.IdStage), typeof(string));
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDate_" + Convert.ToString(item.IdStage), typeof(DateTime));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "PlannedDeliveryDate_" + Convert.ToString(item.IdStage), HeaderText = "Planned Delivery Date", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PlannedDeliveryDate2, Visible = false });
                                            DataTableForGridLayout.Columns.Add("Days_" + Convert.ToString(item.IdStage), typeof(int));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Days_" + Convert.ToString(item.IdStage), HeaderText = "Days", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Days, Visible = false });

                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDateHtmlColor_" + Convert.ToString(item.IdStage), typeof(string));
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDate_" + Convert.ToString(item.IdStage), typeof(DateTime));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "PlannedDeliveryDate_" + Convert.ToString(item.IdStage), HeaderText = "Planned Delivery Date", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PlannedDeliveryDate2, Visible = true });
                                            DataTableForGridLayout.Columns.Add("Days_" + Convert.ToString(item.IdStage), typeof(int));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Days_" + Convert.ToString(item.IdStage), HeaderText = "Days", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Days, Visible = true });

                                        }
                                        #endregion
                                        #endregion
                                        #region [GEOS2-4252] [gulab lakade][06 03 2023]
                                        if (AppSettingData.Contains(Convert.ToInt32(item.IdStage)))
                                        {
                                            DataTableForGridLayout.Columns.Add("Expected_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Expected_" + Convert.ToString(item.IdStage), HeaderText = "Expected", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.ExpectedTime });
                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("Expected_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Expected_" + Convert.ToString(item.IdStage), HeaderText = "Expected", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProductionExpected });
                                        }
                                        #endregion
                                        
                                        #region [gulab lakade][11 03 2024][GEOS2-5466]
                                        //rajashri[GEOS2-5799]
                                        if (AppSettingData.Contains(Convert.ToInt32(item.IdStage)))
                                        {
                                            DataTableForGridLayout.Columns.Add("Production_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Production_" + Convert.ToString(item.IdStage), HeaderText = "D.Time", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Production });

                                            #region [GEOS2-7094] [dhawal bhalerao][18 04 2025]
                                            
                                            DataTableForGridLayout.Columns.Add("Download_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Download_" + Convert.ToString(item.IdStage), HeaderText = "Download", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Download });

                                            DataTableForGridLayout.Columns.Add("Transferred_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Transferred_" + Convert.ToString(item.IdStage), HeaderText = "Transferred", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Transferred });

                                            DataTableForGridLayout.Columns.Add("AddIn_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "AddIn_" + Convert.ToString(item.IdStage), HeaderText = "AddIn", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.AddIn });

                                            DataTableForGridLayout.Columns.Add("PostServer_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "PostServer_" + Convert.ToString(item.IdStage), HeaderText = "PostServer", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PostServer });

                                            DataTableForGridLayout.Columns.Add("EDS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "EDS_" + Convert.ToString(item.IdStage), HeaderText = "EDS", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.EDS });

                                            #endregion
                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("Production_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Production_" + Convert.ToString(item.IdStage), HeaderText = "Production", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProductionWithoutCOMCADCAM });
                                        }
                                      

                                        //DataTableForGridLayout.Columns.Add("POWS_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "POWS_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Production-OWS", Width = 116, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PrOWS });
                                        #region [rani dhamankar][20-02-2025][GEOS2-6685]
                                        if (AppSettingData.Contains(Convert.ToInt32(item.IdStage)))
                                        {
                                            DataTableForGridLayout.Columns.Add("POWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "POWS_" + Convert.ToString(item.IdStage), HeaderText = "Production-OWS", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PrOWS });

                                            DataTableForGridLayout.Columns.Add("Rework_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Rework_" + Convert.ToString(item.IdStage), HeaderText = "Rework", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Rework });

                                            DataTableForGridLayout.Columns.Add("ROWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "ROWS_" + Convert.ToString(item.IdStage), HeaderText = "Rework-OWS", Width = 95, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.ReOWS });


                                            //string fieldName10 = "Real_" + Convert.ToString(timeTrackingStage.IdStage);
                                            DataTableForGridLayout.Columns.Add("Real_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Real_" + Convert.ToString(item.IdStage), HeaderText = "Real", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProduction });
                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("POWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "POWS_" + Convert.ToString(item.IdStage), HeaderText = "Production-OWS", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicPOWSWithoutCOMCADCAM });
                                            DataTableForGridLayout.Columns.Add("Rework_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Rework_" + Convert.ToString(item.IdStage), HeaderText = "Rework", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicReworkWithoutCOMCADCAM });

                                            DataTableForGridLayout.Columns.Add("ROWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "ROWS_" + Convert.ToString(item.IdStage), HeaderText = "Rework-OWS", Width = 95, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicROWSWithoutCOMCADCAM });


                                            //string fieldName10 = "Real_" + Convert.ToString(timeTrackingStage.IdStage);
                                            DataTableForGridLayout.Columns.Add("Real_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Real_" + Convert.ToString(item.IdStage), HeaderText = "Real", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicRealWithoutCOMCADCAM });
                                        }
                                        #endregion


                                        #endregion
                                        //string fieldName10 = "Real_" + Convert.ToString(timeTrackingStage.IdStage);
                                        //DataTableForGridLayout.Columns.Add("Real_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Real_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Real", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProduction });

                                        //string fieldName11 = "Remaining_" + Convert.ToString(timeTrackingStage.IdStage);
                                        DataTableForGridLayout.Columns.Add("Remaining_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                        band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Remaining_" + Convert.ToString(item.IdStage), HeaderText = "Remaining", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.RemainingTime2 });
                                        DataTableForGridLayout.Columns.Add("Tempcolor_" + Convert.ToString(item.IdStage), typeof(bool));
                                        break;
                                    case 3:
                                        #region [GEOS2-5320][pallavi jadhav][15 10 2024]

                                        #region [Geos2-6786][gulab lakade][30 12 2024]
                                        if (isMatch == false && IscheckplantInSetting == true)
                                        {
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDateHtmlColor_" + Convert.ToString(item.IdStage), typeof(string));
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDate_" + Convert.ToString(item.IdStage), typeof(DateTime));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "PlannedDeliveryDate_" + Convert.ToString(item.IdStage), HeaderText = "Planned Delivery Date", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PlannedDeliveryDate2, Visible = false });
                                            DataTableForGridLayout.Columns.Add("Days_" + Convert.ToString(item.IdStage), typeof(int));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Days_" + Convert.ToString(item.IdStage), HeaderText = "Days", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Days, Visible = false });

                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDateHtmlColor_" + Convert.ToString(item.IdStage), typeof(string));
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDate_" + Convert.ToString(item.IdStage), typeof(DateTime));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "PlannedDeliveryDate_" + Convert.ToString(item.IdStage), HeaderText = "Planned Delivery Date", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PlannedDeliveryDate2, Visible = true });
                                            DataTableForGridLayout.Columns.Add("Days_" + Convert.ToString(item.IdStage), typeof(int));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Days_" + Convert.ToString(item.IdStage), HeaderText = "Days", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Days, Visible = true });

                                        }
                                        #endregion
                                        #endregion
                                        #region [GEOS2-4252] [gulab lakade][06 03 2023]
                                        if (AppSettingData.Contains(Convert.ToInt32(item.IdStage)))
                                        {
                                            DataTableForGridLayout.Columns.Add("Expected_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Expected_" + Convert.ToString(item.IdStage), HeaderText = "Expected", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.ExpectedTime });
                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("Expected_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Expected_" + Convert.ToString(item.IdStage), HeaderText = "Expected", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProductionExpected });
                                        }
                                        #endregion
                                        #region [gulab lakade][11 03 2024][GEOS2-5466]
                                        //rajashri[GEOS2-5799]
                                        if (AppSettingData.Contains(Convert.ToInt32(item.IdStage)))
                                        {
                                            DataTableForGridLayout.Columns.Add("Production_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Production_" + Convert.ToString(item.IdStage), HeaderText = "Production", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Production });

                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("Production_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Production_" + Convert.ToString(item.IdStage), HeaderText = "Production", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProductionWithoutCOMCADCAM });
                                        }
                                        //DataTableForGridLayout.Columns.Add("POWS_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "POWS_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Production-OWS", Width = 116, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PrOWS });
                                        #region [rani dhamankar][20-02-2025][GEOS2-6685]
                                        if (AppSettingData.Contains(Convert.ToInt32(item.IdStage)))
                                        {
                                            DataTableForGridLayout.Columns.Add("POWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "POWS_" + Convert.ToString(item.IdStage), HeaderText = "Production-OWS", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PrOWS });

                                            DataTableForGridLayout.Columns.Add("Rework_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Rework_" + Convert.ToString(item.IdStage), HeaderText = "Rework", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Rework });

                                            DataTableForGridLayout.Columns.Add("ROWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "ROWS_" + Convert.ToString(item.IdStage), HeaderText = "Rework-OWS", Width = 95, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.ReOWS });


                                            //string fieldName10 = "Real_" + Convert.ToString(timeTrackingStage.IdStage);
                                            DataTableForGridLayout.Columns.Add("Real_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Real_" + Convert.ToString(item.IdStage), HeaderText = "Real", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProduction });
                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("POWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "POWS_" + Convert.ToString(item.IdStage), HeaderText = "Production-OWS", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicPOWSWithoutCOMCADCAM });
                                            DataTableForGridLayout.Columns.Add("Rework_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Rework_" + Convert.ToString(item.IdStage), HeaderText = "Rework", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicReworkWithoutCOMCADCAM });

                                            DataTableForGridLayout.Columns.Add("ROWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "ROWS_" + Convert.ToString(item.IdStage), HeaderText = "Rework-OWS", Width = 95, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicROWSWithoutCOMCADCAM });


                                            //string fieldName10 = "Real_" + Convert.ToString(timeTrackingStage.IdStage);
                                            DataTableForGridLayout.Columns.Add("Real_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Real_" + Convert.ToString(item.IdStage), HeaderText = "Real", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicRealWithoutCOMCADCAM });
                                        }
                                        #endregion
                                        //DataTableForGridLayout.Columns.Add("Rework_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Rework_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Rework", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Rework });

                                        //DataTableForGridLayout.Columns.Add("ROWS_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "ROWS_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Rework-OWS", Width = 95, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.ReOWS });

                                        #endregion
                                        ////string fieldName10 = "Real_" + Convert.ToString(timeTrackingStage.IdStage);
                                        //DataTableForGridLayout.Columns.Add("Real_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Real_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Real", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProduction });

                                        //string fieldName11 = "Remaining_" + Convert.ToString(timeTrackingStage.IdStage);
                                        DataTableForGridLayout.Columns.Add("Remaining_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                        band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Remaining_" + Convert.ToString(item.IdStage), HeaderText = "Remaining", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.RemainingTime3 });
                                        DataTableForGridLayout.Columns.Add("Tempcolor_" + Convert.ToString(item.IdStage), typeof(bool));
                                        break;
                                    case 4:
                                        #region [GEOS2-5320][pallavi jadhav][15 10 2024]

                                        #region [Geos2-6786][gulab lakade][30 12 2024]
                                        if (isMatch == false && IscheckplantInSetting == true)
                                        {
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDateHtmlColor_" + Convert.ToString(item.IdStage), typeof(string));
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDate_" + Convert.ToString(item.IdStage), typeof(DateTime));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "PlannedDeliveryDate_" + Convert.ToString(item.IdStage), HeaderText = "Planned Delivery Date", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PlannedDeliveryDate2, Visible = false });
                                            DataTableForGridLayout.Columns.Add("Days_" + Convert.ToString(item.IdStage), typeof(int));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Days_" + Convert.ToString(item.IdStage), HeaderText = "Days", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Days, Visible = false });

                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDateHtmlColor_" + Convert.ToString(item.IdStage), typeof(string));
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDate_" + Convert.ToString(item.IdStage), typeof(DateTime));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "PlannedDeliveryDate_" + Convert.ToString(item.IdStage), HeaderText = "Planned Delivery Date", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PlannedDeliveryDate2, Visible = true });
                                            DataTableForGridLayout.Columns.Add("Days_" + Convert.ToString(item.IdStage), typeof(int));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Days_" + Convert.ToString(item.IdStage), HeaderText = "Days", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Days, Visible = true });

                                        }
                                        #endregion
                                        #endregion
                                        #region [GEOS2-4252] [gulab lakade][06 03 2023]
                                        if (AppSettingData.Contains(Convert.ToInt32(item.IdStage)))
                                        {
                                            DataTableForGridLayout.Columns.Add("Expected_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Expected_" + Convert.ToString(item.IdStage), HeaderText = "Expected", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.ExpectedTime });
                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("Expected_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Expected_" + Convert.ToString(item.IdStage), HeaderText = "Expected", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProductionExpected });
                                        }
                                        #endregion
                                        #region [gulab lakade][11 03 2024][GEOS2-5466]
                                        //rajashri[GEOS2-5799]
                                        if (AppSettingData.Contains(Convert.ToInt32(item.IdStage)))
                                        {
                                            DataTableForGridLayout.Columns.Add("Production_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Production_" + Convert.ToString(item.IdStage), HeaderText = "Production", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Production });

                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("Production_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Production_" + Convert.ToString(item.IdStage), HeaderText = "Production", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProductionWithoutCOMCADCAM });
                                        }
                                        //DataTableForGridLayout.Columns.Add("POWS_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "POWS_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Production-OWS", Width = 116, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PrOWS });
                                        #region [rani dhamankar][20-02-2025][GEOS2-6685]
                                        if (AppSettingData.Contains(Convert.ToInt32(item.IdStage)))
                                        {
                                            DataTableForGridLayout.Columns.Add("POWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "POWS_" + Convert.ToString(item.IdStage), HeaderText = "Production-OWS", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PrOWS });

                                            DataTableForGridLayout.Columns.Add("Rework_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Rework_" + Convert.ToString(item.IdStage), HeaderText = "Rework", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Rework });

                                            DataTableForGridLayout.Columns.Add("ROWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "ROWS_" + Convert.ToString(item.IdStage), HeaderText = "Rework-OWS", Width = 95, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.ReOWS });


                                            //string fieldName10 = "Real_" + Convert.ToString(timeTrackingStage.IdStage);
                                            DataTableForGridLayout.Columns.Add("Real_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Real_" + Convert.ToString(item.IdStage), HeaderText = "Real", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProduction });
                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("POWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "POWS_" + Convert.ToString(item.IdStage), HeaderText = "Production-OWS", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicPOWSWithoutCOMCADCAM });
                                            DataTableForGridLayout.Columns.Add("Rework_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Rework_" + Convert.ToString(item.IdStage), HeaderText = "Rework", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicReworkWithoutCOMCADCAM });

                                            DataTableForGridLayout.Columns.Add("ROWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "ROWS_" + Convert.ToString(item.IdStage), HeaderText = "Rework-OWS", Width = 95, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicROWSWithoutCOMCADCAM });


                                            //string fieldName10 = "Real_" + Convert.ToString(timeTrackingStage.IdStage);
                                            DataTableForGridLayout.Columns.Add("Real_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Real_" + Convert.ToString(item.IdStage), HeaderText = "Real", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicRealWithoutCOMCADCAM });
                                        }
                                        #endregion
                                        //DataTableForGridLayout.Columns.Add("Rework_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Rework_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Rework", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Rework });

                                        //DataTableForGridLayout.Columns.Add("ROWS_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "ROWS_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Rework-OWS", Width = 95, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.ReOWS });

                                        #endregion
                                        ////string fieldName10 = "Real_" + Convert.ToString(timeTrackingStage.IdStage);
                                        //DataTableForGridLayout.Columns.Add("Real_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Real_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Real", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProduction });

                                        //string fieldName11 = "Remaining_" + Convert.ToString(timeTrackingStage.IdStage);
                                        DataTableForGridLayout.Columns.Add("Remaining_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                        band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Remaining_" + Convert.ToString(item.IdStage), HeaderText = "Remaining", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.RemainingTime4 });
                                        DataTableForGridLayout.Columns.Add("Tempcolor_" + Convert.ToString(item.IdStage), typeof(bool));
                                        break;
                                    case 5:
                                        #region [GEOS2-5320][pallavi jadhav][15 10 2024]

                                        #region [Geos2-6786][gulab lakade][30 12 2024]
                                        if (isMatch == false && IscheckplantInSetting == true)
                                        {
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDateHtmlColor_" + Convert.ToString(item.IdStage), typeof(string));
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDate_" + Convert.ToString(item.IdStage), typeof(DateTime));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "PlannedDeliveryDate_" + Convert.ToString(item.IdStage), HeaderText = "Planned Delivery Date", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PlannedDeliveryDate2, Visible = false });
                                            DataTableForGridLayout.Columns.Add("Days_" + Convert.ToString(item.IdStage), typeof(int));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Days_" + Convert.ToString(item.IdStage), HeaderText = "Days", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Days, Visible = false });

                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDateHtmlColor_" + Convert.ToString(item.IdStage), typeof(string));
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDate_" + Convert.ToString(item.IdStage), typeof(DateTime));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "PlannedDeliveryDate_" + Convert.ToString(item.IdStage), HeaderText = "Planned Delivery Date", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PlannedDeliveryDate2, Visible = true });
                                            DataTableForGridLayout.Columns.Add("Days_" + Convert.ToString(item.IdStage), typeof(int));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Days_" + Convert.ToString(item.IdStage), HeaderText = "Days", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Days, Visible = true });

                                        }
                                        #endregion
                                        #endregion
                                        #region [GEOS2-4252] [gulab lakade][06 03 2023]
                                        if (AppSettingData.Contains(Convert.ToInt32(item.IdStage)))
                                        {
                                            DataTableForGridLayout.Columns.Add("Expected_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Expected_" + Convert.ToString(item.IdStage), HeaderText = "Expected", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.ExpectedTime });
                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("Expected_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Expected_" + Convert.ToString(item.IdStage), HeaderText = "Expected", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProductionExpected });
                                        }
                                        #endregion
                                        #region [gulab lakade][11 03 2024][GEOS2-5466]
                                        //rajashri[GEOS2-5799]
                                        if (AppSettingData.Contains(Convert.ToInt32(item.IdStage)))
                                        {
                                            DataTableForGridLayout.Columns.Add("Production_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Production_" + Convert.ToString(item.IdStage), HeaderText = "Production", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Production });

                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("Production_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Production_" + Convert.ToString(item.IdStage), HeaderText = "Production", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProductionWithoutCOMCADCAM });
                                        }
                                        //DataTableForGridLayout.Columns.Add("POWS_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "POWS_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Production-OWS", Width = 116, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PrOWS });
                                        #region [rani dhamankar][20-02-2025][GEOS2-6685]
                                        if (AppSettingData.Contains(Convert.ToInt32(item.IdStage)))
                                        {
                                            DataTableForGridLayout.Columns.Add("POWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "POWS_" + Convert.ToString(item.IdStage), HeaderText = "Production-OWS", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PrOWS });

                                            DataTableForGridLayout.Columns.Add("Rework_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Rework_" + Convert.ToString(item.IdStage), HeaderText = "Rework", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Rework });

                                            DataTableForGridLayout.Columns.Add("ROWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "ROWS_" + Convert.ToString(item.IdStage), HeaderText = "Rework-OWS", Width = 95, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.ReOWS });


                                            //string fieldName10 = "Real_" + Convert.ToString(timeTrackingStage.IdStage);
                                            DataTableForGridLayout.Columns.Add("Real_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Real_" + Convert.ToString(item.IdStage), HeaderText = "Real", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProduction });
                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("POWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "POWS_" + Convert.ToString(item.IdStage), HeaderText = "Production-OWS", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicPOWSWithoutCOMCADCAM });
                                            DataTableForGridLayout.Columns.Add("Rework_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Rework_" + Convert.ToString(item.IdStage), HeaderText = "Rework", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicReworkWithoutCOMCADCAM });

                                            DataTableForGridLayout.Columns.Add("ROWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "ROWS_" + Convert.ToString(item.IdStage), HeaderText = "Rework-OWS", Width = 95, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicROWSWithoutCOMCADCAM });


                                            //string fieldName10 = "Real_" + Convert.ToString(timeTrackingStage.IdStage);
                                            DataTableForGridLayout.Columns.Add("Real_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Real_" + Convert.ToString(item.IdStage), HeaderText = "Real", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicRealWithoutCOMCADCAM });
                                        }
                                        #endregion
                                        //DataTableForGridLayout.Columns.Add("Rework_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Rework_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Rework", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Rework });

                                        //DataTableForGridLayout.Columns.Add("ROWS_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "ROWS_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Rework-OWS", Width = 95, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.ReOWS });

                                        #endregion
                                        ////string fieldName10 = "Real_" + Convert.ToString(timeTrackingStage.IdStage);
                                        //DataTableForGridLayout.Columns.Add("Real_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Real_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Real", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProduction });

                                        //string fieldName11 = "Remaining_" + Convert.ToString(timeTrackingStage.IdStage);
                                        DataTableForGridLayout.Columns.Add("Remaining_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                        band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Remaining_" + Convert.ToString(item.IdStage), HeaderText = "Remaining", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.RemainingTime5 });
                                        DataTableForGridLayout.Columns.Add("Tempcolor_" + Convert.ToString(item.IdStage), typeof(bool));
                                        break;
                                    case 6:
                                        #region [GEOS2-5320][pallavi jadhav][15 10 2024]

                                        #region [Geos2-6786][gulab lakade][30 12 2024]
                                        if (isMatch == false && IscheckplantInSetting == true)
                                        {
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDateHtmlColor_" + Convert.ToString(item.IdStage), typeof(string));
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDate_" + Convert.ToString(item.IdStage), typeof(DateTime));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "PlannedDeliveryDate_" + Convert.ToString(item.IdStage), HeaderText = "Planned Delivery Date", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PlannedDeliveryDate2, Visible = false });
                                            DataTableForGridLayout.Columns.Add("Days_" + Convert.ToString(item.IdStage), typeof(int));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Days_" + Convert.ToString(item.IdStage), HeaderText = "Days", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Days, Visible = false });

                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDateHtmlColor_" + Convert.ToString(item.IdStage), typeof(string));
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDate_" + Convert.ToString(item.IdStage), typeof(DateTime));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "PlannedDeliveryDate_" + Convert.ToString(item.IdStage), HeaderText = "Planned Delivery Date", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PlannedDeliveryDate2, Visible = true });
                                            DataTableForGridLayout.Columns.Add("Days_" + Convert.ToString(item.IdStage), typeof(int));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Days_" + Convert.ToString(item.IdStage), HeaderText = "Days", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Days, Visible = true });

                                        }
                                        #endregion
                                        #endregion
                                        #region [GEOS2-4252] [gulab lakade][06 03 2023]

                                        if (AppSettingData.Contains(Convert.ToInt32(item.IdStage)))
                                        {
                                            DataTableForGridLayout.Columns.Add("Expected_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Expected_" + Convert.ToString(item.IdStage), HeaderText = "Expected", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.ExpectedTime });
                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("Expected_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Expected_" + Convert.ToString(item.IdStage), HeaderText = "Expected", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProductionExpected });
                                        }
                                        #endregion

                                        #region [gulab lakade][11 03 2024][GEOS2-5466]
                                        //rajashri[GEOS2-5799]
                                        if (AppSettingData.Contains(Convert.ToInt32(item.IdStage)))
                                        {
                                            DataTableForGridLayout.Columns.Add("Production_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Production_" + Convert.ToString(item.IdStage), HeaderText = "Production", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Production });

                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("Production_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Production_" + Convert.ToString(item.IdStage), HeaderText = "Production", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProductionWithoutCOMCADCAM });
                                        }
                                        //DataTableForGridLayout.Columns.Add("POWS_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "POWS_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Production-OWS", Width = 116, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PrOWS });
                                        #region [rani dhamankar][20-02-2025][GEOS2-6685]
                                        if (AppSettingData.Contains(Convert.ToInt32(item.IdStage)))
                                        {
                                            DataTableForGridLayout.Columns.Add("POWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "POWS_" + Convert.ToString(item.IdStage), HeaderText = "Production-OWS", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PrOWS });

                                            DataTableForGridLayout.Columns.Add("Rework_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Rework_" + Convert.ToString(item.IdStage), HeaderText = "Rework", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Rework });

                                            DataTableForGridLayout.Columns.Add("ROWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "ROWS_" + Convert.ToString(item.IdStage), HeaderText = "Rework-OWS", Width = 95, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.ReOWS });


                                            //string fieldName10 = "Real_" + Convert.ToString(timeTrackingStage.IdStage);
                                            DataTableForGridLayout.Columns.Add("Real_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Real_" + Convert.ToString(item.IdStage), HeaderText = "Real", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProduction });
                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("POWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "POWS_" + Convert.ToString(item.IdStage), HeaderText = "Production-OWS", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicPOWSWithoutCOMCADCAM });
                                            DataTableForGridLayout.Columns.Add("Rework_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Rework_" + Convert.ToString(item.IdStage), HeaderText = "Rework", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicReworkWithoutCOMCADCAM });

                                            DataTableForGridLayout.Columns.Add("ROWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "ROWS_" + Convert.ToString(item.IdStage), HeaderText = "Rework-OWS", Width = 95, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicROWSWithoutCOMCADCAM });


                                            //string fieldName10 = "Real_" + Convert.ToString(timeTrackingStage.IdStage);
                                            DataTableForGridLayout.Columns.Add("Real_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Real_" + Convert.ToString(item.IdStage), HeaderText = "Real", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicRealWithoutCOMCADCAM });
                                        }
                                        #endregion
                                        //DataTableForGridLayout.Columns.Add("Rework_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Rework_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Rework", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Rework });

                                        //DataTableForGridLayout.Columns.Add("ROWS_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "ROWS_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Rework-OWS", Width = 95, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.ReOWS });

                                        #endregion
                                        ////string fieldName10 = "Real_" + Convert.ToString(timeTrackingStage.IdStage);
                                        //DataTableForGridLayout.Columns.Add("Real_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Real_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Real", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProduction });

                                        //string fieldName11 = "Remaining_" + Convert.ToString(timeTrackingStage.IdStage);
                                        DataTableForGridLayout.Columns.Add("Remaining_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                        band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Remaining_" + Convert.ToString(item.IdStage), HeaderText = "Remaining", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.RemainingTime6 });
                                        DataTableForGridLayout.Columns.Add("Tempcolor_" + Convert.ToString(item.IdStage), typeof(bool));
                                        break;
                                    case 7:
                                        #region [GEOS2-5320][pallavi jadhav][15 10 2024]

                                        #region [Geos2-6786][gulab lakade][30 12 2024]
                                        if (isMatch == false && IscheckplantInSetting == true)
                                        {
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDateHtmlColor_" + Convert.ToString(item.IdStage), typeof(string));
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDate_" + Convert.ToString(item.IdStage), typeof(DateTime));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "PlannedDeliveryDate_" + Convert.ToString(item.IdStage), HeaderText = "Planned Delivery Date", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PlannedDeliveryDate2, Visible = false });
                                            DataTableForGridLayout.Columns.Add("Days_" + Convert.ToString(item.IdStage), typeof(int));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Days_" + Convert.ToString(item.IdStage), HeaderText = "Days", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Days, Visible = false });

                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDateHtmlColor_" + Convert.ToString(item.IdStage), typeof(string));
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDate_" + Convert.ToString(item.IdStage), typeof(DateTime));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "PlannedDeliveryDate_" + Convert.ToString(item.IdStage), HeaderText = "Planned Delivery Date", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PlannedDeliveryDate2, Visible = true });
                                            DataTableForGridLayout.Columns.Add("Days_" + Convert.ToString(item.IdStage), typeof(int));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Days_" + Convert.ToString(item.IdStage), HeaderText = "Days", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Days, Visible = true });

                                        }
                                        #endregion
                                        #endregion
                                        #region [GEOS2-4252] [gulab lakade][06 03 2023]
                                        if (AppSettingData.Contains(Convert.ToInt32(item.IdStage)))
                                        {
                                            DataTableForGridLayout.Columns.Add("Expected_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Expected_" + Convert.ToString(item.IdStage), HeaderText = "Expected", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.ExpectedTime });
                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("Expected_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Expected_" + Convert.ToString(item.IdStage), HeaderText = "Expected", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProductionExpected });
                                        }
                                        #endregion
                                        #region [gulab lakade][11 03 2024][GEOS2-5466]
                                        //rajashri[GEOS2-5799]
                                        if (AppSettingData.Contains(Convert.ToInt32(item.IdStage)))
                                        {
                                            DataTableForGridLayout.Columns.Add("Production_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Production_" + Convert.ToString(item.IdStage), HeaderText = "Production", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Production });

                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("Production_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Production_" + Convert.ToString(item.IdStage), HeaderText = "Production", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProductionWithoutCOMCADCAM });
                                        }
                                        //DataTableForGridLayout.Columns.Add("POWS_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "POWS_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Production-OWS", Width = 116, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PrOWS });
                                        #region [rani dhamankar][20-02-2025][GEOS2-6685]
                                        if (AppSettingData.Contains(Convert.ToInt32(item.IdStage)))
                                        {
                                            DataTableForGridLayout.Columns.Add("POWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "POWS_" + Convert.ToString(item.IdStage), HeaderText = "Production-OWS", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PrOWS });

                                            DataTableForGridLayout.Columns.Add("Rework_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Rework_" + Convert.ToString(item.IdStage), HeaderText = "Rework", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Rework });

                                            DataTableForGridLayout.Columns.Add("ROWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "ROWS_" + Convert.ToString(item.IdStage), HeaderText = "Rework-OWS", Width = 95, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.ReOWS });


                                            //string fieldName10 = "Real_" + Convert.ToString(timeTrackingStage.IdStage);
                                            DataTableForGridLayout.Columns.Add("Real_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Real_" + Convert.ToString(item.IdStage), HeaderText = "Real", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProduction });
                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("POWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "POWS_" + Convert.ToString(item.IdStage), HeaderText = "Production-OWS", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicPOWSWithoutCOMCADCAM });
                                            DataTableForGridLayout.Columns.Add("Rework_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Rework_" + Convert.ToString(item.IdStage), HeaderText = "Rework", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicReworkWithoutCOMCADCAM });

                                            DataTableForGridLayout.Columns.Add("ROWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "ROWS_" + Convert.ToString(item.IdStage), HeaderText = "Rework-OWS", Width = 95, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicROWSWithoutCOMCADCAM });


                                            //string fieldName10 = "Real_" + Convert.ToString(timeTrackingStage.IdStage);
                                            DataTableForGridLayout.Columns.Add("Real_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Real_" + Convert.ToString(item.IdStage), HeaderText = "Real", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicRealWithoutCOMCADCAM });
                                        }
                                        #endregion
                                        //DataTableForGridLayout.Columns.Add("Rework_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Rework_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Rework", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Rework });

                                        //DataTableForGridLayout.Columns.Add("ROWS_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "ROWS_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Rework-OWS", Width = 95, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.ReOWS });

                                        #endregion
                                        ////string fieldName10 = "Real_" + Convert.ToString(timeTrackingStage.IdStage);
                                        //DataTableForGridLayout.Columns.Add("Real_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Real_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Real", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProduction });

                                        //string fieldName11 = "Remaining_" + Convert.ToString(timeTrackingStage.IdStage);
                                        DataTableForGridLayout.Columns.Add("Remaining_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                        band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Remaining_" + Convert.ToString(item.IdStage), HeaderText = "Remaining", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.RemainingTime7 });
                                        DataTableForGridLayout.Columns.Add("Tempcolor_" + Convert.ToString(item.IdStage), typeof(bool));
                                        break;
                                    case 8:
                                        #region [GEOS2-5320][pallavi jadhav][15 10 2024]

                                        #region [Geos2-6786][gulab lakade][30 12 2024]
                                        if (isMatch == false && IscheckplantInSetting == true)
                                        {
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDateHtmlColor_" + Convert.ToString(item.IdStage), typeof(string));
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDate_" + Convert.ToString(item.IdStage), typeof(DateTime));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "PlannedDeliveryDate_" + Convert.ToString(item.IdStage), HeaderText = "Planned Delivery Date", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PlannedDeliveryDate2, Visible = false });
                                            DataTableForGridLayout.Columns.Add("Days_" + Convert.ToString(item.IdStage), typeof(int));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Days_" + Convert.ToString(item.IdStage), HeaderText = "Days", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Days, Visible = false });

                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDateHtmlColor_" + Convert.ToString(item.IdStage), typeof(string));
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDate_" + Convert.ToString(item.IdStage), typeof(DateTime));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "PlannedDeliveryDate_" + Convert.ToString(item.IdStage), HeaderText = "Planned Delivery Date", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PlannedDeliveryDate2, Visible = true });
                                            DataTableForGridLayout.Columns.Add("Days_" + Convert.ToString(item.IdStage), typeof(int));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Days_" + Convert.ToString(item.IdStage), HeaderText = "Days", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Days, Visible = true });

                                        }
                                        #endregion
                                        #endregion
                                        #region [GEOS2-4252] [gulab lakade][06 03 2023]
                                        if (AppSettingData.Contains(Convert.ToInt32(item.IdStage)))
                                        {
                                            DataTableForGridLayout.Columns.Add("Expected_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Expected_" + Convert.ToString(item.IdStage), HeaderText = "Expected", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.ExpectedTime });
                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("Expected_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Expected_" + Convert.ToString(item.IdStage), HeaderText = "Expected", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProductionExpected });
                                        }
                                        #endregion
                                        #region [gulab lakade][11 03 2024][GEOS2-5466]
                                        //rajashri[GEOS2-5799]
                                        if (AppSettingData.Contains(Convert.ToInt32(item.IdStage)))
                                        {
                                            DataTableForGridLayout.Columns.Add("Production_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Production_" + Convert.ToString(item.IdStage), HeaderText = "Production", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Production });

                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("Production_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Production_" + Convert.ToString(item.IdStage), HeaderText = "Production", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProductionWithoutCOMCADCAM });
                                        }
                                        //DataTableForGridLayout.Columns.Add("POWS_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "POWS_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Production-OWS", Width = 116, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PrOWS });
                                        #region [rani dhamankar][20-02-2025][GEOS2-6685]
                                        if (AppSettingData.Contains(Convert.ToInt32(item.IdStage)))
                                        {
                                            DataTableForGridLayout.Columns.Add("POWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "POWS_" + Convert.ToString(item.IdStage), HeaderText = "Production-OWS", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PrOWS });

                                            DataTableForGridLayout.Columns.Add("Rework_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Rework_" + Convert.ToString(item.IdStage), HeaderText = "Rework", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Rework });

                                            DataTableForGridLayout.Columns.Add("ROWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "ROWS_" + Convert.ToString(item.IdStage), HeaderText = "Rework-OWS", Width = 95, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.ReOWS });

                                            //#endregion
                                            //string fieldName10 = "Real_" + Convert.ToString(timeTrackingStage.IdStage);
                                            DataTableForGridLayout.Columns.Add("Real_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Real_" + Convert.ToString(item.IdStage), HeaderText = "Real", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProduction });
                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("POWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "POWS_" + Convert.ToString(item.IdStage), HeaderText = "Production-OWS", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicPOWSWithoutCOMCADCAM });
                                            DataTableForGridLayout.Columns.Add("Rework_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Rework_" + Convert.ToString(item.IdStage), HeaderText = "Rework", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicReworkWithoutCOMCADCAM });

                                            DataTableForGridLayout.Columns.Add("ROWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "ROWS_" + Convert.ToString(item.IdStage), HeaderText = "Rework-OWS", Width = 95, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicROWSWithoutCOMCADCAM });

                                            //#endregion
                                            //string fieldName10 = "Real_" + Convert.ToString(timeTrackingStage.IdStage);
                                            DataTableForGridLayout.Columns.Add("Real_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Real_" + Convert.ToString(item.IdStage), HeaderText = "Real", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicRealWithoutCOMCADCAM });
                                        }
                                        #endregion
                                        //DataTableForGridLayout.Columns.Add("Rework_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Rework_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Rework", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Rework });

                                        //DataTableForGridLayout.Columns.Add("ROWS_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "ROWS_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Rework-OWS", Width = 95, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.ReOWS });

                                        #endregion
                                        ////string fieldName10 = "Real_" + Convert.ToString(timeTrackingStage.IdStage);
                                        //DataTableForGridLayout.Columns.Add("Real_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Real_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Real", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProduction });

                                        //string fieldName11 = "Remaining_" + Convert.ToString(timeTrackingStage.IdStage);
                                        DataTableForGridLayout.Columns.Add("Remaining_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                        band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Remaining_" + Convert.ToString(item.IdStage), HeaderText = "Remaining", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.RemainingTime8 });
                                        DataTableForGridLayout.Columns.Add("Tempcolor_" + Convert.ToString(item.IdStage), typeof(bool));
                                        break;
                                    case 9:
                                        #region [GEOS2-5320][pallavi jadhav][15 10 2024]

                                        #region [Geos2-6786][gulab lakade][30 12 2024]
                                        if (isMatch == false && IscheckplantInSetting == true)
                                        {
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDateHtmlColor_" + Convert.ToString(item.IdStage), typeof(string));
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDate_" + Convert.ToString(item.IdStage), typeof(DateTime));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "PlannedDeliveryDate_" + Convert.ToString(item.IdStage), HeaderText = "Planned Delivery Date", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PlannedDeliveryDate2, Visible = false });
                                            DataTableForGridLayout.Columns.Add("Days_" + Convert.ToString(item.IdStage), typeof(int));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Days_" + Convert.ToString(item.IdStage), HeaderText = "Days", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Days, Visible = false });

                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDateHtmlColor_" + Convert.ToString(item.IdStage), typeof(string));
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDate_" + Convert.ToString(item.IdStage), typeof(DateTime));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "PlannedDeliveryDate_" + Convert.ToString(item.IdStage), HeaderText = "Planned Delivery Date", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PlannedDeliveryDate2, Visible = true });
                                            DataTableForGridLayout.Columns.Add("Days_" + Convert.ToString(item.IdStage), typeof(int));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Days_" + Convert.ToString(item.IdStage), HeaderText = "Days", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Days, Visible = true });

                                        }
                                        #endregion
                                        #endregion
                                        #region [GEOS2-4252] [gulab lakade][06 03 2023]
                                        if (AppSettingData.Contains(Convert.ToInt32(item.IdStage)))
                                        {
                                            DataTableForGridLayout.Columns.Add("Expected_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Expected_" + Convert.ToString(item.IdStage), HeaderText = "Expected", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.ExpectedTime });
                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("Expected_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Expected_" + Convert.ToString(item.IdStage), HeaderText = "Expected", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProductionExpected });
                                        }
                                        #endregion
                                        #region [gulab lakade][11 03 2024][GEOS2-5466]
                                        //rajashri[GEOS2-5799]
                                        if (AppSettingData.Contains(Convert.ToInt32(item.IdStage)))
                                        {
                                            DataTableForGridLayout.Columns.Add("Production_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Production_" + Convert.ToString(item.IdStage), HeaderText = "Production", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Production });

                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("Production_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Production_" + Convert.ToString(item.IdStage), HeaderText = "Production", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProductionWithoutCOMCADCAM });
                                        }
                                        //DataTableForGridLayout.Columns.Add("POWS_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "POWS_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Production-OWS", Width = 116, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PrOWS });
                                        #region [rani dhamankar][20-02-2025][GEOS2-6685]
                                        if (AppSettingData.Contains(Convert.ToInt32(item.IdStage)))
                                        {
                                            DataTableForGridLayout.Columns.Add("POWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "POWS_" + Convert.ToString(item.IdStage), HeaderText = "Production-OWS", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PrOWS });

                                            DataTableForGridLayout.Columns.Add("Rework_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Rework_" + Convert.ToString(item.IdStage), HeaderText = "Rework", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Rework });

                                            DataTableForGridLayout.Columns.Add("ROWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "ROWS_" + Convert.ToString(item.IdStage), HeaderText = "Rework-OWS", Width = 95, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.ReOWS });


                                            //string fieldName10 = "Real_" + Convert.ToString(timeTrackingStage.IdStage);
                                            DataTableForGridLayout.Columns.Add("Real_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Real_" + Convert.ToString(item.IdStage), HeaderText = "Real", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProduction });
                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("POWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "POWS_" + Convert.ToString(item.IdStage), HeaderText = "Production-OWS", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicPOWSWithoutCOMCADCAM });
                                            DataTableForGridLayout.Columns.Add("Rework_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Rework_" + Convert.ToString(item.IdStage), HeaderText = "Rework", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicReworkWithoutCOMCADCAM });

                                            DataTableForGridLayout.Columns.Add("ROWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "ROWS_" + Convert.ToString(item.IdStage), HeaderText = "Rework-OWS", Width = 95, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicROWSWithoutCOMCADCAM });


                                            //string fieldName10 = "Real_" + Convert.ToString(timeTrackingStage.IdStage);
                                            DataTableForGridLayout.Columns.Add("Real_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Real_" + Convert.ToString(item.IdStage), HeaderText = "Real", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicRealWithoutCOMCADCAM });
                                        }
                                        #endregion
                                        //DataTableForGridLayout.Columns.Add("Rework_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Rework_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Rework", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Rework });

                                        //DataTableForGridLayout.Columns.Add("ROWS_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "ROWS_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Rework-OWS", Width = 95, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.ReOWS });

                                        #endregion
                                        ////string fieldName10 = "Real_" + Convert.ToString(timeTrackingStage.IdStage);
                                        //DataTableForGridLayout.Columns.Add("Real_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Real_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Real", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProduction });

                                        //string fieldName11 = "Remaining_" + Convert.ToString(timeTrackingStage.IdStage);
                                        DataTableForGridLayout.Columns.Add("Remaining_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                        band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Remaining_" + Convert.ToString(item.IdStage), HeaderText = "Remaining", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.RemainingTime9 });
                                        DataTableForGridLayout.Columns.Add("Tempcolor_" + Convert.ToString(item.IdStage), typeof(bool));
                                        break;
                                    case 10:
                                        #region [GEOS2-5320][pallavi jadhav][15 10 2024]
                                        #region [Geos2-6786][gulab lakade][30 12 2024]
                                        if (isMatch == false && IscheckplantInSetting == true)
                                        {
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDateHtmlColor_" + Convert.ToString(item.IdStage), typeof(string));
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDate_" + Convert.ToString(item.IdStage), typeof(DateTime));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "PlannedDeliveryDate_" + Convert.ToString(item.IdStage), HeaderText = "Planned Delivery Date", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PlannedDeliveryDate2, Visible = false });
                                            DataTableForGridLayout.Columns.Add("Days_" + Convert.ToString(item.IdStage), typeof(int));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Days_" + Convert.ToString(item.IdStage), HeaderText = "Days", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Days, Visible = false });

                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDateHtmlColor_" + Convert.ToString(item.IdStage), typeof(string));
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDate_" + Convert.ToString(item.IdStage), typeof(DateTime));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "PlannedDeliveryDate_" + Convert.ToString(item.IdStage), HeaderText = "Planned Delivery Date", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PlannedDeliveryDate2, Visible = true });
                                            DataTableForGridLayout.Columns.Add("Days_" + Convert.ToString(item.IdStage), typeof(int));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Days_" + Convert.ToString(item.IdStage), HeaderText = "Days", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Days, Visible = true });

                                        }
                                        #endregion
                                        #endregion
                                        #region [GEOS2-4252] [gulab lakade][06 03 2023]
                                        if (AppSettingData.Contains(Convert.ToInt32(item.IdStage)))
                                        {
                                            DataTableForGridLayout.Columns.Add("Expected_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Expected_" + Convert.ToString(item.IdStage), HeaderText = "Expected", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.ExpectedTime });
                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("Expected_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Expected_" + Convert.ToString(item.IdStage), HeaderText = "Expected", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProductionExpected });
                                        }
                                        #endregion
                                        #region [gulab lakade][11 03 2024][GEOS2-5466]
                                        //rajashri[GEOS2-5799]
                                        if (AppSettingData.Contains(Convert.ToInt32(item.IdStage)))
                                        {
                                            DataTableForGridLayout.Columns.Add("Production_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Production_" + Convert.ToString(item.IdStage), HeaderText = "Production", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Production });

                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("Production_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Production_" + Convert.ToString(item.IdStage), HeaderText = "Production", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProductionWithoutCOMCADCAM });
                                        }
                                        //DataTableForGridLayout.Columns.Add("POWS_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "POWS_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Production-OWS", Width = 116, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PrOWS });
                                        #region [rani dhamankar][20-02-2025][GEOS2-6685]
                                        if (AppSettingData.Contains(Convert.ToInt32(item.IdStage)))
                                        {
                                            DataTableForGridLayout.Columns.Add("POWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "POWS_" + Convert.ToString(item.IdStage), HeaderText = "Production-OWS", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PrOWS });

                                            DataTableForGridLayout.Columns.Add("Rework_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Rework_" + Convert.ToString(item.IdStage), HeaderText = "Rework", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Rework });

                                            DataTableForGridLayout.Columns.Add("ROWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "ROWS_" + Convert.ToString(item.IdStage), HeaderText = "Rework-OWS", Width = 95, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.ReOWS });


                                            //string fieldName10 = "Real_" + Convert.ToString(timeTrackingStage.IdStage);
                                            DataTableForGridLayout.Columns.Add("Real_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Real_" + Convert.ToString(item.IdStage), HeaderText = "Real", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProduction });
                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("POWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "POWS_" + Convert.ToString(item.IdStage), HeaderText = "Production-OWS", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicPOWSWithoutCOMCADCAM });
                                            DataTableForGridLayout.Columns.Add("Rework_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Rework_" + Convert.ToString(item.IdStage), HeaderText = "Rework", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicReworkWithoutCOMCADCAM });

                                            DataTableForGridLayout.Columns.Add("ROWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "ROWS_" + Convert.ToString(item.IdStage), HeaderText = "Rework-OWS", Width = 95, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicROWSWithoutCOMCADCAM });


                                            //string fieldName10 = "Real_" + Convert.ToString(timeTrackingStage.IdStage);
                                            DataTableForGridLayout.Columns.Add("Real_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Real_" + Convert.ToString(item.IdStage), HeaderText = "Real", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicRealWithoutCOMCADCAM });
                                        }
                                        #endregion
                                        //DataTableForGridLayout.Columns.Add("Rework_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Rework_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Rework", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Rework });

                                        //DataTableForGridLayout.Columns.Add("ROWS_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "ROWS_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Rework-OWS", Width = 95, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.ReOWS });

                                        #endregion
                                        ////string fieldName10 = "Real_" + Convert.ToString(timeTrackingStage.IdStage);
                                        //DataTableForGridLayout.Columns.Add("Real_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Real_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Real", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProduction });

                                        //string fieldName11 = "Remaining_" + Convert.ToString(timeTrackingStage.IdStage);
                                        DataTableForGridLayout.Columns.Add("Remaining_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                        band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Remaining_" + Convert.ToString(item.IdStage), HeaderText = "Remaining", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.RemainingTime10 });
                                        DataTableForGridLayout.Columns.Add("Tempcolor_" + Convert.ToString(item.IdStage), typeof(bool));

                                        break;
                                    case 11:
                                        #region [GEOS2-5320][pallavi jadhav][15 10 2024]
                                        #region [Geos2-6786][gulab lakade][30 12 2024]
                                        if (isMatch == false && IscheckplantInSetting == true)
                                        {
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDateHtmlColor_" + Convert.ToString(item.IdStage), typeof(string));
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDate_" + Convert.ToString(item.IdStage), typeof(DateTime));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "PlannedDeliveryDate_" + Convert.ToString(item.IdStage), HeaderText = "Planned Delivery Date", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PlannedDeliveryDate2, Visible = false });
                                            DataTableForGridLayout.Columns.Add("Days_" + Convert.ToString(item.IdStage), typeof(int));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Days_" + Convert.ToString(item.IdStage), HeaderText = "Days", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Days, Visible = false });

                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDateHtmlColor_" + Convert.ToString(item.IdStage), typeof(string));
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDate_" + Convert.ToString(item.IdStage), typeof(DateTime));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "PlannedDeliveryDate_" + Convert.ToString(item.IdStage), HeaderText = "Planned Delivery Date", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PlannedDeliveryDate2, Visible = true });
                                            DataTableForGridLayout.Columns.Add("Days_" + Convert.ToString(item.IdStage), typeof(int));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Days_" + Convert.ToString(item.IdStage), HeaderText = "Days", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Days, Visible = true });

                                        }
                                        #endregion
                                        #endregion
                                        #region [GEOS2-4252] [gulab lakade][06 03 2023]
                                        if (AppSettingData.Contains(Convert.ToInt32(item.IdStage)))
                                        {
                                            DataTableForGridLayout.Columns.Add("Expected_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Expected_" + Convert.ToString(item.IdStage), HeaderText = "Expected", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.ExpectedTime });
                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("Expected_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Expected_" + Convert.ToString(item.IdStage), HeaderText = "Expected", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProductionExpected });
                                        }
                                        #endregion
                                        #region [gulab lakade][11 03 2024][GEOS2-5466]
                                        //rajashri[GEOS2-5799]
                                        if (AppSettingData.Contains(Convert.ToInt32(item.IdStage)))
                                        {
                                            DataTableForGridLayout.Columns.Add("Production_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Production_" + Convert.ToString(item.IdStage), HeaderText = "Production", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Production });

                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("Production_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Production_" + Convert.ToString(item.IdStage), HeaderText = "Production", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProductionWithoutCOMCADCAM });
                                        }
                                        //DataTableForGridLayout.Columns.Add("POWS_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "POWS_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Production-OWS", Width = 116, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PrOWS });
                                        #region [rani dhamankar][20-02-2025][GEOS2-6685]
                                        if (AppSettingData.Contains(Convert.ToInt32(item.IdStage)))
                                        {
                                            DataTableForGridLayout.Columns.Add("POWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "POWS_" + Convert.ToString(item.IdStage), HeaderText = "Production-OWS", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PrOWS });

                                            DataTableForGridLayout.Columns.Add("Rework_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Rework_" + Convert.ToString(item.IdStage), HeaderText = "Rework", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Rework });

                                            DataTableForGridLayout.Columns.Add("ROWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "ROWS_" + Convert.ToString(item.IdStage), HeaderText = "Rework-OWS", Width = 95, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.ReOWS });


                                            //string fieldName10 = "Real_" + Convert.ToString(timeTrackingStage.IdStage);
                                            DataTableForGridLayout.Columns.Add("Real_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Real_" + Convert.ToString(item.IdStage), HeaderText = "Real", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProduction });
                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("POWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "POWS_" + Convert.ToString(item.IdStage), HeaderText = "Production-OWS", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicPOWSWithoutCOMCADCAM });
                                            DataTableForGridLayout.Columns.Add("Rework_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Rework_" + Convert.ToString(item.IdStage), HeaderText = "Rework", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicReworkWithoutCOMCADCAM });

                                            DataTableForGridLayout.Columns.Add("ROWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "ROWS_" + Convert.ToString(item.IdStage), HeaderText = "Rework-OWS", Width = 95, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicROWSWithoutCOMCADCAM });


                                            //string fieldName10 = "Real_" + Convert.ToString(timeTrackingStage.IdStage);
                                            DataTableForGridLayout.Columns.Add("Real_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Real_" + Convert.ToString(item.IdStage), HeaderText = "Real", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicRealWithoutCOMCADCAM });
                                        }
                                        #endregion
                                        //DataTableForGridLayout.Columns.Add("Rework_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Rework_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Rework", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Rework });

                                        //DataTableForGridLayout.Columns.Add("ROWS_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "ROWS_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Rework-OWS", Width = 95, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.ReOWS });

                                        #endregion
                                        ////string fieldName10 = "Real_" + Convert.ToString(timeTrackingStage.IdStage);
                                        //DataTableForGridLayout.Columns.Add("Real_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Real_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Real", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProduction });

                                        //string fieldName11 = "Remaining_" + Convert.ToString(timeTrackingStage.IdStage);
                                        DataTableForGridLayout.Columns.Add("Remaining_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                        band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Remaining_" + Convert.ToString(item.IdStage), HeaderText = "Remaining", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.RemainingTime11 });
                                        DataTableForGridLayout.Columns.Add("Tempcolor_" + Convert.ToString(item.IdStage), typeof(bool));
                                        break;
                                    case 12:
                                        #region [GEOS2-5320][pallavi jadhav][15 10 2024]

                                        #region [Geos2-6786][gulab lakade][30 12 2024]
                                        if (isMatch == false && IscheckplantInSetting == true)
                                        {
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDateHtmlColor_" + Convert.ToString(item.IdStage), typeof(string));
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDate_" + Convert.ToString(item.IdStage), typeof(DateTime));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "PlannedDeliveryDate_" + Convert.ToString(item.IdStage), HeaderText = "Planned Delivery Date", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PlannedDeliveryDate2, Visible = false });
                                            DataTableForGridLayout.Columns.Add("Days_" + Convert.ToString(item.IdStage), typeof(int));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Days_" + Convert.ToString(item.IdStage), HeaderText = "Days", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Days, Visible = false });

                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDateHtmlColor_" + Convert.ToString(item.IdStage), typeof(string));
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDate_" + Convert.ToString(item.IdStage), typeof(DateTime));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "PlannedDeliveryDate_" + Convert.ToString(item.IdStage), HeaderText = "Planned Delivery Date", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PlannedDeliveryDate2, Visible = true });
                                            DataTableForGridLayout.Columns.Add("Days_" + Convert.ToString(item.IdStage), typeof(int));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Days_" + Convert.ToString(item.IdStage), HeaderText = "Days", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Days, Visible = true });

                                        }
                                        #endregion
                                        #endregion
                                        #region [GEOS2-4252] [gulab lakade][06 03 2023]
                                        if (AppSettingData.Contains(Convert.ToInt32(item.IdStage)))
                                        {
                                            DataTableForGridLayout.Columns.Add("Expected_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Expected_" + Convert.ToString(item.IdStage), HeaderText = "Expected", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.ExpectedTime });
                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("Expected_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Expected_" + Convert.ToString(item.IdStage), HeaderText = "Expected", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProductionExpected });
                                        }
                                        #endregion
                                        #region [gulab lakade][11 03 2024][GEOS2-5466]
                                        //rajashri[GEOS2-5799]
                                        if (AppSettingData.Contains(Convert.ToInt32(item.IdStage)))
                                        {
                                            DataTableForGridLayout.Columns.Add("Production_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Production_" + Convert.ToString(item.IdStage), HeaderText = "Production", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Production });

                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("Production_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Production_" + Convert.ToString(item.IdStage), HeaderText = "Production", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProductionWithoutCOMCADCAM });
                                        }
                                        //DataTableForGridLayout.Columns.Add("POWS_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "POWS_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Production-OWS", Width = 116, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PrOWS });
                                        #region [rani dhamankar][20-02-2025][GEOS2-6685]
                                        if (AppSettingData.Contains(Convert.ToInt32(item.IdStage)))
                                        {
                                            DataTableForGridLayout.Columns.Add("POWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "POWS_" + Convert.ToString(item.IdStage), HeaderText = "Production-OWS", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PrOWS });

                                            DataTableForGridLayout.Columns.Add("Rework_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Rework_" + Convert.ToString(item.IdStage), HeaderText = "Rework", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Rework });

                                            DataTableForGridLayout.Columns.Add("ROWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "ROWS_" + Convert.ToString(item.IdStage), HeaderText = "Rework-OWS", Width = 95, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.ReOWS });


                                            //string fieldName10 = "Real_" + Convert.ToString(timeTrackingStage.IdStage);
                                            DataTableForGridLayout.Columns.Add("Real_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Real_" + Convert.ToString(item.IdStage), HeaderText = "Real", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProduction });
                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("POWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "POWS_" + Convert.ToString(item.IdStage), HeaderText = "Production-OWS", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicPOWSWithoutCOMCADCAM });
                                            DataTableForGridLayout.Columns.Add("Rework_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Rework_" + Convert.ToString(item.IdStage), HeaderText = "Rework", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicReworkWithoutCOMCADCAM });

                                            DataTableForGridLayout.Columns.Add("ROWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "ROWS_" + Convert.ToString(item.IdStage), HeaderText = "Rework-OWS", Width = 95, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicROWSWithoutCOMCADCAM });


                                            //string fieldName10 = "Real_" + Convert.ToString(timeTrackingStage.IdStage);
                                            DataTableForGridLayout.Columns.Add("Real_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Real_" + Convert.ToString(item.IdStage), HeaderText = "Real", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicRealWithoutCOMCADCAM });
                                        }
                                        #endregion
                                        //DataTableForGridLayout.Columns.Add("Rework_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Rework_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Rework", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Rework });

                                        //DataTableForGridLayout.Columns.Add("ROWS_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "ROWS_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Rework-OWS", Width = 95, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.ReOWS });

                                        #endregion
                                        ////string fieldName10 = "Real_" + Convert.ToString(timeTrackingStage.IdStage);
                                        //DataTableForGridLayout.Columns.Add("Real_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Real_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Real", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProduction });

                                        //string fieldName11 = "Remaining_" + Convert.ToString(timeTrackingStage.IdStage);
                                        DataTableForGridLayout.Columns.Add("Remaining_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                        band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Remaining_" + Convert.ToString(item.IdStage), HeaderText = "Remaining", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.RemainingTime12 });
                                        DataTableForGridLayout.Columns.Add("Tempcolor_" + Convert.ToString(item.IdStage), typeof(bool));
                                        break;
                                    case 21:
                                        #region [GEOS2-5320][pallavi jadhav][15 10 2024]

                                        #region [Geos2-6786][gulab lakade][30 12 2024]
                                        if (isMatch == false && IscheckplantInSetting == true)
                                        {
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDateHtmlColor_" + Convert.ToString(item.IdStage), typeof(string));
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDate_" + Convert.ToString(item.IdStage), typeof(DateTime));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "PlannedDeliveryDate_" + Convert.ToString(item.IdStage), HeaderText = "Planned Delivery Date", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PlannedDeliveryDate2, Visible = false });
                                            DataTableForGridLayout.Columns.Add("Days_" + Convert.ToString(item.IdStage), typeof(int));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Days_" + Convert.ToString(item.IdStage), HeaderText = "Days", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Days, Visible = false });

                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDateHtmlColor_" + Convert.ToString(item.IdStage), typeof(string));
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDate_" + Convert.ToString(item.IdStage), typeof(DateTime));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "PlannedDeliveryDate_" + Convert.ToString(item.IdStage), HeaderText = "Planned Delivery Date", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PlannedDeliveryDate2, Visible = true });
                                            DataTableForGridLayout.Columns.Add("Days_" + Convert.ToString(item.IdStage), typeof(int));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Days_" + Convert.ToString(item.IdStage), HeaderText = "Days", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Days, Visible = true });

                                        }
                                        #endregion
                                        #endregion
                                        #region [GEOS2-4252] [gulab lakade][06 03 2023]
                                        if (AppSettingData.Contains(Convert.ToInt32(item.IdStage)))
                                        {
                                            DataTableForGridLayout.Columns.Add("Expected_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Expected_" + Convert.ToString(item.IdStage), HeaderText = "Expected", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.ExpectedTime });
                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("Expected_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Expected_" + Convert.ToString(item.IdStage), HeaderText = "Expected", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProductionExpected });
                                        }
                                        #endregion
                                        #region [gulab lakade][11 03 2024][GEOS2-5466]
                                        //rajashri[GEOS2-5799]
                                        if (AppSettingData.Contains(Convert.ToInt32(item.IdStage)))
                                        {
                                            DataTableForGridLayout.Columns.Add("Production_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Production_" + Convert.ToString(item.IdStage), HeaderText = "Production", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Production });

                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("Production_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Production_" + Convert.ToString(item.IdStage), HeaderText = "Production", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProductionWithoutCOMCADCAM });
                                        }
                                        //DataTableForGridLayout.Columns.Add("POWS_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "POWS_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Production-OWS", Width = 116, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PrOWS });
                                        #region [rani dhamankar][20-02-2025][GEOS2-6685]
                                        if (AppSettingData.Contains(Convert.ToInt32(item.IdStage)))
                                        {
                                            DataTableForGridLayout.Columns.Add("POWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "POWS_" + Convert.ToString(item.IdStage), HeaderText = "Production-OWS", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PrOWS });

                                            DataTableForGridLayout.Columns.Add("Rework_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Rework_" + Convert.ToString(item.IdStage), HeaderText = "Rework", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Rework });

                                            DataTableForGridLayout.Columns.Add("ROWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "ROWS_" + Convert.ToString(item.IdStage), HeaderText = "Rework-OWS", Width = 95, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.ReOWS });


                                            //string fieldName10 = "Real_" + Convert.ToString(timeTrackingStage.IdStage);
                                            DataTableForGridLayout.Columns.Add("Real_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Real_" + Convert.ToString(item.IdStage), HeaderText = "Real", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProduction });
                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("POWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "POWS_" + Convert.ToString(item.IdStage), HeaderText = "Production-OWS", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicPOWSWithoutCOMCADCAM });
                                            DataTableForGridLayout.Columns.Add("Rework_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Rework_" + Convert.ToString(item.IdStage), HeaderText = "Rework", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicReworkWithoutCOMCADCAM });

                                            DataTableForGridLayout.Columns.Add("ROWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "ROWS_" + Convert.ToString(item.IdStage), HeaderText = "Rework-OWS", Width = 95, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicROWSWithoutCOMCADCAM });


                                            //string fieldName10 = "Real_" + Convert.ToString(timeTrackingStage.IdStage);
                                            DataTableForGridLayout.Columns.Add("Real_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Real_" + Convert.ToString(item.IdStage), HeaderText = "Real", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicRealWithoutCOMCADCAM });
                                        }
                                        #endregion
                                        //DataTableForGridLayout.Columns.Add("Rework_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Rework_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Rework", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Rework });

                                        //DataTableForGridLayout.Columns.Add("ROWS_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "ROWS_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Rework-OWS", Width = 95, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.ReOWS });

                                        #endregion
                                        ////string fieldName10 = "Real_" + Convert.ToString(timeTrackingStage.IdStage);
                                        //DataTableForGridLayout.Columns.Add("Real_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Real_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Real", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProduction });

                                        //string fieldName11 = "Remaining_" + Convert.ToString(timeTrackingStage.IdStage);
                                        DataTableForGridLayout.Columns.Add("Remaining_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                        band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Remaining_" + Convert.ToString(item.IdStage), HeaderText = "Remaining", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.RemainingTime21 });
                                        DataTableForGridLayout.Columns.Add("Tempcolor_" + Convert.ToString(item.IdStage), typeof(bool));
                                        break;
                                    case 26:
                                        #region [GEOS2-5320][pallavi jadhav][15 10 2024]

                                        #region [Geos2-6786][gulab lakade][30 12 2024]
                                        if (isMatch == false && IscheckplantInSetting == true)
                                        {
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDateHtmlColor_" + Convert.ToString(item.IdStage), typeof(string));
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDate_" + Convert.ToString(item.IdStage), typeof(DateTime));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "PlannedDeliveryDate_" + Convert.ToString(item.IdStage), HeaderText = "Planned Delivery Date", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PlannedDeliveryDate2, Visible = false });
                                            DataTableForGridLayout.Columns.Add("Days_" + Convert.ToString(item.IdStage), typeof(int));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Days_" + Convert.ToString(item.IdStage), HeaderText = "Days", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Days, Visible = false });

                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDateHtmlColor_" + Convert.ToString(item.IdStage), typeof(string));
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDate_" + Convert.ToString(item.IdStage), typeof(DateTime));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "PlannedDeliveryDate_" + Convert.ToString(item.IdStage), HeaderText = "Planned Delivery Date", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PlannedDeliveryDate2, Visible = true });
                                            DataTableForGridLayout.Columns.Add("Days_" + Convert.ToString(item.IdStage), typeof(int));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Days_" + Convert.ToString(item.IdStage), HeaderText = "Days", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Days, Visible = true });

                                        }
                                        #endregion
                                        #endregion
                                        #region [GEOS2-4252] [gulab lakade][06 03 2023]
                                        if (AppSettingData.Contains(Convert.ToInt32(item.IdStage)))
                                        {
                                            DataTableForGridLayout.Columns.Add("Expected_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Expected_" + Convert.ToString(item.IdStage), HeaderText = "Expected", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.ExpectedTime });
                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("Expected_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Expected_" + Convert.ToString(item.IdStage), HeaderText = "Expected", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProductionExpected });
                                        }
                                        #endregion
                                        #region [gulab lakade][11 03 2024][GEOS2-5466]
                                        //rajashri[GEOS2-5799]
                                        if (AppSettingData.Contains(Convert.ToInt32(item.IdStage)))
                                        {
                                            DataTableForGridLayout.Columns.Add("Production_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Production_" + Convert.ToString(item.IdStage), HeaderText = "Production", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Production });

                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("Production_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Production_" + Convert.ToString(item.IdStage), HeaderText = "Production", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProductionWithoutCOMCADCAM });
                                        }
                                        //DataTableForGridLayout.Columns.Add("POWS_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "POWS_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Production-OWS", Width = 116, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PrOWS });
                                        #region [rani dhamankar][20-02-2025][GEOS2-6685]
                                        if (AppSettingData.Contains(Convert.ToInt32(item.IdStage)))
                                        {
                                            DataTableForGridLayout.Columns.Add("POWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "POWS_" + Convert.ToString(item.IdStage), HeaderText = "Production-OWS", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PrOWS });

                                            DataTableForGridLayout.Columns.Add("Rework_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Rework_" + Convert.ToString(item.IdStage), HeaderText = "Rework", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Rework });

                                            DataTableForGridLayout.Columns.Add("ROWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "ROWS_" + Convert.ToString(item.IdStage), HeaderText = "Rework-OWS", Width = 95, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.ReOWS });

                                            //#endregion
                                            //string fieldName10 = "Real_" + Convert.ToString(timeTrackingStage.IdStage);
                                            DataTableForGridLayout.Columns.Add("Real_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Real_" + Convert.ToString(item.IdStage), HeaderText = "Real", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProduction });
                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("POWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "POWS_" + Convert.ToString(item.IdStage), HeaderText = "Production-OWS", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicPOWSWithoutCOMCADCAM });
                                            DataTableForGridLayout.Columns.Add("Rework_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Rework_" + Convert.ToString(item.IdStage), HeaderText = "Rework", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicReworkWithoutCOMCADCAM });

                                            DataTableForGridLayout.Columns.Add("ROWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "ROWS_" + Convert.ToString(item.IdStage), HeaderText = "Rework-OWS", Width = 95, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicROWSWithoutCOMCADCAM });

                                            //#endregion
                                            //string fieldName10 = "Real_" + Convert.ToString(timeTrackingStage.IdStage);
                                            DataTableForGridLayout.Columns.Add("Real_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Real_" + Convert.ToString(item.IdStage), HeaderText = "Real", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicRealWithoutCOMCADCAM });
                                        }
                                        #endregion
                                        //DataTableForGridLayout.Columns.Add("Rework_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Rework_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Rework", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Rework });

                                        //DataTableForGridLayout.Columns.Add("ROWS_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "ROWS_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Rework-OWS", Width = 95, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.ReOWS });

                                        #endregion
                                        ////string fieldName10 = "Real_" + Convert.ToString(timeTrackingStage.IdStage);
                                        //DataTableForGridLayout.Columns.Add("Real_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Real_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Real", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProduction });

                                        //string fieldName11 = "Remaining_" + Convert.ToString(timeTrackingStage.IdStage);
                                        DataTableForGridLayout.Columns.Add("Remaining_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                        band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Remaining_" + Convert.ToString(item.IdStage), HeaderText = "Remaining", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.RemainingTime26 });
                                        DataTableForGridLayout.Columns.Add("Tempcolor_" + Convert.ToString(item.IdStage), typeof(bool));
                                        break;
                                    case 27:
                                        #region [GEOS2-5320][pallavi jadhav][15 10 2024]

                                        #region [Geos2-6786][gulab lakade][30 12 2024]
                                        if (isMatch == false && IscheckplantInSetting == true)
                                        {
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDateHtmlColor_" + Convert.ToString(item.IdStage), typeof(string));
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDate_" + Convert.ToString(item.IdStage), typeof(DateTime));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "PlannedDeliveryDate_" + Convert.ToString(item.IdStage), HeaderText = "Planned Delivery Date", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PlannedDeliveryDate2, Visible = false });
                                            DataTableForGridLayout.Columns.Add("Days_" + Convert.ToString(item.IdStage), typeof(int));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Days_" + Convert.ToString(item.IdStage), HeaderText = "Days", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Days, Visible = false });

                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDateHtmlColor_" + Convert.ToString(item.IdStage), typeof(string));
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDate_" + Convert.ToString(item.IdStage), typeof(DateTime));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "PlannedDeliveryDate_" + Convert.ToString(item.IdStage), HeaderText = "Planned Delivery Date", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PlannedDeliveryDate2, Visible = true });
                                            DataTableForGridLayout.Columns.Add("Days_" + Convert.ToString(item.IdStage), typeof(int));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Days_" + Convert.ToString(item.IdStage), HeaderText = "Days", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Days, Visible = true });

                                        }
                                        #endregion
                                        #endregion
                                        #region [GEOS2-4252] [gulab lakade][06 03 2023]
                                        if (AppSettingData.Contains(Convert.ToInt32(item.IdStage)))
                                        {
                                            DataTableForGridLayout.Columns.Add("Expected_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Expected_" + Convert.ToString(item.IdStage), HeaderText = "Expected", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.ExpectedTime });
                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("Expected_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Expected_" + Convert.ToString(item.IdStage), HeaderText = "Expected", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProductionExpected });
                                        }
                                        #endregion
                                        #region [gulab lakade][11 03 2024][GEOS2-5466]
                                        //rajashri[GEOS2-5799]
                                        if (AppSettingData.Contains(Convert.ToInt32(item.IdStage)))
                                        {
                                            DataTableForGridLayout.Columns.Add("Production_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Production_" + Convert.ToString(item.IdStage), HeaderText = "Production", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Production });

                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("Production_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Production_" + Convert.ToString(item.IdStage), HeaderText = "Production", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProductionWithoutCOMCADCAM });
                                        }
                                        //DataTableForGridLayout.Columns.Add("POWS_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "POWS_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Production-OWS", Width = 116, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PrOWS });
                                        #region [rani dhamankar][20-02-2025][GEOS2-6685]
                                        if (AppSettingData.Contains(Convert.ToInt32(item.IdStage)))
                                        {
                                            DataTableForGridLayout.Columns.Add("POWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "POWS_" + Convert.ToString(item.IdStage), HeaderText = "Production-OWS", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PrOWS });

                                            DataTableForGridLayout.Columns.Add("Rework_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Rework_" + Convert.ToString(item.IdStage), HeaderText = "Rework", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Rework });

                                            DataTableForGridLayout.Columns.Add("ROWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "ROWS_" + Convert.ToString(item.IdStage), HeaderText = "Rework-OWS", Width = 95, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.ReOWS });


                                            //string fieldName10 = "Real_" + Convert.ToString(timeTrackingStage.IdStage);
                                            DataTableForGridLayout.Columns.Add("Real_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Real_" + Convert.ToString(item.IdStage), HeaderText = "Real", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProduction });
                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("POWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "POWS_" + Convert.ToString(item.IdStage), HeaderText = "Production-OWS", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicPOWSWithoutCOMCADCAM });
                                            DataTableForGridLayout.Columns.Add("Rework_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Rework_" + Convert.ToString(item.IdStage), HeaderText = "Rework", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicReworkWithoutCOMCADCAM });

                                            DataTableForGridLayout.Columns.Add("ROWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "ROWS_" + Convert.ToString(item.IdStage), HeaderText = "Rework-OWS", Width = 95, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicROWSWithoutCOMCADCAM });


                                            //string fieldName10 = "Real_" + Convert.ToString(timeTrackingStage.IdStage);
                                            DataTableForGridLayout.Columns.Add("Real_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Real_" + Convert.ToString(item.IdStage), HeaderText = "Real", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicRealWithoutCOMCADCAM });
                                        }
                                        #endregion
                                        //DataTableForGridLayout.Columns.Add("Rework_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Rework_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Rework", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Rework });

                                        //DataTableForGridLayout.Columns.Add("ROWS_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "ROWS_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Rework-OWS", Width = 95, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.ReOWS });

                                        #endregion
                                        ////string fieldName10 = "Real_" + Convert.ToString(timeTrackingStage.IdStage);
                                        //DataTableForGridLayout.Columns.Add("Real_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Real_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Real", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProduction });

                                        //string fieldName11 = "Remaining_" + Convert.ToString(timeTrackingStage.IdStage);
                                        DataTableForGridLayout.Columns.Add("Remaining_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                        band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Remaining_" + Convert.ToString(item.IdStage), HeaderText = "Remaining", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.RemainingTime27 });
                                        DataTableForGridLayout.Columns.Add("Tempcolor_" + Convert.ToString(item.IdStage), typeof(bool));
                                        break;
                                    case 28:
                                        #region [GEOS2-5320][pallavi jadhav][15 10 2024]

                                        #region [Geos2-6786][gulab lakade][30 12 2024]
                                        if (isMatch == false && IscheckplantInSetting == true)
                                        {
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDateHtmlColor_" + Convert.ToString(item.IdStage), typeof(string));
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDate_" + Convert.ToString(item.IdStage), typeof(DateTime));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "PlannedDeliveryDate_" + Convert.ToString(item.IdStage), HeaderText = "Planned Delivery Date", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PlannedDeliveryDate2, Visible = false });
                                            DataTableForGridLayout.Columns.Add("Days_" + Convert.ToString(item.IdStage), typeof(int));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Days_" + Convert.ToString(item.IdStage), HeaderText = "Days", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Days, Visible = false });

                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDateHtmlColor_" + Convert.ToString(item.IdStage), typeof(string));
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDate_" + Convert.ToString(item.IdStage), typeof(DateTime));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "PlannedDeliveryDate_" + Convert.ToString(item.IdStage), HeaderText = "Planned Delivery Date", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PlannedDeliveryDate2, Visible = true });
                                            DataTableForGridLayout.Columns.Add("Days_" + Convert.ToString(item.IdStage), typeof(int));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Days_" + Convert.ToString(item.IdStage), HeaderText = "Days", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Days, Visible = true });

                                        }
                                        #endregion
                                        #endregion
                                        #region [GEOS2-4252] [gulab lakade][06 03 2023]
                                        if (AppSettingData.Contains(Convert.ToInt32(item.IdStage)))
                                        {
                                            DataTableForGridLayout.Columns.Add("Expected_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Expected_" + Convert.ToString(item.IdStage), HeaderText = "Expected", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.ExpectedTime });
                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("Expected_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Expected_" + Convert.ToString(item.IdStage), HeaderText = "Expected", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProductionExpected });
                                        }
                                        #endregion
                                        #region [gulab lakade][11 03 2024][GEOS2-5466]
                                        //rajashri[GEOS2-5799]
                                        if (AppSettingData.Contains(Convert.ToInt32(item.IdStage)))
                                        {
                                            DataTableForGridLayout.Columns.Add("Production_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Production_" + Convert.ToString(item.IdStage), HeaderText = "Production", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Production });

                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("Production_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Production_" + Convert.ToString(item.IdStage), HeaderText = "Production", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProductionWithoutCOMCADCAM });
                                        }
                                        //DataTableForGridLayout.Columns.Add("POWS_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "POWS_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Production-OWS", Width = 116, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PrOWS });
                                        #region [rani dhamankar][20-02-2025][GEOS2-6685]
                                        if (AppSettingData.Contains(Convert.ToInt32(item.IdStage)))
                                        {
                                            DataTableForGridLayout.Columns.Add("POWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "POWS_" + Convert.ToString(item.IdStage), HeaderText = "Production-OWS", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PrOWS });

                                            DataTableForGridLayout.Columns.Add("Rework_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Rework_" + Convert.ToString(item.IdStage), HeaderText = "Rework", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Rework });

                                            DataTableForGridLayout.Columns.Add("ROWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "ROWS_" + Convert.ToString(item.IdStage), HeaderText = "Rework-OWS", Width = 95, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.ReOWS });


                                            //string fieldName10 = "Real_" + Convert.ToString(timeTrackingStage.IdStage);
                                            DataTableForGridLayout.Columns.Add("Real_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Real_" + Convert.ToString(item.IdStage), HeaderText = "Real", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProduction });
                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("POWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "POWS_" + Convert.ToString(item.IdStage), HeaderText = "Production-OWS", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicPOWSWithoutCOMCADCAM });
                                            DataTableForGridLayout.Columns.Add("Rework_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Rework_" + Convert.ToString(item.IdStage), HeaderText = "Rework", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicReworkWithoutCOMCADCAM });

                                            DataTableForGridLayout.Columns.Add("ROWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "ROWS_" + Convert.ToString(item.IdStage), HeaderText = "Rework-OWS", Width = 95, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicROWSWithoutCOMCADCAM });


                                            //string fieldName10 = "Real_" + Convert.ToString(timeTrackingStage.IdStage);
                                            DataTableForGridLayout.Columns.Add("Real_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Real_" + Convert.ToString(item.IdStage), HeaderText = "Real", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicRealWithoutCOMCADCAM });
                                        }
                                        #endregion
                                        //DataTableForGridLayout.Columns.Add("Rework_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Rework_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Rework", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Rework });

                                        //DataTableForGridLayout.Columns.Add("ROWS_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "ROWS_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Rework-OWS", Width = 95, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.ReOWS });

                                        #endregion
                                        ////string fieldName10 = "Real_" + Convert.ToString(timeTrackingStage.IdStage);
                                        //DataTableForGridLayout.Columns.Add("Real_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Real_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Real", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProduction });

                                        //string fieldName11 = "Remaining_" + Convert.ToString(timeTrackingStage.IdStage);
                                        DataTableForGridLayout.Columns.Add("Remaining_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                        band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Remaining_" + Convert.ToString(item.IdStage), HeaderText = "Remaining", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.RemainingTime28 });
                                        DataTableForGridLayout.Columns.Add("Tempcolor_" + Convert.ToString(item.IdStage), typeof(bool));
                                        break;
                                    case 33:
                                        #region [GEOS2-5320][pallavi jadhav][15 10 2024]

                                        #region [Geos2-6786][gulab lakade][30 12 2024]
                                        if (isMatch == false && IscheckplantInSetting == true)
                                        {
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDateHtmlColor_" + Convert.ToString(item.IdStage), typeof(string));
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDate_" + Convert.ToString(item.IdStage), typeof(DateTime));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "PlannedDeliveryDate_" + Convert.ToString(item.IdStage), HeaderText = "Planned Delivery Date", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PlannedDeliveryDate2, Visible = false });
                                            DataTableForGridLayout.Columns.Add("Days_" + Convert.ToString(item.IdStage), typeof(int));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Days_" + Convert.ToString(item.IdStage), HeaderText = "Days", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Days, Visible = false });

                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDateHtmlColor_" + Convert.ToString(item.IdStage), typeof(string));
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDate_" + Convert.ToString(item.IdStage), typeof(DateTime));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "PlannedDeliveryDate_" + Convert.ToString(item.IdStage), HeaderText = "Planned Delivery Date", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PlannedDeliveryDate2, Visible = true });
                                            DataTableForGridLayout.Columns.Add("Days_" + Convert.ToString(item.IdStage), typeof(int));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Days_" + Convert.ToString(item.IdStage), HeaderText = "Days", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Days, Visible = true });

                                        }
                                        #endregion
                                        #endregion
                                        #region [GEOS2-4252] [gulab lakade][06 03 2023]
                                        if (AppSettingData.Contains(Convert.ToInt32(item.IdStage)))
                                        {
                                            DataTableForGridLayout.Columns.Add("Expected_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Expected_" + Convert.ToString(item.IdStage), HeaderText = "Expected", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.ExpectedTime });
                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("Expected_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Expected_" + Convert.ToString(item.IdStage), HeaderText = "Expected", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProductionExpected });
                                        }
                                        #endregion
                                        #region [gulab lakade][11 03 2024][GEOS2-5466]
                                        //rajashri[GEOS2-5799]
                                        if (AppSettingData.Contains(Convert.ToInt32(item.IdStage)))
                                        {
                                            DataTableForGridLayout.Columns.Add("Production_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Production_" + Convert.ToString(item.IdStage), HeaderText = "Production", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Production });

                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("Production_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Production_" + Convert.ToString(item.IdStage), HeaderText = "Production", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProductionWithoutCOMCADCAM });
                                        }
                                        //DataTableForGridLayout.Columns.Add("POWS_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "POWS_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Production-OWS", Width = 116, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PrOWS });
                                        #region [rani dhamankar][20-02-2025][GEOS2-6685]
                                        if (AppSettingData.Contains(Convert.ToInt32(item.IdStage)))
                                        {
                                            DataTableForGridLayout.Columns.Add("POWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "POWS_" + Convert.ToString(item.IdStage), HeaderText = "Production-OWS", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PrOWS });

                                            DataTableForGridLayout.Columns.Add("Rework_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Rework_" + Convert.ToString(item.IdStage), HeaderText = "Rework", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Rework });

                                            DataTableForGridLayout.Columns.Add("ROWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "ROWS_" + Convert.ToString(item.IdStage), HeaderText = "Rework-OWS", Width = 95, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.ReOWS });


                                            //string fieldName10 = "Real_" + Convert.ToString(timeTrackingStage.IdStage);
                                            DataTableForGridLayout.Columns.Add("Real_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Real_" + Convert.ToString(item.IdStage), HeaderText = "Real", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProduction });
                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("POWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "POWS_" + Convert.ToString(item.IdStage), HeaderText = "Production-OWS", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicPOWSWithoutCOMCADCAM });
                                            DataTableForGridLayout.Columns.Add("Rework_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Rework_" + Convert.ToString(item.IdStage), HeaderText = "Rework", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicReworkWithoutCOMCADCAM });

                                            DataTableForGridLayout.Columns.Add("ROWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "ROWS_" + Convert.ToString(item.IdStage), HeaderText = "Rework-OWS", Width = 95, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicROWSWithoutCOMCADCAM });


                                            //string fieldName10 = "Real_" + Convert.ToString(timeTrackingStage.IdStage);
                                            DataTableForGridLayout.Columns.Add("Real_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Real_" + Convert.ToString(item.IdStage), HeaderText = "Real", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicRealWithoutCOMCADCAM });
                                        }
                                        #endregion
                                        //DataTableForGridLayout.Columns.Add("Rework_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Rework_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Rework", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Rework });

                                        //DataTableForGridLayout.Columns.Add("ROWS_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "ROWS_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Rework-OWS", Width = 95, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.ReOWS });

                                        #endregion
                                        ////string fieldName10 = "Real_" + Convert.ToString(timeTrackingStage.IdStage);
                                        //DataTableForGridLayout.Columns.Add("Real_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Real_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Real", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProduction });

                                        //string fieldName11 = "Remaining_" + Convert.ToString(timeTrackingStage.IdStage);
                                        DataTableForGridLayout.Columns.Add("Remaining_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                        band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Remaining_" + Convert.ToString(item.IdStage), HeaderText = "Remaining", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.RemainingTime33 });
                                        DataTableForGridLayout.Columns.Add("Tempcolor_" + Convert.ToString(item.IdStage), typeof(bool));
                                        break;
                                    case 35:
                                        #region [GEOS2-5320][pallavi jadhav][15 10 2024]

                                        #region [Geos2-6786][gulab lakade][30 12 2024]
                                        if (isMatch == false && IscheckplantInSetting == true)
                                        {
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDateHtmlColor_" + Convert.ToString(item.IdStage), typeof(string));
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDate_" + Convert.ToString(item.IdStage), typeof(DateTime));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "PlannedDeliveryDate_" + Convert.ToString(item.IdStage), HeaderText = "Planned Delivery Date", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PlannedDeliveryDate2, Visible = false });
                                            DataTableForGridLayout.Columns.Add("Days_" + Convert.ToString(item.IdStage), typeof(int));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Days_" + Convert.ToString(item.IdStage), HeaderText = "Days", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Days, Visible = false });

                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDateHtmlColor_" + Convert.ToString(item.IdStage), typeof(string));
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDate_" + Convert.ToString(item.IdStage), typeof(DateTime));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "PlannedDeliveryDate_" + Convert.ToString(item.IdStage), HeaderText = "Planned Delivery Date", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PlannedDeliveryDate2, Visible = true });
                                            DataTableForGridLayout.Columns.Add("Days_" + Convert.ToString(item.IdStage), typeof(int));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Days_" + Convert.ToString(item.IdStage), HeaderText = "Days", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Days, Visible = true });

                                        }
                                        #endregion
                                        #endregion
                                        #region [GEOS2-4252] [gulab lakade][06 03 2023]
                                        if (AppSettingData.Contains(Convert.ToInt32(item.IdStage)))
                                        {
                                            DataTableForGridLayout.Columns.Add("Expected_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Expected_" + Convert.ToString(item.IdStage), HeaderText = "Expected", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.ExpectedTime });
                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("Expected_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Expected_" + Convert.ToString(item.IdStage), HeaderText = "Expected", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProductionExpected });
                                        }
                                        #endregion
                                        #region [gulab lakade][11 03 2024][GEOS2-5466]
                                        //rajashri[GEOS2-5799]
                                        if (AppSettingData.Contains(Convert.ToInt32(item.IdStage)))
                                        {
                                            DataTableForGridLayout.Columns.Add("Production_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Production_" + Convert.ToString(item.IdStage), HeaderText = "Production", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Production });

                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("Production_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Production_" + Convert.ToString(item.IdStage), HeaderText = "Production", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProductionWithoutCOMCADCAM });
                                        }
                                        //DataTableForGridLayout.Columns.Add("POWS_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "POWS_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Production-OWS", Width = 116, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PrOWS });
                                        #region [rani dhamankar][20-02-2025][GEOS2-6685]
                                        if (AppSettingData.Contains(Convert.ToInt32(item.IdStage)))
                                        {
                                            DataTableForGridLayout.Columns.Add("POWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "POWS_" + Convert.ToString(item.IdStage), HeaderText = "Production-OWS", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PrOWS });

                                            DataTableForGridLayout.Columns.Add("Rework_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Rework_" + Convert.ToString(item.IdStage), HeaderText = "Rework", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Rework });

                                            DataTableForGridLayout.Columns.Add("ROWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "ROWS_" + Convert.ToString(item.IdStage), HeaderText = "Rework-OWS", Width = 95, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.ReOWS });


                                            //string fieldName10 = "Real_" + Convert.ToString(timeTrackingStage.IdStage);
                                            DataTableForGridLayout.Columns.Add("Real_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Real_" + Convert.ToString(item.IdStage), HeaderText = "Real", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProduction });
                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("POWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "POWS_" + Convert.ToString(item.IdStage), HeaderText = "Production-OWS", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicPOWSWithoutCOMCADCAM });
                                            DataTableForGridLayout.Columns.Add("Rework_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Rework_" + Convert.ToString(item.IdStage), HeaderText = "Rework", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicReworkWithoutCOMCADCAM });

                                            DataTableForGridLayout.Columns.Add("ROWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "ROWS_" + Convert.ToString(item.IdStage), HeaderText = "Rework-OWS", Width = 95, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicROWSWithoutCOMCADCAM });

                                            //string fieldName10 = "Real_" + Convert.ToString(timeTrackingStage.IdStage);
                                            DataTableForGridLayout.Columns.Add("Real_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Real_" + Convert.ToString(item.IdStage), HeaderText = "Real", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicRealWithoutCOMCADCAM });
                                        }
                                        #endregion
                                        //DataTableForGridLayout.Columns.Add("Rework_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Rework_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Rework", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Rework });

                                        //DataTableForGridLayout.Columns.Add("ROWS_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "ROWS_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Rework-OWS", Width = 95, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.ReOWS });

                                        #endregion
                                        //string fieldName10 = "Real_" + Convert.ToString(timeTrackingStage.IdStage);
                                        //DataTableForGridLayout.Columns.Add("Real_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Real_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Real", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProduction });

                                        //string fieldName11 = "Remaining_" + Convert.ToString(timeTrackingStage.IdStage);
                                        DataTableForGridLayout.Columns.Add("Remaining_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                        band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Remaining_" + Convert.ToString(item.IdStage), HeaderText = "Remaining", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.RemainingTime35 });
                                        DataTableForGridLayout.Columns.Add("Tempcolor_" + Convert.ToString(item.IdStage), typeof(bool));
                                        break;
                                    case 37:
                                        #region [GEOS2-5320][pallavi jadhav][15 10 2024]

                                        #region [Geos2-6786][gulab lakade][30 12 2024]
                                        if (isMatch == false && IscheckplantInSetting == true)
                                        {
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDateHtmlColor_" + Convert.ToString(item.IdStage), typeof(string));
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDate_" + Convert.ToString(item.IdStage), typeof(DateTime));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "PlannedDeliveryDate_" + Convert.ToString(item.IdStage), HeaderText = "Planned Delivery Date", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PlannedDeliveryDate2, Visible = false });
                                            DataTableForGridLayout.Columns.Add("Days_" + Convert.ToString(item.IdStage), typeof(int));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Days_" + Convert.ToString(item.IdStage), HeaderText = "Days", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Days, Visible = false });

                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDateHtmlColor_" + Convert.ToString(item.IdStage), typeof(string));
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDate_" + Convert.ToString(item.IdStage), typeof(DateTime));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "PlannedDeliveryDate_" + Convert.ToString(item.IdStage), HeaderText = "Planned Delivery Date", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PlannedDeliveryDate2, Visible = true });
                                            DataTableForGridLayout.Columns.Add("Days_" + Convert.ToString(item.IdStage), typeof(int));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Days_" + Convert.ToString(item.IdStage), HeaderText = "Days", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Days, Visible = true });

                                        }
                                        #endregion
                                        #endregion
                                        #region [GEOS2-4252] [gulab lakade][06 03 2023]
                                        if (AppSettingData.Contains(Convert.ToInt32(item.IdStage)))
                                        {
                                            DataTableForGridLayout.Columns.Add("Expected_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Expected_" + Convert.ToString(item.IdStage), HeaderText = "Expected", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.ExpectedTime });
                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("Expected_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Expected_" + Convert.ToString(item.IdStage), HeaderText = "Expected", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProductionExpected });
                                        }
                                        #endregion
                                        #region [gulab lakade][11 03 2024][GEOS2-5466]
                                        //rajashri[GEOS2-5799]
                                        if (AppSettingData.Contains(Convert.ToInt32(item.IdStage)))
                                        {
                                            DataTableForGridLayout.Columns.Add("Production_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Production_" + Convert.ToString(item.IdStage), HeaderText = "Production", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Production });

                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("Production_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Production_" + Convert.ToString(item.IdStage), HeaderText = "Production", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProductionWithoutCOMCADCAM });
                                        }
                                        //DataTableForGridLayout.Columns.Add("POWS_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "POWS_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Production-OWS", Width = 116, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PrOWS });
                                        #region [rani dhamankar][20-02-2025][GEOS2-6685]
                                        if (AppSettingData.Contains(Convert.ToInt32(item.IdStage)))
                                        {
                                            DataTableForGridLayout.Columns.Add("POWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "POWS_" + Convert.ToString(item.IdStage), HeaderText = "Production-OWS", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PrOWS });

                                            DataTableForGridLayout.Columns.Add("Rework_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Rework_" + Convert.ToString(item.IdStage), HeaderText = "Rework", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Rework });

                                            DataTableForGridLayout.Columns.Add("ROWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "ROWS_" + Convert.ToString(item.IdStage), HeaderText = "Rework-OWS", Width = 95, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.ReOWS });


                                            //string fieldName10 = "Real_" + Convert.ToString(timeTrackingStage.IdStage);
                                            DataTableForGridLayout.Columns.Add("Real_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Real_" + Convert.ToString(item.IdStage), HeaderText = "Real", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProduction });
                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("POWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "POWS_" + Convert.ToString(item.IdStage), HeaderText = "Production-OWS", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicPOWSWithoutCOMCADCAM });
                                            DataTableForGridLayout.Columns.Add("Rework_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Rework_" + Convert.ToString(item.IdStage), HeaderText = "Rework", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicReworkWithoutCOMCADCAM });

                                            DataTableForGridLayout.Columns.Add("ROWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "ROWS_" + Convert.ToString(item.IdStage), HeaderText = "Rework-OWS", Width = 95, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicROWSWithoutCOMCADCAM });


                                            //string fieldName10 = "Real_" + Convert.ToString(timeTrackingStage.IdStage);
                                            DataTableForGridLayout.Columns.Add("Real_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Real_" + Convert.ToString(item.IdStage), HeaderText = "Real", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicRealWithoutCOMCADCAM });
                                        }
                                        #endregion
                                        //DataTableForGridLayout.Columns.Add("Rework_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Rework_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Rework", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Rework });

                                        //DataTableForGridLayout.Columns.Add("ROWS_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "ROWS_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Rework-OWS", Width = 95, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.ReOWS });

                                        #endregion
                                        //string fieldName10 = "Real_" + Convert.ToString(timeTrackingStage.IdStage);
                                        //DataTableForGridLayout.Columns.Add("Real_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Real_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Real", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProduction });

                                        //string fieldName11 = "Remaining_" + Convert.ToString(timeTrackingStage.IdStage);
                                        DataTableForGridLayout.Columns.Add("Remaining_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                        band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Remaining_" + Convert.ToString(item.IdStage), HeaderText = "Remaining", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.RemainingTime37 });
                                        DataTableForGridLayout.Columns.Add("Tempcolor_" + Convert.ToString(item.IdStage), typeof(bool));
                                        break;
                                    default:
                                        #region [GEOS2-5320][pallavi jadhav][15 10 2024]

                                        #region [Geos2-6786][gulab lakade][30 12 2024]
                                        if (isMatch == false && IscheckplantInSetting == true)
                                        {
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDateHtmlColor_" + Convert.ToString(item.IdStage), typeof(string));
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDate_" + Convert.ToString(item.IdStage), typeof(DateTime));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "PlannedDeliveryDate_" + Convert.ToString(item.IdStage), HeaderText = "Planned Delivery Date", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PlannedDeliveryDate2, Visible = false });
                                            DataTableForGridLayout.Columns.Add("Days_" + Convert.ToString(item.IdStage), typeof(int));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Days_" + Convert.ToString(item.IdStage), HeaderText = "Days", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Days, Visible = false });

                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDateHtmlColor_" + Convert.ToString(item.IdStage), typeof(string));
                                            DataTableForGridLayout.Columns.Add("PlannedDeliveryDate_" + Convert.ToString(item.IdStage), typeof(DateTime));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "PlannedDeliveryDate_" + Convert.ToString(item.IdStage), HeaderText = "Planned Delivery Date", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PlannedDeliveryDate2, Visible = true });
                                            DataTableForGridLayout.Columns.Add("Days_" + Convert.ToString(item.IdStage), typeof(int));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Days_" + Convert.ToString(item.IdStage), HeaderText = "Days", Width = 110, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Days, Visible = true });

                                        }
                                        #endregion
                                        #endregion
                                        #region [GEOS2-4252] [gulab lakade][06 03 2023]
                                        if (AppSettingData.Contains(Convert.ToInt32(item.IdStage)))
                                        {
                                            DataTableForGridLayout.Columns.Add("Expected_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Expected_" + Convert.ToString(item.IdStage), HeaderText = "Expected", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.ExpectedTime });
                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("Expected_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Expected_" + Convert.ToString(item.IdStage), HeaderText = "Expected", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProductionExpected });
                                        }
                                        #endregion
                                        #region [gulab lakade][11 03 2024][GEOS2-5466]
                                        //rajashri[GEOS2-5799]
                                        if (AppSettingData.Contains(Convert.ToInt32(item.IdStage)))
                                        {
                                            DataTableForGridLayout.Columns.Add("Production_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Production_" + Convert.ToString(item.IdStage), HeaderText = "Production", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Production });

                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("Production_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Production_" + Convert.ToString(item.IdStage), HeaderText = "Production", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProductionWithoutCOMCADCAM });
                                        }
                                        //DataTableForGridLayout.Columns.Add("POWS_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "POWS_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Production-OWS", Width = 116, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PrOWS });
                                        #region [rani dhamankar][20-02-2025][GEOS2-6685]
                                        if (AppSettingData.Contains(Convert.ToInt32(item.IdStage)))
                                        {
                                            DataTableForGridLayout.Columns.Add("POWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "POWS_" + Convert.ToString(item.IdStage), HeaderText = "Production-OWS", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PrOWS });

                                            DataTableForGridLayout.Columns.Add("Rework_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Rework_" + Convert.ToString(item.IdStage), HeaderText = "Rework", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Rework });

                                            DataTableForGridLayout.Columns.Add("ROWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "ROWS_" + Convert.ToString(item.IdStage), HeaderText = "Rework-OWS", Width = 95, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.ReOWS });


                                            //string fieldName10 = "Real_" + Convert.ToString(timeTrackingStage.IdStage);
                                            DataTableForGridLayout.Columns.Add("Real_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Real_" + Convert.ToString(item.IdStage), HeaderText = "Real", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProduction });
                                        }
                                        else
                                        {
                                            DataTableForGridLayout.Columns.Add("POWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "POWS_" + Convert.ToString(item.IdStage), HeaderText = "Production-OWS", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicPOWSWithoutCOMCADCAM });
                                            DataTableForGridLayout.Columns.Add("Rework_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Rework_" + Convert.ToString(item.IdStage), HeaderText = "Rework", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicReworkWithoutCOMCADCAM });

                                            DataTableForGridLayout.Columns.Add("ROWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "ROWS_" + Convert.ToString(item.IdStage), HeaderText = "Rework-OWS", Width = 95, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicROWSWithoutCOMCADCAM });

                                            //string fieldName10 = "Real_" + Convert.ToString(timeTrackingStage.IdStage);
                                            DataTableForGridLayout.Columns.Add("Real_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                            band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Real_" + Convert.ToString(item.IdStage), HeaderText = "Real", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicRealWithoutCOMCADCAM });
                                        }
                                        #endregion
                                        //DataTableForGridLayout.Columns.Add("Rework_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Rework_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Rework", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.Rework });

                                        //DataTableForGridLayout.Columns.Add("ROWS_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "ROWS_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Rework-OWS", Width = 95, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.ReOWS });

                                        #endregion
                                        //string fieldName10 = "Real_" + Convert.ToString(timeTrackingStage.IdStage);
                                        //DataTableForGridLayout.Columns.Add("Real_" + Convert.ToString(timeTrackingStage.IdStage), typeof(TimeSpan));
                                        //band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Real_" + Convert.ToString(timeTrackingStage.IdStage), HeaderText = "Real", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProduction });

                                        //string fieldName11 = "Remaining_" + Convert.ToString(timeTrackingStage.IdStage);
                                        DataTableForGridLayout.Columns.Add("Remaining_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                        band4.Columns.Add(new ColumnItem() { ColumnFieldName = "Remaining_" + Convert.ToString(item.IdStage), HeaderText = "Remaining", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.RemainingTime });
                                        DataTableForGridLayout.Columns.Add("Tempcolor_" + Convert.ToString(item.IdStage), typeof(bool));
                                        break;


                                }


                                //    string fieldName9 = "Expected_" + Convert.ToString(timeTrackingStage.IdStage);
                                //DataTableForGridLayout.Columns.Add(fieldName9, typeof(TimeSpan));
                                //bandBU.Columns.Add(new ColumnItem() { ColumnFieldName = fieldName9, HeaderText = "Expected", Width = 100, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.SalePrice });

                                //string fieldName10 = "Real_" + Convert.ToString(timeTrackingStage.IdStage);
                                //DataTableForGridLayout.Columns.Add(fieldName10, typeof(TimeSpan));
                                //bandBU.Columns.Add(new ColumnItem() { ColumnFieldName = fieldName10, HeaderText = "Real", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.SalePrice });

                                //string fieldName11 = "Remaining_" + Convert.ToString(timeTrackingStage.IdStage);
                                //DataTableForGridLayout.Columns.Add(fieldName11, typeof(TimeSpan));
                                //bandBU.Columns.Add(new ColumnItem() { ColumnFieldName = fieldName11, HeaderText = "Remaining", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.RemainingTime });
                                Bands.Add(band4);
                                string fieldName9 = "Expected_" + Convert.ToString(item.IdStage);
                                string fieldName10 = "Real_" + Convert.ToString(item.IdStage);
                                string fieldName11 = "Remaining_" + Convert.ToString(item.IdStage);
                                string fieldName15 = "Production_" + Convert.ToString(item.IdStage);//[gulab lakade][11 03 2024][GEOS2-5466]
                                string fieldName21 = "POWS_" + Convert.ToString(item.IdStage);//rajashri GEOS2-6054
                                string fieldName16 = "Rework_" + Convert.ToString(item.IdStage);//[gulab lakade][11 03 2024][GEOS2-5466]
                                string fieldName22 = "ROWS_" + Convert.ToString(item.IdStage);//rajashri GEOS2-6054
                                //TotalSummary.Add(new Summary() { Type = SummaryItemType.Sum, FieldName = fieldName9, DisplayFormat = @" {0:hh\:mm\:ss}" });
                                //TotalSummary.Add(new Summary() { Type = SummaryItemType.Sum, FieldName = fieldName15, DisplayFormat = @" {0:hh\:mm\:ss}" });//[gulab lakade][11 03 2024][GEOS2-5466]
                                //TotalSummary.Add(new Summary() { Type = SummaryItemType.Sum, FieldName = fieldName16, DisplayFormat = @" {0:hh\:mm\:ss}" });//[gulab lakade][11 03 2024][GEOS2-5466]
                                //TotalSummary.Add(new Summary() { Type = SummaryItemType.Sum, FieldName = fieldName10, DisplayFormat = @" {0:hh\:mm\:ss}" });
                                //TotalSummary.Add(new Summary() { Type = SummaryItemType.Sum, FieldName = fieldName11, DisplayFormat = @" {0:hh\:mm\:ss}" });

                                //[GEOS2-5519][Rupali Sarode][15-04-2024]
                                TotalSummary.Add(new Summary() { Type = SummaryItemType.Sum, FieldName = fieldName9, DisplayFormat = @" {0:dd\.hh\:mm\:ss}" });
                                TotalSummary.Add(new Summary() { Type = SummaryItemType.Sum, FieldName = fieldName15, DisplayFormat = @" {0:dd\.hh\:mm\:ss}" });//[gulab lakade][11 03 2024][GEOS2-5466]
                                TotalSummary.Add(new Summary() { Type = SummaryItemType.Sum, FieldName = fieldName21, DisplayFormat = @" {0:dd\.hh\:mm\:ss}" });//rajashri GEOS2-6054
                                TotalSummary.Add(new Summary() { Type = SummaryItemType.Sum, FieldName = fieldName16, DisplayFormat = @" {0:dd\.hh\:mm\:ss}" });//[gulab lakade][11 03 2024][GEOS2-5466]
                                TotalSummary.Add(new Summary() { Type = SummaryItemType.Sum, FieldName = fieldName22, DisplayFormat = @" {0:dd\.hh\:mm\:ss}" });//rajashri GEOS2-6054
                                TotalSummary.Add(new Summary() { Type = SummaryItemType.Sum, FieldName = fieldName10, DisplayFormat = @" {0:dd\.hh\:mm\:ss}" });
                                TotalSummary.Add(new Summary() { Type = SummaryItemType.Sum, FieldName = fieldName11, DisplayFormat = @" {0:dd\.hh\:mm\:ss}" });

                            }
                        }
                    }
                }

                //total
                BandItem band5 = new BandItem()
                {
                    BandName = "Total",
                    BandHeader = "Total",
                    Visible = true
                };
                //BandItem band4 = new BandItem() { BandName = "Total", BandHeader = "Total", Visible = true };
                band5.Columns = new ObservableCollection<ColumnItem>();

                string fieldName12 = "Expected";
                DataTableForGridLayout.Columns.Add("Expected", typeof(TimeSpan));
                band5.Columns.Add(new ColumnItem() { ColumnFieldName = "Expected", HeaderText = "Expected", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProductionExpected });
                #region [gulab lakade][11 03 2024][GEOS2-5466]
                //rajashri GEOS2-5799   for total
                string fieldName17 = "Production";
                DataTableForGridLayout.Columns.Add(fieldName17, typeof(TimeSpan));
                band5.Columns.Add(new ColumnItem() { ColumnFieldName = fieldName17, HeaderText = "Production", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicProductionWithoutCOMCADCAM });

                //////rajashri GEOS2-6054
                //string fieldName19 = "POWS";
                //DataTableForGridLayout.Columns.Add(fieldName19, typeof(TimeSpan));
                //band5.Columns.Add(new ColumnItem() { ColumnFieldName = fieldName19, HeaderText = "Production-OWS", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.PrOWS });
                #region  // [rani dhamankar] [20/02/2025][GEOS2-6685]
                string fieldName19 = "POWS";
                DataTableForGridLayout.Columns.Add(fieldName19, typeof(TimeSpan));
                band5.Columns.Add(new ColumnItem() { ColumnFieldName = fieldName19, HeaderText = "Production-OWS", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicPOWSWithoutCOMCADCAM });

                string fieldName18 = "Rework";
                DataTableForGridLayout.Columns.Add(fieldName18, typeof(TimeSpan));
                band5.Columns.Add(new ColumnItem() { ColumnFieldName = fieldName18, HeaderText = "Rework", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicReworkWithoutCOMCADCAM });

                //////rajashri GEOS2-6054
                string fieldName20 = "ROWS";
                DataTableForGridLayout.Columns.Add(fieldName20, typeof(TimeSpan));
                band5.Columns.Add(new ColumnItem() { ColumnFieldName = fieldName20, HeaderText = "Rework-OWS", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicROWSWithoutCOMCADCAM });


                #endregion
                string fieldName13 = "Real";
                DataTableForGridLayout.Columns.Add(fieldName13, typeof(TimeSpan));
                band5.Columns.Add(new ColumnItem() { ColumnFieldName = fieldName13, HeaderText = "Real", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.DynamicRealWithoutCOMCADCAM });
                #endregion
                string fieldName14 = "Remaining";
                DataTableForGridLayout.Columns.Add(fieldName14, typeof(TimeSpan));
                band5.Columns.Add(new ColumnItem() { ColumnFieldName = fieldName14, HeaderText = "Remaining", Width = 80, Visible = true, IsVertical = false, TimetrackingSetting = TimeTrackingColumnTemplateSelector.TimetrackingSettingType.RemainingTime });

                DataTableForGridLayout.Columns.Add("Tempcolor", typeof(bool));
                //[GEOS2-4150] [Gulab Lakade] [30 01 2023]
                DataTableForGridLayout.Columns.Add("ExpectedHtmlColorFlag", typeof(string));
                DataTableForGridLayout.Columns.Add("ProductionHtmlColorFlag", typeof(string));
                //start [rani dhamankar][GEOS2-6685][19-02-2025]
                DataTableForGridLayout.Columns.Add("POWSHtmlColorFlag", typeof(string));
                DataTableForGridLayout.Columns.Add("ROWSHtmlColorFlag", typeof(string));
                DataTableForGridLayout.Columns.Add("ReworkHtmlColorFlag", typeof(string));
                DataTableForGridLayout.Columns.Add("RealHtmlColorFlag", typeof(string));
                DataTableForGridLayout.Columns.Add("RemainingHtmlColorFlag1", typeof(string));
                DataTableForGridLayout.Columns.Add("RemainingHtmlColorFlag2", typeof(string));
                DataTableForGridLayout.Columns.Add("RemainingHtmlColorFlag6", typeof(string));
                //end [rani dhamankar][GEOS2-6685][19-02-2025]
                #region GEOS2[8309] [rajashri telvekar] [7/11/2025]
                DataTableForGridLayout.Columns.Add("ExpectedHtmlColorFlag1", typeof(string));
                DataTableForGridLayout.Columns.Add("ProductionHtmlColorFlag1", typeof(string));
                DataTableForGridLayout.Columns.Add("POWSHtmlColorFlag1", typeof(string));
                DataTableForGridLayout.Columns.Add("ROWSHtmlColorFlag1", typeof(string));
                DataTableForGridLayout.Columns.Add("ReworkHtmlColorFlag1", typeof(string));
                DataTableForGridLayout.Columns.Add("RealHtmlColorFlag1", typeof(string));
                #region GEOS2-8309  rajashri
                DataTableForGridLayout.Columns.Add("DownloadHtmlColorFlag", typeof(string));
                DataTableForGridLayout.Columns.Add("TransfferdHtmlColorFlag", typeof(string));
                DataTableForGridLayout.Columns.Add("AddinHtmlColorFlag", typeof(string));
                DataTableForGridLayout.Columns.Add("PostServerHtmlColorFlag", typeof(string));
                DataTableForGridLayout.Columns.Add("EDSHtmlColorFlag", typeof(string));
                #endregion
                #endregion
                Bands.Add(band5);
                //  DataTable = DataTableForGridLayout;
                Bands = new ObservableCollection<BandItem>(Bands);

                TotalSummary.Add(new Summary() { Type = SummaryItemType.Sum, FieldName = "QTY", DisplayFormat = "{0}" });
                TotalSummary.Add(new Summary() { Type = SummaryItemType.Sum, FieldName = fieldName3, DisplayFormat = "{0:N}" });
                TotalSummary.Add(new Summary() { Type = SummaryItemType.Sum, FieldName = fieldName4, DisplayFormat = "{0:N}" });
                TotalSummary.Add(new Summary() { Type = SummaryItemType.Sum, FieldName = fieldName8, DisplayFormat = "{0}" });
                TotalSummary.Add(new Summary() { Type = SummaryItemType.Sum, FieldName = fieldName12, DisplayFormat = @" {0:dd\.hh\:mm\:ss}" });
                TotalSummary.Add(new Summary() { Type = SummaryItemType.Sum, FieldName = fieldName17, DisplayFormat = @" {0:dd\.hh\:mm\:ss}" });
                TotalSummary.Add(new Summary() { Type = SummaryItemType.Sum, FieldName = fieldName19, DisplayFormat = @" {0:dd\.hh\:mm\:ss}" });
                TotalSummary.Add(new Summary() { Type = SummaryItemType.Sum, FieldName = fieldName18, DisplayFormat = @" {0:dd\.hh\:mm\:ss}" });
                TotalSummary.Add(new Summary() { Type = SummaryItemType.Sum, FieldName = fieldName20, DisplayFormat = @" {0:dd\.hh\:mm\:ss}" });
                TotalSummary.Add(new Summary() { Type = SummaryItemType.Sum, FieldName = fieldName13, DisplayFormat = @" {0:dd\.hh\:mm\:ss}" });
                TotalSummary.Add(new Summary() { Type = SummaryItemType.Sum, FieldName = fieldName14, DisplayFormat = @" {0:dd\.hh\:mm\:ss}" });
                GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTableWithoutBands executed Successfully", category: Category.Info, priority: Priority.Low);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in AddColumnsToDataTableWithoutBands() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in AddColumnsToDataTableWithoutBands() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in AddColumnsToDataTableWithoutBands() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void GetPlants()
        {
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                GeosApplication.Instance.Logger.Log("Method GetPlants()...", category: Category.Info, priority: Priority.Low);
                if (PlantList == null || PlantList.Count == 0)
                {
                    PlantList = new ObservableCollection<Site>(PLMService.GetPlants_V2120());
                    AllPlantList = PlantList.ToList();
                }

                List<Site> plantOwners = ERMCommon.Instance.SelectedAuthorizedPlantsList.Cast<Site>().ToList();
                var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.IdSite));

                List<Site> PlantList1 = new List<Site>();
                foreach (Site item in plantOwners)
                {

                    UInt32 plantid = Convert.ToUInt32(item.IdSite);
                    PlantList1 = PlantList.Where(x => x.IdSite == plantid).ToList();
                    if (SelectedPlant == null)
                        SelectedPlant = new List<object>();

                    foreach (Site plant in PlantList1)
                    {
                        SelectedPlant.Add(PlantList.FirstOrDefault(a => a.IdSite == plant.IdSite));
                        if (SelectedPlantold == null)
                            SelectedPlantold = new List<object>();
                        SelectedPlantold = SelectedPlant;
                    }

                }

                GeosApplication.Instance.Logger.Log("Method GetPlants()....executed successfully", category: Category.Info, priority: Priority.Low);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetPlants() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetPlants() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method GetPlants() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        ObservableCollection<TrackingAccordian> templateWithTimeTracking;
        //ObservableCollection<TemplateWithCPTypes> templateWithCPTypes;
        public ObservableCollection<TrackingAccordian> TemplateWithTimeTracking
        {
            get { return templateWithTimeTracking; }
            set
            {
                templateWithTimeTracking = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TemplateWithTimeTracking"));
            }
        }
        private void FillDeliveryweek()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillDeliveryweek()...", category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                List<TimeTracking> DeliverWeekGroup = new List<TimeTracking>();
                var temp = TimeTrackingList.GroupBy(x => x.DeliveryWeek)
                    .Select(group => new
                    {
                        DeliveryWeek = TimeTrackingList.FirstOrDefault(a => a.DeliveryWeek == group.Key).DeliveryWeek,

                        Count = TimeTrackingList.Where(b => b.DeliveryWeek == null).Count(),
                    }).ToList().OrderBy(i => i.DeliveryWeek);     ////GEOS2-4045 Gulab lakade Order by CW ASC

                TemplateWithTimeTracking = new ObservableCollection<TrackingAccordian>();

                foreach (var item in temp)
                {

                    DeliverWeekGroup = TimeTrackingList.Where(x => x.DeliveryWeek == item.DeliveryWeek).ToList();
                    //DeliverWeekGroup = TimeTrackingList.Where(x => x.PlannedDeliveryDate).ToList();
                    TrackingAccordian templateWithTimeTracking = new TrackingAccordian();
                    var currentculter = CultureInfo.CurrentCulture;

                    var tempdateByPlanningDeliveryDate = (from dw in DeliverWeekGroup   //[GEOS2-4217] [Pallavi Jadhav] [27 02 2023]
                                                          select new
                                                          {
                                                              FDeliveryDate = dw.DeliveryDate,
                                                              dw.PlannedDeliveryDate,
                                                              DeliveryDate = (dw.PlannedDeliveryDate != null ? dw.PlannedDeliveryDate : dw.DeliveryDate)
                                                          }
                                              ).Distinct().OrderBy(a => a.DeliveryDate).ToList();

                    // List<DateTime?> tempDate = tempdateByPlanningDeliveryDate.Select(i => i.DeliveryDate).Distinct().ToList();
                    //List<DateTime?> tempDate = DeliverWeekGroup.Select(i => i.DeliveryDate).Distinct().ToList();
                    List<DateTime?> tempDate = tempdateByPlanningDeliveryDate.Select(a => a.DeliveryDate).Distinct().ToList();
                    if (templateWithTimeTracking.TimeTracking == null)
                        templateWithTimeTracking.TimeTracking = new List<string>();
                    List<DateTime?> tempDateorderBy = tempDate.OrderBy(a => a.Value).ToList();  //// Gulab lakade Order by CW ASC 04-05-2023
                    foreach (DateTime item1 in tempDateorderBy)
                    {
                        string TempDate = item1.ToString("d", currentculter);
                        if (!templateWithTimeTracking.TimeTracking.Contains(TempDate))
                        {
                            //List<string> tempstring = new List<string> { "22/7/2022", "12/7/2022", "11/7/2022" };         //only testing purpose
                            //if (!tempstring.Contains(TempDate))
                            //{
                            templateWithTimeTracking.TimeTracking.Add(TempDate);
                            //templateWithTimeTracking.TimeTracking.Sort();
                            //}
                        }
                    }

                    templateWithTimeTracking.deliwaryWeek = item.DeliveryWeek + " (" + templateWithTimeTracking.TimeTracking.Count + ")"; ;
                    templateWithTimeTracking.copyDeliwaryWeek = item.DeliveryWeek;
                    TemplateWithTimeTracking.Add(templateWithTimeTracking);
                }
                TrackingAccordian templateWithTimeTrackingAll = new TrackingAccordian();
                if (templateWithTimeTrackingAll.TimeTracking == null)
                    templateWithTimeTrackingAll.TimeTracking = new List<string>();
                templateWithTimeTrackingAll.deliwaryWeek = "All";
                TemplateWithTimeTracking.Insert(0, templateWithTimeTrackingAll);

                //TemplateWithTimeTracking.Where(f => "2022CW32 (3)".Contains(f.deliwaryWeek)).ToList().ForEach(i => TemplateWithTimeTracking.Remove(i));       // only testing purpose

                //SelectedItem = "All";// TemplateWithTimeTracking[0];
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method FillDeliveryweek()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillDeliveryweek() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillDeliveryweek() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillDeliveryweek() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void GetRecordbyDeliveryDate()
        {
            GeosApplication.Instance.Logger.Log("Method GetRecordbyDeliveryDate ...", category: Category.Info, priority: Priority.Low);
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                if(TimeTrackingList_Cloned!=null)
                {
                    TimeTrackingList = new List<TimeTracking>(TimeTrackingList_Cloned);
                }
                //TimeTrackingList = ERMService.GetAllTimeTracking_V2330();
                if (TimeTrackingList.Count > 0)
                {
                    TimeTrackingListCopy = new List<TimeTracking>();
                    TimeTrackingListCopy.AddRange(TimeTrackingList);
                    var currentculter = CultureInfo.CurrentCulture;
                    TimeTracking TimeTracking = new TimeTracking();
                    TimeTracking = TimeTrackingList.FirstOrDefault();
                    #region GEOS2-4045 Gulab lakade
                    if (SelectedItem != null)
                    {
                        if (SelectedItem.ToString().Contains("CW"))
                        {
                            string tempselectItem = Convert.ToString(SelectedItem);
                            int index = tempselectItem.LastIndexOf("(");
                            if (index > 0)
                                tempselectItem = tempselectItem.Substring(0, index);

                            TimeTrackingList = TimeTrackingList.Where(x => x.DeliveryWeek.ToString().Contains(tempselectItem.Trim())).ToList();
                        }
                        else if (SelectedItem.ToString().Contains("All"))
                        {
                            TimeTrackingList = TimeTrackingList.ToList();
                        }
                        else
                        if (!string.IsNullOrEmpty(SelectedItem.ToString()))
                        {
                            int tempTimeTrackingcount = 0;
                            List<TimeTracking> TimeTrackingListtemp = new List<TimeTracking>();
                            tempTimeTrackingcount = TimeTrackingList.Where(x => x.PlannedDeliveryDate != null && x.PlannedDeliveryDate.Value.ToString().Contains(SelectedItem.ToString())).ToList().Count();
                            if (tempTimeTrackingcount != 0)
                            {
                                TimeTrackingListtemp = TimeTrackingList.Where(x => x.PlannedDeliveryDate != null && x.PlannedDeliveryDate.Value.ToString().Contains(SelectedItem.ToString())).ToList();
                                TimeTrackingListtemp.AddRange(TimeTrackingList.Where(x => x.DeliveryDate.Value.ToString().Contains(SelectedItem.ToString())).ToList());
                                TimeTrackingList = new List<TimeTracking>();
                                TimeTrackingList.AddRange(TimeTrackingListtemp);
                            }
                            else
                            {
                                TimeTrackingList = TimeTrackingList.Where(x => x.DeliveryDate.Value.ToString().Contains(SelectedItem.ToString())).ToList();
                            }
                        }
                        #endregion

                        //AddColumnsToDataTableWithoutBands();
                        FlagCalculateRework = true;
                        FillDashboard();
                        TimeTrackingList = new List<TimeTracking>();
                        TimeTrackingList.AddRange(TimeTrackingListCopy);
                    }
                    //else
                    //{
                    //    Init();
                    //}
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Method GetRecordbyDeliveryDate() executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetRecordbyDeliveryDate()", category: Category.Exception, priority: Priority.Low);
            }
        }

        private void RefreshTimeTrackingCommandAction(object obj)
        {
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                GeosApplication.Instance.Logger.Log("Method RefreshTimeTrackingCommandAction()...", category: Category.Info, priority: Priority.Low);


                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                //gulab lakade plant not selection chnages 04-05-2023
                SelectedPlant = new List<object>();
                SelectedPlant = ERMCommon.Instance.SelectedAuthorizedPlantsList;

                //   TimeTrackingView.ClearText();

                OTCode = string.Empty;
                Item = string.Empty;

                //end
                if (SelectedPlant != null)
                {
                    ERMCommon.Instance.WarningFailedPlants = string.Empty;
                    FailedPlants = new List<string>();
                    IsGridOpen = true;
                    IsBandOpen = false;
                    FillSelectedPlant();
                    FillDeliveryweek();
                    FlagCalculateRework = false;
                    FillDashboard();

                    #region GEOS2-4082
                    var CurrencyNameFromSetting = String.Empty;
                    if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("ERM_SelectedCurrency"))
                    {
                        CurrencyNameFromSetting = GeosApplication.Instance.UserSettings["ERM_SelectedCurrency"];
                    }
                    string Plantname = string.Empty;
                    foreach (Site item in SelectedPlant)
                    {
                        if (!string.IsNullOrEmpty(Plantname))
                        {
                            Plantname += ", " + Convert.ToString(item.Name);
                        }
                        else
                        {
                            Plantname += Convert.ToString(item.Name);
                        }

                    }

                    //[GEOS2-4149][Rupali Sarode][30-01-2023]
                    #region ////[GEOS2-5098] [gulab lakade][30 11 2023]
                    //ERMTimeTrackingGetData.GetERMTimeTrackingFromServiceAsync(ERMService, PLMService, TimeTrackingList, GridControl1, CurrencyNameFromSetting,
                    //    Plantname, DtTimetracking, DataTableForGridLayout, GeosAppSettingList, PlantListForTrackingData.ToList(), TemplateWithTimeTracking, WarningFailedPlants, IsShowFailedPlantWarning, AllPlantList);
                    //ERMTimeTrackingGetData.GetERMTimeTrackingFromServiceAsync(ERMService, PLMService, TimeTrackingList, GridControl1, CurrencyNameFromSetting,
                    //   Plantname, DtTimetracking, DataTableForGridLayout, GeosAppSettingList, TimetrackingProductionList.ToList(), TemplateWithTimeTracking, WarningFailedPlants, IsShowFailedPlantWarning, AllPlantList);
                    //ERMTimeTrackingGetData.GetERMTimeTrackingFromServiceAsync(ERMService, PLMService, TimeTrackingList, GridControl1, CurrencyNameFromSetting,
                    // Plantname, DtTimetracking, DataTableForGridLayout, GeosAppSettingList, TimetrackingProductionList.ToList(), TemplateWithTimeTracking, WarningFailedPlants, IsShowFailedPlantWarning, AllPlantList, TimeTrackingProductionStage);//[gulab lakade][11 03 2024][GEOS2-5466]

                    //ERMTimeTrackingGetData.GetERMTimeTrackingFromServiceAsync(ERMService, PLMService, TimeTrackingList, GridControl1, CurrencyNameFromSetting,
                    //Plantname, DtTimetracking, DataTableForGridLayout, GeosAppSettingList, TimetrackingProductionList.ToList(), TemplateWithTimeTracking, WarningFailedPlants, IsShowFailedPlantWarning, AllPlantList, TimeTrackingProductionStage, TimetrackingStagesList); // [Rupali Sarode][GEOS2-5522][22-03-2024]

                    //                    ERMTimeTrackingGetData.GetERMTimeTrackingFromServiceAsync(ERMService, PLMService, TimeTrackingList, GridControl1, CurrencyNameFromSetting,
                    //Plantname, DtTimetracking, DataTableForGridLayout, GeosAppSettingList, TimetrackingProductionList.ToList(), TemplateWithTimeTracking, WarningFailedPlants, IsShowFailedPlantWarning, AllPlantList, TimeTrackingProductionStage, TimetrackingStagesList, CADCAMDesignTypeList); //[GEOS2-5854][gulab lakade][19 07 2024]


                    // ERMTimeTrackingGetData.GetERMTimeTrackingFromServiceAsync(ERMService, PLMService, TimeTrackingList, GridControl1, CurrencyNameFromSetting,
                    //Plantname, DtTimetracking, DataTableForGridLayout, GeosAppSettingList, TimetrackingProductionList.ToList(), TemplateWithTimeTracking, WarningFailedPlants, IsShowFailedPlantWarning, AllPlantList, TimeTrackingProductionStage, TimetrackingStagesList, CADCAMDesignTypeList); //[GEOS2-5988]

                    #endregion
                    #endregion
                }
                else
                {
                    FailedPlants = new List<string>();
                    TimeTrackingList.Clear();
                    DataTableForGridLayout.Clear();
                    DtTimetracking = null;
                    FillDeliveryweek();
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method RefreshTimeTrackingCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RefreshTimeTrackingCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RefreshTimeTrackingCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshTimeTrackingCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void PrintTimeTrackingCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintTimeTrackingCommandAction()...", category: Category.Info, priority: Priority.Low);

                IsBusy = true;

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                PrintableControlLink pcl = new PrintableControlLink((TableView)obj);
                pcl.Margins.Bottom = 5;
                pcl.Margins.Top = 5;
                pcl.Margins.Left = 5;
                pcl.Margins.Right = 5;

                pcl.PaperKind = System.Drawing.Printing.PaperKind.A4;
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["TimeTrackingPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["TimeTrackingPrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                IsBusy = false;
                window.Show();

                DevExpress.XtraPrinting.PrintTool printTool = new DevExpress.XtraPrinting.PrintTool(pcl.PrintingSystem);
                printTool.PrinterSettings.PrinterName = GeosApplication.Instance.UserSettings["SelectedPrinter"];

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method PrintTimeTrackingCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintTimeTrackingCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ExportTimeTrackingCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintTimeTrackingCommandAction()...", category: Category.Info, priority: Priority.Low);

                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "CAM_CAD Time Tracking";
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
                    if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                    ResultFileName = (saveFile.FileName);
                    TableView activityTableView = ((TableView)obj);
                    activityTableView.ShowTotalSummary = false;
                    activityTableView.ShowFixedTotalSummary = false;
                    XlsxExportOptionsEx options = new XlsxExportOptionsEx();
                    #region[GEOS2-6658][Daivshala Vighne][21 01 2025]
                    options.CustomizeCell += Options_CustomizeCell;
                    #endregion
                    activityTableView.ExportToXlsx(ResultFileName, options);
                    IsBusy = false;
                    activityTableView.ShowFixedTotalSummary = true;
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);
                    activityTableView.ShowTotalSummary = true;
                    GeosApplication.Instance.Logger.Log("Method PrintTimeTrackingCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintTimeTrackingCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #region[GEOS2-6658][Daivshala Vighne][21 01 2025]
        private void Options_CustomizeCell(DevExpress.Export.CustomizeCellEventArgs e)
        {
            string columnName = e.ColumnFieldName;
            if (e.DataSourceRowIndex >= 0)
            {
                if (e.Value is TimeSpan)
                {
                    e.Handled = true;
                    TimeSpan timeValue;
                    if (TimeSpan.TryParse(e.Value.ToString(), out timeValue))
                    {
                        if (timeValue.TotalMilliseconds < 0)
                        {
                            e.Value = "-" + timeValue.Duration().ToString(@"dd\.hh\:mm\:ss");
                        }
                        else
                        {
                            e.Value = timeValue.ToString(@"dd\.hh\:mm\:ss");
                        }
                    }
                }
            }

        }
        #endregion

        private void ChangePlantCommandAction(object obj)
        {
            try
            {


                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                OTCode = string.Empty;
                Item = string.Empty;

                if (obj == null)
                {
                    FailedPlants = new List<string>();
                    TimeTrackingList.Clear();
                    DataTableForGridLayout.Clear();
                    DtTimetracking = null;
                    FillDeliveryweek();
                    // return;
                }
                else
                {
                    FailedPlants = new List<string>();
                    if (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
                    {
                        //return;
                    }
                    else
                    {

                        GeosApplication.Instance.Logger.Log("Method ChangePlantCommandAction()...", category: Category.Info, priority: Priority.Low);
                        var TempSelectedPlant = (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).EditValue);
                        SelectedPlant = new List<object>();
                        foreach (var item in (dynamic)TempSelectedPlant)
                        {
                            SelectedPlant.Add(item);
                        }
                        //List<object> temp12= TempSelectedPlant
                        //SelectedPlant = (Site)TempSelectedPlant;
                        if (SelectedPlant != null)
                        {
                            ERMCommon.Instance.WarningFailedPlants = string.Empty;
                            /// gulab lakade plant selection changes 04-05-2023
                            ERMCommon.Instance.SelectedAuthorizedPlantsList = new List<object>();
                            ERMCommon.Instance.SelectedAuthorizedPlantsList = SelectedPlant;

                            #region [Geos2-6786][gulab lakade][30 12 2024]
                            List<Site> plantOwners = ERMCommon.Instance.SelectedAuthorizedPlantsList.Cast<Site>().ToList();
                            string ActivePlantString = ActivePlantList.Select(a => a.DefaultValue).FirstOrDefault();
                            var activePlantIds = ActivePlantString?.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(id => Convert.ToUInt32(id)).ToList();
                            var plantOwnerNames = plantOwners.Select(plantOwner => plantOwner.IdSite).ToList();
                            bool isMatch = activePlantIds != null && activePlantIds.Any(id => plantOwnerNames.Contains(id));
                            if (isMatch == false)
                            {
                                IscheckplantInSetting = true;
                            }
                            #endregion
                            FillSelectedPlant();
                            //FillAllDataByPlant();
                            AddColumnsToDataTableWithoutBands();  //[Rupali Sarode][GEOS2-4347][09-05-2023]
                            FillDeliveryweek();
                            FlagCalculateRework = false;
                            FillDashboard();
                            #region GEOS2-4082
                            var CurrencyNameFromSetting = String.Empty;
                            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("ERM_SelectedCurrency"))
                            {
                                CurrencyNameFromSetting = GeosApplication.Instance.UserSettings["ERM_SelectedCurrency"];
                            }
                            string Plantname = string.Empty;
                            foreach (Site item in SelectedPlant)
                            {
                                if (!string.IsNullOrEmpty(Plantname))
                                {
                                    Plantname += ", " + Convert.ToString(item.Name);
                                }
                                else
                                {
                                    Plantname += Convert.ToString(item.Name);
                                }

                            }

                            //ERMTimeTrackingGetData.GetERMTimeTrackingFromServiceAsync(ERMService, PLMService, TimeTrackingList, GridControl1, CurrencyNameFromSetting,
                            //    Plantname, DtTimetracking, DataTableForGridLayout, GeosAppSettingList, PlantList.ToList(), TemplateWithTimeTracking, WarningFailedPlants, IsShowFailedPlantWarning);

                            //[GEOS2-4149][Rupali Sarode][30-01-2023]
                            ////start[GEOS2-5098][gulab lakade][30 11 2023]
                            //ERMTimeTrackingGetData.GetERMTimeTrackingFromServiceAsync(ERMService, PLMService, TimeTrackingList, GridControl1, CurrencyNameFromSetting,
                            //        Plantname, DtTimetracking, DataTableForGridLayout, GeosAppSettingList, PlantListForTrackingData.ToList(), TemplateWithTimeTracking, WarningFailedPlants, IsShowFailedPlantWarning, AllPlantList);

                            //ERMTimeTrackingGetData.GetERMTimeTrackingFromServiceAsync(ERMService, PLMService, TimeTrackingList, GridControl1, CurrencyNameFromSetting,
                            //        Plantname, DtTimetracking, DataTableForGridLayout, GeosAppSettingList, TimetrackingProductionList.ToList(), TemplateWithTimeTracking, WarningFailedPlants, IsShowFailedPlantWarning, AllPlantList);
                            //ERMTimeTrackingGetData.GetERMTimeTrackingFromServiceAsync(ERMService, PLMService, TimeTrackingList, GridControl1, CurrencyNameFromSetting,
                            //       Plantname, DtTimetracking, DataTableForGridLayout, GeosAppSettingList, TimetrackingProductionList.ToList(), TemplateWithTimeTracking, WarningFailedPlants, IsShowFailedPlantWarning, AllPlantList, TimeTrackingProductionStage);//[gulab lakade][11 03 2024][GEOS2-5466]

                            //ERMTimeTrackingGetData.GetERMTimeTrackingFromServiceAsync(ERMService, PLMService, TimeTrackingList, GridControl1, CurrencyNameFromSetting,
                            //Plantname, DtTimetracking, DataTableForGridLayout, GeosAppSettingList, TimetrackingProductionList.ToList(), TemplateWithTimeTracking, WarningFailedPlants, IsShowFailedPlantWarning, AllPlantList, TimeTrackingProductionStage, TimetrackingStagesList); // [Rupali Sarode][GEOS2-5522][21-03-2024]
                            //                            ERMTimeTrackingGetData.GetERMTimeTrackingFromServiceAsync(ERMService, PLMService, TimeTrackingList, GridControl1, CurrencyNameFromSetting,
                            //Plantname, DtTimetracking, DataTableForGridLayout, GeosAppSettingList, TimetrackingProductionList.ToList(), TemplateWithTimeTracking, WarningFailedPlants, IsShowFailedPlantWarning, AllPlantList, TimeTrackingProductionStage, TimetrackingStagesList, CADCAMDesignTypeList); //[GEOS2-5854][gulab lakade][19 07 2024]
                            //ERMTimeTrackingGetData.GetERMTimeTrackingFromServiceAsync(ERMService, PLMService, TimeTrackingList, GridControl1, CurrencyNameFromSetting,
                            //Plantname, DtTimetracking, DataTableForGridLayout, GeosAppSettingList, TimetrackingProductionList.ToList(), TemplateWithTimeTracking, WarningFailedPlants, IsShowFailedPlantWarning, AllPlantList, TimeTrackingProductionStage, TimetrackingStagesList, CADCAMDesignTypeList); //[GEOS2-5854][gulab lakade][19 07 2024]
                            ERMCamCadTimeTrackingGetData.GetERMCamCadTimeTrackingFromServiceAsync(ERMService, PLMService, TimeTrackingList, GridControl1, CurrencyNameFromSetting,
                          Plantname, DtTimetracking, DataTableForGridLayout, GeosAppSettingList, TimetrackingProductionList.ToList(), TemplateWithTimeTracking, WarningFailedPlants, IsShowFailedPlantWarning, AllPlantList, TimeTrackingProductionStage, TimetrackingStagesList, CADCAMDesignTypeList, AppSettingData, FromDate, ToDate); //[GEOS2-6620][gulab lakade][19 12 2024]

                            ////end[GEOS2-5098][gulab lakade][30 11 2023]
                            #endregion
                        }
                        else
                        {
                            TimeTrackingList.Clear();
                            DataTableForGridLayout.Clear();
                            DtTimetracking = null;
                            FillDeliveryweek();
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

        private void TableViewLoadedCommandAction(RoutedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method TableViewLoadedCommandAction ...", category: Category.Info, priority: Priority.Low);
                #region [Geos2-6786][gulab lakade][30 12 2024]
                List<Site> plantOwners = ERMCommon.Instance.SelectedAuthorizedPlantsList.Cast<Site>().ToList();
                string ActivePlantString = ActivePlantList.Select(a => a.DefaultValue).FirstOrDefault();
                var activePlantIds = ActivePlantString?.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(id => Convert.ToUInt32(id)).ToList();
                var plantOwnerNames = plantOwners.Select(plantOwner => plantOwner.IdSite).ToList();
                bool isMatch = activePlantIds != null && activePlantIds.Any(id => plantOwnerNames.Contains(id));
                #endregion
                int visibleFalseColumnsCount = 0;

                if (File.Exists(ERM_TimeTrackinggrid_SettingFilePath))
                {
                    ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.RestoreLayoutFromXml(ERM_TimeTrackinggrid_SettingFilePath);
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
                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.SaveLayoutToXml(ERM_TimeTrackinggrid_SettingFilePath);

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
                    #region [Geos2-6786][gulab lakade][30 12 2024]
                    if (isMatch == false)
                    {
                        if (column.FieldName.Contains("PlannedDeliveryDate_") || column.FieldName.Contains("Days_"))
                        {
                            column.Visible = false;
                            column.ShowInColumnChooser = false;

                        }
                    }
                    else
                    {
                        if (column.FieldName.Contains("PlannedDeliveryDate_") || column.FieldName.Contains("Days_"))
                        {
                            column.Visible = true;
                            column.ShowInColumnChooser = false;
                        }
                    }
                    #endregion
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
                datailView.ShowTotalSummary = true;

                //TableView datailView = ((TableView)((System.Windows.RoutedEventArgs)obj).OriginalSource);
                // datailView.BestFitColumns();
                // GeosApplication.Instance.UserSettings["ERMTimeTraking_IsFileDeleted"] = "1";
                GeosApplication.Instance.Logger.Log("Method TableViewLoadedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in TableViewLoadedCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }



        //    private void TableViewLoadedCommandAction(RoutedEventArgs obj)
        //    {
        //        try
        //        {
        //            GeosApplication.Instance.Logger.Log("Method GridControlLoadedAction...", category: Category.Info, priority: Priority.Low);
        //            {
        //                //if (IsBandOpen)
        //                //{
        //                int visibleFalseColumn = 0;
        //                //GridControl gridControl = obj as GridControl;
        //                //TableView tableView = (TableView)gridControl.View;


        //                if (File.Exists(ERM_TimeTrackinggrid_SettingFilePath))
        //                {
        //                    ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.RestoreLayoutFromXml(ERM_TimeTrackinggrid_SettingFilePath);
        //                    GridControl gridControl = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;
        //                    TableView tableView = (TableView)gridControl.View;
        //                    this.tableViewInstance = tableView;

        //                    if (tableView.SearchString != null)
        //                    {
        //                        tableView.SearchString = null;
        //                    }

        //                    //gridControl.BeginInit();



        //                    //if (iSModuleVisible)
        //                    //{
        //                    if (File.Exists(ERM_TimeTrackinggrid_SettingFilePath))
        //                    {
        //                        gridControl.RestoreLayoutFromXml(ERM_TimeTrackinggrid_SettingFilePath);
        //                    }
        //                    //}
        //                    //if (iSStructureVisible)
        //                    //{
        //                    //    if (File.Exists(StructureBandSettingFilePath))
        //                    //    {
        //                    //        gridControl.RestoreLayoutFromXml(StructureBandSettingFilePath);
        //                    //    }
        //                    //}

        //                    //This code for save grid layout.
        //                    //if (iSModuleVisible)
        //                    gridControl.SaveLayoutToXml(ERM_TimeTrackinggrid_SettingFilePath);
        //                    //else if (iSStructureVisible)
        //                    //    gridControl.SaveLayoutToXml(StructureBandSettingFilePath);

        //                    foreach (GridColumn column in gridControl.Columns)
        //                    {
        //                        DependencyPropertyDescriptor descriptor = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleProperty, typeof(GridColumn));
        //                        if (descriptor != null)
        //                        {
        //                            descriptor.AddValueChanged(column, VisibleChanged);
        //                        }

        //                        DependencyPropertyDescriptor descriptorColumnPosition = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleIndexProperty, typeof(GridColumn));
        //                        if (descriptorColumnPosition != null)
        //                        {
        //                            descriptorColumnPosition.AddValueChanged(column, VisibleIndexChanged);
        //                        }

        //                        if (!column.Visible)
        //                        {
        //                            //if ((column.FieldName != "IdTemplate" && column.FieldName != "IdCPType" && column.FieldName != "HtmlColor"))
        //                            //{
        //                            visibleFalseColumn++;
        //                            ((Emdep.Geos.UI.Helper.ColumnItem)column.DataContext).IsVertical = false;
        //                            //}
        //                        }
        //                    }

        //                    if (visibleFalseColumn > 0)
        //                    {
        //                        IsColumnChooserVisibleForGrid = true;
        //                    }
        //                    else
        //                    {
        //                        IsColumnChooserVisibleForGrid = false;
        //                    }
        //                    TableView datailView = ((TableView)((System.Windows.RoutedEventArgs)obj).OriginalSource);
        //                    datailView.ShowTotalSummary = true;
        ////                    gridControl.TotalSummary.AddRange(new List<GridSummaryItem>() {
        ////new GridSummaryItem() {
        ////    SummaryType = SummaryItemType.Sum,
        ////    FieldName = "Real",
        ////    DisplayFormat= "{0}",
        ////    Visible=true,
        ////   //FixedTotalSummaryElementStyle=TextAlignment.Right
        ////    //Alignment=GridSummaryItemAlignment.Right,

        ////},
        ////new GridSummaryItem() {
        ////    SummaryType = SummaryItemType.Sum,
        ////    FieldName = "Remaining",
        ////    DisplayFormat="{0}",
        ////    Visible=true


        ////}});
        //                    //gridControl.EndInit();
        //                    //tableView.SearchString = null;
        //                    //tableView.ShowGroupPanel = false;
        //                }
        //                //}

        //                //IsBusy = false;
        //            }
        //            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //            GeosApplication.Instance.Logger.Log("Method GridControlLoadedAction executed successfully...", category: Category.Info, priority: Priority.Low);
        //        }
        //        catch (Exception ex)
        //        {
        //            GeosApplication.Instance.Logger.Log("Error on GridControlLoadedAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //        }
        //    }
        //private void TableViewLoadedCommandAction(RoutedEventArgs obj)
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method TableViewLoadedCommandAction ...", category: Category.Info, priority: Priority.Low);

        //        int visibleFalseColumnsCount = 0;

        //        if (File.Exists(ERM_TimeTrackinggrid_SettingFilePath))
        //        {
        //            ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.RestoreLayoutFromXml(ERM_TimeTrackinggrid_SettingFilePath);
        //            GridControl gridControlInstance = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;

        //            TableView tableView = (TableView)gridControlInstance.View;
        //            this.tableViewInstance = tableView;

        //            if (tableView.SearchString != null)
        //            {
        //                tableView.SearchString = null;
        //            }
        //        }

        //        ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.AddHandler(DXSerializer.AllowPropertyEvent, new AllowPropertyEventHandler(OnAllowProperty));

        //        //This code for save grid layout.
        //        ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.SaveLayoutToXml(ERM_TimeTrackinggrid_SettingFilePath);

        //        GridControl gridControl = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;
        //        foreach (GridColumn column in gridControl.Columns)
        //        {
        //            DependencyPropertyDescriptor descriptor = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleProperty, typeof(GridColumn));
        //            if (descriptor != null)
        //            {
        //                descriptor.AddValueChanged(column, VisibleChanged);
        //            }

        //            DependencyPropertyDescriptor descriptorColumnPosition = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleIndexProperty, typeof(GridColumn));
        //            if (descriptorColumnPosition != null)
        //            {
        //                descriptorColumnPosition.AddValueChanged(column, VisibleIndexChanged);
        //            }

        //            if (column.Visible == false)
        //            {
        //                visibleFalseColumnsCount++;
        //            }
        //        }

        //        if (visibleFalseColumnsCount > 0)
        //        {
        //            IsColumnChooserVisibleForGrid = true;
        //        }
        //        else
        //        {
        //            IsColumnChooserVisibleForGrid = false;
        //        }
        //        TableView datailView = ((TableView)((System.Windows.RoutedEventArgs)obj).OriginalSource);
        //        datailView.ShowTotalSummary = true;


        //        //            gridControl.TotalSummary.AddRange(new List<GridSummaryItem>() {
        //        //new GridSummaryItem() {
        //        //    SummaryType = SummaryItemType.Sum,
        //        //    FieldName = "UITempobservedTime",
        //        //    DisplayFormat= "{0}",
        //        //    Visible=true,
        //        //   //FixedTotalSummaryElementStyle=TextAlignment.Right
        //        //    //Alignment=GridSummaryItemAlignment.Right,

        //        //},
        //        //new GridSummaryItem() {
        //        //    SummaryType = SummaryItemType.Sum,
        //        //    FieldName = "UITempNormalTime",
        //        //    DisplayFormat="{0}",
        //        //    Visible=true


        //        //}});
        //        //TableView datailView = ((TableView)((System.Windows.RoutedEventArgs)obj).OriginalSource);
        //        // datailView.BestFitColumns();

        //        GeosApplication.Instance.Logger.Log("Method TableViewLoadedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in TableViewLoadedCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
        //    }
        //}
        void VisibleChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method VisibleChanged ...", category: Category.Info, priority: Priority.Low);

                GridColumn column = sender as GridColumn;
                if (column.ShowInColumnChooser)
                {
                    ((GridControl)((GridControlBand)((System.Windows.FrameworkContentElement)sender).Parent).Parent).SaveLayoutToXml(ERM_TimeTrackinggrid_SettingFilePath);
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
                ((GridControl)((GridControlBand)((System.Windows.FrameworkContentElement)sender).Parent).Parent).SaveLayoutToXml(ERM_TimeTrackinggrid_SettingFilePath);
                //((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(ERM_TimeTrackinggrid_SettingFilePath);

                GeosApplication.Instance.Logger.Log("Method VisibleIndexChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in VisibleIndexChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
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
        private void TableViewUnloadedCommandAction(RoutedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method TableViewUnloadedCommandAction ...", category: Category.Info, priority: Priority.Low);

                //GridControl gridControlInstance = ((TimeTrackingView)obj.OriginalSource).rootGridControl; // ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;
                //TableView tableView = (TableView)gridControlInstance.View;

                //tableView.SearchString = string.Empty;
                //if (gridControlInstance.GroupCount > 0)
                //    gridControlInstance.ClearGrouping();
                //gridControlInstance.ClearSorting();
                //gridControlInstance.FilterString = null;
                //gridControlInstance.SaveLayoutToXml(ERM_TimeTrackinggrid_SettingFilePath);

                //GridControl gridControl = obj as GridControl;
                GridControl gridControl = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;
                TableView tableView = (TableView)gridControl.View;
                tableView.SearchString = string.Empty;
                if (gridControl.GroupCount > 0)
                    gridControl.ClearGrouping();
                gridControl.ClearSorting();
                gridControl.FilterString = null;
                gridControl.SaveLayoutToXml(ERM_TimeTrackinggrid_SettingFilePath);

                //int visibleFalseColumnsCount = 0;

                //if (File.Exists(ERM_WorkOperationsGrid_SettingFilePath))
                //{
                //    ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.RestoreLayoutFromXml(ERM_WorkOperationsGrid_SettingFilePath);
                //    GridControl gridControlInstance = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;
                //    TableView tableView = (TableView)gridControlInstance.View;

                //    if (tableView.SearchString != null)
                //    {
                //        tableView.SearchString = null;
                //    }
                //}

                //// ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.AddHandler(DXSerializer.AllowPropertyEvent, new AllowPropertyEventHandler(OnAllowProperty));

                ////This code for save grid layout.
                //((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.SaveLayoutToXml(ERM_WorkOperationsGrid_SettingFilePath);

                //GridControl gridControl = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;
                //foreach (GridColumn column in gridControl.Columns)
                //{
                //    //DependencyPropertyDescriptor descriptor = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleProperty, typeof(GridColumn));
                //    //if (descriptor != null)
                //    //{
                //    //    descriptor.AddValueChanged(column, VisibleChanged);
                //    //}

                //    //DependencyPropertyDescriptor descriptorColumnPosition = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleIndexProperty, typeof(GridColumn));
                //    //if (descriptorColumnPosition != null)
                //    //{
                //    //    descriptorColumnPosition.AddValueChanged(column, VisibleIndexChanged);
                //    //}

                //    if (column.Visible == false)
                //    {
                //        visibleFalseColumnsCount++;
                //    }
                //}

                //if (visibleFalseColumnsCount > 0)
                //{
                //    IsColumnChooserVisibleForGrid = true;
                //}
                //else
                //{
                //    IsColumnChooserVisibleForGrid = false;
                //}

                ////TableView datailView = ((TableView)((System.Windows.RoutedEventArgs)obj).OriginalSource);
                //// datailView.BestFitColumns();

                GeosApplication.Instance.Logger.Log("Method TableViewUnloadedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in TableViewUnloadedCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }
        private void ItemListTableViewLoadedAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ItemListTableViewLoadedAction()...", category: Category.Info, priority: Priority.Low);

                TableView tableView = obj as TableView;
                tableView.ColumnChooserState = new DefaultColumnChooserState
                {
                    Location = new Point(20, 180),
                    Size = new Size(250, 250)
                };
                GeosApplication.Instance.Logger.Log("Method ItemListTableViewLoadedAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method ItemListTableViewLoadedAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

            }
        }
        //#endregion
        private void HidePanel(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method HidePanel ...", category: Category.Info, priority: Priority.Low);

                if (IsAccordionControlVisible == Visibility.Collapsed)
                    IsAccordionControlVisible = Visibility.Visible;
                else
                    IsAccordionControlVisible = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method HidePanel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #region [GEOS2-4069][Rupali Sarode][07-12-2022]
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
        #endregion

        #region [pallavi jadhav][26-08-2024][GEOS2-6081]




        private void VerifyTheSamplePopupWindowCommandAction(object obj)
        {
            try
            {
                //IERMService ERMService = new ERMServiceController("localhost:6699");
                GeosApplication.Instance.Logger.Log("Method VerifyTheSamplePopupWindowCommandAction()", category: Category.Info, priority: Priority.Low);
                if (obj == null) return;
                string IdStage = string.Empty;
                string CPIds = string.Empty;
                TableView detailView = (TableView)obj;
                System.Data.DataRowView focusedRowData = (System.Data.DataRowView)detailView.FocusedRow;
                string StageName = string.Empty;
                string cellExpectedValue = string.Empty;
                var gridView = detailView.Grid.View as TableView;
                if (gridView != null)
                {
                    var focusedColumn = gridView.FocusedColumn;

                    //DevExpress.Xpf.Grid.GridColumn gridcontrol = (DevExpress.Xpf.Grid.GridColumn)gridView.Column;
                    if (focusedColumn != null)
                    {
                        string focusedColumnName = focusedColumn.FieldName;
                        string[] Stage = focusedColumnName.Split('_');
                        IdStage = Stage[Stage.Length - 1];
                        DevExpress.Xpf.Grid.GridControlBand Parent = (DevExpress.Xpf.Grid.GridControlBand)focusedColumn.Parent;
                        StageName = Convert.ToString(Parent.Header);
                        cellExpectedValue = Convert.ToString(focusedRowData[focusedColumnName]);
                        // cellExpectedValue = Convert.ToString(focusedRowData[focusedColumnName]);

                        TimeSpan timeSpanValue;
                        if (TimeSpan.TryParse(cellExpectedValue, out timeSpanValue))
                        {
                            string formattedValue = string.Format("{0:%d\\.hh\\:mm\\:ss}", timeSpanValue);
                            cellExpectedValue = formattedValue;
                        }
                    }
                }

                TimeTracking data = new TimeTracking
                {

                    OTCode = focusedRowData["OTCode"] != DBNull.Value ? Convert.ToString(focusedRowData["OTCode"]) : "",
                    SerialNumber = focusedRowData["SerialNumber"] != DBNull.Value ? Convert.ToString(focusedRowData["SerialNumber"]) : "",
                    NumItem = focusedRowData.DataView.Table.Columns.Contains("ItemNumber") && focusedRowData["ItemNumber"] != DBNull.Value
           ? Convert.ToInt64(focusedRowData["ItemNumber"])
           : 0,
                    ItemStatus = focusedRowData["ItemStatus"] != DBNull.Value ? Convert.ToString(focusedRowData["ItemStatus"]) : "",
                    Type = focusedRowData["Type"] != DBNull.Value ? Convert.ToString(focusedRowData["Type"]) : "",

                };
                if (TimeTrackingList != null)
                {
                    TimeTrackingListCP = new Data.Common.ERM.TimeTracking();
                    TimeTrackingListCP = TimeTrackingList.Where(a => a.SerialNumber == data.SerialNumber && a.NumItem == data.NumItem).FirstOrDefault();
                }

                LstStandardOperationsDictionaryModules = new List<StandardOperationsDictionaryModules>();

                LstStandardOperationsDictionaryDetection = new List<StandardOperationsDictionaryDetection>();

                LstStandardOperationsDictionaryOption = new List<StandardOperationsDictionaryOption>();

                LstStandardOperationsDictionaryWays = new List<StandardOperationsDictionaryWays>();

                LstStandardOperationsDictionarySupplement = new List<StandardOperationsDictionarySupplement>();

                double TempDesignValue = 1;
                string RoleValue = string.Empty;
                //   LstStandardOperationsDictionarySupplement.AddRange(ERMService.GetStandardOperationsDictionarySupplement_V2560(data.IdStandardOperationsDictionary));
                if (TimeTrackingListCP != null)
                {
                    //List<string> CPIds = TimeTrackingList.Select(i => (i as TimeTracking)?.IdCP.ToString()).Where(id => !string.IsNullOrEmpty(id)).Distinct().ToList();
                    string StageIds = string.Join(",", TimeTrackingList.Where(a => a.SerialNumber == data.SerialNumber && a.NumItem == data.NumItem).SelectMany(i => i.TimeTrackingStage.Select(a => (a as TimeTrackingCurrentStage)?.IdStage?.ToString())).Where(id => !string.IsNullOrEmpty(id)).Distinct());
                    CPIds = string.Join(",", TimeTrackingList.Where(a => a.SerialNumber == data.SerialNumber && a.NumItem == data.NumItem).Select(i => (i as TimeTracking)?.IdCP.ToString()).Where(id => !string.IsNullOrEmpty(id)).Distinct());
                    string CPTypeIds = string.Join(",", TimeTrackingList.Where(a => a.SerialNumber == data.SerialNumber && a.NumItem == data.NumItem).Select(i => (i as TimeTracking)?.IdCPType.ToString()).Where(id => !string.IsNullOrEmpty(id)).Distinct());

                    if (CADCAMDesignTypeList.Count() > 0)
                    {
                        //if (IdStage == "Expected")
                        //{
                        Int32 tempIdStage = 0;
                        //List<int> TempIdStageList = new List<int>();
                        if (IdStage == "Expected")
                        {
                            // tempIdStage = Convert.ToInt32(StageIds);
                            var TempCADCAMTotal = CADCAMDesignTypeList.Where(x => StageIds.Contains(Convert.ToString(x.IdStage)) && x.DesignType == TimeTrackingListCP.DrawingType).FirstOrDefault();
                            if (TempCADCAMTotal != null)
                            {
                                if (TempCADCAMTotal.DesignType == "C")
                                {
                                    TempDesignValue = Convert.ToDouble(TempCADCAMTotal.DesignValue);
                                    RoleValue = Convert.ToString(TempCADCAMTotal.DesignType);
                                }
                                else
                                {
                                    TempDesignValue = Convert.ToDouble(TempCADCAMTotal.DesignValue) / 100;
                                    RoleValue = Convert.ToString(TempCADCAMTotal.DesignType);
                                }

                            }
                        }
                        else
                        {
                            tempIdStage = Convert.ToInt32(IdStage);
                            var TempCADCAM = CADCAMDesignTypeList.Where(x => x.IdStage == tempIdStage && x.DesignType == TimeTrackingListCP.DrawingType).FirstOrDefault();
                            if (TempCADCAM != null)
                            {
                                if (TempCADCAM.DesignType == "C")
                                {
                                    TempDesignValue = Convert.ToDouble(TempCADCAM.DesignValue);
                                    RoleValue = Convert.ToString(TempCADCAM.DesignType);
                                }
                                else
                                {
                                    TempDesignValue = Convert.ToDouble(TempCADCAM.DesignValue) / 100;
                                    RoleValue = Convert.ToString(TempCADCAM.DesignType);
                                }

                            }
                        }

                        //}
                    }
                    // List<string> StageIds = TimeTrackingList.SelectMany(i => i.TimeTrackingStage.Select(a => Convert.ToString((a as TimeTrackingCurrentStage)?.IdStage))).Distinct().ToList();
                    //List<string> CPTypeIds = TimeTrackingList.Select(i => (i as TimeTracking)?.IdCPType.ToString()).Where(id => !string.IsNullOrEmpty(id)).Distinct().ToList();
                    if (StageName == "Total")
                    {
                        //  LstStandardOperationsDictionaryModules.AddRange(ERMService.GetAllStandardOperationsDictionaryModulesById_V2570(TimeTrackingListCP.IdStandardOperationsDictionary, Convert.ToUInt64(TimeTrackingListCP.IdProductionPlant), Convert.ToUInt64(TimeTrackingListCP.IdCPType), Convert.ToUInt64(TimeTrackingListCP.IdCP)));
                        // Nullable<UInt64> IdCPType = null;
                        // Nullable<UInt64> IdCP = null;
                        // Nullable<UInt64> IdStages = null;
                        // Nullable<int> i = null;
                        // CPIds = new List<string>();
                        //LstStandardOperationsDictionaryModules.AddRange(ERMService.GetAllStandardOperationsDictionaryModulesById_V2570(TimeTrackingListCP.IdStandardOperationsDictionary, StageIds, CPTypeIds, CPIds));
                        LstStandardOperationsDictionaryModules.AddRange(ERMService.GetAllStandardOperationsDictionaryModulesById_V2630(TimeTrackingListCP.IdStandardOperationsDictionary, StageIds, CPTypeIds, CPIds));
                        LstStandardOperationsDictionaryDetection.AddRange(ERMService.GetStandardOperationDictionaryDetectionById_V2570(CPIds));
                        LstStandardOperationsDictionaryOption.AddRange(ERMService.GetStandardOperationDictionaryOptionById_V2570(CPIds));
                        LstStandardOperationsDictionaryWays.AddRange(ERMService.GetStandardOperationDictionaryWayById_V2570(CPIds));


                        //  LstStandardOperationsDictionaryModules.Where(a => StageIds.Contains(Convert.ToString(a.IdStage))).ToList();
                        //LstStandardOperationsDictionaryModules.AddRange(ERMService.GetAllStandardOperationsDictionaryModulesById_V2570(TimeTrackingListCP.IdStandardOperationsDictionary, Convert.ToUInt64(TimeTrackingListCP.IdProductionPlant), null, Convert.ToUInt64(null)));
                        //LstStandardOperationsDictionaryDetection.AddRange(ERMService.GetStandardOperationDictionaryDetectionById_V2570(Convert.ToUInt64(null)));
                        //LstStandardOperationsDictionaryOption.AddRange(ERMService.GetStandardOperationDictionaryOptionById_V2570(Convert.ToUInt64(null)));
                        //LstStandardOperationsDictionaryWays.AddRange(ERMService.GetStandardOperationDictionaryWayById_V2570(Convert.ToUInt64(null)));

                    }
                    else
                    {
                        //LstStandardOperationsDictionaryModules.AddRange(ERMService.GetAllStandardOperationsDictionaryModulesById_V2570(TimeTrackingListCP.IdStandardOperationsDictionary, StageIds, CPTypeIds, CPIds));
                        LstStandardOperationsDictionaryModules.AddRange(ERMService.GetAllStandardOperationsDictionaryModulesById_V2630(TimeTrackingListCP.IdStandardOperationsDictionary, StageIds, CPTypeIds, CPIds));
                        LstStandardOperationsDictionaryDetection.AddRange(ERMService.GetStandardOperationDictionaryDetectionById_V2570(CPIds));
                        LstStandardOperationsDictionaryOption.AddRange(ERMService.GetStandardOperationDictionaryOptionById_V2570(CPIds));
                        LstStandardOperationsDictionaryWays.AddRange(ERMService.GetStandardOperationDictionaryWayById_V2570(CPIds));

                    }

                    //LstStandardOperationsDictionaryModules.AddRange(ERMService.GetAllStandardOperationsDictionaryModulesById_V2560(TimeTrackingListCP.IdStandardOperationsDictionary, Convert.ToUInt64(TimeTrackingListCP.IdProductionPlant), Convert.ToUInt64(TimeTrackingListCP.IdCPType), Convert.ToUInt64(TimeTrackingListCP.IdCP)));
                    //LstStandardOperationsDictionaryDetection.AddRange(ERMService.GetStandardOperationDictionaryDetectionById_V2560(Convert.ToUInt64(TimeTrackingListCP.IdCP)));
                    //LstStandardOperationsDictionaryOption.AddRange(ERMService.GetStandardOperationDictionaryOptionById_V2560(Convert.ToUInt64(TimeTrackingListCP.IdCP)));
                    //LstStandardOperationsDictionaryWays.AddRange(ERMService.GetStandardOperationDictionaryWayById_V2560(Convert.ToUInt64(TimeTrackingListCP.IdCP)));
                }

                CPsOperationsTimeInTimetrackingView cPsOperationsTimeInTimetrackingView = new CPsOperationsTimeInTimetrackingView();
                CPsOperationsTimeInTimetrackingViewModel cPsOperationsTimeInTimetrackingViewModel = new CPsOperationsTimeInTimetrackingViewModel();

                cPsOperationsTimeInTimetrackingViewModel.OTCode = data.OTCode;
                cPsOperationsTimeInTimetrackingViewModel.Item = data.NumItem + "(" + TimeTrackingListCP.DrawingType + ")";
                cPsOperationsTimeInTimetrackingViewModel.SerialNumber = data.SerialNumber;
                cPsOperationsTimeInTimetrackingViewModel.CPType = data.Type;
                if (IdStage == "Expected")
                {
                    StageName = string.Empty;
                }
                else
                {
                    cPsOperationsTimeInTimetrackingViewModel.WorkStation = StageName;
                }
                cPsOperationsTimeInTimetrackingViewModel.WorkStation = StageName;

                cPsOperationsTimeInTimetrackingViewModel.TotalExpectedTime = cellExpectedValue;
                cPsOperationsTimeInTimetrackingViewModel.IdCPType = Convert.ToUInt64(TimeTrackingListCP.IdCPType);
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                cPsOperationsTimeInTimetrackingViewModel.BandsCPOperation = BandsCPOperation;
                cPsOperationsTimeInTimetrackingView.DataContext = cPsOperationsTimeInTimetrackingViewModel;
                EventHandler handle = delegate { cPsOperationsTimeInTimetrackingView.Close(); };
                cPsOperationsTimeInTimetrackingViewModel.RequestClose += handle;

                // addEditModuleEquivalencyWeightViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditModulesEquivalencyWeight").ToString();
                // addEditModuleEquivalencyWeightViewModel.LblEquivalentWeight = "Module";
                //[GEOS2-6835][dhawal bhalerao][08 04 2025] : CPIds Parameter added into following Init method to get the expected time from SP
                cPsOperationsTimeInTimetrackingViewModel.Init(LstStandardOperationsDictionaryModules, LstStandardOperationsDictionaryWays, LstStandardOperationsDictionaryDetection, LstStandardOperationsDictionaryOption, IdStage, TempDesignValue, RoleValue, CPIds);

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                var ownerInfo = (detailView as FrameworkElement);
                cPsOperationsTimeInTimetrackingView.Owner = Window.GetWindow(ownerInfo);
                cPsOperationsTimeInTimetrackingView.ShowDialog();




                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method VerifyTheSamplePopupWindowCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method VerifyTheSamplePopupWindowCommandAction()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion


        private void ConvertTimeToMonthDaysFormat(TimeSpan TimeToConvert)
        {

            //int DaysInTime = TimeToConvert.Days;
            //if (DaysInTime > 0)
            //{
            //    string NewTime = Convert.ToString(TimeToConvert).Replace()
            //}
        }

        #region   [pallavi jadhav][GEOS2-7060][25-03-2025]
        private void setDefaultPeriod()
        {

            //int year = DateTime.Now.Year;
            //DateTime StartFromDate = new DateTime(year, 1, 1);
            //DateTime EndToDate = new DateTime(year, 12, 31);
            //FromDate = StartFromDate.ToString("dd/MM/yyyy");
            //ToDate = EndToDate.ToString("dd/MM/yyyy");
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                GeosApplication.Instance.Logger.Log("Method setDefaultPeriod ...", category: Category.Info, priority: Priority.Low);
                if (ERMCommon.Instance.SelectedAuthorizedPlantsList != null)
                {

                    List<Site> plantOwners = ERMCommon.Instance.SelectedAuthorizedPlantsList.Cast<Site>().ToList();
                    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.Name));
                    PreviouslySelectedPlantOwners = string.Join(",", plantOwners.Select(i => i.IdSite));
                    if (ERMCommon.Instance.FailedPlants == null)
                    {
                        ERMCommon.Instance.FailedPlants = new List<string>();
                    }
                    List<MaxMinDate> MaxMinDateList = new List<MaxMinDate>();
                    foreach (var itemPlantOwnerUsers in plantOwners)
                    {
                        try
                        {

                            GeosApplication.Instance.SplashScreenMessage = "Connecting to " + itemPlantOwnerUsers.Name;
                            //if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                            //==========================================================================================
                            string PlantName = Convert.ToString(itemPlantOwnerUsers.Name);
                            uint idSite = itemPlantOwnerUsers.IdSite;
                            string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.Name == itemPlantOwnerUsers.Name).Select(xy => xy.ServiceProviderUrl).FirstOrDefault();
                            ERMService = new ERMServiceController(serviceurl);
                            //ERMService = new ERMServiceController("localhost:6699");

                            MaxMinDateList.AddRange(ERMService.GetDeliveryDateANDPlannedDeliveryDate_V2630(Convert.ToInt32(idSite)));

                        }
                        catch (FaultException<ServiceException> ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed");
                            if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                if (!ERMCommon.Instance.FailedPlants.Contains(itemPlantOwnerUsers.Name))
                                {
                                    ERMCommon.Instance.FailedPlants.Add(itemPlantOwnerUsers.Name);
                                    if (ERMCommon.Instance.FailedPlants != null && FailedPlants.Count > 0)
                                    {
                                        ERMCommon.Instance.IsShowFailedPlantWarning = true;
                                        ERMCommon.Instance.WarningFailedPlants = string.Join(",", ERMCommon.Instance.FailedPlants.Select(x => x.ToString()).ToArray());
                                        ERMCommon.Instance.WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                                    }
                                }

                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed ", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                        }
                        catch (ServiceUnexceptedException ex)
                        {
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

                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }
                        catch (Exception ex)
                        {
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
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }
                    }

                    GeosApplication.Instance.SplashScreenMessage = string.Empty;



                     //ERMService = new ERMServiceController("localhost:6699");
                    //   MaxMinDateList = ERMService.GetDeliveryDateANDPlannedDeliveryDate_V2630(Convert.ToInt32(idsite));
                    if (MaxMinDateList.Count > 0)
                    {
                        DateTime? tempFromDate = MaxMinDateList.Select(a => a.MinStartDate).FirstOrDefault();
                        DateTime? tempToDate = MaxMinDateList.Select(a => a.MaxEndDate).FirstOrDefault();
                        FromDate = tempFromDate.HasValue ? tempFromDate.Value.ToString("dd/MM/yyyy") : string.Empty;
                        ToDate = tempToDate.HasValue ? tempToDate.Value.ToString("dd/MM/yyyy") : string.Empty;
                    }
                }

                GeosApplication.Instance.Logger.Log("Method setDefaultPeriod() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log("Get an error in setDefaultPeriod() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log("Get an error in setDefaultPeriod() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log("Get an error on setDefaultPeriod() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            GeosApplication.Instance.SplashScreenMessage = string.Empty;

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
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                MenuFlyout menu = (MenuFlyout)obj;
                _currentDuration = menu.FlyoutControl.AnimationDuration;
                menu.FlyoutControl.AnimationDuration = new System.Windows.Duration(TimeSpan.FromMilliseconds(1));
                menu.FlyoutControl.Closed += FlyoutControl_Closed;
                menu.IsOpen = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
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
                //[GEOS2-7060][gulab lakade][21 04 2025]
                //This Year
                int Thhisyear = DateTime.Now.Year;
                DateTime ThisStartFromDate = new DateTime(Thhisyear, 1, 1);
                DateTime ThisEndToDate = new DateTime(Thhisyear, 12, 31);
                //end [GEOS2-7060][gulab lakade][21 04 2025]
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
                    //setDefaultPeriod();
                    //[GEOS2-7060][gulab lakade][21 04 2025]
                    FromDate = ThisStartFromDate.ToString("dd/MM/yyyy");
                    ToDate = ThisEndToDate.ToString("dd/MM/yyyy");
                    //end [GEOS2-7060][gulab lakade][21 04 2025]
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
                    SelectedPlant = new List<object>();
                    SelectedPlant = ERMCommon.Instance.SelectedAuthorizedPlantsList;
                    //end
                    if (SelectedPlant != null)
                    {
                        FillSelectedPlant();
                        FillDeliveryweek();
                    }


                }
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

        private void CancelCommandAction(object obj)
        {
            MenuFlyout menu = (MenuFlyout)obj;
            menu.IsOpen = false;
        }

        private void UpdateColor()
        {
            if (IsSelectedOTNumberNotEmpty)
            {
                Color = "Black";
            }
            else
            {
                Color = "White"; // or any other default color
            }
        }

        private void ChangeOTNumberCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChangeCustomerGroupCommandAction()...", category: Category.Info, priority: Priority.Low);





                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                SelectedOTNumber = new List<object>();
                if (obj == null)
                {
                }
                else
                {

                    //DevExpress.Xpf.Editors.ClosePopupEventArgs CloseModetemp = (DevExpress.Xpf.Editors.ClosePopupEventArgs)obj;
                    DevExpress.Xpf.Editors.TextEdit textEdit = (DevExpress.Xpf.Editors.TextEdit)obj;
                    if (textEdit.EditValue == null)
                    {
                        TimeTrackingList = new List<TimeTracking>(TimeTrackingList_Cloned);
                    }
                    else
                    {
                        var TempSelectedOTNumber = textEdit.EditValue;
                        // List<Object> TmpSelectedOTNumber = new List<object>();
                        // List<string> SelectedOTNumber = new List<string>();
                        if (TempSelectedOTNumber != null && TempSelectedOTNumber != "")
                        {
                            SelectedOTNumber.Add(Convert.ToString(TempSelectedOTNumber));

                        }

                        if (SelectedItems.Count >= 0)
                        {
                            List<TimeTracking> filteredData = TimeTrackingList.Where(i => SelectedOTNumber.Any(n => i.OTCode.ToString().Contains((string)n))).ToList();
                            TimeTrackingList = new List<TimeTracking>(filteredData);
                        }
                        else
                        {
                            //   List<TimeTracking> filteredData = new List<TimeTracking>(TimeTrackingList_Cloned.Where(i => SelectedOTNumber.Contains(Convert.ToString(i.OTCode))).Distinct().ToList());
                            List<TimeTracking> filteredData = TimeTrackingList_Cloned.Where(i => SelectedOTNumber.Any(n => i.OTCode.ToString().Contains((string)n))).ToList();
                            TimeTrackingList = new List<TimeTracking>(filteredData);
                        }

                    }
                    if (TimeTrackingList != null && TimeTrackingList.Count > 0)
                    {
                        // TimeTrackingList = new List<TimeTracking>(OTNumberList.ToList());
                        FillDashboard();
                        //  FillOTNumber();

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

        private void FillOTNumber()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillOTNumber()...", category: Category.Info, priority: Priority.Low);

                if (TimeTrackingList_Cloned != null && TimeTrackingList_Cloned.Count > 0)
                {
                    //OTNumberList = new List<TimeTracking>();
                    OTNumberList = new List<TimeTracking>(TimeTrackingList_Cloned.GroupBy(o => o.OTNumber).Select(g => g.First()).ToList());
                    if (OTNumberList.Count > 0)
                    {
                        //SelectedOTNumber = new List<object>();
                        //   SelectedOTNumber = new List<object>(OTNumberList.ToList());

                        //     var otnumbers = OTNumberList.Select(a => a.OTNumber).ToList().Distinct();
                        //   SelectedOTNumber = new List<object>(OTNumberList.Distinct().ToList());


                    }

                }
                GeosApplication.Instance.Logger.Log("Method FillOTNumber()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillOTNumber()-2 Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void SearchCommandCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChangeCustomerGroupCommandAction()...", category: Category.Info, priority: Priority.Low);





                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                SelectedItems = new List<object>();
                SelectedOTNumber = new List<object>();
                if (obj == null)
                {
                }
                else
                {
                    if (obj is object[] values)
                    {
                        string OTCode = values[0]?.ToString();
                        string Item = values[1]?.ToString();
                        string TempSelectedOTCode = string.Empty;
                        string TempSelectedItem = string.Empty;


                        //if (OTCode == "" && Item == "")
                        if ((string.IsNullOrEmpty(OTCode) || OTCode == "") && (string.IsNullOrEmpty(Item) || Item == ""))//[GEOS2-7880][gulab lakade][18 04 2025]
                        {
                            TimeTrackingList = new List<TimeTracking>(TimeTrackingList_Cloned);
                        }
                        //else if (OTCode != "" && Item == "")
                        else if ((!string.IsNullOrEmpty(OTCode) && OTCode != "") && (string.IsNullOrEmpty(Item) || Item == "")) //[GEOS2-7880][gulab lakade][18 04 2025]
                        {
                            #region [GEOS2-7880][gulab lakade][18 04 2025]
                            //TempSelectedOTCode = OTCode;
                            //SelectedOTNumber.Add(Convert.ToString(TempSelectedOTCode));
                            //List<TimeTracking> filteredData = TimeTrackingList_Cloned.Where(i => SelectedOTNumber.Any(n => i.OTCode.ToString().Contains((string)n))).ToList();
                            //TimeTrackingList = new List<TimeTracking>(filteredData);
                            //TempSelectedOTCode = OTCode;
                            //SelectedOTNumber.Add(Convert.ToString(TempSelectedOTCode));
                            List<TimeTracking> filteredData = TimeTrackingList_Cloned.Where(i  => i.OTCode.ToString().Contains(OTCode)).ToList();
                            TimeTrackingList = new List<TimeTracking>(filteredData);
                            #endregion 
                        }
                        //else if (OTCode == "" && Item != "")
                        else if ((string.IsNullOrEmpty(OTCode) || OTCode == "") && (!string.IsNullOrEmpty(Item) && Item != ""))//[GEOS2-7880][gulab lakade][18 04 2025]
                        {
                            TempSelectedItem = Item;
                            SelectedItems.Add(Convert.ToString(TempSelectedItem));
                            List<TimeTracking> filteredData = TimeTrackingList_Cloned.Where(i => SelectedItems.Contains(Convert.ToString(i.NumItem))).ToList();
                            TimeTrackingList = new List<TimeTracking>(filteredData);
                        }
                        else
                        {
                            #region [GEOS2-7880][gulab lakade][18 04 2025]
                            //TempSelectedOTCode = OTCode;
                            //SelectedOTNumber.Add(Convert.ToString(TempSelectedOTCode));
                            //TempSelectedItem = Item;
                            //SelectedItems.Add(Convert.ToString(TempSelectedItem));

                            //List<TimeTracking> filteredData = TimeTrackingList_Cloned.Where(i => SelectedOTNumber.Any(n => i.OTCode.ToString().Contains((string)n))).ToList();
                            //TimeTrackingList = new List<TimeTracking>(filteredData);
                            //if (TimeTrackingList.Count > 0)
                            //{
                            //    List<TimeTracking> filteredData1 = TimeTrackingList.Where(i => SelectedItems.Contains(Convert.ToString(i.NumItem))).ToList();
                            //    TimeTrackingList = new List<TimeTracking>(filteredData1);
                            //}
                            List<TimeTracking> filteredData = TimeTrackingList_Cloned.Where(i => i.OTCode.ToString().Contains(OTCode) && i.NumItem.ToString().Contains(Item)).ToList();
                            TimeTrackingList = new List<TimeTracking>(filteredData);
                            #endregion

                        }
                    }
                    //DevExpress.Xpf.Editors.ClosePopupEventArgs CloseModetemp = (DevExpress.Xpf.Editors.ClosePopupEventArgs)obj;
                    //DevExpress.Xpf.Editors.TextEdit textEdit = (DevExpress.Xpf.Editors.TextEdit)obj;
                    //if (textEdit.EditValue == null)
                    //{
                    //    TimeTrackingList = new List<TimeTracking>(TimeTrackingList_Cloned);
                    //}
                    //else
                    //{
                    //    var TempSelectedItem = textEdit.EditValue;
                    //    // List<Object> TmpSelectedOTNumber = new List<object>();
                    //    List<string> SelectedItem = new List<string>();
                    //    if (TempSelectedItem != null && TempSelectedItem != "")
                    //    {
                    //        SelectedItems.Add(Convert.ToString(TempSelectedItem));

                    //    }

                    //    if (SelectedOTNumber.Count >=0)
                    //    {
                    //        List<TimeTracking> filteredData = TimeTrackingList.Where(i => SelectedItems.Contains(Convert.ToString(i.NumItem))).ToList();
                    //        TimeTrackingList = new List<TimeTracking>(filteredData);
                    //    }
                    //    else
                    //    {

                    //        //   List<TimeTracking> filteredData = new List<TimeTracking>(TimeTrackingList_Cloned.Where(i => SelectedOTNumber.Contains(Convert.ToString(i.OTCode))).Distinct().ToList());
                    //        List<TimeTracking> filteredData = TimeTrackingList_Cloned.Where(i => SelectedItems.Contains(Convert.ToString(i.NumItem))).ToList();
                    //        TimeTrackingList = new List<TimeTracking>(filteredData);
                    //    }
                    //}
                    if (TimeTrackingList != null && TimeTrackingList.Count > 0)
                    {
                        // TimeTrackingList = new List<TimeTracking>(OTNumberList.ToList());
                        FillDashboard();
                        //  FillOTNumber();

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

        #endregion

        private class ReworkData
        {
            public Int64 IdOT;
            public Int64 IdOTItem;
            public Int64 IdDrawing;
            public Int64 IdCounterpart;
            public Int64 IdWorkbookOfCpProducts;//Aishwarya Ingale[Geos2-6034]
        }
        #region [GEOS2-5854][gulab lakade][18 07 2024]
        private void FillCADCAMDesignTypeList()
        {
            try
            {
                List<GeosAppSetting> AppSetting = new List<GeosAppSetting>();
                AppSetting = WorkbenchStartUp.GetSelectedGeosAppSettings("124");
                if (AppSetting.Count > 0)
                {
                    CADCAMDesignTypeList = new List<ERM_CADCAMTimePerDesignType>();
                    List<string> tempWorkStageList = new List<string>();
                    foreach (var item in AppSetting)
                    {
                        string tempstring = Convert.ToString(item.DefaultValue.Replace('(', ' '));
                        tempstring = tempstring.Replace(')', ' ');
                        tempWorkStageList = Convert.ToString(tempstring).Split(',').ToList();
                    }
                    if (tempWorkStageList.Count > 0)
                    {
                        foreach (var item in tempWorkStageList)
                        {
                            List<string> tempIDStageList = Convert.ToString(item.Trim()).Split(';').ToList();
                            if (tempIDStageList.Count() > 0)
                            {
                                ERM_CADCAMTimePerDesignType selectCADCAMDesignTypeList = new ERM_CADCAMTimePerDesignType();
                                selectCADCAMDesignTypeList.IdStage = Convert.ToInt32(tempIDStageList[0]);
                                selectCADCAMDesignTypeList.DesignType = Convert.ToString(tempIDStageList[1]);
                                selectCADCAMDesignTypeList.DesignValue = Convert.ToInt32(tempIDStageList[2]);
                                selectCADCAMDesignTypeList.RoleValue = Convert.ToString(tempIDStageList[3]);
                                CADCAMDesignTypeList.Add(selectCADCAMDesignTypeList);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in FillCADCAMDesignTypeList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        #endregion

    }


}
