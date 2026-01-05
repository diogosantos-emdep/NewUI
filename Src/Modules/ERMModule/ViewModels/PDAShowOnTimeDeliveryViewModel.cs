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
    public class PDAShowOnTimeDeliveryViewModel : ViewModelBase, INotifyPropertyChanged
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
        public ICommand OnDeliveryLoadCommand1 { get; set; }//rajashri GEOS2=5916

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
        private string onTimeDelivery;
        private double oTDOffset;
           private DataTable graphDataTable;
        private DataTable graphDataOnTimeDelivery;//rajashri
          //rajashri GEOS2=5916
        private ChartControl chartControlOnTime;
        private ChartControl chartControlAvg;
        private DataTable dtontime = new DataTable();
        private DataTable dtavg = new DataTable();
     
        private DataTable onTimeDeliveryDataTable;
        private DataTable graphAvgdelivery;
        #endregion Declaration

        #region Property
        #region rajashri
        private double totalPercentage;

        public double TotalPercentage
        {
            get
            {
                return totalPercentage;
            }

            set
            {
                totalPercentage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TotalPercentage"));
            }
        }
        public DataTable GraphDataOnTimeDelivery
        {
            get { return graphDataOnTimeDelivery; }
            set { graphDataOnTimeDelivery = value; OnPropertyChanged(new PropertyChangedEventArgs("GraphDataOnTimeDelivery")); }
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

        private bool isgreater;

        public bool Isgreater
        {
            get
            {
                return isgreater;
            }

            set
            {
                isgreater = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Isgreater"));
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
        public string OnTimeDelivery
        {
            get
            {
                return onTimeDelivery;
            }

            set
            {
                onTimeDelivery = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OnTimeDelivery"));
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
        public DataTable OnTimeDeliveryDataTable
        {
            get { return onTimeDeliveryDataTable; }
            set { onTimeDeliveryDataTable = value; OnPropertyChanged(new PropertyChangedEventArgs("OnTimeDeliveryDataTable")); }
        }
        #endregion Property

        #region Constructor

        public PDAShowOnTimeDeliveryViewModel()
        {
            CloseButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
            MaximizeWindowCommand = new DelegateCommand<SizeChangedEventArgs>(MaximizeWindowCommandAction);
            OnDeliveryLoadCommand1 = new DevExpress.Mvvm.DelegateCommand<object>(OnTimeDeliveryChartLoadAction);
        }

        #endregion Constructor

        #region Methods

        public void Init()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                int CurrentYear = DateTime.Now.Year;
                int LastYear = DateTime.Now.Year - 1;

                //OnTimeDelivery = "ON TIME DELIVERY " + LastYear + "V" + CurrentYear;
                OnTimeDelivery = "ON TIME DELIVERY " + LastYear + "VS" + CurrentYear;//[GEOS2-5442][gulab lakade][01 03 2024]

                LastYearDate = LastYear;
                CurrentYearDate = CurrentYear;
                OTDOffset = -550;
                FillDataONTime();
                //FillAVGDelivery();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }

        private void FillAVGDelivery()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillAVGDelivery ...", category: Category.Info, priority: Priority.Low);
                var CurrentYear = DateTime.Now.Year;
                var LastYear = CurrentYear - 1;
                DateTime TempCurrentYearStartDate = new DateTime(CurrentYear, 1, 1);
                DateTime TempCurrentYearEndDate = new DateTime(CurrentYear, 12, 31);
                DateTime TempLastYearStartDate = new DateTime(LastYear, 1, 1);
                DateTime TempLastYearEndDate = new DateTime(LastYear, 12, 31);
                List<PlantDeliveryAnalysis> CurrentYearAVGDelivery = new List<PlantDeliveryAnalysis>();
                CurrentYearAVGDelivery = ONTimeDeliveryANDAVGDeliveryList.Where(i => i.ShippingDate != null && (i.GoAheadDate != null ? i.GoAheadDate : i.PODate).Value.Year == CurrentYear && i.ShippingDate <= TempCurrentYearEndDate && i.ShippingDate >= TempCurrentYearStartDate).ToList();

                List<int> IntCurrentYearAVGDelivery = ONTimeDeliveryANDAVGDeliveryList.Where(i => i.ShippingDate != null && ((i.GoAheadDate != null ? i.GoAheadDate : i.PODate).Value.Year == CurrentYear) && i.ShippingDate <= TempCurrentYearEndDate && i.ShippingDate >= TempCurrentYearStartDate)
                .Select(i => (i.ShippingDate - (i.GoAheadDate != null ? i.GoAheadDate : i.PODate).Value).Value.Days).ToList();


                if (CurrentYearAVGDelivery.Count > 0)
                {
                    int sumOfAVGCurrentYeardays = IntCurrentYearAVGDelivery.Sum(i => i);
                    //var temp = CurrentYearAVGDelivery.Where(a=>a.ShippingDate!=null)
                    int SumOfCurrentYearTotalDays = CurrentYearAVGDelivery.Sum(i => i.TotalDays);
                    double AVGDeliveryCurrentyear = ((double)SumOfCurrentYearTotalDays / sumOfAVGCurrentYeardays) * 100;
                    int intValue = (int)Math.Floor(AVGDeliveryCurrentyear);
                    AVGDeliveryDataCurrentYearScaleValue = Convert.ToString(intValue);
                    string formattedAVG = AVGDeliveryCurrentyear.ToString("F2");
                    AVGDeliveryDataCurrentYear = formattedAVG;
                    ERMCommon.Instance.CurrentYearDeliveryAVG = formattedAVG;
                }
                else
                {
                    AVGDeliveryDataCurrentYearScaleValue = "0";
                    ERMCommon.Instance.CurrentYearDeliveryAVG = "0";
                }

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
                    int intValue = (int)Math.Floor(AVGDeliveryLastyear);
                    AVGDeliveryDataLastYearScaleValue = Convert.ToString(intValue);
                    string formattedAVG = AVGDeliveryLastyear.ToString("F2");
                    AVGDeliveryDataLastYear = AVGDeliveryDataLastYearScaleValue;
                    ERMCommon.Instance.LastYearDeliveryAVG = AVGDeliveryDataLastYear;
                }
                else
                {
                    AVGDeliveryDataLastYearScaleValue = "0";
                    ERMCommon.Instance.LastYearDeliveryAVG = "0";
                }
                int AddcurrentyearANDLastYear = Convert.ToInt32(AVGDeliveryDataCurrentYearScaleValue) + Convert.ToInt32(AVGDeliveryDataLastYearScaleValue);
                EndValue = Convert.ToString(AddcurrentyearANDLastYear + 15);

                TotalAVGDeliveryTotal = "AVGDelivery" + CurrentYear + " " + AVGDeliveryDataCurrentYearScaleValue + "\n" + "AVGDelivery" + LastYear + " " + AVGDeliveryDataLastYear;
                GeosApplication.Instance.Logger.Log("Method FillAVGDelivery() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillAVGDelivery()", category: Category.Exception, priority: Priority.Low);

            }
        }


        //private void FillDataONTime()
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method FillDataONTime ...", category: Category.Info, priority: Priority.Low);
        //        var CurrentYear = DateTime.Now.Year;
        //        var LastYear = CurrentYear - 1;
        //        DateTime TempCurrentYearStartDate = new DateTime(CurrentYear, 1, 1);
        //        DateTime TempCurrentYearEndDate = new DateTime(CurrentYear, 12, 31);
        //        DateTime TempLastYearStartDate = new DateTime(LastYear, 1, 1);
        //        DateTime TempLastYearEndDate = new DateTime(LastYear, 12, 31);

        //        List<PlantDeliveryAnalysis> CurrentYearPlantDeliveryAnalysis = new List<PlantDeliveryAnalysis>();
        //        CurrentYearPlantDeliveryAnalysis = ONTimeDeliveryANDAVGDeliveryList.Where(i => i.DeliveryDate.HasValue && i.ShippingDate.HasValue && i.DeliveryDate.Value.Year == CurrentYear && i.ShippingDate <= TempCurrentYearEndDate && i.ShippingDate >= TempCurrentYearStartDate).ToList();
        //        if (CurrentYearPlantDeliveryAnalysis != null && CurrentYearPlantDeliveryAnalysis.Count > 0)
        //        {
        //            var CountofShippingDateLessThanEqualsToDeliveryDate = CurrentYearPlantDeliveryAnalysis.Where(a => a.ShippingDate <= a.DeliveryDate).ToList();
        //            double AVGOnTime = ((double)CountofShippingDateLessThanEqualsToDeliveryDate.Count / CurrentYearPlantDeliveryAnalysis.Count) * 100;

        //            int intValue = (int)Math.Floor(AVGOnTime);
        //            AVGONTimeDataCurrentYearScaleValue = Convert.ToString(intValue);
        //            //AVGONTimeDataCurrentYearScaleValue = AVGONTimeDataCurrentYearScaleValue + "%";
        //            string formattedAVGOnTime = AVGOnTime.ToString("F2");
        //            AVGONTimeDataCurrentYear = formattedAVGOnTime;
        //            ERMCommon.Instance.CurrentYearONTimeDeliveryAVG = AVGONTimeDataCurrentYear + "%";
        //        }
        //        else
        //        {
        //            AVGONTimeDataCurrentYear = "0";
        //        }


        //        List<PlantDeliveryAnalysis> LastYearPlantDeliveryAnalysis = new List<PlantDeliveryAnalysis>();

        //        LastYearPlantDeliveryAnalysis = ONTimeDeliveryANDAVGDeliveryList.Where(i => i.DeliveryDate.HasValue && i.ShippingDate.HasValue && i.DeliveryDate.Value.Year == LastYear && i.ShippingDate <= TempLastYearEndDate && i.ShippingDate >= TempLastYearStartDate).ToList();
        //        // var tempOntimeDeliveryAVG = ONTimeDeliveryANDAVGDeliveryList.Where(i => i.ShippingDate != null && i.DeliveryDate != null).ToList();
        //        if (LastYearPlantDeliveryAnalysis != null && LastYearPlantDeliveryAnalysis.Count > 0)
        //        {
        //            var CountofShippingDateLessThanEqualsToDeliveryDate = LastYearPlantDeliveryAnalysis.Where(a => a.ShippingDate <= a.DeliveryDate).ToList();
        //            double AVGOnTime = ((double)CountofShippingDateLessThanEqualsToDeliveryDate.Count / LastYearPlantDeliveryAnalysis.Count) * 100;
        //            int intValue = (int)Math.Floor(AVGOnTime);
        //            AVGONTimeDataLastYearScaleValue = Convert.ToString(intValue);
        //            //  AVGONTimeDataLastYearScaleValue = AVGONTimeDataLastYearScaleValue + "%";
        //            string formattedAVGOnTime = AVGOnTime.ToString("F2");
        //            AVGONTimeDataLastYear = formattedAVGOnTime;
        //            ERMCommon.Instance.LastYearONTimeDeliveryAVG = AVGONTimeDataLastYear + "%";
        //        }

        //        TotalAVGONTimeTotal = "ONTimeDelivery" + CurrentYear + " " + AVGONTimeDataCurrentYear + "%" + "\n" + "ONTimeDelivery" + LastYear + " " + AVGONTimeDataLastYear + "%";
        //        int AddcurrentyearANDLastYearOntime = Convert.ToInt32(AVGONTimeDataCurrentYearScaleValue) + Convert.ToInt32(AVGONTimeDataLastYearScaleValue);
        //        EndValueOntime = Convert.ToString(AddcurrentyearANDLastYearOntime + 15);

        //        GeosApplication.Instance.Logger.Log("Method FillDataONTime() executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in FillDataONTime()", category: Category.Exception, priority: Priority.Low);
        //    }

        //}
            public void FillDataONTime()
        {
            GeosApplication.Instance.Logger.Log("Method FillDataONTime ...", category: Category.Info, priority: Priority.Low);
            try
            {
                var CurrentYear = DateTime.Now.Year;
                var LastYear = CurrentYear - 1;
                DateTime TempCurrentYearStartDate = new DateTime(CurrentYear, 1, 1);
                DateTime TempCurrentYearEndDate = new DateTime(CurrentYear, 12, 31);
                DateTime TempLastYearStartDate = new DateTime(LastYear, 1, 1);
                DateTime TempLastYearEndDate = new DateTime(LastYear, 12, 31);
                DataTable dtontime = new DataTable();
                dtontime.Columns.Add("Year", typeof(string));
                dtontime.Columns.Add("AVGONTimeData", typeof(double));
                //dtontime = CreateDataTable();
                // Last Year Data
                List<PlantDeliveryAnalysis> LastYearPlantDeliveryAnalysis = ONTimeDeliveryANDAVGDeliveryList.Where(i => i.DeliveryDate.HasValue && i.ShippingDate.HasValue && i.DeliveryDate.Value.Year == LastYear && i.ShippingDate <= TempLastYearEndDate && i.ShippingDate >= TempLastYearStartDate).ToList();
                if (LastYearPlantDeliveryAnalysis.Count > 0)
                {
                    var CountofShippingDateLessThanEqualsToDeliveryDate = LastYearPlantDeliveryAnalysis.Where(a => a.ShippingDate <= a.DeliveryDate).ToList();
                    double AVGOnTime = ((double)CountofShippingDateLessThanEqualsToDeliveryDate.Count / LastYearPlantDeliveryAnalysis.Count) * 100;

                    string formattedAVGOnTime = AVGOnTime.ToString("F2");
                    AVGONTimeDataLastYear = formattedAVGOnTime;

                    DataRow drLastYear = dtontime.NewRow();
                    drLastYear["Year"] = LastYear.ToString();
                    drLastYear["AVGONTimeData"] = AVGONTimeDataLastYear;
                    dtontime.Rows.Add(drLastYear);
                }
                else
                {
                    AVGONTimeDataLastYear = "0";
                }
                List<PlantDeliveryAnalysis> CurrentYearPlantDeliveryAnalysis = ONTimeDeliveryANDAVGDeliveryList.Where(i => i.DeliveryDate.HasValue && i.ShippingDate.HasValue && i.DeliveryDate.Value.Year == CurrentYear && i.ShippingDate <= TempCurrentYearEndDate && i.ShippingDate >= TempCurrentYearStartDate).ToList();
                if (CurrentYearPlantDeliveryAnalysis.Count > 0)
                {
                    var CountofShippingDateLessThanEqualsToDeliveryDate = CurrentYearPlantDeliveryAnalysis.Where(a => a.ShippingDate <= a.DeliveryDate).ToList();
                    double AVGOnTime = ((double)CountofShippingDateLessThanEqualsToDeliveryDate.Count / CurrentYearPlantDeliveryAnalysis.Count) * 100;

                    string formattedAVGOnTime = AVGOnTime.ToString("F2");
                    AVGONTimeDataCurrentYear = formattedAVGOnTime;

                    DataRow drCurrentYear = dtontime.NewRow();
                    drCurrentYear["Year"] = CurrentYear.ToString();
                    drCurrentYear["AVGONTimeData"] = AVGONTimeDataCurrentYear;
                    dtontime.Rows.Add(drCurrentYear);
                }
                else
                {
                    AVGONTimeDataCurrentYear = "0";
                }
                OnTimeDeliveryDataTable = dtontime;

                //OnTimeDeliveryDataTable.Columns.Add("MonthYear", typeof(string));
                //OnTimeDeliveryDataTable.Columns.Add("AVGONTimeDataCurrentYear", typeof(double));
                //OnTimeDeliveryDataTable.Columns.Add("AVGONTimeDataLastYear", typeof(double));


                //DataRow row1 = OnTimeDeliveryDataTable.NewRow();
                //row1["Year"] = LastYear;
                //row1["AVGONTimeData"] = AVGONTimeDataLastYear;

                //OnTimeDeliveryDataTable.Rows.Add(row1);

                //    DataRow row2 = OnTimeDeliveryDataTable.NewRow();
                //    row2["MonthYear"] = CurrentYear;
                //    row2["AVGONTimeDataCurrentYear"] = AVGONTimeDataCurrentYear;
                //    OnTimeDeliveryDataTable.Rows.Add(row2);

                double lastYearValue = Convert.ToDouble(AVGONTimeDataLastYear);
                double currentYearValue = Convert.ToDouble(AVGONTimeDataCurrentYear);
                double greaterValue = 0.0, smallerValue = 0.0;
                if (currentYearValue > lastYearValue)
                {
                    greaterValue = Convert.ToDouble(AVGONTimeDataCurrentYear);
                    smallerValue = Convert.ToDouble(AVGONTimeDataLastYear);
                }
                else
                {
                    greaterValue = Convert.ToDouble(AVGONTimeDataLastYear);
                    smallerValue = Convert.ToDouble(AVGONTimeDataCurrentYear);
                }

                TotalPercentage = Math.Round(greaterValue - smallerValue,2);

                Isgreater = lastYearValue < currentYearValue;
                if (chartControlOnTime != null)
                {
                    chartControlOnTime.UpdateData();
                    OntimeDeliveryCreatechart1();
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillDataONTime()", category: Category.Exception, priority: Priority.Low);
            }
        }
        private void OnTimeDeliveryChartLoadAction(object obj)
        {

            try
            {
                GeosApplication.Instance.Logger.Log("Method OnTimeDeliveryChartLoadAction ...", category: Category.Info, priority: Priority.Low);

                chartControlOnTime = (ChartControl)obj;
                // chartcontrol.DataSource = GraphDataTable;

                OntimeDeliveryCreatechart1();



                chartControlOnTime.EndInit();
                chartControlOnTime.AnimationMode = ChartAnimationMode.OnDataChanged;
                chartControlOnTime.Animate();

                GeosApplication.Instance.Logger.Log("Method OnTimeDeliveryChartLoadAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in OnTimeDeliveryChartLoadAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in OnTimeDeliveryChartLoadAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in OnTimeDeliveryChartLoadAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }
        private void OntimeDeliveryCreatechart1()
        {
            try
            {

                XYDiagram2D diagram1 = new XYDiagram2D();
                chartControlOnTime.Diagram = diagram1;
                chartControlOnTime.BeginInit();

                diagram1.ActualAxisX.Label = new AxisLabel();
                //diagram.ActualAxisX.Label.TextPattern ="45";
                diagram1.ActualAxisY.Label = new AxisLabel();
                diagram1.ActualAxisY.Label.TextPattern = "{V:F0}%";

                // Configure the X and Y axes

                diagram1.ActualAxisX.QualitativeScaleOptions = new QualitativeScaleOptions
                {
                    AutoGrid = false
                };

                //BarSideBySideSeries2D seriesCurrentYear = new BarSideBySideSeries2D
                //{
                //    ArgumentDataMember = "MonthYear",
                //    ValueDataMember = "AVGONTimeDataCurrentYear"
                //};

                //BarSideBySideSeries2D seriesLastYear = new BarSideBySideSeries2D
                //{
                //    ArgumentDataMember = "MonthYear",
                //    ValueDataMember = "AVGONTimeDataLastYear"
                //};

                BarSideBySideSeries2D seriesCurrentYear = new BarSideBySideSeries2D
                {
                    ArgumentDataMember = "Year",
                    ValueDataMember = "AVGONTimeData",
                    BarWidth = 0.3,
                    CrosshairLabelPattern = "{V:F2}%"

                };
                Color commonColor = Color.FromRgb(130, 163, 255); // CornflowerBlue color
                seriesCurrentYear.Brush = new SolidColorBrush(commonColor);
                //seriesLastYear.Brush = new SolidColorBrush(commonColor);
                seriesCurrentYear.DataSource = OnTimeDeliveryDataTable;
                //seriesLastYear.DataSource = OnTimeDeliveryDataTable;


                //trendline
                LineSeries2D lineDashedontimeDelivery = new LineSeries2D();
                lineDashedontimeDelivery.LineStyle = new LineStyle();
                lineDashedontimeDelivery.LineStyle.DashStyle = new DashStyle();
                lineDashedontimeDelivery.LineStyle.Thickness = 2;
                lineDashedontimeDelivery.LineStyle.DashStyle.Dashes = new DoubleCollection(new double[] { 3, 3, 3, 3 });
                lineDashedontimeDelivery.CrosshairLabelVisibility = false;
                lineDashedontimeDelivery.ArgumentDataMember = "Year";
                lineDashedontimeDelivery.ValueDataMember = "AVGONTimeData";
                diagram1.Series.Add(lineDashedontimeDelivery);

                //chartControl.Diagram.Series.Add(seriesCurrentYear);
                //RefreshChartControl();

                diagram1.Series.Add(seriesCurrentYear);
                //diagram.Series.Add(seriesLastYear);
                chartControlOnTime.EndInit();
                chartControlOnTime.AnimationMode = ChartAnimationMode.OnDataChanged;
                chartControlOnTime.Animate();
                GeosApplication.Instance.Logger.Log("Method Createchart1() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in Createchart1() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
        }
        private DataTable CreateDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Year", typeof(string));
            dt.Columns.Add("AVGONTimeData", typeof(double));
            return dt;
        }
        private void MaximizeWindowCommandAction(SizeChangedEventArgs e)
        {
            OTDOffset = (e.NewSize.Width / 2) * -1;
        }

        #endregion Methods
    }
}
