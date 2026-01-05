using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.SAM.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;

namespace Emdep.Geos.Modules.SAM.ViewModels
{
    public class DashboardViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable
    {

        #region Services

        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        IWarehouseService WarehouseService = new WarehouseServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        public ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ISAMService SAMService = new SAMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchService = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion // End Services Region

        #region Declaration
       
        private DispatcherTimer timer1 = null;
        private List<Ots> mainOtsList = new List<Ots>();
        private DataTable dttable;
        private bool isBusy;
        private object geosAppSettingList;
        private List<Ots> mainOtsList_New = new List<Ots>();
        SolidColorBrush gridControlcardViewBackGroundColor;

        private string totalTimeRegisteredByALLOperators_Today;
        private string totalTimeRegisteredByALLOperators_CurrentWeek;
        private string totalTimeRegisteredByALLOperators_LastWeek;
        private string totalTimeRegisteredByALLOperators_ThisMonth;

        private SolidColorBrush totalTimeRegisteredByALLOperators_TodayBackground;
        private SolidColorBrush totalTimeRegisteredByALLOperators_CurrentWeekBackground;
        private SolidColorBrush totalTimeRegisteredByALLOperators_LastWeekBackground;
        private SolidColorBrush totalTimeRegisteredByALLOperators_ThisMonthBackground;

       // private bool refreshButtonIsEnabled;
        private string autoRefreshLabelContent;

        #endregion // End Of Declaration

        #region Properties
        public string AutoRefreshLabelContent
        {
            get { return autoRefreshLabelContent; }
            set
            {
                autoRefreshLabelContent = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AutoRefreshLabelContent"));
            }
        }

        public bool RefreshButtonIsEnabled
        {
            get {

                var UserSettings = GeosApplication.Instance.UserSettings; // (Dictionary<string, string>)value;

                if (UserSettings.ContainsKey("SAMAutoRefresh"))
                {
                    if (UserSettings["SAMAutoRefresh"] == "Yes")
                    {
                        return false;
                    }
                    else if (UserSettings["SAMAutoRefresh"] == "No")
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }

                return true; // default true
            }
            //set
            //{
            //    refreshButtonIsEnabled = value;
            //    OnPropertyChanged(new PropertyChangedEventArgs("RefreshButtonIsEnabled"));
            //}
        }

        public string TotalTimeRegisteredByALLOperators_Today
        {
            get { return totalTimeRegisteredByALLOperators_Today; }
            set
            {
                totalTimeRegisteredByALLOperators_Today = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TotalTimeRegisteredByALLOperators_Today"));
            }
        }

        public string TotalTimeRegisteredByALLOperators_CurrentWeek
        {
            get { return totalTimeRegisteredByALLOperators_CurrentWeek; }
            set
            {
                totalTimeRegisteredByALLOperators_CurrentWeek = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TotalTimeRegisteredByALLOperators_CurrentWeek"));
            }
        }

        public string TotalTimeRegisteredByALLOperators_LastWeek
        {
            get { return totalTimeRegisteredByALLOperators_LastWeek; }
            set
            {
                totalTimeRegisteredByALLOperators_LastWeek = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TotalTimeRegisteredByALLOperators_LastWeek"));
            }
        }

        public string TotalTimeRegisteredByALLOperators_ThisMonth
        {
            get { return totalTimeRegisteredByALLOperators_ThisMonth; }
            set
            {
                totalTimeRegisteredByALLOperators_ThisMonth = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TotalTimeRegisteredByALLOperators_ThisMonth"));
            }
        }

        public SolidColorBrush TotalTimeRegisteredByALLOperators_TodayBackground
        {
            get { return totalTimeRegisteredByALLOperators_TodayBackground; }
            set
            {
                totalTimeRegisteredByALLOperators_TodayBackground = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TotalTimeRegisteredByALLOperators_TodayBackground"));
            }
        }

        public SolidColorBrush TotalTimeRegisteredByALLOperators_CurrentWeekBackground
        {
            get { return totalTimeRegisteredByALLOperators_CurrentWeekBackground; }
            set
            {
                totalTimeRegisteredByALLOperators_CurrentWeekBackground = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TotalTimeRegisteredByALLOperators_CurrentWeekBackGround"));
            }
        }

        public SolidColorBrush TotalTimeRegisteredByALLOperators_LastWeekBackground
        {
            get { return totalTimeRegisteredByALLOperators_LastWeekBackground; }
            set
            {
                totalTimeRegisteredByALLOperators_LastWeekBackground = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TotalTimeRegisteredByALLOperators_LastWeekBackground"));
            }
        }

        public SolidColorBrush TotalTimeRegisteredByALLOperators_ThisMonthBackground
        {
            get { return totalTimeRegisteredByALLOperators_ThisMonthBackground; }
            set
            {
                totalTimeRegisteredByALLOperators_ThisMonthBackground = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TotalTimeRegisteredByALLOperators_ThisMonthBackground"));
            }
        }

        public SolidColorBrush GridControlcardViewBackGroundColor
        {
            get { return gridControlcardViewBackGroundColor; }
            set
            {
                gridControlcardViewBackGroundColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GridControlcardViewBackGroundColor"));
            }
        }
        
