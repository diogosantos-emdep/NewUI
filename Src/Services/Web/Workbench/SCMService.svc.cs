using Emdep.Geos.Data.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Emdep.Geos.Data.Common.SCM;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Services.Contracts;
using System.Configuration;
using Emdep.Geos.Data.Common;

namespace Emdep.Geos.Services.Web.Workbench
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "SCMService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select SCMService.svc or SCMService.svc.cs at the Solution Explorer and start debugging.
    public class SCMService : ISCMService
    {
        SCMManager mgr = new SCMManager();

        public List<Color> GetAllColors(string language)
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllColors(localGeosConnectionString, language);
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


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Family> GetAllFamilies(string language)
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllFamilies(localGeosConnectionString, language);
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


        public List<Subfamily> GetAllSubfamilies(string language)
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllSubfamilies(localGeosConnectionString, language);
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

        List<Data.Common.SCM.Company> ISCMService.GetAllCompany()
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllCompany(localGeosConnectionString);
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

        //[rdixit][GEOS2-4399][23.06.2023]
        public List<Gender> GetGender(String language)
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetGender(localGeosConnectionString, language);
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
        //[nsatpute][10-07-2023] SCM - Properties Manager (2/4) GEOS2-4501
        public List<ConnectorProperties> GetConnectorProperties(String language)
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.GetConnectorProperties(localGeosConnectionString, language);
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


        //[Sudhir.Jangra][GEOS2-4502][11/07/2023]

        /// <summary>
        /// This method is used to insert product type.
        /// </summary>
        /// 
        public bool AddNewValueKey(ValueKey valueKey, string GeosSettingsKey)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                //string ConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.AddNewValueKey(valueKey, GeosSettingsKey, MainServerConnectionString);
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



        //[Sudhir.Jangra][GEOS2-4502][18/07/2023]
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


        //[Sudhir.Jangra][GEOS2-4501][19/07/2023]
        //[nsatpute][GEOS2-4501][20/07/2023]
        public void AddPropertyManager(List<ConnectorProperties> connectorProperty)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                mgr.AddPropertyManager(MainServerConnectionString, connectorProperty);
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
        //[nsatpute][GEOS2-4501][26/07/2023]
        public void DeletePropertyManager(List<ConnectorProperties> connectorProperty)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                mgr.DeletePropertyManager(MainServerConnectionString, connectorProperty);
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
        //[nsatpute][GEOS2-4501][20/07/2023]
        public List<ConnectorProperties> GetPropertyManager()
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.GetPropertyManager(MainServerConnectionString);
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

        //[Sudhir.Jangra][GEOS2-4502][19/07/2023]
        public bool AddCustomProperty(CustomProperty customProperty)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddCustomProperty(MainServerConnectionString, customProperty);
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


        //[Sudhir.Jangra][GEOS2-4502]
        public List<Data.Common.SCM.ValueType> GetValueType()
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetValueType(localGeosConnectionString);
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

        //[Sudhir.Jangra][GEOS2-4504]
        public List<ValueKey> GetValueKey(string LookupValueKey)
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetValueKey(localGeosConnectionString, LookupValueKey);
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
        //[Sudhir.Jangra][GEOS2-4504]
        public bool UpdateValueKey(ValueKey valueKey)
        {
            try
            {
                //string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateValueKey(MainServerConnectionString, valueKey);
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


        //[Sudhir.Jangra][GEOS2-4503]
        public CustomProperty GetEditCustomProperty(Int32 Id)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.GetEditCustomProperty(MainServerConnectionString, Id);
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

        //[Sudhir.Jangra][GEOS2-4505]
        public bool UpdateEditCustomProperty(CustomProperty customProperty)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateEditCustomProperty(MainServerConnectionString, customProperty);
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

        //[pallavi.jadhav][GEOS2-4562][27/07/2023]
        public List<ConnectorFamilies> GetConnectorFamilies()
        {
            try
            {

                string LocalGeosContextConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                //  string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.GetConnectorFamilies(LocalGeosContextConnectionString, Properties.Settings.Default.ConnectorFamilyVisualAidsPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();

                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-4563][rupali sarode][31-07-2023]
        public ConnectorFamilies AddConnectorFamilies_V2420(ConnectorFamilies ConnectorFamily, List<Subfamily> SubFamiliesList)
        {
            //ConnectorFamilies ConnectorFamily
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddConnectorFamilies_V2420(MainServerConnectionString, ConnectorFamily, Properties.Settings.Default.ConnectorFamilyVisualAidsPath, SubFamiliesList);
                //ConnectorFamily
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

        //[gulab lakade][GEOS2-4506][16 08 2023]
        public List<ValueKey> GetAllLookupKey()
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllLookupKey(localGeosConnectionString);
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

        //[gulab lakade][GEOS2-4506][16 08 2023]
        public List<LookUpValues> GetAllLookUpValuesRecordByIDLookupkey(int IdLookupKey)
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllLookUpValuesRecordByIDLookupkey(localGeosConnectionString, IdLookupKey);
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

        //[gulab lakade][GEOS2-4506][16 08 2023]
        public bool InsertLookupKey_V2420(LookUpValues valueKey)
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.InsertLookupKey_V2420(valueKey, localGeosConnectionString);
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
        //[gulab lakade][GEOS2-4506][16 08 2023]
        public bool UpdateLookupKey_V2420(LookUpValues valueKey)
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateLookupKey_V2420(localGeosConnectionString, valueKey);
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

        public bool IsDeletedValueList_V2420(int IdLookupValue)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.IsDeletedValueList_V2420(IdLookupValue, MainServerConnectionString);
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

        public ConnectorFamilies UpdateConnectorFamilies_V2420(ConnectorFamilies ConnectorFamily, Int32 IdFamily, List<Subfamily> SubFamiliesList)
        {
            //ConnectorFamilies ConnectorFamily
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateConnectorFamilies_V2420(MainServerConnectionString, ConnectorFamily, IdFamily, Properties.Settings.Default.ConnectorFamilyVisualAidsPath, SubFamiliesList);
                //ConnectorFamily
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

        //[Aishwarya Ingale]]
        public List<Subfamily> GetSubFamily(int IdFamily)
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.GetSubFamily(localGeosConnectionString, IdFamily, Properties.Settings.Default.ConnectorFamilyVisualAidsPath);
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

        #region V2430
        public List<ConnectorSubFamily> GetAllSubfamilies_V2430(string language)
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllSubfamilies_V2430(localGeosConnectionString, language);
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
        public List<ConnectorFamily> GetConnectorFamilies_V2430()
        {
            try
            {

                string LocalGeosContextConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetConnectorFamilies_V2430(LocalGeosContextConnectionString, Properties.Settings.Default.ConnectorFamilyVisualAidsPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();

                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public bool UpdateSubFamily_V2430(ConnectorSubFamily subFamily)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateSubFamily_V2430(MainServerConnectionString, subFamily, Properties.Settings.Default.ConnectorFamilyVisualAidsPath);
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
        public ConnectorFamily AddConnectorFamilies_V2430(ConnectorFamily ConnectorFamily)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddConnectorFamilies_V2430(MainServerConnectionString, ConnectorFamily, Properties.Settings.Default.ConnectorFamilyVisualAidsPath);
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
        public ConnectorFamily UpdateConnectorFamilies_V2430(ConnectorFamily ConnectorFamily)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateConnectorFamilies_V2430(MainServerConnectionString, ConnectorFamily, Properties.Settings.Default.ConnectorFamilyVisualAidsPath);
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
        public List<FamilyImage> GetFamilyImageImagesByIdFamily_V2430(int IdFamily, string FamilyName)
        {
            try
            {
                string LocalGeosContextConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetFamilyImageImagesByIdFamily_V2430(LocalGeosContextConnectionString, IdFamily, FamilyName, Properties.Settings.Default.ConnectorFamilyVisualAidsPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();

                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public List<ConnectorSubFamily> GetSubFamily_V2430(int IdFamily, string FamilyName)
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.GetSubFamily_V2430(localGeosConnectionString, IdFamily, FamilyName, Properties.Settings.Default.ConnectorFamilyVisualAidsPath);
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

        public ConnectorSubFamily GetSubFamilyDetails(int idSubFamily, string familyName)
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.GetSubFamilyDetails(localGeosConnectionString, idSubFamily, familyName, Properties.Settings.Default.ConnectorFamilyVisualAidsPath);
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
        #endregion

        //Shubham[skadam] GEOS2-4595 SCM - Search results (1/4) 08 09 2023 
        //public List<Connectors> GetAllConnectors(uint? IdCompany, uint? IdSubFamily, uint? IdFamily, uint? IdColor, uint? IdGender, Int32? IdShape)
        public List<Connectors> GetAllConnectors(Connectors Connectors)
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.GetAllConnectors(localGeosConnectionString, Connectors, Properties.Settings.Default.ConnectorImage);
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
        //Shubham[skadam] GEOS2-4596 SCM - Search results (2/4) 12 09 2023 
        public List<SCMConnectorImage> GetAllConnectorImages(Connectors Connectors)
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.GetAllConnectorImages(localGeosConnectionString, Connectors, Properties.Settings.Default.ConnectorImage);
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
        //[rdixit][14.09.2023][GEOS2-4602]
        public List<ComponentType> GetAllComponentTypes()
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.GetAllComponentTypes(localGeosConnectionString);
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

        public List<CustomProperty> GetAllCustomeData()
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.GetAllCustomeData(localGeosConnectionString);
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

        public List<Connectors> GetAllConnectors_V2440(Connectors Connectors)
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.GetAllConnectors_V2440(localGeosConnectionString, Connectors, Properties.Settings.Default.ConnectorImage);
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
        #region [rdixit][GEOS2-4958][20.10.2023]
        public List<ConnectorFamily> GetConnectorFamilies_V2450()
        {
            try
            {

                string LocalGeosContextConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetConnectorFamilies_V2450(LocalGeosContextConnectionString, Properties.Settings.Default.ConnectorFamilyVisualAidsPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();

                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<ConnectorSubFamily> GetSubFamily_V2450(int IdFamily, string FamilyName)
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.GetSubFamily_V2450(localGeosConnectionString, IdFamily, FamilyName, Properties.Settings.Default.ConnectorFamilyVisualAidsPath);
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

        public ConnectorSubFamily GetSubFamilyDetails_V2450(int idSubFamily, string familyName)
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.GetSubFamilyDetails_V2450(localGeosConnectionString, idSubFamily, familyName, Properties.Settings.Default.ConnectorFamilyVisualAidsPath);
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

        public List<FamilyImage> GetFamilyImageImagesByIdFamily_V2450(int IdFamily, string FamilyName)
        {
            try
            {
                string LocalGeosContextConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetFamilyImageImagesByIdFamily_V2450(LocalGeosContextConnectionString, IdFamily, FamilyName, Properties.Settings.Default.ConnectorFamilyVisualAidsPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();

                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<Family> GetAllFamilies_V2450(string language)
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllFamilies_V2450(localGeosConnectionString, language);
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


        public ConnectorFamily AddConnectorFamilies_V2450(ConnectorFamily ConnectorFamily)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddConnectorFamilies_V2450(MainServerConnectionString, ConnectorFamily, Properties.Settings.Default.ConnectorFamilyVisualAidsPath);
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

        public bool UpdateSubFamily_V2450(ConnectorSubFamily subFamily)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateSubFamily_V2450(MainServerConnectionString, subFamily, Properties.Settings.Default.ConnectorFamilyVisualAidsPath);
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

        public ConnectorFamily UpdateConnectorFamilies_V2450(ConnectorFamily ConnectorFamily)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateConnectorFamilies_V2450(MainServerConnectionString, ConnectorFamily, Properties.Settings.Default.ConnectorFamilyVisualAidsPath);
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
        #endregion

        //[rdixit][29.11.2023][GEOS2-4955]
        public List<ConnectorProperties> GetPropertyManager_V2460()
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.GetPropertyManager_V2460(MainServerConnectionString);
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

        //[rdixit][29.11.2023][GEOS2-4955]
        public void AddPropertyManager_V2460(List<ConnectorProperties> connectorProperty)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                mgr.AddPropertyManager_V2460(MainServerConnectionString, connectorProperty);
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


        //[Sudhir.Jangra][GEOS2-4973]
        public List<Family> GetFamiliesForSearchFamilyConfiguration_V2460(string language)
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetFamiliesForSearchFamilyConfiguration_V2460(localGeosConnectionString, language);
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

        //[rdixit][GEOS2-4951][08.12.2023]
        public List<Connectors> GetAllConnectors_V2460(Connectors Connectors)
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.GetAllConnectors_V2460(localGeosConnectionString, Connectors, Properties.Settings.Default.ConnectorImage);
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
        //[rdixit][GEOS2-4951][08.12.2023]
        public List<SCMConnectorImage> GetAllConnectorImages_V2460(Connectors Connectors)
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.GetAllConnectorImages_V2460(localGeosConnectionString, Connectors, Properties.Settings.Default.ConnectorImage);
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
        public List<ConfigurationFamily> GetConfigurationsForSearchFilters_V2460()
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetConfigurationsForSearchFilters_V2460(localGeosConnectionString);
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
        public bool SaveConfigurationsForSearchFilters(List<ConfigurationFamily> ConfigurationList)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.SaveConfigurationsForSearchFilters(ConfigurationList, MainServerConnectionString);
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
        //rajashri GEOS2-4956
        public ConnectorFamily GetFamilyDetails_V2470(Int32 idFamily)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.GetFamilyDetails_V2470(MainServerConnectionString, idFamily);
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

        #region [rdixit][GEOS2-5148,5149,5150][29.01.2024]
        public List<ConnectorFamily> GetConnectorFamilies_V2480()
        {
            try
            {

                string LocalGeosContextConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetConnectorFamilies_V2480(LocalGeosContextConnectionString, Properties.Settings.Default.ConnectorFamilyVisualAidsPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();

                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public ConnectorFamily AddConnectorFamilies_V2480(ConnectorFamily ConnectorFamily)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddConnectorFamilies_V2480(MainServerConnectionString, ConnectorFamily, Properties.Settings.Default.ConnectorFamilyVisualAidsPath);
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

        public ConnectorFamily UpdateConnectorFamilies_V2480(ConnectorFamily ConnectorFamily)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateConnectorFamilies_V2480(MainServerConnectionString, ConnectorFamily, Properties.Settings.Default.ConnectorFamilyVisualAidsPath);
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

        public List<Family> GetAllFamilies_V2480(string language)
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllFamilies_V2480(localGeosConnectionString, language);
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
        public List<ConnectorSubFamily> GetAllSubfamilies_V2480(string language)
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllSubfamilies_V2480(localGeosConnectionString, language);
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

        public List<Connectors> GetAllConnectors_V2480(Connectors Connectors)
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.GetAllConnectors_V2480(localGeosConnectionString, Connectors, Properties.Settings.Default.ConnectorImage);
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
        #endregion
        //rajashri GEOS2-5227
        public List<ConnectorProperties> GetConnectorProperties_V2480(String language)
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetConnectorProperties_V2480(localGeosConnectionString, language);
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
        public List<ValueKey> GetValueKeyOfCustomProperties_V2480(Int32 IdCustomConnectorProperty)
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetValueKeyOfCustomProperties_V2480(localGeosConnectionString, IdCustomConnectorProperty);
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
        public List<ConnectorProperties> GetPropertyManagerByFamily_V2480(UInt32 familyId)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetPropertyManagerByFamily_V2480(MainServerConnectionString, familyId);
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
        public List<Connectors> GetAllConnectors_V2490(Connectors Connectors)
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllConnectors_V2490(localGeosConnectionString, Connectors, Properties.Settings.Default.ConnectorImage);
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

        //[Sudhir.jangra][GEOS2-5203]
        public List<Data.Common.Company> GetAuthorizedPlantsByIdUser_V2490(Int32 idUser)
        {
            List<Data.Common.Company> companies = new List<Data.Common.Company>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                companies = mgr.GetAuthorizedPlantsByIdUser_V2490(WorkbenchConnectionString, idUser);
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

        //[Sudhir.jangra][GEOS2-5204]
        public List<Samples> GetModifiedSamplesByIdSite_V2490(DateTime startDate, DateTime endDate, Int32 idSite)
        {
            List<Samples> samples = new List<Samples>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                samples = mgr.GetModifiedSamplesByIdSite_V2490(WorkbenchConnectionString, startDate, endDate, idSite);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return samples;
        }


        //[Sudhir.jangra][GEOS2-5205]
        public List<ThreeDConnectorItems> Get3dConnectorByIdSite_V2490(DateTime startDate, DateTime endDate, Int32 idSite)
        {
            List<ThreeDConnectorItems> threeDConnectorItems = new List<ThreeDConnectorItems>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                threeDConnectorItems = mgr.Get3dConnectorByIdSite_V2490(WorkbenchConnectionString, startDate, endDate, idSite);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return threeDConnectorItems;
        }


        //[Sudhir.jangra][GEOS2-5203]
        public List<Samples> GetNewSamplesByIdSite_V2490(DateTime startDate, DateTime endDate, Int32 idSite)
        {
            List<Samples> samples = new List<Samples>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                samples = mgr.GetNewSamplesByIdSite_V2490(WorkbenchConnectionString, startDate, endDate, idSite);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return samples;
        }

        //[Sudhir.jangra][GEOS2-5203]
        public List<WorkflowStatus> GetStatusForNewSamples_V2490()
        {
            List<WorkflowStatus> status = new List<WorkflowStatus>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                status = mgr.GetStatusForNewSamples_V2490(WorkbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return status;
        }

        //[GEOS2-5296][rdixit][29.02.2024]
        public List<ConnectorProperties> GetPropertyManager_V2490()
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetPropertyManager_V2490(MainServerConnectionString);
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

        //[GEOS2-5297][rdixit][29.02.2024]
        public List<ConnectorProperties> GetPropertyManagerByFamily_V2490(UInt32 familyId)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetPropertyManagerByFamily_V2490(MainServerConnectionString, familyId);
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

        #region [GEOS2-5437][rdixit][07.03.2024]
        public List<SimilarColorsByConfiguration> GetSimilarColorsByConfiguration()
        {
            List<SimilarColorsByConfiguration> colors = new List<SimilarColorsByConfiguration>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                colors = mgr.GetSimilarColorsByConfiguration(WorkbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return colors;
        }

        public List<ConfigurationFamily> GetConfigurationsForSearchFilters_V2490()
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetConfigurationsForSearchFilters_V2490(localGeosConnectionString);
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

        public bool SaveConfigurationsForSearchFilters_V2490(List<ConfigurationFamily> configurationList)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.SaveConfigurationsForSearchFilters_V2490(configurationList, MainServerConnectionString);
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

        public bool AddColorsByConfiguration_V2490(List<Tuple<int, int>> colorsList, int idUser)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddColorsByConfiguration_V2490(MainServerConnectionString, colorsList, idUser);
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
        #endregion

        //[GEOS2-5296][rdixit][11.03.2024]
        List<Data.Common.SCM.Company> ISCMService.GetAllCompany_V2490()
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllCompany_V2490(localGeosConnectionString);
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

        // [rdixit][GEOS2-5485][13.03.2024]
        public bool SaveConfigurationsForSearchFilters_V2500(List<ConfigurationFamily> configurationList)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.SaveConfigurationsForSearchFilters_V2500(configurationList, MainServerConnectionString);
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

        //[pramod.misal][GEOS2-5378][27-03-2024]
        public Connectors GetConnectorProperties_V2500(Connectors Connectors)
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetConnectorProperties_V2500(localGeosConnectionString, Connectors, Properties.Settings.Default.ConnectorImage);
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

        public List<ConnectorProperties> GetConnectorCustomPropertiesByFamily_V2500(UInt32 familyId)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetConnectorCustomPropertiesByFamily_V2500(MainServerConnectionString, familyId);
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

        //[rdixit][29.03.2024][GEOS2-5380]
        public List<SCMConnectorImage> GetAllConnectorImages_V2500(Connectors Connectors)
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllConnectorImages_V2500(localGeosConnectionString, Connectors);
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

        //[pramod.misal][GEOS2-5379][29-03-2024]
        public List<ConnectorReference> GetConnectorReferencesByRef_V2500(String ConnectorRef)
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetConnectorReferencesByRef_V2500(localGeosConnectionString, ConnectorRef);
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

        public List<ConnectorWorkflowStatus> GetAllConnectorStatus()
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllConnectorStatus(localGeosConnectionString);
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

        public List<ConnectorWorkflowTransitions> GetAllWorkflowTransitions()
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllWorkflowTransitions(localGeosConnectionString);
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

        //[pramod.misal] [GEOS2-5381] [02.04.2024]
        public List<ConnectorComponents> GetConnectorComponentsByRef_V2500(String ConnectorRef)
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetConnectorComponentsByRef_V2500(localGeosConnectionString, ConnectorRef);
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

        //[GEOS2-5383][rdixit][11.04.2024]
        public List<Connectors> GetAllLinkedConnectorByRef(long idConnector)
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllLinkedConnectorByRef(localGeosConnectionString, idConnector);
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

        //[pramod.misal] [GEOS2-5387] [09.04.2024]
        public List<ConnectorAttachements> GetConnectorAttachementsByRef_V2510(String ConnectorRef)
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetConnectorAttachementsByRef_V2510(localGeosConnectionString, ConnectorRef, Properties.Settings.Default.ConnectorAttachementsPath);
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


        //[Sudhir.jangra][GEOS2-5384]
        public List<SCMConnectorImage> GetAllConnectorImagesForImageSection_V2510(Connectors Connectors)
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllConnectorImagesForImageSection_V2510(localGeosConnectionString, Connectors);
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

        //[GEOS2-5382][rdixit][19.04.2024]
        public List<ConnectorLocation> GetConnectorLocationsByRef(string connref)
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetConnectorLocationsByRef(localGeosConnectionString, connref);
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

        //[rdixit][GEOS2-5390][22.04.2024]
        public List<ConnectorLogEntry> GetConnectorLogEntries(long idConnector)
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetConnectorLogEntries(localGeosConnectionString, idConnector);
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

        //[pramod.misal] [GEOS2-5391] [22.04.2024]
        public List<ConnectorLogEntry> GetConnectorCommentsByRef_V2510(Int64 IdConnector)
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetConnectorCommentsByRef_V2510(localGeosConnectionString, IdConnector);
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

        //[rdixit][GEOS2-5389][26.04.2024]
        public List<ScmDrawing> GetDrawingsByConnectorRef(long idConnector)
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetDrawingsByConnectorRef(localGeosConnectionString, idConnector);
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

        //[rdixit][GEOS2-5389][26.04.2024]
        public List<ArticlesbyDrawing> GetArticlesByDrawing(uint idDrawing)
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetArticlesByDrawing(localGeosConnectionString, idDrawing);
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

        //[rdixit][02.05.2024][GEOS2-5476]
        public List<Data.Common.Company> GetAuthorizedPlants(Int32 idUser)
        {
            List<Data.Common.Company> companies = new List<Data.Common.Company>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                companies = mgr.GetAuthorizedPlants(WorkbenchConnectionString, idUser);
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

        //[pramod.misal][GEOS2-5479][30-04-2024]
        public bool UpdateConnectorStatus_V2510(ConnectorSearch connectorSearch)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateConnectorStatus_V2510(MainServerConnectionString, connectorSearch);
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

        //[rdixit][14.05.2024][GEOS2-5477]
        public bool UpdateConnectorStatus_V2520(ConnectorSearch connectorSearch)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateConnectorStatus_V2520(MainServerConnectionString, connectorSearch, Properties.Settings.Default.ConnectorAttachementsPath);
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

        //[pramod.misal][15.05.2024]
        public List<ConnectorAttachements> GetAllConnectorAttachmentTypes()
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllConnectorAttachmentTypes(localGeosConnectionString);
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

        //[pramod.misal] [GEOS2-5477] [16.05.2024]
        public List<ConnectorAttachements> GetConnectorAttachementsByRef_V2520(String ConnectorRef)
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetConnectorAttachementsByRef_V2520(localGeosConnectionString, ConnectorRef, Properties.Settings.Default.ConnectorAttachementsPath);
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

        //[rdixit][14.05.2024][GEOS2-5477]
        public List<ConnectorReference> GetConnectorReferencesByRef_V2520(String ConnectorRef)
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetConnectorReferencesByRef_V2520(localGeosConnectionString, ConnectorRef);
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

        //[rdixit][14.05.2024][GEOS2-5477]
        public List<Data.Common.SCM.Company> GetAllCompanyList_V2520()
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllCompanyList_V2520(localGeosConnectionString);
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

        //[pramod.misal] [GEOS2-5391] [22.04.2024]
        public List<ConnectorLogEntry> GetConnectorCommentsByRef_V2520(Int64 IdConnector)
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetConnectorCommentsByRef_V2520(localGeosConnectionString, IdConnector);
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

        //[rdiixt][22.05.2024][GEOS2-5751]
        public List<ConnectorComponents> GetConnectorComponentsByRef_V2520(String ConnectorRef)
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetConnectorComponentsByRef_V2520(localGeosConnectionString, ConnectorRef);
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

        public string CheckOtherRefIsValid(string _ref)
        {
            string ExistRef = string.Empty;
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                ExistRef = mgr.CheckOtherRefIsValid(WorkbenchConnectionString, _ref);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return ExistRef;
        }

        //[rdiixt][27.05.2024][GEOS2-5753]
        public List<Connectors> GetAllLinkedConnectorByRef_V2520(long idConnector)
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllLinkedConnectorByRef_V2520(localGeosConnectionString, idConnector);
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

        //[rdiixt][27.05.2024][GEOS2-5753]
        public List<LinkType> GetAllLinkTypes()
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllLinkTypes(localGeosConnectionString);
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

        //[rdiixt][27.05.2024][GEOS2-5753]
        public Connectors CheckLinkedRefIsValid(string _ref)
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.CheckLinkedRefIsValid(localGeosConnectionString, _ref);
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

        //[rushikesh.gaikwad][GEOS2-5583][18.06.2024]
        public List<ScmDrawing> GetDrawingsByConnectorRef_V2530(long idConnector)
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetDrawingsByConnectorRef_V2530(localGeosConnectionString, idConnector);
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


        //[pramod.misal][GEOS2-5524][05.08.2024]
        public List<SCMLocationsManager> GetSCMLocationsManagerByIdSCM_V2550(Data.Common.Company selectedPlant)
        {
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetSCMLocationsManagerByIdSCM_V2550(WorkbenchConnectionString,selectedPlant);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[pramod.misal][GEOS2-5525][08.08.2024]
        public Int64 GetMaxPosition(Int64 idParent, Int64 idCompany)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetMaxPosition(idParent, idCompany, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rushikesh.gaikwad] [GEOS2-5524] [08.07.2024]
        public SCMLocationsManager AddSCMLocation(SCMLocationsManager scmLocationsManager)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddSCMLocation(connectionString, scmLocationsManager);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pramod.misal] [GEOS2-5525] [09.08.2024]
        public SCMLocationsManager UpdateSCMLocation(SCMLocationsManager scmLocationsManager)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateSCMLocation(connectionString, scmLocationsManager);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pramod.misal] [GEOS2-55552524] [08.08.2024]
        public bool IsInUSESCMLocation(Int64 idSampleLocation, Int64 idCompany)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.IsInUSESCMLocation(connectionString, idSampleLocation, idCompany);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pramod.misal][GEOS2-5524][05.08.2024]
        public List<SCMLocationsManager> GetIsLeafSCMLocationsManager_V2550(Data.Common.Company selectedPlant)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetIsLeafSCMLocationsManager_V2550(connectionString, selectedPlant);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Rahul.Gadhave][GEOS2-5804][Date-5/08/2024]
        public List<ConnectorLocation> GetConnectorLocationsByRef_V2550(string connref)
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetConnectorLocationsByRef_V2550(localGeosConnectionString, connref);
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
        //[Rahul.Gadhave][GEOS2-5804][Date-5/08/2024]
        public bool UpdateConnectorStatus_V2550(ConnectorSearch connectorSearch)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateConnectorStatus_V2550(MainServerConnectionString, connectorSearch, Properties.Settings.Default.ConnectorAttachementsPath);
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

        //[pramod.misal] [GEOS2-5525] [08.08.2024]
        public Int64 GetMaxPosition_V2550(Int64 idParent, Int64 idCompany, string fullName)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetMaxPosition_V2550(idParent, idCompany, fullName, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pramod.misal] [GEOS2-5525] [08.08.2024]
        public bool IsExistSCMLocationsManagerName(string name, Int64 parent, Int64 idCompany)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.IsExistSCMLocationsManagerName(connectionString, name, parent, idCompany);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][12.08.2024][GEOS2-5752]
        public List<Data.Common.Company> GetAuthorizedPlants_V2550(Int32 idUser)
        {
            List<Data.Common.Company> companies = new List<Data.Common.Company>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                companies = mgr.GetAuthorizedPlants_V2550(WorkbenchConnectionString, idUser);
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

        //[Rahul.Gadhave][GEOS2-5779][Date-09/08/2024]
        public List<SCMConnectorImage> GetAllConnectorImagesForImageSection_V2550(Connectors Connectors)
        {
            try
            {
                string ConnectorWtgImagesPath = Properties.Settings.Default.ConnectorWtgImages; // Get the local path
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllConnectorImagesForImageSection_V2550(localGeosConnectionString, Connectors, ConnectorWtgImagesPath);
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

        //[pramod.misal][GEOS2-5754][28-08-2024]
        public ConnectorSearch UpdateConnectorStatus_V2560(ConnectorSearch connectorSearch)
        {
            try
            {    //ConnectorWtgImages
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateConnectorStatus_V2560(MainServerConnectionString, connectorSearch, Properties.Settings.Default.ConnectorAttachementsPath, Properties.Settings.Default.ConnectorImage,Properties.Settings.Default.ConnectorWtgImages);
               
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

        #region [rdixit][GEOS2-5802][05.09.2024]
        public List<Connectors> GetSampleRegistrationAllConnectors_V2560(Connectors Connectors)
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetSampleRegistrationAllConnectors_V2560(localGeosConnectionString, Connectors, Properties.Settings.Default.ConnectorImage);
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
        public List<Connectors> GetConnectorsBySearchConfiguration_V2560(Connectors Connectors)
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetConnectorsBySearchConfiguration_V2560(localGeosConnectionString, Connectors, Properties.Settings.Default.ConnectorImage);
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
        public List<ConnectorProperties> GetAllConnectorCustomProperties_V2560()
        {
            try
            {
                string LocalGeosContext = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                string WorkbenchContext = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllConnectorCustomProperties_V2560(LocalGeosContext, WorkbenchContext);
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

        //[rushikesh.gaikwad][GEOS2-5801][12.09.2024]   
        public List<UserRoles> GetUserRoles_V2560()
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.GetUserRoles_V2560(localGeosConnectionString);
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
        #endregion
        //[GEOS2-5803][rdixit][13.09.2024]
        public string GetLatestConnectorReference(string year)
        {
            try
            {
                string LocalGeosContext = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetLatestConnectorReference(year, LocalGeosContext);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-5803][rdixit][13.09.2024]
        public bool AddConnector_V2560(ConnectorSearch connectorSearch)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddConnector_V2560(MainServerConnectionString, connectorSearch, Properties.Settings.Default.ConnectorAttachementsPath, Properties.Settings.Default.ConnectorImage, Properties.Settings.Default.ConnectorWtgImages);
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

        //[GEOS2-6601][14.11.2024][rdixit]
        public List<Connectors> GetAllConnectors_V2580(Connectors Connectors, string componentQuery)
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllConnectors_V2580(localGeosConnectionString, Connectors, componentQuery, Properties.Settings.Default.ConnectorImage);
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

        //[GEOS2-6080][05.12.2024][rdixit]
        public List<ScmDrawing> GetDrawingsByConnectorRef_V2590(long idConnector)
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetDrawingsByConnectorRef_V2590(localGeosConnectionString, idConnector);
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

        //[rdixit][GEOS2-6654][03.01.2025]
        public List<Samples> GetReferenceByCustomer_V2600( DateTime startDate, DateTime endDate, string idCustomerList)
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetReferenceByCustomer_V2600(localGeosConnectionString, startDate, endDate, idCustomerList);
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

        //[rdixit][14.01.2025][GEOS2-6857]
        public string GetLatestConnectorReference_V2600(string year)
        {
            try
            {
                string LocalGeosContext = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetLatestConnectorReference_V2600(year, LocalGeosContext);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[rdixit][GEOS2-6987][03.03.2025]
        public List<Samples> GetModifiedSamplesByIdSite_V2620(DateTime startDate, DateTime endDate, Int32 idSite)
        {
            List<Samples> samples = new List<Samples>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                samples = mgr.GetModifiedSamplesByIdSite_V2620(WorkbenchConnectionString, startDate, endDate, idSite);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return samples;
        }

        //[rdixit][05.03.2025][GEOS2-7026] 
        public bool UpdateSubFamily_V2620(ConnectorSubFamily subFamily)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateSubFamily_V2620(MainServerConnectionString, subFamily, Properties.Settings.Default.ConnectorFamilyVisualAidsPath);
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

        //[rdixit][05.03.2025][GEOS2-7026] 
        public ConnectorFamily AddConnectorFamilies_V2620(ConnectorFamily ConnectorFamily)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddConnectorFamilies_V2620(MainServerConnectionString, ConnectorFamily, Properties.Settings.Default.ConnectorFamilyVisualAidsPath);
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

        //[rdixit][05.03.2025][GEOS2-7026] 
        public ConnectorFamily UpdateConnectorFamilies_V2620(ConnectorFamily ConnectorFamily)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateConnectorFamilies_V2620(MainServerConnectionString, ConnectorFamily, Properties.Settings.Default.ConnectorFamilyVisualAidsPath);
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

        //[rdixit][05.03.2025][GEOS2-7026] 
        public List<ConnectorFamily> GetConnectorFamilies_V2620()
        {
            try
            {

                string LocalGeosContextConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetConnectorFamilies_V2620(LocalGeosContextConnectionString, Properties.Settings.Default.ConnectorFamilyVisualAidsPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();

                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][05.03.2025][GEOS2-7026] 
        public ConnectorSubFamily GetSubFamilyDetails_V2620(int idSubFamily, string familyName)
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.GetSubFamilyDetails_V2620(localGeosConnectionString, idSubFamily, familyName, Properties.Settings.Default.ConnectorFamilyVisualAidsPath);
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

        //[rdixit][05.03.2025][GEOS2-7026] 
        public List<ConnectorSubFamily> GetSubFamiliesListByIdFamily_V2620(int idFamily)
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.GetSubFamiliesListByIdFamily_V2620(localGeosConnectionString, idFamily, Properties.Settings.Default.ConnectorFamilyVisualAidsPath);
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

        //[rdixit][05.03.2025][GEOS2-7026] 
        public List<FamilyImage> GetFamilyImagesByIdFamily_V2620(int IdFamily)
        {
            try
            {
                string LocalGeosContextConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetFamilyImagesByIdFamily_V2620(LocalGeosContextConnectionString, IdFamily, Properties.Settings.Default.ConnectorFamilyVisualAidsPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();

                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public bool InsertSubFamily_V2620(ConnectorSubFamily subFamily)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.InsertSubFamily_V2620(MainServerConnectionString, subFamily, Properties.Settings.Default.ConnectorFamilyVisualAidsPath);
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

        //[rdixit][GEOS2-6984][27.03.2025]
        public List<Connectors> GetAllConnectors_V2630(Connectors Connectors, string componentQuery)
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllConnectors_V2630(localGeosConnectionString, Connectors, componentQuery, Properties.Settings.Default.ConnectorImage);
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

        //[rdixit][GEOS2-7859][14.05.2025]
        public List<Samples> GetNewSamplesByIdSite_V2640(DateTime startDate, DateTime endDate, Int32 idSite)
        {
            List<Samples> samples = new List<Samples>();
            try
            {
                string Emdep_GeosConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                string GeosConnectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                samples = mgr.GetNewSamplesByIdSite_V2640(Emdep_GeosConnectionString, GeosConnectionString, startDate, endDate, idSite);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return samples;
        }

        #region #region [GEOS2-7863][rani dhamankar][12-05-2025]
        public List<ThreeDConnectorItems> Get3dConnectorByIdSite_V2640(DateTime startDate, DateTime endDate, Int32 idSite)
        {
            List<ThreeDConnectorItems> threeDConnectorItems = new List<ThreeDConnectorItems>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                threeDConnectorItems = mgr.Get3dConnectorByIdSite_V2640(WorkbenchConnectionString, startDate, endDate, idSite);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return threeDConnectorItems;
        }
        #endregion

        //[rdixit][GEOS2-7861][14.05.2025]
        public List<Samples> GetModifiedSamplesByIdSite_V2640(DateTime startDate, DateTime endDate, Int32 idSite)
        {
            List<Samples> samples = new List<Samples>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                samples = mgr.GetModifiedSamplesByIdSite_V2640(WorkbenchConnectionString, startDate, endDate, idSite);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return samples;
        }

        #region #region [GEOS2-7855][rani dhamankar][15-05-2025]
        public List<ConnectorProperties> GetPropertyManagerByFamily_V2640(String familyId)
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetPropertyManagerByFamily_V2640(localGeosConnectionString, familyId);
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


        public List<Connectors> GetAllConnectors_V2640(Connectors Connectors, string componentQuery, bool getAllRecords)
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllConnectors_V2640(localGeosConnectionString, Connectors, componentQuery, Properties.Settings.Default.ConnectorImage, getAllRecords);
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
        #endregion

        //[rdixit][GEOS2-8133][26.05.2025]
        public List<Connectors> GetSampleRegistrationAllConnectors_V2640(Connectors Connectors, bool isLoadAll)
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetSampleRegistrationAllConnectors_V2640(localGeosConnectionString, Connectors, Properties.Settings.Default.ConnectorImage, isLoadAll);
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

        //[GEOS2-8088][rdixit][29.05.2025]
        public string AddConnector_V2650(ConnectorSearch connectorSearch)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddConnector_V2650(MainServerConnectionString, connectorSearch, Properties.Settings.Default.ConnectorAttachementsPath, Properties.Settings.Default.ConnectorImage, Properties.Settings.Default.ConnectorWtgImages);
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

        //[rdixit][11.06.2025][GEOS2-6644]
        public bool SaveConfigurationsForSearchFilters_V2650(List<ConfigurationFamily> configurationList)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.SaveConfigurationsForSearchFilters_V2650(configurationList, MainServerConnectionString);
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

        //[rdixit][11.06.2025][GEOS2-6644]
        public List<SimilarCharactersByConfiguration> GetSimilarCharachtersByConfiguration()
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetSimilarCharactersByConfiguration(localGeosConnectionString);
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
        //[rdixit][19.06.2025][GEOS2-6645]
        public List<Connectors> GetAllConnectors_V2650(Connectors Connectors, string componentQuery, bool getAllRecords)
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllConnectors_V2650(localGeosConnectionString, Connectors, componentQuery, Properties.Settings.Default.ConnectorImage, getAllRecords);
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
        //[nsatpute][16.07.2025][GEOS2-8090]
        public bool IsConnectorExists(string connectorRerence)
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.IsConnectorExists(localGeosConnectionString, connectorRerence);
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
        //[nsatpute][22.07.2025][GEOS2-8090]
        public List<ScmDrawing> GetDrawingsByCustomerRef_V2660(string customerRef)
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetDrawingsByCustomerRef_V2660(localGeosConnectionString, customerRef);
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
        //[nsatpute][22.07.2025][GEOS2-8090]
        public List<Emdep.Geos.Data.Common.Company> GetAllCompaniesWithServiceProvider_V2660()
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllCompaniesWithServiceProvider_V2660(localGeosConnectionString);
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

        //[rdixit][GEOS2-9199][11.09.2025]
        public ConnectorSearch UpdateConnectorStatus_V2670(ConnectorSearch connectorSearch)
        {
            try
            {    //ConnectorWtgImages
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateConnectorStatus_V2670(MainServerConnectionString, connectorSearch, Properties.Settings.Default.ConnectorAttachementsPath, Properties.Settings.Default.ConnectorImage, Properties.Settings.Default.ConnectorWtgImages);

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

        //[rdixit][18.09.2025][GEOS2-]
        public Connectors GetConnectorProperties_V2670(Connectors Connectors)
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                string ConnectorWtgImagesPath = Properties.Settings.Default.ConnectorWtgImages;
                return mgr.GetConnectorProperties_V2670(localGeosConnectionString, Connectors, ConnectorWtgImagesPath, Properties.Settings.Default.ConnectorAttachementsPath);
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

        //[rdixit][18.09.2025][GEOS2-]
        public List<ConnectorProperties> GetCustomeProperties_V2670()
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                string ConnectorWtgImagesPath = Properties.Settings.Default.ConnectorWtgImages;
                return mgr.GetCustomeProperties_V2670(localGeosConnectionString, WorkbenchConnectionString);
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

        //[rdixit][01.10.2025][GEOS2-9552]
        public List<Connectors> GetAllConnectors_V2670(Connectors Connectors, string componentQuery, bool getAllRecords)
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllConnectors_V2670(localGeosConnectionString, Connectors, componentQuery, Properties.Settings.Default.ConnectorImage, getAllRecords);
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

        //[rdixit][14.10.2025][GEOS2-8895]
        public string AddConnector_V2680(ConnectorSearch connectorSearch)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddConnector_V2680(MainServerConnectionString, connectorSearch, Properties.Settings.Default.ConnectorAttachementsPath, Properties.Settings.Default.ConnectorImage, Properties.Settings.Default.ConnectorWtgImages);
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
        //[Rahul.Gadhave][GEOS2-9556][Date:04/11/2025]
        public bool DeleteReference_V2680(string Reference, Int64 IdConnector, Int32 IdUser)
        {
            try
            {
                string MainServerContext = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.DeleteReference_V2680(Reference, IdConnector, IdUser, MainServerContext);
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

        public bool GetConnectorCreatorEmailAndSendMail_V2680(Int64 IdConnector)
        {
            try
            {
                string LocalGeosContext = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetConnectorCreatorEmailAndSendMail_V2680(IdConnector, Properties.Settings.Default.MailServerName, Properties.Settings.Default.MailServerPort, LocalGeosContext);
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
        //[Rahul.Gadhave][GEOS2-9556][Date:04/11/2025]
        public List<Connectors> GetAllConnectors_V2680(Connectors Connectors, string componentQuery, bool getAllRecords)
        {
            try
            {
                string localGeosConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllConnectors_V2680(localGeosConnectionString, Connectors, componentQuery, Properties.Settings.Default.ConnectorImage, getAllRecords);
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
    }
}
