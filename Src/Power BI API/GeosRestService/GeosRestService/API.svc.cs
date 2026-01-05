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
using System.Runtime.InteropServices;
namespace GeosRestService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "GeosService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select GeosService.svc or GeosService.svc.cs at the Solution Explorer and start debugging.
    [ServiceBehavior(AddressFilterMode = AddressFilterMode.Any, InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class API : IAPI
    {
        string MainConn = string.Empty;

        private Error Validate(string FromDate, string ToDate, string Plants, string Currency, string conn,string Type, [Optional] string Year)
        {
            Error error = new Error();
            if(Type== "SalesQuota" || Type== "PlantQuota" || Type == "CustomerTargets")
            {
                if(string.IsNullOrEmpty(Year))
                {
                    error.code = "460";
                    error.info = "Missing Year parameter";
                }
            }
            if(Type== "Activity" || Type == "Offer" || Type== "Orders" || Type== "CurrenciesRates" || Type== "ProductionOffer" || Type== "EngineeringAnalysis")
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

        public EngineeringAnalysisResponse GetEngineeringAnalysis(string FromDate, string ToDate, string Plants)
        {
            MainConn = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
            EngineeringAnalysisResponse engineeringAnalysisResponse = new EngineeringAnalysisResponse();
            Error error = new Error();
            try
            {
                error = Validate(FromDate, ToDate, string.IsNullOrEmpty(Plants) ? "" : Plants.ToUpper()
                    , null, MainConn, "EngineeringAnalysis");
                if (!string.IsNullOrEmpty(error.code))
                {
                    engineeringAnalysisResponse.success = false;
                    engineeringAnalysisResponse.error = error;
                    return engineeringAnalysisResponse;
                }
                EngineeringAnalysisManager engineeringAnalysisManager = new EngineeringAnalysisManager(MainConn);
                engineeringAnalysisResponse.EngineeringAnalysis = engineeringAnalysisManager.GetEngineeringAnalysisDetails(Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate)
                    ,string.IsNullOrEmpty(Plants) ? "0" : Plants.ToUpper());
                engineeringAnalysisResponse.success = true;
            }
            catch (Exception ex)
            {
                engineeringAnalysisResponse.success = false;
                engineeringAnalysisResponse.error = new Error();
                engineeringAnalysisResponse.error = new Error();
                engineeringAnalysisResponse.error.code = "500";
                engineeringAnalysisResponse.error.info = ex.HelpLink + " - " + ex.Message + " " + ex.InnerException +
                       " [Technical Supoort:" + ConfigurationManager.AppSettings["Email"] + "]";

            }
            return engineeringAnalysisResponse;
        }

        public ProductionOfferResponse GetProductionOffers(string FromDate,string ToDate,string Plants)
        {
            ProductionOfferResponse offerResponse = new ProductionOfferResponse();
            //OfferSuccessResponse offerSuccessResponse = new OfferSuccessResponse();
            Error error = new Error();
            try
            {
                MainConn = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                
                error = Validate(FromDate, ToDate, string.IsNullOrEmpty(Plants) ? "" : Plants.ToUpper()
                    , null, MainConn, "ProductionOffer");
                if (!string.IsNullOrEmpty(error.code))
                {
                    offerResponse.success = false;
                    offerResponse.error = error;
                    return offerResponse;
                }
                OpportunityManager offerManager = new OpportunityManager(MainConn);
                offerResponse.success = true;
                offerResponse.CI = offerManager.GetOfferProductionData(Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate),
                    string.IsNullOrEmpty(Plants) ? "0" : Plants.ToUpper());



            }
            catch (Exception ex)
            {
                offerResponse.success = false;
                offerResponse.error = new Error();
                offerResponse.error = new Error();
                offerResponse.error.code = "500";
                offerResponse.error.info = ex.HelpLink + " - " + ex.Message + " " + ex.InnerException +
                       " [Technical Supoort:" + ConfigurationManager.AppSettings["Email"] + "]";

            }
            return offerResponse;
        }
        public CustomerTargetResponse GetCustomerTargets(string Currency, String Year)
        {
            CustomerTargetResponse customerTargetResponse = new CustomerTargetResponse();
            Error error = new Error();
            string FromDate = Year + "-" + "1" + "-" + "1";
            string ToDate = Year + "-" + "12" + "-" + "31";
            try
            {
                MainConn = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                QuotaManager quotaManager = new QuotaManager(MainConn);
                error = Validate(null, null, null, Currency, MainConn, "CustomerTargets", Year);
                if (!string.IsNullOrEmpty(error.code))
                {
                    customerTargetResponse.success = false;
                    customerTargetResponse.error = error;
                    return customerTargetResponse;
                }
                customerTargetResponse.success = true;
                customerTargetResponse.CustomerTargets = quotaManager.GetCustomerTarget(string.IsNullOrEmpty(Currency) ? "EUR" : Currency, FromDate, ToDate);
                return customerTargetResponse;

            }
            catch (Exception ex)
            {
                customerTargetResponse.success = false;
                customerTargetResponse.error = new Error();
                customerTargetResponse.error.code = "500";
                customerTargetResponse.error.info = ex.Message +
                    " [Technical Supoort:" + ConfigurationManager.AppSettings["Email"] + "]";
                return customerTargetResponse;
            }
        }

        public PlantQuotaResponse GetPlantQuota(string Currency,String Year)
        {
            PlantQuotaResponse plantQuotaResponse = new PlantQuotaResponse();
            Error error = new Error();
            string FromDate = Year + "-" + "1" + "-" + "1";
            string ToDate = Year + "-" + "12" + "-" + "31";
            try
            {
                MainConn = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                QuotaManager quotaManager = new QuotaManager(MainConn);
                //error = Validate(FromDate, ToDate, null, Currency, MainConn,"plantQuota");
                error = Validate(null, null, null, Currency, MainConn, "PlantQuota", Year);
                if (!string.IsNullOrEmpty(error.code))
                {
                    plantQuotaResponse.success = false;
                    plantQuotaResponse.error = error;
                    return plantQuotaResponse;
                }
                plantQuotaResponse.success = true;
                plantQuotaResponse.PlantTargets = quotaManager.GetPlantTarget(string.IsNullOrEmpty(Currency) ? "EUR" : Currency,FromDate,ToDate);
                return plantQuotaResponse;

            }
            catch (Exception ex)
            {
                plantQuotaResponse.success = false;
                plantQuotaResponse.error = new Error();
                plantQuotaResponse.error.code = "500";
                plantQuotaResponse.error.info = ex.Message +
                    " [Technical Supoort:" + ConfigurationManager.AppSettings["Email"] + "]";
                return plantQuotaResponse;
            }
        }
        public SalesQuotaResponse GetSalesQuota(string Currency,string Year)
        {
            SalesQuotaResponse salesQuotaResponse = new SalesQuotaResponse();
            Error error = new Error();
            try
            {
                MainConn = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                QuotaManager quotaManager = new QuotaManager(MainConn);
                error = Validate(null, null, null, Currency, MainConn, "SalesQuota",Year);
                if (!string.IsNullOrEmpty(error.code))
                {
                    salesQuotaResponse.success = false;
                    salesQuotaResponse.error = error;
                    return salesQuotaResponse;
                }
                salesQuotaResponse.success = true;
                salesQuotaResponse.EmployeeTargets = quotaManager.GetSalesQuotas(string.IsNullOrEmpty(Currency)?"EUR": Currency,Year);
                return salesQuotaResponse;

            }
            catch (Exception ex)
            {
                salesQuotaResponse.success = false;
                salesQuotaResponse.error = new Error();
                salesQuotaResponse.error.code = "500";
                salesQuotaResponse.error.info = ex.Message +
                    " [Technical Supoort:" + ConfigurationManager.AppSettings["Email"] + "]";
                return salesQuotaResponse;
            }
        }
        public ActivityResponse GetActivities(string FromDate, string ToDate, string Plants)
        {
            ActivityResponse activityResponse = new ActivityResponse();
            Error error = new Error();
            try
            {
                MainConn = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                error = Validate(FromDate, ToDate, string.IsNullOrEmpty(Plants) ? "" : Plants.ToUpper(), null,MainConn,"Activity");
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
                offerResponse.error.info = ex.HelpLink + " - " + ex.Message + " " + ex.InnerException +
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
                orderResponse.error.info = ex.HelpLink + " - " + ex.Message +
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
                MainConn = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                
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
                MainConn = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                
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
                    , string.IsNullOrEmpty(Source) ? "" : Source.ToUpper(), MainConn, "CurrenciesRates");
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
