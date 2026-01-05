using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Charts;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Serialization;
using DevExpress.Xpf.Grid;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.ERM;
using Emdep.Geos.Data.Common.PCM;
using Emdep.Geos.Modules.ERM.CommonClasses;
using Emdep.Geos.Modules.ERM.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using DevExpress.Xpf.Gauges;
using DevExpress.Xpf.Editors.Flyout;
using DevExpress.Xpf.WindowsUI;
using System.Collections.ObjectModel;
using DevExpress.Data;
using System.Globalization;
using System.Data;
using System.Windows.Markup;
using System.Xml;
using Emdep.Geos.UI.Commands;


namespace Emdep.Geos.Modules.ERM.ViewModels
{
    public class PDAShowAverageDaysPOViewModel : ViewModelBase, INotifyPropertyChanged
    {
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
        #endregion Events

        #region Command
        public ICommand CloseButtonCommand { get; set; }
        public ICommand ChartDaysPOtoShipmentLoadActionCommand { get; set; }
        public ICommand ChartDashboardSaleCustomDrawCrosshairCommand { get; set; }

        #endregion Command

        #region Declaration
        private List<POGoAheadAndSampleDays> pOGoAheadAndSampleDaysList;
        private List<PlantDeliveryAnalysis> plantNameList;
        private List<int> yearsAverageDayPOtoShipmentList;
        private ObservableCollection<PlantDeliveryAnalysis> plantDeliveryAnalysisList;
        private DataTable pOtoShipmentDataTable;
        private DataTable dt = new DataTable();
        private ChartControl chartControl;
        private ChartControl chartControlSample;
        XYDiagram2D diagramSample = new XYDiagram2D();
        DateTime fromDate;
        DateTime toDate;
        #endregion Declaration

        #region Property

        public DateTime FromDate
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
        public DateTime ToDate
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
        public List<POGoAheadAndSampleDays> POGoAheadAndSampleDaysList
        {
            get
            {
                return pOGoAheadAndSampleDaysList;
            }

            set
            {
                pOGoAheadAndSampleDaysList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("POGoAheadAndSampleDaysList"));
            }
        }

        public List<PlantDeliveryAnalysis> PlantNameList
        {
            get
            {
                return plantNameList;
            }

            set
            {
                plantNameList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PlantNameList"));
            }
        }

