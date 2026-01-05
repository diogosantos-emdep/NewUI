using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Charts;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors.Flyout;
using DevExpress.Xpf.WindowsUI;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.OptimizedClass;
using Emdep.Geos.Modules.Warehouse.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    public class Dashboard2ViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable
    {
        #region 

        IWarehouseService WmsService = new WarehouseServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmStartUp = new CrmServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion

        #region Declaration
        string fromDate;
        string toDate;
        int isButtonStatus;
        DateTime startDate;
        DateTime endDate;
        Visibility isCalendarVisible;
        private double wonAmount;
        private string currencySymbol;
        private DataTable graphDataTable;
        private DataTable graphDataTableCustomer;
        private DataTable graphDataTableInventory;
        private ChartControl chartControl;
        private ChartControl chartControlCustomer;
        private ChartControl chartControlInventory;
        private DataTable dt = new DataTable();
        private double stockAmount;
        private double obsoleteAmount;
        private double sleepingAmount;
        private Currency selectedCurrency;
        private List<OfferDetail> salesStatusByMonthList;
        private List<WarehouseCustomer> salesStatusByCustomerList;
        private string minAmount;
        private string maxAmount;
        private string avgAmount;
        private string actAmount;
        private string color;
        private List<int> months;
        private List<int> years;
        private List<YearMonth> ListYearMonths;
        private XYDiagram2D diagram;
        private XYDiagram2D diagram1;
        private BarSideBySideSeries2D barSideBySideSeries2D;
        private BarSideBySideStackedSeries2D barSideBySideStackedSeries2D;
        private YearMonth yearMonth;
        #endregion

        #region Properties
        public List<OfferDetail> SalesStatusByMonthList
        {
            get { return salesStatusByMonthList; }
            set { salesStatusByMonthList = value; }
        }
        public List<WarehouseCustomer> SalesStatusByCustomerList
        {
            get { return salesStatusByCustomerList; }
            set { salesStatusByCustomerList = value; }
        }
        public DataTable GraphDataTable
        {
            get { return graphDataTable; }
            set { graphDataTable = value; OnPropertyChanged(new PropertyChangedEventArgs("GraphDataTable")); }
        }
        public DataTable GraphDataTableCustomer
        {
            get { return graphDataTableCustomer; }
            set { graphDataTableCustomer = value; OnPropertyChanged(new PropertyChangedEventArgs("GraphDataTableCustomer")); }
        }
        public DataTable GraphDataTableInventory
        {
            get { return graphDataTableInventory; }
            set { graphDataTableInventory = value; OnPropertyChanged(new PropertyChangedEventArgs("GraphDataTableInventory")); }
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
        public double WonAmount
        {
            get
            {
                return wonAmount;
            }

            set
            {
                wonAmount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WonAmount"));
            }
        }
        public string CurrencySymbol
        {
            get
            {
                return currencySymbol;
            }

            set
            {
                currencySymbol = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CurrencySymbol"));
            }
        }
        public double StockAmount
        {
            get
            {
                return stockAmount;
            }

            set
            {
                stockAmount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StockAmount"));
            }
        }
        public double ObsoleteAmount
        {
            get
            {
                return obsoleteAmount;
            }

            set
            {
                obsoleteAmount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ObsoleteAmount"));
            }
        }
        public double SleepingAmount
        {
            get
            {
                return sleepingAmount;
            }

            set
            {
                sleepingAmount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SleepingAmount"));
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

        public string MinAmount
        {
            get
            {
                return minAmount;
            }

            set
            {
                minAmount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MinAmount"));
            }
        }

        public string MaxAmount
        {
            get
            {
                return maxAmount;
            }

            set
            {
                maxAmount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MaxAmount"));
            }
        }

        public string AvgAmount
        {
            get
            {
                return avgAmount;
            }

            set
            {
                avgAmount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AvgAmount"));
            }
        }

        public string ActAmount
        {
            get
            {
                return actAmount;
            }

            set
            {
                actAmount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ActAmount"));
            }
        }
        public string Color
        {
            get
            {
                return color;
            }

            set
            {
                color = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Color"));
            }
        }
        #endregion

        #region Commands
        public ICommand PeriodThisYearCommand { get; set; }
        public ICommand PeriodLastYearCommand { get; set; }
        public ICommand PeriodLast12MonthsCommand { get; set; }
        public ICommand PeriodCustomRangeCommand { get; set; }
        public ICommand ApplyCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand ChartLoadCommand { get; set; }
        public ICommand ChartLoadCustomerCommand { get; set; }
        public ICommand ChartDashboardSaleCustomDrawCrosshairCommand { get; set; }
        public ICommand WarehouseChangeCommand { get; set; }
        public ICommand ChartLoadInventoryCommand { get; set; }
        public ICommand ChartDashboardInventoryDrawCrosshairCommand { get; set; }
       public ICommand RefreshDashboard2ViewCommand { get; set; }

      
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
        #endregion

        #region Constructor
        public Dashboard2ViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor Dashboard2ViewModel ...", category: Category.Info, priority: Priority.Low);

                Processing();
                SetColor();
                PeriodThisYearCommand = new DelegateCommand<object>(PeriodThisYearCommandAction);
                PeriodLastYearCommand = new DelegateCommand<object>(PeriodLastYearCommandAction);
                PeriodLast12MonthsCommand = new DelegateCommand<object>(PeriodLast12MonthsCommandAction);
                PeriodCustomRangeCommand = new DelegateCommand<object>(PeriodCustomRangeCommandAction);
                ApplyCommand = new DelegateCommand<object>(ApplyCommandAction);
                CancelCommand = new DelegateCommand<object>(CancelCommandAction);

                ChartLoadCommand = new DelegateCommand<object>(ChartLoadAction);
                ChartLoadCustomerCommand = new DelegateCommand<object>(ChartLoadCustomerAction);
                //ChartLoadInventoryCommand = new DelegateCommand<object>(ChartLoadInventoryCommandAction);
                ChartDashboardSaleCustomDrawCrosshairCommand = new DelegateCommand<object>(ChartDashboardSaleCustomDrawCrosshairCommandAction);
                //ChartDashboardInventoryDrawCrosshairCommand = new DelegateCommand<object>(ChartDashboardInventoryCustomDrawCrosshairCommandAction);
                WarehouseChangeCommand = new DelegateCommand<object>(WarehouseChangeCommandAction);
                RefreshDashboard2ViewCommand = new DelegateCommand<object>(RefreshDashboard2ViewCommandAction);
                setDefaultPeriod();

                List<Currency> CurrencyList = GeosApplication.Instance.Currencies.ToList();
                SelectedCurrency = CurrencyList.FirstOrDefault(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency_Warehouse"]);
                GeosApplication.Instance.WMSCurrentCurrencySymbol = CurrencyList.FirstOrDefault(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency_Warehouse"]).Symbol;
                SetWonAmount();
                SetStockAmount();
                SetObsoleteAmount();
                SetSleepingAmount();
                CreateTable();
                CreateTableCustomer();
                //CreateTableInventory();
                StartDate = DateTime.Today.Date;
                EndDate = DateTime.Today.Date;

                IsCalendarVisible = Visibility.Collapsed;
                if (DXSplashScreen.IsActive ) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method Dashboard2ViewModel....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method Dashboard2ViewModel...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        #endregion

        #region Methods
        private void SetColor()
        {

            try
            {
                GeosApplication.Instance.Logger.Log("Method SetColor ...", category: Category.Info, priority: Priority.Low);

                List<SalesStatusType> ListAllSalesStatusType = CrmStartUp.GetAllSalesStatusType().ToList();
                if (ListAllSalesStatusType != null)
                {
                    Color = ListAllSalesStatusType.Where(a => a.IdSalesStatusType == 4).FirstOrDefault().HtmlColor;
                    if (Color == null || Color == "")
                    {
                        Color = "#70AD47";
                    }
                }
                GeosApplication.Instance.Logger.Log("Method SetColor() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SetColor() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SetColor() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SetColor() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        private Duration _currentDuration;

        private void FlyoutControl_Closed(object sender, EventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor FlyoutControl_Closed ...", category: Category.Info, priority: Priority.Low);
                var flyout = (sender as FlyoutControl);
                flyout.AnimationDuration = _currentDuration;
                flyout.Closed -= FlyoutControl_Closed;
                Processing();
                if (IsButtonStatus == 0)
                {
                    setDefaultPeriod();
                }
                else if (IsButtonStatus == 1)
                {
                    int year = DateTime.Now.Year - 1;
                    DateTime StartFromDate = new DateTime(year, 1, 1);
                    DateTime EndToDate = new DateTime(year, 12, 31);

                    FromDate = StartFromDate.ToShortDateString();
                    ToDate = EndToDate.ToShortDateString();
                }
                else if (IsButtonStatus == 2)
                {
                    DateTime Date_F = DateTime.Now.Date.AddMonths(-12);
                    DateTime Date_T = DateTime.Now.Date;
                    FromDate = Date_F.ToShortDateString();
                    ToDate = Date_T.ToShortDateString();
                }
                else if (IsButtonStatus == 3)
                {
                    FromDate = StartDate.ToShortDateString();
                    ToDate = EndDate.ToShortDateString();
                }
                List<Currency> CurrencyList = GeosApplication.Instance.Currencies.ToList();
                SelectedCurrency = CurrencyList.FirstOrDefault(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency_Warehouse"]);
                GeosApplication.Instance.WMSCurrentCurrencySymbol = CurrencyList.FirstOrDefault(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency_Warehouse"]).Symbol;
                GeosApplication.SetCurrencySymbol_warehouse(GeosApplication.Instance.WMSCurrentCurrencySymbol);

                //if (chartControl.Diagram.Series.Count > 1)
                //{
                //    int Seriescount = chartControl.Diagram.Series.Count - 1;
                //    for (int i = 0; i < Seriescount; i++)
                //    {
                //        //chartControl.Diagram.Series.Remove(chartControl.Diagram.Series[i].Item.Series);
                //    }
                //}

                chartControl.Diagram = null;

                SetWonAmount();
                SetStockAmount();
                SetObsoleteAmount();
                SetSleepingAmount();
                if (IsButtonStatus == 3)
                {
                    GetSalesStatusMonthListAndCreateTable();
                    CreateSeriesPoints();
                }
                else
                {
                    chartControl.Diagram = diagram;
                    CreateTable();
                }
                CreateTableCustomer();
                //CreateTableInventory();
                SetColor();
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

        /// <summary>
        /// Create a chartcontrol seriespoints as per the selected date.
        /// [001] [vsana][03-12-2020][GEOS2-2712] Dashboard2 Sales by month data is overlapped when selecting 2 year period.
        /// </summary>
        private void CreateSeriesPoints()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CreateSeriesPoints ...", category: Category.Info, priority: Priority.Low);
                int currentYearsCount = 0;

                diagram1 = new XYDiagram2D();

                diagram1.ActualAxisX.Title = new AxisTitle() { Content = "Months" };
                diagram1.ActualAxisY.Title = new AxisTitle() { Content = "Amount" };
                diagram1.ActualAxisX.QualitativeScaleOptions = new QualitativeScaleOptions();
                diagram1.ActualAxisX.QualitativeScaleOptions.AutoGrid = false;
                diagram1.ActualAxisY.NumericOptions = new NumericOptions();
                diagram1.ActualAxisY.NumericOptions.Format = NumericFormat.Currency;
                GeosApplication.SetCurrencySymbol_warehouse(GeosApplication.Instance.WMSCurrentCurrencySymbol);


                foreach (var eachYear in years)
                {
                    currentYearsCount++;
                    if (years.Count >= (currentYearsCount))
                    {
                        barSideBySideSeries2D = new BarSideBySideSeries2D();
                        barSideBySideSeries2D.ArgumentScaleType = ScaleType.Auto;
                        barSideBySideSeries2D.ValueScaleType = ScaleType.Numerical;
                        barSideBySideSeries2D.DisplayName = eachYear.ToString();
                        barSideBySideSeries2D.CrosshairLabelPattern = "{S} : {V:c2}";
                        barSideBySideSeries2D.Model = new SimpleBar2DModel();
                        barSideBySideSeries2D.ShowInLegend = false;

                        foreach (var eachMonth in months)
                        {
                            SeriesPoint sp = new SeriesPoint();
                            sp.Argument = GetFullMonthName(eachMonth, eachYear);
                            sp.Value = ListYearMonths.FirstOrDefault(m => m.Month == eachMonth && m.Year == eachYear).Value;
                            barSideBySideSeries2D.Points.Add(sp);
                        }

                        barSideBySideSeries2D.AnimationAutoStartMode = AnimationAutoStartMode.SetStartState;
                        Bar2DSlideFromTopAnimation seriesAnimation = new Bar2DSlideFromTopAnimation();
                        seriesAnimation.Duration = new TimeSpan(0, 0, 2);
                        barSideBySideSeries2D.PointAnimation = seriesAnimation;

                        diagram1.Series.Add(barSideBySideSeries2D);
                    }
                }
                chartControl.Diagram = diagram1;
                GeosApplication.Instance.Logger.Log("Method CreateSeriesPoints() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in CreateSeriesPoints() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in CreateSeriesPoints() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CreateSeriesPoints() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Get the list from the SalesSatusPerMonthList and create the DataTable as per the list.
        /// [001] [vsana][03-12-2020][GEOS2-2712] Dashboard2 Sales by month data is overlapped when selecting 2 year period.
        /// </summary>
        private void GetSalesStatusMonthListAndCreateTable()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GetSalesStatusMonthListAndCreateTable ...", category: Category.Info, priority: Priority.Low);
                //[001] Add Service Method GetSalesByMonth
                SalesStatusByMonthList = WmsService.GetSalesByMonth(WarehouseCommon.Instance.Selectedwarehouse, SelectedCurrency.IdCurrency, Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate));

                dt = new DataTable();
                dt.Columns.Add("Month");
                dt.Columns.Add("Year");
                dt.Columns.Add("MonthYear");
                dt.Columns.Add("WON", typeof(double));
                dt.Rows.Clear();

                DataRow dr = null;
                years = new List<int>();
                months = new List<int>();
                ListYearMonths = new List<YearMonth>();

                int fromDateMonth = GetMonthNumber(FromDate);
                int fromDateYear = GetYearNumber(FromDate);
                int toDateMonth = GetMonthNumber(ToDate);
                int toDateYear = GetYearNumber(ToDate);

                //Get unselected months and set values as zero
                StartMonthEmptyValue(fromDateMonth, fromDateYear, dr);
                foreach (var eachList in SalesStatusByMonthList)
                {
                    dr = dt.NewRow();
                    dr[0] = null;
                    dr[1] = eachList.CurrentYear;
                    int column = 3;
                    if (!years.Contains(eachList.CurrentYear))
                    {
                        years.Add(eachList.CurrentYear);
                    }
                    if (!months.Contains(eachList.CurrentMonth))
                    {
                        months.Add(eachList.CurrentMonth);
                    }
                    dr[2] = GetFullMonthName(eachList.CurrentMonth, eachList.CurrentYear);
                    foreach (var item in dt.Columns)
                    {
                        if (item.ToString() != "Month" && item.ToString() != "Year" && item.ToString() != "MonthYear")
                        {
                            dr[column] = eachList.Value;
                            yearMonth = new YearMonth()
                            {
                                Month = eachList.CurrentMonth,
                                Year = eachList.CurrentYear,
                                Value = eachList.Value
                            };
                        }
                    }
                    dt.Rows.Add(dr);
                    ListYearMonths.Add(yearMonth);
                }
                // Get unselected months and set values as zero
                EndMonthEmptyValue(toDateMonth, toDateYear, dr);

                GraphDataTable = dt;
                if (chartControl != null) { chartControl.UpdateData(); }
                GeosApplication.Instance.Logger.Log("Method GetSalesStatusMonthListAndCreateTable() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetSalesStatusMonthListAndCreateTable() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
            
        }

        /// <summary>
        /// Set the Month Value as zero as per the custom selection from Calander ToDate.
        /// [001] [vsana][03-12-2020][GEOS2-2712] Dashboard2 Sales by month data is overlapped when selecting 2 year period.
        /// </summary>
        /// /// <param name="toDateMonth"></param>
        /// <param name="toDateYear"></param>
        /// <param name="dr"></param>
        private void EndMonthEmptyValue(int toDateMonth, int toDateYear, DataRow dr)
        {
            // This loop is to check the ToDate if not fill with dec the we add the value zero at remaining month
            for (int endOfMonth = toDateMonth; endOfMonth <= 12; endOfMonth++)
            {
                dr = dt.NewRow();
                yearMonth = new YearMonth()
                {
                    Month = endOfMonth,
                    Year = toDateYear,
                    Value = 0
                };
                dr[0] = null;
                dr[1] = toDateYear;
                dr[2] = GetFullMonthName(endOfMonth, toDateYear);
                dr[3] = 0;
                if (!years.Contains(yearMonth.Year))
                {
                    years.Add(yearMonth.Year);
                }
                if (!months.Contains(yearMonth.Month))
                {
                    months.Add(yearMonth.Month);
                }
                ListYearMonths.Add(yearMonth);
                dt.Rows.Add(dr);
            }
        }

        /// <summary>
        /// Set the Month Value as zero as per the CustomRange Selection from Calander FromDate.
        /// [001] [vsana][03-12-2020][GEOS-2712] Dashboard2 Sales by month data is overlapper when selecting 2 year period.
        /// </summary>
        /// <param name="fromDateMonth"></param>
        /// <param name="fromDateYear"></param>
        /// <param name="dr"></param>
        private void StartMonthEmptyValue(int fromDateMonth, int fromDateYear, DataRow dr)
        {
            // This Loop is to check the FromDate and any months are not there then all values will be Zero and year remains same
            for (int orderOfMonth = 1; orderOfMonth < fromDateMonth; orderOfMonth++)
            {
                dr = dt.NewRow();
                yearMonth = new YearMonth()
                {
                    Month = orderOfMonth,
                    Year = fromDateYear,
                    Value = 0
                };
                dr[0] = null;
                dr[1] = fromDateYear;
                dr[2] = GetFullMonthName(orderOfMonth, fromDateYear);
                dr[3] = 0;
                if (!years.Contains(yearMonth.Year))
                {
                    years.Add(yearMonth.Year);
                }
                if (!months.Contains(yearMonth.Month))
                {
                    months.Add(yearMonth.Month);
                }
                ListYearMonths.Add(yearMonth);
                dt.Rows.Add(dr);
            }
        }

        /// <summary>
        /// Get only Month number using the string Date.
        /// [001] [vsana][03-12-2020][GEOS-2712] Dashboard2 Sales by month data is overlapped when selecting 2 year period.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private int GetMonthNumber(string date)
        {
            DateTime dt = Convert.ToDateTime(date);
            return dt.Month;
        }

        /// <summary>
        /// Get only Year Number using the string date.
        /// [001] [vsana][03-12-2020][GEOS-2712] Dashboard2 Sales by month date is overlapped when selecting 2 year period.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private int GetYearNumber(string date)
        {
            DateTime dt = Convert.ToDateTime(date);
            return dt.Year;
        }

        /// <summary>
        /// Get the Full Month Name using the month and year values
        /// [001] [vsana][03-12-2020][GEOS-2712] Dashboard2 Sales by month data is overlapped when selectng 2 year period.
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        private string GetFullMonthName(int month, int year)
        {
            DateTime date = new DateTime(year, month, 1);
            return date.ToString("MMMM");
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
        
        private void CancelCommandAction(object obj)
        {
            MenuFlyout menu = (MenuFlyout)obj;
            menu.IsOpen = false;
        }

        private void PeriodCustomRangeCommandAction(object obj)
        {
            IsButtonStatus = 3;
            IsCalendarVisible = Visibility.Visible;
        }

        private void PeriodLast12MonthsCommandAction(object obj)
        {
            IsButtonStatus = 2;
            IsCalendarVisible = Visibility.Collapsed;
        }

        private void PeriodLastYearCommandAction(object obj)
        {
            IsButtonStatus = 1;
            IsCalendarVisible = Visibility.Collapsed;
        }

        private void PeriodThisYearCommandAction(object obj)
        {
            IsButtonStatus = 0;
            IsCalendarVisible = Visibility.Collapsed;
        }
        private void setDefaultPeriod()
        {
            int year = DateTime.Now.Year;
            DateTime StartFromDate = new DateTime(year, 1, 1);
            DateTime EndToDate = new DateTime(year, 12, 31);

            FromDate = StartFromDate.ToShortDateString();
            ToDate = EndToDate.ToShortDateString();
        }
        private void WarehouseChangeCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor WarehouseChangeCommandAction ...", category: Category.Info, priority: Priority.Low);

                Processing();
                List<Currency> CurrencyList = GeosApplication.Instance.Currencies.ToList();
                SelectedCurrency = CurrencyList.FirstOrDefault(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency_Warehouse"]);
                GeosApplication.Instance.WMSCurrentCurrencySymbol = CurrencyList.FirstOrDefault(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency_Warehouse"]).Symbol;
                GeosApplication.SetCurrencySymbol_warehouse(GeosApplication.Instance.WMSCurrentCurrencySymbol);

                SetWonAmount();
                SetStockAmount();
                SetObsoleteAmount();
                SetSleepingAmount();
                if(SalesStatusByMonthList.Count > 13)
                {
                    GetSalesStatusMonthListAndCreateTable();
                }
                else
                {
                    CreateTable();
                }
                CreateTableCustomer();
                //CreateTableInventory();
                SetColor();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method WarehouseChangeCommandAction....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method WarehouseChangeCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        
        private void RefreshDashboard2ViewCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor RefreshDashboard2ViewCommandAction ...", category: Category.Info, priority: Priority.Low);

                Processing();
                List<Currency> CurrencyList = GeosApplication.Instance.Currencies.ToList();
                SelectedCurrency = CurrencyList.FirstOrDefault(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency_Warehouse"]);
                GeosApplication.Instance.WMSCurrentCurrencySymbol = CurrencyList.FirstOrDefault(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency_Warehouse"]).Symbol;
                GeosApplication.SetCurrencySymbol_warehouse(GeosApplication.Instance.WMSCurrentCurrencySymbol);

                SetWonAmount();
                SetStockAmount();
                SetObsoleteAmount();
                SetSleepingAmount();
                if (SalesStatusByMonthList.Count > 13)
                {
                    GetSalesStatusMonthListAndCreateTable();
                }
                else
                {
                    CreateTable();
                }
                CreateTableCustomer();
                //CreateTableInventory();
                SetColor();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method RefreshDashboard2ViewCommandAction....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshDashboard2ViewCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void SetWonAmount()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SetWonAmount ...", category: Category.Info, priority: Priority.Low);
                
                WonAmount =Math.Round(WmsService.GetWONOfferAmount(WarehouseCommon.Instance.Selectedwarehouse, SelectedCurrency.IdCurrency, Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate)),2);
                CurrencySymbol = SelectedCurrency.Symbol;
                GeosApplication.Instance.Logger.Log("Method SetWonAmount() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SetWonAmount() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SetWonAmount() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SetWonAmount() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }
        private void SetStockAmount()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SetStockAmount ...", category: Category.Info, priority: Priority.Low);

                StockAmount =Math.Round(WmsService.GetArticleStockAmountInWarehouse(WarehouseCommon.Instance.Selectedwarehouse, SelectedCurrency.IdCurrency, Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate)),2);
                GeosApplication.Instance.Logger.Log("Method SetStockAmount() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SetStockAmount() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SetStockAmount() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SetStockAmount() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        private void SetObsoleteAmount()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SetObsoleteAmount ...", category: Category.Info, priority: Priority.Low);

                ObsoleteAmount =Math.Round(WmsService.GetAbosleteArticleStockAmountInWarehouse(WarehouseCommon.Instance.Selectedwarehouse, SelectedCurrency.IdCurrency, Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate)),2);
                GeosApplication.Instance.Logger.Log("Method SetObsoleteAmount() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SetObsoleteAmount() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SetObsoleteAmount() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SetObsoleteAmount() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        private void SetSleepingAmount()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SetSleepingAmount ...", category: Category.Info, priority: Priority.Low);

                SleepingAmount = Math.Round(WmsService.GetSleepedArticleStockAmountInWarehouse(WarehouseCommon.Instance.Selectedwarehouse, SelectedCurrency.IdCurrency, Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate),WarehouseCommon.Instance.ArticleSleepDays),2);
                GeosApplication.Instance.Logger.Log("Method SetSleepingAmount() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SetSleepingAmount() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SetSleepingAmount() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SetSleepingAmount() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        private void ChartLoadAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChartLoadAction ...", category: Category.Info, priority: Priority.Low);

                chartControl = (ChartControl)obj;
                chartControl.BeginInit();
                diagram = new XYDiagram2D();
                chartControl.Diagram = diagram;
                diagram.ActualAxisX.Title = new AxisTitle() { Content = "Months" };
                diagram.ActualAxisY.Title = new AxisTitle() { Content = "Amount" };
                diagram.ActualAxisX.QualitativeScaleOptions = new QualitativeScaleOptions();
                diagram.ActualAxisX.QualitativeScaleOptions.AutoGrid = false;
                diagram.ActualAxisY.NumericOptions = new NumericOptions();
                diagram.ActualAxisY.NumericOptions.Format = NumericFormat.Currency;
                GeosApplication.SetCurrencySymbol_warehouse(GeosApplication.Instance.WMSCurrentCurrencySymbol);

                barSideBySideStackedSeries2D = new BarSideBySideStackedSeries2D();
                barSideBySideStackedSeries2D.ArgumentScaleType = ScaleType.Auto;
                barSideBySideStackedSeries2D.ValueScaleType = ScaleType.Numerical;
                barSideBySideStackedSeries2D.DisplayName = "WON";

                #region brush	
                //if (SalesStatusByMonthList != null && SalesStatusByMonthList.Count > 0)	
                //{	
                //    barSideBySideSeries2D.Brush = (SolidColorBrush)(new BrushConverter().ConvertFrom(SalesStatusByMonthList.FirstOrDefault(a => a.SaleStatusHTMLColor != "" && a.SaleStatusHTMLColor != null).SaleStatusHTMLColor.ToString()));	
                //}	
                //else if (SalesStatusByCustomerList != null && SalesStatusByCustomerList.Count > 0)	
                //{	
                //    barSideBySideSeries2D.Brush = (SolidColorBrush)(new BrushConverter().ConvertFrom(SalesStatusByCustomerList.FirstOrDefault(a => a.SaleStatusHTMLColor != "" && a.SaleStatusHTMLColor != null).SaleStatusHTMLColor.ToString()));	
                //}	
                //else	
                //{	
                //    if (Color == null || Color == "")	
                //    {	
                //        barSideBySideSeries2D.Brush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#70AD47"));	
                //    }	
                //    else	
                //    {	
                //        barSideBySideSeries2D.Brush = (SolidColorBrush)(new BrushConverter().ConvertFrom(Color));	
                //    }	
                //}	
                #endregion brush	

                barSideBySideStackedSeries2D.ArgumentDataMember = "MonthYear";
                barSideBySideStackedSeries2D.ValueDataMember = "WON";
                barSideBySideStackedSeries2D.CrosshairLabelPattern = "{S} : {V:c2}";
                barSideBySideStackedSeries2D.Model = new DevExpress.Xpf.Charts.SimpleBar2DModel();
                barSideBySideStackedSeries2D.ShowInLegend = false;

                barSideBySideStackedSeries2D.AnimationAutoStartMode = AnimationAutoStartMode.SetStartState;
                Bar2DSlideFromTopAnimation seriesAnimation = new Bar2DSlideFromTopAnimation();
                seriesAnimation.Duration = new TimeSpan(0, 0, 2);
                barSideBySideStackedSeries2D.PointAnimation = seriesAnimation;

                diagram.Series.Add(barSideBySideStackedSeries2D);

                chartControl.EndInit();
                chartControl.AnimationMode = ChartAnimationMode.OnDataChanged;
                chartControl.Animate();

                GeosApplication.Instance.Logger.Log("Method ChartLoadAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in ChartLoadAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in ChartLoadAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ChartLoadAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        
        private void ChartLoadCustomerAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChartLoadCustomerAction ...", category: Category.Info, priority: Priority.Low);

                chartControlCustomer = (ChartControl)obj;
                chartControlCustomer.BeginInit();
                XYDiagram2D diagram = new XYDiagram2D();
                chartControlCustomer.Diagram = diagram;
                diagram.ActualAxisX.Title = new AxisTitle() { Content = "Customer" };
                diagram.ActualAxisY.Title = new AxisTitle() { Content = "Amount" };
                diagram.ActualAxisX.QualitativeScaleOptions = new QualitativeScaleOptions();
                diagram.ActualAxisX.QualitativeScaleOptions.AutoGrid = false;
                diagram.ActualAxisY.NumericOptions = new NumericOptions();
                diagram.ActualAxisY.NumericOptions.Format = NumericFormat.Currency;
                GeosApplication.SetCurrencySymbol_warehouse(GeosApplication.Instance.WMSCurrentCurrencySymbol);

                BarSideBySideStackedSeries2D barSideBySideStackedSeries2D = new BarSideBySideStackedSeries2D();
                barSideBySideStackedSeries2D.ArgumentScaleType = ScaleType.Auto;
                barSideBySideStackedSeries2D.ValueScaleType = ScaleType.Numerical;
                barSideBySideStackedSeries2D.DisplayName = "Amount";
                if (SalesStatusByMonthList != null && SalesStatusByMonthList.Count > 0)
                {
                    barSideBySideStackedSeries2D.Brush = (SolidColorBrush)(new BrushConverter().ConvertFrom(SalesStatusByMonthList.FirstOrDefault(a => a.SaleStatusHTMLColor != "" && a.SaleStatusHTMLColor != null).SaleStatusHTMLColor.ToString()));
                }
                else if (SalesStatusByCustomerList != null && SalesStatusByCustomerList.Count > 0)
                {
                    barSideBySideStackedSeries2D.Brush = (SolidColorBrush)(new BrushConverter().ConvertFrom(SalesStatusByCustomerList.FirstOrDefault(a => a.SaleStatusHTMLColor != "" && a.SaleStatusHTMLColor != null).SaleStatusHTMLColor.ToString()));
                }
                else
                {
                    if (Color == null || Color == "")
                    {
                        barSideBySideStackedSeries2D.Brush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#70AD47"));
                    }
                    else
                    {
                        barSideBySideStackedSeries2D.Brush = (SolidColorBrush)(new BrushConverter().ConvertFrom(Color));
                    }
                }
                barSideBySideStackedSeries2D.ArgumentDataMember = "Alias";
                barSideBySideStackedSeries2D.ValueDataMember = "Amount";
                barSideBySideStackedSeries2D.CrosshairLabelPattern = "{S} : {V:c2}";
                barSideBySideStackedSeries2D.Model = new DevExpress.Xpf.Charts.SimpleBar2DModel();
                barSideBySideStackedSeries2D.ShowInLegend = false;

                barSideBySideStackedSeries2D.AnimationAutoStartMode = AnimationAutoStartMode.SetStartState;
                Bar2DSlideFromTopAnimation seriesAnimation = new Bar2DSlideFromTopAnimation();
                seriesAnimation.Duration = new TimeSpan(0, 0, 2);
                barSideBySideStackedSeries2D.PointAnimation = seriesAnimation;

                diagram.Series.Add(barSideBySideStackedSeries2D);
                chartControlCustomer.EndInit();
                chartControlCustomer.AnimationMode = ChartAnimationMode.OnDataChanged;
                chartControlCustomer.Animate();

                GeosApplication.Instance.Logger.Log("Method ChartLoadCustomerAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in ChartLoadCustomerAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in ChartLoadCustomerAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ChartLoadCustomerAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        
        private void ChartDashboardSaleCustomDrawCrosshairCommandAction(object obj)
        {
            try
            {
                CustomDrawCrosshairEventArgs e = (CustomDrawCrosshairEventArgs)obj;
                foreach (var group in e.CrosshairElementGroups)
                {
                    var reverseList = group.CrosshairElements.ToList();
                    group.CrosshairElements.Clear();
                    foreach (var item in reverseList)
                    {
                            item.Series.DisplayName = item.SeriesPoint.ActualArgument;
                            group.CrosshairElements.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ChartDashboardSaleCustomDrawCrosshairCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        private void ChartDashboardInventoryCustomDrawCrosshairCommandAction(object obj)
        {
            try
            {
                CustomDrawCrosshairEventArgs e = (CustomDrawCrosshairEventArgs)obj;
                foreach (var group in e.CrosshairElementGroups)
                {
                    var reverseList = group.CrosshairElements.ToList();
                    group.CrosshairElements.Clear();
                    foreach (var item in reverseList)
                    {
                        item.Series.DisplayName = item.SeriesPoint.ActualArgument;
                        group.CrosshairElements.Add(item);
                        
                    }
                    if (reverseList.FirstOrDefault().SeriesPoint.Value == 0)
                    {
                        ActAmount = "0K";
                    }
                    else
                    {
                        ActAmount = GetConvertedValue(reverseList.FirstOrDefault().SeriesPoint.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ChartDashboardInventoryCustomDrawCrosshairCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        private void CreateTable()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CreateTable ...", category: Category.Info, priority: Priority.Low);
                SalesStatusByMonthList = WmsService.GetSalesByMonth(WarehouseCommon.Instance.Selectedwarehouse, SelectedCurrency.IdCurrency, Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate));

                dt = new DataTable();
                dt.Columns.Add("Month");
                dt.Columns.Add("Year");
                dt.Columns.Add("MonthYear");

                dt.Columns.Add("WON", typeof(double));


                dt.Rows.Clear();
                int[] icol = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

                foreach (var mt in icol)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = null;
                    dr[1] = GetYearNumber(FromDate);
                    dr[2] = GetFullMonthName(mt,GetYearNumber(FromDate));
                    int k = 3;
                    foreach (var item in dt.Columns)
                    {
                        if (item.ToString() != "Month" && item.ToString() != "Year" && item.ToString() != "MonthYear")
                        {
                            dr[k] = SalesStatusByMonthList.Where(m => m.CurrentMonth == mt).Select(mv => mv.Value).ToList().Sum();
                            k++;
                        }
                    }
                    dt.Rows.Add(dr);
                }
                

                GeosApplication.Instance.Logger.Log("Method CreateTable() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CreateTable() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

            GraphDataTable = dt;
            if (chartControl != null) { chartControl.UpdateData(); }
        }

       
        private void CreateTableCustomer()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CreateTableCustomer ...", category: Category.Info, priority: Priority.Low);
                SalesStatusByCustomerList = WmsService.GetSalesByCustomer(WarehouseCommon.Instance.Selectedwarehouse, SelectedCurrency.IdCurrency, Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate));

                dt = new DataTable();
                List<string> list = SalesStatusByCustomerList.Where(x=>x.Alias!="" && x.Alias!= null).Select(a => a.Alias.ToString()).Distinct().ToList();
                
                dt.Columns.Add("Alias");
                dt.Columns.Add("Amount",typeof(double));

                dt.Rows.Clear();
                int k = 1;
                foreach (string alias in list)
                {
                    foreach (var item in dt.Columns)
                    {
                        if (item.ToString() != "Alias")
                        {
                            DataRow dr = dt.NewRow();
                            dr[0] = alias;
                            dr[k] = SalesStatusByCustomerList.Where(m => m.Alias == alias).Select(mv => mv.Amount).ToList().Sum();
                            dt.Rows.Add(dr);
                        }
                    }
                }

                GeosApplication.Instance.Logger.Log("Method CreateTableCustomer() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CreateTableCustomer() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

            GraphDataTableCustomer = dt;
            if (chartControlCustomer != null) { chartControlCustomer.UpdateData(); }
        }

        private void CreateTableInventory()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CreateTableInventory ...", category: Category.Info, priority: Priority.Low);

                dt = new DataTable();

                List<WarehouseInventoryWeek> list = WmsService.GetInventoryWeek(WarehouseCommon.Instance.Selectedwarehouse, SelectedCurrency.IdCurrency, Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate));

                if(list!=null && list.Count<=0)
                {
                    MinAmount = "0K";
                    MaxAmount = "0K";
                    AvgAmount = "0K";
                    ActAmount = "0K";
                }
                else
                {
                    MinAmount = GetConvertedValue(list.Min(a => a.Amount));
                    MaxAmount = GetConvertedValue(list.Max(a => a.Amount));
                    double totalAmount = list.Select(a => a.Amount).Sum();
                    if (totalAmount > 0)
                        AvgAmount = GetConvertedValue((totalAmount / list.Count()));
                    else
                        AvgAmount = "0K";

                    ActAmount = "0K";
                }

                List<long> weeks = list.Select(a => a.StockWeek).Distinct().ToList();
                int year_count = 0;
                string year = weeks.FirstOrDefault().ToString().Substring(0,4);
                foreach(long week in weeks)
                {
                    if(week.ToString().Substring(0,4)!=year)
                    {
                        year_count++;
                        break;
                    }
                }
                dt.Columns.Add("Week");

                dt.Columns.Add("Amount");
                dt.Columns.Add("Inventory_Week", typeof(double)).DefaultValue = 0;

                dt.Rows.Clear();

                int k = 1;
                int count = 1;
                double Inventory_Amount = 0.0;
                if (weeks.Count() > 0)
                {
                    Inventory_Amount = ((list.Max(a => a.Amount)) - (list.Min(a => a.Amount))) / (weeks.Count()-1);
                }
                foreach (long week in weeks)
                {
                    foreach (var item in dt.Columns)
                    {
                        if (item.ToString() != "Week" && item.ToString()!= "Inventory_Week")
                        {
                            DataRow dr = dt.NewRow();
                            if (year_count == 0)
                            {
                                dr[0] = week.ToString().Substring(4);
                            }
                            else
                            {
                                dr[0] = week.ToString();
                            }
                            double StockAmount = list.Where(m => m.StockWeek == week).Select(mv => mv.Amount).ToList().Sum();
                            dr[k] = StockAmount;

                            if (count== 1)
                            {
                                dr["Inventory_Week"] = list.Min(a => a.Amount);
                            }
                            else
                            {
                                dr["Inventory_Week"] = list.Min(a => a.Amount) + (count-1) * Inventory_Amount;
                            }
                            dt.Rows.Add(dr);
                            count++;
                        }
                    }
                }

                GeosApplication.Instance.Logger.Log("Method CreateTableInventory() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CreateTableInventory() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

            GraphDataTableInventory = dt;
            if (chartControlInventory != null) { chartControlInventory.UpdateData(); }
        }

        
        private void ChartLoadInventoryCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChartLoadInventoryCommandAction ...", category: Category.Info, priority: Priority.Low);

                chartControlInventory = (ChartControl)obj;
                chartControlInventory.BeginInit();
                XYDiagram2D diagram = new XYDiagram2D();
                chartControlInventory.Diagram = diagram;
                diagram.ActualAxisX.Title = new AxisTitle() { Content = "Week" };
                diagram.ActualAxisY.Title = new AxisTitle() { Content = "Amount" };
                diagram.ActualAxisX.QualitativeScaleOptions = new QualitativeScaleOptions();
                diagram.ActualAxisX.QualitativeScaleOptions.AutoGrid = false;
                diagram.ActualAxisY.NumericOptions = new NumericOptions();
                diagram.ActualAxisY.NumericOptions.Format = NumericFormat.Currency;
                GeosApplication.SetCurrencySymbol_warehouse(GeosApplication.Instance.WMSCurrentCurrencySymbol);

                LineSeries2D lineSeries2D = new LineSeries2D();
                lineSeries2D.ArgumentScaleType = ScaleType.Auto;
                lineSeries2D.ValueScaleType = ScaleType.Numerical;
                lineSeries2D.DisplayName = "Amount";
                lineSeries2D.ArgumentDataMember = "Week";
                lineSeries2D.ValueDataMember = "Amount";
                lineSeries2D.CrosshairLabelPattern = "{S} : {V:c2}";
                lineSeries2D.ShowInLegend = false;

                diagram.Series.Add(lineSeries2D);

                LineSeries2D lineDashedInventory_WeekMinMax = new LineSeries2D();
                lineDashedInventory_WeekMinMax.LineStyle = new LineStyle();
                lineDashedInventory_WeekMinMax.LineStyle.DashStyle = new DashStyle();
                lineDashedInventory_WeekMinMax.LineStyle.Thickness = 2;
                lineDashedInventory_WeekMinMax.LineStyle.DashStyle.Dashes = new DoubleCollection(new double[] { 3, 3, 3, 3 });
                lineDashedInventory_WeekMinMax.ArgumentScaleType = ScaleType.Auto;
                lineDashedInventory_WeekMinMax.ValueScaleType = ScaleType.Numerical;
                lineDashedInventory_WeekMinMax.DisplayName = "Inventory_Week";
                lineDashedInventory_WeekMinMax.CrosshairLabelPattern = "{S} : {V:c2}";

                lineDashedInventory_WeekMinMax.ArgumentDataMember = "Week";
                lineDashedInventory_WeekMinMax.ValueDataMember = "Inventory_Week";
                lineDashedInventory_WeekMinMax.ShowInLegend = false;
                chartControlInventory.Diagram.Series.Add(lineDashedInventory_WeekMinMax);

                chartControlInventory.EndInit();
                chartControlInventory.AnimationMode = ChartAnimationMode.OnDataChanged;
                chartControlInventory.Animate();

                GeosApplication.Instance.Logger.Log("Method ChartLoadInventoryCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in ChartLoadInventoryCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in ChartLoadInventoryCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ChartLoadInventoryCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
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
        private string GetConvertedValue(double amount)
        {
            string Value = "";
            if (amount > 1000000000000000)
                Value = Math.Floor(amount / 1000000000000000).ToString() + "Q";
            else if (amount > 1000000000000)
                Value = Math.Floor(amount / 1000000000000).ToString() + "T";
            else if (amount > 1000000000)
                Value = Math.Floor(amount / 1000000000).ToString() + "B";
            else if (amount > 1000000)
                Value = Math.Floor(amount / 1000000).ToString() + "M";
            else if (amount > 1000)
                Value =  Math.Floor(amount / 1000).ToString() + "K";
            else
                Value =Math.Round(amount,2).ToString();

            return Value;
        }
        public void Dispose()
        {
        }
        #endregion
    }
    public class YearMonth
    {
        Int32 month;
        Int32 year;
        double values;

        public Int32 Month
        {
            get { return month; }
            set { month = value; }
        }
        public Int32 Year
        {
            get { return year; }
            set { year = value; }
        }
        public double Value
        {
            get { return values; }
            set { values = value; }
        }
    }

}
