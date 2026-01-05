using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Serialization;
using DevExpress.Xpf.Editors.Flyout;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using DevExpress.Xpf.WindowsUI;
using DevExpress.XtraPrinting;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.SAM.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System.Windows.Controls;
using DevExpress.Export;
using DevExpress.Export.Xl;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Threading;

namespace Emdep.Geos.Modules.SAM.ViewModels
{
    public class OrdersReportViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable
    {

        #region Services
                
        ISAMService SAMService = new SAMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchService = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion // End Services Region

        #region Declaration

        string fromDate;
        string toDate;
        int isButtonStatus;
        DateTime startDate;
        DateTime endDate;
        private Duration _currentDuration;
        const string shortDateFormat = "dd/MM/yyyy";
        public Visibility isCalendarVisible;

        private List<Ots> mainOtsList = new List<Ots>();
                
        private DataTable dttable;
        private DataTable dttableCopy;
        private bool isBusy;
        private int visibleRowCount;
        private bool isWorkOrderColumnChooserVisible;
        public string SAM_OrdersReportView__SettingPath = GeosApplication.Instance.UserSettingFolderName + "SAM_OrdersReportView__Setting.Xml";        
        private List<Ots> mainOtsList_New = new List<Ots>();
        private int visibleChildRowCount;
        #endregion // End Of Declaration

        #region Properties

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
        public DataTable Dttable
        {
            get { return dttable; }
            set
            {
                dttable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Dttable"));
            }
        }
        public DataTable DttableCopy
        {
            get { return dttableCopy; }
            set
            {
                dttableCopy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DttableCopy"));
            }
        }

        public virtual string ResultFileName { get; set; }

        public virtual bool DialogResult { get; set; }
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

        public bool IsWorkOrderColumnChooserVisible
        {
            get { return isWorkOrderColumnChooserVisible; }
            set
            {
                isWorkOrderColumnChooserVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsWorkOrderColumnChooserVisible"));
            }
        }

        public List<Ots> MainOtsList_New
        {
            get { return mainOtsList_New; }
            set
            {
                mainOtsList_New = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MainOtsList_New"));
            }
        }

        public int VisibleChildRowCount
        {
            get { return visibleChildRowCount; }
            set
            {
                visibleChildRowCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("VisibleChildRowCount"));
            }
        }

        #region //Shubham[skadam] [V.2.6.7.0] GEOS2-8849&GEOS2-8855&GEOS2-8859 SAM module very slow when trying to load informations - Orders ->Structures 16 09 2025

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

        public ICommand PeriodCommand { get; set; }
        public ICommand PeriodCustomRangeCommand { get; set; }
        public ICommand ApplyCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public ICommand PlantOwnerPopupClosed { get; private set; }
        public ICommand RefreshWorkOrderViewCommand { get; set; }
        public ICommand PrintWorkOrderViewCommand { get; set; }
        public ICommand ExportWorkOrderViewCommand { get; set; }
        public ICommand TableViewLoadedCommand { get; set; }
        public ICommand TableViewUnloadedCommand { get; set; }
        public ICommand WorkOrderViewHyperlinkClickCommand { get; set; }
       
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

        public OrdersReportViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor OrdersReportViewModel....", category: Category.Info, priority: Priority.Low);

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
                RefreshWorkOrderViewCommand = new RelayCommand(new Action<object>(RefreshWorkOrderList));
                PrintWorkOrderViewCommand = new RelayCommand(new Action<object>(PrintWorkOrderList));
                ExportWorkOrderViewCommand = new RelayCommand(new Action<object>(ExportWorkOrderList));
                TableViewLoadedCommand = new DelegateCommand<RoutedEventArgs>(TableViewLoadedCommandAction);
                TableViewUnloadedCommand = new DelegateCommand<RoutedEventArgs>(TableViewUnloadedCommandAction);
                WorkOrderViewHyperlinkClickCommand = new DelegateCommand<object>(ShowSelectedGridRowItemWindowAction);

