using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Charts;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid.TreeList;
using DevExpress.Xpf.Scheduler;
using DevExpress.Xpf.Scheduler.Menu;
using DevExpress.Xpf.Scheduler.UI;
using Emdep.Geos.Modules.Crm.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Microsoft.Win32;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections.ObjectModel;
using Emdep.Geos.Data.Common;
using System.ServiceModel;
using Emdep.Geos.Data.Common.Epc;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using DevExpress.Xpf.Core.Serialization;
using Emdep.Geos.UI.Helper;
using DevExpress.Xpf.Spreadsheet;
using DevExpress.Spreadsheet;
using DevExpress.XtraPrinting;
using DevExpress.Export.Xl;
using System.IO;
using DevExpress.Xpf.Scheduler.Drawing;
using DevExpress.XtraScheduler;
using System.Windows.Media.Imaging;
using DevExpress.Data.Filtering;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Threading;
using Newtonsoft.Json;


namespace Emdep.Geos.Modules.Crm.ViewModels
{
    public class ActivityViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable
    {
        #region Services

        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryServiceStartUp = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion // Services

        #region Declaration

        //List<NewCompany> companies;

        public string AccountGridSettingFilePath = GeosApplication.Instance.UserSettingFolderName + "AccountGridSetting.Xml";
        bool isBusy;
        private string myFilterString;
        private ObservableCollection<Activity> activityList;
        private ObservableCollection<ActivityGrid> activitygridList;
        private Activity selectedObject;
        private ActivityGrid selectedObjectActivityGrid;
        Boolean isShowFailedPlantWarning;
        string warningFailedPlants;
        private AppointmentLabelCollection labels;
        private Visibility isGridVisible;
        private Visibility isFilterSearchVisible;
        private Visibility isCalendarVisible;
        private List<TempAppointment> appointmentsMainList;
        private List<TempAppointment> appointmentsFilteredList;
        private List<TempAppointment> appointmentsFilteredBackUpList;
        private string activityFilterCriteria;

        private List<object> selectedActivityTask;
        private List<object> selectedActivityTaskStatus;
        private List<object> selectedSalesOwnerList;

        private List<object> selectedCompaniesList;
        private List<Company> companiesList;
        private List<Company> perviousSelectedCompanyList;
        private List<UserManagerDtl> salesOwnerList;
        private bool isInit;
        private bool isActivityColumnChooserVisible;
        private Visibility isMultipleDeleteVisible;
        //Resource
        private List<Emdep.Geos.Data.Common.ResourceStorage> resourceStorageList;

        public List<Emdep.Geos.Data.Common.ResourceStorage> ResourceStorageList
        {
            get
            {
                return resourceStorageList;
            }

            set
            {
                resourceStorageList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ResourceStorageList"));
            }
        }
        Dictionary<Int32, BitmapImage> ResourceImages = new Dictionary<Int32, BitmapImage>();

        private ObservableCollection<People> attendeesList;

        #endregion // Declaration

        #region  public Properties

        public string ServiceUrl { get; set; }
        public ObservableCollection<People> AttendeesList
        {
            get { return attendeesList; }
            set
            {
                attendeesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AttendeesList"));
            }
        }
        public bool IsActivityColumnChooserVisible
        {
            get { return isActivityColumnChooserVisible; }
            set
            {
                isActivityColumnChooserVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsActivityColumnChooserVisible"));
            }
        }
        public IList<LookupValue> ActivityTypeList { get; set; }
        public List<LookupValue> ActivityTaskStatusList { get; set; }
        public List<LogEntriesByActivity> ChangedLogsEntries { get; set; }

        public bool IsInit
        {
            get { return isInit; }
            set { isInit = value; }
        }

        public List<UserManagerDtl> SalesOwnerList
        {
            get
            {
                return salesOwnerList;
            }

            set
            {
                salesOwnerList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SalesOwnerList"));
            }
        }

        public List<Company> CompaniesList
        {
            get { return companiesList; }
            set
            {
                companiesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CompaniesList"));
            }
        }

        public List<Company> PerviousSelectedCompanyList
        {
            get { return perviousSelectedCompanyList; }
            set
            {
                perviousSelectedCompanyList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PerviousSelectedCompanyList"));
            }
        }

        public List<object> SelectedCompaniesList
        {
            get { return selectedCompaniesList; }
            set
            {
                selectedCompaniesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCompaniesList"));
            }
        }
        public List<object> SelectedSalesOwnerList
        {
            get
            {
                return selectedSalesOwnerList;
            }

            set
            {
                selectedSalesOwnerList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedSalesOwnerList"));
            }
        }
        public List<object> SelectedActivityTask
        {
            get
            {
                return selectedActivityTask;
            }

            set
            {
                selectedActivityTask = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedActivityTask"));
            }
        }

        public List<object> SelectedActivityTaskStatus
        {
            get
            {
                return selectedActivityTaskStatus;
            }

            set
            {
                selectedActivityTaskStatus = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedActivityTaskStatus"));
            }
        }

        /// <summary>
        /// Apply Filter as per Criteria.
        /// </summary>
        public string ActivityFilterCriteria
        {
            get { return activityFilterCriteria; }
            set
            {
                activityFilterCriteria = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ActivityFilterCriteria"));

                try
                {
                    if (string.IsNullOrEmpty(activityFilterCriteria) || string.IsNullOrWhiteSpace(activityFilterCriteria))
                    {
                        AppointmentsFilteredList = new List<TempAppointment>(AppointmentsFilteredBackUpList);
                    }
                    else
                    {
                        List<TempAppointment> _TempAppointmentsFilteredList = new List<TempAppointment>();

                        _TempAppointmentsFilteredList = AppointmentsFilteredBackUpList.Where(ap => ap.Subject.ToUpper().Contains(activityFilterCriteria.Trim().ToUpper())).Select(app => app).ToList();
                        AppointmentsFilteredList = new List<TempAppointment>(_TempAppointmentsFilteredList);
                    }

                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in ActivityFilterCriteria Property set " + ex.Message, category: Category.Exception, priority: Priority.Low);
                }
            }
        }

        public List<TempAppointment> AppointmentsMainList
        {
            get { return appointmentsMainList; }
            set
            {
                appointmentsMainList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Appointments"));
            }
        }

        public List<TempAppointment> AppointmentsFilteredList
        {
            get { return appointmentsFilteredList; }
            set
            {
                appointmentsFilteredList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AppointmentsFilteredList"));
            }
        }

        public List<TempAppointment> AppointmentsFilteredBackUpList
        {
            get { return appointmentsFilteredBackUpList; }
            set
            {
                appointmentsFilteredBackUpList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AppointmentsFilteredBackUpList"));
            }
        }

