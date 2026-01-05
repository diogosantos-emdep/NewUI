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

namespace Emdep.Geos.Modules.ERM.ViewModels
{
    public class PlanningDateReviewViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Service
        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        IPLMService PLMService = new PLMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
    // IERMService ERMService = new ERMServiceController("localhost:6699");
       // IPLMService PLMService = new PLMServiceController("localhost:6699");
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
        #endregion

        #region Constructor

        public PlanningDateReviewViewModel()
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Constructor PlanningDateReviewViewModel()...", category: Category.Info, priority: Priority.Low);
                IsInit = true;

                RefreshTimeTrackingCommand = new RelayCommand(new Action<object>(RefreshTimeTrackingCommandAction));
                PrintPlanningDateReviewCommand = new RelayCommand(new Action<object>(PrintPlanningDateReviewAction));
                ExportPlanningDateReviewCommand = new RelayCommand(new Action<object>(ExportPlanningDateReviewCommandAction));
                HidePanelCommand = new RelayCommand(new Action<object>(HidePanel));
                PeriodCommand = new DelegateCommand<object>(PeriodCommandAction);
                PeriodCustomRangeCommand = new DelegateCommand<object>(PeriodCustomRangeCommandAction);
                ApplyCommand = new DelegateCommand<object>(ApplyCommandAction);
                CancelCommand = new DelegateCommand<object>(CancelCommandAction);
                ChangePlantCommand = new DelegateCommand<object>(ChangePlantCommandAction);
                ShowSchedulerViewCommand = new RelayCommand(new Action<object>(ShowAttendanceSchedulerView));
                ShowGridViewCommand = new RelayCommand(new Action<object>(ShowPlanningDateGridView));
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
                List<GeosAppSetting> PendingPOColorList = WorkbenchStartUp.GetSelectedGeosAppSettings("2,3,4,5");
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
                TableViewLoadedCommand = new DelegateCommand<RoutedEventArgs>(TableViewLoadedCommandAction);
                TableViewUnloadedCommand = new DelegateCommand<RoutedEventArgs>(TableViewUnloadedCommandAction);
                ShowPlanningDateValidationCommand = new RelayCommand(new Action<object>(ShowPlanningDateValidationCommandAction));
                SaveGridPlanningDateCommand = new RelayCommand(new Action<object>(SaveGridPlanningDateCommandAction));
                CellValueUpdatedCommnadCommand = new DelegateCommand<CellValueChangedEventArgs>(CellValueUpdatedCommnadAction);
                ///SumChangedCommand = new DelegateCommand<RoutedEventArgs>(GetRecordbyDeliveryDate);
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
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                IsPeriod = false;
                GetPlants();
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
               // IsBand = true;
                //[Rupali Sarode][GEOS2-4161][21-2-2023]
                PlanningDateReviewList = new List<PlanningDateReview>();
                ProductionPlanningReviewListCopy = new List<ProductionPlanningReview>();
                ProductionPlanningReviewListCopy.AddRange(ProductionPlanningReviewList);
                // PlanningDateSelectAccordain.PlanningDateSelectAccordain();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method Init() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Init()", category: Category.Exception, priority: Priority.Low);
            }

        }


        private void FillProductionPlanningReview()
        {
            try
            {
                ProductionPlanningReviewList = new List<ProductionPlanningReview>();
                // ProductionPlanningReviewList.AddRange(ERMService.GetProductionPlanningReview_V2360());
                List<ProductionPlanningReview> ProductionPlanningReviewList1 = new List<ProductionPlanningReview>();
                if (GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
                {
                    List<Company> plantOwners = GeosApplication.Instance.SelectedPlantOwnerUsersList.Cast<Company>().ToList();
                    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.ShortName));
                    ERMCommon.Instance.FailedPlants = new List<string>();
                    ERMCommon.Instance.IsShowFailedPlantWarning = false;
                    ERMCommon.Instance.WarningFailedPlants = string.Empty;
                    foreach (var itemPlantOwnerUsers in plantOwners)
                    {
                        DateTime tempFromyear = DateTime.Parse(FromDate.ToString());
                        string year = Convert.ToString(tempFromyear.Year);
                        string PlantName = Convert.ToString(itemPlantOwnerUsers.Alias);
                        try
                        {
                       //ERMService = new ERMServiceController("localhost:6699");
                            //ProductionPlanningReviewList.AddRange(ERMService.GetProductionPlanningReview_V2360(PlantName,
                            //    DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),
                            //    DateTime.ParseExact(ToDate, "dd/MM/yyyy", null)));
                            ProductionPlanningReviewList.AddRange(ERMService.GetProductionPlanningReview_V2370(PlantName,
                               DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),
                               DateTime.ParseExact(ToDate, "dd/MM/yyyy", null), GeosAppSettingList));
                            //companyHolidays.AddRangeWithTemporarySuppressedNotification(ERMService.GetCompanyHolidaysBySelectedIdCompany(PlantName, year));
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
                        modelActivity.ContentSubject = item.OTCode + "; " + item.NumItem + "; " + item.Type + ";" + item.QTY;
                        //modelActivity.Status = Convert.ToInt32(item.IdCPType);

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

                        AppointmentItems.Add(modelActivity);

                    }

                    List<GeosAppSetting> PendingPOColorList = WorkbenchStartUp.GetSelectedGeosAppSettings("2,3,4,5");
                    //  IsHolidayDate = Visibility.Collapsed;
                   

                   FillERMDataInObjectsByCallingLatestServiceMethods.GetERMDataOnceFromServiceForAttendance(
                  ref ERMService, ref CrmStartUp,
                    ref companyHolidays,
                  ref holidayList, ref labelItems, ref PendingPOColorList,
                  ref appointmentItems, ref fromDate, ref toDate
                   );

                    // AppointmentItems.Add(appointmentItems);
                    // AppointmentItems = new ObservableCollection<UI.Helper.PlanningAppointment>(AppointmentItems);

                    FillERMDataInObjectsByCallingLatestServiceMethods.ClosePleaseWaitScreen();
                   // ShowCompanyHolidayAppointmentsForSelectedCompany();
                }
              //  RefreshPlanningDateCommandAction(null);
                #endregion
            }
            catch (FaultException<ServiceException> ex)
            { }
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
                    PlantList = new ObservableCollection<Site>(PLMService.GetPlants_V2120());
                }

                List<Company> plantOwners = GeosApplication.Instance.SelectedPlantOwnerUsersList.Cast<Company>().ToList();
                var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.ConnectPlantId));

                List<Site> PlantList1 = new List<Site>();
                foreach (Company item in plantOwners)
                {

                    UInt32 plantid = Convert.ToUInt32(item.ConnectPlantId);
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
                        var tempDateFinal = tempdate.Select(a => a.Value.Date).Distinct().ToList();
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

            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillTemplateMenulist() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillTemplateMenulist() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillTemplateMenulist() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }



        private void RefreshTimeTrackingCommandAction(object obj)
        {
            try
            {
                
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
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
                if (SelectedPlant != null)
                {
                    GetPlanningDateRecordByPlantAndDate(SelectedPlant, FromDate, ToDate);
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

        private void ShowPlanningDateGridView(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowAttendanceGridView ...", category: Category.Info, priority: Priority.Low);


               IsBand = false;
                IsGrid = true;
                IsGridViewVisible = Visibility.Visible;
                IsSchedulerViewVisible = Visibility.Hidden;
                ProductionPlanningReviewList = new List<ProductionPlanningReview>();
                //      ProductionPlanningReviewList.AddRange(ERMService.GetProductionPlanningReview_V2360());
                List<ProductionPlanningReview> ProductionPlanningReviewList1 = new List<ProductionPlanningReview>();
                if (GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
                {
                    List<Company> plantOwners = GeosApplication.Instance.SelectedPlantOwnerUsersList.Cast<Company>().ToList();
                    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.ShortName));
                    ERMCommon.Instance.FailedPlants = new List<string>();
                    ERMCommon.Instance.IsShowFailedPlantWarning = false;
                    ERMCommon.Instance.WarningFailedPlants = string.Empty;
                    foreach (var itemPlantOwnerUsers in plantOwners)
                    {
                        string PlantName = Convert.ToString(itemPlantOwnerUsers.Alias);
                        try
                        {
                        //  ERMService = new ERMServiceController("localhost:6699");
                            ProductionPlanningReviewList.AddRange(ERMService.GetProductionPlanningReview_V2370(PlantName,
                               DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),
                               DateTime.ParseExact(ToDate, "dd/MM/yyyy", null), GeosAppSettingList));
                            //ProductionPlanningReviewGridList = new ObservableCollection<ProductionPlanningReview>(ERMService.GetProductionPlanningReviewForGrid_V2370(PlantName,
                            //   DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),
                            //   DateTime.ParseExact(ToDate, "dd/MM/yyyy", null), GeosAppSettingList));
                            //  ProductionPlanningReviewList1 = ProductionPlanningReviewList;
                          SumOfQTY = ProductionPlanningReviewList.Sum(x=>Convert.ToInt32(x.QTY));
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
                    //GridControl gridControl = ((DevExpress.Xpf.Grid.GridControl)obj);
                    //DevExpress.Xpf.Grid.GridSummaryItem summ = (DevExpress.Xpf.Grid.GridSummaryItem)gridControl.TotalSummary[0];
                   FillDeliveryweek();
                    //summ.DisplayFormat = " {0}";


                    //            TotalSummary = new ObservableCollection<Summary>() {
                    //    //TotalSummary.Add(
                    //        new Summary() { Type = SummaryItemType.Sum, FieldName = "QTY" , DisplayFormat = "{0}" }
                    //        //)
                    //};
                }

                GeosApplication.Instance.Logger.Log("Method ShowAttendanceGridView() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ShowAttendanceGridView()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ShowAttendanceSchedulerView(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowAttendanceSchedulerView ...", category: Category.Info, priority: Priority.Low);

                IsBand = true;
                IsGrid = false;
                IsSchedulerViewVisible = Visibility.Visible;
                /*   AttendanceFilterAttendanceIsEnabled = true;
                   AttendanceFilterAttendanceVisible = Visibility.Visible;
                   AttendanceFilterAttendanceWidth = 50; */
                PlanningDateReviewList = new List<PlanningDateReview>();
               // ProductionPlanningReviewListCopy = new List<ProductionPlanningReview>();
                ProductionPlanningReviewList.AddRange(ProductionPlanningReviewListCopy);
               // PlanningDateReviewList = ProductionPlanningReviewListCopy;
                FillDeliveryweek();
                IsGridViewVisible = Visibility.Hidden;

                GeosApplication.Instance.Logger.Log("Method ShowAttendanceSchedulerView() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ShowAttendanceSchedulerView()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void GetRecordbyDeliveryDate()
        {
            try
            {
                if (IsBand)
                {
                    List<ProductionPlanningReview> tempProductionPlanningReviewList = new List<ProductionPlanningReview>();
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
                    if (SelectedItem.ToString().Contains("All"))
                    {
                        tempProductionPlanningReviewList = ProductionPlanningReviewList.ToList();
                        var TempDeliveryDate = tempProductionPlanningReviewList.OrderBy(b => b.DeliveryDate).ToList();
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
                            tempProductionPlanningReviewList = ProductionPlanningReviewList.Where(x => x.DeliveryWeek == Convert.ToString(tempDeliveryWeek.copyDeliveryWeek)).ToList();
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

                            tempProductionPlanningReviewList = ProductionPlanningReviewList.Where(a => (a.PlanningDeliveryDate != null ? a.PlanningDeliveryDate.Value.ToString() : a.DeliveryDate.Value.ToString()).Contains(selectedDeliveryDate.copyDeliveryDate)).ToList();
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
                            AccordianFromDate = tempFromDate.ToString("dd/MM/yyyy");
                            IsAccordianFromDate = true;
                        }
                        IsPeriod = true;
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
                            if (item.PlanningDeliveryDate.Value.Date < GeosApplication.Instance.ServerDateTime.Date)
                                modelActivity.Label = Convert.ToInt32(Labels[0].Id);
                            else if (item.PlanningDeliveryDate.Value.Date == GeosApplication.Instance.ServerDateTime.Date)
                                modelActivity.Label = Convert.ToInt32(Labels[1].Id);
                            else if (item.PlanningDeliveryDate.Value.Date > GeosApplication.Instance.ServerDateTime.Date)
                            {
                                if (item.PlanningDeliveryDate.Value.Date >= GeosApplication.Instance.ServerDateTime.Date.AddDays(7))
                                    modelActivity.Label = Convert.ToInt32(Labels[3].Id);
                                else
                                    modelActivity.Label = Convert.ToInt32(Labels[2].Id);
                            }
                        }
                        else
                        {
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
                        modelActivity.ContentSubject = item.OTCode + "; " + item.NumItem + "; " + item.Type + ";" + item.QTY;
                        //modelActivity.Status = Convert.ToInt32(item.IdCPType);

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

                        AppointmentItems.Add(modelActivity);

                    }
                }
                if (IsGrid)
                {
                    if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
               
                    if (ProductionPlanningReviewList.Count == 0)
                    {
                        ProductionPlanningReviewListCopy = new List<ProductionPlanningReview>();
                        ProductionPlanningReviewListCopy.AddRange(ProductionPlanningReviewList);
                    }
                   
                    //TimeTrackingList = ERMService.GetAllTimeTracking_V2330();
                    if (ProductionPlanningReviewList.Count > 0)
                    {
                        
                        var currentculter = CultureInfo.CurrentCulture;
                        ProductionPlanningReview ProductionPlanningReview = new ProductionPlanningReview();
                        ProductionPlanningReview = ProductionPlanningReviewList.FirstOrDefault();
                        #region 
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
                                productionPlanningReviewTemp = ProductionPlanningReviewListCopy.Where(x => x.PlanningDeliveryDate != null && x.PlanningDeliveryDate.Value.ToString().Contains(SelectedItem.ToString())).ToList();
                                productionPlanningReviewTemp.AddRange(ProductionPlanningReviewListCopy.Where(x => x.DeliveryDate.Value.ToString().Contains(SelectedItem.ToString())).ToList());
                                ProductionPlanningReviewList = new List<ProductionPlanningReview>();
                                ProductionPlanningReviewList.AddRange(productionPlanningReviewTemp);
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
                        SumOfQTY = 0;
                        SumOfQTY = ProductionPlanningReviewList.Sum(x => Convert.ToInt32(x.QTY));
                        
                        //GridControl gridControl = ((DevExpress.Xpf.Grid.GridControl)obj);
                        //DevExpress.Xpf.Grid.GridSummaryItem summ = (DevExpress.Xpf.Grid.GridSummaryItem)gridControl.TotalSummary[0];
                        //summ.DisplayFormat = " {0}" + SumOfQTY;
                    }
                    
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                }

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method GetRecordbyDeliveryDate() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
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

                    if (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
                    {
                        //return;
                    }
                    else
                    {
                        GeosApplication.Instance.Logger.Log("Method ChangePlantCommandAction()...", category: Category.Info, priority: Priority.Low);
                        if (SelectedPlant != null)
                        {
                            try
                            {
                                GetPlanningDateRecordByPlantAndDate(SelectedPlant, FromDate, ToDate);
                            }
                            catch (FaultException<ServiceException> ex)
                            { }
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
        private void GetPlanningDateRecordByPlantAndDate(List<object> SelectedPlant, string FromDate, string ToDate)
        {
            try
            {
                ProductionPlanningReviewList = new List<ProductionPlanningReview>();
                List<ProductionPlanningReview> ProductionPlanningReviewList1 = new List<ProductionPlanningReview>();
                ProductionPlanningReviewList1 = new List<ProductionPlanningReview>();
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
                        string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.Name == item.Name).Select(xy => xy.ServiceProviderUrl).FirstOrDefault();

                    ERMService = new ERMServiceController(serviceurl);
                        //ProductionPlanningReviewList.AddRange(ERMService.GetProductionPlanningReview_V2360(PlantName,
                        //    DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),
                        //        DateTime.ParseExact(ToDate, "dd/MM/yyyy", null)));
                      // ERMService = new ERMServiceController("localhost:6699");
                        ProductionPlanningReviewList.AddRange(ERMService.GetProductionPlanningReview_V2370(PlantName,
                               DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),
                               DateTime.ParseExact(ToDate, "dd/MM/yyyy", null), GeosAppSettingList));
                    string PlantAlise = Convert.ToString(item.IdSite);
                    //string serviceurlholidays = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.Name == PlantAlise).Select(xy => xy.ServiceProviderUrl).FirstOrDefault();
                    //ERMService = new ERMServiceController(serviceurlholidays);
                 //   ERMService = new ERMServiceController("localhost:6699");
                    DateTime tempFromyear = DateTime.Parse(FromDate.ToString()); 
                    string year = Convert.ToString(tempFromyear.Year);
                   
                    companyHolidays.AddRangeWithTemporarySuppressedNotification(ERMService.GetCompanyHolidaysBySelectedIdCompany(PlantAlise, year));

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
                //List<Company> plantOwners = GeosApplication.Instance.SelectedPlantOwnerUsersList.Cast<Company>().ToList();
                //var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.ShortName.Contains(SelectedPlant.ToString())));

                //foreach (Company item1 in plantOwners)
                //{
                //    string PlantAlise = Convert.ToString(item1.Alias);
                //    string serviceurlholidays = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.Name == item1.Alias).Select(xy => xy.ServiceProviderUrl).FirstOrDefault();
                //    ERMService = new ERMServiceController(serviceurlholidays);
                  // ERMService = new ERMServiceController("localhost:6699");
                //    companyHolidaysList.AddRangeWithTemporarySuppressedNotification(ERMService.GetCompanyHolidaysBySelectedIdCompany(PlantAlise));
                //}
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
                        modelActivity.ContentSubject = item.OTCode + "; " + item.NumItem + "; " + item.Type + ";" + item.QTY;
                        //modelActivity.Status = Convert.ToInt32(item.IdCPType);

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

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DefaultLoadCommandAction()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
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
                object tempobj = new object();

                try
                {
                    var temp = (DevExpress.Xpf.Scheduling.VisibleIntervalsChangedEventArgs)obj;
                    
                  tempobj = temp.VisibleDates;
                  if(tempobj!=null)
                    {
                        return;
                    } 
                }
                catch(Exception ex)
                {
                }
                try
                {
                    tempobj = (Object[])obj;
                }
                catch (Exception ex)
                {
                }
                var values = (Object[])tempobj;
                GeosApplication.Instance.Logger.Log("Method VisibleIntervalsChanged()...", category: Category.Info, priority: Priority.Low);
               
               // var values=(Object[])obj;
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

                DateTime? tmpDeliveryDate = null;
                UInt32 tempIdOT = 0;
                UInt32 tmpIdCounterpart = 0;

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
                if (Convert.ToDateTime(DragtoDate) < DateTime.Now)
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

                        // If not exists then Add item to list
                        //if (!PlanningDateReviewList.Any(x => x.IDOT == tempIdOT && x.IdCounterpart == tmpIdCounterpart))
                        if (!PlanningDateReviewList.Any(x => x.IDOT == tempIdOT))
                        {
                            PlanningDateReview tPlanningDateReview = new PlanningDateReview();

                            tPlanningDateReview.IDOT = tempIdOT;
                            //  tPlanningDateReview.IdCounterpart = tmpIdCounterpart;
                            tPlanningDateReview.IdCounterpart = 0;
                            tPlanningDateReview.PlannedDeliveryDate = Convert.ToDateTime(e.HitInterval.Start);
                            tPlanningDateReview.CreatedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                            tPlanningDateReview.CreatedIn = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                            tPlanningDateReview.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);

                            PlanningDateReviewList.Add(tPlanningDateReview);
                        }
                        else
                        {
                            // PlanningDateReviewList.Where(y => y.IDOT == tempIdOT && y.IdCounterpart == tmpIdCounterpart).ToList().ForEach(i => i.PlannedDeliveryDate = Convert.ToDateTime(e.HitInterval.Start));
                            // PlanningDateReviewList.Where(y => y.IDOT == tempIdOT && y.IdCounterpart == tmpIdCounterpart).ToList().ForEach(i => i.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser));
                            PlanningDateReviewList.Where(y => y.IDOT == tempIdOT).ToList().ForEach(i => i.PlannedDeliveryDate = Convert.ToDateTime(e.HitInterval.Start));
                            PlanningDateReviewList.Where(y => y.IDOT == tempIdOT).ToList().ForEach(i => i.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser));
                            PlanningDateReviewList.Where(y => y.IDOT == tempIdOT).ToList().ForEach(i => i.ModifiedIn = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")));
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
                // if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                GeosApplication.Instance.Logger.Log("Method RefreshPlanningDateCommandAction()...", category: Category.Info, priority: Priority.Low);
                if (IsBand) 
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
                    SearchOTItem = string.Empty;
                    PlanningDateReviewList = null;

                    PlanningDateReviewList = new List<PlanningDateReview>();
                    GetPlanningDateRecordByPlantAndDate(SelectedPlant, FromDate, ToDate);
                }
                if(IsGrid)  //[GEOS2-4143] [Pallavi Jadhav] [15 03 2023]
                {
                    view = PlanningDateReviewCellEditHelper.Viewtableview;
                    if (PlanningDateReviewCellEditHelper.IsValueChanged)
                    {
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
                  ShowPlanningDateGridView(null);
                }
            // if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
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

                bool flag;
              //  ERMService = new ERMServiceController("localhost:6699");
                flag = ERMService.AddUpdatePlanningDateReview(PlanningDateReviewList);

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
                if (SelectedItemOld == null)
                    SelectedItemOld = new object();
                SelectedItemOld = SelectedItem;

                GetPlanningDateRecordByPlantAndDate(SelectedPlant, FromDate, ToDate);
                //FillDeliveryweek();
                //     SelectedItem = ProductionPlanningReviewList.Where(x => x.PlanningDeliveryDate == Convert.ToDateTime(AccordianFromDate, CurrentCulture)).FirstOrDefault();
                // SelectedItem = ProductionPlanningReviewList.Where(x => x.DeliveryDate == Convert.ToDateTime(SelectedItemOld.DeliveryDate)).FirstOrDefault();

                if (SelectedItem == null)
                    SelectedItem = new object();
                SelectedItem = SelectedItemOld;

               ShowCompanyHolidayAppointmentsForSelectedCompany();
               // PlanningDateReviewList = new List<PlanningDateReview>();
            }
            catch (Exception ex)
            {

                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshControls()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        private void SearchOTCommandAction(object obj)
        //private void SearchOTCommandAction()
        {
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                GeosApplication.Instance.Logger.Log("Method RefreshPlanningDateCommandAction()...", category: Category.Info, priority: Priority.Low);
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
                        modelActivity.ContentSubject = item.OTCode + "; " + item.NumItem + "; " + item.Type + ";" + item.QTY;
                        //modelActivity.Status = Convert.ToInt32(item.IdCPType);
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

        //private void LostFocusCommandAction(RoutedEventArgs e)
        //{


        //}
        private void DefaultUnLoadCommandAction(RoutedEventArgs obj)
        {

        }

        private void TableViewLoadedCommandAction(RoutedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method TableViewLoadedCommandAction ...", category: Category.Info, priority: Priority.Low);
                int visibleFalseCoulumn = 0;

                if (File.Exists(PendingPlanningDateReviewGridSettingFilePath))
                {
                    ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.RestoreLayoutFromXml(PendingPlanningDateReviewGridSettingFilePath);
                    GridControl GridControlSTDetails = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;
                  
                    TableView tableView = (TableView)GridControlSTDetails.View;
                    if (tableView.SearchString != null)
                    {
                        tableView.SearchString = null;
                    }
                }

                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.AddHandler(DXSerializer.AllowPropertyEvent, new AllowPropertyEventHandler(OnAllowProperty));

                //This code for save grid layout.
                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.SaveLayoutToXml(PendingPlanningDateReviewGridSettingFilePath);

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
                        visibleFalseCoulumn++;
                    }
                }

                FillERMDataInObjectsByCallingLatestServiceMethods.ShowPleaseWaitScreen();
                List<GeosAppSetting> PendingPOColorList = WorkbenchStartUp.GetSelectedGeosAppSettings("2,3,4,5");
                //  IsHolidayDate = Visibility.Collapsed;


                FillERMDataInObjectsByCallingLatestServiceMethods.GetERMDataOnceFromServiceForAttendance(
               ref ERMService, ref CrmStartUp,
                 ref companyHolidays,
               ref holidayList, ref labelItems, ref PendingPOColorList,
               ref appointmentItems, ref fromDate, ref toDate
                );

                // AppointmentItems.Add(appointmentItems);
                // AppointmentItems = new ObservableCollection<UI.Helper.PlanningAppointment>(AppointmentItems);

                FillERMDataInObjectsByCallingLatestServiceMethods.ClosePleaseWaitScreen();
                // ShowCompanyHolidayAppointmentsForSelectedCompany();
            
                GeosApplication.Instance.Logger.Log("Method TableViewLoadedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in TableViewLoadedCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
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
                    ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(PendingPlanningDateReviewGridSettingFilePath);
                }

                if (column.Visible == false)
                {
                    //  IsWorkOrderColumnChooserVisible = true;
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
                ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(PendingPlanningDateReviewGridSettingFilePath);

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

            DevExpress.Xpf.Grid.CellValueChangedEventArgs tempObj = (DevExpress.Xpf.Grid.CellValueChangedEventArgs)obj;
           
            //ProductionPlanningReview ProductionPlanningReviewTemp = new ProductionPlanningReview();
            //ProductionPlanningReviewTemp = ProductionPlanningReviewList.FirstOrDefault();
            DateTime? tmpPlanningDeliveryDate = Convert.ToDateTime(tempObj.Value);
            ProductionPlanningReview ProductionPlanningReviewTemp = (ProductionPlanningReview)tempObj.Row;
            
            DateTime? tmpDeliveryDate = ProductionPlanningReviewTemp.DeliveryDate;
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
            if (tmpPlanningDeliveryDate < DateTime.Now)
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

        private void PrintPlanningDateReviewAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintLeavesList()...", category: Category.Info, priority: Priority.Low);
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
                GeosApplication.Instance.Logger.Log("Method PrintLeavesList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintLeavesList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        private void ExportPlanningDateReviewCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportLeavesList()...", category: Category.Info, priority: Priority.Low);
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
                    LeavesTableView.ShowTotalSummary = false;
                    LeavesTableView.ShowFixedTotalSummary = true;
                }
                GeosApplication.Instance.Logger.Log("Method ExportLeavesList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportLeavesList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        //[GEOS2-4143] [Pallavi Jadhav] [15 03 2023]
        public void SaveGridPlanningDateCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SaveGridPlanningDateCommandAction()...", category: Category.Info, priority: Priority.Low);
                //view = PlanningDateReviewCellEditHelper.Viewtableview;
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
                    // _PlanningDateReview.ModifiedIn = DateTime.Now;
                    _PlanningDateReview.CreatedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                    _PlanningDateReview.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                    bool flag;
                    
                 //   ERMService = new ERMServiceController("localhost:6699");
                    flag = ERMService.AddUpdatePlanningDateReview_V2370(_PlanningDateReview);
                }


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

                //IsSaveEnabled = false;
                //PlanningAppointment.IsSaveButtonEnabled = false;
                PlanningDateReviewCellEditHelper.IsValueChanged = false;

                if (view != null)
                {
                    PlanningDateReviewCellEditHelper.SetIsValueChanged(view, false);
                }
                ShowPlanningDateGridView(null);

                
            }
            catch (Exception ex)
            {

                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshControls()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CellValueUpdatedCommnadAction(CellValueChangedEventArgs obj)
        {
            

            obj.Source.PostEditor();
           
        }
      
    }
}
