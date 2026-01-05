using Emdep.Geos.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Emdep.Geos.Data.BusinessLogic;
using System.IO;
using Emdep.Geos.Services.Contracts;
using System.Net;
using Emdep.Geos.Utility;
using System.ServiceModel.Activation;

using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Emdep.Geos.Data.Common.HarnessPart;

namespace Emdep.Geos.Services.Web.Workbench
{
    /// <summary>
    /// HarnessPartService class use for getting information of HarnessPart
    /// </summary>
   public class HarnessPartService : IHarnessPartService
    {
        /// <summary>
        /// This method is to get list of all languages
        /// </summary>
        /// <returns>List of all languages</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IHarnessPartService HarnessPartControl = new HarnessPartController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         List&lt;Language&gt; languages = HarnessPartControl.GetAllLanguage();
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<Language> GetAllLanguage()
        {
            List<Language> languages = null;
            try
            {
                LanguageManager languagemgr = new LanguageManager();
                languages = languagemgr.GetAllLanguage();
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return languages;
         }

         /// <summary>
        /// This method is to get all Color
        /// </summary>
        /// <returns>List of all color from class color</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IHarnessPartService HarnessPartControl = new HarnessPartController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         List&lt;Color&gt; Colors = HarnessPartControl.GetAllColor();
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<Color> GetAllColor()
        {
            List<Color> Colors = null;
            try
            {
                ColorManager colormgr = new ColorManager();
                Colors = colormgr.GetAllColor();
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return Colors;
        }

        /// <summary>
         /// This method is to get all companies
         /// </summary>
         /// <returns>List of company</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IHarnessPartService HarnessPartControl = new HarnessPartController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         List&lt;Company&gt; companies = HarnessPartControl.GetAllCompanies();
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<Company> GetAllCompanies()
        {
            List<Company> companies = null;
            try
            {
                HarnessPartManager harnessPartMgr = new HarnessPartManager();
                companies = harnessPartMgr.GetAllCompanies();
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return companies;
        }

        public List<HarnessPart> GetEnumTest()
        {
            List<HarnessPart> harnessparts = null;
            try
            {
                HarnessPartManager harnessPartMgr = new HarnessPartManager();
                harnessparts = harnessPartMgr.GetEnumTest();
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return harnessparts;
        }

        /// <summary>
        /// This method is to get all Harness Part Accessory Type
        /// </summary>
        /// <returns>List of all Harness Part Accessory Type</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IHarnessPartService HarnessPartControl = new HarnessPartController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         List&lt;HarnessPartAccessoryType&gt; harnessPartAccessoryTypes = HarnessPartControl.GetAllHarnessPartAccessoryType();
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<HarnessPartAccessoryType> GetAllHarnessPartAccessoryType()
        {
            List<HarnessPartAccessoryType> harnessPartAccessoryTypes = null;
            try
            {
                HarnessPartManager harnessPartMgr = new HarnessPartManager();
                harnessPartAccessoryTypes = harnessPartMgr.GetAllHarnessPartAccessoryType();
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return harnessPartAccessoryTypes;
        }

           /// <summary>
        /// This method is to get all enterprise group
        /// </summary>
        /// <returns>List of all enterprise group</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IHarnessPartService HarnessPartControl = new HarnessPartController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         List&lt;EnterpriseGroup&gt; enterpriseGroups = HarnessPartControl.GetAllEnterpriseGroup();
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<EnterpriseGroup> GetAllEnterpriseGroup()
        {
            List<EnterpriseGroup> enterpriseGroups = null;
            try
            {
                HarnessPartManager harnessPartMgr = new HarnessPartManager();
                enterpriseGroups = harnessPartMgr.GetAllEnterpriseGroup();
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return enterpriseGroups;
        }

        /// <summary>
        /// This method is to get all harness part type
        /// </summary>
        /// <returns>List of all harness part type</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IHarnessPartService HarnessPartControl = new HarnessPartController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         List&lt;HarnessPartType&gt; harnessPartTypes = HarnessPartControl.GetAllHarnessPartType();
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<HarnessPartType> GetAllHarnessPartType()
        {
            List<HarnessPartType> harnessPartTypes = null;
            try
            {
                HarnessPartManager harnessPartMgr = new HarnessPartManager();
                harnessPartTypes = harnessPartMgr.GetAllHarnessPartType();
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return harnessPartTypes;
        }


         /// <summary>
        /// This method is to get all harness part search result
        /// </summary>
        /// <param name="harnessPartSearch">Get harnessPartSearch class</param>
        /// <returns>List of all harness part search result</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IHarnessPartService HarnessPartControl = new HarnessPartController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         List&lt;HarnessPart&gt; harnessParts = HarnessPartControl.GetAllHarnessPartSearchResult(harnessPartSearch);
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<HarnessPart> GetAllHarnessPartSearchResult(HarnessPartSearch harnessPartSearch)
        {
            List<HarnessPart> harnessParts = null;
            try
            {
                HarnessPartManager harnessPartMgr = new HarnessPartManager();
                harnessParts = harnessPartMgr.GetAllHarnessPartSearchResult(harnessPartSearch);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return harnessParts;
        }

        /// <summary>
        /// This method is to get all harness part search result for possible Search
        /// </summary>
        /// <param name="harnessPartSearch">Get harnessPartSearch class</param>
        /// <returns>List of all harness part search result for possible Search</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IHarnessPartService HarnessPartControl = new HarnessPartController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         List&lt;HarnessPart&gt; harnessParts = HarnessPartControl.GetAllHarnessPartSearchResultPossibleSearch(harnessPartSearch);
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<HarnessPart> GetAllHarnessPartSearchResultPossibleSearch(HarnessPartSearch harnessPartSearch)
        {
            List<HarnessPart> harnessParts = null;
            try
            {
                HarnessPartManager harnessPartMgr = new HarnessPartManager();
                harnessParts = harnessPartMgr.GetAllHarnessPartSearchResultPossibleSearch(harnessPartSearch);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return harnessParts;
        }
    }
}
