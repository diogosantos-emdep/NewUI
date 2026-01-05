using DevExpress.Xpf.WindowsUI;


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

namespace Emdep.Geos.Modules.Crm.Views
{
    /// <summary>
    /// Interaction logic for ExchangeRateEditView.xaml
    /// </summary>
    public partial class ExchangeRateEditView : WinUIDialogWindow
    {
        //ObservableCollection<ExchangeRate> Data { get; set; }
        //public ObservableCollection<Column> Columns { get; private set; }
        public ExchangeRateEditView()
        {
            InitializeComponent();
            //Columns = new ObservableCollection<Column>() {
            //    new Column() { FieldName="", HeaderText=" ",Settings = SettingsType.ImageWithText, AllowCellMerge=true,Width=90,AllowEditing=false ,FixedWidth=true},
            //    new Column() { FieldName="ExchangeRateAndDateList[0]",  ImageIndex=0, HeaderText="EUR", Settings = SettingsType.Array, AllowCellMerge=false , Width=90, FixedWidth=true,AllowEditing=false },
            //     new Column() { FieldName="ExchangeRateAndDateList[1]",ImageIndex=1,  HeaderText="USD", Settings = SettingsType.Array, AllowCellMerge=false , Width=90, FixedWidth=true,AllowEditing=false },
            //     new Column() { FieldName="ExchangeRateAndDateList[2]",ImageIndex=2,   HeaderText="RON", Settings = SettingsType.Array, AllowCellMerge=false , Width=90, FixedWidth=true,AllowEditing=false },
            //     new Column() { FieldName="ExchangeRateAndDateList[3]", ImageIndex=3, HeaderText="MXN", Settings = SettingsType.Array, AllowCellMerge=false , Width=90, FixedWidth=true,AllowEditing=false },
            //     new Column() { FieldName="ExchangeRateAndDateList[4]",ImageIndex=4,  HeaderText="CNY", Settings = SettingsType.Array, AllowCellMerge=false , Width=90, FixedWidth=true,AllowEditing=false },
            //     new Column() { FieldName="ExchangeRateAndDateList[5]",ImageIndex=5,  HeaderText="HNL", Settings = SettingsType.Array, AllowCellMerge=false , Width=90, FixedWidth=true,AllowEditing=false },
            //     new Column() { FieldName="ExchangeRateAndDateList[6]",ImageIndex=6,  HeaderText="TND", Settings = SettingsType.Array, AllowCellMerge=false , Width=90, FixedWidth=true,AllowEditing=false },
            //     new Column() { FieldName="ExchangeRateAndDateList[7]",ImageIndex=7,  HeaderText="BRL", Settings = SettingsType.Array, AllowCellMerge=false , Width=90, FixedWidth=true,AllowEditing=false },
            //     new Column() { FieldName="ExchangeRateAndDateList[8]",ImageIndex=8,  HeaderText="MAD", Settings = SettingsType.Array, AllowCellMerge=false , Width=90, FixedWidth=true,AllowEditing=false },
            //     new Column() { FieldName="ExchangeRateAndDateList[9]",ImageIndex=9,  HeaderText="PYG", Settings = SettingsType.Array, AllowCellMerge=false , Width=90, FixedWidth=true,AllowEditing=false },
            //     new Column() { FieldName="ExchangeRateAndDateList[10]",ImageIndex=10,  HeaderText="RUB", Settings = SettingsType.Array, AllowCellMerge=false , Width=90, FixedWidth=true,AllowEditing=false },
            //     new Column() { FieldName="ExchangeRateAndDateList[11]",ImageIndex=11,  HeaderText="INR", Settings = SettingsType.Array, AllowCellMerge=false , Width=90, FixedWidth=true,AllowEditing=false },
              
            //    };


            //Data = new ObservableCollection<ExchangeRate>();
            //Data.Add(new ExchangeRate() { HeaderText = "1 EUR", Trend = "0", RateCurrency1 = "dssd", 
            //    ExchangeRateAndDateList = new ObservableCollection<ExchangeRateAndDate>() 
            //    { new ExchangeRateAndDate() { Rate = "4,45586", Date = "04/03/2016" }, 
            //        new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, 
            //        new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, 
            //        new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, 
            //        new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, 
            //        new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, 
            //        new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" },
            //        new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, 
            //        new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, 
            //        new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" },
            //        new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" },
            //        new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" } } });
            ////Data.Add(new ExchangeRate() { HeaderText = "1 USD", Trend = "1", ExchangeRateAndDateList = new ObservableCollection<ExchangeRateAndDate>() { new ExchangeRateAndDate() { Rate = "1,0907", Date = "14/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" } } });
            ////Data.Add(new ExchangeRate() { HeaderText = "1 RON", Trend = "2", ExchangeRateAndDateList = new ObservableCollection<ExchangeRateAndDate>() { new ExchangeRateAndDate() { Rate = "18,1706", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" } } });
            ////Data.Add(new ExchangeRate() { HeaderText = "1 MXN", Trend = "3", ExchangeRateAndDateList = new ObservableCollection<ExchangeRateAndDate>() { new ExchangeRateAndDate() { Rate = "7,2525", Date = "04/01/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" } } });
            ////Data.Add(new ExchangeRate() { HeaderText = "1 CNY", Trend = "4", ExchangeRateAndDateList = new ObservableCollection<ExchangeRateAndDate>() { new ExchangeRateAndDate() { Rate = "25,8732", Date = "11/02/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" } } });
            ////Data.Add(new ExchangeRate() { HeaderText = "1 HNL", Trend = "5", ExchangeRateAndDateList = new ObservableCollection<ExchangeRateAndDate>() { new ExchangeRateAndDate() { Rate = "2,30277", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" } } });
            //////Data.Add(new ExchangeRate() { HeaderText = "RON", ExchangeRateAndDateList = new ObservableCollection<ExchangeRateAndDate>() { new ExchangeRateAndDate() { Rate = "4,45586", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" } } });
            ////Data.Add(new ExchangeRate() { HeaderText = "1 TND", Trend = "6", ExchangeRateAndDateList = new ObservableCollection<ExchangeRateAndDate>() { new ExchangeRateAndDate() { Rate = "4,45586", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" } } });
            ////Data.Add(new ExchangeRate() { HeaderText = "1 BRL", Trend = "7", ExchangeRateAndDateList = new ObservableCollection<ExchangeRateAndDate>() { new ExchangeRateAndDate() { Rate = "4,45586", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" } } });
            ////Data.Add(new ExchangeRate() { HeaderText = "1 MAD", Trend = "8", ExchangeRateAndDateList = new ObservableCollection<ExchangeRateAndDate>() { new ExchangeRateAndDate() { Rate = "4,45586", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" } } });
            ////Data.Add(new ExchangeRate() { HeaderText = "1 PYG", Trend = "9", ExchangeRateAndDateList = new ObservableCollection<ExchangeRateAndDate>() { new ExchangeRateAndDate() { Rate = "4,45586", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" } } });
            ////Data.Add(new ExchangeRate() { HeaderText = "1 RUB", Trend = "10", ExchangeRateAndDateList = new ObservableCollection<ExchangeRateAndDate>() { new ExchangeRateAndDate() { Rate = "4,45586", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" } } });
            ////Data.Add(new ExchangeRate() { HeaderText = "1 INR", Trend = "11", ExchangeRateAndDateList = new ObservableCollection<ExchangeRateAndDate>() { new ExchangeRateAndDate() { Rate = "4,45586", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" }, new ExchangeRateAndDate() { Rate = "1,0907", Date = "04/03/2016" } } });
            //grid.ColumnsSource = Columns;
            //grid.ItemsSource = Data;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void TextEdit_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void pivotGridControl1_FocusedCellChanged(object sender, RoutedEventArgs e)
        {

        }

        //private void edit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        //{

        //}
    }
}