        public DataTable Dttable
        {
            get { return dttable; }
            set
            {
                dttable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Dttable"));
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
        
        public List<Ots> MainOtsList
        {
            get { return mainOtsList; }
            set
            {
                mainOtsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MainOtsList"));
            }
        }
        
        public object GeosAppSettingList
        {
            get { return geosAppSettingList; }
            set
            {
                geosAppSettingList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GeosAppSettingList"));
            }
        }

        //public List<Ots> MainOtsList_New
        //{
        //    get { return mainOtsList_New; }
        //    set
        //    {
        //        mainOtsList_New = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("MainOtsList_New"));
        //    }
        //}

        #region GEOS2-8853
        //Shubham[skadam] [V.2.6.9.0] GEOS2-8853 SAM module very slow when trying to load informations - Dashboard (6/6) 25 11 2025
        List<string> failedPlants;
        List<string> successPlantList;
        Boolean isShowFailedPlantWarning;
        string warningFailedPlants;

        public List<string> FailedPlants
        {
            get { return failedPlants; }
            set { failedPlants = value; }
        }
        public List<string> SuccessPlantList
        {
            get { return successPlantList; }
            set { successPlantList = value; }
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

        #endregion

        #endregion //End Of Properties

        #region Icommands

        public ICommand PlantOwnerPopupClosed { get; private set; }
        public ICommand RefreshWorkOrderViewCommand { get; set; }

        #endregion //End Of Icommand

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion // End Of Events 

        #region Constructor

        public DashboardViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor DashboardViewModel....", category: Category.Info, priority: Priority.Low);

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
                                
                PlantOwnerPopupClosed = new DevExpress.Mvvm.DelegateCommand<object>(PlantOwnerPopupClosedAction);
                RefreshWorkOrderViewCommand = new RelayCommand(new Action<object>(RefreshPendingWorkOrderList));
                
                Dttable = new DataTable();
                gridControlcardViewBackGroundColor = new SolidColorBrush(Color.FromRgb(14, 180, 69));
                
                FillListOfColor();  // Called only once for colors
                //LoadData();
                //FillLeadGridDetails();
                                                
                StartTimer1();

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Constructor DashboardViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DashboardViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion //End Of Constructor

        #region Methods
        //Shubham[skadam] [V.2.6.9.0] GEOS2-8853 SAM module very slow when trying to load informations - Dashboard (6/6) 25 11 2025
        public async Task InitAsync()
        {
            GeosApplication.Instance.Logger.Log("Method InitAsync....", category: Category.Info, priority: Priority.Low);
            try
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
                        return new Views.SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }
                await FillMainItemOtListAsync();
                FillLeadGridDetailsAsync();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in InitAsync() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void StartTimer1()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method StartTimer1....", category: Category.Info, priority: Priority.Low);

                var UserSettings = GeosApplication.Instance.UserSettings;
                if (UserSettings.ContainsKey("SAMAutoRefresh"))
                {
                    if (UserSettings["SAMAutoRefresh"] == "Yes")
                    {
                        timer1 = new DispatcherTimer();
                        if (UserSettings.ContainsKey("SAMAutoRefreshInterval"))
                        {
                            var minutesInterval = double.Parse(GeosApplication.Instance.UserSettings["SAMAutoRefreshInterval"]);
                            this.timer1.Interval = new TimeSpan(0, (int) minutesInterval, 0);

                            this.timer1.Tick += new EventHandler(this.timer1_Tick);
                            timer1.Start();
                            DateTime nextUpdateTime = DateTime.Now.AddMinutes(minutesInterval);
                            AutoRefreshLabelContent = $"Auto Refresh is ON. Next update at {nextUpdateTime.ToLongTimeString()}.";
                        }
                    }
                }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method StartTimer1() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in StartTimer1() Method in SAM Dashboard " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method timer1_Tick....", category: Category.Info, priority: Priority.Low);
                callRefreshMethods();
                GeosApplication.Instance.Logger.Log("Method timer1_Tick() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in timer1_Tick() Method in SAM Dashboard " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void callRefreshMethods()
        {
            RefreshPendingWorkOrderList(null);
            var millisecondsInterval = double.Parse(GeosApplication.Instance.UserSettings["SAMAutoRefreshInterval"]) * 60 * 1000;
            DateTime nextUpdateTime = DateTime.Now.AddMilliseconds(millisecondsInterval);
            AutoRefreshLabelContent = $"Auto Refresh is ON. Next update at {nextUpdateTime.ToLongTimeString()}.";
        }

        private void LoadData()
        {
            FillMainOtList();
        }
        //Shubham[skadam] [V.2.6.9.0] GEOS2-8853 SAM module very slow when trying to load informations - Dashboard (6/6) 25 11 2025
        private async Task LoadDataAsync()
        {
            try
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
                await FillMainItemOtListAsync();
                FillLeadGridDetailsAsync();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in LoadDataAsync() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// After warehouse selection  fill list as per it.
        /// </summary>
        /// <param name="obj"></param>
        private void PlantOwnerPopupClosedAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method PlantOwnerPopupClosedAction...", category: Category.Info, priority: Priority.Low);

            if (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
            {
                return;
            }

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

            //LoadData();
            //FillLeadGridDetails();
            LoadDataAsync();
            //if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            GeosApplication.Instance.Logger.Log("Method PlantOwnerPopupClosedAction executed successfully.", category: Category.Info, priority: Priority.Low);
        }

        public void RefreshPendingWorkOrderList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshPendingWorkOrderList()...", category: Category.Info, priority: Priority.Low);

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
                
                IsBusy = true;
                
                //LoadData();
                //FillLeadGridDetails();
                LoadDataAsync();
                IsBusy = false;

                //if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method RefreshPendingWorkOrderList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshPendingWorkOrderList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillMainOtList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillMainOtList...", category: Category.Info, priority: Priority.Low);

                if (SAMCommon.Instance.SelectedPlantOwnerList != null)
                {
                    List<Ots> TempMainOtsList = new List<Ots>();
                    MainOtsList = new List<Ots>();

                    try
                    {
                        foreach (Company plant in SAMCommon.Instance.SelectedPlantOwnerList)
                        {
                            TempMainOtsList = new List<Ots>(SAMService.GetPendingWorkordersForDashboard_V2180(plant));
                          
                            MainOtsList.AddRange(TempMainOtsList);                            
                        }
                    }
                    catch (FaultException<ServiceException> ex)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Error in FillMainOtList() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                    catch (ServiceUnexceptedException ex)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Error in FillMainOtList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                    }
                    catch (Exception ex)
                    {
                        IsBusy = false;
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Error in Method FillMainOtList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                    }

                    MainOtsList = new List<Ots>(MainOtsList);
                }
                else
                {
                    MainOtsList = new List<Ots>();
                }
                
                GeosApplication.Instance.Logger.Log("Method FillMainOtList executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillMainOtList() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //Shubham[skadam] [V.2.6.9.0] GEOS2-8853 SAM module very slow when trying to load informations - Dashboard (6/6) 25 11 2025
        private async Task FillMainItemOtListAsync()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillMainItemOtListAsync...",Category.Info,Priority.Low);
                if (SAMCommon.Instance.SelectedPlantOwnerList == null)
                {
                    MainOtsList = new List<Ots>();
                    return;
                }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
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
                    return new SplashScreenCustomView() { DataContext = new SplashScreenViewModel() };
                }, null, null);
                GeosApplication.Instance.CustomeSplashScreenMessage = "Please wait";
                GeosApplication.Instance.CustomeSplashScreenInformationMessage = "Collecting the information from plants ...";
                GeosApplication.Instance.StatusMessages = new ObservableCollection<Data.Common.Crm.StatusMessage>();
                var successPlants = new List<string>();
                var failedPlants = new List<string>();
                FailedPlants = new List<string>();
                SuccessPlantList = new List<string>();
                IsShowFailedPlantWarning = false;
                WarningFailedPlants = String.Empty;

                List<Ots> TempMainOtsList = new List<Ots>();
                MainOtsList = new List<Ots>();
                if (SAMCommon.Instance.SelectedPlantOwnerList != null)
                {
                    try
                    {
                        // Initialize plant status
                        foreach (Company com in SAMCommon.Instance.SelectedPlantOwnerList)
                        {
                            GeosApplication.Instance.StatusMessages.Add(new Data.Common.Crm.StatusMessage() { Symbol = "", Message = com.ShortName, IsSuccess = 0 });
                        }
                        try
                        {
                            var tasks = SAMCommon.Instance.SelectedPlantOwnerList.Cast<Company>().Select(async plant =>
                            {
                                try
                                {
                                    GeosApplication.Instance.SplashScreenMessage = "Connecting to " + plant.Alias;
                                    var tempList = await Task.Run(() => SAMService.GetPendingWorkordersForDashboard_V2180(plant));
                                    // Run service call asynchronously
                                    lock (MainOtsList)
                                    {
                                        MainOtsList.AddRange(tempList);
                                        successPlants.Add(plant.Alias);
                                        var statusMsg = GeosApplication.Instance.StatusMessages.FirstOrDefault(x => x.Message == plant.Alias);
                                        if (statusMsg != null) statusMsg.IsSuccess = 1;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    lock (FailedPlants)
                                    {
                                        if (!FailedPlants.Any(a => a.Equals(plant.Alias, StringComparison.OrdinalIgnoreCase)))
                                            FailedPlants.Add(plant.Alias);
                                        IsShowFailedPlantWarning = true;
                                        WarningFailedPlants = string.Format((string)System.Windows.Application.Current.FindResource("DataLoadingFailMessage"), string.Join(",", FailedPlants));
                                        var statusMsg = GeosApplication.Instance.StatusMessages.FirstOrDefault(x => x.Message == plant.Alias);
                                        if (statusMsg != null) statusMsg.IsSuccess = 2;
                                    }
                                    GeosApplication.Instance.Logger.Log($"Error fetching work log data for plant {plant.Alias}: {ex.Message}", Category.Exception, Priority.Low);
                                }
                            });
                            await Task.WhenAll(tasks);
                            MainOtsList = new List<Ots>(MainOtsList);
                        }
                        catch (FaultException<ServiceException> ex)
                        {
                            if (DXSplashScreen.IsActive) DXSplashScreen.Close();
                            GeosApplication.Instance.Logger.Log("Error in FillMainItemOtListAsync() method " + ex.Detail.ErrorMessage, Category.Exception, Priority.Low);
                            CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        }
                        catch (ServiceUnexceptedException ex)
                        {
                            if (DXSplashScreen.IsActive) DXSplashScreen.Close();
                            GeosApplication.Instance.Logger.Log("Error in FillMainItemOtListAsync() Method - ServiceUnexceptedException " + ex.Message, Category.Exception, Priority.Low);
                            GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                        }
                        catch (Exception ex)
                        {
                            if (DXSplashScreen.IsActive) DXSplashScreen.Close();
                            GeosApplication.Instance.Logger.Log("Error in Method FillMainItemOtListAsync()...." + ex.Message, Category.Exception, Priority.Low);
                        }
                    }
                    catch (FaultException<ServiceException> ex)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Error in FillMainItemOtListAsync() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                    catch (ServiceUnexceptedException ex)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Error in FillMainItemOtListAsync() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                    }
                    catch (Exception ex)
                    {
                        IsBusy = false;
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Error in Method FillMainItemOtListAsync()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                    }
                }
                else
                {
                    MainOtsList = new List<Ots>();
                }
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                if (FailedPlants == null || FailedPlants.Count == 0)
                {
                    IsShowFailedPlantWarning = false;
                    WarningFailedPlants = string.Empty;
                }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) DXSplashScreen.Close();
                GeosApplication.Instance.Logger.Log("Get an error in FillMainItemOtListAsync() method " + ex.Message,Category.Exception,Priority.Low);
            }
            finally
            {
                if (DXSplashScreen.IsActive) DXSplashScreen.Close();
            }
        }


