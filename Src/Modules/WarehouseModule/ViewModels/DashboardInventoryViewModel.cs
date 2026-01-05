//using System;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Charts;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors.Flyout;
using DevExpress.Xpf.WindowsUI;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.OptimizedClass;
using Emdep.Geos.Data.Common.WMS;
using Emdep.Geos.Modules.Warehouse.Views;
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
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using DevExpress.Xpf.Charts;
namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    public class DashboardInventoryViewModel : ViewModelBase, INotifyPropertyChanged, IDisposable
    {
        #region Services
        IWorkbenchStartUp WorkbenchService = new WorkbenchServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IWarehouseService WarehouseService = new WarehouseServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        IWarehouseService WmsService = new WarehouseServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmStartUp = new CrmServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion // End Services Region

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
        //public DataTable GraphDataTable
        //{
        //    get { return graphDataTable; }
        //    set { graphDataTable = value; OnPropertyChanged(new PropertyChangedEventArgs("GraphDataTable")); }
        //}
        //public DataTable GraphDataTableCustomer
        //{
        //    get { return graphDataTableCustomer; }
        //    set { graphDataTableCustomer = value; OnPropertyChanged(new PropertyChangedEventArgs("GraphDataTableCustomer")); }
        //}
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


        List<ArticleMaterialType> materialType;
        public List<ArticleMaterialType> MaterialType
        {
            get
            {
                return materialType;
            }

            set
            {
                materialType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MaterialType"));
            }
        }
        string runningMaterialColor;
        public string RunningMaterialColor
        {
            get
            {
                return runningMaterialColor;
            }

            set
            {
                runningMaterialColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RunningMaterialColor"));
            }
        }
        ArticleMaterialType runningMaterial;
        public ArticleMaterialType RunningMaterial
        {
            get
            {
                return runningMaterial;
            }

            set
            {
                runningMaterial = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RunningMaterial"));
            }
        }
        string specialMaterialColor;
        public string SpecialMaterialColor
        {
            get
            {
                return specialMaterialColor;
            }

            set
            {
                specialMaterialColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SpecialMaterialColor"));
            }
        }
        ArticleMaterialType specialMaterial;
        public ArticleMaterialType SpecialMaterial
        {
            get
            {
                return specialMaterial;
            }

            set
            {
                specialMaterial = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SpecialMaterial"));
            }
        }

        string specialAssetColor;
        public string SpecialAssetColor
        {
            get
            {
                return specialAssetColor;
            }

            set
            {
                specialAssetColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SpecialAssetColor"));
            }
        }
        ArticleMaterialType specialAsset;
        public ArticleMaterialType SpecialAsset
        {
            get
            {
                return specialAsset;
            }

            set
            {
                specialAsset = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SpecialAsset"));
            }
        }
        string warehouseStockTargetsColor;
        public string WarehouseStockTargetsColor
        {
            get
            {
                return warehouseStockTargetsColor;
            }

            set
            {
                warehouseStockTargetsColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WarehouseStockTargetsColor"));
            }
        }

        ArticleMaterialType warehouseStockTargets;
        public ArticleMaterialType WarehouseStockTargets
        {
            get
            {
                return warehouseStockTargets;
            }

            set
            {
                warehouseStockTargets = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WarehouseStockTargets"));
            }
        }
        string specialToolingColor;
        public string SpecialToolingColor
        {
            get
            {
                return specialToolingColor;
            }

            set
            {
                specialToolingColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SpecialToolingColor"));
            }
        }
        ArticleMaterialType specialTooling;
        public ArticleMaterialType SpecialTooling
        {
            get
            {
                return specialTooling;
            }

            set
            {
                specialTooling = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SpecialTooling"));
            }
        }
        string specialRunningColor;
        public string SpecialRunningColor
        {
            get
            {
                return specialRunningColor;
            }

            set
            {
                specialRunningColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SpecialRunningColor"));
            }
        }
        ArticleMaterialType specialRunning;
        public ArticleMaterialType SpecialRunning
        {
            get
            {
                return specialRunning;
            }

            set
            {
                specialRunning = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SpecialRunning"));
            }
        }

        //[Sudhir.Jangra][GEOS2-5471]
        ArticleMaterialType softwareRunning;
        public ArticleMaterialType SoftwareRunning
        {
            get { return softwareRunning; }
            set
            {
                softwareRunning = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SoftwareRunning"));
            }
        }
        //[Sudhir.Jangra][GEOS2-5471]
        string softwareRunningColor;
        public string SoftwareRunningColor
        {
            get { return softwareRunningColor; }
            set
            {
                softwareRunningColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SoftwareRunningColor"));
            }
        }

        string sleepingColor;
        public string SleepingColor
        {
            get
            {
                return sleepingColor;
            }

            set
            {
                sleepingColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SleepingColor"));
            }
        }
        string obsoleteColor;
        public string ObsoleteColor
        {
            get
            {
                return obsoleteColor;
            }

            set
            {
                obsoleteColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ObsoleteColor"));
            }
        }
        #endregion

        #region Public Events

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
        }
        #endregion // End Of Events 

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
        public ICommand ShowLineGraphViewCommand { get; set; }//[pramod.misal][GEOS2-5989][23.09.2024]
        public ICommand ShowBarGraphViewCommand { get; set; }//[pramod.misal][GEOS2-5989][23.09.2024]


        #endregion

        #region Constructor
        public DashboardInventoryViewModel()
        {
            try
            {
                SleepingColor = "#fcba03";
                ObsoleteColor = "#96968f";
                GeosApplication.Instance.Logger.Log("Constructor WorkOrderViewModel....", category: Category.Info, priority: Priority.Low);
                Processing();
                SetColor();
                PeriodThisYearCommand = new DelegateCommand<object>(PeriodThisYearCommandAction);
                PeriodLastYearCommand = new DelegateCommand<object>(PeriodLastYearCommandAction);
                PeriodLast12MonthsCommand = new DelegateCommand<object>(PeriodLast12MonthsCommandAction);
                PeriodCustomRangeCommand = new DelegateCommand<object>(PeriodCustomRangeCommandAction);
                setDefaultPeriod();
                ApplyCommand = new DelegateCommand<object>(ApplyCommandAction);
                CancelCommand = new DelegateCommand<object>(CancelCommandAction);

                WarehouseChangeCommand = new DelegateCommand<object>(WarehouseChangeCommandAction);
                RefreshDashboard2ViewCommand = new DelegateCommand<object>(RefreshDashboard2ViewCommandAction);

                ChartLoadInventoryCommand = new DelegateCommand<object>(ChartLoadInventoryCommandAction);
                ChartDashboardInventoryDrawCrosshairCommand = new DelegateCommand<object>(ChartDashboardInventoryCustomDrawCrosshairCommandAction);
                //[pramod.misal][GEOS2-5689][14.08.2024]
                ShowLineGraphViewCommand = new DelegateCommand<object>(LineGraphViewAction);
                ShowBarGraphViewCommand = new DelegateCommand<object>(BarGraphViewAction);

                List<Currency> CurrencyList = GeosApplication.Instance.Currencies.ToList();
                SelectedCurrency = CurrencyList.FirstOrDefault(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency_Warehouse"]);
                GeosApplication.Instance.WMSCurrentCurrencySymbol = CurrencyList.FirstOrDefault(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency_Warehouse"]).Symbol;
                SetWonAmount();
                SetStockAmount();
                SetObsoleteAmount();
                SetSleepingAmount();
                SetArticleMaterialType();
                CreateTableInventory();

                StartDate = DateTime.Today.Date;
                EndDate = DateTime.Today.Date;
                IsCalendarVisible = Visibility.Collapsed;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Constructor WorkOrderViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in WorkOrderViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Methods

        private void CreateTableInventory()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CreateTableInventory ...", category: Category.Info, priority: Priority.Low);

                dt = new DataTable();
                //WmsService = new WarehouseServiceController("localhost:6699");
                List<WarehouseInventoryWeek> list = WmsService.GetInventoryWeek_V2390(WarehouseCommon.Instance.Selectedwarehouse, SelectedCurrency.IdCurrency, Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate));
                //WmsService = new WarehouseServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                if (list != null && list.Count <= 0)
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
                string year = weeks.FirstOrDefault().ToString().Substring(0, 4);
                foreach (long week in weeks)
                {
                    if (week.ToString().Substring(0, 4) != year)
                    {
                        year_count++;
                        break;
                    }
                }
                dt.Columns.Add("Week");
                dt.Columns.Add("Stock", typeof(double)).DefaultValue = 0;
                dt.Columns.Add("Inventory_Week", typeof(double)).DefaultValue = 0;
                dt.Columns.Add("RM", typeof(double)).DefaultValue = 0;
                dt.Columns.Add("SM", typeof(double)).DefaultValue = 0;
                dt.Columns.Add("SA", typeof(double)).DefaultValue = 0;
                dt.Columns.Add("ST", typeof(double)).DefaultValue = 0;
                dt.Columns.Add("SR", typeof(double)).DefaultValue = 0;
                dt.Columns.Add("SF", typeof(double)).DefaultValue = 0;//[Sudhir.jangra][GEOS2-5471]
                dt.Columns.Add("Target", typeof(double)).DefaultValue = 0;
              
                dt.Rows.Clear();

                int k = 1;
                int count = 1;
                double Inventory_Amount = 0.0;
                if (weeks.Count() > 0)
                {
                    Inventory_Amount = ((list.Max(a => a.Amount)) - (list.Min(a => a.Amount))) / (weeks.Count() - 1);
                }
                foreach (long week in weeks)
                {
                    foreach (var item in dt.Columns)
                    {
                        if (item.ToString() != "Week" && item.ToString() != "Inventory_Week"
                            && item.ToString() != "RM" && item.ToString() != "SM"
                            && item.ToString() != "SA" && item.ToString() != "ST"
                            && item.ToString() != "SR" && item.ToString() != "Target" && item.ToString() != "SF")
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

                            if (count == 1)
                            {
                                dr["Inventory_Week"] = list.Min(a => a.Amount);
                            }
                            else
                            {
                                dr["Inventory_Week"] = list.Min(a => a.Amount) + (count - 1) * Inventory_Amount;
                            }
                            try
                            {
                                WarehouseInventoryWeek tempwarehouseInventoryWeek = list.Where(m => m.StockWeek == week).FirstOrDefault();
                                #region ArticleMaterialType
                                //try
                                //{
                                //    foreach (ArticleMaterialType itemArticleMaterialType in tempwarehouseInventoryWeek.ArticleMaterialType)
                                //    {
                                //        if (itemArticleMaterialType.MaterialTypeId == 226)
                                //        {
                                //            dr["RM"] = itemArticleMaterialType.ArticleStockAmount;
                                //        }
                                //        else if (itemArticleMaterialType.MaterialTypeId == 227)
                                //        {
                                //            dr["RM"] = itemArticleMaterialType.ArticleStockAmount;
                                //        }
                                //        else if (itemArticleMaterialType.MaterialTypeId == 228)
                                //        {
                                //            dr["RM"] = itemArticleMaterialType.ArticleStockAmount;
                                //        }
                                //        else if (itemArticleMaterialType.MaterialTypeId == 229)
                                //        {
                                //            dr["RM"] = itemArticleMaterialType.ArticleStockAmount;
                                //        }
                                //        else if (itemArticleMaterialType.MaterialTypeId == 1526)
                                //        {
                                //            dr["RM"] = itemArticleMaterialType.ArticleStockAmount;
                                //        }
                                //    }
                                //}
                                //catch (Exception ex)
                                //{
                                //}
                                #endregion
                                //dr[k + 2] = StockAmount + 2000000;
                                //dr[k + 3] = StockAmount - 2000000;
                                if (tempwarehouseInventoryWeek != null)
                                {
                                    dr[k + 2] = tempwarehouseInventoryWeek.ArticleMaterialType.Where(w => w.MaterialTypeId == 226).Select(s => s.ArticleStockAmount).DefaultIfEmpty(0).FirstOrDefault();
                                    dr[k + 3] = tempwarehouseInventoryWeek.ArticleMaterialType.Where(w => w.MaterialTypeId == 227).Select(s => s.ArticleStockAmount).DefaultIfEmpty(0).FirstOrDefault();
                                    dr[k + 4] = tempwarehouseInventoryWeek.ArticleMaterialType.Where(w => w.MaterialTypeId == 228).Select(s => s.ArticleStockAmount).DefaultIfEmpty(0).FirstOrDefault();
                                    dr[k + 5] = tempwarehouseInventoryWeek.ArticleMaterialType.Where(w => w.MaterialTypeId == 229).Select(s => s.ArticleStockAmount).DefaultIfEmpty(0).FirstOrDefault();
                                    dr[k + 6] = tempwarehouseInventoryWeek.ArticleMaterialType.Where(w => w.MaterialTypeId == 1526).Select(s => s.ArticleStockAmount).DefaultIfEmpty(0).FirstOrDefault();
                                    dr[k + 7] = tempwarehouseInventoryWeek.ArticleMaterialType.Where(w => w.MaterialTypeId == 1880).Select(s => s.ArticleStockAmount).DefaultIfEmpty(0).FirstOrDefault();//[Sudhir.jangra][GEOS2-5471]

                                }
                                if (WarehouseStockTargets != null)
                                {
                                    dr[k + 8] = WarehouseStockTargets.ArticleStockAmount;
                                }
                            }
                            catch (Exception ex)
                            {
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

        #region  old ChartLoadInventoryCommandAction Modified for GEOS2-4853
        //private void ChartLoadInventoryCommandAction(object obj)
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method ChartLoadInventoryCommandAction ...", category: Category.Info, priority: Priority.Low);

        //        chartControlInventory = (ChartControl)obj;
        //        chartControlInventory.BeginInit();
        //        XYDiagram2D diagram = new XYDiagram2D();
        //        chartControlInventory.Diagram = diagram;
        //        diagram.ActualAxisX.Title = new AxisTitle() { Content = "Week" };
        //        diagram.ActualAxisY.Title = new AxisTitle() { Content = "Amount" };
        //        diagram.ActualAxisX.QualitativeScaleOptions = new QualitativeScaleOptions();
        //        diagram.ActualAxisX.QualitativeScaleOptions.AutoGrid = false;
        //        diagram.ActualAxisY.NumericOptions = new NumericOptions();
        //        diagram.ActualAxisY.NumericOptions.Format = NumericFormat.Currency;
        //        GeosApplication.SetCurrencySymbol_warehouse(GeosApplication.Instance.WMSCurrentCurrencySymbol);

        //        LineSeries2D lineSeries2D = new LineSeries2D();
        //        System.Drawing.Color ColorAmount = System.Drawing.ColorTranslator.FromHtml(string.IsNullOrEmpty(Color) ? Color : Color);
        //        lineSeries2D.Brush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(ColorAmount.R, ColorAmount.G, ColorAmount.B));
        //        lineSeries2D.ArgumentScaleType = ScaleType.Auto;
        //        lineSeries2D.ValueScaleType = ScaleType.Numerical;
        //        lineSeries2D.DisplayName = "Amount";
        //        lineSeries2D.ArgumentDataMember = "Week";
        //        lineSeries2D.ValueDataMember = "Amount";
        //        lineSeries2D.CrosshairLabelPattern = "{S} : {V:c2}";
        //        lineSeries2D.ShowInLegend = false;
        //        diagram.Series.Add(lineSeries2D);

        //        try
        //        {
        //            #region lineDashedInventory_WeekMinMax
        //            //LineSeries2D lineDashedInventory_WeekMinMax = new LineSeries2D();
        //            //lineDashedInventory_WeekMinMax.LineStyle = new LineStyle();
        //            //lineDashedInventory_WeekMinMax.LineStyle.DashStyle = new DashStyle();
        //            //lineDashedInventory_WeekMinMax.LineStyle.Thickness = 2;
        //            //lineDashedInventory_WeekMinMax.LineStyle.DashStyle.Dashes = new DoubleCollection(new double[] { 3, 3, 3, 3 });
        //            //lineDashedInventory_WeekMinMax.ArgumentScaleType = ScaleType.Auto;
        //            //lineDashedInventory_WeekMinMax.ValueScaleType = ScaleType.Numerical;
        //            //lineDashedInventory_WeekMinMax.DisplayName = "Inventory_Week";
        //            //lineDashedInventory_WeekMinMax.CrosshairLabelPattern = "{S} : {V:c2}";
        //            //lineDashedInventory_WeekMinMax.ArgumentDataMember = "Week";
        //            //lineDashedInventory_WeekMinMax.ValueDataMember = "Inventory_Week";
        //            //lineDashedInventory_WeekMinMax.ShowInLegend = false;
        //            //chartControlInventory.Diagram.Series.Add(lineDashedInventory_WeekMinMax);
        //            #endregion

        //            LineSeries2D lineSeries2DForTarget = new LineSeries2D();
        //            lineSeries2DForTarget.LineStyle = new LineStyle();
        //            lineSeries2DForTarget.LineStyle.DashStyle = new DashStyle();
        //            lineSeries2DForTarget.LineStyle.Thickness = 2;
        //            lineSeries2DForTarget.LineStyle.DashStyle.Dashes = new DoubleCollection(new double[] { 3, 3, 3, 3 });
        //            lineSeries2DForTarget.ArgumentScaleType = ScaleType.Auto;
        //            lineSeries2DForTarget.ValueScaleType = ScaleType.Numerical;
        //            lineSeries2DForTarget.DisplayName = "Target";
        //            lineSeries2DForTarget.CrosshairLabelPattern = "{S} : {V:c2}";
        //            lineSeries2DForTarget.ArgumentDataMember = "Week";
        //            lineSeries2DForTarget.ValueDataMember = "Target";
        //            lineSeries2DForTarget.ShowInLegend = false;
        //            chartControlInventory.Diagram.Series.Add(lineSeries2DForTarget);



        //            LineSeries2D lineSeries2DForRunningMaterial = new LineSeries2D();
        //            //System.Drawing.Color runningMaterialColor = System.Drawing.ColorTranslator.FromHtml(string.IsNullOrEmpty(RunningMaterial.HtmlColor) ? "#f0f0f0" : RunningMaterial.HtmlColor);
        //            //if (string.IsNullOrEmpty(RunningMaterialColor))
        //            //{
        //            //    RunningMaterialColor = "#" + runningMaterialColor.Name;
        //            //}
        //            //lineSeries2DForRunningMaterial.Brush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(runningMaterialColor.R, runningMaterialColor.G, runningMaterialColor.B));
        //            if (!string.IsNullOrEmpty(RunningMaterialColor))
        //            {
        //                System.Drawing.Color runningMaterialColor = System.Drawing.ColorTranslator.FromHtml(RunningMaterial.HtmlColor);
        //                lineSeries2DForRunningMaterial.Brush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(runningMaterialColor.R, runningMaterialColor.G, runningMaterialColor.B));
        //            }
        //            lineSeries2DForRunningMaterial.ArgumentScaleType = ScaleType.Auto;
        //            lineSeries2DForRunningMaterial.ValueScaleType = ScaleType.Numerical;
        //            lineSeries2DForRunningMaterial.DisplayName = "RM";
        //            lineSeries2DForRunningMaterial.ArgumentDataMember = "Week";
        //            lineSeries2DForRunningMaterial.ValueDataMember = "RM";
        //            lineSeries2DForRunningMaterial.CrosshairLabelPattern = "{S} : {V:c2}";
        //            lineSeries2DForRunningMaterial.ShowInLegend = false;
        //            chartControlInventory.Diagram.Series.Add(lineSeries2DForRunningMaterial);

        //            LineSeries2D lineSeries2DForSpecialMaterial = new LineSeries2D();
        //            //System.Drawing.Color specialMaterialColor = System.Drawing.ColorTranslator.FromHtml(string.IsNullOrEmpty(SpecialMaterial.HtmlColor) ? "#c7c4fa" : SpecialMaterial.HtmlColor);
        //            //if (string.IsNullOrEmpty(SpecialMaterialColor))
        //            //{
        //            //    SpecialMaterialColor = "#" + specialMaterialColor.Name;
        //            //}
        //            //lineSeries2DForRunningMaterial.Brush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(specialMaterialColor.R, specialMaterialColor.G, specialMaterialColor.B));
        //            if (!string.IsNullOrEmpty(SpecialMaterialColor))
        //            {
        //                System.Drawing.Color specialMaterialColor = System.Drawing.ColorTranslator.FromHtml(SpecialMaterial.HtmlColor);
        //                lineSeries2DForSpecialMaterial.Brush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(specialMaterialColor.R, specialMaterialColor.G, specialMaterialColor.B));
        //            }
        //            lineSeries2DForSpecialMaterial.ArgumentScaleType = ScaleType.Auto;
        //            lineSeries2DForSpecialMaterial.ValueScaleType = ScaleType.Numerical;
        //            lineSeries2DForSpecialMaterial.DisplayName = "SM";
        //            lineSeries2DForSpecialMaterial.ArgumentDataMember = "Week";
        //            lineSeries2DForSpecialMaterial.ValueDataMember = "SM";
        //            lineSeries2DForSpecialMaterial.CrosshairLabelPattern = "{S} : {V:c2}";
        //            lineSeries2DForSpecialMaterial.ShowInLegend = false;
        //            chartControlInventory.Diagram.Series.Add(lineSeries2DForSpecialMaterial);

        //            LineSeries2D lineSeries2DForSpecialAsset = new LineSeries2D();
        //            //System.Drawing.Color specialAssetColor = System.Drawing.ColorTranslator.FromHtml(string.IsNullOrEmpty(SpecialAsset.HtmlColor) ? "#302d67" : SpecialAsset.HtmlColor);
        //            //if (string.IsNullOrEmpty(SpecialAssetColor))
        //            //{
        //            //     SpecialAssetColor = "#" + specialAssetColor.Name;
        //            //}
        //            //lineSeries2DForRunningMaterial.Brush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(specialAssetColor.R, specialAssetColor.G, specialAssetColor.B));
        //            if (!string.IsNullOrEmpty(SpecialAssetColor))
        //            {
        //                System.Drawing.Color specialAssetColor = System.Drawing.ColorTranslator.FromHtml(SpecialAsset.HtmlColor);
        //                lineSeries2DForSpecialAsset.Brush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(specialAssetColor.R, specialAssetColor.G, specialAssetColor.B));
        //            }
        //            lineSeries2DForSpecialAsset.ArgumentScaleType = ScaleType.Auto;
        //            lineSeries2DForSpecialAsset.ValueScaleType = ScaleType.Numerical;
        //            lineSeries2DForSpecialAsset.DisplayName = "SA";
        //            lineSeries2DForSpecialAsset.ArgumentDataMember = "Week";
        //            lineSeries2DForSpecialAsset.ValueDataMember = "SA";
        //            lineSeries2DForSpecialAsset.CrosshairLabelPattern = "{S} : {V:c2}";
        //            lineSeries2DForSpecialAsset.ShowInLegend = false;
        //            chartControlInventory.Diagram.Series.Add(lineSeries2DForSpecialAsset);


        //            LineSeries2D lineSeries2DForSpecialTooling = new LineSeries2D();
        //            //System.Drawing.Color specialToolingColor = System.Drawing.ColorTranslator.FromHtml(string.IsNullOrEmpty(SpecialTooling.HtmlColor) ? "#9d6a14" : SpecialTooling.HtmlColor);
        //            //if (string.IsNullOrEmpty(SpecialToolingColor))
        //            //{
        //            //    SpecialToolingColor = "#"+ specialToolingColor.Name;
        //            //}
        //            //lineSeries2DForRunningMaterial.Brush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(specialToolingColor.R, specialToolingColor.G, specialToolingColor.B));
        //            if (!string.IsNullOrEmpty(SpecialToolingColor))
        //            {
        //                System.Drawing.Color specialToolingColor = System.Drawing.ColorTranslator.FromHtml(SpecialTooling.HtmlColor);
        //                lineSeries2DForSpecialTooling.Brush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(specialToolingColor.R, specialToolingColor.G, specialToolingColor.B));
        //            }
        //            lineSeries2DForSpecialTooling.ArgumentScaleType = ScaleType.Auto;
        //            lineSeries2DForSpecialTooling.ValueScaleType = ScaleType.Numerical;
        //            lineSeries2DForSpecialTooling.DisplayName = "ST";
        //            lineSeries2DForSpecialTooling.ArgumentDataMember = "Week";
        //            lineSeries2DForSpecialTooling.ValueDataMember = "ST";
        //            lineSeries2DForSpecialTooling.CrosshairLabelPattern = "{S} : {V:c2}";
        //            lineSeries2DForSpecialTooling.ShowInLegend = false;
        //            chartControlInventory.Diagram.Series.Add(lineSeries2DForSpecialTooling);

        //            LineSeries2D lineSeries2DForSpecialRunning = new LineSeries2D();
        //            //System.Drawing.Color specialRunningcolor = System.Drawing.ColorTranslator.FromHtml(string.IsNullOrEmpty(SpecialRunning.HtmlColor) ? "#808080" : SpecialRunning.HtmlColor);
        //            //if (string.IsNullOrEmpty(SpecialRunningColor))
        //            //{
        //            //    SpecialRunningColor = "#" + specialRunningcolor.Name;
        //            //}
        //            //lineSeries2DForRunningMaterial.Brush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(specialRunningcolor.R, specialRunningcolor.G, specialRunningcolor.B));
        //            if (!string.IsNullOrEmpty(SpecialRunningColor))
        //            {
        //                System.Drawing.Color specialRunningcolor = System.Drawing.ColorTranslator.FromHtml(SpecialRunning.HtmlColor);
        //                lineSeries2DForSpecialRunning.Brush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(specialRunningcolor.R, specialRunningcolor.G, specialRunningcolor.B));
        //            }
        //            lineSeries2DForSpecialRunning.ArgumentScaleType = ScaleType.Auto;
        //            lineSeries2DForSpecialRunning.ValueScaleType = ScaleType.Numerical;
        //            lineSeries2DForSpecialRunning.DisplayName = "SR";
        //            lineSeries2DForSpecialRunning.ArgumentDataMember = "Week";
        //            lineSeries2DForSpecialRunning.ValueDataMember = "SR";
        //            lineSeries2DForSpecialRunning.CrosshairLabelPattern = "{S} : {V:c2}";
        //            lineSeries2DForSpecialRunning.ShowInLegend = false;
        //            chartControlInventory.Diagram.Series.Add(lineSeries2DForSpecialRunning);


        //            //  LineSeries2D lineSeries2DForTarget = new LineSeries2D();
        //            #region Color
        //            //lineSeries2DForTarget.Brush.Color = System.Windows.Media.Color.FromRgb(252, 3, 3);
        //            //lineSeries2DForTarget.Brush.Color = new SolidColorBrush(Colors.Red);
        //            //lineSeries2DForTarget.Style = new Style(typeof(LineSeries2D));
        //            //lineSeries2DForTarget.Style.Setters.Add(new Setter(LineSeries2D.LineStyleProperty, new SolidColorBrush(Colors.Red)));
        //            // Set the line color
        //            //lineSeries2DForTarget.SetCurrentValue(LineSeries2D.LineStyleProperty, new LineStyle() {  LineBrush = new SolidColorBrush(Colors.Red) });
        //            // Create a custom style for the line series
        //            //Style lineStyle = new Style(typeof(LineSeries2D));
        //            //lineStyle.Setters.Add(new Setter(LineSeries2D.LineStyleProperty, new SolidColorBrush(Colors.Red)));
        //            //// Apply the custom style to the line series
        //            //lineSeries2DForTarget.Style = lineStyle;
        //            //lineSeries2DForTarget.BorderBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(4, 127, 73)); // Set the line color
        //            //lineSeries2DForTarget.BorderThickness = new Thickness(2); // Set the line thickness
        //            //lineSeries2DForTarget.ColorDataMember="#047f49";
        //            //lineSeries2DForTarget.Brush = chartControlInventory.ChartControl.ActualPalette[chartControlInventory.ChartControl.Diagram.Series.IndexOf(series)];
        //            #endregion
        //            // Customize the appearance of the line series
        //            //System.Drawing.Color red = System.Drawing.ColorTranslator.FromHtml("#047f4a");
        //            //lineSeries2DForTarget.Brush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(4, 127, 73));
        //            //lineSeries2DForTarget.ArgumentScaleType = ScaleType.Auto;
        //            //lineSeries2DForTarget.ValueScaleType = ScaleType.Numerical;
        //            //lineSeries2DForTarget.DisplayName = "Target";
        //            //lineSeries2DForTarget.ArgumentDataMember = "Week";
        //            //lineSeries2DForTarget.ValueDataMember = "Target";
        //            //lineSeries2DForTarget.CrosshairLabelPattern = "{S} : {V:c2}";
        //            //lineSeries2DForTarget.ShowInLegend = false;
        //            //chartControlInventory.Diagram.Series.Add(lineSeries2DForTarget);
        //        }
        //        catch (Exception ex)
        //        {

        //        }
        //        chartControlInventory.EndInit();
        //        chartControlInventory.AnimationMode = ChartAnimationMode.OnDataChanged;
        //        chartControlInventory.Animate();

        //        GeosApplication.Instance.Logger.Log("Method ChartLoadInventoryCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (FaultException<ServiceException> ex)
        //    {
        //        if (DXSplashScreen.IsActive)
        //        {
        //            DXSplashScreen.Close();
        //        }
        //        GeosApplication.Instance.Logger.Log("Get an error in ChartLoadInventoryCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
        //        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
        //    }
        //    catch (ServiceUnexceptedException ex)
        //    {
        //        if (DXSplashScreen.IsActive)
        //        {
        //            DXSplashScreen.Close();
        //        }
        //        GeosApplication.Instance.Logger.Log("Get an error in ChartLoadInventoryCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
        //        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in ChartLoadInventoryCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
        //    }
        //}

        #endregion

        //[pramod.misal][01.11.2023][GEOS2-4853]
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


                AreaSeries2D areaSeries2D = new AreaSeries2D();
                System.Drawing.Color ColorAmount = System.Drawing.ColorTranslator.FromHtml(string.IsNullOrEmpty(Color) ? Color : Color);
                areaSeries2D.Brush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(128, ColorAmount.R, ColorAmount.G, ColorAmount.B));
                areaSeries2D.ArgumentScaleType = ScaleType.Auto;
                areaSeries2D.ValueScaleType = ScaleType.Numerical;
                areaSeries2D.DisplayName = "Stock";
                areaSeries2D.ArgumentDataMember = "Week";
                areaSeries2D.ValueDataMember = "Stock";
                areaSeries2D.CrosshairLabelPattern = "{S} : {V:c2}";
                areaSeries2D.ShowInLegend = true;//Shubham[skadam] GEOS2-4852 set a legend for the inventory graph 11 01 2024
                diagram.Series.Add(areaSeries2D);
                areaSeries2D.Transparency = 0.5;
                areaSeries2D.MarkerVisible = true;

                try
                {

                    LineSeries2D lineSeries2DForTarget = new LineSeries2D();
                    lineSeries2DForTarget.LineStyle = new LineStyle();
                    lineSeries2DForTarget.LineStyle.DashStyle = new DashStyle();
                    lineSeries2DForTarget.LineStyle.Thickness = 2;
                    lineSeries2DForTarget.LineStyle.DashStyle.Dashes = new DoubleCollection(new double[] { 3, 3, 3, 3 });
                    lineSeries2DForTarget.ArgumentScaleType = ScaleType.Auto;
                    lineSeries2DForTarget.ValueScaleType = ScaleType.Numerical;
                    lineSeries2DForTarget.DisplayName = "Target";
                    lineSeries2DForTarget.CrosshairLabelPattern = "{S} : {V:c2}";
                    lineSeries2DForTarget.ArgumentDataMember = "Week";
                    lineSeries2DForTarget.ValueDataMember = "Target";
                    lineSeries2DForTarget.ShowInLegend = true;//Shubham[skadam] GEOS2-4852 set a legend for the inventory graph 11 01 2024
                    chartControlInventory.Diagram.Series.Add(lineSeries2DForTarget);

                    #region Area for Target GEOS2-4853
                    //AreaSeries2D areaSeriesForTarget = new AreaSeries2D();
                    //areaSeriesForTarget.Brush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(128, ColorAmount.R, ColorAmount.G, ColorAmount.B));
                    //areaSeriesForTarget.ArgumentScaleType = ScaleType.Auto;
                    //areaSeriesForTarget.ValueScaleType = ScaleType.Numerical;
                    //areaSeriesForTarget.DisplayName = "Target";
                    //areaSeriesForTarget.ArgumentDataMember = "Week";
                    //areaSeriesForTarget.ValueDataMember = "Target";
                    //areaSeriesForTarget.CrosshairLabelPattern = "{S} : {V:c2}";
                    //areaSeriesForTarget.ShowInLegend = false;
                    //lineSeries2DForTarget.Transparency = 0.5;
                    //chartControlInventory.Diagram.Series.Add(areaSeriesForTarget);

                    #endregion

                    AreaSeries2D areaSeriesForRunningMaterial = new AreaSeries2D();

                    if (!string.IsNullOrEmpty(RunningMaterialColor))
                    {
                        System.Drawing.Color ColorAmountRM = System.Drawing.ColorTranslator.FromHtml(string.IsNullOrEmpty(RunningMaterial.HtmlColor) ? RunningMaterial.HtmlColor : RunningMaterial.HtmlColor);
                        areaSeriesForRunningMaterial.Brush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(128, ColorAmountRM.R, ColorAmountRM.G, ColorAmountRM.B));

                    }
                    areaSeriesForRunningMaterial.ArgumentScaleType = ScaleType.Auto;
                    areaSeriesForRunningMaterial.ValueScaleType = ScaleType.Numerical;
                    areaSeriesForRunningMaterial.DisplayName = "RM";
                    areaSeriesForRunningMaterial.ArgumentDataMember = "Week";
                    areaSeriesForRunningMaterial.ValueDataMember = "RM";
                    areaSeriesForRunningMaterial.CrosshairLabelPattern = "{S} : {V:c2}";
                    areaSeriesForRunningMaterial.ShowInLegend = true;//Shubham[skadam] GEOS2-4852 set a legend for the inventory graph 11 01 2024
                    //areaSeriesForRunningMaterial.Transparency = 0.5;
                    areaSeriesForRunningMaterial.Transparency = 0.5;
                    areaSeriesForRunningMaterial.MarkerVisible = true;
                    chartControlInventory.Diagram.Series.Add(areaSeriesForRunningMaterial);

                    AreaSeries2D areaSeries2DForSpecialMaterial = new AreaSeries2D();

                    if (!string.IsNullOrEmpty(SpecialMaterialColor))
                    {
                        System.Drawing.Color ColorAmountSM = System.Drawing.ColorTranslator.FromHtml(string.IsNullOrEmpty(SpecialMaterial.HtmlColor) ? SpecialMaterial.HtmlColor : SpecialMaterial.HtmlColor);
                        areaSeries2DForSpecialMaterial.Brush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(128, ColorAmountSM.R, ColorAmountSM.G, ColorAmountSM.B));
                    }

                    areaSeries2DForSpecialMaterial.ArgumentScaleType = ScaleType.Auto;
                    areaSeries2DForSpecialMaterial.ValueScaleType = ScaleType.Numerical;
                    areaSeries2DForSpecialMaterial.DisplayName = "SM";
                    areaSeries2DForSpecialMaterial.ArgumentDataMember = "Week";
                    areaSeries2DForSpecialMaterial.ValueDataMember = "SM";
                    areaSeries2DForSpecialMaterial.CrosshairLabelPattern = "{S} : {V:c2}";
                    areaSeries2DForSpecialMaterial.ShowInLegend = true;//Shubham[skadam] GEOS2-4852 set a legend for the inventory graph 11 01 2024
                    //areaSeries2DForSpecialMaterial.Transparency = 0.7;
                    areaSeries2DForSpecialMaterial.Transparency = 0.5;
                    areaSeries2DForSpecialMaterial.MarkerVisible = true;
                    chartControlInventory.Diagram.Series.Add(areaSeries2DForSpecialMaterial);

                    AreaSeries2D areaSeries2DForSpecialAsset = new AreaSeries2D();

                    if (!string.IsNullOrEmpty(SpecialAssetColor))
                    {

                        System.Drawing.Color ColorAmountSA = System.Drawing.ColorTranslator.FromHtml(string.IsNullOrEmpty(SpecialAsset.HtmlColor) ? SpecialAsset.HtmlColor : SpecialAsset.HtmlColor);
                        areaSeries2DForSpecialAsset.Brush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(128, ColorAmountSA.R, ColorAmountSA.G, ColorAmountSA.B));
                    }


                    areaSeries2DForSpecialAsset.ArgumentScaleType = ScaleType.Auto;
                    areaSeries2DForSpecialAsset.ValueScaleType = ScaleType.Numerical;
                    areaSeries2DForSpecialAsset.DisplayName = "SA";
                    areaSeries2DForSpecialAsset.ArgumentDataMember = "Week";
                    areaSeries2DForSpecialAsset.ValueDataMember = "SA";
                    areaSeries2DForSpecialAsset.CrosshairLabelPattern = "{S} : {V:c2}";
                    areaSeries2DForSpecialAsset.ShowInLegend = true;//Shubham[skadam] GEOS2-4852 set a legend for the inventory graph 11 01 2024
                    //areaSeries2DForSpecialAsset.Transparency = 0.8;
                    areaSeries2DForSpecialAsset.Transparency = 0.5;
                    areaSeries2DForSpecialAsset.MarkerVisible = true;
                    chartControlInventory.Diagram.Series.Add(areaSeries2DForSpecialAsset);



                    AreaSeries2D areaSeries2DForSpecialTooling = new AreaSeries2D();

                    if (!string.IsNullOrEmpty(SpecialToolingColor))
                    {
                        System.Drawing.Color ColorAmountST = System.Drawing.ColorTranslator.FromHtml(string.IsNullOrEmpty(SpecialTooling.HtmlColor) ? SpecialTooling.HtmlColor : SpecialTooling.HtmlColor);
                        areaSeries2DForSpecialTooling.Brush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(128, ColorAmountST.R, ColorAmountST.G, ColorAmountST.B));
                    }

                    areaSeries2DForSpecialTooling.ArgumentScaleType = ScaleType.Auto;
                    areaSeries2DForSpecialTooling.ValueScaleType = ScaleType.Numerical;
                    areaSeries2DForSpecialTooling.DisplayName = "ST";
                    areaSeries2DForSpecialTooling.ArgumentDataMember = "Week";
                    areaSeries2DForSpecialTooling.ValueDataMember = "ST";
                    areaSeries2DForSpecialTooling.CrosshairLabelPattern = "{S} : {V:c2}";
                    areaSeries2DForSpecialTooling.ShowInLegend = true;//Shubham[skadam] GEOS2-4852 set a legend for the inventory graph 11 01 2024
                    areaSeries2DForSpecialTooling.Transparency = 0.5;
                    areaSeries2DForSpecialTooling.MarkerVisible = true;
                    chartControlInventory.Diagram.Series.Add(areaSeries2DForSpecialTooling);

                    AreaSeries2D areaSeries2DForSpecialRunning = new AreaSeries2D();

                    if (!string.IsNullOrEmpty(SpecialRunningColor))
                    {
                        System.Drawing.Color ColorAmountSR = System.Drawing.ColorTranslator.FromHtml(string.IsNullOrEmpty(SpecialRunning.HtmlColor) ? SpecialRunning.HtmlColor : SpecialRunning.HtmlColor);
                        areaSeries2DForSpecialRunning.Brush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(128, ColorAmountSR.R, ColorAmountSR.G, ColorAmountSR.B));
                    }

                    areaSeries2DForSpecialRunning.ArgumentScaleType = ScaleType.Auto;
                    areaSeries2DForSpecialRunning.ValueScaleType = ScaleType.Numerical;
                    areaSeries2DForSpecialRunning.DisplayName = "SR";
                    areaSeries2DForSpecialRunning.ArgumentDataMember = "Week";
                    areaSeries2DForSpecialRunning.ValueDataMember = "SR";
                    areaSeries2DForSpecialRunning.CrosshairLabelPattern = "{S} : {V:c2}";
                    areaSeries2DForSpecialRunning.ShowInLegend = true;//Shubham[skadam] GEOS2-4852 set a legend for the inventory graph 11 01 2024
                    //areaSeries2DForSpecialRunning.Transparency = 0.8;
                    areaSeries2DForSpecialRunning.Transparency = 0.5;
                    areaSeries2DForSpecialRunning.MarkerVisible = true;
                    chartControlInventory.Diagram.Series.Add(areaSeries2DForSpecialRunning);

                    AreaSeries2D areaSeries2DForSoftwareRunning = new AreaSeries2D();//[Sudhir.Jangra][GEOS2-5471]

                    if (!string.IsNullOrEmpty(SoftwareRunningColor))
                    {
                        System.Drawing.Color ColorAmountSF = System.Drawing.ColorTranslator.FromHtml(string.IsNullOrEmpty(SoftwareRunning.HtmlColor) ? SoftwareRunning.HtmlColor : SoftwareRunning.HtmlColor);
                        areaSeries2DForSoftwareRunning.Brush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(128, ColorAmountSF.R, ColorAmountSF.G, ColorAmountSF.B));
                    }

                    areaSeries2DForSoftwareRunning.ArgumentScaleType = ScaleType.Auto;
                    areaSeries2DForSoftwareRunning.ValueScaleType = ScaleType.Numerical;
                    areaSeries2DForSoftwareRunning.DisplayName = "SF";
                    areaSeries2DForSoftwareRunning.ArgumentDataMember = "Week";
                    areaSeries2DForSoftwareRunning.ValueDataMember = "SF";
                    areaSeries2DForSoftwareRunning.CrosshairLabelPattern = "{S} : {V:c2}";
                    areaSeries2DForSoftwareRunning.ShowInLegend = true;
                    areaSeries2DForSoftwareRunning.Transparency = 0.5;
                    areaSeries2DForSoftwareRunning.MarkerVisible = true;
                    chartControlInventory.Diagram.Series.Add(areaSeries2DForSoftwareRunning);



                }
                catch (Exception ex)
                {

                }
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
                        //Shubham[skadam] GEOS2-4852 set a legend for the inventory graph 11 01 2024
                        //item.Series.DisplayName = item.SeriesPoint.ActualArgument;
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
                Value = Math.Floor(amount / 1000).ToString() + "K";
            else
                Value = Math.Round(amount, 2).ToString();

            return Value;
        }
        private void SetWonAmount()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SetWonAmount ...", category: Category.Info, priority: Priority.Low);

                WonAmount = Math.Round(WmsService.GetWONOfferAmount(WarehouseCommon.Instance.Selectedwarehouse, SelectedCurrency.IdCurrency, Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate)), 2);
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

                //StockAmount = Math.Round(WmsService.GetArticleStockAmountInWarehouse(WarehouseCommon.Instance.Selectedwarehouse, SelectedCurrency.IdCurrency, Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate)), 2);
                //Shubham[skadam] GEOS2-4227 New Inventory Dashboard 08 08 2023 
                StockAmount = Math.Round(WmsService.GetArticleStockAmountInWarehouse_V2420(WarehouseCommon.Instance.Selectedwarehouse, SelectedCurrency.IdCurrency, Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate)), 2);
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

                ObsoleteAmount = Math.Round(WmsService.GetAbosleteArticleStockAmountInWarehouse(WarehouseCommon.Instance.Selectedwarehouse, SelectedCurrency.IdCurrency, Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate)), 2);
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

                //SleepingAmount = Math.Round(WmsService.GetSleepedArticleStockAmountInWarehouse(WarehouseCommon.Instance.Selectedwarehouse, SelectedCurrency.IdCurrency, Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate), WarehouseCommon.Instance.ArticleSleepDays), 2);
                //Shubham[skadam] GEOS2-4227 New Inventory Dashboard 08 08 2023 
                SleepingAmount = Math.Round(WmsService.GetSleepedArticleStockAmountInWarehouse_V2420(WarehouseCommon.Instance.Selectedwarehouse, SelectedCurrency.IdCurrency, Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate), WarehouseCommon.Instance.ArticleSleepDays), 2);
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

        private void SetArticleMaterialType()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SetSleepingAmount ...", category: Category.Info, priority: Priority.Low);
                MaterialType = new List<ArticleMaterialType>();
                //WmsService = new WarehouseServiceController("localhost:6699");
                MaterialType = WmsService.GetArticleMaterialTypeStockAmountInWarehouse_V2390(WarehouseCommon.Instance.Selectedwarehouse, SelectedCurrency.IdCurrency, Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate));

                foreach (ArticleMaterialType MaterialTypeitem in MaterialType)
                {
                    if (MaterialTypeitem.MaterialTypeId == 226)
                    {
                        RunningMaterial = new ArticleMaterialType();
                        RunningMaterial = MaterialTypeitem;
                        RunningMaterialColor = MaterialTypeitem.HtmlColor;
                    }
                    else if (MaterialTypeitem.MaterialTypeId == 227)
                    {
                        SpecialMaterial = new ArticleMaterialType();
                        SpecialMaterial = MaterialTypeitem;
                        SpecialMaterialColor = MaterialTypeitem.HtmlColor;
                    }
                    else if (MaterialTypeitem.MaterialTypeId == 228)
                    {
                        SpecialAsset = new ArticleMaterialType();
                        SpecialAsset = MaterialTypeitem;
                        SpecialAssetColor = MaterialTypeitem.HtmlColor;
                    }
                    else if (MaterialTypeitem.MaterialTypeId == 229)
                    {
                        SpecialTooling = new ArticleMaterialType();
                        SpecialTooling = MaterialTypeitem;
                        SpecialToolingColor = MaterialTypeitem.HtmlColor;
                    }
                    else if (MaterialTypeitem.MaterialTypeId == 1526)
                    {
                        SpecialRunning = new ArticleMaterialType();
                        SpecialRunning = MaterialTypeitem;
                        SpecialRunningColor = MaterialTypeitem.HtmlColor;
                    }
                    else if (MaterialTypeitem.MaterialTypeId == 1880)//[Sudhir.Jangra][GEOS2-5471]
                    {
                        SoftwareRunning = new ArticleMaterialType();
                        SoftwareRunning = MaterialTypeitem;
                        SoftwareRunningColor = MaterialTypeitem.HtmlColor;
                    }
                    else
                    {
                        WarehouseStockTargets = new ArticleMaterialType();
                        WarehouseStockTargets = MaterialTypeitem;
                        WarehouseStockTargetsColor = MaterialTypeitem.HtmlColor;
                    }
                }
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
                if (chartControl != null)
                {
                    chartControl.Diagram = null;
                }


                SetWonAmount();
                SetStockAmount();
                SetObsoleteAmount();
                SetSleepingAmount();
                if (IsButtonStatus == 3)
                {
                    //GetSalesStatusMonthListAndCreateTable();
                    //CreateSeriesPoints();
                }
                else
                {
                    //chartControl.Diagram = diagram;
                    //CreateTable();
                }

                CreateTableCustomer();
                SetArticleMaterialType();
                CreateTableInventory();
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
            //DateTime StartFromDate = new DateTime(2019, 1, 1);
            //DateTime EndToDate = new DateTime(2019, 1, 31);

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
                SetArticleMaterialType();
                //if (SalesStatusByMonthList.Count > 13)
                //{
                //    GetSalesStatusMonthListAndCreateTable();
                //}
                //else
                //{
                //    CreateTable();
                //}
                //CreateTableCustomer();
                CreateTableInventory();
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
                SetArticleMaterialType();

                //if (SalesStatusByMonthList.Count > 13)
                //{
                //    GetSalesStatusMonthListAndCreateTable();
                //}
                //else
                //{
                //    CreateTable();
                //}
                //CreateTableCustomer();

                CreateTableInventory();

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

        private void CreateTable()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CreateTable ...", category: Category.Info, priority: Priority.Low);
                //SalesStatusByMonthList = WmsService.GetSalesByMonth(WarehouseCommon.Instance.Selectedwarehouse, SelectedCurrency.IdCurrency, Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate));

                //dt = new DataTable();
                //dt.Columns.Add("Month");
                //dt.Columns.Add("Year");
                //dt.Columns.Add("MonthYear");

                //dt.Columns.Add("WON", typeof(double));


                //dt.Rows.Clear();
                //int[] icol = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

                //foreach (var mt in icol)
                //{
                //    DataRow dr = dt.NewRow();
                //    dr[0] = null;
                //    dr[1] = GetYearNumber(FromDate);
                //    dr[2] = GetFullMonthName(mt, GetYearNumber(FromDate));
                //    int k = 3;
                //    foreach (var item in dt.Columns)
                //    {
                //        if (item.ToString() != "Month" && item.ToString() != "Year" && item.ToString() != "MonthYear")
                //        {
                //            dr[k] = SalesStatusByMonthList.Where(m => m.CurrentMonth == mt).Select(mv => mv.Value).ToList().Sum();
                //            k++;
                //        }
                //    }
                //    dt.Rows.Add(dr);
                //}
                GeosApplication.Instance.Logger.Log("Method CreateTable() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CreateTable() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

            //GraphDataTable = dt;
            //if (chartControl != null) { chartControl.UpdateData(); }
        }
        private void CreateTableCustomer()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CreateTableCustomer ...", category: Category.Info, priority: Priority.Low);
                //SalesStatusByCustomerList = WmsService.GetSalesByCustomer(WarehouseCommon.Instance.Selectedwarehouse, SelectedCurrency.IdCurrency, Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate));

                //dt = new DataTable();
                //List<string> list = SalesStatusByCustomerList.Where(x => x.Alias != "" && x.Alias != null).Select(a => a.Alias.ToString()).Distinct().ToList();

                //dt.Columns.Add("Alias");
                //dt.Columns.Add("Amount", typeof(double));

                //dt.Rows.Clear();
                //int k = 1;
                //foreach (string alias in list)
                //{
                //    foreach (var item in dt.Columns)
                //    {
                //        if (item.ToString() != "Alias")
                //        {
                //            DataRow dr = dt.NewRow();
                //            dr[0] = alias;
                //            dr[k] = SalesStatusByCustomerList.Where(m => m.Alias == alias).Select(mv => mv.Amount).ToList().Sum();
                //            dt.Rows.Add(dr);
                //        }
                //    }
                //}
                GeosApplication.Instance.Logger.Log("Method CreateTableCustomer() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CreateTableCustomer() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
            //GraphDataTableCustomer = dt;
            //if (chartControlCustomer != null) { chartControlCustomer.UpdateData(); }
        }
        private void GetSalesStatusMonthListAndCreateTable()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GetSalesStatusMonthListAndCreateTable ...", category: Category.Info, priority: Priority.Low);
                //[001] Add Service Method GetSalesByMonth
                //SalesStatusByMonthList = WmsService.GetSalesByMonth(WarehouseCommon.Instance.Selectedwarehouse, SelectedCurrency.IdCurrency, Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate));

                //dt = new DataTable();
                //dt.Columns.Add("Month");
                //dt.Columns.Add("Year");
                //dt.Columns.Add("MonthYear");
                //dt.Columns.Add("WON", typeof(double));
                //dt.Rows.Clear();

                //DataRow dr = null;
                //years = new List<int>();
                //months = new List<int>();
                //ListYearMonths = new List<YearMonth>();

                //int fromDateMonth = GetMonthNumber(FromDate);
                //int fromDateYear = GetYearNumber(FromDate);
                //int toDateMonth = GetMonthNumber(ToDate);
                //int toDateYear = GetYearNumber(ToDate);

                ////Get unselected months and set values as zero
                //StartMonthEmptyValue(fromDateMonth, fromDateYear, dr);
                //foreach (var eachList in SalesStatusByMonthList)
                //{
                //    dr = dt.NewRow();
                //    dr[0] = null;
                //    dr[1] = eachList.CurrentYear;
                //    int column = 3;
                //    if (!years.Contains(eachList.CurrentYear))
                //    {
                //        years.Add(eachList.CurrentYear);
                //    }
                //    if (!months.Contains(eachList.CurrentMonth))
                //    {
                //        months.Add(eachList.CurrentMonth);
                //    }
                //    dr[2] = GetFullMonthName(eachList.CurrentMonth, eachList.CurrentYear);
                //    foreach (var item in dt.Columns)
                //    {
                //        if (item.ToString() != "Month" && item.ToString() != "Year" && item.ToString() != "MonthYear")
                //        {
                //            dr[column] = eachList.Value;
                //            yearMonth = new YearMonth()
                //            {
                //                Month = eachList.CurrentMonth,
                //                Year = eachList.CurrentYear,
                //                Value = eachList.Value
                //            };
                //        }
                //    }
                //    dt.Rows.Add(dr);
                //    ListYearMonths.Add(yearMonth);
                //}
                //// Get unselected months and set values as zero
                //EndMonthEmptyValue(toDateMonth, toDateYear, dr);

                //GraphDataTable = dt;
                //if (chartControl != null) { chartControl.UpdateData(); }
                GeosApplication.Instance.Logger.Log("Method GetSalesStatusMonthListAndCreateTable() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetSalesStatusMonthListAndCreateTable() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
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

        //[pramod.misal][GEOS2-5689][14.08.2024]
        private void LineGraphViewAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method LineGraphViewAction ...", category: Category.Info, priority: Priority.Low);

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


                AreaSeries2D areaSeries2D = new AreaSeries2D();
                System.Drawing.Color ColorAmount = System.Drawing.ColorTranslator.FromHtml(string.IsNullOrEmpty(Color) ? Color : Color);
                areaSeries2D.Brush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(128, ColorAmount.R, ColorAmount.G, ColorAmount.B));
                areaSeries2D.ArgumentScaleType = ScaleType.Auto;
                areaSeries2D.ValueScaleType = ScaleType.Numerical;
                areaSeries2D.DisplayName = "Stock";
                areaSeries2D.ArgumentDataMember = "Week";
                areaSeries2D.ValueDataMember = "Stock";
                areaSeries2D.CrosshairLabelPattern = "{S} : {V:c2}";
                areaSeries2D.ShowInLegend = true;//Shubham[skadam] GEOS2-4852 set a legend for the inventory graph 11 01 2024
                diagram.Series.Add(areaSeries2D);
                areaSeries2D.Transparency = 0.5;
                areaSeries2D.MarkerVisible = true;

                try
                {

                    LineSeries2D lineSeries2DForTarget = new LineSeries2D();
                    lineSeries2DForTarget.LineStyle = new LineStyle();
                    lineSeries2DForTarget.LineStyle.DashStyle = new DashStyle();
                    lineSeries2DForTarget.LineStyle.Thickness = 2;
                    lineSeries2DForTarget.LineStyle.DashStyle.Dashes = new DoubleCollection(new double[] { 3, 3, 3, 3 });
                    lineSeries2DForTarget.ArgumentScaleType = ScaleType.Auto;
                    lineSeries2DForTarget.ValueScaleType = ScaleType.Numerical;
                    lineSeries2DForTarget.DisplayName = "Target";
                    lineSeries2DForTarget.CrosshairLabelPattern = "{S} : {V:c2}";
                    lineSeries2DForTarget.ArgumentDataMember = "Week";
                    lineSeries2DForTarget.ValueDataMember = "Target";
                    lineSeries2DForTarget.ShowInLegend = true;//Shubham[skadam] GEOS2-4852 set a legend for the inventory graph 11 01 2024
                    chartControlInventory.Diagram.Series.Add(lineSeries2DForTarget);

                    #region Area for Target GEOS2-4853
                    //AreaSeries2D areaSeriesForTarget = new AreaSeries2D();
                    //areaSeriesForTarget.Brush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(128, ColorAmount.R, ColorAmount.G, ColorAmount.B));
                    //areaSeriesForTarget.ArgumentScaleType = ScaleType.Auto;
                    //areaSeriesForTarget.ValueScaleType = ScaleType.Numerical;
                    //areaSeriesForTarget.DisplayName = "Target";
                    //areaSeriesForTarget.ArgumentDataMember = "Week";
                    //areaSeriesForTarget.ValueDataMember = "Target";
                    //areaSeriesForTarget.CrosshairLabelPattern = "{S} : {V:c2}";
                    //areaSeriesForTarget.ShowInLegend = false;
                    //lineSeries2DForTarget.Transparency = 0.5;
                    //chartControlInventory.Diagram.Series.Add(areaSeriesForTarget);

                    #endregion

                    AreaSeries2D areaSeriesForRunningMaterial = new AreaSeries2D();

                    if (!string.IsNullOrEmpty(RunningMaterialColor))
                    {
                        System.Drawing.Color ColorAmountRM = System.Drawing.ColorTranslator.FromHtml(string.IsNullOrEmpty(RunningMaterial.HtmlColor) ? RunningMaterial.HtmlColor : RunningMaterial.HtmlColor);
                        areaSeriesForRunningMaterial.Brush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(128, ColorAmountRM.R, ColorAmountRM.G, ColorAmountRM.B));

                    }
                    areaSeriesForRunningMaterial.ArgumentScaleType = ScaleType.Auto;
                    areaSeriesForRunningMaterial.ValueScaleType = ScaleType.Numerical;
                    areaSeriesForRunningMaterial.DisplayName = "RM";
                    areaSeriesForRunningMaterial.ArgumentDataMember = "Week";
                    areaSeriesForRunningMaterial.ValueDataMember = "RM";
                    areaSeriesForRunningMaterial.CrosshairLabelPattern = "{S} : {V:c2}";
                    areaSeriesForRunningMaterial.ShowInLegend = true;
                    //areaSeriesForRunningMaterial.Transparency = 0.5;
                    areaSeriesForRunningMaterial.Transparency = 0.5;
                    areaSeriesForRunningMaterial.MarkerVisible = true;
                    chartControlInventory.Diagram.Series.Add(areaSeriesForRunningMaterial);

                    AreaSeries2D areaSeries2DForSpecialMaterial = new AreaSeries2D();

                    if (!string.IsNullOrEmpty(SpecialMaterialColor))
                    {
                        System.Drawing.Color ColorAmountSM = System.Drawing.ColorTranslator.FromHtml(string.IsNullOrEmpty(SpecialMaterial.HtmlColor) ? SpecialMaterial.HtmlColor : SpecialMaterial.HtmlColor);
                        areaSeries2DForSpecialMaterial.Brush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(128, ColorAmountSM.R, ColorAmountSM.G, ColorAmountSM.B));
                    }

                    areaSeries2DForSpecialMaterial.ArgumentScaleType = ScaleType.Auto;
                    areaSeries2DForSpecialMaterial.ValueScaleType = ScaleType.Numerical;
                    areaSeries2DForSpecialMaterial.DisplayName = "SM";
                    areaSeries2DForSpecialMaterial.ArgumentDataMember = "Week";
                    areaSeries2DForSpecialMaterial.ValueDataMember = "SM";
                    areaSeries2DForSpecialMaterial.CrosshairLabelPattern = "{S} : {V:c2}";
                    areaSeries2DForSpecialMaterial.ShowInLegend = true;
                    //areaSeries2DForSpecialMaterial.Transparency = 0.7;
                    areaSeries2DForSpecialMaterial.Transparency = 0.5;
                    areaSeries2DForSpecialMaterial.MarkerVisible = true;
                    chartControlInventory.Diagram.Series.Add(areaSeries2DForSpecialMaterial);

                    AreaSeries2D areaSeries2DForSpecialAsset = new AreaSeries2D();

                    if (!string.IsNullOrEmpty(SpecialAssetColor))
                    {

                        System.Drawing.Color ColorAmountSA = System.Drawing.ColorTranslator.FromHtml(string.IsNullOrEmpty(SpecialAsset.HtmlColor) ? SpecialAsset.HtmlColor : SpecialAsset.HtmlColor);
                        areaSeries2DForSpecialAsset.Brush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(128, ColorAmountSA.R, ColorAmountSA.G, ColorAmountSA.B));
                    }


                    areaSeries2DForSpecialAsset.ArgumentScaleType = ScaleType.Auto;
                    areaSeries2DForSpecialAsset.ValueScaleType = ScaleType.Numerical;
                    areaSeries2DForSpecialAsset.DisplayName = "SA";
                    areaSeries2DForSpecialAsset.ArgumentDataMember = "Week";
                    areaSeries2DForSpecialAsset.ValueDataMember = "SA";
                    areaSeries2DForSpecialAsset.CrosshairLabelPattern = "{S} : {V:c2}";
                    areaSeries2DForSpecialAsset.ShowInLegend = true;
                    //areaSeries2DForSpecialAsset.Transparency = 0.8;
                    areaSeries2DForSpecialAsset.Transparency = 0.5;
                    areaSeries2DForSpecialAsset.MarkerVisible = true;
                    chartControlInventory.Diagram.Series.Add(areaSeries2DForSpecialAsset);



                    AreaSeries2D areaSeries2DForSpecialTooling = new AreaSeries2D();

                    if (!string.IsNullOrEmpty(SpecialToolingColor))
                    {
                        System.Drawing.Color ColorAmountST = System.Drawing.ColorTranslator.FromHtml(string.IsNullOrEmpty(SpecialTooling.HtmlColor) ? SpecialTooling.HtmlColor : SpecialTooling.HtmlColor);
                        areaSeries2DForSpecialTooling.Brush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(128, ColorAmountST.R, ColorAmountST.G, ColorAmountST.B));
                    }

                    areaSeries2DForSpecialTooling.ArgumentScaleType = ScaleType.Auto;
                    areaSeries2DForSpecialTooling.ValueScaleType = ScaleType.Numerical;
                    areaSeries2DForSpecialTooling.DisplayName = "ST";
                    areaSeries2DForSpecialTooling.ArgumentDataMember = "Week";
                    areaSeries2DForSpecialTooling.ValueDataMember = "ST";
                    areaSeries2DForSpecialTooling.CrosshairLabelPattern = "{S} : {V:c2}";
                    areaSeries2DForSpecialTooling.ShowInLegend = true;
                    areaSeries2DForSpecialTooling.Transparency = 0.5;
                    areaSeries2DForSpecialTooling.MarkerVisible = true;
                    chartControlInventory.Diagram.Series.Add(areaSeries2DForSpecialTooling);

                    AreaSeries2D areaSeries2DForSpecialRunning = new AreaSeries2D();

                    if (!string.IsNullOrEmpty(SpecialRunningColor))
                    {
                        System.Drawing.Color ColorAmountSR = System.Drawing.ColorTranslator.FromHtml(string.IsNullOrEmpty(SpecialRunning.HtmlColor) ? SpecialRunning.HtmlColor : SpecialRunning.HtmlColor);
                        areaSeries2DForSpecialRunning.Brush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(128, ColorAmountSR.R, ColorAmountSR.G, ColorAmountSR.B));
                    }

                    areaSeries2DForSpecialRunning.ArgumentScaleType = ScaleType.Auto;
                    areaSeries2DForSpecialRunning.ValueScaleType = ScaleType.Numerical;
                    areaSeries2DForSpecialRunning.DisplayName = "SR";
                    areaSeries2DForSpecialRunning.ArgumentDataMember = "Week";
                    areaSeries2DForSpecialRunning.ValueDataMember = "SR";
                    areaSeries2DForSpecialRunning.CrosshairLabelPattern = "{S} : {V:c2}";
                    areaSeries2DForSpecialRunning.ShowInLegend = true;
                    //areaSeries2DForSpecialRunning.Transparency = 0.8;
                    areaSeries2DForSpecialRunning.Transparency = 0.5;
                    areaSeries2DForSpecialRunning.MarkerVisible = true;
                    chartControlInventory.Diagram.Series.Add(areaSeries2DForSpecialRunning);

                    AreaSeries2D areaSeries2DForSoftwareRunning = new AreaSeries2D();

                    if (!string.IsNullOrEmpty(SoftwareRunningColor))
                    {
                        System.Drawing.Color ColorAmountSF = System.Drawing.ColorTranslator.FromHtml(string.IsNullOrEmpty(SoftwareRunning.HtmlColor) ? SoftwareRunning.HtmlColor : SoftwareRunning.HtmlColor);
                        areaSeries2DForSoftwareRunning.Brush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(128, ColorAmountSF.R, ColorAmountSF.G, ColorAmountSF.B));
                    }

                    areaSeries2DForSoftwareRunning.ArgumentScaleType = ScaleType.Auto;
                    areaSeries2DForSoftwareRunning.ValueScaleType = ScaleType.Numerical;
                    areaSeries2DForSoftwareRunning.DisplayName = "SF";
                    areaSeries2DForSoftwareRunning.ArgumentDataMember = "Week";
                    areaSeries2DForSoftwareRunning.ValueDataMember = "SF";
                    areaSeries2DForSoftwareRunning.CrosshairLabelPattern = "{S} : {V:c2}";
                    areaSeries2DForSoftwareRunning.ShowInLegend = true;
                    areaSeries2DForSoftwareRunning.Transparency = 0.5;
                    areaSeries2DForSoftwareRunning.MarkerVisible = true;
                    chartControlInventory.Diagram.Series.Add(areaSeries2DForSoftwareRunning);



                }
                catch (Exception ex)
                {

                }
                chartControlInventory.EndInit();
                chartControlInventory.AnimationMode = ChartAnimationMode.OnDataChanged;
                chartControlInventory.Animate();

                GeosApplication.Instance.Logger.Log("Method LineGraphViewAction() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {

                GeosApplication.Instance.Logger.Log("Get an error in LineGraphViewAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

        }

        //[pramod.misal][GEOS2-5689][14.08.2024]
        private void BarGraphViewAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method BarGraphViewAction ...", category: Category.Info, priority: Priority.Low);

                chartControlInventory = (ChartControl)obj;
                chartControlInventory.BeginInit();
                XYDiagram2D diagram = new XYDiagram2D();
                chartControlInventory.Diagram = diagram;
                diagram.ActualAxisX.Title = new AxisTitle() { Content = "Week" };
                diagram.ActualAxisY.Title = new AxisTitle() { Content = "Amount" };
                diagram.ActualAxisX.QualitativeScaleOptions = new QualitativeScaleOptions
                {
                    AutoGrid = false
                };
                diagram.ActualAxisY.NumericOptions = new NumericOptions
                {
                    Format = NumericFormat.Currency
                };
                GeosApplication.SetCurrencySymbol_warehouse(GeosApplication.Instance.WMSCurrentCurrencySymbol);

                // Define a series for each category
                //var seriesForStock = CreateStackedBarSeries("Stock", "Stock", RunningMaterialColor);
                //var seriesForRunningMaterial = CreateStackedBarSeries("RM", "RM", RunningMaterialColor);
                //var seriesForSpecialMaterial = CreateStackedBarSeries("SM", "SM", SpecialMaterialColor);
                //var seriesForSpecialAsset = CreateStackedBarSeries("SA", "SA", SpecialAssetColor);
                //var seriesForSpecialTooling = CreateStackedBarSeries("ST", "ST", SpecialToolingColor);
                //var seriesForSpecialRunning = CreateStackedBarSeries("SR", "SR", SpecialRunningColor);
                //var seriesForSoftwareRunning = CreateStackedBarSeries("SF", "SF", SoftwareRunningColor);

                BarStackedSeries2D areaSeries2D = new BarStackedSeries2D();
                //Stock

                System.Drawing.Color ColorAmount = System.Drawing.ColorTranslator.FromHtml(string.IsNullOrEmpty(Color) ? Color : Color);
                areaSeries2D.Brush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, ColorAmount.R, ColorAmount.G, ColorAmount.B));
                areaSeries2D.ArgumentScaleType = ScaleType.Auto;
                areaSeries2D.ValueScaleType = ScaleType.Numerical;
                areaSeries2D.DisplayName = "Stock";
                areaSeries2D.ArgumentDataMember = "Week";
                areaSeries2D.ValueDataMember = "Stock";
                areaSeries2D.CrosshairLabelPattern = "{S} : {V:c2}";
                areaSeries2D.ShowInLegend = true;
                diagram.Series.Add(areaSeries2D);

                //Target
                LineSeries2D lineSeries2DForTarget = new LineSeries2D();
                lineSeries2DForTarget.LineStyle = new LineStyle();
                lineSeries2DForTarget.LineStyle.DashStyle = new DashStyle();
                lineSeries2DForTarget.LineStyle.Thickness = 2;
                lineSeries2DForTarget.LineStyle.DashStyle.Dashes = new DoubleCollection(new double[] { 3, 3, 3, 3 });
                lineSeries2DForTarget.ArgumentScaleType = ScaleType.Auto;
                lineSeries2DForTarget.ValueScaleType = ScaleType.Numerical;
                lineSeries2DForTarget.DisplayName = "Target";
                lineSeries2DForTarget.CrosshairLabelPattern = "{S} : {V:c2}";
                lineSeries2DForTarget.ArgumentDataMember = "Week";
                lineSeries2DForTarget.ValueDataMember = "Target";
                lineSeries2DForTarget.ShowInLegend = true;
                chartControlInventory.Diagram.Series.Add(lineSeries2DForTarget);


                //RM
                BarStackedSeries2D areaSeriesForRunningMaterial = new BarStackedSeries2D();

                if (!string.IsNullOrEmpty(RunningMaterialColor))
                {
                    System.Drawing.Color ColorAmountRM = System.Drawing.ColorTranslator.FromHtml(string.IsNullOrEmpty(RunningMaterial.HtmlColor) ? RunningMaterial.HtmlColor : RunningMaterial.HtmlColor);
                    areaSeriesForRunningMaterial.Brush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, ColorAmountRM.R, ColorAmountRM.G, ColorAmountRM.B));

                }
                areaSeriesForRunningMaterial.ArgumentScaleType = ScaleType.Auto;
                areaSeriesForRunningMaterial.ValueScaleType = ScaleType.Numerical;
                areaSeriesForRunningMaterial.DisplayName = "RM";
                areaSeriesForRunningMaterial.ArgumentDataMember = "Week";
                areaSeriesForRunningMaterial.ValueDataMember = "RM";
                areaSeriesForRunningMaterial.CrosshairLabelPattern = "{S} : {V:c2}";
                areaSeriesForRunningMaterial.ShowInLegend = true;
                chartControlInventory.Diagram.Series.Add(areaSeriesForRunningMaterial);

                //RM End


                //SM
                BarStackedSeries2D areaSeries2DForSpecialMaterial = new BarStackedSeries2D();

                if (!string.IsNullOrEmpty(SpecialMaterialColor))
                {
                    System.Drawing.Color ColorAmountSM = System.Drawing.ColorTranslator.FromHtml(string.IsNullOrEmpty(SpecialMaterial.HtmlColor) ? SpecialMaterial.HtmlColor : SpecialMaterial.HtmlColor);
                    areaSeries2DForSpecialMaterial.Brush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, ColorAmountSM.R, ColorAmountSM.G, ColorAmountSM.B));
                }

                areaSeries2DForSpecialMaterial.ArgumentScaleType = ScaleType.Auto;
                areaSeries2DForSpecialMaterial.ValueScaleType = ScaleType.Numerical;
                areaSeries2DForSpecialMaterial.DisplayName = "SM";
                areaSeries2DForSpecialMaterial.ArgumentDataMember = "Week";
                areaSeries2DForSpecialMaterial.ValueDataMember = "SM";
                areaSeries2DForSpecialMaterial.CrosshairLabelPattern = "{S} : {V:c2}";
                areaSeries2DForSpecialMaterial.ShowInLegend = true;
                chartControlInventory.Diagram.Series.Add(areaSeries2DForSpecialMaterial);
                //SM end

                //SA
                BarStackedSeries2D areaSeries2DForSpecialAsset = new BarStackedSeries2D();

                if (!string.IsNullOrEmpty(SpecialAssetColor))
                {

                    System.Drawing.Color ColorAmountSA = System.Drawing.ColorTranslator.FromHtml(string.IsNullOrEmpty(SpecialAsset.HtmlColor) ? SpecialAsset.HtmlColor : SpecialAsset.HtmlColor);
                    areaSeries2DForSpecialAsset.Brush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, ColorAmountSA.R, ColorAmountSA.G, ColorAmountSA.B));
                }


                areaSeries2DForSpecialAsset.ArgumentScaleType = ScaleType.Auto;
                areaSeries2DForSpecialAsset.ValueScaleType = ScaleType.Numerical;
                areaSeries2DForSpecialAsset.DisplayName = "SA";
                areaSeries2DForSpecialAsset.ArgumentDataMember = "Week";
                areaSeries2DForSpecialAsset.ValueDataMember = "SA";
                areaSeries2DForSpecialAsset.CrosshairLabelPattern = "{S} : {V:c2}";
                areaSeries2DForSpecialAsset.ShowInLegend = true;
                chartControlInventory.Diagram.Series.Add(areaSeries2DForSpecialAsset);

                //SA end

                //ST
                BarStackedSeries2D areaSeries2DForSpecialTooling = new BarStackedSeries2D();

                if (!string.IsNullOrEmpty(SpecialToolingColor))
                {
                    System.Drawing.Color ColorAmountST = System.Drawing.ColorTranslator.FromHtml(string.IsNullOrEmpty(SpecialTooling.HtmlColor) ? SpecialTooling.HtmlColor : SpecialTooling.HtmlColor);
                    areaSeries2DForSpecialTooling.Brush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, ColorAmountST.R, ColorAmountST.G, ColorAmountST.B));
                }

                areaSeries2DForSpecialTooling.ArgumentScaleType = ScaleType.Auto;
                areaSeries2DForSpecialTooling.ValueScaleType = ScaleType.Numerical;
                areaSeries2DForSpecialTooling.DisplayName = "ST";
                areaSeries2DForSpecialTooling.ArgumentDataMember = "Week";
                areaSeries2DForSpecialTooling.ValueDataMember = "ST";
                areaSeries2DForSpecialTooling.CrosshairLabelPattern = "{S} : {V:c2}";
                areaSeries2DForSpecialTooling.ShowInLegend = true;
                chartControlInventory.Diagram.Series.Add(areaSeries2DForSpecialTooling);
                //ST end

                //SR
                BarStackedSeries2D areaSeries2DForSpecialRunning = new BarStackedSeries2D();

                if (!string.IsNullOrEmpty(SpecialRunningColor))
                {
                    System.Drawing.Color ColorAmountSR = System.Drawing.ColorTranslator.FromHtml(string.IsNullOrEmpty(SpecialRunning.HtmlColor) ? SpecialRunning.HtmlColor : SpecialRunning.HtmlColor);
                    areaSeries2DForSpecialRunning.Brush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, ColorAmountSR.R, ColorAmountSR.G, ColorAmountSR.B));
                }

                areaSeries2DForSpecialRunning.ArgumentScaleType = ScaleType.Auto;
                areaSeries2DForSpecialRunning.ValueScaleType = ScaleType.Numerical;
                areaSeries2DForSpecialRunning.DisplayName = "SR";
                areaSeries2DForSpecialRunning.ArgumentDataMember = "Week";
                areaSeries2DForSpecialRunning.ValueDataMember = "SR";
                areaSeries2DForSpecialRunning.CrosshairLabelPattern = "{S} : {V:c2}";
                areaSeries2DForSpecialRunning.ShowInLegend = true;
                chartControlInventory.Diagram.Series.Add(areaSeries2DForSpecialRunning);
                //SR end

                // Software running
                BarStackedSeries2D areaSeries2DForSoftwareRunning = new BarStackedSeries2D();

                if (!string.IsNullOrEmpty(SoftwareRunningColor))
                {
                    System.Drawing.Color ColorAmountSF = System.Drawing.ColorTranslator.FromHtml(string.IsNullOrEmpty(SoftwareRunning.HtmlColor) ? SoftwareRunning.HtmlColor : SoftwareRunning.HtmlColor);
                    areaSeries2DForSoftwareRunning.Brush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, ColorAmountSF.R, ColorAmountSF.G, ColorAmountSF.B));
                }

                areaSeries2DForSoftwareRunning.ArgumentScaleType = ScaleType.Auto;
                areaSeries2DForSoftwareRunning.ValueScaleType = ScaleType.Numerical;
                areaSeries2DForSoftwareRunning.DisplayName = "SF";
                areaSeries2DForSoftwareRunning.ArgumentDataMember = "Week";
                areaSeries2DForSoftwareRunning.ValueDataMember = "SF";
                areaSeries2DForSoftwareRunning.CrosshairLabelPattern = "{S} : {V:c2}";
                areaSeries2DForSoftwareRunning.ShowInLegend = true;
                chartControlInventory.Diagram.Series.Add(areaSeries2DForSoftwareRunning);

                //end software running


                // Add the series to the diagram
                //diagram.Series.Add(areaSeries2D);
                //diagram.Series.Add(seriesForRunningMaterial);
                //diagram.Series.Add(seriesForSpecialMaterial);
                //diagram.Series.Add(seriesForSpecialAsset);
                //diagram.Series.Add(seriesForSpecialTooling);
                //diagram.Series.Add(seriesForSpecialRunning);
                //diagram.Series.Add(seriesForSoftwareRunning);

                chartControlInventory.EndInit();
                chartControlInventory.AnimationMode = ChartAnimationMode.Disabled;
                chartControlInventory.Animate();

                GeosApplication.Instance.Logger.Log("Method BarGraphViewAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in BarGraphViewAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        private BarStackedSeries2D CreateStackedBarSeries(string displayName, string valueDataMember, string colorHtml)
        {
            var series = new BarStackedSeries2D
            {
                ArgumentScaleType = ScaleType.Auto,
                ValueScaleType = ScaleType.Numerical,
                DisplayName = displayName,
                ArgumentDataMember = "Week",
                ValueDataMember = valueDataMember,
                CrosshairLabelPattern = "{S} : {V:c2}",
                ShowInLegend = true,
                BarWidth = 0.8 // Adjust bar width as needed
            };

            if (!string.IsNullOrEmpty(colorHtml))
            {
                System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml(colorHtml);
                series.Brush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(128, color.R, color.G, color.B));
            }

            return series;
        }

        #endregion

    }
}

