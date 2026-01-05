using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Emdep.Geos.Data.Common.Epc;

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    public class SystemSettingsViewModel : INotifyPropertyChanged
    {
        #region TaskLog
        //[Sprint_66]__[GEOS2-1603]__[Deshabilitar opcion inventario]__[sdesai]
        //[002][Sprint_72]__[GEOS2-1656]__[Add article Sleeping days column in warehouse section]__[sdesai]
        //[003][Sprint_78][avpawar][GEOS2-1889][Add a new setting to specify the customers requiring one order one box]
        #endregion

        #region Services
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWarehouseService WarehouseService = new WarehouseServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmService = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region Declaration
        private List<GeosAppSetting> geosAppSettingList;
        private ObservableCollection<Tuple<Int32, string>> selectedCustomerList;
        private ObservableCollection<Tuple<Int32, string>> customerList;



        private bool isBusy;
        private Visibility isFillingVisible;
        private Visibility isPickingVisible;
        private Visibility isPackingVisible;
        private Visibility isWarehouseAvailabilitySectionVisible;

        private DateTime startTimeMondayForFilling;
        private DateTime endTimeMondayForFilling;
        private DateTime startTimeTuesdayForFilling;
        private DateTime endTimeTuesdayForFilling;
        private DateTime startTimeWednesdayForFilling;
        private DateTime endTimeWednesdayForFilling;
        private DateTime startTimeThursdayForFilling;
        private DateTime endTimeThursdayForFilling;
        private DateTime startTimeFridayForFilling;
        private DateTime endTimeFridayForFilling;
        private DateTime startTimeSaturdayForFilling;
        private DateTime endTimeSaturdayForFilling;
        private DateTime startTimeSundayForFilling;
        private DateTime endTimeSundayForFilling;

        private DateTime startTimeMondayForPicking;
        private DateTime endTimeMondayForPicking;
        private DateTime startTimeTuesdayForPicking;
        private DateTime endTimeTuesdayForPicking;
        private DateTime startTimeWednesdayForPicking;
        private DateTime endTimeWednesdayForPicking;
        private DateTime startTimeThursdayForPicking;
        private DateTime endTimeThursdayForPicking;
        private DateTime startTimeFridayForPicking;
        private DateTime endTimeFridayForPicking;
        private DateTime startTimeSaturdayForPicking;
        private DateTime endTimeSaturdayForPicking;
        private DateTime startTimeSundayForPicking;
        private DateTime endTimeSundayForPicking;

        private DateTime startTimeMondayForPacking;
        private DateTime endTimeMondayForPacking;
        private DateTime startTimeTuesdayForPacking;
        private DateTime endTimeTuesdayForPacking;
        private DateTime startTimeWednesdayForPacking;
        private DateTime endTimeWednesdayForPacking;
        private DateTime startTimeThursdayForPacking;
        private DateTime endTimeThursdayForPacking;
        private DateTime startTimeFridayForPacking;
        private DateTime endTimeFridayForPacking;
        private DateTime startTimeSaturdayForPacking;
        private DateTime endTimeSaturdayForPacking;
        private DateTime startTimeSundayForPacking;
        private DateTime endTimeSundayForPacking;

        private StageAvailability fillingStageInfo;
        private StageAvailability pickingStageInfo;
        private StageAvailability packingStageInfo;

        private ObservableCollection<LookupValue> logisticList;
        private int selectedIndexForLogistic;
        private int globalSafety;        
        private int globalFrequency;

        #endregion

        #region Properties
        public List<GeosAppSetting> GeosAppSettingList
        {
            get
            {
                return geosAppSettingList;
            }

            set
            {
                geosAppSettingList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GeosAppSettingList"));
            }
        }

        public ObservableCollection<Tuple<Int32, string>> SelectedCustomerList
        {
            get
            {
                return selectedCustomerList;
            }

            set
            {
                selectedCustomerList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCustomerList"));
            }
        }

        public ObservableCollection<Tuple<Int32, string>> CustomerList
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


        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
            }
        }

        public Visibility IsFillingVisible
        {
            get { return isFillingVisible; }
            set
            {
                isFillingVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsFillingVisible"));
            }
        }

        public Visibility IsPickingVisible
        {
            get { return isPickingVisible; }
            set
            {
                isPickingVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPickingVisible"));
            }
        }

        public Visibility IsPackingVisible
        {
            get { return isPackingVisible; }
            set
            {
                isPackingVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPackingVisible"));
            }
        }
        public ObservableCollection<LookupValue> LogisticList
        {
            get { return logisticList; }
            set
            {
                logisticList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LogisticList"));
            }
        }
        public int SelectedIndexForLogistic
        {
            get { return selectedIndexForLogistic; }
            set
            {
                selectedIndexForLogistic = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexForLogistic"));
            }
        }
		//[nsatpute][GEOS2-9362][17.11.2025]
        public int GlobalSafety
        {
            get { return globalSafety; }
            set
            {
                globalSafety = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GlobalSafety"));
            }
        }

        public int GlobalFrequency
        {
            get { return globalFrequency; }
            set
            {
                globalFrequency = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GlobalFrequency"));
            }
        }

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
        #endregion // Events

        #region public ICommand

        public ICommand AcceptButtonCommand { get; set; }
        public ICommand CancelButtonCommand { get; set; }
        public ICommand AddCustomerRowMouseDoubleClickCommand { get; set; }
        public ICommand CustomerCancelCommand { get; set; }

        //for filling start
        public DateTime StartTimeMondayForFilling
        {
            get
            {
                return startTimeMondayForFilling;
            }

            set
            {
                startTimeMondayForFilling = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StartTimeMondayForFilling"));
            }
        }

        public DateTime EndTimeMondayForFilling
        {
            get
            {
                return endTimeMondayForFilling;
            }

            set
            {
                endTimeMondayForFilling = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EndTimeMondayForFilling"));
            }
        }

        public DateTime StartTimeTuesdayForFilling
        {
            get
            {
                return startTimeTuesdayForFilling;
            }

            set
            {
                startTimeTuesdayForFilling = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StartTimeTuesdayForFilling"));
            }
        }

        public DateTime EndTimeTuesdayForFilling
        {
            get
            {
                return endTimeTuesdayForFilling;
            }

            set
            {
                endTimeTuesdayForFilling = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EndTimeTuesdayForFilling"));
            }
        }

        public DateTime StartTimeWednesdayForFilling
        {
            get
            {
                return startTimeWednesdayForFilling;
            }

            set
            {
                startTimeWednesdayForFilling = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StartTimeWednesdayForFilling"));
            }
        }

        public DateTime EndTimeWednesdayForFilling
        {
            get
            {
                return endTimeWednesdayForFilling;
            }

            set
            {
                endTimeWednesdayForFilling = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EndTimeWednesdayForFilling"));
            }
        }

        public DateTime StartTimeThursdayForFilling
        {
            get
            {
                return startTimeThursdayForFilling;
            }

            set
            {
                startTimeThursdayForFilling = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StartTimeThursdayForFilling"));
            }
        }

        public DateTime EndTimeThursdayForFilling
        {
            get
            {
                return endTimeThursdayForFilling;
            }

            set
            {
                endTimeThursdayForFilling = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EndTimeThursdayForFilling"));
            }
        }

        public DateTime StartTimeFridayForFilling
        {
            get
            {
                return startTimeFridayForFilling;
            }

            set
            {
                startTimeFridayForFilling = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StartTimeFridayForFilling"));
            }
        }

        public DateTime EndTimeFridayForFilling
        {
            get
            {
                return endTimeFridayForFilling;
            }

            set
            {
                endTimeFridayForFilling = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EndTimeFridayForFilling"));
            }
        }

        public DateTime StartTimeSaturdayForFilling
        {
            get
            {
                return startTimeSaturdayForFilling;
            }

            set
            {
                startTimeSaturdayForFilling = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StartTimeSaturdayForFilling"));
            }
        }

        public DateTime EndTimeSaturdayForFilling
        {
            get
            {
                return endTimeSaturdayForFilling;
            }

            set
            {
                endTimeSaturdayForFilling = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EndTimeSaturdayForFilling"));
            }
        }

        public DateTime StartTimeSundayForFilling
        {
            get
            {
                return startTimeSundayForFilling;
            }

            set
            {
                startTimeSundayForFilling = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StartTimeSundayForFilling"));
            }
        }

        public DateTime EndTimeSundayForFilling
        {
            get
            {
                return endTimeSundayForFilling;
            }

            set
            {
                endTimeSundayForFilling = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EndTimeSundayForFilling"));
            }
        }
        //For filling End

        //For picking Start
        public DateTime StartTimeMondayForPicking
        {
            get
            {
                return startTimeMondayForPicking;
            }

            set
            {
                startTimeMondayForPicking = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StartTimeMondayForPicking"));
            }
        }

        public DateTime EndTimeMondayForPicking
        {
            get
            {
                return endTimeMondayForPicking;
            }

            set
            {
                endTimeMondayForPicking = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EndTimeMondayForPicking"));
            }
        }

        public DateTime StartTimeTuesdayForPicking
        {
            get
            {
                return startTimeTuesdayForPicking;
            }

            set
            {
                startTimeTuesdayForPicking = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StartTimeTuesdayForPicking"));
            }
        }

        public DateTime EndTimeTuesdayForPicking
        {
            get
            {
                return endTimeTuesdayForPicking;
            }

            set
            {
                endTimeTuesdayForPicking = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EndTimeTuesdayForPicking"));
            }
        }

        public DateTime StartTimeWednesdayForPicking
        {
            get
            {
                return startTimeWednesdayForPicking;
            }

            set
            {
                startTimeWednesdayForPicking = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StartTimeWednesdayForPicking"));
            }
        }

        public DateTime EndTimeWednesdayForPicking
        {
            get
            {
                return endTimeWednesdayForPicking;
            }

            set
            {
                endTimeWednesdayForPicking = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EndTimeWednesdayForPicking"));
            }
        }

        public DateTime StartTimeThursdayForPicking
        {
            get
            {
                return startTimeThursdayForPicking;
            }

            set
            {
                startTimeThursdayForPicking = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StartTimeThursdayForPicking"));
            }
        }

        public DateTime EndTimeThursdayForPicking
        {
            get
            {
                return endTimeThursdayForPicking;
            }

            set
            {
                endTimeThursdayForPicking = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EndTimeThursdayForPicking"));
            }
        }

        public DateTime StartTimeFridayForPicking
        {
            get
            {
                return startTimeFridayForPicking;
            }

            set
            {
                startTimeFridayForPicking = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StartTimeFridayForPicking"));
            }
        }

        public DateTime EndTimeFridayForPicking
        {
            get
            {
                return endTimeFridayForPicking;
            }

            set
            {
                endTimeFridayForPicking = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EndTimeFridayForPicking"));
            }
        }

        public DateTime StartTimeSaturdayForPicking
        {
            get
            {
                return startTimeSaturdayForPicking;
            }

            set
            {
                startTimeSaturdayForPicking = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StartTimeSaturdayForPicking"));
            }
        }

        public DateTime EndTimeSaturdayForPicking
        {
            get
            {
                return endTimeSaturdayForPicking;
            }

            set
            {
                endTimeSaturdayForPicking = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EndTimeSaturdayForPicking"));
            }
        }

        public DateTime StartTimeSundayForPicking
        {
            get
            {
                return startTimeSundayForPicking;
            }

            set
            {
                startTimeSundayForPicking = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StartTimeSundayForPicking"));
            }
        }

        public DateTime EndTimeSundayForPicking
        {
            get
            {
                return endTimeSundayForPicking;
            }

            set
            {
                endTimeSundayForPicking = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EndTimeSundayForPicking"));
            }
        }
        // For picking end

        //for packing start
        public DateTime StartTimeMondayForPacking
        {
            get
            {
                return startTimeMondayForPacking;
            }

            set
            {
                startTimeMondayForPacking = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StartTimeMondayForPacking"));
            }
        }

        public DateTime EndTimeMondayForPacking
        {
            get
            {
                return endTimeMondayForPacking;
            }

            set
            {
                endTimeMondayForPacking = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EndTimeMondayForPacking"));
            }
        }

        public DateTime StartTimeTuesdayForPacking
        {
            get
            {
                return startTimeTuesdayForPacking;
            }

            set
            {
                startTimeTuesdayForPacking = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StartTimeTuesdayForPacking"));
            }
        }

        public DateTime EndTimeTuesdayForPacking
        {
            get
            {
                return endTimeTuesdayForPacking;
            }

            set
            {
                endTimeTuesdayForPacking = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EndTimeTuesdayForPacking"));
            }
        }

        public DateTime StartTimeWednesdayForPacking
        {
            get
            {
                return startTimeWednesdayForPacking;
            }

            set
            {
                startTimeWednesdayForPacking = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StartTimeWednesdayForPacking"));
            }
        }

        public DateTime EndTimeWednesdayForPacking
        {
            get
            {
                return endTimeWednesdayForPacking;
            }

            set
            {
                endTimeWednesdayForPacking = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EndTimeWednesdayForPacking"));
            }
        }

        public DateTime StartTimeThursdayForPacking
        {
            get
            {
                return startTimeThursdayForPacking;
            }

            set
            {
                startTimeThursdayForPacking = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StartTimeThursdayForPacking"));
            }
        }

        public DateTime EndTimeThursdayForPacking
        {
            get
            {
                return endTimeThursdayForPacking;
            }

            set
            {
                endTimeThursdayForPacking = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EndTimeThursdayForPacking"));
            }
        }

        public DateTime StartTimeFridayForPacking
        {
            get
            {
                return startTimeFridayForPacking;
            }

            set
            {
                startTimeFridayForPacking = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StartTimeFridayForPacking"));
            }
        }

        public DateTime EndTimeFridayForPacking
        {
            get
            {
                return endTimeFridayForPacking;
            }

            set
            {
                endTimeFridayForPacking = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EndTimeFridayForPacking"));
            }
        }

        public DateTime StartTimeSaturdayForPacking
        {
            get
            {
                return startTimeSaturdayForPacking;
            }

            set
            {
                startTimeSaturdayForPacking = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StartTimeSaturdayForPacking"));
            }
        }

        public DateTime EndTimeSaturdayForPacking
        {
            get
            {
                return endTimeSaturdayForPacking;
            }

            set
            {
                endTimeSaturdayForPacking = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EndTimeSaturdayForPacking"));
            }
        }

        public DateTime StartTimeSundayForPacking
        {
            get
            {
                return startTimeSundayForPacking;
            }

            set
            {
                startTimeSundayForPacking = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StartTimeSundayForPacking"));
            }
        }

        public DateTime EndTimeSundayForPacking
        {
            get
            {
                return endTimeSundayForPacking;
            }

            set
            {
                endTimeSundayForPacking = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EndTimeSundayForPacking"));
            }
        }

        public StageAvailability FillingStageInfo
        {
            get
            {
                return fillingStageInfo;
            }

            set
            {
                fillingStageInfo = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FillingStageInfo"));
            }
        }

        public StageAvailability PickingStageInfo
        {
            get
            {
                return pickingStageInfo;
            }

            set
            {
                pickingStageInfo = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PickingStageInfo"));
            }
        }

        public StageAvailability PackingStageInfo
        {
            get
            {
                return packingStageInfo;
            }

            set
            {
                packingStageInfo = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PackingStageInfo"));
            }
        }

        public WarehouseAvailability WarehouseAvailabilityObj { get; set; }

        public Visibility IsWarehouseAvailabilitySectionVisible
        {
            get
            {
                return isWarehouseAvailabilitySectionVisible;
            }

            set
            {
                isWarehouseAvailabilitySectionVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsWarehouseAvailabilitySectionVisible"));
            }
        }

        //For packing end

        #endregion // ICommand

        #region Constructor
        public SystemSettingsViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor SystemSettingsViewModel ...", category: Category.Info, priority: Priority.Low);

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
                        return new Views.SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }
                CancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                AcceptButtonCommand = new RelayCommand(new Action<object>(SaveSystemSettings));
                AddCustomerRowMouseDoubleClickCommand = new DelegateCommand<object>(AddCustomerRowMouseDoubleClickCommandAction);
                CustomerCancelCommand = new DelegateCommand<object>(CustomerCancelCommandAction);
                FillGeosAppSettingList();
                FillLogisticList();
				//[nsatpute][GEOS2-9362][17.11.2025]
                GlobalSafety = Convert.ToInt32(GeosAppSettingList?.FirstOrDefault(x => x.IdAppSetting == 171).DefaultValue);
                int defaultValueGlobalLogistic = Convert.ToInt32(GeosAppSettingList?.FirstOrDefault(x => x.IdAppSetting == 172).DefaultValue);
                SelectedIndexForLogistic = FindLogisticIndex(defaultValueGlobalLogistic);
                GlobalFrequency = Convert.ToInt32(GeosAppSettingList?.FirstOrDefault(x => x.IdAppSetting == 173).DefaultValue);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Constructor SystemSettingsViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }

            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SystemSettingsViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
		//[nsatpute][GEOS2-9362][17.11.2025]
        private int FindLogisticIndex(int idLookupValue)
        {
            if (!LogisticList.Any())
                return -1;

            return LogisticList.ToList().FindIndex(x => x.IdLookupValue == idLookupValue);
        }
        #endregion

        #region Methods
        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }

        /// <summary>
        /// [001][skale][24-07-2019][GEOS2-1667]Priorize first empty locations in Refill
        /// </summary>
        private void FillGeosAppSettingList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillGeosAppSettingList()...", category: Category.Info, priority: Priority.Low);

                GeosAppSettingList = WorkbenchStartUp.GetSelectedGeosAppSettings("24,25,30,35,64,118,171,172,173");
                if (GeosAppSettingList.Count > 0)
                {
                    string[] GeosAppSettingDefaultValues = GeosAppSettingList[0].DefaultValue.Split(',');

                    if (GeosAppSettingDefaultValues.Any(x => x.Contains(WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse.ToString())))
                        WarehouseCommon.Instance.IsWarehouseInInventory = true;
                    else
                        WarehouseCommon.Instance.IsWarehouseInInventory = false;

                    // [001] added
                    string[] RefillEmptyLocationsDefaultValues = GeosAppSettingList[1].DefaultValue.Split(',');

                    if (RefillEmptyLocationsDefaultValues.Any(x => x.Contains(WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse.ToString())))
                        WarehouseCommon.Instance.IsRefillEmptyLocationsFirst = true;
                    else
                        WarehouseCommon.Instance.IsRefillEmptyLocationsFirst = false;

                    //[002]added
                    WarehouseCommon.Instance.ArticleSleepDays = Convert.ToInt32(GeosAppSettingList[2].DefaultValue);
                    WarehouseCommon.Instance.ArticleSleepMonths = Convert.ToInt32(GeosAppSettingList[4].DefaultValue);
                    FillCustomerList();

                    string[] WarehouseStagesValues = GeosAppSettingList[3].DefaultValue.Split(',');
                    if (WarehouseStagesValues.Any(x => x.Contains("30")))
                        IsFillingVisible = Visibility.Visible;
                    else
                        isFillingVisible = Visibility.Collapsed;

                    if (WarehouseStagesValues.Any(x => x.Contains("31")))
                        IsPickingVisible = Visibility.Visible;
                    else
                        IsPickingVisible = Visibility.Collapsed;

                    if (WarehouseStagesValues.Any(x => x.Contains("32")))
                        IsPackingVisible = Visibility.Visible;
                    else
                        IsPackingVisible = Visibility.Collapsed;

                    if(IsFillingVisible == Visibility.Collapsed && 
                        IsPickingVisible == Visibility.Collapsed && 
                        IsPackingVisible == Visibility.Collapsed)
                    {
                        IsWarehouseAvailabilitySectionVisible = Visibility.Collapsed;
                    }
                    else
                        IsWarehouseAvailabilitySectionVisible = Visibility.Visible;

                    FillWarehouseAvailabilityTiming();
					//Shubham[skadam] GEOS2-5168 Timer automático 15 02 2024
                    WarehouseCommon.Instance.InactivityMinutes = Convert.ToInt32(GeosAppSettingList[5].DefaultValue);
                }

                GeosApplication.Instance.Logger.Log("Method FillGeosAppSettingList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillGeosAppSettingList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillGeosAppSettingList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillGeosAppSettingList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        
        /// <summary>
        /// Fill Customer List
        /// </summary>
        private void FillCustomerList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCustomerList ...", category: Category.Info, priority: Priority.Low);
                
                // CustomerList = WarehouseService.GetEMDEPSites(); ;

                CustomerList = new ObservableCollection<Tuple<int, string>>(WarehouseService.GetEMDEPSites());

                SelectedCustomerList = new ObservableCollection<Tuple<int, string>>(WarehouseService.GetCustomersWithOneOrderBox().ToList());
                List<Int32> idsSites = SelectedCustomerList.Select(i => i.Item1).ToList();

                foreach (Tuple<Int32, string> item in CustomerList.Where(ik => idsSites.Contains(ik.Item1)).ToList())
                {
                    CustomerList.Remove(item);
                }

                GeosApplication.Instance.Logger.Log("Method FillCustomerList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillCustomerList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillCustomerList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCustomerList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillWarehouseAvailabilityTiming()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillWarehouseAvailabilityTiming ...", category: Category.Info, priority: Priority.Low);

                WarehouseAvailabilityObj = WarehouseService.GetAllWarehouseAvailabilityByIdCompany_V2140(WarehouseCommon.Instance.Selectedwarehouse);
                if (WarehouseAvailabilityObj.StageAvailabilityList != null && WarehouseAvailabilityObj.StageAvailabilityList.Any(i => i.StageInfo.IdStage == 30))
                {
                    //Filling
                    StartTimeMondayForFilling = new DateTime().Add(WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 30).StartTimeMonday);
                    StartTimeTuesdayForFilling = new DateTime().Add(WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 30).StartTimeTuesday);
                    StartTimeWednesdayForFilling = new DateTime().Add(WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 30).StartTimeWednesday);
                    StartTimeThursdayForFilling = new DateTime().Add(WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 30).StartTimeThursday);
                    StartTimeFridayForFilling = new DateTime().Add(WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 30).StartTimeFriday);
                    StartTimeSaturdayForFilling = new DateTime().Add(WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 30).StartTimeSaturday);
                    StartTimeSundayForFilling = new DateTime().Add(WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 30).StartTimeSunday);

                    EndTimeMondayForFilling = new DateTime().Add(WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 30).EndTimeMonday);
                    EndTimeTuesdayForFilling = new DateTime().Add(WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 30).EndTimeTuesday);
                    EndTimeWednesdayForFilling = new DateTime().Add(WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 30).EndTimeWednesday);
                    EndTimeThursdayForFilling = new DateTime().Add(WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 30).EndTimeThursday);
                    EndTimeFridayForFilling = new DateTime().Add(WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 30).EndTimeFriday);
                    EndTimeSaturdayForFilling = new DateTime().Add(WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 30).EndTimeSaturday);
                    EndTimeSundayForFilling = new DateTime().Add(WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 30).EndTimeSunday);
                }
                else
                {
                    WarehouseAvailabilityObj.IdWarehouse = WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse;
                    StageAvailability stageAvailability = new StageAvailability();
                      stageAvailability.StageInfo = new Stage();
                   
                        stageAvailability.StageInfo.IdStage = 30;
                    
                        stageAvailability.StartTimeMonday = new TimeSpan() ;
                    
                        stageAvailability.EndTimeMonday = new TimeSpan();
                    
                        stageAvailability.StartTimeTuesday = new TimeSpan();
                    
                        stageAvailability.EndTimeTuesday = new TimeSpan();
                    
                        stageAvailability.StartTimeWednesday = new TimeSpan();
                    
                        stageAvailability.EndTimeWednesday = new TimeSpan();
                    
                        stageAvailability.StartTimeThursday = new TimeSpan();
                    
                        stageAvailability.EndTimeThursday = new TimeSpan();
                    
                        stageAvailability.StartTimeFriday = new TimeSpan();
                    
                        stageAvailability.EndTimeFriday = new TimeSpan();
                    
                        stageAvailability.StartTimeSaturday = new TimeSpan();
                    
                        stageAvailability.EndTimeSaturday = new TimeSpan();
                       stageAvailability.StartTimeSunday = new TimeSpan();
                      stageAvailability.EndTimeSunday = new TimeSpan();

                    WarehouseAvailabilityObj.StageAvailabilityList.Add(stageAvailability);
                }
                if (WarehouseAvailabilityObj.StageAvailabilityList != null && WarehouseAvailabilityObj.StageAvailabilityList.Any(i => i.StageInfo.IdStage == 31))
                {
                    //Picking
                    StartTimeMondayForPicking = new DateTime().Add(WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 31).StartTimeMonday);
                    StartTimeTuesdayForPicking = new DateTime().Add(WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 31).StartTimeTuesday);
                    StartTimeWednesdayForPicking = new DateTime().Add(WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 31).StartTimeWednesday);
                    StartTimeThursdayForPicking = new DateTime().Add(WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 31).StartTimeThursday);
                    StartTimeFridayForPicking = new DateTime().Add(WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 31).StartTimeFriday);
                    StartTimeSaturdayForPicking = new DateTime().Add(WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 31).StartTimeSaturday);
                    StartTimeSundayForPicking = new DateTime().Add(WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 31).StartTimeSunday);

                    EndTimeMondayForPicking = new DateTime().Add(WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 31).EndTimeMonday);
                    EndTimeTuesdayForPicking = new DateTime().Add(WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 31).EndTimeTuesday);
                    EndTimeWednesdayForPicking = new DateTime().Add(WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 31).EndTimeWednesday);
                    EndTimeThursdayForPicking = new DateTime().Add(WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 31).EndTimeThursday);
                    EndTimeFridayForPicking = new DateTime().Add(WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 31).EndTimeFriday);
                    EndTimeSaturdayForPicking = new DateTime().Add(WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 31).EndTimeSaturday);
                    EndTimeSundayForPicking = new DateTime().Add(WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 31).EndTimeSunday);
                }
                else
                {
                    WarehouseAvailabilityObj.IdWarehouse = WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse;
                    StageAvailability stageAvailability = new StageAvailability();
                    stageAvailability.StageInfo = new Stage();

                    stageAvailability.StageInfo.IdStage = 31;

                    stageAvailability.StartTimeMonday = new TimeSpan();

                    stageAvailability.EndTimeMonday = new TimeSpan();

                    stageAvailability.StartTimeTuesday = new TimeSpan();

                    stageAvailability.EndTimeTuesday = new TimeSpan();

                    stageAvailability.StartTimeWednesday = new TimeSpan();

                    stageAvailability.EndTimeWednesday = new TimeSpan();

                    stageAvailability.StartTimeThursday = new TimeSpan();

                    stageAvailability.EndTimeThursday = new TimeSpan();

                    stageAvailability.StartTimeFriday = new TimeSpan();

                    stageAvailability.EndTimeFriday = new TimeSpan();

                    stageAvailability.StartTimeSaturday = new TimeSpan();

                    stageAvailability.EndTimeSaturday = new TimeSpan();
                    stageAvailability.StartTimeSunday = new TimeSpan();
                    stageAvailability.EndTimeSunday = new TimeSpan();

                    WarehouseAvailabilityObj.StageAvailabilityList.Add(stageAvailability);
                }

                if (WarehouseAvailabilityObj.StageAvailabilityList != null && WarehouseAvailabilityObj.StageAvailabilityList.Any(i => i.StageInfo.IdStage == 32))
                {

                    //Packing
                    StartTimeMondayForPacking = new DateTime().Add(WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 32).StartTimeMonday);
                    StartTimeTuesdayForPacking = new DateTime().Add(WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 32).StartTimeTuesday);
                    StartTimeWednesdayForPacking = new DateTime().Add(WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 32).StartTimeWednesday);
                    StartTimeThursdayForPacking = new DateTime().Add(WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 32).StartTimeThursday);
                    StartTimeFridayForPacking = new DateTime().Add(WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 32).StartTimeFriday);
                    StartTimeSaturdayForPacking = new DateTime().Add(WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 32).StartTimeSaturday);
                    StartTimeSundayForPacking = new DateTime().Add(WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 32).StartTimeSunday);

                    EndTimeMondayForPacking = new DateTime().Add(WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 32).EndTimeMonday);
                    EndTimeTuesdayForPacking = new DateTime().Add(WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 32).EndTimeTuesday);
                    EndTimeWednesdayForPacking = new DateTime().Add(WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 32).EndTimeWednesday);
                    EndTimeThursdayForPacking = new DateTime().Add(WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 32).EndTimeThursday);
                    EndTimeFridayForPacking = new DateTime().Add(WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 32).EndTimeFriday);
                    EndTimeSaturdayForPacking = new DateTime().Add(WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 32).EndTimeSaturday);
                    EndTimeSundayForPacking = new DateTime().Add(WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 32).EndTimeSunday);
                }
                else
                {
                    WarehouseAvailabilityObj.IdWarehouse = WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse;
                    StageAvailability stageAvailability = new StageAvailability();
                    stageAvailability.StageInfo = new Stage();

                    stageAvailability.StageInfo.IdStage = 32;

                    stageAvailability.StartTimeMonday = new TimeSpan();

                    stageAvailability.EndTimeMonday = new TimeSpan();

                    stageAvailability.StartTimeTuesday = new TimeSpan();

                    stageAvailability.EndTimeTuesday = new TimeSpan();

                    stageAvailability.StartTimeWednesday = new TimeSpan();

                    stageAvailability.EndTimeWednesday = new TimeSpan();

                    stageAvailability.StartTimeThursday = new TimeSpan();

                    stageAvailability.EndTimeThursday = new TimeSpan();

                    stageAvailability.StartTimeFriday = new TimeSpan();

                    stageAvailability.EndTimeFriday = new TimeSpan();

                    stageAvailability.StartTimeSaturday = new TimeSpan();

                    stageAvailability.EndTimeSaturday = new TimeSpan();
                    stageAvailability.StartTimeSunday = new TimeSpan();
                    stageAvailability.EndTimeSunday = new TimeSpan();

                    WarehouseAvailabilityObj.StageAvailabilityList.Add(stageAvailability);
                }
                GeosApplication.Instance.Logger.Log("Method FillWarehouseAvailabilityTiming() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillWarehouseAvailabilityTiming() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillWarehouseAvailabilityTiming() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillWarehouseAvailabilityTiming() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// [001][skale][24-07-2019][GEOS2-1667]Priorize first empty locations in Refill
        /// </summary>
        /// <param name="obj"></param>
        private void SaveSystemSettings(object obj)
        {
            try
            {
                string IsRefillEmptyLocations = string.Empty;
                GeosApplication.Instance.Logger.Log("Method SaveSystemSettings()...", category: Category.Info, priority: Priority.Low);
                
                bool result = WarehouseService.UpdateGeosAppSetting(24, WarehouseCommon.Instance.IsWarehouseInInventory, WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse);
                //[001] added
                bool IsResult = WarehouseService.UpdateGeosAppSetting(25, WarehouseCommon.Instance.IsRefillEmptyLocationsFirst, WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse);
                
                GeosAppSetting geosAppSetting = GeosAppSettingList[2];
                geosAppSetting.DefaultValue = WarehouseCommon.Instance.ArticleSleepDays.ToString();
                bool isSleepDay = WarehouseService.UpdateGeosAppSettingById(geosAppSetting);

				//[nsatpute][GEOS2-9362][17.11.2025]
                GeosAppSetting geosAppSettingSafety = GeosAppSettingList.FirstOrDefault(x=> x.IdAppSetting == 171);
                geosAppSettingSafety.DefaultValue = GlobalSafety.ToString();
                WarehouseService.UpdateGeosAppSettingById(geosAppSettingSafety);

                GeosAppSetting geosAppSettingTransport = GeosAppSettingList.FirstOrDefault(x => x.IdAppSetting == 172);
                geosAppSettingTransport.DefaultValue = LogisticList[SelectedIndexForLogistic].IdLookupValue.ToString();
                WarehouseService.UpdateGeosAppSettingById(geosAppSettingTransport);

                GeosAppSetting geosAppSettingFrequency = GeosAppSettingList.FirstOrDefault(x => x.IdAppSetting == 173);
                geosAppSettingFrequency.DefaultValue = GlobalFrequency.ToString();
                WarehouseService.UpdateGeosAppSettingById(geosAppSettingFrequency);

                string OneOrderOneBox = string.Join(",", SelectedCustomerList.Select(x => x.Item1.ToString()).ToArray());
                GeosAppSetting geosAppSettingObj = new GeosAppSetting();
                geosAppSettingObj.DefaultValue = OneOrderOneBox;
                geosAppSettingObj.IdAppSetting = 34;
                bool isOneOrderOneBox = WarehouseService.UpdateGeosAppSettingById(geosAppSettingObj);

                GeosAppSetting geosAppSettingSleepMonthsObj = new GeosAppSetting();
                geosAppSettingSleepMonthsObj.DefaultValue = WarehouseCommon.Instance.ArticleSleepMonths.ToString();
                geosAppSettingSleepMonthsObj.IdAppSetting = 64;
                WarehouseService.UpdateGeosAppSettingById(geosAppSettingSleepMonthsObj);

                WarehouseAvailabilityObj.IdWarehouse = WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse;
                //Filling
                WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 30).StartTimeMonday = StartTimeMondayForFilling.TimeOfDay;
                WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 30).StartTimeTuesday = StartTimeTuesdayForFilling.TimeOfDay;
                WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 30).StartTimeWednesday = StartTimeWednesdayForFilling.TimeOfDay;
                WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 30).StartTimeThursday = StartTimeThursdayForFilling.TimeOfDay;
                WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 30).StartTimeFriday = StartTimeFridayForFilling.TimeOfDay;
                WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 30).StartTimeSaturday = StartTimeSaturdayForFilling.TimeOfDay;
                WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 30).StartTimeSunday = StartTimeSundayForFilling.TimeOfDay;

                WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 30).EndTimeMonday = EndTimeMondayForFilling.TimeOfDay;
                WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 30).EndTimeTuesday = EndTimeTuesdayForFilling.TimeOfDay;
                WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 30).EndTimeWednesday = EndTimeWednesdayForFilling.TimeOfDay;
                WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 30).EndTimeThursday = EndTimeThursdayForFilling.TimeOfDay;
                WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 30).EndTimeFriday = EndTimeFridayForFilling.TimeOfDay;
                WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 30).EndTimeSaturday = EndTimeSaturdayForFilling.TimeOfDay;
                WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 30).EndTimeSunday = EndTimeSundayForFilling.TimeOfDay;

                //Picking
                WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 31).StartTimeMonday = StartTimeMondayForPicking.TimeOfDay;
                WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 31).StartTimeTuesday = StartTimeTuesdayForPicking.TimeOfDay;
                WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 31).StartTimeWednesday = StartTimeWednesdayForPicking.TimeOfDay;
                WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 31).StartTimeThursday = StartTimeThursdayForPicking.TimeOfDay;
                WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 31).StartTimeFriday = StartTimeFridayForPicking.TimeOfDay;
                WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 31).StartTimeSaturday = StartTimeSaturdayForPicking.TimeOfDay;
                WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 31).StartTimeSunday = StartTimeSundayForPicking.TimeOfDay;

                WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 31).EndTimeMonday = EndTimeMondayForPicking.TimeOfDay;
                WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 31).EndTimeTuesday = EndTimeTuesdayForPicking.TimeOfDay;
                WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 31).EndTimeWednesday = EndTimeWednesdayForPicking.TimeOfDay;
                WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 31).EndTimeThursday = EndTimeThursdayForPicking.TimeOfDay;
                WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 31).EndTimeFriday = EndTimeFridayForPicking.TimeOfDay;
                WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 31).EndTimeSaturday = EndTimeSaturdayForPicking.TimeOfDay;
                WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 31).EndTimeSunday = EndTimeSundayForPicking.TimeOfDay;

                //Packing
                WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 32).StartTimeMonday = StartTimeMondayForPacking.TimeOfDay;
                WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 32).StartTimeTuesday = StartTimeTuesdayForPacking.TimeOfDay;
                WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 32).StartTimeWednesday = StartTimeWednesdayForPacking.TimeOfDay;
                WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 32).StartTimeThursday = StartTimeThursdayForPacking.TimeOfDay;
                WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 32).StartTimeFriday = StartTimeFridayForPacking.TimeOfDay;
                WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 32).StartTimeSaturday = StartTimeSaturdayForPacking.TimeOfDay;
                WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 32).StartTimeSunday = StartTimeSundayForPacking.TimeOfDay;

                WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 32).EndTimeMonday = EndTimeMondayForPacking.TimeOfDay;
                WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 32).EndTimeTuesday = EndTimeTuesdayForPacking.TimeOfDay;
                WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 32).EndTimeWednesday = EndTimeWednesdayForPacking.TimeOfDay;
                WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 32).EndTimeThursday = EndTimeThursdayForPacking.TimeOfDay;
                WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 32).EndTimeFriday = EndTimeFridayForPacking.TimeOfDay;
                WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 32).EndTimeSaturday = EndTimeSaturdayForPacking.TimeOfDay;
                WarehouseAvailabilityObj.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 32).EndTimeSunday = EndTimeSundayForPacking.TimeOfDay;
                #region GEOS2-5168
                //Shubham[skadam] GEOS2-5168 Timer automático 15 02 2024
                try
                {
                    GeosAppSetting geosAppSettingInactivity = GeosAppSettingList[5];
                    geosAppSettingInactivity.DefaultValue = WarehouseCommon.Instance.InactivityMinutes.ToString();
                    bool isgeosAppSettingInactivity = WarehouseService.UpdateGeosAppSettingById(geosAppSettingInactivity);
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in Method SaveSystemSettings()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                }
                #endregion
                bool isSave;
                isSave = WarehouseService.UpdateWarehouseAvailability_V2140(WarehouseAvailabilityObj);

                //if(isSave == true)
                //{
                //    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddShiftSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                //}
               
                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("Method SaveSystemSettings()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SaveSystemSettings() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SaveSystemSettings() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SaveSystemSettings()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        ///  [003][Added][Add a new setting to specify the customers requiring one order one box]
        /// This Method for Add Customer
        /// </summary>
        /// <param name="e"></param>
        public void AddCustomerRowMouseDoubleClickCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method AddAttendeeRowMouseDoubleClickCommandAction ...", category: Category.Info, priority: Priority.Low);

            try
            {
                IsBusy = true;
                if (obj is Tuple<Int32, string>)
                {
                    Tuple<Int32, string> customer = obj as Tuple<Int32, string>;

                    if (!SelectedCustomerList.Any(x => x.Item1 == customer.Item1))
                    {
                        Tuple<Int32, string> selectedCustomer = CustomerList.FirstOrDefault(x => x.Item1 == customer.Item1);


                        if (selectedCustomer != null)
                        {
                            SelectedCustomerList.Add(selectedCustomer);
                            CustomerList.Remove(selectedCustomer);
                        }
                    }
                }
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Method AddCustomerRowMouseDoubleClickCommandAction() executed successfully", category: Category.Exception, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in AddCustomerRowMouseDoubleClickCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            GeosApplication.Instance.Logger.Log("Method AddCustomerRowMouseDoubleClickCommandAction() executed successfully", category: Category.Exception, priority: Priority.Low);
        }


        /// <summary>
        ///  [003][Added][Add a new setting to specify the customers requiring one order one box]
        /// Method to delete Customer from Customers.
        /// </summary>
        /// <param name="obj"></param>
        private void CustomerCancelCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method CustomerCancelCommandAction ...", category: Category.Info, priority: Priority.Low);

            try
            {
                if (obj is Tuple<Int32, string>)
                {
                    Tuple<Int32, string> customer = obj as Tuple<Int32, string>;
                    CustomerList.Add((Tuple<Int32, string>)SelectedCustomerList.FirstOrDefault(x => x.Item1 == customer.Item1));
                    SelectedCustomerList.Remove(SelectedCustomerList.FirstOrDefault(x => x.Item1 == customer.Item1));
                }

                GeosApplication.Instance.Logger.Log("Method CustomerCancelCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CustomerCancelCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
		//[nsatpute][GEOS2-9362][17.11.2025]
        private void FillLogisticList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillLogisticList()...", category: Category.Info, priority: Priority.Low);                
                IList<LookupValue> temptypeList = CrmService.GetLookupValues(101);
                LogisticList = new ObservableCollection<LookupValue>(temptypeList);
                GeosApplication.Instance.Logger.Log("Method FillLogisticList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillLogisticList " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                UI.CustomControls.CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillLogisticList " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillLogisticList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

    }
}
