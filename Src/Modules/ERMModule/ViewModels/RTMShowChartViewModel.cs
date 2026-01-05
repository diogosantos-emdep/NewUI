using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Charts;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Serialization;
using DevExpress.Xpf.Editors.Flyout;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.TreeList;
using DevExpress.Xpf.LayoutControl;
using DevExpress.Xpf.Printing;
using DevExpress.Xpf.Scheduler;
using DevExpress.Xpf.Scheduler.Menu;
using DevExpress.Xpf.Scheduler.UI;
using DevExpress.Xpf.WindowsUI;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.ERM;
using Emdep.Geos.Data.Common.OptimizedClass;
using Emdep.Geos.Data.Common.PCM;
using Emdep.Geos.Modules.ERM.CommonClasses;
using Emdep.Geos.Modules.ERM.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.Utility.Text;
using Prism.Commands;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using DevExpress.Data;
using Emdep.Geos.UI.Commands;

namespace Emdep.Geos.Modules.ERM.ViewModels
{
    public class RTMShowChartViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region declaration
        private ChartControl chartControl;
        private DataTable actiongraphDataTable;
        private DataTable graphDataTable;
        private ObservableCollection<ERMDeliveryVisualManagement> eRMDeliveryVisualManagementList;
        private DataTable dp = new DataTable();
        private List<DeliveryVisualManagementStages> eRMDeliveryVisualManagementStagesList;
        string[] ArrActiveInPlants;
        public bool FlagPresentInActivePlants = false;
        private UInt64 conditionQuality;
        private UInt64 conditionGreaterThan7Days;
        private UInt64 conditionDelay;
        private UInt64 conditionLessThanEqual7Day;
        private UInt64 conditionOnHoldTS;
        private UInt64 conditionComponents;
        private UInt64 conditionGoAhead;
        private UInt64 totalQuantity;
        private DataTable dt = new DataTable();
        private string loadsxWorkstation;
        private ChartControl ChartCADCOM;

        #endregion declaration


        #region Public Commands
        public ICommand ChartDashboardSaleCustomDrawCrosshairCommand { get; set; }
        public ICommand ActionLoadCommand { get; set; }
        public ICommand CloseButtonCommand { get; set; }

        #endregion Public Commands



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

        #endregion // End Of Events 

        #region Property
        public DataTable GraphDataTable
        {
            get { return graphDataTable; }
            set { graphDataTable = value; OnPropertyChanged(new PropertyChangedEventArgs("GraphDataTable")); }
        }

        public DataTable ActionGraphDataTable
        {
            get { return actiongraphDataTable; }
            set { actiongraphDataTable = value; OnPropertyChanged(new PropertyChangedEventArgs("ActionGraphDataTable")); }
        }

        public ObservableCollection<ERMDeliveryVisualManagement> ERMDeliveryVisualManagementList
        {
            get
            {
                return eRMDeliveryVisualManagementList;
            }
            set
            {
                eRMDeliveryVisualManagementList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ERMDeliveryVisualManagementList"));
            }
        }

        public List<DeliveryVisualManagementStages> ERMDeliveryVisualManagementStagesList
        {
            get
            {
                return eRMDeliveryVisualManagementStagesList;
            }
            set
            {
                eRMDeliveryVisualManagementStagesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ERMDeliveryVisualManagementStagesList"));
            }
        }

        public UInt64 ConditionQuality
        {
            get { return conditionQuality; }
            set
            {
                conditionQuality = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ConditionQuality"));
            }
        }

        public UInt64 ConditionGreaterThan7Days
        {
            get { return conditionGreaterThan7Days; }
            set
            {
                conditionGreaterThan7Days = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ConditionGreaterThan7Days"));
            }
        }

        public UInt64 ConditionDelay
        {
            get { return conditionDelay; }
            set
            {
                conditionDelay = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ConditionDelay"));
            }
        }
        public UInt64 ConditionLessThanEqual7Day
        {
            get { return conditionLessThanEqual7Day; }
            set
            {
                conditionLessThanEqual7Day = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ConditionLessThanEqual7Day"));
            }
        }
        public UInt64 ConditionOnHoldTS
        {
            get { return conditionOnHoldTS; }
            set
            {
                conditionOnHoldTS = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ConditionOnHoldTS"));
            }
        }
        public UInt64 ConditionComponents
        {
            get { return conditionComponents; }
            set
            {
                conditionComponents = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ConditionComponents"));
            }
        }
        public UInt64 ConditionGoAhead
        {
            get { return conditionGoAhead; }
            set
            {
                conditionGoAhead = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ConditionGoAhead"));
            }
        }

        public UInt64 TotalQuantity
        {
            get { return totalQuantity; }
            set
            {
                totalQuantity = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TotalQuantity"));
            }
        }
        public string LoadsxWorkstation
        {
            get
            {
                return loadsxWorkstation;
            }
            set
            {
                loadsxWorkstation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LoadsxWorkstation"));
            }
        }
        #endregion Property


        #region Constructor
        public RTMShowChartViewModel()
        {
            try
            {
                ActionLoadCommand = new DevExpress.Mvvm.DelegateCommand<object>(ChartLoadAction);
                ChartDashboardSaleCustomDrawCrosshairCommand = new DevExpress.Mvvm.DelegateCommand<object>(ChartDashboardSaleCustomDrawCrosshairCommandAction);
                CloseButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
            }
            catch (Exception ex)
            {

            }

        }

        #endregion Constructor



        #region Methods

        public void Init()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init ...", category: Category.Info, priority: Priority.Low);

                dt = new DataTable();
                dt.Columns.Add("Month");
                dt.Columns.Add("Year");
                dt.Columns.Add("MonthYear");
                dt.Columns.Add("Components");
                dt.Columns.Add("OnHold-T-S");
                dt.Columns.Add(">7Day");
                dt.Columns.Add("<=7Day");
                dt.Columns.Add("Delay");
                dt.Columns.Add("Quality");
                dt.Columns.Add("GoAhead");
                dt.Columns.Add("NewYear");
                CreateTable();  // Fill data for Stages other than COM & CAD


                dp = new DataTable();
                dp.Columns.Add("Month");
                dp.Columns.Add("Year");
                dp.Columns.Add("MonthYear");
                dp.Columns.Add("Components");
                dp.Columns.Add("OnHold-T-S");
                dp.Columns.Add(">7Day");
                dp.Columns.Add("<=7Day");
                dp.Columns.Add("Delay");
                dp.Columns.Add("Quality");
                dp.Columns.Add("GoAhead");
                dp.Columns.Add("NewYear");
                ActionCreateTable();  // Fill data for COM & CAD Stages 

                TotalQuantity = ConditionGreaterThan7Days + ConditionLessThanEqual7Day + ConditionDelay + ConditionQuality + ConditionGoAhead;
                LoadsxWorkstation = System.Windows.Application.Current.FindResource("LoadsxWorkstation").ToString();

