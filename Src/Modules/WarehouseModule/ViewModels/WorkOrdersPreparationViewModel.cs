using DevExpress.Data.Filtering.Helpers;
using DevExpress.Data.Filtering;
using DevExpress.Export;
using DevExpress.Export.Xl;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Serialization;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrinting.Preview;
using DevExpress.XtraReports.UI;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.Warehouse.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.Utility;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DevExpress.Xpf.WindowsUI;
using DevExpress.Xpf.Editors.Flyout;
using System.Windows.Controls;
using DevExpress.Xpf.Charts;
using System.Threading;
using System.Globalization;

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    public class WorkOrdersPreparationViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable
    {
        #region Services
        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        IWarehouseService WarehouseService = new WarehouseServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchService = new WorkbenchServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        //IWarehouseService WarehouseService = new WarehouseServiceController("localhost:6699");
       //IWorkbenchStartUp WorkbenchService = new WorkbenchServiceController("localhost:6699");


        #endregion // End Services Region

        #region Declaration
        ChartControl chartcontrol;
        string fromDate;
        string toDate;
        int isButtonStatus;
        DateTime startDate;
        DateTime endDate;
        private Duration _currentDuration;
        const string shortDateFormat = "dd/MM/yyyy";
        public Visibility isCalendarVisible;

        private ObservableCollection<TileBarFilters> listOfFilterTile = new ObservableCollection<TileBarFilters>();
        private ObservableCollection<Template> listOfTemplate = new ObservableCollection<Template>();

        private List<Ots> mainOtsList = new List<Ots>();
        private List<Ots> filterWiseListOfWorkOrder = new List<Ots>();

        private string filterString;
        private bool isBusy;
        private TileBarFilters selectedTileBarItem;
        private int visibleRowCount;
        private string userSettingsKey = "WMS_WorkOrder_";
        private bool isEdit;
        private bool isWorkOrderColumnChooserVisible;
        public string WorkOrderGridSettingFilePath = GeosApplication.Instance.UserSettingFolderName + "WorkOrderPreparationGridSettingFilePath.Xml";
        private object geosAppSettingList;

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

        public string FilterString
        {
            get { return filterString; }
            set
            {
                filterString = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FilterString"));
                InitChartControl();
            }
        }

        public List<Ots> FilterWiseListOfWorkOrder
        {
            get { return filterWiseListOfWorkOrder; }
            set
            {
                filterWiseListOfWorkOrder = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FilterWiseListOfWorkOrder"));
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

        public ObservableCollection<Template> ListOfTemplate
        {
            get { return listOfTemplate; }

            set
            {
                listOfTemplate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("listOfTemplate"));
            }
        }

        public ObservableCollection<TileBarFilters> ListOfFilterTile
        {
            get { return listOfFilterTile; }
            set
            {
                listOfFilterTile = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListOfFilterTile"));
            }
        }

        public TileBarFilters SelectedTileBarItem
        {
            get { return selectedTileBarItem; }
            set
            {
                selectedTileBarItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedTileBarItem"));
            }
        }
        public int VisibleRowCount
        {
            get { return visibleRowCount; }
            set
            {
                visibleRowCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("VisibleRowCount"));
            }
        }
        public bool IsEdit
        {
            get
            {
                return isEdit;
            }
            set
            {
                isEdit = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEdit"));
            }
        }
        public string CustomFilterStringName { get; set; }

        public bool IsWorkOrderColumnChooserVisible
        {
            get
            {
                return isWorkOrderColumnChooserVisible;
            }

            set
            {
                isWorkOrderColumnChooserVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsWorkOrderColumnChooserVisible"));
            }
        }

        public object GeosAppSettingList
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

        #endregion //End Of Properties

        #region Icommands

        public ICommand ChartItemsForecastLoadCommand { get; set; }
      //  public ICommand ChartDashboardSaleCustomDrawCrosshairCommand { get; set; }
        public ICommand PeriodCommand { get; set; }
        public ICommand PeriodCustomRangeCommand { get; set; }
        public ICommand ApplyCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand CustomShowFilterPopupCommand { get; set; }
        public ICommand CommandFilterTileClick { get; set; }
        public ICommand CommandGridDoubleClick { get; set; }
        public ICommand CommandWarehouseEditValueChanged { get; private set; }
        public ICommand RefreshWorkOrderViewCommand { get; set; }
        public ICommand PrintWorkOrderViewCommand { get; set; }
        public ICommand ExportWorkOrderViewCommand { get; set; }
        public ICommand ScanWorkOderCommand { get; set; }
        public ICommand RefundWorkOrderCommand { get; set; }
        public ICommand FilterEditorCreatedCommand { get; set; }
        public ICommand CommandTileBarDoubleClick { get; set; }
        public ICommand TableViewLoadedCommand { get; set; }
        public ICommand CustomCellAppearanceCommand { get; set; }

        public ICommand WorkOrderHyperlinkClickCommand { get; set; }

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
        public WorkOrdersPreparationViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor WorkOrdersPreparationViewModel....", category: Category.Info, priority: Priority.Low);

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


                ChartItemsForecastLoadCommand = new DevExpress.Mvvm.DelegateCommand<object>(ChatControlLoadedEvent);
              //  ChartDashboardSaleCustomDrawCrosshairCommand = new DevExpress.Mvvm.DelegateCommand<object>(ChartDashboardSaleCustomDrawCrosshairCommandAction);

                PeriodCommand = new DelegateCommand<object>(PeriodCommandAction);
                PeriodCustomRangeCommand = new DelegateCommand<object>(PeriodCustomRangeCommandAction);
                ApplyCommand = new DelegateCommand<object>(ApplyCommandAction);
                CancelCommand = new DelegateCommand<object>(CancelCommandAction);

                CommandFilterTileClick = new DelegateCommand<object>(ShowSelectedFilterGridAction);
                CommandGridDoubleClick = new DelegateCommand<object>(ShowSelectedGridRowItemWindowAction);
                CommandWarehouseEditValueChanged = new DevExpress.Mvvm.DelegateCommand<object>(WarehouseEditValueChangedCommandAction);
                RefreshWorkOrderViewCommand = new RelayCommand(new Action<object>(RefreshPendingWorkOrderList));
                PrintWorkOrderViewCommand = new RelayCommand(new Action<object>(PrintPendingWorkOrderList));
                ExportWorkOrderViewCommand = new RelayCommand(new Action<object>(ExportPendingWorkOrderList));
                ScanWorkOderCommand = new DevExpress.Mvvm.DelegateCommand<object>(ScanWorkOrder);
                RefundWorkOrderCommand = new DelegateCommand<object>(RefundWorkOrderCommandAction);
                FilterEditorCreatedCommand = new DelegateCommand<FilterEditorEventArgs>(FilterEditorCreatedCommandAction);
                CommandTileBarDoubleClick = new DelegateCommand<object>(CommandTileBarDoubleClickAction);
                TableViewLoadedCommand = new DelegateCommand<RoutedEventArgs>(TableViewLoadedCommandAction);
                CustomCellAppearanceCommand = new DelegateCommand<CustomRowAppearanceEventArgs>(CustomCellAppearanceAction);
                WorkOrderHyperlinkClickCommand = new DelegateCommand<object>(WorkOrderHyperlinkClickCommandAction);
                CustomShowFilterPopupCommand = new DelegateCommand<DevExpress.Xpf.Grid.FilterPopupEventArgs>(CustomShowFilterPopupAction);
                setDefaultPeriod();
                IsCalendarVisible = Visibility.Collapsed;
                StartDate = DateTime.Today.Date;
                EndDate = DateTime.Today.Date;

                FillMainOtList();
                FillListofColor();
                FillPendingWorkOrderFilterList();
                AddCustomSetting();
                if (ListOfFilterTile.Count > 0)
                    SelectedTileBarItem = ListOfFilterTile.FirstOrDefault();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Constructor WorkOrdersPreparationViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in WorkOrdersPreparationViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion //End Of Constructor

        #region Methods

        /// <summary>
        /// Method to open Work Order details view
        /// </summary>
        /// <param name="obj"></param>
        private void WorkOrderHyperlinkClickCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method HyperlinkClickCommandAction....", category: Category.Info, priority: Priority.Low);

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
                TableView detailView = (TableView)obj;
                var selected = (Ots)detailView.DataControl.CurrentItem;
                // Article article = selectedOffer.Quotations[0].Ots[0].OtItems[0].RevisionItem.WarehouseProduct.Article; //  detailView.FocusedRow;
                //string selectedWOMergeCode = selectedOffer1.Quotations[0].Ots[0].IdOT;

                //WOItem unPackedItem = (WOItem)obj;
                WorkOrderItemDetailsViewModel workOrderItemDetailsViewModel = new WorkOrderItemDetailsViewModel();
                WorkOrderItemDetailsView workOrderItemDetailsView = new WorkOrderItemDetailsView();
                EventHandler handle = delegate { workOrderItemDetailsView.Close(); };
                workOrderItemDetailsViewModel.RequestClose += handle;
                workOrderItemDetailsViewModel.OtSite = WarehouseCommon.Instance.Selectedwarehouse.Company;//[Sudhir.Jangra][GEOS2-5644]
                workOrderItemDetailsViewModel.Init(selected.IdOT, WarehouseCommon.Instance.Selectedwarehouse);
                //workOrderItemDetailsViewModel.
                workOrderItemDetailsView.DataContext = workOrderItemDetailsViewModel;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                workOrderItemDetailsView.ShowDialogWindow();
                //   FocusUserControl = true;
                detailView.Focus();
                GeosApplication.Instance.Logger.Log("Method HyperlinkClickCommandAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method HyperlinkClickCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
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

                //if (IsGridViewVisible == Visibility.Visible)
                //{
                    FillMainOtList();
                //}

                IsBusy = false;
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


        /// <summary>
        ///[001][smazhar][22-06-2020][GEOS2-2346]Add Country flag
        /// This method is for to convert from Bytearray to ImageSource
        /// </summary>
        /// <param name="byteArrayIn"></param>
        /// <returns></returns>
        public ImageSource ByteArrayToBitmapImage(byte[] byteArrayIn)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method convert byteArrayToImage ...", category: Category.Info, priority: Priority.Low);

                if (byteArrayIn == null || byteArrayIn.Length == 0) return null;
                var image = new BitmapImage();
                using (var mem = new System.IO.MemoryStream(byteArrayIn))
                {
                    mem.Position = 0;
                    image.BeginInit();
                    image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.UriSource = null;
                    image.StreamSource = mem;
                    image.EndInit();
                }

                image.Freeze();
                GeosApplication.Instance.Logger.Log("Method byteArrayToImage() executed successfully", category: Category.Info, priority: Priority.Low);

                return image;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in byteArrayToImage() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            return null;
        }
        /// <summary>
        ///[001][smazhar][22-06-2020][GEOS2-2346]Add Country flag
        ///[002][rdixit][GEOS2-3150][05.08.2022]
        /// </summary>
        private void FillMainOtList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillMainOtList...", category: Category.Info, priority: Priority.Low);

                if (WarehouseCommon.Instance.Selectedwarehouse != null)
                {
                    List<Ots> TempMainOtsList = new List<Ots>();
                    MainOtsList = new List<Ots>();

                    try
                    {
                        var WA = WarehouseService.GetAllWarehouseAvailabilityByIdCompany_V2140(
                        WarehouseCommon.Instance.Selectedwarehouse);
                        var packingWorkSchedule = WA.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 32);
                        var shippingWorkSchedule = WA.StageAvailabilityList.FirstOrDefault(x => x.StageInfo.IdStage == 30);
                        #region Service Methods Comments
                        ///[001]service method change
                        //TempMainOtsList = new List<Ots>(WarehouseService.GetPendingMaterialWorkOrdersByWarehouse_V2044(WarehouseCommon.Instance.Selectedwarehouse));
                        // FromDate = "04-01-2021"; ToDate = "04-01-2021";
                        //[001] Changed service method GetPendingMaterialWorkOrdersByWarehouse_V2044 to GetWorkOrdersPreparationReport_V2150
                        //[002] Changed service method GetWorkOrdersPreparationReport_V2150 to GetWorkOrdersPreparationReport_V2300 by [rdixit][GEOS2-3150][05.08.2022]
                        //[003] Changed service method GetWorkOrdersPreparationReport_V2300 to GetWorkOrdersPreparationReport_V2301 by [rdixit][14.09.2022][GEOS2-3902]
                        #endregion
                      //  TempMainOtsList = new List<Ots>(WarehouseService.GetWorkOrdersPreparationReport_V2301(WarehouseCommon.Instance.Selectedwarehouse,DateTime.ParseExact(FromDate, shortDateFormat, null),DateTime.ParseExact(ToDate, shortDateFormat, null)));

                        //[Sudhir.Jangra][GEOS2-5373]
                        //TempMainOtsList = new List<Ots>(WarehouseService.GetWorkOrdersPreparationReport_V2500(WarehouseCommon.Instance.Selectedwarehouse, DateTime.ParseExact(FromDate, shortDateFormat, null), DateTime.ParseExact(ToDate, shortDateFormat, null)));

                        //[pramod.misal][02.07.2024][GEOS2-5904]
                        TempMainOtsList = new List<Ots>(WarehouseService.GetWorkOrdersPreparationReport_V2540(WarehouseCommon.Instance.Selectedwarehouse, DateTime.ParseExact(FromDate, shortDateFormat, null), DateTime.ParseExact(ToDate, shortDateFormat, null)));


                        foreach (var item in TempMainOtsList)
                        {
                            if (item.FirstShipment != null)
                            {
                                if (item.FirstShipment.IdShipment == 0)
                                {
                                    item.FirstShipment = null;
                                    item.TotalTimeForShipmentCreatedIn = null;
                                    item.TotalTimeForShipmentDelivery = null;
                                }
                            }

                            if (item.FirstShipment != null)
                            {
                                if (item.FirstShipment.CreatedIn == DateTime.MinValue)
                                {
                                    item.FirstShipment = null;
                                    item.TotalTimeForShipmentCreatedIn = null;
                                    item.TotalTimeForShipmentDelivery = null;
                                }
                            }

                            if (item.FirstShipment != null &&
                                packingWorkSchedule != null &&
                                shippingWorkSchedule != null)
                            {
                                TimeSpan? totalWorkingTime = new TimeSpan();

                                TotalTimeCalculator.GetTotalTimeByCreatingDateTimeSlots(
                                    item.PoDate, item.FirstShipment.CreatedIn,
                                    out totalWorkingTime, packingWorkSchedule, item.TotalTimeForShipmentCreatedIn);
                                item.TotalTimeForShipmentCreatedIn = totalWorkingTime;

                                TotalTimeCalculator.GetTotalTimeByCreatingDateTimeSlots(
                                    item.PoDate, item.FirstShipment.DeliveryDate,
                                    out totalWorkingTime, packingWorkSchedule, item.TotalTimeForShipmentDelivery);
                                item.TotalTimeForShipmentDelivery = totalWorkingTime;
                            }
                        }
                        
                        
                        //Step 1 Get warehouse working hours data from database
                        //Step 2 Take anyone OT record like TempMainOtsList[0]
                        //Step 3 check calculated TotalTimeForShipmentCreatedIn is correct or not
                        // it is Difference of seconds between packing datetime and
                        //po reception datetime ignoring the non-working hours
                        //Step 4 start Calculating total Non-working hours between packing datetime and
                        //po reception datetime


                        //DateTime.Parse("2021-01-04"),
                        //DateTime.Parse("2021-01-04")));

                        //if (TempMainOtsList != null)
                        //{
                        //    foreach (var otitem in TempMainOtsList.GroupBy(tpa => tpa.Quotation.Site.Country.Iso))
                        //    {

                        //        ImageSource countryFlagImage = ByteArrayToBitmapImage(otitem.ToList().FirstOrDefault().Quotation.Site.Country.CountryIconBytes);
                        //        otitem.ToList().Where(oti => oti.Quotation.Site.Country.Iso == otitem.Key).ToList().ForEach(oti => oti.Quotation.Site.Country.CountryIconImage = countryFlagImage);
                        //    }
                        //}
                        MainOtsList.AddRange(TempMainOtsList);
                    }
                    catch (FaultException<ServiceException> ex)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Get an error in FillMainOtList() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                    catch (ServiceUnexceptedException ex)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Get an error in FillMainOtList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                    }

                    MainOtsList = new List<Ots>(MainOtsList);
                }
                else
                {
                    MainOtsList = new List<Ots>();
                }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method FillMainOtList executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillMainOtList() method " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// After warehouse selection  fill list as per it.
        /// </summary>
        /// <param name="obj"></param>
        private void WarehouseEditValueChangedCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method WarehousePopupClosedCommandAction...", category: Category.Info, priority: Priority.Low);

            //When setting the warehouse from default the data should not be refreshed
            if (!WarehouseCommon.Instance.IsWarehouseChangedEventCanOccur)
                return;

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

            //fill data as per selected warehouse
            FillMainOtList();
            //Update count for Templates
            FillPendingWorkOrderFilterList();
            AddCustomSetting();
            FilterString = string.Empty;
            if (ListOfFilterTile.Count > 0)
                SelectedTileBarItem = ListOfFilterTile.FirstOrDefault();
            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

            GeosApplication.Instance.Logger.Log("Method WarehousePopupClosedCommandAction executed successfully.", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method To Fill Pending Work Order Filter List
        /// </summary>
        public void FillPendingWorkOrderFilterList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillPendingWorkOrderFilterList....", category: Category.Info, priority: Priority.Low);

                ListOfTemplate = new ObservableCollection<Template>(WarehouseService.GetTemplatesByIdTemplateType(2));
                FillPendingWorkOrderFilterTiles(ListOfTemplate, MainOtsList);

                GeosApplication.Instance.Logger.Log("Method FillPendingWorkOrderFilterList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillPendingWorkOrderFilterList() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillPendingWorkOrderFilterList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillPendingWorkOrderFilterList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method To Fill Pending Work Order Filter Tiles 
        /// </summary>
        public void FillPendingWorkOrderFilterTiles(ObservableCollection<Template> FilterList, List<Ots> MainListWorkOrder)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillPendingWorkOrderFilterTiles....", category: Category.Info, priority: Priority.Low);

                ListOfFilterTile = new ObservableCollection<TileBarFilters>();

                ListOfFilterTile.Add(new TileBarFilters()
                {
                    Caption = string.Format(System.Windows.Application.Current.FindResource("AllWorkOrder").ToString()),
                    Id = 0,
                    EntitiesCount = MainListWorkOrder.Count(),
                    ImageUri = "Template.png",
                    BackColor = "Wheat",
                    EntitiesCountVisibility = Visibility.Visible,
                    Height = 80,
                    width = 200
                });

                foreach (Template item in FilterList)
                {
                    ListOfFilterTile.Add(new TileBarFilters()
                    {
                        Caption = item.Name.ToString(),
                        Id = item.IdTemplate,
                        Type = item.Name,
                        EntitiesCount = MainListWorkOrder.Count(x => x.Quotation.IdDetectionsTemplate == item.IdTemplate),
                        ImageUri = "Template.png",
                        BackColor = item.HtmlColor,
                        EntitiesCountVisibility = Visibility.Visible,
                        Height = 80,
                        width = 200
                    });
                }

                ListOfFilterTile.Add(new TileBarFilters()
                {
                    Caption = System.Windows.Application.Current.FindResource("CustomFilters").ToString(),
                    Id = 0,
                    BackColor = null,
                    ForeColor = null,
                    EntitiesCountVisibility = Visibility.Collapsed,
                    Height = 30,
                    width = 200,
                });

                GeosApplication.Instance.Logger.Log("Method FillPendingWorkOrderFilterTiles() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillPendingWorkOrderFilterTiles() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for showing Grid as per Filter Tile Selection
        /// </summary>
        private void ShowSelectedFilterGridAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowSelectedFilterGridAction....", category: Category.Info, priority: Priority.Low);

                if (ListOfFilterTile.Count > 0)
                {
                    //int IdTemplate = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).Id;
                    string Template = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).Type;
                    string _FilterString = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).FilterCriteria;
                    CustomFilterStringName = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).Caption;

                    if (CustomFilterStringName.Equals(System.Windows.Application.Current.FindResource("CustomFilters").ToString()))
                        return;

                    if (Template == null)
                    {
                        if (!string.IsNullOrEmpty(_FilterString))
                            FilterString = _FilterString;
                        else
                            FilterString = string.Empty;
                    }
                    else
                    {
                        FilterString = "[Quotation.Template.Name] In ('" + Template + "')";
                    }
                }
                GeosApplication.Instance.Logger.Log("Method ShowSelectedFilterGridAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ShowSelectedFilterGridAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for showing Grid's selected row Item detailed Window
        /// </summary>
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
                WorkOrderItemDetailsViewModel workOrderItemDetailsViewModel = new WorkOrderItemDetailsViewModel();
                WorkOrderItemDetailsView workOrderItemDetailsView = new WorkOrderItemDetailsView();

                EventHandler handle = delegate { workOrderItemDetailsView.Close(); };
                workOrderItemDetailsViewModel.RequestClose += handle;

                Ots ot = (Ots)detailView.Grid.SelectedItem;
                workOrderItemDetailsViewModel.OtSite = WarehouseCommon.Instance.Selectedwarehouse.Company;//[Sudhir.Jangra][GEOS2-5644]
                workOrderItemDetailsViewModel.Init(ot.IdOT, WarehouseCommon.Instance.Selectedwarehouse);
                workOrderItemDetailsView.DataContext = workOrderItemDetailsViewModel;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                var ownerInfo = (detailView as FrameworkElement);
                workOrderItemDetailsView.Owner = Window.GetWindow(ownerInfo);
                workOrderItemDetailsView.ShowDialogWindow();

                //if download quantity is zero then remove item from main grid. 
                if (workOrderItemDetailsViewModel.DownloadRemainingQuantity == 0)
                {
                    MainOtsList.Remove(MainOtsList.Where(oti => oti.IdOT == ot.IdOT).FirstOrDefault());
                    MainOtsList = new List<Ots>(MainOtsList);
                    FillPendingWorkOrderFilterList();
                }

                GeosApplication.Instance.Logger.Log("Method ShowSelectedGridRowItemWindowAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ShowSelectedGridRowItemWindowAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void RefreshPendingWorkOrderList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshPendingWorkOrderList()...", category: Category.Info, priority: Priority.Low);
                TableView detailView = (TableView)obj;
                GridControl gridControl = (detailView).Grid;
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

                // code for hide column chooser if empty
                TableView tableView = (TableView)gridControl.View;
                int visibleFalseCoulumn = 0;
                foreach (GridColumn column in gridControl.Columns)
                {
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

                FillMainOtList();
                FillPendingWorkOrderFilterList();
                AddCustomSetting();
                FilterString = string.Empty;
                detailView.SearchString = null;
                IsBusy = false;
                if (ListOfFilterTile.Count > 0)
                    SelectedTileBarItem = ListOfFilterTile.FirstOrDefault();
                InitChartControl();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method RefreshPendingWorkOrderList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshPendingWorkOrderList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void PrintPendingWorkOrderList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintPendingWorkOrderList()...", category: Category.Info, priority: Priority.Low);
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
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["PendingWorkOrderListReportPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["PendingWorkOrderListReportPrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);
                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                IsBusy = false;
                window.Show();
                DevExpress.XtraPrinting.PrintTool printTool = new DevExpress.XtraPrinting.PrintTool(pcl.PrintingSystem);
                printTool.PrinterSettings.PrinterName = GeosApplication.Instance.UserSettings["SelectedPrinter"];
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method PrintPendingWorkOrderList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintPendingWorkOrderList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ExportPendingWorkOrderList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportPendingWorkOrderList()...", category: Category.Info, priority: Priority.Low);

                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Pending Work Order List";
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
                    options.CustomizeDocumentColumn += Options_CustomizeDocumentColumn;
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

        /// <summary>
        /// Open Scan ot view
        /// </summary>
        /// <param name="obj"></param>
        private void ScanWorkOrder(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("WorkOrdersPreparationViewModel Method ScanWorkOrder...", category: Category.Info, priority: Priority.Low);

                TableView detailView = (TableView)obj;
                WorkOrderScanView workOrderScanView = new WorkOrderScanView();
                WorkOrderScanViewModel workOrderScanViewModel = new WorkOrderScanViewModel();
                workOrderScanViewModel.WindowHeader = Application.Current.FindResource("WorkOrderScanHeader").ToString();
                workOrderScanViewModel.Init(MainOtsList);
                workOrderScanViewModel.IsRefund = false;
                EventHandler handler = delegate { workOrderScanView.Close(); };
                workOrderScanViewModel.RequestClose += handler;
                workOrderScanView.DataContext = workOrderScanViewModel;
                var ownerInfo = (detailView as FrameworkElement);
                workOrderScanView.Owner = Window.GetWindow(ownerInfo);
                workOrderScanView.ShowDialogWindow();
                GeosApplication.Instance.Logger.Log("WorkOrdersPreparationViewModel Method ScanWorkOrder executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in WorkOrdersPreparationViewModel ScanWorkOrder() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void Options_CustomizeCell(CustomizeCellEventArgs e)
        {
            e.Formatting.Alignment = new XlCellAlignment()
            {
                WrapText = true
            };
            e.Handled = true;
        }

        private void Options_CustomizeDocumentColumn(CustomizeDocumentColumnEventArgs e)
        {
            if (e.ColumnFieldName == "Quotation.Offer.IsCritical")
                e.DocumentColumn.WidthInPixels = 90;
        }

        public void Dispose()
        {
        }

        private void RefundWorkOrderCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefundWorkOrderCommandAction()...", category: Category.Info, priority: Priority.Low);

                TableView detailView = (TableView)obj;
                WorkOrderScanView workOrderScanView = new WorkOrderScanView();
                WorkOrderScanViewModel workOrderScanViewModel = new WorkOrderScanViewModel();
                workOrderScanViewModel.WindowHeader = Application.Current.FindResource("RefundWorkOrderHeader").ToString();
                workOrderScanViewModel.Init(MainOtsList);
                workOrderScanViewModel.IsRefund = true;
                EventHandler handler = delegate { workOrderScanView.Close(); };
                workOrderScanViewModel.RequestClose += handler;
                workOrderScanView.DataContext = workOrderScanViewModel;
                var ownerInfo = (detailView as FrameworkElement);
                workOrderScanView.Owner = Window.GetWindow(ownerInfo);
                workOrderScanView.ShowDialogWindow();
                GeosApplication.Instance.Logger.Log("Method RefundWorkOrderCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in RefundWorkOrderCommandAction() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FilterEditorCreatedCommandAction(FilterEditorEventArgs obj)
        {
            obj.Handled = true;
            TableView table = (TableView)obj.OriginalSource;
            GridControl gridControl = (table).Grid;
            ShowFilterEditor(obj);
        }
        private void ShowFilterEditor(FilterEditorEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowFilterEditor()...", category: Category.Info, priority: Priority.Low);
                CustomFilterEditorView customFilterEditorView = new CustomFilterEditorView();
                CustomFilterEditorViewModel customFilterEditorViewModel = new CustomFilterEditorViewModel();
                string titleText = DevExpress.Xpf.Grid.GridControlLocalizer.Active.GetLocalizedString(GridControlStringId.FilterEditorTitle);
                if (IsEdit)
                {
                    customFilterEditorViewModel.FilterName = CustomFilterStringName;
                    customFilterEditorViewModel.IsSave = true;
                    customFilterEditorViewModel.IsNew = false;
                    IsEdit = false;
                }
                else
                    customFilterEditorViewModel.IsNew = true;

                customFilterEditorViewModel.Init(e.FilterControl, ListOfFilterTile);
                customFilterEditorView.DataContext = customFilterEditorViewModel;
                EventHandler handle = delegate { customFilterEditorView.Close(); };
                customFilterEditorViewModel.RequestClose += handle;
                customFilterEditorView.Title = titleText;
                customFilterEditorView.Icon = DevExpress.Xpf.Core.Native.ImageHelper.CreateImageFromCoreEmbeddedResource("Editors.Images.FilterControl.filter.png");
                customFilterEditorView.Grid.Children.Add(e.FilterControl);
                customFilterEditorView.ShowDialog();

                if (customFilterEditorViewModel.IsDelete && !string.IsNullOrEmpty(customFilterEditorViewModel.FilterName))
                {
                    TileBarFilters tileBarItem = ListOfFilterTile.FirstOrDefault(x => x.Caption.Equals(CustomFilterStringName));
                    if (tileBarItem != null)
                    {
                        ListOfFilterTile.Remove(tileBarItem);
                        List<Tuple<string, string>> lstUserConfiguration = new List<Tuple<string, string>>();
                        foreach (KeyValuePair<string, string> setting in GeosApplication.Instance.UserSettings)
                        {
                            string key = setting.Key;

                            if (setting.Key.Contains(userSettingsKey))
                                key = setting.Key.Replace(userSettingsKey, "");

                            if (!key.Equals(tileBarItem.Caption))
                                lstUserConfiguration.Add(new Tuple<string, string>(setting.Key.ToString(), setting.Value.ToString()));
                        }
                        ApplicationOperation.CreateNewSetting(lstUserConfiguration, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                        GeosApplication.Instance.UserSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.UserSettingFilePath);
                    }
                }
                else if (!string.IsNullOrEmpty(customFilterEditorViewModel.FilterName) && customFilterEditorViewModel.IsSave && !customFilterEditorViewModel.IsNew && customFilterEditorViewModel.IsCancel)
                {
                    TileBarFilters tileBarItem = ListOfFilterTile.FirstOrDefault(x => x.Caption.Equals(CustomFilterStringName));
                    if (tileBarItem != null)
                    {
                        CustomFilterStringName = customFilterEditorViewModel.FilterName;
                        string filterCaption = tileBarItem.Caption;
                        tileBarItem.Caption = customFilterEditorViewModel.FilterName;
                        tileBarItem.EntitiesCount = VisibleRowCount;
                        tileBarItem.EntitiesCountVisibility = Visibility.Visible;
                        tileBarItem.FilterCriteria = customFilterEditorViewModel.FilterCriteria;
                        List<Tuple<string, string>> lstUserConfiguration = new List<Tuple<string, string>>();
                        foreach (KeyValuePair<string, string> setting in GeosApplication.Instance.UserSettings)
                        {
                            string key = setting.Key;

                            if (setting.Key.Contains(userSettingsKey))
                                key = setting.Key.Replace(userSettingsKey, "");

                            if (!key.Equals(filterCaption))
                                lstUserConfiguration.Add(new Tuple<string, string>(setting.Key.ToString(), setting.Value.ToString()));
                            else
                                lstUserConfiguration.Add(new Tuple<string, string>((userSettingsKey + tileBarItem.Caption), tileBarItem.FilterCriteria));
                        }
                        ApplicationOperation.CreateNewSetting(lstUserConfiguration, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                        GeosApplication.Instance.UserSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.UserSettingFilePath);
                    }
                }
                else if (customFilterEditorViewModel.FilterName != null && !string.IsNullOrEmpty(customFilterEditorViewModel.FilterName) && customFilterEditorViewModel.IsSave && customFilterEditorViewModel.IsNew && customFilterEditorViewModel.IsCancel)
                {
                    ListOfFilterTile.Add(new TileBarFilters()
                    {
                        Caption = customFilterEditorViewModel.FilterName,
                        Id = 0,
                        BackColor = null,
                        ForeColor = null,
                        EntitiesCountVisibility = Visibility.Visible,
                        FilterCriteria = customFilterEditorViewModel.FilterCriteria,
                        Height = 80,
                        width = 200,
                        EntitiesCount = VisibleRowCount
                    });

                    string filterName = userSettingsKey + customFilterEditorViewModel.FilterName;
                    GeosApplication.Instance.UserSettings[filterName] = customFilterEditorViewModel.FilterCriteria;
                    ApplicationOperation.CreateNewSetting(GeosApplication.Instance.UserSettings, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                    GeosApplication.Instance.UserSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.UserSettingFilePath);
                    SelectedTileBarItem = ListOfFilterTile.LastOrDefault();
                }

                GeosApplication.Instance.Logger.Log("Method ShowFilterEditor() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ShowFilterEditor() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void AddCustomSetting()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddCustomSetting()...", category: Category.Info, priority: Priority.Low);
                List<KeyValuePair<string, string>> tempUserSettings = GeosApplication.Instance.UserSettings.Where(x => x.Key.Contains(userSettingsKey)).ToList();
                if (tempUserSettings != null)
                {
                    foreach (var item in tempUserSettings)
                    {
                        ExpressionEvaluator evaluator = new ExpressionEvaluator(TypeDescriptor.GetProperties(typeof(Ots)), item.Value, false);
                        List<Ots> tempList = new List<Ots>();
                        foreach (var ot in MainOtsList)
                        {
                            if (evaluator.Fit(ot))
                                tempList.Add(ot);
                        }
                        FilterString = item.Value;
                        ListOfFilterTile.Add(
                        new TileBarFilters()
                        {
                            Caption = item.Key.Replace(userSettingsKey, ""),
                            Id = 0,
                            BackColor = null,
                            ForeColor = null,
                            FilterCriteria = item.Value,
                            EntitiesCount = tempList.Count,
                            EntitiesCountVisibility = Visibility.Visible,
                            Height = 80,
                            width = 200
                        });
                    }
                }

                GeosApplication.Instance.Logger.Log("Method AddCustomSetting() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddCustomSetting() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CommandTileBarDoubleClickAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CommandTileBarDoubleClickAction()...", category: Category.Info, priority: Priority.Low);
                if (ListOfTemplate.Any(x => x.Name == CustomFilterStringName) || CustomFilterStringName.Equals(System.Windows.Application.Current.FindResource("CustomFilters").ToString()) || CustomFilterStringName.Equals(System.Windows.Application.Current.FindResource("AllWorkOrder").ToString()))
                    return;
                TableView table = (TableView)obj;
                GridControl gridControl = (table).Grid;
                List<GridColumn> GridColumnList = gridControl.Columns.Where(x => x.FieldName != null).ToList();
                string columnName = FilterString.Substring(FilterString.IndexOf("[") + 1, FilterString.IndexOf("]") - 2 - FilterString.IndexOf("[") + 1);
                GridColumn column = GridColumnList.FirstOrDefault(x => x.FieldName.ToString().Equals(columnName));
                IsEdit = true;
                table.ShowFilterEditor(column);

                GeosApplication.Instance.Logger.Log("Method CommandTileBarDoubleClickAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CommandTileBarDoubleClickAction() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// [001][skhade][2019-10-17][GEOS2-1548] Add a new column "#Out of Stock" in picking -> Work orders
        /// </summary>
        /// <param name="e"></param>
        private void CustomCellAppearanceAction(CustomRowAppearanceEventArgs e)
        {
            if (((CustomCellAppearanceEventArgs)e).Column != null && ((CustomCellAppearanceEventArgs)e).Column.Name == "OutOfStock")
            {
                e.Result = e.ConditionalValue;
                e.Handled = true;
            }
        }

        /// <summary>
        /// [001][psutar][2019-06-20][GEOS2-65] Allow to save grid configuration in work order section
        /// Method for saving grid layoutInvokeDelegateCommand
        /// [002][skhade][2019-10-17][GEOS2-1548] Add a new column "#Out of Stock" in picking -> Work orders
        /// </summary>
        /// <param name="obj"></param>
        private void TableViewLoadedCommandAction(RoutedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CustomCellAppearanceGridControl ...", category: Category.Info, priority: Priority.Low);

                int visibleFalseCoulumn = 0;

                //if (File.Exists(WorkOrderGridSettingFilePath))
                //{
                //    ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.RestoreLayoutFromXml(WorkOrderGridSettingFilePath);
                //    GridControl GridControlEmpolyeeDetails = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;
                //    TableView EmployeeProfileTableView = (TableView)GridControlEmpolyeeDetails.View;

                //    if (EmployeeProfileTableView.SearchString != null)
                //    {
                //        EmployeeProfileTableView.SearchString = null;
                //    }
                //}

                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.AddHandler(DXSerializer.AllowPropertyEvent, new AllowPropertyEventHandler(OnAllowProperty));

                //This code for save grid layout.
                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.SaveLayoutToXml(WorkOrderGridSettingFilePath);

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

                TableView datailView = ((TableView)((System.Windows.RoutedEventArgs)obj).OriginalSource);

                //002 - This code is added because format conditions are deleted when restore from old layout.
                if (datailView.FormatConditions == null || datailView.FormatConditions.Count == 0)
                {
                    var profitFormatCondition = new FormatCondition()
                    {
                        Expression = "[OutOfStockItemCount] = [OtItemCount]",
                        FieldName = "OutOfStockItemCount",
                        Format = new DevExpress.Xpf.Core.ConditionalFormatting.Format()
                        {
                            Background = Brushes.Red
                        }
                    };
                    datailView.FormatConditions.Add(profitFormatCondition);
                }

                datailView.BestFitColumns();

                GeosApplication.Instance.Logger.Log("Method CustomCellAppearanceGridControl() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CustomCellAppearanceGridControl() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for remove filter save on grid layout.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAllowProperty(object sender, AllowPropertyEventArgs e)
        {
            if (e.DependencyProperty == GridControl.FilterStringProperty)
                e.Allow = false;

            if (e.Property.Name == "GroupCount")
                e.Allow = false;

            if (e.DependencyProperty == TableViewEx.SearchStringProperty)
                e.Allow = false;

            if (e.Property.Name == "FormatConditions")
                e.Allow = false;
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
                    ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(WorkOrderGridSettingFilePath);
                }

                if (column.Visible == false)
                {
                    isWorkOrderColumnChooserVisible = true;
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
                ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(WorkOrderGridSettingFilePath);

                GeosApplication.Instance.Logger.Log("Method VisibleIndexChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in VisibleIndexChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// [000] [avpawar] [26-06-2019] [GEOS2-1630] [Put OT colors in work orders grid in picking]
        /// </summary>
        private void FillListofColor()
        {
            GeosAppSettingList = WorkbenchService.GetSelectedGeosAppSettings("14,15,16,17");
        }
        
        private void ChatControlLoadedEvent(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChatControlLoadedEvent ...", category: Category.Info, priority: Priority.Low);

                chartcontrol = (ChartControl)obj;
                InitChartControl();

                GeosApplication.Instance.Logger.Log("Method ChartDashboardSalebyCustomerAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ChartDashboardSalebyCustomerAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void InitChartControl()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method InitChartControl ...", category: Category.Info, priority: Priority.Low);
                if (chartcontrol != null)
                {
                    chartcontrol.BeginInit();
                    chartcontrol.HorizontalAlignment = HorizontalAlignment.Stretch;
                    chartcontrol.VerticalAlignment = VerticalAlignment.Stretch;
                    chartcontrol.Legend = new Legend();
                    chartcontrol.Legend.HorizontalPosition = HorizontalPosition.RightOutside;
                    //chartcontrol.CrosshairOptions.CrosshairLabelMode = CrosshairLabelMode.ShowCommonForAllSeries;
                    chartcontrol.CrosshairOptions = new CrosshairOptions();
                    chartcontrol.CrosshairOptions.GroupHeaderPattern = "Week Start: {A: dddd, dd/MM/yyyy}";
                    //chartcontrol.CrosshairOptions.ShowArgumentLabels = true;
                    chartcontrol.CrosshairOptions.ShowArgumentLine = true;
                    chartcontrol.CrosshairOptions.ShowCrosshairLabels = true;
                    chartcontrol.CrosshairOptions.ShowGroupHeaders = true;
                    //chartcontrol.CrosshairOptions.ShowValueLabels = true;
                    chartcontrol.CrosshairOptions.ShowValueLine = true;
                    var diagram = new XYDiagram2D();
                    diagram.ActualAxisX.Title = new AxisTitle { Content = "Work Order Expected Delivery Week" };
                    diagram.ActualAxisX.Title.Alignment = TitleAlignment.Center;
                    diagram.ActualAxisY.Title = new AxisTitle { Content = "Total Time (h m)" };//[rdixit][GEOS2-4269][28.06.2023]
                    diagram.ActualAxisY.GridLinesMinorVisible = true;
                    diagram.ActualAxisX.LabelVisibilityMode = AxisLabelVisibilityMode.AutoGeneratedAndCustom;
                    diagram.ActualAxisX.LabelPosition = AxisLabelPosition.Outside;
                    diagram.ActualAxisX.LabelAlignment = AxisLabelAlignment.Auto;
                    diagram.ActualAxisX.Interlaced = true;
                    diagram.ActualAxisX.Label = new AxisLabel();
                    diagram.ActualAxisX.Label.Formatter = new XAxisLabelFormatter();
                    diagram.ActualAxisY.Label = new AxisLabel();
                    diagram.ActualAxisY.Label.Formatter = new YAxisLabelFormatter();
                    diagram.EqualBarWidth = true;
                    //diagram.ShowCrosshair(); // = true;
                    //diagram.ShouldSerializeCrosshairSeriesLabelItems();// = true;
                    //diagram.ShouldSerializeCrosshairAxisLabelItems = true;

                    diagram.ActualAxisX.DateTimeScaleOptions = new ManualDateTimeScaleOptions()
                    {
                        MeasureUnit = DateTimeMeasureUnit.Week,
                        GridAlignment = DateTimeGridAlignment.Week,
                        AutoGrid = false
                    };

                    diagram.ActualAxisY.DateTimeScaleOptions = new ContinuousDateTimeScaleOptions()
                    {
                        AutoGrid = false,
                        GridAlignment = DateTimeGridAlignment.Hour
                    };

                    chartcontrol.Diagram = diagram;
                    #region Commented Code
                    //BarSideBySideSeries2D barSideBySide2D = new BarSideBySideSeries2D();
                    //barSideBySide2D.ArgumentScaleType = ScaleType.DateTime;
                    //barSideBySide2D.ValueScaleType = ScaleType.TimeSpan;
                    //barSideBySide2D.DisplayName = "Total Picking Time";
                    //barSideBySide2D.Brush = (new SolidColorBrush(Colors.Red));// (new BrushConverter().ConvertFrom(Colors.Red));
                    //barSideBySide2D.BarWidth = 0.8;

                    //barSideBySide2D.AggregateFunction = SeriesAggregateFunction.Sum;
                    ////  barSideBySide2D.ColorEach = true;
                    //barSideBySide2D.CrosshairEnabled = true;
                    ////barSideBySide2D.Label = new SeriesLabel() { TextPattern = item.MergeCode,
                    ////TextOrientation = TextOrientation.BottomToTop};
                    ////barSideBySide2D.Label.IsVisible = true;

                    //barSideBySide2D.ArgumentDataMember = "DeliveryDate";
                    ////if (item.TotalTimeForPickingOT != null)
                    ////{
                    //barSideBySide2D.ValueDataMember = "TotalTimeForPickingOT";
                    ////}
                    ////else
                    ////{
                    ////    barSideBySide2D.ValueDataMember = "TotalTimeForPickingOT";
                    ////}
                    //barSideBySide2D.ShowInLegend = true;
                    //barSideBySide2D.Model = new DevExpress.Xpf.Charts.SimpleBar2DModel();
                    //barSideBySide2D.FilterString = FilterString;
                    ////barSideBySide2D.AnimationAutoStartMode = AnimationAutoStartMode.SetStartState;
                    ////Bar2DSlideFromTopAnimation seriesAnimation = new Bar2DSlideFromTopAnimation();
                    ////seriesAnimation.Duration = new TimeSpan(0, 0, 2);
                    ////barSideBySide2D.PointAnimation = seriesAnimation;

                    //chartcontrol.Diagram.Series.Add(barSideBySide2D);

                    /////

                    //BarSideBySideSeries2D barSideBySide2D2 = new BarSideBySideSeries2D();
                    //barSideBySide2D2.ArgumentScaleType = ScaleType.DateTime;
                    //barSideBySide2D2.ValueScaleType = ScaleType.TimeSpan;
                    //barSideBySide2D2.DisplayName = "Total Packing Time";
                    //barSideBySide2D2.Brush = (new SolidColorBrush(Colors.Green));// (new BrushConverter().ConvertFrom(Colors.Red));
                    //barSideBySide2D2.BarWidth = 0.8;
                    //barSideBySide2D.AggregateFunction = SeriesAggregateFunction.Sum;
                    ////  barSideBySide2D.ColorEach = true;
                    //barSideBySide2D2.CrosshairEnabled = true;
                    //// barSideBySide2D.Label = new SeriesLabel() { TextPattern = item.MergeCode };


                    //barSideBySide2D2.ArgumentDataMember = "DeliveryDate";
                    ////if (item.TotalTimeForShipmentCreatedIn != null)
                    ////{
                    //barSideBySide2D2.ValueDataMember = "TotalTimeForShipmentCreatedIn";
                    ////}
                    ////else
                    ////{
                    ////    barSideBySide2D2.ValueDataMember = "TotalTimeForShipmentCreatedIn";
                    ////}
                    //barSideBySide2D2.ShowInLegend = true;
                    //barSideBySide2D2.Model = new DevExpress.Xpf.Charts.SimpleBar2DModel();
                    //barSideBySide2D2.FilterString = FilterString;
                    ////barSideBySide2D.AnimationAutoStartMode = AnimationAutoStartMode.SetStartState;
                    ////Bar2DSlideFromTopAnimation seriesAnimation = new Bar2DSlideFromTopAnimation();
                    ////seriesAnimation.Duration = new TimeSpan(0, 0, 2);
                    ////barSideBySide2D.PointAnimation = seriesAnimation;

                    //chartcontrol.Diagram.Series.Add(barSideBySide2D2);
                    #endregion
                    AddSeriesInDiagram(
                        displayName: "Total Picking Time",
                        brush: (new SolidColorBrush(Colors.Red)),
                        valueDataMember: "TotalTimeForPickingOT"
                        );

                    AddSeriesInDiagram(
                        displayName: "Total Packing Time",
                        brush: (new SolidColorBrush(Colors.Green)),
                        valueDataMember: "TotalTimeForShipmentCreatedIn"
                        );

                    AddSeriesInDiagram(
                        displayName: "Total Shipping Time",
                        brush: (new SolidColorBrush(Colors.Blue)),
                        valueDataMember: "TotalTimeForShipmentDelivery"
                        );

                    chartcontrol.EndInit();
                }
                GeosApplication.Instance.Logger.Log("Method InitChartControl() executed successfully", category: Category.Info, priority: Priority.Low);
                
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in InitChartControl() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        private void AddSeriesInDiagram(string displayName,
            SolidColorBrush brush, string valueDataMember)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddSeriesInDiagram...", category: Category.Info, priority: Priority.Low);

                var barSideBySide2D = new BarSideBySideSeries2D
                {
                    ArgumentScaleType = ScaleType.DateTime,
                    ValueScaleType = ScaleType.TimeSpan,
                    DisplayName = displayName,// "Total Shipping Time",
                    Brush = brush, //(new SolidColorBrush(Colors.Blue)),
                    BarWidth = 0.8,
                    AggregateFunction = SeriesAggregateFunction.Sum,
                    CrosshairEnabled = true,
                    ArgumentDataMember = "DeliveryDate",
                    ValueDataMember = valueDataMember,//"TotalTimeForShipmentDelivery",
                    Model = new DevExpress.Xpf.Charts.SimpleBar2DModel(),
                    FilterString = FilterString,
                    CrosshairContentShowMode = CrosshairContentShowMode.Label,
                    CrosshairLabelPattern = @"{S}: {V: d\d\ h\h\ m\m}",
                    CrosshairLabelVisibility = true
                };
                chartcontrol.Diagram.Series.Add(barSideBySide2D);

                GeosApplication.Instance.Logger.Log("Method AddSeriesInDiagram() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddSeriesInDiagram() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        ///// <summary>
        ///// This method is used to display data on hover of Bar. (order of CrosshairElements is reversed)
        ///// </summary>
        ///// <param name="obj"></param>
        //private void ChartDashboardSaleCustomDrawCrosshairCommandAction(object obj)
        //{
        //    try
        //    {
        //        CustomDrawCrosshairEventArgs e = (CustomDrawCrosshairEventArgs)obj;
        //        foreach (var group in e.CrosshairElementGroups)
        //        {
        //            var reverseList = group.CrosshairElements.ToList();
        //            group.CrosshairElements.Clear();
        //            foreach (var item in reverseList)
        //            {
        //                if (item.Series.DisplayName == "TARGET")
        //                    group.CrosshairElements.Add(item);
        //                else
        //                    group.CrosshairElements.Insert(0, item);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in ChartDashboardSaleCustomDrawCrosshairCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}

        private void CustomShowFilterPopupAction(DevExpress.Xpf.Grid.FilterPopupEventArgs e)//[rdixit][GEOS2-3150][05.08.2022]
        {
            GeosApplication.Instance.Logger.Log(" Method CustomShowFilterPopupAction ...", category: Category.Info, priority: Priority.Low);
            if (e.Column.FieldName != "OperatorUserName" )
            {
                return;
            }
            try {
                List<object> filterItems = new List<object>();
                if (e.Column.FieldName != "OperatorUserName")
                {
                    return;
                }
                    #region OperatorUserName
                    if (e.Column.FieldName == "OperatorUserName")
                {
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("OperatorUserName = ''")
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("OperatorUserName <> ''")
                    });

                    foreach (var dataObject in MainOtsList)
                    {
                        if (dataObject.OperatorUserName == null)
                        {
                            continue;
                        }
                        else if (dataObject.OperatorUserName != null)
                        {
                            if (dataObject.OperatorUserName.Contains("\n"))
                            {
                                string tempOperatorUserName = dataObject.OperatorUserName;
                                for (int index = 0; index < tempOperatorUserName.Length; index++)
                                {
                                    string empJobCodes = tempOperatorUserName.Split('\n').First();

                                    if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == empJobCodes))
                                    {
                                        CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                        customComboBoxItem.DisplayValue = empJobCodes;
                                        customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("OperatorUserName Like '%{0}%'", empJobCodes));
                                        filterItems.Add(customComboBoxItem);
                                    }
                                    if (tempOperatorUserName.Contains("\n"))
                                        tempOperatorUserName = tempOperatorUserName.Remove(0, empJobCodes.Length + 1);
                                    else
                                        break;
                                }
                            }
                            else
                            {
                                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == MainOtsList.Where(y => y.OperatorUserName == dataObject.OperatorUserName).Select(slt => slt.OperatorUserName).FirstOrDefault().Trim()))
                                {
                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                    customComboBoxItem.DisplayValue = MainOtsList.Where(y => y.OperatorUserName == dataObject.OperatorUserName).Select(slt => slt.OperatorUserName).FirstOrDefault().Trim();
                                    customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("OperatorUserName Like '%{0}%'", MainOtsList.Where(y => y.OperatorUserName == dataObject.OperatorUserName).Select(slt => slt.OperatorUserName).FirstOrDefault().Trim()));
                                    filterItems.Add(customComboBoxItem);
                                }
                            }
                        }
                    }
                }
                #endregion
                e.ComboBoxEdit.ItemsSource = filterItems.OrderBy(sort => Convert.ToString(((CustomComboBoxItem)sort).DisplayValue)).ToList();
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CustomShowFilterPopupAction() method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

            #endregion //End Of Methods
        }
    public class XAxisLabelFormatter : IAxisLabelFormatter
    {
        public string GetAxisLabelText(object axisValue)
        {
            var date = (DateTime)axisValue;
            var c = Thread.CurrentThread.CurrentCulture;
            return $"{date.Year}{c.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday).ToString("00")}";
        }
    }
    public class YAxisLabelFormatter : IAxisLabelFormatter
    {
        public string GetAxisLabelText(object axisValue)
        {
            if (axisValue != null)
            {
                var totalTimeValue = (TimeSpan)axisValue;
                var c = Thread.CurrentThread.CurrentCulture;
                //[rdixit][GEOS2-4269][28.06.2023]
                if (totalTimeValue.Days > 0)
                    return ((totalTimeValue.Hours + (totalTimeValue.Days * 24)) + "H" + " " + totalTimeValue.Minutes + "M");
                else
                    return (totalTimeValue.Hours + "H" + " " + totalTimeValue.Minutes + "M");              
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
