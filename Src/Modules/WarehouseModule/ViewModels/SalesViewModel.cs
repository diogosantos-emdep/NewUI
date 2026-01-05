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
    public class SalesViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable
    {

        #region Services

           IWarehouseService WmsService = new WarehouseServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
          ICrmService CrmStartUp = new CrmServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());


       // IWarehouseService WmsService = new WarehouseServiceController("localhost:6699");
        //  ICrmService CrmStartUp = new CrmServiceController("localhost:6699");



        #endregion

        #region Declaration
        private double wonAmount;
        private Currency selectedCurrency;
        private string currencySymbol;
        private List<OfferDetail> salesStatusByMonthList;
        private DataTable dt = new DataTable();
        private List<int> months;
        private List<int> years;
        private List<YearMonth> ListYearMonths;
        string fromDate;
        string toDate;
        private List<WarehouseCustomer> salesStatusByCustomerList;
        private string color;
        private DataTable graphDataTableCustomer;
        private ChartControl chartControlCustomer;
        private YearMonth yearMonth;
        private DataTable graphDataTable;
        private ChartControl chartControl;
        int isButtonStatus;
        Visibility isCalendarVisible;
        DateTime startDate;
        DateTime endDate;
        private XYDiagram2D diagram;
        private XYDiagram2D diagram1;
        private BarSideBySideSeries2D barSideBySideSeries2D;
        private BarSideBySideStackedSeries2D barSideBySideStackedSeries2D;
        #endregion

        #region Properties

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
        public List<OfferDetail> SalesStatusByMonthList
        {
            get { return salesStatusByMonthList; }
            set { salesStatusByMonthList = value; }
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
        public List<WarehouseCustomer> SalesStatusByCustomerList
        {
            get { return salesStatusByCustomerList; }
            set { salesStatusByCustomerList = value; }
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
        public DataTable GraphDataTableCustomer
        {
            get { return graphDataTableCustomer; }
            set { graphDataTableCustomer = value; OnPropertyChanged(new PropertyChangedEventArgs("GraphDataTableCustomer")); }
        }
        public DataTable GraphDataTable
        {
            get { return graphDataTable; }
            set { graphDataTable = value; OnPropertyChanged(new PropertyChangedEventArgs("GraphDataTable")); }
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
        #endregion

        #region Commands
        public ICommand WarehouseChangeCommand { get; set; }
        public ICommand PeriodThisYearCommand { get; set; }
        public ICommand PeriodLastYearCommand { get; set; }
        public ICommand PeriodLast12MonthsCommand { get; set; }
        public ICommand PeriodCustomRangeCommand { get; set; }
        public ICommand ApplyCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public ICommand RefreshDashboard2ViewCommand { get; set; }

        public ICommand ChartLoadCustomerCommand { get; set; }

        public ICommand ChartDashboardSaleCustomDrawCrosshairCommand { get; set; }

        public ICommand ChartLoadCommand { get; set; }

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
        public SalesViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor DashboardSalesViewModel ...", category: Category.Info, priority: Priority.Low);
                Processing();
                SetColor();
                WarehouseChangeCommand = new DelegateCommand<object>(WarehouseChangeCommandAction);
                PeriodThisYearCommand = new DelegateCommand<object>(PeriodThisYearCommandAction);
                PeriodLastYearCommand = new DelegateCommand<object>(PeriodLastYearCommandAction);
                PeriodLast12MonthsCommand = new DelegateCommand<object>(PeriodLast12MonthsCommandAction);
                PeriodCustomRangeCommand = new DelegateCommand<object>(PeriodCustomRangeCommandAction);
                ApplyCommand = new DelegateCommand<object>(ApplyCommandAction);
                CancelCommand = new DelegateCommand<object>(CancelCommandAction);
                RefreshDashboard2ViewCommand = new DelegateCommand<object>(RefreshDashboard2ViewCommandAction);
                ChartLoadCustomerCommand = new DelegateCommand<object>(ChartLoadCustomerAction);
                ChartDashboardSaleCustomDrawCrosshairCommand = new DelegateCommand<object>(ChartDashboardSaleCustomDrawCrosshairCommandAction);
                ChartLoadCommand = new DelegateCommand<object>(ChartLoadAction);
                setDefaultPeriod();
                List<Currency> CurrencyList = GeosApplication.Instance.Currencies.ToList();
                SelectedCurrency = CurrencyList.FirstOrDefault(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency_Warehouse"]);
                GeosApplication.Instance.WMSCurrentCurrencySymbol = CurrencyList.FirstOrDefault(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency_Warehouse"]).Symbol;



                SetWonAmount();
                CreateTable();
                CreateTableCustomer();
                StartDate = DateTime.Today.Date;
                EndDate = DateTime.Today.Date;

                IsCalendarVisible = Visibility.Collapsed;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method DashboardSalesViewModel....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method DashboardSalesViewModel...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Method
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

        private void setDefaultPeriod()
        {
            int year = DateTime.Now.Year;
            DateTime StartFromDate = new DateTime(year, 1, 1);
            DateTime EndToDate = new DateTime(year, 12, 31);

            FromDate = StartFromDate.ToShortDateString();
            ToDate = EndToDate.ToShortDateString();
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
                //SetStockAmount();
                //SetObsoleteAmount();
                //SetSleepingAmount();
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
                //SetStockAmount();
                //SetObsoleteAmount();
                //SetSleepingAmount();
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
        private void CancelCommandAction(object obj)
        {
            MenuFlyout menu = (MenuFlyout)obj;
            menu.IsOpen = false;
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
                //SetStockAmount();
                //SetObsoleteAmount();
                //SetSleepingAmount();
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

                GeosApplication.Instance.Logger.Log("Method WarehouseChangeCommandAction....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method WarehouseChangeCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
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
        private void CreateTableCustomer()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CreateTableCustomer ...", category: Category.Info, priority: Priority.Low);
                SalesStatusByCustomerList = WmsService.GetSalesByCustomer(WarehouseCommon.Instance.Selectedwarehouse, SelectedCurrency.IdCurrency, Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate));

                dt = new DataTable();
                List<string> list = SalesStatusByCustomerList.Where(x => x.Alias != "" && x.Alias != null).Select(a => a.Alias.ToString()).Distinct().ToList();

                dt.Columns.Add("Alias");
                dt.Columns.Add("Amount", typeof(double));

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
        private string GetFullMonthName(int month, int year)
        {
            DateTime date = new DateTime(year, month, 1);
            return date.ToString("MMMM");
        }
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
        private int GetMonthNumber(string date)
        {
            DateTime dt = Convert.ToDateTime(date);
            return dt.Month;
        }
        private int GetYearNumber(string date)
        {
            DateTime dt = Convert.ToDateTime(date);
            return dt.Year;
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
                    dr[2] = GetFullMonthName(mt, GetYearNumber(FromDate));
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
        public void Dispose()
        {
        }

        #endregion
    }
}
