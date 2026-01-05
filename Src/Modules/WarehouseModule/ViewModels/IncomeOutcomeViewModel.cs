using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Charts;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors.Flyout;
using DevExpress.Xpf.WindowsUI;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.Warehouse.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    public class IncomeOutcomeViewModel : NavigationViewModelBase, IDisposable, INotifyPropertyChanged
    {

        #region Service
        IWarehouseService WarehouseService = new WarehouseServiceController(((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString()));
        // IWarehouseService WarehouseService = new WarehouseServiceController("localhost:6699");
        #endregion

        #region Public Events

        public event EventHandler RequestClose;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
        #endregion //Events

        #region Declaration
        private string fromDate;
        private string toDate;
        private int isButtonStatus;
        private Visibility isCalenderVisible;
        private Duration _currentDuration;
        private DateTime startDate;
        private DateTime endDate;
        private ObservableCollection<ArticlesStock> incomeOutcomeData;
        ChartControl chartcontrol;
        private string filterString;
        private ChartControl chartControl;
        private XYDiagram2D diagram;
        const string shortDateFormat = "dd/MM/yyyy";
        private Currency selectedCurrency;
        ObservableCollection<string> incomeData;
        ObservableCollection<string> outcomeData;

        #endregion

        #region Properties
        public ObservableCollection<string> IncomeData
        {
            get { return incomeData; }
            set
            {
                incomeData = value;
                OnPropertyChanged(new PropertyChangedEventArgs("incomeData"));
            }
        }
        public ObservableCollection<string> OutcomeData
        {
            get { return outcomeData; }
            set
            {
                outcomeData = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OutcomeData"));
            }
        }
        public string FromDate
        {
            get { return fromDate; }
            set
            {
                fromDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FromDate"));
            }
        }

        public string ToDate
        {
            get { return toDate; }
            set
            {
                toDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ToDate"));
            }
        }

        public int IsButtonStatus
        {
            get { return isButtonStatus; }
            set
            {
                isButtonStatus = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsButtonStatus"));
            }
        }

        public Visibility IsCalendarVisible
        {
            get { return isCalenderVisible; }
            set
            {
                isCalenderVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCalendarVisible"));
            }
        }

        public DateTime StartDate
        {
            get { return startDate; }
            set
            {
                startDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StartDate"));
            }
        }

        public DateTime EndDate
        {
            get { return endDate; }
            set
            {
                endDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EndDate"));
            }
        }

        public ObservableCollection<ArticlesStock> IncomeOutcomeData
        {
            get { return incomeOutcomeData; }
            set
            {
                incomeOutcomeData = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IncomeOutcomeData"));
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

        public Currency SelectedCurrency
        {
            get
            {
                return selectedCurrency;
            }

            set
            {
                selectedCurrency = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCurrency"));
            }
        }


        #endregion

        #region ICommands
        public ICommand ChangePlantCommand { get; set; }

        public ICommand PeriodCommand { get; set; }

        public ICommand PeriodCustomRangeCommand { get; set; }

        public ICommand ApplyCommand { get; set; }

        public ICommand CancelCommand { get; set; }
        public ICommand ChartItemsForecastLoadCommand { get; set; }

        public ICommand RefreshIncomeOutcomeCommand { get; set; }
        #endregion

        #region Constructor
        public IncomeOutcomeViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor IncomeOutcomeViewModel() ...", category: Category.Info, priority: Priority.Low);
                Processing();
                ChangePlantCommand = new RelayCommand(new Action<object>(ChangePlantCommandAction));
                PeriodCommand = new RelayCommand(new Action<object>(PeriodCommandAction));
                PeriodCustomRangeCommand = new RelayCommand(new Action<object>(PeriodCustomRangeCommandAction));
                ApplyCommand = new RelayCommand(new Action<object>(ApplyCommandAction));
                CancelCommand = new RelayCommand(new Action<object>(CancelCommandAction));
                ChartItemsForecastLoadCommand = new RelayCommand(new Action<object>(ChatControlLoadedEvent));
                RefreshIncomeOutcomeCommand = new RelayCommand(new Action<object>(RefreshIncomeOutcomeCommandAction));
                setDefaultPeriod();
                StartDate = DateTime.Today.Date;
                EndDate = DateTime.Today.Date;
                IsCalendarVisible = Visibility.Collapsed;

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method IncomeOutcomeViewModel....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Constructor IncomeOutcomeViewModel()." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Methods

        public void Init()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
                Processing();
                List<Currency> CurrencyList = GeosApplication.Instance.Currencies.ToList();
                SelectedCurrency = CurrencyList.FirstOrDefault(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency_Warehouse"]);
                GeosApplication.Instance.WMSCurrentCurrencySymbol = CurrencyList.FirstOrDefault(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency_Warehouse"]).Symbol;
                GeosApplication.SetCurrencySymbol_warehouse(GeosApplication.Instance.WMSCurrentCurrencySymbol);

                GlobalProperties.FromDate = FromDate;
                GlobalProperties.ToDate = ToDate;
                DateTime fromDate;
                DateTime toDate;
                if (DateTime.TryParse(FromDate, CultureInfo.CreateSpecificCulture("en-GB"), DateTimeStyles.None, out fromDate))
                {
                    fromDate.ToString("dd/MM/yyyy");
                }
                if (DateTime.TryParse(ToDate, CultureInfo.CreateSpecificCulture("en-GB"), DateTimeStyles.None, out toDate))
                {
                    toDate.ToString("dd/MM/yyyy");
                }


                //var temp = new ObservableCollection<ArticlesStock>(WarehouseService.GetFinancialIncomeOutcome_V2450(WarehouseCommon.Instance.Selectedwarehouse,
                //    SelectedCurrency.IdCurrency, fromDate,
                //  toDate));

                //[Sudhir.Jangra][GEOS2-4859]
                var temp = new ObservableCollection<ArticlesStock>(WarehouseService.GetFinancialIncomeOutcome_V2460(WarehouseCommon.Instance.Selectedwarehouse,
                    SelectedCurrency.IdCurrency, fromDate,
                  toDate));


                IncomeOutcomeData = new ObservableCollection<ArticlesStock>(temp.Where(x => x.UploadedIn >= fromDate && x.UploadedIn <= toDate));


                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void RefreshIncomeOutcomeCommandAction(object obj)
        {
            Processing();
            setDefaultPeriod();
            Init();
            InitChartControl();

            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        }
        private void ChangePlantCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method ChangePlantCommandAction...", category: Category.Info, priority: Priority.Low);
            if (!WarehouseCommon.Instance.IsWarehouseChangedEventCanOccur)
                return;

            Init();
            InitChartControl();
            GeosApplication.Instance.Logger.Log("Method ChangePlantCommandAction executed successfully.", category: Category.Info, priority: Priority.Low);
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

                Init();
                InitChartControl();
                chartControl.Diagram = null;



                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method FlyoutControl_Closed....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FlyoutControl_Closed() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FlyoutControl_Closed() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FlyoutControl_Closed() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        private Action Processing()
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
            return null;
        }

        private void setDefaultPeriod()
        {
            int year = DateTime.Now.Year;
            DateTime StartFromDate = new DateTime(year, 1, 1);
            DateTime EndToDate = new DateTime(year, 12, 31);
            FromDate = StartFromDate.ToString("dd/MM/yyyy");
            ToDate = EndToDate.ToString("dd/MM/yyyy");
        }

        private void CancelCommandAction(object obj)
        {
            MenuFlyout menu = (MenuFlyout)obj;
            menu.IsOpen = false;
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
                    chartcontrol.Legend = new DevExpress.Xpf.Charts.Legend();
                    chartcontrol.Legend.HorizontalPosition = HorizontalPosition.RightOutside;
                    chartcontrol.Legend.Visible = true;
                    chartcontrol.CrosshairOptions = new CrosshairOptions();
                    chartcontrol.CrosshairOptions.CrosshairLabelMode = CrosshairLabelMode.ShowCommonForAllSeries;
                    chartcontrol.CrosshairOptions.ShowArgumentLabels = true;
                    chartcontrol.CrosshairOptions.ShowArgumentLine = true;
                    chartcontrol.CrosshairOptions.ShowCrosshairLabels = true;
                    chartcontrol.CrosshairOptions.ShowGroupHeaders = true;
                    chartcontrol.CrosshairOptions.ShowValueLabels = true;
                    chartcontrol.CrosshairOptions.ShowValueLine = true;
                    diagram = new XYDiagram2D();
                    diagram.ActualAxisX.Title = new AxisTitle { Content = "Time Period" };
                    diagram.ActualAxisX.Title.Alignment = TitleAlignment.Center;
                    diagram.ActualAxisY.Title = new AxisTitle { Content = "Sum of Amount" + "(" + GeosApplication.Instance.WMSCurrentCurrencySymbol + ")" };

                    diagram.ActualAxisY.NumericOptions = new NumericOptions();
                    diagram.ActualAxisY.NumericOptions.Format = NumericFormat.Currency;
                    diagram.ActualAxisY.GridLinesMinorVisible = true;
                    diagram.ActualAxisX.LabelVisibilityMode = AxisLabelVisibilityMode.AutoGeneratedAndCustom;
                    diagram.ActualAxisX.LabelPosition = AxisLabelPosition.Outside;
                    diagram.ActualAxisX.LabelAlignment = AxisLabelAlignment.Auto;
                    diagram.ActualAxisX.Interlaced = true;
                    diagram.ActualAxisY.CrosshairAxisLabelOptions = new CrosshairAxisLabelOptions();
                    //  diagram.ActualAxisY.CrosshairAxisLabelOptions.Pattern = "{V:F4}";
                        diagram.ActualAxisY.CrosshairAxisLabelOptions.Pattern = "{S} : {V:c2}";  //[GEOS2-4859][Rupali Sarode][12-12-2023]

                    diagram.ActualAxisX.Label = new AxisLabel();
                    diagram.ActualAxisX.Label.Formatter = new XAxisLabelFormatterForStockHistoryGroupByWeek();
                    diagram.ActualAxisX.DateTimeScaleOptions = new ManualDateTimeScaleOptions()
                    {
                        MeasureUnit = DateTimeMeasureUnit.Week,
                        GridAlignment = DateTimeGridAlignment.Week,
                        AutoGrid = false
                    };
                    diagram.ActualAxisY.Label = new AxisLabel();
                    diagram.ActualAxisY.Label.Formatter = new YAxisLabelFormatterForStockHistory();
                    diagram.EqualBarWidth = true;
                    diagram.ActualAxisY.DateTimeScaleOptions = new ContinuousDateTimeScaleOptions()
                    {
                        AutoGrid = false
                    };
                    chartcontrol.Diagram = diagram;
                    AddSeriesInDiagram(
                            displayName: "Sum Of Income",
                            brush: (new SolidColorBrush(Colors.Green)),
                            valueDataMember: "QuantityConversionIncome"
                            );
                    AddSeriesInDiagram(
                   displayName: "Sum Of Outcome",
                   brush: (new SolidColorBrush(Colors.Red)),
                   valueDataMember: "QuantityConversionOutcome"
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
                    ValueScaleType = ScaleType.Numerical,
                    DisplayName = displayName,
                    Brush = brush,
                    ShowInLegend = true,
                    BarWidth = 0.8,
                    AggregateFunction = SeriesAggregateFunction.Sum,
                    CrosshairEnabled = true,
                    ArgumentDataMember = "UploadedIn",
                    ValueDataMember = valueDataMember,
                    Model = new DevExpress.Xpf.Charts.SimpleBar2DModel(),
                    FilterString = FilterString,
                    CrosshairContentShowMode = CrosshairContentShowMode.Default,
                    // CrosshairLabelPattern = "{S}: {V:f4}",
                     CrosshairLabelPattern = "{S} : {V:c2}", //[GEOS2-4859][Rupali Sarode][12-12-2023]

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

        #endregion

        public static class GlobalProperties
        {
            public static string FromDate { get; set; }
            public static string ToDate { get; set; }
        }

        public class XAxisLabelFormatterForStockHistoryGroupByWeek : IAxisLabelFormatter
        {
            public string GetAxisLabelText(object axisValue)
            {
                if (axisValue != null)
                {
                    try
                    {
                        DateTime fromDate;
                        DateTime toDate;
                        if (DateTime.TryParse(GlobalProperties.FromDate, CultureInfo.CreateSpecificCulture("en-GB"), DateTimeStyles.None, out fromDate))
                        {
                            fromDate.ToString("dd/MM/yyyy");
                        }
                        if (DateTime.TryParse(GlobalProperties.ToDate, CultureInfo.CreateSpecificCulture("en-GB"), DateTimeStyles.None, out toDate))
                        {
                            toDate.ToString("dd/MM/yyyy");
                        }
                        var date = (DateTime)axisValue;
                        if (date >= fromDate && date <= toDate)
                        {
                            var c = Thread.CurrentThread.CurrentCulture;
                            CultureInfo myCI = CultureInfo.CurrentCulture;
                            System.Globalization.Calendar myCal = myCI.Calendar;
                            CalendarWeekRule myCWR = myCI.DateTimeFormat.CalendarWeekRule;
                            DayOfWeek myFirstDOW = myCI.DateTimeFormat.FirstDayOfWeek;
                            int Number_of_Weeks = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(date, myCWR, myFirstDOW);
                            string test = $"{date.Year}{Number_of_Weeks}";
                            return test;
                        }
                        else
                        {
                            return string.Empty;
                        }
                    }
                    catch (Exception ex)
                    {
                        return string.Empty;
                    }
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public class YAxisLabelFormatterForStockHistory : IAxisLabelFormatter
        {
            public string GetAxisLabelText(object axisValue)
            {
                return axisValue.ToString();
            }
        }
    }
}
