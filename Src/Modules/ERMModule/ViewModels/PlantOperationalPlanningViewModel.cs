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
using System.Windows.Controls;

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
using DevExpress.Xpf.WindowsUI;
using DevExpress.Xpf.Editors.Flyout;
using DevExpress.Xpf.Bars;
using Microsoft.Win32;
using System.Threading;
using DevExpress.Mvvm.Xpf;
using DevExpress.Xpf.Editors;

namespace Emdep.Geos.Modules.ERM.ViewModels
{
    public class PlantOperationalPlanningViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Service
        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        IPLMService PLMService = new PLMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IERMService ERMService = new ERMServiceController("localhost:6699");
        //IPLMService PLMService = new PLMServiceController("localhost:6699");
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IERMService ERMService = new ERMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion
        #region Declaration
        private bool isInit;
        private string myFilterString;
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
        string fromDate;
        string toDate;
        private List<object> selectedPlantold;
        int isButtonStatus;
        DateTime startDate;
        DateTime endDate;
        Visibility isCalendarVisible;
        Visibility isHoliday;
        private bool isSaveEnabled;
        private Duration _currentDuration;

        DateTime? fromDates;
        DateTime? toDates;
        private bool isPeriod;
        private string searchOTItem;
        private bool isBusy;
        private ObservableCollection<BandItem> bands = new ObservableCollection<BandItem>();
        private DataTable dataTableForGridLayout;
        DataTable dtPlantOperation;
        private List<GeosAppSetting> workStages = new List<GeosAppSetting>();
        private string idstages;
        private List<ERMPlantOperationalPlanning> plantOperationalPlanning = new List<ERMPlantOperationalPlanning>();
        private string jobDescriptioID;
        private List<ERMEmployeePlantOperation> employeeplantOperationalList = new List<ERMEmployeePlantOperation>();
        private ERMEmployeePlantOperation employeeplantOperational = new ERMEmployeePlantOperation();
        public virtual string ResultFileName { get; protected set; }
        public virtual bool DialogResult { get; protected set; }
        private List<ERMNonOTItemType> nonOTTimeTypesList; //[GEOS2-4553][Rupali Sarode][09-06-2023]
        private List<ERMEmployeePlantOperation> employeeplantOperationalListForRealTime = new List<ERMEmployeePlantOperation>(); //[GEOS2-4553][Rupali Sarode][19-06-2023]

        private Visibility isButtonVisible;
        ObservableCollection<PlanningDateReviewStages> plantOperationStagesList;//[GEOS2-4708][gulab lakade][25 07 2023]
        private Int32 iD;//[GEOS2-4708][gulab lakade][25 07 2023]
        public List<TempSelctPlantByWeek> tempSelctPlantByWeek = new List<TempSelctPlantByWeek>();  //[GEOS2-4839][gulab lakade][20 09 2023]
        #endregion // Declaration
        #region Public Properties

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


