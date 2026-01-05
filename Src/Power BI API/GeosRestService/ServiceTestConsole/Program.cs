using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic;
using Entities;
using System.Dynamic;
using GeosRestService;
using GeosRestService.Response;

namespace ServiceTestConsole
{
    class Program 
    {
       
        static void Main(string[] args)
        {
            string Loginconn = "Data Source=10.0.9.7;Database=emdep_geos;User ID=GeosUsr;Password=GEOS;Convert Zero Datetime=True";
            string Mainconn = "Data Source=10.0.9.202;Database=geos;User ID=GeosUsr;Password=GEOS;Convert Zero Datetime=True";
            DateTime FromDate = new DateTime(2017, 1, 1);
            DateTime ToDate = new DateTime(2018, 12, 31);

            //EngineeringAnalysisManager engineeringAnalysisManager = new EngineeringAnalysisManager(Mainconn);
            //engineeringAnalysisManager.GetEngineeringAnalysisDetails(FromDate, ToDate, "0");

            //API api = new API();

            // api.GetProductionOffers(FromDate.ToString(), ToDate.ToString(), "");

            //api.GetOrders(FromDate.ToString(), ToDate.ToString(), "EAES,EPIN", "EUR");
            //return;
            //CurrencyManager currencyManager = new CurrencyManager(Mainconn);
            //currencyManager.GetCurrencyRates(FromDate, ToDate, "EUR");
            //return;
            //TestClass t = new TestClass();
            //t.Address = "address";
            //t.ID = 132;
            //t.Name = string.Empty;
            //t.DateTime = null;

            //t.Dump();
            //var ret = t.FixMeUp();
            //var v = ((object)ret);



            //string key = Helper.DecodeBase64("dGVzdHVzZXI6dGVzdHBhc3N3b3Jk");
            //Console.WriteLine(key);
            //string s = Helper.DecodeBase64("MzhmNDVhYWMtNjk3OC0xMWU4LWFkYzAtZmE3YWUwMWJiZWJj");
            //string textDecoded = System.Text.Encoding.UTF8.DecodeBase64("cmF2aQ==");
            ////string textEncoded = System.Text.Encoding.UTF8.EncodeBase64("ravi");
            //string conn = "Data Source=10.0.9.7;Database=emdep_geos;User ID=GeosUsr;Password=GEOS;Convert Zero Datetime=True";

            ////bool isSucess= Authentication.CheckAuthentication("rdeshmukh12", "38f45aac-6978-11e8-adc0-fa7ae01bbebc", conn);


            //ActivityManager activityManager = new ActivityManager(Mainconn);
            //List<Activity> Activities = new List<Activity>();
            //string Plants = "EAAB";

            //Activities = activityManager.GetActivities(dtFromDate, dtToDate, Plants);



            // OpportunityManager offerManager = new OpportunityManager(Mainconn);
            // List<Opportunities> offers = new List<Opportunities>();
            // offers = offerManager.GetOffers(FromDate, ToDate, "EAES,EPIN", "INR");

            //OrderManager orderManager = new OrderManager(Mainconn);
            //List<Order> Orders = new List<Order>();
            //Orders = orderManager.GetOrders(FromDate, ToDate, "EAES,EBRO");
           // return;
            QuotaManager quotaManager = new QuotaManager(Mainconn);
            //quotaManager.GetSalesQuotas("EUR");
            quotaManager.GetPlantTarget("usd", FromDate.ToString("yyyy-MM-dd"), ToDate.ToString("yyyy-MM-dd"));
            //quotaManager.GetCustomerTarget("EUR", FromDate.ToString("yyyy-MM-dd"), ToDate.ToString("yyyy-MM-dd"));

        }

        static object MyMethod(string cls)
        {
            object o = new object();
            if(cls== "TestClass")
            {
                TestClass obj = new TestClass();
                obj.Name = "Lucky";
                o= obj;
            }
            else
            {
                anoutherClass obj = new anoutherClass();
                obj.Address = "Pune";
                o= obj;
            }
            return o;
        }
    }
    public static class ReClasser
    {
        public static dynamic FixMeUp<T>(this T fixMe)
        {
            var t = fixMe.GetType();
            var returnClass = new ExpandoObject() as IDictionary<string, object>;
            foreach (var pr in t.GetProperties())
            {
                var val = pr.GetValue(fixMe);
                if (val is string && string.IsNullOrWhiteSpace(val.ToString()))
                {
                }
                else if (val == null)
                {
                }
                else
                {
                    returnClass.Add(pr.Name, val);
                }
            }
            return returnClass;
        }
    }
    public class anoutherClass
    {
        public string Address;
    }
    public class TestClass
    {
        public string Name { get; set; }
        public int ID { get; set; }
        public DateTime? DateTime { get; set; }
        public string Address { get; set; }
    }
    public static class EncodingForBase64
    {
        public static string EncodeBase64(this System.Text.Encoding encoding, string text)
        {
            if (text == null)
            {
                return null;
            }

            byte[] textAsBytes = encoding.GetBytes(text);
            return System.Convert.ToBase64String(textAsBytes);
        }

        public static string DecodeBase64(this System.Text.Encoding encoding, string encodedText)
        {
            if (encodedText == null)
            {
                return null;
            }

            byte[] textAsBytes = System.Convert.FromBase64String(encodedText);
            return encoding.GetString(textAsBytes);
        }
    }
}
