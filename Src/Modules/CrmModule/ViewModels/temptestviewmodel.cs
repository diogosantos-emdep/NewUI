using DevExpress.Mvvm;
using DevExpress.Xpf.Charts;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.ServiceProcess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace Emdep.Geos.Modules.Crm.ViewModels
{
    public class temptestviewmodel : NavigationViewModelBase
    {
        XYDiagram2D diagram = new XYDiagram2D();
        private IList<Offer> salesStatusByMonthList;
        ICrmService CrmStartUp;
        public IList<Offer> SalesStatusByMonthList
        {
            get { return salesStatusByMonthList; }
            set { salesStatusByMonthList = value; }
        }
        DataTable dt = null;
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
        public ICommand ChartLoadCommand { get; set; }
        public ICommand GaugeLoadCommand { get; set; }
        public ICommand LoadCommand { get; private set; }

        public temptestviewmodel()
        {
            CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
            ChartLoadCommand = new DevExpress.Mvvm.DelegateCommand<object>(ChartLoadAction);



            //LoadCommand = new DevExpress.Mvvm.DelegateCommand<object>(LoadAction);
            //GaugeLoadCommand = new Prism.Commands.DelegateCommand<object>(GaugeLoadAction);
        }
        private void ChartLoadAction(object obj)
        {
            SalesStatusByMonthList = new List<Offer>();

            SalesStatusByMonthList = CrmStartUp.GetSalesStatusByMonth(GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, 2016, null, GeosApplication.Instance.IdUserPermission);
            ListSalesStatusType = new ObservableCollection<SalesStatusType>(CrmStartUp.GetAllSalesStatusType().AsEnumerable());
            ChartControl chartcontrol = (ChartControl)obj;
            chartcontrol.DataSource = CreateTable();
            //dt = CreateTable();
            //chartcontrol.BeginInit();

            chartcontrol.Diagram = diagram;

            // var xAxis = ((XYDiagram2D)chartcontrol.Diagram).AxisX;
            diagram.ActualAxisX.Title = new AxisTitle() { Content = "Months" };
            diagram.ActualAxisY.Title = new AxisTitle() { Content = "Amount" };

            //diagram.ActualAxisY.Logarithmic = true;
            //diagram.ActualAxisY.LogarithmicBase = 100;

            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    foreach (var item in ListSalesStatusType)
            //    {
            //        if (item != null)
            //        {
            //            BarSideBySideStackedSeries2D series1 = new BarSideBySideStackedSeries2D();

            //            SimpleBar2DModel SimpleBar2DModel = new DevExpress.Xpf.Charts.SimpleBar2DModel();
            //            series1.Model = SimpleBar2DModel;
            //            //series1.ArgumentScaleType = ScaleType.Auto;
            //            //series1.ValueScaleType = ScaleType.Numerical;
            //            //series1.DisplayName = item.Name;
            //            series1.Brush = (SolidColorBrush)(new BrushConverter().ConvertFrom(item.HtmlColor.ToString()));
            //            //series1.ArgumentDataMember = i.ToString();
            //            //series1.ValueDataMember = dt.Rows[i][item.Name.ToString()].ToString();
            //            // double yATmax = ToDouble(dt.Rows[i][item.Name.ToString()].ToString());
            //            //dt.Rows[i].ToString()
            //            double d = Convert.ToDouble(dt.Rows[i][item.Name.ToString()]);
            //            series1.Points.Add(new SeriesPoint(String.Format(i.ToString(), ("00")), d));
            //            // series1.Points.Add(new point(i.ToString(), dt.Rows[i][item.Name.ToString()].ToString()));
            //            diagram.Series.Add(series1);
            //            //diagram.ActualAxisY.ActualVisualRange.SideMarginsValue = series1.MaxSize;
            //            //diagram.ActualAxisY.ActualWholeRange.SideMarginsValue = ser1.MaxSize;


            //        }
            //    }

            //}

            chartcontrol.Legend = new DevExpress.Xpf.Charts.Legend();
            // chartcontrol.Legend.Orientation = System.Windows.Controls.Orientation.Horizontal;
            chartcontrol.Legend.HorizontalPosition = HorizontalPosition.RightOutside;


            //chartcontrol.EndInit();
        }
        private DataTable CreateTable()
        {
            dt = new DataTable();
            dt.Columns.Add("Month");
            foreach (var item in ListSalesStatusType)
            {
                BarSideBySideStackedSeries2D series = new BarSideBySideStackedSeries2D();
                series.Brush = (SolidColorBrush)(new BrushConverter().ConvertFrom(item.HtmlColor.ToString()));
                series.ArgumentDataMember = item.Name;
                diagram.Series.Add(series);
                SimpleBar2DModel SimpleBar2DModell = new DevExpress.Xpf.Charts.SimpleBar2DModel();
                series.Model = SimpleBar2DModell;
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


                for (int l = 0; l < 1; l++)
                {
                    foreach (Series ser in diagram.Series)
                    {
                        Double abc = Convert.ToDouble(dt.Rows[dt.Rows.Count - 1][ser.ArgumentDataMember.ToString()].ToString());

                        ser.Points.Add(new SeriesPoint(mt.ToString().PadLeft(2, '0'), abc));

                    }
                }
            }


            return dt;
        }
    }
}