        public Visibility IsGridVisible
        {
            get
            {
                return isGridVisible;
            }

            set
            {
                isGridVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsGridVisible"));
            }
        }
        public Visibility IsFilterSearchVisible
        {
            get
            {
                return isFilterSearchVisible;
            }

            set
            {
                isFilterSearchVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsFilterSearchVisible"));
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


        public string WarningFailedPlants
        {
            get { return warningFailedPlants; }
            set
            {
                warningFailedPlants = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WarningFailedPlants"));
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
        public Activity SelectedObject
        {
            get { return selectedObject; }
            set
            {
                selectedObject = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedObject"));
            }
        }
        public ActivityGrid SelectedObjectActivityGrid
        {
            get { return selectedObjectActivityGrid; }
            set
            {
                selectedObjectActivityGrid = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedObjectActivityGrid"));
            }
        }
        public ObservableCollection<Activity> ActivityList
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
        public ObservableCollection<ActivityGrid> ActivityGridList
        {
            get
            {
                return activitygridList;
            }

            set
            {
                activitygridList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ActivityGridList"));
            }
        }
        public string PreviouslySelectedSalesOwners { get; set; }
        public string PreviouslySelectedPlantOwners { get; set; }


        public string MyFilterString
        {
            get { return myFilterString; }
            set
            {
                myFilterString = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MyFilterString"));
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

        public Visibility IsMultipleDeleteVisible
        {
            get
            {
                return isMultipleDeleteVisible;
            }

            set
            {
                isMultipleDeleteVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsMultipleDeleteVisible"));
            }
        }
        // Export Excel .xlsx
        public virtual string ResultFileName { get; set; }
        public virtual bool DialogResult { get; set; }

        #endregion // public Properties.

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

        #region Public Commands

        public ICommand CustomCellAppearanceCommand { get; private set; }
        public ICommand PrintButtonCommand { get; set; }
        public ICommand CommandNewActivityClick { get; set; }
        public ICommand ActivitiesGridDoubleClickCommand { get; set; }
        public ICommand SalesOwnerUnboundDataCommand { get; set; }
        public ICommand SalesOwnerCustomShowFilterPopup { get; set; }
        public ICommand SalesOwnerPopupClosedCommand { get; private set; }
        public ICommand CalendarSalesOwnerPopupClosedCommand { get; private set; }
        public ICommand CalendarAccountPopupClosedCommand { get; private set; }
        public ICommand PlantOwnerPopupClosedCommand { get; private set; }
        public ICommand RefreshActivityViewCommand { get; set; }
        public ICommand ExportButtonCommand { get; set; }
        public ICommand AllowEditAppointmentCommand { get; private set; }
        public ICommand PopupMenuShowingCommand { get; private set; }
        public ICommand ShowGridViewCommand { get; private set; }
        public ICommand ShowCalanderviewCommand { get; private set; }
        public ICommand DeleteActivityCommand { get; private set; }
        public ICommand FilterActivityAcceptCommand { get; private set; }
        public ICommand FilterActivityCancelCommand { get; private set; }
        public ICommand ActivityAttendeesCustomShowFilterPopup { get; set; }
        public ICommand DeleteSelectedRowsButtonCommand { get; set; }

        #endregion // Commands

        #region Constructor

        public ActivityViewModel()
        {
            try
            {
                if (!DXSplashScreen.IsActive)
                    DXSplashScreen.Show<SplashScreenView>();

                GetServiceUrl();

                IsInit = true;
                GeosApplication.Instance.Logger.Log("Constructor ActivityViewModel ...", category: Category.Info, priority: Priority.Low);

                IsShowFailedPlantWarning = false;
                WarningFailedPlants = String.Empty;

                ActivitiesGridDoubleClickCommand = new DelegateCommand<object>(EditActivityViewWindowShow);
                PrintButtonCommand = new Prism.Commands.DelegateCommand<object>(PrintAction);
                CommandNewActivityClick = new RelayCommand(new Action<object>(AddActivityViewWindowShow));
                RefreshActivityViewCommand = new RelayCommand(new Action<object>(RefreshActivityDetails));
                PlantOwnerPopupClosedCommand = new DevExpress.Mvvm.DelegateCommand<object>(PlantOwnerPopupClosedCommandAction);
                //SalesOwnerCustomShowFilterPopup = new DelegateCommand<FilterPopupEventArgs>(SalesOwnerCustomShowFilterPopupAction);
                SalesOwnerPopupClosedCommand = new DevExpress.Mvvm.DelegateCommand<object>(SalesOwnerPopupClosedCommandAction);
                CustomCellAppearanceCommand = new DevExpress.Mvvm.DelegateCommand<RoutedEventArgs>(CustomCellAppearanceGridControl);
                CalendarSalesOwnerPopupClosedCommand = new DevExpress.Mvvm.DelegateCommand<object>(CalendarActivitySalesOwnerCommandAction);
                CalendarAccountPopupClosedCommand = new DevExpress.Mvvm.DelegateCommand<object>(CalendarAccountPopupClosedCommandAction);
                ExportButtonCommand = new RelayCommand(new Action<object>(ExportActivityGridButtonCommandAction));
                DeleteActivityCommand = new DelegateCommand<object>(DeleteActivityRowCommandAction);
                AllowEditAppointmentCommand = new DevExpress.Mvvm.DelegateCommand<EditAppointmentFormEventArgs>(AllowEditAppointmentAction);
                PopupMenuShowingCommand = new DevExpress.Mvvm.DelegateCommand<SchedulerMenuEventArgs>(PopupMenuShowingAction);

                ShowGridViewCommand = new RelayCommand(new Action<object>(ShowGridView));
                ShowCalanderviewCommand = new RelayCommand(new Action<object>(ShowCalanderview));

                FilterActivityAcceptCommand = new RelayCommand(new Action<object>(FilterActivityOk));
                FilterActivityCancelCommand = new RelayCommand(new Action<object>(FilterActivityCancel));

                ResourceStorageList = new List<Emdep.Geos.Data.Common.ResourceStorage>();
                ActivityAttendeesCustomShowFilterPopup = new DelegateCommand<FilterPopupEventArgs>(ActivityAttendeesCustomShowFilterPopupAction);
                DeleteSelectedRowsButtonCommand=new RelayCommand(new Action<object>(DeleteMultipleActivities));

                FillAppointmentLabel();
                FillCmbSalesOwner();
                FillActivityFilterLists();
                FillSalesOwnerAndAcccount(true);
                FillAttendiesList();


                MyFilterString = string.Empty;
                ChangedLogsEntries = new List<LogEntriesByActivity>();
                IsGridVisible = Visibility.Visible;
                IsCalendarVisible = Visibility.Collapsed;
                IsFilterSearchVisible = Visibility.Collapsed;
                IsActivityColumnChooserVisible = true;
                IsInit = false;
                GeosApplication.Instance.Logger.Log("Constructor ActivityViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ActivityViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }




        #endregion // Constructor.

        #region Methods
        private void GetServiceUrl()
        {
            try
            {
                string ServicePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Emdep\Geos\" + "config.txt";
                if (File.Exists(ServicePath))
                {
                    StreamReader reader = new StreamReader(ServicePath);
                    ServiceUrl = reader.ReadLine();
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        /// <summary>
        /// Fill Attendies List
        /// </summary>
        private void FillAttendiesList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillAttendiesList ...", category: Category.Info, priority: Priority.Low);

                AttendeesList = new ObservableCollection<People>(CrmStartUp.GetAllAttendesList("0").ToList());

                GeosApplication.Instance.Logger.Log("Method FillAttendiesList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillAttendiesList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillAttendiesList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillAttendiesList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// This method is used to Display the Custom Filter for Activity Attendees as 
        /// to display values of multiple properties in a single column.
        /// </summary>
        /// <param name="e"></param>
        private void ActivityAttendeesCustomShowFilterPopupAction(FilterPopupEventArgs e)
        {
            GeosApplication.Instance.Logger.Log(" Method ActivityAttendeesCustomShowFilterPopupAction ...", category: Category.Info, priority: Priority.Low);

            if (e.Column.FieldName != "ActivityAttendeesString")
            {
                return;
            }

            try
            {
                List<object> filterItems = new List<object>();

                if (e.Column.FieldName == "ActivityAttendeesString")
                {
                    //filterItems.Add(new CustomComboBoxItem()
                    //{
                    //    DisplayValue = "(All)",
                    //    EditValue = new CustomComboBoxItem()
                    //});
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("ActivityAttendeesString = ''")   // ActivityAttendeesString is equal to ' '
                    });
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("ActivityAttendeesString <> ''")  // ActivityAttendeesString does not equal to ' '
                    });

                    foreach (People item in AttendeesList)
                    {
                        CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                        customComboBoxItem.DisplayValue = item.FullName.Trim();
                        customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("ActivityAttendeesString Like '%{0}%'", item.FullName.Trim()));
                        filterItems.Add(customComboBoxItem);
                    }
                }

                e.ComboBoxEdit.ItemsSource = filterItems.OrderBy(sort => Convert.ToString(((CustomComboBoxItem)sort).DisplayValue)).ToList();

                GeosApplication.Instance.Logger.Log("Method ActivityAttendeesCustomShowFilterPopupAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ActivityAttendeesCustomShowFilterPopupAction() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillSalesOwnerAndAcccount(bool isSelected)
        {
            GeosApplication.Instance.Logger.Log("Method FillSalesOwnerAndAcccount ...", category: Category.Info, priority: Priority.Low);

            try
            {

                //fill account as per user conditions.
                if (GeosApplication.Instance.IdUserPermission == 21)
                {
                    if (GeosApplication.Instance.SelectedSalesOwnerUsersList != null && SelectedSalesOwnerList != null && SelectedSalesOwnerList.Count > 0)
                    {
                        List<UserManagerDtl> salesOwners = SelectedSalesOwnerList.Cast<UserManagerDtl>().ToList();
                        var salesOwnersIds = string.Join(",", salesOwners.Select(i => i.IdUser));
                        //Account
                        CompaniesList = new List<Company>(CrmStartUp.GetSelectedUserCustomerPlant(salesOwnersIds).ToList());
                        if (isSelected)
                        {
                            SelectedCompaniesList = new List<object>();
                            SelectedCompaniesList.AddRange(CompaniesList);
                        }

                    }
                    else
                    {

                        CompaniesList = new List<Company>();
                        SelectedCompaniesList = new List<object>();

                    }


                }
                else if (GeosApplication.Instance.IdUserPermission == 22)
                {
                    if (SelectedSalesOwnerList != null)
                    {
                        //Account
                        CompaniesList = new List<Company>(CrmStartUp.GetCustomerPlant(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission).ToList());
                        if (isSelected)
                        {
                            SelectedCompaniesList = new List<object>();
                            SelectedCompaniesList.AddRange(CompaniesList);
                        }

                    }

                    else
                    {
                        CompaniesList = new List<Company>();
                        SelectedCompaniesList = new List<object>();
                    }

                }
                else
                {
                    if (SelectedSalesOwnerList != null)
                    {
                        //Account
                        CompaniesList = new List<Company>(CrmStartUp.GetCustomerPlant(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission).ToList());
                        if (isSelected)
                        {
                            SelectedCompaniesList = new List<object>();
                            SelectedCompaniesList.AddRange(CompaniesList);
                        }
                    }
                    else
                    {
                        CompaniesList = new List<Company>();
                        SelectedCompaniesList = new List<object>();
                    }
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DeleteActivityRowCommandAction() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DeleteActivityRowCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in DeleteActivityRowCommandAction() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            if (!isSelected)
            {
                if (PerviousSelectedCompanyList != null)
                {
                    List<Int32> LstCompanyids = PerviousSelectedCompanyList.Select(pc => pc.IdCompany).ToList();
                    SelectedCompaniesList = new List<object>();
                    SelectedCompaniesList.AddRange(CompaniesList.Where(cl => LstCompanyids.Contains(cl.IdCompany)).ToList());
                }
            }
            GeosApplication.Instance.Logger.Log("Method FillSalesOwnerAndAcccount() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        private void CalendarActivitySalesOwnerCommandAction(object obj)
        {
            if (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
            {
                return;
            }


            FillSalesOwnerAndAcccount(false);


        }

        private void CalendarAccountPopupClosedCommandAction(object obj)
        {
            if (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
            {
                return;
            }

            if (SelectedCompaniesList != null)
            {
                PerviousSelectedCompanyList = new List<Company>();
                PerviousSelectedCompanyList = new List<Company>(SelectedCompaniesList.Cast<Company>().ToList());
            }
        }


        private void FilterActivityOk(object obj)
        {

            try
            {
                GeosApplication.Instance.Logger.Log("Method FilterActivityOk ...", category: Category.Info, priority: Priority.Low);

                int _filterCount = 0;
                List<ActivityGrid> TempActivityList = new List<ActivityGrid>(ActivityGridList);

                if (SelectedActivityTask != null && SelectedActivityTask.Count > 0)
                {
                    List<int> idActivityTaskList = SelectedActivityTask.Cast<LookupValue>().ToList().Select(sa => sa.IdLookupValue).ToList();

                    TempActivityList = TempActivityList.Where(sa => idActivityTaskList.Contains(Convert.ToInt32(sa.IdActivityType))).ToList();
                    _filterCount++;
                }

                if (SelectedActivityTaskStatus != null && SelectedActivityTaskStatus.Count > 0)
                {
                    List<int> idActivityTaskStatusList = SelectedActivityTaskStatus.Cast<LookupValue>().ToList().Select(sa => sa.IdLookupValue).ToList();

                    TempActivityList = TempActivityList.Where(sa => idActivityTaskStatusList.Contains(Convert.ToInt32(sa.IdActivityStatus))).ToList();
                    _filterCount++;
                }

                if (SelectedSalesOwnerList != null && SelectedSalesOwnerList.Count > 0)
                {
                    List<int> idSaleOwnerList = SelectedSalesOwnerList.Cast<UserManagerDtl>().ToList().Select(sa => sa.IdUser).ToList();

                    TempActivityList = TempActivityList.Where(sa => idSaleOwnerList.Contains(Convert.ToInt32(sa.IdOwner))).ToList();
                    _filterCount++;
                }

                if (SelectedCompaniesList != null && SelectedCompaniesList.Count > 0)
                {
                    List<int> idCompanyList = SelectedCompaniesList.Cast<Company>().ToList().Select(sa => sa.IdCompany).ToList();

                    TempActivityList = TempActivityList.Where(sa => idCompanyList.Contains(Convert.ToInt32(sa.IdSite))).ToList();

                    _filterCount++;
                }

                // if all filter are not apply then clear data from TempActivityList.
                if (_filterCount == 0)
                {
                    TempActivityList = new List<ActivityGrid>();
                }

                List<TempAppointment> modelAppointmentList = new List<TempAppointment>();

                foreach (ActivityGrid item in TempActivityList)
                {
                    TempAppointment modelAppointment = new TempAppointment();

                    modelAppointment.IdOffer = item.IdActivity;
                    modelAppointment.Description = item.Description;
                    modelAppointment.ResourceId = item.IdOwner;
                    modelAppointment.Owner = item.FullName;
                    modelAppointment.StartTime = item.FromDate;
                    modelAppointment.EndTime = item.ToDate;
                    modelAppointment.Label = item.IdActivityType;

                    if (item.IsCompleted == 0)
                        modelAppointment.Status = 0;
                    if (item.IsCompleted == 1)
                        modelAppointment.Status = 1;

                    modelAppointment.Tag = item.ActivityType;

                    if (!ResourceStorageList.Exists(x => x.Id == item.IdOwner))
                    {
                        Emdep.Geos.Data.Common.ResourceStorage resourceStorage = new Emdep.Geos.Data.Common.ResourceStorage();
                        resourceStorage.Id = item.IdOwner;
                        resourceStorage.Model = item.Login;
                        resourceStorage.Picture = GeosRepositoryServiceStartUp.GetUserProfileImageWithoutException(item.Login);
                        ResourceStorageList.Add(resourceStorage);
                    }

                    if (item.IdActivityType == 37 || item.IdActivityType == 96)
                    {
                        if (item.SiteNameWithoutCountry != null && item.SiteNameWithoutCountry != string.Empty)
                        {
                            modelAppointment.Subject = item.SiteNameWithoutCountry;
                            modelAppointment.TooltipTitle = item.SiteNameWithoutCountry;
                            modelAppointment.TooltipSubject = item.Subject;
                        }
                        else
                        {
                            modelAppointment.Subject = item.Subject;
                            modelAppointment.TooltipSubject = item.Subject;
                        }
                    }
                    else
                    {
                        modelAppointment.Subject = item.Subject;
                        modelAppointment.TooltipSubject = item.Subject;
                    }
                    modelAppointmentList.Add(modelAppointment);
                }

                AppointmentsFilteredBackUpList = new List<TempAppointment>(modelAppointmentList);
                AppointmentsFilteredList = new List<TempAppointment>(modelAppointmentList);
                ActivityFilterCriteria = "";

                if (((DropDownButton)obj) != null)
                    ((DropDownButton)obj).IsPopupOpen = false;

                GeosApplication.Instance.Logger.Log("Method FilterActivityOk() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in FilterActivityOk() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FilterActivityCancel(object obj)
        {
            ((DropDownButton)obj).IsPopupOpen = false;
        }


        /// <summary>
        /// Method for show grid view.
        /// </summary>
        /// <param name="obj"></param>
        private void ShowGridView(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method ShowGridView ...", category: Category.Info, priority: Priority.Low);

            IsGridVisible = Visibility.Visible;
            IsCalendarVisible = Visibility.Collapsed;
            IsFilterSearchVisible = Visibility.Collapsed;
            IsMultipleDeleteVisible = Visibility.Visible;
            // code for hide column chooser if empty
            TableView detailView = ((TableView)obj);
            detailView.SearchString = null;

            GridControl gridControl = (detailView).Grid;


            int visibleFalseCoulumn = 0;
            visibleFalseCoulumn = gridControl.Columns.Where(col => col.Visible == false).Count();
            if (visibleFalseCoulumn > 0)
            {
                IsActivityColumnChooserVisible = true;
            }
            else
            {
                IsActivityColumnChooserVisible = false;
            }

            GeosApplication.Instance.Logger.Log("Method ShowGridView() executed successfully", category: Category.Info, priority: Priority.Low);

        }

        /// <summary>
        /// Method for show Calander view.
        /// </summary>
        /// <param name="obj"></param>
        private void ShowCalanderview(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method ShowCalanderview ...", category: Category.Info, priority: Priority.Low);
            IsActivityColumnChooserVisible = false;
            IsGridVisible = Visibility.Collapsed;
            IsCalendarVisible = Visibility.Visible;
            IsFilterSearchVisible = Visibility.Visible;
            IsMultipleDeleteVisible = Visibility.Collapsed;
            GeosApplication.Instance.Logger.Log("Method ShowCalanderview() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method for hide option from Calendar view on Right click.
        /// </summary>
        /// <param name="parameter"></param>
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
        /// Method for edit activity from Calendar View.
        /// </summary>
        /// <param name="obj"></param>
        private void AllowEditAppointmentAction(EditAppointmentFormEventArgs obj)
        {

            try
            {
                GeosApplication.Instance.Logger.Log("Method AllowEditAppointmentAction...", category: Category.Info, priority: Priority.Low);
                obj.Cancel = true;
                if (obj == null) return;
                if (Convert.ToInt64(obj.Appointment.Id) > 0)
                {
                    ActivityGrid activity = ActivityGridList.FirstOrDefault(Act => Act.IdActivity == (Convert.ToInt64(obj.Appointment.Id)));

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

                    Activity tempActivity = new Activity();

                    tempActivity = CrmStartUp.GetActivityByIdActivity_V2035(activity.IdActivity);
                    EditActivityViewModel editActivityViewModel = new EditActivityViewModel();
                    if (tempActivity.LookupValue.IdLookupValue == 96)
                    {
                        List<ActivityGrid> LstActivity = new List<ActivityGrid>();
                        LstActivity = ActivityGridList.Where(al => al.IdActivityType == 96 && al.IdOwner == GeosApplication.Instance.ActiveUser.IdUser).ToList().OrderBy(d => d.ToDate).ToList();
                        if (LstActivity.Count > 1)
                        {
                            editActivityViewModel.PreviousPlannedDate = LstActivity.FirstOrDefault().ToDate.Value;
                            editActivityViewModel.NextPlannedDate = LstActivity.LastOrDefault().ToDate.Value;
                        }
                    }
                    if (editActivityViewModel.PreviousPlannedDate == DateTime.MinValue && editActivityViewModel.NextPlannedDate == DateTime.MinValue)
                    {
                        editActivityViewModel.PreviousPlannedDate = GeosApplication.Instance.SelectedyearStarDate;
                        editActivityViewModel.NextPlannedDate = GeosApplication.Instance.SelectedyearEndDate;
                    }
                    EditActivityView editActivityView = new EditActivityView();
                    editActivityViewModel.Init(tempActivity);

                    EventHandler handle = delegate { editActivityView.Close(); };
                    editActivityViewModel.RequestClose += handle;
                    editActivityView.DataContext = editActivityViewModel;

                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                    editActivityView.ShowDialogWindow();

                    if (editActivityViewModel.objActivity != null)
                    {
                        // code for update detail on Grid view.
                        activity.Subject = editActivityViewModel.objActivity.Subject;
                        activity.Description = editActivityViewModel.objActivity.Description;
                        activity.ActivityType = editActivityViewModel.objActivity.LookupValue.Value;
                        activity.IdActivityType = editActivityViewModel.objActivity.LookupValue.IdLookupValue;
                        activity.IdImageActivityType = editActivityViewModel.objActivity.LookupValue.IdImage;
                        activity.Location = editActivityViewModel.objActivity.Location;
                        activity.ToDate = editActivityViewModel.objActivity.ToDate;
                        activity.FromDate = editActivityViewModel.objActivity.FromDate;
                        activity.IsCompleted = editActivityViewModel.objActivity.IsCompleted;
                        activity.ActivityTagsString = editActivityViewModel.objActivity.ActivityTagsString;
                        activity.ActivityAttendeesString = editActivityViewModel.objActivity.ActivityAttendeesString;
                        activity.FullName = editActivityViewModel.objActivity.People.FullName;
                        activity.Login = editActivityViewModel.objActivity.People.Login;


                        activity.CustomerName = editActivityViewModel.objActivity.ActivityLinkedItem[0].Customer.CustomerName;
                        activity.CompanyName = editActivityViewModel.objActivity.ActivityLinkedItem[0].Company.Name;
                        activity.SiteNameWithoutCountry = editActivityViewModel.objActivity.ActivityLinkedItem[0].Company.SiteNameWithoutCountry;

                        //activity.IsInternal = editActivityViewModel.objActivity.IsInternal;

                        if (activity.IsCompleted == 1)
                        {
                            activity.ActivityGridStatus = "Completed";
                            activity.CloseDate = GeosApplication.Instance.ServerDateTime;

                        }
                        else
                        {
                            activity.ActivityGridStatus = editActivityViewModel.objActivity.ActivityStatus != null ? editActivityViewModel.objActivity.ActivityStatus.Value : "";
                            activity.CloseDate = null;
                        }

                        // code for update detail on calendar view.
                        TempAppointment modelActivity = AppointmentsMainList.FirstOrDefault(Act => Act.IdOffer == (Convert.ToInt64(obj.Appointment.Id)));
                        TempAppointment modelActivityFilteredBackUp = AppointmentsFilteredBackUpList.FirstOrDefault(Act => Act.IdOffer == (Convert.ToInt64(obj.Appointment.Id)));
                        TempAppointment modelActivityFiltered = AppointmentsFilteredList.FirstOrDefault(Act => Act.IdOffer == (Convert.ToInt64(obj.Appointment.Id)));

                        if (modelActivity != null)
                        {
                            modelActivity.Tag = editActivityViewModel.objActivity.LookupValue.Value;
                            modelActivity.Label = editActivityViewModel.objActivity.IdActivityType;
                            modelActivity.ResourceId = editActivityViewModel.objActivity.IdOwner;

                            if (!ResourceStorageList.Exists(x => x.Id == editActivityViewModel.objActivity.IdOwner))
                            {
                                Emdep.Geos.Data.Common.ResourceStorage resourceStorage = new Emdep.Geos.Data.Common.ResourceStorage();
                                resourceStorage.Id = editActivityViewModel.objActivity.IdOwner;
                                resourceStorage.Model = editActivityViewModel.objActivity.People.Login;
                                //resourceStorage.Picture = editActivityViewModel.objActivity.People.ImageBytes;
                                resourceStorage.Picture = GeosRepositoryServiceStartUp.GetUserProfileImageWithoutException(editActivityViewModel.objActivity.People.Login);
                                ResourceStorageList.Add(resourceStorage);
                            }

                            if (editActivityViewModel.objActivity.LookupValue.IdLookupValue == 37 && editActivityViewModel.objActivity.IsInternal == 0)
                            {
                                if (editActivityViewModel.objActivity.ActivityLinkedItem != null && editActivityViewModel.objActivity.ActivityLinkedItem.Count > 0)
                                {
                                    ActivityLinkedItem ali = editActivityViewModel.objActivity.ActivityLinkedItem.FirstOrDefault(x => x != null && x.IdLinkedItemType == 42);

                                    if (ali != null)
                                    {
                                        modelActivity.Subject = ali.Name;
                                        modelActivity.TooltipTitle = ali.Company.SiteNameWithoutCountry;
                                        modelActivity.TooltipSubject = activity.Subject;
                                    }
                                    else
                                    {
                                        modelActivity.Subject = activity.Subject;
                                        modelActivity.TooltipSubject = activity.Subject;
                                    }
                                }
                                else
                                {
                                    modelActivity.Subject = activity.Subject;
                                    modelActivity.TooltipSubject = activity.Subject;
                                }
                            }
                            else if (editActivityViewModel.objActivity.LookupValue.IdLookupValue == 96)
                            {
                                if (editActivityViewModel.objActivity.ActivityLinkedItem != null && editActivityViewModel.objActivity.ActivityLinkedItem.Count > 0)
                                {
                                    ActivityLinkedItem ali = editActivityViewModel.objActivity.ActivityLinkedItem.FirstOrDefault(x => x != null && x.IdLinkedItemType == 42);

                                    if (ali != null)
                                    {

                                        modelActivity.Subject = ali.Company.SiteNameWithoutCountry;
                                        modelActivity.TooltipTitle = ali.Company.SiteNameWithoutCountry;
                                        modelActivity.TooltipSubject = activity.Subject;
                                    }
                                    else
                                    {
                                        modelActivity.Subject = activity.Subject;
                                        modelActivity.TooltipSubject = activity.Subject;
                                    }
                                }
                                else
                                {
                                    modelActivity.Subject = activity.Subject;
                                    modelActivity.TooltipSubject = activity.Subject;
                                }
                            }
                            else
                            {
                                modelActivity.Subject = activity.Subject;
                                modelActivity.TooltipSubject = activity.Subject;
                                modelActivity.TooltipTitle = null;
                            }

                            if (editActivityViewModel.objActivity.People != null)
                                modelActivity.Owner = editActivityViewModel.objActivity.People.FullName;

                            modelActivity.Description = editActivityViewModel.objActivity.Description;
                            modelActivity.StartTime = editActivityViewModel.objActivity.FromDate;
                            modelActivity.EndTime = editActivityViewModel.objActivity.ToDate;

                            //**condition for set status value as per IsCompleted value for show image as per it.
                            if (editActivityViewModel.objActivity.IsCompleted == null)
                                modelActivity.Status = 0;
                            if (editActivityViewModel.objActivity.IsCompleted != null && editActivityViewModel.objActivity.IsCompleted == 0)
                                modelActivity.Status = 0;
                            if (editActivityViewModel.objActivity.IsCompleted != null && editActivityViewModel.objActivity.IsCompleted == 1)
                                modelActivity.Status = 1;

                            //**condition for set status value as per IsCompleted value for show image as per it.


                            AppointmentsFilteredBackUpList.Remove(modelActivityFilteredBackUp);
                            AppointmentsFilteredList.Remove(modelActivityFiltered);

                            AppointmentsFilteredBackUpList.Add(modelActivity);
                            AppointmentsFilteredList.Add(modelActivity);
                        }

                        AppointmentsFilteredBackUpList = new List<TempAppointment>(AppointmentsFilteredBackUpList);
                        AppointmentsFilteredList = new List<TempAppointment>(AppointmentsFilteredList);
                        AppointmentsMainList = new List<TempAppointment>(AppointmentsMainList);

                    }
                }
                GeosApplication.Instance.Logger.Log("Method AllowEditAppointmentAction executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AllowEditAppointmentAction() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AllowEditAppointmentAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AllowEditAppointmentAction() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        /// <summary>
        /// Method For Deleting Activity
        /// </summary>
        public void DeleteActivityRowCommandAction(object parameter)
        {
            try
            {
                Activity activity = new Activity();
                ActivityGrid ObjActivity = (ActivityGrid)parameter;
                GeosApplication.Instance.Logger.Log("Method DeleteActivityRowCommandAction() ...", category: Category.Info, priority: Priority.Low);
                DXSplashScreen.Show<SplashScreenView>();
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                if (GeosApplication.Instance.IsPermissionAuditor)
                {
                    IsBusy = true;
                    bool result = false;
                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["ActivityDeleteMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                   
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        if (ActivityGridList != null && ActivityGridList.Count > 0)
                        {

                            activity.IdActivity = ObjActivity.IdActivity;
                            activity.ActivityAttachment = new List<ActivityAttachment>();
                            activity.IsDeleted = 1;
                            ChangedLogsEntries = new List<LogEntriesByActivity>();
                            ChangedLogsEntries.Add(new LogEntriesByActivity() { IdActivity = ObjActivity.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityRemoveChangeLog").ToString(), ObjActivity.Subject), IdLogEntryType = 2 });
                            activity.LogEntriesByActivity = ChangedLogsEntries;
                            result = CrmStartUp.DeleteActivity(activity);
                            ActivityGridList.Remove((ActivityGrid)ObjActivity);
                        }

                        if (result)
                        {
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ActivityDeleteSuccess").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                            //CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("DeleteEmployeeLeaveSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                            IsBusy = false;
                            GeosApplication.Instance.Logger.Log("Method DeleteActivityRowCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
                        }
                        else
                        {
                            IsBusy = false;
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ActivityDeleteFailed").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        }
                    }
                    else
                    {
                        IsBusy = false;
                    }
                }


                else if (ObjActivity.IdActivityType != 96)
                {

                    IsBusy = true;
                    bool result = false;

                    if (ObjActivity.IdOwner != GeosApplication.Instance.ActiveUser.IdUser)
                    {
                        CustomMessageBox.Show(System.Windows.Application.Current.FindResource("ActivityDelete").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        IsBusy = false;
                        return;
                    }

                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["ActivityDeleteMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        if (ActivityGridList != null && ActivityGridList.Count > 0)
                        {
                            //Code to remove image in ActivityLinkedItem
                            //if (ObjActivity.ActivityLinkedItem != null && ObjActivity.ActivityLinkedItem.Count > 0)
                            //{
                            //    foreach (ActivityLinkedItem linkedItem in ObjActivity.ActivityLinkedItem)
                            //    {
                            //        linkedItem.ActivityLinkedItemImage = null;
                            //    }
                            //}
                            // this stmt is used to remove activity attachment as it cause some image error.
                            activity.IdActivity = ObjActivity.IdActivity;
                            activity.ActivityAttachment = new List<ActivityAttachment>();
                            activity.IsDeleted = 1;
                            ChangedLogsEntries = new List<LogEntriesByActivity>();
                            ChangedLogsEntries.Add(new LogEntriesByActivity() { IdActivity = ObjActivity.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityRemoveChangeLog").ToString(), ObjActivity.Subject), IdLogEntryType = 2 });
                            activity.LogEntriesByActivity = ChangedLogsEntries;
                            result = CrmStartUp.DeleteActivity(activity);
                            ActivityGridList.Remove((ActivityGrid)ObjActivity);
                        }

                        if (result)
                        {
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ActivityDeleteSuccess").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                            IsBusy = false;
                            GeosApplication.Instance.Logger.Log("Method DeleteActivityRowCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
                        }
                        else
                        {
                            IsBusy = false;
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ActivityDeleteFailed").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        }
                    }
                    else
                    {
                        IsBusy = false;
                    }

                }
                else
                {
                    IsBusy = false;
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("PlannedActivityDelete").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }

            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DeleteActivityRowCommandAction() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DeleteActivityRowCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in DeleteActivityRowCommandAction() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for export to excel.
        /// </summary>
        /// <param name="obj"></param>
        private void ExportActivityGridButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportActivityGridButtonCommandAction ...", category: Category.Info, priority: Priority.Low);

                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Activity";
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

                    ResultFileName = (saveFile.FileName);

                    TableView activityTableView = ((TableView)obj);
                    activityTableView.ShowTotalSummary = false;
                    activityTableView.ShowFixedTotalSummary = false;
                    XlsxExportOptionsEx options = new XlsxExportOptionsEx();
                    options.CustomizeCell += Options_CustomizeCell;
                    activityTableView.ExportToXlsx(ResultFileName, options);

                    IsBusy = false;
                    //activityTableView.ShowTotalSummary = true;
                    activityTableView.ShowFixedTotalSummary = true;
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);

                }

                GeosApplication.Instance.Logger.Log("Method ExportActivityGridButtonCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in ExportAccountsGridButtonCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void Options_CustomizeCell(DevExpress.Export.CustomizeCellEventArgs e)
        {
            e.Formatting.Alignment = new XlCellAlignment() { WrapText = true };
            e.Handled = true;
        }

        private void SalesOwnerPopupClosedCommandAction(object obj)
        {
            if (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
            {
                return;
            }

            FillCmbSalesOwner();
        }

        private void PlantOwnerPopupClosedCommandAction(object obj)
        {

            if (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
            {
                return;
            }

            FillCmbSalesOwner();
        }

        /// <summary>
        /// Method for refresh activity data.
        /// </summary>
        /// <param name="obj"></param>
        private void RefreshActivityDetails(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method RefreshActivityDetails...", category: Category.Info, priority: Priority.Low);
            TableView detailView = (TableView)obj;
            GridControl gridControl = (detailView).Grid;

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
            MyFilterString = string.Empty;

            //SelectedActivityTask = new List<object>();
            //SelectedActivityTask.AddRange(ActivityTypeList);
            //SelectedActivityTaskStatus = new List<object>();
            FillActivityFilterLists();
            FillSalesOwnerAndAcccount(true);

            FillCmbSalesOwner();
            // code for hide column chooser if empty
            int visibleFalseCoulumn = 0;
            visibleFalseCoulumn = gridControl.Columns.Where(col => col.Visible == false).Count();
            if (visibleFalseCoulumn > 0 && IsCalendarVisible != Visibility.Visible)
            {
                IsActivityColumnChooserVisible = true;
            }
            else
            {
                IsActivityColumnChooserVisible = false;
            }
            detailView.SearchString = null;
            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
            {
                DXSplashScreen.Close();
            }
            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

            GeosApplication.Instance.Logger.Log("Method RefreshActivityDetails() executed successfully...", category: Category.Info, priority: Priority.Low);
        }
        private ActivityGrid AddActivitytoActivityGrid(Activity newActivity)
        {
            ActivityGrid activity = new ActivityGrid();
            try
            {
                activity.IdActivity = newActivity.IdActivity;
                activity.Subject = newActivity.Subject;
                activity.Description = newActivity.Description;
                activity.ActivityType = newActivity.LookupValue.Value;
                activity.IdActivityType = newActivity.LookupValue.IdLookupValue;
                activity.IdImageActivityType = newActivity.LookupValue.IdImage;
                activity.Location = newActivity.Location;
                activity.ToDate = newActivity.ToDate;
                activity.FromDate = newActivity.FromDate;
                activity.IsCompleted = newActivity.IsCompleted;
                activity.ActivityTagsString = newActivity.ActivityTagsString;
                activity.ActivityAttendeesString = newActivity.ActivityAttendeesString;
                activity.FullName = newActivity.People.FullName;
                activity.Login = newActivity.People.Login;
                activity.IdOwner = newActivity.IdOwner;
                if (newActivity.IsInternal == 0)
                {
                    activity.CustomerName = newActivity.ActivityLinkedItem[0].Customer.CustomerName;
                    // activity.CompanyName = newActivity.ActivityLinkedItem[0].Company.Name;
                    //Show only site name remmove country name from site name in Grid after add new activity
                    activity.CompanyName = newActivity.ActivityLinkedItem[0].Company.Name;
                    activity.SiteNameWithoutCountry = newActivity.ActivityLinkedItem[0].Company.SiteNameWithoutCountry;
                }


                //activity.IsInternal = editActivityViewModel.objActivity.IsInternal;

                if (activity.IsCompleted == 1)
                {
                    activity.ActivityGridStatus = "Completed";
                    activity.CloseDate = GeosApplication.Instance.ServerDateTime;

                }
                else
                {
                    activity.ActivityGridStatus = newActivity.ActivityStatus != null ? newActivity.ActivityStatus.Value : "";
                    activity.CloseDate = null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return activity;
        }
        /// <summary>
        /// Method for add new activity.
        /// </summary>
        /// <param name="obj"></param>
        private void AddActivityViewWindowShow(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddActivityViewWindowShow...", category: Category.Info, priority: Priority.Low);
                DXSplashScreen.Show<SplashScreenView>();
                AddActivityView addActivityView = new AddActivityView();
                AddActivityViewModel addActivityViewModel = new AddActivityViewModel();
                addActivityViewModel.IsInternalEnable = true;
                EventHandler handle = delegate { addActivityView.Close(); };
                addActivityViewModel.RequestClose += handle;
                addActivityView.DataContext = addActivityViewModel;

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                addActivityView.ShowDialog();

                if (addActivityViewModel.IsActivitySave)
                {
                    foreach (Activity newActivity in addActivityViewModel.NewCreatedActivityList)
                    {


                        SelectedObject = newActivity;
                        ActivityGrid objActivityGrid = new ActivityGrid();
                        objActivityGrid = AddActivitytoActivityGrid(newActivity);
                        ActivityGridList.Add(objActivityGrid);

                        SelectedObjectActivityGrid = objActivityGrid;
                        if (SelectedObjectActivityGrid.IsCompleted == 1)
                        {
                            SelectedObjectActivityGrid.ActivityGridStatus = "Completed";
                            SelectedObjectActivityGrid.CloseDate = GeosApplication.Instance.ServerDateTime;
                        }
                        else
                        {
                            //SelectedObjectActivityGrid.ActivityGridStatus = SelectedObjectActivityGrid. != null ? SelectedObject.ActivityStatus.Value : "";
                            SelectedObjectActivityGrid.CloseDate = null;
                        }

                        // For update Calendar List.

                        List<TempAppointment> tempAppointments = new List<TempAppointment>(AppointmentsMainList);

                        TempAppointment modelActivity = new TempAppointment();
                        modelActivity.IdOffer = newActivity.IdActivity;
                        modelActivity.Tag = newActivity.LookupValue.Value;
                        modelActivity.Label = newActivity.IdActivityType;
                        modelActivity.Description = newActivity.Description;
                        modelActivity.StartTime = newActivity.FromDate;
                        modelActivity.EndTime = newActivity.ToDate;
                        modelActivity.ResourceId = newActivity.IdOwner;
                       
                        if (!ResourceStorageList.Exists(x => x.Id == newActivity.IdOwner))
                        {
                            Emdep.Geos.Data.Common.ResourceStorage resourceStorage = new Emdep.Geos.Data.Common.ResourceStorage();
                            resourceStorage.Id = newActivity.IdOwner;
                            resourceStorage.Model = newActivity.People.Login;
                            resourceStorage.Picture = GeosRepositoryServiceStartUp.GetUserProfileImageWithoutException(newActivity.People.Login);
                            ResourceStorageList.Add(resourceStorage);
                        }

                        if (newActivity.ActivityLinkedItem != null && newActivity.ActivityLinkedItem.Count > 0)
                        {
                            ActivityLinkedItem ali = newActivity.ActivityLinkedItem.FirstOrDefault(x => x != null && x.IdLinkedItemType == 42);

                            if (ali != null)
                                newActivity.ActivityLinkedItem[0].Company.SiteNameWithoutCountry = ali.Name;
                        }

                        if (newActivity.IdActivityType == 37 && newActivity.IsInternal == 0)
                        {
                            if (newActivity.ActivityLinkedItem != null && newActivity.ActivityLinkedItem.Count > 0)
                            {
                                ActivityLinkedItem ali = newActivity.ActivityLinkedItem.FirstOrDefault(x => x != null && x.IdLinkedItemType == 42);
                                if (ali != null)
                                {
                                    modelActivity.Subject = ali.Name;
                                    modelActivity.TooltipTitle = ali.Company.SiteNameWithoutCountry;
                                    modelActivity.TooltipSubject = newActivity.Subject;
                                }
                                else
                                {
                                    modelActivity.Subject = newActivity.Subject;
                                    modelActivity.TooltipSubject = newActivity.Subject;
                                }
                            }
                            else
                            {
                                modelActivity.Subject = newActivity.Subject;
                                modelActivity.TooltipSubject = newActivity.Subject;
                            }
                        }
                        else if (newActivity.IdActivityType == 96)
                        {
                            if (newActivity.ActivityLinkedItem != null && newActivity.ActivityLinkedItem.Count > 0)
                            {
                                ActivityLinkedItem ali = newActivity.ActivityLinkedItem.FirstOrDefault(x => x != null && x.IdLinkedItemType == 42);
                                if (ali != null)
                                {
                                    modelActivity.Subject = ali.Name;
                                    modelActivity.TooltipTitle = ali.Company.SiteNameWithoutCountry;
                                    modelActivity.TooltipSubject = newActivity.Subject;
                                }
                                else
                                {
                                    modelActivity.Subject = newActivity.Subject;
                                    modelActivity.TooltipSubject = newActivity.Subject;
                                }
                            }
                            else
                            {
                                modelActivity.Subject = newActivity.Subject;
                                modelActivity.TooltipSubject = newActivity.Subject;
                            }
                        }
                        else
                        {
                            modelActivity.Subject = newActivity.Subject;
                            modelActivity.TooltipSubject = newActivity.Subject;
                        }

                        if (newActivity.People != null)
                            modelActivity.Owner = newActivity.People.FullName;

                        if (newActivity.IsCompleted == null)
                            modelActivity.Status = 0;
                        if (newActivity.IsCompleted != null && newActivity.IsCompleted == 0)
                            modelActivity.Status = 0;
                        if (newActivity.IsCompleted != null && newActivity.IsCompleted == 1)
                            modelActivity.Status = 1;

                        tempAppointments.Add(modelActivity);
                        AppointmentsMainList = new List<TempAppointment>(tempAppointments);

                        AppointmentsFilteredBackUpList.Add(modelActivity);
                        AppointmentsFilteredList.Add(modelActivity);

                    }

                    AppointmentsFilteredBackUpList = new List<TempAppointment>(AppointmentsFilteredBackUpList);
                    AppointmentsFilteredList = new List<TempAppointment>(AppointmentsFilteredList);
                }
                // code for hide column chooser if empty
                TableView detailView = ((TableView)obj);
                detailView.SearchString = null;

                GridControl gridControl = (detailView).Grid;

                int visibleFalseCoulumn = 0;
                visibleFalseCoulumn = gridControl.Columns.Where(col => col.Visible == false).Count();
                if (visibleFalseCoulumn > 0 && IsCalendarVisible != Visibility.Visible)
                {
                    IsActivityColumnChooserVisible = true;
                }
                else
                {
                    IsActivityColumnChooserVisible = false;
                }
                detailView.Focus();
                GeosApplication.Instance.Logger.Log("Method AddActivityViewWindowShow() executed successfully...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in AddActivityViewWindowShow() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        private void PrintAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintAction ...", category: Category.Info, priority: Priority.Low);

                IsBusy = true;
                PrintableControlLink pcl = new PrintableControlLink((TableView)obj);
                pcl.Margins.Bottom = 5;
                pcl.Margins.Top = 5;
                pcl.Margins.Left = 5;
                pcl.Margins.Right = 5;
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["ActivityViewCustomPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["ActivityViewCustomPrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                IsBusy = false;
                window.Show();

                GeosApplication.Instance.Logger.Log("Method PrintAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in PrintAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void EditActivityViewWindowShow(object obj)
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Method EditActivityViewWindowShow...", category: Category.Info, priority: Priority.Low);

                TableView detailView = (TableView)obj;
                GridControl gridControl = (detailView).Grid;
                if ((ActivityGrid)detailView.FocusedRow != null)
                {
                    //if (obj == null) return;

                    ActivityGrid activity = ((ActivityGrid)detailView.FocusedRow);

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

                    Activity tempActivity = new Activity();
                    int IdActivity = Convert.ToInt32(((ActivityGrid)detailView.DataControl.CurrentItem).IdActivity);
                    tempActivity = CrmStartUp.GetActivityByIdActivity_V2035(IdActivity);

                    EditActivityViewModel editActivityViewModel = new EditActivityViewModel();
                    editActivityViewModel.IsInternalEnable = true;

                    EditActivityView editActivityView = new EditActivityView();
                    editActivityViewModel.Init(tempActivity);

                    EventHandler handle = delegate { editActivityView.Close(); };
                    editActivityViewModel.RequestClose += handle;
                    editActivityView.DataContext = editActivityViewModel;

                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                    editActivityView.ShowDialogWindow();

                    if (editActivityViewModel.objActivity != null)
                    {
                        activity.Subject = editActivityViewModel.objActivity.Subject;
                        activity.Description = editActivityViewModel.objActivity.Description;
                        activity.ActivityType = editActivityViewModel.objActivity.LookupValue.Value;
                        activity.IdActivityType = editActivityViewModel.objActivity.LookupValue.IdLookupValue;
                        activity.IdImageActivityType = editActivityViewModel.objActivity.LookupValue.IdImage;
                        activity.Location = editActivityViewModel.objActivity.Location;
                        activity.ToDate = editActivityViewModel.objActivity.ToDate;
                        activity.FromDate = editActivityViewModel.objActivity.FromDate;
                        activity.IsCompleted = editActivityViewModel.objActivity.IsCompleted;
                        activity.ActivityTagsString = editActivityViewModel.objActivity.ActivityTagsString;
                        activity.ActivityAttendeesString = editActivityViewModel.objActivity.ActivityAttendeesString;
                        activity.FullName = editActivityViewModel.objActivity.People.FullName;
                        activity.Login = editActivityViewModel.objActivity.People.Login;


                        //activity.CustomerName = editActivityViewModel.objActivity.ActivityLinkedItem[0].Customer.CustomerName;
                        //activity.CompanyName = editActivityViewModel.objActivity.ActivityLinkedItem[0].Company.Name;
                        //activity.SiteNameWithoutCountry = editActivityViewModel.objActivity.ActivityLinkedItem[0].Company.SiteNameWithoutCountry;

                        //If internal is on then show Group and plant blank in Grid
                        if (editActivityViewModel.objActivity.IsInternal == 1)
                        {
                            activity.CustomerName = "";
                            activity.CompanyName = "";
                            activity.SiteNameWithoutCountry = "";
                        }
                        else
                        {
                           
                            //Show only site name remove country name from site name in Grid after edit activity
                            if (editActivityViewModel.objActivity.IdActivityType != 96 || GeosApplication.Instance.IsPermissionAuditor == true )
                            {
                                activity.CustomerName = editActivityViewModel.objActivity.ActivityLinkedItem[0].Customer.CustomerName;
                                activity.CompanyName = editActivityViewModel.objActivity.ActivityLinkedItem[0].Company.SiteNameWithoutCountry;
                                activity.SiteNameWithoutCountry = editActivityViewModel.objActivity.ActivityLinkedItem[0].Company.SiteNameWithoutCountry;
                            }
                          
                        }

                        //activity.IsInternal = editActivityViewModel.objActivity.IsInternal;

                        if (activity.IsCompleted == 1)
                        {
                            activity.ActivityGridStatus = "Completed";
                            activity.CloseDate = GeosApplication.Instance.ServerDateTime;

                        }
                        else
                        {
                            activity.ActivityGridStatus = editActivityViewModel.objActivity.ActivityStatus != null ? editActivityViewModel.objActivity.ActivityStatus.Value : "";
                            activity.CloseDate = null;
                        }

                        // code for update detail on calendar view.
                        TempAppointment modelActivity = AppointmentsMainList.FirstOrDefault(Act => Act.IdOffer == activity.IdActivity);

                        if (modelActivity != null)
                        {
                            modelActivity.Tag = editActivityViewModel.objActivity.LookupValue.Value;
                            modelActivity.Label = editActivityViewModel.objActivity.IdActivityType;
                            modelActivity.Description = editActivityViewModel.objActivity.Description;

                            modelActivity.StartTime = editActivityViewModel.objActivity.FromDate;
                            modelActivity.EndTime = editActivityViewModel.objActivity.ToDate;


                            if (editActivityViewModel.objActivity.LookupValue.IdLookupValue == 96 || editActivityViewModel.objActivity.LookupValue.IdLookupValue == 37)
                            {
                                if (editActivityViewModel.objActivity.ActivityLinkedItem != null && editActivityViewModel.objActivity.ActivityLinkedItem.Count > 0)
                                {
                                    ActivityLinkedItem ali = editActivityViewModel.objActivity.ActivityLinkedItem.FirstOrDefault(x => x != null && (x.IdLinkedItemType == 42 || x.IdLinkedItemType == 92));
                                    if (ali != null)
                                    {
                                        modelActivity.Subject = ali.Company.SiteNameWithoutCountry;
                                        modelActivity.TooltipTitle = ali.Company.SiteNameWithoutCountry;
                                        modelActivity.TooltipSubject = editActivityViewModel.Subject;
                                    }
                                    else
                                    {
                                        modelActivity.Subject = editActivityViewModel.objActivity.Subject;
                                        modelActivity.TooltipSubject = editActivityViewModel.objActivity.Subject;
                                    }
                                }
                                else
                                {
                                    modelActivity.Subject = editActivityViewModel.objActivity.Subject;
                                    modelActivity.TooltipSubject = editActivityViewModel.objActivity.Subject;
                                }
                            }
                            else
                            {
                                modelActivity.Subject = editActivityViewModel.objActivity.Subject;
                            }

                            if (editActivityViewModel.objActivity.IsCompleted == null)
                                modelActivity.Status = 0;
                            if (editActivityViewModel.objActivity.IsCompleted != null && editActivityViewModel.objActivity.IsCompleted == 0)
                                modelActivity.Status = 0;
                            if (editActivityViewModel.objActivity.IsCompleted != null && editActivityViewModel.objActivity.IsCompleted == 1)
                                modelActivity.Status = 1;
                        }
                    }
                    // code for hide column chooser if empty
                    int visibleFalseCoulumn = 0;
                    detailView.SearchString = null;
                    visibleFalseCoulumn = gridControl.Columns.Where(col => col.Visible == false).Count();
                    //foreach (GridColumn column in gridControl.Columns)
                    //{
                    //    if (column.Visible == false)
                    //    {
                    //        visibleFalseCoulumn++;
                    //    }
                    //}
                    if (visibleFalseCoulumn > 0)
                    {
                        IsActivityColumnChooserVisible = true;
                    }
                    else
                    {
                        IsActivityColumnChooserVisible = false;
                    }
                }
                GeosApplication.Instance.Logger.Log("Method EditActivityViewWindowShow executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EditActivityViewWindowShow() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EditActivityViewWindowShow() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EditActivityViewWindowShow() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private ObservableCollection<ActivityGrid> GetActivities(ActivityParams objActivityParams)
        {

            ObservableCollection<ActivityGrid> Activities = new ObservableCollection<ActivityGrid>();
            string ServicePath = GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString();

            string ServiceUrl = "http://" + ServicePath + "/CrmRestService.svc" + "/GetActivity";

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(ServiceUrl);
            request.Method = "POST";
            request.ContentType = "application/json";
            //JsonSerializerSettings microsoftDateFormatSettings = 
            //    new JsonSerializerSettings
            //{
            //    DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
            //};
            /*string s = JsonConvert.SerializeObject(objActivityParams, microsoftDateFormatSettings);
            StreamWriter writer = new StreamWriter("JsonData.txt");
            writer.WriteLine(s);
            writer.Close();*/
            DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(ActivityParams));
            json.WriteObject(request.GetRequestStream(), objActivityParams);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();

            DataContractJsonSerializer jsonResp = new DataContractJsonSerializer(typeof(ObservableCollection<ActivityGrid>));

            Activities = (ObservableCollection<ActivityGrid>)jsonResp.ReadObject(stream);


            stream.Flush();
            stream.Close();
            return Activities;
        }
        private void FillAppointmentLabel()
        {
            try
            {
                Labels = new AppointmentLabelCollection();
                Labels.Clear();
                List<LookupValue> GetLookupValues = CrmStartUp.GetLookupValues(9).ToList();
                foreach (var item in GetLookupValues)
                {
                    Labels.Add(Labels.CreateNewLabel(item.IdLookupValue, item.Value, item.Value, (Color)System.Windows.Media.ColorConverter.ConvertFromString(item.HtmlColor != null ? item.HtmlColor : "#FFFFFF")));
                }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillAppointmentLabel() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        private void GetLookupValuesforLabel()
        {
            try
            {
                Labels = new AppointmentLabelCollection();
                Labels.Clear();
                List<LookupValue> GetLookupValues = CrmStartUp.GetLookupValues(9).ToList();
                foreach (var item in GetLookupValues)
                {
                    Labels.Add(Labels.CreateNewLabel(item.IdLookupValue, item.Value, item.Value, (Color)System.Windows.Media.ColorConverter.ConvertFromString(item.HtmlColor != null ? item.HtmlColor : "#FFFFFF")));
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void FillCmbSalesOwner()
        {
            //System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            //sw.Start();
            try
            {
                if (!DXSplashScreen.IsActive)
                    DXSplashScreen.Show<SplashScreenView>();

                GeosApplication.Instance.Logger.Log("Method FillCmbSalesOwner...", category: Category.Info, priority: Priority.Low);

                if (GeosApplication.Instance.IdUserPermission == 21)
                {
                    if (GeosApplication.Instance.SelectedSalesOwnerUsersList != null)
                    {
                        List<UserManagerDtl> salesOwners = GeosApplication.Instance.SelectedSalesOwnerUsersList.Cast<UserManagerDtl>().ToList();
                        var salesOwnersIds = string.Join(",", salesOwners.Select(i => i.IdUser));

                        //==========================================================================================
                        ActivityParams objActivityParams = new ActivityParams();

                        objActivityParams.idActiveUser = GeosApplication.Instance.ActiveUser.IdUser;
                        objActivityParams.idOwner = salesOwnersIds;
                        objActivityParams.idPermission = 21;
                        objActivityParams.idPlant = "0";
                        objActivityParams.accountingYearFrom = GeosApplication.Instance.SelectedyearStarDate;
                        objActivityParams.accountingYearTo = GeosApplication.Instance.SelectedyearEndDate;

                        ActivityGridList = GetActivities(objActivityParams);
                        //==========================================================================================

                    }
                    else
                    {
                        ActivityGridList = new ObservableCollection<ActivityGrid>();
                    }
                }
                else if (GeosApplication.Instance.IdUserPermission == 22)
                {
                    if (GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
                    {
                        List<Company> plantOwners = GeosApplication.Instance.SelectedPlantOwnerUsersList.Cast<Company>().ToList();
                        var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.ConnectPlantId));
                        //==========================================================================================
                        ActivityParams objActivityParams = new ActivityParams();

                        objActivityParams.idActiveUser = GeosApplication.Instance.ActiveUser.IdUser;
                        objActivityParams.idOwner = GeosApplication.Instance.ActiveUser.IdUser.ToString();
                        objActivityParams.idPermission = 22;
                        objActivityParams.idPlant = plantOwnersIds;
                        objActivityParams.accountingYearFrom = GeosApplication.Instance.SelectedyearStarDate;
                        objActivityParams.accountingYearTo = GeosApplication.Instance.SelectedyearEndDate;

                        ActivityGridList = GetActivities(objActivityParams);

                        //sw.Stop();
                        //GeosApplication.Instance.Logger.Log("Activity Data Received from Server In- " + (sw.ElapsedMilliseconds/1000).ToString() + " Secounds", Category.Info, Priority.Medium);
                        //==========================================================================================

                    }
                    else
                    {
                        ActivityGridList = new ObservableCollection<ActivityGrid>();
                    }
                }
                else
                {
                    ActivityParams objActivityParams = new ActivityParams();

                    objActivityParams.idActiveUser = GeosApplication.Instance.ActiveUser.IdUser;
                    objActivityParams.idOwner = GeosApplication.Instance.ActiveUser.IdUser.ToString();
                    objActivityParams.idPermission = 20;
                    objActivityParams.idPlant = "0";
                    objActivityParams.accountingYearFrom = GeosApplication.Instance.SelectedyearStarDate;
                    objActivityParams.accountingYearTo = GeosApplication.Instance.SelectedyearEndDate;

                    ActivityGridList = GetActivities(objActivityParams);

                }

                //Fill Calender Details
                BindCalender(ActivityGridList);

                //GeosApplication.Instance.Logger.Log("Grid and Calender Binded in - " + (sw.ElapsedMilliseconds / 1000).ToString() + " Secounds", category: Category.Info, priority: Priority.Low);
                //GeosApplication.Instance.Logger.Log("Method FillCmbSalesOwner executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillCmbSalesOwner() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillCmbSalesOwner() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillCmbSalesOwner() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            finally
            {
                if (DXSplashScreen.IsActive)
                    DXSplashScreen.Close();
            }
        }
        private void BindCalender(ObservableCollection<ActivityGrid> ActivityGridList)
        {
            try
            {
                List<TempAppointment> modelAppointmentList = new List<TempAppointment>();

                foreach (ActivityGrid item in ActivityGridList)
                {
                    TempAppointment modelAppointment = new TempAppointment();

                    modelAppointment.IdOffer = item.IdActivity;
                    modelAppointment.Description = item.Description;
                    modelAppointment.ResourceId = item.IdOwner;
                    modelAppointment.Owner = item.FullName;
                    modelAppointment.StartTime = item.FromDate;
                    modelAppointment.EndTime = item.ToDate;
                    modelAppointment.Label = item.IdActivityType;

                    if (item.IsCompleted == 0)
                        modelAppointment.Status = 0;
                    if (item.IsCompleted == 1)
                        modelAppointment.Status = 1;

                    modelAppointment.Tag = item.ActivityType;

                    if (!ResourceStorageList.Exists(x => x.Id == item.IdOwner))
                    {
                        Emdep.Geos.Data.Common.ResourceStorage resourceStorage = new Emdep.Geos.Data.Common.ResourceStorage();
                        resourceStorage.Id = item.IdOwner;
                        resourceStorage.Model = item.Login;
                        resourceStorage.Picture = GeosRepositoryServiceStartUp.GetUserProfileImageWithoutException(item.Login);
                        ResourceStorageList.Add(resourceStorage);
                    }

                    if (item.IdActivityType == 37 || item.IdActivityType == 96)
                    {
                        if (item.SiteNameWithoutCountry != null && item.SiteNameWithoutCountry != string.Empty)
                        {
                            modelAppointment.Subject = item.SiteNameWithoutCountry;
                            modelAppointment.TooltipTitle = item.SiteNameWithoutCountry;
                            modelAppointment.TooltipSubject = item.Subject;
                        }
                        else
                        {
                            modelAppointment.Subject = item.Subject;
                            modelAppointment.TooltipSubject = item.Subject;
                        }
                    }
                    else
                    {
                        modelAppointment.Subject = item.Subject;
                        modelAppointment.TooltipSubject = item.Subject;
                    }
                    modelAppointmentList.Add(modelAppointment);
                }

                AppointmentsMainList = new List<TempAppointment>(modelAppointmentList);
                AppointmentsFilteredList = new List<TempAppointment>(modelAppointmentList);
                AppointmentsFilteredBackUpList = new List<TempAppointment>(modelAppointmentList);

                ActivityFilterCriteria = "";
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in BindCalender() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in BindCalender() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in BindCalender() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for fill data for filter criteria.
        /// </summary>
        private void FillActivityFilterLists()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillActivityFilterLists...", category: Category.Info, priority: Priority.Low);

                //fill accoun as per user conditions.
                if (GeosApplication.Instance.IdUserPermission == 21)
                {
                    if (GeosApplication.Instance.SelectedSalesOwnerUsersList != null)
                    {

                        //Sales Owner
                        SalesOwnerList = GeosApplication.Instance.SalesOwnerUsersList;

                        SelectedSalesOwnerList = new List<object>();
                        SelectedSalesOwnerList.AddRange(GeosApplication.Instance.SelectedSalesOwnerUsersList);
                    }
                }
                else if (GeosApplication.Instance.IdUserPermission == 22)
                {
                    List<Company> plantOwners = GeosApplication.Instance.SelectedPlantOwnerUsersList.Cast<Company>().ToList();

                    //Sales Owner
                    string idPlants = string.Join(",", plantOwners.Select(i => i.ConnectPlantId));
                    SalesOwnerList = CrmStartUp.GetSalesUserByPlant(idPlants);

                    SelectedSalesOwnerList = new List<object>();
                    SelectedSalesOwnerList.AddRange(SalesOwnerList);
                }

                else
                {

                    User user = WorkbenchStartUp.GetUserById(GeosApplication.Instance.ActiveUser.IdUser);
                    if (user != null)
                    {
                        UserManagerDtl userManagerDtl = new UserManagerDtl();
                        SalesOwnerList = new List<UserManagerDtl>();
                        SelectedSalesOwnerList = new List<object>();
                        userManagerDtl.IdUser = GeosApplication.Instance.ActiveUser.IdUser;
                        userManagerDtl.User = user;
                        SalesOwnerList.Add(userManagerDtl);
                        SelectedSalesOwnerList.Add(userManagerDtl);

                        SalesOwnerList = new List<UserManagerDtl>(SalesOwnerList);
                        SelectedSalesOwnerList = new List<object>(SelectedSalesOwnerList);
                        //GeosApplication.Instance.SelectedSalesOwnerUsersList = SelectedSalesOwnerList;
                    }
                }

                //fill Activity type
                if (ActivityTypeList == null)
                    ActivityTypeList = CrmStartUp.GetLookupValues(9);

                if (ActivityTypeList != null)
                {
                    SelectedActivityTask = new List<object>();
                    SelectedActivityTask.AddRange(ActivityTypeList);
                }

                //fill Activity status
                if (ActivityTaskStatusList == null)
                    ActivityTaskStatusList = CrmStartUp.GetLookupValues(11).ToList();

                SelectedActivityTaskStatus = new List<object>();

                //SelectedActivityTaskStatus.AddRange(ActivityTaskStatusList);



                GeosApplication.Instance.Logger.Log("Method FillActivityFilterLists executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillActivityFilterLists() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillActivityFilterLists() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillActivityFilterLists() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for remove filter save on grid layout.PrintButtonCommand
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAllowProperty(object sender, AllowPropertyEventArgs e)
        {
            if (e.DependencyProperty == GridControl.FilterStringProperty)
                e.Allow = false;

            if (e.Property.Name == "GroupCount")
                e.Allow = false;

            //if (e.DependencyProperty == TableViewEx.SearchStringProperty)
            //    e.Allow = false;

        }
        private void CustomCellAppearanceGridControl(RoutedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CustomCellAppearanceGridControl ...", category: Category.Info, priority: Priority.Low);

                int visibleFalseCoulumn = 0;

                //if (File.Exists(OrderGridSettingFilePath))
                //{
                //    ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.RestoreLayoutFromXml(OrderGridSettingFilePath);
                //}

                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.AddHandler(DXSerializer.AllowPropertyEvent, new AllowPropertyEventHandler(OnAllowProperty));
                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.View.AddHandler(DXSerializer.AllowPropertyEvent, new AllowPropertyEventHandler(OnAllowProperty));

                //This code for save grid layout...
                //((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.SaveLayoutToXml(OrderGridSettingFilePath);

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

                if (visibleFalseCoulumn > 0)
                {
                    IsActivityColumnChooserVisible = true;
                }
                else
                {
                    IsActivityColumnChooserVisible = false;
                }


                TableView datailView = ((TableView)((System.Windows.RoutedEventArgs)obj).OriginalSource);
                datailView.BestFitColumns();

                GeosApplication.Instance.Logger.Log("Method CustomCellAppearanceGridControl() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CustomCellAppearanceGridControl() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for save Grid as per user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void VisibleIndexChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method VisibleIndexChanged ...", category: Category.Info, priority: Priority.Low);

                GridColumn column = sender as GridColumn;
                //((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(OrderGridSettingFilePath);

                GeosApplication.Instance.Logger.Log("Method VisibleIndexChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in VisibleIndexChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for save Grid as per user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void VisibleChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method VisibleChanged ...", category: Category.Info, priority: Priority.Low);

                GridColumn column = sender as GridColumn;
                if (column.ShowInColumnChooser)
                {
                    //((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(OrderGridSettingFilePath);
                }

                if (column.Visible == false)
                {
                    IsActivityColumnChooserVisible = true;
                }

                GeosApplication.Instance.Logger.Log("Method VisibleChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in VisibleChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Sprint 44-----CRM--M044-08--Multiselection for delete activities
        /// Method to Delete Multiple Activities
        /// </summary>
        /// <param name="obj"></param>
        private void DeleteMultipleActivities(Object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteMultipleActivities() ...", category: Category.Info, priority: Priority.Low);
                TableView detailView = (TableView)obj;
               
                DXSplashScreen.Show<SplashScreenView>();
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                IsBusy = true;
                bool result = false;
                var SelectedRows = detailView.SelectedRows;
                List<Activity> activities = new List<Activity>();
                GeosApplication.Instance.IsPermissionAuditor = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 29 );
                if (SelectedRows.Count > 0)
                {
                    if (GeosApplication.Instance.IsPermissionAuditor)
                    {

                        MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["MultipleActivityDeleteMessage"].ToString(),  Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                        if (MessageBoxResult == MessageBoxResult.Yes)
                        {

                            if (SelectedRows.Count > 0)
                            {

                                foreach (ActivityGrid row in SelectedRows)
                                {

                                    Activity activity = new Activity();
                                    activity.IdActivity = row.IdActivity;
                                    activity.Subject = row.Subject;
                                    activities.Add(activity);
                                }
                                
                                LogEntriesByActivity deleteLog = new LogEntriesByActivity() { IdActivity = 0, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = Convert.ToString(Application.Current.FindResource("ActivityRemoveChangeLog")), IdLogEntryType = 2 };
                                result = CrmStartUp.DeleteMultipleActivities(activities, deleteLog);


                                foreach (Activity item in activities)
                                {
                                    ActivityGridList.Remove(ActivityGridList.FirstOrDefault(x => x.IdActivity == item.IdActivity));
                                }



                            }
                            if (result)
                            {
                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("MultipleActivityDeleteSuccess").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                                IsBusy = false;
                                GeosApplication.Instance.Logger.Log("Method DeleteMultipleActivities() executed successfully", category: Category.Info, priority: Priority.Low);
                            }
                            else
                            {
                                IsBusy = false;
                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("MultipleActivityDeleteFailed").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                            }
                        }
                        else
                        {
                            IsBusy = false;
                        }
                    }
                    else
                    {
                        if (SelectedRows.Count > 0)
                        {
                            foreach (ActivityGrid row in SelectedRows)
                            {
                                if (row.IdOwner == GeosApplication.Instance.ActiveUser.IdUser && row.IdActivityType != 96)
                                {

                                    Activity activity = new Activity();
                                    activity.IdActivity = row.IdActivity;
                                    activity.Subject = row.Subject;
                                    activities.Add(activity);
                                }
                            }

                            if(activities.Count >0)
                            {
                                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["MultipleActivityDeleteMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                                if (MessageBoxResult == MessageBoxResult.Yes)
                                {
                                    LogEntriesByActivity deleteLog = new LogEntriesByActivity() { IdActivity = 0, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = Convert.ToString(Application.Current.FindResource("ActivityRemoveChangeLog")), IdLogEntryType = 2 };
                                    result = CrmStartUp.DeleteMultipleActivities(activities, deleteLog);

                                    foreach (Activity item in activities)
                                    {
                                        ActivityGridList.Remove(ActivityGridList.FirstOrDefault(x => x.IdActivity == item.IdActivity));
                                    }

                                    if (result)
                                    {
                                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("MultipleActivityDeleteSuccess").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                                        IsBusy = false;
                                        GeosApplication.Instance.Logger.Log("Method DeleteMultipleActivities() executed successfully", category: Category.Info, priority: Priority.Low);
                                    }
                                    else
                                    {
                                        IsBusy = false;
                                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("MultipleActivityDeleteFailed").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                    }
                                }
                                else
                                {
                                    IsBusy = false;
                                }
                            }

                            else
                            {
                                CustomMessageBox.Show(System.Windows.Application.Current.FindResource("MultipleActivityDelete").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                IsBusy = false;
                            }
                        }
                    }
                }

                else
                {
                    IsBusy = false;
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("MultipleActivitySelectFailed").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
             
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DeleteMultipleActivities() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DeleteMultipleActivities() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in DeleteMultipleActivities() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }


        }
        #endregion // Method.
    }
}