        private ObservableCollection<Company> plantList;
        public ObservableCollection<Company> PlantList
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
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPlant"));
            }
        }
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
        private object selectedItem;
        public virtual object SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                GetRecordbyCalendarWeek();
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedItem"));
            }
        }

        private object selectedItemOld;
        public virtual object SelectedItemOld
        {
            get { return selectedItemOld; }
            set
            {
                selectedItemOld = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedItemOld"));
            }
        }

        public bool IsSaveEnabled
        {
            get
            {
                return isSaveEnabled;
            }
            set
            {
                isSaveEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSaveEnabled"));
            }
        }
        private ObservableCollection<PlanningDateAccordian> planningDate;
        public ObservableCollection<PlanningDateAccordian> PlanningDate
        {
            get
            {
                return planningDate;
            }

            set
            {
                planningDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PlanningDate"));
            }
        }

        private ObservableCollection<PlanningDeliveryDate> planningDeliveryDate;
        public ObservableCollection<PlanningDeliveryDate> PlanningDeliveryDate
        {
            get
            {
                return planningDeliveryDate;
            }

            set
            {
                planningDeliveryDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PlanningDeliveryDate"));
            }
        }

        public bool IsPeriod
        {
            get { return isPeriod; }
            set { isPeriod = value; }
        }
        public string SearchOTItem
        {
            get
            {
                return searchOTItem;
            }

            set
            {
                searchOTItem = value;

                OnPropertyChanged(new PropertyChangedEventArgs("SearchOTItem"));
            }
        }
        private string accordianFromDate;
        public string AccordianFromDate
        {
            get
            {
                return accordianFromDate;
            }

            set
            {
                accordianFromDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AccordianFromDate"));
            }
        }
        private bool isAccordianFromDate;
        public bool IsAccordianFromDate
        {
            get { return isAccordianFromDate; }
            set { isAccordianFromDate = value; }
        }




        private bool isVisibleChanged;
        public bool IsVisibleChanged
        {
            get { return isVisibleChanged; }
            set
            {
                isVisibleChanged = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsVisibleChanged"));
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

        ObservableCollection<PlanningDateAccordian> templateWithPlanningDate;
        public ObservableCollection<PlanningDateAccordian> TemplateWithPlanningDate
        {
            get { return templateWithPlanningDate; }
            set
            {
                templateWithPlanningDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TemplateWithPlanningDate"));
            }
        }
        public List<PlantOperationProductionStage> PlantOperationProductionStage { get; set; }
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
        public DataTable DtPlantOperation
        {
            get { return dtPlantOperation; }
            set
            {
                dtPlantOperation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DtPlantOperation"));
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
        Tuple<int, string, string> IdWorkStage_IdJobDescription = new Tuple<int, string, string>(1, "", "");
        private List<ERMWorkStageWiseJobDescription> workStageWiseJobDescription = new List<ERMWorkStageWiseJobDescription>();
        public List<ERMWorkStageWiseJobDescription> WorkStageWiseJobDescription
        {
            get { return workStageWiseJobDescription; }
            set
            {
                workStageWiseJobDescription = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkStageWiseJobDescription"));
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

        public string Idstages
        {
            get { return idstages; }
            set
            {
                idstages = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Idstages"));
            }
        }
        public string JobDescriptioID
        {
            get
            {
                return jobDescriptioID;
            }

            set
            {
                jobDescriptioID = value;
                OnPropertyChanged(new PropertyChangedEventArgs("JobDescriptioID"));
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
        ObservableCollection<PlantOperationPlanningAccordion> templateWithPlantAccordion;
        public ObservableCollection<PlantOperationPlanningAccordion> TemplateWithPlantAccordion
        {
            get { return templateWithPlantAccordion; }
            set
            {
                templateWithPlantAccordion = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TemplateWithPlantAccordion"));
            }
        }
        public List<ERMEmployeePlantOperation> EmployeeplantOperationallistCopy { get; set; }
        private ObservableCollection<ParentBandItem> bands_FirstLevel;
        public ObservableCollection<ParentBandItem> Bands_FirstLevel
        {
            get { return bands_FirstLevel; }
            set
            {
                bands_FirstLevel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Bands_FirstLevel"));
            }
        }
        public ObservableCollection<Summary> TotalSummary { get; private set; }
        public ObservableCollection<Summary> GroupSummary { get; private set; }////[GEOS2-4549][gulablakade][08 06 2023]

        private List<PlantOperationWeek> plantWeekList = new List<PlantOperationWeek>();

        public List<PlantOperationWeek> PlantWeekList
        {
            get { return plantWeekList; }
            set
            {
                plantWeekList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PlantWeekList"));
            }
        }
        private PlantOperationWeek plantWeek = new PlantOperationWeek();
        public PlantOperationWeek PlantWeek
        {
            get { return plantWeek; }
            set
            {
                plantWeek = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PlantWeek"));
            }
        }

        #region [GEOS2-4553][Rupali Sarode][09-06-2023]
        public List<ERMNonOTItemType> NonOTTimeTypesList
        {
            get { return nonOTTimeTypesList; }
            set
            {
                nonOTTimeTypesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NonOTTimeTypesList"));
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
        #endregion [GEOS2-4553][Rupali Sarode][09-06-2023]

        public Visibility IsButtonVisible
        {
            get { return isButtonVisible; }
            set
            {
                isButtonVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsButtonVisible"));
            }
        }
        //start[GEOS2-4708][gulab lakade][25 07 2023]
        public ObservableCollection<PlanningDateReviewStages> PlantOperationStagesList
        {

            get
            {
                return plantOperationStagesList;
            }

            set
            {
                plantOperationStagesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PlantOperationStagesList"));
            }

        }

        public Int32 ID
        {
            get { return iD; }
            set
            {
                iD = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ID"));
            }
        }
        List<string> stageCodes = new List<string>();
        List<int> iDJobdescriptionlist = new List<int>();
        public List<int> IDJobdescriptionlist
        {
            get { return iDJobdescriptionlist; }
            set
            {
                iDJobdescriptionlist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IDJobdescriptionlist"));
            }
        }
        public List<PlantOperationProductionStage> TempPlantOperationProductionStage = new List<PlantOperationProductionStage>();

        public List<ERMWorkStageWiseJobDescription> TempWorkStageWiseJobDescription = new List<ERMWorkStageWiseJobDescription>();
        public List<ERMEmployeePlantOperation> TempEmployeeplantOperationallist = new List<ERMEmployeePlantOperation>();
        //end [GEOS2-4708][gulab lakade][25 07 2023]
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
        public ICommand RefreshPlantOperationalPlanningCommand { get; set; }
        public ICommand PrintPlantOperationalPlanningCommand { get; set; }
        public ICommand ExportPlantOperationalPlanningCommand { get; set; }
        public ICommand ChangePlantCommand { get; set; }
        public ICommand TableViewLoadedCommand { get; set; }
        public ICommand TableViewUnloadedCommand { get; set; }
        public ICommand ItemListTableViewLoadedCommand { get; set; }
        public ICommand CustomSummaryCommand { get; set; }
        public ICommand CommandAccordionControl_Drop { get; set; }
        public ICommand HidePanelCommand { get; set; }
        public ICommand PeriodCommand { get; set; }
        public ICommand ApplyCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand ShowGridViewCommand { get; private set; }
        public ICommand DefaultLoadCommand { get; set; }
        public ICommand SelectedIntervalCommand { get; set; }
        public ICommand PeriodCustomRangeCommand { get; set; }
        public ICommand DefaultUnLoadCommand { get; set; }
        public ICommand ShowPlanningDateValidationCommand { get; set; }
        public ICommand PrintPlanningDateReviewCommand { get; set; }
        public ICommand ExportPlanningDateReviewCommand { get; set; }

        public ICommand PlantOperationalTimeTypeRealCommand { get; set; }
        public ICommand FilterOptionLoadedCommand { get; set; }//[GEOS2-4708][gulab lakade][25 07 2023]
        public ICommand FilterOptionEditValueChangedCommand { get; set; }//[GEOS2-4708][gulab lakade][25 07 2023]
        #endregion
        #region Constructor
        public PlantOperationalPlanningViewModel()
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Constructor PlantOperationalPlanningViewModel()...", category: Category.Info, priority: Priority.Low);
                IsInit = true;

                RefreshPlantOperationalPlanningCommand = new RelayCommand(new Action<object>(RefreshPlantOperationalPlanningCommandAction));
                PrintPlantOperationalPlanningCommand = new RelayCommand(new Action<object>(PrintPlantOperationalPlanningCommandAction));
                ExportPlantOperationalPlanningCommand = new RelayCommand(new Action<object>(ExportPlantOperationalPlanningCommandAction));
                HidePanelCommand = new RelayCommand(new Action<object>(HidePanel));
                PeriodCommand = new DelegateCommand<object>(PeriodCommandAction);
                PeriodCustomRangeCommand = new DelegateCommand<object>(PeriodCustomRangeCommandAction);
                ApplyCommand = new DelegateCommand<object>(ApplyCommandAction);
                CancelCommand = new DelegateCommand<object>(CancelCommandAction);
                ChangePlantCommand = new DelegateCommand<object>(ChangePlantCommandAction);
                PlantOperationalTimeTypeRealCommand = new DelegateCommand<object>(PlantOperationalTimeTypeRealCommandAction);
                CustomSummaryCommand = new DelegateCommand<object>(CustomSummaryCommandAction); //[GEOS2-4549][rupali sarode][12-07-2023]
                                                                                                //ShowGridViewCommand = new RelayCommand(new Action<object>(ShowPlanningDateGridView));
                                                                                                // RefreshPlanningDateCommand = new RelayCommand(new Action<object>(RefreshPlanningDateCommandAction));
                                                                                                //AppointmentDropCommand = new DelegateCommand<AppointmentItemDragDropEventArgs>(AppointmentDropCommandAction);
                                                                                                //PopupMenuShowingCommand = new DelegateCommand<PopupMenuShowingEventArgs>(PopupMenuShowingCommandAction);

                //TableViewLoadedCommand = new DelegateCommand<RoutedEventArgs>(TableViewLoadedCommandAction);
                //TableViewUnloadedCommand = new DelegateCommand<RoutedEventArgs>(TableViewUnloadedCommandAction);
                //ShowPlanningDateValidationCommand = new RelayCommand(new Action<object>(ShowPlanningDateValidationCommandAction));
                FilterOptionLoadedCommand = new RelayCommand(new Action<object>(FilterOptionLoadedCommandAction));//[GEOS2-4708][gulab lakade][25 07 2023]
                FilterOptionEditValueChangedCommand = new DevExpress.Mvvm.DelegateCommand<object>(FilterOptionEditValueChangedCommandAction);//[GEOS2-4708][gulab lakade][25 07 2023]
                GeosApplication.Instance.Logger.Log("Constructor Constructor PlantOperationalPlanningViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Constructor PlantOperationalPlanningViewModel()." + ex.Message, category: Category.Exception, priority: Priority.Low);
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


                IsPeriod = false;
                IsVisibleChanged = true;
                IsCalendarVisible = Visibility.Collapsed;
                ERMCommon.Instance.IsShowFailedPlantWarning = false;//[GEOS2-4839][gulab lakade][20 09 2023]
                ERMCommon.Instance.WarningFailedPlants = string.Empty;//[GEOS2-4839][gulab lakade][20 09 2023]
                // SelectedPlant = (Site)ERMCommon.Instance.SelectedAuthorizedPlantsList.FirstOrDefault();   //[GEOS2-4839][gulab lakade][20 09 2023]

                //SelectedPlant = ERMCommon.Instance.SelectedAuthorizedPlantsList;
                // GetPlants();
                GetIdStageAndJobDescriptionByAppSetting();
                setDefaultPeriod();
                FillStages();//[GEOS2-4708][gulab lakade][25 07 2023]
                GetWeekList();
                FillProductionStage(Idstages);
                AddColumnsToDataTableWithoutBands();
                FillPlantOperationPlanningData();
                FillAllNonOTTimes();   ///[GEOS2-4553][gulab lakade][14 06 2023]
                // FillCalenderweek();
                // FillDashboard();

                StartDate = DateTime.Today.Date;
                EndDate = DateTime.Today.Date;
                IsSaveEnabled = false;

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method Init() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init()", category: Category.Exception, priority: Priority.Low);
            }

        }
        #region GetWeekList
        private void GetWeekList()
        {
            try
            {

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                GeosApplication.Instance.Logger.Log("Method GetWeekList ...", category: Category.Info, priority: Priority.Low);

                #region

                CultureInfo CultureEnglish = new CultureInfo("en-GB");
                var culture = System.Threading.Thread.CurrentThread.CurrentCulture;
                DateTime TempFromDate = DateTime.Parse(FromDate, CultureEnglish, DateTimeStyles.AdjustToUniversal);
                int MinweekNum = CultureEnglish.Calendar.GetWeekOfYear(Convert.ToDateTime(TempFromDate).Date, CalendarWeekRule.FirstFourDayWeek, culture.DateTimeFormat.FirstDayOfWeek);
                // int MaxweekNum = CultureEnglish.Calendar.GetWeekOfYear(Convert.ToDateTime(ToDate).Date, CalendarWeekRule.FirstFourDayWeek, culture.DateTimeFormat.FirstDayOfWeek);

                var diff = Convert.ToDateTime(TempFromDate).Date.DayOfWeek - culture.DateTimeFormat.FirstDayOfWeek;

                if (diff < 0)
                {
                    diff += 7;
                }

                DateTime FirstDateOfWeek = Convert.ToDateTime(TempFromDate).Date.AddDays(-diff).Date;

                DateTime LastDateOfWeek = FirstDateOfWeek.AddDays(6);
                PlantWeekList = new List<PlantOperationWeek>();
                DateTime EndDate = DateTime.Parse(ToDate, CultureEnglish, DateTimeStyles.AdjustToUniversal); //DateTime.Parse(ToDate);

                while (FirstDateOfWeek.Date < EndDate.Date)
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
                #endregion
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method GetWeekList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {

            }
        }
        #endregion
        #region GetIdStageAndJobDescriptionByAppSetting,FillProductionStage,AddColumnsToDataTableWithoutBands,FillDashboard, FillPlantOperationPlanningData,FillCalenderWeek,FillEmployeeData,GetRecordbyCalendarWeek
        private void GetIdStageAndJobDescriptionByAppSetting()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GetIdStageAndJobDescriptionByAppSetting ...", category: Category.Info, priority: Priority.Low);
                //if(SelectedPlant!=null)
                //{
                //    string PlantName = Convert.ToString(SelectedPlant.Name);
                //    string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.Name == PlantName).Select(xy => xy.ServiceProviderUrl).FirstOrDefault();
                //    WorkbenchStartUp = new WorkbenchServiceController(serviceurl);
                //}
                Idstages = string.Empty;
                jobDescriptioID = string.Empty;
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
                            //string tempstring = Convert.ToString(item.Remove('0',','));
                            List<string> tempIDStageList = Convert.ToString(item.Trim()).Split(';').ToList();
                            if (tempIDStageList.Count > 0)
                            {
                                if (!string.IsNullOrEmpty(tempIDStageList[0]) && !string.IsNullOrEmpty(tempIDStageList[1]))
                                {
                                    ERMWorkStageWiseJobDescription IDStage = new ERMWorkStageWiseJobDescription();
                                    string tempstring = Convert.ToString(tempIDStageList[0].Replace(',', ' '));
                                    IDStage.IdWorkStage = Convert.ToInt32(tempstring.Trim());
                                    //IDStage.IdJobDescription = Convert.ToString(tempIDStageList[1].Trim());
                                    IDStage.IdJobDescription = new List<string>();
                                    //TempJobDescriptionID = Convert.ToString(tempIDStageList[1].Trim()).Split(',').ToList();
                                    TempJobDescriptionID.AddRange(Convert.ToString(tempIDStageList[1].Trim()).Split(',').ToList());
                                    //IDStage.IdJobDescription = TempJobDescriptionID;
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
                                    if (string.IsNullOrEmpty(jobDescriptioID))
                                    {
                                        jobDescriptioID = Convert.ToString(Tempitem);
                                    }
                                    else
                                    {
                                        jobDescriptioID = jobDescriptioID + "," + Convert.ToString(Tempitem);
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
        private void FillProductionStage(string Idstages)
        {
            try
            {

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                GeosApplication.Instance.Logger.Log("Method FillProductionStage ...", category: Category.Info, priority: Priority.Low);
                if (SelectedPlant != null)
                {
                    string PlantName = Convert.ToString(SelectedPlant.Name);
                    string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.Name == PlantName).Select(xy => xy.ServiceProviderUrl).FirstOrDefault();
                    ERMService = new ERMServiceController(serviceurl);
                }
                PlantOperationProductionStage = new List<PlantOperationProductionStage>();
                //ERMService = new ERMServiceController("localhost:6699");
                PlantOperationProductionStage.AddRange(ERMService.GetAllPlantOperationProductioStage_V2380(Idstages));

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method FillProductionStage() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillProductionStage() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //private void AddColumnsToDataTableWithoutBands()
        //{
        //    try
        //    {
        //        if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

        //        CultureInfo ci = new CultureInfo(Thread.CurrentThread.CurrentCulture.Name);
        //        if (Bands_FirstLevel == null)
        //        {
        //            Bands_FirstLevel = new ObservableCollection<ParentBandItem>();
        //        }
        //        else
        //        {
        //            Bands_FirstLevel.Clear();
        //        }
        //        ParentBandItem ParentBandItem1 = new ParentBandItem()
        //        {
        //            Header = "ParentBandItem1",
        //            Name = "ParentBandItem1",
        //            HeaderToolTip = ""
        //        };
        //        Bands_FirstLevel.Add(ParentBandItem1);

        //        BandItem band1 = new BandItem() { BandHeader = "CalenderWeek" };
        //        band1.Columns = new ObservableCollection<ColumnItem>();
        //        band1.Columns.Add(new ColumnItem() { ColumnFieldName = "CalenderWeek", HeaderText = "", Width = 80, IsVertical = false, PlantOperationPlanningSetting = PlantOperationPlanningColumnTemplateSelector.PlantOperationPlanningSettingType.CalenderWeek, Visible = true });

        //        Bands_FirstLevel[0].Bands.Add(band1);
        //        DataTableForGridLayout = new DataTable();
        //        DataTableForGridLayout.Columns.Add("CalenderWeek", typeof(string));

        //        ParentBandItem ParentBandItem = new ParentBandItem();
        //        List<PlantOperationProductionStage> tempCurrentstage = new List<PlantOperationProductionStage>();

        //        foreach (PlantOperationProductionStage item in PlantOperationProductionStage)
        //        {
        //            if (Convert.ToString(item) != null)
        //            {

        //                tempCurrentstage.Add(item);
        //                ParentBandItem = new ParentBandItem()
        //                {
        //                    Name = Convert.ToString(item.StageCode),
        //                    Header = Convert.ToString(item.StageCode),
        //                    Visible = true
        //                };
        //                BandItem bandEMP = new BandItem()
        //                {
        //                    BandName = "Employee_" + Convert.ToString(item.IdStage),
        //                    BandHeader = "Employee",
        //                    Visible = true
        //                };
        //                BandItem bandHRExpected = new BandItem()
        //                {
        //                    BandName = "HRExpected_" + Convert.ToString(item.IdStage),
        //                    BandHeader = "HR " + System.Environment.NewLine + "Expected",
        //                    Visible = true
        //                };
        //                BandItem bandHRPlan = new BandItem()
        //                {
        //                    BandName = "HRPlan_" + Convert.ToString(item.IdStage),
        //                    BandHeader = "HR" + System.Environment.NewLine + "Plan",
        //                    Visible = true
        //                };
        //                BandItem bandHRResult = new BandItem()
        //                {
        //                    BandName = "HR Result_" + Convert.ToString(item.IdStage),
        //                    BandHeader = "HR Result",
        //                    Visible = true
        //                };
        //                bandHRResult.Columns = new ObservableCollection<ColumnItem>();

        //                bandEMP.Columns = new ObservableCollection<ColumnItem>();
        //                bandHRExpected.Columns = new ObservableCollection<ColumnItem>();
        //                bandHRPlan.Columns = new ObservableCollection<ColumnItem>();
        //                bandEMP.Columns.Add(new ColumnItem() { ColumnFieldName = "Employee_" + Convert.ToString(item.IdStage), HeaderText = "", Width = 200, Visible = true, IsVertical = false, PlantOperationPlanningSetting = PlantOperationPlanningColumnTemplateSelector.PlantOperationPlanningSettingType.Employee });
        //                DataTableForGridLayout.Columns.Add("Employee_" + Convert.ToString(item.IdStage), typeof(string));
        //                bandHRExpected.Columns.Add(new ColumnItem() { ColumnFieldName = "HRExpected_" + Convert.ToString(item.IdStage), HeaderText = "", Width = 70, Visible = true, IsVertical = false, PlantOperationPlanningSetting = PlantOperationPlanningColumnTemplateSelector.PlantOperationPlanningSettingType.HRExpected });
        //                DataTableForGridLayout.Columns.Add("HRExpected_" + Convert.ToString(item.IdStage), typeof(string));
        //                bandHRPlan.Columns.Add(new ColumnItem() { ColumnFieldName = "HRPlan_" + Convert.ToString(item.IdStage), HeaderText = "", Width = 60, Visible = true, IsVertical = false, PlantOperationPlanningSetting = PlantOperationPlanningColumnTemplateSelector.PlantOperationPlanningSettingType.HRPlan });//DataTableForGridLayout.Columns.Add(totalSummaryField, typeof(double)).AllowDBNull = true;
        //                DataTableForGridLayout.Columns.Add("HRPlan_" + Convert.ToString(item.IdStage), typeof(string));
        //                bandHRResult.Columns.Add(new ColumnItem() { ColumnFieldName = "HRResult_" + Convert.ToString(item.IdStage), HeaderText = "", Width = 100, Visible = true, IsVertical = false, PlantOperationPlanningSetting = PlantOperationPlanningColumnTemplateSelector.PlantOperationPlanningSettingType.HRPlan });//DataTableForGridLayout.Columns.Add(totalSummaryField, typeof(double)).AllowDBNull = true;
        //                DataTableForGridLayout.Columns.Add("HRResult_" + Convert.ToString(item.IdStage), typeof(decimal));

        //                if (!DataTableForGridLayout.Columns.Contains(("JobDescription_" + item.IdStage).ToString()))
        //                {
        //                    bandHRResult.Columns.Add(new ColumnItem() { ColumnFieldName = "JobDescription_" + Convert.ToString(item.IdStage), HeaderText = "JD", Width = 70, Visible = true, IsVertical = false, PlantOperationPlanningSetting = PlantOperationPlanningColumnTemplateSelector.PlantOperationPlanningSettingType.JobDescription });
        //                    DataTableForGridLayout.Columns.Add("JobDescription_" + Convert.ToString(item.IdStage), typeof(string));
        //                }
        //                ParentBandItem.Bands.Add(bandEMP);
        //                ParentBandItem.Bands.Add(bandHRExpected);
        //                ParentBandItem.Bands.Add(bandHRPlan);
        //                ParentBandItem.Bands.Add(bandHRResult);
        //                Bands_FirstLevel.Add(ParentBandItem);

        //            }

        //        }
        //        TotalSummary = new ObservableCollection<Summary>()
        //        {
        //            new Summary() { Type = SummaryItemType.Count, FieldName="CalenderWeek",  DisplayFormat = "Count: {0}" }

        //        };


        //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //    }
        //    catch (FaultException<ServiceException> ex)
        //    {
        //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Error in AddColumnsToDataTableWithoutBands() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
        //        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
        //    }
        //    catch (ServiceUnexceptedException ex)
        //    {
        //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Error in AddColumnsToDataTableWithoutBands() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Error in AddColumnsToDataTableWithoutBands() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}

        private void AddColumnsToDataTableWithoutBands()
        {
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTableWithoutBands ...", category: Category.Info, priority: Priority.Low);
                DtPlantOperation = new DataTable();//[GEOS2-4708][gulab lakade][25 07 2023]
                Bands = new ObservableCollection<BandItem>(); Bands.Clear();
                BandItem band0 = new BandItem() { BandName = "FirstRow", AllowBandMove = false, FixedStyle = FixedStyle.Left, Visible = true, AllowMoving = DevExpress.Utils.DefaultBoolean.False };
                band0.Columns = new ObservableCollection<ColumnItem>();
                band0.Columns.Add(new ColumnItem() { ColumnFieldName = "CalenderWeek", HeaderText = "", Width = 120, IsVertical = false, PlantOperationPlanningSetting = PlantOperationPlanningColumnTemplateSelector.PlantOperationPlanningSettingType.CalenderWeek, Visible = true });
                //[GEOS2-4839][gulab lakade][18 09 2023]
                band0.Columns.Add(new ColumnItem() { ColumnFieldName = "Employee_", HeaderText = "Employee", Width = 120, IsVertical = false, PlantOperationPlanningSetting = PlantOperationPlanningColumnTemplateSelector.PlantOperationPlanningSettingType.Employee, Visible = true });
                band0.Columns.Add(new ColumnItem() { ColumnFieldName = "Plant", HeaderText = "Plant", Width = 120, IsVertical = false, PlantOperationPlanningSetting = PlantOperationPlanningColumnTemplateSelector.PlantOperationPlanningSettingType.Plant, Visible = true });

                band0.Columns.Add(new ColumnItem() { ColumnFieldName = "TimeType_", HeaderText = "", Width = 50, Visible = true, IsVertical = false, PlantOperationPlanningSetting = PlantOperationPlanningColumnTemplateSelector.PlantOperationPlanningSettingType.TimeType });
                band0.Columns.Add(new ColumnItem() { ColumnFieldName = "IdEmployee_00", HeaderText = "IdEmployee", Width = 0, IsVertical = false, PlantOperationPlanningSetting = PlantOperationPlanningColumnTemplateSelector.PlantOperationPlanningSettingType.Hidden, Visible = false });
                // DataTableForGridLayout.Columns.Add("IdStage_" + Convert.ToString(item.IdStage), typeof(string));
                //if (!DataTableForGridLayout.Columns.Contains("IdEmployee_00"))
                //{

                //  DataTableForGridLayout.Columns.Add("IdEmployee_", typeof(ulong));
                //}
                // end [GEOS2-4839][gulab lakade][18 09 2023]
                Bands.Add(band0);
                DataTableForGridLayout = new DataTable();
                DataTableForGridLayout.Columns.Add("CalenderWeek", typeof(string));
                //[GEOS2-4839][gulab lakade][18 09 2023]
                DataTableForGridLayout.Columns.Add("Employee_", typeof(string));
                DataTableForGridLayout.Columns.Add("Plant", typeof(string));
                DataTableForGridLayout.Columns.Add("TimeType_", typeof(string));
                DataTableForGridLayout.Columns.Add("IdEmployee_00", typeof(string));
                //end [GEOS2-4839][gulab lakade][18 09 2023]
                GroupSummary = new ObservableCollection<Summary>();  ////[GEOS2-4549][gulablakade][08 06 2023]
                TotalSummary = new ObservableCollection<Summary>(); ////[GEOS2-4549][gulablakade][08 06 2023]
                List<string> tempCurrentstage = new List<string>();

                if (PlantOperationProductionStage.Count > 0)
                {
                    foreach (var item in PlantOperationProductionStage)
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
                            // Comment for [GEOS2-4839][gulab lakade][18 09 2023]
                            //DataTableForGridLayout.Columns.Add("Employee_" + Convert.ToString(item.IdStage), typeof(string));
                            //band1.Columns.Add(new ColumnItem() { ColumnFieldName = "Employee_" + Convert.ToString(item.IdStage), HeaderText = "Employee", Width = 200, Visible = true, IsVertical = false, PlantOperationPlanningSetting = PlantOperationPlanningColumnTemplateSelector.PlantOperationPlanningSettingType.Employee });
                            // END for [GEOS2-4839][gulab lakade][18 09 2023]
                            DataTableForGridLayout.Columns.Add("HRExpected_" + Convert.ToString(item.IdStage), typeof(string));

                            //[Rupali Sarode][GEOS2-4553][09-06-2023]
                            //DataTableForGridLayout.Columns.Add("TimeType_" + Convert.ToString(item.IdStage), typeof(string));
                            //band1.Columns.Add(new ColumnItem() { ColumnFieldName = "TimeType_" + Convert.ToString(item.IdStage), HeaderText = "", Width = 50, Visible = true, IsVertical = false, PlantOperationPlanningSetting = PlantOperationPlanningColumnTemplateSelector.PlantOperationPlanningSettingType.TimeType });
                            //DataTableForGridLayout.Columns.Add("TempButton_" + Convert.ToString(item.IdStage), typeof(bool));
                            //


                            band1.Columns.Add(new ColumnItem() { ColumnFieldName = "HRExpected_" + Convert.ToString(item.IdStage), HeaderText = "HR " + System.Environment.NewLine + "Expected", Width = 70, Visible = true, IsVertical = false, PlantOperationPlanningSetting = PlantOperationPlanningColumnTemplateSelector.PlantOperationPlanningSettingType.HRExpected });
                            DataTableForGridLayout.Columns.Add("HRPlan_" + Convert.ToString(item.IdStage), typeof(string));
                            band1.Columns.Add(new ColumnItem() { ColumnFieldName = "HRPlan_" + Convert.ToString(item.IdStage), HeaderText = "HR" + System.Environment.NewLine + "Plan", Width = 70, Visible = true, IsVertical = false, PlantOperationPlanningSetting = PlantOperationPlanningColumnTemplateSelector.PlantOperationPlanningSettingType.HRPlan });

                            //[Rupali Sarode][GEOS2-4553][08-06-2023]
                            DataTableForGridLayout.Columns.Add("Real_" + Convert.ToString(item.IdStage), typeof(string));
                            band1.Columns.Add(new ColumnItem() { ColumnFieldName = "Real_" + Convert.ToString(item.IdStage), HeaderText = "Real", Width = 70, Visible = true, IsVertical = false, PlantOperationPlanningSetting = PlantOperationPlanningColumnTemplateSelector.PlantOperationPlanningSettingType.Real });
                            //

                            DataTableForGridLayout.Columns.Add("JobDescription_" + Convert.ToString(item.IdStage), typeof(string));
                            band1.Columns.Add(new ColumnItem() { ColumnFieldName = "JobDescription_" + Convert.ToString(item.IdStage), HeaderText = "JD", Width = 70, Visible = true, IsVertical = false, PlantOperationPlanningSetting = PlantOperationPlanningColumnTemplateSelector.PlantOperationPlanningSettingType.JobDescription });
                            // DataTableForGridLayout.Columns.Add("TempButton", typeof(bool));
                            band1.Columns.Add(new ColumnItem() { ColumnFieldName = "IdStage_", HeaderText = "IdStage", Width = 0, IsVertical = false, PlantOperationPlanningSetting = PlantOperationPlanningColumnTemplateSelector.PlantOperationPlanningSettingType.Hidden, Visible = false });
                            // DataTableForGridLayout.Columns.Add("IdStage_" + Convert.ToString(item.IdStage), typeof(string));
                            if (!DataTableForGridLayout.Columns.Contains("IdStage_"))
                            {
                                DataTableForGridLayout.Columns.Add("IdStage_" + Convert.ToString(item.IdStage), typeof(string));
                                //DataTableForGridLayout.Columns.Add("IdStage_", typeof(ulong));
                            }

                            //band1.Columns.Add(new ColumnItem() { ColumnFieldName = "IdEmployee_", HeaderText = "IdEmployee", Width = 0, IsVertical = false, PlantOperationPlanningSetting = PlantOperationPlanningColumnTemplateSelector.PlantOperationPlanningSettingType.Hidden, Visible = false });
                            //// DataTableForGridLayout.Columns.Add("IdStage_" + Convert.ToString(item.IdStage), typeof(string));
                            //if (!DataTableForGridLayout.Columns.Contains("IdEmployee_"))
                            //{
                            //    DataTableForGridLayout.Columns.Add("IdEmployee_" + Convert.ToString(item.IdStage), typeof(string));
                            //    //  DataTableForGridLayout.Columns.Add("IdEmployee_", typeof(ulong));
                            //}
                            Bands.Add(band1);
                            #region [GEOS2-4549][gulablakade][08 06 2023]

                            //string Employee_Group = "Employee_" + Convert.ToString(item.IdStage);  //comment for [GEOS2-4839][gulab lakade][18 09 2023]
                            string Employee_Group = "Employee_"; //[GEOS2-4839][gulab lakade][18 09 2023]
                            string HRExpected_Group = "HRExpected_" + Convert.ToString(item.IdStage);
                            string HRPlan_Group = "HRPlan_" + Convert.ToString(item.IdStage);
                            string Real_Group = "Real_" + Convert.ToString(item.IdStage); //[GEOS2-4482][Rupali Sarode][16-062023]
                                                                                          //  var nonNullRows = DataTableForGridLayout.AsEnumerable().Where(cell => !cell.IsNull("Employee_" + Convert.ToString(item.IdStage)));

                            int nonNullCellCount = 0;
                            if (DtPlantOperation != null)
                            {
                                foreach (DataRow row in DtPlantOperation.Rows)
                                {
                                    //if (!row.IsNull("Employee_" + Convert.ToString(item.IdStage)))
                                    if (!row.IsNull("Employee_"))
                                    {
                                        nonNullCellCount++;
                                    }
                                }
                            }


                            //GroupSummary.Add(new Summary() { Type = SummaryItemType.Custom, FieldName = "Employee_", DisplayFormat = Convert.ToString(nonNullCellCount) });//[GEOS2-4839][gulab lakade][18 09 2023]
                            //start comment for [GEOS2-4839][gulab lakade][05 10 2023]
                            //GroupSummary.Add(new Summary() { Type = SummaryItemType.Sum, FieldName = HRExpected_Group, DisplayFormat = "{0}" });
                            //GroupSummary.Add(new Summary() { Type = SummaryItemType.Sum, FieldName = HRPlan_Group, DisplayFormat = "{0}" });
                            ////[GEOS2-4482][Rupali Sarode][16-062023]
                            //GroupSummary.Add(new Summary() { Type = SummaryItemType.Sum, FieldName = Real_Group, DisplayFormat = "{0}" });
                            //end comment for [GEOS2-4839][gulab lakade][05 10 2023]
                            TotalSummary.Add(new Summary() { Type = SummaryItemType.Sum, FieldName = HRExpected_Group, DisplayFormat = "{0}" });
                            TotalSummary.Add(new Summary() { Type = SummaryItemType.Sum, FieldName = HRPlan_Group, DisplayFormat = "{0}" });
                            //[GEOS2-4482][Rupali Sarode][16-062023]
                            TotalSummary.Add(new Summary() { Type = SummaryItemType.Sum, FieldName = Real_Group, DisplayFormat = "{0}" });
                            //TotalSummary.Add(new Summary() { Type = SummaryItemType.Custom, FieldName = Employee_Group, DisplayFormat = "{0}" });//[GEOS2-4839][gulab lakade][18 09 2023]
                            // 
                            #endregion

                        }
                    }
                }
                GroupSummary.Add(new Summary() { Type = SummaryItemType.Count, FieldName = "Employee_", DisplayFormat = "{0}" });//[GEOS2-4839][gulab lakade][18 09 2023]
                TotalSummary.Add(new Summary() { Type = SummaryItemType.Count, FieldName = "Employee_", DisplayFormat = "{0}" });//[GEOS2-4839][gulab lakade][18 09 2023]
                GroupSummary.Add(new Summary() { Type = SummaryItemType.Custom, FieldName = "Plant", DisplayFormat = "{0}" });//[GEOS2-4839][gulab lakade][18 09 2023]
                                                                                                                              // TotalSummary.Add(new Summary() { Type = SummaryItemType.Custom, FieldName = "Plant", DisplayFormat = "{0}" });//[GEOS2-4839][gulab lakade][18 09 2023]
                                                                                                                              //  DataTable = DataTableForGridLayout;
                Bands = new ObservableCollection<BandItem>(Bands);
             //   TotalSummary.Add(new Summary() { Type = SummaryItemType.Count, FieldName = "CalenderWeek", DisplayFormat = "Count: {0}" });
                //TotalSummary = new ObservableCollection<Summary>()
                //{
                //    new Summary() { Type = SummaryItemType.Count, FieldName="CalenderWeek",  DisplayFormat = "Count: {0}" }
                //};
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

        //private void FillDashboard()
        //{
        //    try
        //    {
        //        if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
        //        GeosApplication.Instance.Logger.Log("Method FillDashboard ...", category: Category.Info, priority: Priority.Low);
        //        int rowCounter = 0;
        //        //double? totalsum = null;
        //        //double totalsumConvertedAmount = 0;
        //        //int bandvalue = 3;
        //        List<string> tempCurrentstage = new List<string>();
        //        var currentculter = CultureInfo.CurrentCulture;
        //        string DecimalSeperator = currentculter.NumberFormat.NumberDecimalSeparator.ToString();
        //        DataTableForGridLayout.Clear();
        //       // DtPlantOperation = null;
        //        DtPlantOperation = new DataTable();


        //        //#region temprecord For testing
        //        //EmployeeplantOperational = new ERMEmployeePlantOperation();
        //        //EmployeeplantOperational.CalenderWeek = Convert.ToString("2022CW09");
        //        //EmployeeplantOperational.IdEmployee = Convert.ToInt32(2663);
        //        //EmployeeplantOperational.IdCompany = Convert.ToInt32(18);
        //        //EmployeeplantOperational.IdCompany = Convert.ToInt32(18);
        //        //#region JobDescription
        //        //EmployeeplantOperational.IdJobDescription = Convert.ToInt32(83);
        //        //EmployeeplantOperational.JobDescriptionUsage = Convert.ToDecimal(25);
        //        //EmployeeplantOperational.JobDescriptionStartDate = Convert.ToDateTime("20/05/2022 0:00:00");

        //        //#endregion
        //        //EmployeeplantOperational.EmployeeName = Convert.ToString("Abdelkader Aghoulaiche");
        //        //EmployeeplantOperational.HRExpected = Convert.ToDecimal(32);
        //        //EmployeeplantOperational.HRPlan = Convert.ToDecimal(32);

        //        //EmployeeplantOperationallist.Add(EmployeeplantOperational);
        //        //EmployeeplantOperational = new ERMEmployeePlantOperation();

        //        //EmployeeplantOperational.CalenderWeek = Convert.ToString("2022CW20");
        //        //EmployeeplantOperational.IdEmployee = Convert.ToInt32(2673);
        //        //EmployeeplantOperational.IdCompany = Convert.ToInt32(18);
        //        //EmployeeplantOperational.IdCompany = Convert.ToInt32(18);
        //        //#region JobDescription
        //        //EmployeeplantOperational.IdJobDescription = Convert.ToInt32(30);
        //        //EmployeeplantOperational.JobDescriptionUsage = Convert.ToDecimal(25);
        //        //EmployeeplantOperational.JobDescriptionStartDate = Convert.ToDateTime("17/05/2022 0:00:00");

        //        //#endregion
        //        //EmployeeplantOperational.EmployeeName = Convert.ToString("Vlad Mihai Kun Kun");
        //        //EmployeeplantOperational.HRExpected = Convert.ToDecimal(32);
        //        //EmployeeplantOperational.HRPlan = Convert.ToDecimal(32);

        //        //EmployeeplantOperationallist.Add(EmployeeplantOperational);
        //        //#endregion





        //        EmployeeplantOperationallist = EmployeeplantOperationallist.OrderBy(a => a.CalenderWeek).ToList();
        //        var TempCalenderWeek1 = EmployeeplantOperationallist.GroupBy(a => a.CalenderWeek).ToList();
        //        var TempCalenderWeek = TempCalenderWeek1.OrderBy(a => a.Key).ToList();
        //        foreach (var calendar in TempCalenderWeek)
        //        {
        //            //EmployeeplantOperationallist = EmployeeplantOperationallist.OrderByDescending(a => a.CalenderWeek).ToList();
        //            //var GroupByEmployee = EmployeeplantOperationallist.Where(a => a.CalenderWeek == Convert.ToString(calendar.Key).GroupBy(p=>p.IdEmployee).toli
        //            List<ERMEmployeePlantOperation> TempEmployeeplantOperationallist = new List<ERMEmployeePlantOperation>();
        //            TempEmployeeplantOperationallist = EmployeeplantOperationallist.Where(a => a.CalenderWeek == Convert.ToString(calendar.Key)).ToList();

        //            int count = 0;// = TempEmployeeplantOperationallist.Count();

        //            foreach (var Employeedata in TempEmployeeplantOperationallist.GroupBy(a => a.IdEmployee))
        //            {
        //                count++;

        //                DataRow dr = DataTableForGridLayout.NewRow();
        //                dr["CalenderWeek"] = Convert.ToString(calendar.Key);
        //                List<ERMEmployeePlantOperation> TempEmployeelist = new List<ERMEmployeePlantOperation>();
        //                TempEmployeelist = TempEmployeeplantOperationallist.Where(a => a.IdEmployee == Employeedata.Key && a.CalenderWeek == calendar.Key).ToList();
        //                //List<string> tempIDJobDescription = new List<string>();
        //                //foreach (ERMEmployeePlantOperation item in TempEmployeelist)
        //                //{

        //                //        tempIDJobDescription.Add(Convert.ToString(item.IdJobDescription));


        //                //}
        //                if (TempEmployeelist.Count > 0)
        //                {
        //                    DataTable tempdata = new DataTable();
        //                    tempdata.Columns.Add("IdStage");
        //                    tempdata.Columns.Add("IdJobDescription");
        //                    tempdata.Columns.Add("DescriptionUse");
        //                    List<TempIdStage> IdStageList = new List<TempIdStage>();


        //                    foreach (ERMEmployeePlantOperation item in TempEmployeelist)
        //                    {
        //                        //[GEOS2-4839][gulab lakade][18 09 2023]
        //                        dr["Plant"] = Convert.ToString(item.PlantName);
        //                        string Employee = "Employee_";
        //                        dr[Employee] = Convert.ToString(item.EmployeeName);
        //                        string TimeType = "TimeType_";
        //                        dr[TimeType] = "TimeType";
        //                        string IdEmployee = "IdEmployee_00";
        //                        dr[IdEmployee] = Convert.ToString(item.IdEmployee);
        //                        //end [GEOS2-4839][gulab lakade][18 09 2023]
        //                        var TempIdStageList = WorkStageWiseJobDescription.Where(x => x.IdJobDescription.Contains(Convert.ToString(item.IdJobDescription))).ToList();
        //                        if (TempIdStageList.Count > 0)
        //                        {
        //                            foreach (var itemIdStage in TempIdStageList)
        //                            {
        //                                var IsExist_IdStage = PlantOperationProductionStage.Where(x => x.IdStage == itemIdStage.IdWorkStage).ToList();
        //                                if (IsExist_IdStage.Count > 0)
        //                                {
        //                                    TempIdStage tempIdStage = new TempIdStage();
        //                                    tempIdStage.IdStage = Convert.ToInt32(itemIdStage.IdWorkStage);
        //                                    tempIdStage.IdJobDescription = Convert.ToInt32(item.IdJobDescription);
        //                                    tempIdStage.JobDescriptionUse = Convert.ToDecimal(item.JobDescriptionUsage);
        //                                    IdStageList.Add(tempIdStage);
        //                                    // comment for [GEOS2-4839][gulab lakade][18 09 2023]
        //                                    //string Employee = "Employee_" + Convert.ToString(itemIdStage.IdWorkStage);
        //                                    //dr[Employee] = Convert.ToString(item.EmployeeName);
        //                                    // end [GEOS2-4839][gulab lakade][18 09 2023]
        //                                    //string IdEmployee = "IdEmployee_" + Convert.ToString(itemIdStage.IdWorkStage);
        //                                    //dr[IdEmployee] = Convert.ToString(item.IdEmployee);

        //                                    //string TimeType = "TimeType_" + Convert.ToString(itemIdStage.IdWorkStage);

        //                                    //string HRExpected = "HRExpected_" + Convert.ToString(itemIdStage.IdWorkStage);
        //                                    //dr[HRExpected] = Convert.ToString(item.HRExpected.ToString("0"));
        //                                    //string HRPlan = "HRPlan_" + Convert.ToString(itemIdStage.IdWorkStage);
        //                                    //dr[HRPlan] = Convert.ToString(item.HRPlan.ToString("0"));
        //                                    //string JobDescription = "JobDescription_" + Convert.ToString(itemIdStage.IdWorkStage);
        //                                    //dr[JobDescription] = Convert.ToString(item.JobDescriptionUsage) + "%";
        //                                }
        //                            }

        //                        }

        //                    }
        //                    if (IdStageList.Count > 0)
        //                    {

        //                        var tempIdStage = IdStageList.GroupBy(a => a.IdStage).ToList();
        //                        if (tempIdStage.Count > 0)
        //                        {
        //                            foreach (var item in tempIdStage)
        //                            {
        //                                List<TempIdStage> TempidStageList = new List<TempIdStage>();
        //                                TempidStageList = IdStageList.Where(a => a.IdStage == item.Key).ToList();
        //                                if (TempidStageList.Count > 0)
        //                                {
        //                                    int TempHRExpectedValue = 0;
        //                                    int TempHRPlanValue = 0;
        //                                    string TempTimeType = string.Empty;
        //                                    float TempRealTime = 0;
        //                                    //  float NotRegisteredTime = 0;

        //                                    decimal TempJobDescriptionUsage = 0;
        //                                    foreach (var item1 in TempidStageList)
        //                                    {
        //                                        var tempvalue = TempEmployeeplantOperationallist.Where(a => a.IdEmployee == Employeedata.Key && a.CalenderWeek == calendar.Key && a.IdJobDescription == item1.IdJobDescription).FirstOrDefault();

        //                                        if (TempJobDescriptionUsage == 0)
        //                                        {
        //                                            TempJobDescriptionUsage = Convert.ToDecimal(item1.JobDescriptionUse);
        //                                            if (tempvalue != null)
        //                                            {
        //                                                TempHRExpectedValue = Convert.ToInt32(tempvalue.HRExpected);
        //                                                TempHRPlanValue = Convert.ToInt32(tempvalue.HRPlan);
        //                                                if (tempvalue.EmployeePlantOperationalRealTimeList.Count() > 0)
        //                                                {
        //                                                    var tempReal = tempvalue.EmployeePlantOperationalRealTimeList.Where(x => x.CalenderWeek == calendar.Key && x.IdEmployee == Employeedata.Key && x.Idstage == item1.IdStage).FirstOrDefault();
        //                                                    if (tempReal != null)
        //                                                    {
        //                                                        TempRealTime = (float)(tempReal.TimeDifferenceInMinutes);
        //                                                    }
        //                                                }


        //                                            }

        //                                        }
        //                                        else
        //                                        {
        //                                            TempJobDescriptionUsage = TempJobDescriptionUsage + Convert.ToDecimal(item1.JobDescriptionUse);
        //                                            if (tempvalue != null)
        //                                            {
        //                                                TempHRExpectedValue = TempHRExpectedValue + Convert.ToInt32(tempvalue.HRExpected);
        //                                                TempHRPlanValue = TempHRPlanValue + Convert.ToInt32(tempvalue.HRPlan);
        //                                                if (tempvalue.EmployeePlantOperationalRealTimeList.Count() > 0)
        //                                                {
        //                                                    var tempReal = tempvalue.EmployeePlantOperationalRealTimeList.Where(x => x.CalenderWeek == calendar.Key && x.IdEmployee == Employeedata.Key && x.Idstage == item1.IdStage).FirstOrDefault();
        //                                                    if (tempReal != null)
        //                                                    {
        //                                                        TempRealTime = TempRealTime + (float)(tempReal.TimeDifferenceInMinutes);
        //                                                    }
        //                                                }
        //                                            }
        //                                        }


        //                                    }
        //                                    string IdStage = "IdStage_" + Convert.ToString(item.Key);
        //                                    dr[IdStage] = Convert.ToString(item.Key);
        //                                    string HRExpected = "HRExpected_" + Convert.ToString(item.Key);
        //                                    dr[HRExpected] = Convert.ToString(TempHRExpectedValue.ToString("0"));
        //                                    string HRPlan = "HRPlan_" + Convert.ToString(item.Key);
        //                                    dr[HRPlan] = Convert.ToString(TempHRPlanValue.ToString("0"));
        //                                    #region Real
        //                                    string RealTime = "Real_" + Convert.ToString(item.Key);
        //                                    if (TempRealTime == 0)
        //                                    {
        //                                        dr[RealTime] = "";
        //                                    }
        //                                    else
        //                                    {
        //                                        dr[RealTime] = Math.Round(Convert.ToDouble(TempRealTime), 0);
        //                                    }
        //                                    #endregion


        //                                    string JobDescription = "JobDescription_" + Convert.ToString(item.Key);
        //                                    dr[JobDescription] = Convert.ToString(TempJobDescriptionUsage) + "%";
        //                                    //string TimeType = "TimeType_" + Convert.ToString(item.Key);
        //                                    //dr[TimeType] = "TimeType";
        //                                    //string tempcolor = "TempButton_" + Convert.ToString(item.Key);

        //                                    //DataTableForGridLayout.Rows.Add(dr[TimeType], tempTimeType);
        //                                }

        //                            }
        //                        }
        //                    }



        //                    DataTableForGridLayout.Rows.Add(dr);
        //                    //if (DataTableForGridLayout.Rows[DataTableForGridLayout.Rows.Count-1][1] != null || DataTableForGridLayout.Rows[DataTableForGridLayout.Rows.Count - 1][1].ToString() != "")
        //                    //{
        //                    //    IsButtonVisible = Visibility.Visible;
        //                    //}
        //                    //else
        //                    //{
        //                    //    IsButtonVisible = Visibility.Collapsed;
        //                    //}
        //                    rowCounter += 1;

        //                    //#region RND
        //                    //if (IdStageList.Count > 0)
        //                    //{
        //                    //    var tempIdStage = IdStageList.GroupBy(a => a.IdStage).ToList();
        //                    //    if (tempIdStage.Count > 0)
        //                    //    {
        //                    //        if (NonOTTimeTypesList.Count() > 0)
        //                    //        {
        //                    //            foreach (var itemtimetype in NonOTTimeTypesList)
        //                    //            {
        //                    //                DataRow dr1 = DataTableForGridLayout.NewRow();
        //                    //                foreach (var item in tempIdStage)
        //                    //                {
        //                    //                   // var TempReal


        //                    //                    dr1["CalenderWeek"] = Convert.ToString(calendar.Key);
        //                    //                    //string Employee = "Employee_" + Convert.ToString(itemIdStage.IdWorkStage);
        //                    //                    //dr[Employee] = Convert.ToString(item.EmployeeName);
        //                    //                    string TimeType = "TimeType_" + Convert.ToString(item.Key);
        //                    //                    dr1[TimeType] = Convert.ToString(itemtimetype.ReasonValue);
        //                    //                    //if (dr1[TimeType].ToString() == Convert.ToString("TimeType"))
        //                    //                    //{
        //                    //                    //    dr1["TempButton"] = true;
        //                    //                    //}
        //                    //                    //else
        //                    //                    //{
        //                    //                    //    dr1["TempButton"] = false;
        //                    //                    //}
        //                    //                    ////var TempTimeType= TempEmployeeplantOperationallist.
        //                    //                }
        //                    //                DataTableForGridLayout.Rows.Add(dr1);
        //                    //                rowCounter += 1;
        //                    //            }
        //                    //        }
        //                    //    }
        //                    //}
        //                    //#endregion
        //                }
        //            }
        //        }
        //        DtPlantOperation = new DataTable();
        //        DtPlantOperation = DataTableForGridLayout;

        //        //string desiredPropertyValue = "";
        //        //foreach (DataRow row in dtPlantOperation.Rows)
        //        //{
        //        //    // Assuming "PropertyName" is the column name for the desired property
        //        //    desiredPropertyValue = row["TimeType_"].ToString();

        //        //    // Check if the desired value is found, and perform any required actions
        //        //    if (desiredPropertyValue == "TimeType")
        //        //    {
        //        //        // Perform the required actions when the desired value is found
        //        //        break; // Optional: Exit the loop if the desired value is found in the first occurrence
        //        //    }
        //        //}
        //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Method FillDashboard()....executed successfully", category: Category.Info, priority: Priority.Low);

        //    }
        //    catch (Exception ex)
        //    {
        //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Get an error in FillDashboard() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}

        private void FillDashboard()
        {
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                GeosApplication.Instance.Logger.Log("Method FillDashboard ...", category: Category.Info, priority: Priority.Low);
                int rowCounter = 0;
                //double? totalsum = null;
                //double totalsumConvertedAmount = 0;
                //int bandvalue = 3;
                List<string> tempCurrentstage = new List<string>();
                var currentculter = CultureInfo.CurrentCulture;
                string DecimalSeperator = currentculter.NumberFormat.NumberDecimalSeparator.ToString();
                DataTableForGridLayout.Clear();
                // DtPlantOperation = null;
                DtPlantOperation = new DataTable();








              //  EmployeeplantOperationallist = EmployeeplantOperationallist.OrderBy(a => a.CalenderWeek).ToList();
                EmployeeplantOperationallist = EmployeeplantOperationallist.Where(b=>b.EmployeeName!=null).OrderBy(a => a.CalenderWeek).ToList();
                var TempCalenderWeek1 = EmployeeplantOperationallist.GroupBy(a => a.CalenderWeek).ToList();
                var TempCalenderWeek = TempCalenderWeek1.OrderBy(a => a.Key).ToList();
                foreach (var calendar in TempCalenderWeek)
                {

                    List<ERMEmployeePlantOperation> TempEmployeeplantOperationallist = new List<ERMEmployeePlantOperation>();
                    TempEmployeeplantOperationallist = EmployeeplantOperationallist.Where(a => a.CalenderWeek == Convert.ToString(calendar.Key)).ToList();

                    int count = 0;// = TempEmployeeplantOperationallist.Count();
                    var TempRecord = (from emp in TempEmployeeplantOperationallist
                                      from jd in WorkStageWiseJobDescription
                                      from pps in PlantOperationProductionStage
                                      where jd.IdJobDescription.Contains(Convert.ToString(emp.IdJobDescription)) 
                                      && pps.IdStage==jd.IdWorkStage
                                      select new
                                      {
                                          emp.PlantName,
                                          emp.IdEmployee,
                                          emp.EmployeeName,
                                          emp.JobDescriptionUsage,
                                          emp.HRExpected,
                                          emp.HRPlan,
                                          emp.IdJobDescription,
                                          emp.IdCompany,
                                          emp.RealTime,
                                          emp.ReasonValue,
                                          emp.TimeDifferenceInMinutes,
                                          emp.EmployeePlantOperationalRealTimeList,
                                          jd.IdWorkStage,
                                          pps.Sequence

                                      }
                                    ).OrderBy(a => a.Sequence).ToList();
                    List<TempEmployeeByStage> employeeByStage = new List<TempEmployeeByStage>();
                    if (TempRecord.Count() > 0)
                    {
                        foreach (var item in TempRecord)
                        {
                            int CurrentEmployeePresent = 0;
                            if (employeeByStage.Count() > 0)
                            {
                                CurrentEmployeePresent = employeeByStage.Where(a => a.IdEmployee == item.IdEmployee && a.IdStage == item.IdWorkStage).Count();
                            }
                            if (CurrentEmployeePresent == 0)
                            {
                                
                                DataRow dr = DataTableForGridLayout.NewRow();
                                dr["CalenderWeek"] = Convert.ToString(calendar.Key);

                                var tempEmployee = TempRecord.Where(a => a.IdEmployee == item.IdEmployee).ToList();
                                if (tempEmployee.Count() > 0)
                                {
                                    var EmployeeByStageGroup = tempEmployee.GroupBy(a => a.IdWorkStage).ToList();
                                    if(EmployeeByStageGroup.Count()>0)
                                    {
                                        foreach(var stageitem in EmployeeByStageGroup)
                                        {
                                            var TempEmployeeByStagelist = tempEmployee.Where(x => x.IdWorkStage == stageitem.Key).ToList();

                                             var TempEmployeeByStage = tempEmployee.Where(x => x.IdWorkStage == stageitem.Key).FirstOrDefault();
                                            if(TempEmployeeByStage!=null)
                                            {
                                                TempEmployeeByStage SelectedemployeeByStage = new TempEmployeeByStage();
                                                SelectedemployeeByStage.IdEmployee = Convert.ToInt32(TempEmployeeByStage.IdEmployee);
                                                SelectedemployeeByStage.IdStage = Convert.ToInt32(TempEmployeeByStage.IdWorkStage);
                                                employeeByStage.Add(SelectedemployeeByStage);

                                                dr["Plant"] = Convert.ToString(TempEmployeeByStage.PlantName);
                                                string Employee = "Employee_";
                                                dr[Employee] = Convert.ToString(TempEmployeeByStage.EmployeeName);
                                                string TimeType = "TimeType_";
                                                dr[TimeType] = "TimeType";
                                                string IdEmployee = "IdEmployee_00";
                                                dr[IdEmployee] = Convert.ToString(TempEmployeeByStage.IdEmployee);


                                                string IdStage = "IdStage_" + Convert.ToString(TempEmployeeByStage.IdWorkStage);
                                                dr[IdStage] = Convert.ToString(TempEmployeeByStage.IdWorkStage);
                                                string HRExpected = "HRExpected_" + Convert.ToString(TempEmployeeByStage.IdWorkStage);
                                                string HRPlan = "HRPlan_" + Convert.ToString(TempEmployeeByStage.IdWorkStage);
                                                string RealTime = "Real_" + Convert.ToString(TempEmployeeByStage.IdWorkStage);
                                                string JobDescription = "JobDescription_" + Convert.ToString(TempEmployeeByStage.IdWorkStage);
                                                float TempHRExpected = 0;
                                                float TempHRPlan = 0;
                                                float TempRealTime = 0;
                                                int TempJobdescrition = 0;
                                                foreach (var emp in TempEmployeeByStagelist)
                                                {
                                                    TempHRExpected = TempHRExpected + (float)emp.HRExpected;
                                                    TempHRPlan = TempHRPlan + (float)emp.HRPlan;

                                                    var tempReal = emp.EmployeePlantOperationalRealTimeList.Where(x => x.CalenderWeek == calendar.Key && x.IdEmployee == emp.IdEmployee && x.Idstage == emp.IdWorkStage).FirstOrDefault();
                                                    if (tempReal != null)
                                                    {
                                                        TempRealTime = TempRealTime + (float)(tempReal.TimeDifferenceInMinutes);
                                                    }
                                                    //TempRealTime = TempRealTime + (float)emp.HRExpected;

                                                    TempJobdescrition = TempJobdescrition + Convert.ToInt32(emp.JobDescriptionUsage);
                                                }

                                                
                                                dr[HRExpected] = Convert.ToString(TempHRExpected.ToString("0"));
                                               
                                                dr[HRPlan] = Convert.ToString(TempHRPlan.ToString("0"));

                                                
                                                
                                                //var tempReal = emp.EmployeePlantOperationalRealTimeList.Where(x => x.CalenderWeek == calendar.Key && x.IdEmployee == emp.IdEmployee && x.Idstage == emp.IdWorkStage).FirstOrDefault();
                                                //if (tempReal != null)
                                                //{
                                                //    TempRealTime = TempRealTime + (float)(tempReal.TimeDifferenceInMinutes);
                                                //}
                                                dr[RealTime] = Math.Round(Convert.ToDouble(TempRealTime), 0);


                                               
                                                dr[JobDescription] = Convert.ToString(TempJobdescrition) + "%";

                                            }

                                        }
                                    }
                                    //foreach (var emp in tempEmployee)
                                    //{
                                    //    TempEmployeeByStage SelectedemployeeByStage = new TempEmployeeByStage();
                                    //    SelectedemployeeByStage.IdEmployee = Convert.ToInt32(emp.IdEmployee);
                                    //    SelectedemployeeByStage.IdStage = Convert.ToInt32(emp.IdWorkStage);
                                    //    employeeByStage.Add(SelectedemployeeByStage);

                                    //    dr["Plant"] = Convert.ToString(emp.PlantName);
                                    //    string Employee = "Employee_";
                                    //    dr[Employee] = Convert.ToString(emp.EmployeeName);
                                    //    string TimeType = "TimeType_";
                                    //    dr[TimeType] = "TimeType";
                                    //    string IdEmployee = "IdEmployee_00";
                                    //    dr[IdEmployee] = Convert.ToString(emp.IdEmployee);


                                    //    string IdStage = "IdStage_" + Convert.ToString(emp.IdWorkStage);
                                    //    dr[IdStage] = Convert.ToString(emp.IdWorkStage);
                                    //    string HRExpected = "HRExpected_" + Convert.ToString(emp.IdWorkStage);
                                    //    dr[HRExpected] = Convert.ToString(emp.HRExpected.ToString("0"));
                                    //    string HRPlan = "HRPlan_" + Convert.ToString(emp.IdWorkStage);
                                    //    dr[HRPlan] = Convert.ToString(emp.HRPlan.ToString("0"));

                                    //    string RealTime = "Real_" + Convert.ToString(emp.IdWorkStage);
                                    //    float TempRealTime = 0;
                                    //    var tempReal = emp.EmployeePlantOperationalRealTimeList.Where(x => x.CalenderWeek == calendar.Key && x.IdEmployee == emp.IdEmployee && x.Idstage == emp.IdWorkStage).FirstOrDefault();
                                    //    if (tempReal != null)
                                    //    {
                                    //        TempRealTime = TempRealTime + (float)(tempReal.TimeDifferenceInMinutes);
                                    //    }
                                    //    dr[RealTime] = Math.Round(Convert.ToDouble(TempRealTime), 0);


                                    //    string JobDescription = "JobDescription_" + Convert.ToString(emp.IdWorkStage);
                                    //    dr[JobDescription] = Convert.ToString(emp.JobDescriptionUsage) + "%";
                                    //}
                                }
                                DataTableForGridLayout.Rows.Add(dr);
                                rowCounter += 1;
                            }
                            else
                            {

                            }


                        }
                    }


                    //foreach (var Employeedata in TempEmployeeplantOperationallist.GroupBy(a => a.IdEmployee))
                    //{
                    //    count++;

                    //    DataRow dr = DataTableForGridLayout.NewRow();
                    //    dr["CalenderWeek"] = Convert.ToString(calendar.Key);
                    //    List<ERMEmployeePlantOperation> TempEmployeelist = new List<ERMEmployeePlantOperation>();
                    //    TempEmployeelist = TempEmployeeplantOperationallist.Where(a => a.IdEmployee == Employeedata.Key && a.CalenderWeek == calendar.Key).ToList();

                    //    if (TempEmployeelist.Count > 0)
                    //    {
                    //        DataTable tempdata = new DataTable();
                    //        tempdata.Columns.Add("IdStage");
                    //        tempdata.Columns.Add("IdJobDescription");
                    //        tempdata.Columns.Add("DescriptionUse");
                    //        List<TempIdStage> IdStageList = new List<TempIdStage>();







                    //        foreach (ERMEmployeePlantOperation item in TempEmployeelist)
                    //        {
                    //            //[GEOS2-4839][gulab lakade][18 09 2023]
                    //            dr["Plant"] = Convert.ToString(item.PlantName);
                    //            string Employee = "Employee_";
                    //            dr[Employee] = Convert.ToString(item.EmployeeName);
                    //            string TimeType = "TimeType_";
                    //            dr[TimeType] = "TimeType";
                    //            string IdEmployee = "IdEmployee_00";
                    //            dr[IdEmployee] = Convert.ToString(item.IdEmployee);
                    //            //end [GEOS2-4839][gulab lakade][18 09 2023]
                    //            var TempIdStageList = WorkStageWiseJobDescription.Where(x => x.IdJobDescription.Contains(Convert.ToString(item.IdJobDescription))).ToList();
                    //            if (TempIdStageList.Count > 0)
                    //            {
                    //                foreach (var itemIdStage in TempIdStageList)
                    //                {
                    //                    var IsExist_IdStage = PlantOperationProductionStage.Where(x => x.IdStage == itemIdStage.IdWorkStage).ToList();
                    //                    if (IsExist_IdStage.Count > 0)
                    //                    {
                    //                        TempIdStage tempIdStage = new TempIdStage();
                    //                        tempIdStage.IdStage = Convert.ToInt32(itemIdStage.IdWorkStage);
                    //                        tempIdStage.IdJobDescription = Convert.ToInt32(item.IdJobDescription);
                    //                        tempIdStage.JobDescriptionUse = Convert.ToDecimal(item.JobDescriptionUsage);
                    //                        IdStageList.Add(tempIdStage);

                    //                    }
                    //                }

                    //            }

                    //        }
                    //        if (IdStageList.Count > 0)
                    //        {

                    //            var tempIdStage = IdStageList.GroupBy(a => a.IdStage).ToList();
                    //            if (tempIdStage.Count > 0)
                    //            {
                    //                foreach (var item in tempIdStage)
                    //                {
                    //                    List<TempIdStage> TempidStageList = new List<TempIdStage>();
                    //                    TempidStageList = IdStageList.Where(a => a.IdStage == item.Key).ToList();
                    //                    if (TempidStageList.Count > 0)
                    //                    {
                    //                        int TempHRExpectedValue = 0;
                    //                        int TempHRPlanValue = 0;
                    //                        string TempTimeType = string.Empty;
                    //                        float TempRealTime = 0;
                    //                        //  float NotRegisteredTime = 0;

                    //                        decimal TempJobDescriptionUsage = 0;
                    //                        foreach (var item1 in TempidStageList)
                    //                        {
                    //                            var tempvalue = TempEmployeeplantOperationallist.Where(a => a.IdEmployee == Employeedata.Key && a.CalenderWeek == calendar.Key && a.IdJobDescription == item1.IdJobDescription).FirstOrDefault();

                    //                            if (TempJobDescriptionUsage == 0)
                    //                            {
                    //                                TempJobDescriptionUsage = Convert.ToDecimal(item1.JobDescriptionUse);
                    //                                if (tempvalue != null)
                    //                                {
                    //                                    TempHRExpectedValue = Convert.ToInt32(tempvalue.HRExpected);
                    //                                    TempHRPlanValue = Convert.ToInt32(tempvalue.HRPlan);
                    //                                    if (tempvalue.EmployeePlantOperationalRealTimeList.Count() > 0)
                    //                                    {
                    //                                        var tempReal = tempvalue.EmployeePlantOperationalRealTimeList.Where(x => x.CalenderWeek == calendar.Key && x.IdEmployee == Employeedata.Key && x.Idstage == item1.IdStage).FirstOrDefault();
                    //                                        if (tempReal != null)
                    //                                        {
                    //                                            TempRealTime = (float)(tempReal.TimeDifferenceInMinutes);
                    //                                        }
                    //                                    }


                    //                                }

                    //                            }
                    //                            else
                    //                            {
                    //                                TempJobDescriptionUsage = TempJobDescriptionUsage + Convert.ToDecimal(item1.JobDescriptionUse);
                    //                                if (tempvalue != null)
                    //                                {
                    //                                    TempHRExpectedValue = TempHRExpectedValue + Convert.ToInt32(tempvalue.HRExpected);
                    //                                    TempHRPlanValue = TempHRPlanValue + Convert.ToInt32(tempvalue.HRPlan);
                    //                                    if (tempvalue.EmployeePlantOperationalRealTimeList.Count() > 0)
                    //                                    {
                    //                                        var tempReal = tempvalue.EmployeePlantOperationalRealTimeList.Where(x => x.CalenderWeek == calendar.Key && x.IdEmployee == Employeedata.Key && x.Idstage == item1.IdStage).FirstOrDefault();
                    //                                        if (tempReal != null)
                    //                                        {
                    //                                            TempRealTime = TempRealTime + (float)(tempReal.TimeDifferenceInMinutes);
                    //                                        }
                    //                                    }
                    //                                }
                    //                            }


                    //                        }
                    //                        string IdStage = "IdStage_" + Convert.ToString(item.Key);
                    //                        dr[IdStage] = Convert.ToString(item.Key);
                    //                        string HRExpected = "HRExpected_" + Convert.ToString(item.Key);
                    //                        dr[HRExpected] = Convert.ToString(TempHRExpectedValue.ToString("0"));
                    //                        string HRPlan = "HRPlan_" + Convert.ToString(item.Key);
                    //                        dr[HRPlan] = Convert.ToString(TempHRPlanValue.ToString("0"));
                    //                        #region Real
                    //                        string RealTime = "Real_" + Convert.ToString(item.Key);
                    //                        if (TempRealTime == 0)
                    //                        {
                    //                            dr[RealTime] = "";
                    //                        }
                    //                        else
                    //                        {
                    //                            dr[RealTime] = Math.Round(Convert.ToDouble(TempRealTime), 0);
                    //                        }
                    //                        #endregion


                    //                        string JobDescription = "JobDescription_" + Convert.ToString(item.Key);
                    //                        dr[JobDescription] = Convert.ToString(TempJobDescriptionUsage) + "%";
                    //                        //string TimeType = "TimeType_" + Convert.ToString(item.Key);
                    //                        //dr[TimeType] = "TimeType";
                    //                        //string tempcolor = "TempButton_" + Convert.ToString(item.Key);

                    //                        //DataTableForGridLayout.Rows.Add(dr[TimeType], tempTimeType);
                    //                    }

                    //                }
                    //            }
                    //        }



                    //        DataTableForGridLayout.Rows.Add(dr);
                    //        //if (DataTableForGridLayout.Rows[DataTableForGridLayout.Rows.Count-1][1] != null || DataTableForGridLayout.Rows[DataTableForGridLayout.Rows.Count - 1][1].ToString() != "")
                    //        //{
                    //        //    IsButtonVisible = Visibility.Visible;
                    //        //}
                    //        //else
                    //        //{
                    //        //    IsButtonVisible = Visibility.Collapsed;
                    //        //}
                    //        rowCounter += 1;

                    //        //#region RND
                    //        //if (IdStageList.Count > 0)
                    //        //{
                    //        //    var tempIdStage = IdStageList.GroupBy(a => a.IdStage).ToList();
                    //        //    if (tempIdStage.Count > 0)
                    //        //    {
                    //        //        if (NonOTTimeTypesList.Count() > 0)
                    //        //        {
                    //        //            foreach (var itemtimetype in NonOTTimeTypesList)
                    //        //            {
                    //        //                DataRow dr1 = DataTableForGridLayout.NewRow();
                    //        //                foreach (var item in tempIdStage)
                    //        //                {
                    //        //                   // var TempReal


                    //        //                    dr1["CalenderWeek"] = Convert.ToString(calendar.Key);
                    //        //                    //string Employee = "Employee_" + Convert.ToString(itemIdStage.IdWorkStage);
                    //        //                    //dr[Employee] = Convert.ToString(item.EmployeeName);
                    //        //                    string TimeType = "TimeType_" + Convert.ToString(item.Key);
                    //        //                    dr1[TimeType] = Convert.ToString(itemtimetype.ReasonValue);
                    //        //                    //if (dr1[TimeType].ToString() == Convert.ToString("TimeType"))
                    //        //                    //{
                    //        //                    //    dr1["TempButton"] = true;
                    //        //                    //}
                    //        //                    //else
                    //        //                    //{
                    //        //                    //    dr1["TempButton"] = false;
                    //        //                    //}
                    //        //                    ////var TempTimeType= TempEmployeeplantOperationallist.
                    //        //                }
                    //        //                DataTableForGridLayout.Rows.Add(dr1);
                    //        //                rowCounter += 1;
                    //        //            }
                    //        //        }
                    //        //    }
                    //        //}
                    //        //#endregion
                    //    }
                    //}
                }
                DtPlantOperation = new DataTable();
                DtPlantOperation = DataTableForGridLayout;

                //string desiredPropertyValue = "";
                //foreach (DataRow row in dtPlantOperation.Rows)
                //{
                //    // Assuming "PropertyName" is the column name for the desired property
                //    desiredPropertyValue = row["TimeType_"].ToString();

                //    // Check if the desired value is found, and perform any required actions
                //    if (desiredPropertyValue == "TimeType")
                //    {
                //        // Perform the required actions when the desired value is found
                //        break; // Optional: Exit the loop if the desired value is found in the first occurrence
                //    }
                //}
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method FillDashboard()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillDashboard() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private class TempIdStage
        {
            public Int32 IdStage;
            public Int32 IdJobDescription;
            public decimal JobDescriptionUse;
        }

        private void FillPlantOperationPlanningData()
        {
            try
            {

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                GeosApplication.Instance.Logger.Log("Method FillPlantOperationPlanningData ...", category: Category.Info, priority: Priority.Low);

                PlantOperationalPlanning = new List<ERMPlantOperationalPlanning>();
                //if (GeosApplication.Instance.SelectedPlantOwnerUsersList != null) //[GEOS2-4839][gulab lakade][20 09 2023]
                if (ERMCommon.Instance.SelectedAuthorizedPlantsList != null)    //[GEOS2-4839][gulab lakade][20 09 2023]
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
                            GeosApplication.Instance.SplashScreenMessage = "Connecting to " + itemPlantOwnerUsers.Name;
                            string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.Name == itemPlantOwnerUsers.Name).Select(xy => xy.ServiceProviderUrl).FirstOrDefault();
                            ERMService = new ERMServiceController(serviceurl);
                            //ERMService = new ERMServiceController("localhost:6699");

                            //PlantOperationalPlanning.AddRange(ERMService.GetPlantOperationPlanning_V2380(Convert.ToInt32(itemPlantOwnerUsers.IdSite),
                            //   DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),
                            //   DateTime.ParseExact(ToDate, "dd/MM/yyyy", null), jobDescriptioID));
                            //[GEOS2-4338, GEOS2-4434][gulab lakade][15 05 2023]
                            //PlantOperationalPlanning.AddRange(ERMService.GetPlantOperationPlanning_V2390(Convert.ToInt32(itemPlantOwnerUsers.IdSite),
                            //  DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),
                            //  DateTime.ParseExact(ToDate, "dd/MM/yyyy", null), jobDescriptioID));
                            //PlantOperationalPlanning.AddRange(ERMService.GetPlantOperationPlanning_V2400(Convert.ToInt32(itemPlantOwnerUsers.IdSite),
                            // DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),
                            // DateTime.ParseExact(ToDate, "dd/MM/yyyy", null), jobDescriptioID));
                            //[GEOS2-4617][gulab lakade][04-07-2023]
                            //PlantOperationalPlanning.AddRange(ERMService.GetPlantOperationPlanning_V2410(Convert.ToInt32(itemPlantOwnerUsers.IdSite),
                            //DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),
                            //DateTime.ParseExact(ToDate, "dd/MM/yyyy", null), jobDescriptioID));


                            //PlantOperationalPlanning.AddRange(ERMService.GetPlantOperationPlanning_V2420(Convert.ToInt32(itemPlantOwnerUsers.IdSite),
                            //DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),
                            //DateTime.ParseExact(ToDate, "dd/MM/yyyy", null), jobDescriptioID));

                            // [GEOS2-4813][Rupali Sarode][12-09-2023]
                            //  PlantOperationalPlanning.AddRange(ERMService.GetPlantOperationPlanning_V2430(Convert.ToInt32(itemPlantOwnerUsers.IdSite),
                            //DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),
                            //DateTime.ParseExact(ToDate, "dd/MM/yyyy", null), jobDescriptioID));

                            //[GEOS2-4862][Rupali Sarode][04-10-2023]
                         //   PlantOperationalPlanning.AddRange(ERMService.GetPlantOperationPlanning_V2500(Convert.ToInt32(itemPlantOwnerUsers.IdSite),
                         //DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),
                         //DateTime.ParseExact(ToDate, "dd/MM/yyyy", null), jobDescriptioID));
                            PlantOperationalPlanning.AddRange(ERMService.GetPlantOperationPlanning_V2540(Convert.ToInt32(itemPlantOwnerUsers.IdSite),
                        DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),
                        DateTime.ParseExact(ToDate, "dd/MM/yyyy", null), jobDescriptioID)); //[pallavi jadhav][GEOS2-5907][18 07 2024]

                            //FillEmployeeData();//[GEOS2-4839][gulab lakade][18 09 2023]
                            //FillEmployeeData(plantOwners);    //[GEOS2-4839][gulab lakade][18 09 2023]
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
                    FillEmployeeData(plantOwners);  ////[GEOS2-4839][gulab lakade][20 09 2023]

                    //[GEOS2-4649][rupali sarode][10-07-2023]
                    GeosApplication.Instance.SplashScreenMessage = string.Empty;
                    if (ERMCommon.Instance.FailedPlants == null || ERMCommon.Instance.FailedPlants.Count == 0)
                    {
                        ERMCommon.Instance.IsShowFailedPlantWarning = false;
                        ERMCommon.Instance.WarningFailedPlants = string.Empty;
                    }
                }

                //PlantOperationProductionStage = new List<PlantOperationProductionStage>();
                //ERMService = new ERMServiceController("localhost:6699");
                //PlantOperationProductionStage.AddRange(ERMService.GetPlantOperationPlanning_V2380());
                //if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method FillPlantOperationPlanningData() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
        }
        //private void FillEmployeeData() //[GEOS2-4839][gulab lakade][18 09 2023]
     
        private void FillEmployeeData(List<Site> SelectSiteList) //[GEOS2-4839][gulab lakade][18 09 2023]
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillEmployeeData()...", category: Category.Info, priority: Priority.Low);

                string TempEmployeeName = string.Empty;
                EmployeeplantOperationallist = new List<ERMEmployeePlantOperation>();
                EmployeeplantOperationalListForRealTime = new List<ERMEmployeePlantOperation>(); //[Rupali Sarode][GEOS2-4553][19-06-2023]
                ERMEmployeePlantOperation EmployeeplantOperationalForRealTime = new ERMEmployeePlantOperation();
                tempSelctPlantByWeek = new List<TempSelctPlantByWeek>();//[GEOS2-4839][gulab lakade][20 09 2023]
                if (PlantOperationalPlanning != null)
                {
                    //#region rajashri GEOS2-5859[18-06-2024]
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
                    //#endregion
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
                                    var tempPlantName = SelectSiteList.Where(x => x.IdSite == Convert.ToInt32(item.IdCompany)).FirstOrDefault();
                                    if (tempPlantName != null)
                                    {
                                        EmployeeplantOperational.PlantName = Convert.ToString(tempPlantName.Name); //[GEOS2-4839][gulab lakade][18 09 2023]
                                    }

                                   
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
                                        //if (EmployeeplantOperational.EmployeeName == "Uddesh Patil" && EmployeeplantOperational.CalenderWeek == "2024CW22")
                                        //{

                                        //}
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

                                                        ////if (!CNDB_PlantId.Contains(item.IdCompany))
                                                        ////{
                                                        //    EmployeeplantOperational.HRPlan = Convert.ToDecimal(EmployeeplantOperational.HRExpected) - ((Convert.ToDecimal(TotalWeekHours) * (Convert.ToDecimal(item.JobDescriptionUsage) / 100)) + Convert.ToDecimal(breaktime));
                                                        //}
                                                        //else
                                                        //{
                                                            EmployeeplantOperational.HRPlan = Convert.ToDecimal(EmployeeplantOperational.HRExpected) - (Convert.ToDecimal(TotalWeekHours) * (Convert.ToDecimal(item.JobDescriptionUsage) / 100));
                                                        //}

                                                    }
                                                    else if (Leave.IsAllDayEvent == 0)
                                                    {
                                                        double TotalLeaveHours = 0;
                                                        TimeSpan StartDateTime = Convert.ToDateTime(Leave.StartDate).TimeOfDay;
                                                        TimeSpan EndDateTime = Convert.ToDateTime(Leave.EndDate).TimeOfDay;
                                                        TotalLeaveHours = Convert.ToDouble((EndDateTime - StartDateTime).TotalHours);
                                                        #region //rajashri GEOS2-5859
                                                        //double TotalLeaveCount_IncludingHalfDay = 0;
                                                        //TotalLeaveCount_IncludingHalfDay = 1 + (Convert.ToDateTime(Leave.EndDate).Date - Convert.ToDateTime(Leave.StartDate).Date).TotalDays;
                                                        //decimal LeaveCount = (Convert.ToDecimal(TotalWorkingDayCount) - Convert.ToDecimal(TotalLeaveCount_IncludingHalfDay));
                                                        //decimal breaktime = LeaveCount * totalBreakHours;

                                                  //      if (!CNDB_PlantId.Contains(item.IdCompany))
                                                  //      {
                                                  //          EmployeeplantOperational.HRPlan = Convert.ToDecimal(EmployeeplantOperational.HRExpected) -
                                                  //((Convert.ToDecimal(TotalLeaveHours) * (Convert.ToDecimal(item.JobDescriptionUsage) / 100)) + Convert.ToDecimal(breaktime));
                                                  //      }
                                                  //      else
                                                  //      {
                                                            EmployeeplantOperational.HRPlan = Convert.ToDecimal(EmployeeplantOperational.HRExpected) -
                                                  (Convert.ToDecimal(TotalLeaveHours) * (Convert.ToDecimal(item.JobDescriptionUsage) / 100));
                                                        //}
                                                        #endregion
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

                                    //#region RealTime
                                    //List<PlantOperationalPlanningRealInfo> EmployeePlantOperationalRealTimeList = new List<PlantOperationalPlanningRealInfo>();
                                    ////float SumRealTime = 0;
                                    ////float NotRegisteredTime = 0;
                                    ////float FinalRealTime = 0;

                                    //if (item.PlantOperationalPlanningRealInfo != null)
                                    //{
                                    //    EmployeePlantOperationalRealTimeList = item.PlantOperationalPlanningRealInfo;
                                    //}
                                    //try
                                    //{
                                    //    List<PlantOperationalPlanningRealInfo> tmpRealTimeInfoList = EmployeePlantOperationalRealTimeList.Where(i => i.CalenderWeek == weekitem.CalenderWeek).ToList();
                                    //    if (tmpRealTimeInfoList != null)
                                    //    {
                                    //        if (tmpRealTimeInfoList.Count > 0)
                                    //        {
                                    //            if (EmployeePlantOperationalRealTimeList != null)
                                    //            {
                                    //                var tempIdStageForRealTime = EmployeePlantOperationalRealTimeList.GroupBy(x => x.Idstage);

                                    //                foreach (var IdStageForRealTIme in tempIdStageForRealTime)
                                    //                {
                                    //                    SumRealTime = 0;

                                    //                    EmployeeplantOperationalForRealTime = new ERMEmployeePlantOperation();
                                    //                    EmployeeplantOperationalForRealTime.IdEmployee = EmployeeplantOperational.IdEmployee;
                                    //                    EmployeeplantOperationalForRealTime.HRPlan = EmployeeplantOperational.HRPlan;
                                    //                    EmployeeplantOperationalForRealTime.CalenderWeek = EmployeeplantOperational.CalenderWeek;
                                    //                    EmployeeplantOperationalForRealTime.EmployeeName = EmployeeplantOperational.EmployeeName;
                                    //                    EmployeeplantOperationalForRealTime.IdJobDescription = EmployeeplantOperational.IdJobDescription;
                                    //                    EmployeeplantOperationalForRealTime.HRExpected = EmployeeplantOperational.HRExpected;
                                    //                    EmployeeplantOperationalForRealTime.JobDescriptionStartDate = EmployeeplantOperational.JobDescriptionStartDate;
                                    //                    EmployeeplantOperationalForRealTime.IdCompany = EmployeeplantOperational.IdCompany;

                                    //                    //foreach (PlantOperationalPlanningRealInfo RealTimeInfo in EmployeePlantOperationalRealTimeList.Where(i => i.StartTime.Date >= weekitem.FirstDateofweek && i.StartTime.Date <= weekitem.LastDateofWeek).ToList())
                                    //                    foreach (PlantOperationalPlanningRealInfo RealTimeInfo in EmployeePlantOperationalRealTimeList.Where(i => i.CalenderWeek == weekitem.CalenderWeek && i.Idstage == IdStageForRealTIme.Key).ToList())
                                    //                    {
                                    //                        TimeSpan TSReal = RealTimeInfo.EndTime - RealTimeInfo.StartTime;
                                    //                        SumRealTime = SumRealTime + (float)TSReal.TotalMinutes;
                                    //                    }

                                    //                    NotRegisteredTime = (float)TimeSpan.FromHours(Convert.ToDouble(EmployeeplantOperational.HRPlan)).TotalMinutes - SumRealTime;
                                    //                    FinalRealTime = NotRegisteredTime + SumRealTime;
                                    //                    EmployeeplantOperationalForRealTime.RealTime = (float)TimeSpan.FromMinutes(FinalRealTime).TotalHours;
                                    //                    EmployeeplantOperationalForRealTime.IdStage = Convert.ToInt32(IdStageForRealTIme.Key);

                                    //                    EmployeeplantOperationalListForRealTime.Add(EmployeeplantOperationalForRealTime);
                                    //                }
                                    //            }
                                    //        }

                                    //    }
                                    //}
                                    //catch (Exception ex)
                                    //{

                                    //}

                                    //#endregion

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
        private void FillCalenderweek()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCalenderweek()...", category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                //List<PlantOperationPlanningAccordion> CalendarWeek = new List<PlantOperationPlanningAccordion>();
                TemplateWithPlantAccordion = new ObservableCollection<PlantOperationPlanningAccordion>();
                var temp = EmployeeplantOperationallist.GroupBy(x => x.CalenderWeek)
                    .Select(group => new
                    {
                        CalenderWeek = EmployeeplantOperationallist.FirstOrDefault(a => a.CalenderWeek == group.Key).CalenderWeek
                    }).ToList().OrderBy(i => i.CalenderWeek);
                foreach (var item in temp)
                {
                    PlantOperationPlanningAccordion templateWithPlantAccordion = new PlantOperationPlanningAccordion();
                    templateWithPlantAccordion.calendarWeek = Convert.ToString(item.CalenderWeek);
                    templateWithPlantAccordion.copyCalendarWeek = Convert.ToString(item.CalenderWeek);
                    templateWithPlantAccordion.CalendarWeekList = new List<string>();
                    templateWithPlantAccordion.CalendarWeekList.Add(Convert.ToString(item.CalenderWeek));
                    TemplateWithPlantAccordion.Add(templateWithPlantAccordion);

                }
                PlantOperationPlanningAccordion templateWithPlantAccordionAll = new PlantOperationPlanningAccordion();
                if (templateWithPlantAccordionAll == null)
                    templateWithPlantAccordionAll = new PlantOperationPlanningAccordion();
                templateWithPlantAccordionAll.calendarWeek = "All";
                TemplateWithPlantAccordion.Insert(0, templateWithPlantAccordionAll);

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCalenderweek() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCalenderweek() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillCalenderweek() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void GetRecordbyCalendarWeek()
        {
            GeosApplication.Instance.Logger.Log("Method GetRecordbyCalendarWeek ...", category: Category.Info, priority: Priority.Low);
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                //TimeTrackingList = ERMService.GetAllTimeTracking_V2330();
                if (EmployeeplantOperationallist.Count > 0)
                {
                    EmployeeplantOperationallistCopy = new List<ERMEmployeePlantOperation>();
                    EmployeeplantOperationallistCopy.AddRange(EmployeeplantOperationallist);
                    var currentculter = CultureInfo.CurrentCulture;
                    ERMEmployeePlantOperation plantOperation = new ERMEmployeePlantOperation();
                    plantOperation = EmployeeplantOperationallist.FirstOrDefault();
                    #region GEOS2-4045 Gulab lakade
                    if (SelectedItem.ToString().Contains("CW"))
                    {
                        string tempselectItem = Convert.ToString(SelectedItem);
                        int index = tempselectItem.LastIndexOf("(");
                        if (index > 0)
                            tempselectItem = tempselectItem.Substring(0, index);

                        EmployeeplantOperationallist = EmployeeplantOperationallist.Where(x => x.CalenderWeek.ToString().Contains(tempselectItem.Trim())).ToList();
                    }
                    else if (SelectedItem.ToString().Contains("All"))
                    {
                        EmployeeplantOperationallist = EmployeeplantOperationallist.ToList();
                    }

                    #endregion
                    #region[GEOS2-4708][gulab lakade][25 07 2023]
                    TempPlantOperationProductionStage = new List<PlantOperationProductionStage>();
                    TempWorkStageWiseJobDescription = new List<ERMWorkStageWiseJobDescription>();
                    TempEmployeeplantOperationallist = new List<ERMEmployeePlantOperation>();
                    TempEmployeeplantOperationallist = EmployeeplantOperationallist;
                    TempPlantOperationProductionStage.AddRange(PlantOperationProductionStage);
                    TempWorkStageWiseJobDescription = WorkStageWiseJobDescription;

                    GetRecordByStageFilter();
                    AddColumnsToDataTableWithoutBands();
                    // FillCalenderweek();
                    FillDashboard();
                    PlantOperationProductionStage = new List<PlantOperationProductionStage>();
                    PlantOperationProductionStage = TempPlantOperationProductionStage;
                    WorkStageWiseJobDescription = new List<ERMWorkStageWiseJobDescription>();
                    WorkStageWiseJobDescription = TempWorkStageWiseJobDescription;

                    EmployeeplantOperationallist = new List<ERMEmployeePlantOperation>();
                    EmployeeplantOperationallist = TempEmployeeplantOperationallist;
                    //AddColumnsToDataTableWithoutBands();//[GEOS2-4708][gulab lakade][25 07 2023]
                    //FillDashboard();
                    #endregion[GEOS2-4708][gulab lakade][25 07 2023]
                    EmployeeplantOperationallist = new List<ERMEmployeePlantOperation>();
                    EmployeeplantOperationallist.AddRange(EmployeeplantOperationallistCopy);
                }
                else
                {
                    Init();
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method GetRecordbyCalendarWeek() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in GetRecordbyCalendarWeek()", category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
        #region Period Chanegs
        private void setDefaultPeriod()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method setDefaultPeriod()...", category: Category.Info, priority: Priority.Low);

                int year = DateTime.Now.Year;
                DateTime StartFromDate = new DateTime(year, 1, 1);
                DateTime EndToDate = new DateTime(year, 12, 31);
                FromDate = StartFromDate.ToString("dd/MM/yyyy");
                ToDate = EndToDate.ToString("dd/MM/yyyy");

                GeosApplication.Instance.Logger.Log("Method setDefaultPeriod()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {

                GeosApplication.Instance.Logger.Log(string.Format("Error in method setDefaultPeriod() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

            }
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
                    DateTime startDate = new DateTime(Convert.ToInt32(Convert.ToDateTime(FromDate).Year), Convert.ToDateTime(FromDate).Month, 1);

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

                IsBusy = false;
                IsPeriod = true;
                GetWeekList();
                FillPlantOperationPlanningData();
                #region[GEOS2-4708][gulab lakade][25 07 2023]
                //AddColumnsToDataTableWithoutBands();
                //FillDashboard();//[GEOS2-4708][gulab lakade][25 07 2023]
                //FillCalenderweek();//[GEOS2-4708][gulab lakade][25 07 2023]
                TempPlantOperationProductionStage = new List<PlantOperationProductionStage>();
                TempWorkStageWiseJobDescription = new List<ERMWorkStageWiseJobDescription>();
                TempEmployeeplantOperationallist = new List<ERMEmployeePlantOperation>();
                TempEmployeeplantOperationallist = EmployeeplantOperationallist;
                TempPlantOperationProductionStage.AddRange(PlantOperationProductionStage);
                TempWorkStageWiseJobDescription = WorkStageWiseJobDescription;

                GetRecordByStageFilter();
                AddColumnsToDataTableWithoutBands();
                FillCalenderweek();
                FillDashboard();
                PlantOperationProductionStage = new List<PlantOperationProductionStage>();
                PlantOperationProductionStage = TempPlantOperationProductionStage;
                WorkStageWiseJobDescription = new List<ERMWorkStageWiseJobDescription>();
                WorkStageWiseJobDescription = TempWorkStageWiseJobDescription;

                EmployeeplantOperationallist = new List<ERMEmployeePlantOperation>();
                EmployeeplantOperationallist = TempEmployeeplantOperationallist;
                #endregion[GEOS2-4708][gulab lakade][25 07 2023]
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
        #region ChangePlantCommand
        private void ChangePlantCommandAction(object obj)
        {

            try
            {


                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

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
                        //ERMCommon.Instance.SelectedAuthorizedPlantsList = new List<object>();
                        //ERMCommon.Instance.SelectedAuthorizedPlantsList.Add(SelectedPlant);
                        EmployeeplantOperationallist = new List<ERMEmployeePlantOperation>();
                        if (ERMCommon.Instance.SelectedAuthorizedPlantsList != null)
                        {
                            try
                            {

                                GetIdStageAndJobDescriptionByAppSetting();
                                GetWeekList();
                                FillProductionStage(Idstages);
                                FillPlantOperationPlanningData();
                                #region[GEOS2-4708][gulab lakade][25 07 2023]
                                //AddColumnsToDataTableWithoutBands();
                                //FillDashboard();
                                //FillCalenderweek();
                                TempPlantOperationProductionStage = new List<PlantOperationProductionStage>();
                                TempWorkStageWiseJobDescription = new List<ERMWorkStageWiseJobDescription>();
                                TempEmployeeplantOperationallist = new List<ERMEmployeePlantOperation>();
                                TempEmployeeplantOperationallist = EmployeeplantOperationallist;
                                TempPlantOperationProductionStage.AddRange(PlantOperationProductionStage);
                                TempWorkStageWiseJobDescription = WorkStageWiseJobDescription;

                                GetRecordByStageFilter();
                                AddColumnsToDataTableWithoutBands();
                                FillCalenderweek();
                                FillDashboard();
                                PlantOperationProductionStage = new List<PlantOperationProductionStage>();
                                PlantOperationProductionStage = TempPlantOperationProductionStage;
                                WorkStageWiseJobDescription = new List<ERMWorkStageWiseJobDescription>();
                                WorkStageWiseJobDescription = TempWorkStageWiseJobDescription;

                                EmployeeplantOperationallist = new List<ERMEmployeePlantOperation>();
                                EmployeeplantOperationallist = TempEmployeeplantOperationallist;
                                #endregion[GEOS2-4708][gulab lakade][25 07 2023]
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
        #endregion
        #region ToggleButton HidePanel
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
        #endregion
        #region ExportToExcel, Print
        private void PrintPlantOperationalPlanningCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintPlantOperationalPlanningCommandAction()...", category: Category.Info, priority: Priority.Low);
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

                PrintableControlLink pcl = new PrintableControlLink((TableView)obj);
                pcl.Margins.Bottom = 5;
                pcl.Margins.Top = 5;
                pcl.Margins.Left = 5;
                pcl.Margins.Right = 5;
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["PlantOperationPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["PlantOperationPrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                window.Show();

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method PrintPlantOperationalPlanningCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintPlantOperationalPlanningCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        private void ExportPlantOperationalPlanningCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportPlantOperationalPlanningCommandAction()...", category: Category.Info, priority: Priority.Low);
                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "PlantOperationalPlanning";
                saveFile.Filter = "Excel Files (.xlsx)|*.xlsx|All Files (*.*)|*.*";
                saveFile.FilterIndex = 1;
                saveFile.Title = "Save Plant Operational Planning Report";
                DialogResult = (bool)saveFile.ShowDialog();

                if (!DialogResult)
                {
                    ResultFileName = string.Empty;
                }
                else
                {

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
                    ResultFileName = (saveFile.FileName);
                    TableView PlantOperationTableView = ((TableView)obj);
                    PlantOperationTableView.ShowTotalSummary = false;
                    PlantOperationTableView.ShowFixedTotalSummary = false;
                    PlantOperationTableView.ExportToXlsx(ResultFileName);

                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);
                    PlantOperationTableView.ShowTotalSummary = true;
                    PlantOperationTableView.ShowFixedTotalSummary = true;
                }
                GeosApplication.Instance.Logger.Log("Method ExportPlantOperationalPlanningCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportPlantOperationalPlanningCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        #endregion
        #region  Refresh Button
        private void RefreshPlantOperationalPlanningCommandAction(object obj)
        {
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                GeosApplication.Instance.Logger.Log("Method RefreshPlantOperationalPlanningCommandAction()...", category: Category.Info, priority: Priority.Low);

                GetIdStageAndJobDescriptionByAppSetting();
                GetWeekList();
                FillProductionStage(Idstages);
                FillPlantOperationPlanningData();
                #region[GEOS2-4708][gulab lakade][25 07 2023]
                //AddColumnsToDataTableWithoutBands();//[GEOS2-4708][gulab lakade][25 07 2023]
                //FillDashboard();//[GEOS2-4708][gulab lakade][25 07 2023]
                //FillCalenderweek();//[GEOS2-4708][gulab lakade][25 07 2023]
                TempPlantOperationProductionStage = new List<PlantOperationProductionStage>();
                TempWorkStageWiseJobDescription = new List<ERMWorkStageWiseJobDescription>();
                TempEmployeeplantOperationallist = new List<ERMEmployeePlantOperation>();
                TempEmployeeplantOperationallist = EmployeeplantOperationallist;
                TempPlantOperationProductionStage.AddRange(PlantOperationProductionStage);
                TempWorkStageWiseJobDescription = WorkStageWiseJobDescription;

                GetRecordByStageFilter();
                AddColumnsToDataTableWithoutBands();
                FillCalenderweek();
                FillDashboard();
                PlantOperationProductionStage = new List<PlantOperationProductionStage>();
                PlantOperationProductionStage = TempPlantOperationProductionStage;
                WorkStageWiseJobDescription = new List<ERMWorkStageWiseJobDescription>();
                WorkStageWiseJobDescription = TempWorkStageWiseJobDescription;

                EmployeeplantOperationallist = new List<ERMEmployeePlantOperation>();
                EmployeeplantOperationallist = TempEmployeeplantOperationallist;
                #endregion [GEOS2-4708][gulab lakade][25 07 2023]
                MyFilterString = string.Empty;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method RefreshPlantOperationalPlanningCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RefreshPlantOperationalPlanningCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RefreshPlantOperationalPlanningCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshPlantOperationalPlanningCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        #endregion



        #region Test only

        #endregion
        #region [GEOS2-4553][gulab lakade][14 06 2023]
        //[GEOS2-4553][Rupali Sarode][09-06-2023]
        private void FillAllNonOTTimes()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillAllNonOTTimes()...", category: Category.Info, priority: Priority.Low);


                NonOTTimeTypesList = new List<ERMNonOTItemType>();
                //ERMService = new ERMServiceController("localhost:6699");
                NonOTTimeTypesList.Add(new ERMNonOTItemType() { IdReason = 1, ReasonValue = "Production" });
                //NonOTTimeTypesList.AddRange(ERMService.GetAllNonOTTimeType_V2400());
                //NonOTTimeTypesList.AddRange(ERMService.GetAllNonOTTimeType_V2410());    //[GEOS2-4617][gulab lakade][04-07-2023]
                NonOTTimeTypesList.AddRange(ERMService.GetAllNonOTTimeType_V2420());   //[GEOS2-4639][pallavi jadhav][25-07-2023]
                NonOTTimeTypesList.Add(new ERMNonOTItemType() { IdReason = 2, ReasonValue = "HR Plan" });

                GeosApplication.Instance.Logger.Log("Method FillAllNonOTTimes()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillAllNonOTTimes()...." + ex.Message, category: Category.Exception, priority: Priority.Low);

            }
        }
        #endregion


        private void PlantOperationalTimeTypeRealCommandAction(object obj)
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Method PlantOperationalTimeTypeRealCommandAction()...", category: Category.Info, priority: Priority.Low);


                TableView detailView = (TableView)obj;
                GridControl gridControl = detailView.Grid;

                if ((System.Data.DataRowView)detailView.DataControl.CurrentItem != null)
                {
                    DataRowView dr = (System.Data.DataRowView)detailView.DataControl.CurrentItem;
                    // DataRowView dr = (DataRowView)detailView.DataControl.CurrentItem;
                    //string name = detailView.DataControl.CurrentColumn.FieldName;
                    //string[] nameParts = name.Split('_');
                    DataRowView dataRow = (DataRowView)detailView.DataControl.SelectedItem;

                    uint IdEmployees = 0;
                    int HRPlans = 0;
                    //foreach (string item in nameParts)
                    //{

                    //if (item != "TimeType")
                    //{
                    //string IdStage = "IdStage_" + item;
                    //if (!dataRow.Row.Table.Columns.Contains(IdStage))
                    //{
                    string IdStages = null;
                    // }

                    //string IdEmployee = "IdEmployee_" + IdStages;
                    //if (!string.IsNullOrEmpty(dr.Row[IdEmployee]?.ToString()))
                    //{
                    //    IdEmployees = Convert.ToUInt32(dr.Row[IdEmployee].ToString());
                    //}
                    //else
                    //{
                    //    return;
                    //}

                    //string CalenderWeek = Convert.ToString(dr.Row["CalenderWeek"].ToString());
                    //string HRPlan = "HRPlan_" + IdStages;
                    //if (!string.IsNullOrEmpty(dr.Row[HRPlan]?.ToString()))
                    //{

                    //    HRPlans = Convert.ToInt32(dr.Row[HRPlan].ToString());
                    //}


                    // string IdEmployee = "IdEmployee_00";
                    if (!string.IsNullOrEmpty(dr.Row["IdEmployee_00"]?.ToString()))
                    {
                        IdEmployees = Convert.ToUInt32(dr.Row["IdEmployee_00"].ToString());
                    }
                    else
                    {
                        return;
                    }

                    string CalenderWeek = Convert.ToString(dr.Row["CalenderWeek"].ToString());
                    //string HRPlan = "HRPlan_" + IdStages;
                    //if (!string.IsNullOrEmpty(dr.Row[HRPlan]?.ToString()))
                    //{

                    //    HRPlans = Convert.ToInt32(dr.Row[HRPlan].ToString());
                    //}
                    // ERMPlantOperationalPlanning SelectedRow = (ERMPlantOperationalPlanning)detailView.DataControl.CurrentItem;
                    PlantOperationalRealTimeView plantOperationalRealTimeViewtView = new PlantOperationalRealTimeView();
                    PlantOperationalRealTimeViewModel plantOperationalRealTimeViewModel = new PlantOperationalRealTimeViewModel();
                    plantOperationalRealTimeViewtView.DataContext = plantOperationalRealTimeViewModel;

                    EventHandler handle = delegate { plantOperationalRealTimeViewtView.Close(); };
                    plantOperationalRealTimeViewModel.RequestClose += handle;
                    plantOperationalRealTimeViewModel.WindowHeader = System.Windows.Application.Current.FindResource("TimeType").ToString();

                    #region [GEOS2-4707][rupali sarode][26-07-2023]
                    //ERMWorkStageWiseJobDescription TempIDEmployeeJobDescriptionList = WorkStageWiseJobDescription.Where(i => i.IdWorkStage == Convert.ToInt32(IdStages)).FirstOrDefault();

                    List<Int32> IDEmployeeJobDescriptionList = new List<Int32>();
                    //if (TempIDEmployeeJobDescriptionList != null)
                    //{
                    //    IDEmployeeJobDescriptionList = TempIDEmployeeJobDescriptionList.IdJobDescription.Select(Int32.Parse).ToList();
                    //}
                    PlantOperationWeek SelectedWeek = new PlantOperationWeek();
                    SelectedWeek = PlantWeekList.Where(i => i.CalenderWeek == CalenderWeek).FirstOrDefault();
                    IDEmployeeJobDescriptionList = PlantOperationalPlanning.Where(x => x.IdEmployee == IdEmployees && SelectedWeek.FirstDateofweek >= x.JobDescriptionStartDate && SelectedWeek.LastDateofWeek <= x.EndDate).Select(a => a.IdJobDescription).ToList();

                    var tempIdWorkStage = (from IdJD in IDEmployeeJobDescriptionList
                                           from wsjd in WorkStageWiseJobDescription
                                           where wsjd.IdJobDescription.Contains(Convert.ToString(IdJD))
                                           select new
                                           {
                                               wsjd.IdWorkStage
                                           }
                                      ).GroupBy(a => a.IdWorkStage).ToList();
                    Dictionary<int, Int32> HRPLANDictionary = new Dictionary<int, Int32>();
                    var tempStageList = PlantOperationProductionStage.Where(x => stageCodes.Contains(x.StageCode)).ToList();
                    foreach (var item in tempIdWorkStage)
                    {
                        var tempRecord = PlantOperationalPlanning.Where(a => a.IdEmployee == IdEmployees).ToList();

                        if (tempStageList.Where(x => x.IdStage == item.Key).ToList().Count() > 0)
                        {
                            int IDstage = item.Key;
                            string HRPlan = "HRPlan_" + IDstage;
                            //HRPlans = Convert.ToInt32(dr.Row[HRPlan].ToString());
                            HRPLANDictionary.Add(IDstage, Convert.ToInt32(dr.Row[HRPlan].ToString()));

                        }
                    }

                    //plantOperationalRealTimeViewModel.Init(NonOTTimeTypesList, PlantOperationalPlanning, IdStages, IdEmployees, CalenderWeek, HRPlans, IDEmployeeJobDescriptionList);
                    //plantOperationalRealTimeViewModel.Init(NonOTTimeTypesList, PlantOperationalPlanning,  IdEmployees, CalenderWeek, HRPlans, IDEmployeeJobDescriptionList, WorkStageWiseJobDescription, PlantOperationStagesList);
                    plantOperationalRealTimeViewModel.Init(NonOTTimeTypesList, PlantOperationalPlanning, IdEmployees, CalenderWeek, HRPLANDictionary, PlantOperationStagesList, IDEmployeeJobDescriptionList, WorkStageWiseJobDescription, PlantWeekList);
                    #endregion [GEOS2-4707][rupali sarode][26-07-2023]

                    // plantOperationalRealTimeViewModel.Init(NonOTTimeTypesList, PlantOperationalPlanning, IdStages, IdEmployees, CalenderWeek, HRPlans);
                    //var ownerInfo = (detailView as FrameworkElement);
                    //editModulesEquivalentWeightView.Owner = Window.GetWindow(ownerInfo);
                    plantOperationalRealTimeViewtView.ShowDialog();
                    //}
                    //}
                }
                GeosApplication.Instance.Logger.Log("Method PlantOperationalTimeTypeRealCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method PlantOperationalTimeTypeRealCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

            }
        }

        //[GEOS2-4549][rupali sarode][12-07-2023]


        private void CustomSummaryCommandAction(object obj)
        {
            try
            {

                GridCustomSummaryEventArgs e = (GridCustomSummaryEventArgs)obj;

                if (e.SummaryProcess == CustomSummaryProcess.Calculate)
                {
                    if (e.FieldValue != null)
                    {
                        //[GEOS2-4839][gulab lakade][20 09 2023]
                        TempSelctPlantByWeek tempSelctPlant = new TempSelctPlantByWeek();
                        System.Data.DataRowView temprow = ((System.Data.DataRowView)e.Row);
                        System.Data.DataRow tempdatarow = ((System.Data.DataRow)temprow.Row);
                        string TempWeek = Convert.ToString(tempdatarow.ItemArray[0]);
                        string TempPlant = Convert.ToString(tempdatarow.ItemArray[2]);
                        tempSelctPlant.tempWeek = TempWeek;
                        tempSelctPlant.tempPlant = TempPlant;
                        if (tempSelctPlantByWeek.Count() > 0)
                        {
                            if (tempSelctPlantByWeek.Where(a => a.tempWeek == TempWeek && a.tempPlant == TempPlant).Count() == 0)
                            {
                                tempSelctPlantByWeek.Add(tempSelctPlant);
                            }
                        }
                        else
                        {
                            tempSelctPlantByWeek.Add(tempSelctPlant);
                        }


                        //e.TotalValue = Convert.ToInt32(e.TotalValue) + 1;
                        e.TotalValue = tempSelctPlantByWeek.Where(a => a.tempWeek == TempWeek).GroupBy(p => p.tempPlant).Count();
                        //[GEOS2-4839][gulab lakade][20 09 2023]
                    }
                }

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CustomSummaryCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

            }
        }
        #region [GEOS2-4708][gulab lakade][25 07 2023]
        private void FillStages()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillStages ...", category: Category.Info, priority: Priority.Low);

                PlantOperationStagesList = new ObservableCollection<PlanningDateReviewStages>();
                // ERMService = new ERMServiceController("localhost:6699");
                PlanningDateReviewStages PlanOperationStages = new PlanningDateReviewStages();
                PlanOperationStages.IdStage = 0;
                PlanOperationStages.StageCode = "Blanks";
                PlantOperationStagesList.Add(PlanOperationStages);
                PlantOperationStagesList.AddRange(ERMService.GetProductionPlanningReviewStage_V2400());
                GeosApplication.Instance.Logger.Log("Method FillStages() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillStages()", category: Category.Exception, priority: Priority.Low);
            }


        }

        private void FilterOptionLoadedCommandAction(object obj)
        {
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                GeosApplication.Instance.Logger.Log("Method FilterOptionLoadedCommandAction()...", category: Category.Info, priority: Priority.Low);

                //IsFilterLoaded = true;
                ComboBoxEdit combo = obj as ComboBoxEdit;
                //combo.SelectAllItems();
                var items = new List<object>();
                combo.SelectAllItems();

                //IsFilterLoaded = false;
                //IsFilterNotSelectedVisiblity = Visibility.Hidden;
                //IsFilterSomeVisiblity = Visibility.Hidden;
                //IsFilterAllVisiblity = Visibility.Visible;
                //   ProductionPlanningReviewList = ProductionPlanningReviewListCopy;
                GeosApplication.Instance.Logger.Log("Method FilterOptionLoadedCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FilterOptionLoadedCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

            }
        }
        private void FilterOptionEditValueChangedCommandAction(object obj)
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Method FilterOptionEditValueChangedCommandAction()...", category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                DevExpress.Xpf.Editors.ComboBoxEdit temp = (DevExpress.Xpf.Editors.ComboBoxEdit)obj;
                List<ProductionPlanningReview> TempProductionPlanningReviewList = new List<ProductionPlanningReview>();


                TempProductionPlanningReviewList = new List<ProductionPlanningReview>();
                ComboBoxEdit combo = obj as ComboBoxEdit;
                var options = combo.SelectedItems as ObservableCollection<object>;


                if (temp.SelectedItems.Count == PlantOperationStagesList.Count)
                {
                    ID = 1;
                }
                else if (temp.SelectedItems.Count == 0)
                {
                    ID = 2;
                }
                else if (temp.SelectedItems.Count != 0 && temp.SelectedItems.Count < PlantOperationStagesList.Count)
                {
                    ID = 3;
                }
                CustomObservableCollection<UI.Helper.PlanningAppointment> TempAppointmentItems = new CustomObservableCollection<PlanningAppointment>();
                //  List<ProductionPlanningReview> test = ProductionPlanningReviewListCopy.Where(i => i.CurrentWorkStation != null).ToList();

                var propertyValues = options.Select(item => item.GetType().GetProperty("StageCode")?.GetValue(item));
                //List<string> stageCodes = propertyValues.Cast<string>().ToList();

                //start[GEOS2-4708][gulab lakade][25 07 2023]
                stageCodes = new List<string>();
                stageCodes = propertyValues.Cast<string>().ToList();
                // var TempStageCode = PlantOperationStagesList.Select(a => a.StageCode).ToList();

                TempPlantOperationProductionStage = new List<PlantOperationProductionStage>();
                TempWorkStageWiseJobDescription = new List<ERMWorkStageWiseJobDescription>();
                TempEmployeeplantOperationallist = new List<ERMEmployeePlantOperation>();

                TempEmployeeplantOperationallist = EmployeeplantOperationallist;
                TempPlantOperationProductionStage.AddRange(PlantOperationProductionStage);
                TempWorkStageWiseJobDescription = WorkStageWiseJobDescription;

                GetRecordByStageFilter();
                AddColumnsToDataTableWithoutBands();
                FillCalenderweek();
                FillDashboard();
                PlantOperationProductionStage = new List<PlantOperationProductionStage>();
                PlantOperationProductionStage = TempPlantOperationProductionStage;
                WorkStageWiseJobDescription = new List<ERMWorkStageWiseJobDescription>();
                WorkStageWiseJobDescription = TempWorkStageWiseJobDescription;

                EmployeeplantOperationallist = new List<ERMEmployeePlantOperation>();
                EmployeeplantOperationallist = TempEmployeeplantOperationallist;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method FilterOptionEditValueChangedCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FilterOptionEditValueChangedCommandAction()", category: Category.Exception, priority: Priority.Low);
            }
        }
        public void GetRecordByStageFilter()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GetRecordByStageFilter()...", category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                IDJobdescriptionlist = new List<int>();
                PlantOperationProductionStage = new List<PlantOperationProductionStage>();
                if (stageCodes.Count > 0)
                {
                    PlantOperationProductionStage = TempPlantOperationProductionStage.Where(x => stageCodes.Contains(x.StageCode)).ToList();

                    var tempIDStage = PlantOperationProductionStage.Select(a => a.IdStage).ToList();
                    WorkStageWiseJobDescription = new List<ERMWorkStageWiseJobDescription>();
                    if (tempIDStage.Count > 0)
                    {
                        WorkStageWiseJobDescription = TempWorkStageWiseJobDescription.Where(x => tempIDStage.Contains(x.IdWorkStage)).ToList();

                        foreach (var item in WorkStageWiseJobDescription)
                        {
                            foreach (var tempJDID in item.IdJobDescription)
                            {
                                if (!IDJobdescriptionlist.Contains(Convert.ToInt32(tempJDID)))
                                {
                                    IDJobdescriptionlist.Add(Convert.ToInt32(tempJDID));
                                }
                            }
                        }

                    }
                    else
                    {
                        WorkStageWiseJobDescription = new List<ERMWorkStageWiseJobDescription>();

                    }
                    EmployeeplantOperationallist = new List<ERMEmployeePlantOperation>();
                    EmployeeplantOperationallist = TempEmployeeplantOperationallist.Where(a => IDJobdescriptionlist.Contains(a.IdJobDescription)).ToList();

                }
                else
                {
                    WorkStageWiseJobDescription = new List<ERMWorkStageWiseJobDescription>();
                    EmployeeplantOperationallist = new List<ERMEmployeePlantOperation>();
                    EmployeeplantOperationallist = TempEmployeeplantOperationallist.Where(a => IDJobdescriptionlist.Contains(a.IdJobDescription)).ToList();

                }

                //end[GEOS2-4708][gulab lakade][25 07 2023]


                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method FilterOptionEditValueChangedCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in GetRecordByStageFilter()", category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
        #endregion
    }
    //start [GEOS2-4839][gulab lakade][20 09 2023]
    public class TempSelctPlantByWeek
    {
        public string tempWeek;
        public string tempPlant;

    }
    public class TempEmployeeByStage
    {
        public Int32 IdEmployee;
        public Int32 IdStage;

    }
    //end [GEOS2-4839][gulab lakade][20 09 2023]
}
