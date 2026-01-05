using Emdep.Geos.Data.BusinessLogic;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.PCM;
using Emdep.Geos.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

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

    }
}