        /// <summary>
        /// Method for refresh Lead Grid details.
        /// </summary>
        private void FillLeadGridDetails()
        {
            try
            {
                GeosApplication.Instance.Logger.Log(" FillLeadGridDetails...", category: Category.Info, priority: Priority.Low);
                
                AddDataTableColumns();
                FillDataTable();
                GeosApplication.Instance.Logger.Log(" FillLeadGridDetails() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillLeadGridDetails() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for refresh Lead Grid details.
        /// </summary>
        /// Shubham[skadam] [V.2.6.9.0] GEOS2-8853 SAM module very slow when trying to load informations - Dashboard (6/6) 25 11 2025
        private void FillLeadGridDetailsAsync()
        {
            try
            {
                GeosApplication.Instance.Logger.Log(" FillLeadGridDetailsAsync...", category: Category.Info, priority: Priority.Low);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
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
                }, x => { return new SplashScreenCustomMessageView() { DataContext = new SplashScreenViewModel() }; }, null, null);
                GeosApplication.Instance.CustomeSplashScreenInformationMessage = "Loading data...";
                GeosApplication.Instance.Logger.Log(" FillLeadGridDetailsAsync...", category: Category.Info, priority: Priority.Low);

                AddDataTableColumns();
                FillDataTable();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(" FillLeadGridDetailsAsync() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillLeadGridDetailsAsync() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillDataTable()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillDataTable ...", category: Category.Info, priority: Priority.Low);

                var allRowsWorkLogItemList = new List<OTWorkingTime>();

                Dttable.Rows.Clear();
                //MainOtsList_New = new List<Ots>();

                var results = MainOtsList.GroupBy(
                                p => p.IdOffer,
                                (key, g) => new { ParentId = key, OtList = g.ToList() });

                int i = 1;
                foreach (var res in results.ToList())
                {
                    var MinPlannedStartDate = res.OtList.Where(x => x.ExpectedStartDate != null && x.ExpectedStartDate != DateTime.MinValue).Min(a => a.ExpectedStartDate);

                    if (MinPlannedStartDate != null)
                    {
                        i++;
                        Ots parent = MainOtsList.FirstOrDefault(b => b.IdOffer == res.ParentId);

                        DataRow dr1 = Dttable.NewRow();
                        dr1["IdOffer"] = parent.IdOffer;
                        dr1["IsParent"] = true;
                        dr1["ChildId"] = parent.OfferCode;
                        dr1["ParentId"] = i;
                        dr1["OfferType"] = parent.Quotation.Offer.OfferType.IdOfferType;
                        dr1["Code"] = parent.OfferCode.ToString();
                        if (res.OtList.Where(a => a.OperatorNames == null && a.OperatorNames == "").ToList().Count == 0)
                        {
                            dr1["OperatorNames"] = "IsParent";
                            dr1["ParentOperatorNamesNull"] = string.Empty;
                        }

                        if (parent.Quotation != null && parent.Quotation.Template != null)
                            dr1["Type"] = parent.Quotation.Template.Name.ToString();

                        var MinExpectedDeliveryDate = res.OtList.Min(a => a.DeliveryDate);
                        if (MinExpectedDeliveryDate != null)
                        {
                            CultureInfo cul = CultureInfo.CurrentCulture;
                            dr1["DeliveryDate"] = MinExpectedDeliveryDate;
                            int Delay = res.OtList.FirstOrDefault(a => a.DeliveryDate == MinExpectedDeliveryDate).Delay;
                            dr1["Delay"] = Delay;
                        }
                        int SumProgress = res.OtList.Sum(a => a.Progress);
                        dr1["Progress"] = SumProgress / res.OtList.Count();

                        var MaxPlannedStartDate = res.OtList.Where(x => x.ExpectedEndDate != null && x.ExpectedEndDate != DateTime.MaxValue).Max(a => a.ExpectedEndDate);

                        if (MinPlannedStartDate != DateTime.MinValue)
                        {
                            dr1["PlannedStartDate"] = MinPlannedStartDate;
                        }
                        else
                            dr1["PlannedStartDate"] = DBNull.Value;

                        if (MaxPlannedStartDate != null)
                        {
                            if (MaxPlannedStartDate != DateTime.MinValue)
                            {
                                dr1["PlannedEndDate"] = MaxPlannedStartDate;
                            }
                            else
                                dr1["PlannedEndDate"] = DBNull.Value;
                        }

                        if (MinPlannedStartDate != null && MaxPlannedStartDate != null)
                        {
                            if (MinPlannedStartDate != DateTime.MinValue && MaxPlannedStartDate != DateTime.MinValue)
                            {
                                DateTime startdate = (DateTime)MinPlannedStartDate;
                                DateTime enddate = (DateTime)MaxPlannedStartDate;
                                dr1["PlannedDuration"] = (enddate - startdate).TotalDays;
                            }
                        }


                        dr1["Description"] = res.OtList.FirstOrDefault().Quotation.Offer.Description.ToString();
                        dr1["Modules"] = res.OtList.FirstOrDefault().Modules.ToString();
                        dr1["Group"] = res.OtList.FirstOrDefault().Quotation.Site.Customer.CustomerName.ToString();
                        dr1["Plant"] = res.OtList.FirstOrDefault().Quotation.Site.Name.ToString();
                        dr1["Country"] = res.OtList.FirstOrDefault().Quotation.Site.Country.Name.ToString();

                        if (res.OtList.FirstOrDefault().Quotation.Offer.CarOEM != null)
                            dr1["OEM"] = res.OtList.FirstOrDefault().Quotation.Offer.CarOEM.Name;
                        if (res.OtList.FirstOrDefault().Quotation.Offer.CarProject != null)
                            dr1["Project"] = res.OtList.FirstOrDefault().Quotation.Offer.CarProject.Name.ToString();
                        if (res.OtList.FirstOrDefault().Quotation.Offer.BusinessUnit != null)
                            dr1["BusinessUnit"] = res.OtList.FirstOrDefault().Quotation.Offer.BusinessUnit.Value.ToString();


                        if (res.OtList.FirstOrDefault().PoDate != null)
                            dr1["PODate"] = res.OtList.FirstOrDefault().PoDate;
                        else
                            dr1["PODate"] = DBNull.Value;

                        if (res.OtList.Any(ol => ol.Quotation.IdDetectionsTemplate != 24))
                        {
                            dr1["Status"] = res.OtList.Where(ol => ol.Quotation.IdDetectionsTemplate != 24).ToList().FirstOrDefault().WorkflowStatus.Name.ToString();

                            dr1["HtmlColor"] = res.OtList.Where(ol => ol.Quotation.IdDetectionsTemplate != 24).ToList().FirstOrDefault().WorkflowStatus.HtmlColor.ToString();
                        }

                        if (res.OtList.FirstOrDefault().RealStartDate != null)
                            dr1["RealStartDate"] = res.OtList.FirstOrDefault().RealStartDate;
                        else
                            dr1["RealStartDate"] = DBNull.Value;

                        if (res.OtList.FirstOrDefault().RealEndDate != null)
                            dr1["RealEndDate"] = res.OtList.FirstOrDefault().RealEndDate;
                        else
                            dr1["RealEndDate"] = DBNull.Value;

                        if (res.OtList.FirstOrDefault().DeliveryDate != null)
                        {
                            CultureInfo cul = CultureInfo.CurrentCulture;
                            dr1["ExpectedDeliveryWeek"] = res.OtList.FirstOrDefault().DeliveryDate.Value.Year + "CW" + cul.Calendar.GetWeekOfYear((DateTime)res.OtList.FirstOrDefault().DeliveryDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday).ToString().PadLeft(2, '0');
                        }
                        dr1["RealDuration"] = res.OtList.FirstOrDefault().RealDuration;

                        if (res.OtList.Any(a => a.Quotation.Template.Name == "TESTBOARD"))
                            dr1["Remarks"] = string.Join(",", res.OtList.Where(a => a.Quotation.Template.Name == "TESTBOARD" && !string.IsNullOrEmpty(a.Observations)).Select(a => a.Observations));

                        try
                        {

                            if (res.OtList.FirstOrDefault().Quotation.Offer.ProductCategoryGrid.IdProductCategory > 0)
                            {
                                if (res.OtList.FirstOrDefault().Quotation.Offer.ProductCategoryGrid.Category != null)
                                    dr1["Category1"] = res.OtList.FirstOrDefault().Quotation.Offer.ProductCategoryGrid.Category.Name;

                                if (res.OtList.FirstOrDefault().Quotation.Offer.ProductCategoryGrid.IdParent == 0)
                                {
                                    dr1["Category1"] = res.OtList.FirstOrDefault().Quotation.Offer.ProductCategoryGrid.Name;
                                }
                                else
                                {
                                    dr1["Category2"] = res.OtList.FirstOrDefault().Quotation.Offer.ProductCategoryGrid.Name;
                                }
                            }
                            dr1["OfferStartDateMinValue"] = GeosApplication.Instance.ServerDateTime.Date;
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.Logger.Log("Get an error in FillDataTable() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                        }


                        //DttableCopy.Rows.Add(dr1);
                        //MainOtsList_New.Add(parent);
                        //ChildOt
                        List<Ots> ChildOTList = MainOtsList.Where(b => b.IdOffer == res.ParentId).ToList();
                        var allChildOtAssignedUser = new ObservableCollection<OTAssignedUser>();
                        var allChildOtWorkLogItemList = new List<OTWorkingTime>();
                        var currentChildOtAssignedUser = new ObservableCollection<OTAssignedUser>();
                        var currentChildOtWorkLogItemList = new List<OTWorkingTime>();

                        foreach (Ots ot in res.OtList)
                        {
                            if (dr1["ChildOtIdsCommaSeparated"] == null || string.IsNullOrEmpty(dr1["ChildOtIdsCommaSeparated"].ToString()))
                            {
                                dr1["ChildOtIdsCommaSeparated"] = $"{ot.IdOT}";
                            }
                            else
                            {
                                dr1["ChildOtIdsCommaSeparated"] = $"{dr1["ChildOtIdsCommaSeparated"].ToString()},{ot.IdOT}";
                            }

                            currentChildOtAssignedUser = new ObservableCollection<OTAssignedUser>(SAMService.GetOTAssignedUsers(ot.Site, ot.IdOT).ToList());

                            currentChildOtWorkLogItemList = SAMService.GetOTWorkingTimeDetails(ot.IdOT, ot.Site);

                            allChildOtWorkLogItemList.AddRange(currentChildOtWorkLogItemList);
                            dr1["allChildOtWorkLogItemList"] = allChildOtWorkLogItemList;

                            allRowsWorkLogItemList.AddRange(currentChildOtWorkLogItemList);

                            for (int j = 0; j < currentChildOtAssignedUser.Count; j++)
                            {                              

                                var isCurrentlyWorkingOnOT = currentChildOtWorkLogItemList.Exists(
                                    x => x.UserShortDetail.IdUser == currentChildOtAssignedUser[j].IdUser &&
                                    (x.StartTime != null && x.EndTime == null));

                                if (isCurrentlyWorkingOnOT)
                                {
                                    currentChildOtAssignedUser[j].IsCurrentlyWorkingOnOT = true;// isCurrentlyWorkingOnOT;
                                    currentChildOtAssignedUser[j].BorderColor = new SolidColorBrush(Colors.Green);// isCurrentlyWorkingOnOT;                                     
                                    currentChildOtAssignedUser[j].BorderThickness = new Thickness(3);
                                }
                                else
                                {
                                    currentChildOtAssignedUser[j].IsCurrentlyWorkingOnOT = false;// isCurrentlyWorkingOnOT;
                                    currentChildOtAssignedUser[j].BorderColor = new SolidColorBrush(Colors.DarkGray);// isCurrentlyWorkingOnOT;                                    
                                    currentChildOtAssignedUser[j].BorderThickness = new Thickness(1);
                                }

                                if (allChildOtAssignedUser.FirstOrDefault(x => x.IdUser == currentChildOtAssignedUser[j].IdUser) == null)
                                {
                                    allChildOtAssignedUser.Add(currentChildOtAssignedUser[j]);
                                }
                            }

                        }

                        dr1["allChildOtAssignedUser"] = allChildOtAssignedUser;

                        string namesCommaSeparated = string.Empty;
                        foreach (var item in allChildOtAssignedUser)
                        {
                            if (item.UserShortDetail != null && !string.IsNullOrEmpty(item.UserShortDetail.UserName))
                            {
                                if (string.IsNullOrEmpty(namesCommaSeparated))
                                {
                                    namesCommaSeparated = $"{item.UserShortDetail.UserName}";
                                }
                                else
                                {
                                    namesCommaSeparated = $"{namesCommaSeparated},{item.UserShortDetail.UserName}";
                                }
                            }
                        }
                        dr1["OperatorNames"] = namesCommaSeparated;

                        Dttable.Rows.Add(dr1);
                    }
                }

                DownloadAndSetImageToEachUserInEachRow(Dttable);
                ProcessWorkLogItemListAndShowSummary(allRowsWorkLogItemList);
                
                GeosApplication.Instance.Logger.Log("Method FillDataTable() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillDataTable() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            }

        private void ProcessWorkLogItemListAndShowSummary(List<OTWorkingTime> allRowsWorkLogItemList)
        {
            List<OTWorkingTime> splittedAllRowsWorkLogItemList = new List<OTWorkingTime>();
            //Check if any entry is added for multiple days. Split down it into multiple entries.

            try
            {
                GeosApplication.Instance.Logger.Log("Method ProcessWorkLogItemListAndShowSummary()...", category: Category.Info, priority: Priority.Low);
                
                foreach (var workLogItem in allRowsWorkLogItemList)
                {
                    if (workLogItem.StartTime != null && workLogItem.EndTime != null)
                    {
                        //if (workLogItem.EndTime == null)
                        //{
                        //    workLogItem.EndTime = DateTime.Now;
                        //}
                        //else if (workLogItem.StartTime.Value.Date != workLogItem.EndTime.Value.Date)
                        //{ }
                        if (workLogItem.StartTime.Value.Date == workLogItem.EndTime.Value.Date)
                        {
                            splittedAllRowsWorkLogItemList.Add(workLogItem);
                        }
                        else
                        {
                            var workLogItemcloned = (OTWorkingTime)workLogItem.Clone();
                            while (workLogItemcloned.StartTime.Value.Date != workLogItemcloned.EndTime.Value.Date)
                            {
                                var newWorkLogItem_Split1 = (OTWorkingTime)workLogItemcloned.Clone();
                                newWorkLogItem_Split1.EndTime = workLogItemcloned.StartTime.Value.Date.Add(new TimeSpan(23, 59, 59));
                                splittedAllRowsWorkLogItemList.Add(newWorkLogItem_Split1);

                                var newWorkLogItem_Split2 = (OTWorkingTime)workLogItemcloned.Clone();
                                newWorkLogItem_Split2.StartTime = workLogItemcloned.StartTime.Value.Date.AddDays(1);

                                workLogItemcloned = newWorkLogItem_Split2;
                            }

                            if (workLogItemcloned.StartTime.Value != workLogItemcloned.EndTime.Value)
                            {
                                splittedAllRowsWorkLogItemList.Add(workLogItemcloned);
                            }

                        }
                    }
                }
                //Splitting work logs complete.

                CultureInfo CultureEnglish = new CultureInfo("en-GB");
                int thisWeekNumber = CultureEnglish.Calendar.GetWeekOfYear(DateTime.Today, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                int thisMonthNumber = DateTime.Today.Month;

                DateTime thisWeekMondayStartDateTime = FindCurrentWeekMondayDateTimeOrGetToday(DateTime.Today);
                DateTime thisWeekSundayEndDateTime = thisWeekMondayStartDateTime.AddDays(7).AddSeconds(-1);
                DateTime lastWeekMondayStartDateTime = thisWeekMondayStartDateTime.AddDays(-7);
                DateTime lastWeekSundayEndDateTime = thisWeekSundayEndDateTime.AddDays(-7);

                TimeSpan workLogTotalTime_Today = new TimeSpan();
                TimeSpan workLogTotalTime_ThisWeek = new TimeSpan();
                TimeSpan workLogTotalTime_LastWeek = new TimeSpan();
                TimeSpan workLogTotalTime_ThisMonth = new TimeSpan();

                foreach (var item in splittedAllRowsWorkLogItemList)
                {
                    if (item.StartTime != null && item.EndTime != null)
                    {
                        var timeSpent = item.EndTime.Value - item.StartTime.Value;
                        var logIsInToday = (item.StartTime.Value.Date == DateTime.Today);
                        var logIsInThisWeek = (item.StartTime.Value.Date >= thisWeekMondayStartDateTime &&
                                               item.StartTime.Value.Date <= thisWeekSundayEndDateTime);
                        var logIsInLastWeek = (item.StartTime.Value.Date >= lastWeekMondayStartDateTime &&
                                               item.StartTime.Value.Date <= lastWeekSundayEndDateTime);
                        var logIsInThisMonth = item.StartTime.Value.Month == thisMonthNumber;

                        if (logIsInToday)
                        {
                            workLogTotalTime_Today = workLogTotalTime_Today.Add(timeSpent);
                        }
                        if (logIsInThisWeek)
                        {
                            workLogTotalTime_ThisWeek = workLogTotalTime_ThisWeek.Add(timeSpent);
                        }
                        if (logIsInLastWeek)
                        {
                            workLogTotalTime_LastWeek = workLogTotalTime_LastWeek.Add(timeSpent);
                        }
                        if (logIsInThisMonth)
                        {
                            workLogTotalTime_ThisMonth = workLogTotalTime_ThisMonth.Add(timeSpent);
                        }
                    }
                }

                TotalTimeRegisteredByALLOperators_Today =
                    $"{(workLogTotalTime_Today.Days * 24) + workLogTotalTime_Today.Hours}H {workLogTotalTime_Today.Minutes}M";
                TotalTimeRegisteredByALLOperators_CurrentWeek =
                    $"{(workLogTotalTime_ThisWeek.Days * 24) + workLogTotalTime_ThisWeek.Hours}H {workLogTotalTime_ThisWeek.Minutes}M"; // "222H 22M";
                TotalTimeRegisteredByALLOperators_LastWeek =
                    $"{(workLogTotalTime_LastWeek.Days * 24) + workLogTotalTime_LastWeek.Hours}H {workLogTotalTime_LastWeek.Minutes}M"; // "333H 33M";
                TotalTimeRegisteredByALLOperators_ThisMonth =
                    $"{(workLogTotalTime_ThisMonth.Days * 24) + workLogTotalTime_ThisMonth.Hours}H {workLogTotalTime_ThisMonth.Minutes}M"; // "4444H 44M";

                var lightGreenColor = new SolidColorBrush(Color.FromRgb(14, 180, 69));
                SolidColorBrush backColor = new SolidColorBrush(Color.FromRgb(51, 51, 51)); //WhiteAndBlue

                //if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                //{
                //    backColor = new SolidColorBrush(Color.FromRgb(51, 51, 51));
                //}

                TotalTimeRegisteredByALLOperators_TodayBackground = (TotalTimeRegisteredByALLOperators_Today != "0H 0M") ? lightGreenColor : backColor;
                TotalTimeRegisteredByALLOperators_CurrentWeekBackground = (TotalTimeRegisteredByALLOperators_CurrentWeek != "0H 0M") ? lightGreenColor : backColor;
                TotalTimeRegisteredByALLOperators_LastWeekBackground = (TotalTimeRegisteredByALLOperators_LastWeek != "0H 0M") ? lightGreenColor : backColor;
                TotalTimeRegisteredByALLOperators_ThisMonthBackground = (TotalTimeRegisteredByALLOperators_ThisMonth != "0H 0M") ? lightGreenColor : backColor;

                GeosApplication.Instance.Logger.Log("Method ProcessWorkLogItemListAndShowSummary() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ProcessWorkLogItemListAndShowSummary() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private DateTime FindCurrentWeekMondayDateTimeOrGetToday(DateTime currentDateTime)
        {
            var currentWeekMondayDateTime = currentDateTime;

            try
            {
                GeosApplication.Instance.Logger.Log("Method FindCurrentWeekMondayDateTimeOrGetToday()...", category: Category.Info, priority: Priority.Low);

                CultureInfo CultureEnglish = new CultureInfo("en-GB");
                var DayOfCurrentWeek = CultureEnglish.Calendar.GetDayOfWeek(currentDateTime);
                int daysDifferenceWithLastMonday = 0;

                if (DayOfCurrentWeek == DayOfWeek.Sunday)
                {
                    daysDifferenceWithLastMonday = -6;
                }
                else
                {
                    daysDifferenceWithLastMonday = (DayOfCurrentWeek - DayOfWeek.Monday) * -1;
                }

                currentWeekMondayDateTime = DateTime.Today.AddDays(daysDifferenceWithLastMonday);

                GeosApplication.Instance.Logger.Log("Method FindCurrentWeekMondayDateTimeOrGetToday() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FindCurrentWeekMondayDateTimeOrGetToday() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            return currentWeekMondayDateTime;
        }

        private void DownloadAndSetImageToEachUserInEachRow(DataTable dataTableOffers)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DownloadAndSetImageToEachUserInEachRow ...", category: Category.Info, priority: Priority.Low);

                var allAssignedUsers = new ObservableCollection<OTAssignedUser>();
                var processedUserWithUserImage = new Dictionary<string, ImageSource>();

                foreach (DataRow dataRowItem in dataTableOffers.Rows)
                {
                    if (dataRowItem["allChildOtAssignedUser"] != null)
                    {
                        allAssignedUsers = (ObservableCollection<OTAssignedUser>)dataRowItem["allChildOtAssignedUser"];

                        foreach (var assignedUserItem in allAssignedUsers)
                        {
                            try
                            {
                                if (processedUserWithUserImage.ContainsKey(assignedUserItem.UserShortDetail.Login))
                                {
                                    assignedUserItem.UserShortDetail.UserImage = processedUserWithUserImage[assignedUserItem.UserShortDetail.Login];
                                }
                                else
                                {
                                    byte[] UserProfileImageByte = GeosRepositoryServiceController.GetUserProfileImageWithoutException(assignedUserItem.UserShortDetail.Login);

                                    if (UserProfileImageByte != null)
                                    {
                                        assignedUserItem.UserShortDetail.UserImage = SAMCommon.Instance.ByteArrayToBitmapImage(UserProfileImageByte);
                                    }
                                    processedUserWithUserImage.Add(assignedUserItem.UserShortDetail.Login, assignedUserItem.UserShortDetail.UserImage);
                                }
                            }
                            catch (Exception ex)
                            {
                                GeosApplication.Instance.Logger.Log("Get an error in DownloadAndSetImageToEachUserInEachRow() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                            }
                        }
                    }
                }

                GeosApplication.Instance.Logger.Log("Method DownloadAndSetImageToEachUserInEachRow executed Successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DownloadAndSetImageToEachUserInEachRow() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DownloadAndSetImageToEachUserInEachRow() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in DownloadAndSetImageToEachUserInEachRow() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        
        private void AddDataTableColumns()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddDataTableColumns ...", category: Category.Info, priority: Priority.Low);
                
                dttable.Columns.Clear();
                Dttable.Columns.Add("IdOffer", typeof(long));
                Dttable.Columns.Add("ChildOtIdsCommaSeparated", typeof(string));
                // Dttable.Columns.Add("IdOt", typeof(long));
                Dttable.Columns.Add("OfferType", typeof(byte));
                Dttable.Columns.Add("Type", typeof(string));
                Dttable.Columns.Add("Code", typeof(string));
                Dttable.Columns.Add("Description", typeof(string));
                Dttable.Columns.Add("Modules", typeof(string));
                Dttable.Columns.Add("Group", typeof(string));
                Dttable.Columns.Add("Plant", typeof(string));
                Dttable.Columns.Add("Country", typeof(string));
                Dttable.Columns.Add("Project", typeof(string));
                Dttable.Columns.Add("OEM", typeof(string));
                Dttable.Columns.Add("BusinessUnit", typeof(string));
                Dttable.Columns.Add("PODate", typeof(DateTime));
                Dttable.Columns.Add("Status", typeof(string));
                Dttable.Columns.Add("HtmlColor", typeof(string));
                Dttable.Columns.Add("DeliveryDate", typeof(DateTime));
                Dttable.Columns.Add("ExpectedDeliveryWeek", typeof(string));
                Dttable.Columns.Add("Delay", typeof(Int32));
                Dttable.Columns.Add("Progress", typeof(string));
                Dttable.Columns.Add("PlannedStartDate", typeof(DateTime));
                Dttable.Columns.Add("PlannedEndDate", typeof(DateTime));
                Dttable.Columns.Add("OfferStartDateMinValue", typeof(DateTime));
                Dttable.Columns.Add("PlannedDuration", typeof(string));
                Dttable.Columns.Add("RealStartDate", typeof(DateTime));
                Dttable.Columns.Add("RealEndDate", typeof(DateTime));                
                Dttable.Columns.Add("RealDuration", typeof(string));
                Dttable.Columns.Add("Category1", typeof(string));
                Dttable.Columns.Add("Category2", typeof(string));
                Dttable.Columns.Add("OperatorNames", typeof(string));
                Dttable.Columns.Add("ChildId", typeof(string)).DefaultValue = string.Empty;
                Dttable.Columns.Add("ParentId", typeof(string)).DefaultValue = string.Empty;
                Dttable.Columns.Add("IsParent", typeof(bool));
                Dttable.Columns.Add("ParentOperatorNamesNull", typeof(string)).DefaultValue = string.Empty;
                Dttable.Columns.Add("Remarks", typeof(string));
                Dttable.Columns.Add("allChildOtAssignedUser", typeof(Object));
                Dttable.Columns.Add("allChildOtWorkLogItemList", typeof(Object));
                
                GeosApplication.Instance.Logger.Log("Method AddDataTableColumns executed Successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddDataTableColumns() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddDataTableColumns() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddDataTableColumns() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        
        public void Dispose()
        {
            GeosApplication.Instance.Logger.Log("Dispose DashboardViewModel....", category: Category.Info, priority: Priority.Low);
            dttable = null;
            GC.SuppressFinalize(this);
            GeosApplication.Instance.Logger.Log("Dispose DashboardViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
        }
                        
        private void FillListOfColor()
        {
            try
            {
                GeosAppSettingList = WorkbenchService.GetSelectedGeosAppSettings("14,15,16,17");
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
        
        #endregion //End Of Methods
    }
    

}
