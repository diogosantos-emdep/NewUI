using Emdep.Geos.Modules.Epc.Common.EPC;
using Emdep.Geos.UI.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Emdep.Geos.Modules.Epc.Views
{
    /// <summary>
    /// Interaction logic for StrategicMapView.xaml
    /// </summary>
    public partial class StrategicMapView : UserControl
    {
        ObservableCollection<Deshboards> Data { get; set; }
        public ObservableCollection<Column> Columns { get; private set; }

        public StrategicMapView()
        {
            InitializeComponent();

            Columns = new ObservableCollection<Column>() {
                new Column() { FieldName="ProcessFamily",HeaderText="ProcessFamily", Settings = SettingsType.Default, AllowCellMerge=true, Width=50,AllowEditing=false, },
                new Column() { FieldName="ProcessCode", HeaderText="ProcessCode",Settings = SettingsType.Default, AllowCellMerge=true,Width=50,AllowEditing=false},
                new Column() { FieldName="ProcessName",HeaderText="ProcessName", Settings = SettingsType.Default, AllowCellMerge=true,Width=50,AllowEditing=false},
                new Column() { FieldName="GroupIndicator",HeaderText="Indicator", Settings = SettingsType.Default, AllowCellMerge=true,Width=50,AllowEditing=false},
                new Column() { FieldName="Indicator",HeaderText="IndicatorName ", Settings = SettingsType.Default, AllowCellMerge=true,Width=50,AllowEditing=false},
                new Column() { FieldName="Target", HeaderText="Target",Settings = SettingsType.Default, AllowCellMerge=true,Width=20,AllowEditing=false},
                new Column() { FieldName="Months[0]",  HeaderText="Oct", Settings = SettingsType.Array, AllowCellMerge=false , Width=50, FixedWidth=true,AllowEditing=false },
                new Column() { FieldName="Months[1]", HeaderText="Nov", Settings = SettingsType.Array, AllowCellMerge=false , Width=50, FixedWidth=true,AllowEditing=false },
                new Column() { FieldName="Months[2]", HeaderText="Dec", Settings = SettingsType.Array, AllowCellMerge=false , Width=50, FixedWidth=true,AllowEditing=false },
                new Column() { FieldName="Months[3]", HeaderText="Jan", Settings = SettingsType.Array, AllowCellMerge=false , Width=50, FixedWidth=true,AllowEditing=false },
                new Column() { FieldName="Months[4]", HeaderText="Feb", Settings = SettingsType.Array, AllowCellMerge=false , Width=50, FixedWidth=true,AllowEditing=false },
                new Column() { FieldName="Months[5]", HeaderText="Mar", Settings = SettingsType.Array, AllowCellMerge=false , Width=50, FixedWidth=true,AllowEditing=false },
                new Column() { FieldName="Current",HeaderText="Current", Settings = SettingsType.Text, AllowCellMerge=true, Width=50,  FixedWidth=true,AllowEditing=false},
                new Column() { FieldName="Trend", HeaderText="Trend",Settings = SettingsType.ArrowImage, AllowCellMerge=false, Width=50, FixedWidth=true,AllowEditing=false},
                //new Column() { FieldName="Update",HeaderText="Update", Settings = SettingsType.Default, AllowCellMerge=false , Width=70, FixedWidth=true,AllowEditing=false} ,
                new Column() { FieldName="History",HeaderText=" ", Settings = SettingsType.Chart, AllowCellMerge=false , Width=100, FixedWidth=true,AllowEditing=false },

            };

            Data = new ObservableCollection<Deshboards>();
            //pivotGridControl1.FieldCellTemplate = null;
            Data.Add(new Deshboards() { ProcessFamily = "Generic", ProcessCode = "?", ProcessName = "Open Dev. Projects", GroupIndicator = "", Indicator = "", Target = "", Months = new List<string>() { "34%", "64%", "77%", "92%", "58%", "62%" }, Current = new CustomCellValue() { BackgroundColor = "Yellow", ForegroundColor = "Red", TextValue = "25", TextAlignment = TextAlignment.Center }, Trend = "1", History = new List<int>() { 0, 0, 0, 0, 2, 5, 7, 5, 8, 4 } });
            Data.Add(new Deshboards() { ProcessFamily = "Generic", ProcessCode = "?", ProcessName = "Open IT Project", GroupIndicator = "", Indicator = "", Target = "", Months = new List<string>() { "33%", "44%", "66%", "88%", "62%", "58%" }, Current = new CustomCellValue() { BackgroundColor = "Yellow", ForegroundColor = "Red", TextValue = "26", TextAlignment = TextAlignment.Center }, Trend = "2", History = new List<int>() { 0, 3, 4, 6, 2, 5, 4, 5, 8, 4 } });
            Data.Add(new Deshboards() { ProcessFamily = "Strategic Plan", ProcessCode = "PCS_ENG_0023_00", ProcessName = "", GroupIndicator = "", Indicator = "Processed Deployed", Target = "100%", Months = new List<string>() { "45%", "64%", "77%", "92%", "58%", "62%" }, Current = new CustomCellValue() { BackgroundColor = "Green", ForegroundColor = "Red", TextValue = "27", TextAlignment = TextAlignment.Center }, Trend = "2", History = new List<int>() { 0, 4, 6, 3, 2, 2, 6, 95, 8, 4 } });
            Data.Add(new Deshboards() { ProcessFamily = "Strategic Plan", ProcessCode = "PCS_ENG_0023_00", ProcessName = "", GroupIndicator = "", Indicator = "% of Acceptance", Target = "100%", Months = new List<string>() { "34%", "64%", "77%", "92%", "44%", "22%" }, Current = new CustomCellValue() { BackgroundColor = "Green", ForegroundColor = "Red", TextValue = "28", TextAlignment = TextAlignment.Center }, Trend = "3", History = new List<int>() { 0, 3, 4, 6, 2, 5, 7, 5, 8, 4 } });
            Data.Add(new Deshboards() { ProcessFamily = "GEOS", ProcessCode = "2015-2033", ProcessName = "Customer Help Desk", GroupIndicator = "IND27.4", Indicator = "", Target = "", Months = new List<string>() { "35%", "36%", "57%", "91%", "78%", "88%" }, Current = new CustomCellValue() { BackgroundColor = "Green", ForegroundColor = "Red", TextValue = "29", TextAlignment = TextAlignment.Center }, Trend = "1", History = new List<int>() { 0, 0, 0, 0, 0, 5, 3, 6, 2, 9 } });
            Data.Add(new Deshboards() { ProcessFamily = "GEOS", ProcessCode = "2015-2033", ProcessName = "Customer Help Desk", GroupIndicator = "IND28.2", Indicator = "", Target = "", Months = new List<string>() { "34%", "68", "34%", "78%", "41%", "62%" }, Current = new CustomCellValue() { BackgroundColor = "Green", ForegroundColor = "Red", TextValue = "30", TextAlignment = TextAlignment.Center }, Trend = "2", History = new List<int>() { 2, 1, 7, 6, 4, 5, 7, 1, 3, 2 } });
            Data.Add(new Deshboards() { ProcessFamily = "GEOS", ProcessCode = "2015-2033", ProcessName = "Customer Help Desk", GroupIndicator = "IND26.2", Indicator = "", Target = "", Months = new List<string>() { "34%", "64%", "77%", "99%", "35%", "58%" }, Current = new CustomCellValue() { BackgroundColor = "Green", ForegroundColor = "Red", TextValue = "30", TextAlignment = TextAlignment.Center }, Trend = "1", History = new List<int>() { 2, 1, 7, 6, 4, 5, 7, 1, 3, 2 } });
            Data.Add(new Deshboards() { ProcessFamily = "GEOS", ProcessCode = "2015-2033", ProcessName = "Customer Help Desk", GroupIndicator = "IND27.2", Indicator = "", Target = "", Months = new List<string>() { "34%", "61%", "89%", "95%", "69%", "36%" }, Current = new CustomCellValue() { BackgroundColor = "Green", ForegroundColor = "Red", TextValue = "30", TextAlignment = TextAlignment.Center }, Trend = "2", History = new List<int>() { 2, 1, 7, 6, 4, 5, 7, 1, 3, 2 } });
            Data.Add(new Deshboards() { ProcessFamily = "GEOS", ProcessCode = "2015-2033", ProcessName = "Customer Help Desk", GroupIndicator = "IND22.2", Indicator = "", Target = "", Months = new List<string>() { "56%", "45%", "78%", "85%", "98%", "62%" }, Current = new CustomCellValue() { BackgroundColor = "Green", ForegroundColor = "Red", TextValue = "30", TextAlignment = TextAlignment.Center }, Trend = "3", History = new List<int>() { 2, 1, 7, 6, 4, 5, 7, 1, 3, 2 } });
            //   pivotGridControl1.DataSource = Data; ProcessName="",
            grid.ColumnsSource = Columns;
            grid.ItemsSource = Data;


        }
    }
}