                GeosApplication.Instance.Logger.Log("Method Init() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Init()", category: Category.Exception, priority: Priority.Low);
            }
}

        private void ChartLoadAction(object obj)
        {

            try
            {
                GeosApplication.Instance.Logger.Log("Method ChartLoadAction ...", category: Category.Info, priority: Priority.Low);

                chartControl = (ChartControl)obj;
                if (chartControl.Name == "ChartActions")
                {
                    ChartCADCOM = chartControl;
                }

                // chartcontrol.DataSource = GraphDataTable;
                chartControl.BeginInit();
                XYDiagram2D diagram = new XYDiagram2D();
                chartControl.Diagram = diagram;
                //diagram.ActualAxisX.Title = new AxisTitle() { Content = "Months" };
                //diagram.ActualAxisY.Title = new AxisTitle() { Content = "Amount" };
                diagram.ActualAxisX.QualitativeScaleOptions = new QualitativeScaleOptions();
                diagram.ActualAxisX.QualitativeScaleOptions.AutoGrid = false;
                // GeosApplication.SetCurrencySymbol(GeosApplication.Instance.CurrentCurrencySymbol);
                diagram.ActualAxisY.NumericOptions = new NumericOptions();
                //     diagram.ActualAxisY.GridSpacing = 2000; // Set the interval of 50 values on the y-axis
                diagram.ActualAxisY.NumericOptions.Format = NumericFormat.General;

                List<string> monthNameList = new List<string>();
                monthNameList.Add("OnHold-T-S");
                monthNameList.Add(">7Day");
                monthNameList.Add("<=7Day");
                monthNameList.Add("Delay");
                monthNameList.Add("Quality");
                monthNameList.Add("GoAhead");

                Dictionary<string, string> LegentwithColorList = new Dictionary<string, string>();

                LegentwithColorList.Add("OnHold-T-S", "#808080");
                LegentwithColorList.Add(">7Day", "#3A9BDC");
                LegentwithColorList.Add("<=7Day", "#72cc50");
                LegentwithColorList.Add("Delay", "#FFFF00");
                LegentwithColorList.Add("Quality", "#ff0000");
                LegentwithColorList.Add("GoAhead", "#FFA500");

                LineSeries2D lineDashedTargetPlant = new LineSeries2D();
                lineDashedTargetPlant.LineStyle = new LineStyle();
                lineDashedTargetPlant.LineStyle.DashStyle = new DashStyle();
                lineDashedTargetPlant.LineStyle.Thickness = 2;
                lineDashedTargetPlant.LineStyle.DashStyle.Dashes = new DoubleCollection(new double[] { 3, 3, 3, 3 });
                lineDashedTargetPlant.ArgumentScaleType = ScaleType.Auto;
                lineDashedTargetPlant.Brush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#808080"));
                lineDashedTargetPlant.ValueScaleType = ScaleType.Numerical;
                lineDashedTargetPlant.DisplayName = "Components";
                lineDashedTargetPlant.CrosshairLabelPattern = "{S} : {V:n0}";
                lineDashedTargetPlant.ArgumentDataMember = "Year";
                lineDashedTargetPlant.ArgumentDataMember = "MonthYear";
                lineDashedTargetPlant.ValueDataMember = "Components";
                chartControl.Diagram.Series.Add(lineDashedTargetPlant);

                foreach (var item in monthNameList)
                {

                    BarSideBySideStackedSeries2D barSideBySideStackedSeries2Dhidden = new BarSideBySideStackedSeries2D();
                    barSideBySideStackedSeries2Dhidden.Brush = (SolidColorBrush)(new BrushConverter().ConvertFrom(LegentwithColorList.FirstOrDefault(i => i.Key.ToLower() == item.ToLower()).Value));
                    chartControl.Diagram.Series.Add(barSideBySideStackedSeries2Dhidden);
                    barSideBySideStackedSeries2Dhidden.DisplayName = item;
                    //if (TotalStagesQTY != 0)
                    //{
                    //    diagram.ActualAxisX.Label.TextPattern = Convert.ToString(TotalStagesQTY);

                    //}
                    if (item != null)
                    {
                        BarSideBySideStackedSeries2D barSideBySideStackedSeries2D = new BarSideBySideStackedSeries2D();
                        barSideBySideStackedSeries2D.ArgumentScaleType = ScaleType.Auto;
                        barSideBySideStackedSeries2D.ValueScaleType = ScaleType.Numerical;
                        barSideBySideStackedSeries2D.DisplayName = item;

                        if (barSideBySideStackedSeries2D.DisplayName == "OnHold-T-S")
                        {
                            barSideBySideStackedSeries2D.Brush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#808080"));
                            // barSideBySideStackedSeries2D.ArgumentDataMember = "Year";
                            barSideBySideStackedSeries2D.ArgumentDataMember = "MonthYear";
                            barSideBySideStackedSeries2D.ValueDataMember = item;
                            barSideBySideStackedSeries2D.CrosshairLabelPattern = "{S} : {V:n0}";
                            barSideBySideStackedSeries2D.Model = new DevExpress.Xpf.Charts.SimpleBar2DModel();
                            barSideBySideStackedSeries2D.ShowInLegend = false;
                            barSideBySideStackedSeries2D.LabelsVisibility = false;
                            barSideBySideStackedSeries2D.AnimationAutoStartMode = AnimationAutoStartMode.SetStartState;
                            Bar2DSlideFromTopAnimation seriesAnimation = new Bar2DSlideFromTopAnimation();
                            seriesAnimation.Duration = new TimeSpan(0, 0, 2);
                            barSideBySideStackedSeries2D.PointAnimation = seriesAnimation;

                            diagram.Series.Add(barSideBySideStackedSeries2D);


                        }
                        else if (barSideBySideStackedSeries2D.DisplayName == "Quality")
                        {
                            barSideBySideStackedSeries2D.Brush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#ff0000"));
                            barSideBySideStackedSeries2D.ArgumentDataMember = "Month";
                            // barSideBySideStackedSeries2D.ArgumentDataMember = "Year";
                            barSideBySideStackedSeries2D.ArgumentDataMember = "MonthYear";
                            barSideBySideStackedSeries2D.ValueDataMember = item;
                            barSideBySideStackedSeries2D.CrosshairLabelPattern = "{S} : {V:n0}";
                            barSideBySideStackedSeries2D.Model = new DevExpress.Xpf.Charts.SimpleBar2DModel();
                            barSideBySideStackedSeries2D.ShowInLegend = false;

                            barSideBySideStackedSeries2D.AnimationAutoStartMode = AnimationAutoStartMode.SetStartState;
                            Bar2DSlideFromTopAnimation seriesAnimation = new Bar2DSlideFromTopAnimation();
                            seriesAnimation.Duration = new TimeSpan(0, 0, 2);
                            barSideBySideStackedSeries2D.PointAnimation = seriesAnimation;

                            diagram.Series.Add(barSideBySideStackedSeries2D);


                        }
                        else if (barSideBySideStackedSeries2D.DisplayName == ">7Day")
                        {
                            barSideBySideStackedSeries2D.Brush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#3A9BDC"));
                            barSideBySideStackedSeries2D.ArgumentDataMember = "Month";
                            // barSideBySideStackedSeries2D.ArgumentDataMember = "Year";
                            barSideBySideStackedSeries2D.ArgumentDataMember = "MonthYear";
                            barSideBySideStackedSeries2D.ValueDataMember = item;
                            barSideBySideStackedSeries2D.CrosshairLabelPattern = "{S} : {V:n0}";
                            barSideBySideStackedSeries2D.Model = new DevExpress.Xpf.Charts.SimpleBar2DModel();

                            barSideBySideStackedSeries2D.ShowInLegend = false;
                            barSideBySideStackedSeries2D.LabelsVisibility = false;
                            SeriesLabel SeriesLabel = new SeriesLabel();
                            SeriesLabel.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#ff0000"));
                            barSideBySideStackedSeries2D.ColorDataMember = SeriesLabel.Foreground.ToString();
                            barSideBySideStackedSeries2D.AnimationAutoStartMode = AnimationAutoStartMode.SetStartState;
                            Bar2DSlideFromTopAnimation seriesAnimation = new Bar2DSlideFromTopAnimation();
                            seriesAnimation.Duration = new TimeSpan(0, 0, 2);
                            barSideBySideStackedSeries2D.PointAnimation = seriesAnimation;

                            diagram.Series.Add(barSideBySideStackedSeries2D);


                        }
                        else if (barSideBySideStackedSeries2D.DisplayName == "<=7Day")
                        {
                            barSideBySideStackedSeries2D.Brush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#72cc50"));
                            barSideBySideStackedSeries2D.ArgumentDataMember = "Month";
                            //  barSideBySideStackedSeries2D.ArgumentDataMember = "Year"; 
                            barSideBySideStackedSeries2D.ArgumentDataMember = "MonthYear";
                            barSideBySideStackedSeries2D.ValueDataMember = item;

                            barSideBySideStackedSeries2D.CrosshairLabelPattern = "{S} : {V:n0}";
                            barSideBySideStackedSeries2D.Model = new DevExpress.Xpf.Charts.SimpleBar2DModel();
                            barSideBySideStackedSeries2D.ShowInLegend = false;
                            barSideBySideStackedSeries2D.LabelsVisibility = false;

                            //SeriesLabel SeriesLabel = new SeriesLabel();
                            //  SeriesLabel.Foreground.Transform = Labels.p;
                            //  barSideBySideStackedSeries2D.Label.Contains(SeriesLabel.Foreground) = SeriesLabel.Foreground ;
                            barSideBySideStackedSeries2D.AnimationAutoStartMode = AnimationAutoStartMode.SetStartState;
                            Bar2DSlideFromTopAnimation seriesAnimation = new Bar2DSlideFromTopAnimation();
                            seriesAnimation.Duration = new TimeSpan(0, 0, 2);
                            barSideBySideStackedSeries2D.PointAnimation = seriesAnimation;

                            diagram.Series.Add(barSideBySideStackedSeries2D);


                        }
                        else if (barSideBySideStackedSeries2D.DisplayName == "Delay")
                        {
                            barSideBySideStackedSeries2D.Brush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFF00"));
                            barSideBySideStackedSeries2D.ArgumentDataMember = "Month";
                            //barSideBySideStackedSeries2D.ArgumentDataMember = "Year";
                            barSideBySideStackedSeries2D.ArgumentDataMember = "MonthYear";
                            barSideBySideStackedSeries2D.ValueDataMember = item;
                            barSideBySideStackedSeries2D.CrosshairLabelPattern = "{S} : {V:n0}";
                            barSideBySideStackedSeries2D.Model = new DevExpress.Xpf.Charts.SimpleBar2DModel();
                            barSideBySideStackedSeries2D.ShowInLegend = false;

                            barSideBySideStackedSeries2D.AnimationAutoStartMode = AnimationAutoStartMode.SetStartState;
                            Bar2DSlideFromTopAnimation seriesAnimation = new Bar2DSlideFromTopAnimation();
                            seriesAnimation.Duration = new TimeSpan(0, 0, 2);
                            barSideBySideStackedSeries2D.PointAnimation = seriesAnimation;

                            diagram.Series.Add(barSideBySideStackedSeries2D);


                        }
                        else if (barSideBySideStackedSeries2D.DisplayName == "GoAhead")
                        {
                            barSideBySideStackedSeries2D.Brush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFA500"));
                            barSideBySideStackedSeries2D.ArgumentDataMember = "Month";
                            // barSideBySideStackedSeries2D.ArgumentDataMember = "Year";
                            barSideBySideStackedSeries2D.ArgumentDataMember = "MonthYear";
                            barSideBySideStackedSeries2D.ValueDataMember = item;
                            barSideBySideStackedSeries2D.CrosshairLabelPattern = "{S} : {V:n0}";
                            barSideBySideStackedSeries2D.Model = new DevExpress.Xpf.Charts.SimpleBar2DModel();
                            barSideBySideStackedSeries2D.ShowInLegend = false;

                            barSideBySideStackedSeries2D.AnimationAutoStartMode = AnimationAutoStartMode.SetStartState;
                            Bar2DSlideFromTopAnimation seriesAnimation = new Bar2DSlideFromTopAnimation();
                            seriesAnimation.Duration = new TimeSpan(0, 0, 2);
                            barSideBySideStackedSeries2D.PointAnimation = seriesAnimation;

                            diagram.Series.Add(barSideBySideStackedSeries2D);


                        }
                    }

                }

                //diagram.ActualAxisX.ActualLabel.Angle = 270;
                DataTable dtTempCADCOM = new DataTable();
                DataTable dtTemp = new DataTable();

                if (chartControl.Name == "ChartSalesStatusByMonths")
                {
                    if (chartControl.ActualWidth < 860)
                    {
                        diagram.ActualAxisX.ActualLabel.Angle = 270;
                        diagram.ActualAxisX.ActualLabel.FontSize = 10;


                        //dtTempCADCOM = ActionGraphDataTable;

                        //foreach (DataRow dr in dtTempCADCOM.Rows)
                        //{
                        //    dr["MonthYear"] = dr["NewYear"].ToString();
                        //}

                        //ActionGraphDataTable = dtTempCADCOM;

                        //dtTemp = GraphDataTable;
                        //foreach (DataRow drTemp in dtTemp.Rows)
                        //{
                        //    drTemp["MonthYear"] = drTemp["NewYear"].ToString();
                        //}

                    //    GraphDataTable = dtTemp;

                        ChartReLoadAction(ChartCADCOM, chartControl.ActualWidth);

                    }
                }

                chartControl.EndInit();
                chartControl.AnimationMode = ChartAnimationMode.OnDataChanged;
                chartControl.Animate();

                GeosApplication.Instance.Logger.Log("Method ChartLoadAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in ChartLoadAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
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
                        if (item.Series.DisplayName == "Components")
                            group.CrosshairElements.Add(item);
                        else
                            group.CrosshairElements.Insert(0, item);
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ChartDashboardSaleCustomDrawCrosshairCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        //private void ActionCreateTable()
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method ActionCreateTable ...", category: Category.Info, priority: Priority.Low);

        //        dp.Rows.Clear();

        //        var AllStages = ERMDeliveryVisualManagementList.GroupBy(x => x.IdStage)
        //           .Select(group => new
        //           {
        //               Sequence = ERMDeliveryVisualManagementList.FirstOrDefault(a => a.IdStage == group.Key).Sequence,
        //               NewSequence = ERMDeliveryVisualManagementList.FirstOrDefault(a => a.IdStage == group.Key).NewSequence,
        //               IdStage = ERMDeliveryVisualManagementList.FirstOrDefault(a => a.IdStage == group.Key).IdStage,
        //               StageCode = ERMDeliveryVisualManagementList.FirstOrDefault(a => a.IdStage == group.Key).StageCode,
        //               Count = ERMDeliveryVisualManagementList.Where(b => b.IdStage == 0).Count(),
        //           }).ToList().OrderBy(i => i.NewSequence);


        //        DateTime TodaysDate = DateTime.Now.Date;

        //        #region [Pallavi Jadhav][GEOS2-4343][23-05-2023] -- Show stage only if present in ActiveInPlants
        //        foreach (var stage in ERMDeliveryVisualManagementStagesList.Where(x => x.StageCode == "COM" || x.StageCode == "CAD").ToList())
        //        {

        //            if (stage.ActiveInPlants != null && stage.ActiveInPlants != "")
        //            {
        //                ArrActiveInPlants = stage.ActiveInPlants.Split(',');
        //                List<Site> tmpSelectedPlant = ERMCommon.Instance.SelectedAuthorizedPlantsList.Cast<Site>().ToList();
        //                FlagPresentInActivePlants = false;
        //                foreach (Site itemSelectedPlant in tmpSelectedPlant)
        //                {
        //                    if (FlagPresentInActivePlants == true)
        //                    {
        //                        break;
        //                    }

        //                    foreach (var iPlant in ArrActiveInPlants)
        //                    {
        //                        if (Convert.ToUInt16(iPlant) == itemSelectedPlant.IdSite)
        //                        {
        //                            FlagPresentInActivePlants = true;
        //                            break;
        //                        }
        //                    }

        //                }
        //            }

        //            if (FlagPresentInActivePlants == true || stage.ActiveInPlants == null || stage.ActiveInPlants == "")
        //            {

        //                DataRow dr = dp.NewRow();


        //                if (stage.StageCode == "COM")
        //                {
        //                    dr[0] = null;
        //                    dr[1] = null;
        //                    dr[2] = stage.StageCode.ToString().PadLeft(2, '0');

        //                    dr["Quality"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.IdOfferType == 2 || i.IdOfferType == 3) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).Select(x => x.QTY).Sum();
        //                    ConditionQuality = ConditionQuality + Convert.ToUInt64(dr["Quality"]);
        //                    int QualityComCad = Convert.ToInt32(dr["Quality"]);

        //                    DateTime? tempDate = DateTime.Now.AddDays(7);
        //                    dr[">7Day"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.DeliveryDate.Value.Date > tempDate.Value.Date) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).Select(x => x.QTY).Sum();
        //                    ConditionGreaterThan7Days = ConditionGreaterThan7Days + Convert.ToUInt64(dr[">7Day"]);
        //                    int day7ComCad = Convert.ToInt32(dr[">7Day"]);

        //                    tempDate = DateTime.Now;
        //                    dr["Delay"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.DeliveryDate.Value.Date <= tempDate.Value.Date) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).Select(x => x.QTY).Sum();
        //                    ConditionDelay = ConditionDelay + Convert.ToUInt64(dr["Delay"]);
        //                    int DelayComCad = Convert.ToInt32(dr["Delay"]);

        //                    tempDate = DateTime.Now.AddDays(7);
        //                    dr["<=7Day"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.DeliveryDate.Value.Date > TodaysDate && i.DeliveryDate.Value.Date <= tempDate.Value.Date) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).Select(x => x.QTY).Sum();
        //                    ConditionLessThanEqual7Day = ConditionLessThanEqual7Day + Convert.ToUInt64(dr["<=7Day"]);
        //                    int Day7lessthanorEqualtoComCad = Convert.ToInt32(dr["<=7Day"]);

        //                    dr["OnHold-T-S"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.IdOfferStatusType == 18) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).Select(x => x.QTY).Sum();
        //                    ConditionOnHoldTS = ConditionOnHoldTS + Convert.ToUInt64(dr["OnHold-T-S"]);
        //                    int OnHoldTSComCad = Convert.ToInt32(dr["OnHold-T-S"]);

        //                    dr["Components"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.IdTemplate == 9) && i.CurrentWorkStation == stage.StageCode).Select(x => x.QTY).Sum();
        //                    ConditionComponents = ConditionComponents + Convert.ToUInt64(dr["Components"]);
        //                    int ComponentsComCad = Convert.ToInt32(dr["Components"]);

        //                    dr["GoAhead"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.GoAheadDate != null) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).Select(x => x.QTY).Sum();
        //                    ConditionGoAhead = ConditionGoAhead + Convert.ToUInt64(dr["GoAhead"]);
        //                    int GoAheadComCad = Convert.ToInt32(dr["GoAhead"]);

        //                    dr["Year"] = QualityComCad + day7ComCad + DelayComCad + Day7lessthanorEqualtoComCad + OnHoldTSComCad + GoAheadComCad;

        //                    //if (dr["Year"].ToString().Length == 1)
        //                    //{
        //                    //    dr[2] = dr[2].ToString() + "\n   " + dr["Year"].ToString();
        //                    //}
        //                    //else if (dr["Year"].ToString().Length == 2)
        //                    //{
        //                    //    dr[2] = dr[2].ToString() + "\n " + dr["Year"].ToString();
        //                    //}

        //                    //else
        //                    //{
        //                    //    dr[2] = dr[2].ToString() + "\n" + dr["Year"].ToString();
        //                    //}
        //                    if (dr["Year"].ToString().Length == 1)
        //                    {
        //                        dr[2] = "(" + dr["Year"].ToString() + ") " + dr[2].ToString();
        //                    }
        //                    else if (dr["Year"].ToString().Length == 2)
        //                    {
        //                        dr[2] = "(" + dr["Year"].ToString() + ") " + dr[2].ToString();
        //                    }

        //                    else
        //                    {
        //                        dr[2] = "(" + dr["Year"].ToString() + ") " + dr[2].ToString();
        //                    }

        //                    dp.Rows.Add(dr);
        //                }
        //                else
        //                     if (stage.StageCode == "CAD")
        //                {
        //                    dr[0] = null;
        //                    dr[1] = null;
        //                    dr[2] = stage.StageCode.ToString().PadLeft(2, '0');

        //                    dr["Quality"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.IdOfferType == 2 || i.IdOfferType == 3) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).Select(x => x.QTY).Sum();
        //                    ConditionQuality = ConditionQuality + Convert.ToUInt64(dr["Quality"]);
        //                    int QualityComCad = Convert.ToInt32(dr["Quality"]);

        //                    DateTime? tempDate = DateTime.Now.AddDays(7);
        //                    dr[">7Day"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.DeliveryDate.Value.Date > tempDate.Value.Date) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).Select(x => x.QTY).Sum();
        //                    ConditionGreaterThan7Days = ConditionGreaterThan7Days + Convert.ToUInt64(dr[">7Day"]);
        //                    int day7ComCad = Convert.ToInt32(dr[">7Day"]);

        //                    tempDate = DateTime.Now;
        //                    dr["Delay"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.DeliveryDate.Value.Date <= tempDate.Value.Date) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).Select(x => x.QTY).Sum();
        //                    ConditionDelay = ConditionDelay + Convert.ToUInt64(dr["Delay"]);
        //                    int DelayComCad = Convert.ToInt32(dr["Delay"]);

        //                    tempDate = DateTime.Now.AddDays(7);
        //                    dr["<=7Day"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.DeliveryDate.Value.Date > TodaysDate && i.DeliveryDate.Value.Date <= tempDate.Value.Date) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).Select(x => x.QTY).Sum();
        //                    ConditionLessThanEqual7Day = ConditionLessThanEqual7Day + Convert.ToUInt64(dr["<=7Day"]);
        //                    int Day7lessthanorEqualtoComCad = Convert.ToInt32(dr["<=7Day"]);

        //                    dr["OnHold-T-S"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.IdOfferStatusType == 18) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).Select(x => x.QTY).Sum();
        //                    ConditionOnHoldTS = ConditionOnHoldTS + Convert.ToUInt64(dr["OnHold-T-S"]);
        //                    int OnHoldTSComCad = Convert.ToInt32(dr["OnHold-T-S"]);

        //                    dr["Components"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.IdTemplate == 9) && i.CurrentWorkStation == stage.StageCode).Select(x => x.QTY).Sum();
        //                    ConditionComponents = ConditionComponents + Convert.ToUInt64(dr["Components"]);
        //                    int ComponentsComCad = Convert.ToInt32(dr["Components"]);

        //                    dr["GoAhead"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.GoAheadDate != null) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).Select(x => x.QTY).Sum();
        //                    ConditionGoAhead = ConditionGoAhead + Convert.ToUInt64(dr["GoAhead"]);
        //                    int GoAheadComCad = Convert.ToInt32(dr["GoAhead"]);

        //                    dr["Year"] = QualityComCad + day7ComCad + DelayComCad + Day7lessthanorEqualtoComCad + OnHoldTSComCad + GoAheadComCad;

        //                    if (dr["Year"].ToString().Length == 1)
        //                    {
        //                        dr[2] = dr[2].ToString() + "\n   " + dr["Year"].ToString();
        //                    }
        //                    else if (dr["Year"].ToString().Length == 2)
        //                    {
        //                        dr[2] = dr[2].ToString() + "\n " + dr["Year"].ToString();
        //                    }

        //                    else
        //                    {
        //                        dr[2] = dr[2].ToString() + "\n" + dr["Year"].ToString();
        //                    }
        //                    dp.Rows.Add(dr);

        //                }

        //            }
        //        }

        //        #endregion

        //        GeosApplication.Instance.Logger.Log("Method ActionCreateTable() executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in ActionCreateTable() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
        //    }

        //    ActionGraphDataTable = dp;
        //    if (chartControl != null) { chartControl.UpdateData(); }
        //}

        //private void CreateTable()
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method CreateTable ...", category: Category.Info, priority: Priority.Low);

        //        dt.Rows.Clear();

        //        var AllStages = ERMDeliveryVisualManagementList.GroupBy(x => x.IdStage)
        //            .Select(group => new
        //            {
        //                IdStage = ERMDeliveryVisualManagementList.FirstOrDefault(a => a.IdStage == group.Key).IdStage,
        //                //  Sequence  = ERMDeliveryVisualManagementList.FirstOrDefault(a => a.IdStage == group.Key).Sequence,
        //                NewSequence = ERMDeliveryVisualManagementList.FirstOrDefault(a => a.IdStage == group.Key).NewSequence,
        //                StageCode = ERMDeliveryVisualManagementList.FirstOrDefault(a => a.IdStage == group.Key).StageCode,
        //                Count = ERMDeliveryVisualManagementList.Where(b => b.IdStage == 0).Count(),
        //            }).ToList().OrderBy(i => i.NewSequence);


        //        DateTime TodaysDate = DateTime.Now.Date; //[GEOS2-4528][Rupali Sarode][30-05-2023]

        //        #region [Pallavi Jadhav][GEOS2-4343][23-05-2023] -- Show stage only if present in ActiveInPlants
        //        foreach (var stage in ERMDeliveryVisualManagementStagesList.Where(x => x.StageCode != "COM" && x.StageCode != "CAD").ToList())
        //        {

        //            if (stage.ActiveInPlants != null && stage.ActiveInPlants != "")
        //            {
        //                ArrActiveInPlants = stage.ActiveInPlants.Split(',');
        //                List<Site> tmpSelectedPlant = ERMCommon.Instance.SelectedAuthorizedPlantsList.Cast<Site>().ToList();
        //                FlagPresentInActivePlants = false;
        //                foreach (Site itemSelectedPlant in tmpSelectedPlant)
        //                {
        //                    if (FlagPresentInActivePlants == true)
        //                    {
        //                        break;
        //                    }

        //                    foreach (var iPlant in ArrActiveInPlants)
        //                    {
        //                        if (Convert.ToUInt16(iPlant) == itemSelectedPlant.IdSite)
        //                        {
        //                            FlagPresentInActivePlants = true;
        //                            break;
        //                        }
        //                    }

        //                }
        //            }

        //            if (FlagPresentInActivePlants == true || stage.ActiveInPlants == null || stage.ActiveInPlants == "")
        //            {

        //                DataRow dr = dt.NewRow();

        //                if (stage.StageCode == "COM" || stage.StageCode == "CAD")
        //                {
        //                }
        //                else
        //                {
        //                    dr[0] = null;
        //                    dr[1] = null;
        //                    dr[2] = stage.StageCode.ToString().PadLeft(2, '0');

        //                    dr["Quality"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.IdOfferType == 2 || i.IdOfferType == 3) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).Select(x => x.QTY).Sum();
        //                    ConditionQuality = ConditionQuality + Convert.ToUInt64(dr["Quality"]);
        //                    int Quality = Convert.ToInt32(dr["Quality"]);

        //                    DateTime? tempDate = DateTime.Now.AddDays(7);
        //                    dr[">7Day"] = ERMDeliveryVisualManagementList.Where(i => (i.PlannedDeliveryDate != null ? i.PlannedDeliveryDate.Value.Date : i.DeliveryDate.Value.Date) > tempDate.Value.Date && i.IdStage == stage.IdStage && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).Select(x => x.QTY).Sum();

        //                    ConditionGreaterThan7Days = ConditionGreaterThan7Days + Convert.ToUInt64(dr[">7Day"]);
        //                    int Day7 = Convert.ToInt32(dr[">7Day"]);

        //                    tempDate = DateTime.Now;
        //                    dr["Delay"] = ERMDeliveryVisualManagementList.Where(i => (i.PlannedDeliveryDate != null ? i.PlannedDeliveryDate.Value.Date : i.DeliveryDate.Value.Date) <= tempDate.Value.Date && i.IdStage == stage.IdStage && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).Select(x => x.QTY).Sum();

        //                    ConditionDelay = ConditionDelay + Convert.ToUInt64(dr["Delay"]);
        //                    int Delay = Convert.ToInt32(dr["Delay"]);

        //                    tempDate = DateTime.Now.AddDays(7);
        //                    dr["<=7Day"] = ERMDeliveryVisualManagementList.Where(i => (i.PlannedDeliveryDate != null ? i.PlannedDeliveryDate.Value.Date : i.DeliveryDate.Value.Date) > TodaysDate && i.DeliveryDate.Value.Date <= tempDate.Value.Date && i.IdStage == stage.IdStage && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).Select(x => x.QTY).Sum();

        //                    ConditionLessThanEqual7Day = ConditionLessThanEqual7Day + Convert.ToUInt64(dr["<=7Day"]);
        //                    int Day7lessthanequalto = Convert.ToInt32(dr["<=7Day"]);

        //                    dr["OnHold-T-S"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.IdOfferStatusType == 18) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).Select(x => x.QTY).Sum();
        //                    ConditionOnHoldTS = ConditionOnHoldTS + Convert.ToUInt64(dr["OnHold-T-S"]);
        //                    int OnHold = Convert.ToInt32(dr["OnHold-T-S"]);

        //                    dr["Components"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.IdTemplate == 9) && i.CurrentWorkStation == stage.StageCode).Select(x => x.QTY).Sum();
        //                    ConditionComponents = ConditionComponents + Convert.ToUInt64(dr["Components"]);
        //                    int Components = Convert.ToInt32(dr["Components"]);

        //                    dr["GoAhead"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.GoAheadDate != null) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).Select(x => x.QTY).Sum();
        //                    ConditionGoAhead = ConditionGoAhead + Convert.ToUInt64(dr["GoAhead"]);
        //                    int GoAhead = Convert.ToInt32(dr["GoAhead"]);

        //                    dr["Year"] = Quality + Day7 + Delay + Day7lessthanequalto + OnHold + GoAhead;
        //                    //if (dr["Year"].ToString().Length == 1)
        //                    //{
        //                    //    dr[2] = dr[2].ToString() + "\n   " + dr["Year"].ToString();
        //                    //}
        //                    //else if (dr["Year"].ToString().Length == 2)
        //                    //{
        //                    //    dr[2] = dr[2].ToString() + "\n " + dr["Year"].ToString();
        //                    //}

        //                    //else
        //                    //{
        //                    //    dr[2] = dr[2].ToString() + "\n" + dr["Year"].ToString();
        //                    //}
        //                    if (dr["Year"].ToString().Length == 1)
        //                    {
        //                        dr[2] = "(" + dr["Year"].ToString() + ") " + dr[2].ToString();
        //                    }
        //                    else if (dr["Year"].ToString().Length == 2)
        //                    {
        //                        dr[2] = "(" + dr["Year"].ToString() + ") " + dr[2].ToString();
        //                    }

        //                    else
        //                    {
        //                        dr[2] = "(" + dr["Year"].ToString() + ") " + dr[2].ToString();
        //                    }
        //                }
        //                dt.Rows.Add(dr);
        //            }


        //        }
        //        #endregion


        //        GeosApplication.Instance.Logger.Log("Method CreateTable() executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in CreateTable() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
        //    }

        //    GraphDataTable = dt;

        //    if (chartControl != null) { chartControl.UpdateData(); }
        //}

        private void ActionCreateTable()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ActionCreateTable ...", category: Category.Info, priority: Priority.Low);

                dp.Rows.Clear();
                //string[] actions = { "COM", "CAD" };

                //  foreach (var actionIndex in Enumerable.Range(0, actions.Length))

                //var AllStages = ERMDeliveryVisualManagementList.GroupBy(x => x.IdStage)
                //   .Select(group => new
                //   {
                //       Sequence = ERMDeliveryVisualManagementList.FirstOrDefault(a => a.IdStage == group.Key).Sequence,
                //       NewSequence = ERMDeliveryVisualManagementList.FirstOrDefault(a => a.IdStage == group.Key).NewSequence,
                //       IdStage = ERMDeliveryVisualManagementList.FirstOrDefault(a => a.IdStage == group.Key).IdStage,
                //       StageCode = ERMDeliveryVisualManagementList.FirstOrDefault(a => a.IdStage == group.Key).StageCode,
                //       Count = ERMDeliveryVisualManagementList.Where(b => b.IdStage == 0).Count(),
                //   }).ToList().OrderBy(i => i.NewSequence);


                DateTime TodaysDate = DateTime.Now.Date; //[GEOS2-4528][Rupali Sarode][30-05-2023]

                //foreach (var stage in AllStages)
                // foreach (var stage in AllStages.Where(x => x.StageCode == "COM" || x.StageCode == "CAD").ToList())
                #region [Pallavi Jadhav][GEOS2-4343][23-05-2023] -- Show stage only if present in ActiveInPlants
                foreach (var stage in ERMDeliveryVisualManagementStagesList.Where(x => x.StageCode == "COM" || x.StageCode == "CAD").ToList())
                {

                    if (stage.ActiveInPlants != null && stage.ActiveInPlants != "")
                    {
                        ArrActiveInPlants = stage.ActiveInPlants.Split(',');
                        List<Site> tmpSelectedPlant = ERMCommon.Instance.SelectedAuthorizedPlantsList.Cast<Site>().ToList();
                        FlagPresentInActivePlants = false;
                        foreach (Site itemSelectedPlant in tmpSelectedPlant)
                        {
                            if (FlagPresentInActivePlants == true)
                            {
                                break;
                            }

                            foreach (var iPlant in ArrActiveInPlants)
                            {
                                if (Convert.ToUInt16(iPlant) == itemSelectedPlant.IdSite)
                                {
                                    FlagPresentInActivePlants = true;
                                    break;
                                }
                            }

                        }
                    }

                    if (FlagPresentInActivePlants == true || stage.ActiveInPlants == null || stage.ActiveInPlants == "")
                    {

                        DataRow dr = dp.NewRow();

                        if (stage.StageCode == "COM" || stage.StageCode == "CAD")
                        {
                            //DataRow dr = dp.NewRow();
                            dr[0] = null;
                            dr[1] = null;
                            dr[2] = stage.StageCode.ToString().PadLeft(2, '0');

                            dr["Quality"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.IdOfferType == 2 || i.IdOfferType == 3) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).Select(x => x.QTY).Sum();

                            ConditionQuality = ConditionQuality + Convert.ToUInt64(dr["Quality"]);
                            int QualityComCad = Convert.ToInt32(dr["Quality"]);

                            DateTime? tempDate = DateTime.Now.AddDays(7);
                            //  dr[">7Day"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.CounterPSSateDate.Value.Date > tempDate.Value.Date) && i.CurrentWorkStation == stage.StageCode).Select(x => x.QTY).Sum();
                            //[GEOS2-4528][Rupali Sarode][30-05-2023]
                            dr[">7Day"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.DeliveryDate.Value.Date > tempDate.Value.Date) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).Select(x => x.QTY).Sum();

                            ConditionGreaterThan7Days = ConditionGreaterThan7Days + Convert.ToUInt64(dr[">7Day"]);
                            int day7ComCad = Convert.ToInt32(dr[">7Day"]);

                            tempDate = DateTime.Now;
                            // dr["Delay"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.CounterPSSateDate.Value.Date <= tempDate.Value.Date) && i.CurrentWorkStation == stage.StageCode).Select(x => x.QTY).Sum();
                            //[GEOS2-4528][Rupali Sarode][30-05-2023]
                            dr["Delay"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.DeliveryDate.Value.Date <= tempDate.Value.Date) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).Select(x => x.QTY).Sum();

                            ConditionDelay = ConditionDelay + Convert.ToUInt64(dr["Delay"]);
                            int DelayComCad = Convert.ToInt32(dr["Delay"]);

                            tempDate = DateTime.Now.AddDays(7);
                            // dr["<=7Day"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.CounterPSSateDate.Value.Date <= tempDate.Value.Date) && i.CurrentWorkStation == stage.StageCode).Select(x => x.QTY).Sum();
                            //[GEOS2-4528][Rupali Sarode][30-05-2023]
                            dr["<=7Day"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.DeliveryDate.Value.Date > TodaysDate && i.DeliveryDate.Value.Date <= tempDate.Value.Date) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).Select(x => x.QTY).Sum();

                            ConditionLessThanEqual7Day = ConditionLessThanEqual7Day + Convert.ToUInt64(dr["<=7Day"]);
                            int Day7lessthanorEqualtoComCad = Convert.ToInt32(dr["<=7Day"]);

                            dr["OnHold-T-S"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.IdOfferStatusType == 18) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).Select(x => x.QTY).Sum();

                            ConditionOnHoldTS = ConditionOnHoldTS + Convert.ToUInt64(dr["OnHold-T-S"]);
                            int OnHoldTSComCad = Convert.ToInt32(dr["OnHold-T-S"]);

                            dr["Components"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.IdTemplate == 9) && i.CurrentWorkStation == stage.StageCode).Select(x => x.QTY).Sum();

                            ConditionComponents = ConditionComponents + Convert.ToUInt64(dr["Components"]);
                            int ComponentsComCad = Convert.ToInt32(dr["Components"]);

                            //  dr["GoAhead"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.GoAheadDate != null && i.PODate == null) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).Select(x => x.QTY).Sum();
                            dr["GoAhead"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.GoAheadDate != null) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).Select(x => x.QTY).Sum();

                            ConditionGoAhead = ConditionGoAhead + Convert.ToUInt64(dr["GoAhead"]);
                            int GoAheadComCad = Convert.ToInt32(dr["GoAhead"]);

                            dr["Year"] = QualityComCad + day7ComCad + DelayComCad + Day7lessthanorEqualtoComCad + OnHoldTSComCad + GoAheadComCad;

                            //dr[2] = dr[2].ToString() + "\n" + dr["Year"].ToString();

                            if (dr["Year"].ToString().Length == 1)
                            {
                                dr[2] = dr[2].ToString() + "\n   " + dr["Year"].ToString();
                            }
                            else if (dr["Year"].ToString().Length == 2)
                            {
                                dr[2] = dr[2].ToString() + "\n " + dr["Year"].ToString();
                            }

                            else
                            {
                                dr[2] = dr[2].ToString() + "\n" + dr["Year"].ToString();
                            }

                            
                            dp.Rows.Add(dr);
                        }
                        

                    }
                }

                #endregion

                GeosApplication.Instance.Logger.Log("Method ActionCreateTable() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ActionCreateTable() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

            ActionGraphDataTable = dp;
            if (chartControl != null) { chartControl.UpdateData(); }
        }
        private void CreateTable()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CreateTable ...", category: Category.Info, priority: Priority.Low);

                dt.Rows.Clear();

                //var AllStages = ERMDeliveryVisualManagementList.GroupBy(x => x.IdStage)
                //    .Select(group => new
                //    {
                //        IdStage = ERMDeliveryVisualManagementList.FirstOrDefault(a => a.IdStage == group.Key).IdStage,
                //        //  Sequence  = ERMDeliveryVisualManagementList.FirstOrDefault(a => a.IdStage == group.Key).Sequence,
                //        NewSequence = ERMDeliveryVisualManagementList.FirstOrDefault(a => a.IdStage == group.Key).NewSequence,
                //        StageCode = ERMDeliveryVisualManagementList.FirstOrDefault(a => a.IdStage == group.Key).StageCode,
                //        Count = ERMDeliveryVisualManagementList.Where(b => b.IdStage == 0).Count(),
                //    }).ToList().OrderBy(i => i.NewSequence);


                //DateTime? tempDate = DateTime.Now.AddDays(7);

                DateTime TodaysDate = DateTime.Now.Date; //[GEOS2-4528][Rupali Sarode][30-05-2023]

                //foreach (var stage in AllStages.Where(x => x.StageCode != "COM" && x.StageCode != "CAD").ToList())
                #region [Pallavi Jadhav][GEOS2-4343][23-05-2023] -- Show stage only if present in ActiveInPlants
                foreach (var stage in ERMDeliveryVisualManagementStagesList.Where(x => x.StageCode != "COM" && x.StageCode != "CAD").ToList())
                {

                    if (stage.ActiveInPlants != null && stage.ActiveInPlants != "")
                    {
                        ArrActiveInPlants = stage.ActiveInPlants.Split(',');
                        List<Site> tmpSelectedPlant = ERMCommon.Instance.SelectedAuthorizedPlantsList.Cast<Site>().ToList();
                        FlagPresentInActivePlants = false;
                        foreach (Site itemSelectedPlant in tmpSelectedPlant)
                        {
                            if (FlagPresentInActivePlants == true)
                            {
                                break;
                            }

                            foreach (var iPlant in ArrActiveInPlants)
                            {
                                if (Convert.ToUInt16(iPlant) == itemSelectedPlant.IdSite)
                                {
                                    FlagPresentInActivePlants = true;
                                    break;
                                }
                            }

                        }
                    }

                    if (FlagPresentInActivePlants == true || stage.ActiveInPlants == null || stage.ActiveInPlants == "")
                    {

                        DataRow dr = dt.NewRow();



                        if (stage.StageCode == "COM" || stage.StageCode == "CAD")
                        {
                        }
                        else
                        {
                            // DataRow dr = dt.NewRow();
                            dr[0] = null;
                            dr[1] = null;
                            dr[2] = stage.StageCode.ToString().PadLeft(2, '0');

                            dr["Quality"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.IdOfferType == 2 || i.IdOfferType == 3) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).Select(x => x.QTY).Sum();
                            ConditionQuality = ConditionQuality + Convert.ToUInt64(dr["Quality"]);
                            int Quality = Convert.ToInt32(dr["Quality"]);

                            DateTime? tempDate = DateTime.Now.AddDays(7);
                            // dr[">7Day"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.CounterPSSateDate.Value.Date > tempDate.Value.Date) && i.CurrentWorkStation == stage.StageCode).Select(x => x.QTY).Sum();
                            //[GEOS2-4528][Rupali Sarode][30-05-2023]
                            // dr[">7Day"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.DeliveryDate.Value.Date > tempDate.Value.Date) && i.CurrentWorkStation == stage.StageCode).Select(x => x.QTY).Sum();
                            dr[">7Day"] = ERMDeliveryVisualManagementList.Where(i => (i.PlannedDeliveryDate != null ? i.PlannedDeliveryDate.Value.Date : i.DeliveryDate.Value.Date) > tempDate.Value.Date && i.IdStage == stage.IdStage && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).Select(x => x.QTY).Sum();

                            ConditionGreaterThan7Days = ConditionGreaterThan7Days + Convert.ToUInt64(dr[">7Day"]);
                            int Day7 = Convert.ToInt32(dr[">7Day"]);

                            tempDate = DateTime.Now;
                            // dr["Delay"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.CounterPSSateDate.Value.Date <= tempDate.Value.Date) && i.CurrentWorkStation == stage.StageCode).Select(x => x.QTY).Sum();
                            //[GEOS2-4528][Rupali Sarode][30-05-2023]
                            dr["Delay"] = ERMDeliveryVisualManagementList.Where(i => (i.PlannedDeliveryDate != null ? i.PlannedDeliveryDate.Value.Date : i.DeliveryDate.Value.Date) <= tempDate.Value.Date && i.IdStage == stage.IdStage && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).Select(x => x.QTY).Sum();

                            ConditionDelay = ConditionDelay + Convert.ToUInt64(dr["Delay"]);
                            int Delay = Convert.ToInt32(dr["Delay"]);

                            tempDate = DateTime.Now.AddDays(7);
                            //dr["<=7Day"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.CounterPSSateDate.Value.Date <= tempDate.Value.Date) && i.CurrentWorkStation == stage.StageCode).Select(x => x.QTY).Sum();
                            //[GEOS2-4528][Rupali Sarode][30-05-2023]
                            dr["<=7Day"] = ERMDeliveryVisualManagementList.Where(i => (i.PlannedDeliveryDate != null ? i.PlannedDeliveryDate.Value.Date : i.DeliveryDate.Value.Date) > TodaysDate && i.DeliveryDate.Value.Date <= tempDate.Value.Date && i.IdStage == stage.IdStage && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).Select(x => x.QTY).Sum();
                            ConditionLessThanEqual7Day = ConditionLessThanEqual7Day + Convert.ToUInt64(dr["<=7Day"]);
                            int Day7lessthanequalto = Convert.ToInt32(dr["<=7Day"]);

                            dr["OnHold-T-S"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.IdOfferStatusType == 18) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).Select(x => x.QTY).Sum();
                            ConditionOnHoldTS = ConditionOnHoldTS + Convert.ToUInt64(dr["OnHold-T-S"]);
                            int OnHold = Convert.ToInt32(dr["OnHold-T-S"]);

                            dr["Components"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.IdTemplate == 9) && i.CurrentWorkStation == stage.StageCode).Select(x => x.QTY).Sum();
                            ConditionComponents = ConditionComponents + Convert.ToUInt64(dr["Components"]);
                            int Components = Convert.ToInt32(dr["Components"]);

                            //  dr["GoAhead"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.GoAheadDate != null && i.PODate == null) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).Select(x => x.QTY).Sum();
                            dr["GoAhead"] = ERMDeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.GoAheadDate != null) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).Select(x => x.QTY).Sum();
                            ConditionGoAhead = ConditionGoAhead + Convert.ToUInt64(dr["GoAhead"]);
                            int GoAhead = Convert.ToInt32(dr["GoAhead"]);

                            dr["Year"] = Quality + Day7 + Delay + Day7lessthanequalto + OnHold + GoAhead;
                            //if (dr["Year"].ToString().Length == 1)
                            //{
                            //    dr[2] = dr[2].ToString() + "\n   " + dr["Year"].ToString();
                            //}
                            //else if (dr["Year"].ToString().Length == 2)
                            //{
                            //    dr[2] = dr[2].ToString() + "\n " + dr["Year"].ToString();
                            //}

                            //else
                            //{
                            //    dr[2] = dr[2].ToString() + "\n" + dr["Year"].ToString();
                            //}


                            //if (dr["Year"].ToString().Length == 1)
                            //{
                            //    dr[2] = "(" + dr["Year"].ToString() + ") " + dr[2].ToString();
                            //}
                            //else if (dr["Year"].ToString().Length == 2)
                            //{
                            //    dr[2] = "(" + dr["Year"].ToString() + ") " + dr[2].ToString();
                            //}

                            //else
                            //{
                            //    dr[2] = "(" + dr["Year"].ToString() + ") " + dr[2].ToString();
                            //}



                            if (dr["Year"].ToString().Length == 1)
                            {
                                dr[2] = dr[2].ToString() + "\n   " + dr["Year"].ToString();
                            }
                            else if (dr["Year"].ToString().Length == 2)
                            {
                                dr[2] = dr[2].ToString() + "\n " + dr["Year"].ToString();
                            }

                            else
                            {
                                dr[2] = dr[2].ToString() + "\n" + dr["Year"].ToString();
                            }


                        }
                        dt.Rows.Add(dr);



                    }
                    
                }
                #endregion


                GeosApplication.Instance.Logger.Log("Method CreateTable() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CreateTable() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

            GraphDataTable = dt;

            if (chartControl != null) { chartControl.UpdateData(); }
        }

        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }


        private void ChartReLoadAction(ChartControl chartControl, double ChartWidth)
        {

            try
            {
                GeosApplication.Instance.Logger.Log("Method ChartReLoadAction ...", category: Category.Info, priority: Priority.Low);

               // chartControl = (ChartControl)obj;
                //if (chartControl.Name == "ChartActions")
                //{
                //    ChartCADCOM = chartControl;
                //}

                // chartcontrol.DataSource = GraphDataTable;
                chartControl.BeginInit();
                XYDiagram2D diagram = new XYDiagram2D();
                chartControl.Diagram = diagram;
                //diagram.ActualAxisX.Title = new AxisTitle() { Content = "Months" };
                //diagram.ActualAxisY.Title = new AxisTitle() { Content = "Amount" };
                diagram.ActualAxisX.QualitativeScaleOptions = new QualitativeScaleOptions();
                diagram.ActualAxisX.QualitativeScaleOptions.AutoGrid = false;
                // GeosApplication.SetCurrencySymbol(GeosApplication.Instance.CurrentCurrencySymbol);
                diagram.ActualAxisY.NumericOptions = new NumericOptions();
                //     diagram.ActualAxisY.GridSpacing = 2000; // Set the interval of 50 values on the y-axis
                diagram.ActualAxisY.NumericOptions.Format = NumericFormat.General;

                List<string> monthNameList = new List<string>();
                monthNameList.Add("OnHold-T-S");
                monthNameList.Add(">7Day");
                monthNameList.Add("<=7Day");
                monthNameList.Add("Delay");
                monthNameList.Add("Quality");
                monthNameList.Add("GoAhead");

                Dictionary<string, string> LegentwithColorList = new Dictionary<string, string>();

                LegentwithColorList.Add("OnHold-T-S", "#808080");
                LegentwithColorList.Add(">7Day", "#3A9BDC");
                LegentwithColorList.Add("<=7Day", "#72cc50");
                LegentwithColorList.Add("Delay", "#FFFF00");
                LegentwithColorList.Add("Quality", "#ff0000");
                LegentwithColorList.Add("GoAhead", "#FFA500");

                LineSeries2D lineDashedTargetPlant = new LineSeries2D();
                lineDashedTargetPlant.LineStyle = new LineStyle();
                lineDashedTargetPlant.LineStyle.DashStyle = new DashStyle();
                lineDashedTargetPlant.LineStyle.Thickness = 2;
                lineDashedTargetPlant.LineStyle.DashStyle.Dashes = new DoubleCollection(new double[] { 3, 3, 3, 3 });
                lineDashedTargetPlant.ArgumentScaleType = ScaleType.Auto;
                lineDashedTargetPlant.Brush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#808080"));
                lineDashedTargetPlant.ValueScaleType = ScaleType.Numerical;
                lineDashedTargetPlant.DisplayName = "Components";
                lineDashedTargetPlant.CrosshairLabelPattern = "{S} : {V:n0}";
                lineDashedTargetPlant.ArgumentDataMember = "Year";
                lineDashedTargetPlant.ArgumentDataMember = "MonthYear";
                lineDashedTargetPlant.ValueDataMember = "Components";
                chartControl.Diagram.Series.Add(lineDashedTargetPlant);

                foreach (var item in monthNameList)
                {

                    BarSideBySideStackedSeries2D barSideBySideStackedSeries2Dhidden = new BarSideBySideStackedSeries2D();
                    barSideBySideStackedSeries2Dhidden.Brush = (SolidColorBrush)(new BrushConverter().ConvertFrom(LegentwithColorList.FirstOrDefault(i => i.Key.ToLower() == item.ToLower()).Value));
                    chartControl.Diagram.Series.Add(barSideBySideStackedSeries2Dhidden);
                    barSideBySideStackedSeries2Dhidden.DisplayName = item;
                    //if (TotalStagesQTY != 0)
                    //{
                    //    diagram.ActualAxisX.Label.TextPattern = Convert.ToString(TotalStagesQTY);

                    //}
                    if (item != null)
                    {
                        BarSideBySideStackedSeries2D barSideBySideStackedSeries2D = new BarSideBySideStackedSeries2D();
                        barSideBySideStackedSeries2D.ArgumentScaleType = ScaleType.Auto;
                        barSideBySideStackedSeries2D.ValueScaleType = ScaleType.Numerical;
                        barSideBySideStackedSeries2D.DisplayName = item;

                        if (barSideBySideStackedSeries2D.DisplayName == "OnHold-T-S")
                        {
                            barSideBySideStackedSeries2D.Brush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#808080"));
                            // barSideBySideStackedSeries2D.ArgumentDataMember = "Year";
                            barSideBySideStackedSeries2D.ArgumentDataMember = "MonthYear";
                            barSideBySideStackedSeries2D.ValueDataMember = item;
                            barSideBySideStackedSeries2D.CrosshairLabelPattern = "{S} : {V:n0}";
                            barSideBySideStackedSeries2D.Model = new DevExpress.Xpf.Charts.SimpleBar2DModel();
                            barSideBySideStackedSeries2D.ShowInLegend = false;
                            barSideBySideStackedSeries2D.LabelsVisibility = false;
                            barSideBySideStackedSeries2D.AnimationAutoStartMode = AnimationAutoStartMode.SetStartState;
                            Bar2DSlideFromTopAnimation seriesAnimation = new Bar2DSlideFromTopAnimation();
                            seriesAnimation.Duration = new TimeSpan(0, 0, 2);
                            barSideBySideStackedSeries2D.PointAnimation = seriesAnimation;

                            diagram.Series.Add(barSideBySideStackedSeries2D);


                        }
                        else if (barSideBySideStackedSeries2D.DisplayName == "Quality")
                        {
                            barSideBySideStackedSeries2D.Brush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#ff0000"));
                            barSideBySideStackedSeries2D.ArgumentDataMember = "Month";
                            // barSideBySideStackedSeries2D.ArgumentDataMember = "Year";
                            barSideBySideStackedSeries2D.ArgumentDataMember = "MonthYear";
                            barSideBySideStackedSeries2D.ValueDataMember = item;
                            barSideBySideStackedSeries2D.CrosshairLabelPattern = "{S} : {V:n0}";
                            barSideBySideStackedSeries2D.Model = new DevExpress.Xpf.Charts.SimpleBar2DModel();
                            barSideBySideStackedSeries2D.ShowInLegend = false;

                            barSideBySideStackedSeries2D.AnimationAutoStartMode = AnimationAutoStartMode.SetStartState;
                            Bar2DSlideFromTopAnimation seriesAnimation = new Bar2DSlideFromTopAnimation();
                            seriesAnimation.Duration = new TimeSpan(0, 0, 2);
                            barSideBySideStackedSeries2D.PointAnimation = seriesAnimation;

                            diagram.Series.Add(barSideBySideStackedSeries2D);


                        }
                        else if (barSideBySideStackedSeries2D.DisplayName == ">7Day")
                        {
                            barSideBySideStackedSeries2D.Brush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#3A9BDC"));
                            barSideBySideStackedSeries2D.ArgumentDataMember = "Month";
                            // barSideBySideStackedSeries2D.ArgumentDataMember = "Year";
                            barSideBySideStackedSeries2D.ArgumentDataMember = "MonthYear";
                            barSideBySideStackedSeries2D.ValueDataMember = item;
                            barSideBySideStackedSeries2D.CrosshairLabelPattern = "{S} : {V:n0}";
                            barSideBySideStackedSeries2D.Model = new DevExpress.Xpf.Charts.SimpleBar2DModel();

                            barSideBySideStackedSeries2D.ShowInLegend = false;
                            barSideBySideStackedSeries2D.LabelsVisibility = false;
                            SeriesLabel SeriesLabel = new SeriesLabel();
                            SeriesLabel.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#ff0000"));
                            barSideBySideStackedSeries2D.ColorDataMember = SeriesLabel.Foreground.ToString();
                            barSideBySideStackedSeries2D.AnimationAutoStartMode = AnimationAutoStartMode.SetStartState;
                            Bar2DSlideFromTopAnimation seriesAnimation = new Bar2DSlideFromTopAnimation();
                            seriesAnimation.Duration = new TimeSpan(0, 0, 2);
                            barSideBySideStackedSeries2D.PointAnimation = seriesAnimation;

                            diagram.Series.Add(barSideBySideStackedSeries2D);


                        }
                        else if (barSideBySideStackedSeries2D.DisplayName == "<=7Day")
                        {
                            barSideBySideStackedSeries2D.Brush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#72cc50"));
                            barSideBySideStackedSeries2D.ArgumentDataMember = "Month";
                            //  barSideBySideStackedSeries2D.ArgumentDataMember = "Year"; 
                            barSideBySideStackedSeries2D.ArgumentDataMember = "MonthYear";
                            barSideBySideStackedSeries2D.ValueDataMember = item;

                            barSideBySideStackedSeries2D.CrosshairLabelPattern = "{S} : {V:n0}";
                            barSideBySideStackedSeries2D.Model = new DevExpress.Xpf.Charts.SimpleBar2DModel();
                            barSideBySideStackedSeries2D.ShowInLegend = false;
                            barSideBySideStackedSeries2D.LabelsVisibility = false;

                            //SeriesLabel SeriesLabel = new SeriesLabel();
                            //  SeriesLabel.Foreground.Transform = Labels.p;
                            //  barSideBySideStackedSeries2D.Label.Contains(SeriesLabel.Foreground) = SeriesLabel.Foreground ;
                            barSideBySideStackedSeries2D.AnimationAutoStartMode = AnimationAutoStartMode.SetStartState;
                            Bar2DSlideFromTopAnimation seriesAnimation = new Bar2DSlideFromTopAnimation();
                            seriesAnimation.Duration = new TimeSpan(0, 0, 2);
                            barSideBySideStackedSeries2D.PointAnimation = seriesAnimation;

                            diagram.Series.Add(barSideBySideStackedSeries2D);


                        }
                        else if (barSideBySideStackedSeries2D.DisplayName == "Delay")
                        {
                            barSideBySideStackedSeries2D.Brush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFF00"));
                            barSideBySideStackedSeries2D.ArgumentDataMember = "Month";
                            //barSideBySideStackedSeries2D.ArgumentDataMember = "Year";
                            barSideBySideStackedSeries2D.ArgumentDataMember = "MonthYear";
                            barSideBySideStackedSeries2D.ValueDataMember = item;
                            barSideBySideStackedSeries2D.CrosshairLabelPattern = "{S} : {V:n0}";
                            barSideBySideStackedSeries2D.Model = new DevExpress.Xpf.Charts.SimpleBar2DModel();
                            barSideBySideStackedSeries2D.ShowInLegend = false;

                            barSideBySideStackedSeries2D.AnimationAutoStartMode = AnimationAutoStartMode.SetStartState;
                            Bar2DSlideFromTopAnimation seriesAnimation = new Bar2DSlideFromTopAnimation();
                            seriesAnimation.Duration = new TimeSpan(0, 0, 2);
                            barSideBySideStackedSeries2D.PointAnimation = seriesAnimation;

                            diagram.Series.Add(barSideBySideStackedSeries2D);


                        }
                        else if (barSideBySideStackedSeries2D.DisplayName == "GoAhead")
                        {
                            barSideBySideStackedSeries2D.Brush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFA500"));
                            barSideBySideStackedSeries2D.ArgumentDataMember = "Month";
                            // barSideBySideStackedSeries2D.ArgumentDataMember = "Year";
                            barSideBySideStackedSeries2D.ArgumentDataMember = "MonthYear";
                            barSideBySideStackedSeries2D.ValueDataMember = item;
                            barSideBySideStackedSeries2D.CrosshairLabelPattern = "{S} : {V:n0}";
                            barSideBySideStackedSeries2D.Model = new DevExpress.Xpf.Charts.SimpleBar2DModel();
                            barSideBySideStackedSeries2D.ShowInLegend = false;

                            barSideBySideStackedSeries2D.AnimationAutoStartMode = AnimationAutoStartMode.SetStartState;
                            Bar2DSlideFromTopAnimation seriesAnimation = new Bar2DSlideFromTopAnimation();
                            seriesAnimation.Duration = new TimeSpan(0, 0, 2);
                            barSideBySideStackedSeries2D.PointAnimation = seriesAnimation;

                            diagram.Series.Add(barSideBySideStackedSeries2D);


                        }
                    }

                }

                //diagram.ActualAxisX.ActualLabel.Angle = 270;

                //if (chartControl.Name == "ChartSalesStatusByMonths")
                //{
                //    if (chartControl.ActualWidth < 800)
                //    {
                //        diagram.ActualAxisX.ActualLabel.Angle = 270;
                //        diagram.ActualAxisX.ActualLabel.FontSize = 10;

                //        //XYDiagram2D diagramCADCAM = new XYDiagram2D();
                //        //diagramCADCAM.ActualAxisX.ActualLabel.Angle = 270;
                //        //diagramCADCAM.ActualAxisX.ActualLabel.FontSize = 10;
                //        //ChartCADCOM.Diagram = diagramCADCAM;

                //    }
                //}
               if (ChartWidth >= 450 && ChartWidth <= 480)
                {
                    chartControl.Width = 120;
                }
                diagram.ActualAxisX.ActualLabel.Angle = 270;
                diagram.ActualAxisX.ActualLabel.FontSize = 10;
                


                chartControl.EndInit();
                chartControl.AnimationMode = ChartAnimationMode.OnDataChanged;
                chartControl.Animate();

                GeosApplication.Instance.Logger.Log("Method ChartReLoadAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in ChartReLoadAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in ChartReLoadAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ChartReLoadAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        #endregion Methods
    }

}
