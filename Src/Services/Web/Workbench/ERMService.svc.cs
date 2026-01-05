using Emdep.Geos.Data.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Emdep.Geos.Data.Common.ERM;
using System.Configuration;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.PCM;
using Emdep.Geos.Data.Common;
using System.IO;

namespace Emdep.Geos.Services.Web.Workbench
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ERMService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ERMService.svc or ERMService.svc.cs at the Solution Explorer and start debugging.
    public class ERMService : IERMService
    {
        ERMManager mgr = new ERMManager();
        public List<WorkOperation> GetAllWorkOperations()
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllWorkOperations(ERMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }



        //[GEOS2-3235]
        public bool IsDeletedWorkOperationList(UInt32 idWorkOperation)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.IsDeletedWorkOperationList(idWorkOperation, MainServerConnectionString);
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

        public WorkOperation GetWorkOperationByIdWorkOperation(Int32 idWorkOperation)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetWorkOperationByIdWorkOperation(idWorkOperation, MainServerConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public List<Stages> GetAllStages()
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllStages(MainServerConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public List<WorkOperation> GetparentAndOrder()
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetparentAndOrder(MainServerConnectionString);
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
        public bool UpdateWorkOperation(WorkOperation workOperation, List<WorkOperation> workOperationList)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateWorkOperation(workOperation, workOperationList, MainServerConnectionString);
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

        //[GOES2-3242]
        public List<StandardTime> GetStandardTimeList()
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetStandardTimes(ERMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[GOES2-3242]
        public bool IsDeletedStandardTimeList(UInt64 idStandardTime, uint IdModifier)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.IsDeletedStandardTimeList(idStandardTime, MainServerConnectionString, IdModifier);
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


        //[GEOS2-3240]
        // Get code
        public string GetLatestWorkOperationCode()
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetLatestWorkOperationCode(MainServerConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-3240]
        public WorkOperation AddWorkOperation(WorkOperation workOperation, List<WorkOperation> workOperationList)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddWorkOperation(MainServerConnectionString, workOperation, workOperationList);
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

        public List<WorkOperation> GetAllWorkOperations_V2200()
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllWorkOperations_V220(ERMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-3241]GetParentListByIdParentAndCode
        public List<WorkOperation> GetParentListByIdParentAndCode(Int32 IdParent, string Code)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetParentListByIdParentAndCode(MainServerConnectionString, IdParent, Code);
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

        public string GetLatestWorkOperationCodeByCode(string code)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.GetLatestWorkOperationCodeByCode(code, MainServerConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //GEOS2-3535
        public List<WorkOperationByStages> GetAllWorkOperationStages(int IdStage)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllWorkOperationStages(ERMConnectionString, IdStage);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //GEOS2-3535
        public List<Stages> GetStages()
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetStages(ERMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }



        public bool UpdateWorkOperation_V2240(WorkOperation workOperation, List<WorkOperation> workOperationList)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateWorkOperation_V2240(workOperation, workOperationList, MainServerConnectionString);
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



        //[GEOS2-3240]
        public WorkOperation AddWorkOperation_V2240(WorkOperation workOperation, List<WorkOperation> workOperationList)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddWorkOperation_V2240(MainServerConnectionString, workOperation, workOperationList);
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




        public WorkOperation GetWorkOperationByIdWorkOperation_V2240(Int32 idWorkOperation)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetWorkOperationByIdWorkOperation_V2240(idWorkOperation, MainServerConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<WorkOperation> GetAllWorkOperations_V2240()
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllWorkOperations_V2240(ERMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<WorkOperation> GetparentAndOrder_V2240()
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetparentAndOrder_V2240(MainServerConnectionString);
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

        //[GEOS2-3241]GetParentListByIdParentAndCode
        public List<WorkOperation> GetParentListByIdParentAndCode_V2240(Int32 IdParent, string Code)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetParentListByIdParentAndCode_V2240(MainServerConnectionString, IdParent, Code);
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



        public List<WorkOperationByStages> GetAllWorkOperationStages_V2240(int IdStage)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllWorkOperationStages_V2240(ERMConnectionString, IdStage);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<StandardOperationsDictionary> GetStandardOperationsDictionaryList_V2260()
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetStandardOperationsDictionaryList_V2260(ERMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool DeleteOperationFromStandardOperationsDictionary_V2260(UInt64 idStandardOperationsDictionary, uint IdModifier)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.DeleteOperationFromStandardOperationsDictionary_V2260(idStandardOperationsDictionary, MainServerConnectionString, IdModifier);
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

        public string GetLatestSODCode_V2260()
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.GetLatestSODCode_V2260(MainServerConnectionString);
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

        public StandardOperationsDictionary AddStandardOperationsDictionary_V2260(StandardOperationsDictionary sod)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddStandardOperationsDictionary_V2260(sod, MainServerConnectionString);
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


        public bool UpdateStandardOperationsDictionary_V2260(StandardOperationsDictionary sod)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateStandardOperationsDictionary_V2260(sod, MainServerConnectionString);
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

        public List<LookupValue> GetSODSupplementsCategoryName()
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetSODSupplementsCategoryName(ERMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public StandardOperationsDictionary GetStandardOperationsDictionaryDetail(UInt64 idStandardOperationsDictionary)
        {

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetStandardOperationsDictionaryDetail(connectionString, idStandardOperationsDictionary);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public StandardOperationsDictionary AddStandardOperationsDictionary_V2270(StandardOperationsDictionary sod)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddStandardOperationsDictionary_V2270(sod, MainServerConnectionString);
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

        public bool UpdateStandardOperationsDictionary_V2270(StandardOperationsDictionary sod)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateStandardOperationsDictionary_V2270(sod, MainServerConnectionString);
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

        public List<WorkOperationByStages> GetAllWorkOperationStages_V2270(int IdStage)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllWorkOperationStages_V2270(ERMConnectionString, IdStage);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public StandardOperationsDictionary GetStandardOperationsDictionaryDetail_V2280(UInt64 idStandardOperationsDictionary)
        {

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetStandardOperationsDictionaryDetail_V2280(connectionString, idStandardOperationsDictionary);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<StandardOperationsDictionaryModules> GetAllStandardOperationsDictionaryModulesById_V2280(UInt64 idStandardOperationsDictionary)
        {

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllStandardOperationsDictionaryModulesById_V2280(connectionString, idStandardOperationsDictionary);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }



        public bool IsDeletedstandard_operations_dictionary_modulesList(UInt32 idWorkOperation, UInt32 IdStandardOperationsDictionary)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.IsDeletedstandard_operations_dictionary_modulesList(idWorkOperation, IdStandardOperationsDictionary, ERMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<WorkOperationByStages> GetAllWorkOperationStages_V2280(int IdStage)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllWorkOperationStages_V2280(ERMConnectionString, IdStage);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public WorkOperation AddWorkOperation_V2280(WorkOperation workOperation, List<WorkOperation> workOperationList)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddWorkOperation_V2280(MainServerConnectionString, workOperation, workOperationList);
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

        public WorkOperation AddWorkOperation_V2320(WorkOperation workOperation, List<WorkOperation> workOperationList)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddWorkOperation_V2320(MainServerConnectionString, workOperation, workOperationList);
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


        public bool UpdateWorkOperation_V2280(WorkOperation workOperation, List<WorkOperation> workOperationList)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateWorkOperation_V2280(workOperation, workOperationList, MainServerConnectionString);
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

        public bool UpdateWorkOperation_V2320(WorkOperation workOperation, List<WorkOperation> workOperationList)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateWorkOperation_V2320(workOperation, workOperationList, MainServerConnectionString);
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

        //[001][kshinde][08/06/2022][GEOS2-3711]
        public List<WorkOperation> GetAllWorkOperations_V2280()
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllWorkOperations_V2280(ERMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<WorkOperation> GetAllWorkOperations_V2320()
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllWorkOperations_V2320(ERMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<WorkOperation> GetAllWorkOperations_V2330()
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllWorkOperations_V2330(ERMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public WorkOperation GetWorkOperationByIdWorkOperation_V2280(Int32 idWorkOperation)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetWorkOperationByIdWorkOperation_V2280(idWorkOperation, MainServerConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public WorkOperation GetWorkOperationByIdWorkOperation_V2320(Int32 idWorkOperation)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetWorkOperationByIdWorkOperation_V2320(idWorkOperation, MainServerConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool UpdateStandardOperationsDictionary_V2280(StandardOperationsDictionary sod)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateStandardOperationsDictionary_V2280(sod, MainServerConnectionString);
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
        public StandardOperationsDictionary AddStandardOperationsDictionary_V2280(StandardOperationsDictionary sod)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddStandardOperationsDictionary_V2280(sod, MainServerConnectionString);
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

        //[001][rdixit][GEOS2-3710][15/06/2022]
        public bool UpdateWorkOperationMultipleRecords_V2280(WorkOperation workOperation, uint IdModifier)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateWorkOperationMultipleRecords_V2280(workOperation, MainServerConnectionString, IdModifier);
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

        //[GEOS2-3933][Rupali Sarode][20/09/2022]
        public bool UpdateWorkOperationMultipleRecords_V2320(WorkOperation workOperation, uint IdModifier)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateWorkOperationMultipleRecords_V2320(workOperation, MainServerConnectionString, IdModifier);
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

        public List<ERMDetectionsGroups> GetAllDetectionsGroups_V2280()
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllDetectionsGroups_V2280(ERMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<ERMStructures> GetAllStructures_V2320()
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllStructures_V2320(ERMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public List<ERMDetections> GetAllDetections_V2280()
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllDetections_V2280(ERMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public StandardOperationsDictionary GetStandardOperationsDictionaryDetail_V2290(UInt64 idStandardOperationsDictionary)
        {

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetStandardOperationsDictionaryDetail_V2290(connectionString, idStandardOperationsDictionary);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool UpdateStandardOperationsDictionary_V2290(StandardOperationsDictionary sod)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateStandardOperationsDictionary_V2290(sod, MainServerConnectionString);
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
        public StandardOperationsDictionary AddStandardOperationsDictionary_V2290(StandardOperationsDictionary sod)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddStandardOperationsDictionary_V2290(sod, MainServerConnectionString);
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

        public List<ProductTypes> GetAllProductTypes_V2300()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetAllProductTypes_V2300(PCMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
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

        public bool ERM_IsWOExistInSOD(UInt32 idWorkOperation)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.ERM_IsWOExistInSOD(idWorkOperation, MainServerConnectionString);
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

        public List<WorkOperationByStages> GetAllWorkOperationStages_V2300(int IdStage)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllWorkOperationStages_V2300(ERMConnectionString, IdStage);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<WorkOperationByStages> GetAllWorkOperationStages_V2320(int IdStage)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllWorkOperationStages_V2320(ERMConnectionString, IdStage);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        // Gulab lakade 27 07 2022 GEOS2-3837


        public List<ERMOptionsGroups> GetAllOptionGroups_V2300()
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllOptionGroups_V2300(ERMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<ERMOptions> GetAllOptions_V2300()
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllOptions_V2300(ERMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public StandardOperationsDictionary GetStandardOperationsDictionaryDetail_V2300(UInt64 idStandardOperationsDictionary)
        {

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetStandardOperationsDictionaryDetail_V2300(connectionString, idStandardOperationsDictionary);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public StandardOperationsDictionary AddStandardOperationsDictionary_V2300(StandardOperationsDictionary sod)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddStandardOperationsDictionary_V2300(sod, MainServerConnectionString);
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

        public bool UpdateStandardOperationsDictionary_V2300(StandardOperationsDictionary sod)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateStandardOperationsDictionary_V2300(sod, MainServerConnectionString);
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

        public StandardOperationsDictionary GetStandardOperationsDictionaryDetail_V2301(UInt64 idStandardOperationsDictionary)
        {

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetStandardOperationsDictionaryDetail_V2301(connectionString, idStandardOperationsDictionary);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public StandardOperationsDictionary GetStandardOperationsDictionaryDetail_V2320(UInt64 idStandardOperationsDictionary)
        {

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetStandardOperationsDictionaryDetail_V2320(connectionString, idStandardOperationsDictionary);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public StandardOperationsDictionary AddStandardOperationsDictionary_V2301(StandardOperationsDictionary sod)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddStandardOperationsDictionary_V2301(sod, MainServerConnectionString);
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

        public StandardOperationsDictionary AddStandardOperationsDictionary_V2320(StandardOperationsDictionary sod)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddStandardOperationsDictionary_V2320(sod, MainServerConnectionString);
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

        public bool UpdateStandardOperationsDictionary_V2301(StandardOperationsDictionary sod)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateStandardOperationsDictionary_V2301(sod, MainServerConnectionString);
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

        public bool UpdateStandardOperationsDictionary_V2320(StandardOperationsDictionary sod)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateStandardOperationsDictionary_V2320(sod, MainServerConnectionString);
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

        //public List<WorkOperation> GetWorkOperationsByCode_V2300(string Code)
        //{
        //    try
        //    {
        //        string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
        //        return mgr.GetWorkOperationsByCode_V2300(ERMConnectionString, Code);
        //    }
        //    catch (Exception ex)
        //    {
        //        ServiceException exp = new ServiceException();
        //        exp.ErrorMessage = ex.Message;
        //        exp.ErrorDetails = ex.ToString();
        //        throw new FaultException<ServiceException>(exp, ex.ToString());
        //    }
        //}

        // GEOS2-3838 Gulab lakade 05 09 2022


        public List<ERMWaysGroups> GetAllWaysGroups_V2301()
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllWaysGroups_V2301(ERMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<ERMWays> GetAllWays_V2301()
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllWays_V2301(ERMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //GEOS2-3974
        public List<TimeTracking> GetAllTimeTracking_V2330()
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllTimeTracking_V2330(ERMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public List<TimeTracking> GetAllTimeTracking_V2340(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllTimeTracking_V2340(ERMConnectionString, CurrencyName, PlantName, GeosAppSettingList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<TimeTracking> GetAllTimeTracking_V2350(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllTimeTracking_V2350(ERMConnectionString, CurrencyName, PlantName, GeosAppSettingList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<TimeTrackingProductionStage> GetAllTimeTrackingProductioStage_V2320()
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllTimeTrackingProductioStage_V2320(ERMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<TimeTrackingProductionStage> GetAllTimeTrackingProductioStage_V2340()
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllTimeTrackingProductioStage_V2340(ERMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //GEOS2-3839 Gulab Lakade Spare part
        public StandardOperationsDictionary GetStandardOperationsDictionaryDetail_V2340(UInt64 idStandardOperationsDictionary)
        {

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetStandardOperationsDictionaryDetail_V2340(connectionString, idStandardOperationsDictionary);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public StandardOperationsDictionary AddStandardOperationsDictionary_V2340(StandardOperationsDictionary sod)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddStandardOperationsDictionary_V2340(sod, MainServerConnectionString);
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
        public bool UpdateStandardOperationsDictionary_V2340(StandardOperationsDictionary sod)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateStandardOperationsDictionary_V2340(sod, MainServerConnectionString);
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
        public List<ERMSparePartsGroups> GetAllSparePartGroups_V2340()
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllSparePartGroups_V2340(ERMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<ERMSpareParts> GetAllSparePart_V2340()
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllSparePart_V2340(ERMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<TimeTracking> GetTimeTrackingBYPlant_V2340(UInt32 PlantID, string OriginPlant, string Currency, List<GeosAppSetting> GeosAppSettingList)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetTimeTrackingBYPlant_V2340(PlantID, OriginPlant, Currency, ERMConnectionString, GeosAppSettingList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<TimeTracking> GetTimeTrackingBYPlant_V2350(UInt32 PlantID, string OriginPlant, string Currency, List<GeosAppSetting> GeosAppSettingList)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetTimeTrackingBYPlant_V2350(PlantID, OriginPlant, Currency, ERMConnectionString, GeosAppSettingList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        // [GEOS2-3840][sdeshpande][03-01-2023] Get List for Work Stages
        public List<WorkStages> GetAllWorkStages_V2350()
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllWorkStages_V2350(ERMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        // [GEOS2-3841][sdeshpande][06-01-2023] Get List for Work Stages By IDstage
        public WorkStages GetWorkStagesByIdStages_V2350(UInt64 IdStage)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetWorkStagesByIdStages_V2350(ERMConnectionString, IdStage);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool UpdateWorkStage_V2350(WorkStages workStages)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateWorkStage_V2350(MainServerConnectionString, workStages);
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
        public WorkStages AddWorkStage_V2350(WorkStages workStages)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddWorkStage_V2350(MainServerConnectionString, workStages);
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

        //[rdixit][10.01.2022][GEOS2-4121]
        public string GetLatestWorkOperationCodeByCode_V2350(string code)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetLatestWorkOperationCodeByCode_V2350(code, ERMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][10.01.2022][GEOS2-4121]
        public string GetLatestSODCode_V2350()
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetLatestSODCode_V2350(ERMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][10.01.2022][GEOS2-4121]
        public bool ERM_IsWOExistInSOD_V2350(UInt32 idWorkOperation)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.ERM_IsWOExistInSOD_V2350(idWorkOperation, ERMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[GEOS2-3841][Rupali Sarode][10-01-2023]
        public List<WorkStages> GetAllWorkStageSequence_V2350()
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.GetAllWorkStageSequence_V2350(MainServerConnectionString);
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

        //[GEOS2-4150] [Gulab Lakade] [30 01 2023]
        public TimeTrackingWithSites GetAllTimeTracking_V2360(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                //[GEOS2-4150][Rupali Sarode][03-02-2023]
                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllTimeTracking_V2360(ERMConnectionString, crmConnection, CurrencyName, PlantName, GeosAppSettingList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[GEOS2-4150] [Gulab Lakade] [30 01 2023]
        public List<TimeTracking> GetTimeTrackingBYPlant_V2360(UInt32 PlantID, string OriginPlant, string Currency, List<GeosAppSetting> GeosAppSettingList)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                //[GEOS2-4150][Rupali Sarode][03-02-2023]
                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetTimeTrackingBYPlant_V2360(crmConnection, PlantID, OriginPlant, Currency, ERMConnectionString, GeosAppSettingList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[GEOS2-4153] [Gulab Lakade] [02 02 2023]
        public List<ProductionPlanningReview> GetProductionPlanningReview_V2360(string OriginPlant, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetProductionPlanningReview_V2360(ERMConnectionString, OriginPlant, fromDate, toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-4159] [pallavi jadhav] [02 14 2023]
        public List<Holidays> GetCompanyHolidaysBySelectedIdCompany(string OriginPlant, string year)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetCompanyHolidaysBySelectedIdCompany(workbenchConnectionString, OriginPlant, year);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public bool AddUpdatePlanningDateReview(List<PlanningDateReview> PlanningDateReviewList)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                //string ERMConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddUpdatePlanningDateReview(ERMConnectionString, PlanningDateReviewList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[GEOS2-4217] [Pallavi Jadhav] [24 02 2023]
        public TimeTrackingWithSites GetAllTimeTrackings_V2360(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                //[GEOS2-4150][Rupali Sarode][03-02-2023]
                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllTimeTrackings_V2360(ERMConnectionString, crmConnection, CurrencyName, PlantName, GeosAppSettingList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-4145][Pallavi Jadhav][03-03-2023]
        public TimeTrackingWithSites GetAllTimeTrackings_V2370(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllTimeTrackings_V2370(ERMConnectionString, crmConnection, CurrencyName, PlantName, GeosAppSettingList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-4145][Pallavi Jadhav][03-03-2023]
        public List<TimeTracking> GetTimeTrackingBYPlant_V2370(UInt32 PlantID, string OriginPlant, string Currency, List<GeosAppSetting> GeosAppSettingList)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetTimeTrackingBYPlant_V2370(crmConnection, PlantID, OriginPlant, Currency, ERMConnectionString, GeosAppSettingList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[GEOS2-4212][gulab lakade][07 03 2023]
        public List<TimeTracking> GetPlantModuleProductionDelay_V2370(List<Company> AllPlant, Company company, DateTime Date)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetPlantModuleProductionDelay_V2370(ERMConnectionString, AllPlant, company, Date);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-4163] [Pallavi Jadhav] [07 03 2023]
        public List<ProductionPlanningReview> GetProductionPlanningReview_V2370(string OriginPlant, DateTime fromDate, DateTime toDate, List<GeosAppSetting> GeosAppSettingList)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetProductionPlanningReview_V2370(ERMConnectionString, OriginPlant, fromDate, toDate, GeosAppSettingList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-4143] [Pallavi Jadhav] [15 03 2023]
        public bool AddUpdatePlanningDateReview_V2370(ProductionPlanningReview PlanningDateReviewList)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddUpdatePlanningDateReview_V2370(ERMConnectionString, PlanningDateReviewList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-4212][gulab lakade][07 03 2023]
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


        //[Rupali Sarode][GEOS2-4241][16-03-2023]
        public List<WeeklyProductionReportMail> GetWeeklyProductionReportMailIDs_V2370(UInt32 IdCompany)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetWeeklyProductionReportMailIDs_V2370(ERMConnectionString, IdCompany);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-4244] [Gulab lakade] [21 03 2023]
        public List<WeeklyProductionReport> GetAllWeeklyproductionReportByPlant_V2370(Company company, DateTime StartDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllWeeklyproductionReportByPlant_V2370(ERMConnectionString, company, StartDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method to get authorized plant related to id user
        /// </summary>
        /// <param name="idUser">Get id user</param>
        /// <returns>List of companies</returns>
        public List<Company> GetAuthorizedPlantsByIdUser_V2031(Int32 idUser)
        {
            List<Company> companies = new List<Company>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                companies = mgr.GetAuthorizedPlantsByIdUser_V2031(WorkbenchConnectionString, idUser);
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

        //[GOES2-4242][Rupali Sarode][03-04-2023]
        public List<ModulesEquivalencyWeight> GetAllProductTypesEquivalencyWeight_V2380()
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllProductTypesEquivalencyWeight_V2380(ERMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[GEOS2-4212][gulab lakade][07 03 2023]
        public List<PlantProductionDelay> GetEquipmentproductionDelay_V2370(Company company, DateTime Date)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEquipmentproductionDelay_V2370(ERMConnectionString, company, Date);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[Pallavi Jadhav] [GEOS2-4329] [06-04-2023]
        public List<ModulesEquivalencyWeight> GetAllStructuresEquivalencyWeight_V2380()
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllStructuresEquivalencyWeight_V2380(ERMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GOES2-4330][Rupali Sarode][06-04-2023]
        public ModulesEquivalencyWeight GetProductTypesEquivalencyWeightByCPType_V2380(Int32 IdCpType)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetProductTypesEquivalencyWeightByCPType_V2380(ERMConnectionString, IdCpType);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GOES2-4330][Rupali Sarode][12-04-2023]
        public bool SaveEquivalencyWeight_V2380(ModulesEquivalencyWeight modulesEquivalencyWeight)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.SaveEquivalencyWeight_V2380(ERMConnectionString, modulesEquivalencyWeight);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-4330][Rupali Sarode][14-04-2023]
        public ModulesEquivalencyWeight GetStructuresEquivalencyWeightByCPType_V2380(Int32 IdCpType)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetStructuresEquivalencyWeightByCPType_V2380(ERMConnectionString, IdCpType);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Origin and Production][gulab lakade][17 04 2023]
        public TimeTrackingWithSites GetAllTimeTrackings_V2380(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllTimeTrackings_V2380(ERMConnectionString, crmConnection, CurrencyName, PlantName, GeosAppSettingList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Origin and Production][gulab lakade][17 04 2023]
        public List<TimeTracking> GetTimeTrackingBYPlant_V2380(UInt32 PlantID, string OriginPlant, string Currency, List<GeosAppSetting> GeosAppSettingList)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetTimeTrackingBYPlant_V2380(crmConnection, PlantID, OriginPlant, Currency, ERMConnectionString, GeosAppSettingList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[gulab lakade][17 04 2023]
        public List<TimeTracking> GetPlantModuleProductionDelay_V2380(List<Company> AllPlant, Company company, DateTime Date)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetPlantModuleProductionDelay_V2380(ERMConnectionString, AllPlant, company, Date);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[gulab lakade][GEOS2-4241][158-04-2023]
        public List<WeeklyProductionReportMail> GetWeeklyProductionReportMailIDsByAppSettingTemp_V2380(UInt32 IdCompany)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetWeeklyProductionReportMailIDsByAppSettingTemp_V2380(ERMConnectionString, IdCompany);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[GEOS2-4336][gulab lakade][18 04 2023]
        public List<PlantOperationProductionStage> GetAllPlantOperationProductioStage_V2380(string IdStage)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllPlantOperationProductioStage_V2380(ERMConnectionString, IdStage);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-4336][gulab lakade][21 04 2023]
        public List<ERMPlantOperationalPlanning> GetPlantOperationPlanning_V2380(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDeescription)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetPlantOperationPlanning_V2380(ERMConnectionString, IdCompany, StartDate, EndDate, IdJobDeescription);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-4355][Rupali Sarode][3-05-2023]
        public List<ProductTypes> GetAllProductTypes_V2390()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetAllProductTypes_V2390(PCMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
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

        //[GEOS2-4355][Rupali Sarode][3-05-2023]
        public List<ERMDetections> GetAllDetections_V2390()
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllDetections_V2390(ERMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-4355][Rupali Sarode][3-05-2023]
        public List<ERMWays> GetAllWays_V2390()
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllWays_V2390(ERMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-4355][Rupali Sarode][3-05-2023]
        public List<ERMSpareParts> GetAllSparePart_V2390()
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllSparePart_V2390(ERMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-4355][Rupali Sarode][3-05-2023]
        public List<ERMStructures> GetAllStructures_V2390()
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllStructures_V2390(ERMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-4355][Rupali Sarode][3-05-2023]
        public List<ERMOptions> GetAllOptions_V2390()
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllOptions_V2390(ERMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[missmatch QTY for time tracking and planning Date review] [gulab lakade] [03 025 2023]
        public List<ProductionPlanningReview> GetProductionPlanningReview_V2380(string OriginPlant, DateTime fromDate, DateTime toDate, List<GeosAppSetting> GeosAppSettingList)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetProductionPlanningReview_V2380(ERMConnectionString, OriginPlant, fromDate, toDate, GeosAppSettingList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-4425] [Pallavi Jadhav] [04 05 2023]
        public List<WeeklyProductionReport> GetAllWeeklyproductionReportByPlant_V2390(Company company, DateTime StartDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllWeeklyproductionReportByPlant_V2390(ERMConnectionString, company, StartDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Rupali Sarode][GEOS2-4347][05-05-2023]
        public List<TimeTrackingProductionStage> GetAllTimeTrackingProductioStage_V2390()
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllTimeTrackingProductioStage_V2390(ERMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[GEOS2-4338][gulab lakade][15 05 2023]
        public List<ERMPlantOperationalPlanning> GetPlantOperationPlanning_V2390(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDeescription)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetPlantOperationPlanning_V2390(ERMConnectionString, IdCompany, StartDate, EndDate, IdJobDeescription);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-4341][Pallavi Jadhav][05 05 2023]
        public List<ERMDeliveryVisualManagement> GetDVManagementProduction_V2390(string IdSite, string CurrencyName, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetDVManagementProduction_V2390(ERMConnectionString, IdSite, CurrencyName, fromDate, toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        ////[GEOS2-4475][Gulab Lakade][17-05-2023][ERM - Holiday]
        public List<WindowsServicesHolidays> GetAllWindowsServicesHolidays_V2390(Company company, DateTime StartDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllWindowsServicesHolidays_V2390(ERMConnectionString, company, StartDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[gulab lakade][24 05 2023]
        public List<TimeTracking> GetPlantModuleProductionDelay_V2390(List<Company> AllPlant, Company company, DateTime Date)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetPlantModuleProductionDelay_V2390(ERMConnectionString, AllPlant, company, Date);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[GEOS2-4491][Pallavi Jadhav][24 05 2023]
        public List<PlantProductionDelay> GetEquipmentproductionDelay_V2390(Company company, DateTime Date)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEquipmentproductionDelay_V2390(ERMConnectionString, company, Date);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[gulab lakade][GEOS2-4494-batch][26 05 2023]
        public TimeTrackingWithSites GetAllTimeTrackings_V2400(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllTimeTrackings_V2400(ERMConnectionString, crmConnection, CurrencyName, PlantName, GeosAppSettingList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[gulab lakade][GEOS2-4494-batch][26 05 2023]
        public List<TimeTracking> GetTimeTrackingBYPlant_V2400(UInt32 PlantID, string OriginPlant, string Currency, List<GeosAppSetting> GeosAppSettingList)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetTimeTrackingBYPlant_V2400(crmConnection, PlantID, OriginPlant, Currency, ERMConnectionString, GeosAppSettingList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[gulab lakade][GEOS2-4494-batch][26 05 2023]

        //[pallavi jadhav] [GEOS2-4481] [26 05 2023] 
        public List<ProductionPlanningReview> GetProductionPlanningReview_V2400(string OriginPlant, DateTime fromDate, DateTime toDate, List<GeosAppSetting> GeosAppSettingList)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetProductionPlanningReview_V2400(ERMConnectionString, crmConnection, OriginPlant, fromDate, toDate, GeosAppSettingList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }


        }

        //[GEOS2-4343][Pallavi Jadhav][25 05 2023]
        public List<DeliveryVisualManagementStages> GetDVManagementProductionStage_V2400()
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetDVManagementProductionStage_V2400(ERMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[GEOS2-4481]Pallavi Jadhav][29 05 2023]
        public List<PlanningDateReviewStages> GetProductionPlanningReviewStage_V2400()
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetProductionPlanningReviewStage_V2400(ERMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-4483][gulab lakade][31 05 2023]
        public List<WeeklyProductionReport> GetAllWeeklyproductionReportByPlant_V2400(Company company, DateTime StartDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllWeeklyproductionReportByPlant_V2400(ERMConnectionString, company, StartDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //// [Rupali Sarode][GEOS2-4517][05-06-2023]
        //public List<ERMFollowingTwelveWeeksPlan> GetFollowingTwelveWeeksPlan_V2400(string IdSites)
        //{
        //    try
        //    {
        //        string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
        //        return mgr.GetFollowingTwelveWeeksPlan_V2400(ERMConnectionString, IdSites);
        //    }
        //    catch (Exception ex)
        //    {
        //        ServiceException exp = new ServiceException();
        //        exp.ErrorMessage = ex.Message;
        //        exp.ErrorDetails = ex.ToString();
        //        throw new FaultException<ServiceException>(exp, ex.ToString());
        //    }
        //}

        //[GEOS2-4553][Rupali Sarode][12-06-2023]
        public List<ERMPlantOperationalPlanning> GetPlantOperationPlanning_V2400(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDeescription)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetPlantOperationPlanning_V2400(ERMConnectionString, IdCompany, StartDate, EndDate, IdJobDeescription);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        // [Rupali Sarode][GEOS2-4553][09-06-2023]
        public List<ERMNonOTItemType> GetAllNonOTTimeType_V2400()
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllNonOTTimeType_V2400(ERMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[gulab lakade][][14 06 2023]
        public List<WeeklyProductionReportMail> GetPlantProductionDelayReportMailIDsByAppSettingTemp_V2400(UInt32 IdCompany)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetPlantProductionDelayReportMailIDsByAppSettingTemp_V2400(ERMConnectionString, IdCompany);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-4590][rupali sarode][22-06-2023]
        public List<WeeklyReworksReportSummary> GetPlantWeeklyReworksSummary_V2410(Company company, DateTime StartDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetPlantWeeklyReworksSummary_V2410(ERMConnectionString, company, StartDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-4590][Rupali Sarode][26-06-2023]
        public List<WeeklyReworksReportMail> GetWeeklyReworksReportMailIDs_V2410(UInt32 IdCompany)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetWeeklyReworksReportMailIDs_V2410(ERMConnectionString, IdCompany);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-4590][rupali sarode][26-06-2023]
        public List<WeeklyReworksReportMail> GetPlantReworksReportMailIDsByAppSetting_V2410(UInt32 IdCompany)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetPlantReworksReportMailIDsByAppSetting_V2410(ERMConnectionString, IdCompany);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[gulab lakade][GEOS2-4605][26 06 2023]
        public List<WeeklyProductionReport> GetAllWeeklyproductionReportByPlant_V2410(Company company, DateTime StartDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllWeeklyproductionReportByPlant_V2410(ERMConnectionString, company, StartDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-4619][rupali sarode][28-06-2023]
        public List<Site> GetPlants_V2410(int IdUser)
        {
            try
            {

                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetPlants_V2410(connectionString, IdUser);
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
        //[GEOS2-4548][pallavi][23-06-2023]
        public List<WeeklyProductionReportMail> GetWeeklyProductionReportMailIDs_V2410(UInt32 IdCompany, string TO_CC_JobDescription, string TO_JobDescription, string CC_JobDescription)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetWeeklyProductionReportMailIDs_V2410(ERMConnectionString, IdCompany, TO_CC_JobDescription, TO_JobDescription, CC_JobDescription);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[GEOS2-4548][pallavi][23-06-2023]
        public List<WeeklyProductionReportMail> GetWeeklyProductionReportMailIDsByAppSettingTemp_V2410(UInt32 IdCompany, string CC_EmployeeId)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetWeeklyProductionReportMailIDsByAppSettingTemp_V2410(ERMConnectionString, IdCompany, CC_EmployeeId);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[GEOS2-4606][gulab lakade][30 06 2023]
        public List<ERMDeliveryVisualManagement> GetDVManagementProduction_V2410(string IdSite, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetDVManagementProduction_V2410(ERMConnectionString, IdSite, fromDate, toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }



        //[pallavi jadhav] [GEOS2-4481] [26 05 2023] 
        public List<ProductionPlanningReview> GetProductionPlanningReview_V2410(string OriginPlant, DateTime fromDate, DateTime toDate, List<GeosAppSetting> GeosAppSettingList)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetProductionPlanningReview_V2410(ERMConnectionString, crmConnection, OriginPlant, fromDate, toDate, GeosAppSettingList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }


        }

        //[GEOS2-4624][Rupali Sarode][03-07-2023]
        //public string GetDVMFolderPath()
        //{
        //    try
        //    {
        //        string FolderPath = Properties.Settings.Default.DeliveryVisualManagement;
        //        return FolderPath;
        //    }
        //    catch (Exception ex)
        //    {
        //        ServiceException exp = new ServiceException();
        //        exp.ErrorMessage = ex.Message;
        //        exp.ErrorDetails = ex.ToString();
        //        throw new FaultException<ServiceException>(exp, ex.ToString());
        //    }
        //}





        //[GEOS2-4626][Pallavi Jadhav][03 07 2023]
        //public string SaveReportOfTimeTracking(string TimeTrackingReport)
        //{
        //    try
        //    {
        //        string filePath = Properties.Settings.Default.TimeTrackingReport + "\\" + TimeTrackingReport;
        //        if (!Directory.Exists(Convert.ToString(Properties.Settings.Default.TimeTrackingReport)))
        //        {
        //            Directory.CreateDirectory(Convert.ToString(Properties.Settings.Default.TimeTrackingReport));
        //        }
        //        return filePath;
        //    }
        //    catch (Exception ex)
        //    {
        //        ServiceException exp = new ServiceException();
        //        exp.ErrorMessage = ex.Message;
        //        exp.ErrorDetails = ex.ToString();
        //        throw new FaultException<ServiceException>(exp, ex.ToString());
        //    }
        //}
        //[GEOS2-4617][gulab lakade][04-07-2023]
        public List<ERMPlantOperationalPlanning> GetPlantOperationPlanning_V2410(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDeescription)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetPlantOperationPlanning_V2410(ERMConnectionString, IdCompany, StartDate, EndDate, IdJobDeescription);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[GEOS2-4617][gulab lakade][04-07-2023]
        public List<ERMNonOTItemType> GetAllNonOTTimeType_V2410()
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllNonOTTimeType_V2410(ERMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<TimeTracking> GetDeliveryDateForTimeTrackingReport_V2410(Int32 IdSite)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetDeliveryDateForTimeTrackingReport_V2410(ERMConnectionString, IdSite);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }


        }

        //public List<PlantWeeklyReworksMail> GetPlantWeeklyReworksMail_V2410()
        //{
        //    try
        //    {
        //        string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
        //        return mgr.GetPlantWeeklyReworksMail_V2410(ERMConnectionString);
        //    }
        //    catch (Exception ex)
        //    {
        //        ServiceException exp = new ServiceException();
        //        exp.ErrorMessage = ex.Message;
        //        exp.ErrorDetails = ex.ToString();
        //        throw new FaultException<ServiceException>(exp, ex.ToString());
        //    }
        //}

        //[GEOS2-4626][pallavi jadhav][03 07 2023]
        public TimeTrackingWithSites GetAllTimeTrackingRepor_V2410(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllTimeTrackingRepor_V2410(ERMConnectionString, crmConnection, CurrencyName, PlantName, GeosAppSettingList, fromDate, toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<TimeTracking> GetTimeTrackingReportBYPlant_V2410(UInt32 PlantID, string OriginPlant, string Currency, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetTimeTrackingReportBYPlant_V2410(crmConnection, PlantID, OriginPlant, Currency, ERMConnectionString, GeosAppSettingList, fromDate, toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-4639][pallavi jadhav][25-07-2023]
        public List<ERMNonOTItemType> GetAllNonOTTimeType_V2420()
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllNonOTTimeType_V2420(ERMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-4707][rupali sarode][25-07-2023]
        public List<ERMPlantOperationalPlanning> GetPlantOperationPlanning_V2420(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDeescription)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetPlantOperationPlanning_V2420(ERMConnectionString, IdCompany, StartDate, EndDate, IdJobDeescription);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rupali sarode][28/07/2023]
        public List<ProductionPlanningReview> GetProductionPlanningReview_V2420(string OriginPlant, DateTime fromDate, DateTime toDate, List<GeosAppSetting> GeosAppSettingList)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetProductionPlanningReview_V2420(ERMConnectionString, crmConnection, OriginPlant, fromDate, toDate, GeosAppSettingList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }


        }


        //[pallavi jadhav]16 08 2023]
        public TimeTrackingWithSites GetAllTimeTrackings_V2420(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllTimeTrackings_V2420(ERMConnectionString, crmConnection, CurrencyName, PlantName, GeosAppSettingList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pallavi jadhav]16 08 2023]
        public List<TimeTracking> GetTimeTrackingBYPlant_V2420(UInt32 PlantID, string OriginPlant, string Currency, List<GeosAppSetting> GeosAppSettingList)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetTimeTrackingBYPlant_V2420(crmConnection, PlantID, OriginPlant, Currency, ERMConnectionString, GeosAppSettingList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[gulab lakade][GEOS2-4767][22 08 2023]
        public List<WeeklyProductionReport> GetAllWeeklyproductionReportByPlant_V2420(Company company, DateTime StartDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllWeeklyproductionReportByPlant_V2420(ERMConnectionString, company, StartDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-4730][rupali sarode][08-08-2023]
        public List<ERMPlantOperationalPlanning> GetAllRTM_HRResources_V2420(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDeescription)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllRTM_HRResources_V2420(ERMConnectionString, IdCompany, StartDate, EndDate, IdJobDeescription);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-4730][rupali sarode][18-08-2023]
        public List<RTMHRResourcesExpectedTime> GetRTM_HRResourcesExpectedTime_V2420(DateTime StartDate, DateTime EndDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetRTM_HRResourcesExpectedTime_V2420(ERMConnectionString, crmConnection, StartDate, EndDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Pallavi Jadhav][GEOS2-4591][22 06 2023]
        public List<PlantWeeklyReworksMailStage> GetPlantWeeklyReworksMailStage_V2430(Company company)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetPlantWeeklyReworksMailStage_V2430(ERMConnectionString, company);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public List<PlantWeeklyReworksMail> GetPlantWeeklyReworksMail_V2430(Company company, DateTime StartDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetPlantWeeklyReworksMail_V2430(ERMConnectionString, company, StartDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-4626][pallavi jadhav][31 08 2023]
        public TimeTrackingWithSites GetAllTimeTrackingRepor_V2430(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllTimeTrackingRepor_V2430(ERMConnectionString, crmConnection, CurrencyName, PlantName, GeosAppSettingList, fromDate, toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<TimeTracking> GetTimeTrackingReportBYPlant_V2430(UInt32 PlantID, string OriginPlant, string Currency, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetTimeTrackingReportBYPlant_V2430(crmConnection, PlantID, OriginPlant, Currency, ERMConnectionString, GeosAppSettingList, fromDate, toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[GEOS2-4752][gulab lakade][01 09 2023]
        public List<WeeklyProductionEmployeeTime> GetWeeklyProductionEmployeeTime_V2430(Company company, DateTime StartDate, DateTime EndDate, string IdJobDeescription, List<ERM_IDStage_JobDescription> IDStage_JobDescription)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetWeeklyProductionEmployeeTime_V2430(ERMConnectionString, company, StartDate, EndDate, IdJobDeescription, IDStage_JobDescription);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public TimeTrackingWithSites GetPlantModuleProductionDelay_V2430(List<Company> AllPlant, Company company, DateTime Date)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetPlantModuleProductionDelay_V2430(ERMConnectionString, AllPlant, company, Date);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<TimeTracking> GetPlantModuleProductionDelayByPlant_V2430(List<Company> AllPlant, UInt32 IdSite, DateTime Date, UInt32 OriginalPlantIdSite)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetPlantModuleProductionDelayByPlant_V2430(ERMConnectionString, AllPlant, IdSite, Date, OriginalPlantIdSite);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public TimeTrackingWithSites GetALLPlantWeeklyReworksMail_V2430(Company company, DateTime Date)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetALLPlantWeeklyReworksMail_V2430(ERMConnectionString, company, Date);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<TimeTracking> GetPlantWeeklyReworksMailByPlant_V2430(UInt32 IdSite, DateTime Date, List<Company> allPlants)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetPlantWeeklyReworksMailByPlant_V2430(ERMConnectionString, IdSite, Date, allPlants);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-4799][rupali sarode][05-09-2023]
        public List<RTMFutureLoad> GetRTMFutureLoadDetails_V2430(RTMFutureLoadParams FutureLoadParams)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                return mgr.GetRTMFutureLoadDetails_V2430(FutureLoadParams, ERMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-4813][Aishwarya Ingale][11-09-2023]
        public List<ERMPlantOperationalPlanning> GetPlantOperationPlanning_V2430(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDeescription)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetPlantOperationPlanning_V2430(ERMConnectionString, IdCompany, StartDate, EndDate, IdJobDeescription);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<WeeklyReworksReportSummary> GetPlantWeeklyReworksSummary_V2430(Company company, DateTime StartDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetPlantWeeklyReworksSummary_V2430(ERMConnectionString, company, StartDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        // [Pallavi Jadhav][14-09-2023][GEOS2-4818]
        public TimeTrackingWithSites GetAllTimeTrackings_V2430(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllTimeTrackings_V2430(ERMConnectionString, crmConnection, CurrencyName, PlantName, GeosAppSettingList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        // [Pallavi Jadhav][14-09-2023][GEOS2-4818]
        public List<TimeTracking> GetTimeTrackingBYPlant_V2430(UInt32 PlantID, string OriginPlant, string Currency, List<GeosAppSetting> GeosAppSettingList)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetTimeTrackingBYPlant_V2430(crmConnection, PlantID, OriginPlant, Currency, ERMConnectionString, GeosAppSettingList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-4821][Rupali Sarode][14-09-2023]
        public List<DeliveryVisualManagementStages> GetDVManagementProductionStage_V2440()
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetDVManagementProductionStage_V2440(ERMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-4821][Rupali Sarode][14-09-2023]
        public List<ERMDeliveryVisualManagement> GetDVManagementProduction_V2440(string IdSite, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetDVManagementProduction_V2440(ERMConnectionString, IdSite, fromDate, toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-4820][Pallavi Jadhav][13-09-2023]
        public TimeTrackingWithSites GetAllReworkReport_V2440(DateTime fromDate, DateTime toDate, UInt32 IdSite)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllReworkReport_V2440(ERMConnectionString, fromDate, toDate, IdSite);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[pallavi jadhav][GEOS2-4869][9 27 2023]
        public List<DeliveryVisualManagementStages> GetDVManagementRTMHRResourcesStage_V2440(Int32 IdSite)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetDVManagementRTMHRResourcesStage_V2440(ERMConnectionString, IdSite);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-4800][Pallavi Jadhav][02-10-2023]
        public List<RTMFutureLoad> GetRTMFutureLoadDetails_V2440(RTMFutureLoadParams FutureLoadParams)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                return mgr.GetRTMFutureLoadDetails_V2440(FutureLoadParams, ERMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-4862][Rupali Sarode][04-10-2023]
        public List<RTMHRResourcesExpectedTime> GetRTM_HRResourcesExpectedTime_V2440(DateTime StartDate, DateTime EndDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetRTM_HRResourcesExpectedTime_V2440(ERMConnectionString, crmConnection, StartDate, EndDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[GEOS2-4908][pallavi Jadhav][25-10-2023]
        public List<PlantDeliveryAnalysis> GetPlantDeliveryAnalysis_V2450(string CurrencyNameFromSetting, string CurrencySymbolFromSetting, string IdSite, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                // string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetPlantDeliveryAnalysis_V2450(ERMConnectionString, CurrencyNameFromSetting, CurrencySymbolFromSetting, IdSite, fromDate, toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[GEOS2-4996][gulab lakade][27 10 2023]
        public List<WeeklyProductionEmployeeTime> GetWeeklyProductionEmployeeTime_V2450(Company company, DateTime StartDate, DateTime EndDate, string IdJobDeescription, List<ERM_IDStage_JobDescription> IDStage_JobDescription)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetWeeklyProductionEmployeeTime_V2450(ERMConnectionString, company, StartDate, EndDate, IdJobDeescription, IDStage_JobDescription);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-5001][rupali sarode][28-10-2023]
        public List<ERMPlantOperationalPlanning> GetAllRTM_HRResources_V2450(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDeescription)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllRTM_HRResources_V2450(ERMConnectionString, IdCompany, StartDate, EndDate, IdJobDeescription);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-4909][Aishwarya Ingale][27-10-2023]
        public List<ERMProductionTime> GetRTMTestBoardsInProduction_V2450(uint IdSite, DateTime fromDate, DateTime toDate, List<GeosAppSetting> GeosAppSettingList)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetRTMTestBoardsInProduction_V2450(ERMConnectionString, IdSite, fromDate, toDate, GeosAppSettingList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[gulab lakade][GEOS2-4921][30 10 2023]
        public List<TimeTracking> GetProductionOutputReworksMail_V2450(Company company, DateTime StartDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetProductionOutputReworksMail_V2450(ERMConnectionString, company, StartDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[GEOS2-5002] [gulab lakade][31 10 2023]
        public TimeTrackingWithSites GetALLPlantWeeklyReworksMail_V2450(Company company, DateTime Date)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetALLPlantWeeklyReworksMail_V2450(ERMConnectionString, company, Date);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-4867][gulab lakade][19 10 2023]
        public List<ERM_ProductionTimeline> GetProductionTimeline_V2450(Int32 IdCompany, DateTime StartDate, DateTime EndDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetProductionTimeline_V2450(ERMConnectionString, IdCompany, StartDate, EndDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        // [Pallavi Jadhav][24-11-2023][GEOS2-5053]
        public TimeTrackingWithSites GetAllTimeTrackings_V2460(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllTimeTrackings_V2460(ERMConnectionString, crmConnection, CurrencyName, PlantName, GeosAppSettingList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        // [Pallavi Jadhav][14-09-2023][GEOS2-4818]
        public List<TimeTracking> GetTimeTrackingBYPlant_V2460(UInt32 PlantID, UInt32 ProductionIdSite, string Currency, List<GeosAppSetting> GeosAppSettingList)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetTimeTrackingBYPlant_V2460(crmConnection, PlantID, ProductionIdSite, Currency, ERMConnectionString, GeosAppSettingList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        // [Pallavi Jadhav][24-11-2023][GEOS2-5053]
        public TimeTrackingWithSites GetAllTimeTrackingReport_V2460(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllTimeTrackingReport_V2460(ERMConnectionString, crmConnection, CurrencyName, PlantName, GeosAppSettingList, fromDate, toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        // [Pallavi Jadhav][24-11-2023][GEOS2-5053]
        public List<TimeTracking> GetTimeTrackingReportBYPlant_V2460(UInt32 PlantID, UInt32 ProductionIdSite, string Currency, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetTimeTrackingReportBYPlant_V2460(crmConnection, PlantID, ProductionIdSite, Currency, ERMConnectionString, GeosAppSettingList, fromDate, toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        // [Pallavi Jadhav][24-11-2023][GEOS2-5053]
        public TimeTrackingWithSites GetAllReworkReport_V2460(DateTime fromDate, DateTime toDate, UInt32 IdSite)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllReworkReport_V2460(ERMConnectionString, fromDate, toDate, IdSite);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        // [Pallavi Jadhav][24-11-2023][GEOS2-5053]
        public TimeTrackingWithSites GetALLPlantWeeklyReworksMail_V2460(Company company, DateTime Date)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetALLPlantWeeklyReworksMail_V2460(ERMConnectionString, company, Date);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        // [Pallavi Jadhav][24-11-2023][GEOS2-5053]
        public List<PlantDeliveryAnalysis> GetPlantDeliveryAnalysis_V2460(string CurrencyNameFromSetting, string CurrencySymbolFromSetting, string IdSite, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                // string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetPlantDeliveryAnalysis_V2460(ERMConnectionString, CurrencyNameFromSetting, CurrencySymbolFromSetting, IdSite, fromDate, toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }



        //[GEOS2-5006][Rupali Sarode][27-11-2023]
        public List<ProductTypes> GetAllProductTypes_V2460()
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                // string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetAllProductTypes_V2460(ERMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-5006][Rupali Sarode][27-11-2023]
        public List<ERMDetectionsGroups> GetAllDetectionsGroups_V2460()
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllDetectionsGroups_V2460(ERMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-5006][Rupali Sarode][27-11-2023]
        public List<ERMDetections> GetAllDetections_V2460()
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllDetections_V2460(ERMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-5006][Rupali Sarode][27-11-2023]
        public List<ERMOptionsGroups> GetAllOptionGroups_V2460()
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllOptionGroups_V2460(ERMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-5006][Rupali Sarode][27-11-2023]
        public List<ERMOptions> GetAllOptions_V2460()
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllOptions_V2460(ERMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-5006][Rupali Sarode][27-11-2023]
        public List<ERMWays> GetAllWays_V2460()
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllWays_V2460(ERMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-5006][Rupali Sarode][27-11-2023]
        public List<ERMSparePartsGroups> GetAllSparePartGroups_V2460()
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllSparePartGroups_V2460(ERMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-5006][Rupali Sarode][27-11-2023]
        public List<ERMSpareParts> GetAllSparePart_V2460()
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllSparePart_V2460(ERMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-5097][gulab lakade][12 04 2023]
        public List<ProductionPlanningReview> GetProductionPlanningReview_V2460(string OriginPlant, DateTime fromDate, DateTime toDate, List<GeosAppSetting> GeosAppSettingList)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetProductionPlanningReview_V2460(ERMConnectionString, crmConnection, OriginPlant, fromDate, toDate, GeosAppSettingList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }


        }
        //[gulab lakade][miss matchunit prize][05 12 2023]
        public List<WeeklyProductionReport> GetAllWeeklyproductionReportByPlant_V2460(Company company, DateTime StartDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllWeeklyproductionReportByPlant_V2460(ERMConnectionString, company, StartDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[GEOS2-5028][gulab lakade][19 10 2023]
        public List<ERM_ProductionTimeline> GetProductionTimeline_V2460(Int32 IdCompany, DateTime StartDate, DateTime EndDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetProductionTimeline_V2460(ERMConnectionString, IdCompany, StartDate, EndDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-5135][Rupali Sarode][19-12-2023]
        public StandardOperationsDictionary GetStandardOperationsDictionaryDetail_V2460(UInt64 idStandardOperationsDictionary)
        {

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetStandardOperationsDictionaryDetail_V2460(connectionString, idStandardOperationsDictionary);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[GEOS2-5127][gulab lakade][20 12 2023]
        public ERM_ReworkReport GetALLPlantWeeklyReworksMail_V2470(Company company, DateTime Date)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetALLPlantWeeklyReworksMail_V2470(ERMConnectionString, company, Date);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public ERM_ReworkReport GetProductionOutputReworksMail_V2470(Company company, DateTime StartDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetProductionOutputReworksMail_V2470(ERMConnectionString, company, StartDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        // [Pallavi Jadhav][19-12-2023][GEOS2-5035]
        public List<PlantLoadAnalysis> GetAllPlantLoadAnalysis_V2470(Int32 idSite, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                // string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllPlantLoadAnalysis_V2470(ERMConnectionString, idSite, FromDate, ToDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[pallavi jadhav] [GEOS2-5197] [02 01 2024]
        public List<ERM_ProductionTimeline> GetProductionTimeline_V2470(Int32 IdCompany, DateTime StartDate, DateTime EndDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetProductionTimeline_V2470(ERMConnectionString, IdCompany, StartDate, EndDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[wrong week][gulab lakade][09 01 2024]
        public List<WeeklyProductionReport> GetAllWeeklyproductionReportByPlant_V2470(Company company, DateTime StartDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllWeeklyproductionReportByPlant_V2470(ERMConnectionString, company, StartDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public List<WeeklyProductionEmployeeTime> GetWeeklyProductionEmployeeTime_V2470(Company company, DateTime StartDate, DateTime EndDate, string IdJobDeescription, List<ERM_IDStage_JobDescription> IDStage_JobDescription)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetWeeklyProductionEmployeeTime_V2470(ERMConnectionString, company, StartDate, EndDate, IdJobDeescription, IDStage_JobDescription);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public List<WindowsServicesHolidays> GetAllWindowsServicesHolidays_V2470(Company company, DateTime StartDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllWindowsServicesHolidays_V2470(ERMConnectionString, company, StartDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[pallavi jadhav] [GEOS2-5197] [02 01 2024]
        public List<ProductionTimeReportLegend> GetAllProductionTimeReportLegend_V2480()
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllProductionTimeReportLegend_V2480(ERMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<ERM_ProductionTimeline> GetProductionTimeline_V2480(Int32 IdCompany, DateTime StartDate, DateTime EndDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetProductionTimeline_V2480(ERMConnectionString, IdCompany, StartDate, EndDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        // [GEOS2-5037][Rupali Sarode][31-01-2023]
        public List<PlantLoadAnalysis> GetAllPlantLoadAnalysis_V2480(Int32 idSite, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                // string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllPlantLoadAnalysis_V2480(ERMConnectionString, idSite, FromDate, ToDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        #region Aishwarya Ingale
        public List<DeliveryTimeDistribution> GetDeliveryTimeDistributionList_V2480()
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetDeliveryTimeDistributionList_V2480(ERMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public bool DeleteOperationFromDeliveryTimeDistribution_V2480(UInt64 iddeliverytimedistribution, uint IdModifier)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.DeleteOperationFromDeliveryTimeDistribution_V2480(iddeliverytimedistribution, MainServerConnectionString, IdModifier);
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



        #endregion

        //start [GEOS2-5324][gulab lakade][09 02 2023]
        public ERM_ReworkReport GetALLPlantWeeklyReworksMail_V2480(Company company, DateTime Date)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetALLPlantWeeklyReworksMail_V2480(ERMConnectionString, company, Date);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public ERM_ReworkReport GetProductionOutputReworksMail_V2480(Company company, DateTime StartDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetProductionOutputReworksMail_V2480(ERMConnectionString, company, StartDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //end [GEOS2-5324][gulab lakade][09 02 2023]

        //[Rupali Sarode][07-02-2024][GEOS2-5353]
        public List<Template> GetAllTemplates_DTD_V2490()
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetAllTemplates_DTD_V2490(PCMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
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

        //[Rupali Sarode][07-02-2024][GEOS2-5353]
        public List<ProductTypes> GetAllProductTypes_DTD_V2490(string IdCptypes)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                // string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetAllProductTypes_DTD_V2490(ERMConnectionString, IdCptypes);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Rupali Sarode][13-02-2024][GEOS2-5271]
        public string GetLatestDTDCode_V2490()
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetLatestDTDCode_V2490(ERMConnectionString);
            }

            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Rupali Sarode][14-02-2024][GEOS2-5271]
        public DeliveryTimeDistribution GetDeliveryTimeDistributionDetail_V2490(UInt64 idDeliveryTimeDistribution)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetDeliveryTimeDistributionDetail_V2490(ERMConnectionString, idDeliveryTimeDistribution);
            }

            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public DeliveryTimeDistribution AddDeliveryTimeDistribution_V2490(DeliveryTimeDistribution DTD)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddDeliveryTimeDistribution_V2490(DTD, MainServerConnectionString);
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
        public bool UpdateDeliveryTimeDistribution_V2490(DeliveryTimeDistribution DTD)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateDeliveryTimeDistribution_V2490(DTD, MainServerConnectionString);
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
        //[GEOS2-5418] [gulab lakade] [23 02 2024]
        public List<ProductionTimeReportLegend> GetAllProductionTimeReportLegend_V2490()
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllProductionTimeReportLegend_V2490(ERMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<ERM_ProductionTimeline> GetProductionTimeline_V2490(Int32 IdCompany, DateTime StartDate, DateTime EndDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetProductionTimeline_V2490(ERMConnectionString, IdCompany, StartDate, EndDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //  [gulab lakade][11 03 2024][GEOS2-5466]
        public TimeTrackingWithSites GetAllTimeTrackings_V2500(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllTimeTrackings_V2500(ERMConnectionString, crmConnection, CurrencyName, PlantName, GeosAppSettingList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //  [gulab lakade][11 03 2024][GEOS2-5466]
        public List<TimeTracking> GetTimeTrackingBYPlant_V2500(UInt32 PlantID, UInt32 ProductionIdSite, string Currency, List<GeosAppSetting> GeosAppSettingList)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetTimeTrackingBYPlant_V2500(crmConnection, PlantID, ProductionIdSite, Currency, ERMConnectionString, GeosAppSettingList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //start [GEOS2-5324][gulab lakade][14 03 2024]
        public ERM_ReworkReport GetALLPlantWeeklyReworksMail_V2500(Company company, DateTime Date)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetALLPlantWeeklyReworksMail_V2500(ERMConnectionString, company, Date);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Aishwarya Ingale][18 03 2024][GEOS2-5424]
        public List<ERM_ProductionTimeline> GetProductionTimeline_V2500(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDeescription)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetProductionTimeline_V2500(ERMConnectionString, IdCompany, StartDate, EndDate, IdJobDeescription);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        // [gulab lakade][11 03 2024][GEOS2-5466]
        public TimeTrackingWithSites GetAllTimeTrackingReport_V2500(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllTimeTrackingReport_V2500(ERMConnectionString, crmConnection, CurrencyName, PlantName, GeosAppSettingList, fromDate, toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        // [gulab lakade][11 03 2024][GEOS2-5466]
        public List<TimeTracking> GetTimeTrackingReportBYPlant_V2500(UInt32 PlantID, UInt32 ProductionIdSite, string Currency, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetTimeTrackingReportBYPlant_V2500(crmConnection, PlantID, ProductionIdSite, Currency, ERMConnectionString, GeosAppSettingList, fromDate, toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-5420][Rupali Sarode][15-03-2024]
        public ERM_ReworkReport GetProductionOutputReworksMail_V2500(Company company, DateTime StartDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetProductionOutputReworksMail_V2500(ERMConnectionString, company, StartDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-5420][Rupali Sarode][15-03-2024]
        //public ReworksMailStageByOTItemAndIDDrawing GetPlantWeeklyReworksMailStage_V2500(Company company)
        //{
        //    try
        //    {
        //        string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
        //        return mgr.GetPlantWeeklyReworksMailStage_V2500(ERMConnectionString, company);
        //    }
        //    catch (Exception ex)
        //    {
        //        ServiceException exp = new ServiceException();
        //        exp.ErrorMessage = ex.Message;
        //        exp.ErrorDetails = ex.ToString();
        //        throw new FaultException<ServiceException>(exp, ex.ToString());
        //    }
        //}

        // [Rupali Sarode][GEOS2-5522][21-03-2024]
        //public TimeTrackingStageByOTItemAndIDDrawing GetAllTimeTrackingProductionStage_V2500()
        //{
        //    try
        //    {
        //        string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
        //        return mgr.GetAllTimeTrackingProductionStage_V2500(ERMConnectionString);
        //    }
        //    catch (Exception ex)
        //    {
        //        ServiceException exp = new ServiceException();
        //        exp.ErrorMessage = ex.Message;
        //        exp.ErrorDetails = ex.ToString();
        //        throw new FaultException<ServiceException>(exp, ex.ToString());
        //    }
        //}

        // [Rupali Sarode][GEOS2-5523][26-03-2024]
        public TimeTrackingWithSites GetAllReworkReport_V2500(DateTime fromDate, DateTime toDate, UInt32 IdSite)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllReworkReport_V2500(ERMConnectionString, fromDate, toDate, IdSite);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        // [Rupali Sarode][GEOS2-5521][27-03-2024]
        public List<WeeklyProductionReport> GetAllWeeklyproductionReportByPlant_V2500(Company company, DateTime StartDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllWeeklyproductionReportByPlant_V2500(ERMConnectionString, company, StartDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        // [Rupali Sarode][GEOS2-5521][28-03-2024]
        public StageByOTItemAndIDDrawing GetAllStagesPerIDOTItemAndIDDrawing_V2500()
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllStagesPerIDOTItemAndIDDrawing_V2500(ERMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-5546][pallavi jadhav][29 03 2024]
        public List<ERMPlantOperationalPlanning> GetAllRTM_HRResources_V2500(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDeescription)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllRTM_HRResources_V2500(ERMConnectionString, IdCompany, StartDate, EndDate, IdJobDeescription);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public List<ERMPlantOperationalPlanning> GetPlantOperationPlanning_V2500(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDeescription)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetPlantOperationPlanning_V2500(ERMConnectionString, IdCompany, StartDate, EndDate, IdJobDeescription);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[GEOS2-5558][gulab lakade]
        public List<ERM_ProductionTimeline> GetProductionTimeline_V2510(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDescription)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetProductionTimeline_V2510(ERMConnectionString, IdCompany, StartDate, EndDate, IdJobDescription);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-5520][Aishwarya Ingale]
        public List<WeeklyProductionReport> GetAllWeeklyproductionReportByPlant_V2510(Company company, DateTime StartDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllWeeklyproductionReportByPlant_V2510(ERMConnectionString, company, StartDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public ERM_MonthlyProductionTimeline GetMothlyProductionTimeLine_V2510(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDescription)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetMothlyProductionTimeLine_V2510(ERMConnectionString, IdCompany, StartDate, EndDate, IdJobDescription);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public TimeTrackingWithSites GetAllTimeTrackingProductionTimeline_V2510(string PlantName, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllTimeTrackingProductionTimeline_V2510(ERMConnectionString, crmConnection, PlantName, fromDate, toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<TimeTracking> GetAllTimeTrackingProductionTimelineByPlant_V2510(UInt32 PlantID, UInt32 ProductionIdSite, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllTimeTrackingProductionTimelineByPlant_V2510(crmConnection, PlantID, ProductionIdSite, ERMConnectionString, fromDate, toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<PlantLoadAnalysis> GetAllPlantLoadAnalysis_V2520(Int32 idSite, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                // string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllPlantLoadAnalysis_V2520(ERMConnectionString, idSite, FromDate, ToDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[GEOS2-5750][gulab lakade][21052024]
        public ERM_WorkOrder_Other_ProductionTimeline GetWorkorder_Other_MonthlyProduction_V2520()
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;

                return mgr.GetWorkorder_Other_MonthlyProduction_V2520(ERMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-5742][pallavi jadhav]
        public List<ERM_ProductionTimeline> GetProductionTimeline_V2520(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDescription)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetProductionTimeline_V2520(ERMConnectionString, IdCompany, StartDate, EndDate, IdJobDescription);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[GEOS2-5849][gulab lakade][13 06 2024]
        public List<ERMPlantORG_EmployeeDetails> GetWeeklyProd_TimeLine_Plant_ORG_V2530(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDescription)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetWeeklyProd_TimeLine_Plant_ORG_V2530(ERMConnectionString, IdCompany, StartDate, EndDate, IdJobDescription);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[GEOS2-5742][pallavi jadhav]
        public List<ERM_ProductionTimeline> GetProductionTimeline_V2530(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDescription)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetProductionTimeline_V2530(ERMConnectionString, IdCompany, StartDate, EndDate, IdJobDescription);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        #region Aishwarya Ingale [Geos2-5629][18/06/2024]
        //[GEOS2-5629][Aishwarya Ingale][18-06-2024]
        public StandardOperationsDictionary GetStandardOperationsDictionaryDetail_V2530(UInt64 idStandardOperationsDictionary)
        {

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetStandardOperationsDictionaryDetail_V2530(connectionString, idStandardOperationsDictionary);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool UpdateStandardOperationsDictionary_V2530(StandardOperationsDictionary sod)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateStandardOperationsDictionary_V2530(sod, MainServerConnectionString);
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

        public StandardOperationsDictionary AddStandardOperationsDictionary_V2530(StandardOperationsDictionary sod)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddStandardOperationsDictionary_V2530(sod, MainServerConnectionString);
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
        #endregion

        //[rgadhave][GEOS2-5583][20-06-2024] 
        public DeliveryTimeDistribution GetDeliveryTimeDistributionDetail_V2530(UInt64 idDeliveryTimeDistribution)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetDeliveryTimeDistributionDetail_V2530(ERMConnectionString, idDeliveryTimeDistribution);
            }

            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public DeliveryTimeDistribution AddDeliveryTimeDistribution_V2530(DeliveryTimeDistribution DTD)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddDeliveryTimeDistribution_V2530(DTD, MainServerConnectionString);
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

        public bool UpdateDeliveryTimeDistribution_V2530(DeliveryTimeDistribution DTD)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateDeliveryTimeDistribution_V2530(DTD, MainServerConnectionString);
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

        //[GEOS2-5856][gulab lakade][20 06 2024]
        public List<WeeklyProductionEmployeeTime> GetWeeklyProductionEmployeeTime_V2530(Company company, DateTime StartDate, DateTime EndDate, string IdJobDeescription, List<ERM_IDStage_JobDescription> IDStage_JobDescription)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetWeeklyProductionEmployeeTime_V2530(ERMConnectionString, company, StartDate, EndDate, IdJobDeescription, IDStage_JobDescription);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[GEOS2-5856][gulab lakade][25 06 2024]
        public ERM_MonthlyProductionTimeline GetMonthlyProductionTimeLine_V2530(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDescription)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetMonthlyProductionTimeLine_V2530(ERMConnectionString, IdCompany, StartDate, EndDate, IdJobDescription);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<ProductTypes> GetAllProductTypes_DTD_V2530(string IdCptypes)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                // string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetAllProductTypes_DTD_V2530(ERMConnectionString, IdCptypes);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        #region Aishwarya [Geos2-5629]
        public StandardOperationsDictionary AddStandardOperationsDictionary_V2540(StandardOperationsDictionary sod)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddStandardOperationsDictionary_V2540(sod, MainServerConnectionString);
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


        public bool UpdateStandardOperationsDictionary_V2540(StandardOperationsDictionary sod)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateStandardOperationsDictionary_V2540(sod, MainServerConnectionString);
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

        public StandardOperationsDictionary GetStandardOperationsDictionaryDetail_V2540(UInt64 idStandardOperationsDictionary)
        {

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetStandardOperationsDictionaryDetail_V2540(connectionString, idStandardOperationsDictionary);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        #endregion

        //Aishwarya Ingale[Geos2-5853]
        public List<ProductionTimeReportLegend> GetAllProductionTimeReportLegend_V2540()
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllProductionTimeReportLegend_V2540(ERMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //Aishwarya Ingale[Geos2-5853]
        public List<ERM_ProductionTimeline> GetProductionTimeline_V2540(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDescription)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetProductionTimeline_V2540(ERMConnectionString, IdCompany, StartDate, EndDate, IdJobDescription);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pallavi jadhav] [11 07 2024][GEOS2-5901]
        public ERM_MonthlyProductionTimeline GetMonthlyProductionTimeLine_V2540(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDescription)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetMonthlyProductionTimeLine_V2540(ERMConnectionString, IdCompany, StartDate, EndDate, IdJobDescription);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pallavi jadhav] [15 07 2024][GEOS2-5917]
        public List<PlantDeliveryAnalysis> GetPlantDeliveryAnalysis_V2540(string CurrencyNameFromSetting, string CurrencySymbolFromSetting, string IdSite, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                // string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetPlantDeliveryAnalysis_V2540(ERMConnectionString, CurrencyNameFromSetting, CurrencySymbolFromSetting, IdSite, fromDate, toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        #region [pallavi jadhav][GEOS2-5907][17 07 2024]
        public List<RTMFutureLoad> GetRTMFutureLoadDetails_V2540(RTMFutureLoadParams FutureLoadParams)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                return mgr.GetRTMFutureLoadDetails_V2540(FutureLoadParams, ERMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public List<ERMPlantOperationalPlanning> GetAllRTM_HRResources_V2540(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDeescription)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllRTM_HRResources_V2540(ERMConnectionString, IdCompany, StartDate, EndDate, IdJobDeescription);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<RTMHRResourcesExpectedTime> GetRTM_HRResourcesExpectedTime_V2540(DateTime StartDate, DateTime EndDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetRTM_HRResourcesExpectedTime_V2540(ERMConnectionString, crmConnection, StartDate, EndDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<ERMProductionTime> GetRTMTestBoardsInProduction_V2540(uint IdSite, DateTime fromDate, DateTime toDate, List<GeosAppSetting> GeosAppSettingList)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetRTMTestBoardsInProduction_V2540(ERMConnectionString, IdSite, fromDate, toDate, GeosAppSettingList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public TimeTrackingWithSites GetAllTimeTrackings_V2540(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllTimeTrackings_V2540(ERMConnectionString, crmConnection, CurrencyName, PlantName, GeosAppSettingList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<TimeTracking> GetTimeTrackingBYPlant_V2540(UInt32 PlantID, UInt32 ProductionIdSite, string Currency, List<GeosAppSetting> GeosAppSettingList)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetTimeTrackingBYPlant_V2540(crmConnection, PlantID, ProductionIdSite, Currency, ERMConnectionString, GeosAppSettingList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<ProductionPlanningReview> GetProductionPlanningReview_V2540(string OriginPlant, DateTime fromDate, DateTime toDate, List<GeosAppSetting> GeosAppSettingList)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetProductionPlanningReview_V2540(ERMConnectionString, crmConnection, OriginPlant, fromDate, toDate, GeosAppSettingList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }


        }

        public List<ERMPlantOperationalPlanning> GetPlantOperationPlanning_V2540(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDeescription)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetPlantOperationPlanning_V2540(ERMConnectionString, IdCompany, StartDate, EndDate, IdJobDeescription);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<ERMDeliveryVisualManagement> GetDVManagementProduction_V2540(string IdSite, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetDVManagementProduction_V2540(ERMConnectionString, IdSite, fromDate, toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public TimeTrackingWithSites GetAllTimeTrackingReport_V2540(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllTimeTrackingReport_V2540(ERMConnectionString, crmConnection, CurrencyName, PlantName, GeosAppSettingList, fromDate, toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<TimeTracking> GetTimeTrackingReportBYPlant_V2540(UInt32 PlantID, UInt32 ProductionIdSite, string Currency, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetTimeTrackingReportBYPlant_V2540(crmConnection, PlantID, ProductionIdSite, Currency, ERMConnectionString, GeosAppSettingList, fromDate, toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public TimeTrackingWithSites GetAllReworkReport_V2540(DateTime fromDate, DateTime toDate, UInt32 IdSite)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllReworkReport_V2540(ERMConnectionString, fromDate, toDate, IdSite);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public List<TimeTracking> GetPlantModuleProductionDelayByPlant_V2540(List<Company> AllPlant, UInt32 IdSite, DateTime Date, UInt32 OriginalPlantIdSite)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetPlantModuleProductionDelayByPlant_V2540(ERMConnectionString, AllPlant, IdSite, Date, OriginalPlantIdSite);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public TimeTrackingWithSites GetAllTimeTrackingProductionTimeline_V2540(string PlantName, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllTimeTrackingProductionTimeline_V2540(ERMConnectionString, crmConnection, PlantName, fromDate, toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool IsActiveERMPlant()
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.IsActiveERMPlant(ERMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-5760][pallavi jadhav] [19 08 2024]
        public ERM_ReworkReport GetProductionOutputReworksMail_V2550(Company company, DateTime StartDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetProductionOutputReworksMail_V2550(ERMConnectionString, company, StartDate);
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

        #region Aishwarya[Geos2-6034]
        //Aishwarya Ingale[Geos2-6034]
        public ERM_ReworkReport GetALLPlantWeeklyReworksMail_V2550(Company company, DateTime Date)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetALLPlantWeeklyReworksMail_V2550(ERMConnectionString, company, Date);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public TimeTrackingWithSites GetAllReworkReport_V2550(DateTime fromDate, DateTime toDate, UInt32 IdSite)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllReworkReport_V2550(ERMConnectionString, fromDate, toDate, IdSite);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public TimeTrackingWithSites GetAllTimeTrackingReport_V2550(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllTimeTrackingReport_V2550(ERMConnectionString, crmConnection, CurrencyName, PlantName, GeosAppSettingList, fromDate, toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public TimeTrackingWithSites GetAllTimeTrackings_V2550(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllTimeTrackings_V2550(ERMConnectionString, crmConnection, CurrencyName, PlantName, GeosAppSettingList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<TimeTracking> GetTimeTrackingBYPlant_V2550(UInt32 PlantID, UInt32 ProductionIdSite, string Currency, List<GeosAppSetting> GeosAppSettingList)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetTimeTrackingBYPlant_V2550(crmConnection, PlantID, ProductionIdSite, Currency, ERMConnectionString, GeosAppSettingList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<TimeTracking> GetTimeTrackingReportBYPlant_V2550(UInt32 PlantID, UInt32 ProductionIdSite, string Currency, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetTimeTrackingReportBYPlant_V2550(crmConnection, PlantID, ProductionIdSite, Currency, ERMConnectionString, GeosAppSettingList, fromDate, toDate);
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

        //[GEOS2-6040][gulab lakade] [13 08 2024]
        public ERM_MonthlyProductionTimeline GetMonthlyProductionTimeLine_V2550(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDescription)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetMonthlyProductionTimeLine_V2550(ERMConnectionString, IdCompany, StartDate, EndDate, IdJobDescription);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[GEOS2-6038][gulab lakade] [16 08 2024]
        public TimeTrackingWithSites GetAllTimeTrackingProductionTimeline_V2550(string PlantName, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllTimeTrackingProductionTimeline_V2550(ERMConnectionString, crmConnection, PlantName, fromDate, toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-6069][pallavi jadhav][21 08 2024]   
        public List<ERM_ProductionTimeline> GetProductionTimeline_V2550(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDescription)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetProductionTimeline_V2550(ERMConnectionString, IdCompany, StartDate, EndDate, IdJobDescription);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //rajashri GEOS2-5988[22-08-2024]
        public List<TimeTracking> GetMisMatchDataOFIddrawing_V2550(string iddrawing, string serialnumber, Int32 Idsite)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                // string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetMisMatchDataOFIddrawing_V2550(ERMConnectionString, iddrawing, serialnumber, Idsite);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-6058][gulab lakade][27 08 2024]
        public ERM_WeeklyProd_EmployeeTime GetWeeklyProductionEmployeeTime_V2560(Company company, DateTime StartDate, DateTime EndDate, string IdJobDeescription, List<ERM_IDStage_JobDescription> IDStage_JobDescription)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetWeeklyProductionEmployeeTime_V2560(ERMConnectionString, company, StartDate, EndDate, IdJobDeescription, IDStage_JobDescription);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[GEOS2-5520][Aishwarya Ingale]
        public List<WeeklyProductionReport> GetAllWeeklyproductionReportByPlant_V2560(Company company, DateTime StartDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllWeeklyproductionReportByPlant_V2560(ERMConnectionString, company, StartDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //Aishwarya Ingale[Geos2-6431]
        public List<PlantDeliveryAnalysis> GetPlantDeliveryAnalysis_V2560(string CurrencyNameFromSetting, string CurrencySymbolFromSetting, string IdSite, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                // string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetPlantDeliveryAnalysis_V2560(ERMConnectionString, CurrencyNameFromSetting, CurrencySymbolFromSetting, IdSite, fromDate, toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        #region [pallavi jadhav][GEOS2-6081][16 09 2024]
        public TimeTrackingWithSites GetAllTimeTrackings_V2560(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllTimeTrackings_V2560(ERMConnectionString, crmConnection, CurrencyName, PlantName, GeosAppSettingList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public List<TimeTracking> GetTimeTrackingBYPlant_V2560(UInt32 PlantID, UInt32 ProductionIdSite, string Currency, List<GeosAppSetting> GeosAppSettingList)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetTimeTrackingBYPlant_V2560(crmConnection, PlantID, ProductionIdSite, Currency, ERMConnectionString, GeosAppSettingList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public List<StandardOperationsDictionaryModules> GetAllStandardOperationsDictionaryModulesById_V2560(UInt64 IdStandardOperationsDictionary, UInt64 IdSite, UInt64 IdCPType, UInt64 IdCP)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllStandardOperationsDictionaryModulesById_V2560(ERMConnectionString, IdStandardOperationsDictionary, IdSite, IdCPType, IdCP);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public List<StandardOperationsDictionaryOption> GetStandardOperationDictionaryOptionById_V2560(UInt64 IdCP)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetStandardOperationDictionaryOptionById_V2560(ERMConnectionString, IdCP);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public List<StandardOperationsDictionaryDetection> GetStandardOperationDictionaryDetectionById_V2560(UInt64 IdCP)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetStandardOperationDictionaryDetectionById_V2560(ERMConnectionString, IdCP);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public List<StandardOperationsDictionaryWays> GetStandardOperationDictionaryWayById_V2560(UInt64 IdCP)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetStandardOperationDictionaryWayById_V2560(ERMConnectionString, IdCP);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<StandardOperationsDictionaryModules> GetAllStandardOperationsDictionaryModulesById_V2570(UInt64 IdStandardOperationsDictionary, string IdStage, string IdCPType, string IdCP)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllStandardOperationsDictionaryModulesById_V2570(ERMConnectionString, IdStandardOperationsDictionary, IdStage, IdCPType, IdCP);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public List<StandardOperationsDictionaryOption> GetStandardOperationDictionaryOptionById_V2570(string IdCP)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetStandardOperationDictionaryOptionById_V2570(ERMConnectionString, IdCP);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public List<StandardOperationsDictionaryDetection> GetStandardOperationDictionaryDetectionById_V2570(string IdCP)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetStandardOperationDictionaryDetectionById_V2570(ERMConnectionString, IdCP);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public List<StandardOperationsDictionaryWays> GetStandardOperationDictionaryWayById_V2570(string IdCP)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetStandardOperationDictionaryWayById_V2570(ERMConnectionString, IdCP);
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

        //start[GEOS2-6058][gulab lakade][24 09 2024]
        public ERM_MonthlyProductionTimeline GetWeeklyProductionEmployeeTime_V2570(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDeescription)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetWeeklyProductionEmployeeTime_V2570(ERMConnectionString, IdCompany, StartDate, EndDate, IdJobDeescription);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public ERM_WeeklyProduction GetGetWeeklyInProductionData_V2570(Company company, DateTime StartDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetGetWeeklyInProductionData_V2570(ERMConnectionString, company, StartDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //end[GEOS2-6058][gulab lakade][24 09 2024]

        public bool AddUpdatePlanningDeliveryDateGridByStage_V2580(UInt32 IDCompany, ProductionPlanningReview PlanningDateReviewList)
        {
            try
            {
                // string ERMConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.AddUpdatePlanningDeliveryDateGridByStage_V2580(ERMConnectionString, IDCompany, PlanningDateReviewList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<ProductionPlanningReview> GetProductionPlanningReview_V2580(string OriginPlant, DateTime fromDate, DateTime toDate, List<GeosAppSetting> GeosAppSettingList)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetProductionPlanningReview_V2580(ERMConnectionString, crmConnection, OriginPlant, fromDate, toDate, GeosAppSettingList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }


        }

        public bool AddUpdatePlanningDateReviewByStage_V2580(UInt32 IDCompany, List<PlanningDateReview> PlanningDateReviewList)
        {
            try
            {
                // string ERMConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.AddUpdatePlanningDateReviewByStage_V2580(ERMConnectionString, IDCompany, PlanningDateReviewList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-6529][gulab lakade] [22 10 2024]
        public ERM_MonthlyProductionTimeline GetMonthlyProductionTimeLine_V2580(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDescription)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetMonthlyProductionTimeLine_V2580(ERMConnectionString, IdCompany, StartDate, EndDate, IdJobDescription);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public ERM_MonthlyProductionTimeline GetWeeklyProductionEmployeeTime_V2580(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDeescription)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetWeeklyProductionEmployeeTime_V2580(ERMConnectionString, IdCompany, StartDate, EndDate, IdJobDeescription);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public TimeTrackingWithSites GetAllTimeTrackings_V2580(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllTimeTrackings_V2580(ERMConnectionString, crmConnection, CurrencyName, PlantName, GeosAppSettingList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<TimeTracking> GetTimeTrackingBYPlant_V2580(UInt32 PlantID, UInt32 ProductionIdSite, string Currency, List<GeosAppSetting> GeosAppSettingList)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetTimeTrackingBYPlant_V2580(crmConnection, PlantID, ProductionIdSite, Currency, ERMConnectionString, GeosAppSettingList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<ERM_ProductionTimeline> GetProductionTimeline_V2580(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDescription)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetProductionTimeline_V2580(ERMConnectionString, IdCompany, StartDate, EndDate, IdJobDescription);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        #region [pallavi jadhav][GEOS2-5465][06 11 2024]

        public TimeTrackingWithSites GetAllTimeTrackingReport_V2580(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllTimeTrackingReport_V2580(ERMConnectionString, crmConnection, CurrencyName, PlantName, GeosAppSettingList, fromDate, toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<TimeTracking> GetTimeTrackingReportBYPlant_V2580(UInt32 PlantID, UInt32 ProductionIdSite, string Currency, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetTimeTrackingReportBYPlant_V2580(crmConnection, PlantID, ProductionIdSite, Currency, ERMConnectionString, GeosAppSettingList, fromDate, toDate);
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

        //Aishwarya Ingale[Geos2-6611]
        public List<ERM_GlobalComparisonTimes> GetGlobalComparisonTimesResults_V2590(DateTime StartDate, DateTime EndDate, int RecordLimit, string idsite, string Region, string Stage, string Template, string CPType, string DesignSystem, string Type, string DSAStatus, string numofways, string numofDetection, string numofOption, string ways, string Detection, string Option)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetGlobalComparisonTimesResults_V2590(ERMConnectionString, StartDate, EndDate, RecordLimit, idsite, Region, Stage, Template, CPType, DesignSystem, Type, DSAStatus, numofways, numofDetection, numofOption, ways, Detection, Option);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-5319][gulab lakade][18 11 2024]
        public bool AddUpdatePlanningDateReview_V2580(List<PlanningDateReview> PlanningDateReviewList)
        {
            try
            {
                //string ERMConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.AddUpdatePlanningDateReview_V2580(ERMConnectionString, PlanningDateReviewList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public bool AddUpdatePlanningDateGrid_V2580(ProductionPlanningReview PlanningDateReviewList)
        {
            try
            {
                //string ERMConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.AddUpdatePlanningDateGrid_V2580(ERMConnectionString, PlanningDateReviewList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<ProductionPlanningReview> GetProductionPlanningReviewByStage_V2580(string OriginPlant, DateTime fromDate, DateTime toDate, List<GeosAppSetting> GeosAppSettingList)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetProductionPlanningReviewByStage_V2580(ERMConnectionString, crmConnection, OriginPlant, fromDate, toDate, GeosAppSettingList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }


        }
        //rajashri GEOS2-6713
        public List<GlobalTop> GetAllGlobalTopValues_V2590()
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllGlobalTopValues_V2590(ERMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-6554][gulab lakade][28 11 2024]
        public List<ERM_ProductionTimeline> GetProductionTimeline_V2590(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDescription)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetProductionTimeline_V2590(ERMConnectionString, IdCompany, StartDate, EndDate, IdJobDescription);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        #region [GEOS2-6646][Daivshala Vighne] [10-12-2024]
        public TimeTrackingWithSites GetAllTimeTrackings_V2590(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;

                // return mgr.GetAllTimeTrackings_V2590(ERMConnectionString, crmConnection, CurrencyName, PlantName, GeosAppSettingList);
                #region [GEOS2-6646][Daivshala Vighne] [10-12-2024]
                return mgr.GetAllTimeTrackings_V2590(ERMConnectionString, crmConnection, CurrencyName, PlantName, GeosAppSettingList);
                #endregion
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public List<TimeTracking> GetTimeTrackingBYPlant_V2590(UInt32 PlantID, UInt32 ProductionIdSite, string Currency, List<GeosAppSetting> GeosAppSettingList)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                //  return mgr.GetTimeTrackingBYPlant_V2580(crmConnection, PlantID, ProductionIdSite, Currency, ERMConnectionString, GeosAppSettingList);
                //[GEOS2-6646][Daivshala Vighne] [10-12-2024]
                return mgr.GetTimeTrackingBYPlant_V2590(crmConnection, PlantID, ProductionIdSite, Currency, ERMConnectionString, GeosAppSettingList);

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

        public ERM_MonthlyProductionTimeline GetMonthlyProductionTimeLine_V2590(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDescription)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetMonthlyProductionTimeLine_V2590(ERMConnectionString, IdCompany, StartDate, EndDate, IdJobDescription);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        #region [pallavi jadhav] [24 12 2024]
        public List<Templates> GetAllTemplates_V2590()
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllTemplates_V2590(ERMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<CPType> GetAllCPTypes_V2590()
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllCPTypes_V2590(ERMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Region> GetAllRegion_V2590(string IdSite)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllRegion_V2590(ERMConnectionString, IdSite);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<DesignSystemTypeStatus> GetAllDesigns_V2590()
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllDesigns_V2590(ERMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Data.Common.ERM.Detections> GetAllDetections_V2590()
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllDetections_V2590(ERMConnectionString);
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

        #region [GEOS2-6759][Daivshala Vighne][13-01-2025]
        public ERM_MonthlyProductionTimeline GetMonthlyProductionTimeLine_V2600(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDescription)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetMonthlyProductionTimeLine_V2600(ERMConnectionString, IdCompany, StartDate, EndDate, IdJobDescription);
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

        #region [GEOS2-6759][Daivshala Vighne][13-01-2025]
        public ERM_WorkOrder_Other_ProductionTimeline GetWorkorder_Other_MonthlyProduction_V2600()
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;

                return mgr.GetWorkorder_Other_MonthlyProduction_V2600(ERMConnectionString);
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


        #region//[GEOS2-6818][pallavi jadhav] [20 01 2025]

        public ERM_ReworkReport GetALLPlantWeeklyReworksMail_V2600(Company company, DateTime Date)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetALLPlantWeeklyReworksMail_V2600(ERMConnectionString, company, Date);
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
        //[GEOS2-6868][gulab lakade][27 01 2025]
        public List<ERM_ProductionTimeline> GetProductionTimeline_V2600(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDescription)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetProductionTimeline_V2600(ERMConnectionString, IdCompany, StartDate, EndDate, IdJobDescription);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[GEOS2-6900][gulab lakade][28 01 2025]
        public ERM_WeeklyProduction GetGetWeeklyInProductionData_V2610(Company company, DateTime StartDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetGetWeeklyInProductionData_V2610(ERMConnectionString, company, StartDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        #region [GEOS2-6885][Daivshala Vighne][30 01 2025]
        public List<ERM_Warehouses> GetWarehousesByIDSite_V2610(Int32 IdSite)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetWarehousesByIDSite_V2610(ERMConnectionString, IdSite);
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
        //[GEOS2-6900][gulab lakade][30 01 2025]
        public List<ERM_GlobalComparisonTimes> GetGlobalComparisonTimesResults_V2610(DateTime StartDate, DateTime EndDate, int RecordLimit, string idsite, string Region, string Stage, string Template, string CPType, string DesignSystem, string Type, string DSAStatus, string numofways, string numofDetection, string numofOption, string ways, string Detection, string Option)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                //return mgr.GetGlobalComparisonTimesResults_V2610(ERMConnectionString, StartDate, EndDate, RecordLimit, idsite, Region, Stage, Template, CPType, DesignSystem, Type, DSAStatus, IdDetectionNumList, IdDetectionList);
                return mgr.GetGlobalComparisonTimesResults_V2610(ERMConnectionString, StartDate, EndDate, RecordLimit, idsite, Region, Stage, Template, CPType, DesignSystem, Type, DSAStatus, numofways, numofDetection, numofOption, ways, Detection, Option);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public List<DesignSystemTypeStatus> GetAllDesigns_V2610()
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllDesigns_V2610(ERMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        #region [GEOS2-6646][pallavi jadhav][04-02-2025]
        public TimeTrackingWithSites GetAllTimeTrackings_V2600(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;

                // return mgr.GetAllTimeTrackings_V2590(ERMConnectionString, crmConnection, CurrencyName, PlantName, GeosAppSettingList);
                #region [GEOS2-6646][Daivshala Vighne] [10-12-2024]
                return mgr.GetAllTimeTrackings_V2600(ERMConnectionString, crmConnection, CurrencyName, PlantName, GeosAppSettingList);
                #endregion
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public List<TimeTracking> GetTimeTrackingBYPlant_V2600(UInt32 PlantID, UInt32 ProductionIdSite, string Currency, List<GeosAppSetting> GeosAppSettingList)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                //  return mgr.GetTimeTrackingBYPlant_V2580(crmConnection, PlantID, ProductionIdSite, Currency, ERMConnectionString, GeosAppSettingList);
                //[GEOS2-6646][Daivshala Vighne] [10-12-2024]
                return mgr.GetTimeTrackingBYPlant_V2600(crmConnection, PlantID, ProductionIdSite, Currency, ERMConnectionString, GeosAppSettingList);

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
        #region [GEOS2-6771][gulab lakade][05 04 2025]
        public List<ERM_ProductionTimeline> GetProductionTimeline_V2610(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDescription)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetProductionTimeline_V2610(ERMConnectionString, IdCompany, StartDate, EndDate, IdJobDescription);
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
        #region  [GEOS2-6771][gulab lakade][05 04 2025]
        public ERM_MonthlyProductionTimeline GetMonthlyProductionTimeLine_V2610(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDescription)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetMonthlyProductionTimeLine_V2610(ERMConnectionString, IdCompany, StartDate, EndDate, IdJobDescription);
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
        #region [GEOS2-6683][rani dhamankar][12 02 2025] 
        public TimeTrackingWithSites GetAllTimeTrackingProductionTimeline_V2610(string PlantName, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllTimeTrackingProductionTimeline_V2610(ERMConnectionString, crmConnection, PlantName, fromDate, toDate);
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

        #region [GEOS2-6891][pallavi jadhav][13 02 2025]

        public TimeTrackingWithSites GetAllTimeTrackingReport_V2610(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllTimeTrackingReport_V2610(ERMConnectionString, crmConnection, CurrencyName, PlantName, GeosAppSettingList, fromDate, toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<TimeTracking> GetTimeTrackingReportBYPlant_V2610(UInt32 PlantID, UInt32 ProductionIdSite, string Currency, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetTimeTrackingReportBYPlant_V2610(crmConnection, PlantID, ProductionIdSite, Currency, ERMConnectionString, GeosAppSettingList, fromDate, toDate);
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

        #region [GEOS2-6886][Pallavi jadhav][14 02 2025]
        public List<ERMArticleStockPlanning> GetArticleStockPlanningList_V2610(Int32 IdWarehouse, DateTime StartDate, DateTime EndDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                return mgr.GetArticleStockPlanningList_V2610(ERMConnectionString, IdWarehouse, StartDate, EndDate);

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
        #region [GEOS2-6949][gulab lakade][17 02 2025]
        public ERM_MonthlyProductionTimeline GetWeeklyProductionEmployeeTime_V2610(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDeescription)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetWeeklyProductionEmployeeTime_V2610(ERMConnectionString, IdCompany, StartDate, EndDate, IdJobDeescription);
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
        #region [rani dhamankar][17-02-2025][GEOS2-6887]
        public List<ERMOutgoingArticleStockPlanning> GetOutgoingArticleStockPlanningList_V2610(Int32 IdArticle, Int32 IdWarehouse, DateTime StartDate, DateTime EndDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                return mgr.GetOutgoingArticleStockPlanningList_V2610(ERMConnectionString, IdArticle, IdWarehouse, StartDate, EndDate);

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
        #region [GEOS2-7031][gulab lakade][25 02 2025]
        public string GetLatestWorkOperationCodeByCode_V2620(string code)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetLatestWorkOperationCodeByCode_V2620(code, ERMConnectionString);
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
        #region [rani dhamankar][24-02-2025][GEOS2-6889]
        public List<ERMIncomingArticleStockPlanning> GetIncomingArticleStockPlanningList_V2620(Int32 IdArticle, Int32 IdWarehouse, DateTime StartDate, DateTime EndDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetIncomingArticleStockPlanningList_V2620(ERMConnectionString, IdArticle, IdWarehouse, StartDate, EndDate);

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
        #region [rani dhamankar][27-02-2025][GEOS2-6888]
        public List<ERMOutgoingArticleStockPlanning> GetOutgoingArticleStockPlanningList_V2620(Int32 IdArticle, Int32 IdWarehouse, DateTime StartDate, DateTime EndDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                return mgr.GetOutgoingArticleStockPlanningList_V2620(ERMConnectionString, IdArticle, IdWarehouse, StartDate, EndDate);

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
        #region [GEOS2-6965][rani dhamankar][10-03-2025]
        public List<ERM_ProductionTimeline> GetProductionTimeline_V2620(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDescription)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetProductionTimeline_V2620(ERMConnectionString, IdCompany, StartDate, EndDate, IdJobDescription);
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
        #region // [pallavi jadhav][GEOS2-7060][25-03-2025] 
        public TimeTrackingWithSites GetAllTimeTrackings_V2630(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;

                // return mgr.GetAllTimeTrackings_V2590(ERMConnectionString, crmConnection, CurrencyName, PlantName, GeosAppSettingList);

                return mgr.GetAllTimeTrackings_V2630(ERMConnectionString, crmConnection, CurrencyName, PlantName, GeosAppSettingList, fromDate, toDate);

            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public List<TimeTracking> GetTimeTrackingBYPlant_V2630(UInt32 PlantID, UInt32 ProductionIdSite, string Currency, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                //  return mgr.GetTimeTrackingBYPlant_V2580(crmConnection, PlantID, ProductionIdSite, Currency, ERMConnectionString, GeosAppSettingList);
                //[GEOS2-6646][Daivshala Vighne] [10-12-2024]
                return mgr.GetTimeTrackingBYPlant_V2630(crmConnection, PlantID, ProductionIdSite, Currency, ERMConnectionString, GeosAppSettingList, fromDate, toDate);

            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public List<MaxMinDate> GetDeliveryDateANDPlannedDeliveryDate_V2630(Int32 IdSite)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                return mgr.GetDeliveryDateANDPlannedDeliveryDate_V2630(ERMConnectionString, IdSite);

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

        #region [GEOS2-6836][rani dhamankar][26-03-2025]

        public TimeTrackingWithSites GetAllTimeTrackingReport_V2630(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllTimeTrackingReport_V2630(ERMConnectionString, crmConnection, CurrencyName, PlantName, GeosAppSettingList, fromDate, toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<TimeTracking> GetTimeTrackingReportBYPlant_V2630(UInt32 PlantID, UInt32 ProductionIdSite, string Currency, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetTimeTrackingReportBYPlant_V2630(crmConnection, PlantID, ProductionIdSite, Currency, ERMConnectionString, GeosAppSettingList, fromDate, toDate);
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
        #region [GEOS2-7642][gulab lakade][27 03 2025]
        public List<ERM_ProductionTimeline> GetProductionTimeline_V2630(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDescription)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetProductionTimeline_V2630(ERMConnectionString, IdCompany, StartDate, EndDate, IdJobDescription);
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
        #region  [GEOS2-7099][gulab lakade][07 04 2025]
        public ERM_MonthlyProductionTimeline GetMonthlyProductionTimeLine_V2630(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDescription)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetMonthlyProductionTimeLine_V2630(ERMConnectionString, IdCompany, StartDate, EndDate, IdJobDescription);
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
        #region [GEOS2-6835][dhawal bhalerao][08 04 2025]
        public List<WorkOperationByStages> GetAllWorkOperationStages_V2630(int IdStage, string IdCP)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllWorkOperationStages_V2630(ERMConnectionString, IdStage, IdCP);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<StandardOperationsDictionaryModules> GetAllStandardOperationsDictionaryModulesById_V2630(UInt64 IdStandardOperationsDictionary, string IdStage, string IdCPType, string IdCP)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllStandardOperationsDictionaryModulesById_V2630(ERMConnectionString, IdStandardOperationsDictionary, IdStage, IdCPType, IdCP);
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
        #region  [GEOS2-7902][gulab lakade][28 04 2025]
        public ERM_MonthlyProductionTimeline GetMonthlyProductionTimeLine_V2640(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDescription)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetMonthlyProductionTimeLine_V2640(ERMConnectionString, IdCompany, StartDate, EndDate, IdJobDescription);
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
        #region [GEOS2-7098][rani dhamankar][23 04 2025] 
        public TimeTrackingWithSites GetAllTimeTrackingProductionTimeline_V2640(string PlantName, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllTimeTrackingProductionTimeline_V2640(ERMConnectionString, crmConnection, PlantName, fromDate, toDate);
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

        #region [GEOS2-7094][dhawal bhalerao][28 04 2025]
        public TimeTrackingWithSites GetCamCadTimeTrackings_V2640(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;

                // return mgr.GetAllTimeTrackings_V2590(ERMConnectionString, crmConnection, CurrencyName, PlantName, GeosAppSettingList);

                return mgr.GetCamCadTimeTrackings_V2640(ERMConnectionString, crmConnection, CurrencyName, PlantName, GeosAppSettingList, fromDate, toDate);

            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public List<TimeTracking> GetCamCadTimeTrackingBYPlant_V2640(UInt32 PlantID, UInt32 ProductionIdSite, string Currency, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                //  return mgr.GetTimeTrackingBYPlant_V2580(crmConnection, PlantID, ProductionIdSite, Currency, ERMConnectionString, GeosAppSettingList);
                //[GEOS2-6646][Daivshala Vighne] [10-12-2024]
                return mgr.GetCamCadTimeTrackingBYPlant_V2640(crmConnection, PlantID, ProductionIdSite, Currency, ERMConnectionString, GeosAppSettingList, fromDate, toDate);

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

        #region [GEOS2-6573][rani dhamankar][07-05-2025]
        public List<ERM_ProductionTimeline> GetProductionTimeline_V2640(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDescription)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetProductionTimeline_V2640(ERMConnectionString, IdCompany, StartDate, EndDate, IdJobDescription);
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

        #region [GEOS2-7908][gulab lakade][13 05 2025]
        public ERM_MonthlyProductionTimeline GetWeeklyProductionEmployeeTime_V2640(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDeescription)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetWeeklyProductionEmployeeTime_V2640(ERMConnectionString, IdCompany, StartDate, EndDate, IdJobDeescription);
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
        #region [pallavi.jadhav][29 04 2025][GEOS2-7066]
        public List<WeeklyProductionReport> GetAllWeeklyproductionReportByPlant_V2640(Company company, DateTime StartDate)
        {
            try
            {
                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllWeeklyproductionReportByPlant_V2640(ERMConnectionString, crmConnection, company, StartDate);
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

        #region//[Pallavi.jadhav][16 05 2025][GEOS2-8124]

        public TimeTrackingWithSites GetAllTimeTrackings_V2640(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;

                // return mgr.GetAllTimeTrackings_V2590(ERMConnectionString, crmConnection, CurrencyName, PlantName, GeosAppSettingList);

                return mgr.GetAllTimeTrackings_V2640(ERMConnectionString, crmConnection, CurrencyName, PlantName, GeosAppSettingList, fromDate, toDate);

            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public List<TimeTracking> GetTimeTrackingBYPlant_V2640(UInt32 PlantID, UInt32 ProductionIdSite, string Currency, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                //  return mgr.GetTimeTrackingBYPlant_V2580(crmConnection, PlantID, ProductionIdSite, Currency, ERMConnectionString, GeosAppSettingList);
                //[GEOS2-6646][Daivshala Vighne] [10-12-2024]
                return mgr.GetTimeTrackingBYPlant_V2640(crmConnection, PlantID, ProductionIdSite, Currency, ERMConnectionString, GeosAppSettingList, fromDate, toDate);

            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public TimeTrackingWithSites GetAllTimeTrackingReport_V2640(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllTimeTrackingReport_V2640(ERMConnectionString, crmConnection, CurrencyName, PlantName, GeosAppSettingList, fromDate, toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<TimeTracking> GetTimeTrackingReportBYPlant_V2640(UInt32 PlantID, UInt32 ProductionIdSite, string Currency, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetTimeTrackingReportBYPlant_V2640(crmConnection, PlantID, ProductionIdSite, Currency, ERMConnectionString, GeosAppSettingList, fromDate, toDate);
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
        #region [GEOS2-8189][rani dhamankar][29-05-2025]
        public List<ERM_ProductionTimeline> GetProductionTimeline_V2650(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDescription)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetProductionTimeline_V2650(ERMConnectionString, IdCompany, StartDate, EndDate, IdJobDescription);
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
        #region [GEOS2-8187][gulab lakade][09 06 2025][add logic for machine time for CNC Stage]
        public List<WeeklyProductionReport> GetAllWeeklyproductionReportByPlant_V2650(Company company, DateTime StartDate)
        {
            try
            {
                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllWeeklyproductionReportByPlant_V2650(ERMConnectionString, crmConnection, company, StartDate);
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

        //[nsatpute][25-06-2025][GEOS2-8641]
        public TimeTrackingWithSites GetAllTimeTrackingReport_V2650(string CurrencyName, string PlantName, List<SitesByShippingAddress> sitesByShippingAddressList, List<GeosAppSetting> GeosAppSettingList, GeosAppSetting timeTrackingAppSetting, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllTimeTrackingReport_V2650(ERMConnectionString, CurrencyName, PlantName, sitesByShippingAddressList, GeosAppSettingList, timeTrackingAppSetting, fromDate, toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[nsatpute][25-06-2025][GEOS2-8641]
        public List<SitesByShippingAddress> GetAllSitesByShippingAddress()
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllSitesByShippingAddress(ERMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[nsatpute][02.07.2025][GEOS2-8172]
        public List<ERM_GlobalComparisonTimes> GetGlobalComparisonTimesResults_V2650(DateTime StartDate, DateTime EndDate, int RecordLimit, string idsite, string Region, string Stage, string Template, string CPType, string DesignSystem, string Type, string DSAStatus, string numofways, string numofDetection, string numofOption, string ways, string Detection, string Option)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;                
                return mgr.GetGlobalComparisonTimesResults_V2650(ERMConnectionString, StartDate, EndDate, RecordLimit, idsite, Region, Stage, Template, CPType, DesignSystem, Type, DSAStatus, numofways, numofDetection, numofOption, ways, Detection, Option);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        #region [GEOS2-8868][gulab lakade][10 07 2025]
        public TimeTrackingWithSites GetCamCadTimeTrackings_V2660(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;

                // return mgr.GetAllTimeTrackings_V2590(ERMConnectionString, crmConnection, CurrencyName, PlantName, GeosAppSettingList);

                return mgr.GetCamCadTimeTrackings_V2660(ERMConnectionString, crmConnection, CurrencyName, PlantName, GeosAppSettingList, fromDate, toDate);

            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public List<TimeTracking> GetCamCadTimeTrackingBYPlant_V2660(UInt32 PlantID, UInt32 ProductionIdSite, string Currency, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                //  return mgr.GetTimeTrackingBYPlant_V2580(crmConnection, PlantID, ProductionIdSite, Currency, ERMConnectionString, GeosAppSettingList);
                //[GEOS2-6646][Daivshala Vighne] [10-12-2024]
                return mgr.GetCamCadTimeTrackingBYPlant_V2660(crmConnection, PlantID, ProductionIdSite, Currency, ERMConnectionString, GeosAppSettingList, fromDate, toDate);

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

        #region [pallavi.jadhav][11 07 2025][GEOS2-8868] 

        public TimeTrackingWithSites GetAllTimeTrackings_V2660(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;

                // return mgr.GetAllTimeTrackings_V2590(ERMConnectionString, crmConnection, CurrencyName, PlantName, GeosAppSettingList);

                return mgr.GetAllTimeTrackings_V2660(ERMConnectionString, crmConnection, CurrencyName, PlantName, GeosAppSettingList, fromDate, toDate);

            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public List<TimeTracking> GetTimeTrackingBYPlant_V2660(UInt32 PlantID, UInt32 ProductionIdSite, string Currency, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                //  return mgr.GetTimeTrackingBYPlant_V2580(crmConnection, PlantID, ProductionIdSite, Currency, ERMConnectionString, GeosAppSettingList);
                //[GEOS2-6646][Daivshala Vighne] [10-12-2024]
                return mgr.GetTimeTrackingBYPlant_V2660(crmConnection, PlantID, ProductionIdSite, Currency, ERMConnectionString, GeosAppSettingList, fromDate, toDate);

            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public TimeTrackingWithSites GetAllTimeTrackingReport_V2660(string CurrencyName, string PlantName, List<SitesByShippingAddress> sitesByShippingAddressList, List<GeosAppSetting> GeosAppSettingList, GeosAppSetting timeTrackingAppSetting, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllTimeTrackingReport_V2660(ERMConnectionString, CurrencyName, PlantName, sitesByShippingAddressList, GeosAppSettingList, timeTrackingAppSetting, fromDate, toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public List<TimeTracking> GetTimeTrackingReportBYPlant_V2660(UInt32 PlantID, UInt32 ProductionIdSite, string Currency, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetTimeTrackingReportBYPlant_V2660(crmConnection, PlantID, ProductionIdSite, Currency, ERMConnectionString, GeosAppSettingList, fromDate, toDate);
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

        #region [GEOS2-8698][rani dhamankar][15-07-2025]
        public List<Site> GetPlants_V2660(int IdUser)
        {
            try
            {

                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetPlants_V2660(connectionString, IdUser);
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

        // [GEOS2-8698][rani dhamankar][16-07-2025]
        public List<Company> GetAllCompaniesDetails_V2660(Int32 idUser)
        {
            List<Company> connectionstrings = null;
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                connectionstrings = mgr.GetAllCompaniesDetails_V2660(idUser, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                //exp.Logger.Log("Get an error in GetAllCompaniesDetails() Method " + exp.ErrorMessage, category: Category.Info, priority: Priority.Low);
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return connectionstrings;
        }
        #endregion
        #region [GEOS2-8005][rani dhamankar][13-06-2025]
        public List<TimeTracking> GetTimeTrackingCADCAMDetailsPdfEmail_V2660(string PlantName, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetTimeTrackingCADCAMDetailsPdfEmail_V2660(ERMConnectionString, PlantName, fromDate, toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public TimeTrackingWithSites GetAllTimeTrackingProductionTimeline_V2660(string PlantName, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllTimeTrackingProductionTimeline_V2660(ERMConnectionString, crmConnection, PlantName, fromDate, toDate);
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

        #region [pallavi.jadhav][21 07 2025][GEOS2-8814]

        public TimeTrackingWithSites GetAllTimeTrackingReport_V2660V1(string CurrencyName, string PlantName, List<SitesByShippingAddress> sitesByShippingAddressList, List<GeosAppSetting> GeosAppSettingList, GeosAppSetting timeTrackingAppSetting, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllTimeTrackingReport_V2660V1(ERMConnectionString, CurrencyName, PlantName, sitesByShippingAddressList, GeosAppSettingList, timeTrackingAppSetting, fromDate, toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public List<TimeTracking> GetTimeTrackingReportBYPlant_V2660V1(UInt32 PlantID, UInt32 ProductionIdSite, string Currency, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetTimeTrackingReportBYPlant_V2660V1(crmConnection, PlantID, ProductionIdSite, Currency, ERMConnectionString, GeosAppSettingList, fromDate, toDate);
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

        #region  [GEOS2-8189][pallavi jadhav][28-07-2025]
        public List<ERM_ProductionTimeline> GetProductionTimeline_V2660(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDescription)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetProductionTimeline_V2660(ERMConnectionString, IdCompany, StartDate, EndDate, IdJobDescription);
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

        #region [GEOS2-8907][pallavi jadhav][30-07-2025]
        public ERM_MonthlyProductionTimeline GetWeeklyProductionEmployeeTime_V2660(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDeescription)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetWeeklyProductionEmployeeTime_V2660(ERMConnectionString, IdCompany, StartDate, EndDate, IdJobDeescription);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public ERM_MonthlyProductionTimeline GetMonthlyProductionTimeLine_V2660(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDescription)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetMonthlyProductionTimeLine_V2660(ERMConnectionString, IdCompany, StartDate, EndDate, IdJobDescription);
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
        #region [GEOS2-9119][gulab lakade][05 08 2025]
        public ERM_ReworkReport GetProductionOutputReworksMail_V2660(Company company, DateTime StartDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetProductionOutputReworksMail_V2660(ERMConnectionString, company, StartDate);
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
        #region[GEOS2-9201][rani dhamankar][12-08-2025]
        public List<ERM_GlobalComparisonTimes> GetGlobalComparisonTimesResults_V2660(DateTime StartDate, DateTime EndDate, int RecordLimit, string idsite, string Region, string Stage, string Template, string CPType, string DesignSystem, string Type, string DSAStatus, string numofways, string numofDetection, string numofOption, string ways, string Detection, string Option)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                //return mgr.GetGlobalComparisonTimesResults_V2610(ERMConnectionString, StartDate, EndDate, RecordLimit, idsite, Region, Stage, Template, CPType, DesignSystem, Type, DSAStatus, IdDetectionNumList, IdDetectionList);
                return mgr.GetGlobalComparisonTimesResults_V2660(ERMConnectionString, StartDate, EndDate, RecordLimit, idsite, Region, Stage, Template, CPType, DesignSystem, Type, DSAStatus, numofways, numofDetection, numofOption, ways, Detection, Option);
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
        #region[GEOS2-9233][rani dhamankar][13-08-2025]
        public List<ERM_GlobalComparisonTimes> GetGlobalComparisonTimesResults_V2670(DateTime StartDate, DateTime EndDate, int RecordLimit, string idsite, string Region, string Stage, string Template, string CPType, string DesignSystem, string Type, string DSAStatus, string numofways, string numofDetection, string numofOption, string ways, string Detection, string Option)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                //return mgr.GetGlobalComparisonTimesResults_V2610(ERMConnectionString, StartDate, EndDate, RecordLimit, idsite, Region, Stage, Template, CPType, DesignSystem, Type, DSAStatus, IdDetectionNumList, IdDetectionList);
                return mgr.GetGlobalComparisonTimesResults_V2670(ERMConnectionString, StartDate, EndDate, RecordLimit, idsite, Region, Stage, Template, CPType, DesignSystem, Type, DSAStatus, numofways, numofDetection, numofOption, ways, Detection, Option);
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
        #region  [GEOS2-9220][gulab lakade][08 08 2025]
        public ERM_Main_productiontimeline GetProductionTimeline_V2660_V1(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDescription)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetProductionTimeline_V2660_V1(ERMConnectionString, IdCompany, StartDate, EndDate, IdJobDescription);
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
        #region  [GEOS2-9220][gulab lakade][08 08 2025]
        public ERM_Main_productiontimeline GetProductionTimeline_V2670(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDescription)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetProductionTimeline_V2670(ERMConnectionString, IdCompany, StartDate, EndDate, IdJobDescription);
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
        #region [GEOS2-8309][rani dhamankar][29 08 2025]
        public TimeTrackingWithSites GetCamCadTimeTrackings_V2670(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;


                return mgr.GetCamCadTimeTrackings_V2670(ERMConnectionString, crmConnection, CurrencyName, PlantName, GeosAppSettingList, fromDate, toDate);

            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public List<TimeTracking> GetCamCadTimeTrackingBYPlant_V2670(UInt32 PlantID, UInt32 ProductionIdSite, string Currency, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;

                return mgr.GetCamCadTimeTrackingBYPlant_V2670(crmConnection, PlantID, ProductionIdSite, Currency, ERMConnectionString, GeosAppSettingList, fromDate, toDate);

            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public TimeTrackingWithSites GetAllTimeTrackings_V2670(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;


                return mgr.GetAllTimeTrackings_V2670(ERMConnectionString, crmConnection, CurrencyName, PlantName, GeosAppSettingList, fromDate, toDate);

            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public List<TimeTracking> GetTimeTrackingBYPlant_V2670(UInt32 PlantID, UInt32 ProductionIdSite, string Currency, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;

                return mgr.GetTimeTrackingBYPlant_V2670(crmConnection, PlantID, ProductionIdSite, Currency, ERMConnectionString, GeosAppSettingList, fromDate, toDate);

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

        #region [GEOS2-7091][rani dhamankar][11 09 2025]
        public List<Counterpartstracking> GetOperatorDesignSahredItemTimeTrackingDetails_V2670( UInt32 PlantID,   DateTime fromDate, DateTime toDate, UInt32 IdSiteOwnersPlant)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetOperatorDesignSahredItemTimeTrackingDetails_V2670(ERMConnectionString, PlantID,   fromDate,  toDate, IdSiteOwnersPlant);
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

        #region [pallavi.jadhav][GEOS2-8550][23 09 2025]
        public ERM_MonthlyProductionTimeline GetMonthlyProductionTimeLine_V2670(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDescription)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetMonthlyProductionTimeLine_V2670(ERMConnectionString, IdCompany, StartDate, EndDate, IdJobDescription);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public ERM_MonthlyProductionTimeline GetWeeklyProductionEmployeeTime_V2670(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDeescription)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetWeeklyProductionEmployeeTime_V2670(ERMConnectionString, IdCompany, StartDate, EndDate, IdJobDeescription);
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
        #region // [suggestion of yuvraj sir][gulab lakade][03 10 2025]
        public ERM_ReworkReport GetProductionOutputReworksMail_V2670(Company company, DateTime StartDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetProductionOutputReworksMail_V2670(ERMConnectionString, company, StartDate);
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
        #region // [suggestion of yuvraj sir][gulab lakade][03 10 2025]

        public ERM_ReworkReport GetALLPlantWeeklyReworksMail_V2670(Company company, DateTime Date)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                string crmConnection = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetALLPlantWeeklyReworksMail_V2670(ERMConnectionString, company, Date);
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
        #region [GEOS2-9443] [gulab lakade][2025 10 16]
        public ERM_WorkOrder_Other_ProductionTimeline GetWorkorder_Other_MonthlyProduction_V2680()
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;

                return mgr.GetWorkorder_Other_MonthlyProduction_V2680(ERMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public ERM_Main_productiontimeline GetProductionTimeline_V2680(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDescription)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetProductionTimeline_V2680(ERMConnectionString, IdCompany, StartDate, EndDate, IdJobDescription);
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

        #region [GEOS2-10146][pallavi jadhav][11-11-2025]
        public List<Site> GetPlants_V2680(int IdUser)
        {
            try
            {

                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetPlants_V2680(connectionString, IdUser);
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

       
        public List<Company> GetAllCompaniesDetails_V2680(Int32 idUser)
        {
            List<Company> connectionstrings = null;
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                connectionstrings = mgr.GetAllCompaniesDetails_V2680(idUser, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                //exp.Logger.Log("Get an error in GetAllCompaniesDetails() Method " + exp.ErrorMessage, category: Category.Info, priority: Priority.Low);
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return connectionstrings;
        }
        #endregion
        #region[GEOS2-9554][rajashri telvekar][12-11-2025]
        public List<ERM_GlobalComparisonTimes> GetGlobalComparisonTimesResults_V2690(DateTime StartDate, DateTime EndDate, int RecordLimit, string idsite, string Region, string Stage, string Template, string CPType, string DesignSystem, string Type, string DSAStatus, string numofways, string numofDetection, string numofOption, string ways, string Detection, string Option, string CurrentIdstage)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                //return mgr.GetGlobalComparisonTimesResults_V2610(ERMConnectionString, StartDate, EndDate, RecordLimit, idsite, Region, Stage, Template, CPType, DesignSystem, Type, DSAStatus, IdDetectionNumList, IdDetectionList);
                //GetGlobalComparisonTimesResults_V2670
                return mgr.GetGlobalComparisonTimesResults_V2690(ERMConnectionString, StartDate, EndDate, RecordLimit, idsite, Region, Stage, Template, CPType, DesignSystem, Type, DSAStatus, numofways, numofDetection, numofOption, ways, Detection, Option, CurrentIdstage);
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
        #region [GEOS2-9393][pallavi jadhav][12 11 2025]
        public ERM_Main_productiontimeline GetProductionTimeline_V2690(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDescription)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetProductionTimeline_V2690(ERMConnectionString, IdCompany, StartDate, EndDate, IdJobDescription);
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
        #region [GEOS2-9404][gulab lakade][18 11 2025]
        public List<ERM_Warehouses> GetWarehousesByIDSite_V2690(Int32 IdSite)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetWarehousesByIDSite_V2690(ERMConnectionString, IdSite);
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
        #region [GEOS2-9123][gulab lakade][20 11 2025]
        public List<ERMArticleStockPlanning> GetArticleStockPlanningList_V2690(Int32 IdWarehouse, DateTime StartDate, DateTime EndDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                return mgr.GetArticleStockPlanningList_V2690(ERMConnectionString, IdWarehouse, StartDate, EndDate);

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
        #region [GEOS2-9398][gulab lakade][20 11 2025]
        public List<ERMOutgoingArticleStockPlanning> GetOutgoingArticleStockPlanningList_V2690(Int32 IdArticle, Int32 IdWarehouse, DateTime StartDate, DateTime EndDate)
        {
            try
            {
                string ERMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;

                return mgr.GetOutgoingArticleStockPlanningList_V2690(ERMConnectionString, IdArticle, IdWarehouse, StartDate, EndDate);

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
    }
}

