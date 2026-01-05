using Emdep.Geos.Data.BusinessLogic;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Data.Common.PCM;
using Emdep.Geos.Data.Common.PLM;
using Emdep.Geos.Data.Common.SynchronizationClass;
//using Emdep.Geos.Data.Common.SynchronizationClass;
using Emdep.Geos.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
//using System.Net.Http;
//using System.Net.Http.Headers;

using System.Threading.Tasks;

namespace Emdep.Geos.Services.Web.Workbench
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "PCMService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select PCMService.svc or PCMService.svc.cs at the Solution Explorer and start debugging.
    /// <summary>
    /// This Service is contains all PCM service functions.
    /// </summary>
    public class PCMService : IPCMService
    {
        PCMManager mgr = new PCMManager();

        /// <summary>
        /// This method is used to get all Catalogue Items of PCM.
        /// </summary>
        /// <returns>The list of Catalogue Items.</returns>
        public List<CatalogueItem> GetAllCatalogueItems()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetAllCatalogueItems(PCMConnectionString);
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

        /// <summary>
        /// This method is used to get all Product Types of PCM.
        /// </summary>
        /// <returns>The list of Product Types.</returns>
        public List<ProductTypes> GetAllProductTypes()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetAllProductTypes(PCMConnectionString);
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

        /// <summary>
        /// This method is used to get all Templates of PCM.
        /// </summary>
        /// <returns>The list of Templates.</returns>
        public List<Template> GetAllTemplates()
        {
            try
            {

                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetAllTemplates(PCMConnectionString);
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

        /// <summary>
        /// This method is used to get all Families of PCM.
        /// </summary>
        /// <returns>The list of Families.</returns>
        public List<ConnectorFamilies> GetAllFamilies()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetAllFamilies(PCMConnectionString);
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

        /// <summary>
        /// This method is used to get all Detection Types of PCM.
        /// </summary>
        /// <returns>The list of Detection Types.</returns>
        public List<DetectionTypes> GetAllDetectionTypes()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetAllDetectionTypes(PCMConnectionString);
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

        /// <summary>
        /// This method is used to get ways of PCM.
        /// </summary>
        /// <returns>The list of ways.</returns>
        public List<Ways> GetAllWayList()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetAllWayList(PCMConnectionString);
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

        /// <summary>
        /// This method is used to get Detetction of PCM.
        /// </summary>
        /// <returns>The list of detetctions.</returns>
        public List<Detections> GetAllDetectionList()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetAllDetectionList(PCMConnectionString);
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

        /// <summary>
        /// This method is used to get Option of PCM.
        /// </summary>
        /// <returns>The list of options.</returns>
        public List<Options> GetAllOptionList()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetAllOptionList(PCMConnectionString);
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

        /// <summary>
        /// This method is used to get Spare part of PCM.
        /// </summary>
        /// <returns>The list of Spare Parts.</returns>
        public List<SpareParts> GetAllSparePartList()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetAllSparePartList(PCMConnectionString);
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

        /// <summary>
        /// This method is used to get All Customers of PCM.
        /// </summary>
        /// <returns>The list of Customers.</returns>
        public List<RegionsByCustomer> GetCustomersWithRegions()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetCustomersWithRegions(PCMConnectionString);
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

        /// <summary>
        /// This method is to get lookup values
        /// </summary>
        /// <param name="key">Get key</param>
        /// <returns>List of Lookup values</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///          IPCMService PCMControl = new PCMServiceController(ServiceUrl);
        ///          IList&lt;LookupValue&gt; list = PCMControl.GetLookupValues(45);
        ///     }
        /// }
        /// </code>
        /// </example>
        public IList<LookupValue> GetLookupValues(byte key)
        {
            IList<LookupValue> list = null;
            try
            {
                CrmManager mgr = new CrmManager();
                list = mgr.GetLookupValues(key);
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
            return list;
        }

        /// <summary>
        /// This method is used to insert Catalogue item.
        /// </summary>
        public CatalogueItem AddCatalogueItem(CatalogueItem catalogueItem)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddCatalogueItem(catalogueItem, MainServerConnectionString, Properties.Settings.Default.CatalogueItemFiles);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is used to update Catalogue item.
        /// </summary>
        public bool UpdateCatalogueItem(CatalogueItem catalogueItem)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateCatalogueItem(catalogueItem, MainServerConnectionString, Properties.Settings.Default.CatalogueItemFiles);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        /// <summary>
        /// This method is used to delete Catalogue item.
        /// </summary>
        public bool IsDeletedCatalogueItem(UInt32 idCatalogueItem)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.IsDeletedCatalogueItem(idCatalogueItem, MainServerConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is used to get Latest Catalogue Item Code.
        /// </summary>
        public string GetLatestCatalogueItemCode()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetLatestCatalogueItemCode(PCMConnectionString);
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

        /// <summary>
        /// This method is used to get All product types by template of PCM.
        /// </summary>
        /// <returns>The list of product types by template.</returns>
        public List<ProductTypes> GetProductTypesByTemplate(UInt64 idTemplate)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetProductTypesByTemplate(PCMConnectionString, idTemplate);
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

        /// <summary>
        /// This method is used to get Catalogue Item by catalogue item id of PCM.
        /// </summary>
        /// <returns>The data of catalogue item by catalogue item id.</returns>
        public CatalogueItem GetCatalogueItemByIdCatalogueItem(UInt32 IdCatalogueItem)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetCatalogueItemByIdCatalogueItem(PCMConnectionString, IdCatalogueItem, Properties.Settings.Default.CatalogueItemFiles);
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

        /// <summary>
        /// This method is used to get all Test Types of PCM.
        /// </summary>
        /// <returns>The list of Test Types.</returns>
        public List<TestTypes> GetAllTestTypes()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetAllTestTypes(PCMConnectionString);
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

        /// <summary>
        /// This method is used to insert product type.
        /// </summary>
        public ProductTypes AddProductType(ProductTypes productTypes)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.AddProductType(productTypes, MainServerConnectionString, Properties.Settings.Default.ProductTypeImages, Properties.Settings.Default.ProductTypeFiles, PCMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false || mgr.IsConnectionStringNameExist("PCMContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext/PCMContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is used to update product type
        /// </summary>
        public bool UpdateProductType(ProductTypes productTypes)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.UpdateProductType(productTypes, MainServerConnectionString, Properties.Settings.Default.ProductTypeImages, Properties.Settings.Default.ProductTypeFiles, PCMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false || mgr.IsConnectionStringNameExist("PCMContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext/PCMContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is used to insert detection.
        /// </summary>
        public DetectionDetails AddDetection(DetectionDetails detectionDetails)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddDetection(detectionDetails, MainServerConnectionString, Properties.Settings.Default.DetectionFiles, Properties.Settings.Default.DetectionImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is used to update detection
        /// </summary>
        public bool UpdateDetection(DetectionDetails detectionDetails)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateDetection(detectionDetails, MainServerConnectionString, Properties.Settings.Default.DetectionFiles, Properties.Settings.Default.DetectionImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is used to get product type by cptype id of PCM.
        /// </summary>
        /// <returns>The data of product type by cptype id.</returns>
        public ProductTypes GetProductTypeByIdCpType(UInt64 IdCpType, UInt64 IdTemplate)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetProductTypeByIdCpType(PCMConnectionString, IdCpType, Properties.Settings.Default.ProductTypeFiles, Properties.Settings.Default.ProductTypeImages, IdTemplate);
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

        /// <summary>
        /// This method is used to get detection by detection id of PCM.
        /// </summary>
        /// <returns>The data of detection by detection id.</returns>
        public DetectionDetails GetDetectionByIdDetection(UInt32 IdDetection)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetDetectionByIdDetection(PCMConnectionString, IdDetection, Properties.Settings.Default.DetectionFiles, Properties.Settings.Default.DetectionImages);
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

        /// <summary>
        /// This method is used to get All Languages of PCM.
        /// </summary>
        /// <returns>The list of language.</returns>
        public List<Language> GetAllLanguages()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetAllLanguages(PCMConnectionString);
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

        /// <summary>
        /// This method is used to delete product type image.
        /// </summary>
        public bool IsDeleteProductTypeImage(ProductTypeImage productTypeImage)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.IsDeleteProductTypeImage(productTypeImage, MainServerConnectionString, Properties.Settings.Default.ProductTypeImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is used to get product type image by id product type of PCM.
        /// </summary>
        /// <returns>The list of product type image by id product type.</returns>
        public List<ProductTypeImage> GetProductTypeImagesByIdProductType(UInt64 IdProductType)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetProductTypeImagesByIdProductType(PCMConnectionString, IdProductType, Properties.Settings.Default.ProductTypeImages);
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

        /// <summary>
        /// This method is used to get product type images by id of PCM.
        /// </summary>
        /// <returns>The list of product type images by id.</returns>
        public ProductTypeImage GetProductTypeImagesByIdProductTypeImage(UInt64 IdProductTypeImage)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetProductTypeImagesByIdProductTypeImage(PCMConnectionString, IdProductTypeImage, Properties.Settings.Default.ProductTypeImages);
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

        /// <summary>
        /// This method is used to delete product type Attached Doc.
        /// </summary>
        public bool IsDeleteProductTypeAttachedDoc(ProductTypeAttachedDoc productTypeAttachedDoc)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.IsDeleteProductTypeAttachedDoc(productTypeAttachedDoc, MainServerConnectionString, Properties.Settings.Default.ProductTypeFiles);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is used to get product type Attached Doc by id product type of PCM.
        /// </summary>
        /// <returns>The list of product type Attached Doc by id product type.</returns>
        public List<ProductTypeAttachedDoc> GetProductTypeAttachedDocsByIdProductType(UInt64 IdProductType)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetProductTypeAttachedDocsByIdProductType(PCMConnectionString, IdProductType, Properties.Settings.Default.ProductTypeFiles);
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

        /// <summary>
        /// This method is used to get product type Attached Docs by id of PCM.
        /// </summary>
        /// <returns>The list of product type Attached Docs by id.</returns>
        public ProductTypeAttachedDoc GetProductTypeAttachedDocsByIdProductTypeAttachedDoc(Int32 IdProductTypeAttachedDoc)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetProductTypeAttachedDocsByIdProductTypeAttachedDoc(PCMConnectionString, IdProductTypeAttachedDoc, Properties.Settings.Default.ProductTypeFiles);
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

        /// <summary>
        /// This method is used to get catalogue item Attached Doc by id catalogue item of PCM.
        /// </summary>
        /// <returns>The list of catalogue item Attached Doc by id catalogue item.</returns>
        public List<CatalogueItemAttachedDoc> GetCatalogueItemAttachedDocsByIdCatalogueItem(UInt32 IdCatalogueItem)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetCatalogueItemAttachedDocsByIdCatalogueItem(PCMConnectionString, IdCatalogueItem, Properties.Settings.Default.CatalogueItemFiles);
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

        /// <summary>
        /// This method is used to get catalogue item Attached Docs by id of PCM.
        /// </summary>
        /// <returns>The list of catalogue item Attached Docs by id.</returns>
        public CatalogueItemAttachedDoc GetCatalogueItemAttachedDocsById(UInt32 IdCatalogueItemAttachedDoc)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetCatalogueItemAttachedDocsById(PCMConnectionString, IdCatalogueItemAttachedDoc, Properties.Settings.Default.CatalogueItemFiles);
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

        /// <summary>
        /// This method is used to get Detection Attached Doc by id Detection of PCM.
        /// </summary>
        /// <returns>The list of Detection Attached Doc by id Detection.</returns>
        public List<DetectionAttachedDoc> GetDetectionAttachedDocsByIdDetection(UInt32 IdDetection)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetDetectionAttachedDocsByIdDetection(PCMConnectionString, IdDetection, Properties.Settings.Default.DetectionFiles);
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

        /// <summary>
        /// This method is used to get Detection Attached Docs by id of PCM.
        /// </summary>
        /// <returns>The list of Detection Attached Docs by id.</returns>
        public DetectionAttachedDoc GetDetectionAttachedDocsById(UInt32 IdDetectionAttachedDoc)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetDetectionAttachedDocsById(PCMConnectionString, IdDetectionAttachedDoc, Properties.Settings.Default.DetectionFiles);
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

        /// <summary>
        /// This method is used to get Catalogue Item Attached link by id Catalogue Item of PCM.
        /// </summary>
        /// <returns>The list of Catalogue Item Attached link by id Catalogue Item.</returns>
        public List<CatalogueItemAttachedLink> GetCatalogueItemAttachedLinksByIdCatalogueItem(UInt32 IdCatalogueItem)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetCatalogueItemAttachedLinksByIdCatalogueItem(PCMConnectionString, IdCatalogueItem);
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

        /// <summary>
        /// This method is used to get Catalogue Item Attached link by id of PCM.
        /// </summary>
        /// <returns>The list of Catalogue Item Attached link by id.</returns>
        public CatalogueItemAttachedLink GetCatalogueItemAttachedLinkById(UInt32 IdCatalogueItemAttachedLink)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetCatalogueItemAttachedLinkById(PCMConnectionString, IdCatalogueItemAttachedLink);
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

        /// <summary>
        /// This method is used to get Product Type Attached link by id Product Type of PCM.
        /// </summary>
        /// <returns>The list of Product Type Attached link by id Product Type.</returns>
        public List<ProductTypeAttachedLink> GetProductTypeAttachedLinksByIdProductType(UInt64 IdCPType)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetProductTypeAttachedLinksByIdProductType(PCMConnectionString, IdCPType);
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

        /// <summary>
        /// This method is used to get Product Type Attached link by id of PCM.
        /// </summary>
        /// <returns>The list of Product Type Attached link by id.</returns>
        public ProductTypeAttachedLink GetProductTypeAttachedLinkById(UInt32 IdCPTypeAttachedLink)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetProductTypeAttachedLinkById(PCMConnectionString, IdCPTypeAttachedLink);
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

        /// <summary>
        /// This method is used to get Detection Attached link by id Detection of PCM.
        /// </summary>
        /// <returns>The list of Detection Attached link by id Detection.</returns>
        public List<DetectionAttachedLink> GetDetectionAttachedLinksByIdDetection(UInt32 IdDetection)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetDetectionAttachedLinksByIdDetection(PCMConnectionString, IdDetection);
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

        /// <summary>
        /// This method is used to get Detection Attached link by id of PCM.
        /// </summary>
        /// <returns>The list of Detection Attached link by id.</returns>
        public DetectionAttachedLink GetDetectionAttachedLinkById(UInt32 IdDetectionAttachedLink)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetDetectionAttachedLinkById(PCMConnectionString, IdDetectionAttachedLink);
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

        /// <summary>
        /// This method is used to get Latest product type reference.
        /// </summary>
        public string GetLatestProuductTypeReference()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetLatestProuductTypeReference(PCMConnectionString);
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

        /// <summary>
        /// This method is used to delete product type.
        /// </summary>
        public bool IsDeletedProductType(UInt64 IdCPType)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.IsDeletedProductType(IdCPType, MainServerConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is used to get list of default way type.
        /// </summary>
        public List<DefaultWayType> GetAllDefaultWayTypeList()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetAllDefaultWayTypeList(PCMConnectionString);
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

        /// <summary>
        /// This method is used to get detection images by id detection of PCM.
        /// </summary>
        /// <returns>The list of detection images by id detection.</returns>
        public List<DetectionImage> GetDetectionImagesByIdDetection(UInt32 IdDetection)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetDetectionImagesByIdDetection(PCMConnectionString, IdDetection, Properties.Settings.Default.DetectionImages);
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

        /// <summary>
        /// This method is used to get detection images by id of PCM.
        /// </summary>
        /// <returns>The list of detection images by id.</returns>
        public DetectionImage GetDetectionImagesByIdDetectionImage(UInt32 IdDetectionImage)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetDetectionImagesByIdDetectionImage(PCMConnectionString, IdDetectionImage, Properties.Settings.Default.DetectionImages);
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

        /// <summary>
        /// This method is used to get detection groups by detection type of PCM.
        /// </summary>
        /// <returns>The list of detection groups by detection type.</returns>
        public List<DetectionGroup> GetDetectionGroupsByDetectionType(UInt32 IdDetectionType)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetDetectionGroupsByDetectionType(PCMConnectionString, IdDetectionType);
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

        /// <summary>
        /// This method is used to get detection order groups by detection type of PCM.
        /// </summary>
        /// <returns>The list of detection order groups by detection type.</returns>
        public List<DetectionOrderGroup> GetDetectionOrderGroupsWithDetections(UInt32 IdDetectionType)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetDetectionOrderGroupsWithDetections(PCMConnectionString, IdDetectionType);
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

        /// <summary>
        /// This method is used to get detection groups List of PCM.
        /// </summary>
        /// <returns>The list of detection groups.</returns>
        public List<DetectionGroup> GetDetectionGroupsList(UInt32 IdDetectionType)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetDetectionGroupsList(PCMConnectionString, IdDetectionType);
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

        /// <summary>
        /// This method is used to get detection groups by id group of PCM.
        /// </summary>
        /// <returns>The get detection groups by id group.</returns>
        public DetectionGroup GetDetectionGroupsByIdGroup(UInt32 IdGroup)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetDetectionGroupsByIdGroup(PCMConnectionString, IdGroup);
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

        /// <summary>
        /// This method is used to get detection concat by id group of PCM.
        /// </summary>
        /// <returns>The get detection concat by id group.</returns>
        public string GetDetectionsConcatByIdGroup(UInt32 IdGroup)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetDetectionsConcatByIdGroup(PCMConnectionString, IdGroup);
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

        /// <summary>
        /// This method is used to get detection groups of PCM.
        /// </summary>
        /// <returns>The get detection groups.</returns>
        public List<DetectionOrderGroup> GetDetectionOrderGroup(UInt32 IdDetectionType)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetDetectionOrderGroup(PCMConnectionString, IdDetectionType);
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

        /// <summary>
        /// This method is used to get detection groups of PCM.
        /// </summary>
        /// <returns>The get detection groups.</returns>
        public List<Detections> GetDetectionGroups(UInt32 IdDetectionType)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetDetectionGroups(PCMConnectionString, IdDetectionType);
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

        /// <summary>
        /// This method is used to get option groups of PCM.
        /// </summary>
        /// <returns>The get option groups.</returns>
        public List<Options> GetOptionGroups(UInt32 IdDetectionType)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetOptionGroups(PCMConnectionString, IdDetectionType);
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

        /// <summary>
        /// This method is used to get all detections with groups of PCM.
        /// </summary>
        /// <returns>The get all detections with groups.</returns>
        public List<Detections> GetAllDetectionsWithGroups()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetAllDetectionsWithGroups(PCMConnectionString);
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

        /// <summary>
        /// This method is used to get all options with groups of PCM.
        /// </summary>
        /// <returns>The get all options with groups.</returns>
        public List<Options> GetAllOptionsWithGroups()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetAllOptionsWithGroups(PCMConnectionString);
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


        /// <summary>
        /// This method is used to get article categories.
        /// </summary>
        /// <returns>The get article categories.</returns>
        public List<ArticleCategories> GetArticleCategories()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetArticleCategories(PCMConnectionString);
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

        /// <summary>
        /// This method is used to get all articles.
        /// </summary>
        /// <returns>The get all articles.</returns>
        public List<Articles> GetAllArticles()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetAllArticles(PCMConnectionString, Properties.Settings.Default.ArticleVisualAidsPath);
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

        /// <summary>
        /// This method is used to get articles by category.
        /// </summary>
        /// <returns>The get articles by category.</returns>
        public List<Articles> GetArticlesByCategory(uint IdArticleCategory)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetArticlesByCategory(PCMConnectionString, Properties.Settings.Default.ArticleVisualAidsPath, IdArticleCategory);
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

        /// <summary>
        /// This method is used to get pcm article categories.
        /// </summary>
        /// <returns>The get pcm article categories.</returns>
        public List<PCMArticleCategory> GetPCMArticleCategories()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetPCMArticleCategories(PCMConnectionString);
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

        /// <summary>
        /// This method is used to get all pcm articles.
        /// </summary>
        /// <returns>The get all pcm articles.</returns>
        public List<Articles> GetAllPCMArticles()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetAllPCMArticles(PCMConnectionString, Properties.Settings.Default.ArticleVisualAidsPath);
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

        /// <summary>
        /// This method is used to get articles by category.
        /// </summary>
        /// <returns>The get articles by category.</returns>
        public List<Articles> GetPCMArticlesByCategory(uint IdArticleCategory)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetPCMArticlesByCategory(PCMConnectionString, Properties.Settings.Default.ArticleVisualAidsPath, IdArticleCategory);
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

        /// <summary>
        /// This method is used to update PCM Article category in Article.
        /// </summary>
        public bool IsUpdatePCMArticleCategoryInArticle(uint IdPCMArticleCategory, List<Articles> ArticleList)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.IsUpdatePCMArticleCategoryInArticle(MainServerConnectionString, IdPCMArticleCategory, ArticleList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is used to get article categories.
        /// </summary>
        /// <returns>The get article categories.</returns>
        public List<ArticleCategories> GetActiveArticleCategories()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetActiveArticleCategories(PCMConnectionString);
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

        /// <summary>
        /// This method is used to get all articles.
        /// </summary>
        /// <returns>The get all articles.</returns>
        public List<Articles> GetAllActiveArticles()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetAllActiveArticles(PCMConnectionString, Properties.Settings.Default.ArticleVisualAidsPath);
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

        /// <summary>
        /// This method is used to get articles by category.
        /// </summary>
        /// <returns>The get articles by category.</returns>
        public List<Articles> GetActiveArticlesByCategory(uint IdArticleCategory)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetActiveArticlesByCategory(PCMConnectionString, Properties.Settings.Default.ArticleVisualAidsPath, IdArticleCategory);
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

        /// <summary>
        /// This method is used to get pcm article categories.
        /// </summary>
        /// <returns>The get pcm article categories.</returns>
        public List<PCMArticleCategory> GetActivePCMArticleCategories()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetActivePCMArticleCategories(PCMConnectionString);
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

        /// <summary>
        /// This method is used to get all pcm articles.
        /// </summary>
        /// <returns>The get all pcm articles.</returns>
        public List<Articles> GetAllActivePCMArticles()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetAllActivePCMArticles(PCMConnectionString, Properties.Settings.Default.ArticleVisualAidsPath);
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

        /// <summary>
        /// This method is used to get articles by category.
        /// </summary>
        /// <returns>The get articles by category.</returns>
        public List<Articles> GetActivePCMArticlesByCategory(uint IdArticleCategory)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetActivePCMArticlesByCategory(PCMConnectionString, Properties.Settings.Default.ArticleVisualAidsPath, IdArticleCategory);
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

        /// <summary>
        /// This method is used to insert PCM Article Category.
        /// </summary>
        public PCMArticleCategory AddPCMArticleCategory(PCMArticleCategory pcmArticleCategory, List<PCMArticleCategory> pcmArticleCategoryList)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddPCMArticleCategory(MainServerConnectionString, pcmArticleCategory, pcmArticleCategoryList, Properties.Settings.Default.ProductCategoriesImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is used to update PCM Article Category.
        /// </summary>
        public bool IsUpdatePCMArticleCategory(PCMArticleCategory pcmArticleCategory, List<PCMArticleCategory> pcmArticleCategoryList)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.IsUpdatePCMArticleCategory(MainServerConnectionString, pcmArticleCategory, pcmArticleCategoryList, Properties.Settings.Default.ProductCategoriesImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is used to get pcm article category by id.
        /// </summary>
        /// <returns>The get pcm article category by id.</returns>
        public PCMArticleCategory GetPCMArticleCategoryById(uint idPCMArticleCategory)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetPCMArticleCategoryById(PCMConnectionString, idPCMArticleCategory, Properties.Settings.Default.ProductCategoriesImages);
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



        /// <summary>
        /// This method is used to get pcm article by id.
        /// </summary>
        /// <returns>The get pcm article by id.</returns>
        public Articles GetArticleByIdArticle(UInt32 IdArticle)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetArticleByIdArticle(PCMConnectionString, Properties.Settings.Default.ArticleVisualAidsPath, IdArticle, Properties.Settings.Default.ArticleImages);
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

        /// <summary>
        /// This method is used to update pcm Article Category with status in article.
        /// </summary>
        public bool IsUpdatePCMArticleCategoryInArticleWithStatus(uint IdPCMArticleCategory, Articles Article)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.IsUpdatePCMArticleCategoryInArticleWithStatus(MainServerConnectionString, IdPCMArticleCategory, Article, Properties.Settings.Default.ArticleImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is used to Get Product Types With Template.
        /// </summary>
        /// <returns>get Product Types With Template.</returns>
        public List<ProductTypesTemplate> GetProductTypesWithTemplate()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetProductTypesWithTemplate(PCMConnectionString);
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

        /// <summary>
        /// This method is used to Get PCM Articles With Category.
        /// </summary>
        /// <returns>get PCM Articles With Category.</returns>
        public List<PCMArticleCategory> GetPCMArticlesWithCategory()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetPCMArticlesWithCategory(PCMConnectionString);
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


        /// <summary>
        /// This method is used to delete detection.
        /// </summary>
        public bool IsDeletedDetection(UInt32 IdDetection)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.IsDeletedDetection(IdDetection, MainServerConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        /// <summary>
        /// This method is used to Get DWOS.
        /// </summary>
        /// <returns>get all DWOS.</returns>
        public List<DetectionDetails> GetAllDetectionsWaysOptionsSpareParts()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetAllDetectionsWaysOptionsSpareParts(PCMConnectionString);
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

        public List<ArticleDocument> GetArticleAttachmentByIdArticle(UInt32 idArticle)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetArticleAttachmentByIdArticle(PCMConnectionString, idArticle);
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

        public bool IsUpdatedPCMArticleCategoryOrder(List<PCMArticleCategory> pcmArticleCategoryList, uint? IdModifier)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.IsUpdatedPCMArticleCategoryOrder(MainServerConnectionString, pcmArticleCategoryList, IdModifier);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        /// <summary>
        /// This method is used to Get PCM Articles With Category.
        /// </summary>
        /// <returns>get PCM Articles With Category.</returns>
        public List<PCMArticleCategory> GetPCMArticlesWithCategoryForReference()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetPCMArticlesWithCategoryForReference(PCMConnectionString);
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

        public bool IsDeletePCMArticleCategory(List<PCMArticleCategory> pcmArticleCategoryList)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.IsDeletePCMArticleCategory(MainServerConnectionString, pcmArticleCategoryList, Properties.Settings.Default.ProductCategoriesImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public bool IsUpdatePCMArticleCategoryInArticleWithStatus_V2060(uint IdPCMArticleCategory, Articles Article)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.IsUpdatePCMArticleCategoryInArticleWithStatus_V2060(MainServerConnectionString, IdPCMArticleCategory, Article, Properties.Settings.Default.ArticleImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<PCMArticleCategory> GetPCMArticleCategories_V2060()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetPCMArticleCategories_V2060(PCMConnectionString);
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

        public List<Articles> GetAllPCMArticles_V2060()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetAllPCMArticles_V2060(PCMConnectionString, Properties.Settings.Default.ArticleVisualAidsPath);
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
        public Articles GetArticleByIdArticle_V2060(UInt32 IdArticle)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetArticleByIdArticle_V2060(PCMConnectionString, Properties.Settings.Default.ArticleVisualAidsPath, IdArticle, Properties.Settings.Default.ArticleImages);
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

        public List<PCMArticleCategory> GetPCMArticlesWithCategory_V2060()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetPCMArticlesWithCategory_V2060(PCMConnectionString);
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

        public List<PCMArticleCategory> GetPCMArticlesWithCategoryForReference_V2060()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetPCMArticlesWithCategoryForReference_V2060(PCMConnectionString);
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


        public bool AddDeletePCMArticle(uint IdPCMArticleCategory, List<Articles> ArticleList, int IdUser)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddDeletePCMArticle(MainServerConnectionString, IdPCMArticleCategory, ArticleList, IdUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool IsUpdatePCMArticleCategoryInArticleWithStatus_V2070(uint IdPCMArticleCategory, Articles Article)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.IsUpdatePCMArticleCategoryInArticleWithStatus_V2070(MainServerConnectionString, IdPCMArticleCategory, Article, Properties.Settings.Default.ArticleImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Articles> GetAllPCMArticles_V2070()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetAllPCMArticles_V2070(PCMConnectionString, Properties.Settings.Default.ArticleVisualAidsPath);
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

        public Articles GetArticleByIdArticle_V2070(UInt32 IdArticle)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetArticleByIdArticle_V2070(PCMConnectionString, Properties.Settings.Default.ArticleVisualAidsPath, IdArticle, Properties.Settings.Default.ArticleImages);
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

        public bool IsUpdatePCMArticleCategoryInArticleWithStatus_V2090(uint IdPCMArticleCategory, Articles Article)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.IsUpdatePCMArticleCategoryInArticleWithStatus_V2090(MainServerConnectionString, IdPCMArticleCategory, Article, Properties.Settings.Default.ArticleImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public Articles GetArticleByIdArticle_V2090(UInt32 IdArticle)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetArticleByIdArticle_V2090(PCMConnectionString, Properties.Settings.Default.ArticleVisualAidsPath, IdArticle, Properties.Settings.Default.ArticleImages);
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

        public Articles GetArticleByIdArticle_V2100(UInt32 IdArticle)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetArticleByIdArticle_V2100(PCMConnectionString, Properties.Settings.Default.ArticleVisualAidsPath, IdArticle, Properties.Settings.Default.ArticleImages);
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

        public bool IsUpdatePCMArticleCategoryInArticleWithStatus_V2100(uint IdPCMArticleCategory, Articles Article)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.IsUpdatePCMArticleCategoryInArticleWithStatus_V2100(MainServerConnectionString, IdPCMArticleCategory, Article, Properties.Settings.Default.ArticleImages, Properties.Settings.Default.ArticleAttachmentDocPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<DocumentType> GetDocumentTypes()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetDocumentTypes(PCMConnectionString);
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

        public List<Articles> GetAllPCMArticles_V2100()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetAllPCMArticles_V2100(PCMConnectionString, Properties.Settings.Default.ArticleVisualAidsPath);
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


        public List<Articles> GetAllPCMArticles_V2110()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetAllPCMArticles_V2110(PCMConnectionString, Properties.Settings.Default.ArticleVisualAidsPath);
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


        public bool IsUpdatePCMArticleCategoryInArticleWithStatus_V2110(uint IdPCMArticleCategory, Articles Article)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.IsUpdatePCMArticleCategoryInArticleWithStatus_V2110(MainServerConnectionString, IdPCMArticleCategory, Article, Properties.Settings.Default.ArticleImages, Properties.Settings.Default.ArticleAttachmentDocPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<DetectionDetails> GetAllDetectionsWaysOptionsSpareParts_V2110()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetAllDetectionsWaysOptionsSpareParts_V2110(PCMConnectionString);
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


        public List<ProductTypes> GetAllProductTypes_V2110()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetAllProductTypes_V2110(PCMConnectionString);
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

        public int CheckPCMArticleExist(UInt32 IdArticle)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.CheckPCMArticleExist(PCMConnectionString, IdArticle);
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

        public string GetPCMArticleExistNames(UInt32 IdArticle)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetPCMArticleExistNames(PCMConnectionString, IdArticle);
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

        public Articles GetArticleByIdArticleForInformationData(uint IdArticle)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetArticleByIdArticleForInformationData(PCMConnectionString, IdArticle);
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


        public bool IsUpdatePCMArticle(uint IdPCMArticleCategory, Articles Article)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.IsUpdatePCMArticle(MainServerConnectionString, IdPCMArticleCategory, Article, Properties.Settings.Default.ArticleImages, Properties.Settings.Default.ArticleAttachmentDocPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public bool IsUpdatePCMArticleFromGrid(uint IdPCMArticleCategory, Articles Article)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.IsUpdatePCMArticleFromGrid(MainServerConnectionString, IdPCMArticleCategory, Article);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public Articles GetArticleByIdArticle_V2120(UInt32 IdArticle)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetArticleByIdArticle_V2120(PCMConnectionString, Properties.Settings.Default.ArticleVisualAidsPath, IdArticle, Properties.Settings.Default.ArticleImages);
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

        public string GetPCMArticleExistNames_V2120(UInt32 IdArticle)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetPCMArticleExistNames_V2120(PCMConnectionString, IdArticle);
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

        public bool AddDeletePCMArticle_V2120(uint IdPCMArticleCategory, List<Articles> ArticleList, int IdUser)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddDeletePCMArticle_V2120(MainServerConnectionString, IdPCMArticleCategory, ArticleList, IdUser, Properties.Settings.Default.ArticleImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is used to insert detection.
        /// </summary>
        public DetectionDetails AddDetection_V2140(DetectionDetails detectionDetails)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddDetection_V2140(detectionDetails, MainServerConnectionString, Properties.Settings.Default.DetectionFiles, Properties.Settings.Default.DetectionImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is used to update detection
        /// </summary>
        public bool UpdateDetection_V2140(DetectionDetails detectionDetails)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateDetection_V2140(detectionDetails, MainServerConnectionString, Properties.Settings.Default.DetectionFiles, Properties.Settings.Default.DetectionImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is used to get detection by detection id of PCM.
        /// </summary>
        /// <returns>The data of detection by detection id.</returns>
        public DetectionDetails GetDetectionByIdDetection_V2140(UInt32 IdDetection)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetDetectionByIdDetection_V2140(PCMConnectionString, IdDetection, Properties.Settings.Default.DetectionFiles, Properties.Settings.Default.DetectionImages);
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

        /// <summary>
        /// This method is used to get all detections with groups of PCM.
        /// </summary>
        /// <returns>The get all detections with groups.</returns>
        public List<SpareParts> GetAllSparepartsWithGroups()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetAllSparepartsWithGroups(PCMConnectionString);
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

        /// <summary>
        /// This method is used to get product type by cptype id of PCM.
        /// </summary>
        /// <returns>The data of product type by cptype id.</returns>
        public ProductTypes GetProductTypeByIdCpType_V2140(UInt64 IdCpType, UInt64 IdTemplate)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetProductTypeByIdCpType_V2140(PCMConnectionString, IdCpType, Properties.Settings.Default.ProductTypeFiles, Properties.Settings.Default.ProductTypeImages, IdTemplate);
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

        /// <summary>
        /// This method is used to insert product type.
        /// </summary>
        public ProductTypes AddProductType_V2140(ProductTypes productTypes)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.AddProductType_V2140(productTypes, MainServerConnectionString, Properties.Settings.Default.ProductTypeImages, Properties.Settings.Default.ProductTypeFiles, PCMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false || mgr.IsConnectionStringNameExist("PCMContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext/PCMContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is used to update product type
        /// </summary>
        public bool UpdateProductType_V2140(ProductTypes productTypes)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.UpdateProductType_V2140(productTypes, MainServerConnectionString, Properties.Settings.Default.ProductTypeImages, Properties.Settings.Default.ProductTypeFiles, PCMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false || mgr.IsConnectionStringNameExist("PCMContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext/PCMContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool AddDeletePCMArticle_V2140(uint IdPCMArticleCategory, List<Articles> ArticleList, int IdUser)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddDeletePCMArticle_V2140(MainServerConnectionString, IdPCMArticleCategory, ArticleList, IdUser, Properties.Settings.Default.ArticleImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is used to get detection by detection id of PCM.
        /// </summary>
        /// <returns>The data of detection by detection id.</returns>
        public DetectionDetails GetDetectionByIdDetection_V2160(UInt32 IdDetection)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetDetectionByIdDetection_V2160(PCMConnectionString, IdDetection, Properties.Settings.Default.DetectionFiles, Properties.Settings.Default.DetectionImages);
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


        public List<DetectionDetails> GetAllDetectionsWaysOptionsSpareParts_V2160()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetAllDetectionsWaysOptionsSpareParts_V2160(PCMConnectionString);
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


        /// <summary>
        /// This method is used to insert detection.
        /// </summary>
        public DetectionDetails AddDetection_V2160(DetectionDetails detectionDetails)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddDetection_V2160(detectionDetails, MainServerConnectionString, Properties.Settings.Default.DetectionFiles, Properties.Settings.Default.DetectionImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is used to update detection
        /// </summary>
        public bool UpdateDetection_V2160(DetectionDetails detectionDetails)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateDetection_V2160(detectionDetails, MainServerConnectionString, Properties.Settings.Default.DetectionFiles, Properties.Settings.Default.DetectionImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is used to update detection
        /// </summary>
        public bool IsUpdatePCM_DetectionECOSVisibility_Update_V2160(DetectionDetails detectionDetails)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.IsUpdatePCM_DetectionECOSVisibility_Update_V2160(MainServerConnectionString, detectionDetails);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is used to short Detection Details
        /// </summary>
        public List<DetectionDetails> PCM_GetshortDetectionDetails_V2160()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.PCM_GetshortDetectionDetails_V2160(PCMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        /// <summary>
        /// This method is used to dailyscheduledtaskToTransferMicroSigaInformation
        /// </summary>
        public bool IsUpdatePCM_MicroSigaInformation(List<MicroSigainformation> microSigainformation)
        {
            try
            {
                string MainConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.IsUpdatePCM_MicroSigaInformation(microSigainformation, MainConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public Articles GetArticleByIdArticle_V2170(UInt32 IdArticle)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetArticleByIdArticle_V2170(PCMConnectionString, Properties.Settings.Default.ArticleVisualAidsPath, IdArticle, Properties.Settings.Default.ArticleImages, Properties.Settings.Default.CurrenciesImages);
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


        public bool IsUpdatePCMArticle_V2170(uint IdPCMArticleCategory, Articles Article)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.IsUpdatePCMArticle_V2170(MainServerConnectionString, IdPCMArticleCategory, Article, Properties.Settings.Default.ArticleImages, Properties.Settings.Default.ArticleAttachmentDocPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is used to MicroSiga Information
        /// </summary>
        public bool IsUpdatePCM_MicroSigaInformation_V2180(List<MicroSigainformation> microSigainformation)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.IsUpdatePCM_MicroSigaInformation_V2180(microSigainformation, MainServerConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is used to update detection
        /// </summary>
        public bool UpdateDetection_V2180(DetectionDetails detectionDetails)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateDetection_V2180(detectionDetails, MainServerConnectionString, Properties.Settings.Default.DetectionFiles, Properties.Settings.Default.DetectionImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public DetectionDetails GetDetectionByIdDetection_V2180(UInt32 IdDetection)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetDetectionByIdDetection_V2180(PCMConnectionString, IdDetection,
                    Properties.Settings.Default.DetectionFiles,
                    Properties.Settings.Default.DetectionImages,
                    Properties.Settings.Default.CurrenciesImages);
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

        public List<PLMDetectionPrice> GetNotIncludedPLMDetectionPrices(
        string IdBasePriceListCommaSeparated, string IdCustomerPriceListCommaSeparated,
        UInt64 IdDetection)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetNotIncludedPLMDetectionPrices(PCMConnectionString,
                    IdBasePriceListCommaSeparated, IdCustomerPriceListCommaSeparated,
                    Properties.Settings.Default.CurrenciesImages, IdDetection);
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

        /// <summary>
        /// This method is used to insert detection.
        /// </summary>
        public DetectionDetails AddDetection_V2180(DetectionDetails detectionDetails)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddDetection_V2180(detectionDetails, MainServerConnectionString, Properties.Settings.Default.DetectionFiles, Properties.Settings.Default.DetectionImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
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


        public bool ImportArticleCostPriceCalculate(Company company, UInt64 itemArticle, List<PODetail> EWHQArticlesByArticleComponentpoDetailLst, List<ArticlesByArticle> LstAllArticlesByArticle, List<PODetail> ArticlesByArticleComponentpoDetailLst)
        {
            try
            {
                PLMManager mgr = new PLMManager();
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
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }


        }

        public List<PODetail> GetAllArticlesByArticleComponentMaxPOFromEWHQ(string plantconnectionstring)
        {
            try
            {
                PLMManager mgr = new PLMManager();
                return mgr.GetAllArticlesByArticleComponentMaxPOFromEWHQ(plantconnectionstring);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
               
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }


        }

        public List<ArticlesByArticle> GetAllArticlesByArticle(string connectionstring)
        {
            try
            {
                PLMManager mgr = new PLMManager();
                return mgr.GetAllArticlesByArticle(connectionstring);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();

                throw new FaultException<ServiceException>(exp, ex.ToString());
            }


        }

        public List<PODetail> GetAllArticlesByArticleComponentMaxPOFromPlant(string connectionstring)
        {
            try
            {
                PLMManager mgr = new PLMManager();
                return mgr.GetAllArticlesByArticleComponentMaxPOFromPlant(connectionstring);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();

                throw new FaultException<ServiceException>(exp, ex.ToString());
            }


        }

        public List<Articles> GetAllStatusPCMArticles()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllStatusPCMArticles(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();

                throw new FaultException<ServiceException>(exp, ex.ToString());
            }


        }

        public async Task<List<ErrorDetails>> IsPCMArticlesSynchronization(List<GeosAppSetting> GeosAppSettingList, BPLPlantCurrencyDetail itemBPLPlantCurrency, Articles UpdatedArticle)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return await mgr.IsPCMArticlesSynchronization(connectionString, GeosAppSettingList, itemBPLPlantCurrency, UpdatedArticle);
                //return Convert.ToString(mgr.IsPCMArticlesSynchronization(connectionString, tokenService, GeosAppSettingList, UpdatedArticle, ownerInfo));
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();

                throw new FaultException<ServiceException>(exp, ex.ToString());
            }


        }

        public async Task<List<ErrorDetails>> IsPCMAddDetectionSynchronization(List<GeosAppSetting> GeosAppSettingList, BPLPlantCurrencyDetail itemBPLPlantCurrency, DetectionDetails NewDetection)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return await mgr.IsPCMAddDetectionSynchronization(connectionString, GeosAppSettingList, itemBPLPlantCurrency, NewDetection);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();

                throw new FaultException<ServiceException>(exp, ex.ToString());
            }


        }

        public async Task<APIErrorDetailForErrorFalse> IsPCMDetectionSynchronization(List<GeosAppSetting> GeosAppSettingList)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return await mgr.IsPCMDetectionSynchronization(connectionString, GeosAppSettingList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();

                throw new FaultException<ServiceException>(exp, ex.ToString());
            }


        }
        public async Task<List<ErrorDetails>> IsPCMEditDetectionSynchronization(List<GeosAppSetting> GeosAppSettingList, BPLPlantCurrencyDetail itemBPLPlantCurrency, DetectionDetails UpdatedItem)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return await mgr.IsPCMEditDetectionSynchronization(connectionString, GeosAppSettingList, itemBPLPlantCurrency, UpdatedItem);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();

                throw new FaultException<ServiceException>(exp, ex.ToString());
            }


        }
        public async Task<APIErrorDetailForErrorFalse> IsPCMAddEditCategorySynchronization(List<GeosAppSetting> GeosAppSettingList)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return await mgr.IsPCMAddEditCategorySynchronization(connectionString, GeosAppSettingList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();

                throw new FaultException<ServiceException>(exp, ex.ToString());
            }


        }
        public async Task<APIErrorDetailForErrorFalse> IsPCMProductTypeArticleSynchronization(List<GeosAppSetting> GeosAppSettingList, Articles[] foundRow)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return await mgr.IsPCMProductTypeArticleSynchronization(connectionString, GeosAppSettingList, foundRow);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();

                throw new FaultException<ServiceException>(exp, ex.ToString());
            }


        }
        public async Task<List<ErrorDetails>> IsPCMSynchronization(List<GeosAppSetting> GeosAppSettingList, BPLPlantCurrencyDetail itemBPLPlantCurrency, string Details, string Name)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return await mgr.IsPCMSynchronization(connectionString, GeosAppSettingList, itemBPLPlantCurrency, Details, Name);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();

                throw new FaultException<ServiceException>(exp, ex.ToString());
            }


        }

        public DetectionDetails GetDetectionByIdDetection_V2240(UInt32 IdDetection)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetDetectionByIdDetection_V2240(PCMConnectionString, IdDetection,
                    Properties.Settings.Default.DetectionFiles,
                    Properties.Settings.Default.DetectionImages,
                    Properties.Settings.Default.CurrenciesImages);
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

        public List<PLMDetectionPrice> GetNotIncludedPLMDetectionPrices_V2240(string IdBasePriceListCommaSeparated, string IdCustomerPriceListCommaSeparated, UInt64 IdDetection)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetNotIncludedPLMDetectionPrices_V2240(PCMConnectionString,
                    IdBasePriceListCommaSeparated, IdCustomerPriceListCommaSeparated,
                    Properties.Settings.Default.CurrenciesImages, IdDetection);
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

        public Articles GetArticleByIdArticle_V2240(UInt32 IdArticle)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetArticleByIdArticle_V2240(PCMConnectionString, Properties.Settings.Default.ArticleVisualAidsPath, IdArticle, Properties.Settings.Default.ArticleImages, Properties.Settings.Default.CurrenciesImages);
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




        /// <summary>
        /// This method is used to get product type by cptype id of PCM.
        /// </summary>
        /// <returns>The data of product type by cptype id.</returns>
        public ProductTypes GetProductTypeByIdCpType_V2250(UInt64 IdCpType, UInt64 IdTemplate)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetProductTypeByIdCpType_V2250(PCMConnectionString, IdCpType, Properties.Settings.Default.ProductTypeFiles, Properties.Settings.Default.ProductTypeImages, IdTemplate);
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



        /// <summary>
        /// This method is used to insert product type.
        /// </summary>
        public ProductTypes AddProductType_V2250(ProductTypes productTypes)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.AddProductType_V2250(productTypes, MainServerConnectionString, Properties.Settings.Default.ProductTypeImages, Properties.Settings.Default.ProductTypeFiles, PCMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false || mgr.IsConnectionStringNameExist("PCMContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext/PCMContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        /// <summary>
        /// This method is used to update product type
        /// </summary>
        public bool UpdateProductType_V2250(ProductTypes productTypes)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.UpdateProductType_V2250(productTypes, MainServerConnectionString, Properties.Settings.Default.ProductTypeImages, Properties.Settings.Default.ProductTypeFiles, PCMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false || mgr.IsConnectionStringNameExist("PCMContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext/PCMContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public Articles GetArticleByIdArticle_V2250(UInt32 IdArticle, int IdUser)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetArticleByIdArticle_V2250(PCMConnectionString, Properties.Settings.Default.ArticleVisualAidsPath, IdArticle, Properties.Settings.Default.ArticleImages,
                    Properties.Settings.Default.CurrenciesImages, IdUser);
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

        public DetectionDetails GetDetectionByIdDetection_V2250(UInt32 IdDetection, int IdUser)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetDetectionByIdDetection_V2250(PCMConnectionString, IdDetection,
                    Properties.Settings.Default.DetectionFiles,
                    Properties.Settings.Default.DetectionImages,
                    Properties.Settings.Default.CurrenciesImages, IdUser);
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

        public List<PLMDetectionPrice> GetNotIncludedPLMDetectionPrices_V2250(string IdBasePriceListCommaSeparated, string IdCustomerPriceListCommaSeparated, UInt64 IdDetection, int IdUser)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetNotIncludedPLMDetectionPrices_V2250(PCMConnectionString,
                    IdBasePriceListCommaSeparated, IdCustomerPriceListCommaSeparated,
                    Properties.Settings.Default.CurrenciesImages, IdDetection, IdUser);
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

        //
        public bool IsUpdatePCMArticle_V2260(uint IdPCMArticleCategory, Articles Article)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.IsUpdatePCMArticle_V2260(MainServerConnectionString, IdPCMArticleCategory, Article, Properties.Settings.Default.ArticleImages, Properties.Settings.Default.ArticleAttachmentDocPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is used to insert product type.
        /// </summary>
        public ProductTypes AddProductType_V2260(ProductTypes productTypes)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.AddProductType_V2260(productTypes, MainServerConnectionString, Properties.Settings.Default.ProductTypeImages, Properties.Settings.Default.ProductTypeFiles, PCMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false || mgr.IsConnectionStringNameExist("PCMContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext/PCMContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is used to get All Customers of PCM.
        /// </summary>
        /// <returns>The list of Customers.</returns>
        public List<CPLCustomer> GetCustomersWithRegions_V2260(UInt64 IdCpType)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetCustomersWithRegions_V2260(PCMConnectionString, IdCpType);
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

        /// <summary>
        /// This method is used to update product type
        /// </summary>
        public bool UpdateProductType_V2260(ProductTypes productTypes)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.UpdateProductType_V2260(productTypes, MainServerConnectionString, Properties.Settings.Default.ProductTypeImages, Properties.Settings.Default.ProductTypeFiles, PCMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false || mgr.IsConnectionStringNameExist("PCMContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext/PCMContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        /// <summary>
        /// This method is used to get All Customers of PCM.
        /// </summary>
        /// <returns>The list of Customers.</returns>
        public List<CPLCustomer> GetCustomersWithRegionsByIdDetection_V2260(UInt32 IdDetection)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetCustomersWithRegionsByIdDetection_V2260(PCMConnectionString, IdDetection);
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
        /// <summary>
        /// This method is used to insert detection.
        /// </summary>
        public DetectionDetails AddDetection_V2260(DetectionDetails detectionDetails)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddDetection_V2260(detectionDetails, MainServerConnectionString, Properties.Settings.Default.DetectionFiles, Properties.Settings.Default.DetectionImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        /// <summary>
        /// This method is used to update detection
        /// </summary>
        public bool UpdateDetection_V2260(DetectionDetails detectionDetails)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateDetection_V2260(detectionDetails, MainServerConnectionString, Properties.Settings.Default.DetectionFiles, Properties.Settings.Default.DetectionImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        /// <summary>
        /// This method is used to MicroSiga Information
        /// </summary>
        public bool IsUpdatePCM_MicroSigaInformation_V2260(List<MicroSigainformation> microSigainformation)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.IsUpdatePCM_MicroSigaInformation_V2260(microSigainformation, MainServerConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }



        /// <summary>
        /// This method is used to get max welorder from detection type
        /// </summary>
        public Int64 GetDetectionTypeMaxWelOrder()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetDetectionTypeMaxWelOrder(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("PCMContext") == false)
                {
                    exp.ErrorMessage = "PCMContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        /// <summary>
        /// This method is used to is welorder exist in  detection type 
        /// </summary>
        public bool IsExistDetectionTypeWelOrder(UInt32 welOrder)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.IsExistDetectionTypeWelOrder(connectionString, welOrder);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("PCMContext") == false)
                {
                    exp.ErrorMessage = "PCMContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<ProductTypes> GetAllProductTypes_V2270()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetAllProductTypes_V2270(PCMConnectionString);
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


        /// <summary>
        /// This method is used to insert product type.
        /// </summary>
        public ProductTypes AddProductType_V2270(ProductTypes productTypes)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.AddProductType_V2270(productTypes, MainServerConnectionString, Properties.Settings.Default.ProductTypeImages, Properties.Settings.Default.ProductTypeFiles, PCMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false || mgr.IsConnectionStringNameExist("PCMContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext/PCMContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is used to get All Customers of PCM.
        /// </summary>
        /// <returns>The list of Customers.</returns>
        public List<CPLCustomer> GetCustomersWithRegions_V2280(UInt64 IdCpType)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetCustomersWithRegions_V2280(PCMConnectionString, IdCpType);
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


        /// <summary>
        /// This method is used to insert product type.
        /// </summary>
        public ProductTypes AddProductType_V2280(ProductTypes productTypes)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.AddProductType_V2280(productTypes, MainServerConnectionString, Properties.Settings.Default.ProductTypeImages, Properties.Settings.Default.ProductTypeFiles, PCMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false || mgr.IsConnectionStringNameExist("PCMContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext/PCMContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is used to update product type
        /// </summary>
        public bool UpdateProductType_V2280(ProductTypes productTypes)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.UpdateProductType_V2280(productTypes, MainServerConnectionString, Properties.Settings.Default.ProductTypeImages, Properties.Settings.Default.ProductTypeFiles, PCMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false || mgr.IsConnectionStringNameExist("PCMContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext/PCMContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is used to get All Customers of PCM.
        /// </summary>
        /// <returns>The list of Customers.</returns>
        public List<CPLCustomer> GetCustomersWithRegionsByIdDetection_V2280(UInt32 IdDetection)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetCustomersWithRegionsByIdDetection_V2280(PCMConnectionString, IdDetection);
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

        /// <summary>
        /// This method is used to update detection
        /// </summary>
        public bool UpdateDetection_V2280(DetectionDetails detectionDetails)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateDetection_V2280(detectionDetails, MainServerConnectionString, Properties.Settings.Default.DetectionFiles, Properties.Settings.Default.DetectionImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        /// <summary>
        /// This method is used to insert detection.
        /// </summary>
        public DetectionDetails AddDetection_V2280(DetectionDetails detectionDetails)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddDetection_V2280(detectionDetails, MainServerConnectionString, Properties.Settings.Default.DetectionFiles, Properties.Settings.Default.DetectionImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][GEOS2- 2571][04.07.2022][added field pcmCategoryInUse]
        public List<PCMArticleCategory> GetPCMArticleCategories_V2290()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetPCMArticleCategories_V2290(PCMConnectionString);
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


        //[rdixit][GEOS2-GEOS2-2571][06.07.2022]
        public List<Articles> GetAllPCMArticles_V2290()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetAllPCMArticles_V2290(PCMConnectionString, Properties.Settings.Default.ArticleVisualAidsPath);
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

        public List<ArticleDecomposition> GetArticleDeCompostionByIdArticle(UInt32 idArticle)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetArticleDeCompostionByIdArticle(PCMConnectionString, idArticle);
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

        public Articles GetArticleByIdArticle_V2290(UInt32 IdArticle, int IdUser)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetArticleByIdArticle_V2290(PCMConnectionString, Properties.Settings.Default.ArticleVisualAidsPath, IdArticle, Properties.Settings.Default.ArticleImages,
                    Properties.Settings.Default.CurrenciesImages, IdUser);
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

        public List<PCMArticleCategory> GetPCMArticlesWithCategoryForReference_V2290()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetPCMArticlesWithCategoryForReference_V2290(PCMConnectionString);
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


        public List<PCMArticleCategory> GetPCMArticlesWithCategory_V2290()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetPCMArticlesWithCategory_V2290(PCMConnectionString);
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

        /// <summary>
        /// This method is used to get pcm article category by id.
        /// </summary>
        /// <returns>The get pcm article category by id.</returns>
        public PCMArticleCategory GetPCMArticleCategoryById_V2290(uint idPCMArticleCategory)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetPCMArticleCategoryById_V2290(PCMConnectionString, idPCMArticleCategory, Properties.Settings.Default.ProductCategoriesImages);
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

        /// <summary>
        /// This method is used to update PCM Article Category.
        /// </summary>
        public bool IsUpdatePCMArticleCategory_V2290(PCMArticleCategory pcmArticleCategory, List<PCMArticleCategory> pcmArticleCategoryList)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.IsUpdatePCMArticleCategory_V2290(MainServerConnectionString, pcmArticleCategory, pcmArticleCategoryList, Properties.Settings.Default.ProductCategoriesImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public PCMArticleCategory AddPCMArticleCategory_V2290(PCMArticleCategory pcmArticleCategory, List<PCMArticleCategory> pcmArticleCategoryList)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddPCMArticleCategory_V2290(MainServerConnectionString, pcmArticleCategory, pcmArticleCategoryList, Properties.Settings.Default.ProductCategoriesImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        /// <summary>
        /// This method is used to MicroSiga Information
        /// </summary>
        public bool IsUpdatePCM_MicroSigaInformation_V2300(List<MicroSigainformation> microSigainformation)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.IsUpdatePCM_MicroSigaInformation_V2300(microSigainformation, MainServerConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is used to get all options with groups of PCM.
        /// </summary>
        /// <returns>The get all options with groups.</returns>
        public List<Options> GetAllOptionsWithGroups_V2300()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetAllOptionsWithGroups_V2300(PCMConnectionString);
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

        /// <summary>
        /// This method is used to get ways of PCM.
        /// </summary>
        /// <returns>The list of ways.</returns>
        public List<Ways> GetAllWayList_V2300()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetAllWayList_V2300(PCMConnectionString);
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

        /// <summary>
        /// This method is used to get all detections with groups of PCM.
        /// </summary>
        /// <returns>The get all detections with groups.</returns>
        public List<Detections> GetAllDetectionsWithGroups_V2300()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetAllDetectionsWithGroups_V2300(PCMConnectionString);
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

        /// <summary>
        /// This method is used to get all detections with groups of PCM.
        /// </summary>
        /// <returns>The get all detections with groups.</returns>
        public List<SpareParts> GetAllSparepartsWithGroups_V2300()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetAllSparepartsWithGroups_V2300(PCMConnectionString);
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

        /// <summary>
        /// This method is used to get Discounts.
        /// [rdixit][17.08.2022][GEOS2-3099]
        /// </summary>
        /// <returns></returns>
        public List<Discounts> GetDiscounts()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetDiscounts(PCMConnectionString);
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

        //[rdixit][12.09.2022][GEOS2-3100]
        public Discounts AddDiscount(Discounts Discount)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddDiscount(Discount, MainServerConnectionString);
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

        public async Task<List<ErrorDetails>> IsPCMArticlesSynchronization_V2310(List<GeosAppSetting> GeosAppSettingList, BPLPlantCurrencyDetail itemBPLPlantCurrency, Articles UpdatedArticle, bool IsArticleSynchronization)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return await mgr.IsPCMArticlesSynchronization_V2310(connectionString, GeosAppSettingList, itemBPLPlantCurrency, UpdatedArticle, IsArticleSynchronization);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();

                throw new FaultException<ServiceException>(exp, ex.ToString());
            }


        }

        //[rdixit][28.09.2022][GEOS2-3101]
        public List<Discounts> GetDiscounts_V2320()
        {
            try
            {
               
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetDiscounts_V2320(PCMConnectionString);
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

        //[rdixit][28.09.2022][GEOS2-3101]
        public Discounts UpdateDiscount(Discounts Discount, Discounts PrevDiscount)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateDiscount(Discount, PrevDiscount, MainServerConnectionString);
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

        //[rdixit][28.09.2022][GEOS2-3101]
        public List<DiscountCustomers> GetDiscountCustomers(int customer_DiscountId)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetDiscountCustomers(PCMConnectionString, customer_DiscountId);
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
        public List<DiscountLogEntry> GetDiscountLogEntriesByDiscountstring(int customer_DiscountId)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetDiscountLogEntriesByDiscountstring(PCMConnectionString, customer_DiscountId);
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

        //[pjadhav][31.10.2022][GEOS2-3956]
        public DetectionDetails AddDetection_V2330(DetectionDetails detectionDetails)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddDetection_V2330(detectionDetails, MainServerConnectionString, Properties.Settings.Default.DetectionFiles, Properties.Settings.Default.DetectionImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pjadhav][31.10.2022][GEOS2-3956]
        public bool UpdateDetection_V2330(DetectionDetails detectionDetails)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateDetection_V2330(detectionDetails, MainServerConnectionString, Properties.Settings.Default.DetectionFiles, Properties.Settings.Default.DetectionImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pjadhav][31.10.2022][GEOS2-3956]
        public List<DetectionDetails> GetAllDetectionsWaysOptionsSpareParts_V2330()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetAllDetectionsWaysOptionsSpareParts_V2330(PCMConnectionString);
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

        public DetectionDetails GetDetectionByIdDetection_V2330(UInt32 IdDetection, int IdUser)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetDetectionByIdDetection_V2330(PCMConnectionString, IdDetection,
                    Properties.Settings.Default.DetectionFiles,
                    Properties.Settings.Default.DetectionImages,
                    Properties.Settings.Default.CurrenciesImages, IdUser);
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


        public List<DetectionDetails> GetAllStructureDetectionsWaysOptionsSpareParts_V2330()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetAllStructureDetectionsWaysOptionsSpareParts_V2330(PCMConnectionString);
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

        public List<DetectionDetails> GetAllModuleDetectionsWaysOptionsSpareParts_V2330()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetAllModuleDetectionsWaysOptionsSpareParts_V2330(PCMConnectionString);
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

        //[pjadhav][24.11.2022][GEOS2-3970]
        public bool IsUpdatePCM_DetectionECOSVisibility_Update_V2340(DetectionDetails detectionDetails)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.IsUpdatePCM_DetectionECOSVisibility_Update_V2340(MainServerConnectionString, detectionDetails);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][GEOS2-3970][01.12.2022] 
        public List<DetectionDetails> GetAllStructureDetectionsWaysOptionsSpareParts_V2340()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetAllStructureDetectionsWaysOptionsSpareParts_V2340(PCMConnectionString);
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

        //[rdixit][GEOS2-3970][01.12.2022] 
        public List<DetectionDetails> GetAllModuleDetectionsWaysOptionsSpareParts_V2340()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetAllModuleDetectionsWaysOptionsSpareParts_V2340(PCMConnectionString);
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



        //[pjadhav][31.10.2022][GEOS2-3956]
        public bool UpdateDetection_V2340(DetectionDetails detectionDetails)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateDetection_V2340(detectionDetails, MainServerConnectionString, Properties.Settings.Default.DetectionFiles, Properties.Settings.Default.DetectionImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public DetectionDetails GetDetectionByIdDetection_V2340(UInt32 IdDetection, int IdUser)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetDetectionByIdDetection_V2340(PCMConnectionString, IdDetection,
                    Properties.Settings.Default.DetectionFiles,
                    Properties.Settings.Default.DetectionImages,
                    Properties.Settings.Default.CurrenciesImages, IdUser);
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

        //[rdixit][GEOS2-4074][12.12.2022]
        public DetectionDetails AddDetection_V2340(DetectionDetails detectionDetails)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddDetection_V2340(detectionDetails, MainServerConnectionString, Properties.Settings.Default.DetectionFiles, Properties.Settings.Default.DetectionImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[sudhir.Jangra][GEOS2-4072][09/12/2022]

        /// <summary>
        /// This method is used to insert product type.
        /// </summary>
        public ProductTypes AddProductType_V2340(ProductTypes productTypes)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.AddProductType_V2340(productTypes, MainServerConnectionString, Properties.Settings.Default.ProductTypeImages, Properties.Settings.Default.ProductTypeFiles, PCMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false || mgr.IsConnectionStringNameExist("PCMContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext/PCMContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[Sudhir.Jangra][GEOS2-4072][13/12/2022]
        /// <summary>
        /// This method is used to update product type
        /// </summary>
        public bool UpdateProductType_V2340(ProductTypes productTypes)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.UpdateProductType_V2340(productTypes, MainServerConnectionString, Properties.Settings.Default.ProductTypeImages, Properties.Settings.Default.ProductTypeFiles, PCMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false || mgr.IsConnectionStringNameExist("PCMContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext/PCMContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[Sudhir.Jangra][Geos-4072][13/12/2022]
        /// <summary>
        /// This method is used to get product type Attached Doc by id product type of PCM.
        /// </summary>
        /// <returns>The list of product type Attached Doc by id product type.</returns>
        public List<ProductTypeAttachedDoc> GetProductTypeAttachedDocsByIdProductType_V2340(UInt64 IdProductType)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetProductTypeAttachedDocsByIdProductType_V2340(PCMConnectionString, IdProductType, Properties.Settings.Default.ProductTypeFiles);
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


        // GEOS2-2596 Add option in PCM to print a datasheet of a Module [1 of 3] 06 01 2023
        /// <summary>
        /// This method is used to get product type by cptype id of PCM.
        /// </summary>
        /// <returns>The data of product type by cptype id.</returns>
        public ProductTypes GetProductTypeByIdCpType_V2350(UInt64 IdCpType, UInt64 IdTemplate)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetProductTypeByIdCpType_V2350(PCMConnectionString, IdCpType, Properties.Settings.Default.ProductTypeFiles, Properties.Settings.Default.ProductTypeImages, IdTemplate);
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
        // GEOS2-2596 Add option in PCM to print a datasheet of a Module [1 of 3] 06 01 2023
        public List<DetectiontypesInformations>GetDetectionTypes()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetDetectionTypes(PCMConnectionString);
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

        public Articles GetArticleByIdArticle_V2350(UInt32 IdArticle, int IdUser)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetArticleByIdArticle_V2350(PCMConnectionString, Properties.Settings.Default.ArticleVisualAidsPath, IdArticle, Properties.Settings.Default.ArticleImages,
                    Properties.Settings.Default.CurrenciesImages, Properties.Settings.Default.CountryFilePath, IdUser);
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

        public List<ArticleCustomer> GetArticleCustomers()
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetArticleCustomers(PLMConnectionString, Properties.Settings.Default.CountryFilePath);
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
        public bool IsUpdatePCMArticle_V2350(uint IdPCMArticleCategory, Articles Article)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.IsUpdatePCMArticle_V2350(MainServerConnectionString, IdPCMArticleCategory, Article, Properties.Settings.Default.ArticleImages, Properties.Settings.Default.ArticleAttachmentDocPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public List<ArticleCustomer> GetCustomersByIdArticleCustomerReferences(UInt64 IdArticleList)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetCustomersByIdArticleCustomerReferences(PCMConnectionString, IdArticleList, Properties.Settings.Default.CountryFilePath );
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

        //[sdeshpande][GEOS2-3759]
        public List<ProductTypes> GetAllProductTypes_V2360()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetAllProductTypes_V2360(PCMConnectionString);
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
        //Shubham[skadam] GEOS2-4091 Add option in PCM to print a datasheet of a Module [2 of 3]  06 02 2023
        public DetectionDetails GetDetectionByIdDetection_V2360(UInt32 IdDetection, int IdUser)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetDetectionByIdDetection_V2360(PCMConnectionString, IdDetection,
                    Properties.Settings.Default.DetectionFiles,
                    Properties.Settings.Default.DetectionImages,
                    Properties.Settings.Default.CurrenciesImages, IdUser);
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

        //Shubham[skadam] GEOS2-3890 Add a new option to send "Announcement" email for new and reviewed modules in PCM - 2 23 02 2023
        public List<Department> GetAllEmployeesForOrganizationByIdCompany_V2360(string idCompany)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesForOrganizationByIdCompany_V2330(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[plahange][GEOS2-2544][22.02.2022]
        public List<ConfigurationCategories> GetAllPCMCategories_V2360()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetAllPCMCategories_V2360(PCMConnectionString, Properties.Settings.Default.ArticleVisualAidsPath);
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

        //[rdixit][22.02.2023][GEOS2-4176]
        public List<PCMArticleSynchronization> GetBPLPlantCurrencyDetailByIdBPLAndIdCPL(Int32 IdArticle, string IdsBPL, string IdsCPL, string FilterString)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetBPLPlantCurrencyDetailByIdBPLAndIdCPL(PCMConnectionString, IdArticle, IdsBPL, IdsCPL, FilterString);
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
        // [Sudhir.Jangra][GEOS2-3891][27/02/2023]
        public PCMAnnouncementEmailDetails GetEmailForDows_V2360(DateTime startDate,DateTime endDate, Boolean NewChangeType, Boolean UpdateChangeType, string EmployeeContactEmail)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                string template = System.IO.File.ReadAllText(string.Format(@"{0}\{1}", Properties.Settings.Default.EmailTemplate, "PCMAnnouncementEmailTemplate.html"));
                return mgr.GetEmailForDows_V2360(startDate,endDate, PCMConnectionString, Properties.Settings.Default.EmailTemplate,
                    Properties.Settings.Default.MailServerName, Properties.Settings.Default.MailServerPort, Properties.Settings.Default.CommandTimeout,
                    NewChangeType, UpdateChangeType, template, EmployeeContactEmail);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //Shubham[skadam] GEOS2-3891 Add a new option to send "Announcement" email for new and reviewed modules in PCM - 3 01 03 2023
        /// <summary>
        /// This method is to read mail template details
        /// </summary>
        /// <param name="templateName">Get template name</param>
        /// <returns>Mail template</returns>
        public string ReadMailTemplate(string templateName)
        {
            try
            {
                return System.IO.File.ReadAllText(string.Format(@"{0}\{1}", Properties.Settings.Default.EmailTemplate, templateName));
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        #region GEOS2-3891
        public bool IsPCMAnnouncementEmailSend(string EmployeeContactEmail, string PCMAnnouncementEmailTemplate)
        {
            try
            {
                return mgr.IsPCMAnnouncementEmailSend(Properties.Settings.Default.MailServerName, Properties.Settings.Default.MailServerPort, Properties.Settings.Default.CommandTimeout
                     ,PCMAnnouncementEmailTemplate, EmployeeContactEmail);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
           
        }
        public bool IsPCMAnnouncementEmailSend_V2360(string EmployeeContactEmail, string PCMAnnouncementEmailTemplate, Dictionary<string, byte[]> AnnouncementFilebyte)
        {
            try
            {
                return mgr.IsPCMAnnouncementEmailSend_V2360(Properties.Settings.Default.MailServerName, Properties.Settings.Default.MailServerPort, Properties.Settings.Default.CommandTimeout
                     , PCMAnnouncementEmailTemplate, EmployeeContactEmail, AnnouncementFilebyte);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }
        //Shubham[skadam] GEOS2-3891 Add a new option to send "Announcement" email for new and reviewed modules in PCM - 3 01 03 2023
        public PCMAnnouncementEmailDetails GetEmailForDows_V2360New(DateTime startDate, DateTime endDate, Boolean NewChangeType, Boolean UpdateChangeType, string EmployeeContactEmail)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                string template = System.IO.File.ReadAllText(string.Format(@"{0}\{1}", Properties.Settings.Default.EmailTemplate, "PCMAnnouncementEmailTemplate.html"));
                return mgr.GetEmailForDows_V2360New(startDate, endDate, PCMConnectionString, Properties.Settings.Default.EmailTemplate,
                    Properties.Settings.Default.MailServerName, Properties.Settings.Default.MailServerPort, Properties.Settings.Default.CommandTimeout,
                    NewChangeType, UpdateChangeType, template, EmployeeContactEmail);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        #endregion

        public Dictionary<string, byte[]> GetCountryIconFileInBytes()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetCountryIconBytes(Properties.Settings.Default.CountryFilePath, PCMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][Geos2-2922][17/03/2023]
        public List<Articles> GetAllPCMArticles_V2370()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetAllPCMArticles_V2370(PCMConnectionString, Properties.Settings.Default.ArticleVisualAidsPath, Properties.Settings.Default.ArticleImages);
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

        //[Sudhir.Jangra][GEOS2-2922][20/03/2023]
        public List<DetectionDetails> GetAllModuleDetectionsWaysOptionsSpareParts_V2370()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetAllModuleDetectionsWaysOptionsSpareParts_V2370(PCMConnectionString, Properties.Settings.Default.DetectionImages);
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

        //[Sudhir.Jangra][GEOS2-2922][20/03/2023]
        /// <summary>
        /// This method is used to get detection images by id detection of PCM.
        /// </summary>
        /// <returns>The list of detection images by id detection.</returns>
        public ObservableCollection<DetectionImage> GetDetectionImagesByIdDetection_V2370(UInt32 IdDetection)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetDetectionImagesByIdDetection_V2370(PCMConnectionString, IdDetection, Properties.Settings.Default.DetectionImages);
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
        //[Sudhir.Jangra][GEOS2-2922][23/03/2023]
        public List<ProductTypes> GetAllProductTypes_V2370()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetAllProductTypes_V2370(PCMConnectionString, Properties.Settings.Default.ProductTypeImages);
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

        //[Sudhir.Jangra][GEOS2-2922][23/03/2023]
        public List<PCMArticleImage> GetArticleImage_V2370(UInt32 IdArticle, string articleReference)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetArticleImage_V2370(PCMConnectionString, Properties.Settings.Default.ArticleVisualAidsPath, IdArticle,articleReference, Properties.Settings.Default.ArticleImages);
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

        /// <summary>
        /// This method is used to Get Product Types With Template.
        /// </summary>
        /// <returns>get Product Types With Template.</returns>
        public List<BPLModule> GetProductTypesWithTemplate_V2370()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetProductTypesWithTemplate_V2370(PCMConnectionString);
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

        //[Sudhir.Jangra][GEOS2-2922][29/03/2023]
        public List<DetectionDetails> GetAllStructureDetectionsWaysOptionsSpareParts_V2370()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetAllStructureDetectionsWaysOptionsSpareParts_V2370(PCMConnectionString, Properties.Settings.Default.DetectionImages);
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
        #region V2380
        //[Sudhir.Jangra][GEOS2-4221][12/04/2023]
        /// <summary>
        /// This method is used to insert product type.
        /// </summary>
        public ProductTypes AddProductType_V2380(ProductTypes productTypes)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.AddProductType_V2380(productTypes, MainServerConnectionString, Properties.Settings.Default.ProductTypeImages, Properties.Settings.Default.ProductTypeFiles, PCMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false || mgr.IsConnectionStringNameExist("PCMContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext/PCMContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[Sudhir.Jangra][GEOS2-4221][12/04/2023]
        /// <summary>
        /// This method is used to update product type
        /// </summary>
        public bool UpdateProductType_V2380(ProductTypes productTypes)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.UpdateProductType_V2380(productTypes, MainServerConnectionString, Properties.Settings.Default.ProductTypeImages, Properties.Settings.Default.ProductTypeFiles, PCMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false || mgr.IsConnectionStringNameExist("PCMContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext/PCMContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[Sudhir.Jangra][GEOS2-4221][12/04/2023]
        /// <summary>
        /// This method is used to get product type by cptype id of PCM.
        /// </summary>
        /// <returns>The data of product type by cptype id.</returns>
        public ProductTypes GetProductTypeByIdCpType_V2380(UInt64 IdCpType, UInt64 IdTemplate)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetProductTypeByIdCpType_V2380(PCMConnectionString, IdCpType, Properties.Settings.Default.ProductTypeFiles, Properties.Settings.Default.ProductTypeImages, IdTemplate);
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
        //[rdixit][GEOS2-2922][24.04.2023]
        public List<Articles> GetAllPCMArticles_V2380()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetAllPCMArticles_V2380(PCMConnectionString, Properties.Settings.Default.ArticleVisualAidsPath, Properties.Settings.Default.ArticleImages);
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
        //[rdixit][GEOS2-2922][24.04.2023]
        public List<DetectionDetails> GetAllStructureDetectionsWaysOptionsSpareParts_V2380()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetAllStructureDetectionsWaysOptionsSpareParts_V2380(PCMConnectionString, Properties.Settings.Default.DetectionImages);
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
        //[rdixit][GEOS2-2922][24.04.2023]
        public List<DetectionDetails> GetAllModuleDetectionsWaysOptionsSpareParts_V2380()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetAllModuleDetectionsWaysOptionsSpareParts_V2380(PCMConnectionString, Properties.Settings.Default.DetectionImages);
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
        //[rdixit][GEOS2-2922][24.04.2023]
        public List<ProductTypes> GetAllProductTypes_V2380()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetAllProductTypes_V2380(PCMConnectionString, Properties.Settings.Default.ProductTypeImages);
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
        //[rdixit][GEOS2-2922][24.04.2023]
        public List<PCMArticleImage> GetArticleImage_V2380(Articles article)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetArticleImage_V2380(PCMConnectionString, Properties.Settings.Default.ArticleVisualAidsPath, article);
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
        //[rdixit][GEOS2-2922][24.04.2023]
        public ObservableCollection<DetectionImage> GetDetectionImagesByIdDetection_V2380(UInt32 IdDetection)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetDetectionImagesByIdDetection_V2380(PCMConnectionString, IdDetection, Properties.Settings.Default.DetectionImages);
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
        //[rdixit][GEOS2-2922][24.04.2023]
        public List<ProductTypeImage> GetProductTypeImagesByIdProductTypeForGrid_V2380(UInt64 IdProductType)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetProductTypeImagesByIdProductTypeForGrid_V2380(PCMConnectionString, IdProductType, Properties.Settings.Default.ProductTypeImages);
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

        //[Sudhir.Jangra][GEOS2-4221]
        /// <summary>
        /// This method is used to get product type by cptype id of PCM.
        /// </summary>
        /// <returns>The data of product type by cptype id.</returns>
        public ProductTypes GetProductTypeByIdCpType_V2390(UInt64 IdCpType, UInt64 IdTemplate)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetProductTypeByIdCpType_V2390(PCMConnectionString, IdCpType, Properties.Settings.Default.ProductTypeFiles, Properties.Settings.Default.ProductTypeImages, IdTemplate);
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


        //[Sudhir.Jangra][GEOS2-4468][01/06/2023]
        public DetectionDetails AddDetection_V2400(DetectionDetails detectionDetails)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddDetection_V2400(detectionDetails, MainServerConnectionString, Properties.Settings.Default.DetectionFiles, Properties.Settings.Default.DetectionImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[Sudhir.Jangra][GEOS2-4460][26/06/2023]
        /// <summary>
        /// This method is used to get product type by cptype id of PCM.
        /// </summary>
        /// <returns>The data of product type by cptype id.</returns>
        public List<ProductTypes> GetProductTypesByDetection_V2410()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetProductTypesByDetection_V2410(PCMConnectionString);
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

        //[Sudhir.Jangra][GEOS2-4460][28/06/2023]
        public bool UpdateDetection_V2410(DetectionDetails detectionDetails)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateDetection_V2410(detectionDetails, MainServerConnectionString, Properties.Settings.Default.DetectionFiles, Properties.Settings.Default.DetectionImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        #endregion
        //Shubham[skadam]  GEOS2-4394 Modify some names in email about changes in Commercial Catalogue 28 07 2023 
        public bool IsPCMAnnouncementEmailSend_V2420(string EmployeeContactEmail, string PCMAnnouncementEmailTemplate, Dictionary<string, byte[]> AnnouncementFilebyte)
        {
            try
            {
                return mgr.IsPCMAnnouncementEmailSend_V2420(Properties.Settings.Default.MailServerName, Properties.Settings.Default.MailServerPort, Properties.Settings.Default.CommandTimeout
                     , PCMAnnouncementEmailTemplate, EmployeeContactEmail, AnnouncementFilebyte);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }
        //Shubham[skadam]  GEOS2-4394 Modify some names in email about changes in Commercial Catalogue 28 07 2023 
        public PCMAnnouncementEmailDetails GetEmailForDows_V2420(DateTime startDate, DateTime endDate, Boolean NewChangeType, Boolean UpdateChangeType, string EmployeeContactEmail)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                string template = System.IO.File.ReadAllText(string.Format(@"{0}\{1}", Properties.Settings.Default.EmailTemplate, "PCMAnnouncementEmailTemplate.html"));
                return mgr.GetEmailForDows_V2420(startDate, endDate, PCMConnectionString, Properties.Settings.Default.EmailTemplate,
                    Properties.Settings.Default.MailServerName, Properties.Settings.Default.MailServerPort, Properties.Settings.Default.CommandTimeout,
                    NewChangeType, UpdateChangeType, template, EmployeeContactEmail);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[Sudhir.Jangra][GEOS2-4733][22/08/2023]
        public List<Options> GetAllOptionsWithGroups_V2430(int IdScope)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetAllOptionsWithGroups_V2430(PCMConnectionString, IdScope);
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

        //[Sudhir.Jangra][GEOS2-4733][22/08/2023]
        public List<Detections> GetAllDetectionsWithGroups_V2430(int IdScope)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetAllDetectionsWithGroups_V2430(PCMConnectionString, IdScope);
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

        //[PRAMOD.MISAL][GEOS2-4442][29-08-2023]
        public List<FreePlugins> GetAllFreePlugins_byPermission()
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllFreePlugins_byPermission(PLMConnectionString);
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

        public FreePlugins AddUpdateFreePlugins(FreePlugins freePlugins)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddUpdateFreePlugins(MainServerConnectionString,  freePlugins);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<FreePlugins> GetHardlockFreePluginNames()
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllHardLockPlugins_byPermission(PLMConnectionString);
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

        //[Sudhir.Jangra][GEOS2-4441][21/09/2023]
        public List<HardLockLicenses> GetAllHardLockLicenses_V2440()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetAllHardLockLicenses_V2440(PCMConnectionString);
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

        //[sudhir.jangra][GEOS2-4441]
        public Articles GetArticleByIdArticle_V2440(UInt32 IdArticle,UInt32 IdPCMArticle, int IdUser)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetArticleByIdArticle_V2440(PCMConnectionString, Properties.Settings.Default.ArticleVisualAidsPath, IdArticle, IdPCMArticle, Properties.Settings.Default.ArticleImages,
                    Properties.Settings.Default.CurrenciesImages, Properties.Settings.Default.CountryFilePath, IdUser);
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


        //[Sudhir.Jangra][GEOS2-4809]
        public List<Articles> GetAllPCMArticles_V2440()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetAllPCMArticles_V2440(PCMConnectionString, Properties.Settings.Default.ArticleVisualAidsPath, Properties.Settings.Default.ArticleImages);
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

        public bool AddUpdateFreePlugins_V2440(List<FreePlugins> freePlugins)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddUpdateFreePlugins_V2440(MainServerConnectionString, freePlugins);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[Sudhir.jangra][GEOS2-4901]
        public List<HardLockLicenses> GetArticlesForAddEditHardLockLicense_V2450()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetArticlesForAddEditHardLockLicense_V2450(PCMConnectionString);
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

        //[Sudhir.jangra][GEOS2-4901]
        public List<HardLockPlugins> GetAllHardLockPluginForAddEditHardLockLicense_V2450()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetAllHardLockPluginForAddEditHardLockLicense_V2450(PCMConnectionString);
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

        //[Sudhir.Jangra][GEOS2-4901]
        public bool AddHardLockLicense_V2450(UInt32 IdArticle, List<HardLockPlugins> pluginList)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddHardLockLicense_V2450(MainServerConnectionString, IdArticle, pluginList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[Sudhir.Jangra][GEOS2-4901]
        public List<HardLockPlugins> GetSupportedPluginByIdArticle_V2450(UInt32 idArticle)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetSupportedPluginByIdArticle_V2450(PCMConnectionString, idArticle);
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

        //[Sudhir.Jangra][GEOS2-4901]
        public bool DeleteSupportedPluginForHardLockLicense_V2450(UInt32 idPlugin, UInt32 idArticle)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.DeleteSupportedPluginForHardLockLicense_V2450(MainServerConnectionString, idPlugin,idArticle);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[Sudhir.Jangra]
        public HardLockPlugins GetHardLockPluginId_V2450()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetHardLockPluginId_V2450(PCMConnectionString);
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

        //[Sudhir.Jangra][GEOS2-4915]
        public bool AddHardLockPlugin_V2450(UInt32 idPlugin,string Name)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddHardLockPlugin_V2450(MainServerConnectionString, idPlugin, Name);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }
        //[rdixit][GEOS2-4897][01.12.2023]
        public List<Articles> GetAllPCMArticles_V2460(UInt32 idCurrency)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetAllPCMArticles_V2460(PCMConnectionString, Properties.Settings.Default.ArticleVisualAidsPath, Properties.Settings.Default.ArticleImages, idCurrency);
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

        //[rdixit][GEOS2-4897][01.12.2023]
        public List<Company> GetEmdepSites()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetEmdepSites(PCMConnectionString);
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

        //[Sudhir.Jangra][GEOS2-4874]
        public List<ArticleCategory> GetWMSArticlesWithCategoryForReference()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetWMSArticlesWithCategoryForReference(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pramod.misal][GEOS2-5134][18-12-2023]
        public List<HardLockPlugins> GetSupportedPluginByIdArticle_V2470(UInt32 idArticle)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetSupportedPluginByIdArticle_V2470(PCMConnectionString, idArticle);
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
        //Shubham[skadam] GEOS2-5133 Add flag in country column loaded through url service 20 12 2023
        public HardLockPlugins GetHardLockPluginId_V2470()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetHardLockPluginId_V2470(PCMConnectionString);
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

        //[Sudhir.jangra][GEOS2-4935]
        /// <summary>
        /// This method is used to update product type
        /// </summary>
        public bool UpdateProductType_V2470(ProductTypes productTypes)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.UpdateProductType_V2470(productTypes, MainServerConnectionString, Properties.Settings.Default.ProductTypeImages, Properties.Settings.Default.ProductTypeFiles, PCMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false || mgr.IsConnectionStringNameExist("PCMContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext/PCMContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.jangra][GEOS2-4935]
        public ProductTypes AddProductType_V2470(ProductTypes productTypes)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.AddProductType_V2470(productTypes, MainServerConnectionString, Properties.Settings.Default.ProductTypeImages, Properties.Settings.Default.ProductTypeFiles, PCMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false || mgr.IsConnectionStringNameExist("PCMContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext/PCMContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[Sudhir.Jangra][GEOS2-4935]
        /// <summary>
        /// This method is used to get product type by cptype id of PCM.
        /// </summary>
        /// <returns>The data of product type by cptype id.</returns>
        public ProductTypes GetProductTypeByIdCpType_V2470(UInt64 IdCpType, UInt64 IdTemplate)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetProductTypeByIdCpType_V2470(PCMConnectionString, IdCpType, Properties.Settings.Default.ProductTypeFiles, Properties.Settings.Default.ProductTypeImages, IdTemplate);
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

        public List<ArticleCategorieMapping> GetWMS_PCMCategoryMapping()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetWMS_PCMCategoryMapping(connectionString);

            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool AddDeleteArticleCategoryMapping(List<ArticleCategorieMapping> ArticleCategoryMappingList)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddDeleteArticleCategoryMapping(MainServerConnectionString, ArticleCategoryMappingList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }

        //[Sudhir.Jangra][GEOS2-4935]
        public Discounts AddDiscount_V2470(Discounts Discount)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddDiscount_V2470(Discount, MainServerConnectionString);
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
        public List<DiscountLogEntry> GetDiscountLogEntriesByDiscountstring_V2470(int customer_DiscountId)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetDiscountLogEntriesByDiscountstring_V2470(PCMConnectionString, customer_DiscountId);
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

        //[Sudhir.Jangra][GEOS2-4935]
        public List<DiscountLogEntry> GetDiscountCommentsByDiscountstring_V2470(int customer_DiscountId)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetDiscountCommentsByDiscountstring_V2470(PCMConnectionString, customer_DiscountId);
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



        //[Sudhir.jangra][GEOS2-4935]
        public Discounts UpdateDiscount_V2470(Discounts Discount, Discounts PrevDiscount)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateDiscount_V2470(Discount, PrevDiscount, MainServerConnectionString);
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

        //[rdixit][27.12.2023][GEOS2-4875][GEOS2-48756]
        public List<Articles> AddWMSTOPCMArticlesByCategories(List<ArticleCategorieMapping> ArticleCategoryMappingList)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.AddWMSTOPCMArticlesByCategories(connectionString, MainServerConnectionString, ArticleCategoryMappingList);

            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[rdixit][27.12.2023][GEOS2-4875][GEOS2-48756]
        public List<ArticleCategorieMapping> GetWMS_PCMCategoryMappingOfToday()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetWMS_PCMCategoryMappingOfToday(connectionString);

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
        public DetectionDetails AddDetection_V2470(DetectionDetails detectionDetails)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddDetection_V2470(detectionDetails, MainServerConnectionString, Properties.Settings.Default.DetectionFiles, Properties.Settings.Default.DetectionImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[Sudhir.Jangra][GEOS2-4935]
        public DetectionDetails GetDetectionByIdDetection_V2470(UInt32 IdDetection, int IdUser)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetDetectionByIdDetection_V2470(PCMConnectionString, IdDetection,
                    Properties.Settings.Default.DetectionFiles,
                    Properties.Settings.Default.DetectionImages,
                    Properties.Settings.Default.CurrenciesImages, IdUser);
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


        //[Sudhir.jangra][GEOS2-4935]
        public bool UpdateDetection_V2470(DetectionDetails detectionDetails)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateDetection_V2470(detectionDetails, MainServerConnectionString, Properties.Settings.Default.DetectionFiles, Properties.Settings.Default.DetectionImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
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

        //[Sudhir.Jangra][GEOS2-4935]
        //Shubham[skadam] GEOS2-5307 Modules Window 23 02 2024
        /// <summary>
        /// This method is used to get product type by cptype id of PCM.
        /// </summary>
        /// <returns>The data of product type by cptype id.</returns>
        public ProductTypes GetProductTypeByIdCpType_V2490(UInt64 IdCpType, UInt64 IdTemplate, int IdUser)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetProductTypeByIdCpType_V2490(PCMConnectionString, IdCpType, Properties.Settings.Default.ProductTypeFiles, Properties.Settings.Default.ProductTypeImages, IdTemplate,
                    Properties.Settings.Default.CurrenciesImages, IdUser);
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

        //[Sudhir.jangra][GEOS2-4935]
        //Shubham[skadam] GEOS2-5307 Modules Window 23 02 2024
        /// <summary>
        /// This method is used to update product type
        /// </summary>
        public bool UpdateProductType_V2490(ProductTypes productTypes)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.UpdateProductType_V2490(productTypes, MainServerConnectionString, Properties.Settings.Default.ProductTypeImages, Properties.Settings.Default.ProductTypeFiles, PCMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false || mgr.IsConnectionStringNameExist("PCMContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext/PCMContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[Sudhir.jangra][GEOS2-4935]
        //Shubham[skadam] GEOS2-5307 Modules Window 23 02 2024
        public ProductTypes AddProductType_V2490(ProductTypes productTypes)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.AddProductType_V2490(productTypes, MainServerConnectionString, Properties.Settings.Default.ProductTypeImages, Properties.Settings.Default.ProductTypeFiles, PCMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false || mgr.IsConnectionStringNameExist("PCMContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext/PCMContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //Shubham[skadam] GEOS2-5307 Modules Window 23 02 2024
        public List<PLMModulePrice> GetNotIncludedPLMModulePrices_V2490(string IdBasePriceListCommaSeparated, string IdCustomerPriceListCommaSeparated, UInt64 IdCPType, int IdUser)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetNotIncludedPLMModulePrices_V2490(PCMConnectionString,
                    IdBasePriceListCommaSeparated, IdCustomerPriceListCommaSeparated,
                    Properties.Settings.Default.CurrenciesImages, IdCPType, IdUser);
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

        public bool IsDeletedDetection_V2500(UInt32 IdDetection,string detectionName)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.IsDeletedDetection_V2500(IdDetection, detectionName, MainServerConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
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

        //[rushikesh.gaikwad][GEOS2-5583][19.06.2023]

        public ProductTypes GetProductTypeByIdCpType_V2530(UInt64 IdCpType, UInt64 IdTemplate, int IdUser)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetProductTypeByIdCpType_V2530(PCMConnectionString, IdCpType, Properties.Settings.Default.ProductTypeFiles, Properties.Settings.Default.ProductTypeImages, IdTemplate,
                    Properties.Settings.Default.CurrenciesImages, IdUser);
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

        //[rushikesh.gaikwad][GEOS2-5583][19.06.2023]
        public ProductTypes AddProductType_V2530(ProductTypes productTypes)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.AddProductType_V2530(productTypes, MainServerConnectionString, Properties.Settings.Default.ProductTypeImages, Properties.Settings.Default.ProductTypeFiles, PCMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false || mgr.IsConnectionStringNameExist("PCMContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext/PCMContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rushikesh.gaikwad][GEOS2-5583][19.06.2023]
        public bool UpdateProductType_V2530(ProductTypes productTypes)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.UpdateProductType_V2530(productTypes, MainServerConnectionString, Properties.Settings.Default.ProductTypeImages, Properties.Settings.Default.ProductTypeFiles, PCMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false || mgr.IsConnectionStringNameExist("PCMContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext/PCMContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<CPLCustomer> GetCustomersWithRegions_V2530(UInt64 IdCpType)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetCustomersWithRegions_V2530(PCMConnectionString, IdCpType);
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

        //[rdixit][15.07.2024][rdixit]
        public List<ArticleCostPrice> GetArticleCostPricesByCurrency_V2540(UInt32 idCurrency)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetArticleCostPricesByCurrency_V2540(PCMConnectionString, idCurrency);
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
        //[rdixit][15.07.2024][rdixit]
        public List<Articles> GetAllPCMArticles_V2540()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetAllPCMArticles_V2540(PCMConnectionString, Properties.Settings.Default.ArticleVisualAidsPath, Properties.Settings.Default.ArticleImages);
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
        //[rdixit][15.07.2024][rdixit]
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
        //[rdixit][15.07.2024][rdixit]
        public List<BasePriceListByItem> GetSalesPriceForPCMArticleByBPL()
        {
            try
            {

                string connectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetSalesPriceForPCMArticleByBPL(connectionString);
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
        //[Rahul.Gadhave][GEOS2-5898][Date:21-08-2024]
        public List<DetectionChangeDetails> GetDailyUpdateChanges_V2550(DateTime date)
        {
            try
            {

                string connectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetDailyUpdateChanges_V2550(connectionString, date);
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
        //[Rahul.Gadhave][GEOS2-5898][Date:21-08-2024]
        public List<DetectionChangeDetails> GetDailyAddedChanges_V2550(DateTime date)
        {
            try
            {

                string connectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetDailyAddedChanges_V2550(connectionString, date);
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
        //[Rahul.Gadhave][GEOS2-5896][Date:29/08/2024]
        public DetectionDetails AddDetection_V2560(DetectionDetails detectionDetails)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddDetection_V2560(detectionDetails, MainServerConnectionString, Properties.Settings.Default.DetectionFiles, Properties.Settings.Default.DetectionImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[Rahul.Gadhave][GEOS2-5896][Date:05-09-2024]
        public DetectionDetails AddDetectionForAddDetectionViewModel_V2560(DetectionDetails detectionDetails)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddDetectionForAddDetectionViewModel_V2560(detectionDetails, MainServerConnectionString, Properties.Settings.Default.DetectionFiles, Properties.Settings.Default.DetectionImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[RGadhave][GEOS2-5896][25.09.2024]
        public bool IsUpdatePCM_DetectionECOSVisibility_Update_V2560(DetectionDetails detectionDetails)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.IsUpdatePCM_DetectionECOSVisibility_Update_V2560(MainServerConnectionString, detectionDetails);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<BasePriceListByItem> GetSalesPriceForPCMArticleByBPL_V2590()
        {
            try
            {

                string connectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetSalesPriceForPCMArticleByBPL_V2590(connectionString);
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

        //[rdixit][GEOS2-6522][29.11.2024]
        public List<Tuple<ulong, string>> GetArticleCostPricesPlantByCurrency_V2590()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetArticleCostPricesPlantByCurrency_V2590(connectionString);
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

        //[rdixit][GEOS2-6522][29.11.2024]
        public List<Tuple<ulong, string, uint>> GetSalesPriceNameList_V2590()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetSalesPriceNameList_V2590(connectionString);
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

        //[rdixit][GEOS2-6522][29.11.2024]
        public List<CurrencyConversion> GetCurrencyConversionsByLatestDate_V2590(string idCurrencyFrom, uint idCurrencyTo)
        {
            try
            {

                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetCurrencyConversionsByLatestDate_V2590(connectionString, idCurrencyFrom, idCurrencyTo);
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

        //[rdixit][GEOS2-6522][29.11.2024]
        public List<ArticleCostPrice> GetArticleCostPricesByCompany_V2590(string alias,uint idCountry)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetArticleCostPricesByCompany_V2590(PCMConnectionString, alias, idCountry);
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

        //[rdixit][GEOS2-6522][29.11.2024]
        public List<BasePriceListByItem> GetSalesPriceforArticleByIdBasePrice_V2590(ulong idBasePrice)
        {
            try
            {

                string connectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetSalesPriceforArticleByIdBasePrice_V2590(connectionString, idBasePrice);
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

        //[rdixit][GEOS2-6624][10.12.2024]
        public ProductTypes GetProductTypeByIdCpType_V2590(UInt64 IdCpType, UInt64 IdTemplate, int IdUser)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetProductTypeByIdCpType_V2590(PCMConnectionString, IdCpType, Properties.Settings.Default.ProductTypeFiles, Properties.Settings.Default.ProductTypeImages, IdTemplate,
                    Properties.Settings.Default.CurrenciesImages, IdUser);
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

        //[rdixit][GEOS2-6624][10.12.2024]
        public List<ConnectorFamilies> GetAllFamiliesWithSubFamily_V2590()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetAllFamiliesWithSubFamily_V2590(PCMConnectionString);
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

        //[rdixit][GEOS2-6624][10.12.2024]
        public bool UpdateProductType_V2590(ProductTypes productTypes)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.UpdateProductType_V2590(productTypes, MainServerConnectionString, Properties.Settings.Default.ProductTypeImages, Properties.Settings.Default.ProductTypeFiles, PCMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false || mgr.IsConnectionStringNameExist("PCMContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext/PCMContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][GEOS2-6624][10.12.2024]
        public ProductTypes AddProductType_V2590(ProductTypes productTypes)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.AddProductType_V2590(productTypes, MainServerConnectionString, Properties.Settings.Default.ProductTypeImages, Properties.Settings.Default.ProductTypeFiles, PCMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false || mgr.IsConnectionStringNameExist("PCMContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext/PCMContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][31.12.2024][GEOS2-6574][GEOS2-6575]
        public List<ProductTypes> GetAllProductTypes_V2590()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetAllProductTypes_V2590(PCMConnectionString, Properties.Settings.Default.ProductTypeImages);
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

        //[rdixit][31.12.2024][GEOS2-6575]
        public List<DetectionDetails> GetAllModuleDetectionsWaysOptionsSpareParts_V2590()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetAllModuleDetectionsWaysOptionsSpareParts_V2590(PCMConnectionString, Properties.Settings.Default.DetectionImages);
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
        //[nsatpute][19-05-2025][GEOS2-6691]

        public ProductTypes GetProductTypeByIdCpType_V2640(UInt64 IdCpType, UInt64 IdTemplate, int IdUser)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetProductTypeByIdCpType_V2640(PCMConnectionString, IdCpType, Properties.Settings.Default.ProductTypeFiles, Properties.Settings.Default.ProductTypeImages, IdTemplate,
                    Properties.Settings.Default.CurrenciesImages, IdUser);
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

        //[pooja.jadhav][GEOS2-6691][19-05-2025]
        public List<ProductTypes> GetAllProductTypes_V2640()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetAllProductTypes_V2640(PCMConnectionString, Properties.Settings.Default.ProductTypeImages);
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
        //[Rahul.Gadhave][GEOS2-8316][Date:27-06-2025]
        public Articles GetArticleByIdArticle_V2660(UInt32 IdArticle, UInt32 IdPCMArticle, int IdUser)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetArticleByIdArticle_V2660(PCMConnectionString, Properties.Settings.Default.ArticleVisualAidsPath, IdArticle, IdPCMArticle, Properties.Settings.Default.ArticleImages,
                    Properties.Settings.Default.CurrenciesImages, Properties.Settings.Default.CountryFilePath, IdUser);
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


        //[Rahul.Gadhave][GEOS2-8316][Date:27-06-2025]
        public Articles GetArticleByIdArticle_V2660_temp(UInt32 IdArticle, int IdUser)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetArticleByIdArticle_V2660_temp(PCMConnectionString, Properties.Settings.Default.ArticleVisualAidsPath, IdArticle, Properties.Settings.Default.ArticleImages,
                    Properties.Settings.Default.CurrenciesImages, Properties.Settings.Default.CountryFilePath, IdUser);
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

        //[pramod.misal][GEOS2-8321][Date:27-06-2025]
        public Articles GetPLMArticleByIdArticle_V2660(UInt32 IdArticle, int IdUser)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetPLMArticleByIdArticle_V2660(PCMConnectionString, Properties.Settings.Default.ArticleVisualAidsPath, IdArticle, Properties.Settings.Default.ArticleImages,
                    Properties.Settings.Default.CurrenciesImages, IdUser);
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
        public bool IsUpdatePCMArticle_V2660(uint IdPCMArticleCategory, Articles Article, bool IsDetailsChecked, bool IsPricesChecked)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.IsUpdatePCMArticle_V2660(MainServerConnectionString, IdPCMArticleCategory, Article, Properties.Settings.Default.ArticleImages, Properties.Settings.Default.ArticleAttachmentDocPath,IsDetailsChecked,IsPricesChecked);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

     
        public List<PCMArticleImage> GetLinkedArticleImage_V2660(uint IdArticle)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetLinkedArticleImage_V2660(PCMConnectionString, IdArticle);
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
    }
}
