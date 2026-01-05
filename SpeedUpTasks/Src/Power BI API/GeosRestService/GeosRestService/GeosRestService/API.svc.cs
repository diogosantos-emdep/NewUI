using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using GeosRestService.Response;
using BusinessLogic;
using System.Configuration;
using System.ServiceModel.Web;
using System.Net;

namespace GeosRestService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "GeosService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select GeosService.svc or GeosService.svc.cs at the Solution Explorer and start debugging.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class API : IAPI
    {
        string MainConn = string.Empty;

        private Error Validate(string FromDate, string ToDate, string Plants, string Currency, string conn,string Type)
        {
            Error error = new Error();
            if(Type!="Customer" && Type != "Contact")
            {
                if (string.IsNullOrEmpty(FromDate))
                {
                    error.code = "400";
                    error.info = "Missing From Date parameter";
                }
                if (string.IsNullOrEmpty(ToDate))
                {
                    error.code = "410";
                    error.info = "Missing To Date parameter";
                }
                if (!string.IsNullOrEmpty(FromDate))
                {
                    DateTime dt = new DateTime();
                    if (!DateTime.TryParse(FromDate, out dt))
                    {
                        error.code = "420";
                        error.info = "Invalid From Date";
                    }

                }
                if (!string.IsNullOrEmpty(ToDate))
                {
                    DateTime dt = new DateTime();
                    if (!DateTime.TryParse(ToDate, out dt))
                    {
                        error.code = "430";
                        error.info = "Invalid To Date";
                    }

                }
            }
            
            if (!string.IsNullOrEmpty(Plants))
            {

                if (CommonManager.DictonarySites == null || CommonManager.DictonarySites.Count == 0)
                {
                    CommonManager.ConnString = conn;
                    CommonManager.DictonarySites = CommonManager.GetAllSites();
                }
                string[] plants = Plants.Split(',');
                foreach (string plant in plants)
                {
                    if (!CommonManager.DictonarySites.ContainsKey(plant))
                    {
                        error.code = "440";
                        error.info = "Invalid Plant " + plant;
                    }

                }
            }
            if (!string.IsNullOrEmpty(Currency))
            {
                CommonManager.ConnString = conn;
                if (CommonManager.Dictonarycurrency == null || CommonManager.Dictonarycurrency.Count == 0)
                {
                    int idCurrency = CommonManager.GetIdCurrency(Currency);
                    if (idCurrency > 0)
                        CommonManager.Dictonarycurrency.Add(Currency, idCurrency);
                }
                else
                {
                    if (!CommonManager.Dictonarycurrency.ContainsKey(Currency))
                    {
                        int idCurrency = CommonManager.GetIdCurrency(Currency);
                        if (idCurrency > 0)
                            CommonManager.Dictonarycurrency.Add(Currency, idCurrency);
                    }
                        
                }
                if (!CommonManager.Dictonarycurrency.ContainsKey(Currency))
                {
                    error.code = "450";
                    error.info = "Invalid Currency " + Currency;
                }

            }
            return error;
        }
        public ActivityResponse GetActivities(string FromDate, string ToDate, string Plants)
        {
            ActivityResponse activityResponse = new ActivityResponse();
            Error error = new Error();
            try
            {
                MainConn = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                error = Validate(FromDate, ToDate, string.IsNullOrEmpty(Plants) ? "" : Plants.ToUpper(), null, MainConn,"Activity");
                if (!string.IsNullOrEmpty(error.code))
                {
                    activityResponse.success = false;
                    activityResponse.error = error;
                    return activityResponse;
                }
                ActivityManager activityManager = new ActivityManager(MainConn);
                activityResponse.success = true;
                activityResponse.Activities = activityManager.GetActivities(Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate)
                    , string.IsNullOrEmpty(Plants) ? "0" : Plants);
                activityResponse.error = null;
            }
            catch (Exception ex)
            {
                activityResponse.success = false;
                activityResponse.error = new Error();
                activityResponse.error.code = "500";
                activityResponse.error.info = ex.Message +
                        " [Technical Supoort:" + ConfigurationManager.AppSettings["Email"] + "]";
            }


            return activityResponse;
        }

        public OfferResponse GetOffers(string FromDate, string ToDate, string Plants, string Currency)
        {
            OfferResponse offerResponse = new OfferResponse();
            OfferSuccessResponse offerSuccessResponse = new OfferSuccessResponse();
            Error error = new Error();
            try
            {
                MainConn = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                //MainConn = "Data Source=10.0.9.7;Database=geos;User ID=GeosUsr;Password=GEOS;Convert Zero Datetime=True";
                error = Validate(FromDate, ToDate, string.IsNullOrEmpty(Plants) ? "" : Plants.ToUpper()
                    , string.IsNullOrEmpty(Currency) ? "" : Currency.ToUpper(), MainConn,"Offer");
                if (!string.IsNullOrEmpty(error.code))
                {
                    offerResponse.success = false;
                    offerResponse.error = error;
                    return offerResponse;
                }
                OpportunityManager offerManager = new OpportunityManager(MainConn);
                offerResponse.success = true;
                offerResponse.Opportunities = offerManager.GetOffers(Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate),
                    string.IsNullOrEmpty(Plants) ? "0" : Plants.ToUpper(), string.IsNullOrEmpty(Currency) ? "EUR" : Currency.ToUpper());



            }
            catch (Exception ex)
            {
                offerResponse.success = false;
                offerResponse.error = new Error();
                offerResponse.error = new Error();
                offerResponse.error.code = "500";
                offerResponse.error.info = ex.Message + " " + ex.InnerException +
                       " [Technical Supoort:" + ConfigurationManager.AppSettings["Email"] + "]";

            }
            return offerResponse;
        }

        public OrderResponse GetOrders(string FromDate, string ToDate, string Plants, string Currency)
        {
            OrderResponse orderResponse = new OrderResponse();
            Error error = new Error();
            try
            {
                MainConn = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                error = Validate(FromDate, ToDate, string.IsNullOrEmpty(Plants) ? "" : Plants.ToUpper()
                    , string.IsNullOrEmpty(Currency) ? "" : Currency.ToUpper(), MainConn,"Orders");
                if (!string.IsNullOrEmpty(error.code))
                {
                    orderResponse.success = false;
                    orderResponse.error = error;
                    return orderResponse;
                }
                OrderManager orderManager = new OrderManager(MainConn);
                orderResponse.success = true;
                orderResponse.Orders = orderManager.GetOrders(Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate),
                    string.IsNullOrEmpty(Plants) ? "0" : Plants.ToUpper(), string.IsNullOrEmpty(Currency) ? "EUR" : Currency.ToUpper());

            }
            catch (Exception ex)
            {
                orderResponse.success = false;
                orderResponse.error = new Error();
                orderResponse.error.code = "500";
                orderResponse.error.info = ex.Message +
                    " [Technical Supoort:" + ConfigurationManager.AppSettings["Email"] + "]";
            }
            return orderResponse;
        }

        public CustomerResponse GetCustomers(string Plants)
        {
            CustomerResponse customerResponse = new CustomerResponse();
            Error error = new Error();
            try
            {
                //MainConn = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                MainConn = "Data Source=10.0.9.7;Database=geos;User ID=GeosUsr;Password=GEOS;Convert Zero Datetime=True";
                error = Validate(null, null, string.IsNullOrEmpty(Plants) ? "" : Plants.ToUpper()
                    , null, MainConn,"Customer");
                if (!string.IsNullOrEmpty(error.code))
                {
                    customerResponse.success = false;
                    customerResponse.error = error;
                    return customerResponse;
                }
                CustomerManager customerManager = new CustomerManager(MainConn);
                customerResponse.success = true;
                customerResponse.Customers = customerManager.GetCustomers(string.IsNullOrEmpty(Plants) ? "0" : Plants.ToUpper());
            }
            catch (Exception ex)
            {
                customerResponse.success = false;
                customerResponse.error = new Error();
                customerResponse.error.code = "500";
                customerResponse.error.info = ex.Message +
                    " [Technical Supoort:" + ConfigurationManager.AppSettings["Email"] + "]";
            }
            return customerResponse;
        }

        public ContactResponse GetContacts(string Plants)
        {
            ContactResponse contactResponse = new ContactResponse();
            Error error = new Error();
            try
            {
                //MainConn = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                MainConn = "Data Source=10.0.9.7;Database=geos;User ID=GeosUsr;Password=GEOS;Convert Zero Datetime=True";
                error = Validate(null, null, string.IsNullOrEmpty(Plants) ? "" : Plants.ToUpper()
                    , null, MainConn, "Contact");
                if (!string.IsNullOrEmpty(error.code))
                {
                    contactResponse.success = false;
                    contactResponse.error = error;
                    return contactResponse;
                }
                ContactManager contactManager = new ContactManager(MainConn);
                contactResponse.success = true;
                contactResponse.Contacts = contactManager.GetContacts(string.IsNullOrEmpty(Plants) ? "0" : Plants.ToUpper());
            }
            catch (Exception ex)
            {
                contactResponse.success = false;
                contactResponse.error = new Error();
                contactResponse.error.code = "500";
                contactResponse.error.info = ex.Message +
                    " [Technical Supoort:" + ConfigurationManager.AppSettings["Email"] + "]";
            }
            return contactResponse;
        }

        public CurrencyResponse GetCurrencies()
        {
            CurrencyResponse currencyResponse = new CurrencyResponse();
            Error error = new Error();
            try
            {
                MainConn = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                               
                CurrencyManager currencyManager = new CurrencyManager(MainConn);
                currencyResponse.success = true;
                currencyResponse.Currencies = currencyManager.GetCurrencies();
            }
            catch (Exception ex)
            {
                currencyResponse.success = false;
                currencyResponse.error = new Error();
                currencyResponse.error.code = "500";
                currencyResponse.error.info = ex.Message +
                    " [Technical Supoort:" + ConfigurationManager.AppSettings["Email"] + "]";
            }
            return currencyResponse;
        }

        public CurrencyRateResponse GetCurrenciesRates(string FromDate,string ToDate,string Source)
        {
            string LoginConn = string.Empty;
            CurrencyRateResponse currencyRateResponse = new CurrencyRateResponse();
            Error error = new Error();
            try
            {
                MainConn = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                LoginConn= ConfigurationManager.ConnectionStrings["LoginContext"].ConnectionString;
                error = Validate(FromDate, ToDate, null
                    , string.IsNullOrEmpty(Source) ? "" : Source.ToUpper(), MainConn, "");
                if (!string.IsNullOrEmpty(error.code))
                {
                    currencyRateResponse.success = false;
                    currencyRateResponse.error = error;
                    return currencyRateResponse;
                }
                CurrencyManager currencyManager = new CurrencyManager(LoginConn);
                currencyRateResponse.success = true;
                currencyRateResponse.CurrenciesRate = currencyManager.GetCurrencyRates(Convert.ToDateTime(FromDate),
                    Convert.ToDateTime(ToDate), string.IsNullOrEmpty(Source) ? "0" : Source);
            }
            catch (Exception ex)
            {
                currencyRateResponse.success = false;
                currencyRateResponse.error = new Error();
                currencyRateResponse.error.code = "500";
                currencyRateResponse.error.info = ex.Message +
                    " [Technical Supoort:" + ConfigurationManager.AppSettings["Email"] + "]";
            }
            return currencyRateResponse;
        }
        public Test GetData(string value1, string value2)
        {

            Test ob = new Test();
            ob.code = "123";

            return ob;
        }

    }
    public class Test
    {
        public string code { get; set; }
    }
}
