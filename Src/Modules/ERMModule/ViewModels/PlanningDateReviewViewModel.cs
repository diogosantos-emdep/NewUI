using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using DevExpress.XtraScheduler;
using DevExpress.Xpf.Scheduling;
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
using DevExpress.Xpf.Scheduler;
using DevExpress.Xpf.Scheduler.Menu;
using DevExpress.Xpf.Scheduler.UI;
using DevExpress.Xpf.Bars;
using Microsoft.Win32;
using DevExpress.Xpf.Editors;
using System.Reflection;

namespace Emdep.Geos.Modules.ERM.ViewModels
{
    public class PlanningDateReviewViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Service
        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        IPLMService PLMService = new PLMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IERMService ERMService = new ERMServiceController("localhost:6699");
        //IPLMService PLMService = new PLMServiceController("localhost:6699");
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IERMService ERMService = new ERMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());


        protected ISaveFileDialogService SaveFileDialogService { get { return this.GetService<ISaveFileDialogService>(); } }
        #endregion
        #region Declaration

        //private ObservableCollection<UI.Helper.PlanningAppointment> appointmentItems;
        //public ObservableCollection<UI.Helper.PlanningAppointment> AppointmentItems
        //{
        //    get
        //    {
        //        return appointmentItems;
        //    }

        //    set
        //    {
        //        appointmentItems = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("AppointmentItems"));
        //    }
        //}
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
        public List<ProductionPlanningReview> ProductionPlanningReviewList
        {
            get
            {
                return productionPlanningReviewList;
            }

            set
            {
                productionPlanningReviewList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProductionPlanningReviewList"));
            }
        }

        // public List<ProductionPlanningReview> ProductionPlanningReviewList { get; set; }

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
        #region [GEOS2-4152][Rupali Sarode][2-2-2023]

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
        private bool isBusy;
        private Visibility isSchedulerViewVisible;
        private Visibility isGridViewVisible;
        #endregion [GEOS2-4152][Rupali Sarode][2-2-2023]
        private byte viewType;
        private DateTime selectedStartDate;
        private DateTime selectedEndDate;

        DateTime? fromDates;
        DateTime? toDates;
        private bool isPeriod;
        private string searchOTItem;

        //[Rupali Sarode][GEOS2-4161][20-2-2023]
        List<PlanningDateReview> PlanningDateReviewList = new List<PlanningDateReview>();
        private List<ProductionPlanningReview> productionPlanningReviewListCopy;

        private List<GeosAppSetting> geosAppSettingList;
        private bool isBand;
        private bool isGrid;

        public string PendingPlanningDateReviewGridSettingFilePath = GeosApplication.Instance.UserSettingFolderName + "ERM_PlanningDateReviewGridSetting.Xml";

        List<ProductionPlanningReview> productionPlanningReviewList;
        private TableView view;
        private List<PlanningDateReview> clonedPlanningDateReviewList;
        private DateTime planningDelivaryDate;

        public Int32 sumOfQTY;

        ObservableCollection<PlanningDateReviewStages> planningDateReviewStagesList;
        private PlanningDateReviewStages selectedPlanningDateReviewStages;
        public bool IsFilterLoaded = false;
        private Int32 totalQTY;
        private List<ProductionPlanningReview> groupedData;
        // start [GEOS2-4545][gulab lakade][06 05 2023]
        private List<ProductionPlanningReview> originalonPlanningReviewList;
        public List<ProductionPlanningReview> OriginalonPlanningReviewList
        {
            get
            {
                return originalonPlanningReviewList;
            }

            set
            {
                originalonPlanningReviewList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OriginalonPlanningReviewList"));
            }
        }
        List<string> stageCodes = new List<string>();
        // End [GEOS2-4545][gulab lakade][06 05 2023]

        private Visibility isFilterNotSelectedVisiblity;
        private Visibility isFilterAllVisiblity;
        private Visibility isFilterSomeVisiblity;
        #endregion // Declaration


        #region Public Properties
        public virtual string ResultFileName { get; protected set; }
        public virtual bool DialogResult { get; protected set; }
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
        #region [GEOS2-4152][Rupali Sarode][2-2-2023]
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
        #endregion  [GEOS2-4152][Rupali Sarode][2-2-2023]

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

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
            }
        }

        public Visibility IsSchedulerViewVisible
        {
            get
            {
                return isSchedulerViewVisible;
            }

            set
            {
                if (value == Visibility.Visible && IsGridViewVisible != Visibility.Hidden)
                {
                    IsGridViewVisible = Visibility.Hidden;
                }
                isSchedulerViewVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSchedulerViewVisible"));
            }
        }

        public Visibility IsGridViewVisible
        {
            get
            {
                return isGridViewVisible;
            }
            set
            {
                if (value == Visibility.Visible && IsSchedulerViewVisible != Visibility.Hidden)
                {
                    IsSchedulerViewVisible = Visibility.Hidden;
                }

                isGridViewVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsGridViewVisible"));
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
        #region GEOS2-4156
        //private ObservableCollection<PlanningDateAppointment> appointmentsMainList;
        //public ObservableCollection<PlanningDateAppointment> AppointmentsMainList
        //{
        //    get { return appointmentsMainList; }
        //    set
        //    {
        //        appointmentsMainList = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("AppointmentsMainList"));
        //    }
        //}
        private AppointmentLabelCollection labels;
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
        #endregion
        public byte ViewType
        {
            get
            {
                return viewType;
            }

            set
            {
                viewType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ViewType"));
            }
        }
        public DateTime SelectedStartDate
        {
            get
            {
                return selectedStartDate;
            }

            set
            {
                selectedStartDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedStartDate"));
            }
        }
        public DateTime SelectedEndDate
        {
            get
            {
                return selectedEndDate;
            }

            set
            {
                selectedEndDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedEndDate"));
            }
        }

        private CustomObservableCollection<UI.Helper.PlanningAppointment> appointmentItems;
        public CustomObservableCollection<UI.Helper.PlanningAppointment> AppointmentItems
        {
            get
            {
                return appointmentItems;
            }

            set
            {
                appointmentItems = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AppointmentItems"));
            }
        }
        private CustomObservableCollection<Holidays> companyHolidays;

        private CustomObservableCollection<LookupValue> holidayList;
        private CustomObservableCollection<LabelHelper> labelItems;
        public CustomObservableCollection<Holidays> CompanyHolidays
        {
            get
            {
                return companyHolidays;
            }

            set
            {
                companyHolidays = value;
                //OnPropertyChanged(new PropertyChangedEventArgs("CompanyHolidays"));
            }
        }

        private CustomObservableCollection<UI.Helper.PlanningAppointment> tempAppointmentItems;
        public CustomObservableCollection<UI.Helper.PlanningAppointment> TempAppointmentItems
        {
            get
            {
                return tempAppointmentItems;
            }

            set
            {
                tempAppointmentItems = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TempAppointmentItems"));
            }
        }
        public List<LookupValue> HolidaysTypeList { get; set; }
        //public AppointmentLabelCollection LabelHoliday
        //{
        //    get
        //    {
        //        return labelholiday;
        //    }

        //    set
        //    {
        //        labelholiday = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("LabelHoliday"));
        //    }
        //}

        public CustomObservableCollection<LookupValue> HolidayList
        {
            get
            {
                return holidayList;
            }

            set
            {
                holidayList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("HolidayList"));
            }
        }

        public CustomObservableCollection<LabelHelper> LabelItems
        {
            get
            {
                return labelItems;
            }

            set
            {
                labelItems = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LabelItems"));
            }
        }

        public Visibility ISHoliday
        {
            get
            {
                return isHoliday;
            }

            set
            {
                isHoliday = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ISHoliday"));
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
                //SearchOTCommandAction();
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

        public List<GeosAppSetting> GeosAppSettingList
        {
            get { return geosAppSettingList; }
            set
            {
                geosAppSettingList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GeosAppSettingList"));
            }
        }

        public bool IsBand
        {
            get
            {
                return isBand;
            }
            set
            {
                isBand = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBand"));
            }
        }
        public bool IsGrid
        {
            get
            {
                return isGrid;
            }
            set
            {
                isGrid = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsGrid"));
            }
        }

        public List<PlanningDateReview> ClonedPlanningDateReviewList
        {
            get
            {
                return clonedPlanningDateReviewList;
            }

            set
            {
                clonedPlanningDateReviewList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ClonedPlanningDateReviewList"));
            }
        }
        public DateTime PlanningDelivaryDate
        {
            get
            {
                return planningDelivaryDate;
            }

            set
            {
                planningDelivaryDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PlanningDelivaryDate"));
            }
        }

        public List<ProductionPlanningReview> ProductionPlanningReviewListCopy
        {
            get
            {
                return productionPlanningReviewListCopy;
            }

            set
            {
                productionPlanningReviewListCopy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProductionPlanningReviewListCopy"));
            }
        }
        public Int32 SumOfQTY
        {
            get
            {
                return sumOfQTY;
            }

            set
            {
                sumOfQTY = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SumOfQTY"));
            }
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

        public ObservableCollection<PlanningDateReviewStages> PlanningDateReviewStagesList
        {

            get
            {
                return planningDateReviewStagesList;
            }

            set
            {
                planningDateReviewStagesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PlanningDateReviewStagesList"));
            }

        }

        public PlanningDateReviewStages SelectedPlanningDateReviewStages
        {

            get
            {
                return selectedPlanningDateReviewStages;
            }

            set
            {
                selectedPlanningDateReviewStages = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPlanningDateReviewStages"));
            }

        }

        private CustomObservableCollection<UI.Helper.PlanningAppointment> appointmentItemsCopy;
        public CustomObservableCollection<UI.Helper.PlanningAppointment> AppointmentItemsCopy
        {
            get
            {
                return appointmentItemsCopy;
            }

            set
            {
                appointmentItemsCopy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AppointmentItemsCopy"));
            }
        }

        private Visibility isfilter;
        public Visibility Isfilter
        {
            get { return isfilter; }
            set
            {
                isfilter = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Isfilter"));
            }
        }

        public Int32 TotalQTY
        {
            get
            {
                return totalQTY;
            }
            set
            {
                totalQTY = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TotalQTY"));
            }
        }
        public List<ProductionPlanningReview> GroupedData
        {
            get
            {
                return groupedData;
            }
            set
            {
                groupedData = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GroupedData"));
            }
        }

        public PlanningDateReviewView PlanningDateReviewViewInstance { get; set; }

        public Visibility IsFilterNotSelectedVisiblity
        {
            get { return isFilterNotSelectedVisiblity; }
            set
            {
                isFilterNotSelectedVisiblity = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsFilterNotSelectedVisiblity"));
            }
        }

        public Visibility IsFilterAllVisiblity
        {
            get { return isFilterAllVisiblity; }
            set
            {
                isFilterAllVisiblity = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsFilterAllVisiblity"));
            }
        }

        public Visibility IsFilterSomeVisiblity
        {
            get { return isFilterSomeVisiblity; }
            set
            {
                isFilterSomeVisiblity = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsFilterSomeVisiblity"));
            }
        }
        private Int32 iD;
        public Int32 ID
        {
            get { return iD; }
            set
            {
                iD = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ID"));
            }
        }

        private UInt32 iDCompany;
        public UInt32 IDCompany
        {
            get { return iDCompany; }
            set
            {
                iDCompany = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IDCompany"));
            }
        }

        public List<GeosAppSetting> ActivePlantList = new List<GeosAppSetting>();//[GEOS2-5319][gulab lakade][15 11 2024]

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
        //public ICommand TableViewLoadedCommand { get; set; }
        public ICommand TableViewUnloadedCommand { get; set; }
        public ICommand ItemListTableViewLoadedCommand { get; set; }
        public ICommand CustomSummaryCommand { get; set; }
        public ICommand CommandAccordionControl_Drop { get; set; }
        public ICommand HidePanelCommand { get; set; }
        public ICommand PeriodCommand { get; set; }
        public ICommand ApplyCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand ShowSchedulerViewCommand { get; private set; }
        public ICommand ShowGridViewCommand { get; private set; }
        public ICommand DefaultLoadCommand { get; set; }
        public ICommand SelectedIntervalCommand { get; set; }
        public ICommand scheduler_VisibleIntervalsChangedCommand { get; set; }
        public ICommand PeriodCustomRangeCommand { get; set; }

        public ICommand CommandDepartmentSelection { get; set; }
        public ICommand AppointmentDropCommand { get; set; }
        public ICommand RefreshPlanningDateCommand { get; set; }
        public ICommand SavePlanningDateCommand { get; set; }
        public ICommand PopupMenuShowingCommand { get; set; }
        public ICommand SearchOTCommand { get; set; }
        public ICommand DoubleClickCommand { get; set; }
        public ICommand DisableAppointmentCommand { get; set; }
        //public ICommand LostFocusCommand { get; set; }
        public ICommand DefaultUnLoadCommand { get; set; }
        public ICommand ShowPlanningDateValidationCommand { get; set; }
        public ICommand PrintPlanningDateReviewCommand { get; set; }
        public ICommand ExportPlanningDateReviewCommand { get; set; }
        public ICommand SaveGridPlanningDateCommand { get; set; }

        public ICommand CellValueUpdatedCommnadCommand { get; set; }
        public ICommand SumChangedCommand { get; set; }
        public ICommand FilterOptionLoadedCommand { get; set; }
        public ICommand FilterOptionEditValueChangedCommand { get; set; }
        #endregion

        #region Constructor

        public PlanningDateReviewViewModel()
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Constructor PlanningDateReviewViewModel()...", category: Category.Info, priority: Priority.Low);
                IsInit = true;
                IsFilterNotSelectedVisiblity = Visibility.Hidden;
                IsFilterSomeVisiblity = Visibility.Hidden;
                IsFilterAllVisiblity = Visibility.Visible;
                RefreshTimeTrackingCommand = new RelayCommand(new Action<object>(RefreshTimeTrackingCommandAction));
                PrintPlanningDateReviewCommand = new RelayCommand(new Action<object>(PrintPlanningDateReviewAction));
                ExportPlanningDateReviewCommand = new RelayCommand(new Action<object>(ExportPlanningDateReviewCommandAction));
                HidePanelCommand = new RelayCommand(new Action<object>(HidePanel));
                PeriodCommand = new DelegateCommand<object>(PeriodCommandAction);
                PeriodCustomRangeCommand = new DelegateCommand<object>(PeriodCustomRangeCommandAction);
                ApplyCommand = new DelegateCommand<object>(ApplyCommandAction);
                CancelCommand = new DelegateCommand<object>(CancelCommandAction);
                ChangePlantCommand = new DelegateCommand<object>(ChangePlantCommandAction);
                ShowSchedulerViewCommand = new RelayCommand(new Action<object>(ShowSchedulerViewCommandAction));
                ShowGridViewCommand = new RelayCommand(new Action<object>(ShowPlanningDateGridView));
                //ShowGridViewCommand = new DelegateCommand<RoutedEventArgs>(ShowPlanningDateGridView);
                //TableViewLoadedCommand = new DelegateCommand<RoutedEventArgs>(TableViewLoadedCommandAction);
                RefreshPlanningDateCommand = new RelayCommand(new Action<object>(RefreshPlanningDateCommandAction));

                //[Rupali Sarode][GEOS2-4157][16-02-2023]
                AppointmentDropCommand = new DelegateCommand<AppointmentItemDragDropEventArgs>(AppointmentDropCommandAction);
                PopupMenuShowingCommand = new DelegateCommand<PopupMenuShowingEventArgs>(PopupMenuShowingCommandAction);
                //DoubleClickCommand = new DelegateCommand<MouseButtonEventArgs>(DoubleClickCommandAction);
                DisableAppointmentCommand = new DelegateCommand<AppointmentWindowShowingEventArgs>(AppointmentWindowShowing);

                //[Rupali Sarode][GEOS2-4161][20-2-2023]
                SavePlanningDateCommand = new DelegateCommand(SavePlanningDateCommandAction);
                //LostFocusCommand = new DelegateCommand<RoutedEventArgs>(LostFocusCommandAction);
                DefaultUnLoadCommand = new DelegateCommand<RoutedEventArgs>(DefaultUnLoadCommandAction);

                #region GEOS2-4156
                Labels = new AppointmentLabelCollection();
                Labels.Clear();
                List<GeosAppSetting> PendingPOColorList = WorkbenchStartUp.GetSelectedGeosAppSettings("14,15,16");
                foreach (var item in PendingPOColorList)
                {
                    Labels.Add(Labels.CreateNewLabel(item.IdAppSetting, item.AppSettingName, item.AppSettingName, (Color)System.Windows.Media.ColorConverter.ConvertFromString(item.DefaultValue != null ? item.DefaultValue : "#FFFFFF")));
                }
                #endregion
                holidayList = new CustomObservableCollection<LookupValue>();
                holidayList.AddRangeWithTemporarySuppressedNotification(CrmStartUp.GetLookupValues(28).AsEnumerable());
                DefaultLoadCommand = new DelegateCommand<RoutedEventArgs>(DefaultLoadCommandAction);
                SelectedIntervalCommand = new DelegateCommand<MouseButtonEventArgs>(SelectedIntervalCommandAction);
                scheduler_VisibleIntervalsChangedCommand = new RelayCommand(new Action<object>(VisibleIntervalsChanged));
                CommandDepartmentSelection = new RelayCommand(new Action<object>(SelectItemForScheduler));
                SearchOTCommand = new DelegateCommand<object>(SearchOTCommandAction);
                FillListOfColor();  // Called only once for colors     //[GEOS2-4163] [Pallavi Jadhav] [07 03 2023]
                //TableViewLoadedCommand = new DelegateCommand<RoutedEventArgs>(TableViewLoadedCommandAction);
                TableViewUnloadedCommand = new DelegateCommand<RoutedEventArgs>(TableViewUnloadedCommandAction);
                ShowPlanningDateValidationCommand = new RelayCommand(new Action<object>(ShowPlanningDateValidationCommandAction));
                SaveGridPlanningDateCommand = new RelayCommand(new Action<object>(SaveGridPlanningDateCommandAction));
                CellValueUpdatedCommnadCommand = new DelegateCommand<CellValueChangedEventArgs>(CellValueUpdatedCommnadAction);
                ///SumChangedCommand = new DelegateCommand<RoutedEventArgs>(GetRecordbyDeliveryDate);
                FilterOptionLoadedCommand = new RelayCommand(new Action<object>(FilterOptionLoadedCommandAction));
                FilterOptionEditValueChangedCommand = new DevExpress.Mvvm.DelegateCommand<object>(FilterOptionEditValueChangedCommandAction);

                GeosApplication.Instance.Logger.Log("Constructor Constructor PlanningDateReviewViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Constructor PlanningDateReviewViewModel()." + ex.Message, category: Category.Exception, priority: Priority.Low);
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
                //FailedPlants = new List<string>();
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                ActivePlantList = WorkbenchStartUp.GetSelectedGeosAppSettings("134"); //[pallavi jadhav][GEOS2-5320][06 11 2024]
                IsPeriod = false;
                IsVisibleChanged = true;
                Isfilter = Visibility.Hidden;
                //   SelectedPlant = ERMCommon.Instance.SelectedAuthorizedPlantsList;
                // GetPlants();
                FillStages();
                setDefaultPeriod();
                FillProductionPlanningReview();
                FillDeliveryweek();
                IsGrid = false;
                IsBand = true;
                IsCalendarVisible = Visibility.Collapsed;
                StartDate = DateTime.Today.Date;
                EndDate = DateTime.Today.Date;
                IsSaveEnabled = false;
                PlanningAppointment.IsSaveButtonEnabled = false;
                IsBusy = false;
                IsSchedulerViewVisible = Visibility.Visible;
                IsGridViewVisible = Visibility.Hidden;
                IsFilterNotSelectedVisiblity = Visibility.Hidden;
                IsFilterSomeVisiblity = Visibility.Hidden;
                IsFilterAllVisiblity = Visibility.Visible;
                // IsBand = true;
                //[Rupali Sarode][GEOS2-4161][21-2-2023]
                AppointmentItemsCopy = AppointmentItems;
                PlanningDateReviewList = new List<PlanningDateReview>();
                //  ProductionPlanningReviewListCopy = new List<ProductionPlanningReview>();
                // ProductionPlanningReviewListCopy.AddRange(ProductionPlanningReviewList);

                //FillPlanningDateGridView();
                // PlanningDateSelectAccordain.PlanningDateSelectAccordain();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method Init() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init()", category: Category.Exception, priority: Priority.Low);
            }

        }


        private void FillProductionPlanningReview()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillProductionPlanningReview()...", category: Category.Info, priority: Priority.Low);


                ProductionPlanningReviewList = new List<ProductionPlanningReview>();
                // ProductionPlanningReviewList.AddRange(ERMService.GetProductionPlanningReview_V2360());
                List<ProductionPlanningReview> ProductionPlanningReviewList1 = new List<ProductionPlanningReview>();
                if (GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
                {
                    //List<Company> plantOwners = GeosApplication.Instance.SelectedPlantOwnerUsersList.Cast<Company>().ToList();
                    List<Site> plantOwners = ERMCommon.Instance.SelectedAuthorizedPlantsList.Cast<Site>().ToList();
                    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.Name));
                    ERMCommon.Instance.FailedPlants = new List<string>();
                    ERMCommon.Instance.IsShowFailedPlantWarning = false;
                    ERMCommon.Instance.WarningFailedPlants = string.Empty;
                    foreach (var itemPlantOwnerUsers in plantOwners)
                    {
                        DateTime tempFromyear = DateTime.Parse(FromDate.ToString());
                        string year = Convert.ToString(tempFromyear.Year);
                        string PlantName = Convert.ToString(itemPlantOwnerUsers.Name);
                        try
                        {
                            IDCompany = itemPlantOwnerUsers.IdSite;
                            string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.Name == itemPlantOwnerUsers.Name).Select(xy => xy.ServiceProviderUrl).FirstOrDefault();

                            ERMService = new ERMServiceController(serviceurl);
                            //ERMService = new ERMServiceController("localhost:6699");
                            //ProductionPlanningReviewList.AddRange(ERMService.GetProductionPlanningReview_V2360(PlantName,
                            //    DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),
                            //    DateTime.ParseExact(ToDate, "dd/MM/yyyy", null)));
                            //ProductionPlanningReviewList.AddRange(ERMService.GetProductionPlanningReview_V2370(PlantName,
                            //   DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),
                            //   DateTime.ParseExact(ToDate, "dd/MM/yyyy", null), GeosAppSettingList));
                            //ProductionPlanningReviewList.AddRange(ERMService.GetProductionPlanningReview_V2380(PlantName,
                            // DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),
                            // DateTime.ParseExact(ToDate, "dd/MM/yyyy", null), GeosAppSettingList));
                            //  ProductionPlanningReviewList.AddRange(ERMService.GetProductionPlanningReview_V2400(PlantName,  //[pallavi jadhav] [GEOS2-4481] [26 05 2023] 
                            //DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),
                            //DateTime.ParseExact(ToDate, "dd/MM/yyyy", null), GeosAppSettingList));
                            //   ProductionPlanningReviewList.AddRange(ERMService.GetProductionPlanningReview_V2400(PlantName,  //[pallavi jadhav] [GEOS2-4638] [04 07 2023] 
                            //DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),
                            //DateTime.ParseExact(ToDate, "dd/MM/yyyy", null), GeosAppSettingList));

                            //ProductionPlanningReviewList.AddRange(ERMService.GetProductionPlanningReview_V2410(PlantName,  //[pallavi jadhav] [GEOS2-4638] [04 07 2023] 
                            //DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),
                            //DateTime.ParseExact(ToDate, "dd/MM/yyyy", null), GeosAppSettingList));

                            //[rupali sarode][28/07/2023]
                            //ProductionPlanningReviewList.AddRange(ERMService.GetProductionPlanningReview_V2420(PlantName,  //[pallavi jadhav] [GEOS2-4638] [04 07 2023] 
                            //DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),
                            //DateTime.ParseExact(ToDate, "dd/MM/yyyy", null), GeosAppSettingList));
                            //[GEOS2-5097][gulab lakade][04 12 2023]
                            //ProductionPlanningReviewList.AddRange(ERMService.GetProductionPlanningReview_V2460(PlantName,  
                            //DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),
                            //DateTime.ParseExact(ToDate, "dd/MM/yyyy", null), GeosAppSettingList));

                            // ProductionPlanningReviewList.AddRange(ERMService.GetProductionPlanningReview_V2540(PlantName,
                            //DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),
                            //DateTime.ParseExact(ToDate, "dd/MM/yyyy", null), GeosAppSettingList));  //[pallavi jadhav] [GEOS2-5907] [18 07 2024]

                            //[GEOS2-5319][gulab lakade][19 11 2024]
                            #region [GEOS2-5319][gulab lakade][19 11 2024]
                            string ActivePlantString = ActivePlantList.Select(a => a.DefaultValue).FirstOrDefault();
                            List<string> TempActivePlantlist = new List<string>();
                            if (ActivePlantString != null)
                            {
                                TempActivePlantlist = ActivePlantString.Split(',').ToList();
                            }
                            if (TempActivePlantlist != null && TempActivePlantlist.Contains(Convert.ToString(IDCompany)) == true)
                            {
                                ProductionPlanningReviewList.AddRange(ERMService.GetProductionPlanningReviewByStage_V2580(PlantName,
                         DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),
                         DateTime.ParseExact(ToDate, "dd/MM/yyyy", null), GeosAppSettingList));
                            }
                            else
                            {
                                ProductionPlanningReviewList.AddRange(ERMService.GetProductionPlanningReview_V2580(PlantName,
                         DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),
                         DateTime.ParseExact(ToDate, "dd/MM/yyyy", null), GeosAppSettingList));
                            }

                            #endregion
                            OriginalonPlanningReviewList = new List<ProductionPlanningReview>();    // [GEOS2-4545][gulab lakade][06 05 2023]
                            OriginalonPlanningReviewList = ProductionPlanningReviewList;    // [GEOS2-4545][gulab lakade][06 05 2023]
                            ProductionPlanningReviewListCopy = new List<ProductionPlanningReview>();
                            // ProductionPlanningReviewListCopy = ProductionPlanningReviewList; // [GEOS2-4545][gulab lakade][06 05 2023]
                            //companyHolidays.AddRangeWithTemporarySuppressedNotification(ERMService.GetCompanyHolidaysBySelectedIdCompany(PlantName, year));
                            //  List<ProductionPlanningReview> groupedData = ProductionPlanningReviewList.GroupBy(a => a.OTCode).Select(g => new { OTCode = g.Key,TotalQty = g.Sum(a => a.QTY)}).ToList();
                            //GroupedData = new List<ProductionPlanningReview>();
                            //GroupedData = ProductionPlanningReviewList
                            //                                   .GroupBy(a => new { a.OTCode, a.CurrentWorkStation })
                            //                                   .Select(g => new ProductionPlanningReview
                            //                                   {
                            //                                       OTCode = g.Key.OTCode,
                            //                                       QTY = g.Sum(a => a.QTY),
                            //                                       DeliveryDate = g.FirstOrDefault()?.DeliveryDate,
                            //                                       DeliveryDateHtmlColor = g.FirstOrDefault()?.DeliveryDateHtmlColor,
                            //                                       DeliveryWeek = g.FirstOrDefault()?.DeliveryWeek,
                            //                                       OriginalPlant = g.FirstOrDefault()?.OriginalPlant,
                            //                                       ProductionPlant = g.FirstOrDefault()?.ProductionPlant,
                            //                                       Template = g.FirstOrDefault()?.Template,
                            //                                       Type = g.FirstOrDefault()?.Type,
                            //                                       NumItem = g.FirstOrDefault()?.NumItem,
                            //                                       CurrentWorkStation = g.Key.CurrentWorkStation,
                            //                                       PlanningDeliveryDate = g.FirstOrDefault()?.PlanningDeliveryDate
                            //                                   }).ToList();
                            //ProductionPlanningReviewList = GroupedData;
                            ProductionPlanningReviewListCopy = ProductionPlanningReviewList;    // [GEOS2-4545][gulab lakade][06 05 2023]
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
                            System.Threading.Thread.Sleep(1000);
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
                            System.Threading.Thread.Sleep(1000);
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
                            System.Threading.Thread.Sleep(1000);
                            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", PlantName, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }
                    }
                    //[GEOS2-4649][rupali sarode][10-07-2023]
                    //GeosApplication.Instance.SplashScreenMessage = string.Empty;
                    //if (ERMCommon.Instance.FailedPlants == null || ERMCommon.Instance.FailedPlants.Count == 0)
                    //{
                    //    ERMCommon.Instance.IsShowFailedPlantWarning = false;
                    //    ERMCommon.Instance.WarningFailedPlants = string.Empty;
                    //}

                }
                #region GEOS2-4156
                AppointmentItems = new CustomObservableCollection<UI.Helper.PlanningAppointment>();
                FillERMDataInObjectsByCallingLatestServiceMethods.ShowPleaseWaitScreen();
                if (ProductionPlanningReviewList != null && ProductionPlanningReviewList.Count > 0)
                {

                    foreach (var item in ProductionPlanningReviewList)
                    {
                        UI.Helper.PlanningAppointment modelActivity = new UI.Helper.PlanningAppointment();

                        if (item.PlanningDeliveryDate != null)
                        {
                            // string TotalQTY = ProductionPlanningReviewList.Where(a => a.PlanningDeliveryDate == item.PlanningDeliveryDate ).Sum(b => b.QTY).ToString();
                        }
                        else
                        {
                            // modelActivity.= Convert.ToUInt32(ProductionPlanningReviewList.Where(a => a.DeliveryDate == item.DeliveryDate).Sum(b => b.QTY).ToString());

                        }



                        if (item.PlanningDeliveryDate != null)
                        {
                            modelActivity.StartDate = item.PlanningDeliveryDate.Value;
                            modelActivity.EndDate = item.PlanningDeliveryDate.Value.AddMinutes(30);
                        }
                        else
                        {
                            modelActivity.StartDate = item.DeliveryDate.Value;
                            modelActivity.EndDate = item.DeliveryDate.Value.AddMinutes(30);
                        }

                        if (item.DeliveryDate.Value.Date <= GeosApplication.Instance.ServerDateTime.Date)
                            modelActivity.Label = Convert.ToInt32(Labels[0].Id);
                        else if (item.DeliveryDate.Value.Date > GeosApplication.Instance.ServerDateTime.Date && item.DeliveryDate.Value.Date <= GeosApplication.Instance.ServerDateTime.Date.AddDays(6))
                            modelActivity.Label = Convert.ToInt32(Labels[1].Id);
                        else if (item.DeliveryDate.Value.Date >= GeosApplication.Instance.ServerDateTime.Date.AddDays(7))
                            modelActivity.Label = Convert.ToInt32(Labels[2].Id);
                        //else if (item.DeliveryDate.Value.Date > GeosApplication.Instance.ServerDateTime.Date)
                        //{
                        //    if (item.DeliveryDate.Value.Date >= GeosApplication.Instance.ServerDateTime.Date.AddDays(7))
                        //        modelActivity.Label = Convert.ToInt32(Labels[2].Id);
                        //    else
                        //        modelActivity.Label = Convert.ToInt32(Labels[2].Id);
                        //}

                        modelActivity.IsHolidayData = Visibility.Collapsed;  //holiday data
                        modelActivity.IsHolidayDate = Visibility.Visible;  //ot data
                        modelActivity.Subject = item.OTCode;
                        //modelActivity.Description = item.Type;
                        modelActivity.DeliveryDate = item.DeliveryDate;
                        modelActivity.Template = item.Template;
                        modelActivity.NumItem = item.NumItem;
                        modelActivity.QTY = item.QTY;
                        modelActivity.OTCode = item.OTCode;
                        modelActivity.Type = item.Type;
                        modelActivity.OriginPlant = item.OriginalPlant;
                        modelActivity.ContentSubject = item.CurrentWorkStation + ";" + item.OTCode + "; " + item.NumItem + "; " + item.Type + ";" + item.QTY;
                        //modelActivity.Status = Convert.ToInt32(item.IdCPType);
                        modelActivity.CurrentWorkStation = item.CurrentWorkStation;//[pallavi jadhav] [GEOS2-4481] [26 05 2023] 
                        #region //[pallavi jadhav] [GEOS2-4519] [06 06 2023] 
                        var tempReal = item.PlanningDateReviewStages.Select(a => a.Real).ToList();  // Convert the LINQ query result to a list
                        if (tempReal != null)
                        {
                            TimeSpan Temreal = TimeSpan.FromSeconds(tempReal.Sum(d => d.HasValue ? (double)d.Value : 0));
                            modelActivity.Real = Temreal.ToString();
                        }

                        TimeSpan TemExpected = TimeSpan.Parse("0");
                        var tempExpected = item.PlanningDateReviewStages.Select(a => a.Expected).ToList();
                        if (tempExpected != null)
                        {
                            TemExpected = TimeSpan.FromMinutes(tempExpected.Sum(d => d.HasValue ? (double)d.Value : 0));
                            // TemExpected = TimeSpan.FromSeconds(tempExpected.Sum(d => (double)d));
                            modelActivity.Expected = TemExpected.ToString();
                        }
                        #endregion
                        modelActivity.PlanningDeliveryDate = item.PlanningDeliveryDate;
                        if (item.PlanningDeliveryDate != null)
                        {
                            if (item.DeliveryDate != item.PlanningDeliveryDate)
                            {
                                modelActivity.IsPlannedDeliveryDate = true;
                            }
                            else
                            {
                                modelActivity.IsPlannedDeliveryDate = false;
                            }
                            modelActivity.PlanningDeliveryDate = item.PlanningDeliveryDate;
                        }
                        else
                        {
                            modelActivity.IsPlannedDeliveryDate = false;
                        }
                        if (item.LastUpdateDate != null)
                        {
                            modelActivity.LastUpdate = item.LastUpdateDate;
                        }

                        #region [Rupali Sarode][GEOS2-4161][21-2-2023]
                        modelActivity.IdOT = Convert.ToUInt32(item.IdOt);
                        modelActivity.IdCounterpart = Convert.ToUInt32(item.IdCounterpart);
                        #endregion

                        #region [Rupali Sarode][GEOS2-4347][05-05-2023]
                        modelActivity.Customer = item.Customer;
                        modelActivity.Project = item.Project;
                        #endregion
                        #region [pallavi jadhav][GEOS2-5319][22-10-2024]

                        modelActivity.IdCounterpart = Convert.ToUInt32(item.IdCounterpart);
                        modelActivity.IdSite = Convert.ToInt32(item.IdProductionPlant);//[gulab lakade][15 11 2024]
                        #endregion

                        AppointmentItems.Add(modelActivity);

                    }

                    List<GeosAppSetting> PendingPOColorList = WorkbenchStartUp.GetSelectedGeosAppSettings("14,15,16");
                    //  IsHolidayDate = Visibility.Collapsed;
                    Isfilter = Visibility.Hidden;

                    FillERMDataInObjectsByCallingLatestServiceMethods.GetERMDataOnceFromServiceForAttendance(
                   ref ERMService, ref CrmStartUp,
                     ref companyHolidays,
                   ref holidayList, ref labelItems, ref PendingPOColorList,
                   ref appointmentItems, ref fromDate, ref toDate, ref appointmentItems
                    );

                    // AppointmentItems.Add(appointmentItems);
                    // AppointmentItems = new ObservableCollection<UI.Helper.PlanningAppointment>(AppointmentItems);

                    FillERMDataInObjectsByCallingLatestServiceMethods.ClosePleaseWaitScreen();
                    // ShowCompanyHolidayAppointmentsForSelectedCompany();
                }
                //  RefreshPlanningDateCommandAction(null);

                //[GEOS2-4649][rupali sarode][10-07-2023]
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                if (ERMCommon.Instance.FailedPlants == null || ERMCommon.Instance.FailedPlants.Count == 0)
                {
                    ERMCommon.Instance.IsShowFailedPlantWarning = false;
                    ERMCommon.Instance.WarningFailedPlants = string.Empty;
                }

                #endregion

                GeosApplication.Instance.Logger.Log("Method FillProductionPlanningReview()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillProductionPlanningReview()", category: Category.Exception, priority: Priority.Low);
            }
        }
        List<Site> TempSelectedOldPlant = new List<Site>();

        #region Method
        private void GetPlants()
        {
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                GeosApplication.Instance.Logger.Log("Method GetPlants()...", category: Category.Info, priority: Priority.Low);
                if (PlantList == null || PlantList.Count == 0)
                {
                    // PlantList = new ObservableCollection<Site>(PLMService.GetPlants_V2120());
                    // PlantList = ERMCommon.Instance.UserAuthorizedPlantsList;
                }

                List<Company> plantOwners = GeosApplication.Instance.SelectedPlantOwnerUsersList.Cast<Company>().ToList();
                var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.ConnectPlantId));

                //List<Site> PlantList1 = new List<Site>();
                //foreach (Company item in plantOwners)
                //{

                //    UInt32 plantid = Convert.ToUInt32(item.ConnectPlantId);
                //    PlantList1 = PlantList.Where(x => x.IdCompany == plantid).ToList();
                //    if (SelectedPlant == null)
                //        SelectedPlant = new List<object>();

                //    foreach (Site plant in PlantList1)
                //    {
                //        SelectedPlant.Add(PlantList.FirstOrDefault(a => a.IdSite == plant.IdSite));
                //        if (SelectedPlantold == null)
                //            SelectedPlantold = new List<object>();
                //        SelectedPlantold = SelectedPlant;
                //    }

                //}

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method GetPlants()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in GetPlants() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in GetPlants() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method GetPlants() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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
        private void FillDeliveryweek()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillDeliveryweek()...", category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                List<ProductionPlanningReview> DeliverWeekGroup = new List<ProductionPlanningReview>();
                var temp = ProductionPlanningReviewList.GroupBy(x => x.DeliveryWeek)
                    .Select(group => new
                    {
                        DeliveryWeek = ProductionPlanningReviewList.FirstOrDefault(a => a.DeliveryWeek == group.Key).DeliveryWeek,

                        Count = ProductionPlanningReviewList.Where(b => b.DeliveryWeek == null).Count(),
                    }).ToList().OrderBy(i => i.DeliveryWeek);

                TemplateWithPlanningDate = new ObservableCollection<PlanningDateAccordian>();
                foreach (var item in temp)
                {

                    //DeliverWeekGroup = ProductionPlanningReviewList.Where(x => x.DeliveryWeek == item.DeliveryWeek).OrderBy(a => a.DeliveryDate).ToList();
                    DeliverWeekGroup = ProductionPlanningReviewList.Where(x => x.DeliveryWeek == item.DeliveryWeek).OrderBy(a => (a.PlanningDeliveryDate != null ? a.PlanningDeliveryDate : a.DeliveryDate)).ToList();
                    PlanningDateAccordian templateWithPlanningDate = new PlanningDateAccordian();
                    var currentculter = CultureInfo.CurrentCulture;
                    var tempdateByPlanningDeliveryDate = (from dw in DeliverWeekGroup
                                                          select new
                                                          {
                                                              FDeliveryDate = dw.DeliveryDate,
                                                              dw.PlanningDeliveryDate,
                                                              DeliveryDate = (dw.PlanningDeliveryDate != null ? dw.PlanningDeliveryDate : dw.DeliveryDate)
                                                          }
                                              ).Distinct().OrderBy(a => a.DeliveryDate).ToList();
                    //List<DateTime?> tempdate = tempdateByPlanningDeliveryDate.Select(i => i.DeliveryDate).Distinct().ToList();
                    List<DateTime?> tempdate = tempdateByPlanningDeliveryDate.Select(i => (i.PlanningDeliveryDate != null ? i.PlanningDeliveryDate : i.DeliveryDate)).Distinct().ToList();
                    if (templateWithPlanningDate.PlanningDeliveryDate == null)
                        templateWithPlanningDate.PlanningDeliveryDate = new ObservableCollection<PlanningDeliveryDate>();
                    if (tempdate.Count > 0)
                    {
                        List<DateTime> tempDateorderBy = tempdate.Select(a => a.Value.Date).Distinct().ToList();   //// Gulab lakade Order by CW ASC 04-05-2023
                        var tempDateFinal = tempDateorderBy.OrderBy(a => a.Date).ToList();  //// Gulab lakade Order by CW ASC 04-05-2023
                        foreach (var Dateitem in tempDateFinal)
                        {
                            PlanningDeliveryDate tempPlanningDeliveryDate2 = new PlanningDeliveryDate();
                            tempPlanningDeliveryDate2 = new PlanningDeliveryDate();
                            List<ProductionPlanningReview> DeliverWeekGrouptemp = new List<ProductionPlanningReview>();
                            // DeliverWeekGrouptemp = DeliverWeekGroup.Where(a => DateTime.Compare(a.DeliveryDate.Value.Date, Dateitem.Date) == 0).Distinct().ToList();
                            DeliverWeekGrouptemp = DeliverWeekGroup.Where(a => DateTime.Compare(
                                (a.PlanningDeliveryDate != null ? a.PlanningDeliveryDate.Value.Date : a.DeliveryDate.Value.Date),
                                Dateitem.Date) == 0).Distinct().ToList();
                            tempPlanningDeliveryDate2.OtCodeList = new List<string>();
                            int DateCount = 0;
                            string tempDeliveryDate = (Convert.ToDateTime(Dateitem)).ToString("d", currentculter);
                            //var tempdatevalue = ProductionPlanningReviewList.Where(x => DateTime.Compare(x.DeliveryDate.Value.Date, Dateitem.Date) == 0).Distinct().ToList().Select(a => a.OTCode).Distinct().ToList();
                            var tempdatevalue = ProductionPlanningReviewList.Where(x => DateTime.Compare(
                               (x.PlanningDeliveryDate != null ? x.PlanningDeliveryDate.Value.Date : x.DeliveryDate.Value.Date),
                                Dateitem.Date) == 0).Distinct().ToList().Select(a => a.OTCode).Distinct().ToList();
                            //#region comment for some region
                            //foreach (var otitem in tempdatevalue)
                            //{
                            //    if (!tempPlanningDeliveryDate2.OtCodeList.Contains(otitem))
                            //    {
                            //        tempPlanningDeliveryDate2.OtCodeList.Add(otitem);
                            //        DateCount++;
                            //    }

                            //}
                            //#endregion
                            var TempPlanningDateCount = TemplateWithPlanningDate.Where(x => x.copyDeliveryWeek == item.DeliveryWeek).ToList().Select(b => b.PlanningDeliveryDate.Where(c => c.copyDeliveryDate == Dateitem.ToString("d", currentculter)).ToList());
                            if (TempPlanningDateCount.Count() == 0)
                            {
                                tempPlanningDeliveryDate2.copyDeliveryDate = Dateitem.ToString("d", currentculter);
                                //tempPlanningDeliveryDate2.DeliveryDate = Dateitem.ToString("d", currentculter) + " (" + DateCount + ")";    // comment for some region
                                tempPlanningDeliveryDate2.DeliveryDate = Dateitem.ToString("d", currentculter);

                                templateWithPlanningDate.PlanningDeliveryDate.Add(tempPlanningDeliveryDate2);
                            }
                        }
                    }
                    templateWithPlanningDate.deliveryWeek = item.DeliveryWeek + " (" + templateWithPlanningDate.PlanningDeliveryDate.Count + ")"; ;
                    templateWithPlanningDate.copyDeliveryWeek = item.DeliveryWeek;
                    TemplateWithPlanningDate.Add(templateWithPlanningDate);
                }
                PlanningDateAccordian templateWithPlanningDateAll = new PlanningDateAccordian();
                if (templateWithPlanningDateAll.PlanningDeliveryDate == null)
                    templateWithPlanningDateAll.PlanningDeliveryDate = new ObservableCollection<PlanningDeliveryDate>();
                templateWithPlanningDateAll.deliveryWeek = "All";
                TemplateWithPlanningDate.Insert(0, templateWithPlanningDateAll);


                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method FillDeliveryweek()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillDeliveryweek() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillDeliveryweek() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillDeliveryweek() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }



        private void RefreshTimeTrackingCommandAction(object obj)
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Method RefreshTimeTrackingCommandAction()...", category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

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
            // try
            //{
            //    GeosApplication.Instance.Logger.Log("Method PrintTimeTrackingCommandAction()...", category: Category.Info, priority: Priority.Low);

            //    IsBusy = true;

            //    if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

            //    PrintableControlLink pcl = new PrintableControlLink((TableView)obj);
            //    pcl.Margins.Bottom = 5;
            //    pcl.Margins.Top = 5;
            //    pcl.Margins.Left = 5;
            //    pcl.Margins.Right = 5;

            //    pcl.PaperKind = System.Drawing.Printing.PaperKind.A4;
            //    pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["TimeTrackingPrintHeaderTemplate"];
            //    pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["TimeTrackingPrintFooterTemplate"];
            //    pcl.Landscape = true;
            //    pcl.CreateDocument(false);

            //    DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
            //    window.PreviewControl.DocumentSource = pcl;
            //    IsBusy = false;
            //    window.Show();

            //    DevExpress.XtraPrinting.PrintTool printTool = new DevExpress.XtraPrinting.PrintTool(pcl.PrintingSystem);
            //    printTool.PrinterSettings.PrinterName = GeosApplication.Instance.UserSettings["SelectedPrinter"];

            //    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

            //    GeosApplication.Instance.Logger.Log("Method PrintTimeTrackingCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            //}
            //catch (Exception ex)
            //{
            //    IsBusy = false;
            //    GeosApplication.Instance.Logger.Log("Get an error in Method PrintTimeTrackingCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            //}
        }
        private void ExportTimeTrackingCommandAction(object obj)
        {
            //try
            //{
            //    GeosApplication.Instance.Logger.Log("Method PrintTimeTrackingCommandAction()...", category: Category.Info, priority: Priority.Low);

            //    Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
            //    saveFile.DefaultExt = "xlsx";
            //    saveFile.FileName = "Time Tracking";
            //    saveFile.Filter = "Excel Files (.xlsx)|*.xlsx|All Files (*.*)|*.*";
            //    saveFile.FilterIndex = 1;
            //    saveFile.Title = "Save Excel Report";
            //    DialogResult = (Boolean)saveFile.ShowDialog();

            //    if (!DialogResult)
            //    {
            //        ResultFileName = string.Empty;
            //    }
            //    else
            //    {
            //        if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

            //        ResultFileName = (saveFile.FileName);
            //        TableView activityTableView = ((TableView)obj);
            //        activityTableView.ShowTotalSummary = false;
            //        activityTableView.ShowFixedTotalSummary = false;
            //        XlsxExportOptionsEx options = new XlsxExportOptionsEx();
            //        //options.CustomizeCell += Options_CustomizeCell;
            //        activityTableView.ExportToXlsx(ResultFileName, options);

            //        IsBusy = false;
            //        activityTableView.ShowFixedTotalSummary = true;
            //        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
            //        System.Diagnostics.Process.Start(ResultFileName);

            //        GeosApplication.Instance.Logger.Log("Method PrintTimeTrackingCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    IsBusy = false;
            //    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
            //    CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            //    GeosApplication.Instance.Logger.Log("Get an error in Method PrintTimeTrackingCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            //}
        }


        #endregion
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
                    //SelectedStartDate = scheduler.SelectedInterval.Start;
                    //SelectedEndDate = scheduler.SelectedInterval.End;
                }
                else if (IsButtonStatus == 2)//last one month
                {
                    FromDate = lastOneMonthStart.ToString("dd/MM/yyyy");
                    ToDate = lastOneMonthEnd.ToString("dd/MM/yyyy");
                    //PlanningSchedulerControl scheduler =  PlanningSchedulerControl();
                    //DateTime start = Convert.ToDateTime(lastOneMonthStart);
                    //DateTime end = Convert.ToDateTime(lastOneMonthStart).AddDays(1);
                    //scheduler.SelectedInterval = new DevExpress.Mvvm.DateTimeRange(start, end);

                    //DateTime startDate = new DateTime(Convert.ToInt32(Convert.ToDateTime(lastOneMonthStart).Year), Convert.ToDateTime(lastOneMonthStart).Month, 1);
                    //scheduler.Month = startDate;
                    //scheduler.SelectedInterval = new DevExpress.Mvvm.DateTimeRange(Convert.ToDateTime(lastOneMonthStart), Convert.ToDateTime(lastOneMonthStart).AddDays(1));

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

                if (IsSchedulerViewVisible == Visibility.Visible)
                {
                    // WorkLogUserList();
                    // WorklogUserOTAndHours();
                }
                else
                {
                    //FillWorkLogReportGrid();
                }

                if (ERMCommon.Instance.SelectedAuthorizedPlantsList != null)
                {
                    GetPlanningDateRecordByPlantAndDate(ERMCommon.Instance.SelectedAuthorizedPlantsList, FromDate, ToDate);

                    List<ProductionPlanningReview> ProductionPlanningReviewListTemp = new List<ProductionPlanningReview>();
                    ProductionPlanningReviewListTemp = ProductionPlanningReviewList;
                    ProductionPlanningReviewList = new List<ProductionPlanningReview>();
                    ProductionPlanningReviewList = ProductionPlanningReviewListTemp;
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
        #endregion

        #region temp 
        private void FillPlanningDateGridView()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillPlanningDateGridView ...", category: Category.Info, priority: Priority.Low);

                ProductionPlanningReviewList = new List<ProductionPlanningReview>();
                ERMCommon.Instance.FailedPlants = new List<string>();
                ERMCommon.Instance.IsShowFailedPlantWarning = false;
                ERMCommon.Instance.WarningFailedPlants = string.Empty;
                foreach (Site item in SelectedPlant)
                {
                    string PlantName = Convert.ToString(item.Name);
                    try
                    {
                        string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.Name == item.Name).Select(xy => xy.ServiceProviderUrl).FirstOrDefault();
                        ERMService = new ERMServiceController(serviceurl);
                        //ERMService = new ERMServiceController("localhost:6699");
                        //ProductionPlanningReviewList.AddRange(ERMService.GetProductionPlanningReview_V2370(PlantName,
                        //   DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),
                        //   DateTime.ParseExact(ToDate, "dd/MM/yyyy", null), GeosAppSettingList));
                        //ProductionPlanningReviewList.AddRange(ERMService.GetProductionPlanningReview_V2380(PlantName,
                        //  DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),
                        //  DateTime.ParseExact(ToDate, "dd/MM/yyyy", null), GeosAppSettingList));
                        //ProductionPlanningReviewList.AddRange(ERMService.GetProductionPlanningReview_V2400(PlantName,  //[pallavi jadhav] [GEOS2-4481] [26 05 2023] 
                        // DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),
                        // DateTime.ParseExact(ToDate, "dd/MM/yyyy", null), GeosAppSettingList));

                        //ProductionPlanningReviewList.AddRange(ERMService.GetProductionPlanningReview_V2410(PlantName,  //[pallavi jadhav] [GEOS2-4638] [04 07 2023] 
                        //DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),
                        //DateTime.ParseExact(ToDate, "dd/MM/yyyy", null), GeosAppSettingList));
                        //[rupali sarode][28/07/2023]
                        //ProductionPlanningReviewList.AddRange(ERMService.GetProductionPlanningReview_V2420(PlantName,  //[pallavi jadhav] [GEOS2-4638] [04 07 2023] 
                        //DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),
                        //DateTime.ParseExact(ToDate, "dd/MM/yyyy", null), GeosAppSettingList));
                        //[GEOS2-5097][gulab lakade][04 12 2023]
                        // ProductionPlanningReviewList.AddRange(ERMService.GetProductionPlanningReview_V2460(PlantName,  //[pallavi jadhav] [GEOS2-4638] [04 07 2023] 
                        //DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),
                        //DateTime.ParseExact(ToDate, "dd/MM/yyyy", null), GeosAppSettingList));

                        //   ProductionPlanningReviewList.AddRange(ERMService.GetProductionPlanningReview_V2540(PlantName,  //[pallavi jadhav] [GEOS2-5907] [18 07 2024] 
                        //DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),
                        //DateTime.ParseExact(ToDate, "dd/MM/yyyy", null), GeosAppSettingList));


                        #region [GEOS2-5319][gulab lakade][19 11 2024]

                        //      ProductionPlanningReviewList.AddRange(ERMService.GetProductionPlanningReview_V2580(PlantName,  //[pallavi jadhav] [GEOS2-5907] [18 07 2024] 
                        //DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),
                        //DateTime.ParseExact(ToDate, "dd/MM/yyyy", null), GeosAppSettingList));
                        string ActivePlantString = ActivePlantList.Select(a => a.DefaultValue).FirstOrDefault();
                        List<string> TempActivePlantlist = new List<string>();
                        if (ActivePlantString != null)
                        {
                            TempActivePlantlist = ActivePlantString.Split(',').ToList();
                        }
                        if (TempActivePlantlist != null && TempActivePlantlist.Contains(Convert.ToString(IDCompany)) == true)
                        {
                            ProductionPlanningReviewList.AddRange(ERMService.GetProductionPlanningReviewByStage_V2580(PlantName,
                     DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),
                     DateTime.ParseExact(ToDate, "dd/MM/yyyy", null), GeosAppSettingList));
                        }
                        else
                        {
                            ProductionPlanningReviewList.AddRange(ERMService.GetProductionPlanningReview_V2580(PlantName,
                     DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),
                     DateTime.ParseExact(ToDate, "dd/MM/yyyy", null), GeosAppSettingList));
                        }

                        #endregion

                        // SumOfQTY = ProductionPlanningReviewList.Sum(x => Convert.ToInt32(x.QTY));
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
                        System.Threading.Thread.Sleep(1000);
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
                        System.Threading.Thread.Sleep(1000);
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
                        System.Threading.Thread.Sleep(1000);
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", PlantName, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                    }
                }
                //GridControl gridControl = ((DevExpress.Xpf.Grid.GridControl)obj);
                //DevExpress.Xpf.Grid.GridSummaryItem summ = (DevExpress.Xpf.Grid.GridSummaryItem)gridControl.TotalSummary[0];

                //[GEOS2-4649][rupali sarode][10-07-2023]
                //GeosApplication.Instance.SplashScreenMessage = string.Empty;
                //if (ERMCommon.Instance.FailedPlants == null || ERMCommon.Instance.FailedPlants.Count == 0)
                //{
                //    ERMCommon.Instance.IsShowFailedPlantWarning = false;
                //    ERMCommon.Instance.WarningFailedPlants = string.Empty;
                //}

                #region [GEOS2-4545][gulab lakade][06 05 2023] 
                OriginalonPlanningReviewList = new List<ProductionPlanningReview>();
                OriginalonPlanningReviewList.AddRange(ProductionPlanningReviewList);
                //GroupedData = new List<ProductionPlanningReview>();
                //GroupedData = ProductionPlanningReviewList
                //                                       .GroupBy(a => new { a.OTCode, a.CurrentWorkStation })
                //                                       .Select(g => new ProductionPlanningReview
                //                                       {
                //                                           OTCode = g.Key.OTCode,
                //                                           QTY = g.Sum(a => a.QTY),
                //                                           DeliveryDate = g.FirstOrDefault()?.DeliveryDate,
                //                                           DeliveryDateHtmlColor = g.FirstOrDefault()?.DeliveryDateHtmlColor,
                //                                           DeliveryWeek = g.FirstOrDefault()?.DeliveryWeek,
                //                                           OriginalPlant = g.FirstOrDefault()?.OriginalPlant,
                //                                           ProductionPlant = g.FirstOrDefault()?.ProductionPlant,
                //                                           Template = g.FirstOrDefault()?.Template,
                //                                           Type = g.FirstOrDefault()?.Type,
                //                                           NumItem = g.FirstOrDefault()?.NumItem,
                //                                           CurrentWorkStation = g.Key.CurrentWorkStation,
                //                                           PlanningDeliveryDate = g.FirstOrDefault()?.PlanningDeliveryDate
                //                                       }).ToList();
                //ProductionPlanningReviewList = GroupedData;
                ProductionPlanningReviewListCopy = new List<ProductionPlanningReview>();
                ProductionPlanningReviewListCopy = ProductionPlanningReviewList;
                #endregion
                FillDeliveryweek();
                //summ.DisplayFormat = " {0}";


                //            TotalSummary = new ObservableCollection<Summary>() {
                //    //TotalSummary.Add(
                //        new Summary() { Type = SummaryItemType.Sum, FieldName = "QTY" , DisplayFormat = "{0}" }
                //        //)
                //};

                //[GEOS2-4649][rupali sarode][10-07-2023]
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                if (ERMCommon.Instance.FailedPlants == null || ERMCommon.Instance.FailedPlants.Count == 0)
                {
                    ERMCommon.Instance.IsShowFailedPlantWarning = false;
                    ERMCommon.Instance.WarningFailedPlants = string.Empty;
                }

                GeosApplication.Instance.Logger.Log("Method FillPlanningDateGridView() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillPlanningDateGridView()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
        private void ShowPlanningDateGridView(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowPlanningDateGridView ...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                //[Rupali Sarode][11-04-2023][To display Do want to save message]
                if (IsSaveEnabled == true)
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); } //[GEOS2-4636][rupali sarode][19-07-2023]
                    MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["ArticleGridUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        SavePlanningDateCommandAction();
                    }
                    else
                    {
                        PlanningDateReviewList = new List<PlanningDateReview>();
                        IsSaveEnabled = false;
                        PlanningAppointment.IsSaveButtonEnabled = false;
                    }
                }

                IsBand = false;
                IsGrid = true;
                IsGridViewVisible = Visibility.Visible;
                IsSchedulerViewVisible = Visibility.Hidden;
                //start remove column chooser
                //DevExpress.Xpf.Grid.GridControl tempGridcontrol = (DevExpress.Xpf.Grid.GridControl)obj;
                //int visibleFalseCoulumn = 0;
                //if (File.Exists(PendingPlanningDateReviewGridSettingFilePath))
                //{
                //    tempGridcontrol.RestoreLayoutFromXml(PendingPlanningDateReviewGridSettingFilePath);
                //    GridControl GridControlSTDetails = tempGridcontrol;
                //    TableView tableView = (TableView)GridControlSTDetails.View;
                //    if (tableView.SearchString != null)
                //    {
                //        tableView.SearchString = null;
                //    }
                //}

                //tempGridcontrol.AddHandler(DXSerializer.AllowPropertyEvent, new AllowPropertyEventHandler(OnAllowProperty));

                ////This code for save grid layout.
                //tempGridcontrol.SaveLayoutToXml(PendingPlanningDateReviewGridSettingFilePath);

                //GridControl gridControl = tempGridcontrol;
                //foreach (GridColumn column in gridControl.Columns)
                //{
                //    DependencyPropertyDescriptor descriptor = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleProperty, typeof(GridColumn));
                //    if (descriptor != null)
                //    {
                //        descriptor.AddValueChanged(column, VisibleChanged);
                //    }

                //    DependencyPropertyDescriptor descriptorColumnPosition = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleIndexProperty, typeof(GridColumn));
                //    if (descriptorColumnPosition != null)
                //    {
                //        descriptorColumnPosition.AddValueChanged(column, VisibleIndexChanged);
                //    }

                //    if (column.Visible == false)
                //    {
                //        visibleFalseCoulumn++;
                //    }
                //}
                //end remove column chooser
                // FillERMDataInObjectsByCallingLatestServiceMethods.ShowPleaseWaitScreen();
                List<GeosAppSetting> PendingPOColorList = WorkbenchStartUp.GetSelectedGeosAppSettings("2,3,4,5");
                //  IsHolidayDate = Visibility.Collapsed;


                FillERMDataInObjectsByCallingLatestServiceMethods.GetERMDataOnceFromServiceForAttendance(
               ref ERMService, ref CrmStartUp,
                 ref companyHolidays,
               ref holidayList, ref labelItems, ref PendingPOColorList,
               ref appointmentItems, ref fromDate, ref toDate, ref appointmentItems
                );

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method ShowPlanningDateGridView() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ShowPlanningDateGridView()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ShowSchedulerViewCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowSchedulerViewCommandAction ...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                //[Rupali Sarode][11-04-2023][To display Do want to save message]
                if (PlanningDateReviewCellEditHelper.IsValueChanged)
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); } //[GEOS2-4636][rupali sarode][19-07-2023]
                    MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["ArticleGridUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        SaveGridPlanningDateCommandAction(PlanningDateReviewCellEditHelper.Viewtableview);
                    }
                    PlanningDateReviewCellEditHelper.IsValueChanged = false;
                }

                IsBand = true;
                IsGrid = false;
                IsSchedulerViewVisible = Visibility.Visible;
                ///*   AttendanceFilterAttendanceIsEnabled = true;
                //   AttendanceFilterAttendanceVisible = Visibility.Visible;
                //   AttendanceFilterAttendanceWidth = 50; */
                //PlanningDateReviewList = new List<PlanningDateReview>();
                //// ProductionPlanningReviewListCopy = new List<ProductionPlanningReview>();
                //ProductionPlanningReviewList.AddRange(ProductionPlanningReviewListCopy);
                //// PlanningDateReviewList = ProductionPlanningReviewListCopy;
                //FillDeliveryweek();
                IsGridViewVisible = Visibility.Hidden;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method ShowSchedulerViewCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ShowSchedulerViewCommandAction()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void GetRecordbyDeliveryDate()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GetRecordbyDeliveryDate()...", category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                if (IsBand)
                {
                    List<ProductionPlanningReview> tempProductionPlanningReviewList = new List<ProductionPlanningReview>();
                    if (IsSaveEnabled == true)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); } //[GEOS2-4636][rupali sarode][19-07-2023]
                        MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["ArticleGridUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                        if (MessageBoxResult == MessageBoxResult.Yes)
                        {
                            SavePlanningDateCommandAction();
                        }
                        else
                        {
                            PlanningDateReviewList = new List<PlanningDateReview>();
                            IsSaveEnabled = false;
                            PlanningAppointment.IsSaveButtonEnabled = false;
                        }
                    }

                    //if (SelectedItem == null)
                    //{
                    //    tempProductionPlanningReviewList = ProductionPlanningReviewList.ToList();
                    //    //var TempDeliveryDate = tempProductionPlanningReviewList.OrderBy(b => b.DeliveryDate).ToList();
                    //    //DateTime tempFromDate = Convert.ToDateTime(TempDeliveryDate.Select(a => a.DeliveryDate).FirstOrDefault());
                    //    //AccordianFromDate = tempFromDate.ToString("dd/MM/yyyy");
                    //    //IsAccordianFromDate = true;
                    //}
                    //else
                    //{
                    //SelectedItem = new object();
                    if (SelectedItem != null)
                    {
                        if (SelectedItem.ToString().Contains("All"))
                        {
                            tempProductionPlanningReviewList = ProductionPlanningReviewList.ToList();
                            var TempDeliveryDate = ProductionPlanningReviewListCopy.OrderBy(b => b.DeliveryDate).ToList();
                            DateTime tempFromDate = Convert.ToDateTime(TempDeliveryDate.Select(a => a.DeliveryDate).FirstOrDefault());
                            AccordianFromDate = tempFromDate.ToString("dd/MM/yyyy");
                            IsAccordianFromDate = true;
                            IsPeriod = true;

                        }

                        else
                        {
                            if (SelectedItem is PlanningDateAccordian)
                            {
                                var tempDeliveryWeek = (PlanningDateAccordian)SelectedItem;
                                tempProductionPlanningReviewList = ProductionPlanningReviewListCopy.Where(x => x.DeliveryWeek == Convert.ToString(tempDeliveryWeek.copyDeliveryWeek)).ToList();
                                DateTime tempFromDate = Convert.ToDateTime(tempProductionPlanningReviewList.Select(a => a.DeliveryDate).FirstOrDefault());
                                AccordianFromDate = tempFromDate.ToString("dd/MM/yyyy");
                                IsAccordianFromDate = true;
                            }

                            if (SelectedItem is PlanningDeliveryDate)
                            {
                                PlanningDeliveryDate selectedDeliveryDate = new PlanningDeliveryDate();
                                selectedDeliveryDate = (PlanningDeliveryDate)SelectedItem;
                                // List<ProductionPlanningReview> NewProductionPlanningReviewList = new List<ProductionPlanningReview>();
                                //tempProductionPlanningReviewList= ProductionPlanningReviewList.Where(x => x.DeliveryDate.Value.ToString().Contains(selectedDeliveryDate.copyDeliveryDate)).ToList();
                                // tempProductionPlanningReviewList = 
                                // tempProductionPlanningReviewList = ProductionPlanningReviewList.Where(x => x.PlanningDeliveryDate != null && x.PlanningDeliveryDate.Value.ToString().Contains(selectedDeliveryDate.copyDeliveryDate)).ToList();
                                // tempProductionPlanningReviewList.AddRange(ProductionPlanningReviewList.Where(x => x.PlanningDeliveryDate != null && x.PlanningDeliveryDate.Value.ToString().Contains(selectedDeliveryDate.copyDeliveryDate)).ToList());

                                tempProductionPlanningReviewList = ProductionPlanningReviewListCopy.Where(a => (a.PlanningDeliveryDate != null ? a.PlanningDeliveryDate.Value.ToString() : a.DeliveryDate.Value.ToString()).Contains(selectedDeliveryDate.copyDeliveryDate)).ToList();
                                //var tempdateByPlanningDeliveryDate = (from dw in ProductionPlanningReviewList
                                //                                      where (dw.PlanningDeliveryDate != null ? dw.PlanningDeliveryDate.Value.ToString() : dw.DeliveryDate.Value.ToString()).Contains(selectedDeliveryDate.copyDeliveryDate)
                                //                                      select new
                                //                                      {
                                //                                          dw
                                //                                      }
                                //                      ).ToList();
                                //foreach(var item in tempdateByPlanningDeliveryDate)
                                //{
                                //    tempProductionPlanningReviewList.Add(item.);
                                //}

                                // tempProductionPlanningReviewList.AddRange(tempdateByPlanningDeliveryDate);
                                DateTime tempFromDate = Convert.ToDateTime(tempProductionPlanningReviewList.Select(a => a.DeliveryDate).FirstOrDefault());

                                //AccordianFromDate = tempFromDate.ToString("dd/MM/yyyy");
                                //[Rupali Sarode][31-03-2023]
                                AccordianFromDate = Convert.ToDateTime(selectedDeliveryDate.DeliveryDate).ToShortDateString();
                                IsAccordianFromDate = true;
                            }
                            IsPeriod = true;
                        }
                    }
                    //}


                    AppointmentItems = new CustomObservableCollection<PlanningAppointment>();
                    foreach (var item in tempProductionPlanningReviewList)
                    {
                        UI.Helper.PlanningAppointment modelActivity = new UI.Helper.PlanningAppointment();
                        if (item.PlanningDeliveryDate != null)
                        {
                            modelActivity.StartDate = item.PlanningDeliveryDate.Value;
                            modelActivity.EndDate = item.PlanningDeliveryDate.Value.AddMinutes(30);
                        }
                        else
                        {
                            modelActivity.StartDate = item.DeliveryDate.Value;
                            modelActivity.EndDate = item.DeliveryDate.Value.AddMinutes(30);
                        }

                        if (item.PlanningDeliveryDate != null)
                        {

                            if (item.DeliveryDate.Value.Date <= GeosApplication.Instance.ServerDateTime.Date)
                                modelActivity.Label = Convert.ToInt32(Labels[0].Id);
                            else if (item.DeliveryDate.Value.Date > GeosApplication.Instance.ServerDateTime.Date && item.DeliveryDate.Value.Date <= GeosApplication.Instance.ServerDateTime.Date.AddDays(6))
                                modelActivity.Label = Convert.ToInt32(Labels[1].Id);
                            else if (item.DeliveryDate.Value.Date >= GeosApplication.Instance.ServerDateTime.Date.AddDays(7))
                                modelActivity.Label = Convert.ToInt32(Labels[2].Id);
                            //if (item.PlanningDeliveryDate.Value.Date < GeosApplication.Instance.ServerDateTime.Date)
                            //    modelActivity.Label = Convert.ToInt32(Labels[0].Id);
                            //else if (item.PlanningDeliveryDate.Value.Date == GeosApplication.Instance.ServerDateTime.Date)
                            //    modelActivity.Label = Convert.ToInt32(Labels[1].Id);
                            //else if (item.PlanningDeliveryDate.Value.Date > GeosApplication.Instance.ServerDateTime.Date)
                            //{
                            //    if (item.PlanningDeliveryDate.Value.Date >= GeosApplication.Instance.ServerDateTime.Date.AddDays(7))
                            //        modelActivity.Label = Convert.ToInt32(Labels[3].Id);
                            //    else
                            //        modelActivity.Label = Convert.ToInt32(Labels[2].Id);
                            //}
                        }
                        else
                        {
                            if (item.DeliveryDate.Value.Date <= GeosApplication.Instance.ServerDateTime.Date)
                                modelActivity.Label = Convert.ToInt32(Labels[0].Id);
                            else if (item.DeliveryDate.Value.Date > GeosApplication.Instance.ServerDateTime.Date && item.DeliveryDate.Value.Date <= GeosApplication.Instance.ServerDateTime.Date.AddDays(6))
                                modelActivity.Label = Convert.ToInt32(Labels[1].Id);
                            else if (item.DeliveryDate.Value.Date >= GeosApplication.Instance.ServerDateTime.Date.AddDays(7))
                                modelActivity.Label = Convert.ToInt32(Labels[2].Id);
                            //if (item.DeliveryDate.Value.Date < GeosApplication.Instance.ServerDateTime.Date)
                            //    modelActivity.Label = Convert.ToInt32(Labels[0].Id);
                            //else if (item.DeliveryDate.Value.Date == GeosApplication.Instance.ServerDateTime.Date)
                            //    modelActivity.Label = Convert.ToInt32(Labels[1].Id);
                            //else if (item.DeliveryDate.Value.Date > GeosApplication.Instance.ServerDateTime.Date)
                            //{
                            //    if (item.DeliveryDate.Value.Date >= GeosApplication.Instance.ServerDateTime.Date.AddDays(7))
                            //        modelActivity.Label = Convert.ToInt32(Labels[3].Id);
                            //    else
                            //        modelActivity.Label = Convert.ToInt32(Labels[2].Id);
                            //}
                        }
                        modelActivity.IsHolidayData = Visibility.Collapsed;
                        modelActivity.IsHolidayDate = Visibility.Visible;
                        // modelActivity.Label = item.IdTemplate;
                        modelActivity.Subject = item.OTCode;
                        //modelActivity.Description = item.Type;
                        modelActivity.DeliveryDate = item.DeliveryDate;
                        modelActivity.Template = item.Template;
                        modelActivity.NumItem = item.NumItem;
                        modelActivity.QTY = item.QTY;
                        modelActivity.OTCode = item.OTCode;
                        modelActivity.Type = item.Type;
                        modelActivity.OriginPlant = item.OriginalPlant;
                        modelActivity.ContentSubject = item.CurrentWorkStation + ";" + item.OTCode + "; " + item.NumItem + "; " + item.Type + ";" + item.QTY;
                        //modelActivity.Status = Convert.ToInt32(item.IdCPType);
                        modelActivity.CurrentWorkStation = item.CurrentWorkStation;//[pallavi jadhav] [GEOS2-4481] [26 05 2023]
                        #region //[pallavi jadhav] [GEOS2-4519] [06 06 2023]
                        var tempReal = item.PlanningDateReviewStages.Select(a => a.Real).ToList();  // Convert the LINQ query result to a list
                        if (tempReal != null)
                        {
                            TimeSpan Temreal = TimeSpan.FromSeconds(tempReal.Sum(d => d.HasValue ? (double)d.Value : 0));
                            modelActivity.Real = Temreal.ToString();
                        }

                        TimeSpan TemExpected = TimeSpan.Parse("0");
                        var tempExpected = item.PlanningDateReviewStages.Select(a => a.Expected).ToList();
                        if (tempExpected != null)
                        {
                            TemExpected = TimeSpan.FromMinutes(tempExpected.Sum(d => d.HasValue ? (double)d.Value : 0));
                            // TemExpected = TimeSpan.FromSeconds(tempExpected.Sum(d => (double)d));
                            modelActivity.Expected = TemExpected.ToString();
                        }
                        #endregion
                        modelActivity.PlanningDeliveryDate = item.PlanningDeliveryDate;
                        if (item.PlanningDeliveryDate != null)
                        {
                            if (item.DeliveryDate != item.PlanningDeliveryDate)
                            {
                                modelActivity.IsPlannedDeliveryDate = true;
                            }
                            else
                            {
                                modelActivity.IsPlannedDeliveryDate = false;
                            }
                            modelActivity.PlanningDeliveryDate = item.PlanningDeliveryDate;
                        }
                        else
                        {
                            modelActivity.IsPlannedDeliveryDate = false;
                        }
                        if (item.LastUpdateDate != null)
                        {
                            modelActivity.LastUpdate = item.LastUpdateDate;
                        }

                        #region [Rupali Sarode][GEOS2-4161][21-2-2023]
                        modelActivity.IdOT = Convert.ToUInt32(item.IdOt);
                        modelActivity.IdCounterpart = Convert.ToUInt32(item.IdCounterpart);
                        #endregion

                        #region [Rupali Sarode][GEOS2-4347][05-05-2023]
                        modelActivity.Customer = item.Customer;
                        modelActivity.Project = item.Project;
                        #endregion

                        #region [pallavi jadhav][GEOS2-5319][22-10-2024]

                        modelActivity.IdCounterpart = Convert.ToUInt32(item.IdCounterpart);
                        modelActivity.IdSite = Convert.ToInt32(item.IdProductionPlant);//[gulab lakade][15 11 2024]
                        #endregion

                        AppointmentItems.Add(modelActivity);

                    }
                }
                if (IsGrid)
                {
                    if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                    if (ProductionPlanningReviewList.Count == 0)
                    {
                        ProductionPlanningReviewList = new List<ProductionPlanningReview>();
                        ProductionPlanningReviewList.AddRange(ProductionPlanningReviewListCopy);
                    }

                    //TimeTrackingList = ERMService.GetAllTimeTracking_V2330();
                    if (ProductionPlanningReviewList.Count > 0)
                    {

                        var currentculter = CultureInfo.CurrentCulture;
                        ProductionPlanningReview ProductionPlanningReview = new ProductionPlanningReview();
                        ProductionPlanningReview = ProductionPlanningReviewList.FirstOrDefault();
                        // SelectedItem = new object();
                        #region GEOS2-4045 Gulab lakade
                        if (SelectedItem != null)
                        {
                            if (SelectedItem.ToString().Contains("CW"))
                            {
                                string tempselectItem = Convert.ToString(SelectedItem);
                                int index = tempselectItem.LastIndexOf("(");
                                if (index > 0)
                                    tempselectItem = tempselectItem.Substring(0, index);

                                ProductionPlanningReviewList = ProductionPlanningReviewListCopy.Where(x => x.DeliveryWeek.ToString().Contains(tempselectItem.Trim())).ToList();
                            }
                            else if (SelectedItem.ToString().Contains("All"))
                            {
                                ProductionPlanningReviewList = ProductionPlanningReviewListCopy.ToList();
                            }
                            else
                            if (!string.IsNullOrEmpty(SelectedItem.ToString()))
                            {
                                int tempTimeTrackingcount = 0;
                                List<ProductionPlanningReview> productionPlanningReviewTemp = new List<ProductionPlanningReview>();
                                tempTimeTrackingcount = ProductionPlanningReviewListCopy.Where(x => x.PlanningDeliveryDate != null && x.PlanningDeliveryDate.Value.ToString().Contains(SelectedItem.ToString())).ToList().Count();
                                if (tempTimeTrackingcount != 0)
                                {
                                    productionPlanningReviewTemp = ProductionPlanningReviewListCopy.Where(x => x.PlanningDeliveryDate != null && x.PlanningDeliveryDate.Value.ToString().Contains(SelectedItem.ToString()) && !x.DeliveryDate.Value.ToString().Contains(SelectedItem.ToString())).ToList();
                                    productionPlanningReviewTemp.AddRange(ProductionPlanningReviewListCopy.Where(x => x.DeliveryDate.Value.ToString().Contains(SelectedItem.ToString())).ToList());
                                    ProductionPlanningReviewList = productionPlanningReviewTemp;
                                    //ProductionPlanningReviewList = new List<ProductionPlanningReview>();
                                    //ProductionPlanningReviewList.AddRange(productionPlanningReviewTemp);
                                }
                                else
                                {
                                    //    ProductionPlanningReviewList = new List<ProductionPlanningReview>();
                                    ProductionPlanningReviewList = ProductionPlanningReviewListCopy.Where(x => x.DeliveryDate.Value.ToString().Contains(SelectedItem.ToString())).ToList();
                                }
                            }
                            #endregion
                            //SaveGridPlanningDateCommandAction(null);
                            ////AddColumnsToDataTableWithoutBands();
                            ////FillProductionPlanningReview();
                            //ProductionPlanningReviewList = new List<ProductionPlanningReview>();
                            //ProductionPlanningReviewList.AddRange(ProductionPlanningReviewListCopy);
                            //ProductionPlanningReviewList = ProductionPlanningReviewListCopy;

                            //SumOfQTY = 0;
                            //SumOfQTY = ProductionPlanningReviewList.Sum(x => Convert.ToInt32(x.QTY));

                            //GridControl gridControl = ((DevExpress.Xpf.Grid.GridControl)obj);
                            //DevExpress.Xpf.Grid.GridSummaryItem summ = (DevExpress.Xpf.Grid.GridSummaryItem)gridControl.TotalSummary[0];
                            //summ.DisplayFormat = " {0}" + SumOfQTY;
                        }
                    }

                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method GetRecordbyDeliveryDate()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method GetRecordbyDeliveryDate() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ChangePlantCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChangePlantCommandAction()...", category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                if (IsBand)
                {
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

                            if (ERMCommon.Instance.SelectedAuthorizedPlantsList != null)
                            {
                                try
                                {
                                    GetPlanningDateRecordByPlantAndDate(ERMCommon.Instance.SelectedAuthorizedPlantsList, FromDate, ToDate);
                                    List<ProductionPlanningReview> ProductionPlanningReviewListTemp = new List<ProductionPlanningReview>();
                                    ProductionPlanningReviewListTemp = ProductionPlanningReviewList;
                                    ProductionPlanningReviewList = new List<ProductionPlanningReview>();
                                    ProductionPlanningReviewList = ProductionPlanningReviewListTemp;
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
                }
                else if (IsGrid)
                {
                    try
                    {
                        GetPlanningDateRecordByPlantAndDate(ERMCommon.Instance.SelectedAuthorizedPlantsList, FromDate, ToDate);
                        List<ProductionPlanningReview> ProductionPlanningReviewListTemp = new List<ProductionPlanningReview>();
                        ProductionPlanningReviewListTemp = ProductionPlanningReviewList;
                        ProductionPlanningReviewList = new List<ProductionPlanningReview>();
                        ProductionPlanningReviewList = ProductionPlanningReviewListTemp;
                        //ShowPlanningDateGridView(obj);
                    }
                    catch (Exception ex)
                    {

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
        private void GetPlanningDateRecordByPlantAndDate(List<object> SelectedPlant, string FromDate, string ToDate)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GetPlanningDateRecordByPlantAndDate()...", category: Category.Info, priority: Priority.Low);

                ProductionPlanningReviewList = new List<ProductionPlanningReview>();
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                List<ProductionPlanningReview> ProductionPlanningReviewListtemp = new List<ProductionPlanningReview>();
                ProductionPlanningReviewListtemp = new List<ProductionPlanningReview>();
                CustomObservableCollection<Holidays> companyHolidaysList = new CustomObservableCollection<Holidays>();
                // FillERMDataInObjectsByCallingLatestServiceMethods.ShowPleaseWaitScreen();
                companyHolidays = new CustomObservableCollection<Holidays>();
                ERMCommon.Instance.FailedPlants = new List<string>();
                ERMCommon.Instance.IsShowFailedPlantWarning = false;
                ERMCommon.Instance.WarningFailedPlants = string.Empty;
                foreach (Site item in SelectedPlant)
                {
                    string PlantName = Convert.ToString(item.Name);
                    try
                    {
                        IDCompany = item.IdSite;
                        string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.Name == item.Name).Select(xy => xy.ServiceProviderUrl).FirstOrDefault();

                        ERMService = new ERMServiceController(serviceurl);
                        //ProductionPlanningReviewList.AddRange(ERMService.GetProductionPlanningReview_V2360(PlantName,
                        //    DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),
                        //        DateTime.ParseExact(ToDate, "dd/MM/yyyy", null)));
                        //ERMService = new ERMServiceController("localhost:6699");
                        //ProductionPlanningReviewListtemp.AddRange(ERMService.GetProductionPlanningReview_V2370(PlantName,
                        //       DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),
                        //       DateTime.ParseExact(ToDate, "dd/MM/yyyy", null), GeosAppSettingList));
                        //ProductionPlanningReviewListtemp.AddRange(ERMService.GetProductionPlanningReview_V2380(PlantName,
                        //      DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),
                        //      DateTime.ParseExact(ToDate, "dd/MM/yyyy", null), GeosAppSettingList));
                        //ProductionPlanningReviewListtemp.AddRange(ERMService.GetProductionPlanningReview_V2400(PlantName,  //[pallavi jadhav] [GEOS2-4481] [26 05 2023] 
                        //     DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),
                        //     DateTime.ParseExact(ToDate, "dd/MM/yyyy", null), GeosAppSettingList));
                        //  ERMService = new ERMServiceController("localhost:6699");
                        //ProductionPlanningReviewList.AddRange(ERMService.GetProductionPlanningReview_V2410(PlantName,  //[pallavi jadhav] [GEOS2-4638] [04 07 2023] 
                        //DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),
                        //DateTime.ParseExact(ToDate, "dd/MM/yyyy", null), GeosAppSettingList));

                        //[rupali sarode][28/07/2023]
                        //ProductionPlanningReviewList.AddRange(ERMService.GetProductionPlanningReview_V2420(PlantName,  //[pallavi jadhav] [GEOS2-4638] [04 07 2023] 
                        //DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),
                        //DateTime.ParseExact(ToDate, "dd/MM/yyyy", null), GeosAppSettingList));
                        ///[GEOS2-5097][gulab lakade][04 12 2023]
                        //ProductionPlanningReviewList.AddRange(ERMService.GetProductionPlanningReview_V2460(PlantName,  //[pallavi jadhav] [GEOS2-4638] [04 07 2023] 
                        //DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),
                        //DateTime.ParseExact(ToDate, "dd/MM/yyyy", null), GeosAppSettingList));
                        //    ProductionPlanningReviewList.AddRange(ERMService.GetProductionPlanningReview_V2540(PlantName,  //[pallavi jadhav] [GEOS2-5907] [18 07 2024] 
                        //DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),
                        //DateTime.ParseExact(ToDate, "dd/MM/yyyy", null), GeosAppSettingList));
                        //ERMService = new ERMServiceController("localhost:6699");

                        #region [GEOS2-5319][gulab lakade][19 11 2024]
                        //           ProductionPlanningReviewList.AddRange(ERMService.GetProductionPlanningReview_V2580(PlantName,  //[pallavi jadhav] [GEOS2-5907] [18 07 2024] 
                        //DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),
                        //DateTime.ParseExact(ToDate, "dd/MM/yyyy", null), GeosAppSettingList));

                        string ActivePlantString = ActivePlantList.Select(a => a.DefaultValue).FirstOrDefault();
                        List<string> TempActivePlantlist = new List<string>();
                        if (ActivePlantString != null)
                        {
                            TempActivePlantlist = ActivePlantString.Split(',').ToList();
                        }
                        if (TempActivePlantlist != null && TempActivePlantlist.Contains(Convert.ToString(IDCompany)) == true)
                        {
                            ProductionPlanningReviewList.AddRange(ERMService.GetProductionPlanningReviewByStage_V2580(PlantName,
                     DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),
                     DateTime.ParseExact(ToDate, "dd/MM/yyyy", null), GeosAppSettingList));
                        }
                        else
                        {
                            ProductionPlanningReviewList.AddRange(ERMService.GetProductionPlanningReview_V2580(PlantName,
                     DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),
                     DateTime.ParseExact(ToDate, "dd/MM/yyyy", null), GeosAppSettingList));
                        }

                        #endregion
                        //string serviceurlholidays = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.Name == PlantAlise).Select(xy => xy.ServiceProviderUrl).FirstOrDefault();
                        //ERMService = new ERMServiceController(serviceurlholidays);
                        //    ERMService = new ERMServiceController("localhost:6699");
                        string PlantAlise = Convert.ToString(item.IdSite);
                        // DateTime tempFromyear = DateTime.Parse(FromDate.ToString());//[GEOS2-5319][gulab lakade][13 11 2024]
                        DateTime tempFromyear = DateTime.ParseExact(FromDate, "dd/MM/yyyy", null);//[GEOS2-5319][gulab lakade][13 11 2024]
                        string year = Convert.ToString(tempFromyear.Year);

                        companyHolidays.AddRangeWithTemporarySuppressedNotification(ERMService.GetCompanyHolidaysBySelectedIdCompany(PlantAlise, year));

                        //CompanyHolidaysCopy = new CustomObservableCollection<Holidays>();
                        //CompanyHolidaysCopy = companyHolidays;
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
                        System.Threading.Thread.Sleep(1000);
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
                        System.Threading.Thread.Sleep(1000);
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
                        System.Threading.Thread.Sleep(1000);
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", PlantName, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                    }
                }

                //[GEOS2-4649][rupali sarode][10-07-2023]
                //GeosApplication.Instance.SplashScreenMessage = string.Empty;
                //if (ERMCommon.Instance.FailedPlants == null || ERMCommon.Instance.FailedPlants.Count == 0)
                //{
                //    ERMCommon.Instance.IsShowFailedPlantWarning = false;
                //    ERMCommon.Instance.WarningFailedPlants = string.Empty;
                //}

                //List<Company> plantOwners = GeosApplication.Instance.SelectedPlantOwnerUsersList.Cast<Company>().ToList();
                //var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.ShortName.Contains(SelectedPlant.ToString())));

                //foreach (Company item1 in plantOwners)
                //{
                //    string PlantAlise = Convert.ToString(item1.Alias);
                //    string serviceurlholidays = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.Name == item1.Alias).Select(xy => xy.ServiceProviderUrl).FirstOrDefault();
                //    ERMService = new ERMServiceController(serviceurlholidays);
                //ERMService = new ERMServiceController("localhost:6699");
                //    companyHolidaysList.AddRangeWithTemporarySuppressedNotification(ERMService.GetCompanyHolidaysBySelectedIdCompany(PlantAlise));
                //}
                // ProductionPlanningReviewList = ProductionPlanningReviewListtemp;
                OriginalonPlanningReviewList = new List<ProductionPlanningReview>();
                OriginalonPlanningReviewList = ProductionPlanningReviewList;


                #region [GEOS2-4545][gulab lakade][06 05 2023]


                //GroupedData = new List<ProductionPlanningReview>();
                //GroupedData = ProductionPlanningReviewList
                //                                         .GroupBy(a => new { a.OTCode,a.NumItem })
                //                                         .Select(g => new ProductionPlanningReview
                //                                         {
                //                                             OTCode = g.Key.OTCode,
                //                                             QTY = g.Sum(a => a.QTY),
                //                                             DeliveryDate = g.FirstOrDefault()?.DeliveryDate,
                //                                             DeliveryDateHtmlColor = g.FirstOrDefault()?.DeliveryDateHtmlColor,
                //                                             DeliveryWeek = g.FirstOrDefault()?.DeliveryWeek,
                //                                             OriginalPlant = g.FirstOrDefault()?.OriginalPlant,
                //                                             ProductionPlant = g.FirstOrDefault()?.ProductionPlant,
                //                                             Template = g.FirstOrDefault()?.Template,
                //                                             Type = g.FirstOrDefault()?.Type,
                //                                             NumItem = g.FirstOrDefault().NumItem,
                //                                             CurrentWorkStation = g.FirstOrDefault().CurrentWorkStation,
                //                                             PlanningDeliveryDate = g.FirstOrDefault()?.PlanningDeliveryDate
                //                                         }).ToList();

                //ProductionPlanningReviewList = GroupedData;
                //   ProductionPlanningReviewList = new List<ProductionPlanningReview>();
                //ProductionPlanningReviewList.AddRange(OriginalonPlanningReviewList
                //   .Where(i => stageCodes.Contains(i.CurrentWorkStation))
                //   .ToList());

                //ProductionPlanningReviewList = (OriginalonPlanningReviewList
                //   .Where(i => stageCodes.Contains(i.CurrentWorkStation))
                //   .ToList());
                ProductionPlanningReviewListCopy = new List<ProductionPlanningReview>();
                ProductionPlanningReviewListCopy.AddRange(ProductionPlanningReviewList);

                #endregion


                FillDeliveryweek();
                AppointmentItems = new CustomObservableCollection<UI.Helper.PlanningAppointment>();
                #region GEOS2-4156
                if (ProductionPlanningReviewList != null && ProductionPlanningReviewList.Count > 0)
                {
                    foreach (var item in ProductionPlanningReviewList)
                    {
                        UI.Helper.PlanningAppointment modelActivity = new UI.Helper.PlanningAppointment();
                        if (item.PlanningDeliveryDate != null)
                        {
                            modelActivity.StartDate = item.PlanningDeliveryDate.Value;
                            modelActivity.EndDate = item.PlanningDeliveryDate.Value.AddMinutes(30);
                        }
                        else
                        {
                            modelActivity.StartDate = item.DeliveryDate.Value;
                            modelActivity.EndDate = item.DeliveryDate.Value.AddMinutes(30);
                        }

                        if (item.DeliveryDate.Value.Date <= GeosApplication.Instance.ServerDateTime.Date)
                            modelActivity.Label = Convert.ToInt32(Labels[0].Id);
                        else if (item.DeliveryDate.Value.Date > GeosApplication.Instance.ServerDateTime.Date && item.DeliveryDate.Value.Date <= GeosApplication.Instance.ServerDateTime.Date.AddDays(6))
                            modelActivity.Label = Convert.ToInt32(Labels[1].Id);
                        else if (item.DeliveryDate.Value.Date >= GeosApplication.Instance.ServerDateTime.Date.AddDays(7))
                            modelActivity.Label = Convert.ToInt32(Labels[2].Id);

                        //if (item.DeliveryDate.Value.Date < GeosApplication.Instance.ServerDateTime.Date)
                        //    modelActivity.Label = Convert.ToInt32(Labels[0].Id);
                        //else if (item.DeliveryDate.Value.Date == GeosApplication.Instance.ServerDateTime.Date)
                        //    modelActivity.Label = Convert.ToInt32(Labels[1].Id);
                        //else if (item.DeliveryDate.Value.Date > GeosApplication.Instance.ServerDateTime.Date)
                        //{
                        //    if (item.DeliveryDate.Value.Date >= GeosApplication.Instance.ServerDateTime.Date.AddDays(7))
                        //        modelActivity.Label = Convert.ToInt32(Labels[3].Id);
                        //    else
                        //        modelActivity.Label = Convert.ToInt32(Labels[2].Id);
                        //}
                        modelActivity.IsHolidayData = Visibility.Collapsed;
                        modelActivity.IsHolidayDate = Visibility.Visible;
                        modelActivity.Subject = item.OTCode;
                        //modelActivity.Description = item.Type;
                        modelActivity.DeliveryDate = item.DeliveryDate;
                        modelActivity.Template = item.Template;
                        modelActivity.NumItem = item.NumItem;
                        modelActivity.QTY = item.QTY;
                        modelActivity.OTCode = item.OTCode;
                        modelActivity.Type = item.Type;
                        modelActivity.OriginPlant = item.OriginalPlant;
                        modelActivity.ContentSubject = item.CurrentWorkStation + ";" + item.OTCode + "; " + item.NumItem + "; " + item.Type + ";" + item.QTY;
                        //modelActivity.Status = Convert.ToInt32(item.IdCPType);
                        modelActivity.CurrentWorkStation = item.CurrentWorkStation; //[pallavi jadhav] [GEOS2-4481] [26 05 2023] 
                        #region //[pallavi jadhav] [GEOS2-4519] [06 06 2023]
                        var tempReal = item.PlanningDateReviewStages.Select(a => a.Real).ToList();  // Convert the LINQ query result to a list
                        if (tempReal != null)
                        {
                            TimeSpan Temreal = TimeSpan.FromSeconds(tempReal.Sum(d => d.HasValue ? (double)d.Value : 0));
                            modelActivity.Real = Temreal.ToString();
                        }

                        TimeSpan TemExpected = TimeSpan.Parse("0");
                        var tempExpected = item.PlanningDateReviewStages.Select(a => a.Expected).ToList();
                        if (tempExpected != null)
                        {
                            TemExpected = TimeSpan.FromMinutes(tempExpected.Sum(d => d.HasValue ? (double)d.Value : 0));
                            // TemExpected = TimeSpan.FromSeconds(tempExpected.Sum(d => (double)d));
                            modelActivity.Expected = TemExpected.ToString();
                        }
                        #endregion
                        modelActivity.Expected = TemExpected.ToString();

                        modelActivity.PlanningDeliveryDate = item.PlanningDeliveryDate;
                        if (item.PlanningDeliveryDate != null)
                        {
                            if (item.DeliveryDate != item.PlanningDeliveryDate)
                            {
                                modelActivity.IsPlannedDeliveryDate = true;
                            }
                            else
                            {
                                modelActivity.IsPlannedDeliveryDate = false;
                            }
                            modelActivity.PlanningDeliveryDate = item.PlanningDeliveryDate;
                        }
                        else
                        {
                            modelActivity.IsPlannedDeliveryDate = false;
                        }
                        if (item.LastUpdateDate != null)
                        {
                            modelActivity.LastUpdate = item.LastUpdateDate;
                        }
                        #region [Rupali Sarode][GEOS2-4161][21-2-2023]
                        modelActivity.IdOT = Convert.ToUInt32(item.IdOt);
                        modelActivity.IdCounterpart = Convert.ToUInt32(item.IdCounterpart);
                        #endregion

                        #region [Rupali Sarode][GEOS2-4347][05-05-2023]
                        modelActivity.Customer = item.Customer;
                        modelActivity.Project = item.Project;
                        #endregion
                        #region [pallavi jadhav][GEOS2-5319][22-10-2024]

                        modelActivity.IdCounterpart = Convert.ToUInt32(item.IdCounterpart);
                        modelActivity.IdSite = Convert.ToInt32(item.IdProductionPlant);//[gulab lakade][15 11 2024]
                        #endregion
                        AppointmentItems.Add(modelActivity);

                    }
                    //  List<GeosAppSetting> PendingPOColorList = WorkbenchStartUp.GetSelectedGeosAppSettings("2,3,4,5");
                    //  //  IsHolidayDate = Visibility.Collapsed;
                    //  FillERMDataInObjectsByCallingLatestServiceMethods.GetERMDataOnceFromServiceForAttendance(
                    //ref ERMService, ref CrmStartUp,
                    //  ref companyHolidays,
                    //ref holidayList, ref labelItems, ref PendingPOColorList,
                    //ref appointmentItems, ref fromDate, ref toDate
                    // );
                    //  FillERMDataInObjectsByCallingLatestServiceMethods.ClosePleaseWaitScreen();
                    ShowCompanyHolidayAppointmentsForSelectedCompany();

                }
                #endregion
                //[GEOS2-4649][rupali sarode][10-07-2023]
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                if (ERMCommon.Instance.FailedPlants == null || ERMCommon.Instance.FailedPlants.Count == 0)
                {
                    ERMCommon.Instance.IsShowFailedPlantWarning = false;
                    ERMCommon.Instance.WarningFailedPlants = string.Empty;
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method GetPlanningDateRecordByPlantAndDate()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method GetPlanningDateRecordByPlantAndDate()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }
        #region Sceduler Command Action
        private void DefaultLoadCommandAction(RoutedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DefaultLoadCommandAction()...", category: Category.Info, priority: Priority.Low);
                PlanningSchedulerControl scheduler = obj.Source as PlanningSchedulerControl;
                if (scheduler.ActiveView.Caption == "Month View")
                    ViewType = 1;
                else
                    ViewType = 2;
                SelectedStartDate = scheduler.SelectedInterval.Start;
                SelectedEndDate = scheduler.SelectedInterval.End;

                if (scheduler.Month != null)
                {
                    if (scheduler.ActiveViewIndex == 0)
                    {
                        scheduler.DisplayName = String.Format("{0:MMMM yyyy}", scheduler.VisibleIntervals[0].End);
                        SelectedEndDate = SelectedEndDate.AddDays(-1);
                    }
                    else
                    {
                        if (DevExpress.XtraScheduler.Native.DateTimeHelper.GetWeekOfYear(scheduler.Start) < 10)
                        {
                            scheduler.DisplayName = String.Format("Week 0{0} of {1:yyyy}", DevExpress.XtraScheduler.Native.DateTimeHelper.GetWeekOfYear(scheduler.Start), scheduler.Start);
                        }
                        else
                        {
                            scheduler.DisplayName = String.Format("Week {0} of {1:yyyy}", DevExpress.XtraScheduler.Native.DateTimeHelper.GetWeekOfYear(scheduler.Start), scheduler.Start);
                        }
                    }
                }

                GeosApplication.Instance.Logger.Log("Method DefaultLoadCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DefaultLoadCommandAction()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void SelectedIntervalCommandAction(MouseButtonEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SelectedIntervalCommandAction()...", category: Category.Info, priority: Priority.Low);
                PlanningSchedulerControl scheduler = e.Source as PlanningSchedulerControl;
                SelectedStartDate = scheduler.SelectedInterval.Start;
                SelectedEndDate = scheduler.SelectedInterval.End;
                if (scheduler.ActiveView.Caption == "Month View")
                    SelectedEndDate = SelectedEndDate.AddDays(-1);
                GeosApplication.Instance.Logger.Log("Method SelectedIntervalCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SelectedIntervalCommandAction()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void VisibleIntervalsChanged(object obj)
        {
            try
            {
                if (IsVisibleChanged == true)
                {
                    IsVisibleChanged = false;
                    return;
                }

                //PlanningSchedulerControl temp = (DevExpress.Xpf.Scheduling.MonthView)obj;
                GeosApplication.Instance.Logger.Log("Method VisibleIntervalsChanged()...", category: Category.Info, priority: Priority.Low);
                var values = (object[])obj;
                PlanningSchedulerControl scheduler = (PlanningSchedulerControl)values[0];
                DateTime tempStartDate = DateTime.ParseExact(FromDate, "dd/MM/yyyy", null);
                DateTime tempEndDate = DateTime.ParseExact(ToDate, "dd/MM/yyyy", null);



                if (IsPeriod == true)
                {
                    if (IsAccordianFromDate == true)
                    {
                        DateTime dateTimeNew = scheduler.Month.Value;
                        DateTime AccordianSelectedDate = DateTime.ParseExact(AccordianFromDate, "dd/MM/yyyy", null);
                        int MonthDeference = ((AccordianSelectedDate.Year - dateTimeNew.Year) * 12) + (Convert.ToInt32(AccordianSelectedDate.Month) - Convert.ToInt32(dateTimeNew.Month));
                        if (MonthDeference != 0)
                        {
                            if (IsSaveEnabled == true)
                            {
                                MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["ArticleGridUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                                if (MessageBoxResult == MessageBoxResult.Yes)
                                {
                                    SavePlanningDateCommandAction();
                                }
                                else
                                {
                                    PlanningDateReviewList = new List<PlanningDateReview>();
                                    IsSaveEnabled = false;
                                    PlanningAppointment.IsSaveButtonEnabled = false;
                                }

                            }
                        }


                        Navigate(scheduler, MonthDeference);
                        IsAccordianFromDate = false;
                    }
                    else
                    {
                        DateTime dateTimeNew = scheduler.Month.Value;
                        int MonthDeference = ((tempStartDate.Year - dateTimeNew.Year) * 12) + (Convert.ToInt32(tempStartDate.Month) - Convert.ToInt32(dateTimeNew.Month));
                        if (MonthDeference != 0)
                        {
                            if (IsSaveEnabled == true)
                            {
                                MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["ArticleGridUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                                if (MessageBoxResult == MessageBoxResult.Yes)
                                {
                                    SavePlanningDateCommandAction();
                                }
                                else
                                {
                                    PlanningDateReviewList = new List<PlanningDateReview>();
                                    IsSaveEnabled = false;
                                    PlanningAppointment.IsSaveButtonEnabled = false;
                                }
                            }
                        }
                        Navigate(scheduler, MonthDeference);
                    }
                    IsPeriod = false;
                }

                SelectedStartDate = scheduler.SelectedInterval.Start;
                SelectedEndDate = scheduler.SelectedInterval.End;
                scheduler.Month = scheduler.VisibleIntervals[0].End;

                if (tempStartDate > scheduler.Month)
                {
                    //scheduler.Month = scheduler.Month.Value.AddMonths(1);
                    scheduler.Month = tempStartDate;
                }
                else if (tempEndDate < scheduler.Month)
                {
                    //scheduler.Month = scheduler.Month.Value.AddMonths(-1);
                    scheduler.Month = tempEndDate;
                }
                if (scheduler.Month != null)
                {
                    if (scheduler.ActiveViewIndex == 0)
                    {
                        scheduler.DisplayName = String.Format("{0:MMMM yyyy}", scheduler.VisibleIntervals[0].End);
                        SelectedEndDate = SelectedEndDate.AddDays(-1);
                    }
                    else
                    {
                        if (DevExpress.XtraScheduler.Native.DateTimeHelper.GetWeekOfYear(scheduler.Start) < 10)
                        {
                            scheduler.DisplayName = String.Format("Week 0{0} of {1:yyyy}", DevExpress.XtraScheduler.Native.DateTimeHelper.GetWeekOfYear(scheduler.Start), scheduler.Start);
                        }
                        else
                        {
                            scheduler.DisplayName = String.Format("Week {0} of {1:yyyy}", DevExpress.XtraScheduler.Native.DateTimeHelper.GetWeekOfYear(scheduler.Start), scheduler.Start);
                        }
                    }
                    //[004][005]
                    DateTime dtStartCalenderInterval = new DateTime(scheduler.Month.Value.Year, scheduler.Month.Value.Month, 1);
                    DateTime dtEndCalenderInterval = dtStartCalenderInterval.AddMonths(1).AddDays(-1);
                }

                GeosApplication.Instance.Logger.Log("Method VisibleIntervalsChanged()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method VisibleIntervalsChanged()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        static void Navigate(PlanningSchedulerControl scheduler, int count)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Navigate()...", category: Category.Info, priority: Priority.Low);

                if (scheduler.ActiveViewIndex == 0)
                {
                    DateTime dateTimeNew = scheduler.Month.Value.AddMonths(count);
                    if (dateTimeNew >= scheduler.LimitInterval.Start &&
                        dateTimeNew <= scheduler.LimitInterval.End)
                    {
                        scheduler.Uid = count.ToString();
                        scheduler.Month = dateTimeNew;
                    }
                }
                else if (scheduler.ActiveViewIndex == 1)
                {
                    DateTime dateTimeNew = scheduler.Start.AddDays(7 * count);
                    if (dateTimeNew >= scheduler.LimitInterval.Start &&
                        dateTimeNew <= scheduler.LimitInterval.End)
                    {
                        scheduler.Start = dateTimeNew;
                    }
                }
                else if (scheduler.ActiveViewIndex == 2)
                {
                    scheduler.Start = scheduler.Start.AddDays(count);
                }
                GeosApplication.Instance.Logger.Log("Method Navigate()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Navigate()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        public void SelectItemForScheduler(object obj)
        {
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                GeosApplication.Instance.Logger.Log("Method SelectItemForScheduler()...", category: Category.Info, priority: Priority.Low);


                ShowCompanyHolidayAppointmentsForSelectedCompany();


                //StatusItems
                //IsSet = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method SelectItemForScheduler()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method SelectItemForScheduler()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ShowCompanyHolidayAppointmentsForSelectedCompany()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowCompanyHolidayAppointmentsForSelectedCompany()...", category: Category.Info, priority: Priority.Low);

                foreach (Holidays CompanyHoliday in CompanyHolidays)
                {

                    UI.Helper.PlanningAppointment modelActivity = new UI.Helper.PlanningAppointment();
                    modelActivity.Subject = CompanyHoliday.Name;
                    modelActivity.StartDate = CompanyHoliday.StartDate;
                    modelActivity.EndDate = CompanyHoliday.EndDate;
                    modelActivity.Label = CompanyHoliday.IdHoliday;
                    modelActivity.ContentSubject = CompanyHoliday.Name;
                    modelActivity.IsHolidayData = Visibility.Visible;  //holiday data
                    modelActivity.IsHolidayDate = Visibility.Collapsed;  //ot data
                    modelActivity.IsPlannedDeliveryDate = true;
                    AppointmentItems.Add(modelActivity);

                    //AppointmentItems = new CustomObservableCollection<UI.Helper.PlanningAppointment>(AppointmentItems);
                }

                GeosApplication.Instance.Logger.Log("Method ShowCompanyHolidayAppointmentsForSelectedCompany()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                // IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method ShowCompanyHolidayAppointmentsForSelectedCompany()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion


        //[Rupali Sarode][GEOS2-4157][16-02-2023]
        private void AppointmentDropCommandAction(AppointmentItemDragDropEventArgs e)
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Method AppointmentDropCommandAction()...", category: Category.Info, priority: Priority.Low);

                DateTime? tmpDeliveryDate = null;
                UInt32 tempIdOT = 0;
                UInt32 tmpIdCounterpart = 0;
                Int32 tmpIdSite = 0;//[GEOS2-5319][gulab lakade][15 11 2024]
                if (!GeosApplication.Instance.IsEditProductionPlanPermissionERM)
                {
                    e.Allow = false;
                    // CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("PlanningDateNoEditPermission").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }

                List<AppointmentDragResizeViewModel> draggedViewModelList = new List<AppointmentDragResizeViewModel>();

                AppointmentDragResizeViewModel draggedViewModel = e.ViewModels.First() as AppointmentDragResizeViewModel;
                //AppointmentDragResizeViewModel draggedViewModel = e.ViewModels as AppointmentDragResizeViewModel;

                if (draggedViewModel.CustomFields["DeliveryDate"] == null)
                {
                    e.Allow = false;
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddEditSODCopyWorkOperationNotAllowed").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }

                if (draggedViewModel.CustomFields["DeliveryDate"] != null)
                {
                    tmpDeliveryDate = (DateTime)draggedViewModel.CustomFields["DeliveryDate"];
                }

                if (tmpDeliveryDate == null)
                {
                    e.Allow = false;
                    return;
                }
                ResourceItem r = e.HitResource;
                var DragtoDate = e.HitInterval.Start.ToShortDateString();
                if (Convert.ToDateTime(DragtoDate) > tmpDeliveryDate)
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("PlannedDeliveryDateValidation").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    e.Allow = false;
                    return;
                }
                if (Convert.ToDateTime(DragtoDate) < DateTime.Now.Date)
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("PlanningDateTodaysDateValidation").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    e.Allow = false;
                    return;
                }
                if (Convert.ToDateTime(DragtoDate) <= tmpDeliveryDate)
                {
                    e.Allow = true;

                    //if (draggedViewModel.CustomFields["IdCounterpart"] != null)
                    //{
                    //    tmpIdCounterpart = Convert.ToUInt32(draggedViewModel.CustomFields["IdCounterpart"]);
                    //}

                    draggedViewModelList = e.ViewModels.ToList();

                    foreach (AppointmentDragResizeViewModel tmpdraggedViewModel in draggedViewModelList)
                    {
                        //if (draggedViewModel.CustomFields["IdOT"] != null)
                        //{
                        //    tempIdOT = Convert.ToUInt32(draggedViewModel.CustomFields["IdOT"]);
                        //}

                        if (tmpdraggedViewModel.CustomFields["IdOT"] != null)
                        {
                            tempIdOT = Convert.ToUInt32(tmpdraggedViewModel.CustomFields["IdOT"]);
                        }
                        if (tmpdraggedViewModel.CustomFields["IdCounterpart"] != null)
                        {
                            tmpIdCounterpart = Convert.ToUInt32(tmpdraggedViewModel.CustomFields["IdCounterpart"]);
                        }
                        if (tmpdraggedViewModel.CustomFields["IdSite"] != null)
                        {
                            tmpIdSite = Convert.ToInt32(tmpdraggedViewModel.CustomFields["IdSite"]);
                        }
                        // If not exists then Add item to list
                        //if (!PlanningDateReviewList.Any(x => x.IDOT == tempIdOT && x.IdCounterpart == tmpIdCounterpart))
                        if (!PlanningDateReviewList.Any(x => x.IDOT == tempIdOT))
                        {
                            PlanningDateReview tPlanningDateReview = new PlanningDateReview();

                            tPlanningDateReview.IDOT = tempIdOT;
                            tPlanningDateReview.IdCounterpart = tmpIdCounterpart;
                            //  tPlanningDateReview.IdCounterpart = 0;
                            tPlanningDateReview.PlannedDeliveryDate = Convert.ToDateTime(e.HitInterval.Start);
                            tPlanningDateReview.CreatedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                            tPlanningDateReview.CreatedIn = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                            tPlanningDateReview.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                            tPlanningDateReview.IdSite = tmpIdSite;////[GEOS2-5319][gulab lakade][15 11 2024]
                            PlanningDateReviewList.Add(tPlanningDateReview);
                        }
                        else
                        {
                            // PlanningDateReviewList.Where(y => y.IDOT == tempIdOT && y.IdCounterpart == tmpIdCounterpart).ToList().ForEach(i => i.PlannedDeliveryDate = Convert.ToDateTime(e.HitInterval.Start));
                            // PlanningDateReviewList.Where(y => y.IDOT == tempIdOT && y.IdCounterpart == tmpIdCounterpart).ToList().ForEach(i => i.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser));
                            PlanningDateReviewList.Where(y => y.IDOT == tempIdOT).ToList().ForEach(i => i.IdCounterpart = tmpIdCounterpart);
                            PlanningDateReviewList.Where(y => y.IDOT == tempIdOT).ToList().ForEach(i => i.PlannedDeliveryDate = Convert.ToDateTime(e.HitInterval.Start));
                            PlanningDateReviewList.Where(y => y.IDOT == tempIdOT).ToList().ForEach(i => i.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser));
                            PlanningDateReviewList.Where(y => y.IDOT == tempIdOT).ToList().ForEach(i => i.ModifiedIn = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")));
                            PlanningDateReviewList.Where(y => y.IDOT == tempIdOT).ToList().ForEach(i => i.IdSite = tmpIdSite);//[GEOS2-5319][gulab lakade][15 11 2024]
                        }
                    }
                }
                //else
                //{
                //    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("PlannedDeliveryDateValidation").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                //    e.Allow = false;
                //    return;
                //}
                if (GeosApplication.Instance.IsEditProductionPlanPermissionERM)
                {
                    IsSaveEnabled = true;
                    PlanningAppointment.IsSaveButtonEnabled = true;
                }
                else
                {
                    IsSaveEnabled = false;
                    PlanningAppointment.IsSaveButtonEnabled = false;
                }
                GeosApplication.Instance.Logger.Log("Method AppointmentDropCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }

            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AppointmentDropCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void RefreshPlanningDateCommandAction(object obj)
        {
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                GeosApplication.Instance.Logger.Log("Method RefreshPlanningDateCommandAction()...", category: Category.Info, priority: Priority.Low);
                if (IsBand)
                {
                    if (IsSaveEnabled == true)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); } //[GEOS2-4636][rupali sarode][19-07-2023]
                        MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["ArticleGridUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                        if (MessageBoxResult == MessageBoxResult.Yes)
                        {
                            SavePlanningDateCommandAction();
                        }
                        else
                        {
                            PlanningDateReviewList = new List<PlanningDateReview>();
                            IsSaveEnabled = false;
                            PlanningAppointment.IsSaveButtonEnabled = false;
                        }
                    }
                    SearchOTItem = string.Empty;
                    PlanningDateReviewList = null;

                    PlanningDateReviewList = new List<PlanningDateReview>();
                    GetPlanningDateRecordByPlantAndDate(ERMCommon.Instance.SelectedAuthorizedPlantsList, FromDate, ToDate);
                }
                if (IsGrid)  //[GEOS2-4143] [Pallavi Jadhav] [15 03 2023]
                {
                    view = PlanningDateReviewCellEditHelper.Viewtableview;
                    if (PlanningDateReviewCellEditHelper.IsValueChanged)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); } //[GEOS2-4636][rupali sarode][19-07-2023]
                        MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["ArticleGridUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                        if (MessageBoxResult == MessageBoxResult.Yes)
                        {
                            SaveGridPlanningDateCommandAction(PlanningDateReviewCellEditHelper.Viewtableview);
                        }
                        PlanningDateReviewCellEditHelper.IsValueChanged = false;
                    }

                    PlanningDateReviewCellEditHelper.IsValueChanged = false;

                    if (view != null)
                    {
                        PlanningDateReviewCellEditHelper.SetIsValueChanged(view, false);
                    }
                    SearchOTItem = string.Empty;
                    PlanningDateReviewList = null;

                    PlanningDateReviewList = new List<PlanningDateReview>();
                    //[rupali sarode][28/07/2023]
                    GetPlanningDateRecordByPlantAndDate(ERMCommon.Instance.SelectedAuthorizedPlantsList, FromDate, ToDate);

                    ShowPlanningDateGridView(obj);
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method RefreshPlanningDateCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RefreshPlanningDateCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RefreshPlanningDateCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshPlanningDateCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        //[Rupali Sarode][20-2-2023]
        private void PopupMenuShowingCommandAction(PopupMenuShowingEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PopupMenuShowingCommandAction()...", category: Category.Info, priority: Priority.Low);

                if (obj.MenuType == ContextMenuType.CellContextMenu)
                {
                    PopupMenu menu = (PopupMenu)obj.Menu;
                    object open = menu.Items.FirstOrDefault(x => x is BarItem && (string)((BarItem)x).Content == "Change View To");

                    ((DevExpress.Xpf.Bars.PopupMenu)obj.Menu).Items.Clear();
                    ((DevExpress.Xpf.Bars.PopupMenu)obj.Menu).Items.Add((BarItem)open);
                }
                else if (obj.MenuType == ContextMenuType.AppointmentContextMenu)
                {
                    ((DevExpress.Xpf.Bars.PopupMenu)obj.Menu).Items.Clear();
                }

                GeosApplication.Instance.Logger.Log("Method PopupMenuShowingCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method PopupMenuShowingCommandAction()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        //private void DoubleClickCommandAction(MouseButtonEventArgs obj)
        //{
        //    if (obj.MenuType == ContextMenuType.CellContextMenu || obj.MenuType == ContextMenuType.AppointmentContextMenu)
        //    {
        //        ((DevExpress.Xpf.Bars.PopupMenu)obj.Menu).Items.Clear();
        //    }


        //}

        private void AppointmentWindowShowing(AppointmentWindowShowingEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AppointmentWindowShowing()...", category: Category.Info, priority: Priority.Low);
                obj.Cancel = true;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AppointmentWindowShowing()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #region [Rupali Sarode][GEOS2-4161][20-2-2023]
        public void SavePlanningDateCommandAction()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SavePlanningDateCommandAction()...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                bool flag;
                // ERMService = new ERMServiceController("localhost:6699");
                //   flag = ERMService.AddUpdatePlanningDateReview(PlanningDateReviewList);
                #region //[pallavi jadhav][GEOS2-5320][06 11 2024]
                //List<GeosAppSetting> ActivePlantList = WorkbenchStartUp.GetSelectedGeosAppSettings("134"); //[pallavi jadhav][GEOS2-5320][06 11 2024]
                List<Site> plantOwners = ERMCommon.Instance.SelectedAuthorizedPlantsList.Cast<Site>().ToList();
                //List<string> defaultValues = plantOwners.Select(a => Convert.ToString(a.IdSite)).ToList();
                Int32 TempIDSite = 0;
                TempIDSite = PlanningDateReviewList.Select(a => a.IdSite).FirstOrDefault();
                string ActivePlantString = ActivePlantList.Select(a => a.DefaultValue).FirstOrDefault();
                List<string> TempActivePlantlist = new List<string>();
                if (ActivePlantString != null)
                {
                    TempActivePlantlist = ActivePlantString.Split(',').ToList();
                }

                if (TempActivePlantlist != null && TempActivePlantlist.Contains(Convert.ToString(TempIDSite)) == true)
                {
                    //[GEOS2-5319][gulab lakade][15 11 2024]
                    foreach (Site item in plantOwners.Where(a => a.IdSite == TempIDSite).ToList())
                    {
                        string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.Name == item.Name).Select(xy => xy.ServiceProviderUrl).FirstOrDefault();
                        ERMService = new ERMServiceController(serviceurl);
                        //ERMService = new ERMServiceController("localhost:6699");
                        flag = ERMService.AddUpdatePlanningDateReviewByStage_V2580(Convert.ToUInt32(TempIDSite), PlanningDateReviewList);
                    }
                    //[GEOS2-5319][gulab lakade][15 11 2024]
                }
                else
                {
                    //ERMService = new ERMServiceController("localhost:6699");
                    //flag = ERMService.AddUpdatePlanningDateReview(PlanningDateReviewList);
                    flag = ERMService.AddUpdatePlanningDateReview_V2580(PlanningDateReviewList);
                }
                #endregion
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); } //[GEOS2-4636][rupali sarode][19-07-2023]
                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("PlanningDateAddSuccessMessage").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);

                RefreshControls();


                GeosApplication.Instance.Logger.Log("Method SavePlanningDateCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }

            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method SavePlanningDateCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //[Rupali Sarode][GEOS2-4161][21-2-2023]
        private void RefreshControls()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshControls()...", category: Category.Info, priority: Priority.Low);

                IsSaveEnabled = false;
                PlanningAppointment.IsSaveButtonEnabled = false;

                //Update main list using PlanningDateReviewList list
                //foreach (PlanningDateReview tmpPlanningDate in PlanningDateReviewList)
                //{
                //    ProductionPlanningReviewList.Where(x => x.IdOt == tmpPlanningDate.IDOT).ToList().ForEach(y => y.PlanningDeliveryDate = tmpPlanningDate.PlannedDeliveryDate);
                //    if (tmpPlanningDate.ModifiedIn != null)
                //    {
                //        ProductionPlanningReviewList.Where(x => x.IdOt == tmpPlanningDate.IDOT).ToList().ForEach(y => y.LastUpdateDate = tmpPlanningDate.ModifiedIn);
                //    }
                //    else
                //    {
                //        ProductionPlanningReviewList.Where(x => x.IdOt == tmpPlanningDate.IDOT).ToList().ForEach(y => y.LastUpdateDate = tmpPlanningDate.CreatedIn);
                //    }
                //}

                //GetRecordbyDeliveryDate();
                if (SelectedPlantold == null)
                    SelectedPlantold = new List<object>();
                SelectedPlantold = ERMCommon.Instance.SelectedAuthorizedPlantsList;

                GetPlanningDateRecordByPlantAndDate(ERMCommon.Instance.SelectedAuthorizedPlantsList, FromDate, ToDate);
                //FillDeliveryweek();
                //     SelectedItem = ProductionPlanningReviewList.Where(x => x.PlanningDeliveryDate == Convert.ToDateTime(AccordianFromDate, CurrentCulture)).FirstOrDefault();
                // SelectedItem = ProductionPlanningReviewList.Where(x => x.DeliveryDate == Convert.ToDateTime(SelectedItemOld.DeliveryDate)).FirstOrDefault();

                if (ERMCommon.Instance.SelectedAuthorizedPlantsList == null)
                    ERMCommon.Instance.SelectedAuthorizedPlantsList = new List<object>();
                ERMCommon.Instance.SelectedAuthorizedPlantsList = SelectedPlantold;

                // ShowCompanyHolidayAppointmentsForSelectedCompany();
                // PlanningDateReviewList = new List<PlanningDateReview>();
            }
            catch (Exception ex)
            {

                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshControls()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        private void SearchOTCommandAction(object obj)
        {
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                GeosApplication.Instance.Logger.Log("Method SearchOTCommandAction()...", category: Category.Info, priority: Priority.Low);
                List<ProductionPlanningReview> tempProductionPlanningReviewList = new List<ProductionPlanningReview>();
                string tempOT = Convert.ToString(SearchOTItem);
                if (!string.IsNullOrEmpty(tempOT) && tempOT != "SearchOTItem")
                {
                    tempProductionPlanningReviewList = ProductionPlanningReviewList.Where(x => x.OTCode.Contains(tempOT)).ToList();


                    AppointmentItems = new CustomObservableCollection<UI.Helper.PlanningAppointment>();
                    foreach (var item in tempProductionPlanningReviewList)
                    {
                        UI.Helper.PlanningAppointment modelActivity = new UI.Helper.PlanningAppointment();
                        if (item.PlanningDeliveryDate != null)
                        {
                            modelActivity.StartDate = item.PlanningDeliveryDate.Value;
                            modelActivity.EndDate = item.PlanningDeliveryDate.Value.AddMinutes(30);
                        }
                        else
                        {
                            modelActivity.StartDate = item.DeliveryDate.Value;
                            modelActivity.EndDate = item.DeliveryDate.Value.AddMinutes(30);
                        }

                        if (item.DeliveryDate.Value.Date < GeosApplication.Instance.ServerDateTime.Date)
                            modelActivity.Label = Convert.ToInt32(Labels[0].Id);
                        else if (item.DeliveryDate.Value.Date == GeosApplication.Instance.ServerDateTime.Date)
                            modelActivity.Label = Convert.ToInt32(Labels[1].Id);
                        else if (item.DeliveryDate.Value.Date > GeosApplication.Instance.ServerDateTime.Date)
                        {
                            if (item.DeliveryDate.Value.Date >= GeosApplication.Instance.ServerDateTime.Date.AddDays(7))
                                modelActivity.Label = Convert.ToInt32(Labels[3].Id);
                            else
                                modelActivity.Label = Convert.ToInt32(Labels[2].Id);
                        }

                        // modelActivity.Label = item.IdTemplate;
                        modelActivity.Subject = item.OTCode;
                        //modelActivity.Description = item.Type;
                        modelActivity.DeliveryDate = item.DeliveryDate;
                        modelActivity.Template = item.Template;
                        modelActivity.NumItem = item.NumItem;
                        modelActivity.QTY = item.QTY;
                        modelActivity.OTCode = item.OTCode;
                        modelActivity.Type = item.Type;
                        modelActivity.OriginPlant = item.OriginalPlant;
                        modelActivity.ContentSubject = item.CurrentWorkStation + ";" + item.OTCode + "; " + item.NumItem + "; " + item.Type + ";" + item.QTY;

                        //modelActivity.Status = Convert.ToInt32(item.IdCPType);
                        modelActivity.CurrentWorkStation = item.CurrentWorkStation;//[pallavi jadhav] [GEOS2-4481] [26 05 2023] 
                        #region//[pallavi jadhav] [GEOS2-4519] [06 06 2023] 
                        var tempReal = item.PlanningDateReviewStages.Select(a => a.Real).ToList();  // Convert the LINQ query result to a list
                        if (tempReal != null)
                        {
                            TimeSpan Temreal = TimeSpan.FromSeconds(tempReal.Sum(d => d.HasValue ? (double)d.Value : 0));
                            modelActivity.Real = Temreal.ToString();
                        }

                        TimeSpan TemExpected = TimeSpan.Parse("0");
                        var tempExpected = item.PlanningDateReviewStages.Select(a => a.Expected).ToList();
                        if (tempExpected != null)
                        {
                            TemExpected = TimeSpan.FromMinutes(tempExpected.Sum(d => d.HasValue ? (double)d.Value : 0));
                            // TemExpected = TimeSpan.FromSeconds(tempExpected.Sum(d => (double)d));
                            modelActivity.Expected = TemExpected.ToString();
                        }
                        #endregion
                        if (item.DeliveryDate != item.DeliveryDate)
                        {
                            modelActivity.IsPlannedDeliveryDate = true;
                        }
                        else
                        {
                            modelActivity.IsPlannedDeliveryDate = false;
                        }
                        modelActivity.PlanningDeliveryDate = item.PlanningDeliveryDate;
                        if (item.PlanningDeliveryDate != null)
                        {
                            if (item.DeliveryDate != item.PlanningDeliveryDate)
                            {
                                modelActivity.IsPlannedDeliveryDate = true;
                            }
                            else
                            {
                                modelActivity.IsPlannedDeliveryDate = false;
                            }
                            modelActivity.PlanningDeliveryDate = item.PlanningDeliveryDate;
                        }
                        else
                        {
                            modelActivity.IsPlannedDeliveryDate = false;
                        }
                        if (item.LastUpdateDate != null)
                        {
                            modelActivity.LastUpdate = item.LastUpdateDate;
                        }

                        #region [Rupali Sarode][GEOS2-4347][05-05-2023]
                        modelActivity.Customer = item.Customer;
                        modelActivity.Project = item.Project;
                        #endregion

                        #region [pallavi jadhav][GEOS2-5319][22-10-2024]

                        modelActivity.IdCounterpart = Convert.ToUInt32(item.IdCounterpart);
                        modelActivity.IdSite = Convert.ToInt32(item.IdProductionPlant);//[gulab lakade][15 11 2024]
                        #endregion
                        AppointmentItems.Add(modelActivity);

                    }
                    // AppointmentItems.AddRange(AppointmentItems);
                    // AppointmentItems = new CustomObservableCollection<UI.Helper.PlanningAppointment>(AppointmentItems);
                }

                if (Keyboard.PrimaryDevice != null)
                {
                    if (Keyboard.PrimaryDevice.ActiveSource != null)
                    {
                        var e1 = new KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.Tab) { RoutedEvent = Keyboard.KeyDownEvent };
                        InputManager.Current.ProcessInput(e1);
                    }
                }
                // e.Key == Key.Tab;
                // e.Key = Key.Tab;
                //if (e.Key==Key.Tab)
                //{

                //}

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method SearchOTCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SearchOTCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SearchOTCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method SearchOTCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        //private void LostFocusCommandAction(RoutedEventArgs e)
        //{


        //}
        private void DefaultUnLoadCommandAction(RoutedEventArgs obj)
        {

        }

        //private void TableViewLoadedCommandAction(RoutedEventArgs obj)
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method TableViewLoadedCommandAction ...", category: Category.Info, priority: Priority.Low);
        //        int visibleFalseCoulumn = 0;

        //        if (File.Exists(PendingPlanningDateReviewGridSettingFilePath))
        //        {
        //            ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.RestoreLayoutFromXml(PendingPlanningDateReviewGridSettingFilePath);
        //            GridControl GridControlSTDetails = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;

        //            TableView tableView = (TableView)GridControlSTDetails.View;
        //            if (tableView.SearchString != null)
        //            {
        //                tableView.SearchString = null;
        //            }
        //        }

        //        ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.AddHandler(DXSerializer.AllowPropertyEvent, new AllowPropertyEventHandler(OnAllowProperty));

        //        //This code for save grid layout.
        //        ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.SaveLayoutToXml(PendingPlanningDateReviewGridSettingFilePath);

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
        //                visibleFalseCoulumn++;
        //            }
        //        }

        //        FillERMDataInObjectsByCallingLatestServiceMethods.ShowPleaseWaitScreen();
        //        List<GeosAppSetting> PendingPOColorList = WorkbenchStartUp.GetSelectedGeosAppSettings("2,3,4,5");
        //        //  IsHolidayDate = Visibility.Collapsed;


        //        FillERMDataInObjectsByCallingLatestServiceMethods.GetERMDataOnceFromServiceForAttendance(
        //       ref ERMService, ref CrmStartUp,
        //         ref companyHolidays,
        //       ref holidayList, ref labelItems, ref PendingPOColorList,
        //       ref appointmentItems, ref fromDate, ref toDate
        //        );

        //        // AppointmentItems.Add(appointmentItems);
        //        // AppointmentItems = new ObservableCollection<UI.Helper.PlanningAppointment>(AppointmentItems);

        //        FillERMDataInObjectsByCallingLatestServiceMethods.ClosePleaseWaitScreen();
        //        // ShowCompanyHolidayAppointmentsForSelectedCompany();

        //        GeosApplication.Instance.Logger.Log("Method TableViewLoadedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in TableViewLoadedCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
        //    }
        //}
        //void VisibleChanged(object sender, EventArgs args)
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method VisibleChanged ...", category: Category.Info, priority: Priority.Low);

        //        GridColumn column = sender as GridColumn;
        //        if (column.ShowInColumnChooser)
        //        {
        //            ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(PendingPlanningDateReviewGridSettingFilePath);
        //        }

        //        if (column.Visible == false)
        //        {
        //            //  IsWorkOrderColumnChooserVisible = true;
        //        }

        //        GeosApplication.Instance.Logger.Log("Method VisibleChanged() executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in VisibleChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}

        //void VisibleIndexChanged(object sender, EventArgs args)
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method VisibleIndexChanged ...", category: Category.Info, priority: Priority.Low);

        //        GridColumn column = sender as GridColumn;
        //        ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(PendingPlanningDateReviewGridSettingFilePath);

        //        GeosApplication.Instance.Logger.Log("Method VisibleIndexChanged() executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in VisibleIndexChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}

        private void OnAllowProperty(object sender, AllowPropertyEventArgs e)
        {
            try
            {
                if (e.Property.Name == "GroupCount")
                    e.Allow = false;

                if (e.DependencyProperty == TableViewEx.SearchStringProperty)
                    e.Allow = false;

                if (e.Property.Name == "FormatConditions")
                    e.Allow = false;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in OnAllowProperty() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void TableViewUnloadedCommandAction(RoutedEventArgs obj)
        {
            try
            {
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in TableViewUnloadedCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
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

        private void ShowPlanningDateValidationCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowPlanningDateValidationCommandAction()...", category: Category.Info, priority: Priority.Low);

                DevExpress.Xpf.Grid.CellValueChangedEventArgs tempObj = (DevExpress.Xpf.Grid.CellValueChangedEventArgs)obj;

                //ProductionPlanningReview ProductionPlanningReviewTemp = new ProductionPlanningReview();
                //ProductionPlanningReviewTemp = ProductionPlanningReviewList.FirstOrDefault();
                DateTime? tmpPlanningDeliveryDate = Convert.ToDateTime(tempObj.Value);
                ProductionPlanningReview ProductionPlanningReviewTemp = (ProductionPlanningReview)tempObj.Row;

                DateTime? tmpDeliveryDate = ProductionPlanningReviewTemp.DeliveryDate;
                if (tmpDeliveryDate < tmpPlanningDeliveryDate && tmpPlanningDeliveryDate < DateTime.Now.Date)
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("PlanningDateANDTodaysDateValidation").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);

                    view = PlanningDateReviewCellEditHelper.Viewtableview;
                    PlanningDateReviewCellEditHelper.IsValueChanged = false;
                    if (view != null)
                    {
                        PlanningDateReviewCellEditHelper.SetIsValueChanged(view, false);
                    }
                }
                else
                {


                    if (tmpDeliveryDate < tmpPlanningDeliveryDate)
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("PlannedDeliveryDateValidation").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        //tmpPlanningDeliveryDate = (DateTime?)null;
                        //obj = null;
                        //tmpPlanningDeliveryDate= string.Empty

                        view = PlanningDateReviewCellEditHelper.Viewtableview;

                        PlanningDateReviewCellEditHelper.IsValueChanged = false;

                        if (view != null)
                        {
                            PlanningDateReviewCellEditHelper.SetIsValueChanged(view, false);
                        }

                    }
                    else
                    if (tmpPlanningDeliveryDate < DateTime.Now.Date)
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("PlanningDateTodaysDateValidation").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        view = PlanningDateReviewCellEditHelper.Viewtableview;

                        PlanningDateReviewCellEditHelper.IsValueChanged = false;

                        if (view != null)
                        {
                            PlanningDateReviewCellEditHelper.SetIsValueChanged(view, false);
                        }

                    }
                }

                GeosApplication.Instance.Logger.Log("Method ShowPlanningDateValidationCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ShowPlanningDateValidationCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);

            }
        }

        private void PrintPlanningDateReviewAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintPlanningDateReviewAction()...", category: Category.Info, priority: Priority.Low);
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
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["LeavesReportPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["LeavesReportPrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                window.Show();

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method PrintPlanningDateReviewAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintPlanningDateReviewAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        private void ExportPlanningDateReviewCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportPlanningDateReviewCommandAction()...", category: Category.Info, priority: Priority.Low);
                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "PlanningDateReview";
                saveFile.Filter = "Excel Files (.xlsx)|*.xlsx|All Files (*.*)|*.*";
                saveFile.FilterIndex = 1;
                saveFile.Title = "Save Planning Date Review Report";
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
                    TableView LeavesTableView = ((TableView)obj);
                    LeavesTableView.ShowTotalSummary = false;
                    LeavesTableView.ShowFixedTotalSummary = false;
                    LeavesTableView.ExportToXlsx(ResultFileName);

                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);
                    LeavesTableView.ShowTotalSummary = true;
                    LeavesTableView.ShowFixedTotalSummary = true;
                }
                GeosApplication.Instance.Logger.Log("Method ExportPlanningDateReviewCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportPlanningDateReviewCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        //[GEOS2-4143] [Pallavi Jadhav] [15 03 2023]
        public void SaveGridPlanningDateCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SaveGridPlanningDateCommandAction()...", category: Category.Info, priority: Priority.Low);
                //view = PlanningDateReviewCellEditHelper.Viewtableview;
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                view = obj as TableView;
                //GridControl gridControl = obj as GridControl;
                GridControl gridControl = (view).Grid;
                gridControl = (view).Grid;
                //if (view != null)
                //{
                //    PlanningDateReviewCellEditHelper.SetIsValueChanged(view, false);
                //}
                ObservableCollection<object> selectedRows = (ObservableCollection<object>)view.SelectedRows;
                PlanningDateReviewView detectionsView = new Views.PlanningDateReviewView();
                // PlanningDelivaryDate = PlanningDateReviewCellEditHelper.PlanningDeliveryDate;
                //if()
                //{

                //}
                #region //[pallavi jadhav][GEOS2-5320][06 11 2024]
                // List<GeosAppSetting> ActivePlantList = WorkbenchStartUp.GetSelectedGeosAppSettings("134"); //[pallavi jadhav][GEOS2-5320][06 11 2024]
                List<Site> plantOwners = ERMCommon.Instance.SelectedAuthorizedPlantsList.Cast<Site>().ToList();
                //List<string> defaultValues = plantOwners.Select(a=>Convert.ToString(a.IdSite)).ToList();
                //var activeplants = ActivePlantList.Where(a => defaultValues.Contains(a.DefaultValue)).ToList();
                #endregion
                ProductionPlanningReview[] foundRow = ProductionPlanningReviewList.AsEnumerable().Where(row => row.IsUpdatedRow == true).ToArray();
                foreach (ProductionPlanningReview item in foundRow)
                {
                    //  PlanningDateReview oldDetectionDetails = temp.Where(t => t.IdDetections == item.IdDetections).FirstOrDefault();
                    //  ProductionPlanningReview oldData = ClonedPlanningDateReviewList.FirstOrDefault(a => a.IdCP == item.IdCP);
                    ProductionPlanningReview _PlanningDateReview = new ProductionPlanningReview();
                    _PlanningDateReview.IdOt = item.IdOt;
                    _PlanningDateReview.IdCounterpart = item.IdCounterpart;
                    _PlanningDateReview.PlanningDeliveryDate = item.PlanningDeliveryDate;
                    //  _PlanningDateReview.CreatedIn = DateTime.Now;
                    _PlanningDateReview.IdCounterpart = item.IdCounterpart;
                    _PlanningDateReview.IdProductionPlant = item.IdProductionPlant;//[GEOS2-5319][gulab lakade][15 11 2024]
                    // _PlanningDateReview.ModifiedIn = DateTime.Now;
                    _PlanningDateReview.CreatedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                    _PlanningDateReview.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                    bool flag;
                    _PlanningDateReview.TransactionOperation = ModelBase.TransactionOperations.Add;

                    //  flag = ERMService.AddUpdatePlanningDateReview_V2370(_PlanningDateReview);

                    #region //[pallavi jadhav][GEOS2-5320][06 11 2024]
                    Int32 TempIDSite = 0;
                    TempIDSite = item.IdProductionPlant;
                    string ActivePlantString = ActivePlantList.Select(a => a.DefaultValue).FirstOrDefault();
                    List<string> TempActivePlantlist = new List<string>();
                    if (ActivePlantString != null)
                    {
                        TempActivePlantlist = ActivePlantString.Split(',').ToList();
                    }
                    if (TempActivePlantlist != null && TempActivePlantlist.Contains(Convert.ToString(TempIDSite)) == true)
                    {
                        //[GEOS2-5319][gulab lakade][15 11 2024]
                        //Int32 TempIDSite = PlanningDateReviewList.Select(a => a.IdSite).FirstOrDefault();
                        foreach (Site item1 in plantOwners.Where(a => a.IdSite == TempIDSite).ToList())
                        {
                            string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.Name == item1.Name).Select(xy => xy.ServiceProviderUrl).FirstOrDefault();
                            ERMService = new ERMServiceController(serviceurl);
                            //ERMService = new ERMServiceController("localhost:6699");
                            flag = ERMService.AddUpdatePlanningDeliveryDateGridByStage_V2580(Convert.ToUInt32(TempIDSite), _PlanningDateReview);
                        }
                        //[GEOS2-5319][gulab lakade][15 11 2024]

                    }
                    else
                    {
                        //ERMService = new ERMServiceController("localhost:6699");
                        //flag = ERMService.AddUpdatePlanningDateReview_V2370(_PlanningDateReview);
                        flag = ERMService.AddUpdatePlanningDateGrid_V2580(_PlanningDateReview);
                    }
                    #endregion
                }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); } //[GEOS2-4636][rupali sarode][19-07-2023]

                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("PlanningDateAddSuccessMessage").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);

                RefreshGridControls();


                GeosApplication.Instance.Logger.Log("Method SaveGridPlanningDateCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }

            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method SaveGridPlanningDateCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void RefreshGridControls()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshGridControls()...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                //IsSaveEnabled = false;
                //PlanningAppointment.IsSaveButtonEnabled = false;
                PlanningDateReviewCellEditHelper.IsValueChanged = false;

                if (view != null)
                {
                    PlanningDateReviewCellEditHelper.SetIsValueChanged(view, false);
                }
                ShowPlanningDateGridView(null);

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method RefreshGridControls()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshGridControls()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CellValueUpdatedCommnadAction(CellValueChangedEventArgs obj)
        {


            obj.Source.PostEditor();

        }

        #region   [GEOS2-4481]Pallavi Jadhav][29 05 2023]
        private void FillStages()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillStages ...", category: Category.Info, priority: Priority.Low);

                PlanningDateReviewStagesList = new ObservableCollection<PlanningDateReviewStages>();
                //   ERMService = new ERMServiceController("localhost:6699");
                PlanningDateReviewStages PlanningDateReviewStage = new PlanningDateReviewStages();
                PlanningDateReviewStage.IdStage = 0;
                PlanningDateReviewStage.StageCode = "Blanks";
                PlanningDateReviewStagesList.Add(PlanningDateReviewStage);
                PlanningDateReviewStagesList.AddRange(ERMService.GetProductionPlanningReviewStage_V2400());
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
                GeosApplication.Instance.Logger.Log("Method FilterOptionLoadedCommandAction()...", category: Category.Info, priority: Priority.Low);

                IsFilterLoaded = true;
                ComboBoxEdit combo = obj as ComboBoxEdit;
                //combo.SelectAllItems();
                var items = new List<object>();
                combo.SelectAllItems();

                IsFilterLoaded = false;
                IsFilterNotSelectedVisiblity = Visibility.Hidden;
                IsFilterSomeVisiblity = Visibility.Hidden;
                IsFilterAllVisiblity = Visibility.Visible;
                //   ProductionPlanningReviewList = ProductionPlanningReviewListCopy;
                GeosApplication.Instance.Logger.Log("Method FilterOptionLoadedCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {

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

                CustomObservableCollection<UI.Helper.PlanningAppointment> TempAppointmentItems = new CustomObservableCollection<PlanningAppointment>();
                //  List<ProductionPlanningReview> test = ProductionPlanningReviewListCopy.Where(i => i.CurrentWorkStation != null).ToList();

                AppointmentItems = new CustomObservableCollection<PlanningAppointment>();

                var propertyValues = options.Select(item => item.GetType().GetProperty("StageCode")?.GetValue(item));
                //List<string> stageCodes = propertyValues.Cast<string>().ToList();
                stageCodes = new List<string>();
                stageCodes = propertyValues.Cast<string>().ToList();

                //if (IsBand)
                //{
                //  TempAppointmentItems.AddRange(AppointmentItemsCopy.Where(i => stageCodes.Contains(i.CurrentWorkStation)).ToList());
                List<GeosAppSetting> PendingPOColorList = WorkbenchStartUp.GetSelectedGeosAppSettings("14,15,16");
                if (stageCodes.Contains("Blanks") == true)
                {
                    TempAppointmentItems.AddRange(AppointmentItemsCopy
                   .Where(i => i.CurrentWorkStation == "" || i.CurrentWorkStation == null)
                   .GroupBy(a => new { a.NumItem, a.OTCode, a.CurrentWorkStation })
                    .Select(g => new UI.Helper.PlanningAppointment
                    {
                        OTCode = g.Key.OTCode,
                        QTY = g.Sum(a => a.QTY),
                        DeliveryDate = g.FirstOrDefault()?.DeliveryDate,
                        StartDate = g.FirstOrDefault()?.StartDate,
                        Label = g.FirstOrDefault()?.Label,
                        LastUpdate = g.FirstOrDefault()?.LastUpdate,

                        Subject = g.FirstOrDefault()?.Subject,
                        ContentSubject = g.FirstOrDefault()?.ContentSubject,
                        OriginPlant = g.FirstOrDefault()?.OriginPlant,
                        Template = g.FirstOrDefault()?.Template,
                        Type = g.FirstOrDefault()?.Type,
                        NumItem = g.FirstOrDefault()?.NumItem,
                        CurrentWorkStation = g.Key.CurrentWorkStation,
                        PlanningDeliveryDate = g.FirstOrDefault()?.PlanningDeliveryDate,
                        Real = g.FirstOrDefault()?.Real,
                        Expected = g.FirstOrDefault()?.Expected,
                        Customer = g.FirstOrDefault()?.Customer,
                        Project = g.FirstOrDefault()?.Project,
                        IdCounterpart = g.FirstOrDefault().IdCounterpart,
                    }).ToList());

                    // FillERMDataInObjectsByCallingLatestServiceMethods.ShowPleaseWaitScreen();




                    // FillERMDataInObjectsByCallingLatestServiceMethods.GetERMDataOnceFromServiceForAttendance(
                    //ref ERMService, ref CrmStartUp,
                    //  ref companyHolidays,
                    //ref holidayList, ref labelItems, ref PendingPOColorList,
                    //ref appointmentItems, ref fromDate, ref toDate, ref TempAppointmentItems
                    // );

                    // FillERMDataInObjectsByCallingLatestServiceMethods.ClosePleaseWaitScreen();
                }

                TempAppointmentItems.AddRange(AppointmentItemsCopy
                .Where(i => stageCodes.Contains(i.CurrentWorkStation))
                .GroupBy(a => new { a.NumItem, a.OTCode, a.CurrentWorkStation })
                 .Select(g => new UI.Helper.PlanningAppointment
                 {

                     OTCode = g.Key.OTCode,
                     QTY = g.Sum(a => a.QTY),
                     DeliveryDate = g.FirstOrDefault()?.DeliveryDate,
                     StartDate = g.FirstOrDefault()?.StartDate,
                     Label = g.FirstOrDefault()?.Label,
                     LastUpdate = g.FirstOrDefault()?.LastUpdate,

                     Subject = g.FirstOrDefault()?.Subject,
                     ContentSubject = g.FirstOrDefault()?.ContentSubject,
                     OriginPlant = g.FirstOrDefault()?.OriginPlant,
                     Template = g.FirstOrDefault()?.Template,
                     Type = g.FirstOrDefault()?.Type,
                     NumItem = g.FirstOrDefault()?.NumItem,
                     CurrentWorkStation = g.Key.CurrentWorkStation,
                     PlanningDeliveryDate = g.FirstOrDefault()?.PlanningDeliveryDate,
                     Real = g.FirstOrDefault()?.Real,
                     Expected = g.FirstOrDefault()?.Expected,
                     Customer = g.FirstOrDefault()?.Customer,
                     Project = g.FirstOrDefault()?.Project,
                     IdCounterpart = g.FirstOrDefault().IdCounterpart,
                 }).ToList());

                FillERMDataInObjectsByCallingLatestServiceMethods.ShowPleaseWaitScreen();
                //List<GeosAppSetting> PendingPOColorList = WorkbenchStartUp.GetSelectedGeosAppSettings("14,15,16");

                //companyHolidays = CompanyHolidaysCopy;

                FillERMDataInObjectsByCallingLatestServiceMethods.GetERMDataOnceFromServiceForAttendance(
               ref ERMService, ref CrmStartUp,
                 ref companyHolidays,
               ref holidayList, ref labelItems, ref PendingPOColorList,
               ref appointmentItems, ref fromDate, ref toDate, ref TempAppointmentItems
                );

                FillERMDataInObjectsByCallingLatestServiceMethods.ClosePleaseWaitScreen();

                if (TempAppointmentItems.Count > 0)
                {
                    AppointmentItems = TempAppointmentItems;
                }
                //else
                //{
                //    AppointmentItems = AppointmentItemsCopy;
                //}
                //}
                //else
                //{
                //GroupedData = ProductionPlanningReviewListCopy.GroupBy(a => a.OTCode).Select(g => new ProductionPlanningReview
                //{
                //    OTCode = g.Key,
                //    QTY = g.Sum(a => a.QTY),
                //    DeliveryDate = g.FirstOrDefault()?.DeliveryDate,
                //    DeliveryDateHtmlColor = g.FirstOrDefault()?.DeliveryDateHtmlColor,
                //    DeliveryWeek = g.FirstOrDefault()?.DeliveryWeek,
                //    OriginalPlant = g.FirstOrDefault()?.OriginalPlant,
                //    ProductionPlant = g.FirstOrDefault()?.ProductionPlant,
                //    Template = g.FirstOrDefault()?.Template,
                //    Type = g.FirstOrDefault()?.Type,
                //    NumItem = g.FirstOrDefault()?.NumItem,
                //    CurrentWorkStation = g.FirstOrDefault()?.CurrentWorkStation,
                //    PlanningDeliveryDate = g.FirstOrDefault()?.PlanningDeliveryDate
                //}).ToList();

                //TempProductionPlanningReviewList.AddRange(OriginalonPlanningReviewList
                //.Where(i => stageCodes.Contains(i.CurrentWorkStation))
                //.GroupBy(a => new { a.OTCode, a.CurrentWorkStation })
                // .Select(g => new ProductionPlanningReview
                //{
                //    OTCode = g.Key.OTCode,
                //    QTY = g.Sum(a => a.QTY),
                //    DeliveryDate = g.FirstOrDefault()?.DeliveryDate,
                //    DeliveryDateHtmlColor = g.FirstOrDefault()?.DeliveryDateHtmlColor,
                //    DeliveryWeek = g.FirstOrDefault()?.DeliveryWeek,
                //    OriginalPlant = g.FirstOrDefault()?.OriginalPlant,
                //    ProductionPlant = g.FirstOrDefault()?.ProductionPlant,
                //    Template = g.FirstOrDefault()?.Template,
                //    Type = g.FirstOrDefault()?.Type,
                //    NumItem = g.FirstOrDefault()?.NumItem,
                //    CurrentWorkStation = g.Key.CurrentWorkStation,
                //    PlanningDeliveryDate = g.FirstOrDefault()?.PlanningDeliveryDate
                //}).ToList());
                if (stageCodes.Contains("Blanks") == true)
                {
                    TempProductionPlanningReviewList.AddRange(OriginalonPlanningReviewList
                      .Where(i => stageCodes.Contains(i.CurrentWorkStation) || i.CurrentWorkStation == null)
                      .ToList());
                    ProductionPlanningReviewList = TempProductionPlanningReviewList;

                }

                else
                {
                    TempProductionPlanningReviewList.AddRange(OriginalonPlanningReviewList
                       .Where(i => stageCodes.Contains(i.CurrentWorkStation))
                       .ToList());
                    ProductionPlanningReviewList = TempProductionPlanningReviewList;
                }
                //if (TempProductionPlanningReviewList.Count > 0)
                //    {
                //        ProductionPlanningReviewList = TempProductionPlanningReviewList;
                //    }
                //    else
                //    {
                //        ProductionPlanningReviewList = GroupedData;
                //    }
                // }
                ProductionPlanningReviewListCopy = new List<ProductionPlanningReview>();
                ProductionPlanningReviewListCopy = ProductionPlanningReviewList;
                FillDeliveryweek();//[GEOS2-4518][gulab lakade][05 06 2023]
                                   //var propertyValues = options.Select(item => item.GetType().GetProperty("StageCode")?.GetValue(item));


                //TempProductionPlanningReviewList.AddRange(test.Where(i => propertyList.StageCode.Contains(i.CurrentWorkStation)).ToList());



                //foreach (PlanningDateReviewStages item in options)
                //{

                //    //ProductionPlanningReview TempProductionPlanningReview = new ProductionPlanningReview();
                //    //TempProductionPlanningReview = ;

                //    //TempProductionPlanningReviewList = test.Where(i => i.CurrentWorkStation == item.StageCode).ToList();
                //}
                if (temp.SelectedItems.Count == PlanningDateReviewStagesList.Count)
                {
                    ID = 1;
                }
                else if (temp.SelectedItems.Count == 0)
                {
                    ID = 2;
                }
                else if (temp.SelectedItems.Count != 0 && temp.SelectedItems.Count < PlanningDateReviewStagesList.Count)
                {
                    ID = 3;
                }

                //IsFilterNotSelectedVisiblity = Visibility.Hidden;
                //IsFilterSomeVisiblity = Visibility.Hidden;
                //IsFilterAllVisiblity = Visibility.Visible;

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method FilterOptionEditValueChangedCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FilterOptionEditValueChangedCommandAction()", category: Category.Exception, priority: Priority.Low);
            }
        }


        #endregion

    }
}