        public List<int> YearsAverageDayPOtoShipmentList
        {
            get
            {
                return yearsAverageDayPOtoShipmentList;
            }

            set
            {
                yearsAverageDayPOtoShipmentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("YearsAverageDayPOtoShipmentList"));
            }
        }
        public ObservableCollection<PlantDeliveryAnalysis> PlantDeliveryAnalysisList
        {
            get
            {
                return plantDeliveryAnalysisList;
            }

            set
            {
                plantDeliveryAnalysisList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PlantDeliveryAnalysisList"));
            }
        }
        public DataTable POtoShipmentDataTable
        {
            get { return pOtoShipmentDataTable; }
            set { pOtoShipmentDataTable = value; OnPropertyChanged(new PropertyChangedEventArgs("POtoShipmentDataTable")); }
        }

        #endregion Property

        #region Constructor

        public PDAShowAverageDaysPOViewModel()
        {
            CloseButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
            ChartDaysPOtoShipmentLoadActionCommand = new DevExpress.Mvvm.DelegateCommand<object>(ChartDaysPOtoShipmentLoadAction);
            ChartDashboardSaleCustomDrawCrosshairCommand = new DevExpress.Mvvm.DelegateCommand<object>(ChartDashboardSaleCustomDrawCrosshairCommandAction);
        }

        #endregion Constructor

        #region Methods
        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }

        public void Init()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

               // FindYearforAverageDayPOtoShipment();
                CreateDaysPOtoShipmentTable();

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //private void FindYearforAverageDayPOtoShipment()
        //{
        //    try
        //    {


        //        DateTime tempFromDate = Convert.ToDateTime(FromDate);
        //        int year1 = tempFromDate.Year;
        //        DateTime tempToDate = Convert.ToDateTime(ToDate);
        //        int Year2 = tempToDate.Year;

        //        YearsAverageDayPOtoShipmentList = GetYearsBetween(year1, Year2);

        //    }
        //    catch (Exception ex)
        //    {


        //    }
        //}
        //static List<int> GetYearsBetween(int fromYear, int toYear)
        //{

        //    List<int> yearsBetween = new List<int>();

        //    for (int year = fromYear; year <= toYear; year++)
        //    {
        //        yearsBetween.Add(year);
        //    }

        //    return yearsBetween;
        //}

        private void CreateDaysPOtoShipmentTable()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CreateDaysPOtoShipmentTable ...", category: Category.Info, priority: Priority.Low);

                //     dt.Rows.Clear();

                PlantNameList = new List<PlantDeliveryAnalysis>();
                POGoAheadAndSampleDaysList = new List<POGoAheadAndSampleDays>();

                List<Site> plantOwners = ERMCommon.Instance.SelectedAuthorizedPlantsList.Cast<Site>().ToList();
                foreach (var Plantname in plantOwners)
                {
                    Int32 IdSite = Convert.ToInt32(Plantname.IdSite);


                    int Count = YearsAverageDayPOtoShipmentList.Count();
                    foreach (int year in YearsAverageDayPOtoShipmentList)
                    {
                        POGoAheadAndSampleDays POGoAheadAndSampleDays = new POGoAheadAndSampleDays();
                        POGoAheadAndSampleDays.Plant = Plantname.Name;
                        POGoAheadAndSampleDays.Year = Convert.ToString(year);

                        // DataRow dr = dtSample.NewRow();
                        //   dr[0] = Plantname.Name.ToString().PadLeft(2, '0');

                        // dr["year"] = year;
                        string Year = Convert.ToString(year);
                        int fyear = DateTime.Now.Year;
                        //if (fyear != year)
                        //{
                        var tempPlantDeliveryAnalysisList = PlantDeliveryAnalysisList.Where(i => i.DeliveryDate.Value.Year == year).ToList();
                        if (tempPlantDeliveryAnalysisList != null)
                        {
                            List<PlantDeliveryAnalysis> templist = new List<PlantDeliveryAnalysis>();

                            templist = PlantDeliveryAnalysisList.Where(item =>
                            {
                                DateTime? selectedDate = null;
                                if (item.OTIdSite == IdSite)
                                {
                                    if (item.SamplesDate != null)
                                    {
                                        if (item.SamplesDate.Value.Year == year)
                                        {
                                            selectedDate = item.PODate != null ? item.SamplesDate : (item.SamplesDate ?? item.PODate);
                                        }
                                        else if (item.PODate != null && item.PODate.Value.Year == year)
                                        {
                                            selectedDate = item.PODate != null ? item.GoAheadDate : (item.GoAheadDate ?? item.PODate);
                                        }
                                    }
                                    else
                                    {

                                        if (item.GoAheadDate != null && item.GoAheadDate.Value.Year == year)
                                        {
                                            selectedDate = item.GoAheadDate != null ? item.GoAheadDate : item.PODate;
                                        }
                                        else if (item.PODate != null && item.PODate.Value.Year == year)
                                        {
                                            selectedDate = item.PODate != null ? item.PODate : item.GoAheadDate;
                                        }
                                    }
                                }

                                return selectedDate != null;
                            }).ToList();

                            if (templist.Count > 0)
                            {
                                var TotalRecordCount = templist.Where(a => a.ShippingDate <= a.DeliveryDate && !a.DaystoShippment.Contains("-") && a.DaystoShippment != "0").ToList();//Aishwarya Ingale[Geos2-6431]
                                if (TotalRecordCount.Count > 0)
                                {
                                    List<int> totalSampleDays = new List<int>();
                                    List<int> totalPOorGoaheadDays = new List<int>();
                                    foreach (var item in TotalRecordCount)
                                    {
                                        if (item.Sample == "C")
                                        {
                                            if (item.SamplesDate != null)
                                            {
                                                int days = (item.ShippingDate.Value.Date - item.SamplesDate.Value.Date).Days;
                                                totalSampleDays.Add(days);
                                            }

                                        }
                                        else
                                        {
                                            if (item.GoAheadDate != null)
                                            {
                                                int days = (item.ShippingDate.Value.Date - item.GoAheadDate.Value.Date).Days;
                                                totalPOorGoaheadDays.Add(days);
                                            }
                                            else
                                            {
                                                int days = (item.ShippingDate.Value.Date - item.PODate.Value.Date).Days;
                                                totalPOorGoaheadDays.Add(days);
                                            }
                                        }

                                    }
                                    int sumofSampleDays = totalSampleDays.Sum(i => i);
                                    int AVGSampleDays = sumofSampleDays / TotalRecordCount.Count;
                                    int sumofPOGoAhead = totalPOorGoaheadDays.Sum(a => a);

                                    int AVGtotalPOorGoaheadDays = sumofPOGoAhead / TotalRecordCount.Count;

                                    POGoAheadAndSampleDays.PoGoheadDays = AVGtotalPOorGoaheadDays;
                                    POGoAheadAndSampleDays.SampleDays = AVGSampleDays;
                                }

                            }
                        }
                        POGoAheadAndSampleDaysList.Add(POGoAheadAndSampleDays);
                        
                    }
                }

                GeosApplication.Instance.Logger.Log("Method CreateDaysPOtoShipmentTable() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CreateDaysPOtoShipmentTable() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

            POtoShipmentDataTable = dt;



            if (chartControl != null)
            {
                chartControl.UpdateData();
                Createloadchart();
            }
        }

        private void ChartDaysPOtoShipmentLoadAction(object obj)
        {

            try
            {
                GeosApplication.Instance.Logger.Log("Method ChartLoadAction ...", category: Category.Info, priority: Priority.Low);

                chartControlSample = (ChartControl)obj;
                // chartcontrol.DataSource = GraphDataTable;
                //chartControl.BeginInit();

                Createloadchart();



                //diagram.ActualAxisX.ActualLabel.Angle = 270;

                chartControlSample.EndInit();
                chartControlSample.AnimationMode = ChartAnimationMode.OnDataChanged;
                chartControlSample.Animate();

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

        private void Createloadchart()
        {
            XYDiagram2D diagramSample = new XYDiagram2D();
            chartControlSample.Diagram = diagramSample;
            chartControlSample.BeginInit();
            //diagram.ActualAxisX.Title = new AxisTitle() { Content = "Months" };
            //diagram.ActualAxisY.Title = new AxisTitle() { Content = "Amount" };
            diagramSample.ActualAxisX.QualitativeScaleOptions = new QualitativeScaleOptions();
            diagramSample.ActualAxisX.QualitativeScaleOptions.AutoGrid = false;
            // GeosApplication.SetCurrencySymbol(GeosApplication.Instance.CurrentCurrencySymbol);
            diagramSample.ActualAxisY.NumericOptions = new NumericOptions();
            //     diagram.ActualAxisY.GridSpacing = 2000; // Set the interval of 50 values on the y-axis
            diagramSample.ActualAxisY.NumericOptions.Format = NumericFormat.General;
            AxisY2D axisY = new AxisY2D();
            AxisTitle axisYTitle = new AxisTitle();
            axisYTitle.Content = "Zero = Agreed ETD";
            axisY.Title = axisYTitle;
            diagramSample.AxisY = axisY;
            diagramSample.ActualAxisX.Label = new AxisLabel();
            diagramSample.ActualAxisX.Label.Angle = 45;
            #region RND
            if (POGoAheadAndSampleDaysList != null)
            {
                if (POGoAheadAndSampleDaysList.Count() > 0)
                {
                    var PlantName = POGoAheadAndSampleDaysList.GroupBy(a => a.Plant).ToList();
                    if (PlantName.Count() > 0)
                    {
                        foreach (var plantitem in PlantName)
                        {
                            var YearGroup = POGoAheadAndSampleDaysList.Where(x => x.Plant == plantitem.Key).GroupBy(a => a.Year).ToList();
                            if (YearGroup.Count() > 0)
                            {
                                int YearCount = 1;
                                foreach (var yearitem in YearGroup)
                                {
                                    var YearRecord = POGoAheadAndSampleDaysList.Where(x => x.Plant == plantitem.Key && x.Year == yearitem.Key).FirstOrDefault();
                                    if (YearRecord != null)
                                    {
                                        BarSideBySideStackedSeries2D firstSeries = new BarSideBySideStackedSeries2D
                                        {
                                            DisplayName = yearitem.Key + "(Sample Days)",
                                            StackedGroup = YearCount,
                                            BarWidth = 0.5,
                                            ArgumentScaleType = ScaleType.Auto,
                                            ValueScaleType = ScaleType.Numerical,
                                            LabelsVisibility = true,
                                            ShowInLegend = false,
                                            //  Model = new Quasi3DBar2DModel()

                                            Label = new SeriesLabel
                                            {
                                                Visible = true,

                                                TextOrientation = TextOrientation.BottomToTop
                                             
                                            }
                                        };

                                        string graycolor = "#3F76BF";
                                        firstSeries.Points.Add(new SeriesPoint(plantitem.Key, YearRecord.SampleDays)
                                        {
                                            Brush = (SolidColorBrush)(new BrushConverter().ConvertFrom(graycolor))

                                        });

                                        // Create the second BarSideBySideStackedSeries2D
                                        BarSideBySideStackedSeries2D secondSeries = new BarSideBySideStackedSeries2D
                                        {
                                            DisplayName = yearitem.Key + "(GoAhead PO Days)",
                                            StackedGroup = YearCount,
                                            BarWidth = 0.5,
                                            ArgumentScaleType = ScaleType.Auto,
                                            ValueScaleType = ScaleType.Numerical,
                                            LabelsVisibility = true,
                                            ShowInLegend = false,

                                            Label = new SeriesLabel
                                            {
                                                Visible = true,

                                                TextOrientation = TextOrientation.BottomToTop
                                              
                                            }

                                        };

                                        // secondSeries.Points.Add(new SeriesPoint(plantitem.Key, YearRecord.PoGoheadDays));
                                        string bluecolor = "#9E9E9E";
                                        secondSeries.Points.Add(new SeriesPoint(plantitem.Key, YearRecord.PoGoheadDays)
                                        {
                                            Brush = (SolidColorBrush)(new BrushConverter().ConvertFrom(bluecolor)),

                                        });


                                        diagramSample.Series.Add(firstSeries);
                                        diagramSample.Series.Add(secondSeries);
                                        Legend legend = new Legend
                                        {
                                            Name = "year" + yearitem.Key,
                                            HorizontalPosition = HorizontalPosition.Center,
                                            VerticalPosition = VerticalPosition.TopOutside,
                                            Orientation = Orientation.Horizontal
                                        };

                                        chartControlSample.Legends.Add(legend);

                                        firstSeries.Legend = legend;
                                        secondSeries.Legend = legend;
                                    }
                                    YearCount++;


                                }
                            }
                            //  chartControl.Legends.Add(legend);
                        }
                    }

                }
            }

            chartControlSample.Diagram = diagramSample;

            #endregion

            chartControlSample.EndInit();
            chartControlSample.AnimationMode = ChartAnimationMode.OnDataChanged;
            chartControlSample.Animate();
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
                        if (item.Series.DisplayName == "OnHold-T-S")
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

        #endregion Methods
    }
}
