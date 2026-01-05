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
    public class PDAShowAvgDeliveryDaysViewModel : ViewModelBase, INotifyPropertyChanged
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
        public ICommand MaximizeWindowCommand { get; set; }
        public ICommand ActionLoadCommandAvgDelivery { get; set; }//rajashri GEOS2-5916

        #endregion Command

        #region Declaration
        private string aVGDeliveryDataCurrentYearScaleValue;
        private string aVGDeliveryDataLastYearScaleValue;
        private List<PlantDeliveryAnalysis> oNTimeDeliveryANDAVGDeliveryList;
        private string aVGDeliveryDataCurrentYear;
        private string aVGDeliveryDataLastYear;
        private string endValue;
        private string totalAVGDeliveryTotal;
        private string aVGONTimeDataCurrentYearScaleValue;
        private string aVGONTimeDataCurrentYear;
        private string aVGONTimeDataLastYear;
        private string aVGONTimeDataLastYearScaleValue;
        private string totalAVGONTimeTotal;
        private string endValueOnTime;
        private string avgDeliveryDays;
        private double oTDOffset;
        //rajashri GEOS2-5916
        private ChartControl chartControlAvg;
        private DataTable dtavg = new DataTable();

        #endregion Declaration

        #region Property
        public string AvgDeliveryDays
        {
            get
            {
                return avgDeliveryDays;
            }

            set
            {
                avgDeliveryDays = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AvgDeliveryDays"));
            }
        }
        
        public string AVGDeliveryDataCurrentYearScaleValue
        {
            get
            {
                return aVGDeliveryDataCurrentYearScaleValue;
            }

            set
            {
                aVGDeliveryDataCurrentYearScaleValue = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AVGDeliveryDataCurrentYearScaleValue"));
            }
        }
        public string AVGDeliveryDataLastYearScaleValue
        {
            get
            {
                return aVGDeliveryDataLastYearScaleValue;
            }

            set
            {
                aVGDeliveryDataLastYearScaleValue = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AVGDeliveryDataLastYearScaleValue"));
            }
        }

        public List<PlantDeliveryAnalysis> ONTimeDeliveryANDAVGDeliveryList
        {
            get
            {
                return oNTimeDeliveryANDAVGDeliveryList;
            }

            set
            {
                oNTimeDeliveryANDAVGDeliveryList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ONTimeDeliveryANDAVGDeliveryList"));
            }
        }
        public string AVGDeliveryDataCurrentYear
        {
            get
            {
                return aVGDeliveryDataCurrentYear;
            }

            set
            {
                aVGDeliveryDataCurrentYear = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AVGDeliveryDataCurrentYear"));
            }
        }

        public string AVGDeliveryDataLastYear
        {
            get
            {
                return aVGDeliveryDataLastYear;
            }

            set
            {
                aVGDeliveryDataLastYear = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AVGDeliveryDataLastYear"));
            }
        }

        public string EndValue
        {
            get
            {
                return endValue;
            }

            set
            {
                endValue = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EndValue"));
            }
        }

        public string TotalAVGDeliveryTotal
        {
            get
            {
                return totalAVGDeliveryTotal;
            }

            set
            {
                totalAVGDeliveryTotal = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TotalAVGDeliveryTotal"));
            }
        }

        public string AVGONTimeDataCurrentYearScaleValue
        {
            get
            {
                return aVGONTimeDataCurrentYearScaleValue;
            }

            set
            {
                aVGONTimeDataCurrentYearScaleValue = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AVGONTimeDataCurrentYearScaleValue"));
            }
        }
        public string AVGONTimeDataCurrentYear
        {
            get
            {
                return aVGONTimeDataCurrentYear;
            }

            set
            {
                aVGONTimeDataCurrentYear = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AVGONTimeDataCurrentYear"));
            }
        }


        public string AVGONTimeDataLastYear
        {
            get
            {
                return aVGONTimeDataLastYear;
            }

            set
            {
                aVGONTimeDataLastYear = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AVGONTimeDataLastYear"));
            }
        }

        public string AVGONTimeDataLastYearScaleValue
        {
            get
            {
                return aVGONTimeDataLastYearScaleValue;
            }

            set
            {
                aVGONTimeDataLastYearScaleValue = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AVGONTimeDataLastYearScaleValue"));
            }
        }

        public string TotalAVGONTimeTotal
        {
            get
            {
                return totalAVGONTimeTotal;
            }

            set
            {
                totalAVGONTimeTotal = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TotalAVGONTimeTotal"));
            }
        }
        public string EndValueOntime
        {
            get
            {
                return endValueOnTime;
            }

            set
            {
                endValueOnTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EndValueOntime"));
            }
        }
        public double OTDOffset
        {
            get { return oTDOffset; }
            set
            {
                oTDOffset = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OTDOffset"));
            }
        }
        #region rajashri geos2=5916
        public DataTable graphAvgdelivery;
        public DataTable GraphAvgdelivery
        {
            get { return graphAvgdelivery; }
            set { graphAvgdelivery = value; OnPropertyChanged(new PropertyChangedEventArgs("GraphAvgdelivery")); }
        }
        private double totalPercentage1;

        public double TotalPercentage1
        {
            get
            {
                return totalPercentage1;
            }

            set
            {
                totalPercentage1 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TotalPercentage1"));
            }
        }
        private bool isgreaterAvgdelivery;

        public bool IsgreaterAvgdelivery
        {
            get
            {
                return isgreaterAvgdelivery;
            }

            set
            {
                isgreaterAvgdelivery = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsgreaterAvgdelivery"));
            }
        }
        private int lastYearDate;
        public int LastYearDate
        {
            get
            {
                return lastYearDate;
            }

            set
            {
                lastYearDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LastYearDate"));
            }
        }
        private int currentYearDate;
        public int CurrentYearDate
        {
            get
            {
                return currentYearDate;
            }

            set
            {
                currentYearDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CurrentYearDate"));
            }
        }
        #endregion
        #endregion Property

        #region Constructor

        public PDAShowAvgDeliveryDaysViewModel()
        {
            CloseButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
            MaximizeWindowCommand = new DelegateCommand<SizeChangedEventArgs>(MaximizeWindowCommandAction);
            ActionLoadCommandAvgDelivery = new DevExpress.Mvvm.DelegateCommand<object>(AvgDeliveryChartLoadAction1);


        }

        #endregion Constructor

        #region Methods
        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }

        private void FillAVGDelivery()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillAVGDelivery ...", category: Category.Info, priority: Priority.Low);

                DataTable dtavg = new DataTable();
                dtavg.Columns.Add("Year", typeof(string));
                dtavg.Columns.Add("AVGDaysData", typeof(double));

                var CurrentYear = DateTime.Now.Year;
                var LastYear = CurrentYear - 1;
                DateTime TempCurrentYearStartDate = new DateTime(CurrentYear, 1, 1);
                DateTime TempCurrentYearEndDate = new DateTime(CurrentYear, 12, 31);
                DateTime TempLastYearStartDate = new DateTime(LastYear, 1, 1);
                DateTime TempLastYearEndDate = new DateTime(LastYear, 12, 31);
                List<PlantDeliveryAnalysis> CurrentYearAVGDelivery = new List<PlantDeliveryAnalysis>();
                CurrentYearAVGDelivery = ONTimeDeliveryANDAVGDeliveryList.Where(i => i.ShippingDate != null && (i.GoAheadDate != null ? i.GoAheadDate : i.PODate).Value.Year == CurrentYear && i.ShippingDate <= TempCurrentYearEndDate && i.ShippingDate >= TempCurrentYearStartDate).ToList();
                // CurrentYearAVGDelivery = ONTimeDeliveryANDAVGDeliveryList.Where(i => i.ShippingDate != null&& i.ShippingDate <= TempCurrentYearEndDate && i.ShippingDate >= TempCurrentYearStartDate).ToList();
                List<int> IntCurrentYearAVGDelivery = ONTimeDeliveryANDAVGDeliveryList.Where(i => i.ShippingDate != null && ((i.GoAheadDate != null ? i.GoAheadDate : i.PODate).Value.Year == CurrentYear) && i.ShippingDate <= TempCurrentYearEndDate && i.ShippingDate >= TempCurrentYearStartDate)
                .Select(i => (i.ShippingDate - (i.GoAheadDate != null ? i.GoAheadDate : i.PODate).Value).Value.Days).ToList();

                List<PlantDeliveryAnalysis> LastYearAVGDelivery = new List<PlantDeliveryAnalysis>();
                LastYearAVGDelivery = ONTimeDeliveryANDAVGDeliveryList.Where(i => i.ShippingDate != null && (i.GoAheadDate != null ? i.GoAheadDate : i.PODate).Value.Year == LastYear && i.ShippingDate <= TempLastYearEndDate && i.ShippingDate >= TempLastYearStartDate).ToList();


                List<int> IntLastYearAVGDelivery = ONTimeDeliveryANDAVGDeliveryList.Where(i => i.ShippingDate != null && ((i.GoAheadDate != null ? i.GoAheadDate : i.PODate).Value.Year == LastYear) && i.ShippingDate <= TempLastYearEndDate && i.ShippingDate >= TempLastYearStartDate)
               .Select(i => (i.ShippingDate - (i.GoAheadDate != null ? i.GoAheadDate : i.PODate).Value).Value.Days).ToList();

                //LastYearAVGDelivery = ONTimeDeliveryANDAVGDeliveryList.Where(i => i.ShippingDate != null).Where(item => item.ShippingDate <= TempLastYearEndDate && item.ShippingDate >= TempLastYearStartDate).ToList();

                if (LastYearAVGDelivery.Count > 0)
                {
                    int sumOfAVGLastYeardays = IntLastYearAVGDelivery.Sum(i => i);
                    int SumOfLastyearTotalDays = LastYearAVGDelivery.Sum(i => i.TotalDays);
                    double AVGDeliveryLastyear = ((double)SumOfLastyearTotalDays / sumOfAVGLastYeardays) * 100;
                    int intValue = (int)Math.Round(AVGDeliveryLastyear);
                    AVGDeliveryDataLastYearScaleValue = Convert.ToString(intValue);
                    string formattedAVG = AVGDeliveryLastyear.ToString("F2");
                    AVGDeliveryDataLastYear = AVGDeliveryDataLastYearScaleValue;
                    ERMCommon.Instance.LastYearDeliveryAVG = AVGDeliveryDataLastYear;

                    DataRow drLastYear = dtavg.NewRow();
                    drLastYear["Year"] = LastYear.ToString();
                    drLastYear["AVGDaysData"] = AVGDeliveryDataLastYearScaleValue;
                    dtavg.Rows.Add(drLastYear);
                }
                else
                {
                    AVGDeliveryDataLastYearScaleValue = "0";
                    ERMCommon.Instance.LastYearDeliveryAVG = "0";
                }


                if (CurrentYearAVGDelivery.Count > 0)
                {
                    int sumOfAVGCurrentYeardays = IntCurrentYearAVGDelivery.Sum(i => i);
                    //var temp = CurrentYearAVGDelivery.Where(a=>a.ShippingDate!=null)
                    int SumOfCurrentYearTotalDays = CurrentYearAVGDelivery.Sum(i => i.TotalDays);
                    double AVGDeliveryCurrentyear = ((double)SumOfCurrentYearTotalDays / sumOfAVGCurrentYeardays) * 100;
                    int intValue = (int)Math.Round(AVGDeliveryCurrentyear);
                    AVGDeliveryDataCurrentYearScaleValue = Convert.ToString(intValue);
                    string formattedAVG = AVGDeliveryCurrentyear.ToString("F2");
                    AVGDeliveryDataCurrentYear = formattedAVG;
                    ERMCommon.Instance.CurrentYearDeliveryAVG = formattedAVG;

                    DataRow drCurrentYear = dtavg.NewRow();
                    drCurrentYear["Year"] = CurrentYear.ToString();
                    drCurrentYear["AVGDaysData"] = AVGDeliveryDataCurrentYearScaleValue;
                    dtavg.Rows.Add(drCurrentYear);
                }
                else
                {
                    AVGDeliveryDataCurrentYearScaleValue = "0";
                    ERMCommon.Instance.CurrentYearDeliveryAVG = "0";
                }


                int AddcurrentyearANDLastYear = Convert.ToInt32(AVGDeliveryDataCurrentYearScaleValue) + Convert.ToInt32(AVGDeliveryDataLastYearScaleValue);
                EndValue = Convert.ToString(AddcurrentyearANDLastYear + 15);

                TotalAVGDeliveryTotal = "AVGDelivery" + CurrentYear + " " + AVGDeliveryDataCurrentYearScaleValue + "\n" + "AVGDelivery" + LastYear + " " + AVGDeliveryDataLastYear;


                GraphAvgdelivery = dtavg;


                double lastYearValue = Convert.ToDouble(AVGDeliveryDataLastYearScaleValue);
                double currentYearValue = Convert.ToDouble(AVGDeliveryDataCurrentYearScaleValue);

                double greaterValue1 = 0.0, smallerValue1 = 0.0;
                if (currentYearValue > lastYearValue)
                {
                    greaterValue1 = Convert.ToDouble(AVGDeliveryDataCurrentYearScaleValue);
                    smallerValue1 = Convert.ToDouble(AVGDeliveryDataLastYearScaleValue);
                }
                else
                {
                    greaterValue1 = Convert.ToDouble(AVGDeliveryDataLastYearScaleValue);
                    smallerValue1 = Convert.ToDouble(AVGDeliveryDataCurrentYearScaleValue);
                }

                TotalPercentage1 = greaterValue1 - smallerValue1;

                IsgreaterAvgdelivery = lastYearValue < currentYearValue;
                if (chartControlAvg != null)
                {
                    chartControlAvg.UpdateData();
                    CreatechartAvgDelivery();
                }
                GeosApplication.Instance.Logger.Log("Method FillAVGDelivery() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillAVGDelivery()", category: Category.Exception, priority: Priority.Low);

            }
        }
        private void AvgDeliveryChartLoadAction1(object obj)
        {

            try
            {
                GeosApplication.Instance.Logger.Log("Method ChartLoadAction ...", category: Category.Info, priority: Priority.Low);

                chartControlAvg = (ChartControl)obj;

                CreatechartAvgDelivery();
                //chartControlAvg.EndInit();
                //chartControlAvg.AnimationMode = ChartAnimationMode.OnDataChanged;
                //chartControlAvg.Animate();

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
        private void CreatechartAvgDelivery()
        {
            try
            {

                XYDiagram2D diagram2 = new XYDiagram2D();
                chartControlAvg.Diagram = diagram2;
                //diagram.ActualAxisY.NumericOptions = new NumericOptions();
                //diagram.ActualAxisY.NumericOptions.Format = NumericFormat.General;
                chartControlAvg.BeginInit();

                // Configure the X and Y axes
                diagram2.ActualAxisX.QualitativeScaleOptions = new QualitativeScaleOptions
                {
                    AutoGrid = false
                };

                //BarSideBySideSeries2D seriesCurrentYear = new BarSideBySideSeries2D
                //{
                //    ArgumentDataMember = "MonthYear",
                //    ValueDataMember = "AVGDeliveryDataCurrentYear"
                //};

                //BarSideBySideSeries2D seriesLastYear = new BarSideBySideSeries2D
                //{
                //    ArgumentDataMember = "MonthYear",
                //    ValueDataMember = "AVGDeliveryDataLastYear"
                //};

                BarSideBySideSeries2D seriesLastCurrentYear = new BarSideBySideSeries2D
                {
                    ArgumentDataMember = "Year",
                    ValueDataMember = "AVGDaysData",
                      BarWidth = 0.3
                };
                Color commonColor = Color.FromRgb(130, 163, 255); // CornflowerBlue color
                seriesLastCurrentYear.Brush = new SolidColorBrush(commonColor);
                seriesLastCurrentYear.DataSource = GraphAvgdelivery;

                //trendline
                LineSeries2D lineDashedontimeDelivery = new LineSeries2D();
                lineDashedontimeDelivery.LineStyle = new LineStyle();
                lineDashedontimeDelivery.LineStyle.DashStyle = new DashStyle();
                lineDashedontimeDelivery.LineStyle.Thickness = 2;
                lineDashedontimeDelivery.LineStyle.DashStyle.Dashes = new DoubleCollection(new double[] { 3, 3, 3, 3 });
                lineDashedontimeDelivery.ArgumentScaleType = ScaleType.Auto;
                lineDashedontimeDelivery.CrosshairLabelVisibility = false;
                lineDashedontimeDelivery.ValueScaleType = ScaleType.Numerical;
                lineDashedontimeDelivery.ArgumentDataMember = "Year";
                lineDashedontimeDelivery.ValueDataMember = "AVGDaysData";
                diagram2.Series.Add(lineDashedontimeDelivery);

                diagram2.Series.Add(seriesLastCurrentYear);
                chartControlAvg.EndInit();
                chartControlAvg.AnimationMode = ChartAnimationMode.OnDataChanged;
                chartControlAvg.Animate();
                GeosApplication.Instance.Logger.Log("Method CreatechartAvgDelivery() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in CreatechartAvgDelivery() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
        }
        public void Init()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                int CurrentYear = DateTime.Now.Year;
                int LastYear = DateTime.Now.Year - 1;
                LastYearDate = LastYear;
                CurrentYearDate = CurrentYear;
                //AvgDeliveryDays = "AVG DELIVERY DAYS " + LastYear + "V" + CurrentYear;
                AvgDeliveryDays = "AVG DELIVERY DAYS " + LastYear + "VS" + CurrentYear;//[GEOS2-5442][gulab lakade][01 03 2024]
                OTDOffset = -550;
                FillAVGDelivery();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void MaximizeWindowCommandAction(SizeChangedEventArgs e)
        {
            OTDOffset = (e.NewSize.Width / 2) * -1;
        }

        #endregion Methods

    }
}
