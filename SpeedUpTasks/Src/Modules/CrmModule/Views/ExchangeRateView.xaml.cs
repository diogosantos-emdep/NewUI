using DevExpress.Xpf.Charts;
using DevExpress.Xpf.PivotGrid;
using DevExpress.XtraPivotGrid.Data;

//using Emdep.Geos.Modules.Crm.UI.Helper;
using Emdep.Geos.Modules.Crm.ViewModels;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Emdep.Geos.Modules.Crm.Views
{
    /// <summary>
    /// Interaction logic for ExchangeRate.xaml
    /// </summary>
    public partial class ExchangeRateView : UserControl
    {     
        public ExchangeRateView()
        {
            InitializeComponent();
        }
        private void ChartCurrencyExchangeRate_BoundDataChanged(object sender, RoutedEventArgs e)
        {
            if (ChartCurrencyExchangeRate.Diagram.Series.Count > 0)
            {
                foreach (LineSeries2D series in ChartCurrencyExchangeRate.Diagram.Series)
                {
                    for (int i = 0; i < series.Points.Count; i++)
                    {
                        Emdep.Geos.Data.Common.DailyCurrencyConversion rate = (Emdep.Geos.Data.Common.DailyCurrencyConversion)series.Points[i].Tag;
                        if (rate.AxisYName == "AxisY2")
                            XYDiagram2D.SetSeriesAxisY(series, AxisY2);
                    }
                }
            }
        }
    }
}
