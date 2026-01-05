using DevExpress.Mvvm;
using DevExpress.Xpf.Charts;
using DevExpress.Xpf.Scheduler;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.Crm.Emdep.Geos.Data.Common;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.Utility.Text;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace Emdep.Geos.Modules.Crm.ViewModels
{
    public class SalesPerformanceViewModel : NavigationViewModelBase, IDisposable
    {
        #region Declaration

        ICrmService CrmStartUp;
        private IList<Offer> salesPerformanceList;
        private IList<Offer> tempSalesPerformanceList;

        public IList<Offer> TempSalesPerformanceList
        {
            get { return tempSalesPerformanceList; }
            set { tempSalesPerformanceList = value; }
        }
        private IList<SalesTargetBySite> salesPerformanceTargetList;
        private IList<Offer> offersList;
        private IList<Offer> leadsList;
        private IList<Offer> salesStatusByMonthList;

        public IList<Offer> SalesStatusByMonthList
        {
            get { return salesStatusByMonthList; }
            set { salesStatusByMonthList = value; }
        }

        private ObservableCollection<Dictionary<String, Object>> salePerfomanceDictionary;
        public Double TargetAmount { get; set; }
        public ObservableCollection<Dictionary<String, Object>> SalePerfomanceDictionary
        {
            get { return salePerfomanceDictionary; }
            set { salePerfomanceDictionary = value; }
        }

        private ObservableCollection<SalesStatusType> listSalesStatusType = new ObservableCollection<SalesStatusType>();
        public ObservableCollection<SalesStatusType> ListSalesStatusType
        {
            get
            {
                return listSalesStatusType;
            }

            set
            {
                SetProperty(ref listSalesStatusType, value, () => ListSalesStatusType);
            }
        }

        private double gaugeSuccessRate;
        public double GaugeSuccessRate
        {
            get { return gaugeSuccessRate; }
            set { SetProperty(ref gaugeSuccessRate, value, () => GaugeSuccessRate); }
        }

        private double gaugeTargetAchieved;
        public double GaugeTargetAchieved
        {
            get { return gaugeTargetAchieved; }
            set { SetProperty(ref gaugeTargetAchieved, value, () => GaugeTargetAchieved); }
        }

        public ICommand ChartLoadCommand { get; set; }
        public ICommand GaugeLoadCommand { get; set; }
        public ICommand LoadCommand { get; private set; }
        public ICommand AllowEditAppointmentCommand { get; private set; }
        public List<ModelAppointment> Appointments { get; set; }
        private string offerTotal;

        #endregion

        #region  public Properties
        public IList<Offer> SalesPerformanceList
        {
            get
            {
                return salesPerformanceList;
            }

            set
            {
                salesPerformanceList = value;
            }
        }
        public IList<SalesTargetBySite> SalesPerformanceTargetList
        {
            get { return salesPerformanceTargetList; }
            set { salesPerformanceTargetList = value; }
        }

        public IList<Offer> OffersList
        {
            get { return offersList; }
            set { offersList = value; }
        }
        public IList<Offer> LeadsList
        {
            get { return leadsList; }
            set { leadsList = value; }
        }

        public string OfferTotal
        {
            get
            {
                return offerTotal;
            }

            set
            {
                offerTotal = value; OnPropertyChanged(new PropertyChangedEventArgs("OfferTotal"));
            }
        }

        private ObservableCollection<AppointmentModel> OffersWithoutPurchaseOrdeList = new ObservableCollection<AppointmentModel>();
        DateTime fromDate;
        DateTime toDate;
        public ObservableCollection<DateTime> SelectedDates { get; set; }
        public DateTime SelectedDates1 { get; set; }
        public ObservableCollection<AppointmentModel> ProjectTasksDeliveryList;
        XYDiagram2D diagram = new XYDiagram2D();


        public ICommand SelectDateCommand { get; private set; }
        public List<object> SelectedFilters { get; set; }

        DataTable dt = null;

        #endregion
        #region  public event
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region  Constructor

        public SalesPerformanceViewModel()
        {
            CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString(), GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

            fillSalesPerformance();
            fillSalesPerformanceTarget();
            fillOrder();
            getCurrentYearWeek();
            ChartLoadCommand = new DevExpress.Mvvm.DelegateCommand<object>(ChartLoadAction);
            LoadCommand = new DevExpress.Mvvm.DelegateCommand<object>(LoadAction);
            GaugeLoadCommand = new Prism.Commands.DelegateCommand<object>(GaugeLoadAction);
            SelectDateCommand = new DevExpress.Mvvm.DelegateCommand<EventArgs>(SelectDatesAction);
            Appointments = new List<ModelAppointment>();           
            AllowEditAppointmentCommand = new DevExpress.Mvvm.DelegateCommand<EditAppointmentFormEventArgs>(AllowEditAppointmentAction);
            ListSalesStatusType = new ObservableCollection<SalesStatusType>(CrmStartUp.GetAllSalesStatusType().AsEnumerable());
            Appointments = CrmStartUp.GetOffersModelAppointment(GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone,2016).ToList();
        }

        private void GaugeLoadAction(object obj)
        {

        }

        private void AllowEditAppointmentAction(EditAppointmentFormEventArgs obj)
        {
            obj.Cancel = true;
        }

        private void LoadAction(object obj)
        {


            SelectedDates1 = DateTime.Today;
            LeadsList = new List<Offer>();
          
            OffersWithoutPurchaseOrdeList = new ObservableCollection<AppointmentModel>();
            ModelAppointment apt = null;
          

            DateTime baseTime = DateTime.Today;

        }
        private void SelectDatesAction(EventArgs parameter)
        {

            if (SelectedDates.Count > 0)
            {
                fromDate = SelectedDates[0].Date;
                toDate = SelectedDates[0].Date;
                if (SelectedDates.Count > 1)
                {
                    toDate = SelectedDates[SelectedDates.Count - 1].Date;
                }
                ProjectTasksDeliveryList.Clear();

                if (SelectedFilters != null)
                    foreach (var titem in SelectedFilters)
                    {

                        DataHelper data = titem as DataHelper;
                        LeadsList = new List<Offer>();
                        LeadsList = CrmStartUp.GetOffersWithoutPurchaseOrder(GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone,2016);
                        foreach (var item in LeadsList)
                        {
                            OffersWithoutPurchaseOrdeList.Add(new AppointmentModel() { Data = item, Subject = item.Description, ResourceId = item.IdOffer, StartTime = item.DeliveryDate.Value, Label = 1, EndTime = item.DeliveryDate.Value, Description = item.Description, AppointmentType = 1 });
                        }
                    }
            }
        }


        private void ChartLoadAction(object obj)
        {
            SalesStatusByMonthList = new List<Offer>();
            SalesStatusByMonthList = CrmStartUp.GetSalesStatusByMonth(GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone,2016);
            ChartControl chartcontrol = (ChartControl)obj;
            chartcontrol.DataSource = CreateTable();
            chartcontrol.BeginInit();
            XYDiagram2D diagram = new XYDiagram2D();
            chartcontrol.Diagram = diagram;      
            diagram.ActualAxisX.Title = new AxisTitle() { Content = "Months" };
            diagram.ActualAxisY.Title = new AxisTitle() { Content = "Amount" };


            foreach (var item in ListSalesStatusType)
            {
                if (item != null)
                {
                    BarSideBySideStackedSeries2D series1 = new BarSideBySideStackedSeries2D();
                    series1.ArgumentScaleType = ScaleType.Auto;
                    series1.ValueScaleType = ScaleType.Numerical;
                    series1.DisplayName = item.Name;
                    series1.Brush = (SolidColorBrush)(new BrushConverter().ConvertFrom(item.HtmlColor.ToString()));
                    series1.ArgumentDataMember = "Month";
                    series1.ValueDataMember = item.Name;
                    diagram.Series.Add(series1);
                    SimpleBar2DModel SimpleBar2DModel = new DevExpress.Xpf.Charts.SimpleBar2DModel();
                    series1.Model = SimpleBar2DModel;
            


                }
            }

            chartcontrol.Legend = new DevExpress.Xpf.Charts.Legend();
            // chartcontrol.Legend.Orientation = System.Windows.Controls.Orientation.Horizontal;
            chartcontrol.Legend.HorizontalPosition = HorizontalPosition.RightOutside;


            chartcontrol.EndInit();
        }
      


        private DataTable CreateTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Month");
            foreach (var item in ListSalesStatusType)
            {
                dt.Columns.Add(item.Name);
            }
            int i = dt.Columns.Count;
            int[] icol = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            foreach (var mt in icol)
            {
                DataRow dr = dt.NewRow();
                dr[0] = mt.ToString().PadLeft(2, '0');
                int k = 1;
                foreach (var item in dt.Columns)
                {
                    if (item.ToString() != "Month")
                    {
                        dr[k] = SalesStatusByMonthList.Where(m => m.Status == item.ToString() && m.CurrentMonth == mt).Select(mv => mv.Value).SingleOrDefault();
                        k++;
                    }
                }
                dt.Rows.Add(dr);
            }


            return dt;
        }

        #endregion

        #region  Methods

        private void fillSalesPerformance()
        {

            SalesPerformanceList = new List<Offer>();
            SalesPerformanceList = CrmStartUp.GetSalesStatus(GeosApplication.Instance.IdCurrencyByRegion, 
                GeosApplication.Instance.ActiveUser.IdUser,
            GeosApplication.Instance.ActiveUser.Company.Country.IdZone,2016); 

        }

        private void fillSalesPerformanceTarget()
        {
            SalesPerformanceTargetList = new List<SalesTargetBySite>();
            SalesPerformanceTargetList = CrmStartUp.GetSalesStatusTargetDashboard(GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone,2016);
            TargetAmount = Convert.ToDouble(SalesPerformanceTargetList[0].TargetAmount);


            Double Won = SalesPerformanceList.Where(s => s.Status == "WON").Select(x => x.Value).SingleOrDefault();

            Double TargetAchieved = Math.Round(((Won / TargetAmount) * 100), 2);

            Double Lost = SalesPerformanceList.Where(s => s.Status == "LOST").Select(x => x.Value).SingleOrDefault();

            Double SuccessRate = Math.Round(((Won / (Won + Lost)) * 100), 2);

            SalePerfomanceDictionary = new ObservableCollection<Dictionary<string, object>>();
            Dictionary<String, Object> SalePerfomanceDictionaryItems = new Dictionary<string, object>();
            SalePerfomanceDictionaryItems.Add("Name", "SUCCESS RATE");
            SalePerfomanceDictionaryItems.Add("Text", SuccessRate);
            SalePerfomanceDictionary.Add(SalePerfomanceDictionaryItems);
            Dictionary<String, Object> SalePerfomanceDictionaryItems1 = new Dictionary<string, object>();
            SalePerfomanceDictionaryItems1.Add("Name", "TARGET ACHIEVED");
            SalePerfomanceDictionaryItems1.Add("Text", TargetAchieved);

            SalePerfomanceDictionary.Add(SalePerfomanceDictionaryItems1);
            GaugeSuccessRate = SuccessRate;

            GaugeTargetAchieved = TargetAchieved;





        }
        public void getCurrentYearWeek()
        {


        }

        private void fillOrder()
        {
            OffersList = new List<Offer>();
            OffersList = CrmStartUp.GetTopOffers(GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone,20,2016);
            OfferTotal = "Total = " + Math.Round(offersList.Sum(i => i.Value), 2).ToString("C", System.Globalization.CultureInfo.CurrentCulture);
           // TextUtility.NumberToWords(offersList.Count).ToUpper();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }


        #endregion
    }


}
