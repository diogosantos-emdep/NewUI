using Emdep.Geos.Data.BusinessLogic;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Data.Common.PCM;
using Emdep.Geos.Data.Common.SCM;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.Services.Web.Workbench;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Emdep.Geos.Services.Web.Workbench
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "WorkbenchMainService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select WorkbenchMainService.svc or WorkbenchMainService.svc.cs at the Solution Explorer and start debugging.
    public class WorkbenchMainService : IWorkbenchMainService
    {
        WorkbenchMainServiceManager mgr = new WorkbenchMainServiceManager();

        public DetectionDetails AddDetection_V2550(DetectionDetails detectionDetails)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddDetection_V2550(detectionDetails, MainServerConnectionString, Properties.Settings.Default.DetectionFiles, Properties.Settings.Default.DetectionImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist_V2550("MainServerContext") == false)
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
        public bool UpdateDetection_V2550(DetectionDetails detectionDetails)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateDetection_V2550(detectionDetails, MainServerConnectionString, Properties.Settings.Default.DetectionFiles, Properties.Settings.Default.DetectionImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist_V2550("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        /// <summary>
        /// This method is used to delete product type.
        /// </summary>
        public bool IsDeletedProductType_V2550(UInt64 IdCPType)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.IsDeletedProductType_V2550(IdCPType, MainServerConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist_V2550("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public bool IsUpdatedPCMArticleCategoryOrder_V2550(List<PCMArticleCategory> pcmArticleCategoryList, uint? IdModifier)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.IsUpdatedPCMArticleCategoryOrder_V2550(MainServerConnectionString, pcmArticleCategoryList, IdModifier);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist_V2550("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool IsDeletePCMArticleCategory_V2550(List<PCMArticleCategory> pcmArticleCategoryList)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.IsDeletePCMArticleCategory_V2550(MainServerConnectionString, pcmArticleCategoryList, Properties.Settings.Default.ProductCategoriesImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist_V2550("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public bool IsUpdatePCMArticleCategoryInArticleWithStatus_V2550(uint IdPCMArticleCategory, Articles Article)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.IsUpdatePCMArticleCategoryInArticleWithStatus_V2550(MainServerConnectionString, IdPCMArticleCategory, Article, Properties.Settings.Default.ArticleImages, Properties.Settings.Default.ArticleAttachmentDocPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist_V2550("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool AddDeletePCMArticle_V2550(uint IdPCMArticleCategory, List<Articles> ArticleList, int IdUser)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddDeletePCMArticle_V2550(MainServerConnectionString, IdPCMArticleCategory, ArticleList, IdUser, Properties.Settings.Default.ArticleImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist_V2550("MainServerContext") == false)
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
        public bool UpdateDetectionForAddDetectionViewModel_V2550(DetectionDetails detectionDetails)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateDetectionForAddDetectionViewModel_V2550(detectionDetails, MainServerConnectionString, Properties.Settings.Default.DetectionFiles, Properties.Settings.Default.DetectionImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist_V2550("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public bool ImportArticleCostPriceCalculate_V2550(Data.Common.Company company, UInt64 itemArticle, List<PODetail> EWHQArticlesByArticleComponentpoDetailLst, List<ArticlesByArticle> LstAllArticlesByArticle, List<PODetail> ArticlesByArticleComponentpoDetailLst)
        {
            try
            {

                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.ImportArticleCostPriceCalculate_V2550(company, MainServerConnectionString, workbenchConnectionString, itemArticle, EWHQArticlesByArticleComponentpoDetailLst, LstAllArticlesByArticle, ArticlesByArticleComponentpoDetailLst);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist_V2550("MainServerContext") == false)
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
        public bool IsUpdatePCMArticleCategory_V2550(PCMArticleCategory pcmArticleCategory, List<PCMArticleCategory> pcmArticleCategoryList)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.IsUpdatePCMArticleCategory_V2550(MainServerConnectionString, pcmArticleCategory, pcmArticleCategoryList, Properties.Settings.Default.ProductCategoriesImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist_V2550("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public PCMArticleCategory AddPCMArticleCategory_V2550(PCMArticleCategory pcmArticleCategory, List<PCMArticleCategory> pcmArticleCategoryList)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddPCMArticleCategory_V2550(MainServerConnectionString, pcmArticleCategory, pcmArticleCategoryList, Properties.Settings.Default.ProductCategoriesImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist_V2550("MainServerContext") == false)
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
        public bool IsUpdatePCM_MicroSigaInformation_V2550(List<MicroSigainformation> microSigainformation)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.IsUpdatePCM_MicroSigaInformation_V2550(microSigainformation, MainServerConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist_V2550("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool UpdateDetection_ForAddDetectionViewModeln_V2550(DetectionDetails detectionDetails)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateDetection_ForAddDetectionViewModeln_V2550(detectionDetails, MainServerConnectionString, Properties.Settings.Default.DetectionFiles, Properties.Settings.Default.DetectionImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist_V2550("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool IsUpdatePCM_DetectionECOSVisibility_Update_V2550(DetectionDetails detectionDetails)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.IsUpdatePCM_DetectionECOSVisibility_Update_V2550(MainServerConnectionString, detectionDetails);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist_V2550("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public bool IsUpdatePCMArticle_V2550(uint IdPCMArticleCategory, Articles Article)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.IsUpdatePCMArticle_V2550(MainServerConnectionString, IdPCMArticleCategory, Article, Properties.Settings.Default.ArticleImages, Properties.Settings.Default.ArticleAttachmentDocPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist_V2550("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public DetectionDetails AddDetectionForEditDetectionViewModel_V2550(DetectionDetails detectionDetails)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddDetectionForEditDetectionViewModel_V2550(detectionDetails, MainServerConnectionString, Properties.Settings.Default.DetectionFiles, Properties.Settings.Default.DetectionImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist_V2550("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public FreePlugins AddUpdateFreePlugins_V2550(FreePlugins freePlugins)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddUpdateFreePlugins_V2550(MainServerConnectionString, freePlugins);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist_V2550("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool AddUpdateFreePluginsForAddPluginsViewModel_V2550(List<FreePlugins> freePlugins)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddUpdateFreePluginsForAddPluginsViewModel_V2550(MainServerConnectionString, freePlugins);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist_V2550("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public bool AddHardLockLicense_V2550(UInt32 IdArticle, List<HardLockPlugins> pluginList)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddHardLockLicense_V2550(MainServerConnectionString, IdArticle, pluginList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist_V2550("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public bool DeleteSupportedPluginForHardLockLicense_V2550(UInt32 idPlugin, UInt32 idArticle)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.DeleteSupportedPluginForHardLockLicense_V2550(MainServerConnectionString, idPlugin, idArticle);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist_V2550("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool AddHardLockPlugin_V2550(UInt32 idPlugin, string Name)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddHardLockPlugin_V2550(MainServerConnectionString, idPlugin, Name);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist_V2550("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }
        public bool AddDeleteArticleCategoryMapping_V2550(List<ArticleCategorieMapping> ArticleCategoryMappingList)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddDeleteArticleCategoryMapping_V2550(MainServerConnectionString, ArticleCategoryMappingList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist_V2550("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }

        public Discounts AddDiscount_V2550(Discounts Discount)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddDiscount_V2550(Discount, MainServerConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist_V2550("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public Discounts UpdateDiscount_V2550(Discounts Discount, Discounts PrevDiscount)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateDiscount_V2550(Discount, PrevDiscount, MainServerConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist_V2550("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Articles> AddWMSTOPCMArticlesByCategories_V2550(List<ArticleCategorieMapping> ArticleCategoryMappingList)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.AddWMSTOPCMArticlesByCategories_V2550(connectionString, MainServerConnectionString, ArticleCategoryMappingList);

            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public DetectionDetails AddDetectionForNewAddDetectionViewModel_V2550(DetectionDetails detectionDetails)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddDetectionForNewAddDetectionViewModel_V2550(detectionDetails, MainServerConnectionString, Properties.Settings.Default.DetectionFiles, Properties.Settings.Default.DetectionImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist_V2550("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool UpdateDetectionForEditDetectionViewModelNew_V2550(DetectionDetails detectionDetails)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateDetectionForEditDetectionViewModelNew_V2550(detectionDetails, MainServerConnectionString, Properties.Settings.Default.DetectionFiles, Properties.Settings.Default.DetectionImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist_V2550("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public bool IsDeletedDetection_V2550(UInt32 IdDetection, string detectionName)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.IsDeletedDetection_V2550(IdDetection, detectionName, MainServerConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist_V2550("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public ProductTypes AddProductType_V2550(ProductTypes productTypes)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.AddProductType_V2550(productTypes, MainServerConnectionString, Properties.Settings.Default.ProductTypeImages, Properties.Settings.Default.ProductTypeFiles, PCMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist_V2550("MainServerContext") == false || mgr.IsConnectionStringNameExist_V2550("PCMContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext/PCMContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //HRM
        public List<EmployeeAttendance> AddEmployeeImportAttendance_V2550(List<EmployeeAttendance> employeeAttendanceList)
        {
            try
            {
                string mainServerWorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddEmployeeImportAttendance_V2550(mainServerWorkbenchConnectionString, employeeAttendanceList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
    }
}
