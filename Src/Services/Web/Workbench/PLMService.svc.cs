using Emdep.Geos.Data.BusinessLogic;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.PCM;
using Emdep.Geos.Data.Common.PLM;
//using Emdep.Geos.Data.Common.SynchronizationClass;
using Emdep.Geos.Services.Contracts;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
//using System.Net.Http;
//using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Services.Web.Workbench
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "PLMService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select PLMService.svc or PLMService.svc.cs at the Solution Explorer and start debugging.
    public class PLMService : IPLMService
    {
        PLMManager mgr = new PLMManager();

        public List<BasePrice> GetBasePrices()
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetBasePrices(PLMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool IsDeletedBasePriceList(UInt64 idBasePriceList)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.IsDeletedBasePriceList(idBasePriceList, MainServerConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Language> GetLanguages()
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetLanguages(PLMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Site> GetPlants()
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetPlants(PLMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Currency> GetCurrencies()
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetCurrencies(PLMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public string GetLatestBasePriceCode()
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetLatestBasePriceCode(PLMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public BasePrice AddBasePrice(BasePrice basePrice)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddBasePrice(basePrice, MainServerConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool UpdateBasePrice(BasePrice basePrice)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateBasePrice(basePrice, MainServerConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public BasePrice GetBasePriceDetailById(UInt64 IdBasePriceList)
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetBasePriceDetailById(PLMConnectionString, IdBasePriceList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<PCMArticleCategory> GetPCMArticlesWithCategory()
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetPCMArticlesWithCategory(PLMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<ArticleCostPrice> GetArticleCostPrices()
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetArticleCostPrices(PLMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<PCMArticleCategory> GetPCMArticlesWithCategoryForReference()
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetPCMArticlesWithCategoryForReference(PLMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<ArticleCostPrice> GetArticleCostPricesByCurrency(UInt32 IdCurrency)
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetArticleCostPricesByCurrency(PLMConnectionString, IdCurrency);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<CurrencyConversion> GetCurrencyConversionsByCurrency(UInt32 IdCurrency)
        {
            try
            {

                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetCurrencyConversionsByCurrency(connectionString, IdCurrency);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("WorkbenchContext") == false)
                {
                    exp.ErrorMessage = "WorkbenchContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<CustomerPrice> GetCustomerPrices()
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetCustomerPrices(PLMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool IsDeletedCustomerPriceList(UInt64 idCustomerPriceList)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.IsDeletedCustomerPriceList(idCustomerPriceList, MainServerConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public CustomerPrice GetCustomerPricesById(UInt64 idCustomerPriceList)
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetCustomerPricesById(PLMConnectionString, idCustomerPriceList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public string GetLatestCustomerPriceCode()
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetLatestCustomerPriceCode(PLMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<BasePrice> GetBasePriceNames()
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetBasePriceNames(PLMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public CustomerPrice AddCustomerPrice(CustomerPrice customerPrice)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddCustomerPrice(customerPrice, MainServerConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool UpdateCustomerPrice(CustomerPrice customerPrice)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateCustomerPrice(customerPrice, MainServerConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public BasePrice AddBasePrice_V2090(BasePrice basePrice)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddBasePrice_V2090(basePrice, MainServerConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool UpdateBasePrice_V2090(BasePrice basePrice)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateBasePrice_V2090(basePrice, MainServerConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public BasePrice GetBasePriceDetailById_V2090(UInt64 IdBasePriceList)
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetBasePriceDetailById_V2090(PLMConnectionString, IdBasePriceList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<CPLCustomer> GetCPLCustomers()
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetCPLCustomers(PLMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<PCMArticleCategory> GetPCMArticlesWithCategory_V2090()
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetPCMArticlesWithCategory_V2090(PLMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<PCMArticleCategory> GetPCMArticlesWithCategoryForReference_V2090()
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetPCMArticlesWithCategoryForReference_V2090(PLMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<CustomerPrice> GetCustomerPrices_V2090()
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetCustomerPrices_V2090(PLMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<PCMArticleCategory> GetPCMArticlesWithCategoryByIdBasePriceList(UInt64 IdBasePriceList)
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetPCMArticlesWithCategoryByIdBasePriceList(PLMConnectionString, IdBasePriceList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<PCMArticleCategory> GetPCMArticlesWithCategoryForReferenceByIdBasePriceList(UInt64 IdBasePriceList)
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetPCMArticlesWithCategoryForReferenceByIdBasePriceList(PLMConnectionString, IdBasePriceList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public BasePrice GetBasePriceDetailById_ForCPL(UInt64 IdBasePriceList)
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetBasePriceDetailById_ForCPL(PLMConnectionString, IdBasePriceList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public CustomerPrice AddCustomerPrice_V2100(CustomerPrice customerPrice)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddCustomerPrice_V2100(customerPrice, MainServerConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public bool UpdateCustomerPrice_V2100(CustomerPrice customerPrice)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateCustomerPrice_V2100(customerPrice, MainServerConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public CustomerPrice GetCustomerPricesById_V2100(UInt64 idCustomerPriceList)
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetCustomerPricesById_V2100(PLMConnectionString, idCustomerPriceList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Group> GetGroups()
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetGroups(PLMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Region> GetRegionsByGroupAndCountryAndSites(int IdGroup, string CountryNames, string SiteNames)
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetRegionsByGroupAndCountryAndSites(PLMConnectionString, IdGroup, CountryNames,SiteNames);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Country> GetCountriesByGroupAndRegionAndSites(int IdGroup, string RegionNames, string SiteNames)
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetCountriesByGroupAndRegionAndSites(PLMConnectionString, IdGroup, RegionNames, SiteNames);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Site> GetPlantsByGroupAndRegionAndCountry(int IdGroup, string RegionNames, string CountryNames)
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetPlantsByGroupAndRegionAndCountry(PLMConnectionString, IdGroup, RegionNames, CountryNames);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<CurrencyConversion> GetCurrencyConversionsByCurrency_V2100(UInt32 IdCurrency, DateTime CurrencyConversionDate)
        {
            try
            {

                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetCurrencyConversionsByCurrency_V2100(connectionString, IdCurrency, CurrencyConversionDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("WorkbenchContext") == false)
                {
                    exp.ErrorMessage = "WorkbenchContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public BasePrice AddBasePrice_V2110(BasePrice basePrice)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddBasePrice_V2110(basePrice, MainServerConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool UpdateBasePrice_V2110(BasePrice basePrice)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateBasePrice_V2110(basePrice, MainServerConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public BasePrice GetBasePriceDetailById_V2110(UInt64 IdBasePriceList)
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetBasePriceDetailById_V2110(PLMConnectionString, IdBasePriceList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public CustomerPrice AddCustomerPrice_V2110(CustomerPrice customerPrice)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddCustomerPrice_V2110(customerPrice, MainServerConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public bool UpdateCustomerPrice_V2110(CustomerPrice customerPrice)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateCustomerPrice_V2110(customerPrice, MainServerConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public CustomerPrice GetCustomerPricesById_V2110(UInt64 idCustomerPriceList)
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetCustomerPricesById_V2110(PLMConnectionString, idCustomerPriceList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public BasePrice GetBasePriceDetailById_ForCPL_V2110(UInt64 IdBasePriceList)
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetBasePriceDetailById_ForCPL_V2110(PLMConnectionString, IdBasePriceList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<CustomerPrice> GetCustomerPriceIFExistItem(UInt64 IdBasePriceList, UInt64 IdArticle)
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetCustomerPriceIFExistItem(PLMConnectionString, IdBasePriceList, IdArticle);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool DeleteCustomerPriceListItem(List<CustomerPriceListByItem> customerPriceListByItem)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.DeleteCustomerPriceListItem(MainServerConnectionString, customerPriceListByItem);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public bool CalculateArticleCostPrice()
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string WorkbenchContext = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.CalculateArticleCostPrice(PLMConnectionString, MainServerConnectionString, WorkbenchContext);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<Region> GetRegionsByGroupAndCountryAndSites_V2110(int IdGroup, string CountryNames, string SiteNames)
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetRegionsByGroupAndCountryAndSites_V2110(PLMConnectionString, IdGroup, CountryNames, SiteNames);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Country> GetCountriesByGroupAndRegionAndSites_V2110(int IdGroup, string RegionNames, string SiteNames)
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetCountriesByGroupAndRegionAndSites_V2110(PLMConnectionString, IdGroup, RegionNames, SiteNames);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Site> GetPlantsByGroupAndRegionAndCountry_V2110(int IdGroup, string RegionNames, string CountryNames)
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetPlantsByGroupAndRegionAndCountry_V2110(PLMConnectionString, IdGroup, RegionNames, CountryNames);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<CustomerPrice> GetCustomerPrices_V2110()
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetCustomerPrices_V2110(PLMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<CurrencyConversion> GetCurrencyConversionsMaxRate()
        {
            try
            {

                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetCurrencyConversionsMaxRate(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("WorkbenchContext") == false)
                {
                    exp.ErrorMessage = "WorkbenchContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<CurrencyConversion> GetCurrencyConversionsMaxRate_V2110(DateTime MinDate, DateTime MaxDate)
        {
            try
            {

                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetCurrencyConversionsMaxRate_V2110(connectionString, MinDate, MaxDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("WorkbenchContext") == false)
                {
                    exp.ErrorMessage = "WorkbenchContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<DateTime> GetMinMaxArticleExchangeRateDate()
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetMinMaxArticleExchangeRateDate(PLMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<BPLDetection> GetAllDetections()
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllDetections(PLMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public BasePrice AddBasePrice_V2120(BasePrice basePrice)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddBasePrice_V2120(basePrice, MainServerConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public bool UpdateBasePrice_V2120(BasePrice basePrice)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateBasePrice_V2120(basePrice, MainServerConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public BasePrice GetBasePriceDetailById_V2120(UInt64 IdBasePriceList)
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetBasePriceDetailById_V2120(PLMConnectionString, IdBasePriceList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<PCMArticleCategory> GetPCMArticlesWithCategory_V2120()
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetPCMArticlesWithCategory_V2120(PLMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<PCMArticleCategory> GetPCMArticlesWithCategoryForReference_V2120()
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetPCMArticlesWithCategoryForReference_V2120(PLMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public BasePrice GetBasePriceDetailById_ForCPL_V2120(UInt64 IdBasePriceList)
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetBasePriceDetailById_ForCPL_V2120(PLMConnectionString, IdBasePriceList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<Site> GetPlants_V2120()
        {
            try
            {

                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetPlants_V2120(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("WorkbenchContext") == false)
                {
                    exp.ErrorMessage = "WorkbenchContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<CurrencyConversion> GetCurrencyConversionsMaxRate_ByPreviousDate()
        {
            try
            {

                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetCurrencyConversionsMaxRate_ByPreviousDate(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("WorkbenchContext") == false)
                {
                    exp.ErrorMessage = "WorkbenchContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<BasePrice> GetBasePrices_V2120()
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetBasePrices_V2120(PLMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<CustomerPrice> GetCustomerPriceIFExistItem_V2120(UInt64 IdBasePriceList, string IdArticles)
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetCustomerPriceIFExistItem_V2120(PLMConnectionString, IdBasePriceList, IdArticles);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public BasePrice GetBasePriceDetailById_ForCPL_V2130(UInt64 IdBasePriceList)
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetBasePriceDetailById_ForCPL_V2130(PLMConnectionString, IdBasePriceList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public CustomerPrice AddCustomerPrice_V2130(CustomerPrice customerPrice)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddCustomerPrice_V2130(customerPrice, MainServerConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public CustomerPrice GetCustomerPricesById_V2130(UInt64 idCustomerPriceList)
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetCustomerPricesById_V2130(PLMConnectionString, idCustomerPriceList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public bool UpdateCustomerPrice_V2130(CustomerPrice customerPrice)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateCustomerPrice_V2130(customerPrice, MainServerConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public BasePrice AddBasePrice_V2140(BasePrice basePrice)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddBasePrice_V2140(basePrice, MainServerConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool UpdateBasePrice_V2140(BasePrice basePrice)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateBasePrice_V2140(basePrice, MainServerConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public BasePrice GetBasePriceDetailById_V2140(UInt64 IdBasePriceList)
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetBasePriceDetailById_V2140(PLMConnectionString, IdBasePriceList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<CurrencyConversion> GetCurrencyConversionsByCurrency_V2140(UInt32 IdCurrency, DateTime CurrencyConversionDate)
        {
            try
            {

                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetCurrencyConversionsByCurrency_V2140(connectionString, IdCurrency, CurrencyConversionDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("WorkbenchContext") == false)
                {
                    exp.ErrorMessage = "WorkbenchContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public BasePrice GetBasePriceDetailById_ForCPL_V2140(UInt64 IdBasePriceList)
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetBasePriceDetailById_ForCPL_V2140(PLMConnectionString, IdBasePriceList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<CurrencyConversion> GetCurrencyConversionsMaxRate_V2140()
        {
            try
            {

                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetCurrencyConversionsMaxRate_V2140(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("WorkbenchContext") == false)
                {
                    exp.ErrorMessage = "WorkbenchContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<CurrencyConversion> GetCurrencyConversionsMaxRate_ByPreviousDate_V2140()
        {
            try
            {

                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetCurrencyConversionsMaxRate_ByPreviousDate_V2140(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("WorkbenchContext") == false)
                {
                    exp.ErrorMessage = "WorkbenchContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<CurrencyConversion> GetCurrencyConversionsDetailsByDate(DateTime currencyConversionDate)
        {
            try
            {

                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetCurrencyConversionsDetailsByDate(connectionString,currencyConversionDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("WorkbenchContext") == false)
                {
                    exp.ErrorMessage = "WorkbenchContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public BasePrice AddBasePrice_V2150(BasePrice basePrice)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddBasePrice_V2150(basePrice, MainServerConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool UpdateBasePrice_V2150(BasePrice basePrice)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateBasePrice_V2150(basePrice, MainServerConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public BasePrice GetBasePriceDetailById_V2150(UInt64 IdBasePriceList)
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetBasePriceDetailById_V2150(PLMConnectionString, IdBasePriceList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<BasePrice> GetBasePrices_V2150()
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetBasePrices_V2150(PLMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public CustomerPrice AddCustomerPrice_V2150(CustomerPrice customerPrice)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddCustomerPrice_V2150(customerPrice, MainServerConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool UpdateCustomerPrice_V2150(CustomerPrice customerPrice)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateCustomerPrice_V2150(customerPrice, MainServerConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public CustomerPrice GetCustomerPricesById_V2150(UInt64 idCustomerPriceList)
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetCustomerPricesById_V2150(PLMConnectionString, idCustomerPriceList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public BasePrice GetBasePriceDetailById_ForCPL_V2150(UInt64 IdBasePriceList)
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetBasePriceDetailById_ForCPL_V2150(PLMConnectionString, IdBasePriceList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<CustomerPrice> GetCustomerPrices_V2150()
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetCustomerPrices_V2150(PLMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<PCMArticleCategory> GetPCMArticlesWithCategory_V2160()
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetPCMArticlesWithCategory_V2160(PLMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<PCMArticleCategory> GetPCMArticlesWithCategoryForReference_V2160()
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetPCMArticlesWithCategoryForReference_V2160(PLMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<BPLDetection> GetAllDetections_V2160()
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllDetections_V2160(PLMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


      

        public BasePrice GetBasePriceDetailById_ForCPL_V2160(UInt64 IdBasePriceList)
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetBasePriceDetailById_ForCPL_V2160(PLMConnectionString, IdBasePriceList, Properties.Settings.Default.CurrenciesImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public CustomerPrice GetCustomerPricesById_V2160(UInt64 idCustomerPriceList)
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetCustomerPricesById_V2160(PLMConnectionString, idCustomerPriceList, Properties.Settings.Default.CurrenciesImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<ArticleCostPrice> GetArticleCostPricesByCurrency_V2160(UInt32 IdCurrency)
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetArticleCostPricesByCurrency_V2160(PLMConnectionString, IdCurrency);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<ArticleCostPrice> GetArticleCostPricesByCurrency_V2640(UInt32 IdCurrency)
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetArticleCostPricesByCurrency_V2640(PLMConnectionString, IdCurrency);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public List<ArticleCostPrice> GetArticleCostPricesByCurrency_V2640New(UInt32 IdCurrency)
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetArticleCostPricesByCurrency_V2640New(PLMConnectionString, IdCurrency);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<Currency> GetCurrencies_V2160()
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetCurrencies_V2160(PLMConnectionString, Properties.Settings.Default.CurrenciesImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<BasePrice> GetBasePrices_V2160()
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetBasePrices_V2160(PLMConnectionString, Properties.Settings.Default.CurrenciesImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<CustomerPrice> GetCustomerPrices_V2160()
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetCustomerPrices_V2160(PLMConnectionString, Properties.Settings.Default.CurrenciesImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public BasePrice GetBasePriceDetailById_V2160(UInt64 IdBasePriceList)
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetBasePriceDetailById_V2160(PLMConnectionString, IdBasePriceList, Properties.Settings.Default.CurrenciesImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public bool CalculateArticleCostPrice_V2160(Company company)
        {
            try
            {
                //string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.CalculateArticleCostPrice_V2160(company, MainServerConnectionString, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<CurrencyConversion> GetCurrencyConversionsDetailsByLatestDate()
        {
            try
            {

                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetCurrencyConversionsDetailsByLatestDate(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("WorkbenchContext") == false)
                {
                    exp.ErrorMessage = "WorkbenchContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public IList<Company> GetEmdepSitesCompanies()
        {
            try
            {

                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetEmdepSitesCompanies(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }


        }

        public IList<Company> GetEmdepSitesCompanies_V2490()
        {
            try
            {

                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetEmdepSitesCompanies_V2490(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }


        }


        public List<UInt32> GetAllPCMIdArticles()
        {
            try
            {

                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllPCMIdArticles(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public string GetEWHQDatabaseDetail()
        {
            try
            {

                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetEWHQDatabaseDetail(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public string GetEWHQDatabaseDetail_V2490()
        {
            try
            {

                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetEWHQDatabaseDetail_V2490(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<PODetail> GetAllArticleMaxPODeliveryDateDetailFromEWHQ()
        {
            try
            {

                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllArticleMaxPODeliveryDateDetailFromEWHQ(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<PODetail> GetAllArticlesByArticleComponentMaxPOFromEWHQ()
        {
            try
            {

                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllArticlesByArticleComponentMaxPOFromEWHQ(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<ArticlesByArticle> GetAllArticlesByArticle()
        {
            try
            {

                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllArticlesByArticle(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public BasePrice GetBasePriceDetailsAfterSavedDataById(UInt64 IdBasePriceList)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.GetBasePriceDetailById_V2160(MainServerConnectionString, IdBasePriceList, Properties.Settings.Default.CurrenciesImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public CustomerPrice GetCustomerPriceDetailsAfterSavedDataById(UInt64 idCustomerPriceList)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.GetCustomerPricesById_V2160(MainServerConnectionString, idCustomerPriceList, Properties.Settings.Default.CurrenciesImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<BasePrice> GetBasePrices_V2180()
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetBasePrices_V2180(PLMConnectionString, Properties.Settings.Default.CurrenciesImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool IsDeletedBasePriceList_V2180(UInt64 idBasePriceList, uint IdModifier)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.IsDeletedBasePriceList_V2180(idBasePriceList, MainServerConnectionString, IdModifier);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool IsDeletedCustomerPriceList_V2180(UInt64 idCustomerPriceList, uint IdModifier)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.IsDeletedCustomerPriceList_V2180(idCustomerPriceList, MainServerConnectionString, IdModifier);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<CustomerPrice> GetCustomerPriceIFExistDetections(UInt64 IdBasePriceList, string IdDetections)
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetCustomerPriceIFExistDetections(PLMConnectionString, IdBasePriceList, IdDetections);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool UpdateBasePrice_V2180(BasePrice basePrice)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateBasePrice_V2180(basePrice, MainServerConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public BasePrice GetBasePriceDetailById_V2180(UInt64 IdBasePriceList)
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetBasePriceDetailById_V2180(PLMConnectionString, IdBasePriceList, Properties.Settings.Default.CurrenciesImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public string GetCustomerPriceCodesByBPL(UInt64 IdBasePriceList)
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetCustomerPriceCodesByBPL(PLMConnectionString, IdBasePriceList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<CustomerPrice> GetCustomerPriceIFExistCurrencies(UInt64 IdBasePriceList, string IdCurrencies)
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetCustomerPriceIFExistCurrencies(PLMConnectionString, IdBasePriceList, IdCurrencies);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public bool ImportArticleCostPriceCalculate(Company company, UInt64 itemArticle, List<PODetail> EWHQArticlesByArticleComponentpoDetailLst, List<ArticlesByArticle> LstAllArticlesByArticle, List<PODetail> ArticlesByArticleComponentpoDetailLst)
        {
            try
            {
                //string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.ImportArticleCostPriceCalculate(company, MainServerConnectionString, workbenchConnectionString, itemArticle, EWHQArticlesByArticleComponentpoDetailLst, LstAllArticlesByArticle, ArticlesByArticleComponentpoDetailLst);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<BPLPlantCurrencyDetail> GetBPLPlantCurrencyDetail(string IdArticles, string filtertext)
        {
            try
            {
              
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetBPLPlantCurrencyDetail(connectionString, IdArticles, filtertext);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();

                throw new FaultException<ServiceException>(exp, ex.ToString());
            }


        }

        public List<BPLPlantCurrencyDetail> GetBPLPlantCurrencyDetailByIdBPLAndIdCPL(Int32 IdArticle, string IdsBPL, string IdsCPL, string filtertext)
        {
            try
            {

                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetBPLPlantCurrencyDetailByIdBPLAndIdCPL(connectionString, IdArticle, IdsBPL, IdsCPL,filtertext);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();

                throw new FaultException<ServiceException>(exp, ex.ToString());
            }


        }


        //public async Task<Tuple<APIErrorDetailForErrorFalse, APIErrorDetail>> ArticlesPriceSynchronization(List<GeosAppSetting> geosAppSetting, string name)
        //{
        //    APIErrorDetailForErrorFalse valuesErrorFalse = new APIErrorDetailForErrorFalse();
        //    APIErrorDetail values = new APIErrorDetail();
        //    var token = new AuthToken();
        //    try
        //    {
        //        string[] tokeninformations = geosAppSetting.Where(i => i.IdAppSetting == 59).FirstOrDefault().DefaultValue.Split(';');

        //        var client = new HttpClient();
        //        var client_id = tokeninformations[1];
        //        var client_secret = tokeninformations[2];
        //        var clientCreds = System.Text.Encoding.UTF8.GetBytes($"{client_id}:{client_secret}");
        //        client.DefaultRequestHeaders.Authorization =
        //            new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(clientCreds));

        //        var postMessage = new Dictionary<string, string>();
        //        postMessage.Add("grant_type", "client_credentials");

        //        var request = new HttpRequestMessage(HttpMethod.Post, tokeninformations[0])
        //        {
        //            Content = new FormUrlEncodedContent(postMessage)
        //        };

        //        var response = await client.SendAsync(request);
        //        if (response.IsSuccessStatusCode)
        //        {
        //            var json = await response.Content.ReadAsStringAsync();
        //            token = JsonConvert.DeserializeObject<AuthToken>(json);
        //            token.ExpiresAt = DateTime.UtcNow.AddSeconds(token.ExpiresIn);
        //            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);

        //            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        //            string linkURL = null;
        //            if (!string.IsNullOrEmpty(name.Trim()))
        //            {
        //                linkURL = string.Format("{0}?references={1}", geosAppSetting.Where(i => i.IdAppSetting == 57).FirstOrDefault().DefaultValue, name.Trim());
        //            }
        //            else
        //            {
        //                linkURL = string.Format("{0}", geosAppSetting.Where(i => i.IdAppSetting == 57).FirstOrDefault().DefaultValue);
        //            }
        //            //  var res = await client.PostAsync(string.Format("{0}", GeosAppSettingList.Where(i => i.IdAppSetting == 56).FirstOrDefault().DefaultValue), httpContent);
        //            using (HttpResponseMessage responseProduct = await client.PostAsync(linkURL, httpContent))
        //            {
        //                responseProduct.EnsureSuccessStatusCode();
        //                string responseBody = await responseProduct.Content.ReadAsStringAsync();

        //                if (responseBody.Contains("false"))
        //                    valuesErrorFalse = JsonConvert.DeserializeObject<APIErrorDetailForErrorFalse>(responseBody);
        //                else if (responseBody.Contains("true") && responseBody.Contains("461"))
        //                    values = JsonConvert.DeserializeObject<APIErrorDetail>(responseBody);
        //                else if (responseBody.Contains("true") && responseBody.Contains("500"))
        //                    valuesErrorFalse = JsonConvert.DeserializeObject<APIErrorDetailForErrorFalse>(responseBody);
        //                else if (responseBody.Contains("true"))
        //                    values = JsonConvert.DeserializeObject<APIErrorDetail>(responseBody);
        //                else
        //                    valuesErrorFalse = JsonConvert.DeserializeObject<APIErrorDetailForErrorFalse>(responseBody);
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        ServiceException exp = new ServiceException();
        //        exp.ErrorMessage = ex.Message;
        //        exp.ErrorDetails = ex.ToString();

        //        throw new FaultException<ServiceException>(exp, ex.ToString());
        //    }

        //    return new Tuple<APIErrorDetailForErrorFalse, APIErrorDetail>(valuesErrorFalse, values);
        //}

        public bool CalculateArticleCostPrice_V2210(Company company)
        {
            try
            {
                //string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.CalculateArticleCostPrice_V2210(company, MainServerConnectionString, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<BPLDetection> GetAllDetections_V2220()
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllDetections_V2220(PLMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public async Task<List<ErrorDetails>> IsPLMAddEditBasePriceSynchronization(List<GeosAppSetting> GeosAppSettingList, BasePriceListByPlantCurrency itemPlantCurrency, DataTable DtArticle)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return await mgr.IsPLMAddEditBasePriceSynchronization(connectionString, GeosAppSettingList, itemPlantCurrency, DtArticle);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();

                throw new FaultException<ServiceException>(exp, ex.ToString());
            }


        }

        public async Task<List<ErrorDetails>> IsPLMUpdateEditBasePriceSynchronization(List<GeosAppSetting> GeosAppSettingList, BasePriceListByPlantCurrency itemBasePriceListByPlantCurrency, List<Articles> LstArticles)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return await mgr.IsPLMUpdateEditBasePriceSynchronization(connectionString, GeosAppSettingList, itemBasePriceListByPlantCurrency, LstArticles);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();

                throw new FaultException<ServiceException>(exp, ex.ToString());
            }


        }

        public async Task<List<ErrorDetails>> IsPLMAddEditCPLCustomerSynchronization(List<GeosAppSetting> GeosAppSettingList, BPLPlantCurrencyDetail itemBPLPlantCurrencyDetail, DataColumn[] columns, DataTable DtArticle)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return await mgr.IsPLMAddEditCPLCustomerSynchronization(connectionString, GeosAppSettingList, itemBPLPlantCurrencyDetail, columns, DtArticle);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();

                throw new FaultException<ServiceException>(exp, ex.ToString());
            }


        }

        public async Task<List<ErrorDetails>> IsPLMUpdateEditCPLCustomerSynchronization(List<GeosAppSetting> GeosAppSettingList, BPLPlantCurrencyDetail itemBasePriceListByPlantCurrency, List<Articles> LstArticles)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return await mgr.IsPLMUpdateEditCPLCustomerSynchronization(connectionString, GeosAppSettingList, itemBasePriceListByPlantCurrency, LstArticles);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();

                throw new FaultException<ServiceException>(exp, ex.ToString());
            }


        }
        public async Task<List<ErrorDetails>> IsPLMSynchronization(List<GeosAppSetting> GeosAppSettingList, BPLPlantCurrencyDetail BPLPlantCurrencyDetail, string Details, string Name)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return await mgr.IsPLMSynchronization(connectionString, GeosAppSettingList, BPLPlantCurrencyDetail, Details, Name);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();

                throw new FaultException<ServiceException>(exp, ex.ToString());
            }


        }


        public List<AdditionalArticleCost> GetAllAddedAdditionalArticleCost()
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllAddedAdditionalArticleCost(workbenchConnectionString,connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();

                throw new FaultException<ServiceException>(exp, ex.ToString());
            }


        }

        //GEOS2-2999
        public bool CalculateArticleCostPrice_V2220(Company itemCompany)
        {
            try
            {

                string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.CalculateArticleCostPrice_V2220(itemCompany, mainServerConnectionString, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();

                throw new FaultException<ServiceException>(exp, ex.ToString());
            }


        }


        //GEOS2-3511
        public List<Users> GetAllUsersList()
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllUsersList(workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();

                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


      


        //GEOS2-3511


        public List<UserPermissionByBPLPriceList> GetAllUserPermissionsByBPLPriceList(DateTime dtFrom, DateTime dtTo)
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllUserPermissionsByBPLPriceList(PLMConnectionString, dtFrom, dtTo);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<UserPermissionByCPLPriceList> GetAllUserPermissionsByCPLPriceList(DateTime dtFrom, DateTime dtTo)
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllUserPermissionsByCPLPriceList(PLMConnectionString, dtFrom, dtTo);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool InsertUpdateUserPermissionByBPLForParticularColumn(UserPermissionByBPLPriceList userPermissionByBPLPriceList)
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.InsertUpdateUserPermissionByBPLForParticularColumn(userPermissionByBPLPriceList, PLMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool InsertUpdateUserPermissionByCPLForParticularColumn(UserPermissionByCPLPriceList userPermissionByCPLPriceList)
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.InsertUpdateUserPermissionByCPLForParticularColumn(userPermissionByCPLPriceList, PLMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //GEOS2-3511


        public List<BasePrice> GetBasePriceListItem_User_ById(int IdUser)
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetBasePriceListItem_User_ById(PLMConnectionString, IdUser, Properties.Settings.Default.CurrenciesImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<CustomerPrice> GetCustomerPriceListItem_User_ById(int IdUser)
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetCustomerPriceListItem_User_ById(PLMConnectionString, IdUser, Properties.Settings.Default.CurrenciesImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //GEOS2-3511

        public List<BasePrice> GetBasePriceListByDates(DateTime fromDate, DateTime toDate)
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetBasePriceListByDates(PLMConnectionString, Properties.Settings.Default.CurrenciesImages, fromDate, toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<CustomerPrice> GetCustomerPriceListByDates(DateTime fromDate, DateTime toDate)
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetCustomerPriceListByDates(PLMConnectionString, Properties.Settings.Default.CurrenciesImages, fromDate, toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

       

        

        public bool DeleteUserPermissionByBPLForParticularColumn(Int32 idUser, UInt64 idBasePriceList)
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.DeleteUserPermissionByBPLForParticularColumn(PLMConnectionString, idUser, idBasePriceList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool DeleteUserPermissionByCPLForParticularColumn(Int32 idUser, UInt64 idCustomerPriceList)
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.DeleteUserPermissionByCPLForParticularColumn(PLMConnectionString, idUser, idCustomerPriceList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool DeleteUserPermissionByForParticularUser(Int32 idUser)
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.DeleteUserPermissionByForParticularUser(PLMConnectionString, idUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        // shubham[skadam] GEOS2-3851 Article_Cost_Price -> PK idarticle + idPlant  10 Aug 2022
        public bool CalculateArticleCostPrice_V2300(Company itemCompany)
        {
            try
            {

                string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.CalculateArticleCostPrice_V2300(itemCompany, mainServerConnectionString, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();

                throw new FaultException<ServiceException>(exp, ex.ToString());
            }


        }

        public BasePrice AddBasePrice_V2340(BasePrice basePrice) 
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddBasePrice_V2340(basePrice, MainServerConnectionString, Properties.Settings.Default.ArticleAttachmentDocPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<BPLDocument> GetBPLAttachmentByIdBasePriceList(UInt32 IdBasePriceList)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetBPLAttachmentByIdBasePriceList(PCMConnectionString, IdBasePriceList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("PCMContext") == false)
                {
                    exp.ErrorMessage = "PCMContext - connection string name not found.";
                    exp.ErrorCode = "000090";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public BasePrice GetBasePriceDetailById_V2340(UInt64 IdBasePriceList)
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetBasePriceDetailById_V2340(PLMConnectionString, Convert.ToUInt32(IdBasePriceList), Properties.Settings.Default.CurrenciesImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool UpdateBasePrice_V2340(BasePrice basePrice)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateBasePrice_V2340(basePrice, MainServerConnectionString, Properties.Settings.Default.ArticleAttachmentDocPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public CustomerPrice AddCustomerPrice_V2340(CustomerPrice customerPrice)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddCustomerPrice_V2340(customerPrice, MainServerConnectionString, Properties.Settings.Default.ArticleAttachmentDocPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool UpdateCustomerPrice_V2340(CustomerPrice customerPrice)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateCustomerPrice_V2340(customerPrice, MainServerConnectionString, Properties.Settings.Default.ArticleAttachmentDocPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public CustomerPrice GetCustomerPricesById_V2340(UInt64 idCustomerPriceList)
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetCustomerPricesById_V2340(PLMConnectionString, idCustomerPriceList, Properties.Settings.Default.CurrenciesImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[pjadhav][GEOS2-4015][10-01-2023]
        public List<Country> GetCountriesByGroupAndRegionAndSites_V2350(int IdGroup, string RegionNames, string CountryNames)
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetCountriesByGroupAndRegionAndSites_V2350(PLMConnectionString, IdGroup, RegionNames, CountryNames, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        #region GEOS2-2886
        //Shubham[skadam] GEOS2-2886 [Only Modules ] - Able to  add and access to the tab Modules/Detection /Articles a Base Price List [3/3] [#PLM07] 28 03 2023
        public List<CustomerPrice> GetCustomerPriceIFExistModules(UInt64 IdBasePriceList, string IdCPType)
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetCustomerPriceIFExistModules(PLMConnectionString, IdBasePriceList, IdCPType);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool UpdateBasePrice_V2370(BasePrice basePrice)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateBasePrice_V2370(basePrice, MainServerConnectionString, Properties.Settings.Default.ArticleAttachmentDocPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public BasePrice AddBasePrice_V2370(BasePrice basePrice)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddBasePrice_V2370(basePrice, MainServerConnectionString, Properties.Settings.Default.ArticleAttachmentDocPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public BasePrice GetBasePriceDetailById_V2370(UInt64 IdBasePriceList)
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetBasePriceDetailById_V2370(PLMConnectionString, Convert.ToUInt32(IdBasePriceList), Properties.Settings.Default.CurrenciesImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<BasePrice> GetBasePriceListItem_User_ById_V2370(int IdUser)
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetBasePriceListItem_User_ById_V2370(PLMConnectionString, IdUser, Properties.Settings.Default.CurrenciesImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        #endregion

        #region GEOS2-4300 & GEOS2-3180
        public BasePrice GetBasePriceDetailById_ForCPL_V2380(UInt64 IdBasePriceList)
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetBasePriceDetailById_ForCPL_V2380(PLMConnectionString, IdBasePriceList, Properties.Settings.Default.CurrenciesImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public List<CustomerPrice> GetCustomerPriceListItem_User_ById_V2380(int IdUser)
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetCustomerPriceListItem_User_ById_V2380(PLMConnectionString, IdUser, Properties.Settings.Default.CurrenciesImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool UpdateCustomerPrice_V2380(CustomerPrice customerPrice)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateCustomerPrice_V2380(customerPrice, MainServerConnectionString, Properties.Settings.Default.ArticleAttachmentDocPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public CustomerPrice AddCustomerPrice_V2380(CustomerPrice customerPrice)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddCustomerPrice_V2380(customerPrice, MainServerConnectionString, Properties.Settings.Default.ArticleAttachmentDocPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public CustomerPrice GetCustomerPricesById_V2380(UInt64 idCustomerPriceList)
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetCustomerPricesById_V2380(PLMConnectionString, idCustomerPriceList, Properties.Settings.Default.CurrenciesImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        #endregion


        public IList<Company> GetEmdepSitesCompaniesWithServiceURL()
        {
            try
            {

                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetEmdepSitesCompaniesWithServiceURL(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }


        }

        //[rdixit][GEOS2-4474][19.06.2023]
        public bool CalculateArticleCostPrice_V2400(Company itemCompany,List<PODetail> EWHQpoDetailLst, List<PODetail> EWHQArticlesByArticleComponentpoDetailLst, List<ArticlePOAVG> GetPOAverageByAllPCMArticleEWHQLst)
        {
            try
            {
                string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.CalculateArticleCostPrice_V2400(itemCompany, mainServerConnectionString, workbenchConnectionString, EWHQpoDetailLst, EWHQArticlesByArticleComponentpoDetailLst, GetPOAverageByAllPCMArticleEWHQLst);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[rdixit][GEOS2-4474][19.06.2023]
        public List<PODetail> GetAllArticleMaxPODeliveryDateDetailFromEWHQ_V2210()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllArticleMaxPODeliveryDateDetailFromEWHQ_V2210(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][GEOS2-4474][19.06.2023]
        public List<PODetail> GetAllArticlesByArticleComponentMaxPOFromEWHQ_V2210()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllArticlesByArticleComponentMaxPOFromEWHQ_V2210(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][GEOS2-4474][19.06.2023]
        public List<ArticlePOAVG> WS_GetPOAverageByAllPCMArticleEWHQ(Int32 NumberPOAvg)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.WS_GetPOAverageByAllPCMArticleEWHQ(connectionString, NumberPOAvg);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][GEOS2-4520][05.10.2023]
        public List<BasePriceListByPlantCurrency> GetPlantCurrencyByIdBasePrice(UInt64 IdBasePriceList)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetPlantCurrencyByIdBasePrice(connectionString, IdBasePriceList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[Sudhir.Jangra][GEOS2-4935]
        public BasePrice AddBasePrice_V2470(BasePrice basePrice)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddBasePrice_V2470(basePrice, MainServerConnectionString, Properties.Settings.Default.ArticleAttachmentDocPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[Sudhir.Jangra][GEOS2-4935]
        public BasePrice GetBasePriceDetailById_V2470(UInt64 IdBasePriceList)
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetBasePriceDetailById_V2470(PLMConnectionString, Convert.ToUInt32(IdBasePriceList), Properties.Settings.Default.CurrenciesImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[Sudhir.Jangra][GEOS2-4935]
        //GEOS2-6688 PCM- Improve performance to open BPL grid and any BPL
        public BasePrice GetBasePriceDetailById_V2640(UInt64 IdBasePriceList)
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetBasePriceDetailById_V2640(PLMConnectionString, Convert.ToUInt32(IdBasePriceList), Properties.Settings.Default.CurrenciesImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-4935]
        public bool UpdateBasePrice_V2470(BasePrice basePrice)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateBasePrice_V2470(basePrice, MainServerConnectionString, Properties.Settings.Default.ArticleAttachmentDocPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[Sudhir.Jangra][GEOS2-4935]
        public CustomerPrice AddCustomerPrice_V2470(CustomerPrice customerPrice)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddCustomerPrice_V2470(customerPrice, MainServerConnectionString, Properties.Settings.Default.ArticleAttachmentDocPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[Sudhir.Jangra][GEOS2-4935]
        public CustomerPrice GetCustomerPricesById_V2470(UInt64 idCustomerPriceList)
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetCustomerPricesById_V2470(PLMConnectionString, idCustomerPriceList, Properties.Settings.Default.CurrenciesImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }



        //[Sudhir.Jangra][GEOS2-4935]
        public bool UpdateCustomerPrice_V2470(CustomerPrice customerPrice)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateCustomerPrice_V2470(customerPrice, MainServerConnectionString, Properties.Settings.Default.ArticleAttachmentDocPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[cpatil][GEOS2-5299][27.02.2024]
        public List<ArticlePOAVG> WS_GetPOAverageByAllPCMArticleEWHQ_V2490(Int32 NumberPOAvg)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.WS_GetPOAverageByAllPCMArticleEWHQ_V2490(connectionString, NumberPOAvg);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public IList<Company> GetEmdepSitesCompaniesWithServiceURL_V2490()
        {
            try
            {

                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetEmdepSitesCompaniesWithServiceURL_V2490(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][27.03.2024][GEOS2-5556]
        public IList<Company> GetEmdepSitesCompanies_V2500()
        {
            try
            {

                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetEmdepSitesCompanies_V2500(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[nsatpute][28-05-2025][GEOS2-6689]
        public CustomerPrice GetCustomerPricesById_V2650(UInt64 idCustomerPriceList)
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetCustomerPricesById_V2650(PLMConnectionString, idCustomerPriceList, Properties.Settings.Default.CurrenciesImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[nsatpute][28-05-2025][GEOS2-6689]
        public List<ArticleCostPrice> GetArticleCostPricesByCurrency_V2650(UInt32 IdCurrency)
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetArticleCostPricesByCurrency_V2650(PLMConnectionString, IdCurrency);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[nsatpute][28-05-2025][GEOS2-6689]
        public BasePrice GetBasePriceDetailById_ForCPL_V2650(UInt64 IdBasePriceList)
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetBasePriceDetailById_ForCPL_V2650(PLMConnectionString, IdBasePriceList, Properties.Settings.Default.CurrenciesImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
    }
}
