using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.ServiceProcess;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
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

namespace CurrencyConvertHistorical
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Services
       
        ICrmService CrmStartUp = new CrmServiceController(CurrencyConvertHistorical.Properties.Settings.Default.SERVICE_PROVIDER_URL);
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(CurrencyConvertHistorical.Properties.Settings.Default.SERVICE_PROVIDER_URL);

        #endregion // Services

        #region Declaration


        public IList<LookupValue> TypeList { get; set; }
        DateTime currentDateTime;
        public IList<Currency> Currencies { get; set; }
        DailyCurrencyConversion objDailyCurrencyConversion;
        public string nameOfString;

        #endregion // Declaration
        public MainWindow()
        {
            InitializeComponent();
           
        }



        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
           
            try
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                DateTime StartDate1 = StartDate.DateTime;
                DateTime EndDate1 = EndDate.DateTime;
                Currencies = CrmStartUp.GetCurrencies();
                nameOfString = (string.Join(",", Currencies.Select(x => x.Name.ToString()).ToArray()));
                for (var day = StartDate1.Date; day.Date <= EndDate1.Date; day = day.AddDays(1))
                {
                    string date = day.Date.ToString("yyyy-MM-dd");

                    foreach (Currency itemCurrencies in Currencies)
                    {
                        string svalue = CurrencyConvertHistorical.Properties.Settings.Default.CurrencyLayer.ToString();
                        string skey = CurrencyConvertHistorical.Properties.Settings.Default.CurrencyLayerKey.ToString();
                        string currencysource = itemCurrencies.Name;
                        string finalstring = svalue + "historical?" + "access_key=" + skey + "&source=" + currencysource + "&format=1&date=" + date + "&currencies=" + nameOfString;
                        string json = new WebClient().DownloadString(finalstring);

                        Emdep.Geos.CurrencyConvertApi.Models.HistoryModel todayRatesQueried = JsonConvert.DeserializeObject<Emdep.Geos.CurrencyConvertApi.Models.HistoryModel>(json);
                        // http://apilayer.net/api/live?access_key=34fe8672eefff10710e2b1a23b0a6d40&source=USD&format=1&date%20=%202005-02-01%20&currencies=EUR,RON,USD,MXN,CNY,HNL,BRL,TND,MAD,PYG,RUB,INR,GBP,CAD

                        string consource = String.Empty;
                        try
                        {
                            consource = todayRatesQueried.Source.ToString();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(todayRatesQueried.Error.Info.ToString());
                         
                        }
                        objDailyCurrencyConversion = new DailyCurrencyConversion();
                        objDailyCurrencyConversion.IdCurrencyConversionFrom = Currencies.FirstOrDefault(x => x.Name == consource).IdCurrency;
                        foreach (var item in todayRatesQueried.quotes)
                        {

                            string[] tokens = item.Key.ToString().Split(new[] { consource }, StringSplitOptions.None);
                            if (tokens[1] != "")
                            {
                                objDailyCurrencyConversion.IdCurrencyConversionTo = Currencies.FirstOrDefault(x => x.Name == tokens[1]).IdCurrency;
                            }
                            else
                            {
                                objDailyCurrencyConversion.IdCurrencyConversionTo = objDailyCurrencyConversion.IdCurrencyConversionFrom;
                            }
                            objDailyCurrencyConversion.CurrencyConversionDate = day;
                            float value = float.Parse(item.Value, CultureInfo.CreateSpecificCulture("en-GB"));
                            objDailyCurrencyConversion.CurrencyConversationRate = value;
                            objDailyCurrencyConversion.IsCorrect = 1;
                            CrmStartUp.UpdateCurrencyConversionDaily(objDailyCurrencyConversion);
                        }

                    }
                }
                MessageBox.Show("Currency Historical Saved Successfully ");
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
            }

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


    }
}