                PeriodCommand = new DelegateCommand<object>(PeriodCommandAction);
                PeriodCustomRangeCommand = new DelegateCommand<object>(PeriodCustomRangeCommandAction);
                ApplyCommand = new DelegateCommand<object>(ApplyCommandAction);
                CancelCommand = new DelegateCommand<object>(CancelCommandAction);

                setDefaultPeriod();
                IsCalendarVisible = Visibility.Collapsed;
                StartDate = DateTime.Today.Date;
                EndDate = DateTime.Today.Date;
                /*
                LoadData();
                */
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Constructor OrdersReportViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OrdersReportViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        #endregion //End Of Constructor

        #region Methods

        private void LoadData()
        {
            FillMainOtList();            
        }

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
            /*
            LoadData();
            */
            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            PlantOwnerPopupClosedActionAsync();
            GeosApplication.Instance.Logger.Log("Method PlantOwnerPopupClosedAction executed successfully.", category: Category.Info, priority: Priority.Low);
        }
        //Shubham[skadam] [V.2.6.7.0] GEOS2-8849&GEOS2-8855&GEOS2-8859 SAM module very slow when trying to load informations - Orders ->Structures 16 09 2025
        public async Task PlantOwnerPopupClosedActionAsync()
        {
            try
            {
                await FillMainOtListAsync();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method PlantOwnerPopupClosedActionAsync()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void RefreshWorkOrderList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshWorkOrderList()...", category: Category.Info, priority: Priority.Low);
                
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
                /*
                var tableView = (TableView)obj;
                tableView.BeginInit();
                tableView.SearchString = null;
                LoadData();
                tableView.EndInit();
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                */
                RefreshWorkOrderListAsync(obj);
                GeosApplication.Instance.Logger.Log("Method RefreshWorkOrderList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshWorkOrderList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //Shubham[skadam] [V.2.6.7.0] GEOS2-8849&GEOS2-8855&GEOS2-8859 SAM module very slow when trying to load informations - Orders ->Structures 16 09 2025
        public async Task RefreshWorkOrderListAsync(object obj)
        {
            try
            {
                var tableView = (TableView)obj;
                tableView.BeginInit();
                tableView.SearchString = null;
                await FillMainOtListAsync();
                tableView.EndInit();
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshWorkOrderList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //Shubham[skadam] [V.2.6.7.0] GEOS2-8849&GEOS2-8855&GEOS2-8859 SAM module very slow when trying to load informations - Orders ->Structures 16 09 2025
        public async Task InitAsync()
        {
            GeosApplication.Instance.Logger.Log("Constructor Init....", category: Category.Info, priority: Priority.Low);
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
                await FillMainOtListAsync();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //Shubham[skadam] [V.2.6.7.0] GEOS2-8849&GEOS2-8855&GEOS2-8859 SAM module very slow when trying to load informations - Orders ->Structures 16 09 2025
        private async Task FillMainOtListAsync_notworking()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillMainOtListAsync...", Category.Info, Priority.Low);
                if (DXSplashScreen.IsActive) DXSplashScreen.Close();
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
                },x => new SplashScreenCustomView() { DataContext = new SplashScreenViewModel() },null, null);

                MainOtsList = new List<Ots>();
                FailedPlants = new List<string>();
                SuccessPlantList = new List<string>();
                IsShowFailedPlantWarning = false;
                WarningFailedPlants = string.Empty;
                GeosApplication.Instance.CustomeSplashScreenMessage = "Please wait";
                GeosApplication.Instance.CustomeSplashScreenInformationMessage = "Collecting the information from plants :";
                GeosApplication.Instance.StatusMessages = new ObservableCollection<Data.Common.Crm.StatusMessage>();
                GeosApplication.Instance.Logger.Log("Method FillMainOtListAsync started...", Category.Info, Priority.Low);
                if (SAMCommon.Instance.SelectedPlantOwnerList != null)
                {
                    var mainOtsList = new List<Ots>();

                    // Initialize status for all plants
                    // Add initial status for all plants
                    foreach (Company com in SAMCommon.Instance.SelectedPlantOwnerList)
                    {
                        GeosApplication.Instance.StatusMessages.Add(new Data.Common.Crm.StatusMessage() { Symbol = "", Message = com.ShortName, IsSuccess = 0 });
                    }
                    try
                    {
                        foreach (Company plant in SAMCommon.Instance.SelectedPlantOwnerList)
                        {
                            try
                            {
                                GeosApplication.Instance.SplashScreenMessage = "Connecting to " + plant.Alias;
                                // Offload blocking service call
                                var tempMainOtsList = await Task.Run(() =>SAMService.GetSAMWorkOrdersReport_V2260(plant,DateTime.ParseExact(FromDate, shortDateFormat, null),DateTime.ParseExact(ToDate, shortDateFormat, null)));

                                lock (mainOtsList)
                                {
                                    mainOtsList.AddRange(tempMainOtsList);
                                    SuccessPlantList.Add(plant.Alias);
                                    var statusMsg = GeosApplication.Instance.StatusMessages.FirstOrDefault(x => x.Message == plant.Alias);
                                    if (statusMsg != null)statusMsg.IsSuccess = 1;
                                }
                            }
                            catch (Exception ex)
                            {
                                FailedPlants.Add(plant.Alias);
                                var statusMsg = GeosApplication.Instance.StatusMessages.FirstOrDefault(x => x.Message == plant.Alias);
                                IsShowFailedPlantWarning = true;
                                WarningFailedPlants = string.Format((string)Application.Current.FindResource("DataLoadingFailMessage"), string.Join(",", FailedPlants));
                                if (statusMsg != null) statusMsg.IsSuccess = 2;
                                GeosApplication.Instance.Logger.Log( $"Error fetching data for plant {plant.Alias}: {ex.Message}", Category.Exception, Priority.Low);
                            }
                        }
                        // Clean invalid shipment dates
                        foreach (var ot in mainOtsList)
                        {
                            if (ot.FirstShipment != null && ot.FirstShipment.DeliveryDate == DateTime.MinValue)
                                ot.FirstShipment = null;
                        }
                        MainOtsList = new List<Ots>(mainOtsList);
                    }
                    catch (FaultException<ServiceException> ex)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Error in FillMainOtListAsync() method " + ex.Detail.ErrorMessage, Category.Exception, Priority.Low);
                        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                    catch (ServiceUnexceptedException ex)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Error in FillMainOtListAsync() Method - ServiceUnexceptedException " + ex.Message, Category.Exception, Priority.Low);
                        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                    }
                    catch (Exception ex)
                    {
                        IsBusy = false;
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Error in Method FillMainOtListAsync()...." + ex.Message, Category.Exception, Priority.Low);
                    }
                }
                else
                {
                    MainOtsList = new List<Ots>();
                }

                GeosApplication.Instance.Logger.Log("Method FillMainOtListAsync executed successfully.", Category.Info, Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillMainOtListAsync() method " + ex.Message, Category.Exception, Priority.Low);
            }
        }
        //Shubham[skadam] [V.2.6.7.0] GEOS2-8849&GEOS2-8855&GEOS2-8859 SAM module very slow when trying to load informations - Orders ->Structures 16 09 2025
        private async Task FillMainOtList_Async()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillMainOtListAsync...", Category.Info, Priority.Low);

                if (DXSplashScreen.IsActive) DXSplashScreen.Close();

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
                }, x => new SplashScreenCustomView() { DataContext = new SplashScreenViewModel() }, null, null);
                var mainOtsList = new List<Ots>();
                FailedPlants = new List<string>();
                SuccessPlantList = new List<string>();
                IsShowFailedPlantWarning = false;
                WarningFailedPlants = string.Empty;
                GeosApplication.Instance.CustomeSplashScreenMessage = "Please wait";
                GeosApplication.Instance.CustomeSplashScreenInformationMessage = "Collecting the information from plants :";
                GeosApplication.Instance.StatusMessages = new ObservableCollection<Data.Common.Crm.StatusMessage>();
                GeosApplication.Instance.Logger.Log("Method FillMainOtListAsync started...", Category.Info, Priority.Low);
                if (SAMCommon.Instance.SelectedPlantOwnerList != null)
                {
                    // Add initial status for all plants
                    foreach (Company com in SAMCommon.Instance.SelectedPlantOwnerList)
                    {
                        GeosApplication.Instance.StatusMessages.Add(new Data.Common.Crm.StatusMessage() { Symbol = "", Message = com.ShortName, IsSuccess = 0 });
                    }
                    try
                    {
                        var semaphore = new SemaphoreSlim(10); // allow up to 10 plants in parallel
                        var tasks = SAMCommon.Instance.SelectedPlantOwnerList.Cast<Company>().Select(async plant =>
                        {
                            await semaphore.WaitAsync();
                            try
                            {
                                GeosApplication.Instance.SplashScreenMessage = "Connecting to " + plant.Alias;
                                var tempMainOtsList = await Task.Run(() =>SAMService.GetSAMWorkOrdersReport_V2260(plant,DateTime.ParseExact(FromDate, shortDateFormat, null), DateTime.ParseExact(ToDate, shortDateFormat, null)));
                                lock (mainOtsList)
                                {
                                    mainOtsList.AddRange(tempMainOtsList);
                                    SuccessPlantList.Add(plant.Alias);
                                    var statusMsg = GeosApplication.Instance.StatusMessages.FirstOrDefault(x => x.Message == plant.Alias);
                                    if (statusMsg != null)statusMsg.IsSuccess = 1;
                                }
                            }
                            catch (Exception ex)
                            {
                                FailedPlants.Add(plant.Alias);
                                var statusMsg = GeosApplication.Instance.StatusMessages.FirstOrDefault(x => x.Message == plant.Alias);
                                WarningFailedPlants = string.Format((string)Application.Current.FindResource("DataLoadingFailMessage"), string.Join(",", FailedPlants));
                                if (statusMsg != null) statusMsg.IsSuccess = 2;
                                GeosApplication.Instance.Logger.Log($"Error fetching data for plant {plant.Alias}: {ex.Message}",Category.Exception, Priority.Low);
                            }
                            finally
                            {
                                semaphore.Release();
                            }
                        });

                        await Task.WhenAll(tasks);

                        foreach (var ot in mainOtsList)
                        {
                            if (ot.FirstShipment != null && ot.FirstShipment.DeliveryDate == DateTime.MinValue)
                                ot.FirstShipment = null;
                        }

                        MainOtsList = new List<Ots>(mainOtsList);
                    }
                    catch (FaultException<ServiceException> ex)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Error in FillMainOtListAsync() method " + ex.Detail.ErrorMessage, Category.Exception, Priority.Low);
                        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                    catch (ServiceUnexceptedException ex)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Error in FillMainOtListAsync() Method - ServiceUnexceptedException " + ex.Message, Category.Exception, Priority.Low);
                        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                    }
                    catch (Exception ex)
                    {
                        IsBusy = false;
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Error in Method FillMainOtListAsync()...." + ex.Message, Category.Exception, Priority.Low);
                    }
                }
                else
                {
                    MainOtsList = new List<Ots>();
                }

                GeosApplication.Instance.Logger.Log("Method FillMainOtListAsync executed successfully.", Category.Info, Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillMainOtListAsync() method " + ex.Message, Category.Exception, Priority.Low);
            }
        }

        private async Task FillMainOtListAsync()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillMainOtListAsync...", Category.Info, Priority.Low);
                if (DXSplashScreen.IsActive) DXSplashScreen.Close();
                DXSplashScreen.Show(
                    x =>
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
                    },
                    x => new SplashScreenCustomView() { DataContext = new SplashScreenViewModel() },
                    null,
                    null
                );

                MainOtsList = new List<Ots>();
                FailedPlants = new List<string>();
                SuccessPlantList = new List<string>();
                IsShowFailedPlantWarning = false;
                WarningFailedPlants = string.Empty;
                GeosApplication.Instance.CustomeSplashScreenMessage = "Please wait";
                GeosApplication.Instance.CustomeSplashScreenInformationMessage = "Collecting the information from plants :";
                GeosApplication.Instance.StatusMessages = new ObservableCollection<Data.Common.Crm.StatusMessage>();
                GeosApplication.Instance.Logger.Log("Method FillMainOtListAsync started...", Category.Info, Priority.Low);
                if (SAMCommon.Instance.SelectedPlantOwnerList != null)
                {
                    var mainOtsList = new List<Ots>();
                    // Add initial status for all plants
                    foreach (Company com in SAMCommon.Instance.SelectedPlantOwnerList)
                    {
                        GeosApplication.Instance.StatusMessages.Add(new Data.Common.Crm.StatusMessage() { Symbol = "", Message = com.ShortName, IsSuccess = 0 });
                    }
                    try
                    {
                        // Create tasks for all plants
                        var tasks = SAMCommon.Instance.SelectedPlantOwnerList.Cast<Company>().Select(async plant =>
                        {
                            try
                            {
                                GeosApplication.Instance.SplashScreenMessage = "Connecting to " + plant.Alias;
                                var tempMainOtsList = await Task.Run(() =>SAMService.GetSAMWorkOrdersReport_V2260(plant,DateTime.ParseExact(FromDate, shortDateFormat, null),DateTime.ParseExact(ToDate, shortDateFormat, null)));
                                lock (mainOtsList)
                                {
                                    mainOtsList.AddRange(tempMainOtsList);
                                    SuccessPlantList.Add(plant.Alias);
                                    var statusMsg = GeosApplication.Instance.StatusMessages.FirstOrDefault(x => x.Message == plant.Alias);
                                    if (statusMsg != null) statusMsg.IsSuccess = 1;
                                }
                            }
                            catch (Exception ex)
                            {
                                lock (FailedPlants)
                                {
                                    FailedPlants.Add(plant.Alias);
                                    var statusMsg = GeosApplication.Instance.StatusMessages.FirstOrDefault(x => x.Message == plant.Alias);
                                    if (statusMsg != null) statusMsg.IsSuccess = 2;
                                    IsShowFailedPlantWarning = true;
                                    WarningFailedPlants = string.Format((string)Application.Current.FindResource("DataLoadingFailMessage"),string.Join(",", FailedPlants)
                                    );
                                }
                                GeosApplication.Instance.Logger.Log($"Error fetching data for plant {plant.Alias}: {ex.Message}",Category.Exception,Priority.Low);
                            }
                        });
                        // Wait for all tasks to finish
                        await Task.WhenAll(tasks);
                        // Clean invalid shipment dates
                        foreach (var ot in mainOtsList)
                        {
                            if (ot.FirstShipment != null && ot.FirstShipment.DeliveryDate == DateTime.MinValue)
                                ot.FirstShipment = null;
                        }
                        MainOtsList = new List<Ots>(mainOtsList);
                    }
                    catch (FaultException<ServiceException> ex)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Error in FillMainOtListAsync() method " + ex.Detail.ErrorMessage,Category.Exception,Priority.Low);
                        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null),"Red",CustomMessageBox.MessageImagePath.NotOk,MessageBoxButton.OK);
                    }
                    catch (ServiceUnexceptedException ex)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Error in FillMainOtListAsync() Method - ServiceUnexceptedException " + ex.Message,Category.Exception,Priority.Low);
                        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType,GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(),null);
                    }
                    catch (Exception ex)
                    {
                        IsBusy = false;
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Error in Method FillMainOtListAsync()...." + ex.Message,Category.Exception,Priority.Low);
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
                GeosApplication.Instance.Logger.Log("Method FillMainOtListAsync executed successfully.", Category.Info, Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillMainOtListAsync() method " + ex.Message, Category.Exception, Priority.Low);
            }
            finally
            {
                if (DXSplashScreen.IsActive) DXSplashScreen.Close();
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
                            TempMainOtsList = new List<Ots>(SAMService.GetSAMWorkOrdersReport_V2260(plant,
                            DateTime.ParseExact(FromDate, shortDateFormat, null),
                            DateTime.ParseExact(ToDate, shortDateFormat, null)));

                            MainOtsList.AddRange(TempMainOtsList);                            
                        }

                        foreach (var ot in MainOtsList)
                        {
                            if (ot.FirstShipment != null && ot.FirstShipment.DeliveryDate == DateTime.MinValue) ot.FirstShipment = null;
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
        
        private void ShowSelectedGridRowItemWindowAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowSelectedGridRowItemWindowAction....", category: Category.Info, priority: Priority.Low);

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
                
                TableView detailView = (TableView)obj;
                var ot = (Ots)detailView.DataControl.CurrentItem;

                long OtId = ot.IdOT;

                Ots FoundRow = MainOtsList.Where(mol => mol.IdOT == OtId).FirstOrDefault();
                //[001]
                if (FoundRow.Quotation.IdDetectionsTemplate == 24)
                {
                    WorkOrderItemDetailsForStructureViewModel workOrderItemDetailsForStructureViewModel = new WorkOrderItemDetailsForStructureViewModel();
                    WorkOrderItemDetailsForStructureView workOrderItemDetailsForStructureView = new WorkOrderItemDetailsForStructureView();

                    EventHandler handle1 = delegate { workOrderItemDetailsForStructureView.Close(); };
                    workOrderItemDetailsForStructureViewModel.RequestClose += handle1;
                    workOrderItemDetailsForStructureViewModel.Init(FoundRow);
                    workOrderItemDetailsForStructureView.DataContext = workOrderItemDetailsForStructureViewModel;
                    workOrderItemDetailsForStructureView.ShowDialogWindow();
                    if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                }
                else
                {
                    WorkOrderItemDetailsViewModel workOrderItemDetailsViewModel = new WorkOrderItemDetailsViewModel();
                    WorkOrderItemDetailsView workOrderItemDetailsView = new WorkOrderItemDetailsView();

                    EventHandler handle = delegate { workOrderItemDetailsView.Close(); };
                    workOrderItemDetailsViewModel.RequestClose += handle;
                    workOrderItemDetailsViewModel.Init(FoundRow);
                    workOrderItemDetailsView.DataContext = workOrderItemDetailsViewModel;
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    workOrderItemDetailsView.ShowDialogWindow();
                    if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                    if (workOrderItemDetailsViewModel.WorkflowStatus != null)
                    {
                        MainOtsList_New.Where(mol => mol.IdOT == OtId).FirstOrDefault().WorkflowStatus = workOrderItemDetailsViewModel.WorkflowStatus;
                        MainOtsList_New.Where(mol => mol.IdOT == OtId).FirstOrDefault().WorkflowStatus.IdWorkflowStatus = workOrderItemDetailsViewModel.WorkflowStatus.IdWorkflowStatus;
                        MainOtsList.Where(mol => mol.IdOT == OtId).FirstOrDefault().WorkflowStatus = workOrderItemDetailsViewModel.WorkflowStatus;
                        MainOtsList.Where(mol => mol.IdOT == OtId).FirstOrDefault().WorkflowStatus.IdWorkflowStatus = workOrderItemDetailsViewModel.WorkflowStatus.IdWorkflowStatus;
                    }
                    MainOtsList.Where(mol => mol.IdOT == OtId).FirstOrDefault().Observations = workOrderItemDetailsViewModel.Remark;
                }
                
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method ShowSelectedGridRowItemWindowAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ShowSelectedGridRowItemWindowAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void PrintWorkOrderList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintWorkOrderList()...", category: Category.Info, priority: Priority.Low);
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
                PrintableControlLink pcl = new PrintableControlLink((TableView)obj);
                pcl.Margins.Bottom = 5;
                pcl.Margins.Top = 5;
                pcl.Margins.Left = 5;
                pcl.Margins.Right = 5;
                //pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["WorkOrderListReportPrintHeaderTemplate"];
                //pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["WorkOrderListReportPrintFooterTemplate"];
                pcl.Landscape = true;

                pcl.PaperKind = System.Drawing.Printing.PaperKind.A3;
                pcl.CreateDocument(false);
                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                IsBusy = false;
                window.Show();

                DevExpress.XtraPrinting.PrintTool printTool = new DevExpress.XtraPrinting.PrintTool(pcl.PrintingSystem);
                printTool.PrinterSettings.PrinterName = GeosApplication.Instance.UserSettings["SelectedPrinter"];
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method PrintWorkOrderList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintWorkOrderList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ExportWorkOrderList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportWorkOrderList()...", category: Category.Info, priority: Priority.Low);
                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Orders Report";
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
                    TableView activityTableView = ((TableView)obj);
                    activityTableView.ShowTotalSummary = false;
                    activityTableView.ShowFixedTotalSummary = false;
                    XlsxExportOptionsEx options = new XlsxExportOptionsEx();
                    options.CustomizeCell += Options_CustomizeCell; 
                    activityTableView.ExportToXlsx(ResultFileName, options);
                    IsBusy = false;
                    activityTableView.ShowFixedTotalSummary = true;
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);
                    GeosApplication.Instance.Logger.Log("Method ExportPendingWorkOrderList()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportPendingWorkOrderList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void Options_CustomizeCell(CustomizeCellEventArgs e)
        {            
            try
            {
                GeosApplication.Instance.Logger.Log("Method Options_CustomizeCell()...", category: Category.Info, priority: Priority.Low);
                if (e.ColumnFieldName == "CostDeviation")
                {
                    e.Formatting.Alignment = new XlCellAlignment() { HorizontalAlignment = XlHorizontalAlignment.Right };
                    e.Formatting.FormatType = DevExpress.Utils.FormatType.Numeric;
                    e.Formatting.NumberFormat = "0%";
                }
            
                e.Handled = true;
                GeosApplication.Instance.Logger.Log("Method Options_CustomizeCell()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Options_CustomizeCell()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void TableViewLoadedCommandAction(RoutedEventArgs obj)
        {
            try
            {
               GeosApplication.Instance.Logger.Log("Method TableViewLoadedCommandAction ...", category: Category.Info, priority: Priority.Low);
                int visibleFalseCoulumn = 0;

                if (File.Exists(SAM_OrdersReportView__SettingPath))
                {
                    ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.RestoreLayoutFromXml(SAM_OrdersReportView__SettingPath);
                    GridControl GridControlSTDetails = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;
                    TableView tableView = (TableView)GridControlSTDetails.View;
                    if (tableView.SearchString != null)
                    {
                        tableView.SearchString = null;
                    }
                }

                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.AddHandler(DXSerializer.AllowPropertyEvent, new AllowPropertyEventHandler(OnAllowProperty));

                //This code for save grid layout.
                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.SaveLayoutToXml(SAM_OrdersReportView__SettingPath);

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
                    IsWorkOrderColumnChooserVisible = true;
                }
                else
                {
                    IsWorkOrderColumnChooserVisible = false;
                }
                                
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
                    ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(SAM_OrdersReportView__SettingPath);
                }

                if (column.Visible == false)
                {
                    IsWorkOrderColumnChooserVisible = true;
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
                ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(SAM_OrdersReportView__SettingPath);

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
                GeosApplication.Instance.Logger.Log("Method TableViewUnloadedCommandAction ...", category: Category.Info, priority: Priority.Low);

                GridControl gridControlInstance = ((WorkPlanningView)obj.OriginalSource).grid; // ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;
                TableView tableView = (TableView)gridControlInstance.View;

                tableView.SearchString = string.Empty;
                if (gridControlInstance.GroupCount > 0)
                    gridControlInstance.ClearGrouping();
                gridControlInstance.ClearSorting();
                gridControlInstance.FilterString = null;
                gridControlInstance.SaveLayoutToXml(SAM_OrdersReportView__SettingPath);

                GeosApplication.Instance.Logger.Log("Method TableViewUnloadedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in TableViewUnloadedCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        public void Dispose()
        {
            Dttable = null;
            DttableCopy = null;
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

        private void setDefaultPeriod()
        {
            try
            {
                int year = DateTime.Now.Year;
                DateTime StartFromDate = new DateTime(year, 1, 1);
                DateTime EndToDate = new DateTime(year, 12, 31);

                FromDate = StartFromDate.ToString(shortDateFormat);
                ToDate = EndToDate.ToString(shortDateFormat);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method setDefaultPeriod()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
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
                    FromDate = thisMonthStart.ToString(shortDateFormat);
                    ToDate = thisMonthEnd.ToString(shortDateFormat);
                }
                else if (IsButtonStatus == 2)//last one month
                {
                    FromDate = lastOneMonthStart.ToString(shortDateFormat);
                    ToDate = lastOneMonthEnd.ToString(shortDateFormat);
                }
                else if (IsButtonStatus == 3) //last month
                {
                    FromDate = lastMonthStart.ToString(shortDateFormat);
                    ToDate = lastMonthEnd.ToString(shortDateFormat);
                }
                else if (IsButtonStatus == 4) //this week
                {
                    FromDate = thisWeekStart.ToString(shortDateFormat);
                    ToDate = thisWeekEnd.ToString(shortDateFormat);
                }
                else if (IsButtonStatus == 5) //last one week
                {
                    FromDate = lastOneWeekStart.ToString(shortDateFormat);
                    ToDate = lastOneWeekEnd.ToString(shortDateFormat);
                }
                else if (IsButtonStatus == 6) //last week
                {
                    FromDate = lastWeekStart.ToString(shortDateFormat);
                    ToDate = lastWeekEnd.ToString(shortDateFormat);
                }
                else if (IsButtonStatus == 7) //custome range
                {
                    FromDate = StartDate.ToString(shortDateFormat);
                    ToDate = EndDate.ToString(shortDateFormat);
                }
                else if (IsButtonStatus == 8)//this year
                {
                    setDefaultPeriod();
                }
                else if (IsButtonStatus == 9)//last year
                {
                    FromDate = StartFromDate.ToString(shortDateFormat);
                    ToDate = EndToDate.ToString(shortDateFormat);
                }

                else if (IsButtonStatus == 10)//last 12 month
                {
                    DateTime Date_F = DateTime.Now.Date.AddMonths(-12);
                    DateTime Date_T = DateTime.Now.Date;
                    FromDate = Date_F.ToShortDateString();
                    ToDate = Date_T.ToShortDateString();
                }
                /*
                FillMainOtList();
                */
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                InitAsync();
                IsBusy = false;
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

        private void PeriodCustomRangeCommandAction(object obj)
        {
            IsButtonStatus = 7;
            IsCalendarVisible = Visibility.Visible;
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


        #endregion //End Of Methods
    }

}
