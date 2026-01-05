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
    public class PDAShowAverageDeliveryDaysXPlantViewModel : ViewModelBase, INotifyPropertyChanged
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
        public ICommand ActionLoadCommand { get; set; }
        public ICommand ChartDashboardSaleCustomDrawCrosshairCommand { get; set; }

        #endregion Command

        #region Declaration
        private DataTable dt = new DataTable();
        private DataTable graphDataTable;
        XYDiagram2D diagram = new XYDiagram2D();
        private ChartControl chartControl;
        DateTime fromDate;
        DateTime toDate;
        private List<PlantDeliveryAnalysis> plantNameList;
        private List<int> yearsList;
        private ObservableCollection<PlantDeliveryAnalysis> plantDeliveryAnalysisList;
        #endregion Declaration

        #region Property
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

        public List<int> YearsList
        {
            get
            {
                return yearsList;
            }

            set
            {
                yearsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("YearsList"));
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
        public DataTable GraphDataTable
        {
            get { return graphDataTable; }
            set { graphDataTable = value; OnPropertyChanged(new PropertyChangedEventArgs("GraphDataTable")); }
        }

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

        #endregion Property

        #region Constructor

        public PDAShowAverageDeliveryDaysXPlantViewModel()
        {
            CloseButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
            ActionLoadCommand = new DevExpress.Mvvm.DelegateCommand<object>(ChartLoadAction);
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
             //   setDefaultPeriod();
                FindYear();
                CreateTable();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //private void setDefaultPeriod()
        //{

        //    int year = DateTime.Now.Year;
        //    DateTime StartFromDate = new DateTime(year, 1, 1);
        //    DateTime EndToDate = new DateTime(year, 12, 31);
        //    FromDate = StartFromDate;
        //    ToDate = EndToDate;
        //}
        static List<int> GetYearsBetween(int fromYear, int toYear)
        {

            List<int> yearsBetween = new List<int>();

            for (int year = fromYear; year <= toYear; year++)
            {
                yearsBetween.Add(year);
            }

            return yearsBetween;
        }

        private void FindYear()
        {
            try
            {


                DateTime tempFromDate = Convert.ToDateTime(FromDate);
                int year1 = tempFromDate.Year;
                DateTime tempToDate = Convert.ToDateTime(ToDate);
                int Year2 = tempToDate.Year;

                YearsList = GetYearsBetween(year1, Year2);

                dt = new DataTable();
                dt.Columns.Add("MonthYear");
                foreach (int item in YearsList)
                {
                    dt.Columns.Add(Convert.ToString(item));
                }
            }
            catch (Exception ex)
            {


            }
        }

        private void ChartLoadAction(object obj)
        {

            try
            {
                GeosApplication.Instance.Logger.Log("Method ChartLoadAction ...", category: Category.Info, priority: Priority.Low);

                chartControl = (ChartControl)obj;
                // chartcontrol.DataSource = GraphDataTable;
                chartControl.BeginInit();

                Createchart();



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


        private void Createchart()
        {
            try
            {
                //  chartControl.BeginInit();
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
                AxisY2D axisY = new AxisY2D();
                AxisTitle axisYTitle = new AxisTitle();
                axisYTitle.Content = "Zero = Agreed ETD";
                axisY.Title = axisYTitle;
                diagram.AxisY = axisY;

                int count1 = 1;
                int columnIndex1 = 1;
                List<string> columns1 = new List<string>();
                //for (int i = 0; i < GraphDataTable.Columns.Count; i++)
                //{


                foreach (DataRow item in GraphDataTable.Rows)
                {
                    string argumentColumnName = "MonthYear";

                    System.Data.DataRow datarow = (System.Data.DataRow)item;
                    var columns = datarow.Table.Columns;

                    foreach (System.Data.DataColumn columnPrefix in columns)
                    {
                        string valueColumnName = columnPrefix.ColumnName;
                        //object columnValue = datarow[columnPrefix.ColumnName];


                        if (valueColumnName != argumentColumnName)
                        {
                            if (GraphDataTable.Columns.Contains(valueColumnName))
                            {
                                // Check if the series already exists for the column
                                BarSideBySideSeries2D existingSeries = diagram.Series
                                    .OfType<BarSideBySideSeries2D>()
                                    .FirstOrDefault(series => series.ValueDataMember == valueColumnName);

                                // If the series doesn't exist, create a new one
                                if (existingSeries == null)
                                {


                                    //int bottum = Convert.ToInt32(columnValue);
                                    //int paddingbottum = bottum * 7;
                                    BarSideBySideSeries2D series = new BarSideBySideSeries2D
                                    {
                                        Name = "series",
                                        ArgumentDataMember = argumentColumnName,
                                        ValueDataMember = valueColumnName,
                                        DisplayName = valueColumnName,
                                        LabelsVisibility = true,

                                        Label = new SeriesLabel
                                        {
                                            Visible = true,


                                            //Padding = new Thickness(0,0,0, paddingbottum)


                                        }

                                    };

                                    Legend Legend = new Legend
                                    {
                                        Name = "year" + valueColumnName,
                                        HorizontalPosition = HorizontalPosition.Center,
                                        VerticalPosition = VerticalPosition.TopOutside,
                                        Orientation = Orientation.Horizontal,
                                        // IndentFromDiagram = new Thickness(10)
                                        // MaxCrosshairContentHeight= 10
                                        // Padding = new Thickness(50, 0, 0, 0) 

                                    };


                                    series.DataSource = GraphDataTable;
                                    chartControl.Legends.Add(Legend);
                                    diagram.Series.Add(series);
                                }
                            }
                        }
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
        private void CreateTable()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CreateTable ...", category: Category.Info, priority: Priority.Low);

                dt.Rows.Clear();

                PlantNameList = new List<PlantDeliveryAnalysis>();

                List<Site> plantOwners = ERMCommon.Instance.SelectedAuthorizedPlantsList.Cast<Site>().ToList();
                foreach (var Plantname in plantOwners)
                {
                    Int32 IdSite = Convert.ToInt32(Plantname.IdSite);
                    DataRow dr = dt.NewRow();
                    dr[0] = Plantname.Name.ToString().PadLeft(2, '0');
                    int Count = YearsList.Count();
                    foreach (int year in YearsList)
                    {


                        string Year = Convert.ToString(year);
                        int fyear = DateTime.Now.Year;
                        if (fyear != year)
                        {
                            var tempPlantDeliveryAnalysisList = PlantDeliveryAnalysisList.Where(i => i.DeliveryDate.Value.Year == year).ToList();
                            if (tempPlantDeliveryAnalysisList != null)
                            {
                                List<PlantDeliveryAnalysis> templist = new List<PlantDeliveryAnalysis>();

                                templist = PlantDeliveryAnalysisList.Where(item =>
                                {
                                    DateTime? selectedDate = null;
                                    if (item.OTIdSite == IdSite)
                                    {
                                        if (item.GoAheadDate != null)
                                        {
                                            if (item.GoAheadDate.Value.Year == year)
                                            {
                                                selectedDate = item.PODate != null ? item.GoAheadDate : (item.GoAheadDate ?? item.PODate);
                                            }
                                            else if (item.PODate != null && item.PODate.Value.Year == year)
                                            {
                                                selectedDate = item.PODate != null ? item.GoAheadDate : (item.GoAheadDate ?? item.PODate);
                                            }
                                        }
                                        else
                                        {

                                            if (item.PODate != null && item.PODate.Value.Year == year)
                                            {
                                                selectedDate = item.PODate != null ? item.PODate : item.SamplesDate;
                                            }
                                            else if (item.SamplesDate != null && item.SamplesDate.Value.Year == year)
                                            {
                                                selectedDate = item.PODate != null ? item.PODate : item.SamplesDate;
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
                                        List<int> totalDays = new List<int>();
                                        foreach (var item in TotalRecordCount)
                                        {
                                            int days = (item.DeliveryDate.Value.Date - item.ShippingDate.Value.Date).Days;
                                            totalDays.Add(days);
                                        }
                                        int sumofDays = totalDays.Sum(i => i);
                                        int AVGDays = sumofDays / TotalRecordCount.Count;

                                        dr[Year] = AVGDays;
                                    }

                                    //  dr[Year] = 100;
                                }
                            }

                        }
                        else
                        {
                            var tempPlantDeliveryAnalysisList = PlantDeliveryAnalysisList.Where(i => i.DeliveryDate.Value.Year == year).ToList();
                            if (tempPlantDeliveryAnalysisList != null)
                            {
                                List<PlantDeliveryAnalysis> templist = new List<PlantDeliveryAnalysis>();

                                templist = PlantDeliveryAnalysisList.Where(item =>
                                {
                                    DateTime? selectedDate = null;
                                    if (item.OTIdSite == IdSite)
                                    {
                                        if (item.GoAheadDate != null)
                                        {
                                            if (item.GoAheadDate.Value.Year == year)
                                            {
                                                selectedDate = item.PODate != null ? item.GoAheadDate : (item.GoAheadDate ?? item.PODate);
                                            }
                                            else if (item.PODate != null && item.PODate.Value.Year == year)
                                            {
                                                selectedDate = item.PODate != null ? item.GoAheadDate : (item.GoAheadDate ?? item.PODate);
                                            }
                                        }
                                        else
                                        {

                                            if (item.PODate != null && item.PODate.Value.Year == year)
                                            {
                                                selectedDate = item.PODate != null ? item.PODate : item.SamplesDate;
                                            }
                                            else if (item.SamplesDate != null && item.SamplesDate.Value.Year == year)
                                            {
                                                selectedDate = item.PODate != null ? item.PODate : item.SamplesDate;
                                            }
                                        }
                                    }

                                    return selectedDate != null;
                                }).ToList();
                                if (templist.Count > 0)
                                {

                                    //result.Where(a=>a.v)
                                    var TotalRecordCount = templist.Where(a => a.ShippingDate <= a.DeliveryDate && !a.DaystoShippment.Contains("-") && a.DaystoShippment != "0").ToList();//Aishwarya Ingale[Geos2-6431]
                                    if (TotalRecordCount.Count > 0)
                                    {
                                        List<int> totalDays = new List<int>();
                                        foreach (var item in TotalRecordCount)
                                        {
                                            int days = (item.DeliveryDate.Value.Date - item.ShippingDate.Value.Date).Days;
                                            totalDays.Add(days);
                                        }
                                        int sumofDays = totalDays.Sum(i => i);
                                        int AVGDays = sumofDays / TotalRecordCount.Count;

                                        dr[Year] = AVGDays;
                                    }

                                    //  dr[Year] = 100;
                                }
                            }
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

            if (chartControl != null)
            {
                chartControl.UpdateData();
                Createchart();
            }
        }

        #endregion Methods
    }
}
