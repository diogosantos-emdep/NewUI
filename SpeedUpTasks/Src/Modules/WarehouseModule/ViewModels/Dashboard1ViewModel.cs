using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using Emdep.Geos.UI.Common;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Globalization;
using System.Windows.Media.Imaging;
using DevExpress.Mvvm;
using System.Windows.Input;
using Emdep.Geos.Modules.Warehouse.Views;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.ServiceProcess;
using System.ServiceModel;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.Data.Common;
using System.Collections.ObjectModel;
using System.IO;
using System.Data;
using DevExpress.Xpf.Charts;

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    public class Dashboard1ViewModel : INotifyPropertyChanged
    {
        #region TaskLog
        //[Sprint_67] [22-07-2019]----(GEOS2-1535) New Dashboard1---[sdsai]
        #endregion

        #region Services
        IWorkbenchStartUp WorkbenchService = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWarehouseService WarehouseService = new WarehouseServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion // End Services Region

        #region Declaration
        private ObservableCollection<OrderProcessing> orderProcessingList;
        private ObservableCollection<OrderPreparation> orderPreparationList;
        private string currentWeek;
        private long complaintOffersCount;
        private ObservableCollection<MyWarehouse> itemsOutOfStockList;
        private ObservableCollection<MyWarehouse> itemsUnderMinStockList;
        private DashboardInventory totalItemsToPutAway;
        private DashboardInventory totalItemsToRefill;
        private ObservableCollection<OrderPreparation> orderPreparationDetailsList;
        private long urgentOffersCount;
        private long upComingOffersCount;
        private long futureOffersCount;
        private long currentDateWeek;
        private List<GeosAppSetting> geosAppSettingColorList;
        private Decimal complaintOffersAvgValue;
        private Decimal urgentOffersAvgValue;
        private Decimal upcomingOffersAvgValue;
        private Decimal futureOffersAvgValue;
        private ObservableCollection<MyWarehouse> warehouseArticleDetailsList;
        private long itemsOutOfStockAvgValue;
        private long itemsUnderMinStockAvgValue;
        private Int64 countOfRefillWithNoStock;
        private DataTable dataTableTotalOrdersGraph;
        private List<List<OrderPreparation>> orderPreparationGraphList;
        private DataTable copyMainGraphDataTable;
        #endregion

        #region Properties
        public ObservableCollection<OrderProcessing> OrderProcessingList
        {
            get
            {
                return orderProcessingList;
            }

            set
            {
                orderProcessingList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OrderProcessingList"));
            }
        }
        public ObservableCollection<OrderPreparation> OrderPreparationList
        {
            get
            {
                return orderPreparationList;
            }

            set
            {
                orderPreparationList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OrderPreparationList"));
            }
        }

        public string CurrentWeek
        {
            get
            {
                return currentWeek;
            }

            set
            {
                currentWeek = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CurrentWeek"));
            }
        }
        public long ComplaintOffersCount
        {
            get
            {
                return complaintOffersCount;
            }

            set
            {
                complaintOffersCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ComplaintOffersCount"));
            }
        }

        public ObservableCollection<MyWarehouse> ItemsOutOfStockList
        {
            get
            {
                return itemsOutOfStockList;
            }

            set
            {
                itemsOutOfStockList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ItemsOutOfStockList"));
            }
        }

        public ObservableCollection<MyWarehouse> ItemsUnderMinStockList
        {
            get
            {
                return itemsUnderMinStockList;
            }

            set
            {
                itemsUnderMinStockList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ItemsUnderMinStockList"));
            }
        }

        public DashboardInventory TotalItemsToPutAway
        {
            get
            {
                return totalItemsToPutAway;
            }

            set
            {
                totalItemsToPutAway = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TotalItemsToPutAway"));
            }
        }

        public DashboardInventory TotalItemsToRefill
        {
            get
            {
                return totalItemsToRefill;
            }

            set
            {
                totalItemsToRefill = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TotalItemsToRefill"));
            }
        }

        public ObservableCollection<OrderPreparation> OrderPreparationDetailsList
        {
            get
            {
                return orderPreparationDetailsList;
            }

            set
            {
                orderPreparationDetailsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OrderPreparationDetailsList"));
            }
        }
        public long UrgentOffersCount
        {
            get
            {
                return urgentOffersCount;
            }

            set
            {
                urgentOffersCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UrgentOffersCount"));
            }
        }

        public long UpComingOffersCount
        {
            get
            {
                return upComingOffersCount;
            }

            set
            {
                upComingOffersCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpComingOffersCount"));
            }
        }

        public long FutureOffersCount
        {
            get
            {
                return futureOffersCount;
            }

            set
            {
                futureOffersCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FutureOffersCount"));
            }
        }
        public long CurrentDateWeek
        {
            get
            {
                return currentDateWeek;
            }

            set
            {
                currentDateWeek = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CurrentDateWeek"));
            }
        }
        public List<GeosAppSetting> GeosAppSettingColorList
        {
            get
            {
                return geosAppSettingColorList;
            }

            set
            {
                geosAppSettingColorList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GeosAppSettingColorList"));
            }
        }
        public Decimal ComplaintOffersAvgValue
        {
            get
            {
                return complaintOffersAvgValue;
            }

            set
            {
                complaintOffersAvgValue = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ComplaintOffersAvgValue"));
            }
        }
        public Decimal UrgentOffersAvgValue
        {
            get
            {
                return urgentOffersAvgValue;
            }

            set
            {
                urgentOffersAvgValue = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UrgentOffersAvgValue"));
            }
        }

        public Decimal UpcomingOffersAvgValue
        {
            get
            {
                return upcomingOffersAvgValue;
            }

            set
            {
                upcomingOffersAvgValue = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpcomingOffersAvgValue"));
            }
        }

        public Decimal FutureOffersAvgValue
        {
            get
            {
                return futureOffersAvgValue;
            }

            set
            {
                futureOffersAvgValue = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FutureOffersAvgValue"));
            }
        }
        public ObservableCollection<MyWarehouse> WarehouseArticleDetailsList
        {
            get
            {
                return warehouseArticleDetailsList;
            }

            set
            {
                warehouseArticleDetailsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WarehouseArticleDetailsList"));
            }
        }
        public long ItemsOutOfStockAvgValue
        {
            get
            {
                return itemsOutOfStockAvgValue;
            }

            set
            {
                itemsOutOfStockAvgValue = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ItemsOutOfStockAvgValue"));
            }
        }
        public long ItemsUnderMinStockAvgValue
        {
            get
            {
                return itemsUnderMinStockAvgValue;
            }

            set
            {
                itemsUnderMinStockAvgValue = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ItemsUnderMinStockAvgValue"));
            }
        }
        public long CountOfRefillWithNoStock
        {
            get
            {
                return countOfRefillWithNoStock;
            }

            set
            {
                countOfRefillWithNoStock = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CountOfRefillWithNoStock"));
            }
        }
        public DataTable DataTableTotalOrdersGraph
        {
            get
            {
                return dataTableTotalOrdersGraph;
            }

            set
            {
                dataTableTotalOrdersGraph = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DataTableTotalOrdersGraph"));
            }
        }
        public List<List<OrderPreparation>> OrderPreparationGraphList
        {
            get
            {
                return orderPreparationGraphList;
            }

            set
            {
                orderPreparationGraphList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OrderPreparationGraphList"));
            }
        }
        public DataTable CopyMainGraphDataTable
        {
            get
            {
                return copyMainGraphDataTable;
            }

            set
            {
                copyMainGraphDataTable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CopyMainGraphDataTable"));
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

        #endregion // End Of Events 

        #region Commands
        public ICommand CommandWarehouseEditValueChanged { get; private set; }
        public ICommand RefreshButtonCommand { get; set; }
        public ICommand ChartLoadCommand { get; set; }
        public ICommand SelectedItemChangedCommand { get; set; }
        #endregion

        #region Constructor
        public Dashboard1ViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor WorkOrderViewModel....", category: Category.Info, priority: Priority.Low);
                CommandWarehouseEditValueChanged = new DevExpress.Mvvm.DelegateCommand<object>(WarehousePopupClosedCommandAction);
                RefreshButtonCommand = new RelayCommand(new Action<object>(RefreshButtonCommandAction));
                ChartLoadCommand = new DevExpress.Mvvm.DelegateCommand<object>(ChartLoadCommandAction);//new DelegateCommand<RoutedEventArgs>(ChartLoadCommandAction);
                                                                                                       //  SelectedItemChangedCommand = new DelegateCommand<DevExpress.Xpf.Docking.Base.SelectedItemChangedEventArgs>(SelectedItemChangedCommandAction);
                GetCurrentWeek();
                FillDashBoardDetails();

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
        /// <summary>
        /// Method to Fill DashBoard Details.
        /// </summary>
        private void FillDashBoardDetails()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillDashBoardDetails...", category: Category.Info, priority: Priority.Low);
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

                OrderProcessingList = new ObservableCollection<OrderProcessing>(WarehouseService.GetOrderInProcessing_V2051(WarehouseCommon.Instance.Selectedwarehouse));
                OrderPreparationDetailsList = new ObservableCollection<OrderPreparation>(WarehouseService.GetPendingWorkOrders_V2037(WarehouseCommon.Instance.Selectedwarehouse));
                OrderPreparationList = new ObservableCollection<OrderPreparation>(OrderPreparationDetailsList.Where(x => x.OtDeliveryDateWeek == CurrentDateWeek).ToList());

                ComplaintOffersCount = OrderPreparationDetailsList.Where(x => x.IdOfferType == 2 || x.IdOfferType == 3).ToList().Count;
                ComplaintOffersAvgValue = Convert.ToDecimal(ComplaintOffersCount) * 100 / Convert.ToDecimal(OrderPreparationDetailsList.Count);
                ComplaintOffersAvgValue = Math.Round(ComplaintOffersAvgValue, 2);


                UrgentOffersCount = OrderPreparationDetailsList.Where(x => x.OTDeliveryDate.Date <= GeosApplication.Instance.ServerDateTime.Date && (x.IdOfferType != 2 && x.IdOfferType != 3)).ToList().Count;
                UrgentOffersAvgValue = Convert.ToDecimal(UrgentOffersCount) * 100 / Convert.ToDecimal(OrderPreparationDetailsList.Count);
                UrgentOffersAvgValue = Math.Round(UrgentOffersAvgValue, 2);

                UpComingOffersCount = OrderPreparationDetailsList.Where(x => x.OTDeliveryDate.Date > DateTime.Now.Date && x.OTDeliveryDate.Date <= DateTime.Now.Date.AddDays(6) && (x.IdOfferType != 2 && x.IdOfferType != 3)).ToList().Count;
                UpcomingOffersAvgValue = Convert.ToDecimal(UpComingOffersCount) * 100 / Convert.ToDecimal(OrderPreparationDetailsList.Count);
                UpcomingOffersAvgValue = Math.Round(UpcomingOffersAvgValue, 2);

                FutureOffersCount = OrderPreparationDetailsList.Where(x => x.OTDeliveryDate.Date >= DateTime.Now.Date.AddDays(7) && (x.IdOfferType != 2 && x.IdOfferType != 3)).ToList().Count;
                FutureOffersAvgValue = Convert.ToDecimal(FutureOffersCount) * 100 / Convert.ToDecimal(OrderPreparationDetailsList.Count);
                FutureOffersAvgValue = Math.Round(FutureOffersAvgValue, 2);


                WarehouseArticleDetailsList = new ObservableCollection<MyWarehouse>(WarehouseService.GetWarehouseArticleDetails_V2035(WarehouseCommon.Instance.Selectedwarehouse).ToList());

                ItemsOutOfStockList = new ObservableCollection<MyWarehouse>(WarehouseArticleDetailsList.Where(x => x.CurrentStock == 0 && x.MinimumStock > 0).ToList());
                // ItemsOutOfStockAvgValue = ItemsOutOfStockList.Count * 100 / WarehouseArticleDetailsList.Count;

                ItemsUnderMinStockList = new ObservableCollection<MyWarehouse>(WarehouseArticleDetailsList.Where(x => x.MinimumStock > 0 && x.CurrentStock < x.MinimumStock));
                // ItemsUnderMinStockAvgValue = ItemsUnderMinStockList.Count * 100 / WarehouseArticleDetailsList.Count;

                TotalItemsToPutAway = WarehouseService.GetTotalItemsToLocate_V2037(WarehouseCommon.Instance.Selectedwarehouse);
                TotalItemsToRefill = WarehouseService.GetTotalItemsToRefill_V2037(WarehouseCommon.Instance.Selectedwarehouse);

                GeosAppSettingColorList = WorkbenchService.GetSelectedGeosAppSettings("14,15,16,17");
                CountOfRefillWithNoStock = TotalItemsToRefill.TotalItemsToRefillOutOfStock; //WarehouseService.GetCountOfRefillWithNoStock(WarehouseCommon.Instance.Selectedwarehouse);

                OrderPreparationGraphList = new List<List<OrderPreparation>>();
                foreach (var item in OrderPreparationDetailsList.GroupBy(x => x.Customer))
                {
                    List<OrderPreparation> sublist = new List<OrderPreparation>(item.ToList());
                    OrderPreparationGraphList.Add(sublist);
                }

                FillGraphDataTable();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method FillDashBoardDetails executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillDashBoardDetails() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillDashBoardDetails() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillDashBoardDetails() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to Get Current Week of year.
        /// </summary>
        private void GetCurrentWeek()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GetCurrentWeek()...", category: Category.Info, priority: Priority.Low);
                int weekNumber = 0;

                //if (DevExpress.XtraScheduler.Native.DateTimeHelper.GetWeekOfYear(GeosApplication.Instance.ServerDateTime) < 10)
                //    weekNumber = DevExpress.XtraScheduler.Native.DateTimeHelper.GetWeekOfYear((GeosApplication.Instance.ServerDateTime));
                //else

                weekNumber = DevExpress.XtraScheduler.Native.DateTimeHelper.GetWeekOfYear(GeosApplication.Instance.ServerDateTime);

                CurrentWeek = string.Format("{0}{1}", "CW", weekNumber);
                string _currentDateWeek = string.Format("{0}{1}", GeosApplication.Instance.ServerDateTime.Year.ToString(), weekNumber.ToString().PadLeft(2, '0'));
                CurrentDateWeek = (long)Convert.ToDouble(_currentDateWeek);
                GeosApplication.Instance.Logger.Log("Method EditLeaveInformation()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method EditLeaveInformation() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void WarehousePopupClosedCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method WarehousePopupClosedCommandAction...", category: Category.Info, priority: Priority.Low);

            //When setting the warehouse from default the data should not be refreshed
            if (!WarehouseCommon.Instance.IsWarehouseChangedEventCanOccur)
                return;

            FillDashBoardDetails();
            GeosApplication.Instance.Logger.Log("Method WarehousePopupClosedCommandAction executed successfully.", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Methos to refresh DashBoard
        /// </summary>
        /// <param name="obj"></param>
        private void RefreshButtonCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method RefreshButtonCommandAction...", category: Category.Info, priority: Priority.Low);
            FillDashBoardDetails();
            GeosApplication.Instance.Logger.Log("Method RefreshButtonCommandAction executed successfully.", category: Category.Info, priority: Priority.Low);
        }
        private void FillGraphDataTable()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillGraphDataTable ...", category: Category.Info, priority: Priority.Low);

                DataTableTotalOrdersGraph = new DataTable();
                DataTableTotalOrdersGraph.Columns.Add("Customer", typeof(string));
                DataTableTotalOrdersGraph.Columns.Add("Complaints", typeof(Int16));
                DataTableTotalOrdersGraph.Columns.Add("Urgent", typeof(Int16));
                DataTableTotalOrdersGraph.Columns.Add("In next 7 days", typeof(Int16));
                DataTableTotalOrdersGraph.Columns.Add("After a week", typeof(Int16));


                DataTableTotalOrdersGraph.Rows.Clear();
                foreach (var item in OrderPreparationGraphList)
                {   // Parent
                    DataRow graph = DataTableTotalOrdersGraph.NewRow();
                    graph["Customer"] = item.FirstOrDefault().Customer;
                    graph["Complaints"] = item.Where(x => x.IdOfferType == 2 || x.IdOfferType == 3).ToList().Count;
                    graph["Urgent"] = item.Where(x => x.OTDeliveryDate.Date <= GeosApplication.Instance.ServerDateTime.Date && (x.IdOfferType != 2 && x.IdOfferType != 3)).ToList().Count;
                    graph["In next 7 days"] = item.Where(x => x.OTDeliveryDate.Date > DateTime.Now.Date && x.OTDeliveryDate.Date <= DateTime.Now.Date.AddDays(6)).ToList().Count;
                    graph["After a week"] = item.Where(x => x.OTDeliveryDate.Date >= DateTime.Now.Date.AddDays(7)).ToList().Count;
                    DataTableTotalOrdersGraph.Rows.Add(graph);
                }

                CopyMainGraphDataTable = DataTableTotalOrdersGraph.Copy();
                DataTableTotalOrdersGraph = CopyMainGraphDataTable;
                GeosApplication.Instance.Logger.Log("Method FillGraphDataTable() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillGraphDataTable() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ChartLoadCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChartLoadCommandAction ...", category: Category.Info, priority: Priority.Low);

                ChartControl chartcontrol = (ChartControl)obj;
                chartcontrol.BeginInit();
                XYDiagram2D diagram = new XYDiagram2D();
                chartcontrol.Diagram = diagram;
                diagram.ActualAxisX.Title = new AxisTitle() { Content = " " };
                diagram.ActualAxisY.Title = new AxisTitle() { Content = " " };

                diagram.ActualAxisX.QualitativeScaleOptions = new QualitativeScaleOptions() { AutoGrid = false };
                diagram.ActualAxisX.ActualLabel.Angle = 90;

                diagram.ActualAxisX.Title.Visible = false;
                diagram.ActualAxisY.Title.Visible = false;

                foreach (var item in GeosAppSettingColorList)
                {
                    if (item != null)
                    {
                        if (item.IdAppSetting == 14)
                            item.AppSettingName = "Urgent";
                        else if (item.IdAppSetting == 15)
                            item.AppSettingName = "In next 7 days";
                        else if (item.IdAppSetting == 16)
                            item.AppSettingName = "After a week";
                        else if (item.IdAppSetting == 17)
                            item.AppSettingName = "Complaints";

                        BarSideBySideStackedSeries2D barSideBySideStackedSeries2D = new BarSideBySideStackedSeries2D();
                        barSideBySideStackedSeries2D.ArgumentScaleType = ScaleType.Auto;
                        barSideBySideStackedSeries2D.ValueScaleType = ScaleType.Numerical;
                        barSideBySideStackedSeries2D.DisplayName = item.AppSettingName;
                        barSideBySideStackedSeries2D.CrosshairLabelPattern = "{S} : {V}";
                        barSideBySideStackedSeries2D.Brush = (SolidColorBrush)(new BrushConverter().ConvertFrom(item.DefaultValue.ToString()));
                        barSideBySideStackedSeries2D.ArgumentDataMember = "Customer";
                        barSideBySideStackedSeries2D.ValueDataMember = item.AppSettingName;
                        barSideBySideStackedSeries2D.Model = new DevExpress.Xpf.Charts.SimpleBar2DModel();
                        barSideBySideStackedSeries2D.ShowInLegend = false;

                        barSideBySideStackedSeries2D.AnimationAutoStartMode = AnimationAutoStartMode.SetStartState;
                        Bar2DSlideFromTopAnimation seriesAnimation = new Bar2DSlideFromTopAnimation();
                        seriesAnimation.Duration = new TimeSpan(0, 0, 2);
                        barSideBySideStackedSeries2D.PointAnimation = seriesAnimation;

                        chartcontrol.Diagram.Series.Add(barSideBySideStackedSeries2D);

                        BarSideBySideStackedSeries2D barSideBySideStackedSeries2Dhidden = new BarSideBySideStackedSeries2D();
                        barSideBySideStackedSeries2Dhidden.Brush = (SolidColorBrush)(new BrushConverter().ConvertFrom(item.DefaultValue.ToString()));
                        chartcontrol.Diagram.Series.Insert(0, barSideBySideStackedSeries2Dhidden);
                        barSideBySideStackedSeries2Dhidden.DisplayName = item.AppSettingName;
                    }
                }
                chartcontrol.EndInit();
                chartcontrol.AnimationMode = ChartAnimationMode.OnDataChanged;
                chartcontrol.Animate();
                GeosApplication.Instance.Logger.Log("Method ChartLoadCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ChartLoadCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
    }


}
