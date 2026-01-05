using BusinessLogic;
using GeosAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Runtime.Caching;
using System.Configuration;

namespace GeosAPI
{
    public class Vallidate
    {
        MemoryCache memCache = new MemoryCache("Cache");
        public Error Validate(string FromDate, string ToDate, string Plants, string Currency, string conn, string Type, [Optional] string Year)
        {
            Error error = new Error();
            if (Type == "SalesQuota" || Type == "PlantQuota" || Type == "CustomerTargets")
            {
                if (string.IsNullOrEmpty(Year))
                {
                    error.code = "460";
                    error.info = "Missing Year parameter";
                }
                else
                {
                    try
                    {
                        int.Parse(Year);
                    }
                    catch (Exception)
                    {

                        error.code = "470";
                        error.info = "Invalid Year parameter";
                    }
                    
                }
            }
            if (Type == "Activity" || Type == "Offer" || Type == "Orders" || Type == "CurrenciesRates" || Type == "ProductionOffer" || Type == "EngineeringAnalysis" || Type == "TechnicalService" || Type == "TechnicalServiceList")
            {
                //if (string.IsNullOrEmpty(FromDate))
                //{
                //    error.code = "400";
                //    error.info = "Missing From Date parameter";
                //}
                if (string.IsNullOrEmpty(FromDate) || string.IsNullOrEmpty(ToDate))
                {
                    error.code = "410";
                    error.info = "Missing From/To Date parameter(s).";
                }
                if (!string.IsNullOrEmpty(FromDate))
                {
                    DateTime dt = new DateTime();
                    if (!DateTime.TryParse(FromDate, out dt))
                    {
                        error.code = "420";
                        error.info = "Invalid From Date.";
                    }

                }
                if (!string.IsNullOrEmpty(ToDate))
                {
                    DateTime dt = new DateTime();
                    if (!DateTime.TryParse(ToDate, out dt))
                    {
                        error.code = "430";
                        error.info = "Invalid To Date.";
                    }

                }
            }

            if (!string.IsNullOrEmpty(Plants) && Plants !="0")
            {

                if (CommonManager.DictonarySites == null || CommonManager.DictonarySites.Count == 0)
                {
                    CommonManager.ConnString = conn;
                    CommonManager.DictonarySites = CommonManager.GetAllSites();
                    //var res = memCache.Get("allSites");
                    //if (res != null)
                    //{

                    //}
                    //else
                    //{
                    //    memCache.Add("allSites", CommonManager.DictonarySites, DateTimeOffset.UtcNow.AddMinutes(30));
                    //}

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
            if(!string.IsNullOrEmpty(error.info))
            {
                error.info = error.info + " [Technical Supoort:" + ConfigurationManager.AppSettings["Email"] + "]";
            }
            return error;
        }
    }
}